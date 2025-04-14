using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Master.Interface.Alarm;
using System.Diagnostics;

namespace Master.Equipment.Port
{
    /// <summary>
    /// Port_2BP.cs는 Shuttle이 제어되는 공정에서 LP Buffer가 있는 형태의 제어 공정입니다.
    /// </summary>
    partial class Port
    {
        public enum LP_2BP_AutoStep
        {
            Step000_Check_Direction = 0,
            /// <summary>
            /// LP Input Mode
            /// 1. 자재가 없고 BP의 위치가 멀어지게 되면 Mode에 따라 Await 상태 진입 (AGv, OHT -> Await PIO  / MGV -> Await MGV)
            /// 2. 자재가 로드 된 경우 또는 처음부터 있었던 경우 ID Read Step(400) 진행
            /// 3. ID Read 완료 후 BP에 Unload 요청 및 대기
            /// 4. BP에서 제품 가져가는 경우 ID Clear
            /// </summary>
            Step100_InMode_Check_LP_CST_Load                = 100,      //Carrier 적재 상황 확인
            Step110_InMode_Check_BP_Location                = 110,      //없는 경우 셔틀 위치 확인 후 Await 진입

            Step200_InMode_Await_PIO_CS                     = 200,      //Carrier Load 요청 (AGV or OHT)
            Step210_InMode_Check_PIO_Valid                  = 210,
            //Step211_InMode_Call_AGV_Load_Req                = 211,
            Step220_InMode_Check_PIO_TR                     = 220,
            Step230_InMode_Check_PIO_Busy                   = 230,
            Step240_InMode_Check_PIO_Complete               = 240,
            Step250_InMode_Check_PIO_End                    = 250,
            Step260_InMode_Check_CST_Load_And_Safe          = 260,
            
            Step300_InMode_Await_MGV_CST_Load               = 300,      //Carrier Load 요청 (MGV)

            Step400_InMode_Call_BP_Unload_Req               = 400,      //Carrier Unload 요청 (셔틀)
            Step410_InMode_Clear_LP_CST_ID                  = 410,


            /// <summary>
            /// LP Output Mode
            /// 1. 자재가 없는 경우 BP에 Load 요청, 있는 경우 Mode에 따라 Await 상태 진입 (AGV, OHT -> Await PIO / MGV -> Await MGV)
            /// 2. 자재가 없다가 BP에서 Load된 경우 BP 위치가 멀어지게 되면 Mode에 따라 Await 상태 진입
            /// 3. 자재가 있는 경우 Await 상태로 출하 대기 및 출하시 ID Clear
            /// </summary>
            Step600_OutMode_Check_LP_CST_Load               = 600,      //Carrier 적재 상황 확인

            Step700_OutMode_Call_BP_Load_Req                = 710,      //Carrier Load 요청 (셔틀)
            Step710_OutMode_Check_BP_Location               = 720,      //적재 된 경우 셔틀 위치 확인

            Step800_OutMode_Await_PIO_CS                    = 800,      //Carrier Unload 대기 (AGV or OHT)
            Step810_OutMode_Check_PIO_Valid                 = 810,
            //Step811_OutMode_Call_AGV_Unload_Req             = 811,
            Step820_OutMode_Check_PIO_TR                    = 820,
            Step830_OutMode_Check_PIO_Busy                  = 830,
            Step840_OutMode_Check_PIO_Complete              = 840,
            Step850_OutMode_Check_PIO_End                   = 850,
            Step860_OutMode_Check_LP_CST_Unload_And_Safe    = 860,      //Carrier ID Clear

            Step900_OutMode_Await_MGV_CST_Unload            = 900,      //Carrier Unload 대기 (MGV), Carrier ID Clear
        }
        public enum OP_2BP_AutoStep
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
        public enum Shuttle_2BP_AutoStep
        {
            Step000_Check_Direction = 0,
            Step001_Change_Direction = 1,

            /// <summary>
            /// BP Input Mode
            /// 1. 100번대 영역 : BP 자세 확인 (시작시 셔틀에 CST 있는 경우는 무조건 에러)
            /// 2. 200번대 영역 : LP에서 제품 Unload 요구 오는 경우 제품 Loading Run
            /// 3. 300번대 영역 : OP에서 제품 Load 요구 오는 경우 제품 Unload Run
            /// </summary>
            Step100_InMode_Check_BP_CST_Load        = 100,      //로드 체크
            Step110_InMode_Check_BP_Pose            = 110,      //자세 확인
            Step120_InMode_Move_Z_Down              = 120,      //초기 자세 이니셜
            Step130_InMode_Move_X_WaitorLP          = 130,
            Step140_InMode_Move_T_180_Deg           = 140,

            Step200_InMode_Await_LP_Unload_Req      = 200,      //LP Unload 요청 체크(대기)
            Step210_InMode_Move_X_LP                = 210,      //제품 언로드 위치로 이동 및 BP 제품 로드
            Step211_InMode_Ready_CST_ID_Read        = 211,      //Carrier ID Read Step
            Step212_InMode_Start_CST_ID_Read        = 212,
            Step213_InMode_Await_OP_Load_Req        = 213,
            Step220_InMode_Move_Z_Up                = 220,
            Step230_InMode_Read_LP_CST_ID           = 230,      //ID 이전
            Step240_InMode_Move_X_WaitorLP          = 240,      //대기 위치로 이동
            Step250_InMode_Move_T_0_Deg             = 250,

            Step300_InMode_Await_OP_Load_Req        = 300,      //OP load 요청 체크(대기)
            Step310_InMode_Move_X_OP                = 310,      //제품 로드 위치로 이동 및 BP 제품 언로드
            Step320_InMode_Move_Z_Down              = 320,
            Step330_InMode_Send_OP_CST_ID           = 330,      //ID 이전
            Step340_InMode_Move_X_WaitorLP          = 340,      //초기 이니셜 자세로 이동
            Step350_InMode_Move_T_180_Deg           = 350,
            Step360_InMode_Move_Z_Down              = 360,


            /// <summary>
            /// BP Output Mode
            /// 1. 600번대 영역 : BP 자세 확인 (시작시 셔틀에 CST 있는 경우는 무조건 에러)
            /// 2. 700번대 영역 : OP에서 제품 Unload 요구 오는 경우 제품 Loading Run
            /// 3. 800번대 영역 : LP에서 제품 Load 요구 오는 경우 제품 Unload Run
            /// </summary>
            Step600_OutMode_Check_BP_CST_Load       = 600,      //로드 체크
            Step610_OutMode_Check_BP_Pose           = 610,      //자세 확인
            Step620_OutMode_Move_Z_Down             = 620,      //초기 자세 이니셜
            Step630_OutMode_Move_X_WaitorLP         = 630,
            Step640_OutMode_Move_T_0_Deg            = 640,
            
            Step700_OutMode_Await_OP_Unload_Req     = 700,      //OP Unload 요청 체크(대기)
            Step710_OutMode_Move_X_OP               = 710,      //제품 언로드 위치로 이동 및 BP 제품 로드
            Step711_OutMode_Await_LP_Load_Req       = 711,
            Step720_OutMode_Move_Z_Up               = 720,
            Step730_OutMode_Read_OP_CST_ID          = 730,      //ID 이전
            Step740_OutMode_Move_X_WaitorLP         = 740,      //대기 위치로 이동
            Step750_OutMode_Move_T_180_Deg          = 750,
            
            Step800_OutMode_Await_LP_Load_Req       = 800,      //LP Load 요청 체크(대기)
            Step810_OutMode_Move_X_LP               = 810,      //제품 로드 위치로 이동 및 BP 제품 언로드
            Step820_OutMode_Move_Z_Down             = 820,
            Step830_OutMode_Send_LP_CST_ID          = 830,      //ID 이전
            Step840_OutMode_Move_X_WaitorLP         = 840,      //초기 이니셜 자세로 이동
            Step850_OutMode_Move_T_0_Deg            = 850,
            Step860_OutMode_Move_Z_Down             = 860
        }

        /// <summary>
        /// LP 2BP Type In/Out Control
        /// </summary>
        LP_2BP_AutoStep m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step000_Check_Direction;
        OP_2BP_AutoStep m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step000_Check_Direction;
        Shuttle_2BP_AutoStep m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step000_Check_Direction;

        LP_2BP_AutoStep Pre_LP_2BP_AutoStep = LP_2BP_AutoStep.Step000_Check_Direction;
        OP_2BP_AutoStep Pre_OP_2BP_AutoStep = OP_2BP_AutoStep.Step000_Check_Direction;
        Shuttle_2BP_AutoStep Pre_Shuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step000_Check_Direction;

        bool m_bLP_2BP_AutoStopEnable = false;
        bool m_bOP_2BP_AutoStopEnable = false;
        bool m_bShuttle_2BP_AutoStopEnable = false;

        //Auto
        private bool m_bLP_2BP_AutoRunning = false;
        private bool m_bOP_2BP_AutoRunning = false;
        private bool m_bShuttle_2BP_AutoRunning = false;

        /// <summary>
        /// LP or OP or Shuttle의 정지 가능 상태를 나타냄
        /// Call, Await Step에서 대부분 정지 가능(요청 또는 대기 상태)
        /// </summary>
        private void Auto_2BP_LPStopEnableUpdate()
        {
            switch (m_eLP_2BP_AutoStep)
            {
                case LP_2BP_AutoStep.Step000_Check_Direction:
                case LP_2BP_AutoStep.Step100_InMode_Check_LP_CST_Load:
                case LP_2BP_AutoStep.Step200_InMode_Await_PIO_CS:
                //case LP_2BP_AutoStep.Step211_InMode_Call_AGV_Load_Req:
                case LP_2BP_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                case LP_2BP_AutoStep.Step400_InMode_Call_BP_Unload_Req:
                case LP_2BP_AutoStep.Step600_OutMode_Check_LP_CST_Load:
                case LP_2BP_AutoStep.Step700_OutMode_Call_BP_Load_Req:
                case LP_2BP_AutoStep.Step800_OutMode_Await_PIO_CS:
                //case LP_2BP_AutoStep.Step811_OutMode_Call_AGV_Unload_Req:
                case LP_2BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload:
                    m_bLP_2BP_AutoStopEnable = true;
                    break;
                case LP_2BP_AutoStep.Step210_InMode_Check_PIO_Valid:
                case LP_2BP_AutoStep.Step810_OutMode_Check_PIO_Valid:
                    {
                        if (AGVFullFlagOption)
                        {
                            m_bLP_2BP_AutoStopEnable = false;
                        }
                        else
                        {
                            m_bLP_2BP_AutoStopEnable = true;
                        }
                    }
                    break;
                default:
                    m_bLP_2BP_AutoStopEnable = false;
                    break;
            }
        }
        private void Auto_2BP_OPStopEnableUpdate()
        {
            switch (m_eOP_2BP_AutoStep)
            {
                case OP_2BP_AutoStep.Step000_Check_Direction:
                //case OP_2BP_AutoStep.Step100_InMode_Check_OP_CST_Load:
                case OP_2BP_AutoStep.Step200_InMode_Call_BP_Load_Req:
                case OP_2BP_AutoStep.Step300_InMode_Await_PIO_TR_REQ:
                //case OP_2BP_AutoStep.Step600_OutMode_Check_OP_CST_Load:
                case OP_2BP_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                case OP_2BP_AutoStep.Step800_OutMode_Call_BP_Unload_Req:
                    m_bOP_2BP_AutoStopEnable = true;
                    break;
                default:
                    m_bOP_2BP_AutoStopEnable = false;
                    break;
            }
        }
        private void Auto_2BP_BPStopEnableUpdate()
        {
            switch (m_eShuttle_2BP_AutoStep)
            {
                case Shuttle_2BP_AutoStep.Step000_Check_Direction:
                case Shuttle_2BP_AutoStep.Step001_Change_Direction:
                case Shuttle_2BP_AutoStep.Step100_InMode_Check_BP_CST_Load:
                case Shuttle_2BP_AutoStep.Step110_InMode_Check_BP_Pose:
                case Shuttle_2BP_AutoStep.Step200_InMode_Await_LP_Unload_Req:
                case Shuttle_2BP_AutoStep.Step213_InMode_Await_OP_Load_Req:
                case Shuttle_2BP_AutoStep.Step300_InMode_Await_OP_Load_Req:
                case Shuttle_2BP_AutoStep.Step600_OutMode_Check_BP_CST_Load:
                case Shuttle_2BP_AutoStep.Step610_OutMode_Check_BP_Pose:
                case Shuttle_2BP_AutoStep.Step700_OutMode_Await_OP_Unload_Req:
                case Shuttle_2BP_AutoStep.Step711_OutMode_Await_LP_Load_Req:
                case Shuttle_2BP_AutoStep.Step800_OutMode_Await_LP_Load_Req:
                    m_bShuttle_2BP_AutoStopEnable = true;
                    break;
                default:
                    {
                        m_bShuttle_2BP_AutoStopEnable = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// 자재가 있어야 할 스텝, 없어야 할 스텝을 구분하여 자재 판단
        /// </summary>
        private void Auto_2BP_LP_Placement_Detect_AlarmCheck()
        {
            switch (m_eLP_2BP_AutoStep)
            {
                //없어야 함
                case LP_2BP_AutoStep.Step110_InMode_Check_BP_Location:
                case LP_2BP_AutoStep.Step200_InMode_Await_PIO_CS:
                case LP_2BP_AutoStep.Step210_InMode_Check_PIO_Valid:
                //case LP_2BP_AutoStep.Step211_InMode_Call_AGV_Load_Req:
                case LP_2BP_AutoStep.Step220_InMode_Check_PIO_TR:
                case LP_2BP_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                case LP_2BP_AutoStep.Step410_InMode_Clear_LP_CST_ID:
                case LP_2BP_AutoStep.Step710_OutMode_Check_BP_Location:
                case LP_2BP_AutoStep.Step850_OutMode_Check_PIO_End:
                case LP_2BP_AutoStep.Step860_OutMode_Check_LP_CST_Unload_And_Safe:
                    {
                        if (Carrier_CheckLP_ExistProduct(true, false))
                        {
                            Watchdog_Start(WatchdogList.LP_Placement_ErrorTimer);
                            if(Watchdog_IsDetect(WatchdogList.LP_Placement_ErrorTimer))
                                AlarmInsert((short)PortAlarm.LP_Placement_Detect_Error, AlarmLevel.Error);
                        }
                        else
                            Watchdog_Stop(WatchdogList.LP_Placement_ErrorTimer, true);
                    }
                    break;

                //있어야 함
                case LP_2BP_AutoStep.Step250_InMode_Check_PIO_End:
                case LP_2BP_AutoStep.Step260_InMode_Check_CST_Load_And_Safe:
                case LP_2BP_AutoStep.Step800_OutMode_Await_PIO_CS:
                //case LP_2BP_AutoStep.Step811_OutMode_Call_AGV_Unload_Req:
                case LP_2BP_AutoStep.Step810_OutMode_Check_PIO_Valid:
                case LP_2BP_AutoStep.Step820_OutMode_Check_PIO_TR:
                case LP_2BP_AutoStep.Step830_OutMode_Check_PIO_Busy:
                    {
                        if (Carrier_CheckLP_ExistProduct(false, false))
                        {
                            Watchdog_Start(WatchdogList.LP_Placement_ErrorTimer);
                            if (Watchdog_IsDetect(WatchdogList.LP_Placement_ErrorTimer))
                                AlarmInsert((short)PortAlarm.LP_Placement_Detect_Error, AlarmLevel.Error);
                        }
                        else
                            Watchdog_Stop(WatchdogList.LP_Placement_ErrorTimer, true);
                    }
                    break;
                default:
                    Watchdog_Stop(WatchdogList.LP_Placement_ErrorTimer, true);
                    break;
            }
        }
        private void Auto_2BP_OP_Placement_Detect_AlarmCheck()
        {
            switch (m_eOP_2BP_AutoStep)
            {
                //있어야 함
                case OP_2BP_AutoStep.Step210_InMode_Check_BP_Location:
                case OP_2BP_AutoStep.Step300_InMode_Await_PIO_TR_REQ:
                case OP_2BP_AutoStep.Step310_InMode_Check_PIO_Busy:
                case OP_2BP_AutoStep.Step730_OutMode_Check_PIO_End:
                case OP_2BP_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe:
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
                case OP_2BP_AutoStep.Step330_InMode_Check_PIO_End:
                case OP_2BP_AutoStep.Step340_InMode_Check_OP_CST_Unload_And_Safe:
                case OP_2BP_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                case OP_2BP_AutoStep.Step710_OutMode_Check_PIO_Busy:
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
        private void Auto_2BP_BP_Placement_Detect_AlarmCheck()
        {
            switch (m_eShuttle_2BP_AutoStep)
            {
                ///없어야 함
                case Shuttle_2BP_AutoStep.Step100_InMode_Check_BP_CST_Load:
                case Shuttle_2BP_AutoStep.Step110_InMode_Check_BP_Pose:
                case Shuttle_2BP_AutoStep.Step120_InMode_Move_Z_Down:
                case Shuttle_2BP_AutoStep.Step130_InMode_Move_X_WaitorLP:
                case Shuttle_2BP_AutoStep.Step140_InMode_Move_T_180_Deg:
                case Shuttle_2BP_AutoStep.Step200_InMode_Await_LP_Unload_Req:
                case Shuttle_2BP_AutoStep.Step210_InMode_Move_X_LP:
                case Shuttle_2BP_AutoStep.Step213_InMode_Await_OP_Load_Req: //뜨기 직전
                case Shuttle_2BP_AutoStep.Step330_InMode_Send_OP_CST_ID:
                case Shuttle_2BP_AutoStep.Step340_InMode_Move_X_WaitorLP:
                case Shuttle_2BP_AutoStep.Step350_InMode_Move_T_180_Deg:
                case Shuttle_2BP_AutoStep.Step360_InMode_Move_Z_Down:
                case Shuttle_2BP_AutoStep.Step600_OutMode_Check_BP_CST_Load:
                case Shuttle_2BP_AutoStep.Step610_OutMode_Check_BP_Pose:
                case Shuttle_2BP_AutoStep.Step620_OutMode_Move_Z_Down:
                case Shuttle_2BP_AutoStep.Step630_OutMode_Move_X_WaitorLP:
                case Shuttle_2BP_AutoStep.Step640_OutMode_Move_T_0_Deg:
                case Shuttle_2BP_AutoStep.Step700_OutMode_Await_OP_Unload_Req:
                case Shuttle_2BP_AutoStep.Step710_OutMode_Move_X_OP:
                case Shuttle_2BP_AutoStep.Step711_OutMode_Await_LP_Load_Req:
                case Shuttle_2BP_AutoStep.Step830_OutMode_Send_LP_CST_ID:
                case Shuttle_2BP_AutoStep.Step840_OutMode_Move_X_WaitorLP:
                case Shuttle_2BP_AutoStep.Step850_OutMode_Move_T_0_Deg:
                case Shuttle_2BP_AutoStep.Step860_OutMode_Move_Z_Down:
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
                case Shuttle_2BP_AutoStep.Step230_InMode_Read_LP_CST_ID:
                case Shuttle_2BP_AutoStep.Step240_InMode_Move_X_WaitorLP:
                case Shuttle_2BP_AutoStep.Step250_InMode_Move_T_0_Deg:
                case Shuttle_2BP_AutoStep.Step300_InMode_Await_OP_Load_Req:
                case Shuttle_2BP_AutoStep.Step310_InMode_Move_X_OP:
                case Shuttle_2BP_AutoStep.Step730_OutMode_Read_OP_CST_ID:
                case Shuttle_2BP_AutoStep.Step740_OutMode_Move_X_WaitorLP:
                case Shuttle_2BP_AutoStep.Step750_OutMode_Move_T_180_Deg:
                case Shuttle_2BP_AutoStep.Step800_OutMode_Await_LP_Load_Req:
                case Shuttle_2BP_AutoStep.Step810_OutMode_Move_X_LP:
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
                    Watchdog_Stop(WatchdogList.BP_Placement_ErrorTimer, true);
                    break;

            }
        }

        /// <summary>
        /// 대기 중인 스텝을 제외하고 모션 중인 스텝에서 Time Out 확인
        /// </summary>
        private void Auto_2BP_LP_StepTimeOut_AlarmCheck()
        {
            if (m_eLP_2BP_AutoStep == Pre_LP_2BP_AutoStep && 
                !m_bLP_2BP_AutoStopEnable)
            {
                Watchdog_Start(WatchdogList.LP_Step_Timer);
                if (Watchdog_IsDetect(WatchdogList.LP_Step_Timer))
                    AlarmInsert((short)PortAlarm.Step_TimeOver_Error, AlarmLevel.Error);
            }
            else
            {
                Watchdog_Stop(WatchdogList.LP_Step_Timer, true);
            }

            if(m_eLP_2BP_AutoStep != Pre_LP_2BP_AutoStep)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortStepInfo, $"2BP LP Step : {Pre_LP_2BP_AutoStep} => {m_eLP_2BP_AutoStep}");

            Pre_LP_2BP_AutoStep = m_eLP_2BP_AutoStep;
        }
        private void Auto_2BP_OP_StepTimeOut_AlarmCheck()
        {
            if (m_eOP_2BP_AutoStep == Pre_OP_2BP_AutoStep &&
                !m_bOP_2BP_AutoStopEnable)
            {
                Watchdog_Start(WatchdogList.OP_Step_Timer);
                if (Watchdog_IsDetect(WatchdogList.OP_Step_Timer))
                    AlarmInsert((short)PortAlarm.Step_TimeOver_Error, AlarmLevel.Error);
            }
            else
            {
                Watchdog_Stop(WatchdogList.OP_Step_Timer, true);
            }

            if(m_eOP_2BP_AutoStep != Pre_OP_2BP_AutoStep)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortStepInfo, $"2BP OP Step : {Pre_OP_2BP_AutoStep} => {m_eOP_2BP_AutoStep}");

            Pre_OP_2BP_AutoStep = m_eOP_2BP_AutoStep;
        }
        private void Auto_2BP_BP_StepTimeOut_AlarmCheck()
        {
            if (m_eShuttle_2BP_AutoStep == Pre_Shuttle_2BP_AutoStep &&
                !m_bShuttle_2BP_AutoStopEnable)
            {
                Watchdog_Start(WatchdogList.BP_Step_Timer);
                if (Watchdog_IsDetect(WatchdogList.BP_Step_Timer))
                    AlarmInsert((short)PortAlarm.Step_TimeOver_Error, AlarmLevel.Error);
            }
            else
            {
                Watchdog_Stop(WatchdogList.BP_Step_Timer, true);
            }

            if(m_eShuttle_2BP_AutoStep != Pre_Shuttle_2BP_AutoStep)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortStepInfo, $"2BP Shuttle Step : {Pre_Shuttle_2BP_AutoStep} => {m_eShuttle_2BP_AutoStep}");

            Pre_Shuttle_2BP_AutoStep = m_eShuttle_2BP_AutoStep;
        }

        /// <summary>
        /// 장비 및 STK의 PIO 과정에서 Timeout 확인
        /// </summary>
        private void Auto_2BP_AGVorOHT_PIO_WatchdogUpdate()
        {
            if (GetPortOperationMode() == PortOperationMode.OHT)
            {
                if ((m_eLP_2BP_AutoStep >= LP_2BP_AutoStep.Step810_OutMode_Check_PIO_Valid && m_eLP_2BP_AutoStep <= LP_2BP_AutoStep.Step850_OutMode_Check_PIO_End) ||
                    (m_eLP_2BP_AutoStep >= LP_2BP_AutoStep.Step210_InMode_Check_PIO_Valid && m_eLP_2BP_AutoStep <= LP_2BP_AutoStep.Step250_InMode_Check_PIO_End))
                {
                    Watchdog_Start(WatchdogList.AGVorOHT_PIO_Timer);
                }
                else
                {
                    Watchdog_Stop(WatchdogList.AGVorOHT_PIO_Timer, true);
                }
            }
            else if(GetPortOperationMode() == PortOperationMode.AGV)
            {
                if ((m_eLP_2BP_AutoStep >= LP_2BP_AutoStep.Step820_OutMode_Check_PIO_TR && m_eLP_2BP_AutoStep <= LP_2BP_AutoStep.Step850_OutMode_Check_PIO_End) ||
                    (m_eLP_2BP_AutoStep >= LP_2BP_AutoStep.Step220_InMode_Check_PIO_TR && m_eLP_2BP_AutoStep <= LP_2BP_AutoStep.Step250_InMode_Check_PIO_End))
                {
                    Watchdog_Start(WatchdogList.AGVorOHT_PIO_Timer);
                }
                else
                {
                    Watchdog_Stop(WatchdogList.AGVorOHT_PIO_Timer, true);
                }
            }
        }
        private void Auto_2BP_RackMaster_PIO_WatchdogUpdate()
        {
            if ((m_eOP_2BP_AutoStep >= OP_2BP_AutoStep.Step710_OutMode_Check_PIO_Busy && m_eOP_2BP_AutoStep <= OP_2BP_AutoStep.Step730_OutMode_Check_PIO_End) ||
                (m_eOP_2BP_AutoStep >= OP_2BP_AutoStep.Step310_InMode_Check_PIO_Busy && m_eOP_2BP_AutoStep <= OP_2BP_AutoStep.Step330_InMode_Check_PIO_End))
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
        private void Auto_2BP_LP_StopInit()
        {
            m_bLP_2BP_AutoRunning = false;
            Port_To_AGV_PIO_Init();
            Port_To_OHT_PIO_Init();
            Watchdog_Stop(WatchdogList.LP_Placement_ErrorTimer, false);
            Watchdog_Stop(WatchdogList.AGVorOHT_PIO_Timer, false);
        }
        private void Auto_2BP_OP_StopInit()
        {
            m_bOP_2BP_AutoRunning = false;
            Port_To_RM_PIO_Init();
            Watchdog_Stop(WatchdogList.OP_Placement_ErrorTimer, false);
            Watchdog_Stop(WatchdogList.RackMaster_PIO_Timer, true);
        }
        private void Auto_2BP_BP_StopInit()
        {
            m_bShuttle_2BP_AutoRunning = false;
            m_bCycleRunning = false;
            Watchdog_Stop(WatchdogList.BP_Placement_ErrorTimer, false);

            CMD_PortStop();
        }

        /// <summary>
        /// 실제 오토 공정 함수
        /// </summary>
        private void Auto_2BP_Start_LP_Control()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                while (m_bLP_2BP_AutoRunning)
                {
                    //1. Port가 알람 상태인 경우 정지
                    if (GetAlarmLevel() == AlarmLevel.Error)
                    {
                        Auto_2BP_LP_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAlarmStop, $"LP 2BP");
                        break;
                    }

                    //2. Step에 연관된 알람 상황 체크
                    Auto_2BP_LP_Placement_Detect_AlarmCheck();
                    Auto_2BP_LP_StepTimeOut_AlarmCheck();

                    //3. Step에 연관된 상태 업데이트
                    Auto_2BP_LPStopEnableUpdate();
                    Auto_2BP_AGVorOHT_PIO_WatchdogUpdate();

                    //4. 정지 명령 시 중단
                    if (IsAutoStopReq(true))
                    {
                        if(m_bLP_2BP_AutoStopEnable)
                        {
                            Auto_2BP_LP_StopInit();
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AutoControlStopCommand, $"LP 2BP");
                            break;
                        }
                    }

                    //5. 동작 수행
                    switch (m_eLP_2BP_AutoStep)
                    {
                        case LP_2BP_AutoStep.Step000_Check_Direction:
                            {
                                Port_To_AGV_PIO_Init();
                                Port_To_OHT_PIO_Init();

                                if (GetOperationDirection() == PortDirection.Input)
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step100_InMode_Check_LP_CST_Load;
                                else
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step600_OutMode_Check_LP_CST_Load;
                            }
                            break;
                        case LP_2BP_AutoStep.Step100_InMode_Check_LP_CST_Load:
                            {
                                if (Carrier_CheckLP_ExistProduct(true)) //&& Carrier_CheckLP_ExistID()
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step400_InMode_Call_BP_Unload_Req;
                                else if (Carrier_CheckLP_ExistProduct(false) && !Carrier_CheckLP_ExistID())
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step110_InMode_Check_BP_Location;
                                //else if (Carrier_CheckLP_ExistProduct(true) && !Carrier_CheckLP_ExistID())
                                //{
                                //    //20230417 : ID Reading 변경으로 인한 스텝변경
                                //    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step400_InMode_Call_BP_Unload_Req;
                                //}
                                else if (Carrier_CheckLP_ExistProduct(false) && Carrier_CheckLP_ExistID())
                                {
                                    LP_CarrierID = string.Empty;
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step110_InMode_Check_BP_Location:
                            {
                                if (m_eShuttle_2BP_AutoStep <= Shuttle_2BP_AutoStep.Step200_InMode_Await_LP_Unload_Req ||
                                    m_eShuttle_2BP_AutoStep >= Shuttle_2BP_AutoStep.Step300_InMode_Await_OP_Load_Req)
                                {
                                    if (IsOHT())
                                        m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step200_InMode_Await_PIO_CS;
                                    else if(IsAGV())
                                        m_eLP_2BP_AutoStep = AGVFullFlagOption ? LP_2BP_AutoStep.Step200_InMode_Await_PIO_CS : LP_2BP_AutoStep.Step210_InMode_Check_PIO_Valid;
                                    else if (IsMGV())
                                        m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step300_InMode_Await_MGV_CST_Load;
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step200_InMode_Await_PIO_CS:
                            {
                                if (Is_EquipType_To_Port_PIO_CSFlag() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step210_InMode_Check_PIO_Valid;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                                else if (Carrier_CheckLP_ExistProduct(true))
                                {
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step250_InMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step210_InMode_Check_PIO_Valid:
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
                                    //Set_Port_To_Equip_PIO_ESFlag(true);
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step220_InMode_Check_PIO_TR;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                                else if (Carrier_CheckLP_ExistProduct(true))
                                {
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step250_InMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        //case LP_2BP_AutoStep.Step211_InMode_Call_AGV_Load_Req:
                        //    {
                        //        if (Carrier_CheckLP_ExistProduct(false) &&
                        //            !Is_LightCurtain_or_Hoist_SensorCheck())
                        //        {
                        //            Set_Port_To_AGVorOHT_PIO_Load_ReqFlag(true);

                        //            if(Is_AGVorOHT_To_Port_PIO_TR_ReqFlag())
                        //            {
                        //                m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step220_InMode_Check_PIO_TR;
                        //                Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                        //            }
                        //        }
                        //        else if (Carrier_CheckLP_ExistProduct(true))
                        //        {
                        //            Set_Port_To_AGVorOHT_PIO_Load_ReqFlag(false);
                        //            m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step250_InMode_Check_PIO_End;
                        //            Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                        //        }
                        //        break;
                        //    }
                        case LP_2BP_AutoStep.Step220_InMode_Check_PIO_TR:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if(!Is_EquipType_To_Port_PIO_TR_ReqFlag())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (IsAGV())
                                {
                                    Watchdog_ResetAndStop(WatchdogList.PortArea_Timer);
                                }

                                if (Is_EquipType_To_Port_PIO_TR_ReqFlag())
                                {
                                    Set_Port_To_Equip_PIO_ReadyFlag(true);
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step230_InMode_Check_PIO_Busy;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step230_InMode_Check_PIO_Busy:
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
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step240_InMode_Check_PIO_Complete;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step240_InMode_Check_PIO_Complete:
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
                                if (Carrier_CheckLP_ExistProduct(true))
                                    Set_Port_To_Equip_PIO_Load_ReqFlag(false);

                                if (!Is_EquipType_To_Port_PIO_TR_ReqFlag() &&
                                    !Is_EquipType_To_Port_PIO_BusyFlag() &&
                                    Is_EquipType_To_Port_PIO_Complete() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck() &&
                                    !Is_Port_To_Equip_PIO_Load_ReqFlag())
                                {
                                    Set_Port_To_Equip_PIO_ReadyFlag(false);
                                    //Set_Port_To_Equip_PIO_ESFlag(false);
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step250_InMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step250_InMode_Check_PIO_End:
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
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step260_InMode_Check_CST_Load_And_Safe;
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step260_InMode_Check_CST_Load_And_Safe:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) && !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step400_InMode_Call_BP_Unload_Req; //LP_2BP_AutoStep.Step400_InMode_Ready_CST_ID_Read;
                                }
                                else if(!Carrier_CheckLP_ExistProduct(true) && !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    //Abnormal Case (PIO 이후 적재 없거나 이상한 상황)
                                    AlarmInsert((short)PortAlarm.LP_Placement_Detect_Error, AlarmLevel.Error);
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                            {
                                if (Is_CartDetct1_Check() || Is_CartDetct2_Check())
                                    Watchdog_ResetAndStop(WatchdogList.PortArea_Timer);

                                if (Carrier_CheckLP_ExistProduct(true) && !Is_LightCurtain_or_Hoist_SensorCheck() &&
                                    !Is_CartDetct1_Check() &&
                                    !Is_CartDetct2_Check() &&
                                    !Is_OHTDoorOpen_Check())
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step400_InMode_Call_BP_Unload_Req;//LP_2BP_AutoStep.Step400_InMode_Ready_CST_ID_Read;
                            }
                            break;
                        case LP_2BP_AutoStep.Step400_InMode_Call_BP_Unload_Req:
                            {
                                if (Carrier_CheckLP_ExistProduct(false) && IsPortAxisBusy(PortAxis.Shuttle_X))
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step410_InMode_Clear_LP_CST_ID;
                            }
                            break;
                        case LP_2BP_AutoStep.Step410_InMode_Clear_LP_CST_ID:
                            {
                                if (Carrier_CheckLP_ExistProduct(false))
                                {
                                    if (Carrier_CheckLP_ExistID())
                                        LP_CarrierID = string.Empty;

                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step000_Check_Direction;
                                }
                            }
                            break;

                        case LP_2BP_AutoStep.Step600_OutMode_Check_LP_CST_Load:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) && Carrier_CheckLP_ExistID())
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step710_OutMode_Check_BP_Location;
                                else if (Carrier_CheckLP_ExistProduct(false) && !Carrier_CheckLP_ExistID())
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step700_OutMode_Call_BP_Load_Req;
                                else if (Carrier_CheckLP_ExistProduct(true) && !Carrier_CheckLP_ExistID())
                                {
                                    //if (m_bTestMode)
                                    //    LP_CarrierID = "TestID";
                                    //else
                                        AlarmInsert((short)PortAlarm.LP_No_Cassette_ID_Error, AlarmLevel.Error);
                                }
                                else if (Carrier_CheckLP_ExistProduct(false) && Carrier_CheckLP_ExistID())
                                {
                                    LP_CarrierID = string.Empty;
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step700_OutMode_Call_BP_Load_Req:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) && Carrier_CheckLP_ExistID())
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step710_OutMode_Check_BP_Location;
                            }
                            break;
                        case LP_2BP_AutoStep.Step710_OutMode_Check_BP_Location:
                            {
                                if (m_eShuttle_2BP_AutoStep < Shuttle_2BP_AutoStep.Step800_OutMode_Await_LP_Load_Req ||
                                    m_eShuttle_2BP_AutoStep > Shuttle_2BP_AutoStep.Step830_OutMode_Send_LP_CST_ID)
                                {
                                    if (IsOHT())
                                        m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step800_OutMode_Await_PIO_CS;
                                    else if(IsAGV())
                                        m_eLP_2BP_AutoStep = AGVFullFlagOption ? LP_2BP_AutoStep.Step800_OutMode_Await_PIO_CS : LP_2BP_AutoStep.Step810_OutMode_Check_PIO_Valid;
                                    else if (IsMGV())
                                        m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload;
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step800_OutMode_Await_PIO_CS:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) &&
                                    Is_EquipType_To_Port_PIO_CSFlag())
                                {
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step810_OutMode_Check_PIO_Valid;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                                else if (Carrier_CheckLP_ExistProduct(false))
                                {
                                    //PIO 대기 중 제품이 사라진 경우 ID를 삭제 후 다시 첫 스텝으로 회귀
                                    if (Carrier_CheckLP_ExistID())
                                        LP_CarrierID = string.Empty;

                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step000_Check_Direction;
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step810_OutMode_Check_PIO_Valid:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (!Is_EquipType_To_Port_PIO_ValidFlag() || !Is_EquipType_To_Port_PIO_CSFlag())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (Carrier_CheckLP_ExistProduct(true) &&
                                     ((IsOHT() || (IsAGV() && AGVFullFlagOption)) ? Is_EquipType_To_Port_PIO_CSFlag() : true) &&
                                    Is_EquipType_To_Port_PIO_ValidFlag() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    Set_Port_To_Equip_PIO_Unload_ReqFlag(true);
                                    //Set_Port_To_Equip_PIO_ESFlag(true);
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step820_OutMode_Check_PIO_TR;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                                else if (Carrier_CheckLP_ExistProduct(false))
                                {
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step850_OutMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        //case LP_2BP_AutoStep.Step811_OutMode_Call_AGV_Unload_Req:
                        //    {
                        //        if (Carrier_CheckLP_ExistProduct(true) &&
                        //            !Is_LightCurtain_or_Hoist_SensorCheck())
                        //        {
                        //            Set_Port_To_AGVorOHT_PIO_Unload_ReqFlag(true);

                        //            if(Is_AGVorOHT_To_Port_PIO_TR_ReqFlag())
                        //            {
                        //                m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step820_OutMode_Check_PIO_TR;
                        //                Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                        //            }
                        //        }
                        //        else if (Carrier_CheckLP_ExistProduct(false))
                        //        {
                        //            Set_Port_To_AGVorOHT_PIO_Unload_ReqFlag(false);
                        //            m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step850_OutMode_Check_PIO_End;
                        //            Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                        //        }
                        //    }
                        //    break;
                        case LP_2BP_AutoStep.Step820_OutMode_Check_PIO_TR:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (!Is_EquipType_To_Port_PIO_ValidFlag() || !Is_EquipType_To_Port_PIO_CSFlag())
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (IsAGV())
                                {
                                    Watchdog_ResetAndStop(WatchdogList.PortArea_Timer);
                                }

                                if (Is_EquipType_To_Port_PIO_TR_ReqFlag())
                                {
                                    Set_Port_To_Equip_PIO_ReadyFlag(true);
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step830_OutMode_Check_PIO_Busy;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step830_OutMode_Check_PIO_Busy:
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
                                    Carrier_CheckLP_ExistProduct(true))
                                {
                                    //Set_Port_To_Equip_PIO_Unload_ReqFlag(false);
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step840_OutMode_Check_PIO_Complete;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step840_OutMode_Check_PIO_Complete:
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
                                if(Carrier_CheckLP_ExistProduct(false))
                                    Set_Port_To_Equip_PIO_Unload_ReqFlag(false);

                                if (Carrier_CheckLP_ExistProduct(false, false))
                                    LP_CarrierID = string.Empty;

                                if (!Is_EquipType_To_Port_PIO_TR_ReqFlag() &&
                                    !Is_EquipType_To_Port_PIO_BusyFlag() &&
                                    Is_EquipType_To_Port_PIO_Complete() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck() &&
                                    !Is_Port_To_Equip_PIO_Unload_ReqFlag())
                                {
                                    Set_Port_To_Equip_PIO_ReadyFlag(false);
                                    //Set_Port_To_Equip_PIO_ESFlag(false);
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step850_OutMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step850_OutMode_Check_PIO_End:
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
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step860_OutMode_Check_LP_CST_Unload_And_Safe;
                                }
                            }
                            break;
                        case LP_2BP_AutoStep.Step860_OutMode_Check_LP_CST_Unload_And_Safe:
                        case LP_2BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload:
                            {
                                if (Is_CartDetct1_Check() || Is_CartDetct2_Check())
                                    Watchdog_ResetAndStop(WatchdogList.PortArea_Timer);

                                if (Carrier_CheckLP_ExistProduct(false) && !Is_LightCurtain_or_Hoist_SensorCheck() &&
                                !Is_CartDetct1_Check() &&
                                !Is_CartDetct2_Check()&&
                                !Is_OHTDoorOpen_Check())
                                {
                                    LP_CarrierID = string.Empty;
                                    m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step000_Check_Direction;
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
        private void Auto_2BP_Start_OP_Control()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                while (m_bOP_2BP_AutoRunning)
                {
                    //1. Port가 알람 상태인 경우 정지
                    if (GetAlarmLevel() == AlarmLevel.Error)
                    {
                        Auto_2BP_OP_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAlarmStop, $"OP 2BP");
                        break;
                    }

                    //2. Step에 연관된 알람 상황 체크
                    Auto_2BP_OP_Placement_Detect_AlarmCheck();
                    Auto_2BP_OP_StepTimeOut_AlarmCheck();

                    //3. Step에 연관된 상태 업데이트
                    Auto_2BP_OPStopEnableUpdate();
                    Auto_2BP_RackMaster_PIO_WatchdogUpdate();

                    //4. 정지 명령 시 중단
                    if (IsAutoStopReq(true))
                    {
                        if (m_bOP_2BP_AutoStopEnable)
                        {
                            Auto_2BP_OP_StopInit();
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AutoControlStopCommand, $"OP 2BP");
                            break;
                        }
                    }

                    //5. 동작 수행
                    switch (m_eOP_2BP_AutoStep)
                    {
                        case OP_2BP_AutoStep.Step000_Check_Direction:
                            {
                                Port_To_RM_PIO_Init();

                                if (GetOperationDirection() == PortDirection.Input)
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step100_InMode_Check_OP_CST_Load;
                                else
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step600_OutMode_Check_OP_CST_Load;
                            }
                            break;
                        case OP_2BP_AutoStep.Step100_InMode_Check_OP_CST_Load:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) && Carrier_CheckOP_ExistID())
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step210_InMode_Check_BP_Location;
                                else if (Carrier_CheckOP_ExistProduct(false) && !Carrier_CheckOP_ExistID())
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step200_InMode_Call_BP_Load_Req;
                                else if (Carrier_CheckOP_ExistProduct(true) && !Carrier_CheckOP_ExistID())
                                {
                                    //if (m_bTestMode)
                                    //    OP_CarrierID = "TestID";
                                    //else
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
                        case OP_2BP_AutoStep.Step200_InMode_Call_BP_Load_Req:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) && Carrier_CheckOP_ExistID())
                                {
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step210_InMode_Check_BP_Location;
                                }
                            }
                            break;
                        case OP_2BP_AutoStep.Step210_InMode_Check_BP_Location:
                            {
                                if (m_eShuttle_2BP_AutoStep >= Shuttle_2BP_AutoStep.Step200_InMode_Await_LP_Unload_Req &&
                                    m_eShuttle_2BP_AutoStep <= Shuttle_2BP_AutoStep.Step300_InMode_Await_OP_Load_Req && 
                                    IsXAxisPos_WaitorLP())
                                {
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step300_InMode_Await_PIO_TR_REQ;
                                }
                            }
                            break;
                        case OP_2BP_AutoStep.Step300_InMode_Await_PIO_TR_REQ:
                            {
                                if(INPUT_DIR_STK_TR_REQ_Await())
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step310_InMode_Check_PIO_Busy;
                            }
                            break;
                        case OP_2BP_AutoStep.Step310_InMode_Check_PIO_Busy:
                            {
                                if (INPUT_DIR_STK_BUSY_Check())
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step320_InMode_Check_PIO_Complete;
                            }
                            break;
                        case OP_2BP_AutoStep.Step320_InMode_Check_PIO_Complete:
                            {
                                if (INPUT_DIR_STK_COMPLETE_Check())
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step330_InMode_Check_PIO_End;

                                //if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
                                //{
                                //    if (!PIOStatus_STKToPort_Busy || !PIOStatus_STKToPort_Complete)
                                //        AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                //}

                                //if (Carrier_CheckOP_ExistProduct(false) &&
                                //    PIOStatus_STKToPort_Busy &&
                                //    PIOStatus_STKToPort_Complete)
                                //{
                                //    string OPCarrierID = OP_CarrierID;
                                //    Carrier_ACK_PortToRM_CarrierID(OPCarrierID); //OP ID -> RM

                                //    if (Carrier_GetRMToPort_RecvMapCarrierID() == OPCarrierID) //|| m_eControlMode == ControlMode.CIMMode //RM Ack Check
                                //    {
                                //        OP_CarrierID = string.Empty;
                                //        PIOStatus_PortToSTK_Unload_Req = false;
                                //        m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step330_InMode_Check_PIO_End;
                                //        Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);
                                //    }
                                //}
                            }
                            break;
                        case OP_2BP_AutoStep.Step330_InMode_Check_PIO_End:
                            {
                                if (INPUT_DIR_STK_PIO_END_Check())
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step340_InMode_Check_OP_CST_Unload_And_Safe;

                                //if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
                                //{
                                //    if (PIOStatus_STKToPort_TR_REQ || PIOStatus_STKToPort_Busy || PIOStatus_STKToPort_Complete)
                                //        AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                //}

                                //if (!PIOStatus_STKToPort_TR_REQ &&
                                //    !PIOStatus_STKToPort_Busy &&
                                //    !PIOStatus_STKToPort_Complete)
                                //{
                                //    PIOStatus_PortToSTK_Ready = false;
                                //    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step340_InMode_Check_OP_CST_Unload_And_Safe;
                                //}
                            }
                            break;
                        case OP_2BP_AutoStep.Step340_InMode_Check_OP_CST_Unload_And_Safe:
                            {
                                if (!Sensor_OP_Fork_Detect)
                                {
                                    if (Carrier_CheckOP_ExistProduct(false))
                                    {
                                        Carrier_ClearPortToRM_CarrierID();
                                        m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step000_Check_Direction;
                                    }
                                    else
                                        AlarmInsert((short)PortAlarm.OP_CST_Detect_Sensor_Group_Error, AlarmLevel.Error);
                                }
                            }
                            break;
                        case OP_2BP_AutoStep.Step600_OutMode_Check_OP_CST_Load:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) && Carrier_CheckOP_ExistID())
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step800_OutMode_Call_BP_Unload_Req;
                                else if (Carrier_CheckOP_ExistProduct(false) && !Carrier_CheckOP_ExistID())
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step700_OutMode_Await_PIO_TR_REQ;
                                else if (Carrier_CheckOP_ExistProduct(true) && !Carrier_CheckOP_ExistID())
                                {
                                    //if (m_bTestMode)
                                    //    OP_CarrierID = "TestID";
                                    //else
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
                        case OP_2BP_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                            {
                                if (OUTPUT_DIR_STK_PIO_TR_REQ_Await())
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step710_OutMode_Check_PIO_Busy;

                                //PIOStatus_PortToSTK_Load_Req = true;
                                //if (PIOStatus_STKToPort_TR_REQ)
                                //{
                                //    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step710_OutMode_Check_PIO_Busy;
                                //    Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);
                                //}
                            }
                            break;
                        case OP_2BP_AutoStep.Step710_OutMode_Check_PIO_Busy:
                            {
                                if (OUTPUT_DIR_STK_PIO_BUSY_Check())
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step720_OutMode_Check_PIO_Complete;

                                //if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
                                //{
                                //    if (!PIOStatus_STKToPort_Busy)
                                //        AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                //}

                                //if (PIOStatus_STKToPort_Busy)
                                //{
                                //    PIOStatus_PortToSTK_Ready = true;
                                //    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step720_OutMode_Check_PIO_Complete;
                                //    Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);
                                //    m_RMCSTIDRWTimer.Stop();
                                //    m_RMCSTIDRWTimer.Reset();
                                //    m_RMCSTIDRWTimer.Start();
                                //}
                            }
                            break;
                        case OP_2BP_AutoStep.Step720_OutMode_Check_PIO_Complete:
                            {
                                if (OUTPUT_DIR_STK_PIO_COMPLETE_Check())
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step730_OutMode_Check_PIO_End;

                                //if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
                                //{
                                //    if (!PIOStatus_STKToPort_Busy || !PIOStatus_STKToPort_Complete)
                                //        AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                //}

                                //string RMCarrierID = Carrier_GetRMToPort_RecvMapCarrierID();

                                //if (PIOStatus_STKToPort_Busy && PIOStatus_STKToPort_Complete)
                                //    Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);

                                //if (Carrier_CheckOP_ExistProduct(true) &&
                                //    PIOStatus_STKToPort_Busy &&
                                //    PIOStatus_STKToPort_Complete) //&& RMCarrierID != string.Empty
                                //{
                                //    Carrier_ACK_PortToRM_CarrierID(RMCarrierID);

                                //    if (RMCarrierID != string.Empty) //|| m_eControlMode == ControlMode.CIMMode
                                //    {
                                //        m_RMCSTIDRWTimer.Stop();
                                //        m_RMCSTIDRWTimer.Reset();
                                //        PIOStatus_PortToSTK_Load_Req = false;
                                //        m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step730_OutMode_Check_PIO_End;
                                //    }
                                //    else if (m_RMCSTIDRWTimer.Elapsed.TotalSeconds > 30.0)
                                //    {
                                //        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"RM -> Port CST ID Read Timeout");
                                //        m_RMCSTIDRWTimer.Stop();
                                //        m_RMCSTIDRWTimer.Reset();
                                //        PIOStatus_PortToSTK_Load_Req = false;
                                //        m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step730_OutMode_Check_PIO_End;
                                //    }
                                //}
                            }
                            break;
                        case OP_2BP_AutoStep.Step730_OutMode_Check_PIO_End:
                            {
                                if (OUTPUT_DIR_STK_PIO_END_Check())
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe;

                                //if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
                                //{
                                //    if (PIOStatus_STKToPort_TR_REQ || PIOStatus_STKToPort_Busy || PIOStatus_STKToPort_Complete)
                                //        AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                //}

                                //if (!PIOStatus_STKToPort_TR_REQ &&
                                //    !PIOStatus_STKToPort_Busy &&
                                //    !PIOStatus_STKToPort_Complete)
                                //{
                                //    OP_CarrierID = Carrier_GetPortToRM_SendMapCarrierID();
                                //    if (OP_CarrierID == Carrier_GetPortToRM_SendMapCarrierID() && OP_CarrierID != string.Empty)
                                //    {
                                //        Carrier_ClearPortToRM_CarrierID();
                                //        PIOStatus_PortToSTK_Ready = false;
                                //        m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe;
                                //    }
                                //    else if (OP_CarrierID == string.Empty)
                                //    {
                                //        //Timeout의 경우
                                //        OP_CarrierID = "CST_ID_READ_FAIL";
                                //        Carrier_ClearPortToRM_CarrierID();
                                //        PIOStatus_PortToSTK_Ready = false;
                                //        m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe;
                                //    }
                                //}
                            }
                            break;
                        case OP_2BP_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) && !Sensor_OP_Fork_Detect)
                                {
                                    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step800_OutMode_Call_BP_Unload_Req;
                                }
                            }
                            break;
                        case OP_2BP_AutoStep.Step800_OutMode_Call_BP_Unload_Req:
                            {
                                if(m_eShuttle_2BP_AutoStep >= Shuttle_2BP_AutoStep.Step810_OutMode_Move_X_LP && 
                                !Carrier_CheckOP_ExistID() && 
                                !Sensor_OP_Fork_Detect)
                                {
                                    if(Carrier_CheckOP_ExistProduct(false))
                                        m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step000_Check_Direction;
                                    else
                                        AlarmInsert((short)PortAlarm.OP_CST_Detect_Sensor_Group_Error, AlarmLevel.Error);
                                }
                                //if (Carrier_CheckOP_ExistProduct(false) && !Carrier_CheckOP_ExistID() && !Sensor_OP_Fork_Detect)
                                //{
                                //    m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step000_Check_Direction;
                                //}
                            }
                            break;
                    }
                    Thread.Sleep(Master.StepUpdateTime);
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Start();
        }
        private void Auto_2BP_Start_Shuttle_Control()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                while (m_bShuttle_2BP_AutoRunning || m_bCycleRunning)
                {
                    //1. Auto Flag, Cycle Flag가 둘다 true인 경우 알람 (실제 발생 가능성은 거의 없지만 안전 장치)
                    if (m_bShuttle_2BP_AutoRunning && m_bCycleRunning)
                    {
                        Auto_2BP_BP_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAndCycleCrash, $"Shuttle 2BP");
                        break;
                    }

                    bool bAuto = m_bShuttle_2BP_AutoRunning;

                    //2. Port가 알람 상태인 경우 정지
                    if (GetAlarmLevel() == AlarmLevel.Error)
                    {
                        Auto_2BP_BP_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAlarmStop, $"Shuttle 2BP");
                        break;
                    }

                    //3. Step에 연관된 알람 상황 체크
                    Auto_2BP_BP_Placement_Detect_AlarmCheck();
                    Auto_2BP_BP_StepTimeOut_AlarmCheck();

                    //4. Step에 연관된 상태 업데이트
                    Auto_2BP_BPStopEnableUpdate();

                    //5. 정지 명령 시 중단
                    if (IsAutoStopReq(bAuto))
                    {
                        if (m_bShuttle_2BP_AutoStopEnable)
                        {
                            Auto_2BP_BP_StopInit();
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AutoControlStopCommand, $"Shuttle 2BP");
                            break;
                        }
                    }

                    //6. 동작 수행
                    switch (m_eShuttle_2BP_AutoStep)
                    {
                        case Shuttle_2BP_AutoStep.Step000_Check_Direction:
                            {
                                ServoCtrl_ClearAxisTorqueCal();

                                if (GetOperationDirection() == PortDirection.Input)
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step100_InMode_Check_BP_CST_Load;
                                else
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step600_OutMode_Check_BP_CST_Load;
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step001_Change_Direction:
                            {
                                if (GetOperationDirection() == PortDirection.Input)
                                    GetMotionParam().ePortDirection = PortDirection.Output;
                                else
                                    GetMotionParam().ePortDirection = PortDirection.Input;

                                m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step000_Check_Direction;
                            }
                            break;

                        ///In Mode Step
                        case Shuttle_2BP_AutoStep.Step100_InMode_Check_BP_CST_Load:
                            {
                                if (Carrier_CheckShuttle_ExistProduct(false))
                                {
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step110_InMode_Check_BP_Pose;
                                }
                                else if (Carrier_CheckShuttle_ExistProduct(true))
                                {
                                    AlarmInsert((short)PortAlarm.Shuttle_Placement_Detect_Error, AlarmLevel.Error);
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step110_InMode_Check_BP_Pose:
                            {
                                if (IsTAxisPos_0_Deg() || IsTAxisPos_180_Deg())
                                {
                                    if(!IsZAxisPos_DOWN(PortAxis.Shuttle_Z))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step120_InMode_Move_Z_Down;
                                    else
                                    {
                                        if (!IsXAxisPos_WaitorLP())
                                            m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step130_InMode_Move_X_WaitorLP;
                                        else if (!IsTAxisPos_180_Deg())
                                            m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step140_InMode_Move_T_180_Deg;
                                        else
                                            m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step200_InMode_Await_LP_Unload_Req;
                                    }
                                }
                                else
                                {
                                    if (!IsXAxisPos_WaitorLP())
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step130_InMode_Move_X_WaitorLP;
                                    else
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step140_InMode_Move_T_180_Deg;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step120_InMode_Move_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Down_Pos))
                                {
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step110_InMode_Check_BP_Pose;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step130_InMode_Move_X_WaitorLP:
                            {
                                if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.Wait_Pos))
                                    {
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step110_InMode_Check_BP_Pose;
                                    }
                                }
                                else
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step110_InMode_Check_BP_Pose;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step140_InMode_Move_T_180_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree180_Pos))
                                {
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step110_InMode_Check_BP_Pose;
                                }

                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step200_InMode_Await_LP_Unload_Req:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    if (bAuto && m_eLP_2BP_AutoStep == LP_2BP_AutoStep.Step400_InMode_Call_BP_Unload_Req)
                                    {
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step210_InMode_Move_X_LP;
                                    }
                                    else if(!bAuto)
                                    {
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step210_InMode_Move_X_LP;
                                    }
                                }
                                else
                                {
                                    if(!Carrier_CheckLP_ExistProduct(true) && m_eLP_2BP_AutoStep == LP_2BP_AutoStep.Step400_InMode_Call_BP_Unload_Req)
                                        AlarmInsert((short)PortAlarm.LP_Placement_Detect_Error, AlarmLevel.Error);
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step210_InMode_Move_X_LP:
                            {
                                if (Carrier_CheckLP_ExistProduct(true, false) &&
                                    Carrier_CheckShuttle_ExistProduct(false)) //20230523 && Auto 삭제(CST Reading 위치 BP로 이동) //20230417 Carrier_CheckLP_ExistID() 삭제 카세트 리딩 루틴 변경
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step211_InMode_Ready_CST_ID_Read;
                                }
                                //else if (Carrier_CheckLP_ExistProduct(true, false) &&
                                //        Carrier_CheckShuttle_ExistProduct(false) &&
                                //        !bAuto)
                                //{
                                //    //Manual 상황 : LP에 제품이 있으며 BP에 제품이 없는 경우 ID직접 부여후 LP 위치로 이동
                                //    //Carrier_SetLP_CarrierID("ManualTest");
                                //    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.LP_Pos))
                                //        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step211_InMode_Ready_CST_ID_Read;
                                //}
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step211_InMode_Ready_CST_ID_Read:
                            {
                                if (TAG_READ_INIT())
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step212_InMode_Start_CST_ID_Read;
                                //if (m_TagReader_Interface.GetReaderEquipType() == TagReader.TagReaderType.RFID)
                                //    m_TagReader_Interface.GetRFIDReader().n_RFIDReadCount = 0;
                                //m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step212_InMode_Start_CST_ID_Read;
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step212_InMode_Start_CST_ID_Read:
                            {
                                if (Carrier_CheckLP_ExistProduct(true))
                                {
                                    if (TAG_READ_TRY(TAG_ID_READ_SET_SECTION.LP))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step213_InMode_Await_OP_Load_Req;
                                }

                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step213_InMode_Await_OP_Load_Req:
                            {
                                //신규 스텝 추가 기존 212 -> 220으로 가던 스텝을
                                //Z축 Down 상태로 대기 할 수 있도록 변경
                                if (Carrier_CheckOP_ExistProduct(false) && !Sensor_OP_Fork_Detect)
                                {
                                    if (bAuto && m_eOP_2BP_AutoStep == OP_2BP_AutoStep.Step200_InMode_Call_BP_Load_Req)
                                    {
                                        //자동 공정
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step220_InMode_Move_Z_Up;
                                    }
                                    else if (!bAuto)
                                    {
                                        //매뉴얼 공정
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step220_InMode_Move_Z_Up;
                                    }
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step220_InMode_Move_Z_Up:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Up_Pos))
                                {
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step230_InMode_Read_LP_CST_ID;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step230_InMode_Read_LP_CST_ID:
                            {
                                if (Carrier_CheckLP_ExistProduct(false, false) &&
                                    Carrier_CheckShuttle_ExistProduct(true))
                                {
                                    if (Carrier_LP_To_Shuttle_CST_ID_Transfer())
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step240_InMode_Move_X_WaitorLP;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step240_InMode_Move_X_WaitorLP:
                            {
                                if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.Wait_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step250_InMode_Move_T_0_Deg;
                                }
                                else
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step300_InMode_Await_OP_Load_Req;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step250_InMode_Move_T_0_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree0_Pos))
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step300_InMode_Await_OP_Load_Req;
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step300_InMode_Await_OP_Load_Req:
                            {
                                if (Carrier_CheckOP_ExistProduct(false) && !Sensor_OP_Fork_Detect)
                                {
                                    if (bAuto && m_eOP_2BP_AutoStep == OP_2BP_AutoStep.Step200_InMode_Call_BP_Load_Req)
                                    {
                                        //자동 공정
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step310_InMode_Move_X_OP;
                                    }
                                    else if(!bAuto)
                                    {
                                        //매뉴얼 공정
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step310_InMode_Move_X_OP;
                                    }
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step310_InMode_Move_X_OP:
                            {
                                if (Carrier_CheckShuttle_ExistProduct(true) &&
                                    Carrier_CheckOP_ExistProduct(false, false) &&
                                    !Carrier_CheckOP_ExistID() && bAuto)
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.OP_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step320_InMode_Move_Z_Down;
                                }
                                else if(Carrier_CheckShuttle_ExistProduct(true) &&
                                    Carrier_CheckOP_ExistProduct(false, false) &&
                                    !bAuto)
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.OP_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step320_InMode_Move_Z_Down;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step320_InMode_Move_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Down_Pos))
                                {
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step330_InMode_Send_OP_CST_ID;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step330_InMode_Send_OP_CST_ID:
                            {
                                if (Carrier_CheckShuttle_ExistProduct(false) &&
                                    Carrier_CheckOP_ExistProduct(true))
                                {
                                    if (Carrier_Shuttle_To_OP_CST_ID_Transfer())
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step340_InMode_Move_X_WaitorLP;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step340_InMode_Move_X_WaitorLP:
                            {
                                if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.Wait_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step350_InMode_Move_T_180_Deg;
                                }
                                else
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step360_InMode_Move_Z_Down;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step350_InMode_Move_T_180_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree180_Pos))
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step360_InMode_Move_Z_Down;
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step360_InMode_Move_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Down_Pos) && bAuto)
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step000_Check_Direction;
                                else if(!bAuto)
                                {
                                    if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                    {
                                        //카운트를 증가시키고
                                        m_CycleControlProgressCount++;
                                        //-----------Direction Change 실행
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step001_Change_Direction;
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

                            ///Out Mode Step
                        case Shuttle_2BP_AutoStep.Step600_OutMode_Check_BP_CST_Load:
                            {
                                if (Carrier_CheckShuttle_ExistProduct(false))
                                {
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step610_OutMode_Check_BP_Pose;
                                }
                                else if (Carrier_CheckShuttle_ExistProduct(true))
                                {
                                    AlarmInsert((short)PortAlarm.Shuttle_Placement_Detect_Error, AlarmLevel.Error);
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step610_OutMode_Check_BP_Pose:
                            {
                                if (IsTAxisPos_0_Deg() || IsTAxisPos_180_Deg()) //수평 자세인 경우
                                {
                                    if (!IsZAxisPos_DOWN(PortAxis.Shuttle_Z))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step620_OutMode_Move_Z_Down;
                                    else
                                    {
                                        if (!IsXAxisPos_WaitorLP())
                                            m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step630_OutMode_Move_X_WaitorLP;
                                        else if (!IsTAxisPos_0_Deg())
                                            m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step640_OutMode_Move_T_0_Deg;
                                        else
                                            m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step700_OutMode_Await_OP_Unload_Req;
                                    }
                                }
                                else
                                {
                                    if (!IsXAxisPos_WaitorLP())
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step630_OutMode_Move_X_WaitorLP;
                                    else
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step640_OutMode_Move_T_0_Deg;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step620_OutMode_Move_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Down_Pos))
                                {
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step610_OutMode_Check_BP_Pose;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step630_OutMode_Move_X_WaitorLP:
                            {
                                if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.Wait_Pos))
                                    {
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step610_OutMode_Check_BP_Pose;
                                    }
                                }
                                else
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step610_OutMode_Check_BP_Pose;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step640_OutMode_Move_T_0_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree0_Pos))
                                {
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step610_OutMode_Check_BP_Pose;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step700_OutMode_Await_OP_Unload_Req:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) && 
                                    !Sensor_OP_Fork_Detect)
                                {
                                    if(m_eOP_2BP_AutoStep == OP_2BP_AutoStep.Step800_OutMode_Call_BP_Unload_Req && bAuto)
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step710_OutMode_Move_X_OP;
                                    else if(!bAuto)
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step710_OutMode_Move_X_OP;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step710_OutMode_Move_X_OP:
                            {
                                if (Carrier_CheckOP_ExistProduct(true, false) &&
                                    Carrier_CheckShuttle_ExistProduct(false) &&
                                    Carrier_CheckOP_ExistID() && bAuto)
                                {
                                    //Auto 상황 : OP에 제품이 있으며 ID가 부여되어 있고 BP에는 Carrier가 없다면 OP 위치로 이동
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.OP_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step711_OutMode_Await_LP_Load_Req;
                                }
                                else if (Carrier_CheckOP_ExistProduct(true, false) &&
                                        Carrier_CheckShuttle_ExistProduct(false) &&
                                        !bAuto)
                                {
                                    //Manual 상황 : OP에 제품이 있으며 BP에 제품이 없는 경우 ID직접 부여후 OP 위치로 이동
                                    OP_CarrierID = "CYCLE_TEST_ID";
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.OP_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step711_OutMode_Await_LP_Load_Req;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step711_OutMode_Await_LP_Load_Req:
                            {
                                //신규 스텝 710 -> 720 가던 부분을 Z축 다운 상태로 대기 요청
                                //신시누 기준
                                if (!Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    if (m_eLP_2BP_AutoStep == LP_2BP_AutoStep.Step700_OutMode_Call_BP_Load_Req && bAuto)
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step720_OutMode_Move_Z_Up;
                                    else if (!bAuto)
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step720_OutMode_Move_Z_Up;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step720_OutMode_Move_Z_Up:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Up_Pos))
                                {
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step730_OutMode_Read_OP_CST_ID;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step730_OutMode_Read_OP_CST_ID:
                            {
                                if (Carrier_CheckOP_ExistProduct(false, false) &&
                                    Carrier_CheckShuttle_ExistProduct(true))
                                {
                                    if (Carrier_OP_To_Shuttle_CST_ID_Transfer())
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step740_OutMode_Move_X_WaitorLP;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step740_OutMode_Move_X_WaitorLP:
                            {
                                if (Carrier_CheckLP_ExistProduct(false, false))
                                {
                                    if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                    {
                                        if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.Wait_Pos))
                                            m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step750_OutMode_Move_T_180_Deg;
                                    }
                                    else
                                    {
                                        if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                            m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step800_OutMode_Await_LP_Load_Req;
                                    }
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step750_OutMode_Move_T_180_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree180_Pos))
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step800_OutMode_Await_LP_Load_Req;
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step800_OutMode_Await_LP_Load_Req:
                            {
                                if (!Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    if (m_eLP_2BP_AutoStep == LP_2BP_AutoStep.Step700_OutMode_Call_BP_Load_Req && bAuto)
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step810_OutMode_Move_X_LP;
                                    else if(!bAuto)
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step810_OutMode_Move_X_LP;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step810_OutMode_Move_X_LP:
                            {
                                if (Carrier_CheckShuttle_ExistProduct(true) &&
                                    Carrier_CheckLP_ExistProduct(false, false))
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step820_OutMode_Move_Z_Down;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step820_OutMode_Move_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Down_Pos))
                                {
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step830_OutMode_Send_LP_CST_ID;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step830_OutMode_Send_LP_CST_ID:
                            {
                                if (Carrier_CheckShuttle_ExistProduct(false) &&
                                    Carrier_CheckLP_ExistProduct(true))
                                {
                                    if (Carrier_Shuttle_To_LP_CST_ID_Transfer())
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step840_OutMode_Move_X_WaitorLP;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step840_OutMode_Move_X_WaitorLP:
                            {
                                if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, Teaching_X_Pos.Wait_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step850_OutMode_Move_T_0_Deg;
                                }
                                else
                                {
                                    if (X_Axis_MotionAndDone(PortAxis.Shuttle_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step860_OutMode_Move_Z_Down;
                                }
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step850_OutMode_Move_T_0_Deg:
                            {
                                if (T_Axis_MotionAndDone(PortAxis.Shuttle_T, Teaching_T_Pos.Degree0_Pos))
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step860_OutMode_Move_Z_Down;
                            }
                            break;
                        case Shuttle_2BP_AutoStep.Step860_OutMode_Move_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Shuttle_Z, Teaching_Z_Pos.Down_Pos) && bAuto)
                                    m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step000_Check_Direction;
                                else if (!bAuto)
                                {
                                    if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                    {
                                        //카운트를 증가시키고
                                        m_CycleControlProgressCount++;
                                        //-----------Direction Change 실행
                                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step001_Change_Direction;
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
        /// LP or OP or Shuttle의 현재 스텝 텍스트
        /// </summary>
        /// <returns></returns>
        private string Auto_2BP_Get_LP_StepStr()
        {
            switch (m_eLP_2BP_AutoStep)
            {
                case LP_2BP_AutoStep.Step000_Check_Direction:
                    return $"Check Direction";
                case LP_2BP_AutoStep.Step100_InMode_Check_LP_CST_Load:
                case LP_2BP_AutoStep.Step600_OutMode_Check_LP_CST_Load:
                    return $"Check LP CST Load";
                case LP_2BP_AutoStep.Step110_InMode_Check_BP_Location:
                case LP_2BP_AutoStep.Step710_OutMode_Check_BP_Location:
                    return $"Check BP Location";
                case LP_2BP_AutoStep.Step200_InMode_Await_PIO_CS:
                case LP_2BP_AutoStep.Step800_OutMode_Await_PIO_CS:
                    return $"Await PIO-CS";
                case LP_2BP_AutoStep.Step210_InMode_Check_PIO_Valid:
                case LP_2BP_AutoStep.Step810_OutMode_Check_PIO_Valid:
                    return $"Check PIO-Valid";
                case LP_2BP_AutoStep.Step220_InMode_Check_PIO_TR:
                case LP_2BP_AutoStep.Step820_OutMode_Check_PIO_TR:
                    return $"Check PIO-TR";
                case LP_2BP_AutoStep.Step230_InMode_Check_PIO_Busy:
                case LP_2BP_AutoStep.Step830_OutMode_Check_PIO_Busy:
                    return $"Check PIO-Busy";
                case LP_2BP_AutoStep.Step240_InMode_Check_PIO_Complete:
                case LP_2BP_AutoStep.Step840_OutMode_Check_PIO_Complete:
                    return $"Check PIO-Complete";
                case LP_2BP_AutoStep.Step250_InMode_Check_PIO_End:
                case LP_2BP_AutoStep.Step850_OutMode_Check_PIO_End:
                    return $"Check PIO-End Off";
                case LP_2BP_AutoStep.Step260_InMode_Check_CST_Load_And_Safe:
                    return $"Check LP CST Load and Safe";
                case LP_2BP_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                    return $"Await MGV CST Load";
                //case LP_2BP_AutoStep.Step400_InMode_Ready_CST_ID_Read:
                //    return $"Ready CST ID Read";
                //case LP_2BP_AutoStep.Step410_InMode_Start_CST_ID_Read:
                //    return $"Start CST ID Read";
                case LP_2BP_AutoStep.Step400_InMode_Call_BP_Unload_Req:
                    return $"Call BP(Unload Req)";
                case LP_2BP_AutoStep.Step410_InMode_Clear_LP_CST_ID:
                    return $"Clear LP CST ID";

                case LP_2BP_AutoStep.Step700_OutMode_Call_BP_Load_Req:
                    return $"Call BP(Load Req)";
                
                case LP_2BP_AutoStep.Step860_OutMode_Check_LP_CST_Unload_And_Safe:
                    return $"Check LP CST Unload and Safe";
                case LP_2BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload:
                    return $"Await MGV CST Unload";
                //case LP_2BP_AutoStep.Step211_InMode_Call_AGV_Load_Req:
                //    return $"Call AGV CST Load Req";
                //case LP_2BP_AutoStep.Step811_OutMode_Call_AGV_Unload_Req:
                //    return $"Call AGV CST Unload Req";
                default:
                    return "Not def step str";
            }
        }
        private string Auto_2BP_Get_OP_StepStr()
        {
            switch (m_eOP_2BP_AutoStep)
            {
                case OP_2BP_AutoStep.Step000_Check_Direction:
                    return $"Check Direction";
                case OP_2BP_AutoStep.Step100_InMode_Check_OP_CST_Load:
                case OP_2BP_AutoStep.Step600_OutMode_Check_OP_CST_Load:
                    return $"Check OP CST Load";
                case OP_2BP_AutoStep.Step200_InMode_Call_BP_Load_Req:
                    return $"Call BP(Load Req)";
                case OP_2BP_AutoStep.Step210_InMode_Check_BP_Location:
                    return $"Check BP Location";
                case OP_2BP_AutoStep.Step300_InMode_Await_PIO_TR_REQ:
                case OP_2BP_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                    return $"Await PIO-TR_REQ";
                case OP_2BP_AutoStep.Step310_InMode_Check_PIO_Busy:
                case OP_2BP_AutoStep.Step710_OutMode_Check_PIO_Busy:
                    return $"Check PIO-Busy";
                case OP_2BP_AutoStep.Step320_InMode_Check_PIO_Complete:
                case OP_2BP_AutoStep.Step720_OutMode_Check_PIO_Complete:
                    return $"Check PIO-Complete";
                case OP_2BP_AutoStep.Step330_InMode_Check_PIO_End:
                case OP_2BP_AutoStep.Step730_OutMode_Check_PIO_End:
                    return $"Check PIO-End off";
                case OP_2BP_AutoStep.Step340_InMode_Check_OP_CST_Unload_And_Safe:
                    return $"Check OP CST Unload and Safe";


                case OP_2BP_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe:
                    return $"Check OP CST Load and Safe";
                case OP_2BP_AutoStep.Step800_OutMode_Call_BP_Unload_Req:
                    return $"Call BP(Unload Req)";
                default:
                    return "Not def step str";
            }
        }
        private string Auto_2BP_Get_BP_StepStr()
        {
            switch (m_eShuttle_2BP_AutoStep)
            {
                case Shuttle_2BP_AutoStep.Step000_Check_Direction:
                    return $"Check Direction";
                case Shuttle_2BP_AutoStep.Step001_Change_Direction:
                    return $"Change Direction";

                ///In Mode Step
                case Shuttle_2BP_AutoStep.Step100_InMode_Check_BP_CST_Load:
                case Shuttle_2BP_AutoStep.Step600_OutMode_Check_BP_CST_Load:
                    return $"Check BP CST Load";
                case Shuttle_2BP_AutoStep.Step110_InMode_Check_BP_Pose:
                case Shuttle_2BP_AutoStep.Step610_OutMode_Check_BP_Pose:
                    return $"Check BP Pose";
                case Shuttle_2BP_AutoStep.Step120_InMode_Move_Z_Down:
                case Shuttle_2BP_AutoStep.Step620_OutMode_Move_Z_Down:
                    return $"Move Z Down Pos";
                case Shuttle_2BP_AutoStep.Step130_InMode_Move_X_WaitorLP:
                case Shuttle_2BP_AutoStep.Step630_OutMode_Move_X_WaitorLP:
                    return GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X) ? $"Move X Wait Pos" : $"Move X LP Pos";
                case Shuttle_2BP_AutoStep.Step140_InMode_Move_T_180_Deg:
                    return $"Move T 180 Deg Pos";
                case Shuttle_2BP_AutoStep.Step200_InMode_Await_LP_Unload_Req:
                    return $"Await Unload Req(LP)";
                case Shuttle_2BP_AutoStep.Step210_InMode_Move_X_LP:
                    return $"Move X LP Pos (LP Unloading)";
                case Shuttle_2BP_AutoStep.Step211_InMode_Ready_CST_ID_Read:
                    return $"Ready CST ID Read";
                case Shuttle_2BP_AutoStep.Step212_InMode_Start_CST_ID_Read:
                    return $"Start CST ID Read";
                case Shuttle_2BP_AutoStep.Step213_InMode_Await_OP_Load_Req:
                    return $"Await Load Req(OP)";
                case Shuttle_2BP_AutoStep.Step220_InMode_Move_Z_Up:
                    return $"Move Z Up Pos (LP Unloading)";
                case Shuttle_2BP_AutoStep.Step230_InMode_Read_LP_CST_ID:
                    return $"Read LP CST ID (LP -> BP)";
                case Shuttle_2BP_AutoStep.Step240_InMode_Move_X_WaitorLP:
                    return GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X) ? $"Move X Wait Pos (LP Unloading)" : $"Move X LP Pos (LP Unloading)";
                case Shuttle_2BP_AutoStep.Step250_InMode_Move_T_0_Deg:
                    return $"Move T 0 Deg Pos (LP Unloading)";
                case Shuttle_2BP_AutoStep.Step300_InMode_Await_OP_Load_Req:
                    return $"Await Load Req(OP)";
                case Shuttle_2BP_AutoStep.Step310_InMode_Move_X_OP:
                    return $"Move X OP Pos (OP loading)";
                case Shuttle_2BP_AutoStep.Step320_InMode_Move_Z_Down:
                    return $"Move Z Down Pos (OP loading)";
                case Shuttle_2BP_AutoStep.Step330_InMode_Send_OP_CST_ID:
                    return $"Send OP CST ID (BP -> OP)";
                case Shuttle_2BP_AutoStep.Step340_InMode_Move_X_WaitorLP:
                    return GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X) ? $"Move X Wait Pos (OP loading)" : $"Move X LP Pos (OP loading)";
                case Shuttle_2BP_AutoStep.Step350_InMode_Move_T_180_Deg:
                    return $"Move T 180 Deg Pos (OP loading)";
                case Shuttle_2BP_AutoStep.Step360_InMode_Move_Z_Down:
                    return $"Move Z Down Pos (OP loading)";


                case Shuttle_2BP_AutoStep.Step640_OutMode_Move_T_0_Deg:
                    return $"Move T 0 Deg Pos";
                case Shuttle_2BP_AutoStep.Step700_OutMode_Await_OP_Unload_Req:
                    return $"Await Unload Req(OP)";
                case Shuttle_2BP_AutoStep.Step710_OutMode_Move_X_OP:
                    return $"Move X OP Pos (OP Unloading)";
                case Shuttle_2BP_AutoStep.Step711_OutMode_Await_LP_Load_Req:
                    return $"Await Load Req(LP)";
                case Shuttle_2BP_AutoStep.Step720_OutMode_Move_Z_Up:
                    return $"Move Z Up Pos (OP Unloading)";
                case Shuttle_2BP_AutoStep.Step730_OutMode_Read_OP_CST_ID:
                    return $"Read OP CST ID (OP -> BP)";
                case Shuttle_2BP_AutoStep.Step740_OutMode_Move_X_WaitorLP:
                    return GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X) ? $"Move X Wait Pos (OP Unloading)" : $"Move X LP Pos (OP Unloading)";
                case Shuttle_2BP_AutoStep.Step750_OutMode_Move_T_180_Deg:
                    return $"Move T 180 Deg Pos (OP Unloading)";
                case Shuttle_2BP_AutoStep.Step800_OutMode_Await_LP_Load_Req:
                    return $"Await Load Req(LP)";
                case Shuttle_2BP_AutoStep.Step810_OutMode_Move_X_LP:
                    return $"Move X LP Pos (LP loading)";
                case Shuttle_2BP_AutoStep.Step820_OutMode_Move_Z_Down:
                    return $"Move Z Down Pos (LP loading)";
                case Shuttle_2BP_AutoStep.Step830_OutMode_Send_LP_CST_ID:
                    return $"Send LP CST ID (BP -> LP)";
                case Shuttle_2BP_AutoStep.Step840_OutMode_Move_X_WaitorLP:
                    return GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X) ? $"Move X Wait Pos (LP loading)" : $"Move X LP Pos (LP loading)";
                case Shuttle_2BP_AutoStep.Step850_OutMode_Move_T_0_Deg:
                    return $"Move T 0 Deg Pos (LP loading)";
                case Shuttle_2BP_AutoStep.Step860_OutMode_Move_Z_Down:
                    return $"Move Z Down Pos (LP loading)";
                default:
                    return "Not def step str";
            }
        }

        /// <summary>
        /// LP or OP or Shuttle의 현재 스텝 번호
        /// </summary>
        /// <returns></returns>
        private int Auto_2BP_Get_LP_StepNum()
        {
            return (int)m_eLP_2BP_AutoStep;
        }
        private int Auto_2BP_Get_OP_StepNum()
        {
            return (int)m_eOP_2BP_AutoStep;
        }
        private int Auto_2BP_Get_BP_StepNum()
        {
            return (int)m_eShuttle_2BP_AutoStep;
        }
    }
}
