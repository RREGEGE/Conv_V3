using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.Alarm;
using System.Threading;

namespace Master.Equipment.Port
{
    /// <summary>
    /// Port_Conveyor.cs는 Buffer 자체의 Z 모션과 컨베이어가 제어되는 타입의 공정입니다.
    /// 음성 공장 Type
    /// LP CV + LP Z Inverter(I/O Type)
    /// OP CV
    /// 
    /// BP 스텝은 있으나 BP 상황에 대한 검증 해본적 없음.
    /// </summary>
    partial class Port
    {
        public enum LP_CV_AutoStep
        {
            Step000_Check_Direction = 0,

            Step100_InMode_Check_LP_CST_Load_And_Pose       = 100,      //Carrier 적재 상황 확인
            Step110_InMode_Move_LP_X_LP                     = 110,
            Step120_InMode_Move_LP_Z_Down                   = 120,
            Step130_InMode_Move_LP_T_180_Deg                = 130,
            Step140_InMode_Move_LP_Stopper_Up               = 140,
            Step150_InMode_Move_LP_Centering_BWD            = 150,

            Step200_InMode_Await_PIO_CS                     = 200,      //Carrier Load 요청 (AGV or OHT)
            Step210_InMode_Check_PIO_Valid                  = 210,
            //Step211_InMode_Call_AGV_Load_Req                = 211,      //AGV PIO
            Step220_InMode_Check_PIO_TR                     = 220,
            Step230_InMode_Check_PIO_Busy                   = 230,
            Step240_InMode_Check_PIO_Complete               = 240,
            Step250_InMode_Check_PIO_End                    = 250,
            Step260_InMode_Check_CST_Load_And_Safe          = 260,

            Step300_InMode_Await_MGV_CST_Load               = 300,      //Carrier Load 대기 (MGV), Carrier ID Clear

            Step400_InMode_Ready_CST_ID_Read                = 400,      //Carrier ID Read Step
            Step410_InMode_Start_CST_ID_Read                = 410,
            Step420_InMode_Move_LP_Centering_FWD            = 420,
            Step430_InMode_Move_LP_X_OP                     = 430,
            Step440_InMode_Move_LP_Z_Up                     = 440,
            Step450_InMode_Move_LP_T_0_Deg                  = 450,
            Step460_InMode_Move_LP_Stopper_Down             = 460,
            Step470_InMode_Move_LP_Centering_BWD            = 470,

            Step500_InMode_Await_OPorBP_Load_Req            = 500,      //Pass CST
            Step510_InMode_Move_LP_CV_Rolling               = 510,
            Step520_InMode_Move_LP_CV_Stop                  = 520,
            Step530_InMode_Send_LP_CST_ID                   = 530,
            Step540_InMode_Clear_LP_CST_ID                  = 540,


            Step600_OutMode_Check_LP_CST_Load_And_Pose      = 600,      //Carrier 적재 상황 확인
            Step610_OutMode_Move_LP_X_OP                    = 610,
            Step620_OutMode_Move_LP_Z_Up                    = 620,
            Step630_OutMode_Move_LP_T_0_Deg                 = 630,
            Step640_OutMode_Move_LP_Stopper_Up              = 640,
            Step650_OutMode_Move_LP_Centering_BWD           = 650,

            Step700_OutMode_Call_OPorBP_Load_Req            = 710,      //Carrier Load 요청
            Step710_OutMode_Move_LP_Rolling                 = 720,
            Step720_OutMode_Move_LP_CV_Stop                 = 730,
            Step730_OutMode_Check_CST_ID                    = 740,

            Step800_OutMode_Move_LP_Centering_FWD           = 800,
            Step810_OutMode_Move_LP_X_LP                    = 810,
            Step820_OutMode_Move_LP_Z_Down                  = 820,
            Step830_OutMode_Move_LP_T_180_Deg               = 830,
            Step840_OutMode_Move_LP_Stopper_Down            = 840,
            Step850_OutMode_Move_LP_Centering_BWD           = 850,

            Step900_OutMode_Await_PIO_CS                    = 900,      //Carrier Unload 대기 (AGV or OHT)
            Step910_OutMode_Check_PIO_Valid                 = 910,
            //Step911_OutMode_Call_AGV_Unload_Req             = 911,
            Step920_OutMode_Check_PIO_TR                    = 920,
            Step930_OutMode_Check_PIO_Busy                  = 930,
            Step940_OutMode_Check_PIO_Complete              = 940,
            Step950_OutMode_Check_PIO_End                   = 950,
            Step960_OutMode_Check_LP_CST_Unload_And_Safe    = 960,      //Carrier ID Clear

            Step990_OutMode_Await_MGV_CST_Unload            = 990,      //Carrier Unload 대기 (MGV), Carrier ID Clear
        }
        public enum BP_CV_AutoStep
        {
            Step000_Check_Direction = 0,

            Step100_InMode_Check_All_BP_CST_Load_And_Status        = 100,      //Carrier 적재 상황 확인

            Step200_InMode_Start_CST_Pass_From_LP                   = 200,
            Step210_InMode_Move_BP_CV_Rolling                       = 210,
            Step220_InMode_Move_BP_CV_Stop                          = 220,
            Step230_InMode_Check_CST_ID                             = 230,

            Step300_InMode_Start_CST_Pass_To_OP                     = 300,      //Pass CST
            Step310_InMode_Move_BP_CV_Rolling                       = 310,
            Step320_InMode_Move_BP_CV_Stop                          = 320,
            Step330_InMode_Send_BP_CST_ID                           = 330,
            Step340_InMode_Clear_BP_CST_ID                          = 340,

            Step400_InMode_Pass_CST_BP_to_BP                        = 400,
            Step410_InMode_Move_BP_CV_Rolling                       = 410,
            Step420_InMode_Move_BP_CV_Stop                          = 420,
            Step430_InMode_Pass_CST_ID                              = 430,


            Step600_OutMode_Check_All_BP_CST_Load_And_Status        = 600,      //Carrier 적재 상황 확인

            Step700_OutMode_Start_CST_Pass_From_OP              = 700,
            Step710_OutMode_Move_BP_CV_Rolling                  = 710,
            Step720_OutMode_Move_BP_CV_Stop                     = 720,
            Step730_OutMode_Check_CST_ID                        = 730,

            Step800_OutMode_Start_CST_Pass_To_LP              = 800,      //Pass CST
            Step810_OutMode_Move_BP_CV_Rolling                  = 810,
            Step820_OutMode_Move_BP_CV_Stop                     = 820,
            Step830_OutMode_Send_BP_CST_ID                      = 830,
            Step840_OutMode_Clear_BP_CST_ID                     = 840,

            Step900_OutMode_Pass_CST_BP_to_BP                   = 900,
            Step910_OutMode_Move_BP_CV_Rolling                  = 910,
            Step920_OutMode_Move_BP_CV_Stop                     = 920,
            Step930_OutMode_Pass_CST_ID                         = 930,
        }
        public enum OP_CV_AutoStep
        {
            Step000_Check_Direction = 0,

            Step100_InMode_Check_OP_CST_Load_And_Pose   = 100,      
            
            Step110_InMode_Move_OP_X_LP                 = 110,
            Step120_InMode_Move_OP_Z_Down               = 120,
            Step130_InMode_Move_OP_T_180_Deg            = 130,
            Step140_InMode_Move_OP_Stopper_Up           = 140,
            Step150_InMode_Move_OP_Centering_BWD        = 150,

            Step200_InMode_Call_LPorBP_Load_Req         = 200,      
            Step210_InMode_Move_OP_Rolling              = 210,
            Step220_InMode_Move_OP_CV_Stop              = 220,
            Step230_InMode_Check_CST_ID                 = 230,

            Step300_InMode_Move_OP_Centering_FWD        = 300,
            Step310_InMode_Move_OP_X_OP                 = 310,
            Step320_InMode_Move_OP_Z_Up                 = 320,
            Step330_InMode_Move_OP_T_0_Deg              = 330,
            Step340_InMode_Move_OP_Stopper_Down         = 340,
            Step350_InMode_Move_OP_Centering_BWD        = 350,

            Step400_InMode_Await_PIO_TR_REQ             = 400,      
            Step410_InMode_Check_PIO_Busy               = 410,
            Step420_InMode_Check_PIO_Complete           = 420,     
            Step430_InMode_Check_PIO_End                = 430,
            Step440_InMode_Check_OP_CST_Unload_And_Safe = 440,     


            Step600_OutMode_Check_OP_CST_Load_And_Pose  = 600,      
            
            Step610_OutMode_Move_OP_X_OP                = 610,
            Step620_OutMode_Move_OP_Z_Up                = 620,
            Step630_OutMode_Move_OP_T_0_Deg             = 630,
            Step640_OutMode_Move_OP_Stopper_Up          = 640,
            Step650_OutMode_Move_OP_Centering_BWD       = 650,

            Step700_OutMode_Await_PIO_TR_REQ            = 700,      //Carrier Load 요청(RM)
            Step710_OutMode_Check_PIO_Busy              = 710,
            Step720_OutMode_Check_PIO_Complete          = 720,      //ID 복사 RM Word Map -> Port Word Map
            Step730_OutMode_Check_PIO_End               = 730,      //ID 적용 Port WordMap -> OP Carrier ID
            Step740_OutMode_Check_OP_CST_Load_And_Safe  = 740,

            Step800_OutMode_Move_OP_Centering_FWD       = 800,
            Step810_OutMode_Move_OP_X_LP                = 810,
            Step820_OutMode_Move_OP_Z_Down              = 820,
            Step830_OutMode_Move_OP_T_180_Deg           = 830,
            Step840_OutMode_Move_OP_Stopper_Down        = 840,
            Step850_OutMode_Move_OP_Centering_BWD       = 850,

            Step900_OutMode_Await_LPorBP_Load_Req      = 900,       
            Step910_OutMode_Move_OP_CV_Rolling          = 910,
            Step920_OutMode_Move_OP_CV_Stop             = 920,
            Step930_OutMode_Send_OP_CST_ID              = 930,
            Step940_OutMode_Clear_OP_CST_ID             = 940
        }

        /// <summary>
        /// LP 2BP Type In/Out Control
        /// </summary>
        LP_CV_AutoStep m_eLP_CV_AutoStep = LP_CV_AutoStep.Step000_Check_Direction;
        BP_CV_AutoStep m_eBP_CV_AutoStep = BP_CV_AutoStep.Step000_Check_Direction;
        OP_CV_AutoStep m_eOP_CV_AutoStep = OP_CV_AutoStep.Step000_Check_Direction;

        LP_CV_AutoStep Pre_LP_CV_AutoStep = LP_CV_AutoStep.Step000_Check_Direction;
        BP_CV_AutoStep Pre_BP_CV_AutoStep = BP_CV_AutoStep.Step000_Check_Direction;
        OP_CV_AutoStep Pre_OP_CV_AutoStep = OP_CV_AutoStep.Step000_Check_Direction;

        bool m_bLP_CV_AutoStopEnable = false;
        bool m_bBP_CV_AutoStopEnable = false;
        bool m_bOP_CV_AutoStopEnable = false;

        //Auto
        private bool m_bLP_CV_AutoRunning = false;
        private bool m_bBP_CV_AutoRunning = false;
        private bool m_bOP_CV_AutoRunning = false;

        BufferCV m_eBPtoBPStartBP = BufferCV.Buffer_LP;

        /// <summary>
        /// LP or OP or BP의 정지 가능 상태를 나타냄
        /// Call, Await Step에서 대부분 정지 가능(요청 또는 대기 상태)
        /// </summary>
        private void Auto_CV_LPStopEnableUpdate()
        {
            switch (m_eLP_CV_AutoStep)
            {
                case LP_CV_AutoStep.Step000_Check_Direction:
                case LP_CV_AutoStep.Step100_InMode_Check_LP_CST_Load_And_Pose:
                case LP_CV_AutoStep.Step200_InMode_Await_PIO_CS:
                case LP_CV_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                case LP_CV_AutoStep.Step500_InMode_Await_OPorBP_Load_Req:
                case LP_CV_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose:
                case LP_CV_AutoStep.Step700_OutMode_Call_OPorBP_Load_Req:
                case LP_CV_AutoStep.Step900_OutMode_Await_PIO_CS:
                case LP_CV_AutoStep.Step990_OutMode_Await_MGV_CST_Unload:
                    m_bLP_CV_AutoStopEnable = true;
                    break;
                case LP_CV_AutoStep.Step210_InMode_Check_PIO_Valid:
                case LP_CV_AutoStep.Step910_OutMode_Check_PIO_Valid:
                    {
                        if (AGVFullFlagOption)
                            m_bLP_CV_AutoStopEnable = false;
                        else
                            m_bLP_CV_AutoStopEnable = true;
                    }
                    break;
                default:
                    m_bLP_CV_AutoStopEnable = false;
                    break;
            }
        }
        private void Auto_CV_OPStopEnableUpdate()
        {
            switch (m_eOP_CV_AutoStep)
            {
                case OP_CV_AutoStep.Step000_Check_Direction:
                case OP_CV_AutoStep.Step200_InMode_Call_LPorBP_Load_Req:
                case OP_CV_AutoStep.Step400_InMode_Await_PIO_TR_REQ:
                case OP_CV_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose:
                case OP_CV_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                case OP_CV_AutoStep.Step900_OutMode_Await_LPorBP_Load_Req:
                    m_bOP_CV_AutoStopEnable = true;
                    break;
                default:
                    m_bOP_CV_AutoStopEnable = false;
                    break;
            }
        }
        private void Auto_CV_BPStopEnableUpdate()
        {
            switch (m_eBP_CV_AutoStep)
            {
                case BP_CV_AutoStep.Step000_Check_Direction:
                case BP_CV_AutoStep.Step100_InMode_Check_All_BP_CST_Load_And_Status:
                case BP_CV_AutoStep.Step600_OutMode_Check_All_BP_CST_Load_And_Status:
                    m_bBP_CV_AutoStopEnable = true;
                    break;
                default:
                    {
                        m_bBP_CV_AutoStopEnable = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// 자재가 있어야 할 스텝, 없어야 할 스텝을 구분하여 자재 판단
        /// </summary>
        private void Auto_CV_LP_Placement_Detect_AlarmCheck()
        {
            switch (m_eLP_CV_AutoStep)
            {
                //없어야 함
                case LP_CV_AutoStep.Step110_InMode_Move_LP_X_LP:
                case LP_CV_AutoStep.Step120_InMode_Move_LP_Z_Down:
                case LP_CV_AutoStep.Step130_InMode_Move_LP_T_180_Deg:
                case LP_CV_AutoStep.Step140_InMode_Move_LP_Stopper_Up:
                case LP_CV_AutoStep.Step150_InMode_Move_LP_Centering_BWD:
                case LP_CV_AutoStep.Step200_InMode_Await_PIO_CS:
                case LP_CV_AutoStep.Step210_InMode_Check_PIO_Valid:
                //case LP_CV_AutoStep.Step211_InMode_Call_AGV_Load_Req:
                case LP_CV_AutoStep.Step220_InMode_Check_PIO_TR:
                case LP_CV_AutoStep.Step530_InMode_Send_LP_CST_ID:
                case LP_CV_AutoStep.Step540_InMode_Clear_LP_CST_ID:
                case LP_CV_AutoStep.Step610_OutMode_Move_LP_X_OP:
                case LP_CV_AutoStep.Step620_OutMode_Move_LP_Z_Up:
                case LP_CV_AutoStep.Step630_OutMode_Move_LP_T_0_Deg:
                case LP_CV_AutoStep.Step640_OutMode_Move_LP_Stopper_Up:
                case LP_CV_AutoStep.Step650_OutMode_Move_LP_Centering_BWD:
                case LP_CV_AutoStep.Step700_OutMode_Call_OPorBP_Load_Req:
                case LP_CV_AutoStep.Step940_OutMode_Check_PIO_Complete:
                case LP_CV_AutoStep.Step950_OutMode_Check_PIO_End:
                case LP_CV_AutoStep.Step960_OutMode_Check_LP_CST_Unload_And_Safe:
                    {
                        if (Carrier_CheckLP_ExistProduct(true, false))
                        {
                            Watchdog_Start(WatchdogList.LP_Placement_ErrorTimer);
                            if (Watchdog_IsDetect(WatchdogList.LP_Placement_ErrorTimer))
                                AlarmInsert((short)PortAlarm.LP_Placement_Detect_Error, AlarmLevel.Error);
                        }
                        else
                            Watchdog_Stop(WatchdogList.LP_Placement_ErrorTimer, true);
                    }
                    break;

                //있어야 함
                case LP_CV_AutoStep.Step240_InMode_Check_PIO_Complete:
                case LP_CV_AutoStep.Step250_InMode_Check_PIO_End:
                case LP_CV_AutoStep.Step260_InMode_Check_CST_Load_And_Safe:
                case LP_CV_AutoStep.Step400_InMode_Ready_CST_ID_Read:
                case LP_CV_AutoStep.Step410_InMode_Start_CST_ID_Read:
                case LP_CV_AutoStep.Step420_InMode_Move_LP_Centering_FWD:
                case LP_CV_AutoStep.Step440_InMode_Move_LP_Z_Up:
                case LP_CV_AutoStep.Step430_InMode_Move_LP_X_OP:
                case LP_CV_AutoStep.Step450_InMode_Move_LP_T_0_Deg:
                case LP_CV_AutoStep.Step460_InMode_Move_LP_Stopper_Down:
                case LP_CV_AutoStep.Step470_InMode_Move_LP_Centering_BWD:
                case LP_CV_AutoStep.Step500_InMode_Await_OPorBP_Load_Req:
                case LP_CV_AutoStep.Step800_OutMode_Move_LP_Centering_FWD:
                case LP_CV_AutoStep.Step810_OutMode_Move_LP_X_LP:
                case LP_CV_AutoStep.Step820_OutMode_Move_LP_Z_Down:
                case LP_CV_AutoStep.Step830_OutMode_Move_LP_T_180_Deg:
                case LP_CV_AutoStep.Step840_OutMode_Move_LP_Stopper_Down:
                case LP_CV_AutoStep.Step850_OutMode_Move_LP_Centering_BWD:
                case LP_CV_AutoStep.Step900_OutMode_Await_PIO_CS:
                case LP_CV_AutoStep.Step910_OutMode_Check_PIO_Valid:
                //case LP_CV_AutoStep.Step911_OutMode_Call_AGV_Unload_Req:
                case LP_CV_AutoStep.Step920_OutMode_Check_PIO_TR:
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
        private void Auto_CV_OP_Placement_Detect_AlarmCheck()
        {
            switch (m_eOP_CV_AutoStep)
            {
                //있어야 함
                case OP_CV_AutoStep.Step300_InMode_Move_OP_Centering_FWD:
                case OP_CV_AutoStep.Step310_InMode_Move_OP_X_OP:
                case OP_CV_AutoStep.Step320_InMode_Move_OP_Z_Up:
                case OP_CV_AutoStep.Step330_InMode_Move_OP_T_0_Deg:
                case OP_CV_AutoStep.Step340_InMode_Move_OP_Stopper_Down:
                case OP_CV_AutoStep.Step350_InMode_Move_OP_Centering_BWD:
                case OP_CV_AutoStep.Step400_InMode_Await_PIO_TR_REQ:
                case OP_CV_AutoStep.Step410_InMode_Check_PIO_Busy:
                case OP_CV_AutoStep.Step730_OutMode_Check_PIO_End:
                case OP_CV_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe:
                case OP_CV_AutoStep.Step800_OutMode_Move_OP_Centering_FWD:
                case OP_CV_AutoStep.Step820_OutMode_Move_OP_Z_Down:
                case OP_CV_AutoStep.Step810_OutMode_Move_OP_X_LP:
                case OP_CV_AutoStep.Step830_OutMode_Move_OP_T_180_Deg:
                case OP_CV_AutoStep.Step840_OutMode_Move_OP_Stopper_Down:
                case OP_CV_AutoStep.Step850_OutMode_Move_OP_Centering_BWD:
                case OP_CV_AutoStep.Step900_OutMode_Await_LPorBP_Load_Req:
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
                case OP_CV_AutoStep.Step110_InMode_Move_OP_X_LP:
                case OP_CV_AutoStep.Step120_InMode_Move_OP_Z_Down:
                case OP_CV_AutoStep.Step130_InMode_Move_OP_T_180_Deg:
                case OP_CV_AutoStep.Step140_InMode_Move_OP_Stopper_Up:
                case OP_CV_AutoStep.Step150_InMode_Move_OP_Centering_BWD:
                case OP_CV_AutoStep.Step200_InMode_Call_LPorBP_Load_Req:
                case OP_CV_AutoStep.Step430_InMode_Check_PIO_End:
                case OP_CV_AutoStep.Step440_InMode_Check_OP_CST_Unload_And_Safe:
                case OP_CV_AutoStep.Step610_OutMode_Move_OP_X_OP:
                case OP_CV_AutoStep.Step620_OutMode_Move_OP_Z_Up:
                case OP_CV_AutoStep.Step630_OutMode_Move_OP_T_0_Deg:
                case OP_CV_AutoStep.Step640_OutMode_Move_OP_Stopper_Up:
                case OP_CV_AutoStep.Step650_OutMode_Move_OP_Centering_BWD:
                case OP_CV_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                case OP_CV_AutoStep.Step710_OutMode_Check_PIO_Busy:
                case OP_CV_AutoStep.Step930_OutMode_Send_OP_CST_ID:
                case OP_CV_AutoStep.Step940_OutMode_Clear_OP_CST_ID:
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

        /// <summary>
        /// 대기 중인 스텝을 제외하고 모션 중인 스텝에서 Time Out 확인
        /// </summary>
        private void Auto_CV_LP_StepTimeOut_AlarmCheck()
        {
            if (m_eLP_CV_AutoStep == Pre_LP_CV_AutoStep &&
                !m_bLP_CV_AutoStopEnable)
            {
                Watchdog_Start(WatchdogList.LP_Step_Timer);
                if (Watchdog_IsDetect(WatchdogList.LP_Step_Timer))
                    AlarmInsert((short)PortAlarm.Step_TimeOver_Error, AlarmLevel.Error);
            }
            else
            {
                Watchdog_Stop(WatchdogList.LP_Step_Timer, true);
            }

            if(m_eLP_CV_AutoStep != Pre_LP_CV_AutoStep)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortStepInfo, $"CV LP Step : {Pre_LP_CV_AutoStep} => {m_eLP_CV_AutoStep}");
            
            Pre_LP_CV_AutoStep = m_eLP_CV_AutoStep;
        }
        private void Auto_CV_OP_StepTimeOut_AlarmCheck()
        {
            if (m_eOP_CV_AutoStep == Pre_OP_CV_AutoStep &&
                !m_bOP_CV_AutoStopEnable)
            {
                Watchdog_Start(WatchdogList.OP_Step_Timer);
                if (Watchdog_IsDetect(WatchdogList.OP_Step_Timer))
                    AlarmInsert((short)PortAlarm.Step_TimeOver_Error, AlarmLevel.Error);
            }
            else
            {
                Watchdog_Stop(WatchdogList.OP_Step_Timer, true);
            }

            if(m_eOP_CV_AutoStep != Pre_OP_CV_AutoStep)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortStepInfo, $"CV OP Step : {Pre_OP_CV_AutoStep} => {m_eOP_CV_AutoStep}");

            Pre_OP_CV_AutoStep = m_eOP_CV_AutoStep;
        }
        private void Auto_CV_BP_StepTimeOut_AlarmCheck()
        {
            if (m_eBP_CV_AutoStep == Pre_BP_CV_AutoStep &&
                !m_bBP_CV_AutoStopEnable)
            {
                Watchdog_Start(WatchdogList.BP_Step_Timer);
                if (Watchdog_IsDetect(WatchdogList.BP_Step_Timer))
                    AlarmInsert((short)PortAlarm.Step_TimeOver_Error, AlarmLevel.Error);
            }
            else
            {
                Watchdog_Stop(WatchdogList.BP_Step_Timer, true);
            }

            if(m_eBP_CV_AutoStep != Pre_BP_CV_AutoStep)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortStepInfo, $"CV BP Step : {Pre_BP_CV_AutoStep} => {m_eBP_CV_AutoStep}");

            Pre_BP_CV_AutoStep = m_eBP_CV_AutoStep;
        }

        /// <summary>
        /// 장비 및 STK의 PIO 과정에서 Timeout 확인
        /// </summary>
        private void Auto_CV_AGVorOHT_PIO_WatchdogUpdate()
        {
            if ((m_eLP_CV_AutoStep >= LP_CV_AutoStep.Step920_OutMode_Check_PIO_TR && m_eLP_CV_AutoStep <= LP_CV_AutoStep.Step950_OutMode_Check_PIO_End) ||
                (m_eLP_CV_AutoStep >= LP_CV_AutoStep.Step220_InMode_Check_PIO_TR && m_eLP_CV_AutoStep <= LP_CV_AutoStep.Step250_InMode_Check_PIO_End))
            {
                Watchdog_Start(WatchdogList.AGVorOHT_PIO_Timer);
            }
            else
            {
                Watchdog_Stop(WatchdogList.AGVorOHT_PIO_Timer, true);
            }
        }
        private void Auto_CV_RackMaster_PIO_WatchdogUpdate()
        {
            if ((m_eOP_CV_AutoStep >= OP_CV_AutoStep.Step710_OutMode_Check_PIO_Busy && m_eOP_CV_AutoStep <= OP_CV_AutoStep.Step730_OutMode_Check_PIO_End) ||
                (m_eOP_CV_AutoStep >= OP_CV_AutoStep.Step410_InMode_Check_PIO_Busy && m_eOP_CV_AutoStep <= OP_CV_AutoStep.Step430_InMode_Check_PIO_End))
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
        private void Auto_CV_LP_StopInit()
        {
            m_bLP_CV_AutoRunning = false;
            m_bCycleRunning = false;
            Port_To_AGV_PIO_Init();
            Port_To_OHT_PIO_Init();
            Watchdog_Stop(WatchdogList.LP_Placement_ErrorTimer, false);
            Watchdog_Stop(WatchdogList.AGVorOHT_PIO_Timer, false);

            CMD_PortStop();
        }
        private void Auto_CV_OP_StopInit()
        {
            m_bOP_CV_AutoRunning = false;
            m_bCycleRunning = false;
            Port_To_RM_PIO_Init();
            Watchdog_Stop(WatchdogList.OP_Placement_ErrorTimer, false);
            Watchdog_Stop(WatchdogList.RackMaster_PIO_Timer, true);

            m_RMCSTIDRWTimer.Stop();
            m_RMCSTIDRWTimer.Reset();

            CMD_PortStop();
        }
        private void Auto_CV_BP_StopInit()
        {
            m_bBP_CV_AutoRunning = false;
            m_bCycleRunning = false;
            Watchdog_Stop(WatchdogList.BP_Placement_ErrorTimer, false);

            CMD_PortStop();
        }

        /// <summary>
        /// CV 부가 기능
        /// Stopper, Centering (기능만 있고 검증 X)
        /// </summary>
        /// <param name="eBufferCV"></param>
        private void StopperUp(BufferCV eBufferCV)
        {
            Interlock_SetStopperMove(BufferCV.Buffer_LP, CylCtrlList.BWD, false, true, InterlockFrom.ApplicationLoop);
            Interlock_SetStopperMove(BufferCV.Buffer_LP, CylCtrlList.FWD, true, true, InterlockFrom.ApplicationLoop);
        }
        private void StopperDown(BufferCV eBufferCV)
        {
            Interlock_SetStopperMove(BufferCV.Buffer_LP, CylCtrlList.BWD, true, true, InterlockFrom.ApplicationLoop);
            Interlock_SetStopperMove(BufferCV.Buffer_LP, CylCtrlList.FWD, false, true, InterlockFrom.ApplicationLoop);
        }
        private void CenteringFWD(BufferCV eBufferCV)
        {
            Interlock_SetCenteringMove(BufferCV.Buffer_LP, CylCtrlList.BWD, false, true, InterlockFrom.ApplicationLoop);
            Interlock_SetCenteringMove(BufferCV.Buffer_LP, CylCtrlList.FWD, true, true, InterlockFrom.ApplicationLoop);
        }
        private void CenteringBWD(BufferCV eBufferCV)
        {
            Interlock_SetCenteringMove(BufferCV.Buffer_LP, CylCtrlList.BWD, true, true, InterlockFrom.ApplicationLoop);
            Interlock_SetCenteringMove(BufferCV.Buffer_LP, CylCtrlList.FWD, false, true, InterlockFrom.ApplicationLoop);
        }
        
        /// <summary>
        /// CV 회전, 정지 기능
        /// </summary>
        /// <param name="eBufferCV"></param>
        private void CVHighSpeedFWDRolling(BufferCV eBufferCV)
        {
            if (GetMotionParam().GetBufferCVParam(eBufferCV).CVParam.InvCtrlMode == InvCtrlMode.IOControl)
            {
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.HighSpeedFWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.LowSpeedFWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.HighSpeedBWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.HighSpeedFWD, true, true, InterlockFrom.ApplicationLoop);
            }
            else
            {
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.FreqFWD, true, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.FreqBWD, false, true, InterlockFrom.ApplicationLoop);
            }
        }
        private void CVLowSpeedFWDRolling(BufferCV eBufferCV)
        {
            if (GetMotionParam().GetBufferCVParam(eBufferCV).CVParam.InvCtrlMode == InvCtrlMode.IOControl)
            {
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.LowSpeedBWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.LowSpeedFWD, true, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.HighSpeedBWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.HighSpeedFWD, false, true, InterlockFrom.ApplicationLoop);
            }
            else
            {
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.FreqFWD, true, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.FreqBWD, false, true, InterlockFrom.ApplicationLoop);
            }
        }
        private void CVHighSpeedBWDRolling(BufferCV eBufferCV)
        {
            if (GetMotionParam().GetBufferCVParam(eBufferCV).CVParam.InvCtrlMode == InvCtrlMode.IOControl)
            {
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.LowSpeedBWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.LowSpeedFWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.HighSpeedBWD, true, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.HighSpeedFWD, false, true, InterlockFrom.ApplicationLoop);
            }
            else
            {
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.FreqFWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.FreqBWD, true, true, InterlockFrom.ApplicationLoop);
            }
        }
        private void CVLowSpeedBWDRolling(BufferCV eBufferCV)
        {
            if (GetMotionParam().GetBufferCVParam(eBufferCV).CVParam.InvCtrlMode == InvCtrlMode.IOControl)
            {
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.LowSpeedBWD, true, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.LowSpeedFWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.HighSpeedBWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.HighSpeedFWD, false, true, InterlockFrom.ApplicationLoop);
            }
            else
            {
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.FreqFWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.FreqBWD, true, true, InterlockFrom.ApplicationLoop);
            }
        }
        private void CVStop(BufferCV eBufferCV)
        {
            Interlock_ConveyorMotionStop(eBufferCV, true, InterlockFrom.ApplicationLoop);
        }

        /// <summary>
        /// 실제 오토 공정 함수
        /// </summary>
        private void Auto_CV_Start_LP_Control()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                while (m_bLP_CV_AutoRunning || m_bCycleRunning)
                {
                    if (m_bLP_CV_AutoRunning && m_bCycleRunning)
                    {
                        Auto_CV_LP_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAndCycleCrash, $"LP CV");
                        break;
                    }

                    //Global Alarm Check
                    if (GetAlarmLevel() == AlarmLevel.Error)
                    {
                        Auto_CV_LP_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAlarmStop, $"LP CV");
                        break;
                    }

                    //My Type Alarm Check
                    Auto_CV_LP_Placement_Detect_AlarmCheck();
                    Auto_CV_LP_StepTimeOut_AlarmCheck();

                    //Update
                    Auto_CV_LPStopEnableUpdate();
                    Auto_CV_AGVorOHT_PIO_WatchdogUpdate();

                    if (IsAutoStopReq(!m_bCycleRunning))
                    {
                        if (m_bLP_CV_AutoStopEnable)
                        {
                            Auto_CV_LP_StopInit();
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AutoControlStopCommand, $"LP CV");
                            break;
                        }
                    }

                    switch (m_eLP_CV_AutoStep)
                    {
                        case LP_CV_AutoStep.Step000_Check_Direction:
                            {
                                Port_To_AGV_PIO_Init();
                                Port_To_OHT_PIO_Init();

                                if (GetOperationDirection() == PortDirection.Input)
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step100_InMode_Check_LP_CST_Load_And_Pose;
                                else
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose;
                            }
                            break;
                        case LP_CV_AutoStep.Step100_InMode_Check_LP_CST_Load_And_Pose:
                            {
                                if (Carrier_CheckLP_ExistProduct(true))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step400_InMode_Ready_CST_ID_Read;
                                else if (Carrier_CheckLP_ExistProduct(false) && !Carrier_CheckLP_ExistID())
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step110_InMode_Move_LP_X_LP;
                                else if (Carrier_CheckLP_ExistProduct(false) && Carrier_CheckLP_ExistID())
                                {
                                    LP_CarrierID = string.Empty;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step110_InMode_Move_LP_X_LP:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_LP_X))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step120_InMode_Move_LP_Z_Down;
                                    break;
                                }

                                if (X_Axis_MotionAndDone(PortAxis.Buffer_LP_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step120_InMode_Move_LP_Z_Down;
                            }
                            break;
                        case LP_CV_AutoStep.Step120_InMode_Move_LP_Z_Down:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_LP_Z))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step130_InMode_Move_LP_T_180_Deg;
                                    break;
                                }

                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_LP_Z, Teaching_Z_Pos.Down_Pos))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step130_InMode_Move_LP_T_180_Deg;
                            }
                            break;
                        case LP_CV_AutoStep.Step130_InMode_Move_LP_T_180_Deg:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_LP_T))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step140_InMode_Move_LP_Stopper_Up;
                                    break;
                                }

                                if (T_Axis_MotionAndDone(PortAxis.Buffer_LP_T, Teaching_T_Pos.Degree180_Pos))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step140_InMode_Move_LP_Stopper_Up;
                            }
                            break;
                        case LP_CV_AutoStep.Step140_InMode_Move_LP_Stopper_Up:
                            {
                                if (!GetMotionParam().IsStopperEnable(BufferCV.Buffer_LP))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step150_InMode_Move_LP_Centering_BWD;
                                    break;
                                }

                                StopperUp(BufferCV.Buffer_LP);
                                m_eLP_CV_AutoStep = LP_CV_AutoStep.Step150_InMode_Move_LP_Centering_BWD;
                            }
                            break;
                        case LP_CV_AutoStep.Step150_InMode_Move_LP_Centering_BWD:
                            {
                                if (!GetMotionParam().IsCenteringEnable(BufferCV.Buffer_LP))
                                {
                                    if(GetPortOperationMode() == PortOperationMode.Conveyor && !m_bCycleRunning)
                                        m_eLP_CV_AutoStep = AGVFullFlagOption ? LP_CV_AutoStep.Step200_InMode_Await_PIO_CS : LP_CV_AutoStep.Step210_InMode_Check_PIO_Valid;
                                    else if(GetPortOperationMode() == PortOperationMode.MGV || m_bCycleRunning)
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step300_InMode_Await_MGV_CST_Load;
                                    break;
                                }

                                CenteringBWD(BufferCV.Buffer_LP);

                                if (GetPortOperationMode() == PortOperationMode.Conveyor && !m_bCycleRunning)
                                    m_eLP_CV_AutoStep = AGVFullFlagOption ? LP_CV_AutoStep.Step200_InMode_Await_PIO_CS : LP_CV_AutoStep.Step210_InMode_Check_PIO_Valid;
                                else if (GetPortOperationMode() == PortOperationMode.MGV || m_bCycleRunning)
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step300_InMode_Await_MGV_CST_Load;
                            }
                            break;
                        case LP_CV_AutoStep.Step200_InMode_Await_PIO_CS:
                            {
                                if (Is_EquipType_To_Port_PIO_CSFlag() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step210_InMode_Check_PIO_Valid;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                                else if (Carrier_CheckLP_ExistProduct(true))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step250_InMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step210_InMode_Check_PIO_Valid:
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
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step220_InMode_Check_PIO_TR;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                                else if (Carrier_CheckLP_ExistProduct(true))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step250_InMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        //case LP_CV_AutoStep.Step211_InMode_Call_AGV_Load_Req:
                        //    {
                        //        if (Carrier_CheckLP_ExistProduct(false) &&
                        //            !Is_LightCurtain_or_Hoist_SensorCheck())
                        //        {
                        //            Set_Port_To_AGVorOHT_PIO_Load_ReqFlag(true);

                        //            if (Is_AGVorOHT_To_Port_PIO_TR_ReqFlag())
                        //            {
                        //                m_eLP_CV_AutoStep = LP_CV_AutoStep.Step220_InMode_Check_PIO_TR;
                        //                Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                        //            }
                        //        }
                        //        else if (Carrier_CheckLP_ExistProduct(true))
                        //        {
                        //            Set_Port_To_AGVorOHT_PIO_Load_ReqFlag(false);
                        //            m_eLP_CV_AutoStep = LP_CV_AutoStep.Step400_InMode_Ready_CST_ID_Read;
                        //            Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                        //        }
                        //        break;
                        //    }
                        case LP_CV_AutoStep.Step220_InMode_Check_PIO_TR:
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
                                    CVHighSpeedFWDRolling(BufferCV.Buffer_LP);
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step230_InMode_Check_PIO_Busy;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step230_InMode_Check_PIO_Busy:
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
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step240_InMode_Check_PIO_Complete;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step240_InMode_Check_PIO_Complete:
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

                                if (GetMotionParam().IsSlowSensorEnable(BufferCV.Buffer_LP) && Sensor_LP_CV_SLOW)
                                    CVLowSpeedFWDRolling(BufferCV.Buffer_LP);

                                //PIO 중 자재가 생기면 Off
                                if (Carrier_CheckLP_ExistProduct(true))
                                    Set_Port_To_Equip_PIO_Load_ReqFlag(false);

                                if (Sensor_LP_CV_STOP)
                                    CVStop(BufferCV.Buffer_LP);

                                if (!Is_EquipType_To_Port_PIO_TR_ReqFlag() &&
                                    !Is_EquipType_To_Port_PIO_BusyFlag() &&
                                    Is_EquipType_To_Port_PIO_Complete() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck() &&
                                    !Is_Port_To_Equip_PIO_Load_ReqFlag())
                                {
                                    Set_Port_To_Equip_PIO_ReadyFlag(false);
                                    //Set_Port_To_Equip_PIO_ESFlag(false);
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step250_InMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step250_InMode_Check_PIO_End:
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
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step260_InMode_Check_CST_Load_And_Safe;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step260_InMode_Check_CST_Load_And_Safe:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) && !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step400_InMode_Ready_CST_ID_Read;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) && !Is_LightCurtain_or_Hoist_SensorCheck())
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step400_InMode_Ready_CST_ID_Read;
                            }
                            break;
                        case LP_CV_AutoStep.Step400_InMode_Ready_CST_ID_Read:
                            {
                                if (TAG_READ_INIT())
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step410_InMode_Start_CST_ID_Read;

                                //if (m_TagReader_Interface.GetReaderEquipType() == TagReader.TagReaderType.RFID)
                                //    m_TagReader_Interface.GetRFIDReader().n_RFIDReadCount = 0;
                                //m_eLP_CV_AutoStep = LP_CV_AutoStep.Step410_InMode_Start_CST_ID_Read;
                            }
                            break;
                        case LP_CV_AutoStep.Step410_InMode_Start_CST_ID_Read:
                            {
                                if (Carrier_CheckLP_ExistProduct(true))
                                {
                                    if (TAG_READ_TRY(TAG_ID_READ_SET_SECTION.LP))
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step420_InMode_Move_LP_Centering_FWD;

                                    //if (m_TagReader_Interface.GetReaderEquipType() == TagReader.TagReaderType.RFID)
                                    //{
                                    //    var Reader = m_TagReader_Interface.GetRFIDReader();

                                    //    if (Reader.IsConnected())
                                    //    {
                                    //        Reader.n_RFIDReadCount++;

                                    //        Reader.TagRead();

                                    //        if (Reader.n_RFIDReadCount > 5)
                                    //        {
                                    //            //AlarmInsert((short)PortAlarm.Tag_Read_Fail, AlarmLevel.Error);
                                    //            AlarmInsert((short)PortAlarm.Tag_Read_Fail, AlarmLevel.Warning);
                                    //            LP_CarrierID = "CST_ID_READ_FAIL";
                                    //            m_eLP_CV_AutoStep = LP_CV_AutoStep.Step420_InMode_Move_LP_Centering_FWD;
                                    //        }
                                    //        else if (Reader.IsTagReadSuccess())
                                    //        {
                                    //            string TagValue = Reader.GetTag();
                                    //            LP_CarrierID = TagValue;

                                    //            m_eLP_CV_AutoStep = LP_CV_AutoStep.Step420_InMode_Move_LP_Centering_FWD;
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        AlarmInsert((short)PortAlarm.Tag_Disconnection, AlarmLevel.Error);
                                    //    }
                                    //}
                                    //else if (m_TagReader_Interface.GetReaderEquipType() == TagReader.TagReaderType.None)
                                    //{
                                    //    LP_CarrierID = "TAG_READER_NOT_SET";
                                    //    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step420_InMode_Move_LP_Centering_FWD;
                                    //}
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step420_InMode_Move_LP_Centering_FWD:
                            if (!GetMotionParam().IsCenteringEnable(BufferCV.Buffer_LP))
                            {
                                m_eLP_CV_AutoStep = LP_CV_AutoStep.Step430_InMode_Move_LP_X_OP;
                                break;
                            }

                            CenteringFWD(BufferCV.Buffer_LP);
                            m_eLP_CV_AutoStep = LP_CV_AutoStep.Step430_InMode_Move_LP_X_OP;
                            break;
                        case LP_CV_AutoStep.Step430_InMode_Move_LP_X_OP:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_LP_X))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step440_InMode_Move_LP_Z_Up;
                                    break;
                                }

                                if (X_Axis_MotionAndDone(PortAxis.Buffer_LP_X, Teaching_X_Pos.OP_Pos))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step440_InMode_Move_LP_Z_Up;
                            }
                            break;
                        case LP_CV_AutoStep.Step440_InMode_Move_LP_Z_Up:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_LP_Z))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step450_InMode_Move_LP_T_0_Deg;
                                    break;
                                }

                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_LP_Z, Teaching_Z_Pos.Up_Pos))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step450_InMode_Move_LP_T_0_Deg;
                            }
                            break;
                        case LP_CV_AutoStep.Step450_InMode_Move_LP_T_0_Deg:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_LP_T))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step460_InMode_Move_LP_Stopper_Down;
                                    break;
                                }

                                if (T_Axis_MotionAndDone(PortAxis.Buffer_LP_T, Teaching_T_Pos.Degree0_Pos))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step460_InMode_Move_LP_Stopper_Down;
                            }
                            break;
                        case LP_CV_AutoStep.Step460_InMode_Move_LP_Stopper_Down:
                            {
                                if (!GetMotionParam().IsStopperEnable(BufferCV.Buffer_LP))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step470_InMode_Move_LP_Centering_BWD;
                                    break;
                                }

                                StopperDown(BufferCV.Buffer_LP);
                                m_eLP_CV_AutoStep = LP_CV_AutoStep.Step470_InMode_Move_LP_Centering_BWD;
                            }
                            break;
                        case LP_CV_AutoStep.Step470_InMode_Move_LP_Centering_BWD:
                            if (!GetMotionParam().IsCenteringEnable(BufferCV.Buffer_LP))
                            {
                                m_eLP_CV_AutoStep = LP_CV_AutoStep.Step500_InMode_Await_OPorBP_Load_Req;
                                break;
                            }

                            CenteringBWD(BufferCV.Buffer_LP);
                            m_eLP_CV_AutoStep = LP_CV_AutoStep.Step500_InMode_Await_OPorBP_Load_Req;
                            break;
                        case LP_CV_AutoStep.Step500_InMode_Await_OPorBP_Load_Req:
                            {
                                if(GetMotionParam().IsBPCVUsed())
                                {
                                    if (m_eBP_CV_AutoStep == BP_CV_AutoStep.Step200_InMode_Start_CST_Pass_From_LP)
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step510_InMode_Move_LP_CV_Rolling;
                                }
                                else
                                {
                                    if(m_eOP_CV_AutoStep == OP_CV_AutoStep.Step200_InMode_Call_LPorBP_Load_Req)
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step510_InMode_Move_LP_CV_Rolling;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step510_InMode_Move_LP_CV_Rolling:
                            {
                                CVHighSpeedFWDRolling(BufferCV.Buffer_LP);

                                if (GetMotionParam().IsBPCVUsed())
                                {
                                    if (m_eBP_CV_AutoStep == BP_CV_AutoStep.Step210_InMode_Move_BP_CV_Rolling)
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step520_InMode_Move_LP_CV_Stop;
                                }
                                else
                                {
                                    if (m_eOP_CV_AutoStep == OP_CV_AutoStep.Step210_InMode_Move_OP_Rolling)
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step520_InMode_Move_LP_CV_Stop;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step520_InMode_Move_LP_CV_Stop:
                            {
                                if(Carrier_CheckLP_ExistProduct(false, false))
                                {
                                    CVStop(BufferCV.Buffer_LP);
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step530_InMode_Send_LP_CST_ID;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step530_InMode_Send_LP_CST_ID:
                            {
                                if (Carrier_CheckLP_ExistProduct(false, false) &&
                                    Carrier_CheckLP_ExistID())
                                {
                                    if (GetMotionParam().IsBPCVUsed())
                                    {
                                        string LPCarrierID = LP_CarrierID;
                                        Carrier_SetBP_CarrierID(0, LPCarrierID);
                                        if(Carrier_GetBP_CarrierID(0) == LPCarrierID)
                                            m_eLP_CV_AutoStep = LP_CV_AutoStep.Step540_InMode_Clear_LP_CST_ID;
                                    }
                                    else
                                    {
                                        string LPCarrierID = LP_CarrierID;
                                        OP_CarrierID = LPCarrierID;
                                        if (OP_CarrierID == LPCarrierID)
                                            m_eLP_CV_AutoStep = LP_CV_AutoStep.Step540_InMode_Clear_LP_CST_ID;
                                    }
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step540_InMode_Clear_LP_CST_ID:
                            {
                                if (Carrier_CheckLP_ExistProduct(false) &&
                                    Carrier_CheckLP_ExistID())
                                {
                                    LP_CarrierID = string.Empty;
                                }
                                else if (Carrier_CheckLP_ExistProduct(false) &&
                                        !Carrier_CheckLP_ExistID())
                                {
                                    if (!m_bCycleRunning)
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step000_Check_Direction;
                                    else
                                    {
                                        if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                        {
                                            //Inmode인 경우에는 OP에서 카운트를 증가
                                            //m_CycleControlProgressCount++;
                                            //-----------Direction Change 실행
                                            Port_To_AGV_PIO_Init();
                                            Port_To_OHT_PIO_Init();
                                            m_eLP_CV_AutoStep = LP_CV_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose;
                                        }
                                        else
                                        {
                                            //Inmode인 경우에는 OP에서 싸이클 종료
                                            //m_CycleControlProgressCount++;
                                            //m_bAutoManualCycleStopReq = true;
                                            //m_bCycleRunning = false;
                                        }
                                    }
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) && Carrier_CheckLP_ExistID())
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step800_OutMode_Move_LP_Centering_FWD;
                                else if (Carrier_CheckLP_ExistProduct(false) && !Carrier_CheckLP_ExistID())
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step610_OutMode_Move_LP_X_OP;
                                else if (Carrier_CheckLP_ExistProduct(true) && !Carrier_CheckLP_ExistID())
                                {
                                    AlarmInsert((short)PortAlarm.LP_No_Cassette_ID_Error, AlarmLevel.Error);
                                }
                                else if (Carrier_CheckLP_ExistProduct(false) && Carrier_CheckLP_ExistID())
                                {
                                    LP_CarrierID = string.Empty;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step610_OutMode_Move_LP_X_OP:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_LP_X))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step620_OutMode_Move_LP_Z_Up;
                                    break;
                                }

                                if (X_Axis_MotionAndDone(PortAxis.Buffer_LP_X, Teaching_X_Pos.OP_Pos))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step620_OutMode_Move_LP_Z_Up;
                            }
                            break;
                        case LP_CV_AutoStep.Step620_OutMode_Move_LP_Z_Up:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_LP_Z))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step630_OutMode_Move_LP_T_0_Deg;
                                    break;
                                }

                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_LP_Z, Teaching_Z_Pos.Up_Pos))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step630_OutMode_Move_LP_T_0_Deg;
                            }
                            break;
                        case LP_CV_AutoStep.Step630_OutMode_Move_LP_T_0_Deg:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_LP_T))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step640_OutMode_Move_LP_Stopper_Up;
                                    break;
                                }

                                if (T_Axis_MotionAndDone(PortAxis.Buffer_LP_T, Teaching_T_Pos.Degree0_Pos))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step640_OutMode_Move_LP_Stopper_Up;
                            }
                            break;
                        case LP_CV_AutoStep.Step640_OutMode_Move_LP_Stopper_Up:
                            {
                                if (!GetMotionParam().IsStopperEnable(BufferCV.Buffer_LP))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step650_OutMode_Move_LP_Centering_BWD;
                                    break;
                                }

                                StopperUp(BufferCV.Buffer_LP);
                                m_eLP_CV_AutoStep = LP_CV_AutoStep.Step650_OutMode_Move_LP_Centering_BWD;
                            }
                            break;
                        case LP_CV_AutoStep.Step650_OutMode_Move_LP_Centering_BWD:
                            {
                                if (!GetMotionParam().IsCenteringEnable(BufferCV.Buffer_LP))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step700_OutMode_Call_OPorBP_Load_Req;
                                    break;
                                }

                                CenteringBWD(BufferCV.Buffer_LP);
                                m_eLP_CV_AutoStep = LP_CV_AutoStep.Step700_OutMode_Call_OPorBP_Load_Req;
                            }
                            break;
                        case LP_CV_AutoStep.Step700_OutMode_Call_OPorBP_Load_Req:
                            {
                                if (GetMotionParam().IsBPCVUsed())
                                {
                                    if (m_eBP_CV_AutoStep == BP_CV_AutoStep.Step800_OutMode_Start_CST_Pass_To_LP)
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step710_OutMode_Move_LP_Rolling;
                                }
                                else
                                {
                                    if (m_eOP_CV_AutoStep == OP_CV_AutoStep.Step910_OutMode_Move_OP_CV_Rolling)
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step710_OutMode_Move_LP_Rolling;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step710_OutMode_Move_LP_Rolling:
                            {
                                CVHighSpeedBWDRolling(BufferCV.Buffer_LP);

                                if (GetMotionParam().IsBPCVUsed())
                                {
                                    if (m_eBP_CV_AutoStep == BP_CV_AutoStep.Step810_OutMode_Move_BP_CV_Rolling)
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step720_OutMode_Move_LP_CV_Stop;
                                }
                                else
                                {
                                    if (m_eOP_CV_AutoStep == OP_CV_AutoStep.Step920_OutMode_Move_OP_CV_Stop)
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step720_OutMode_Move_LP_CV_Stop;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step720_OutMode_Move_LP_CV_Stop:
                            {
                                //OUT 방향인 경우 Sensor 뒤집어서 확인
                                // IN - STOP
                                // <-<-<-<-<-<-<-
                                if (GetMotionParam().IsSlowSensorEnable(BufferCV.Buffer_LP) && Sensor_LP_CV_SLOW)
                                    CVLowSpeedBWDRolling(BufferCV.Buffer_LP);

                                if (Sensor_LP_CV_IN)
                                {
                                    CVStop(BufferCV.Buffer_LP);
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step730_OutMode_Check_CST_ID;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step730_OutMode_Check_CST_ID:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) &&
                                    Carrier_CheckLP_ExistID())
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step800_OutMode_Move_LP_Centering_FWD;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step800_OutMode_Move_LP_Centering_FWD:
                            {
                                if (!GetMotionParam().IsCenteringEnable(BufferCV.Buffer_LP))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step810_OutMode_Move_LP_X_LP;
                                    break;
                                }

                                CenteringFWD(BufferCV.Buffer_LP);
                                m_eLP_CV_AutoStep = LP_CV_AutoStep.Step810_OutMode_Move_LP_X_LP;
                            }
                            break;
                        case LP_CV_AutoStep.Step810_OutMode_Move_LP_X_LP:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_LP_X))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step820_OutMode_Move_LP_Z_Down;
                                    break;
                                }

                                if (X_Axis_MotionAndDone(PortAxis.Buffer_LP_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step820_OutMode_Move_LP_Z_Down;
                            }
                            break;
                        case LP_CV_AutoStep.Step820_OutMode_Move_LP_Z_Down:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_LP_Z))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step830_OutMode_Move_LP_T_180_Deg;
                                    break;
                                }

                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_LP_Z, Teaching_Z_Pos.Down_Pos))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step830_OutMode_Move_LP_T_180_Deg;
                            }
                            break;
                        case LP_CV_AutoStep.Step830_OutMode_Move_LP_T_180_Deg:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_LP_T))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step840_OutMode_Move_LP_Stopper_Down;
                                    break;
                                }

                                if (T_Axis_MotionAndDone(PortAxis.Buffer_LP_T, Teaching_T_Pos.Degree180_Pos))
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step840_OutMode_Move_LP_Stopper_Down;
                            }
                            break;
                        case LP_CV_AutoStep.Step840_OutMode_Move_LP_Stopper_Down:
                            {
                                if (!GetMotionParam().IsStopperEnable(BufferCV.Buffer_LP))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step850_OutMode_Move_LP_Centering_BWD;
                                    break;
                                }

                                StopperDown(BufferCV.Buffer_LP);
                                m_eLP_CV_AutoStep = LP_CV_AutoStep.Step850_OutMode_Move_LP_Centering_BWD;
                            }
                            break;
                        case LP_CV_AutoStep.Step850_OutMode_Move_LP_Centering_BWD:
                            {
                                if (!GetMotionParam().IsCenteringEnable(BufferCV.Buffer_LP))
                                {
                                    if (GetPortOperationMode() == PortOperationMode.Conveyor && !m_bCycleRunning)
                                    {
                                        m_eLP_CV_AutoStep = AGVFullFlagOption ? LP_CV_AutoStep.Step900_OutMode_Await_PIO_CS : LP_CV_AutoStep.Step910_OutMode_Check_PIO_Valid;
                                    }
                                    else if (GetPortOperationMode() == PortOperationMode.MGV || m_bCycleRunning)
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step990_OutMode_Await_MGV_CST_Unload;
                                    break;
                                }

                                CenteringBWD(BufferCV.Buffer_LP);

                                if (GetPortOperationMode() == PortOperationMode.Conveyor)
                                {
                                    if (!m_bCycleRunning)
                                        m_eLP_CV_AutoStep = AGVFullFlagOption ? LP_CV_AutoStep.Step900_OutMode_Await_PIO_CS : LP_CV_AutoStep.Step910_OutMode_Check_PIO_Valid;
                                    else
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step990_OutMode_Await_MGV_CST_Unload;
                                }
                                else if (GetPortOperationMode() == PortOperationMode.MGV)
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step990_OutMode_Await_MGV_CST_Unload;
                            }
                            break;
                        case LP_CV_AutoStep.Step900_OutMode_Await_PIO_CS:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) &&
                                    Is_EquipType_To_Port_PIO_CSFlag())
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step910_OutMode_Check_PIO_Valid;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                                else if (Carrier_CheckLP_ExistProduct(false))
                                {
                                    //PIO 대기 중 제품이 사라진 경우 ID를 삭제 후 다시 첫 스텝으로 회귀
                                    if (Carrier_CheckLP_ExistID())
                                        LP_CarrierID = string.Empty;

                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step000_Check_Direction;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step910_OutMode_Check_PIO_Valid:
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
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step920_OutMode_Check_PIO_TR;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                                else if (Carrier_CheckLP_ExistProduct(false))
                                {
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step950_OutMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        //case LP_CV_AutoStep.Step911_OutMode_Call_AGV_Unload_Req:
                        //    {
                        //        if (Carrier_CheckLP_ExistProduct(true) &&
                        //            !Is_LightCurtain_or_Hoist_SensorCheck())
                        //        {
                        //            Set_Port_To_AGVorOHT_PIO_Unload_ReqFlag(true);

                        //            if (Is_AGVorOHT_To_Port_PIO_TR_ReqFlag())
                        //            {
                        //                m_eLP_CV_AutoStep = LP_CV_AutoStep.Step920_OutMode_Check_PIO_TR;
                        //                Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                        //            }
                        //        }
                        //        else if (Carrier_CheckLP_ExistProduct(false))
                        //        {
                        //            Set_Port_To_AGVorOHT_PIO_Unload_ReqFlag(false);
                        //            m_eLP_CV_AutoStep = LP_CV_AutoStep.Step950_OutMode_Check_PIO_End;
                        //            Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                        //        }
                        //    }
                        //    break;
                        case LP_CV_AutoStep.Step920_OutMode_Check_PIO_TR:
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
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step930_OutMode_Check_PIO_Busy;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step930_OutMode_Check_PIO_Busy:
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
                                    CVHighSpeedBWDRolling(BufferCV.Buffer_LP);
                                    //Set_Port_To_Equip_PIO_Unload_ReqFlag(false);
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step940_OutMode_Check_PIO_Complete;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step940_OutMode_Check_PIO_Complete:
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
                                if (Carrier_CheckLP_ExistProduct(false))
                                    Set_Port_To_Equip_PIO_Unload_ReqFlag(false);

                                if (Carrier_CheckLP_ExistProduct(false))
                                    LP_CarrierID = string.Empty;

                                if (!Is_EquipType_To_Port_PIO_TR_ReqFlag() &&
                                    !Is_EquipType_To_Port_PIO_BusyFlag() &&
                                    Is_EquipType_To_Port_PIO_Complete() &&
                                    !Is_LightCurtain_or_Hoist_SensorCheck() &&
                                    !Is_Port_To_Equip_PIO_Unload_ReqFlag())
                                {
                                    CVStop(BufferCV.Buffer_LP);
                                    Set_Port_To_Equip_PIO_ReadyFlag(false);
                                    //Set_Port_To_Equip_PIO_ESFlag(false);
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step950_OutMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step950_OutMode_Check_PIO_End:
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
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step960_OutMode_Check_LP_CST_Unload_And_Safe;
                                }
                            }
                            break;
                        case LP_CV_AutoStep.Step960_OutMode_Check_LP_CST_Unload_And_Safe:
                        case LP_CV_AutoStep.Step990_OutMode_Await_MGV_CST_Unload:
                            {
                                if (Carrier_CheckLP_ExistProduct(false) && !Is_LightCurtain_or_Hoist_SensorCheck() && !m_bCycleRunning)
                                {
                                    LP_CarrierID = string.Empty;
                                    m_eLP_CV_AutoStep = LP_CV_AutoStep.Step000_Check_Direction;
                                }
                                else if(!Is_LightCurtain_or_Hoist_SensorCheck() && m_bCycleRunning)
                                {
                                    LP_CarrierID = string.Empty;
                                    if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                    {
                                        //Outmode인 경우에는 LP에서 카운트를 증가
                                        m_CycleControlProgressCount++;
                                        //-----------Direction Change 실행
                                        Port_To_AGV_PIO_Init();
                                        Port_To_OHT_PIO_Init();
                                        GetMotionParam().ePortDirection = PortDirection.Input;
                                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step100_InMode_Check_LP_CST_Load_And_Pose;
                                    }
                                    else
                                    {
                                        //Outmode인 경우에는 LP에서 싸이클 종료
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
        private void Auto_CV_Start_OP_Control()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                while (m_bOP_CV_AutoRunning || m_bCycleRunning)
                {
                    if (m_bOP_CV_AutoRunning && m_bCycleRunning)
                    {
                        Auto_CV_OP_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAndCycleCrash, $"OP CV");
                        break;
                    }

                    //Global Alarm Check
                    if (GetAlarmLevel() == AlarmLevel.Error)
                    {
                        Auto_CV_OP_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAlarmStop, $"OP CV");
                        break;
                    }

                    //My Type Alarm Check
                    Auto_CV_OP_Placement_Detect_AlarmCheck();
                    Auto_CV_OP_StepTimeOut_AlarmCheck();

                    //Update
                    Auto_CV_OPStopEnableUpdate();
                    Auto_CV_RackMaster_PIO_WatchdogUpdate();

                    if (IsAutoStopReq(!m_bCycleRunning))
                    {
                        if (m_bOP_CV_AutoStopEnable)
                        {
                            Auto_CV_OP_StopInit();
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AutoControlStopCommand, $"OP CV");
                            break;
                        }
                    }

                    switch (m_eOP_CV_AutoStep)
                    {
                        case OP_CV_AutoStep.Step000_Check_Direction:
                            {
                                Port_To_RM_PIO_Init();

                                if (GetOperationDirection() == PortDirection.Input)
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose;
                                else
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose;
                            }
                            break;
                        case OP_CV_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) && Carrier_CheckOP_ExistID())
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step300_InMode_Move_OP_Centering_FWD;
                                else if (Carrier_CheckOP_ExistProduct(false) && !Carrier_CheckOP_ExistID())
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step110_InMode_Move_OP_X_LP;
                                else if (Carrier_CheckOP_ExistProduct(true) && !Carrier_CheckOP_ExistID())
                                {
                                    AlarmInsert((short)PortAlarm.OP_No_Cassette_ID_Error, AlarmLevel.Error);
                                }
                                else if (Carrier_CheckOP_ExistProduct(false) && Carrier_CheckOP_ExistID())
                                {
                                    OP_CarrierID = string.Empty;
                                }
                            }
                            break;
                        case OP_CV_AutoStep.Step110_InMode_Move_OP_X_LP:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_OP_X))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step120_InMode_Move_OP_Z_Down;
                                    break;
                                }

                                if (X_Axis_MotionAndDone(PortAxis.Buffer_OP_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step120_InMode_Move_OP_Z_Down;
                            }
                            break;
                        case OP_CV_AutoStep.Step120_InMode_Move_OP_Z_Down:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_OP_Z))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step130_InMode_Move_OP_T_180_Deg;
                                    break;
                                }

                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_OP_Z, Teaching_Z_Pos.Down_Pos))
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step130_InMode_Move_OP_T_180_Deg;
                            }
                            break;
                        case OP_CV_AutoStep.Step130_InMode_Move_OP_T_180_Deg:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_OP_T))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step140_InMode_Move_OP_Stopper_Up;
                                    break;
                                }

                                if (T_Axis_MotionAndDone(PortAxis.Buffer_OP_T, Teaching_T_Pos.Degree180_Pos))
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step140_InMode_Move_OP_Stopper_Up;
                            }
                            break;
                        case OP_CV_AutoStep.Step140_InMode_Move_OP_Stopper_Up:
                            {
                                if (!GetMotionParam().IsStopperEnable(BufferCV.Buffer_OP))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step150_InMode_Move_OP_Centering_BWD;
                                    break;
                                }

                                StopperUp(BufferCV.Buffer_OP);
                                m_eOP_CV_AutoStep = OP_CV_AutoStep.Step150_InMode_Move_OP_Centering_BWD;
                            }
                            break;
                        case OP_CV_AutoStep.Step150_InMode_Move_OP_Centering_BWD:
                            {
                                if (!GetMotionParam().IsCenteringEnable(BufferCV.Buffer_OP))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step200_InMode_Call_LPorBP_Load_Req;
                                    break;
                                }

                                CenteringBWD(BufferCV.Buffer_OP);

                                m_eOP_CV_AutoStep = OP_CV_AutoStep.Step200_InMode_Call_LPorBP_Load_Req;
                            }
                            break;
                        case OP_CV_AutoStep.Step200_InMode_Call_LPorBP_Load_Req:
                            {
                                if (GetMotionParam().IsBPCVUsed())
                                {
                                    if (m_eBP_CV_AutoStep == BP_CV_AutoStep.Step300_InMode_Start_CST_Pass_To_OP)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step210_InMode_Move_OP_Rolling;
                                }
                                else
                                {
                                    if (m_eLP_CV_AutoStep == LP_CV_AutoStep.Step510_InMode_Move_LP_CV_Rolling)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step210_InMode_Move_OP_Rolling;
                                }
                            }
                            break;
                        case OP_CV_AutoStep.Step210_InMode_Move_OP_Rolling:
                            {
                                CVHighSpeedFWDRolling(BufferCV.Buffer_OP);

                                if (GetMotionParam().IsBPCVUsed())
                                {
                                    if (m_eBP_CV_AutoStep == BP_CV_AutoStep.Step310_InMode_Move_BP_CV_Rolling)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step220_InMode_Move_OP_CV_Stop;
                                }
                                else
                                {
                                    if (m_eLP_CV_AutoStep == LP_CV_AutoStep.Step520_InMode_Move_LP_CV_Stop ||
                                        m_eLP_CV_AutoStep == LP_CV_AutoStep.Step530_InMode_Send_LP_CST_ID ||
                                        m_eLP_CV_AutoStep == LP_CV_AutoStep.Step540_InMode_Clear_LP_CST_ID)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step220_InMode_Move_OP_CV_Stop;
                                }
                            }
                            break;
                        case OP_CV_AutoStep.Step220_InMode_Move_OP_CV_Stop:
                            {
                                if(GetMotionParam().IsSlowSensorEnable(BufferCV.Buffer_OP) && Sensor_OP_CV_SLOW)
                                    CVLowSpeedFWDRolling(BufferCV.Buffer_OP);

                                if (Sensor_OP_CV_STOP)
                                {
                                    CVStop(BufferCV.Buffer_OP);
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step230_InMode_Check_CST_ID;
                                }
                            }
                            break;
                        case OP_CV_AutoStep.Step230_InMode_Check_CST_ID:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) &&
                                    Carrier_CheckOP_ExistID())
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step300_InMode_Move_OP_Centering_FWD;
                                }
                            }
                            break;
                        case OP_CV_AutoStep.Step300_InMode_Move_OP_Centering_FWD:
                            {
                                if (!GetMotionParam().IsCenteringEnable(BufferCV.Buffer_OP))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step310_InMode_Move_OP_X_OP;
                                    break;
                                }

                                CenteringFWD(BufferCV.Buffer_OP);
                                m_eOP_CV_AutoStep = OP_CV_AutoStep.Step310_InMode_Move_OP_X_OP;
                            }
                            break;
                        case OP_CV_AutoStep.Step310_InMode_Move_OP_X_OP:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_OP_X))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step320_InMode_Move_OP_Z_Up;
                                    break;
                                }

                                if (X_Axis_MotionAndDone(PortAxis.Buffer_OP_X, Teaching_X_Pos.OP_Pos))
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step320_InMode_Move_OP_Z_Up;
                            }
                            break;
                        case OP_CV_AutoStep.Step320_InMode_Move_OP_Z_Up:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_OP_Z))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step330_InMode_Move_OP_T_0_Deg;
                                    break;
                                }

                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_OP_Z, Teaching_Z_Pos.Up_Pos))
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step330_InMode_Move_OP_T_0_Deg;
                            }
                            break;
                        case OP_CV_AutoStep.Step330_InMode_Move_OP_T_0_Deg:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_OP_T))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step340_InMode_Move_OP_Stopper_Down;
                                    break;
                                }

                                if (T_Axis_MotionAndDone(PortAxis.Buffer_OP_T, Teaching_T_Pos.Degree180_Pos))
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step340_InMode_Move_OP_Stopper_Down;
                            }
                            break;
                        case OP_CV_AutoStep.Step340_InMode_Move_OP_Stopper_Down:
                            {
                                if (!GetMotionParam().IsStopperEnable(BufferCV.Buffer_OP))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step350_InMode_Move_OP_Centering_BWD;
                                    break;
                                }

                                StopperDown(BufferCV.Buffer_OP);
                                m_eOP_CV_AutoStep = OP_CV_AutoStep.Step350_InMode_Move_OP_Centering_BWD;
                            }
                            break;
                        case OP_CV_AutoStep.Step350_InMode_Move_OP_Centering_BWD:
                            {
                                if (!GetMotionParam().IsCenteringEnable(BufferCV.Buffer_OP))
                                {
                                    if (GetPortOperationMode() == PortOperationMode.Conveyor && !m_bCycleRunning)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step400_InMode_Await_PIO_TR_REQ;
                                    else if(GetPortOperationMode() == PortOperationMode.MGV || m_bCycleRunning)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step440_InMode_Check_OP_CST_Unload_And_Safe;
                                    break;
                                }

                                CenteringBWD(BufferCV.Buffer_OP);

                                if (GetPortOperationMode() == PortOperationMode.Conveyor && !m_bCycleRunning)
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step400_InMode_Await_PIO_TR_REQ;
                                else if (GetPortOperationMode() == PortOperationMode.MGV || m_bCycleRunning)
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step440_InMode_Check_OP_CST_Unload_And_Safe;
                            }
                            break;
                        case OP_CV_AutoStep.Step400_InMode_Await_PIO_TR_REQ:
                            {
                                if (INPUT_DIR_STK_TR_REQ_Await())
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step410_InMode_Check_PIO_Busy;
                            }
                            break;
                        case OP_CV_AutoStep.Step410_InMode_Check_PIO_Busy:
                            {
                                if (INPUT_DIR_STK_BUSY_Check())
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step420_InMode_Check_PIO_Complete;
                            }
                            break;
                        case OP_CV_AutoStep.Step420_InMode_Check_PIO_Complete:
                            {
                                if (INPUT_DIR_STK_COMPLETE_Check())
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step430_InMode_Check_PIO_End;

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
                                //        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step430_InMode_Check_PIO_End;
                                //        Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);
                                //    }
                                //}
                            }
                            break;
                        case OP_CV_AutoStep.Step430_InMode_Check_PIO_End:
                            {
                                if (INPUT_DIR_STK_PIO_END_Check())
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step440_InMode_Check_OP_CST_Unload_And_Safe;

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
                                //    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step440_InMode_Check_OP_CST_Unload_And_Safe;
                                //}
                            }
                            break;
                        case OP_CV_AutoStep.Step440_InMode_Check_OP_CST_Unload_And_Safe:
                            {
                                if (Carrier_CheckOP_ExistProduct(false) && !Sensor_OP_Fork_Detect && !m_bCycleRunning)
                                {
                                    Carrier_ClearPortToRM_CarrierID();
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step000_Check_Direction;
                                }
                                else if(!Sensor_OP_Fork_Detect && m_bCycleRunning)
                                {
                                    Carrier_ClearPortToRM_CarrierID();
                                    if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                    {
                                        //Inmode인 경우에는 OP에서 카운트를 증가
                                        m_CycleControlProgressCount++;
                                        //-----------Direction Change 실행
                                        Port_To_RM_PIO_Init();
                                        GetMotionParam().ePortDirection = PortDirection.Output;
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose;
                                    }
                                    else
                                    {
                                        //Inmode인 경우에는 OP에서 싸이클 종료
                                        m_CycleControlProgressCount++;
                                        m_bAutoManualCycleStopReq = true;
                                        m_bCycleRunning = false;
                                    }
                                }
                            }
                            break;
                        case OP_CV_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) && Carrier_CheckOP_ExistID())
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step800_OutMode_Move_OP_Centering_FWD;
                                else if (Carrier_CheckOP_ExistProduct(false) && !Carrier_CheckOP_ExistID())
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step610_OutMode_Move_OP_X_OP;
                                else if (Carrier_CheckOP_ExistProduct(true) && !Carrier_CheckOP_ExistID())
                                {
                                    AlarmInsert((short)PortAlarm.OP_No_Cassette_ID_Error, AlarmLevel.Error);
                                }
                                else if (Carrier_CheckOP_ExistProduct(false) && Carrier_CheckOP_ExistID())
                                {
                                    OP_CarrierID = string.Empty;
                                }
                            }
                            break;
                        case OP_CV_AutoStep.Step610_OutMode_Move_OP_X_OP:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_OP_X))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step620_OutMode_Move_OP_Z_Up;
                                    break;
                                }

                                if (X_Axis_MotionAndDone(PortAxis.Buffer_OP_X, Teaching_X_Pos.OP_Pos))
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step620_OutMode_Move_OP_Z_Up;
                            }
                            break;
                        case OP_CV_AutoStep.Step620_OutMode_Move_OP_Z_Up:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_OP_Z))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step630_OutMode_Move_OP_T_0_Deg;
                                    break;
                                }

                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_OP_Z, Teaching_Z_Pos.Up_Pos))
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step630_OutMode_Move_OP_T_0_Deg;
                            }
                            break;
                        case OP_CV_AutoStep.Step630_OutMode_Move_OP_T_0_Deg:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_OP_T))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step640_OutMode_Move_OP_Stopper_Up;
                                    break;
                                }

                                if (T_Axis_MotionAndDone(PortAxis.Buffer_OP_T, Teaching_T_Pos.Degree0_Pos))
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step640_OutMode_Move_OP_Stopper_Up;
                            }
                            break;
                        case OP_CV_AutoStep.Step640_OutMode_Move_OP_Stopper_Up:
                            {
                                if (!GetMotionParam().IsStopperEnable(BufferCV.Buffer_OP))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step650_OutMode_Move_OP_Centering_BWD;
                                    break;
                                }

                                StopperUp(BufferCV.Buffer_OP);
                                m_eOP_CV_AutoStep = OP_CV_AutoStep.Step650_OutMode_Move_OP_Centering_BWD;
                            }
                            break;
                        case OP_CV_AutoStep.Step650_OutMode_Move_OP_Centering_BWD:
                            {
                                if (!GetMotionParam().IsCenteringEnable(BufferCV.Buffer_OP))
                                {
                                    if (GetPortOperationMode() == PortOperationMode.Conveyor && !m_bCycleRunning)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step700_OutMode_Await_PIO_TR_REQ;
                                    else if (GetPortOperationMode() == PortOperationMode.MGV || m_bCycleRunning)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe;

                                    break;
                                }

                                CenteringBWD(BufferCV.Buffer_OP);

                                if (GetPortOperationMode() == PortOperationMode.Conveyor && !m_bCycleRunning)
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step700_OutMode_Await_PIO_TR_REQ;
                                else if (GetPortOperationMode() == PortOperationMode.MGV || m_bCycleRunning)
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe;
                            }
                            break;
                        case OP_CV_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                            {
                                if (OUTPUT_DIR_STK_PIO_TR_REQ_Await())
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step710_OutMode_Check_PIO_Busy;

                                //PIOStatus_PortToSTK_Load_Req = true;
                                //if (PIOStatus_STKToPort_TR_REQ)
                                //{
                                //    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step710_OutMode_Check_PIO_Busy;
                                //    Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);
                                //}
                            }
                            break;
                        case OP_CV_AutoStep.Step710_OutMode_Check_PIO_Busy:
                            {
                                if (OUTPUT_DIR_STK_PIO_BUSY_Check())
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step720_OutMode_Check_PIO_Complete;

                                //if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
                                //{
                                //    if (!PIOStatus_STKToPort_Busy)
                                //        AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                //}

                                //if (PIOStatus_STKToPort_Busy)
                                //{
                                //    PIOStatus_PortToSTK_Ready = true;
                                //    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step720_OutMode_Check_PIO_Complete;
                                //    Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);
                                //    m_RMCSTIDRWTimer.Stop();
                                //    m_RMCSTIDRWTimer.Reset();
                                //    m_RMCSTIDRWTimer.Start();
                                //}
                            }
                            break;
                        case OP_CV_AutoStep.Step720_OutMode_Check_PIO_Complete:
                            {
                                if (OUTPUT_DIR_STK_PIO_COMPLETE_Check())
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step730_OutMode_Check_PIO_End;

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
                                //    PIOStatus_STKToPort_Complete) // && RMCarrierID != string.Empty
                                //{
                                //    Carrier_ACK_PortToRM_CarrierID(RMCarrierID);

                                //    if (RMCarrierID != string.Empty) //|| m_eControlMode == ControlMode.CIMMode
                                //    {
                                //        m_RMCSTIDRWTimer.Stop();
                                //        m_RMCSTIDRWTimer.Reset();
                                //        PIOStatus_PortToSTK_Load_Req = false;
                                //        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step730_OutMode_Check_PIO_End;
                                //    }
                                //    else if (m_RMCSTIDRWTimer.Elapsed.TotalSeconds > 30.0)
                                //    {
                                //        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"RM -> Port CST ID Read Timeout");
                                //        m_RMCSTIDRWTimer.Stop();
                                //        m_RMCSTIDRWTimer.Reset();
                                //        PIOStatus_PortToSTK_Load_Req = false;
                                //        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step730_OutMode_Check_PIO_End;
                                //    }
                                //}
                            }
                            break;
                        case OP_CV_AutoStep.Step730_OutMode_Check_PIO_End:
                            {
                                if (OUTPUT_DIR_STK_PIO_END_Check())
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe;

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
                                //        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe;
                                //    }
                                //    else if (OP_CarrierID == string.Empty)
                                //    {
                                //        //Timeout의 경우
                                //        OP_CarrierID = "CST_ID_READ_FAIL";
                                //        Carrier_ClearPortToRM_CarrierID();
                                //        PIOStatus_PortToSTK_Ready = false;
                                //        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe;
                                //    }
                                //}
                            }
                            break;
                        case OP_CV_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe:
                            {
                                if (Carrier_CheckOP_ExistProduct(true) && !Sensor_OP_Fork_Detect)
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step800_OutMode_Move_OP_Centering_FWD;
                                }
                            }
                            break;
                        case OP_CV_AutoStep.Step800_OutMode_Move_OP_Centering_FWD:
                            {
                                if (!GetMotionParam().IsCenteringEnable(BufferCV.Buffer_OP))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step810_OutMode_Move_OP_X_LP;
                                    break;
                                }

                                CenteringFWD(BufferCV.Buffer_OP);

                                m_eOP_CV_AutoStep = OP_CV_AutoStep.Step810_OutMode_Move_OP_X_LP;
                            }
                            break;
                        case OP_CV_AutoStep.Step810_OutMode_Move_OP_X_LP:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_OP_X))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step820_OutMode_Move_OP_Z_Down;
                                    break;
                                }

                                if (X_Axis_MotionAndDone(PortAxis.Buffer_OP_X, IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos))
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step820_OutMode_Move_OP_Z_Down;
                            }
                            break;
                        case OP_CV_AutoStep.Step820_OutMode_Move_OP_Z_Down:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_OP_Z))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step830_OutMode_Move_OP_T_180_Deg;
                                    break;
                                }

                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_OP_Z, Teaching_Z_Pos.Down_Pos))
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step830_OutMode_Move_OP_T_180_Deg;
                            }
                            break;
                        case OP_CV_AutoStep.Step830_OutMode_Move_OP_T_180_Deg:
                            {
                                if (GetMotionParam().IsAxisUnUsed(PortAxis.Buffer_OP_T))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step840_OutMode_Move_OP_Stopper_Down;
                                    break;
                                }

                                if (T_Axis_MotionAndDone(PortAxis.Buffer_OP_T, Teaching_T_Pos.Degree180_Pos))
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step840_OutMode_Move_OP_Stopper_Down;
                            }
                            break;
                        case OP_CV_AutoStep.Step840_OutMode_Move_OP_Stopper_Down:
                            {
                                if (!GetMotionParam().IsStopperEnable(BufferCV.Buffer_OP))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step850_OutMode_Move_OP_Centering_BWD;
                                    break;
                                }

                                StopperDown(BufferCV.Buffer_OP);
                                m_eOP_CV_AutoStep = OP_CV_AutoStep.Step850_OutMode_Move_OP_Centering_BWD;
                            }
                            break;
                        case OP_CV_AutoStep.Step850_OutMode_Move_OP_Centering_BWD:
                            {
                                if (!GetMotionParam().IsCenteringEnable(BufferCV.Buffer_OP))
                                {
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step900_OutMode_Await_LPorBP_Load_Req;
                                    break;
                                }

                                CenteringBWD(BufferCV.Buffer_OP);

                                m_eOP_CV_AutoStep = OP_CV_AutoStep.Step900_OutMode_Await_LPorBP_Load_Req;
                            }
                            break;
                        case OP_CV_AutoStep.Step900_OutMode_Await_LPorBP_Load_Req:
                            {
                                if (GetMotionParam().IsBPCVUsed())
                                {
                                    if (m_eBP_CV_AutoStep == BP_CV_AutoStep.Step700_OutMode_Start_CST_Pass_From_OP)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step910_OutMode_Move_OP_CV_Rolling;
                                }
                                else
                                {
                                    if (m_eLP_CV_AutoStep == LP_CV_AutoStep.Step700_OutMode_Call_OPorBP_Load_Req)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step910_OutMode_Move_OP_CV_Rolling;
                                }
                            }
                            break;
                        case OP_CV_AutoStep.Step910_OutMode_Move_OP_CV_Rolling:
                            {
                                CVHighSpeedBWDRolling(BufferCV.Buffer_OP);

                                if (GetMotionParam().IsBPCVUsed())
                                {
                                    if (m_eBP_CV_AutoStep == BP_CV_AutoStep.Step710_OutMode_Move_BP_CV_Rolling)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step920_OutMode_Move_OP_CV_Stop;
                                }
                                else
                                {
                                    if (m_eLP_CV_AutoStep == LP_CV_AutoStep.Step710_OutMode_Move_LP_Rolling)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step920_OutMode_Move_OP_CV_Stop;
                                }
                            }
                            break;
                        case OP_CV_AutoStep.Step920_OutMode_Move_OP_CV_Stop:
                            {
                                if (Carrier_CheckOP_ExistProduct(false))
                                {
                                    CVStop(BufferCV.Buffer_OP);
                                    m_eOP_CV_AutoStep = OP_CV_AutoStep.Step930_OutMode_Send_OP_CST_ID;
                                }
                            }
                            break;
                        case OP_CV_AutoStep.Step930_OutMode_Send_OP_CST_ID:
                            {
                                if (Carrier_CheckOP_ExistProduct(false) &&
                                    Carrier_CheckOP_ExistID())
                                {
                                    if (GetMotionParam().IsBPCVUsed())
                                    {
                                        string OPCarrierID = OP_CarrierID;
                                        int LastBPIndex = (int)GetMotionParam().GetLastUsedBP() - 2;
                                        Carrier_SetBP_CarrierID(LastBPIndex, OPCarrierID);
                                        if(OPCarrierID == Carrier_GetBP_CarrierID(LastBPIndex))
                                            m_eOP_CV_AutoStep = OP_CV_AutoStep.Step940_OutMode_Clear_OP_CST_ID;
                                    }
                                    else
                                    {
                                        string OPCarrierID = OP_CarrierID;
                                        LP_CarrierID = OPCarrierID;
                                        if (OPCarrierID == LP_CarrierID)
                                            m_eOP_CV_AutoStep = OP_CV_AutoStep.Step940_OutMode_Clear_OP_CST_ID;
                                    }
                                }
                            }
                            break;
                        case OP_CV_AutoStep.Step940_OutMode_Clear_OP_CST_ID:
                            {
                                if (Carrier_CheckOP_ExistProduct(false) &&
                                    Carrier_CheckOP_ExistID())
                                {
                                    OP_CarrierID = string.Empty;
                                }
                                else if (Carrier_CheckOP_ExistProduct(false) &&
                                        !Carrier_CheckOP_ExistID())
                                {
                                    if (!m_bCycleRunning)
                                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step000_Check_Direction;
                                    else
                                    {
                                        if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                        {
                                            //Outmode인 경우에는 LP에서 카운트를 증가
                                            //m_CycleControlProgressCount++;
                                            //-----------Direction Change 실행
                                            Port_To_RM_PIO_Init();
                                            m_eOP_CV_AutoStep = OP_CV_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose;
                                        }
                                        else
                                        {
                                            //Outmode인 경우에는 LP에서 싸이클 종료
                                            //m_CycleControlProgressCount++;
                                            //m_bAutoManualCycleStopReq = true;
                                            //m_bCycleRunning = false;
                                        }
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
        private void Auto_CV_Start_BP_Control()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                while (m_bBP_CV_AutoRunning || m_bCycleRunning)
                {
                    if (m_bBP_CV_AutoRunning && m_bCycleRunning)
                    {
                        Auto_CV_BP_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAndCycleCrash, $"BP CV");
                        break;
                    }

                    //Global Alarm Check
                    if (GetAlarmLevel() == AlarmLevel.Error)
                    {
                        Auto_CV_BP_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAlarmStop, $"BP CV");
                        break;
                    }

                    //Update
                    Auto_CV_BPStopEnableUpdate();
                    Auto_CV_BP_StepTimeOut_AlarmCheck();

                    if (IsAutoStopReq(!m_bCycleRunning))
                    {
                        if (m_bBP_CV_AutoStopEnable)
                        {
                            Auto_CV_BP_StopInit();
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AutoControlStopCommand, $"BP CV");
                            break;
                        }
                    }

                    switch (m_eBP_CV_AutoStep)
                    {
                        case BP_CV_AutoStep.Step000_Check_Direction:
                            {
                                if (GetOperationDirection() == PortDirection.Input)
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step100_InMode_Check_All_BP_CST_Load_And_Status;
                                else
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step600_OutMode_Check_All_BP_CST_Load_And_Status;
                            }
                            break;
                        case BP_CV_AutoStep.Step100_InMode_Check_All_BP_CST_Load_And_Status:
                            {
                                if(m_eLP_CV_AutoStep >= LP_CV_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose &&
                                    m_eOP_CV_AutoStep >= OP_CV_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose)
                                {
                                    //LP와 OP가 모두 OutMode인 경우 방향 다시 체크
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step000_Check_Direction;
                                    break;
                                }


                                m_eBPtoBPStartBP = BufferCV.Buffer_LP;
                                var origin = Enum.GetValues(typeof(BufferCV));
                                Array.Reverse(origin);
                                foreach (BufferCV eBufferCV in origin)
                                {
                                    if (eBufferCV < BufferCV.Buffer_BP1 || eBufferCV > BufferCV.Buffer_BP4)
                                        continue;

                                    if (GetMotionParam().IsCVUsed(eBufferCV) && 
                                        eBufferCV == GetMotionParam().GetLastUsedBP() &&
                                        GetMotionParam().IsCSTDetectSensorEnable(eBufferCV) &&
                                        BufferCtrl_BP_CSTDetect_Status(eBufferCV))
                                    {
                                        if (m_eOP_CV_AutoStep == OP_CV_AutoStep.Step200_InMode_Call_LPorBP_Load_Req)
                                        {
                                            m_eBP_CV_AutoStep = BP_CV_AutoStep.Step300_InMode_Start_CST_Pass_To_OP;
                                            break;
                                        }
                                    }
                                    else if (GetMotionParam().IsCVUsed(eBufferCV) && 
                                            eBufferCV == GetMotionParam().GetFirstUsedBP() &&
                                            GetMotionParam().IsCSTDetectSensorEnable(eBufferCV) &&
                                            !BufferCtrl_BP_CSTDetect_Status(eBufferCV))
                                    {
                                        if (m_eLP_CV_AutoStep == LP_CV_AutoStep.Step500_InMode_Await_OPorBP_Load_Req)
                                        {
                                            m_eBP_CV_AutoStep = BP_CV_AutoStep.Step200_InMode_Start_CST_Pass_From_LP;
                                            break;
                                        }
                                    }
                                    else if (GetMotionParam().IsCVUsed(eBufferCV) && GetMotionParam().IsCVUsed(eBufferCV + 1) &&
                                             GetMotionParam().IsCSTDetectSensorEnable(eBufferCV) && GetMotionParam().IsCSTDetectSensorEnable(eBufferCV + 1))
                                    {
                                        if (BufferCtrl_BP_CSTDetect_Status(eBufferCV) && !BufferCtrl_BP_CSTDetect_Status(eBufferCV + 1))
                                        {
                                            m_eBPtoBPStartBP = eBufferCV;
                                            m_eBP_CV_AutoStep = BP_CV_AutoStep.Step400_InMode_Pass_CST_BP_to_BP;
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        case BP_CV_AutoStep.Step200_InMode_Start_CST_Pass_From_LP:
                            {
                                if (m_eLP_CV_AutoStep == LP_CV_AutoStep.Step510_InMode_Move_LP_CV_Rolling)
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step210_InMode_Move_BP_CV_Rolling;
                            }
                            break;
                        case BP_CV_AutoStep.Step210_InMode_Move_BP_CV_Rolling:
                            {
                                var FirstBP = GetMotionParam().GetFirstUsedBP();
                                CVHighSpeedFWDRolling(FirstBP);
                                
                                if (m_eLP_CV_AutoStep == LP_CV_AutoStep.Step520_InMode_Move_LP_CV_Stop ||
                                    m_eLP_CV_AutoStep == LP_CV_AutoStep.Step530_InMode_Send_LP_CST_ID ||
                                    m_eLP_CV_AutoStep == LP_CV_AutoStep.Step540_InMode_Clear_LP_CST_ID)
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step220_InMode_Move_BP_CV_Stop;
                            }
                            break;
                        case BP_CV_AutoStep.Step220_InMode_Move_BP_CV_Stop:
                            {
                                var FirstBP = GetMotionParam().GetFirstUsedBP();
                                if (GetMotionParam().IsCSTDetectSensorEnable(FirstBP) && BufferCtrl_BP_CSTDetect_Status(FirstBP))
                                {
                                    CVStop(FirstBP);
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step230_InMode_Check_CST_ID;
                                }
                            }
                            break;
                        case BP_CV_AutoStep.Step230_InMode_Check_CST_ID:
                            {
                                var FirstBP = GetMotionParam().GetFirstUsedBP();
                                int BPIndex = (int)FirstBP - 2;

                                if (GetMotionParam().IsCSTDetectSensorEnable(FirstBP) &&
                                    BufferCtrl_BP_CSTDetect_Status(FirstBP) &&
                                    Carrier_CheckBP_ExistID(BPIndex))
                                {
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step100_InMode_Check_All_BP_CST_Load_And_Status;
                                }
                            }
                            break;

                        case BP_CV_AutoStep.Step300_InMode_Start_CST_Pass_To_OP:
                            {
                                if (m_eOP_CV_AutoStep == OP_CV_AutoStep.Step210_InMode_Move_OP_Rolling)
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step310_InMode_Move_BP_CV_Rolling;
                            }
                            break;
                        case BP_CV_AutoStep.Step310_InMode_Move_BP_CV_Rolling:
                            {
                                var LastBP = GetMotionParam().GetLastUsedBP();
                                CVHighSpeedFWDRolling(LastBP);

                                if (m_eOP_CV_AutoStep == OP_CV_AutoStep.Step220_InMode_Move_OP_CV_Stop ||
                                    m_eOP_CV_AutoStep == OP_CV_AutoStep.Step230_InMode_Check_CST_ID)
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step320_InMode_Move_BP_CV_Stop;
                            }
                            break;
                        case BP_CV_AutoStep.Step320_InMode_Move_BP_CV_Stop:
                            {
                                var LastBP = GetMotionParam().GetLastUsedBP();
                                if (GetMotionParam().IsCSTDetectSensorEnable(LastBP) && !BufferCtrl_BP_CSTDetect_Status(LastBP))
                                {
                                    CVStop(LastBP);
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step330_InMode_Send_BP_CST_ID;
                                }
                            }
                            break;
                        case BP_CV_AutoStep.Step330_InMode_Send_BP_CST_ID:
                            {
                                var LastBP = GetMotionParam().GetLastUsedBP();
                                int BPIndex = (int)LastBP - 2;
                                string BPCarrierID = Carrier_GetBP_CarrierID(BPIndex);
                                OP_CarrierID = BPCarrierID;

                                if(OP_CarrierID == BPCarrierID && !string.IsNullOrEmpty(BPCarrierID))
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step340_InMode_Clear_BP_CST_ID;
                            }
                            break;
                        case BP_CV_AutoStep.Step340_InMode_Clear_BP_CST_ID:
                            {
                                var LastBP = GetMotionParam().GetLastUsedBP();
                                int BPIndex = (int)LastBP - 2;
                                Carrier_ClearBP_CarrierID(BPIndex);

                                if (string.IsNullOrEmpty(Carrier_GetBP_CarrierID(BPIndex)))
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step100_InMode_Check_All_BP_CST_Load_And_Status;
                            }
                            break;
                        case BP_CV_AutoStep.Step400_InMode_Pass_CST_BP_to_BP:
                            {
                                var StartBP = m_eBPtoBPStartBP;
                                var NextBP = m_eBPtoBPStartBP + 1;
                                if (GetMotionParam().IsCVUsed(StartBP) && GetMotionParam().IsCVUsed(NextBP) &&
                                    GetMotionParam().IsCSTDetectSensorEnable(StartBP) && GetMotionParam().IsCSTDetectSensorEnable(NextBP))
                                {
                                    if (BufferCtrl_BP_CSTDetect_Status(StartBP) && !BufferCtrl_BP_CSTDetect_Status(NextBP))
                                    {
                                        m_eBP_CV_AutoStep = BP_CV_AutoStep.Step410_InMode_Move_BP_CV_Rolling;
                                    }
                                }
                            }
                            break;
                        case BP_CV_AutoStep.Step410_InMode_Move_BP_CV_Rolling:
                            {
                                var StartBP = m_eBPtoBPStartBP;
                                var NextBP = m_eBPtoBPStartBP + 1;
                                CVHighSpeedFWDRolling(StartBP);
                                CVHighSpeedFWDRolling(NextBP);

                                m_eBP_CV_AutoStep = BP_CV_AutoStep.Step420_InMode_Move_BP_CV_Stop;

                            }
                            break;
                        case BP_CV_AutoStep.Step420_InMode_Move_BP_CV_Stop:
                            {
                                var StartBP = m_eBPtoBPStartBP;
                                var NextBP = m_eBPtoBPStartBP + 1;

                                if (!BufferCtrl_BP_CSTDetect_Status(StartBP) && BufferCtrl_BP_CSTDetect_Status(NextBP))
                                {
                                    CVStop(StartBP);
                                    CVStop(NextBP);
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step430_InMode_Pass_CST_ID;
                                }
                            }
                            break;
                        case BP_CV_AutoStep.Step430_InMode_Pass_CST_ID:
                            {
                                var StartBP = m_eBPtoBPStartBP;
                                var NextBP = m_eBPtoBPStartBP + 1;

                                int StartBPIndex = (int)StartBP - 2;
                                int NextBPIndex = (int)NextBP - 2;

                                string GetStartBPCarrierID = Carrier_GetBP_CarrierID(StartBPIndex);
                                Carrier_SetBP_CarrierID(NextBPIndex, GetStartBPCarrierID);

                                if(GetStartBPCarrierID == Carrier_GetBP_CarrierID(NextBPIndex) && !string.IsNullOrEmpty(GetStartBPCarrierID))
                                {
                                    Carrier_ClearBP_CarrierID(StartBPIndex);
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step100_InMode_Check_All_BP_CST_Load_And_Status;
                                }
                            }
                            break;

                        case BP_CV_AutoStep.Step600_OutMode_Check_All_BP_CST_Load_And_Status:
                            {
                                if ((m_eLP_CV_AutoStep >= LP_CV_AutoStep.Step100_InMode_Check_LP_CST_Load_And_Pose && m_eLP_CV_AutoStep < LP_CV_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose) &&
                                    (m_eOP_CV_AutoStep >= OP_CV_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose && m_eOP_CV_AutoStep < OP_CV_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose))
                                {
                                    //LP와 OP가 모두 InMode인 경우 방향 다시 체크
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step000_Check_Direction;
                                    break;
                                }

                                m_eBPtoBPStartBP = BufferCV.Buffer_LP;
                                var origin = Enum.GetValues(typeof(BufferCV));
                                foreach (BufferCV eBufferCV in origin)
                                {
                                    if (eBufferCV < BufferCV.Buffer_BP1 || eBufferCV > BufferCV.Buffer_BP4)
                                        continue;

                                    if (GetMotionParam().IsCVUsed(eBufferCV) && 
                                        eBufferCV == GetMotionParam().GetLastUsedBP() &&
                                        GetMotionParam().IsCSTDetectSensorEnable(eBufferCV) &&
                                        !BufferCtrl_BP_CSTDetect_Status(eBufferCV))
                                    {
                                        if (m_eOP_CV_AutoStep == OP_CV_AutoStep.Step900_OutMode_Await_LPorBP_Load_Req)
                                        {
                                            m_eBP_CV_AutoStep = BP_CV_AutoStep.Step700_OutMode_Start_CST_Pass_From_OP;
                                            break;
                                        }
                                    }
                                    else if (GetMotionParam().IsCVUsed(eBufferCV) && 
                                            eBufferCV == GetMotionParam().GetFirstUsedBP() &&
                                            GetMotionParam().IsCSTDetectSensorEnable(eBufferCV) &&
                                            BufferCtrl_BP_CSTDetect_Status(eBufferCV))
                                    {
                                        if (m_eLP_CV_AutoStep == LP_CV_AutoStep.Step700_OutMode_Call_OPorBP_Load_Req)
                                        {
                                            m_eBP_CV_AutoStep = BP_CV_AutoStep.Step800_OutMode_Start_CST_Pass_To_LP;
                                            break;
                                        }
                                    }
                                    else if (GetMotionParam().IsCVUsed(eBufferCV) && GetMotionParam().IsCVUsed(eBufferCV - 1))
                                    {
                                        if (GetMotionParam().IsCSTDetectSensorEnable(eBufferCV) && GetMotionParam().IsCSTDetectSensorEnable(eBufferCV - 1))
                                        {
                                            if (BufferCtrl_BP_CSTDetect_Status(eBufferCV) &&
                                                !BufferCtrl_BP_CSTDetect_Status(eBufferCV - 1))
                                            {
                                                m_eBPtoBPStartBP = eBufferCV;
                                                m_eBP_CV_AutoStep = BP_CV_AutoStep.Step900_OutMode_Pass_CST_BP_to_BP;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case BP_CV_AutoStep.Step700_OutMode_Start_CST_Pass_From_OP:
                            {
                                if (m_eOP_CV_AutoStep == OP_CV_AutoStep.Step910_OutMode_Move_OP_CV_Rolling)
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step710_OutMode_Move_BP_CV_Rolling;
                            }
                            break;
                        case BP_CV_AutoStep.Step710_OutMode_Move_BP_CV_Rolling:
                            {
                                var LastBP = GetMotionParam().GetLastUsedBP();
                                CVHighSpeedBWDRolling(LastBP);

                                if (m_eOP_CV_AutoStep == OP_CV_AutoStep.Step920_OutMode_Move_OP_CV_Stop ||
                                    m_eOP_CV_AutoStep == OP_CV_AutoStep.Step930_OutMode_Send_OP_CST_ID ||
                                    m_eOP_CV_AutoStep == OP_CV_AutoStep.Step940_OutMode_Clear_OP_CST_ID)
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step720_OutMode_Move_BP_CV_Stop;
                            }
                            break;
                        case BP_CV_AutoStep.Step720_OutMode_Move_BP_CV_Stop:
                            {
                                var LastBP = GetMotionParam().GetLastUsedBP();
                                if (GetMotionParam().IsCSTDetectSensorEnable(LastBP) && BufferCtrl_BP_CSTDetect_Status(LastBP))
                                {
                                    CVStop(LastBP);
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step730_OutMode_Check_CST_ID;
                                }
                            }
                            break;
                        case BP_CV_AutoStep.Step730_OutMode_Check_CST_ID:
                            {
                                var LastBP = GetMotionParam().GetLastUsedBP();
                                int BPIndex = (int)LastBP - 2;

                                if (GetMotionParam().IsCSTDetectSensorEnable(LastBP) &&
                                    BufferCtrl_BP_CSTDetect_Status(LastBP) &&
                                    Carrier_CheckBP_ExistID(BPIndex))
                                {
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step600_OutMode_Check_All_BP_CST_Load_And_Status;
                                }
                            }
                            break;
                        case BP_CV_AutoStep.Step800_OutMode_Start_CST_Pass_To_LP:
                            {
                                if (m_eLP_CV_AutoStep == LP_CV_AutoStep.Step710_OutMode_Move_LP_Rolling)
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step810_OutMode_Move_BP_CV_Rolling;
                            }
                            break;
                        case BP_CV_AutoStep.Step810_OutMode_Move_BP_CV_Rolling:
                            {
                                var FirstBP = GetMotionParam().GetFirstUsedBP();
                                CVHighSpeedBWDRolling(FirstBP);

                                if (m_eLP_CV_AutoStep == LP_CV_AutoStep.Step720_OutMode_Move_LP_CV_Stop ||
                                    m_eLP_CV_AutoStep == LP_CV_AutoStep.Step730_OutMode_Check_CST_ID)
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step820_OutMode_Move_BP_CV_Stop;
                            }
                            break;
                        case BP_CV_AutoStep.Step820_OutMode_Move_BP_CV_Stop:
                            {
                                var FirstBP = GetMotionParam().GetFirstUsedBP();
                                if (GetMotionParam().IsCSTDetectSensorEnable(FirstBP) && !BufferCtrl_BP_CSTDetect_Status(FirstBP))
                                {
                                    CVStop(FirstBP);
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step830_OutMode_Send_BP_CST_ID;
                                }
                            }
                            break;
                        case BP_CV_AutoStep.Step830_OutMode_Send_BP_CST_ID:
                            {
                                var FirstBP = GetMotionParam().GetFirstUsedBP();
                                int BPIndex = (int)FirstBP - 2;
                                string BPCarrierID = Carrier_GetBP_CarrierID(BPIndex);
                                LP_CarrierID = BPCarrierID;

                                if (LP_CarrierID == BPCarrierID && !string.IsNullOrEmpty(BPCarrierID))
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step840_OutMode_Clear_BP_CST_ID;
                            }
                            break;
                        case BP_CV_AutoStep.Step840_OutMode_Clear_BP_CST_ID:
                            {
                                var FirstBP = GetMotionParam().GetFirstUsedBP();
                                int BPIndex = (int)FirstBP - 2;
                                Carrier_ClearBP_CarrierID(BPIndex);

                                if (string.IsNullOrEmpty(Carrier_GetBP_CarrierID(BPIndex)))
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step600_OutMode_Check_All_BP_CST_Load_And_Status;
                            }
                            break;
                        case BP_CV_AutoStep.Step900_OutMode_Pass_CST_BP_to_BP:
                            {
                                var StartBP = m_eBPtoBPStartBP;
                                var NextBP = m_eBPtoBPStartBP - 1;
                                if (GetMotionParam().IsCVUsed(StartBP) && GetMotionParam().IsCVUsed(NextBP) &&
                                    GetMotionParam().IsCSTDetectSensorEnable(StartBP) && GetMotionParam().IsCSTDetectSensorEnable(NextBP))
                                {
                                    if (BufferCtrl_BP_CSTDetect_Status(StartBP) && !BufferCtrl_BP_CSTDetect_Status(NextBP))
                                    {
                                        m_eBP_CV_AutoStep = BP_CV_AutoStep.Step910_OutMode_Move_BP_CV_Rolling;
                                    }
                                }
                            }
                            break;
                        case BP_CV_AutoStep.Step910_OutMode_Move_BP_CV_Rolling:
                            {
                                var StartBP = m_eBPtoBPStartBP;
                                var NextBP = m_eBPtoBPStartBP - 1;
                                CVHighSpeedBWDRolling(StartBP);
                                CVHighSpeedBWDRolling(NextBP);

                                m_eBP_CV_AutoStep = BP_CV_AutoStep.Step920_OutMode_Move_BP_CV_Stop;

                            }
                            break;
                        case BP_CV_AutoStep.Step920_OutMode_Move_BP_CV_Stop:
                            {
                                var StartBP = m_eBPtoBPStartBP;
                                var NextBP = m_eBPtoBPStartBP - 1;

                                if (!BufferCtrl_BP_CSTDetect_Status(StartBP) && BufferCtrl_BP_CSTDetect_Status(NextBP))
                                {
                                    CVStop(StartBP);
                                    CVStop(NextBP);
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step930_OutMode_Pass_CST_ID;
                                }
                            }
                            break;
                        case BP_CV_AutoStep.Step930_OutMode_Pass_CST_ID:
                            {
                                var StartBP = m_eBPtoBPStartBP;
                                var NextBP = m_eBPtoBPStartBP - 1;

                                int StartBPIndex = (int)StartBP - 2;
                                int NextBPIndex = (int)NextBP - 2;

                                string GetStartBPCarrierID = Carrier_GetBP_CarrierID(StartBPIndex);
                                Carrier_SetBP_CarrierID(NextBPIndex, GetStartBPCarrierID);

                                if (GetStartBPCarrierID == Carrier_GetBP_CarrierID(NextBPIndex) && !string.IsNullOrEmpty(GetStartBPCarrierID))
                                {
                                    Carrier_ClearBP_CarrierID(StartBPIndex);
                                    m_eBP_CV_AutoStep = BP_CV_AutoStep.Step600_OutMode_Check_All_BP_CST_Load_And_Status;
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
        /// LP or OP or BP의 현재 스텝 텍스트
        /// </summary>
        /// <returns></returns>
        private string Auto_CV_Get_LP_StepStr()
        {
            switch (m_eLP_CV_AutoStep)
            {
                case LP_CV_AutoStep.Step000_Check_Direction:
                    return $"Check Direction";

                case LP_CV_AutoStep.Step100_InMode_Check_LP_CST_Load_And_Pose:
                case LP_CV_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose:
                    return $"Check LP CST Load and Pose";
                case LP_CV_AutoStep.Step110_InMode_Move_LP_X_LP:
                    return $"Move X LP Pos (Init)";
                case LP_CV_AutoStep.Step120_InMode_Move_LP_Z_Down:
                    return $"Move Z Down Pos (Init)";
                case LP_CV_AutoStep.Step130_InMode_Move_LP_T_180_Deg:
                    return $"Move T 180 Deg Pos (Init)";
                case LP_CV_AutoStep.Step140_InMode_Move_LP_Stopper_Up:
                case LP_CV_AutoStep.Step640_OutMode_Move_LP_Stopper_Up:
                    return $"Move Stopper Up (Init)";
                case LP_CV_AutoStep.Step150_InMode_Move_LP_Centering_BWD:
                case LP_CV_AutoStep.Step650_OutMode_Move_LP_Centering_BWD:
                    return $"Move Centering BWD (Init)";

                case LP_CV_AutoStep.Step200_InMode_Await_PIO_CS:
                case LP_CV_AutoStep.Step900_OutMode_Await_PIO_CS:
                    return $"Await PIO-CS";
                case LP_CV_AutoStep.Step210_InMode_Check_PIO_Valid:
                case LP_CV_AutoStep.Step910_OutMode_Check_PIO_Valid:
                    return $"Check PIO-Valid";

                //case LP_CV_AutoStep.Step211_InMode_Call_AGV_Load_Req:
                //case LP_CV_AutoStep.Step911_OutMode_Call_AGV_Unload_Req:
                //    return $"Call AGV CST Load Req";
                case LP_CV_AutoStep.Step220_InMode_Check_PIO_TR:
                case LP_CV_AutoStep.Step920_OutMode_Check_PIO_TR:
                    return $"Check PIO-TR";
                case LP_CV_AutoStep.Step230_InMode_Check_PIO_Busy:
                case LP_CV_AutoStep.Step930_OutMode_Check_PIO_Busy:
                    return $"Check PIO-Busy";
                case LP_CV_AutoStep.Step240_InMode_Check_PIO_Complete:
                case LP_CV_AutoStep.Step940_OutMode_Check_PIO_Complete:
                    return $"Check PIO-Complete";
                case LP_CV_AutoStep.Step250_InMode_Check_PIO_End:
                case LP_CV_AutoStep.Step950_OutMode_Check_PIO_End:
                    return $"Check PIO-End Off";
                
                case LP_CV_AutoStep.Step260_InMode_Check_CST_Load_And_Safe:
                    return $"Check LP CST Load and Safe";
                case LP_CV_AutoStep.Step960_OutMode_Check_LP_CST_Unload_And_Safe:
                    return $"Check LP CST Unload and Safe";

                case LP_CV_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                    return $"Await MGV CST Load";
                case LP_CV_AutoStep.Step990_OutMode_Await_MGV_CST_Unload:
                    return $"Await MGV CST Unload";

                case LP_CV_AutoStep.Step400_InMode_Ready_CST_ID_Read:
                    return $"Ready CST ID Read";
                case LP_CV_AutoStep.Step410_InMode_Start_CST_ID_Read:
                    return $"Start CST ID Read";
                case LP_CV_AutoStep.Step420_InMode_Move_LP_Centering_FWD:
                case LP_CV_AutoStep.Step800_OutMode_Move_LP_Centering_FWD:
                    return $"Move Centering FWD";
                case LP_CV_AutoStep.Step430_InMode_Move_LP_X_OP:
                    return $"Move X OP Pos";
                case LP_CV_AutoStep.Step440_InMode_Move_LP_Z_Up:
                    return $"Move Z Up Pos";
                case LP_CV_AutoStep.Step450_InMode_Move_LP_T_0_Deg:
                    return $"Move T 0 Deg Pos";
                case LP_CV_AutoStep.Step460_InMode_Move_LP_Stopper_Down:
                case LP_CV_AutoStep.Step840_OutMode_Move_LP_Stopper_Down:
                    return $"Move Stopper Down";
                case LP_CV_AutoStep.Step470_InMode_Move_LP_Centering_BWD:
                case LP_CV_AutoStep.Step850_OutMode_Move_LP_Centering_BWD:
                    return $"Move Centering BWD";

                case LP_CV_AutoStep.Step500_InMode_Await_OPorBP_Load_Req:
                    return $"Await Load Req(BP or OP)";
                case LP_CV_AutoStep.Step510_InMode_Move_LP_CV_Rolling:
                case LP_CV_AutoStep.Step710_OutMode_Move_LP_Rolling:
                    return $"LP CV Rolling";
                case LP_CV_AutoStep.Step520_InMode_Move_LP_CV_Stop:
                case LP_CV_AutoStep.Step720_OutMode_Move_LP_CV_Stop:
                    return $"LP CV Stop";
                case LP_CV_AutoStep.Step530_InMode_Send_LP_CST_ID:
                    return $"Send LP CST ID (LP -> BP or OP)";
                case LP_CV_AutoStep.Step540_InMode_Clear_LP_CST_ID:
                    return $"Clear LP CST ID";

                case LP_CV_AutoStep.Step610_OutMode_Move_LP_X_OP:
                    return $"Move X OP Pos (Init)";
                case LP_CV_AutoStep.Step620_OutMode_Move_LP_Z_Up:
                    return $"Move Z Up Pos (Init)";
                case LP_CV_AutoStep.Step630_OutMode_Move_LP_T_0_Deg:
                    return $"Move T 0 Deg Pos (Init)";

                case LP_CV_AutoStep.Step700_OutMode_Call_OPorBP_Load_Req:
                    return $"Call Load Req(BP or OP)";
                case LP_CV_AutoStep.Step730_OutMode_Check_CST_ID:
                    return $"Check LP CST ID";

                case LP_CV_AutoStep.Step810_OutMode_Move_LP_X_LP:
                    return $"Move X LP Pos";
                case LP_CV_AutoStep.Step820_OutMode_Move_LP_Z_Down:
                    return $"Move Z Down Pos";
                case LP_CV_AutoStep.Step830_OutMode_Move_LP_T_180_Deg:
                    return $"Move T 180 Deg Pos";
                default:
                    return "Not def step str";
            }
        }
        private string Auto_CV_Get_OP_StepStr()
        {
            switch (m_eOP_CV_AutoStep)
            {
                case OP_CV_AutoStep.Step000_Check_Direction:
                    return $"Check Direction";

                case OP_CV_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose:
                case OP_CV_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose:
                    return $"Check OP CST Load and Pose";

                case OP_CV_AutoStep.Step110_InMode_Move_OP_X_LP:
                    return $"Move X LP Pos (Init)";
                case OP_CV_AutoStep.Step120_InMode_Move_OP_Z_Down:
                    return $"Move Z Down Pos (Init)";
                case OP_CV_AutoStep.Step130_InMode_Move_OP_T_180_Deg:
                    return $"Move T 180 Deg Pos (Init)";
                case OP_CV_AutoStep.Step140_InMode_Move_OP_Stopper_Up:
                case OP_CV_AutoStep.Step640_OutMode_Move_OP_Stopper_Up:
                    return $"Move Stopper Up (Init)";
                case OP_CV_AutoStep.Step150_InMode_Move_OP_Centering_BWD:
                case OP_CV_AutoStep.Step650_OutMode_Move_OP_Centering_BWD:
                    return $"Move Centering BWD (Init)";

                case OP_CV_AutoStep.Step200_InMode_Call_LPorBP_Load_Req:
                    return $"Call Load Req (LP or BP)";
                case OP_CV_AutoStep.Step210_InMode_Move_OP_Rolling:
                case OP_CV_AutoStep.Step910_OutMode_Move_OP_CV_Rolling:
                    return $"OP CV Rolling";
                case OP_CV_AutoStep.Step220_InMode_Move_OP_CV_Stop:
                case OP_CV_AutoStep.Step920_OutMode_Move_OP_CV_Stop:
                    return $"OP CV Stop";
                case OP_CV_AutoStep.Step230_InMode_Check_CST_ID:
                    return $"Check OP CST ID";

                case OP_CV_AutoStep.Step300_InMode_Move_OP_Centering_FWD:
                case OP_CV_AutoStep.Step800_OutMode_Move_OP_Centering_FWD:
                    return $"Move Centering FWD";
                case OP_CV_AutoStep.Step310_InMode_Move_OP_X_OP:
                    return $"Move X OP Pos";
                case OP_CV_AutoStep.Step320_InMode_Move_OP_Z_Up:
                    return $"Move Z Up Pos";
                case OP_CV_AutoStep.Step330_InMode_Move_OP_T_0_Deg:
                    return $"Move T 0 Deg Pos";
                case OP_CV_AutoStep.Step340_InMode_Move_OP_Stopper_Down:
                case OP_CV_AutoStep.Step840_OutMode_Move_OP_Stopper_Down:
                    return $"Move Stopper Down";
                case OP_CV_AutoStep.Step350_InMode_Move_OP_Centering_BWD:
                case OP_CV_AutoStep.Step850_OutMode_Move_OP_Centering_BWD:
                    return $"Move Centering BWD";

                case OP_CV_AutoStep.Step400_InMode_Await_PIO_TR_REQ:
                case OP_CV_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                    return $"Await PIO-TR_REQ";
                case OP_CV_AutoStep.Step410_InMode_Check_PIO_Busy:
                case OP_CV_AutoStep.Step710_OutMode_Check_PIO_Busy:
                    return $"Check PIO-Busy";
                case OP_CV_AutoStep.Step420_InMode_Check_PIO_Complete:
                case OP_CV_AutoStep.Step720_OutMode_Check_PIO_Complete:
                    return $"Check PIO-Complete";
                case OP_CV_AutoStep.Step430_InMode_Check_PIO_End:
                case OP_CV_AutoStep.Step730_OutMode_Check_PIO_End:
                    return $"Check PIO-End off";
                case OP_CV_AutoStep.Step440_InMode_Check_OP_CST_Unload_And_Safe:
                case OP_CV_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe:
                    return $"Check OP CST Unload and Safe";


                case OP_CV_AutoStep.Step610_OutMode_Move_OP_X_OP:
                    return $"Move X OP Pos (Init)";
                case OP_CV_AutoStep.Step620_OutMode_Move_OP_Z_Up:
                    return $"Move Z Up Pos (Init)";
                case OP_CV_AutoStep.Step630_OutMode_Move_OP_T_0_Deg:
                    return $"Move T 0 Deg Pos (Init)";

                case OP_CV_AutoStep.Step810_OutMode_Move_OP_X_LP:
                    return $"Move X LP Pos";
                case OP_CV_AutoStep.Step820_OutMode_Move_OP_Z_Down:
                    return $"Move Z Down Pos";
                case OP_CV_AutoStep.Step830_OutMode_Move_OP_T_180_Deg:
                    return $"Move T 180 Deg Pos";

                case OP_CV_AutoStep.Step900_OutMode_Await_LPorBP_Load_Req:
                    return $"Await Load Req (LP or BP)";
                case OP_CV_AutoStep.Step930_OutMode_Send_OP_CST_ID:
                    return $"Send OP CST ID (OP -> LP or BP)";
                case OP_CV_AutoStep.Step940_OutMode_Clear_OP_CST_ID:
                    return $"Clear OP CST ID";
                default:
                    return "Not def step str";
            }
        }
        private string Auto_CV_Get_BP_StepStr()
        {
            var StartBP = GetMotionParam().GetFirstUsedBP();
            var LastBP = GetMotionParam().GetLastUsedBP();

            int nStartBPIndex = (int)StartBP - 2;
            int nLastBPIndex = (int)LastBP - 2;

            switch (m_eBP_CV_AutoStep)
            {
                case BP_CV_AutoStep.Step000_Check_Direction:
                    return $"Check Direction";

                case BP_CV_AutoStep.Step100_InMode_Check_All_BP_CST_Load_And_Status:
                case BP_CV_AutoStep.Step600_OutMode_Check_All_BP_CST_Load_And_Status:
                    return $"Check BP CST Load and State";

                case BP_CV_AutoStep.Step200_InMode_Start_CST_Pass_From_LP:
                    return $"BP CV PIO Start (LP -> BP[{nStartBPIndex}])";
                case BP_CV_AutoStep.Step210_InMode_Move_BP_CV_Rolling:
                    return $"BP CV Run (LP -> BP[{nStartBPIndex}])";
                case BP_CV_AutoStep.Step220_InMode_Move_BP_CV_Stop:
                    return $"BP CV Stop (LP -> BP[{nStartBPIndex}])";
                case BP_CV_AutoStep.Step230_InMode_Check_CST_ID:
                    return $"Check BP CST ID (LP -> BP[{nStartBPIndex}])";

                case BP_CV_AutoStep.Step300_InMode_Start_CST_Pass_To_OP:
                    return $"BP CV PIO Start (BP[{nLastBPIndex}] -> OP)";
                case BP_CV_AutoStep.Step310_InMode_Move_BP_CV_Rolling:
                    return $"BP CV Run (BP[{nLastBPIndex}] -> OP)";
                case BP_CV_AutoStep.Step320_InMode_Move_BP_CV_Stop:
                    return $"BP CV Stop (BP[{nLastBPIndex}] -> OP)";
                case BP_CV_AutoStep.Step330_InMode_Send_BP_CST_ID:
                    return $"Send BP CST ID (BP[{nLastBPIndex}] -> OP)";
                case BP_CV_AutoStep.Step340_InMode_Clear_BP_CST_ID:
                    return $"Clear BP CST ID (BP[{nLastBPIndex}] -> OP)";

                case BP_CV_AutoStep.Step400_InMode_Pass_CST_BP_to_BP:
                    return $"BP CV PIO Start (BP[{m_eBPtoBPStartBP - 2}] -> BP[{m_eBPtoBPStartBP - 2 + 1}])";
                case BP_CV_AutoStep.Step410_InMode_Move_BP_CV_Rolling:
                    return $"BP CV Run (BP[{m_eBPtoBPStartBP - 2}] -> BP[{m_eBPtoBPStartBP - 2 + 1}])";
                case BP_CV_AutoStep.Step420_InMode_Move_BP_CV_Stop:
                    return $"BP CV Stop (BP[{m_eBPtoBPStartBP - 2}] -> BP[{m_eBPtoBPStartBP - 2 + 1}])";
                case BP_CV_AutoStep.Step430_InMode_Pass_CST_ID:
                    return $"Pass BP CST ID (BP[{m_eBPtoBPStartBP - 2}] -> BP[{m_eBPtoBPStartBP - 2 + 1}])";

                case BP_CV_AutoStep.Step700_OutMode_Start_CST_Pass_From_OP:
                    return $"BP CV PIO Start (OP -> BP[{nLastBPIndex}])";
                case BP_CV_AutoStep.Step710_OutMode_Move_BP_CV_Rolling:
                    return $"BP CV Run (OP -> BP[{nLastBPIndex}])";
                case BP_CV_AutoStep.Step720_OutMode_Move_BP_CV_Stop:
                    return $"BP CV Stop (OP -> BP[{nLastBPIndex}])";
                case BP_CV_AutoStep.Step730_OutMode_Check_CST_ID:
                    return $"Check BP CST ID (OP -> BP[{nLastBPIndex}])";

                case BP_CV_AutoStep.Step800_OutMode_Start_CST_Pass_To_LP:
                    return $"BP CV PIO Start (BP[{nStartBPIndex}] -> LP)";
                case BP_CV_AutoStep.Step810_OutMode_Move_BP_CV_Rolling:
                    return $"BP CV Run (BP[{nStartBPIndex}] -> LP)";
                case BP_CV_AutoStep.Step820_OutMode_Move_BP_CV_Stop:
                    return $"BP CV Stop (BP[{nStartBPIndex}] -> LP)";
                case BP_CV_AutoStep.Step830_OutMode_Send_BP_CST_ID:
                    return $"Send BP CST ID (BP[{nStartBPIndex}] -> LP)";
                case BP_CV_AutoStep.Step840_OutMode_Clear_BP_CST_ID:
                    return $"Clear BP CST ID (BP[{nStartBPIndex}] -> LP)";

                case BP_CV_AutoStep.Step900_OutMode_Pass_CST_BP_to_BP:
                    return $"BP CV PIO Start (BP[{m_eBPtoBPStartBP - 2}] -> BP[{m_eBPtoBPStartBP - 2 - 1}])";
                case BP_CV_AutoStep.Step910_OutMode_Move_BP_CV_Rolling:
                    return $"BP CV Run (BP[{m_eBPtoBPStartBP - 2}] -> BP[{m_eBPtoBPStartBP - 2 - 1}])";
                case BP_CV_AutoStep.Step920_OutMode_Move_BP_CV_Stop:
                    return $"BP CV Stop (BP[{m_eBPtoBPStartBP - 2}] -> BP[{m_eBPtoBPStartBP - 2 - 1}])";
                case BP_CV_AutoStep.Step930_OutMode_Pass_CST_ID:
                    return $"Pass BP CST ID (BP[{m_eBPtoBPStartBP - 2}] -> BP[{m_eBPtoBPStartBP - 2 - 1}])";
                default:
                    return "Not def step str";
            }
        }

        /// <summary>
        /// LP or OP or BP의 현재 스텝 번호
        /// </summary>
        /// <returns></returns>
        private int Auto_CV_Get_LP_StepNum()
        {
            return (int)m_eLP_CV_AutoStep;
        }
        private int Auto_CV_Get_OP_StepNum()
        {
            return (int)m_eOP_CV_AutoStep;
        }
        private int Auto_CV_Get_BP_StepNum()
        {
            return (int)m_eBP_CV_AutoStep;
        }
    }
}
