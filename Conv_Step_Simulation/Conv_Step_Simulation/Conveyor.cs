using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using WMX3ApiCLR;
using static Conv_Step_Simulation.G_Var;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using static Conv_Step_Simulation.Conveyor;
using System.Diagnostics;

namespace Conv_Step_Simulation
{
    public abstract partial class Conveyor
    {
        public AutoStep prestep;

        public Stopwatch _stepStopWatch = new Stopwatch();

        public bool isCSTEmpty = false;
        public bool isCSTInPosition;

        /// <summary>
        /// Turn Sensor 변수
        /// </summary>
        public bool LoadDetect;
        public bool UnloadDetect;
        public bool POS_0_DegDetect;
        public bool POS_90_DegDetect;
        public bool POS_180_DegDetect;
        public bool POS_270_DegDetect;

        public bool AutoStopEnable;
        public bool AutoRunning = false;
        public bool CycleRunning = false;
        public bool turn_CV_InitComp = false;
        public bool initComp = false;

        public Conveyor beforeConv = null;
        public Conveyor nextConv = null;


        public enum RunDirection
        {
            LowCw,
            HighCw,
            LowCcw,
            HighCcw,
        }
        public enum CnvRun
        {
            Stop,
            Run
        }
        public enum Idle
        {
            Idle,
            Busy
        }
        public enum TurnStatus
        {
            Load,
            Busy,
            Unload,
        }

        /// <summary>
        /// 컨베이어 동작에 대한 정의
        /// </summary>
        public enum AutoStep
        {
            Step000_Check_Direction = 0,
            // InMode Step
            Step100_InMode_Check_CST_Load_Status = 100,
            Step150_InMode_Waitting_Before_Conv_Unload = 150,
            Step155_InMode_Change_210 = 155,

            // 구동 Step 정의
            Step210_InMode_Move_CV_Rolling_In = 210,
            Step215_InMode_Move_CV_Rolling_In_Slow = 215,
            Step220_InMode_Run_Condition_Check = 220,
            Step250_InMode_Move_CV_Rolling_Out = 250,
            Step255_InMode_Wait_NextConv = 255,
            Step260_InMode_Unload_Turn_Stop = 260,
            Step270_InMode_Move_CST_Pass_CV_to_TCV = 270,
            Step280_InMode_Alarm_Condition_280 = 280,
            Step290_InMode_Alarm_Wait_290 = 290,

            // Turn Axis Step 정의
            Step310_InMode_T_Init_Load = 310, // Loading Pos 외 다른 Pos Sensor Check시 int();
            Step320_InMode_T_Check_Init_Load = 320,
            Step330_InMode_Unload_Move = 330,
            Step340_InMode_T_Check_Unload_Location = 340,
            Step350_InMode_T_Return_To_Load = 350,
            Step360_InMode_T_Check_Return_To_Load = 360,

            // OutMode Step
            Step500_OutMode_Check_CST_Load_Status = 500,
            Step550_OutMode_Waitting_Before_Conv_Unload = 550,
            Step555_OutMode_Change_610 = 555,

            // 구동 Step 정의
            Step610_OutMode_Move_CV_Rolling_In = 610,
            Step615_OutMode_Move_CV_Rolling_In_Slow = 615,
            Step620_OutMode_Run_Condition_Check = 620,
            Step650_OutMode_Move_CV_Rolling_Out = 650,
            Step655_OutMode_Wait_NextConv = 655,
            Step660_OutMode_Unload_Turn_Stop = 660,
            Step670_OutMode_Move_CST_Pass_CV_to_TCV = 670,
            Step680_OutMode_Alarm_Condition_680 = 680,
            Step690_OutMode_Alarm_Wait_690 = 690,

            // Turn Axis Step 정의
            Step710_OutMode_T_Init_Unload = 710,
            Step720_OutMode_T_Check_Init_Unload = 720,
            Step730_OutMode_Load_Move = 730,
            Step740_OutMode_Check_Load_Location = 740,
            Step750_OutMode_T_Return_To_Unload = 750,
            Step760_OutMode_T_Check_Return_To_Unload = 760,

            Step900_Final_CV_Wait = 900,

            Step1000_FinalCVN_Wait = 1000
        }
        public enum TurnInitStep
        {
            Step000_POS_Check = 0,
            Step100_Foup_Check = 100,
            Step110_Foup_Both_Check = 110,

            Step200_Turn_Move = 200,
            Step210_Turn_Stop_Check = 210,

            Step500_Turn_Init_Done = 500,
        }
        public enum InitStep
        {
            Step000_Foup_Check = 0,
            Step100_Move = 100,

            Step500_Init_Done = 500,
        }
        public enum PIOStep
        {
            Step000_Direction_Check = 0,

            Step200_Inmode_Check_Foup = 200,
            Step210_InMode_Await_PIO_CS = 210,                            //Carrier Load 요청 (AGV or OHT)
            Step220_InMode_Check_PIO_Valid = 220,
            //Step221_InMode_Call_AGV_Load_Req                = 221,      //AGV PIO
            Step230_InMode_Check_PIO_TR = 230,
            Step240_InMode_Check_PIO_Busy = 240,
            Step245_InMode_Check_PIO_BusyDone = 241,
            Step250_InMode_Check_PIO_Complete = 250,
            Step260_InMode_Check_PIO_End = 260,
            Step270_InMode_Check_CST_Load_And_Safe = 270,

            Step290_InMode_Await_MGV_CST_Load = 290,                      //Carrier Load 대기 (MGV), Carrier ID Clear


            Step300_Outmode_Check_Foup = 300,
            Step310_OutMode_Await_PIO_CS = 310,                          //Carrier Unload 대기 (AGV or OHT)
            Step320_OutMode_Check_PIO_Valid = 320,
            //Step321_OutMode_Call_AGV_Unload_Req             = 321,
            Step330_OutMode_Check_PIO_TR = 330,
            Step340_OutMode_Check_PIO_Busy = 340,
            Step350_OutMode_Check_PIO_Complete = 350,
            Step360_OutMode_Check_PIO_End = 360,
            Step370_OutMode_Check_LP_CST_Unload_And_Safe = 370,          //Carrier ID Clear

            Step390_OutMode_Await_MGV_CST_Unload = 390,                  //Carrier Unload 대기 (MGV), Carrier ID Clear
        }
        public int loadLocation = 0;
        public int unloadLocation = 0;

        public double loadPOS;
        public double loadInterlock_Plus;
        public double loadInterlock_Minus;

        public double unloadPOS;
        public double unloadInterlock_Plus;
        public double unloadInterlock_Minus;

        public bool isInitDone = false;
        public enum LineDirection
        {
            Input,
            Output
        }

        public AutoStep CV_AutoStep = AutoStep.Step000_Check_Direction;
        public InitStep CV_InitStep = InitStep.Step000_Foup_Check;
        public TurnInitStep TCV_InitStep = TurnInitStep.Step000_POS_Check;

        public LineDirection runtDirection = LineDirection.Input;

        public int addr;
        public List<int> bits;

        public int addrTurn;
        public List<int> bitsTurn;
        public int bitTurn;

        public bool IsHomeDone = false;

        public RunDirection runDirection = RunDirection.HighCw;
        public CnvRun run = CnvRun.Stop;
        public ServoOnOff servo = ServoOnOff.Off;
        public ServoOnOff previousServo = ServoOnOff.Off;
        public Mode mode = Mode.Manual;
        public Mode previousMode;
        public AlarmStatus alarmstatus = AlarmStatus.OK;
        public Auto auto = Auto.Disable;
        public SensorOnOff[] CSTDetect = new SensorOnOff[2];
        public SensorOnOff[] longCnvFoupDetect = new SensorOnOff[3];
        public SensorOnOff[] POS = new SensorOnOff[4];
        public InOutMode inoutMode = InOutMode.InMode;
        public TurnStatus turnStatus = TurnStatus.Load;


        public PIOStep CV_InterfaceStep = new PIOStep();
        public PIOStep preInterfaceStep;

        public int axis = -1;
        public int turnAxis = -2;
        public int ID { get; set; }
        public string type { get; set; }
        public string name;
        public double autoVelocity = 100;
        public double autoAcc = 150;
        public double autoDec = 150;
        public double slowVelocity = 20;
        public double initVelocity = 10;
        public bool isInterface = false;
        public bool isPIORun = false;
        public Conveyor(int id, int axis, bool Isinterface)
        {
            ID = id;
            this.axis = axis;
            bits = new List<int>();
            bitsTurn = new List<int>();
            isInterface = Isinterface;
            if(isInterface)
            {
                isPIORun = true;
            }
            Console.WriteLine(this.ID + ":" + isInterface);
        }
        /// <summary>
        /// Mode 검사
        /// </summary>
        /// <returns></returns>
        public void InmodeConvCheck()
        {
            Conveyor beforeConveyor = conveyors.FirstOrDefault(c => c.ID == this.ID - 1);
            Conveyor nNextConveyor = conveyors.FirstOrDefault(c => c.ID == this.ID + 1);
            if (beforeConveyor != null && beforeConveyor.ID > 0)
            {
                beforeConv = beforeConveyor;
            }
            if (nNextConveyor != null && nNextConveyor.ID > 0)
            {
                nextConv = nNextConveyor;
            }
        }
        /// <summary>
        /// Direction 검사
        /// </summary>
        /// <returns></returns>
        public LineDirection GetOperationDirection()
        {
            bool isInputMode = CheckDirection();

            return isInputMode ? LineDirection.Input : LineDirection.Output;
        }
        private bool CheckDirection()
        {
            if(runtDirection == LineDirection.Input)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Foup이 없는 상태 검사
        /// </summary>
        public void CST_Empty_Detect()
        {
            int count = 0;
            // LFoupDetect 배열을 확인하여 자재 유무 체크
            for (int i = 0; i < CSTDetect.Length; i++)
            {
                if (CSTDetect[i] == SensorOnOff.Off) // SensorOnOff 열거형을 사용하여 센서 상태 확인
                {
                    count++;
                }

            }
            if (count > 0)
            {
                isCSTEmpty = true;
            }
            else
            {
                isCSTEmpty = false;
            }
        }
        /// <summary>
        /// Foup_Detect#1,#2 모두 On 상태인지 검사
        /// </summary>
        public void CST_Check_Detect()
        {
            int count = 0;
            // FoupDetect 배열을 확인하여 자재 유무 체크
            for (int i = 0; i < CSTDetect.Length; i++)
            {
                if (CSTDetect[i] == SensorOnOff.On) // SensorOnOff 열거형을 사용하여 센서 상태 확인
                {
                    count++;
                }
            }
            if (count > 0)
            {
                isCSTInPosition = false;
            }
            else
            {
                isCSTInPosition = true;
            }
        }
        public void Load_POS_Check()
        {
            CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
            if (POS[loadLocation] == SensorOnOff.On && cmAxis.ActualPos == loadPOS)
            {
                LoadDetect = true;
            }
            else
            {
                LoadDetect = false;
            }
        }
        public void Unload_POS_Check()
        {
            CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
            if (POS[unloadLocation] == SensorOnOff.On && cmAxis.ActualPos == unloadPOS)
            {
                UnloadDetect = true;
            }
            else
            {
                UnloadDetect = false;
            }
        }
        public void Load_Location_Check()
        {

        }
        /// <summary>
        /// 자재가 떠나서 Foup_Detect#2가 꺼졌는지 검사
        /// </summary>
        public bool CV_Inmode_LastFoup_Detect_Check()
        {
            bool isLast_Foup_Exist;
            if (CSTDetect[CSTDetect.Length - 1] == SensorOnOff.Off)
            {
                isLast_Foup_Exist = true;
            }
            else
            {
                isLast_Foup_Exist = false;
            }
            return isLast_Foup_Exist;
        }
        public bool CV_Outmode_LastFoup_Detect_Check()
        {
            bool IsLast_Foup_Exist;
            if (CSTDetect[0] == SensorOnOff.Off)
            {
                IsLast_Foup_Exist = true;
            }
            else
            {
                IsLast_Foup_Exist = false;
            }
            return IsLast_Foup_Exist;
        }
        public bool LCV_Inmode_LastFoup_Detect_Check()
        {
            bool IsLast_Foup_Exist;
            if (longCnvFoupDetect[longCnvFoupDetect.Length - 1] == SensorOnOff.Off)
            {
                IsLast_Foup_Exist = true;
            }
            else
            {
                IsLast_Foup_Exist = false;
            }
            return IsLast_Foup_Exist;
        }
        public bool LCV_Outmode_LastFoup_Detect_Check()
        {
            bool IsLast_Foup_Exist;
            if (longCnvFoupDetect[0] == SensorOnOff.Off)
            {
                IsLast_Foup_Exist = true;
            }
            else
            {
                IsLast_Foup_Exist = false;
            }
            return IsLast_Foup_Exist;
        }
        public void StartConveyor(double velocity)
        {

            // 컨베이어를 지정된 속도로 조그 구동
            //double speed = 50.0; // 원하는 속도 값 (예시)

            if (runtDirection == LineDirection.Input)
            {
                JogPOS_Auto(velocity);
            }
            else
            {
                JogNEG_Auto(velocity);
            }

        }
        public void StopConveyor()
        {
            JogSTOP();
        }
        public abstract void Auto_Start_CV_Control();
        public void JogPOS(double speed)
        {
            double velocity = speed;
            w_motion.StartJogPos(axis, velocity);
        }
        public void JogNEG(double speed)
        {
            double velocity = speed;
            w_motion.StartJogNeg(axis, velocity);
        }
        public void TurnJogPos(double speed)
        {
            double velocity = speed;
            w_motion.StartJogPos(turnAxis, velocity);
        }
        public void TurnJogNeg(double speed)
        {
            double velocity = speed;
            w_motion.StartJogNeg(turnAxis, velocity);
        }
        public void JogPOS_Auto(double speed)
        {
            double velocity = speed;
            double acc = autoAcc;
            double dec = autoDec;
            w_motion.StartJogPos_Auto(axis, velocity, acc, dec);
        }
        public void JogNEG_Auto(double speed)
        {
            double velocity = speed;
            double acc = autoAcc;
            double dec = autoDec;
            w_motion.StartJogNeg_Auto(axis, velocity, acc, dec);
        }
        public void JogSTOP()
        {
            w_motion.StopJog(axis);
        }
        public void TurnJogPOS(double speed)
        {
            double velocity = speed;
            double acc = autoAcc;
            double dec = autoDec;
            w_motion.StartJogPos_Auto(turnAxis, velocity, acc, dec);
        }
        public void TurnJogNEG(double speed)
        {
            double velocity = speed;
            double acc = autoAcc;
            double dec = autoDec;
            w_motion.StartJogNeg_Auto(turnAxis, velocity, acc, dec);
        }
        public void TurnJogSTOP()
        {
            w_motion.StopJog(turnAxis);
        }
        public void Absolute_Turn_Move_Load()
        {
            w_motion.AbsoluteMove(w_motion.m_AxisProfile[turnAxis],loadPOS);
        }
        public void Absolute_Turn_Move_Unload()
        {
            w_motion.AbsoluteMove(w_motion.m_AxisProfile[turnAxis], unloadPOS);
        }
        /// <summary>
        /// Init 동작
        /// </summary>
        public virtual void Init() { }
        /// <summary>
        /// Turn Init 동작
        /// </summary>
        public virtual void Turn_Init() { }
        public virtual void AddrUpdate()
        {
            addr = 8 + (axis * 4);
        }
        public void AutoMode()
        {
            mode = Mode.Auto;
        }
        public void ManualMode()
        {
            mode = Mode.Manual;
        }

    }
}
