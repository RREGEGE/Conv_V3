using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Master.Interface.Alarm;

namespace Master.Equipment.Port
{
    /// <summary>
    /// Port_1BP.cs는 Shuttle이 제어되는 공정에서 LP Buffer가 없는 형태의 제어 공정입니다.
    /// </summary>
    partial class Port
    {
        public enum OP_1BP_AutoStep
        {
            Step000_Check_Direction = 0,

            /// <summary>
            /// OP Input Mode
            /// 1. 자재가 없는 경우 BP에 Load 요청
            /// 2. 자재가 있는 경우 Await 상태 진입 (RM PIO 대기)
            /// </summary>
            Step100_InMode_Check_OP_CST_Load                = 100,      //Carrier 적재 상황 확인

            Step200_InMode_Call_BP_Load_Req                 = 200,      //Carrier Load 요청(셔틀)
            Step210_InMode_Check_BP_Location                = 210,

            Step300_InMode_Await_PIO_TR_REQ                 = 300,      //Carrier Unload 대기 (RM)
            Step310_InMode_Check_PIO_Busy                   = 310,
            Step320_InMode_Check_PIO_Complete               = 320,      //ID 전송 Port Word Map 및 아크 확인
            Step330_InMode_Check_PIO_End                    = 330,
            Step340_InMode_Check_OP_CST_Unload_And_Safe     = 340,      //Carrier ID Clear

            /// <summary>
            /// OP Output Mode
            /// 1. 자재가 없는 경우 Await 상태 진입 (RM TR_REQ 대기)
            /// 2. 자재가 있는 경우 셔틀에 Unload 요청
            /// </summary>
            Step600_OutMode_Check_OP_CST_Load               = 600,      //Carrier 적재 상황 확인

            Step700_OutMode_Await_PIO_TR_REQ                = 700,      //Carrier Load 요청(RM)
            Step710_OutMode_Check_PIO_Busy                  = 710,
            Step720_OutMode_Check_PIO_Complete              = 720,      //ID 복사 RM Word Map -> Port Word Map
            Step730_OutMode_Check_PIO_End                   = 730,      //ID 적용 Port WordMap -> OP Carrier ID
            Step740_OutMode_Check_OP_CST_Load_And_Safe      = 740,

            Step800_OutMode_Call_BP_Unload_Req              = 800       //Carrier Unload 대기 (셔틀)
        }
        public enum Shuttle_1BP_AutoStep
        {
            Step000_Check_Direction = 0,
            Step001_Change_Direction = 1,

            /// <summary>
            /// BP Input Mode
            /// 1. 100번대 영역 : BP 자세 확인 (시작시 셔틀에 CST 있는 경우는 무조건 에러)
            /// 2. 200번대 영역 : LP에서 제품 Unload 요구 오는 경우 제품 Loading Run
            /// 3. 300번대 영역 : OP에서 제품 Load 요구 오는 경우 제품 Unload Run
            /// </summary>
            Step100_InMode_Check_BP_CST_Load_And_Pose   = 100,      //CST 로드 상태와 자세 확인
            Step110_InMode_Move_Z_Down                  = 110,      //초기 자세 이니셜
            Step120_InMode_Move_X_LP                    = 120,      //초기 자세 이니셜
            Step130_InMode_Move_X_Wait                  = 130,
            Step140_InMode_Move_Z_Up                    = 140,      //없는 경우 초기 자세 이니셜
            Step150_InMode_Move_T_180_Deg               = 150,      //없는 경우 초기 자세 이니셜
            Step160_InMode_Move_T_0_Deg                 = 160,      //있는 경우 초기 자세 이니셜
            Step170_InMode_Check_OperationType          = 170,

            Step200_InMode_Await_PIO_CS                 = 200,      //Carrier Load 요청 (AGV or OHT)
            Step210_InMode_Check_PIO_Valid              = 210,
            //Step211_InMode_Call_AGV_Load_Req            = 211,
            Step220_InMode_Check_PIO_TR                 = 220,
            Step230_InMode_Check_PIO_Busy               = 230,
            Step240_InMode_Check_PIO_Complete           = 240,
            Step250_InMode_Check_PIO_End                = 250,
            Step260_InMode_Check_CST_Load_And_Safe      = 260,

            Step300_InMode_Await_MGV_CST_Load           = 300,      //Carrier Load 요청 (MGV)

            Step400_InMode_Ready_CST_ID_Read            = 400,      //Carrier ID Read Step
            Step410_InMode_Start_CST_ID_Read            = 410,
            Step420_InMode_Move_Z_UP                    = 420,
            Step430_InMode_Move_X_Wait                  = 430,
            Step440_InMode_Move_T_0_Deg                 = 440,

            Step500_InMode_Await_OP_Load_Req            = 500,      //OP load 요청 체크(대기)
            Step510_InMode_Move_X_OP                    = 510,      //제품 로드 위치로 이동 및 BP 제품 언로드
            Step520_InMode_Move_Z_Down                  = 520,
            Step530_InMode_Send_OP_CST_ID               = 530,      //ID 이전
            Step540_InMode_Move_X_LP_or_Wait            = 540,
            Step550_InMode_Move_T_180_Deg               = 550,
            Step560_InMode_Move_X_LP                    = 560,


            /// <summary>
            /// BP Output Mode
            /// 1. 600번대 영역 : BP 자세 확인 (시작시 셔틀에 CST 있는 경우는 무조건 에러)
            /// 2. 700번대 영역 : OP에서 제품 Unload 요구 오는 경우 제품 Loading Run
            /// 3. 800번대 영역 : LP에서 제품 Load 요구 오는 경우 제품 Unload Run
            /// </summary>
            Step600_OutMode_Check_BP_CST_Load_And_Pose  = 600,      //CST 로드 상태와 자세 확인
            Step610_OutMode_Move_Z_Down                 = 610,      //초기 자세 이니셜
            Step620_OutMode_Move_X_LP                   = 620,      //초기 자세 이니셜
            Step630_OutMode_Move_X_Wait                 = 630,      //초기 자세 이니셜
            Step640_OutMode_Move_Z_Up                   = 640,      //제품 있는 경우 이니셜
            Step650_OutMode_Move_T_180_Deg              = 650,      //제품 있는 경우 이니셜
            Step660_OutMode_Move_T_0_Deg                = 660,      //제품 없는 경우 이니셜 자세

            Step700_OutMode_Await_OP_Unload_Req         = 700,      //제품 없는 경우 OP Unload 요청 체크(대기)
            Step710_OutMode_Move_X_OP                   = 710,      //제품 언로드 위치로 이동 및 BP 제품 로드
            Step720_OutMode_Move_Z_Up                   = 720,
            Step730_OutMode_Read_OP_CST_ID              = 730,      //ID 이전
            Step740_OutMode_Move_X_LP_or_Wait           = 740,      //대기 위치로 이동
            Step750_OutMode_Move_T_180_Deg              = 750,
            Step760_OutMode_Move_X_LP                   = 760,      //대기 위치로 이동
            Step770_OutMode_Move_Z_Down                 = 770,
            Step780_OutMode_Check_OperationType         = 780,

            Step800_OutMode_Await_PIO_CS                = 800,      //Carrier Unload 요청 (AGV or OHT)
            Step810_OutMode_Check_PIO_Valid             = 810,
            //Step811_OutMode_Call_AGV_Unload_Req         = 811,
            Step820_OutMode_Check_PIO_TR                = 820,
            Step830_OutMode_Check_PIO_Busy              = 830,
            Step840_OutMode_Check_PIO_Complete          = 840,
            Step850_OutMode_Check_PIO_End               = 850,
            Step860_OutMode_Check_CST_Unload_And_Safe   = 860,

            Step900_OutMode_Await_MGV_CST_Unload        = 900,      //Carrier Unload 요청 (MGV)
        }

        /// <summary>
        /// LP 1BP Type In/Out Control
        /// </summary>
        OP_1BP_AutoStep m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step000_Check_Direction;
        Shuttle_1BP_AutoStep m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step000_Check_Direction;

        OP_1BP_AutoStep Pre_OP_1BP_AutoStep = OP_1BP_AutoStep.Step000_Check_Direction;
        Shuttle_1BP_AutoStep Pre_Shuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step000_Check_Direction;

        bool m_bOP_1BP_AutoStopEnable = false;
        bool m_bShuttle_1BP_AutoStopEnable = false;

        //Auto
        private bool m_bOP_1BP_AutoRunning = false;
        private bool m_bShuttle_1BP_AutoRunning = false;

        /// <summary>
        /// OP or Shuttle의 정지 가능 상태를 나타냄
        /// Call, Await Step에서 대부분 정지 가능(요청 또는 대기 상태)
        /// </summary>
        private void Auto_1BP_OPStopEnableUpdate()
        {
            switch (m_eOP_1BP_AutoStep)
            {
                case OP_1BP_AutoStep.Step000_Check_Direction:
                //case OP_1BP_AutoStep.Step100_InMode_Check_OP_CST_Load:
                case OP_1BP_AutoStep.Step200_InMode_Call_BP_Load_Req:
                case OP_1BP_AutoStep.Step300_InMode_Await_PIO_TR_REQ:
                //case OP_1BP_AutoStep.Step600_OutMode_Check_OP_CST_Load:
                case OP_1BP_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                case OP_1BP_AutoStep.Step800_OutMode_Call_BP_Unload_Req:
                    m_bOP_1BP_AutoStopEnable = true;
                    break;
                default:
                    m_bOP_1BP_AutoStopEnable = false;
                    break;
            }
        }
        private void Auto_1BP_BPStopEnableUpdate()
        {
            switch (m_eShuttle_1BP_AutoStep)
            {
                case Shuttle_1BP_AutoStep.Step000_Check_Direction:
                case Shuttle_1BP_AutoStep.Step001_Change_Direction:
                case Shuttle_1BP_AutoStep.Step100_InMode_Check_BP_CST_Load_And_Pose:
                case Shuttle_1BP_AutoStep.Step200_InMode_Await_PIO_CS:
                case Shuttle_1BP_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                case Shuttle_1BP_AutoStep.Step400_InMode_Ready_CST_ID_Read:
                case Shuttle_1BP_AutoStep.Step500_InMode_Await_OP_Load_Req:
                case Shuttle_1BP_AutoStep.Step600_OutMode_Check_BP_CST_Load_And_Pose:
                case Shuttle_1BP_AutoStep.Step700_OutMode_Await_OP_Unload_Req:
                case Shuttle_1BP_AutoStep.Step800_OutMode_Await_PIO_CS:
                case Shuttle_1BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload:
                    m_bShuttle_1BP_AutoStopEnable = true;
                    break;
                case Shuttle_1BP_AutoStep.Step210_InMode_Check_PIO_Valid:
                case Shuttle_1BP_AutoStep.Step810_OutMode_Check_PIO_Valid:
                    if (AGVFullFlagOption)
                        m_bShuttle_1BP_AutoStopEnable = false;
                    else
                        m_bShuttle_1BP_AutoStopEnable = true;
                    break;
                case Shuttle_1BP_AutoStep.Step220_InMode_Check_PIO_TR:
                case Shuttle_1BP_AutoStep.Step230_InMode_Check_PIO_Busy:
                case Shuttle_1BP_AutoStep.Step240_InMode_Check_PIO_Complete:
                case Shuttle_1BP_AutoStep.Step250_InMode_Check_PIO_End:
                case Shuttle_1BP_AutoStep.Step820_OutMode_Check_PIO_TR:
                case Shuttle_1BP_AutoStep.Step830_OutMode_Check_PIO_Busy:
                case Shuttle_1BP_AutoStep.Step840_OutMode_Check_PIO_Complete:
                case Shuttle_1BP_AutoStep.Step850_OutMode_Check_PIO_End:
                    m_bShuttle_1BP_AutoStopEnable = false;
                    break;
                default:
                    {
                        m_bShuttle_1BP_AutoStopEnable = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// 자재가 있어야 할 스텝, 없어야 할 스텝을 구분하여 자재 판단
        /// </summary>
        private void Auto_1BP_OP_Placement_Detect_AlarmCheck()
        {
            switch (m_eOP_1BP_AutoStep)
            {
                //있어야 함
                case OP_1BP_AutoStep.Step210_InMode_Check_BP_Location:
                case OP_1BP_AutoStep.Step300_InMode_Await_PIO_TR_REQ:
                case OP_1BP_AutoStep.Step310_InMode_Check_PIO_Busy:
                case OP_1BP_AutoStep.Step730_OutMode_Check_PIO_End:
                case OP_1BP_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe:
                    {
                        if (Carrier_CheckOP_ExistProduct(false, false))
                        {
                            Watchdog_Start(WatchdogList.OP_Placement_ErrorTimer);
                            if (Watchdog_IsDetect(WatchdogList.OP_Placement_ErrorTimer))
                                AlarmInsert((short)PortAlarm.OP_Placement_Detect_Error, AlarmLevel.Error);
                        }
                        else
                            Watchdog_Stop(WatchdogList.OP_Placement_ErrorTimer, true);
                    }
                    break;
                //없어야 함
                case OP_1BP_AutoStep.Step330_InMode_Check_PIO_End:
                case OP_1BP_AutoStep.Step340_InMode_Check_OP_CST_Unload_And_Safe:
                case OP_1BP_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                case OP_1BP_AutoStep.Step710_OutMode_Check_PIO_Busy:
                    {
                        if (Carrier_CheckOP_ExistProduct(true, false))
                        {
                            Watchdog_Start(WatchdogList.OP_Placement_ErrorTimer);
                            if (Watchdog_IsDetect(WatchdogList.OP_Placement_ErrorTimer))
                                AlarmInsert((short)PortAlarm.OP_Placement_Detect_Error, AlarmLevel.Error);
                        }
                        else
                            Watchdog_Stop(WatchdogList.OP_Placement_ErrorTimer, true);
                    }
                    break;
                default:
                    Watchdog_Stop(WatchdogList.OP_Placement_ErrorTimer, true);
                    break;
            }
        }
        private void Auto_1BP_BP_Placement_Detect_AlarmCheck()
        {
            switch (m_eShuttle_1BP_AutoStep)
            {
                //없어야 함
                case Shuttle_1BP_AutoStep.Step170_InMode_Check_OperationType:
                case Shuttle_1BP_AutoStep.Step200_InMode_Await_PIO_CS:
                case Shuttle_1BP_AutoStep.Step210_InMode_Check_PIO_Valid:
                //case Shuttle_1BP_AutoStep.Step211_InMode_Call_AGV_Load_Req:
                case Shuttle_1BP_AutoStep.Step220_InMode_Check_PIO_TR:
                case Shuttle_1BP_AutoStep.Step230_InMode_Check_PIO_Busy:
                case Shuttle_1BP_AutoStep.Step530_InMode_Send_OP_CST_ID:
                case Shuttle_1BP_AutoStep.Step540_InMode_Move_X_LP_or_Wait:
                case Shuttle_1BP_AutoStep.Step550_InMode_Move_T_180_Deg:
                case Shuttle_1BP_AutoStep.Step560_InMode_Move_X_LP:
                case Shuttle_1BP_AutoStep.Step700_OutMode_Await_OP_Unload_Req:
                case Shuttle_1BP_AutoStep.Step710_OutMode_Move_X_OP:
                case Shuttle_1BP_AutoStep.Step850_OutMode_Check_PIO_End:
                case Shuttle_1BP_AutoStep.Step860_OutMode_Check_CST_Unload_And_Safe:
                    {
                        if (Carrier_CheckShuttle_ExistProduct(true))
                        {
                            Watchdog_Start(WatchdogList.BP_Placement_ErrorTimer);
                            if (Watchdog_IsDetect(WatchdogList.BP_Placement_ErrorTimer))
                                AlarmInsert((short)PortAlarm.Shuttle_Placement_Detect_Error, AlarmLevel.Error);
                        }
                        else
                            Watchdog_Stop(WatchdogList.BP_Placement_ErrorTimer, true);
                    }
                    break;
                //있어야 함
                case Shuttle_1BP_AutoStep.Step250_InMode_Check_PIO_End:
                case Shuttle_1BP_AutoStep.Step260_InMode_Check_CST_Load_And_Safe:
                case Shuttle_1BP_AutoStep.Step400_InMode_Ready_CST_ID_Read:
                case Shuttle_1BP_AutoStep.Step410_InMode_Start_CST_ID_Read:
                case Shuttle_1BP_AutoStep.Step420_InMode_Move_Z_UP:
                case Shuttle_1BP_AutoStep.Step430_InMode_Move_X_Wait:
                case Shuttle_1BP_AutoStep.Step440_InMode_Move_T_0_Deg:
                case Shuttle_1BP_AutoStep.Step500_InMode_Await_OP_Load_Req:
                case Shuttle_1BP_AutoStep.Step510_InMode_Move_X_OP:
                case Shuttle_1BP_AutoStep.Step730_OutMode_Read_OP_CST_ID:
                case Shuttle_1BP_AutoStep.Step740_OutMode_Move_X_LP_or_Wait:
                case Shuttle_1BP_AutoStep.Step760_OutMode_Move_X_LP:
                case Shuttle_1BP_AutoStep.Step770_OutMode_Move_Z_Down:
                case Shuttle_1BP_AutoStep.Step750_OutMode_Move_T_180_Deg:
                case Shuttle_1BP_AutoStep.Step780_OutMode_Check_OperationType:
                case Shuttle_1BP_AutoStep.Step800_OutMode_Await_PIO_CS:
                case Shuttle_1BP_AutoStep.Step810_OutMode_Check_PIO_Valid:
                //case Shuttle_1BP_AutoStep.Step811_OutMode_Call_AGV_Unload_Req:
                case Shuttle_1BP_AutoStep.Step820_OutMode_Check_PIO_TR:
                case Shuttle_1BP_AutoStep.Step830_OutMode_Check_PIO_Busy:
                    {
                        if (Carrier_CheckShuttle_ExistProduct(false))
                        {
                            Watchdog_Start(WatchdogList.BP_Placement_ErrorTimer);
                            if (Watchdog_IsDetect(WatchdogList.BP_Placement_ErrorTimer))
                                AlarmInsert((short)PortAlarm.Shuttle_Placement_Detect_Error, AlarmLevel.Error);
                        }
                        else
                            Watchdog_Stop(WatchdogList.BP_Placement_ErrorTimer, true);
                    }
                    break;
                default:
                    {
                        Watchdog_Stop(WatchdogList.BP_Placement_ErrorTimer, true);
                    }
                    break;
            }
        }

        /// <summary>
        /// 대기 중인 스텝을 제외하고 모션 중인 스텝에서 Time Out 확인
        /// </summary>
        private void Auto_1BP_OP_StepTimeOut_AlarmCheck()
        {
            if(m_eOP_1BP_AutoStep == Pre_OP_1BP_AutoStep &&
                !m_bOP_1BP_AutoStopEnable)
            {
                Watchdog_Start(WatchdogList.OP_Step_Timer);
                if (Watchdog_IsDetect(WatchdogList.OP_Step_Timer))
                    AlarmInsert((short)PortAlarm.Step_TimeOver_Error, AlarmLevel.Error);
            }
            else
            {
                Watchdog_Stop(WatchdogList.OP_Step_Timer, true);
            }

            if(m_eOP_1BP_AutoStep != Pre_OP_1BP_AutoStep)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortStepInfo, $"1BP OP Step : {Pre_OP_1BP_AutoStep} => {m_eOP_1BP_AutoStep}");

            Pre_OP_1BP_AutoStep = m_eOP_1BP_AutoStep;
        }
        private void Auto_1BP_BP_StepTimeOut_AlarmCheck()
        {
            if (m_eShuttle_1BP_AutoStep == Pre_Shuttle_1BP_AutoStep &&
                !m_bShuttle_1BP_AutoStopEnable)
            {
                Watchdog_Start(WatchdogList.BP_Step_Timer);
                if (Watchdog_IsDetect(WatchdogList.BP_Step_Timer))
                    AlarmInsert((short)PortAlarm.Step_TimeOver_Error, AlarmLevel.Error);
            }
            else
            {
                Watchdog_Stop(WatchdogList.BP_Step_Timer, true);
            }

            if(m_eShuttle_1BP_AutoStep != Pre_Shuttle_1BP_AutoStep)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortStepInfo, $"1BP Shuttle Step : {Pre_Shuttle_1BP_AutoStep} => {m_eShuttle_1BP_AutoStep}");

            Pre_Shuttle_1BP_AutoStep = m_eShuttle_1BP_AutoStep;
        }

        /// <summary>
        /// 장비 및 STK의 PIO 과정에서 Timeout 확인
        /// </summary>
        private void Auto_1BP_AGVorOHT_PIO_WatchdogUpdate()
        {
            if (GetPortOperationMode() == PortOperationMode.OHT)
            {
                if ((m_eShuttle_1BP_AutoStep >= Shuttle_1BP_AutoStep.Step810_OutMode_Check_PIO_Valid && m_eShuttle_1BP_AutoStep <= Shuttle_1BP_AutoStep.Step850_OutMode_Check_PIO_End) ||
                    (m_eShuttle_1BP_AutoStep >= Shuttle_1BP_AutoStep.Step210_InMode_Check_PIO_Valid && m_eShuttle_1BP_AutoStep <= Shuttle_1BP_AutoStep.Step250_InMode_Check_PIO_End))
                {
                    Watchdog_Start(WatchdogList.AGVorOHT_PIO_Timer);
                }
                else
                {
                    Watchdog_Stop(WatchdogList.AGVorOHT_PIO_Timer, true);
                }
            }
            else if (GetPortOperationMode() == PortOperationMode.AGV)
            {
                if ((m_eShuttle_1BP_AutoStep >= Shuttle_1BP_AutoStep.Step820_OutMode_Check_PIO_TR && m_eShuttle_1BP_AutoStep <= Shuttle_1BP_AutoStep.Step850_OutMode_Check_PIO_End) ||
                     (m_eShuttle_1BP_AutoStep >= Shuttle_1BP_AutoStep.Step220_InMode_Check_PIO_TR && m_eShuttle_1BP_AutoStep <= Shuttle_1BP_AutoStep.Step250_InMode_Check_PIO_End))
                {
                    Watchdog_Start(WatchdogList.AGVorOHT_PIO_Timer);
                }
                else
                {
                    Watchdog_Stop(WatchdogList.AGVorOHT_PIO_Timer, true);
                }
            }
        }
        private void Auto_1BP_RackMaster_PIO_WatchdogUpdate()
        {
            if ((m_eOP_1BP_AutoStep >= OP_1BP_AutoStep.Step710_OutMode_Check_PIO_Busy && m_eOP_1BP_AutoStep <= OP_1BP_AutoStep.Step730_OutMode_Check_PIO_End) ||
                (m_eOP_1BP_AutoStep >= OP_1BP_AutoStep.Step310_InMode_Check_PIO_Busy && m_eOP_1BP_AutoStep <= OP_1BP_AutoStep.Step330_InMode_Check_PIO_End))
            {
                Watchdog_Start(WatchdogList.RackMaster_PIO_Timer);
            }
            else
            {
                Watchdog_Stop(WatchdogList.RackMaster_PIO_Timer, true);
            }
        }
        
        /// <summary>
        /// 정지 명령시 상태 및 와치독, 플래그 초기화
        /// </summary>
        private void Auto_1BP_OP_StopInit()
        {
            m_bOP_1BP_AutoRunning = false;
            Port_To_RM_PIO_Init();
            Watchdog_Stop(WatchdogList.OP_Placement_ErrorTimer, false);
            Watchdog_Stop(WatchdogList.RackMaster_PIO_Timer, true);
        }
        private void Auto_1BP_BP_StopInit()
        {
            m_bShuttle_1BP_AutoRunning = false;
            m_bCycleRunning = false;
            Port_To_AGV_PIO_Init();
            Port_To_OHT_PIO_Init();
            Watchdog_Stop(WatchdogList.BP_Placement_ErrorTimer, false);
            Watchdog_Stop(WatchdogList.AGVorOHT_PIO_Timer, true);

            CMD_PortStop();
        }
        
        /// <summary>
        /// 실제 오토 공정 함수
        /// </summary>
        private void Auto_1BP_Start_OP_Control()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                while (m_bOP_1BP_AutoRunning)
                {
                    //1. Port가 알람 상태인 경우 정지
                    if (GetAlarmLevel() == AlarmLevel.Error)
                    {
                        Auto_1BP_OP_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAlarmStop, $"OP 1BP");
                        break;
                    }

                    //2. Step에 연관된 알람 상황 체크
                    Auto_1BP_OP_Placement_Detect_AlarmCheck();
                    Auto_1BP_OP_StepTimeOut_AlarmCheck();

                    //3. Step에 연관된 상태 업데이트
                    Auto_1BP_OPStopEnableUpdate();
                    Auto_1BP_RackMaster_PIO_WatchdogUpdate();

                    //4. 정지 명령 시 중단
                    if (IsAutoStopReq(true))
                    {
                        if (m_bOP_1BP_AutoStopEnable)
                        {
                            Auto_1BP_OP_StopInit();
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AutoControlStopCommand, $"OP 1BP");
                            break;
                        }
                    }

                    //5. 동작 수행
                    switch (m_eOP_1BP_AutoStep)
                    {
                        case OP_1BP_AutoStep.Step000_Check_Direction:
                            {
                                Port_To_RM_PIO_Init();

                                if (GetOperationDirection() == PortDirection.Input)
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step100_InMode_Check_OP_CST_Load;
                                else
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step600_OutMode_Check_OP_CST_Load;
                            }
                            break;
                        case OP_1BP_AutoStep.Step100_InMode_Check_OP_CST_Load:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) && Carrier_CheckOP_ExistID())
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step210_InMode_Check_BP_Location;
                                else if (Carrier_CheckOP_ExistProduct(false) && !Carrier_CheckOP_ExistID())
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step200_InMode_Call_BP_Load_Req;
                                else if (Carrier_CheckOP_ExistProduct(true) && !Carrier_CheckOP_ExistID())
                                {
                                    AlarmInsert((short)PortAlarm.OP_No_Cassette_ID_Error, AlarmLevel.Error);
                                }
                                else if (Carrier_CheckOP_ExistProduct(false) && Carrier_CheckOP_ExistID())
                                {
                                    OP_CarrierID = string.Empty;
                                }
                                else
                                {
                                    AlarmInsert((short)PortAlarm.OP_CST_Detect_Sensor_Group_Error, AlarmLevel.Error);
                                }
                            }
                            break;
                        case OP_1BP_AutoStep.Step200_InMode_Call_BP_Load_Req:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) && Carrier_CheckOP_ExistID())
                                {
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step210_InMode_Check_BP_Location;
                                }
                            }
                            break;
                        case OP_1BP_AutoStep.Step210_InMode_Check_BP_Location:
                            {
                                // 231011 수정 - OHT Port일 경우 Wait에서 Shuttle이 대기하므로 Wait Position에서 Location 체크 하도록 수정 - Louis
                                if (m_eShuttle_1BP_AutoStep >= Shuttle_1BP_AutoStep.Step200_InMode_Await_PIO_CS &&
                                    m_eShuttle_1BP_AutoStep <= Shuttle_1BP_AutoStep.Step500_InMode_Await_OP_Load_Req &&
                                    IsXAxisPos_LP()) // LP에서 대기하는 거로 바뀜 //(GetPortOperationMode() == PortOperationMode.OHT ? IsXAxisPos_WaitorLP() : IsXAxisPos_LP())
                                {
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step300_InMode_Await_PIO_TR_REQ;
                                }
                            }
                            break;
                        case OP_1BP_AutoStep.Step300_InMode_Await_PIO_TR_REQ:
                            {
                                if(INPUT_DIR_STK_TR_REQ_Await())
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step310_InMode_Check_PIO_Busy;
                            }
                            break;
                        case OP_1BP_AutoStep.Step310_InMode_Check_PIO_Busy:
                            {
                                if(INPUT_DIR_STK_BUSY_Check())
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step320_InMode_Check_PIO_Complete;
                            }
                            break;
                        case OP_1BP_AutoStep.Step320_InMode_Check_PIO_Complete:
                            {
                                if(INPUT_DIR_STK_COMPLETE_Check())
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step330_InMode_Check_PIO_End;
                            }
                            break;
                        case OP_1BP_AutoStep.Step330_InMode_Check_PIO_End:
                            {
                                if(INPUT_DIR_STK_PIO_END_Check())
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step340_InMode_Check_OP_CST_Unload_And_Safe;
                            }
                            break;
                        case OP_1BP_AutoStep.Step340_InMode_Check_OP_CST_Unload_And_Safe:
                            {
                                if (!Sensor_OP_Fork_Detect)
                                {
                                    if (Carrier_CheckOP_ExistProduct(false))
                                    {
                                        Carrier_ClearPortToRM_CarrierID();
                                        m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step000_Check_Direction;
                                    }
                                    else
                                        AlarmInsert((short)PortAlarm.OP_CST_Detect_Sensor_Group_Error, AlarmLevel.Error);
                                }
                            }
                            break;
                        case OP_1BP_AutoStep.Step600_OutMode_Check_OP_CST_Load:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) && Carrier_CheckOP_ExistID())
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step800_OutMode_Call_BP_Unload_Req;
                                else if (Carrier_CheckOP_ExistProduct(false) && !Carrier_CheckOP_ExistID())
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step700_OutMode_Await_PIO_TR_REQ;
                                else if (Carrier_CheckOP_ExistProduct(true) && !Carrier_CheckOP_ExistID())
                                {
                                    AlarmInsert((short)PortAlarm.OP_No_Cassette_ID_Error, AlarmLevel.Error);
                                }
                                else if (Carrier_CheckOP_ExistProduct(false) && Carrier_CheckOP_ExistID())
                                {
                                    OP_CarrierID = string.Empty;
                                }
                                else
                                {
                                    AlarmInsert((short)PortAlarm.OP_CST_Detect_Sensor_Group_Error, AlarmLevel.Error);
                                }
                            }
                            break;
                        case OP_1BP_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                            {
                                if(OUTPUT_DIR_STK_PIO_TR_REQ_Await())
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step710_OutMode_Check_PIO_Busy;
                            }
                            break;
                        case OP_1BP_AutoStep.Step710_OutMode_Check_PIO_Busy:
                            {
                                if(OUTPUT_DIR_STK_PIO_BUSY_Check())
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step720_OutMode_Check_PIO_Complete;
                            }
                            break;
                        case OP_1BP_AutoStep.Step720_OutMode_Check_PIO_Complete:
                            {
                                if(OUTPUT_DIR_STK_PIO_COMPLETE_Check())
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step730_OutMode_Check_PIO_End;
                            }
                            break;
                        case OP_1BP_AutoStep.Step730_OutMode_Check_PIO_End:
                            {
                                if(OUTPUT_DIR_STK_PIO_END_Check())
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe;
                            }
                            break;
                        case OP_1BP_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) && !Sensor_OP_Fork_Detect)
                                {
                                    m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step800_OutMode_Call_BP_Unload_Req;
                                }
                            }
                            break;
                        case OP_1BP_AutoStep.Step800_OutMode_Call_BP_Unload_Req:
                            {
                                if(m_eShuttle_1BP_AutoStep >= Shuttle_1BP_AutoStep.Step760_OutMode_Move_X_LP &&
                                    !Carrier_CheckOP_ExistID() && !Sensor_OP_Fork_Detect)
                                {
                                    if(Carrier_CheckOP_ExistProduct(false))
                                        m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step000_Check_Direction;
                                    else
                                        AlarmInsert((short)PortAlarm.OP_CST_Detect_Sensor_Group_Error, AlarmLevel.Error);
                                }
                            }
                            break;
                    }
                    Thread.Sleep(Master.StepUpdateTime);
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Start();
        }
        private void Auto_1BP_Start_Shuttle_Control()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                while (m_bShuttle_1BP_AutoRunning || m_bCycleRunning)
                {
                    //1. Auto Flag, Cycle Flag가 둘다 true인 경우 알람 (실제 발생 가능성은 거의 없지만 안전 장치)
                    if (m_bShuttle_1BP_AutoRunning && m_bCycleRunning)
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAndCycleCrash, $"Shuttle 1BP");
                        Auto_1BP_BP_StopInit();
                        break;
                    }

                    bool bAuto = m_bShuttle_1BP_AutoRunning;

                    //2. Port가 알람 상태인 경우 정지
                    if (GetAlarmLevel() == AlarmLevel.Error)
                    {
                        Auto_1BP_BP_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAlarmStop, $"Shuttle 1BP");
                        break;
                    }

                    //3. Step에 연관된 알람 상황 체크
                    Auto_1BP_BP_Placement_Detect_AlarmCheck();
                    Auto_1BP_BP_StepTimeOut_AlarmCheck();

                    //4. Step에 연관된 상태 업데이트
                    Auto_1BP_BPStopEnableUpdate();
                    Auto_1BP_AGVorOHT_PIO_WatchdogUpdate();

                    //5. 정지 명령 시 중단
                    if (IsAutoStopReq(bAuto))
                    {
                        if (m_bShuttle_1BP_AutoStopEnable)
                        {
                            Auto_1BP_BP_StopInit();
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AutoControlStopCommand, $"Shuttle 1BP");
                            break;
                        }
                    }

                    //6. 동작 수행
                    switch (m_eShuttle_1BP_AutoStep)
                    {
                        case Shuttle_1BP_AutoStep.Step000_Check_Direction:
                            {
                                Port_To_AGV_PIO_Init();
                                Port_To_OHT_PIO_Init();
                                ServoCtrl_ClearAxisTorqueCal();

                                if (GetOperationDirection() == PortDirection.Input)
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step100_InMode_Check_BP_CST_Load_And_Pose;
                                else
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step600_OutMode_Check_BP_CST_Load_And_Pose;
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step001_Change_Direction:
                            {
                                if (GetOperationDirection() == PortDirection.Input)
                                    GetMotionParam().ePortDirection = PortDirection.Output;
                                else
                                    GetMotionParam().ePortDirection = PortDirection.Input;

                                m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step000_Check_Direction;
                            }
                            break;

                        case Shuttle_1BP_AutoStep.Step100_InMode_Check_BP_CST_Load_And_Pose:
                            {
                                if (Carrier_CheckShuttle_ExistProduct(true))
                                {
                                    if(!IsTAxisPos_0_Deg())
                                    {
                                        if (!IsZAxisPos_UP(PortAxis.Shuttle_Z))
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step140_InMode_Move_Z_Up;
                                        else if (!IsXAxisPos_WaitorLP())
                                        {
                                            if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                                m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step130_InMode_Move_X_Wait;
                                            else
                                                m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step120_InMode_Move_X_LP;
                                        }
                                        else
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step160_InMode_Move_T_0_Deg;
                                    }
                                    else
                                    {
                                        if (!IsZAxisPos_UP(PortAxis.Shuttle_Z) && !IsXAxisPos_WaitorLP())
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step140_InMode_Move_Z_Up;
                                        else if (IsZAxisPos_UP(PortAxis.Shuttle_Z) && !IsXAxisPos_WaitorLP())
                                        {
                                            if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                                m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step130_InMode_Move_X_Wait;
                                            else
                                                m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step120_InMode_Move_X_LP;

                                        }
                                        else
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step400_InMode_Ready_CST_ID_Read;
                                    }
                                }
                                else if (Carrier_CheckShuttle_ExistProduct(false) && !Carrier_CheckBP_ExistID(0))
                                {
                                    //받을 준비 루틴
                                    //Z Down
                                    if (!IsZAxisPos_DOWN(PortAxis.Shuttle_Z))
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step110_InMode_Move_Z_Down;
                                    else
                                    {
                                        //180 Deg
                                        if (!IsTAxisPos_180_Deg())
                                        {
                                            //180 도로 보내는데 Wait pos 여부에 따라 회전 위치 지정
                                            if (!IsXAxisPos_WaitorLP())
                                            {
                                                if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step130_InMode_Move_X_Wait;
                                                else
                                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step120_InMode_Move_X_LP;
                                            }
                                            else
                                                m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step150_InMode_Move_T_180_Deg;
                                        }
                                        // ------------------------------------------------------ 230731 updated -----------------------------------------------------
                                        else if (IsTAxisPos_180_Deg())
                                        {
                                            //2023-10-20 OHT도 LP에서 받도록 수정
                                            //if (IsOHT())
                                            //{
                                            //    if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                            //    {
                                            //        //Wait pos에서 대기
                                            //        if (!IsXAxisPos_Wait())
                                            //            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step130_InMode_Move_X_Wait;
                                            //        else
                                            //            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step170_InMode_Check_OperationType;
                                            //    }
                                            //    else
                                            //    {
                                            //        //Wait pos 사용이 아닌 경우 LP에서 대기
                                            //        if (!IsXAxisPos_LP())
                                            //            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step120_InMode_Move_X_LP;
                                            //        else
                                            //            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step170_InMode_Check_OperationType;
                                            //    }                                             
                                            //}
                                            //else
                                            //{
                                                //LP에서 대기
                                                if (!IsXAxisPos_LP())
                                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step120_InMode_Move_X_LP;
                                                else
                                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step170_InMode_Check_OperationType;
                                            //}
                                            // -----------------------------------------------------------------------------------------------------------------------------
                                            // Original Code : 
                                            //if (!IsXAxisPos_LP())
                                            //    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step120_InMode_Move_X_LP; 
                                        }
                                        else
                                        {
                                            if (IsTAxisPos_180_Deg() || IsTAxisPos_0_Deg())
                                            {
                                                if (!IsXAxisPos_WaitorLP())
                                                {
                                                    if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X) && !IsTAxisPos_180_Deg())
                                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step130_InMode_Move_X_Wait;
                                                    else
                                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step120_InMode_Move_X_LP;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (Carrier_CheckShuttle_ExistProduct(false) && Carrier_CheckBP_ExistID(0))
                                {
                                    Carrier_ClearBP_CarrierID(0);
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step110_InMode_Move_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Down_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step100_InMode_Check_BP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step120_InMode_Move_X_LP:
                            {
                                if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step100_InMode_Check_BP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step130_InMode_Move_X_Wait:
                            {
                                if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.Wait_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step100_InMode_Check_BP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step140_InMode_Move_Z_Up:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Up_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step100_InMode_Check_BP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step150_InMode_Move_T_180_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree180_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step100_InMode_Check_BP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step160_InMode_Move_T_0_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree0_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step100_InMode_Check_BP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step170_InMode_Check_OperationType:
                            {
                                if (IsOHT())
                                    m_eShuttle_1BP_AutoStep = bAuto ? Shuttle_1BP_AutoStep.Step200_InMode_Await_PIO_CS : Shuttle_1BP_AutoStep.Step300_InMode_Await_MGV_CST_Load;
                                else if(IsAGV())
                                    m_eShuttle_1BP_AutoStep = AGVFullFlagOption && bAuto ? Shuttle_1BP_AutoStep.Step200_InMode_Await_PIO_CS : bAuto ? Shuttle_1BP_AutoStep.Step210_InMode_Check_PIO_Valid : Shuttle_1BP_AutoStep.Step300_InMode_Await_MGV_CST_Load;
                                else if(IsMGV())
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step300_InMode_Await_MGV_CST_Load;
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step200_InMode_Await_PIO_CS:
                            {
                                if (Is_EquipType_To_Port_PIO_CSFlag() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step210_InMode_Check_PIO_Valid;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                                else if (Carrier_CheckShuttle_ExistProduct(true))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step250_InMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step210_InMode_Check_PIO_Valid:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (!Is_EquipType_To_Port_PIO_ValidFlag() || !Is_EquipType_To_Port_PIO_CSFlag())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (Is_EquipType_To_Port_PIO_ValidFlag() &&
                                     ((IsOHT() || (IsAGV() && AGVFullFlagOption)) ? Is_EquipType_To_Port_PIO_CSFlag() : true) &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    Set_Port_To_Equip_PIO_Load_ReqFlag(true);
                                    //Set_Port_To_Equip_PIO_ESFlag(true); <- 231018 Louis 수정, Normaml 상태에선 ES 신호 올라오면 안 됨
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step220_InMode_Check_PIO_TR;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                                else if (Carrier_CheckShuttle_ExistProduct(true))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step250_InMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        //case Shuttle_1BP_AutoStep.Step211_InMode_Call_AGV_Load_Req:
                        //    {
                        //        if(Carrier_CheckShuttle_ExistProduct(false) &&
                        //            !Is_LightCurtain_or_Hoist_SensorCheck())
                        //        {
                        //            Set_Port_To_AGVorOHT_PIO_Load_ReqFlag(true);

                        //            if(Is_AGVorOHT_To_Port_PIO_TR_ReqFlag())
                        //            {
                        //                m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step220_InMode_Check_PIO_TR;
                        //                Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                        //            }
                        //        }
                        //        else if(Carrier_CheckShuttle_ExistProduct(true))
                        //        {
                        //            Set_Port_To_AGVorOHT_PIO_Load_ReqFlag(false);
                        //            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step250_InMode_Check_PIO_End;
                        //            Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                        //        }
                        //    }
                        //    break;
                        case Shuttle_1BP_AutoStep.Step220_InMode_Check_PIO_TR:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (!Is_EquipType_To_Port_PIO_TR_ReqFlag())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (IsAGV())
                                {
                                    Watchdog_ResetAndStop(WatchdogList.PortArea_Timer);
                                }

                                if (Is_EquipType_To_Port_PIO_TR_ReqFlag())
                                {
                                    Set_Port_To_Equip_PIO_ReadyFlag(true);
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step230_InMode_Check_PIO_Busy;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step230_InMode_Check_PIO_Busy:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (!Is_EquipType_To_Port_PIO_TR_ReqFlag() || !Is_EquipType_To_Port_PIO_BusyFlag())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (IsAGV())
                                {
                                    Watchdog_ResetAndStop(WatchdogList.PortArea_Timer);
                                }

                                if (Is_EquipType_To_Port_PIO_TR_ReqFlag() &&
                                    Is_EquipType_To_Port_PIO_BusyFlag())
                                {
                                    //Set_Port_To_Equip_PIO_Load_ReqFlag(false);
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step240_InMode_Check_PIO_Complete;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step240_InMode_Check_PIO_Complete:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (Is_EquipType_To_Port_PIO_TR_ReqFlag() || Is_EquipType_To_Port_PIO_BusyFlag() || !Is_EquipType_To_Port_PIO_Complete())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (IsAGV())
                                {
                                    Watchdog_ResetAndStop(WatchdogList.PortArea_Timer);
                                }

                                //PIO 중 자재가 생기면 Off
                                if(Carrier_CheckShuttle_ExistProduct(true) && IsLPOppositeAngle())
                                    Set_Port_To_Equip_PIO_Load_ReqFlag(false);

                                if (!Is_EquipType_To_Port_PIO_TR_ReqFlag() &&
                                    !Is_EquipType_To_Port_PIO_BusyFlag() &&
                                    Is_EquipType_To_Port_PIO_Complete() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck() &&
                                    !Is_Port_To_Equip_PIO_Load_ReqFlag())
                                {
                                    Set_Port_To_Equip_PIO_ReadyFlag(false);
                                    //Set_Port_To_Equip_PIO_ESFlag(false);
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step250_InMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step250_InMode_Check_PIO_End:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (Is_EquipType_To_Port_PIO_ValidFlag() || Is_EquipType_To_Port_PIO_CSFlag() || Is_EquipType_To_Port_PIO_TR_ReqFlag() || Is_EquipType_To_Port_PIO_BusyFlag() || Is_EquipType_To_Port_PIO_Complete())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (!Is_EquipType_To_Port_PIO_ValidFlag() &&
                                     ((IsOHT() || (IsAGV() && AGVFullFlagOption)) ? !Is_EquipType_To_Port_PIO_CSFlag() : true) &&
                                    !Is_EquipType_To_Port_PIO_TR_ReqFlag() &&
                                    !Is_EquipType_To_Port_PIO_BusyFlag() &&
                                    !Is_EquipType_To_Port_PIO_Complete())
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step260_InMode_Check_CST_Load_And_Safe;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step260_InMode_Check_CST_Load_And_Safe:
                            {
                                if (Carrier_CheckShuttle_ExistProduct(true) && 
                                    IsLPOppositeAngle() && 
                                    !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step400_InMode_Ready_CST_ID_Read; //Shuttle_1BP_AutoStep.Step400_InMode_Ready_CST_ID_Read;
                                }
                                else
                                {
                                    //Abnormal Case (PIO 이후 적재 없거나 이상한 상황)
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                            {
                                if (Is_CartDetct1_Check() || Is_CartDetct2_Check())
                                    Watchdog_ResetAndStop(WatchdogList.PortArea_Timer);

                                if (Carrier_CheckShuttle_ExistProduct(true) &&
                                    IsLPOppositeAngle() && 
                                    !Is_LightCurtain_or_Hoist_SensorCheck() &&
                                    !Is_CartDetct1_Check() && 
                                    !Is_CartDetct2_Check() &&
                                    !Is_OHTDoorOpen_Check())
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step400_InMode_Ready_CST_ID_Read;//Shuttle_1BP_AutoStep.Step400_InMode_Ready_CST_ID_Read;
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step400_InMode_Ready_CST_ID_Read:
                            {
                                if(TAG_READ_INIT())
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step410_InMode_Start_CST_ID_Read;
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step410_InMode_Start_CST_ID_Read:
                            {                                
                                if (Carrier_CheckShuttle_ExistProduct(true))
                                {
                                    if(TAG_READ_TRY(TAG_ID_READ_SET_SECTION.BP))
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step420_InMode_Move_Z_UP;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step420_InMode_Move_Z_UP:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Up_Pos))
                                {
                                    if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step430_InMode_Move_X_Wait;
                                    else
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step440_InMode_Move_T_0_Deg;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step430_InMode_Move_X_Wait:
                            {
                                if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.Wait_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step440_InMode_Move_T_0_Deg;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step440_InMode_Move_T_0_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree0_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step500_InMode_Await_OP_Load_Req;
                                }
                            }
                            break;

                        case Shuttle_1BP_AutoStep.Step500_InMode_Await_OP_Load_Req:
                            {
                                if (Carrier_CheckOP_ExistProduct(false) && !Sensor_OP_Fork_Detect)
                                {
                                    if (bAuto && m_eOP_1BP_AutoStep == OP_1BP_AutoStep.Step200_InMode_Call_BP_Load_Req)
                                    {
                                        //자동 공정
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step510_InMode_Move_X_OP;
                                    }
                                    else if (!bAuto)
                                    {
                                        //매뉴얼 공정
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step510_InMode_Move_X_OP;
                                    }
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step510_InMode_Move_X_OP:
                            {
                                if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.OP_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step520_InMode_Move_Z_Down;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step520_InMode_Move_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Down_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step530_InMode_Send_OP_CST_ID;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step530_InMode_Send_OP_CST_ID:
                            {
                                if (Carrier_CheckShuttle_ExistProduct(false) &&
                                    Carrier_CheckOP_ExistProduct(true))
                                {
                                    if (Carrier_Shuttle_To_OP_CST_ID_Transfer())
                                    {
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step540_InMode_Move_X_LP_or_Wait;
                                        //if(bAuto)
                                        //    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step000_Check_Direction;
                                        //else
                                        //{
                                        //    if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                        //    {
                                        //        //카운트를 증가시키고
                                        //        m_CycleControlProgressCount++;
                                        //        //-----------Direction Change 실행
                                        //        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step001_Change_Direction;
                                        //    }
                                        //    else
                                        //    {
                                        //        m_CycleControlProgressCount++;
                                        //        m_bAutoManualCycleStopReq = true;
                                        //        m_bCycleRunning = false;
                                        //    }
                                        //}
                                    }
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step540_InMode_Move_X_LP_or_Wait:
                            {
                                if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X) ? Teaching_X_Pos.Wait_Pos : IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step550_InMode_Move_T_180_Deg;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step550_InMode_Move_T_180_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree180_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step560_InMode_Move_X_LP;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step560_InMode_Move_X_LP:
                            {
                                // ------------------------------------------------------ 230731 updated -----------------------------------------------------
                                bool bMotionDone = false;
                                if (IsOHT() && GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                    bMotionDone = X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.Wait_Pos);
                                else
                                    bMotionDone = X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos);
                                // Original Code : 
                                if (bMotionDone)
                                {
                                    if (bAuto)
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step000_Check_Direction;
                                    else
                                    {
                                        if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                        {
                                            //카운트를 증가시키고
                                            m_CycleControlProgressCount++;
                                            //-----------Direction Change 실행
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step001_Change_Direction;
                                        }
                                        else
                                        {
                                            m_CycleControlProgressCount++;
                                            m_bAutoManualCycleStopReq = true;
                                            m_bCycleRunning = false;
                                        }
                                    }
                                }
                            }
                            // -----------------------------------------------------------------------------------------------------------------------------
                            break;
                        case Shuttle_1BP_AutoStep.Step600_OutMode_Check_BP_CST_Load_And_Pose:
                            {
                                if (Carrier_CheckShuttle_ExistProduct(true) && Carrier_CheckBP_ExistID(0))
                                {                                    
                                    if(!IsTAxisPos_180_Deg())
                                    {
                                        if (!IsZAxisPos_UP(PortAxis.Shuttle_Z))
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step640_OutMode_Move_Z_Up;
                                        else if (!IsXAxisPos_WaitorLP())
                                        {
                                            //180도 회전을 위한 위치 이동
                                            if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                                m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step630_OutMode_Move_X_Wait;
                                            else
                                                m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step620_OutMode_Move_X_LP;
                                        }
                                        else
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step650_OutMode_Move_T_180_Deg;

                                    }
                                    else
                                    {
                                        if (!IsZAxisPos_UP(PortAxis.Shuttle_Z) && !IsXAxisPos_LP())
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step640_OutMode_Move_Z_Up;
                                        else if (IsZAxisPos_UP(PortAxis.Shuttle_Z) && !IsXAxisPos_LP())
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step620_OutMode_Move_X_LP;
                                        else if (!IsZAxisPos_DOWN(PortAxis.Shuttle_Z) && IsXAxisPos_LP())
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step610_OutMode_Move_Z_Down;
                                        else
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step780_OutMode_Check_OperationType;
                                    }
                                }
                                else if (Carrier_CheckShuttle_ExistProduct(false) && !Carrier_CheckBP_ExistID(0))
                                {
                                    if (!IsZAxisPos_DOWN(PortAxis.Shuttle_Z))
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step610_OutMode_Move_Z_Down;
                                    else
                                    {
                                        if (!IsXAxisPos_WaitorLP())
                                        {
                                            if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                                m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step630_OutMode_Move_X_Wait;
                                            else
                                                m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step620_OutMode_Move_X_LP;
                                        }
                                        else if (!IsTAxisPos_0_Deg())
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step660_OutMode_Move_T_0_Deg;
                                        else
                                            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step700_OutMode_Await_OP_Unload_Req;
                                    }
                                }
                                else if (Carrier_CheckShuttle_ExistProduct(true) && !Carrier_CheckBP_ExistID(0))
                                {
                                    //if(m_bTestMode)
                                    //    Carrier_SetBP_CarrierID(0, "TestID");
                                    //else
                                        AlarmInsert((short)PortAlarm.Shuttle_No_Cassette_ID_Error, AlarmLevel.Error);
                                }
                                else if (Carrier_CheckShuttle_ExistProduct(false) && Carrier_CheckBP_ExistID(0))
                                {
                                    Carrier_ClearBP_CarrierID(0);
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step610_OutMode_Move_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Down_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step600_OutMode_Check_BP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step620_OutMode_Move_X_LP:
                            {
                                if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step600_OutMode_Check_BP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step630_OutMode_Move_X_Wait:
                            {
                                if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.Wait_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step600_OutMode_Check_BP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step640_OutMode_Move_Z_Up:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Up_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step600_OutMode_Check_BP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step650_OutMode_Move_T_180_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree180_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step600_OutMode_Check_BP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step660_OutMode_Move_T_0_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree0_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step600_OutMode_Check_BP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step700_OutMode_Await_OP_Unload_Req:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) &&
                                    !Sensor_OP_Fork_Detect)
                                {
                                    if (m_eOP_1BP_AutoStep == OP_1BP_AutoStep.Step800_OutMode_Call_BP_Unload_Req && bAuto)
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step710_OutMode_Move_X_OP;
                                    else if(!bAuto)
                                    {
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step710_OutMode_Move_X_OP;
                                    }
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step710_OutMode_Move_X_OP:
                            {
                                if (Carrier_CheckOP_ExistProduct(true, false) &&
                                    Carrier_CheckShuttle_ExistProduct(false) &&
                                    Carrier_CheckOP_ExistID() && bAuto)
                                {
                                    //Auto 상황 : OP에 제품이 있으며 ID가 부여되어 있고 BP에는 Carrier가 없다면 OP 위치로 이동
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.OP_Pos))
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step720_OutMode_Move_Z_Up;
                                }
                                else if (Carrier_CheckOP_ExistProduct(true, false) &&
                                        Carrier_CheckShuttle_ExistProduct(false) &&
                                        !bAuto)
                                {
                                    //Manual 상황 : OP에 제품이 있으며 BP에 제품이 없는 경우 ID직접 부여후 OP 위치로 이동
                                    OP_CarrierID = "CYCLE_TEST_ID";
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.OP_Pos))
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step720_OutMode_Move_Z_Up;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step720_OutMode_Move_Z_Up:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Up_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step730_OutMode_Read_OP_CST_ID;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step730_OutMode_Read_OP_CST_ID:
                            {
                                if (Carrier_CheckOP_ExistProduct(false, false) &&
                                    Carrier_CheckShuttle_ExistProduct(true))
                                {
                                    if (Carrier_OP_To_Shuttle_CST_ID_Transfer())
                                    {
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step740_OutMode_Move_X_LP_or_Wait;
                                    }
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step740_OutMode_Move_X_LP_or_Wait:
                            {
                                if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X) ? Teaching_X_Pos.Wait_Pos : IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step750_OutMode_Move_T_180_Deg;
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step750_OutMode_Move_T_180_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree180_Pos))
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step760_OutMode_Move_X_LP;
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step760_OutMode_Move_X_LP:
                            {
                                //// ------------------------------------------------- 230731 updated --------------------------------------------------------------
                                //if (IsOHT())
                                //{
                                //    if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                //    {
                                //        if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.Wait_Pos))
                                //            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step770_OutMode_Move_Z_Down;
                                //    }
                                //    else
                                //    {
                                //        if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                //            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step770_OutMode_Move_Z_Down;
                                //    }
                                //}
                                //else
                                //{
                                //    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                //        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step770_OutMode_Move_Z_Down;
                                //}
                                //// -----------------------------------------------------------------------------------------------------------------------------
                                // Original Code : 
                                if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step770_OutMode_Move_Z_Down;
                            }
                            break;
                        
                        case Shuttle_1BP_AutoStep.Step770_OutMode_Move_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Down_Pos))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step780_OutMode_Check_OperationType;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step780_OutMode_Check_OperationType:
                            {
                                if (IsOHT())
                                    m_eShuttle_1BP_AutoStep = bAuto ? Shuttle_1BP_AutoStep.Step800_OutMode_Await_PIO_CS : Shuttle_1BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload;
                                else if (IsAGV())
                                    m_eShuttle_1BP_AutoStep = AGVFullFlagOption && bAuto ? Shuttle_1BP_AutoStep.Step800_OutMode_Await_PIO_CS  : bAuto ? Shuttle_1BP_AutoStep.Step810_OutMode_Check_PIO_Valid : Shuttle_1BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload;
                                else if (IsMGV())
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload;
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step800_OutMode_Await_PIO_CS:
                            {
                                if (Carrier_CheckShuttle_ExistProduct(true) &&
                                    IsLPOppositeAngle() &&
                                    Is_EquipType_To_Port_PIO_CSFlag())
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step810_OutMode_Check_PIO_Valid;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                                else if (Carrier_CheckShuttle_ExistProduct(false))
                                {
                                    //PIO 대기 중 제품이 사라진 경우 ID를 삭제 후 다시 첫 스텝으로 회귀
                                    if (Carrier_CheckBP_ExistID(0))
                                        Carrier_ClearBP_CarrierID(0);

                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step000_Check_Direction;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step810_OutMode_Check_PIO_Valid:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (!Is_EquipType_To_Port_PIO_ValidFlag() || !Is_EquipType_To_Port_PIO_CSFlag())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (Carrier_CheckShuttle_ExistProduct(true) &&
                                    IsLPOppositeAngle() &&
                                     ((IsOHT() || (IsAGV() && AGVFullFlagOption)) ? Is_EquipType_To_Port_PIO_CSFlag() : true) &&
                                    Is_EquipType_To_Port_PIO_ValidFlag() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    Set_Port_To_Equip_PIO_Unload_ReqFlag(true);
                                    //Set_Port_To_Equip_PIO_ESFlag(true);
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step820_OutMode_Check_PIO_TR;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                                else if (Carrier_CheckShuttle_ExistProduct(false))
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step850_OutMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        //case Shuttle_1BP_AutoStep.Step811_OutMode_Call_AGV_Unload_Req:
                        //    {
                        //        if (Carrier_CheckShuttle_ExistProduct(true) &&
                        //            !Is_LightCurtain_or_Hoist_SensorCheck())
                        //        {
                        //            Set_Port_To_AGVorOHT_PIO_Unload_ReqFlag(true);

                        //            if (Is_AGVorOHT_To_Port_PIO_TR_ReqFlag())
                        //            {
                        //                m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step820_OutMode_Check_PIO_TR;
                        //                Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                        //            }
                        //        }
                        //        else if (Carrier_CheckShuttle_ExistProduct(false))
                        //        {
                        //            Set_Port_To_AGVorOHT_PIO_Unload_ReqFlag(false);
                        //            m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step850_OutMode_Check_PIO_End;
                        //            Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                        //        }
                        //    }
                        //    break;
                        case Shuttle_1BP_AutoStep.Step820_OutMode_Check_PIO_TR:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (!Is_EquipType_To_Port_PIO_ValidFlag() || !Is_EquipType_To_Port_PIO_CSFlag())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if(IsAGV())
                                {
                                    Watchdog_ResetAndStop(WatchdogList.PortArea_Timer);
                                }

                                if (Is_EquipType_To_Port_PIO_TR_ReqFlag())
                                {
                                    Set_Port_To_Equip_PIO_ReadyFlag(true);
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step830_OutMode_Check_PIO_Busy;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step830_OutMode_Check_PIO_Busy:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (!Is_EquipType_To_Port_PIO_TR_ReqFlag() || !Is_EquipType_To_Port_PIO_BusyFlag())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (IsAGV())
                                {
                                    Watchdog_ResetAndStop(WatchdogList.PortArea_Timer);
                                }

                                if (Is_EquipType_To_Port_PIO_TR_ReqFlag() &&
                                    Is_EquipType_To_Port_PIO_BusyFlag() &&
                                    Carrier_CheckShuttle_ExistProduct(true) &&
                                    IsLPOppositeAngle())
                                {
                                    //Set_Port_To_Equip_PIO_Unload_ReqFlag(false);
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step840_OutMode_Check_PIO_Complete;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step840_OutMode_Check_PIO_Complete:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (Is_EquipType_To_Port_PIO_TR_ReqFlag() || Is_EquipType_To_Port_PIO_BusyFlag() || !Is_EquipType_To_Port_PIO_Complete())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (IsAGV())
                                {
                                    Watchdog_ResetAndStop(WatchdogList.PortArea_Timer);
                                }

                                //PIO 중 자재가 사라지면 Off
                                if(Carrier_CheckShuttle_ExistProduct(false) && !IsLPOppositeAngle())
                                    Set_Port_To_Equip_PIO_Unload_ReqFlag(false);

                                if (Carrier_CheckShuttle_ExistProduct(false))
                                    Carrier_ClearBP_CarrierID(0);

                                if (!Is_EquipType_To_Port_PIO_TR_ReqFlag() &&
                                    !Is_EquipType_To_Port_PIO_BusyFlag() &&
                                    Is_EquipType_To_Port_PIO_Complete() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck() &&
                                    !Is_Port_To_Equip_PIO_Unload_ReqFlag())
                                {
                                    Set_Port_To_Equip_PIO_ReadyFlag(false);
                                    //Set_Port_To_Equip_PIO_ESFlag(false);
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step850_OutMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step850_OutMode_Check_PIO_End:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (Is_EquipType_To_Port_PIO_ValidFlag() || Is_EquipType_To_Port_PIO_CSFlag() || Is_EquipType_To_Port_PIO_TR_ReqFlag() || Is_EquipType_To_Port_PIO_BusyFlag() || Is_EquipType_To_Port_PIO_Complete())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (!Is_EquipType_To_Port_PIO_ValidFlag() &&
                                     ((IsOHT() || (IsAGV() && AGVFullFlagOption)) ? !Is_EquipType_To_Port_PIO_CSFlag() : true) &&
                                    !Is_EquipType_To_Port_PIO_TR_ReqFlag() &&
                                    !Is_EquipType_To_Port_PIO_BusyFlag() &&
                                    !Is_EquipType_To_Port_PIO_Complete())
                                {
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step860_OutMode_Check_CST_Unload_And_Safe;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step860_OutMode_Check_CST_Unload_And_Safe:
                            {
                                if (Carrier_CheckShuttle_ExistProduct(false) &&
                                    !IsLPOppositeAngle() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    Carrier_ClearBP_CarrierID(0);
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step000_Check_Direction;
                                }
                            }
                            break;
                        case Shuttle_1BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload:
                            {
                                if (Is_CartDetct1_Check() || Is_CartDetct2_Check())
                                    Watchdog_ResetAndStop(WatchdogList.PortArea_Timer);

                                if (Carrier_CheckShuttle_ExistProduct(false) &&
                                    !IsLPOppositeAngle() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck() && 
                                    !Is_CartDetct1_Check() &&
                                    !Is_CartDetct2_Check() &&
                                    bAuto &&
                                    !Is_OHTDoorOpen_Check())
                                {
                                    Carrier_ClearBP_CarrierID(0);
                                    m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step000_Check_Direction;

                                }
                                else if(!Is_LightCurtain_or_Hoist_SensorCheck() &&
                                        !Is_CartDetct1_Check() &&
                                        !Is_CartDetct2_Check() &&
                                        !bAuto &&
                                        !Is_OHTDoorOpen_Check())
                                {
                                    if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                    {
                                        //카운트를 증가시키고
                                        m_CycleControlProgressCount++;
                                        //-----------Direction Change 실행
                                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step001_Change_Direction;
                                    }
                                    else
                                    {
                                        m_CycleControlProgressCount++;
                                        m_bAutoManualCycleStopReq = true;
                                        m_bCycleRunning = false;
                                    }
                                }
                            }
                            break;
                    }

                    Thread.Sleep(Master.StepUpdateTime);
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Start();
        }

        /// <summary>
        /// OP or Shuttle의 현재 스텝 텍스트
        /// </summary>
        /// <returns></returns>
        private string Auto_1BP_Get_OP_StepStr()
        {
            switch (m_eOP_1BP_AutoStep)
            {
                case OP_1BP_AutoStep.Step000_Check_Direction:
                    return $"Check Direction";
                case OP_1BP_AutoStep.Step100_InMode_Check_OP_CST_Load:
                case OP_1BP_AutoStep.Step600_OutMode_Check_OP_CST_Load:
                    return $"Check OP CST Load";
                case OP_1BP_AutoStep.Step200_InMode_Call_BP_Load_Req:
                    return $"Call BP(Load Req)";
                case OP_1BP_AutoStep.Step210_InMode_Check_BP_Location:
                    return $"Check BP Location";
                case OP_1BP_AutoStep.Step300_InMode_Await_PIO_TR_REQ:
                case OP_1BP_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                    return $"Await PIO-TR_REQ";
                case OP_1BP_AutoStep.Step310_InMode_Check_PIO_Busy:
                case OP_1BP_AutoStep.Step710_OutMode_Check_PIO_Busy:
                    return $"Check PIO-Busy";
                case OP_1BP_AutoStep.Step320_InMode_Check_PIO_Complete:
                case OP_1BP_AutoStep.Step720_OutMode_Check_PIO_Complete:
                    return $"Check PIO-Complete";
                case OP_1BP_AutoStep.Step330_InMode_Check_PIO_End:
                case OP_1BP_AutoStep.Step730_OutMode_Check_PIO_End:
                    return $"Check PIO-End off";
                case OP_1BP_AutoStep.Step340_InMode_Check_OP_CST_Unload_And_Safe:
                    return $"Check OP CST Unload and Safe";


                case OP_1BP_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe:
                    return $"Check OP CST Load and Safe";
                case OP_1BP_AutoStep.Step800_OutMode_Call_BP_Unload_Req:
                    return $"Call BP(Unload Req)";
                default:
                    return "Not def step str";
            }
        }
        private string Auto_1BP_Get_BP_StepStr()
        {
            switch (m_eShuttle_1BP_AutoStep)
            {
                case Shuttle_1BP_AutoStep.Step000_Check_Direction:
                    return $"Check Direction";
                case Shuttle_1BP_AutoStep.Step001_Change_Direction:
                    return $"Change Direction";


                case Shuttle_1BP_AutoStep.Step100_InMode_Check_BP_CST_Load_And_Pose:
                case Shuttle_1BP_AutoStep.Step600_OutMode_Check_BP_CST_Load_And_Pose:
                    return $"Check BP CST Load and Pose";
                case Shuttle_1BP_AutoStep.Step110_InMode_Move_Z_Down:
                case Shuttle_1BP_AutoStep.Step610_OutMode_Move_Z_Down:
                    return $"Move Z Down Pos(Init)";
                case Shuttle_1BP_AutoStep.Step120_InMode_Move_X_LP:
                case Shuttle_1BP_AutoStep.Step620_OutMode_Move_X_LP:
                    return $"Move X LP Pos(Init)";
                case Shuttle_1BP_AutoStep.Step130_InMode_Move_X_Wait:
                case Shuttle_1BP_AutoStep.Step630_OutMode_Move_X_Wait:
                    return $"Move X Wait Pos(Init)";
                case Shuttle_1BP_AutoStep.Step140_InMode_Move_Z_Up:
                case Shuttle_1BP_AutoStep.Step640_OutMode_Move_Z_Up:
                    return $"Move Z Up Pos(Init)";
                case Shuttle_1BP_AutoStep.Step150_InMode_Move_T_180_Deg:
                case Shuttle_1BP_AutoStep.Step650_OutMode_Move_T_180_Deg:
                    return $"Move T 180 Deg Pos(Init)";
                case Shuttle_1BP_AutoStep.Step160_InMode_Move_T_0_Deg:
                case Shuttle_1BP_AutoStep.Step660_OutMode_Move_T_0_Deg:
                    return $"Move T 0 Deg Pos(Init)";
                case Shuttle_1BP_AutoStep.Step170_InMode_Check_OperationType:
                case Shuttle_1BP_AutoStep.Step780_OutMode_Check_OperationType:
                    return $"Check Operation Type";

                case Shuttle_1BP_AutoStep.Step200_InMode_Await_PIO_CS:
                case Shuttle_1BP_AutoStep.Step800_OutMode_Await_PIO_CS:
                    return $"Await PIO-CS";
                case Shuttle_1BP_AutoStep.Step210_InMode_Check_PIO_Valid:
                case Shuttle_1BP_AutoStep.Step810_OutMode_Check_PIO_Valid:
                    return $"Check PIO-Valid";
                case Shuttle_1BP_AutoStep.Step220_InMode_Check_PIO_TR:
                case Shuttle_1BP_AutoStep.Step820_OutMode_Check_PIO_TR:
                    return $"Check PIO-TR";
                case Shuttle_1BP_AutoStep.Step230_InMode_Check_PIO_Busy:
                case Shuttle_1BP_AutoStep.Step830_OutMode_Check_PIO_Busy:
                    return $"Check PIO-Busy";
                case Shuttle_1BP_AutoStep.Step240_InMode_Check_PIO_Complete:
                case Shuttle_1BP_AutoStep.Step840_OutMode_Check_PIO_Complete:
                    return $"Check PIO-Complete";
                case Shuttle_1BP_AutoStep.Step250_InMode_Check_PIO_End:
                case Shuttle_1BP_AutoStep.Step850_OutMode_Check_PIO_End:
                    return $"Check PIO-End Off";

                case Shuttle_1BP_AutoStep.Step260_InMode_Check_CST_Load_And_Safe:
                    return $"Check BP CST Load and Safe";
                case Shuttle_1BP_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                    return $"Await MGV CST Load";
                case Shuttle_1BP_AutoStep.Step400_InMode_Ready_CST_ID_Read:
                    return $"Ready CST ID Read";
                case Shuttle_1BP_AutoStep.Step410_InMode_Start_CST_ID_Read:
                    return $"Start CST ID Read";
                case Shuttle_1BP_AutoStep.Step500_InMode_Await_OP_Load_Req:
                    return $"Await Load Req(OP)";
                case Shuttle_1BP_AutoStep.Step420_InMode_Move_Z_UP:
                    return $"Move Z Up Pos";
                case Shuttle_1BP_AutoStep.Step430_InMode_Move_X_Wait:
                    return $"Move X Wait Pos";
                case Shuttle_1BP_AutoStep.Step440_InMode_Move_T_0_Deg:
                    return $"Move T 0 Deg Pos";
                case Shuttle_1BP_AutoStep.Step510_InMode_Move_X_OP:
                    return $"Move X OP Pos (OP loading)";
                case Shuttle_1BP_AutoStep.Step520_InMode_Move_Z_Down:
                    return $"Move Z Down Pos (OP loading)";
                case Shuttle_1BP_AutoStep.Step530_InMode_Send_OP_CST_ID:
                    return $"Send OP CST ID (BP -> OP)";
                case Shuttle_1BP_AutoStep.Step540_InMode_Move_X_LP_or_Wait:
                    return $"Move X LP or Wait Pos (OP loading End)";
                case Shuttle_1BP_AutoStep.Step550_InMode_Move_T_180_Deg:
                    return $"Move T 180 Deg Pos(OP loading End)";
                case Shuttle_1BP_AutoStep.Step560_InMode_Move_X_LP:
                    return $"Move X LP Pos(OP loading End)";



                case Shuttle_1BP_AutoStep.Step700_OutMode_Await_OP_Unload_Req:
                    return $"Await Unload Req(OP)";
                case Shuttle_1BP_AutoStep.Step710_OutMode_Move_X_OP:
                    return $"Move X OP Pos (OP Unloading)";
                case Shuttle_1BP_AutoStep.Step720_OutMode_Move_Z_Up:
                    return $"Move Z Up Pos (OP Unloading)";
                case Shuttle_1BP_AutoStep.Step730_OutMode_Read_OP_CST_ID:
                    return $"Read OP CST ID (OP -> BP)";
                case Shuttle_1BP_AutoStep.Step740_OutMode_Move_X_LP_or_Wait:
                    return $"Move X LP or Wait Pos (OP Unloading End)";
                case Shuttle_1BP_AutoStep.Step750_OutMode_Move_T_180_Deg:
                    return $"Move T 180 Deg Pos (OP Unloading End)";
                case Shuttle_1BP_AutoStep.Step760_OutMode_Move_X_LP:
                    return $"Move X LP Pos (OP Unloading End)";
                case Shuttle_1BP_AutoStep.Step770_OutMode_Move_Z_Down:
                    return $"Move Z Down Pos (OP Unloading End)";
                

                case Shuttle_1BP_AutoStep.Step860_OutMode_Check_CST_Unload_And_Safe:
                    return $"Check BP CST Unload and Safe";
                case Shuttle_1BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload:
                    return $"Await MGV CST Unload";
                //case Shuttle_1BP_AutoStep.Step211_InMode_Call_AGV_Load_Req:
                //    return $"Call AGV CST Load Req";
                //case Shuttle_1BP_AutoStep.Step811_OutMode_Call_AGV_Unload_Req:
                //    return $"Call AGV CST Unload Req";
                default:
                    return "Not def step str";
            }
        }

        /// <summary>
        /// OP or Shuttle의 현재 스텝 번호
        /// </summary>
        /// <returns></returns>
        private int Auto_1BP_Get_OP_StepNum()
        {
            return (int)m_eOP_1BP_AutoStep;
        }
        private int Auto_1BP_Get_BP_StepNum()
        {
            return (int)m_eShuttle_1BP_AutoStep;
        }
    }
}
