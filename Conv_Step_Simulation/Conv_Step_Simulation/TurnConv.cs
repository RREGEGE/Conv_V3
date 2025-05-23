﻿
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using WMX3ApiCLR;
using static Conv_Step_Simulation.G_Var;

namespace Conv_Step_Simulation
{
    internal class TurnConv : Conveyor
    {
        
        private enum TurnDirection
        {
            POS,
            NEG
        }
        private TurnDirection turnDirection = TurnDirection.NEG;
        private void StartTSV(TurnDirection turnDirection)
        {
            // 회전축을 지정된 속도로 조그 구동
            double speed = 50.0; // 원하는 속도 값 (예시)

            if (turnDirection == TurnDirection.NEG)
            {
                G_Var.bMouse = true;
                //w_motion.StartJogPos(Axis, 10);
                TurnJogNEG(speed);
            }
            else
            {
                //w_motion.StartJogPos(Axis, 10);
                TurnJogPOS(speed);
            }

        }
        private void StopTSV()
        {
            TurnJogSTOP();
        }
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
                                while(isMainFormOpen && (isAutoRun || isCycleRun) && !initComp && elapsed < 5000)
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
        public override void Turn_Init()
        {
            turn_CV_InitComp = false;
            Thread Turnthread = new Thread(() =>
            {
                Console.WriteLine(ID + ":" + "Turn Init Start");
                while (isMainFormOpen && (isAutoRun || isCycleRun) && !turn_CV_InitComp)
                {
                    switch (TCV_InitStep)
                    {
                        case TurnInitStep.Step000_POS_Check:
                            {
                                CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
                                Load_POS_Check();
                                Unload_POS_Check();
                                if ((LoadDetect && cmAxis.ActualPos == loadPOS) || (UnloadDetect && cmAxis.ActualPos == unloadPOS))
                                {
                                    TCV_InitStep = TurnInitStep.Step500_Turn_Init_Done;
                                }
                                else
                                {
                                    TCV_InitStep = TurnInitStep.Step100_Foup_Check;
                                }
                            }
                            break;

                        case TurnInitStep.Step100_Foup_Check:
                            {
                                CST_Empty_Detect();
                                CST_Check_Detect();
                                if (!isCSTEmpty || isCSTInPosition)
                                {
                                    TCV_InitStep = TurnInitStep.Step200_Turn_Move;
                                }
                                else if (CSTDetect[0] == SensorOnOff.Off && CSTDetect[1] == SensorOnOff.On)
                                {
                                    StartConveyor(initVelocity);
                                    TCV_InitStep = TurnInitStep.Step110_Foup_Both_Check;
                                }
                                else if (CSTDetect[0] == SensorOnOff.On && CSTDetect[1] == SensorOnOff.Off)
                                {
                                    runtDirection = LineDirection.Output;
                                    StartConveyor(initVelocity);
                                    runtDirection = LineDirection.Input;
                                    TCV_InitStep = TurnInitStep.Step110_Foup_Both_Check;
                                }
                            }
                            break;

                        case TurnInitStep.Step110_Foup_Both_Check:
                            {
                                CST_Check_Detect();
                                if (isCSTInPosition)
                                {
                                    StopConveyor();
                                    TCV_InitStep = TurnInitStep.Step200_Turn_Move;
                                }
                            }
                            break;

                        case TurnInitStep.Step200_Turn_Move:
                            {
                                Absolute_Turn_Move_Load();
                                TCV_InitStep = TurnInitStep.Step210_Turn_Stop_Check;
                            }
                            break;

                        case TurnInitStep.Step210_Turn_Stop_Check:
                            {
                                CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
                                Load_POS_Check();
                                if (LoadDetect && cmAxis.ActualPos == loadPOS)
                                {
                                    TCV_InitStep = TurnInitStep.Step500_Turn_Init_Done;
                                }
                            }
                            break;

                        case TurnInitStep.Step500_Turn_Init_Done:
                            {
                                turn_CV_InitComp = true;
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
                turn_CV_InitComp = true;
                Console.WriteLine(ID + " : Turn Init Stop");
            });
            Turnthread.Start();
        }
        /// <summary>
        /// Auto 공정 함수
        /// </summary>
        public override void Auto_Start_CV_Control()
        {
            mode = Mode.Auto;
            InmodeConvCheck();
            runtDirection = LineDirection.Input;
            CV_AutoStep = AutoStep.Step000_Check_Direction;
            bool modechange = isInput;
            Thread LocalThread = new Thread(() =>
            {
                Console.WriteLine(ID + ":" + "Auto Start");
                while (isMainFormOpen && (G_Var.isAutoRun || G_Var.isCycleRun) && (AutoRunning || CycleRunning))
                {
                    if (modechange != isInput)
                    {
                        if (isInput)
                        {
                            runtDirection = LineDirection.Input;
                        }
                        else
                        {
                            {
                                runtDirection = LineDirection.Output;
                            }
                        }
                        CV_AutoStep = AutoStep.Step000_Check_Direction;
                        modechange = isInput;
                    }
                    switch (CV_AutoStep)
                    {
                        /// <summary>
                        /// 1. 운영 방향과 운전 방향 확인
                        /// </summary>
                        case AutoStep.Step000_Check_Direction:
                            {
                                _stepStopWatch.Reset();
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
                        case AutoStep.Step100_InMode_Check_CST_Load_Status:
                            {
                                // Load 상태
                                turnStatus = TurnStatus.Load;
                                Load_POS_Check();
                                CST_Empty_Detect();
                                // Load 위치에 있지 않는 경우
                                if (!LoadDetect)
                                {
                                    CV_AutoStep = AutoStep.Step310_InMode_T_Init_Load;
                                }
                                // Load 위치에 위치해 있고 Foup이 감지 되는 경우
                                else if (LoadDetect && isCSTEmpty)
                                {
                                    Thread.Sleep(2000);
                                    CV_AutoStep = AutoStep.Step210_InMode_Move_CV_Rolling_In;
                                }
                                // Load 위치에 위치해 있지만 Foup이 감지 되지 않는 경우
                                else if (beforeConv != null && LoadDetect && !isCSTEmpty)
                                {
                                    Thread.Sleep(100);
                                    CV_AutoStep = AutoStep.Step150_InMode_Waitting_Before_Conv_Unload;
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
                                // Busy 상태
                                turnStatus = TurnStatus.Busy;
                                // 컨베이어 구동
                                StartConveyor(autoVelocity);
                                if(beforeConv != null)
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
                                    CV_AutoStep = AutoStep.Step215_InMode_Move_CV_Rolling_In_Slow;
                                }


                                //NcvSeqLog(CV_AutoStep, enLogType.Process, enLogLevel.Normal, enLogTitle.TcvAutoStep, $"{ID} Auto ");
                                //Thread.Sleep(500);
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
                                if (nextConv != null && beforeConv != null)
                                {
                                    // Load위치와 Unload위치가 다를 때
                                    if (loadPOS != unloadPOS)
                                    {
                                        // 이전 컨베이어의 타입에 따라 분류
                                        if (beforeConv.type == "Long")
                                        {
                                            beforeConv.LCnV_CST_Empty_Detect();
                                            // 이전 컨베이어에서 Foup이 감지 되지 않는 경우 저속
                                            if (!beforeConv.longCnvCSTDetected)
                                            {
                                                StartConveyor(slowVelocity);

                                            }
                                        }
                                        else
                                        {
                                            beforeConv.CST_Empty_Detect();
                                            // 이전 컨베이어에서 Foup이 감지 되지 않는 경우 저속
                                            if (!beforeConv.isCSTEmpty)
                                            {
                                                StartConveyor(slowVelocity);
                                            }
                                        }
                                    }
                                    // Load 위치와 Unload 위치가 같은 경우
                                    else if (loadPOS == unloadPOS)
                                    {
                                        if (nextConv.mode == Mode.Alarm)
                                        {
                                            StopConveyor();
                                            _stepStopWatch.Reset();
                                            CV_AutoStep = AutoStep.Step280_InMode_Alarm_Condition_280;
                                        }
                                        // 이전 컨베이어의 타입에 따라 분류
                                        else if (nextConv.mode == Mode.Auto && beforeConv.type == "Long")
                                        {
                                            beforeConv.LCnV_CST_Empty_Detect();
                                            // 이전 컨베이어에서 Foup이 감지 되지 않는 경우
                                            if (!beforeConv.longCnvCSTDetected)
                                            {
                                                // 다음 컨베이어의 타입에 따라 분류
                                                if (nextConv.type == "Long")
                                                {
                                                    nextConv.LCnV_CST_Empty_Detect();
                                                    // 다음 컨베이어에 Foup이 있다면 저속
                                                    if (nextConv.longCnvCSTDetected)
                                                    {
                                                        StartConveyor(slowVelocity);
                                                    }
                                                }
                                                else
                                                {
                                                    nextConv.CST_Empty_Detect();
                                                    // 다음 컨베이어에 Foup이 있다면 저속
                                                    if (nextConv.isCSTEmpty)
                                                    {
                                                        StartConveyor(slowVelocity);
                                                    }
                                                }
                                            }
                                        }
                                        else if (nextConv.mode == Mode.Auto)
                                        {
                                            beforeConv.CST_Empty_Detect();
                                            // 이전 컨베이어에서 Foup이 감지 되지 않는 경우
                                            if (!beforeConv.isCSTEmpty)
                                            {
                                                // 다음 컨베이어의 타입에 따라 분류
                                                if (nextConv.type == "Long")
                                                {
                                                    nextConv.LCnV_CST_Empty_Detect();
                                                    // 다음 컨베이어에 Foup이 있다면 저속
                                                    if (nextConv.longCnvCSTDetected)
                                                    {
                                                        StartConveyor(slowVelocity);
                                                    }
                                                }
                                                else
                                                {
                                                    nextConv.CST_Empty_Detect();
                                                    // 다음 컨베이어에 Foup이 있다면 저속
                                                    if (nextConv.isCSTEmpty)
                                                    {
                                                        StartConveyor(slowVelocity);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    CV_AutoStep = AutoStep.Step220_InMode_Run_Condition_Check;
                                }
                                // 다음 컨베이어가 없는 경우 (마지막 컨베이어)
                                else if (nextConv == null)
                                {
                                    StartConveyor(slowVelocity);
                                    CV_AutoStep = AutoStep.Step220_InMode_Run_Condition_Check;
                                }
                                // 이전 컨베이어가 없는 경우 (시작 컨베이어)
                                else if (beforeConv == null)
                                {
                                    StartConveyor(slowVelocity);
                                    CV_AutoStep = AutoStep.Step220_InMode_Run_Condition_Check;
                                }
                            }
                            break;
                        /// <summary>
                        /// 1. Foup Detect#1,#2 모두 감지 되면 정지
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
                                // Load위치와 Unload위치가 같고 Foup이 정위치에 있을 때
                                if ((loadLocation == unloadLocation) && isCSTInPosition)
                                {
                                    if (nextConv != null)
                                    {
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
                                            // 다음 컨베이어가 비어 있는 경우
                                            if (!nextConv.isCSTEmpty)
                                            {
                                                CV_AutoStep = AutoStep.Step260_InMode_Unload_Turn_Stop;
                                            }
                                            // 다음 컨베이어에 Foup이 적재되어 있는 경우 정지
                                            else
                                            {
                                                StopConveyor();
                                                _stepStopWatch.Reset();
                                                CV_AutoStep = AutoStep.Step250_InMode_Move_CV_Rolling_Out;
                                            }
                                        }
                                        // 다음 컨베이어가 Long 컨베이어인 경우
                                        else if (nextConv.mode == Mode.Auto && nextConv.type == "Long")
                                        {
                                            nextConv.LCnV_CST_Empty_Detect();
                                            // 다음 컨베이어가 비어 있는 경우
                                            if (!nextConv.longCnvCSTDetected)
                                            {
                                                CV_AutoStep = AutoStep.Step260_InMode_Unload_Turn_Stop;
                                            }
                                            // 다음 컨베이어에 Foup이 적재되어 있는 경우 정지
                                            else
                                            {
                                                StopConveyor();
                                                _stepStopWatch.Reset();
                                                CV_AutoStep = AutoStep.Step250_InMode_Move_CV_Rolling_Out;
                                            }
                                        }
                                    }
                                    // 마지막 컨베이어인 경우 정지
                                    else
                                    {
                                        StopConveyor();
                                        _stepStopWatch.Reset();
                                        CV_AutoStep = AutoStep.Step900_Final_CV_Wait;
                                    }
                                }
                                // Load 위치와 Unload 위치가 다른 경우 정지
                                else if (isCSTInPosition)
                                {
                                    StopConveyor();
                                    _stepStopWatch.Reset();
                                    CV_AutoStep = AutoStep.Step330_InMode_Unload_Move;
                                }
                            }
                            break;

                        /// <summary>
                        /// 1.다음 컨베이어가 Load 할 수 있는 조건인지 확인
                        /// </summary>
                        case AutoStep.Step250_InMode_Move_CV_Rolling_Out:
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
                                if(nextConv.mode == Mode.Alarm)
                                {
                                    StopConveyor();
                                    CV_AutoStep = AutoStep.Step280_InMode_Alarm_Condition_280;
                                }
                                // 다음 컨베이어의 타입에 따라 분류
                                else if (nextConv.mode == Mode.Auto && nextConv.type == "Normal")
                                {
                                    nextConv.CST_Empty_Detect();
                                    // 다음 컨베이어가 비어 있는 경우
                                    if (!nextConv.isCSTEmpty)
                                    {
                                        // Unload 상태
                                        turnStatus = TurnStatus.Unload;
                                        // Load위치와 Unload위치가 다르고 POS Sensor의 차이가 2가 날 때
                                        //Ex) Load : Pos[0] / Unload : Pos[2]
                                        if(loadLocation != unloadLocation && ((unloadLocation - loadLocation) % 2 == 0))
                                        {
                                            Thread.Sleep(500);
                                            // 방향을 Neg로 변경 후 이동
                                            runtDirection = LineDirection.Output;
                                            StartConveyor(autoVelocity);
                                            // 즉시 방향 Input으로 변경
                                            runtDirection = LineDirection.Input;
                                        }
                                        else
                                        {
                                            Thread.Sleep(500);
                                            StartConveyor(autoVelocity);
                                        }
                                        CV_AutoStep = AutoStep.Step260_InMode_Unload_Turn_Stop;
                                    }
                                }
                                else if (nextConv.mode == Mode.Auto && nextConv.type == "Long")
                                {
                                    nextConv.LCnV_CST_Empty_Detect();
                                    // 다음 컨베이어가 비어 있는 경우
                                    if (!nextConv.longCnvCSTDetected)
                                    {
                                        // Unload 상태
                                        turnStatus = TurnStatus.Unload;
                                        // Load위치와 Unload위치가 다르고 POS Sensor의 차이가 2가 날 때
                                        //Ex) Load : Pos[0] / Unload : Pos[2]
                                        if (loadLocation != unloadLocation && ((unloadLocation - loadLocation) % 2 == 0))
                                        {
                                            Thread.Sleep(500);
                                            // 방향을 Neg로 변경 후 이동
                                            runtDirection = LineDirection.Output;
                                            StartConveyor(autoVelocity);
                                            // 즉시 방향 Input으로 변경
                                            runtDirection = LineDirection.Input;
                                        }
                                        else
                                        {
                                            Thread.Sleep(500);
                                            StartConveyor(autoVelocity);
                                        }
                                        CV_AutoStep = AutoStep.Step260_InMode_Unload_Turn_Stop;
                                    }
                                }
                            }
                            break;

                        /// <summary>
                        /// 1.다음 컨베이어로 Foup을 Unload한 경우 정지
                        /// </summary>
                        case AutoStep.Step260_InMode_Unload_Turn_Stop:
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
                                turnStatus = TurnStatus.Unload;
                                CST_Empty_Detect();
                                // Foup이 다 빠져 나간 경우
                                if (nextConv.mode == Mode.Alarm)
                                {
                                    StopConveyor();
                                    _stepStopWatch.Reset();
                                    CV_AutoStep = AutoStep.Step280_InMode_Alarm_Condition_280;
                                }
                                else if (!isCSTEmpty)
                                {
                                    // 0.3초 후 정지
                                    Thread.Sleep(300);
                                    StopConveyor();
                                    _stepStopWatch.Reset();
                                    // Load 위치와 Unload 위치가 같은 경우 초기화
                                    if (loadLocation == unloadLocation)
                                    {
                                        CV_AutoStep = AutoStep.Step000_Check_Direction;
                                    }
                                    // Load 위치와 Unload 위치가 다른 경우
                                    else
                                    {
                                        // 다음 컨베이어의 타입에 따라 분류
                                        if(nextConv.type == "Long")
                                        {
                                            nextConv.LCnV_CST_Empty_Detect();
                                            // 다음 컨베이어에 Foup이 감지된 경우
                                            if (nextConv.longCnvCSTDetected)
                                            {
                                                CV_AutoStep = AutoStep.Step350_InMode_T_Return_To_Load;
                                            }
                                        }
                                        else
                                        {
                                            nextConv.CST_Empty_Detect();
                                            // 다음 컨베이어에 Foup이 감지된 경우
                                            if (nextConv.isCSTEmpty)
                                            {
                                                CV_AutoStep = AutoStep.Step350_InMode_T_Return_To_Load;
                                            }
                                        }
                                    }
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
                                // Foup이 감지된 경우
                                if (isCSTEmpty)
                                {
                                    CV_AutoStep = AutoStep.Step210_InMode_Move_CV_Rolling_In;
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
                                // Unload 상태
                                turnStatus = TurnStatus.Load;
                                Unload_POS_Check();
                                CST_Empty_Detect();
                                // Unload 위치에 있지 않는 경우
                                if (!UnloadDetect)
                                {
                                    CV_AutoStep = AutoStep.Step710_OutMode_T_Init_Unload;
                                }
                                // Unload 위치에 위치해 있고 Foup이 감지 되는 경우
                                else if(UnloadDetect && isCSTEmpty)
                                {
                                    Thread.Sleep(2000);
                                    CV_AutoStep = AutoStep.Step610_OutMode_Move_CV_Rolling_In;
                                }
                                // Unload 위치에 위치해 있지만 Foup이 감지 되지 않는 경우
                                else if(nextConv != null && UnloadDetect && !isCSTEmpty)
                                {
                                    Thread.Sleep(100);
                                    CV_AutoStep = AutoStep.Step550_OutMode_Waitting_Before_Conv_Unload;
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
                                // Busy 상태
                                turnStatus = TurnStatus.Busy;
                                // 컨베이어 구동
                                if (loadLocation != unloadLocation && ((unloadLocation - loadLocation) % 2 == 0))
                                {
                                    runtDirection = LineDirection.Input;
                                }
                                StartConveyor(autoVelocity);
                                runtDirection = LineDirection.Output;
                                if (nextConv != null)
                                {
                                    // 이전 컨베이어 타입에 따라 분류
                                    if(nextConv.type == "Long")
                                    {
                                        nextConv.LCnV_CST_Empty_Detect();
                                        // 이전 컨베이어의 Foup 감지 센서가 모두 Off면 동작
                                        if (!nextConv.longCnvCSTDetected)
                                        {
                                            CV_AutoStep = AutoStep.Step615_OutMode_Move_CV_Rolling_In_Slow;
                                        }
                                    }
                                    else
                                    {
                                        nextConv.CST_Empty_Detect();
                                        // 이전 컨베이어의 Foup 감지 센서가 모두 Off면 동작
                                        if (!nextConv.isCSTEmpty)
                                        {
                                            CV_AutoStep = AutoStep.Step615_OutMode_Move_CV_Rolling_In_Slow;
                                        }
                                    }
                                }
                                // 이전 컨베이어가 없는 경우 (시작 컨베이어)
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
                                if (beforeConv != null && nextConv != null)
                                {
                                    // Load위치와 Unload위치가 다를 때
                                    if(loadPOS != unloadPOS)
                                    {
                                        // 이전 컨베이어의 타입에 따라 분류
                                        if(nextConv.type == "Long")
                                        {
                                            nextConv.LCnV_CST_Empty_Detect();
                                            // 이전 컨베이어에서 Foup이 감지 되지 않는 경우 저속
                                            if (!nextConv.longCnvCSTDetected)
                                            {
                                                if (loadLocation != unloadLocation && ((unloadLocation - loadLocation) % 2 == 0))
                                                {
                                                    runtDirection = LineDirection.Input;
                                                }
                                                StartConveyor(slowVelocity);
                                                runtDirection = LineDirection.Output;
                                            }
                                        }
                                        else
                                        {
                                            nextConv.CST_Empty_Detect();
                                            // 이전 컨베이어에서 Foup이 감지 되지 않는 경우 저속
                                            if (!nextConv.isCSTEmpty)
                                            {
                                                if (loadLocation != unloadLocation && ((unloadLocation - loadLocation) % 2 == 0))
                                                {
                                                    runtDirection = LineDirection.Input;
                                                }
                                                StartConveyor(slowVelocity);
                                                runtDirection = LineDirection.Output;
                                            }
                                        }
                                    }
                                    // Load 위치와 Unload 위치가 같은 경우
                                    else if(loadPOS == unloadPOS)
                                    {
                                        if (beforeConv.mode == Mode.Alarm)
                                        {
                                            StopConveyor();
                                            _stepStopWatch.Reset();
                                            CV_AutoStep = AutoStep.Step680_OutMode_Alarm_Condition_680;
                                        }
                                        // 이전 컨베이어의 타입에 따라 분류
                                        else if (beforeConv.mode == Mode.Auto && nextConv.type == "Long")
                                        {
                                            nextConv.LCnV_CST_Empty_Detect();
                                            // 이전 컨베이어에서 Foup이 감지 되지 않는 경우
                                            if (!nextConv.longCnvCSTDetected)
                                            {
                                                // 다음 컨베이어의 타입에 따라 분류
                                                if(beforeConv.type == "Long")
                                                {
                                                    beforeConv.LCnV_CST_Empty_Detect();
                                                    // 다음 컨베이어에 Foup이 있다면 저속
                                                    if (beforeConv.longCnvCSTDetected)
                                                    {
                                                        StartConveyor(slowVelocity);
                                                    }
                                                }
                                                else
                                                {
                                                    beforeConv.CST_Check_Detect();
                                                    // 다음 컨베이어에 Foup이 있다면 저속
                                                    if (beforeConv.isCSTEmpty)
                                                    {
                                                        StartConveyor(slowVelocity);
                                                    }
                                                }
                                            }
                                        }
                                        else if(beforeConv.mode == Mode.Auto)
                                        {
                                            nextConv.CST_Empty_Detect();
                                            // 이전 컨베이어에서 Foup이 감지 되지 않는 경우
                                            if (!nextConv.isCSTEmpty)
                                            {
                                                // 다음 컨베이어의 타입에 따라 분류
                                                if(beforeConv.type == "Long")
                                                {
                                                    beforeConv.LCnV_CST_Empty_Detect();
                                                    // 다음 컨베이어에 Foup이 있다면 저속
                                                    if (beforeConv.longCnvCSTDetected)
                                                    {
                                                        StartConveyor(slowVelocity);
                                                    }
                                                }
                                                else
                                                {
                                                    beforeConv.CST_Empty_Detect();
                                                    // 다음 컨베이어에 Foup이 있다면 저속
                                                    if (beforeConv.isCSTEmpty)
                                                    {
                                                        StartConveyor(slowVelocity);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    CV_AutoStep = AutoStep.Step620_OutMode_Run_Condition_Check;
                                }
                                // 다음 컨베이어가 없는 경우 (마지막 컨베이어)
                                else if(beforeConv == null)
                                {
                                    if (loadLocation != unloadLocation && ((unloadLocation - loadLocation) % 2 == 0))
                                    {
                                        runtDirection = LineDirection.Input;
                                    }
                                    StartConveyor(autoVelocity);
                                    runtDirection = LineDirection.Output;
                                    CV_AutoStep = AutoStep.Step620_OutMode_Run_Condition_Check;
                                }
                                // 이전 컨베이어가 없는 경우 (시작 컨베이어)
                                else if(nextConv == null)
                                {
                                    if (loadLocation != unloadLocation && ((unloadLocation - loadLocation) % 2 == 0))
                                    {
                                        runtDirection = LineDirection.Input;
                                    }
                                    StartConveyor(autoVelocity);
                                    runtDirection = LineDirection.Output;
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
                                // Load위치와 Unload위치가 같고 Foup이 정위치에 있을 때
                                if((loadLocation == unloadLocation) && isCSTInPosition)
                                {
                                    if(beforeConv != null)
                                    {
                                        if(beforeConv.mode == Mode.Alarm)
                                        {
                                            StopConveyor();
                                            _stepStopWatch.Reset();
                                            CV_AutoStep = AutoStep.Step680_OutMode_Alarm_Condition_680;
                                        }
                                        // 다음 컨베이어의 타입에 따라 분류
                                        else if(beforeConv.mode == Mode.Auto && beforeConv.type == "Normal")
                                        {
                                            beforeConv.CST_Empty_Detect();
                                            // 다음 컨베이어가 비어 있는 경우
                                            if (!beforeConv.isCSTEmpty)
                                            {
                                                CV_AutoStep = AutoStep.Step660_OutMode_Unload_Turn_Stop;
                                            }
                                            // 다음 컨베이어에 Foup이 적재되어 있는 경우
                                            else
                                            {
                                                StopConveyor();
                                                _stepStopWatch.Reset();
                                                CV_AutoStep = AutoStep.Step650_OutMode_Move_CV_Rolling_Out;
                                            }
                                        }
                                        // 다음 컨베이어가 Long 컨베이어인 경우
                                        else if(beforeConv.mode == Mode.Auto && beforeConv.type == "Long")
                                        {
                                            beforeConv.LCnV_CST_Empty_Detect();
                                            // 다음 컨베이어가 비어 있는 경우
                                            if (!beforeConv.longCnvCSTDetected)
                                            {
                                                CV_AutoStep = AutoStep.Step660_OutMode_Unload_Turn_Stop;
                                            }
                                            // 다음 컨베이어에 Foup이 적재되어 있는 경우 정지
                                            else
                                            {
                                                StopConveyor();
                                                _stepStopWatch.Reset();
                                                CV_AutoStep = AutoStep.Step650_OutMode_Move_CV_Rolling_Out;
                                            }
                                        }
                                    }
                                    // 마지막 컨베이어인 경우 정지
                                    else
                                    {
                                        StopConveyor();
                                        _stepStopWatch.Reset();
                                        CV_AutoStep = AutoStep.Step900_Final_CV_Wait;
                                    }
                                }
                                // Load 위치와 Unload 위치가 다른 경우 정지
                                else if (isCSTInPosition)
                                {
                                    StopConveyor();
                                    _stepStopWatch.Reset();
                                    CV_AutoStep = AutoStep.Step730_OutMode_Load_Move;
                                }
                            }
                            break;

                        case AutoStep.Step650_OutMode_Move_CV_Rolling_Out:
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
                                if(beforeConv.mode == Mode.Alarm)
                                {
                                    StopConveyor();
                                    CV_AutoStep = AutoStep.Step680_OutMode_Alarm_Condition_680;
                                }
                                // 다음 컨베이어의 타입에 따라 분류
                                else if(beforeConv.mode == Mode.Auto && beforeConv.type == "Normal")
                                {
                                    beforeConv.CST_Empty_Detect();
                                    // 다음 컨베이어가 비어 있는 경우
                                    if (!beforeConv.isCSTEmpty)
                                    {
                                        // Load 상태
                                        turnStatus = TurnStatus.Unload;
                                        // Load위치와 Unload위치가 다르고 POS Sensordml 차이가 2가 날 때
                                        // Ex) Load : Pos[0] / Unload : Pos[2]
                                        if(loadLocation != unloadLocation && ((unloadLocation - loadLocation) % 2 == 0))
                                        {
                                            Thread.Sleep(500);
                                            // 방향을 Pos로 변경 후 이동
                                            runtDirection = LineDirection.Output;
                                            StartConveyor(autoVelocity);
                                        }
                                        else
                                        {
                                            Thread.Sleep(500);
                                            StartConveyor(autoVelocity);
                                        }
                                        CV_AutoStep = AutoStep.Step660_OutMode_Unload_Turn_Stop;
                                    }
                                }
                                else if(beforeConv.mode == Mode.Auto && beforeConv.type == "Long")
                                {
                                    beforeConv.LCnV_CST_Empty_Detect();
                                    // 다음 컨베이어가 비어 있는 경우
                                    if (!beforeConv.longCnvCSTDetected)
                                    {
                                        // Unload 상태
                                        turnStatus= TurnStatus.Unload;
                                        // Load위치와 Unload위치가 다르고 POS Sensordml 차이가 2가 날 때
                                        // Ex) Load : Pos[0] / Unload : Pos[2]
                                        if (loadLocation != unloadLocation && ((unloadLocation - loadLocation) % 2 == 0))
                                        {
                                            Thread.Sleep(500);
                                            // 방향을 Pos로 변경 후 이동
                                            runtDirection = LineDirection.Output;
                                            StartConveyor(autoVelocity);
                                        }
                                        else
                                        {
                                            Thread.Sleep(500);
                                            StartConveyor(autoVelocity);
                                        }
                                        CV_AutoStep = AutoStep.Step660_OutMode_Unload_Turn_Stop;
                                    }
                                }
                            }
                            break;

                        case AutoStep.Step660_OutMode_Unload_Turn_Stop:
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
                                turnStatus = TurnStatus.Unload;
                                CST_Empty_Detect();
                                if (beforeConv.mode == Mode.Alarm)
                                {
                                    StopConveyor();
                                    _stepStopWatch.Reset();
                                    CV_AutoStep = AutoStep.Step680_OutMode_Alarm_Condition_680;
                                }
                                // Foup이 다 빠져 나간 경우
                                else if (!isCSTEmpty)
                                {
                                    // 0.3초 후 정지
                                    Thread.Sleep(300);
                                    StopConveyor();
                                    _stepStopWatch.Reset();
                                    // Load 위치와 Unload 위치가 같은 경우 초기화
                                    if (loadLocation == unloadLocation)
                                    {
                                        CV_AutoStep = AutoStep.Step000_Check_Direction;
                                    }
                                    // Load 위치와 Unload 위치가 다른 경우
                                    else
                                    {
                                        // 다음 컨베이어의 타입에 따라 분류
                                        if(beforeConv.type == "Long")
                                        {
                                            beforeConv.LCnV_CST_Empty_Detect();
                                            // 다음 컨베이어에 Foup이 감지된 경우
                                            if (beforeConv.longCnvCSTDetected)
                                            {
                                                CV_AutoStep = AutoStep.Step750_OutMode_T_Return_To_Unload;
                                            }
                                        }
                                        else
                                        {
                                            beforeConv.CST_Empty_Detect();
                                            // 다음 컨베이어에 Foup이 감지된 경우
                                            if (beforeConv.isCSTEmpty)
                                            {
                                                CV_AutoStep = AutoStep.Step750_OutMode_T_Return_To_Unload;
                                            }
                                        }
                                    }
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
                                // Foup이 감지된 경우
                                if (isCSTEmpty)
                                {
                                    CV_AutoStep = AutoStep.Step610_OutMode_Move_CV_Rolling_In;
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
                                    CV_AutoStep = AutoStep.Step290_InMode_Alarm_Wait_290;
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
                        /// 1. 이전 Conveyor에 Foup이 적재되면 구동
                        /// </summary>
                        case AutoStep.Step150_InMode_Waitting_Before_Conv_Unload:
                            {
                                // 이전 컨베이어의 Auto Step Check 후 동작
                                if (beforeConv.CV_AutoStep == AutoStep.Step250_InMode_Move_CV_Rolling_Out)
                                {
                                    // Busy 상태
                                    turnStatus = TurnStatus.Busy;
                                    StartConveyor(autoVelocity);
                                    CV_AutoStep = AutoStep.Step155_InMode_Change_210;
                                }
                            }
                            break;

                        case AutoStep.Step550_OutMode_Waitting_Before_Conv_Unload:
                            {
                                if(nextConv.CV_AutoStep == AutoStep.Step650_OutMode_Move_CV_Rolling_Out)
                                {
                                    turnStatus = TurnStatus.Busy;
                                    if(loadLocation != unloadLocation && ((unloadLocation - loadLocation) % 2 == 0))
                                    {
                                        runtDirection = LineDirection.Input;
                                    }
                                    StartConveyor(autoVelocity);
                                    runtDirection = LineDirection.Output;
                                    CV_AutoStep = AutoStep.Step555_OutMode_Change_610;
                                }
                            }
                            break;

                        case AutoStep.Step310_InMode_T_Init_Load:
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
                                    AddAlarm_Conveyor(this, ConveyorAlarm.Turn_Step_Time_Over_Error);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                // Load 위치로 이동
                                Absolute_Turn_Move_Load();
                                turnStatus = TurnStatus.Busy;
                                CV_AutoStep = AutoStep.Step320_InMode_T_Check_Init_Load;
                            }
                            break;

                        case AutoStep.Step320_InMode_T_Check_Init_Load:
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
                                    AddAlarm_Conveyor(this, ConveyorAlarm.Turn_Step_Time_Over_Error);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                Load_POS_Check();
                                CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
                                // Load 위치와 Current POS값이 같고  Load POS Sensor가 On인 경우
                                if (LoadDetect && cmAxis.ActualPos == loadPOS)
                                {
                                    if(isInterface && isAutoRun && !isCSTEmpty)
                                    {
                                        isPIORun = true;
                                    }
                                    _stepStopWatch.Reset();
                                    CV_AutoStep = AutoStep.Step000_Check_Direction;
                                }
                            }
                            break;

                        /// <summary>
                        /// 1. Turn Coveyor Unload위치로 구동
                        /// </summary>
                        case AutoStep.Step330_InMode_Unload_Move:
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
                                    AddAlarm_Conveyor(this, ConveyorAlarm.Turn_Step_Time_Over_Error);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                CST_Empty_Detect();
                                CST_Check_Detect();
                                if (!isCSTEmpty)
                                {
                                    mode = Mode.Alarm;
                                    AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                else if (!isCSTInPosition)
                                {
                                    mode = Mode.Alarm;
                                    AddAlarm_Conveyor(this, ConveyorAlarm.CST_Over_Run);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                // Load 위치와 Unload 위치가 같은 경우
                                if (loadLocation == unloadLocation)
                                {
                                    // 다음 컨베이어가 있는 경우
                                    if (nextConv != null)
                                    {
                                        CV_AutoStep = AutoStep.Step250_InMode_Move_CV_Rolling_Out;
                                    }
                                    // 다음 컨베이어가 없는 경우 (마지막 컨베이어)
                                    else
                                    {
                                        CV_AutoStep = AutoStep.Step900_Final_CV_Wait;
                                    }
                                }
                                // Load 위치와 Unload 위치가 다른 경우
                                else
                                {
                                    // 0.3초 후 Unload위치로 이동
                                    Thread.Sleep(300);
                                    Absolute_Turn_Move_Unload();
                                    CV_AutoStep = AutoStep.Step340_InMode_T_Check_Unload_Location;
                                }
                            }
                            break;

                        /// <summary>
                        /// 1. Unload위치에 도착 시 정지
                        /// </summary>
                        case AutoStep.Step340_InMode_T_Check_Unload_Location:
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
                                    AddAlarm_Conveyor(this, ConveyorAlarm.Turn_Step_Time_Over_Error);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                CST_Empty_Detect();
                                CST_Check_Detect();
                                if (!isCSTEmpty)
                                {
                                    mode = Mode.Alarm;
                                    AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                else if (!isCSTInPosition)
                                {
                                    mode = Mode.Alarm;
                                    AddAlarm_Conveyor(this, ConveyorAlarm.CST_Over_Run);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                Unload_POS_Check();
                                CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
                                // Unload Pos값과 Current Pos값이 같고 Unload POS Sensor가 On인 경우
                                if (UnloadDetect && cmAxis.ActualPos == unloadPOS)
                                {
                                    // 다음 컨베이어가 있는 경우
                                    if (nextConv != null)
                                    {
                                        CV_AutoStep = AutoStep.Step250_InMode_Move_CV_Rolling_Out;
                                    }
                                    // 다음 컨베이어가 없는 경우 (마지막 컨베이어)
                                    else
                                    {
                                        CV_AutoStep = AutoStep.Step900_Final_CV_Wait;
                                    }
                                }
                            }
                            break;

                        /// <summary>
                        /// 1.Load위치로 Turn Conveyor 복귀
                        /// </summary>
                        case AutoStep.Step350_InMode_T_Return_To_Load:
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
                                    AddAlarm_Conveyor(this, ConveyorAlarm.Turn_Step_Time_Over_Error);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                // Busy 상태
                                turnStatus = TurnStatus.Busy;
                                // 0.5초 후 Load 위치로 이동
                                Thread.Sleep(300);
                                Absolute_Turn_Move_Load();
                                CV_AutoStep = AutoStep.Step360_InMode_T_Check_Return_To_Load;
                            }
                            break;

                        /// <summary>
                        /// 1.Load위치에 위치 시 Turn Conveyor 정지 후 Step 초기화
                        /// </summary>
                        case AutoStep.Step360_InMode_T_Check_Return_To_Load:
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
                                    AddAlarm_Conveyor(this, ConveyorAlarm.Turn_Step_Time_Over_Error);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                Load_POS_Check();
                                CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
                                // Load Pos 값과 Current POS값이 같고 Load POS Sensor가 On인 경우
                                if (LoadDetect && cmAxis.ActualPos == loadPOS)
                                {
                                    CV_AutoStep = AutoStep.Step000_Check_Direction;
                                }
                            }
                            break;

                        case AutoStep.Step710_OutMode_T_Init_Unload:
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
                                    AddAlarm_Conveyor(this, ConveyorAlarm.Turn_Step_Time_Over_Error);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                // Unload 위치로 이동
                                Absolute_Turn_Move_Unload();
                                turnStatus = TurnStatus.Busy;
                                CV_AutoStep = AutoStep.Step720_OutMode_T_Check_Init_Unload;
                            }
                            break;

                        case AutoStep.Step720_OutMode_T_Check_Init_Unload:
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
                                    AddAlarm_Conveyor(this, ConveyorAlarm.Turn_Step_Time_Over_Error);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                Unload_POS_Check();
                                CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
                                // Unload 위치와 Current POS값이 같고 Load POS Sensor가 On인 경우
                                if(UnloadDetect && cmAxis.ActualPos == unloadPOS)
                                {
                                    CV_AutoStep = AutoStep.Step000_Check_Direction;
                                }
                            }
                            break;

                        case AutoStep.Step730_OutMode_Load_Move:
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
                                    AddAlarm_Conveyor(this, ConveyorAlarm.Turn_Step_Time_Over_Error);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                CST_Empty_Detect();
                                CST_Check_Detect();
                                if (!isCSTEmpty)
                                {
                                    mode = Mode.Alarm;
                                    AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                else if (!isCSTInPosition)
                                {
                                    mode = Mode.Alarm;
                                    AddAlarm_Conveyor(this, ConveyorAlarm.CST_Over_Run);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                // Load 위치와 Unload 위치가 같은 경우
                                if (loadLocation == unloadLocation)
                                {
                                    // 다음 컨베이어가 있는 경우
                                    if(beforeConv != null)
                                    {
                                        CV_AutoStep = AutoStep.Step650_OutMode_Move_CV_Rolling_Out;
                                    }
                                    // 다음 컨베이어가 없는 경우 (마지막 컨베이어)
                                    else
                                    {
                                        CV_AutoStep = AutoStep.Step900_Final_CV_Wait;
                                    }
                                }
                                // Load 위치와 Unload 위치가 다른 경우
                                else
                                {
                                    // 0.3초 후 Load위치로 이동
                                    Thread.Sleep(300);
                                    Absolute_Turn_Move_Load();
                                    CV_AutoStep = AutoStep.Step740_OutMode_Check_Load_Location;
                                }
                            }
                            break;
                        
                        case AutoStep.Step740_OutMode_Check_Load_Location:
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
                                    AddAlarm_Conveyor(this, ConveyorAlarm.Turn_Step_Time_Over_Error);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                CST_Empty_Detect();
                                CST_Check_Detect();
                                if (!isCSTEmpty)
                                {
                                    mode = Mode.Alarm;
                                    AddAlarm_Conveyor(this, ConveyorAlarm.CST_Empty);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                else if (!isCSTInPosition)
                                {
                                    mode = Mode.Alarm;
                                    AddAlarm_Conveyor(this, ConveyorAlarm.CST_Over_Run);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                Load_POS_Check();
                                CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
                                // Load Pos값과 Current Pos값이 같고 Load POS Sensor가 On인 경우
                                if(LoadDetect && cmAxis.ActualPos == loadPOS)
                                {
                                    // 다음 컨베이어가 있는 경우
                                    if(beforeConv != null)
                                    {
                                        CV_AutoStep = AutoStep.Step650_OutMode_Move_CV_Rolling_Out;
                                    }
                                    // 다음 컨베이어가 없는 경우 (마지막 컨베이어)
                                    else
                                    {
                                        CV_AutoStep = AutoStep.Step900_Final_CV_Wait;
                                    }
                                }
                            }
                            break;

                        case AutoStep.Step750_OutMode_T_Return_To_Unload:
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
                                    AddAlarm_Conveyor(this, ConveyorAlarm.Turn_Step_Time_Over_Error);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                // Busy 상태
                                turnStatus = TurnStatus.Busy;
                                // 0.5초 후 Unload 위치로 이동
                                Thread.Sleep(300);
                                Absolute_Turn_Move_Unload();
                                CV_AutoStep = AutoStep.Step760_OutMode_T_Check_Return_To_Unload;
                            }
                            break;

                        case AutoStep.Step760_OutMode_T_Check_Return_To_Unload:
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
                                    AddAlarm_Conveyor(this, ConveyorAlarm.Turn_Step_Time_Over_Error);
                                    del_alarm?.Invoke();
                                    AutoRunning = false;
                                    CycleRunning = false;
                                }
                                Unload_POS_Check();
                                CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
                                // Unload Pos 값과 Current POS값이 같고 Unload POS Sensor가 On인 경우
                                if(UnloadDetect && cmAxis.ActualPos == unloadPOS)
                                {
                                    CV_AutoStep = AutoStep.Step000_Check_Direction;
                                }
                            }
                            break;

                        case AutoStep.Step900_Final_CV_Wait:
                            {
                                if (isCycleRun)
                                {
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
                    if (prestep != CV_AutoStep)
                    {
                        Console.WriteLine(ID + ":" + CV_AutoStep + "--"+_stepStopWatch.Elapsed);
                        prestep = CV_AutoStep;
                    }
                    
                    Thread.Sleep(10); // CPU 사용량 감소를 위한 대기
                }
                Console.WriteLine(ID + ":" + "Auto Stop");
                _stepStopWatch.Reset();
                StopConveyor();
                TurnJogSTOP();
                if (mode != Mode.Alarm)
                { mode = Mode.Manual; }
                AutoRunning = false;
                CycleRunning = false;
                G_Var.isAutoRun = false;
                G_Var.isCycleRun = false;
            });
            LocalThread.Start();
        }
        public TurnConv(int id, int axis, bool Isinterface) : base(id, axis, Isinterface)
        {
            type = "Turn";
            turnAxis = base.axis + 1;
            bits.Add(2);
            bits.Add(3);
            bitsTurn.Add(6);
            bitsTurn.Add(2);
            bitsTurn.Add(3);
            bitTurn = 6;
        }
        public override void AddrUpdate()
        {
            addr = 8 + axis * 4;
            addrTurn = addr + 4;
        }
    }
}
