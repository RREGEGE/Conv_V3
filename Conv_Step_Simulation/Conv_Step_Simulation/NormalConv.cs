using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Diagnostics;
using System.Threading;
using WMX3ApiCLR;
using static Conv_Step_Simulation.G_Var;

namespace Conv_Step_Simulation
{
    internal class NormalConv : Conveyor
    {
        /// <summary>
        /// Init동작 함수
        /// </summary>
        public override void Init()
        {
            initComp = false;
            Thread InitThread = new Thread(() =>
            {
                Console.WriteLine(ID + ": Init Start");
                while (isMainFormOpen && (isAutoRun || isCycleRun) && !initComp)
                {
                    switch (CV_InitStep)
                    {
                        case InitStep.Step000_Foup_Check:
                            {
                                CST_Check_Detect();
                                if (isCSTInPosition)
                                {
                                    CV_InitStep = InitStep.Step500_Init_Done;
                                }
                                else
                                {
                                    CV_InitStep = InitStep.Step100_Move;
                                }
                            }
                            break;

                        case InitStep.Step100_Move:
                            {
                                StartConveyor(initVelocity);

                                int elapsed = 0;
                                while (isMainFormOpen && (isAutoRun || isCycleRun) && !initComp && elapsed < 5000)
                                {
                                    CST_Check_Detect();
                                    if (isCSTInPosition)
                                    {
                                        break;
                                    }
                                    Thread.Sleep(100);
                                    elapsed += 100;
                                }
                                StopConveyor();
                                CV_InitStep = InitStep.Step500_Init_Done;
                            }
                            break;

                        case InitStep.Step500_Init_Done:
                            {
                                initComp = true;
                            }
                            break;

                        default:
                            {
                                CV_InitStep = InitStep.Step000_Foup_Check;
                            }
                            break;
                    }
                    Thread.Sleep(10);
                }
                Console.WriteLine(ID + " : Init Stop");
            });
            InitThread.Start();
        }
        /// <summary>
        /// Auto 공정 함수
        /// </summary>
        public override void Auto_Start_CV_Control()
        {
            mode = Mode.Auto;
            InmodeConvCheck();
            interfaceTypeCheck();
            runtDirection = LineDirection.Input;
            CV_AutoStep = AutoStep.Step000_Check_Direction;
            CV_InterfaceStep = PIOStep.Step000_Direction_Check;
            bool modechange = isInput;
            Thread LocalThread = new Thread(() =>
            {
                Console.WriteLine(ID + ":" + "Auto Start");
                while (isMainFormOpen && (G_Var.isAutoRun || G_Var.isCycleRun) && (AutoRunning || CycleRunning))
                {
                    if(modechange != isInput)
                    {
                        if (isInput)
                        {
                            runtDirection = LineDirection.Input;
                        }
                        else
                        {
                            runtDirection = LineDirection.Output;
                        }
                        CV_AutoStep = AutoStep.Step000_Check_Direction;
                        modechange = isInput;
                    }
                    if (isPIORun)
                    {
                        ProcessPIOStep();
                    }
                    if (!isPIORun)
                    {
                        switch (CV_AutoStep)
                        {
                            /// <summary>
                            /// 1. 운영 방향과 운전 방향 확인
                            /// </summary>
                            case AutoStep.Step000_Check_Direction:
                                {
                                    if (GetOperationDirection() == LineDirection.Input)
                                    {
                                        CV_AutoStep = AutoStep.Step100_InMode_Check_CST_Load_Status;
                                    }
                                    else
                                    {
                                        CV_AutoStep = AutoStep.Step500_OutMode_Check_CST_Load_Status;
                                    }
                                }
                                break;

                            /// <summary>
                            /// 1.현재 자재가 있는지 확인 (자재가 있으면 CSTDetected = true)
                            /// 2.현재 스텝을 IDLE로 판단.
                            /// </summary>
                            case AutoStep.Step100_InMode_Check_CST_Load_Status: // 자재가 있고 앞 컨베이어에도 자재가 있는 경우 추가해야됨
                                {
                                    CST_Empty_Detect();
                                    // 자재가 있는 경우
                                    if (isCSTEmpty)
                                    {
                                        CST_Check_Detect();
                                        Thread.Sleep(2000);
                                        if (isCSTInPosition)
                                        {
                                            if (nextConv != null)
                                            {
                                                if (nextConv.type == "Long")
                                                {
                                                    nextConv.LCnV_CST_Empty_Detect();
                                                    if (nextConv.longCnvCSTDetected)
                                                    {
                                                        CV_AutoStep = AutoStep.Step255_InMode_Wait_NextConv;
                                                    }
                                                    else
                                                    {
                                                        CV_AutoStep = AutoStep.Step210_InMode_Move_CV_Rolling_In;
                                                    }
                                                }
                                                else if (nextConv.type == "Normal")
                                                {
                                                    nextConv.CST_Empty_Detect();
                                                    if (nextConv.isCSTEmpty)
                                                    {
                                                        CV_AutoStep = AutoStep.Step255_InMode_Wait_NextConv;
                                                    }
                                                    else
                                                    {
                                                        CV_AutoStep = AutoStep.Step210_InMode_Move_CV_Rolling_In;
                                                    }
                                                }
                                                else if (nextConv.type == "Turn")
                                                {
                                                    if (nextConv.turnStatus != TurnStatus.Load)
                                                    {
                                                        CV_AutoStep = AutoStep.Step270_InMode_Move_CST_Pass_CV_to_TCV;
                                                    }
                                                    else
                                                    {
                                                        CV_AutoStep = AutoStep.Step210_InMode_Move_CV_Rolling_In;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            CV_AutoStep = AutoStep.Step210_InMode_Move_CV_Rolling_In;
                                        }
                                    }
                                    // 자재가 없고  이전 컨베이어가 있는 경우
                                    else if (beforeConv != null && !isCSTEmpty)
                                    {
                                        CV_AutoStep = AutoStep.Step150_InMode_Waitting_Before_Conv_Unload;
                                    }
                                }
                                break;

                            /// <summary>
                            /// 1. 이전 Conveyor에 Foup이 적재되면 구동
                            /// </summary>
                            case AutoStep.Step150_InMode_Waitting_Before_Conv_Unload:
                                {
                                    _stepStopWatch.Reset();
                                    // 이전 컨베이어의 타입이 Turn인 경우
                                    if (beforeConv.type == "Turn")
                                    {
                                        Thread.Sleep(10);
                                        // 이전 컨베이어의 상태가 Unload인 경우 동작
                                        if (beforeConv.turnStatus == TurnStatus.Unload)
                                        {
                                            StartConveyor(autoVelocity);
                                            CV_AutoStep = AutoStep.Step155_InMode_Change_210;
                                        }
                                    }
                                    // 이전 컨베이어의 타입이 Turn이 아닌 경우 이전 컨베이어의 AutoStep Check 후 동작
                                    else if (beforeConv.CV_AutoStep == AutoStep.Step220_InMode_Run_Condition_Check ||
                                     beforeConv.CV_AutoStep == AutoStep.Step255_InMode_Wait_NextConv)
                                    {
                                        StartConveyor(autoVelocity);
                                        CV_AutoStep = AutoStep.Step155_InMode_Change_210;
                                    }
                                }
                                break;

                            /// <summary>
                            /// 1. 물체가 감지되면 Step210으로 변경
                            /// </summary>
                            case AutoStep.Step155_InMode_Change_210:
                                {
                                    if (_stepStopWatch == null)
                                    {
                                        _stepStopWatch = Stopwatch.StartNew();
                                    }
                                    else if (!_stepStopWatch.IsRunning || prestep != CV_AutoStep)
                                    {
                                        _stepStopWatch.Restart();
                                    }
                                    if (_stepStopWatch.Elapsed.TotalSeconds >= 10)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.IN_Step_Time_Over_Error);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    CST_Empty_Detect();
                                    if (isCSTEmpty)
                                    {
                                        CV_AutoStep = AutoStep.Step210_InMode_Move_CV_Rolling_In;
                                    }
                                }
                                break;

                            /// <summary>
                            /// 1. 컨베어구동 동작 진행
                            /// </summary>
                            case AutoStep.Step210_InMode_Move_CV_Rolling_In:
                                {
                                    if (_stepStopWatch == null)
                                    {
                                        _stepStopWatch = Stopwatch.StartNew();
                                    }
                                    else if (!_stepStopWatch.IsRunning || prestep != CV_AutoStep)
                                    {
                                        _stepStopWatch.Restart();
                                    }
                                    if (_stepStopWatch.Elapsed.TotalSeconds >= 10)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.IN_Step_Time_Over_Error);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    CST_Empty_Detect();
                                    if (!isCSTEmpty)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    // 컨베이어 구동
                                    StartConveyor(autoVelocity);
                                    if (beforeConv != null)
                                    {
                                        // 이전 컨베이어 타입에 따라 분류
                                        if (beforeConv.type == "Long")
                                        {
                                            beforeConv.LCnV_CST_Empty_Detect();
                                            // 이전 컨베이어의 Foup 감지 센서가 모두 Off면 동작
                                            if (!beforeConv.longCnvCSTDetected)
                                            {
                                                CV_AutoStep = AutoStep.Step215_InMode_Move_CV_Rolling_In_Slow;
                                            }
                                        }
                                        else
                                        {
                                            beforeConv.CST_Empty_Detect();
                                            // 이전 컨베이어의 Foup 감지 센서가 모두 Off면 동작
                                            if (!beforeConv.isCSTEmpty)
                                            {
                                                CV_AutoStep = AutoStep.Step215_InMode_Move_CV_Rolling_In_Slow;
                                            }
                                        }
                                    }
                                    // 이전 컨베이어가 없는 경우 (시작 컨베이어)
                                    else
                                    {
                                        CV_AutoStep = AutoStep.Step220_InMode_Run_Condition_Check;
                                    }

                                }
                                break;

                            /// <summary>
                            /// 1. 앞 Conv에 Foup 존재 시 속도 줄임
                            /// </summary
                            case AutoStep.Step215_InMode_Move_CV_Rolling_In_Slow:
                                {
                                    if (_stepStopWatch == null)
                                    {
                                        _stepStopWatch = Stopwatch.StartNew();
                                    }
                                    else if (!_stepStopWatch.IsRunning || prestep != CV_AutoStep)
                                    {
                                        _stepStopWatch.Restart();
                                    }
                                    if (_stepStopWatch.Elapsed.TotalSeconds >= 10)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.IN_Step_Time_Over_Error);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    CST_Empty_Detect();
                                    if (!isCSTEmpty)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    if (nextConv != null)
                                    {
                                        if (nextConv.mode == Mode.Alarm)
                                        {
                                            StartConveyor(slowVelocity);
                                        }
                                        // 다음 컨베이어의 타입에 따라 분류
                                        else if (nextConv.type == "Long")
                                        {
                                            nextConv.LCnV_CST_Empty_Detect();
                                            // 다음 컨베이어에 Foup이 있다면 저속
                                            if (nextConv.longCnvCSTDetected)
                                            {
                                                StartConveyor(slowVelocity);
                                            }
                                        }
                                        else if (nextConv.type == "Normal")
                                        {
                                            nextConv.CST_Empty_Detect();
                                            if (nextConv.isCSTEmpty)
                                            {
                                                StartConveyor(slowVelocity);
                                            }
                                        }
                                        else if (nextConv.type == "Turn")
                                        {
                                            if (nextConv.turnStatus != TurnStatus.Load)
                                            {
                                                StartConveyor(slowVelocity);
                                            }
                                        }

                                        CV_AutoStep = AutoStep.Step220_InMode_Run_Condition_Check;
                                    }
                                    // 다음 컨베이어가 없는 경우 (마지막 컨베이어인 경우)
                                    else if (nextConv == null)
                                    {
                                        StartConveyor(slowVelocity);
                                        CV_AutoStep = AutoStep.Step220_InMode_Run_Condition_Check;
                                    }
                                    // 이전 컨베이어가 없는 경우 (시작 컨베이어)
                                    else if (beforeConv == null)
                                    {
                                        CV_AutoStep = AutoStep.Step220_InMode_Run_Condition_Check;
                                    }
                                }
                                break;
                            /// <summary>
                            /// 1. Foup Detect센서가 모두 감지됐을 때 조건 확인
                            /// </summary>
                            case AutoStep.Step220_InMode_Run_Condition_Check:
                                {
                                    if (_stepStopWatch == null)
                                    {
                                        _stepStopWatch = Stopwatch.StartNew();
                                    }
                                    else if (!_stepStopWatch.IsRunning || prestep != CV_AutoStep)
                                    {
                                        _stepStopWatch.Restart();
                                    }
                                    if (_stepStopWatch.Elapsed.TotalSeconds >= 10)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.IN_Step_Time_Over_Error);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    CST_Empty_Detect();
                                    if (!isCSTEmpty)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    CST_Check_Detect();
                                    // 마지막 컨베이어가 아닌 경우
                                    if (nextConv.mode == Mode.Auto && nextConv != null && isCSTInPosition)
                                    {

                                        if (nextConv.mode == Mode.Alarm)
                                        {
                                            StopConveyor();
                                            _stepStopWatch.Reset();
                                            CV_AutoStep = AutoStep.Step280_InMode_Alarm_Condition_280;
                                        }
                                        // 다음 컨베이어가 Turn 컨베이어인 경우
                                        else if (nextConv.mode == Mode.Auto && nextConv.type == "Turn")
                                        {
                                            // 다음 컨베이어가 Load 상태가 아닐 경우 정지
                                            if (nextConv.turnStatus != TurnStatus.Load)
                                            {
                                                _stepStopWatch.Reset();
                                                StopConveyor();
                                                CV_AutoStep = AutoStep.Step270_InMode_Move_CST_Pass_CV_to_TCV;
                                            }
                                            // Load 상태일 경우
                                            else
                                            {
                                                StartConveyor(autoVelocity);
                                                CV_AutoStep = AutoStep.Step250_InMode_Move_CV_Rolling_Out;
                                            }
                                        }
                                        // 다음 컨베이어가 Normal 컨베이어인 경우
                                        else if (nextConv.mode == Mode.Auto && nextConv.type == "Normal")
                                        {
                                            nextConv.CST_Empty_Detect();
                                            // 다음 컨베이어에 Foup이 없는 경우
                                            if (!nextConv.isCSTEmpty)
                                            {
                                                CV_AutoStep = AutoStep.Step250_InMode_Move_CV_Rolling_Out;
                                            }
                                            // 다음 컨베이어에 Foup이 있는 경우 정지
                                            else
                                            {
                                                StopConveyor();
                                                _stepStopWatch.Reset();
                                                CV_AutoStep = AutoStep.Step255_InMode_Wait_NextConv;
                                            }
                                        }
                                        // 다음 컨베이어가 Long 컨베이어인 경우
                                        else if (nextConv.mode == Mode.Auto && nextConv.type == "Long")
                                        {
                                            nextConv.LCnV_CST_Empty_Detect();
                                            // 다음 컨베이어에 Foup이 없는 경우
                                            if (!nextConv.longCnvCSTDetected)
                                            {
                                                CV_AutoStep = AutoStep.Step250_InMode_Move_CV_Rolling_Out;
                                            }
                                            // 다음 컨베이어에 Foup이 있는 경우 정지
                                            else
                                            {
                                                StopConveyor();
                                                _stepStopWatch.Reset();
                                                CV_AutoStep = AutoStep.Step255_InMode_Wait_NextConv;
                                            }
                                        }
                                    }
                                    // 마지막 컨베이어인 경우 정지
                                    else if (nextConv == null && isCSTInPosition)
                                    {

                                        StopConveyor();
                                        _stepStopWatch.Reset();
                                        CV_AutoStep = AutoStep.Step900_Final_CV_Wait;
                                    }
                                }
                                break;

                            /// <summary>
                            /// 1. Foup이 빠져나가면 Step 초기화
                            /// </summary>
                            case AutoStep.Step250_InMode_Move_CV_Rolling_Out:
                                {
                                    if (_stepStopWatch == null)
                                    {
                                        _stepStopWatch = Stopwatch.StartNew();
                                    }
                                    else if (!_stepStopWatch.IsRunning || prestep != CV_AutoStep)
                                    {
                                        _stepStopWatch.Restart();
                                    }
                                    if (_stepStopWatch.Elapsed.TotalSeconds >= 10)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.IN_Step_Time_Over_Error);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    if (nextConv.mode == Mode.Alarm)
                                    {
                                        StopConveyor();
                                        _stepStopWatch.Reset();
                                        CV_AutoStep = AutoStep.Step280_InMode_Alarm_Condition_280;
                                    }
                                    // 마지막 Foup 감지 센서가 꺼진 경우 0.1초 후 정지 - 시간 조정
                                    else if (!CV_Inmode_LastFoup_Detect_Check())
                                    {
                                        Thread.Sleep(100);
                                        StopConveyor();
                                        _stepStopWatch.Reset();
                                        CV_AutoStep = AutoStep.Step000_Check_Direction;
                                    }
                                }
                                break;

                            /// <summary>
                            /// 1. Next Conveyor가 Foup을 Load하지 못하는 경우 대기
                            /// </summary>
                            case AutoStep.Step255_InMode_Wait_NextConv:
                                {
                                    CST_Empty_Detect();
                                    if (!isCSTEmpty)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    if (nextConv.mode == Mode.Alarm)
                                    {
                                        StopConveyor();
                                        _stepStopWatch.Reset();
                                        CV_AutoStep = AutoStep.Step280_InMode_Alarm_Condition_280;
                                    }
                                    // 다음 컨베이어의 타입에 따라 분류
                                    else if (nextConv.mode == Mode.Auto && nextConv.type == "Normal")
                                    {
                                        nextConv.CST_Empty_Detect();
                                        // 다음 컨베이어가 비어 있는 경우 2초 후 동작 - 시간 조정
                                        if (!nextConv.isCSTEmpty)
                                        {
                                            Thread.Sleep(2000);
                                            StartConveyor(autoVelocity);
                                            CV_AutoStep = AutoStep.Step250_InMode_Move_CV_Rolling_Out;
                                        }
                                    }
                                    else if (nextConv.mode == Mode.Auto && nextConv.type == "Long")
                                    {
                                        nextConv.LCnV_CST_Empty_Detect();
                                        // 다음 컨베이어가 비어 있는 경우 2초 후 동작 - 시간 조정
                                        if (!nextConv.longCnvCSTDetected)
                                        {
                                            Thread.Sleep(2000);
                                            StartConveyor(autoVelocity);
                                            CV_AutoStep = AutoStep.Step250_InMode_Move_CV_Rolling_Out;
                                        }
                                    }
                                }
                                break;

                            /// <summary>
                            /// 1. 앞 Turn Conveyor가 Busy인 경우 Load 상태가 될 때 까지 대기
                            /// </summary>
                            case AutoStep.Step270_InMode_Move_CST_Pass_CV_to_TCV:
                                {
                                    CST_Empty_Detect();
                                    if (!isCSTEmpty)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    if (nextConv.mode == Mode.Alarm)
                                    {
                                        StopConveyor();
                                        CV_AutoStep = AutoStep.Step280_InMode_Alarm_Condition_280;
                                    }
                                    // 다음 Turn 컨베이어가 Load 상태인 경우 2초 후 동작 - 시간 조정
                                    else if (nextConv.mode == Mode.Auto && nextConv.turnStatus == TurnStatus.Load)
                                    {
                                        Thread.Sleep(2000);
                                        StartConveyor(autoVelocity);
                                        CV_AutoStep = AutoStep.Step250_InMode_Move_CV_Rolling_Out;
                                    }
                                }
                                break;

                            case AutoStep.Step280_InMode_Alarm_Condition_280:
                                {
                                    CST_Check_Detect();
                                    CST_Empty_Detect();
                                    if (CSTDetect[0] == SensorOnOff.Off && CSTDetect[1] == SensorOnOff.On)
                                    {
                                        StartConveyor(initVelocity);
                                        CV_AutoStep = AutoStep.Step290_InMode_Alarm_Wait_290;
                                    }
                                    else if (CSTDetect[0] == SensorOnOff.On && CSTDetect[1] == SensorOnOff.Off)
                                    {
                                        runtDirection = LineDirection.Output;
                                        StartConveyor(initVelocity);
                                        runtDirection = LineDirection.Input;
                                        CV_AutoStep = AutoStep.Step290_InMode_Alarm_Wait_290;
                                    }
                                    else if (isCSTInPosition || !isCSTEmpty)
                                    {
                                        CV_AutoStep = AutoStep.Step290_InMode_Alarm_Wait_290;
                                    }
                                }
                                break;

                            case AutoStep.Step290_InMode_Alarm_Wait_290:
                                {
                                    CST_Empty_Detect();
                                    CST_Check_Detect();
                                    if (!isCSTEmpty)
                                    {
                                        if (nextConv.mode == Mode.Auto)
                                        {
                                            CV_AutoStep = AutoStep.Step000_Check_Direction;
                                        }
                                    }
                                    else if (isCSTInPosition)
                                    {
                                        StopConveyor();
                                        if (nextConv.mode == Mode.Auto)
                                        {
                                            CV_AutoStep = AutoStep.Step000_Check_Direction;
                                        }
                                    }
                                }
                                break;

                            case AutoStep.Step500_OutMode_Check_CST_Load_Status:
                                {
                                    CST_Empty_Detect();
                                    if (isCSTEmpty)
                                    {
                                        Thread.Sleep(2000);
                                        CV_AutoStep = AutoStep.Step610_OutMode_Move_CV_Rolling_In;
                                    }
                                    else if (nextConv != null && !isCSTEmpty)
                                    {
                                        CV_AutoStep = AutoStep.Step550_OutMode_Waitting_Before_Conv_Unload;
                                    }
                                }
                                break;

                            case AutoStep.Step550_OutMode_Waitting_Before_Conv_Unload:
                                {
                                    if (nextConv.type == "Turn")
                                    {
                                        Thread.Sleep(10);
                                        if (nextConv.turnStatus == TurnStatus.Unload)
                                        {
                                            StartConveyor(autoVelocity);
                                            CV_AutoStep = AutoStep.Step555_OutMode_Change_610;
                                        }
                                    }
                                    else if (nextConv.CV_AutoStep == AutoStep.Step620_OutMode_Run_Condition_Check ||
                                    nextConv.CV_AutoStep == AutoStep.Step655_OutMode_Wait_NextConv)
                                    {
                                        StartConveyor(autoVelocity);
                                        CV_AutoStep = AutoStep.Step555_OutMode_Change_610;
                                    }
                                }
                                break;

                            case AutoStep.Step555_OutMode_Change_610:
                                {
                                    if (_stepStopWatch == null)
                                    {
                                        _stepStopWatch = Stopwatch.StartNew();
                                    }
                                    else if (!_stepStopWatch.IsRunning || prestep != CV_AutoStep)
                                    {
                                        _stepStopWatch.Restart();
                                    }
                                    if (_stepStopWatch.Elapsed.TotalSeconds >= 10)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.OUT_Step_Time_Over_Error);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    CST_Empty_Detect();
                                    if (isCSTEmpty)
                                    {
                                        CV_AutoStep = AutoStep.Step610_OutMode_Move_CV_Rolling_In;
                                    }
                                }
                                break;

                            case AutoStep.Step610_OutMode_Move_CV_Rolling_In:
                                {
                                    if (_stepStopWatch == null)
                                    {
                                        _stepStopWatch = Stopwatch.StartNew();
                                    }
                                    else if (!_stepStopWatch.IsRunning || prestep != CV_AutoStep)
                                    {
                                        _stepStopWatch.Restart();
                                    }
                                    if (_stepStopWatch.Elapsed.TotalSeconds >= 10)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.OUT_Step_Time_Over_Error);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    CST_Empty_Detect();
                                    if (!isCSTEmpty)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    StartConveyor(autoVelocity);
                                    if (nextConv != null)
                                    {
                                        if (nextConv.type == "Long")
                                        {
                                            nextConv.LCnV_CST_Empty_Detect();
                                            if (!nextConv.longCnvCSTDetected)
                                            {
                                                CV_AutoStep = AutoStep.Step615_OutMode_Move_CV_Rolling_In_Slow;
                                            }
                                        }
                                        else
                                        {
                                            nextConv.CST_Empty_Detect();
                                            if (!nextConv.isCSTEmpty)
                                            {
                                                CV_AutoStep = AutoStep.Step615_OutMode_Move_CV_Rolling_In_Slow;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        CV_AutoStep = AutoStep.Step620_OutMode_Run_Condition_Check;
                                    }
                                }
                                break;
                            case AutoStep.Step615_OutMode_Move_CV_Rolling_In_Slow:
                                {
                                    if (_stepStopWatch == null)
                                    {
                                        _stepStopWatch = Stopwatch.StartNew();
                                    }
                                    else if (!_stepStopWatch.IsRunning || prestep != CV_AutoStep)
                                    {
                                        _stepStopWatch.Restart();
                                    }
                                    if (_stepStopWatch.Elapsed.TotalSeconds >= 10)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.OUT_Step_Time_Over_Error);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    CST_Empty_Detect();
                                    if (!isCSTEmpty)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    if (beforeConv != null)
                                    {
                                        if (beforeConv.mode == Mode.Alarm)
                                        {
                                            StartConveyor(slowVelocity);
                                        }
                                        else if (beforeConv.type == "Long")
                                        {
                                            beforeConv.LCnV_CST_Empty_Detect();
                                            if (beforeConv.longCnvCSTDetected)
                                            {
                                                StartConveyor(slowVelocity);
                                            }
                                        }
                                        else if (beforeConv.type == "Normal")
                                        {
                                            beforeConv.CST_Empty_Detect();
                                            if (beforeConv.isCSTEmpty)
                                            {
                                                StartConveyor(slowVelocity);
                                            }
                                        }
                                        else if (beforeConv.type == "Turn")
                                        {
                                            if (beforeConv.turnStatus != TurnStatus.Load)
                                            {
                                                StartConveyor(slowVelocity);
                                            }
                                        }


                                        CV_AutoStep = AutoStep.Step620_OutMode_Run_Condition_Check;
                                    }
                                    else if (beforeConv == null)
                                    {
                                        StartConveyor(slowVelocity);
                                        CV_AutoStep = AutoStep.Step620_OutMode_Run_Condition_Check;
                                    }
                                    else if (nextConv == null)
                                    {
                                        CV_AutoStep = AutoStep.Step620_OutMode_Run_Condition_Check;
                                    }
                                }
                                break;
                            case AutoStep.Step620_OutMode_Run_Condition_Check:
                                {
                                    if (_stepStopWatch == null)
                                    {
                                        _stepStopWatch = Stopwatch.StartNew();
                                    }
                                    else if (!_stepStopWatch.IsRunning || prestep != CV_AutoStep)
                                    {
                                        _stepStopWatch.Restart();
                                    }
                                    if (_stepStopWatch.Elapsed.TotalSeconds >= 10)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.OUT_Step_Time_Over_Error);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    CST_Empty_Detect();
                                    if (!isCSTEmpty)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    CST_Check_Detect();
                                    if (beforeConv != null && isCSTInPosition)
                                    {
                                        if (beforeConv.mode == Mode.Alarm)
                                        {
                                            StopConveyor();
                                            _stepStopWatch.Reset();
                                            CV_AutoStep = AutoStep.Step680_OutMode_Alarm_Condition_680;
                                        }
                                        else if (beforeConv.mode == Mode.Auto && beforeConv.type == "Turn")
                                        {
                                            if (beforeConv.turnStatus != TurnStatus.Load)
                                            {
                                                StopConveyor();
                                                _stepStopWatch.Reset();
                                                CV_AutoStep = AutoStep.Step670_OutMode_Move_CST_Pass_CV_to_TCV;
                                            }
                                            else
                                            {
                                                StartConveyor(autoVelocity);
                                                CV_AutoStep = AutoStep.Step650_OutMode_Move_CV_Rolling_Out;
                                            }
                                        }
                                        else if (beforeConv.mode == Mode.Auto && beforeConv.type == "Normal")
                                        {
                                            beforeConv.CST_Empty_Detect();
                                            if (!beforeConv.isCSTEmpty)
                                            {
                                                CV_AutoStep = AutoStep.Step650_OutMode_Move_CV_Rolling_Out;
                                            }
                                            else
                                            {
                                                StopConveyor();
                                                _stepStopWatch.Reset();
                                                CV_AutoStep = AutoStep.Step655_OutMode_Wait_NextConv;
                                            }
                                        }
                                        else if (beforeConv.mode == Mode.Auto && beforeConv.type == "Long")
                                        {
                                            beforeConv.LCnV_CST_Empty_Detect();
                                            if (!beforeConv.longCnvCSTDetected)
                                            {
                                                CV_AutoStep = AutoStep.Step650_OutMode_Move_CV_Rolling_Out;
                                            }
                                            else
                                            {
                                                StopConveyor();
                                                _stepStopWatch.Reset();
                                                CV_AutoStep = AutoStep.Step655_OutMode_Wait_NextConv;
                                            }
                                        }
                                    }
                                    else if (beforeConv == null && isCSTInPosition)
                                    {
                                        StopConveyor();
                                        _stepStopWatch.Reset();
                                        CV_AutoStep = AutoStep.Step900_Final_CV_Wait;
                                    }
                                }
                                break;
                            case AutoStep.Step650_OutMode_Move_CV_Rolling_Out:
                                {
                                    if (_stepStopWatch == null)
                                    {
                                        _stepStopWatch = Stopwatch.StartNew();
                                    }
                                    else if (!_stepStopWatch.IsRunning || prestep != CV_AutoStep)
                                    {
                                        _stepStopWatch.Restart();
                                    }
                                    if (_stepStopWatch.Elapsed.TotalSeconds >= 10)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.OUT_Step_Time_Over_Error);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    if (beforeConv.mode == Mode.Alarm)
                                    {
                                        StopConveyor();
                                        _stepStopWatch.Reset();
                                        CV_AutoStep = AutoStep.Step680_OutMode_Alarm_Condition_680;
                                    }
                                    else if (!CV_Outmode_LastFoup_Detect_Check())
                                    {
                                        Thread.Sleep(100);
                                        StopConveyor();
                                        _stepStopWatch.Reset();
                                        CV_AutoStep = AutoStep.Step000_Check_Direction;
                                    }
                                }
                                break;
                            case AutoStep.Step655_OutMode_Wait_NextConv:
                                {
                                    CST_Empty_Detect();
                                    if (!isCSTEmpty)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    if (beforeConv.mode == Mode.Alarm)
                                    {
                                        StopConveyor();
                                        CV_AutoStep = AutoStep.Step680_OutMode_Alarm_Condition_680;
                                    }
                                    else if (beforeConv.mode == Mode.Auto && beforeConv.type == "Normal")
                                    {
                                        beforeConv.CST_Empty_Detect();
                                        if (!beforeConv.isCSTEmpty)
                                        {
                                            Thread.Sleep(2000);
                                            StartConveyor(autoVelocity);
                                            CV_AutoStep = AutoStep.Step650_OutMode_Move_CV_Rolling_Out;
                                        }
                                    }
                                    else if (beforeConv.mode == Mode.Auto && beforeConv.type == "Long")
                                    {
                                        beforeConv.LCnV_CST_Empty_Detect();
                                        if (!beforeConv.longCnvCSTDetected)
                                        {
                                            Thread.Sleep(2000);
                                            StartConveyor(autoVelocity);
                                            CV_AutoStep = AutoStep.Step650_OutMode_Move_CV_Rolling_Out;
                                        }
                                    }
                                }
                                break;

                            case AutoStep.Step670_OutMode_Move_CST_Pass_CV_to_TCV:
                                {
                                    CST_Empty_Detect();
                                    if (!isCSTEmpty)
                                    {
                                        mode = Mode.Alarm;
                                        AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                        del_alarm?.Invoke();
                                        AutoRunning = false;
                                        CycleRunning = false;
                                    }
                                    if (beforeConv.mode == Mode.Alarm)
                                    {
                                        StopConveyor();
                                        CV_AutoStep = AutoStep.Step680_OutMode_Alarm_Condition_680;
                                    }
                                    else if (beforeConv.mode == Mode.Auto && beforeConv.turnStatus == TurnStatus.Load)
                                    {
                                        Thread.Sleep(2000);
                                        StartConveyor(autoVelocity);
                                        CV_AutoStep = AutoStep.Step650_OutMode_Move_CV_Rolling_Out;
                                    }
                                }
                                break;                            

                            case AutoStep.Step680_OutMode_Alarm_Condition_680:
                                {
                                    CST_Check_Detect();
                                    CST_Empty_Detect();
                                    if (CSTDetect[0] == SensorOnOff.On && CSTDetect[1] == SensorOnOff.Off)
                                    {
                                        StartConveyor(initVelocity);
                                        CV_AutoStep = AutoStep.Step690_OutMode_Alarm_Wait_690;
                                    }
                                    else if (CSTDetect[0] == SensorOnOff.Off && CSTDetect[1] == SensorOnOff.On)
                                    {
                                        runtDirection = LineDirection.Input;
                                        StartConveyor(initVelocity);
                                        runtDirection = LineDirection.Output;
                                        CV_AutoStep = AutoStep.Step690_OutMode_Alarm_Wait_690;
                                    }
                                    else if (isCSTInPosition || !isCSTEmpty)
                                    {
                                        CV_AutoStep = AutoStep.Step690_OutMode_Alarm_Wait_690;
                                    }
                                }
                                break;

                            case AutoStep.Step690_OutMode_Alarm_Wait_690:
                                {
                                    CST_Empty_Detect();
                                    CST_Check_Detect();
                                    if (!isCSTEmpty)
                                    {
                                        if (nextConv.mode == Mode.Auto)
                                        {
                                            CV_AutoStep = AutoStep.Step000_Check_Direction;
                                        }
                                    }
                                    else if (isCSTInPosition)
                                    {
                                        StopConveyor();
                                        if (nextConv.mode == Mode.Auto)
                                        {
                                            CV_AutoStep = AutoStep.Step000_Check_Direction;
                                        }
                                    }
                                }
                                break;

                            /// <summary>
                            /// 1. 마지막 Conveyor인 경우 물체가 Unload 될 때 까지 대기
                            /// </summary>
                            case AutoStep.Step900_Final_CV_Wait:
                                {
                                    _stepStopWatch.Reset();
                                    if (isCycleRun)
                                    {
                                        CST_Empty_Detect();
                                        if (!isCSTEmpty)
                                        {
                                            mode = Mode.Alarm;
                                            AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                            del_alarm?.Invoke();
                                            AutoRunning = false;
                                            CycleRunning = false;
                                        }
                                        if (lines[0].IdleCheck())
                                        {
                                            if (currentCycle >= targetCycle || stopCycle)
                                            {
                                                currentCycle = 0;
                                                targetCycle = 0;
                                                isCycleRun = false;
                                                lines[0].cycleStopWatch.Stop();
                                                del_ELogSender_Conveyor(enLogType.Process, enLogLevel.Normal, enLogTitle.CycleEnd, $"[{lines[0]}] Cycle End, Cycle Time:{StopWatchFunc.GetRunningTime(lines[0].cycleStopWatch)}");
                                            }
                                            else
                                            {
                                                currentCycle++;
                                                totalCycle++;
                                                Console.WriteLine(currentCycle);
                                                lines[0].ChangeMode(runtDirection);
                                                Thread.Sleep(300);
                                                lines[0].ChangeStep();
                                            }
                                        }
                                    }
                                    // Foup이 없어진 경우 Auto Step 초기화
                                    else
                                    {
                                        CST_Empty_Detect();
                                        if (!isCSTEmpty)
                                        {
                                            CV_AutoStep = AutoStep.Step000_Check_Direction;
                                        }
                                    }
                                }
                                break;

                            default:
                                break;

                        }
                    }
                    if(prestep != CV_AutoStep)
                    {
                        Console.WriteLine(ID + ":" + CV_AutoStep);
                        prestep = CV_AutoStep;
                    }
                    Thread.Sleep(10); // CPU 사용량 감소를 위한 대기
                }
                Console.WriteLine(ID + ":" + "Auto Stop");
                _stepStopWatch.Reset();
                StopConveyor();
                if (mode != Mode.Alarm)
                { mode = Mode.Manual; }
                AutoRunning = false;
                CycleRunning = false;
                G_Var.isAutoRun = false;
                G_Var.isCycleRun = false;
            });
            LocalThread.Start();
        }
        private int Auto_CV_Get_StepNum()
        {
            return (int)CV_AutoStep;
        }
        public NormalConv(int id, int axis, bool Isinterface) : base(id, axis, Isinterface)
        {
            type = "Normal";
            bits.Add(2);
            bits.Add(3);
        }
    }
}
