using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using WMX3ApiCLR;
using static Synustech.G_Var;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using static Synustech.Conveyor;
using System.Diagnostics;

namespace Synustech
{
    public abstract partial class Conveyor
    {
        public static delMonitorLogView del_ELogSender_Conveyor;
        public AutoStep prestep;

        public Stopwatch _stepStopWatch = new Stopwatch();

        public bool normalCnvCSTDetected = false;
        public bool normalCnVCSTBoth;
        public bool longCnvCSTDetected;
        public bool longCnVCSTBoth;

        /// <summary>
        /// Turn Sensor 변수
        /// </summary>
        public bool turnCnvLoadDetect;
        public bool turnCnvUnloadDetect;

        public bool normal_CV_AutoRunning = false;
        public bool normal_CV_CycleRunning = false;
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
        /// 일반 컨베이어 동작에 대한 정의
        /// </summary>
        public enum AutoStep
        {
            Step000_Check_Direction = 0,

            Step100_InMode_Check_CST_Load_Status = 100,

            Step110_InMode_Turn_CST_Load_Status = 110,

            // Axis Step 정의
            Step200_InMode_Start_CST_Pass_CV_to_CV = 200,
            Step210_InMode_Move_CV_Rolling_In = 210,
            Step215_InMode_Move_CV_Slow = 215,
            Step220_InMode_Run_Condition_Check = 220,
            Step230_InMode_Move_CV_Stop = 230,
            Step240_InMode_Check_CST_ID = 240,
            Step250_InMode_Move_CV_Rolling_Out = 250,
            Step255_InMode_Wait_NextConv = 255,
            Step260_InMode_Unload_Turn_Stop = 260,
            Step270_InMode_Move_CST_Pass_CV_to_TCV = 270,
            Step275_InMode_Change_210 = 275,
            Step280_InMode_Alarm_Condition_280 = 280,
            Step290_InMode_Alarm_Wait_290 = 290,
            Step300_OutMode_Check_CST_Load_Status = 300,


            Step400_OutMode_Start_CST_Pass_NCV_to_NCV = 400,
            Step410_OutMode_Move_CV_Rolling_out = 410,
            Step415_OutMode_Move_CV_Slow = 415,
            Step420_OutMode_Run_Condition_Check = 420,
            Step430_OutMode_Move_CV_Stop = 430,
            Step440_OutMode_Check_CST_ID = 440,
            Step450_OutMode_Move_CV_Rolling_Out = 450,
            Step455_OutMode_Wait_NextConv = 455,
            Step460_OutMode_Unload_Turn_Stop = 460,
            Step470_OutMode_Move_CST_Pass_CV_to_TCV = 470,
            Step475_OutMode_Change_410 = 475,
            Step480_OutMode_Alarm_Condition_480 = 480,
            Step490_OutMode_Alarm_Wait_490 = 490,

            Step500_InMode_Get_CST_Rolling = 500,
            Step600_OutMode_Get_CST_Rolling = 600,

            // Turn Axis Step 정의
            Step710_InMode_Move_T_0_Deg = 710, // Loading Pos 외 다른 Pos Sensor Check시 int();
            Step720_InMode_Move_T_Stop = 720,
            Step730_InMode_Unload_Move = 730,
            Step740_InMode_Unload_Stop = 740,
            Step750_InMode_Return_T_Move = 750,
            Step760_InMode_LoadReady_T = 760,

            Step810_OutMode_Move_T_Unload = 810,
            Step820_OutMode_Move_T_Stop = 820,
            Step830_OutMode_Load_Move = 830,
            Step840_OutMode_Load_Stop = 840,
            Step850_OutMode_Return_T_Move = 850,
            Step860_OutMode_UnloadReady_T = 860,

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

        public double unloadPOS;
        public enum LineDirection
        {
            Input,
            Output
        }

        public AutoStep CV_AutoStep = AutoStep.Step000_Check_Direction;
        public InitStep CV_InitStep = InitStep.Step000_Foup_Check;
        public TurnInitStep TCV_InitStep = TurnInitStep.Step000_POS_Check;

        public LineDirection portDirection = LineDirection.Input;

        public List<Line> lines;

        public int addr;
        public List<int> bits;

        public int addrTurn;
        public List<int> bitsTurn;
        public int bitTurn;

        public bool IsHomeDone = false;

        public CnvRun run = CnvRun.Stop;
        public ServoOnOff servo = ServoOnOff.Off;
        public ServoOnOff preServo = ServoOnOff.Off;
        public Mode mode = Mode.Manual;
        public Mode preMode;
        public SensorOnOff[] normalCnvFoupDetect = new SensorOnOff[2];
        public SensorOnOff[] longCnvFoupDetect = new SensorOnOff[3];
        public SensorOnOff[] POS = new SensorOnOff[4];
        public InOutMode inoutMode = InOutMode.InMode;
        public TurnStatus turnStatus = TurnStatus.Load;
        public PIOType InterfaceType = PIOType.None;
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
        public bool PIORun = false;
        public AlarmList m_ConveyorAlarmList;
        public Conveyor(int id, int axis, bool Isinterface)
        {
            ID = id;
            this.axis = axis;
            this.lines = new List<Line>();
            bits = new List<int>();
            bitsTurn = new List<int>();
            m_ConveyorAlarmList = new AlarmList(ID);
            isInterface = Isinterface;
            if(isInterface)
            {
                PIORun = true;
            }
            Console.WriteLine(this.ID + ":" + isInterface);
        }

        /// <summary>
        /// PIO 초기화
        /// </summary>
        public void Initialize_PIO_Status()
        {
            WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Load_Req, 0);
            WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_ES, 0);
            WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Unload_Req, 0);
            WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Ready, 0);
            WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_HOAVBL, 0);

            WMX3.m_wmx3io.SetOutBit(addrAGVPIO, (int)AGVPIO_Passive.AGV_PIO_Load_Req, 0);
            WMX3.m_wmx3io.SetOutBit(addrAGVPIO, (int)AGVPIO_Passive.AGV_PIO_Ready, 0);
            WMX3.m_wmx3io.SetOutBit(addrAGVPIO, (int)AGVPIO_Passive.AGV_PIO_Unload_Req, 0);
        }
        /// <summary>
        /// PIO Check 함수
        /// </summary>
        public bool Is_EquipType_To_Conveyor_PIO_CS_0Flag()
        {
            if (InterfaceType == PIOType.OHT)
                return CS_0 == SensorOnOff.On ? true : false;
            //else if(InterfaceType == PIOType.AGV)
            //    return CS_0 == SensorOnOff.Off ? false : false;
            return true;
        }
        public bool Is_EquipType_To_Conveyor_PIO_ValidFlag()
        {
            if (InterfaceType == PIOType.OHT)
                return VALID == SensorOnOff.On ? true : false;
            //else if(InterfaceType == PIOType.AGV)
            //    return VALID == SensorOnOff.Off ? false : false;
            return true;
        }
        public bool Is_EquipType_To_Conveyor_PIO_TR_REQFlag()
        {
            if (InterfaceType == PIOType.OHT)
            {
                return TR_REQ == SensorOnOff.On ? true : false;
            }

            //else if(InterfaceType == PIOType.AGV)
            //    return TR_REQ == SensorOnOff.Off ? false : false;
            return true;
        }
        public bool Is_EquipType_To_Conveyor_PIO_BUSYFlag()
        {
            if (InterfaceType == PIOType.OHT)
                return BUSY == SensorOnOff.On ? true : false;
            //else if(InterfaceType == PIOType.AGV)
            //    return BUSY == SensorOnOff.Off ? false : false;
            return true;
        }
        public bool Is_EquipType_To_Conveyor_PIO_COMPTFlag()
        {
            if (InterfaceType == PIOType.OHT)
                return COMPT == SensorOnOff.On ? true : false;
            //else if(InterfaceType == PIOType.AGV)
            //    return COMPT == SensorOnOff.Off ? false : false;
            return true;
        }
        // 확실히 돌고 멈추는지 확인 
        public bool Is_EquipType_To_Conveyor_PIO_L_REQFlag()
        {
            if (InterfaceType == PIOType.OHT && this.type == "Turn")
            {
                turnStatus = TurnStatus.Busy;
                CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
                Load_POS_Check();
                if (turnCnvLoadDetect && loadLocation == cmAxis.ActualPos)
                {
                    if (CV_InterfaceStep == PIOStep.Step220_InMode_Check_PIO_Valid)
                        WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Load_Req, 1);


                    return L_REQ == SensorOnOff.On ? true : false;
                }
            }
            else if (InterfaceType == PIOType.OHT)
            {
                if (CV_InterfaceStep == PIOStep.Step220_InMode_Check_PIO_Valid)
                    WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Load_Req, 1);
                return L_REQ == SensorOnOff.On ? true : false;
            }

            //else if(InterfaceType == PIOType.AGV)
            //    return COMPT == SensorOnOff.Off ? false : false;
            return true;
        }
        public bool Is_EquipType_To_Conveyor_PIO_UL_REQFlag()
        {
            if (InterfaceType == PIOType.OHT && this.type == "Turn")
            {
                turnStatus = TurnStatus.Busy;
                CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
                Unload_POS_Check();
                if (turnCnvUnloadDetect && unloadLocation == cmAxis.ActualPos)
                {
                    if (CV_InterfaceStep == PIOStep.Step320_OutMode_Check_PIO_Valid)
                        WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Unload_Req, 1);

                    return UL_REQ == SensorOnOff.On ? true : false;
                }
            }
            else if (InterfaceType == PIOType.OHT)
            {
                if (CV_InterfaceStep == PIOStep.Step320_OutMode_Check_PIO_Valid)
                    WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Unload_Req, 1);
                return UL_REQ == SensorOnOff.On ? true : false;
            }
            //else if(InterfaceType == PIOType.AGV)
            //return COMPT == SensorOnOff.Off ? false : false;
            return true;
        }
        public void ProcessPIOStep()
        {
            switch (CV_InterfaceStep)
            {
                case PIOStep.Step000_Direction_Check:
                    {
                        CST_Empty_Detect();
                        if (beforeConv == null && GetOperationDirection() == LineDirection.Input && !normalCnvCSTDetected)
                        {
                            CV_InterfaceStep = PIOStep.Step200_Inmode_Check_Foup;
                        }
                        else if (nextConv == null && GetOperationDirection() == LineDirection.Output && normalCnvCSTDetected)
                        {
                            CV_InterfaceStep = PIOStep.Step300_Outmode_Check_Foup;
                        }
                    }
                    break;
                case PIOStep.Step200_Inmode_Check_Foup:
                    {
                        CST_Empty_Detect();
                        if (!normalCnvCSTDetected)
                        {
                            if (type == "Turn")
                            {
                                Load_POS_Check();
                            }
                            CV_InterfaceStep = PIOStep.Step210_InMode_Await_PIO_CS;
                        }
                        else
                        {
                            CV_InterfaceStep = PIOStep.Step000_Direction_Check;
                            PIORun = false;
                        }
                    }
                    break;
                case PIOStep.Step210_InMode_Await_PIO_CS:
                    {
                        if (Is_EquipType_To_Conveyor_PIO_CS_0Flag())
                        {
                            CV_InterfaceStep = PIOStep.Step220_InMode_Check_PIO_Valid;
                        }
                    }
                    break;
                case PIOStep.Step220_InMode_Check_PIO_Valid:
                    {
                        if (Is_EquipType_To_Conveyor_PIO_ValidFlag() &&
                        (InterfaceType == PIOType.OHT ? Is_EquipType_To_Conveyor_PIO_CS_0Flag() : true))
                        {
                            Is_EquipType_To_Conveyor_PIO_L_REQFlag();
                            CV_InterfaceStep = PIOStep.Step230_InMode_Check_PIO_TR;
                        }
                    }
                    break;
                case PIOStep.Step230_InMode_Check_PIO_TR:
                    {
                        if (Is_EquipType_To_Conveyor_PIO_TR_REQFlag())
                        {
                            WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Ready, 1);
                            CV_InterfaceStep = PIOStep.Step240_InMode_Check_PIO_Busy;
                        }
                    }
                    break;
                case PIOStep.Step240_InMode_Check_PIO_Busy:
                    {
                        if (Is_EquipType_To_Conveyor_PIO_BUSYFlag())
                        {
                            CV_InterfaceStep = PIOStep.Step250_InMode_Check_PIO_Complete;
                        }
                    }
                    break;
                case PIOStep.Step250_InMode_Check_PIO_Complete:
                    {
                        CST_Empty_Detect();
                        // 자재가 있는 경우
                        if (normalCnvCSTDetected)
                        {
                            WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Load_Req, 0);
                            if (!Is_EquipType_To_Conveyor_PIO_TR_REQFlag() &&
                            !Is_EquipType_To_Conveyor_PIO_BUSYFlag() &&
                            Is_EquipType_To_Conveyor_PIO_COMPTFlag() &&
                            !Is_EquipType_To_Conveyor_PIO_L_REQFlag())
                            {
                                WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Ready, 0);
                                CV_InterfaceStep = PIOStep.Step260_InMode_Check_PIO_End;
                            }
                            if (InterfaceType == PIOType.None)
                                CV_InterfaceStep = PIOStep.Step260_InMode_Check_PIO_End;
                        }
                    }
                    break;
                case PIOStep.Step260_InMode_Check_PIO_End:
                    {
                        if (!Is_EquipType_To_Conveyor_PIO_ValidFlag() &&
                            !Is_EquipType_To_Conveyor_PIO_CS_0Flag() &&
                            !Is_EquipType_To_Conveyor_PIO_COMPTFlag()
                            )
                        {
                            CV_InterfaceStep = PIOStep.Step270_InMode_Check_CST_Load_And_Safe;
                        }
                    }
                    break;
                case PIOStep.Step270_InMode_Check_CST_Load_And_Safe: //Hoist Sensor등 Safety 확인
                    {
                        CST_Empty_Detect();
                        if (normalCnvCSTDetected)
                        {
                            PIORun = false;
                            CV_InterfaceStep = PIOStep.Step000_Direction_Check;
                        }
                    }
                    break;


                case PIOStep.Step300_Outmode_Check_Foup:
                    {
                        CST_Empty_Detect();
                        if (normalCnvCSTDetected)
                        {
                            CV_InterfaceStep = PIOStep.Step310_OutMode_Await_PIO_CS;
                        }
                    }
                    break;
                case PIOStep.Step310_OutMode_Await_PIO_CS:
                    {
                        CST_Empty_Detect();
                        if (Is_EquipType_To_Conveyor_PIO_CS_0Flag() && normalCnvCSTDetected)
                        {
                            CV_InterfaceStep = PIOStep.Step320_OutMode_Check_PIO_Valid;
                        }
                    }
                    break;
                case PIOStep.Step320_OutMode_Check_PIO_Valid:
                    {
                        CST_Empty_Detect();
                        if (Is_EquipType_To_Conveyor_PIO_ValidFlag() && normalCnvCSTDetected)
                        {
                            Is_EquipType_To_Conveyor_PIO_UL_REQFlag();
                            CV_InterfaceStep = PIOStep.Step330_OutMode_Check_PIO_TR;
                        }
                        else if (!normalCnvCSTDetected)
                        {
                            CV_InterfaceStep = PIOStep.Step360_OutMode_Check_PIO_End;
                        }
                    }
                    break;
                case PIOStep.Step330_OutMode_Check_PIO_TR:
                    {
                        CST_Empty_Detect();
                        if (Is_EquipType_To_Conveyor_PIO_TR_REQFlag() && normalCnvCSTDetected)
                        {
                            WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Ready, 1);
                            CV_InterfaceStep = PIOStep.Step340_OutMode_Check_PIO_Busy;
                        }
                        else if (!normalCnvCSTDetected)
                        {
                            CV_InterfaceStep = PIOStep.Step360_OutMode_Check_PIO_End;
                        }
                    }
                    break;
                case PIOStep.Step340_OutMode_Check_PIO_Busy:
                    {
                        CST_Empty_Detect();
                        if (Is_EquipType_To_Conveyor_PIO_BUSYFlag() && normalCnvCSTDetected)
                        {
                            CV_InterfaceStep = PIOStep.Step350_OutMode_Check_PIO_Complete;
                        }
                        else if (!normalCnvCSTDetected)
                        {
                            CV_InterfaceStep = PIOStep.Step360_OutMode_Check_PIO_End;
                        }
                    }
                    break;

                case PIOStep.Step350_OutMode_Check_PIO_Complete:
                    {
                        CST_Empty_Detect();
                        // 자재가 있는 경우
                        if (!normalCnvCSTDetected)
                        {
                            WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Unload_Req, 0);
                            if (!Is_EquipType_To_Conveyor_PIO_TR_REQFlag() &&
                                !Is_EquipType_To_Conveyor_PIO_BUSYFlag() &&
                                Is_EquipType_To_Conveyor_PIO_COMPTFlag() &&
                                !Is_EquipType_To_Conveyor_PIO_UL_REQFlag())
                            {
                                WMX3.m_wmx3io.SetOutBit(addrOHTPIO, (int)OHTPIO_Passive.OHT_PIO_Ready, 0);
                                CV_InterfaceStep = PIOStep.Step360_OutMode_Check_PIO_End;
                            }
                        }
                    }
                    break;

                case PIOStep.Step360_OutMode_Check_PIO_End:
                    {
                        if (!Is_EquipType_To_Conveyor_PIO_ValidFlag() &&
                            !Is_EquipType_To_Conveyor_PIO_CS_0Flag() &&
                            !Is_EquipType_To_Conveyor_PIO_COMPTFlag())
                        {
                            CV_InterfaceStep = PIOStep.Step370_OutMode_Check_LP_CST_Unload_And_Safe;
                        }
                    }
                    break;
                case PIOStep.Step370_OutMode_Check_LP_CST_Unload_And_Safe: //Hoist Sensor등 Safety 확인
                    {
                        CST_Empty_Detect();
                        if (normalCnvCSTDetected)
                        {
                            PIORun = false;
                            CV_InterfaceStep = PIOStep.Step000_Direction_Check;
                        }
                    }
                    break;

                default:
                    break;


            }
        }
        public void interfaceTypeCheck()
        {
            if(isInterface && isCycleRun)
            {
                PIORun = false;
            }
            else if(isInterface && isAutoRun) 
            {
                PIORun = true;           
            }
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
                Console.WriteLine(ID + "BeforeConv : " + beforeConv);
            }
            if (nNextConveyor != null && nNextConveyor.ID > 0)
            {
                nextConv = nNextConveyor;
                Console.WriteLine(ID + "NextConv : " + nextConv);
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
            if(portDirection == LineDirection.Input)
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
            for (int i = 0; i < normalCnvFoupDetect.Length; i++)
            {
                if (normalCnvFoupDetect[i] == SensorOnOff.Off) // SensorOnOff 열거형을 사용하여 센서 상태 확인
                {
                    count++;
                }

            }
            if (count > 0)
            {
                normalCnvCSTDetected = true;
            }
            else
            {
                normalCnvCSTDetected = false;
            }
        }
        public void LCnV_CST_Empty_Detect()
        {
            int count = 0;
            // LFoupDetect 배열을 확인하여 자재 유무 체크
            for (int i = 0; i < longCnvFoupDetect.Length; i++)
            {
                if (longCnvFoupDetect[i] == SensorOnOff.Off) // SensorOnOff 열거형을 사용하여 센서 상태 확인
                {
                    count++;
                }

            }
            if (count > 0)
            {
                longCnvCSTDetected = true;
            }
            else
            {
                longCnvCSTDetected = false;
            }
        }
        /// <summary>
        /// Foup_Detect#1,#2 모두 On 상태인지 검사
        /// </summary>
        public void CST_Check_Detect()
        {
            int count = 0;
            // FoupDetect 배열을 확인하여 자재 유무 체크
            for (int i = 0; i < normalCnvFoupDetect.Length; i++)
            {
                if (normalCnvFoupDetect[i] == SensorOnOff.On) // SensorOnOff 열거형을 사용하여 센서 상태 확인
                {
                    count++;
                }
            }
            if (count > 0)
            {
                normalCnVCSTBoth = false;
            }
            else
            {
                normalCnVCSTBoth = true;
            }
        }
        public void LCV_Inmode_CST_Check_Detect()
        {
            int count = 0;
            for (int i = 1; i < longCnvFoupDetect.Length; i++)
            {
                if (longCnvFoupDetect[i] == SensorOnOff.On)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                longCnVCSTBoth = false;
            }
            else
            {
                longCnVCSTBoth = true;
            }
        }
        public void LCV_Outmode_CST_Check_Detect()
        {
            int count = 0;
            for(int i = 1; i >= 0; i--)
            {
                if (longCnvFoupDetect[i] == SensorOnOff.On)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                longCnVCSTBoth = false;
            }
            else
            {
                longCnVCSTBoth = true;
            }
        }
        public void Load_POS_Check()
        {
            CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
            if (POS[loadLocation] == SensorOnOff.On && cmAxis.ActualPos == loadPOS)
            {
                turnCnvLoadDetect = true;
            }
            else
            {
                turnCnvLoadDetect = false;
            }
        }
        public void Unload_POS_Check()
        {
            CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[turnAxis];
            if (POS[unloadLocation] == SensorOnOff.On && cmAxis.ActualPos == unloadPOS)
            {
                turnCnvUnloadDetect = true;
            }
            else
            {
                turnCnvUnloadDetect = false;
            }
        }
        /// <summary>
        /// 자재가 떠나서 Foup_Detect#2가 꺼졌는지 검사
        /// </summary>
        public bool CV_Inmode_LastFoup_Detect_Check()
        {
            bool isLast_Foup_Exist;
            if (normalCnvFoupDetect[normalCnvFoupDetect.Length - 1] == SensorOnOff.Off)
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
            if (normalCnvFoupDetect[0] == SensorOnOff.Off)
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

            if (portDirection == LineDirection.Input)
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
