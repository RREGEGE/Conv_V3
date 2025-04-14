using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using WMX3ApiCLR;
using static Synustech.G_Var;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace Synustech
{
    public abstract partial class Conveyor
    {
        public static delMonitorLogView del_ELogSender_Conveyor;

        public enum CnvRun
        {
            Stop,
            Run
        }
        public enum TurnStatus
        {
            Load,
            Busy,
            Unload,
        }
        public enum LineDirection
        {
            Input,
            Output
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
        /// <summary>
        /// Conveyor의 제어 모드
        /// </summary>
        public enum ControlMode
        {
            MasterMode,
            CIMMode
        }
        /// <summary>
        /// Common
        /// </summary>
        // Conveyor Parameter 변수
        public int axis = -1;
        public int ID;
        public string type;
        public double autoVelocity;
        public double autoAcc;
        public double autoDec;
        public double slowVelocity;
        public double initVelocity;
        public bool isInterface = false;
        public bool isPIORun = false;
        public AlarmList m_ConveyorAlarmList;

        // Conveyor 상태 관련 변수
        public CnvRun run = CnvRun.Stop;
        public ServoOnOff servo = ServoOnOff.Off;
        public ServoOnOff previousServo = ServoOnOff.Off;
        public Mode mode = Mode.Manual;
        public Mode previousMode;
        public AlarmStatus alarmstatus = AlarmStatus.OK;

        // StepTimeOut 관련  StopWatch
        public Stopwatch _stepStopWatch = new Stopwatch();

        // Init 관련 변수
        public bool initComp = false;
        public InitStep preCV_InitStep;
        public InitStep CV_InitStep = InitStep.Step000_Foup_Check;

        // Auto 관련 변수
        public AutoStep prestep;
        public AutoStep CV_AutoStep = AutoStep.Step000_Check_Direction;
        public LineDirection runDirection = LineDirection.Input;

        // CST Load or Unload를 위한 양 옆 Conveyor 인스턴스
        public Conveyor beforeConv = null;
        public Conveyor nextConv = null;

        // CST 감지 관련 변수
        public bool isCSTEmpty = false;
        public bool isCSTInPosition;
        public bool AutoStopEnable;
        public bool AutoRunning = false;
        public bool CycleRunning = false;

        // Sensor 변수
        public SensorOnOff[] CSTDetect = new SensorOnOff[2];



        /// <summary>
        /// Turn
        /// </summary>
        // Init 관련 변수
        public bool turn_CV_InitComp = false;
        public TurnInitStep preTCV_InitStep;
        public TurnInitStep TCV_InitStep = TurnInitStep.Step000_POS_Check;
        public bool isInitDone = false;

        // T-Axis 변수
        public bool LoadDetect;
        public bool UnloadDetect;

        // POS Sensor 변수
        public SensorOnOff[] POS = new SensorOnOff[4];

        // Load/Unload Position 관련 변수
        public int loadLocation = 0;
        public int unloadLocation = 0;
        public double loadPOS;
        public double unloadPOS;
        
        // HomeDone 변수
        public bool IsHomeDone = false;



        /// <summary>
        /// Interface
        /// </summary>
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

        public PIOStep CV_InterfaceStep = new PIOStep();
        public PIOStep preInterfaceStep;
    }
}
