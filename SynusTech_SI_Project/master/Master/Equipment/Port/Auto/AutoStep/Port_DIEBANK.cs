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
    /// Port_DIEBANK.cs는 Conveyor Type이며 실리콘 박스 Diebank STK가 기존 conveyor 구성과 PIO Type이 달라 별도로 간략히 구성한 공정입니다.
    /// </summary>
    partial class Port
    {
        public enum LP_CV_DIEBANK_AutoStep
        {
            Step000_Check_Direction = 0,

            Step100_InMode_Check_LP_CST_Load_And_Pose = 100,      

            Step200_InMode_Await_Check_Unload_REQ = 200,
            Step210_InMode_Check_PIO_Ready = 210,
            Step220_InMode_Check_PIO_Complete_Wait = 220,
            Step230_InMode_Check_PIO_End = 230,
            Step240_InMode_Check_CST_Load_And_Safe = 240,

            Step300_InMode_Await_MGV_CST_Load = 300,      

            Step400_InMode_Await_OP_Load_Req = 400,
            Step410_InMode_Move_LP_CV_Rolling = 410,
            Step420_InMode_Move_LP_CV_Stop = 420,

            Step600_OutMode_Check_LP_CST_Load_And_Pose = 600,
            Step610_OutMode_Move_LP_CV_Rolling = 610,
            Step620_OutMode_Move_LP_CV_Stop = 620,

            Step700_OutMode_Call_OP_Load_Req = 710,      //Carrier Load 요청
            Step710_OutMode_Move_LP_Rolling = 720,
            Step720_OutMode_Move_LP_CV_Stop = 730,
            Step730_OutMode_Check_CST_ID = 740,

            Step800_OutMode_Await_Check_PIO_Load_REQ = 800,
            Step810_OutMode_Check_PIO_Ready = 810,
            Step820_OutMode_Check_PIO_Complete_Wait = 820,
            Step830_OutMode_Check_PIO_End = 830,
            Step840_OutMode_Check_LP_CST_Unload_And_Safe = 840,      //Carrier ID Clear

            Step900_OutMode_Await_MGV_CST_Unload = 900,      //Carrier Unload 대기 (MGV), Carrier ID Clear
        }
        public enum OP_CV_DIEBANK_AutoStep
        {
            Step000_Check_Direction = 0,

            Step100_InMode_Check_OP_CST_Load_And_Pose = 100,

            Step110_InMode_Move_OP_Z_Up = 110,
            Step120_InMode_Move_OP_Y_FWD = 120,
            Step130_InMode_Move_OP_Y_BWD = 130,
            Step140_InMode_Move_OP_Z_Down = 140,
            Step150_InMode_Move_OP_Rolling = 150,
            Step160_InMode_Move_OP_CV_Stop = 160,

            Step200_InMode_Call_LP_Load_Req = 200,
            Step210_InMode_Move_OP_Rolling = 210,
            Step220_InMode_Move_OP_CV_Stop = 220,

            Step300_InMode_Ready_CST_ID_Read    = 300,      //Carrier ID Read Step
            Step310_InMode_Start_CST_ID_Read    = 310,
            Step320_InMode_Move_OP_Z_Up         = 320,
            Step330_InMode_CST_InPlaceCheck     = 330,
            Step340_InMode_Move_OP_Y_FWD        = 340,

            Step400_InMode_Await_PIO_TR_REQ = 400,
            Step410_InMode_Check_PIO_Busy = 410,
            Step420_InMode_Check_PIO_Complete = 420,
            Step430_InMode_Check_PIO_End = 430,
            Step440_InMode_Check_OP_CST_Unload_And_Safe = 440,


            Step600_OutMode_Check_OP_CST_Load_And_Pose = 600,
            Step610_OutMode_Move_OP_Z_Down = 610,
            Step620_OutMode_Move_OP_Z_Up = 620,
            Step630_InMode_Move_OP_Y_FWD = 630,
            Step640_InMode_Move_OP_Y_BWD = 640,

            Step700_OutMode_Await_PIO_TR_REQ = 700,      //Carrier Load 요청(RM)
            Step710_OutMode_Check_PIO_Busy = 710,
            Step720_OutMode_Check_PIO_Complete = 720,      //ID 복사 RM Word Map -> Port Word Map
            Step730_OutMode_Check_PIO_End = 730,      //ID 적용 Port WordMap -> OP Carrier ID
            Step740_OutMode_Check_OP_CST_Load_And_Safe = 740,

            Step800_InMode_Move_OP_Y_BWD = 800,
            Step810_OutMode_Move_OP_Z_Down = 810,

            Step900_OutMode_Await_LP_Load_Req = 900,
            Step910_OutMode_Move_OP_CV_Rolling = 910,
            Step920_OutMode_Move_OP_CV_Stop = 920,
            Step930_OutMode_Send_OP_CST_ID = 930,
            Step940_OutMode_Clear_OP_CST_ID = 940
        }

        /// <summary>
        /// LP 2BP Type In/Out Control
        /// </summary>
        LP_CV_DIEBANK_AutoStep m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step000_Check_Direction;
        OP_CV_DIEBANK_AutoStep m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step000_Check_Direction;

        LP_CV_DIEBANK_AutoStep Pre_LP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step000_Check_Direction;
        OP_CV_DIEBANK_AutoStep Pre_OP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step000_Check_Direction;

        bool m_bLP_CV_DIEBANK_AutoStopEnable = false;
        bool m_bOP_CV_DIEBANK_AutoStopEnable = false;

        //Auto
        private bool m_bLP_CV_DIEBANK_AutoRunning = false;
        private bool m_bOP_CV_DIEBANK_AutoRunning = false;

        /// <summary>
        /// OP or LP의 정지 가능 상태를 나타냄
        /// Call, Await Step에서 대부분 정지 가능(요청 또는 대기 상태)
        /// </summary>
        private void Auto_CV_LP_DIEBANK_StopEnableUpdate()
        {
            switch (m_eLP_CV_DIEBANK_AutoStep)
            {
                case LP_CV_DIEBANK_AutoStep.Step000_Check_Direction:
                case LP_CV_DIEBANK_AutoStep.Step100_InMode_Check_LP_CST_Load_And_Pose:
                case LP_CV_DIEBANK_AutoStep.Step200_InMode_Await_Check_Unload_REQ:
                case LP_CV_DIEBANK_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                case LP_CV_DIEBANK_AutoStep.Step400_InMode_Await_OP_Load_Req:
                case LP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose:
                case LP_CV_DIEBANK_AutoStep.Step700_OutMode_Call_OP_Load_Req:
                case LP_CV_DIEBANK_AutoStep.Step800_OutMode_Await_Check_PIO_Load_REQ:
                case LP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_MGV_CST_Unload:
                    m_bLP_CV_DIEBANK_AutoStopEnable = true;
                    break;
                default:
                    m_bLP_CV_DIEBANK_AutoStopEnable = false;
                    break;
            }
        }
        private void Auto_CV_OP_DIEBANK_StopEnableUpdate()
        {
            switch (m_eOP_CV_DIEBANK_AutoStep)
            {
                case OP_CV_DIEBANK_AutoStep.Step000_Check_Direction:
                case OP_CV_DIEBANK_AutoStep.Step200_InMode_Call_LP_Load_Req:
                case OP_CV_DIEBANK_AutoStep.Step400_InMode_Await_PIO_TR_REQ:
                //case OP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose:
                case OP_CV_DIEBANK_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                case OP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_LP_Load_Req:
                    m_bOP_CV_DIEBANK_AutoStopEnable = true;
                    break;
                default:
                    m_bOP_CV_DIEBANK_AutoStopEnable = false;
                    break;
            }
        }

        /// <summary>
        /// 자재가 있어야 할 스텝, 없어야 할 스텝을 구분하여 자재 판단
        /// </summary>
        private void Auto_CV_LP_DIEBANK_Placement_Detect_AlarmCheck()
        {
            switch (m_eLP_CV_DIEBANK_AutoStep)
            {
                //없어야 함
                case LP_CV_DIEBANK_AutoStep.Step200_InMode_Await_Check_Unload_REQ:
                case LP_CV_DIEBANK_AutoStep.Step700_OutMode_Call_OP_Load_Req:
                case LP_CV_DIEBANK_AutoStep.Step830_OutMode_Check_PIO_End:
                case LP_CV_DIEBANK_AutoStep.Step840_OutMode_Check_LP_CST_Unload_And_Safe:
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
                case LP_CV_DIEBANK_AutoStep.Step230_InMode_Check_PIO_End:
                case LP_CV_DIEBANK_AutoStep.Step240_InMode_Check_CST_Load_And_Safe:
                case LP_CV_DIEBANK_AutoStep.Step400_InMode_Await_OP_Load_Req:
                case LP_CV_DIEBANK_AutoStep.Step800_OutMode_Await_Check_PIO_Load_REQ:
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
        private void Auto_CV_OP_DIEBANK_Placement_Detect_AlarmCheck()
        {
            switch (m_eOP_CV_DIEBANK_AutoStep)
            {
                //있어야 함
                case OP_CV_DIEBANK_AutoStep.Step120_InMode_Move_OP_Y_FWD:
                case OP_CV_DIEBANK_AutoStep.Step300_InMode_Ready_CST_ID_Read:
                case OP_CV_DIEBANK_AutoStep.Step310_InMode_Start_CST_ID_Read:
                case OP_CV_DIEBANK_AutoStep.Step320_InMode_Move_OP_Z_Up: //스텝중 Inplace 체크하다 CV In, Stop으로 변경 해야함
                case OP_CV_DIEBANK_AutoStep.Step330_InMode_CST_InPlaceCheck:
                case OP_CV_DIEBANK_AutoStep.Step340_InMode_Move_OP_Y_FWD:
                case OP_CV_DIEBANK_AutoStep.Step400_InMode_Await_PIO_TR_REQ:
                case OP_CV_DIEBANK_AutoStep.Step410_InMode_Check_PIO_Busy:
                case OP_CV_DIEBANK_AutoStep.Step610_OutMode_Move_OP_Z_Down: //스텝중 Inplace 체크하다 CV In, Stop으로 변경 해야함
                case OP_CV_DIEBANK_AutoStep.Step640_InMode_Move_OP_Y_BWD:
                case OP_CV_DIEBANK_AutoStep.Step730_OutMode_Check_PIO_End:
                case OP_CV_DIEBANK_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe:
                case OP_CV_DIEBANK_AutoStep.Step800_InMode_Move_OP_Y_BWD:
                case OP_CV_DIEBANK_AutoStep.Step810_OutMode_Move_OP_Z_Down:
                case OP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_LP_Load_Req:
                    {
                        if (IsZAxisPos_UP(PortAxis.Buffer_OP_Z))
                        {
                            if (!Sensor_OP_CST_Detect1 || !Sensor_OP_CST_Detect2)
                            {
                                Watchdog_Start(WatchdogList.OP_Placement_ErrorTimer);
                                if (Watchdog_IsDetect(WatchdogList.OP_Placement_ErrorTimer))
                                    AlarmInsert((short)PortAlarm.OP_Placement_Detect_Error, AlarmLevel.Error);
                            }
                            else
                                Watchdog_Stop(WatchdogList.OP_Placement_ErrorTimer, true);
                        }
                        else if (IsZAxisPos_DOWN(PortAxis.Buffer_OP_Z))
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
                    }
                    break;
                //없어야 함
                case OP_CV_DIEBANK_AutoStep.Step130_InMode_Move_OP_Y_BWD:
                case OP_CV_DIEBANK_AutoStep.Step140_InMode_Move_OP_Z_Down:
                case OP_CV_DIEBANK_AutoStep.Step200_InMode_Call_LP_Load_Req:
                case OP_CV_DIEBANK_AutoStep.Step430_InMode_Check_PIO_End:
                //case OP_CV_DIEBANK_AutoStep.Step440_InMode_Check_OP_CST_Unload_And_Safe: //있을수도 있고 없을수도 있고
                case OP_CV_DIEBANK_AutoStep.Step620_OutMode_Move_OP_Z_Up:
                case OP_CV_DIEBANK_AutoStep.Step630_InMode_Move_OP_Y_FWD:
                case OP_CV_DIEBANK_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                case OP_CV_DIEBANK_AutoStep.Step710_OutMode_Check_PIO_Busy:
                case OP_CV_DIEBANK_AutoStep.Step930_OutMode_Send_OP_CST_ID:
                case OP_CV_DIEBANK_AutoStep.Step940_OutMode_Clear_OP_CST_ID:
                    {
                        if (IsZAxisPos_UP(PortAxis.Buffer_OP_Z))
                        {
                            if (Sensor_OP_CST_Detect1 || Sensor_OP_CST_Detect2)
                            {
                                Watchdog_Start(WatchdogList.OP_Placement_ErrorTimer);
                                if (Watchdog_IsDetect(WatchdogList.OP_Placement_ErrorTimer))
                                    AlarmInsert((short)PortAlarm.OP_Placement_Detect_Error, AlarmLevel.Error);
                            }
                            else
                                Watchdog_Stop(WatchdogList.OP_Placement_ErrorTimer, true);
                        }
                        else if (IsZAxisPos_DOWN(PortAxis.Buffer_OP_Z))
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
        private void Auto_CV_LP_DIEBANK_StepTimeOut_AlarmCheck()
        {
            if (m_eLP_CV_DIEBANK_AutoStep == Pre_LP_CV_DIEBANK_AutoStep &&
                !m_bLP_CV_DIEBANK_AutoStopEnable)
            {
                Watchdog_Start(WatchdogList.LP_Step_Timer);
                if (Watchdog_IsDetect(WatchdogList.LP_Step_Timer))
                    AlarmInsert((short)PortAlarm.Step_TimeOver_Error, AlarmLevel.Error);
            }
            else
            {
                Watchdog_Stop(WatchdogList.LP_Step_Timer, true);
            }

            if (m_eLP_CV_DIEBANK_AutoStep != Pre_LP_CV_DIEBANK_AutoStep)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortStepInfo, $"CV LP Step : {Pre_LP_CV_DIEBANK_AutoStep} => {m_eLP_CV_DIEBANK_AutoStep}");

            Pre_LP_CV_DIEBANK_AutoStep = m_eLP_CV_DIEBANK_AutoStep;
        }
        private void Auto_CV_OP_DIEBANK_StepTimeOut_AlarmCheck()
        {
            if (m_eOP_CV_DIEBANK_AutoStep == Pre_OP_CV_DIEBANK_AutoStep &&
                !m_bOP_CV_DIEBANK_AutoStopEnable)
            {
                Watchdog_Start(WatchdogList.OP_Step_Timer);
                if (Watchdog_IsDetect(WatchdogList.OP_Step_Timer))
                    AlarmInsert((short)PortAlarm.Step_TimeOver_Error, AlarmLevel.Error);
            }
            else
            {
                Watchdog_Stop(WatchdogList.OP_Step_Timer, true);
            }

            if (m_eOP_CV_DIEBANK_AutoStep != Pre_OP_CV_DIEBANK_AutoStep)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortStepInfo, $"CV OP Step : {Pre_OP_CV_DIEBANK_AutoStep} => {m_eOP_CV_DIEBANK_AutoStep}");

            Pre_OP_CV_DIEBANK_AutoStep = m_eOP_CV_DIEBANK_AutoStep;
        }

        /// <summary>
        /// OMRON 및 STK의 PIO 과정에서 Timeout 확인
        /// </summary>
        private void Auto_CV_OMRON_PIO_WatchdogUpdate()
        {
            if ((m_eLP_CV_DIEBANK_AutoStep >= LP_CV_DIEBANK_AutoStep.Step810_OutMode_Check_PIO_Ready && m_eLP_CV_DIEBANK_AutoStep <= LP_CV_DIEBANK_AutoStep.Step830_OutMode_Check_PIO_End) ||
                (m_eLP_CV_DIEBANK_AutoStep >= LP_CV_DIEBANK_AutoStep.Step210_InMode_Check_PIO_Ready && m_eLP_CV_DIEBANK_AutoStep <= LP_CV_DIEBANK_AutoStep.Step230_InMode_Check_PIO_End))
            {
                Watchdog_Start(WatchdogList.AGVorOHT_PIO_Timer);
            }
            else
            {
                Watchdog_Stop(WatchdogList.AGVorOHT_PIO_Timer, true);
            }
        }
        private void Auto_CV_DIEBANK_RackMaster_PIO_WatchdogUpdate()
        {
            if ((m_eOP_CV_DIEBANK_AutoStep >= OP_CV_DIEBANK_AutoStep.Step710_OutMode_Check_PIO_Busy && m_eOP_CV_DIEBANK_AutoStep <= OP_CV_DIEBANK_AutoStep.Step730_OutMode_Check_PIO_End) ||
                (m_eOP_CV_DIEBANK_AutoStep >= OP_CV_DIEBANK_AutoStep.Step410_InMode_Check_PIO_Busy && m_eOP_CV_DIEBANK_AutoStep <= OP_CV_DIEBANK_AutoStep.Step430_InMode_Check_PIO_End))
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
        private void Auto_CV_LP_DIEBANK_StopInit()
        {
            m_bLP_CV_DIEBANK_AutoRunning = false;
            m_bCycleRunning = false;
            Port_To_OMRON_PIO_Init();
            Watchdog_Stop(WatchdogList.LP_Placement_ErrorTimer, false);
            Watchdog_Stop(WatchdogList.AGVorOHT_PIO_Timer, false);

            CMD_PortStop();
        }
        private void Auto_CV_OP_DIEBANK_StopInit()
        {
            m_bOP_CV_DIEBANK_AutoRunning = false;
            m_bCycleRunning = false;
            Port_To_RM_PIO_Init();
            Watchdog_Stop(WatchdogList.OP_Placement_ErrorTimer, false);
            Watchdog_Stop(WatchdogList.RackMaster_PIO_Timer, true);

            m_RMCSTIDRWTimer.Stop();
            m_RMCSTIDRWTimer.Reset();

            CMD_PortStop();
        }

        /// <summary>
        /// CV 회전 명령
        /// </summary>
        /// <param name="eBufferCV"></param>
        private void CVFreqFWDRolling(BufferCV eBufferCV)
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
        private void CVFreqBWDRolling(BufferCV eBufferCV)
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
        private void CVFreqRollingStop(BufferCV eBufferCV)
        {
            if (GetMotionParam().GetBufferCVParam(eBufferCV).CVParam.InvCtrlMode == InvCtrlMode.IOControl)
            {
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.LowSpeedBWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.LowSpeedFWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.HighSpeedBWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.HighSpeedFWD, false, true, InterlockFrom.ApplicationLoop);
            }
            else
            {
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.FreqFWD, false, true, InterlockFrom.ApplicationLoop);
                Interlock_SetConveyorMove(eBufferCV, InvCtrlType.FreqBWD, false, true, InterlockFrom.ApplicationLoop);
            }
        }

        /// <summary>
        /// 중간 연결부 대각 감지
        /// </summary>
        /// <returns></returns>
        private bool IsLPOppositeAngle()
        {
            return Sensor_LP_CST_Presence;
        }
        private bool IsOPOppositeAngle()
        {
            return Sensor_OP_CST_Presence;
        }

        /// <summary>
        /// 실제 오토 공정 함수
        /// </summary>
        private void Auto_CV_Start_DIEBANK_LP_Control()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                while (m_bLP_CV_DIEBANK_AutoRunning || m_bCycleRunning)
                {
                    if (m_bLP_CV_DIEBANK_AutoRunning && m_bCycleRunning)
                    {
                        Auto_CV_LP_DIEBANK_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAndCycleCrash, $"LP CV");
                        break;
                    }

                    //Global Alarm Check
                    if (GetAlarmLevel() == AlarmLevel.Error)
                    {
                        Auto_CV_LP_DIEBANK_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAlarmStop, $"LP CV");
                        break;
                    }

                    //My Type Alarm Check
                    Auto_CV_LP_DIEBANK_Placement_Detect_AlarmCheck();
                    Auto_CV_LP_DIEBANK_StepTimeOut_AlarmCheck();

                    //Update
                    Auto_CV_LP_DIEBANK_StopEnableUpdate();
                    Auto_CV_OMRON_PIO_WatchdogUpdate();

                    if (IsAutoStopReq(!m_bCycleRunning))
                    {
                        if (m_bLP_CV_DIEBANK_AutoStopEnable)
                        {
                            Auto_CV_LP_DIEBANK_StopInit();
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AutoControlStopCommand, $"LP CV");
                            break;
                        }
                    }

                    switch (m_eLP_CV_DIEBANK_AutoStep)
                    {
                        case LP_CV_DIEBANK_AutoStep.Step000_Check_Direction:
                            {
                                Port_To_OMRON_PIO_Init();
                                CVFreqRollingStop(BufferCV.Buffer_LP);

                                if (GetOperationDirection() == PortDirection.Input)
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step100_InMode_Check_LP_CST_Load_And_Pose;
                                else
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose;
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step100_InMode_Check_LP_CST_Load_And_Pose:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) || IsLPOppositeAngle())
                                {
                                    if(m_bCycleRunning)
                                        m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step300_InMode_Await_MGV_CST_Load;
                                    else
                                        m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step400_InMode_Await_OP_Load_Req;
                                }
                                else
                                {
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step200_InMode_Await_Check_Unload_REQ;
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step200_InMode_Await_Check_Unload_REQ:
                            {
                                bool bOmronConnection = (Master.m_Omron?.IsConnected() ?? false);
                                bool bOmronValid = (Master.m_Omron?.m_OmronValid ?? false);

                                if (!bOmronConnection)
                                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Warning, LogMsg.MsgList.OMRON_Info, $"Omron Communicator Not Connected!");
                                else if(!bOmronValid)
                                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Warning, LogMsg.MsgList.OMRON_Info, $"Omron Communicator Invalid State!");

                                if (PIOStatus_OMRONToPort_Unload_REQ)
                                {
                                    PIOStatus_PortToOMRON_TR_REQ = true;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step210_InMode_Check_PIO_Ready;
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step210_InMode_Check_PIO_Ready:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (!PIOStatus_OMRONToPort_Unload_REQ || !PIOStatus_OMRONToPort_Ready)
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (PIOStatus_OMRONToPort_Unload_REQ &&
                                    PIOStatus_OMRONToPort_Ready)
                                {
                                    PIOStatus_PortToOMRON_Busy_REQ = true;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                    CVFreqFWDRolling(BufferCV.Buffer_LP);

                                    if (Sensor_LP_CV_FWD_Status)
                                        m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step220_InMode_Check_PIO_Complete_Wait;

                                    //if (Sensor_LP_CV_FWD_Status)
                                    //{
                                    //    // 240215 - Louis 수정 : Diebank STK의 경우 Tag reader가 None일 때 Omron PLC 메모리맵에서 id를 가져오도록 함
                                    //    if (GetParam().eTagReaderType == TagReader.TagReaderType.None)
                                    //    {
                                    //        string LPCarrier_ID = OMRON_To_Port_CarrierID;
                                    //        if (LPCarrier_ID != string.Empty)
                                    //            LP_CarrierID = LPCarrier_ID;
                                    //    }
                                    //    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step220_InMode_Check_PIO_Complete_Wait;
                                    //}
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step220_InMode_Check_PIO_Complete_Wait:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (!PIOStatus_OMRONToPort_Unload_REQ || !PIOStatus_OMRONToPort_Ready)
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }
                                
                                if (Sensor_LP_CV_STOP)
                                {
                                    CVStop(BufferCV.Buffer_LP);
                                }

                                if (GetParam().eTagReaderType == TagReader.TagReaderType.None)
                                {
                                    string OMRON_Carrier_ID = OMRON_To_Port_CarrierID;
                                    if (OMRON_Carrier_ID != string.Empty)
                                    {
                                        Port_To_OMRON_CarrierID = OMRON_Carrier_ID;
                                        LP_CarrierID            = OMRON_Carrier_ID;
                                    }
                                }

                                if (PIOStatus_OMRONToPort_Unload_REQ &&
                                    PIOStatus_OMRONToPort_Ready &&
                                    Sensor_LP_CV_STOP)
                                {
                                    PIOStatus_PortToOMRON_Busy_REQ = false;
                                    PIOStatus_PortToOMRON_Complete = true;
                                    PIOStatus_PortToOMRON_TR_REQ = false;
                                    CVStop(BufferCV.Buffer_LP);
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step230_InMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step230_InMode_Check_PIO_End:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (PIOStatus_OMRONToPort_Unload_REQ || PIOStatus_OMRONToPort_Ready)
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (!PIOStatus_OMRONToPort_Unload_REQ &&
                                    !PIOStatus_OMRONToPort_Ready)
                                {
                                    PIOStatus_PortToOMRON_Complete = false;
                                    Port_To_OMRON_CarrierID = string.Empty;
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step240_InMode_Check_CST_Load_And_Safe;
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step240_InMode_Check_CST_Load_And_Safe:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) && !Is_LightCurtain_or_Hoist_SensorCheck())
                                {
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step400_InMode_Await_OP_Load_Req;
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                            {
                                if ((Carrier_CheckLP_ExistProduct(true) || IsLPOppositeAngle()) && 
                                    !Is_LightCurtain_or_Hoist_SensorCheck())
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step400_InMode_Await_OP_Load_Req;
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step400_InMode_Await_OP_Load_Req:
                            {
                                if (m_eOP_CV_DIEBANK_AutoStep == OP_CV_DIEBANK_AutoStep.Step200_InMode_Call_LP_Load_Req)
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step410_InMode_Move_LP_CV_Rolling;
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step410_InMode_Move_LP_CV_Rolling:
                            {
                                CVFreqFWDRolling(BufferCV.Buffer_LP);

                                if (m_eOP_CV_DIEBANK_AutoStep == OP_CV_DIEBANK_AutoStep.Step210_InMode_Move_OP_Rolling && Sensor_LP_CV_FWD_Status)
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step420_InMode_Move_LP_CV_Stop;
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step420_InMode_Move_LP_CV_Stop:
                            {
                                if (Carrier_CheckLP_ExistProduct(false, false) && 
                                    !IsLPOppositeAngle() &&
                                    Carrier_CheckOP_ExistProduct(true))
                                {
                                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"[In Direction] LP CV Stop, Sensor Info: LP CV IN[{Sensor_LP_CV_IN}], LP CV STOP[{Sensor_LP_CV_STOP}], LP Opposite[{IsLPOppositeAngle()}], OP CV IN[{Sensor_OP_CV_IN}], OP CV STOP[{Sensor_OP_CV_STOP}]");
                                    CVStop(BufferCV.Buffer_LP);

                                    if (Carrier_CheckLP_ExistID())
                                    {
                                        OP_CarrierID = LP_CarrierID;
                                        LP_CarrierID = string.Empty;
                                    }

                                    if (!m_bCycleRunning)
                                        m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step000_Check_Direction;
                                    else
                                    {
                                        if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                        {
                                            //Inmode인 경우에는 OP에서 카운트를 증가
                                            //m_CycleControlProgressCount++;
                                            //-----------Direction Change 실행
                                            Port_To_OMRON_PIO_Init();
                                            m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose;
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
                        case LP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose:
                            {
                                if (Carrier_CheckLP_ExistProduct(false) && IsLPOppositeAngle())
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step610_OutMode_Move_LP_CV_Rolling;
                                else
                                {
                                    if (Carrier_CheckLP_ExistProduct(true) && Carrier_CheckLP_ExistID())
                                    {
                                        if(m_bCycleRunning)
                                            m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_MGV_CST_Unload;
                                        else
                                            m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step800_OutMode_Await_Check_PIO_Load_REQ;
                                    }
                                    else if(Carrier_CheckLP_ExistProduct(false) && !Carrier_CheckLP_ExistID())
                                    {
                                        m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step700_OutMode_Call_OP_Load_Req;
                                    }
                                    else if(Carrier_CheckLP_ExistProduct(true) && !Carrier_CheckLP_ExistID())
                                    {
                                        AlarmInsert((short)PortAlarm.LP_No_Cassette_ID_Error, AlarmLevel.Error);
                                    }
                                    else if (Carrier_CheckLP_ExistProduct(false) && Carrier_CheckLP_ExistID())
                                    {
                                        LP_CarrierID = string.Empty;
                                    }
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step610_OutMode_Move_LP_CV_Rolling:
                            {
                                CVFreqBWDRolling(BufferCV.Buffer_LP);

                                if(Sensor_LP_CV_BWD_Status)
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step620_OutMode_Move_LP_CV_Stop;
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step620_OutMode_Move_LP_CV_Stop:
                            {
                                if (Sensor_LP_CV_IN)
                                {
                                    CVStop(BufferCV.Buffer_LP);
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step700_OutMode_Call_OP_Load_Req:
                            {
                                if (m_eOP_CV_DIEBANK_AutoStep == OP_CV_DIEBANK_AutoStep.Step910_OutMode_Move_OP_CV_Rolling)
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step710_OutMode_Move_LP_Rolling;
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step710_OutMode_Move_LP_Rolling:
                            {
                                CVFreqBWDRolling(BufferCV.Buffer_LP);

                                if (m_eOP_CV_DIEBANK_AutoStep == OP_CV_DIEBANK_AutoStep.Step920_OutMode_Move_OP_CV_Stop && Sensor_LP_CV_BWD_Status)
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step720_OutMode_Move_LP_CV_Stop;
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step720_OutMode_Move_LP_CV_Stop:
                            {
                                if (Sensor_LP_CV_IN)
                                {
                                    CVStop(BufferCV.Buffer_LP);
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step730_OutMode_Check_CST_ID;
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step730_OutMode_Check_CST_ID:
                            {
                                if (Carrier_CheckLP_ExistProduct(true) &&
                                    Carrier_CheckLP_ExistID())
                                {
                                    if(!m_bCycleRunning)
                                        m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step800_OutMode_Await_Check_PIO_Load_REQ;
                                    else
                                        m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_MGV_CST_Unload;
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step800_OutMode_Await_Check_PIO_Load_REQ:
                            {
                                bool bOmronConnection = (Master.m_Omron?.IsConnected() ?? false);
                                bool bOmronValid = (Master.m_Omron?.m_OmronValid ?? false);

                                if (!bOmronConnection)
                                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Warning, LogMsg.MsgList.OMRON_Info, $"Omron Communicator Not Connected!");
                                else if (!bOmronValid)
                                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Warning, LogMsg.MsgList.OMRON_Info, $"Omron Communicator Invalid State!");

                                if (PIOStatus_OMRONToPort_Load_REQ &&
                                    Carrier_CheckLP_ExistProduct(true))
                                {
                                    PIOStatus_PortToOMRON_TR_REQ = true;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step810_OutMode_Check_PIO_Ready;
                                }
                                else if(Carrier_CheckLP_ExistProduct(false))
                                {
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step810_OutMode_Check_PIO_Ready:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (!PIOStatus_OMRONToPort_Load_REQ || !PIOStatus_OMRONToPort_Ready)
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (PIOStatus_OMRONToPort_Load_REQ &&
                                    PIOStatus_OMRONToPort_Ready &&
                                    Carrier_CheckLP_ExistProduct(true))
                                {
                                    Port_To_OMRON_CarrierID = LP_CarrierID;
                                    PIOStatus_PortToOMRON_Busy_REQ = true;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                    CVFreqBWDRolling(BufferCV.Buffer_LP);
                                    if (Sensor_LP_CV_BWD_Status)
                                        m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step820_OutMode_Check_PIO_Complete_Wait;
                                }
                                else if (Carrier_CheckLP_ExistProduct(false))
                                {
                                    PIOStatus_PortToOMRON_TR_REQ = false;
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step820_OutMode_Check_PIO_Complete_Wait:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (PIOStatus_OMRONToPort_Load_REQ || !PIOStatus_OMRONToPort_Ready)
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (Carrier_CheckLP_ExistProduct(false))
                                {
                                    LP_CarrierID = string.Empty;
                                    CVStop(BufferCV.Buffer_LP);
                                }

                                if (!PIOStatus_OMRONToPort_Load_REQ &&
                                    PIOStatus_OMRONToPort_Ready &&
                                    Carrier_CheckLP_ExistProduct(false))
                                {
                                    PIOStatus_PortToOMRON_Busy_REQ = false;
                                    PIOStatus_PortToOMRON_Complete = true;
                                    PIOStatus_PortToOMRON_TR_REQ = false;
                                    CVStop(BufferCV.Buffer_LP);
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step830_OutMode_Check_PIO_End;
                                    Watchdog_Restart(WatchdogList.AGVorOHT_PIO_Timer);
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step830_OutMode_Check_PIO_End:
                            {
                                if (Watchdog_IsDetect(WatchdogList.AGVorOHT_PIO_Timer))
                                {
                                    if (PIOStatus_OMRONToPort_Load_REQ || PIOStatus_OMRONToPort_Ready)
                                        AlarmInsert((short)PortAlarm.Port_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                }

                                if (!PIOStatus_OMRONToPort_Load_REQ &&
                                    !PIOStatus_OMRONToPort_Ready &&
                                    (OMRON_To_Port_CarrierID == Port_To_OMRON_CarrierID))
                                {
                                    PIOStatus_PortToOMRON_Complete = false;
                                    Port_To_OMRON_CarrierID = string.Empty;
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step840_OutMode_Check_LP_CST_Unload_And_Safe;
                                }
                            }
                            break;
                        case LP_CV_DIEBANK_AutoStep.Step840_OutMode_Check_LP_CST_Unload_And_Safe:
                        case LP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_MGV_CST_Unload:
                            {
                                if (Carrier_CheckLP_ExistProduct(false) && !Is_LightCurtain_or_Hoist_SensorCheck() && !m_bCycleRunning)
                                {
                                    LP_CarrierID = string.Empty;
                                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step000_Check_Direction;
                                }
                                else if (!Is_LightCurtain_or_Hoist_SensorCheck() && m_bCycleRunning)
                                {
                                    LP_CarrierID = string.Empty;
                                    if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                    {
                                        //Outmode인 경우에는 LP에서 카운트를 증가
                                        m_CycleControlProgressCount++;
                                        //-----------Direction Change 실행
                                        Port_To_OMRON_PIO_Init();
                                        GetMotionParam().ePortDirection = PortDirection.Input;
                                        m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step100_InMode_Check_LP_CST_Load_And_Pose;
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
        private void Auto_CV_Start_DIEBANK_OP_Control()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                while (m_bOP_CV_DIEBANK_AutoRunning || m_bCycleRunning)
                {
                    if (m_bOP_CV_DIEBANK_AutoRunning && m_bCycleRunning)
                    {
                        Auto_CV_OP_DIEBANK_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAndCycleCrash, $"OP CV");
                        break;
                    }

                    //Global Alarm Check
                    if (GetAlarmLevel() == AlarmLevel.Error)
                    {
                        Auto_CV_OP_DIEBANK_StopInit();
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AutoControlAlarmStop, $"OP CV");
                        break;
                    }

                    //My Type Alarm Check
                    Auto_CV_OP_DIEBANK_Placement_Detect_AlarmCheck();
                    Auto_CV_OP_DIEBANK_StepTimeOut_AlarmCheck();

                    //Update
                    Auto_CV_OP_DIEBANK_StopEnableUpdate();
                    Auto_CV_DIEBANK_RackMaster_PIO_WatchdogUpdate();

                    if (IsAutoStopReq(!m_bCycleRunning))
                    {
                        if (m_bOP_CV_DIEBANK_AutoStopEnable)
                        {
                            Auto_CV_OP_DIEBANK_StopInit();
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AutoControlStopCommand, $"OP CV");
                            break;
                        }
                    }


                    switch (m_eOP_CV_DIEBANK_AutoStep)
                    {
                        case OP_CV_DIEBANK_AutoStep.Step000_Check_Direction:
                            {
                                Port_To_RM_PIO_Init();

                                if (GetOperationDirection() == PortDirection.Input)
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose;
                                else
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose:
                            {
                                if(IsZAxisPos_UP(PortAxis.Buffer_OP_Z))
                                {
                                    if (Sensor_OP_CST_Detect1 && Sensor_OP_CST_Detect2 && Carrier_CheckOP_ExistID())
                                    {
                                        if (!IsYAxisPos_FWD(PortAxis.Buffer_OP_Y))
                                            m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step120_InMode_Move_OP_Y_FWD;
                                        else
                                            m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step400_InMode_Await_PIO_TR_REQ;
                                    }
                                    else
                                    {
                                        if (!IsYAxisPos_BWD(PortAxis.Buffer_OP_Y))
                                            m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step130_InMode_Move_OP_Y_BWD;
                                        else
                                            m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step140_InMode_Move_OP_Z_Down;
                                    }
                                }
                                else if(IsZAxisPos_DOWN(PortAxis.Buffer_OP_Z))
                                {
                                    if (Carrier_CheckOP_ExistProduct(true))
                                    {
                                        if (Sensor_OP_CV_STOP)
                                        {
                                            if (Carrier_CheckOP_ExistID())
                                            {
                                                if (!IsZAxisPos_UP(PortAxis.Buffer_OP_Z))
                                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step110_InMode_Move_OP_Z_Up;
                                            }
                                            else
                                            {
                                                m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step300_InMode_Ready_CST_ID_Read;
                                            }
                                        }
                                        else
                                            m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step150_InMode_Move_OP_Rolling;
                                    }
                                    else if (Carrier_CheckOP_ExistProduct(false))
                                    {
                                        if (Carrier_CheckOP_ExistID())
                                            OP_CarrierID = string.Empty;

                                        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step200_InMode_Call_LP_Load_Req;
                                    }
                                }
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step110_InMode_Move_OP_Z_Up:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_OP_Z, Teaching_Z_Pos.Up_Pos))
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step120_InMode_Move_OP_Y_FWD:
                            {
                                if (Y_Axis_MotionAndDone(PortAxis.Buffer_OP_Y, Teaching_Y_Pos.FWD_Pos))
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step130_InMode_Move_OP_Y_BWD:
                            {
                                if (Y_Axis_MotionAndDone(PortAxis.Buffer_OP_Y, Teaching_Y_Pos.BWD_Pos))
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step140_InMode_Move_OP_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_OP_Z, Teaching_Z_Pos.Down_Pos))
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step150_InMode_Move_OP_Rolling:
                            {
                                CVFreqFWDRolling(BufferCV.Buffer_OP);

                                if (Sensor_OP_CV_FWD_Status)
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step160_InMode_Move_OP_CV_Stop;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step160_InMode_Move_OP_CV_Stop:
                            {
                                if (Sensor_OP_CV_STOP)
                                {
                                    Thread.Sleep(2000);
                                    CVStop(BufferCV.Buffer_OP);
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose;
                                }
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step200_InMode_Call_LP_Load_Req:
                            {
                                if (m_eLP_CV_DIEBANK_AutoStep == LP_CV_DIEBANK_AutoStep.Step410_InMode_Move_LP_CV_Rolling)
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step210_InMode_Move_OP_Rolling;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step210_InMode_Move_OP_Rolling:
                            {
                                CVFreqFWDRolling(BufferCV.Buffer_OP);

                                    if (m_eLP_CV_DIEBANK_AutoStep >= LP_CV_DIEBANK_AutoStep.Step420_InMode_Move_LP_CV_Stop && Sensor_OP_CV_FWD_Status)
                                        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step220_InMode_Move_OP_CV_Stop;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step220_InMode_Move_OP_CV_Stop:
                            {
                                if (Sensor_OP_CV_STOP)
                                {
                                    Thread.Sleep(2000);
                                    CVStop(BufferCV.Buffer_OP);
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step300_InMode_Ready_CST_ID_Read;
                                }
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step300_InMode_Ready_CST_ID_Read:
                            {
                                if (GetParam().eTagReaderType == TagReader.TagReaderType.None && Carrier_CheckOP_ExistID())
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step320_InMode_Move_OP_Z_Up;
                                else if(GetParam().eTagReaderType != TagReader.TagReaderType.None)
                                {
                                    if (TAG_READ_INIT())
                                        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step310_InMode_Start_CST_ID_Read;
                                }
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step310_InMode_Start_CST_ID_Read:
                            {
                                if (Carrier_CheckOP_ExistProduct(true))
                                {
                                    if (TAG_READ_TRY(TAG_ID_READ_SET_SECTION.OP))
                                        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step320_InMode_Move_OP_Z_Up;
                                }
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step320_InMode_Move_OP_Z_Up:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_OP_Z, Teaching_Z_Pos.Up_Pos))
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step330_InMode_CST_InPlaceCheck;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step330_InMode_CST_InPlaceCheck:
                            {
                                if (Sensor_OP_CST_Detect1 && Sensor_OP_CST_Detect2)
                                {
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step340_InMode_Move_OP_Y_FWD;
                                }
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step340_InMode_Move_OP_Y_FWD:
                            {
                                if (Y_Axis_MotionAndDone(PortAxis.Buffer_OP_Y, Teaching_Y_Pos.FWD_Pos))
                                {
                                    if (!m_bCycleRunning)
                                        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step400_InMode_Await_PIO_TR_REQ;
                                    else
                                        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step440_InMode_Check_OP_CST_Unload_And_Safe;
                                }
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step400_InMode_Await_PIO_TR_REQ:
                            {
                                if (INPUT_DIR_STK_TR_REQ_Await())
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step410_InMode_Check_PIO_Busy;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step410_InMode_Check_PIO_Busy:
                            {
                                if (INPUT_DIR_STK_BUSY_Check())
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step420_InMode_Check_PIO_Complete;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step420_InMode_Check_PIO_Complete:
                            {
                                if (INPUT_DIR_STK_COMPLETE_Check())
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step430_InMode_Check_PIO_End;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step430_InMode_Check_PIO_End:
                            {
                                if (INPUT_DIR_STK_PIO_END_Check())
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step440_InMode_Check_OP_CST_Unload_And_Safe;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step440_InMode_Check_OP_CST_Unload_And_Safe:
                            {
                                if (!Sensor_OP_CST_Detect1 && 
                                    !Sensor_OP_CST_Detect2 && 
                                    !Sensor_OP_Fork_Detect && 
                                    !m_bCycleRunning)
                                {
                                    Carrier_ClearPortToRM_CarrierID();
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step000_Check_Direction;
                                }
                                else if ((IsYAxisPos_FWD(PortAxis.Buffer_OP_Y) ? true : !Sensor_OP_Fork_Detect) && m_bCycleRunning)
                                {
                                    //Carrier_ClearPortToRM_CarrierID();
                                    if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                    {
                                        //Inmode인 경우에는 OP에서 카운트를 증가
                                        m_CycleControlProgressCount++;
                                        //-----------Direction Change 실행
                                        Port_To_RM_PIO_Init();
                                        GetMotionParam().ePortDirection = PortDirection.Output;
                                        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose;
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
                        case OP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose:
                            {
                                if(IsZAxisPos_UP(PortAxis.Buffer_OP_Z))
                                {
                                    if (Sensor_OP_CST_Detect1 && Sensor_OP_CST_Detect2 && Carrier_CheckOP_ExistID())
                                    {
                                        if(!IsYAxisPos_BWD(PortAxis.Buffer_OP_Y))
                                            m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step640_InMode_Move_OP_Y_BWD;
                                        else
                                            m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step610_OutMode_Move_OP_Z_Down;
                                    }
                                    else if (Sensor_OP_CST_Detect1 && Sensor_OP_CST_Detect2 && !Carrier_CheckOP_ExistID())
                                    {
                                        AlarmInsert((short)PortAlarm.OP_No_Cassette_ID_Error, AlarmLevel.Error);
                                    }
                                    else if (!Sensor_OP_CST_Detect1 && !Sensor_OP_CST_Detect2)
                                    {
                                        if (Carrier_CheckOP_ExistID())
                                            OP_CarrierID = string.Empty;

                                        if (!IsYAxisPos_FWD(PortAxis.Buffer_OP_Y))
                                            m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step630_InMode_Move_OP_Y_FWD;
                                        else
                                            m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step700_OutMode_Await_PIO_TR_REQ;
                                    }
                                }
                                else if(IsZAxisPos_DOWN(PortAxis.Buffer_OP_Z))
                                {
                                    if (Carrier_CheckOP_ExistProduct(true) && Carrier_CheckOP_ExistID())
                                        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_LP_Load_Req;
                                    else if(Carrier_CheckOP_ExistProduct(true) && !Carrier_CheckOP_ExistID())
                                    {
                                        AlarmInsert((short)PortAlarm.OP_No_Cassette_ID_Error, AlarmLevel.Error);
                                    }
                                    else if(Carrier_CheckOP_ExistProduct(false))
                                    {
                                        if (Carrier_CheckOP_ExistID())
                                            OP_CarrierID = string.Empty;

                                        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step620_OutMode_Move_OP_Z_Up;
                                    }
                                }
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step610_OutMode_Move_OP_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_OP_Z, Teaching_Z_Pos.Down_Pos))
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step620_OutMode_Move_OP_Z_Up:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_OP_Z, Teaching_Z_Pos.Up_Pos))
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step630_InMode_Move_OP_Y_FWD:
                            {
                                if (Y_Axis_MotionAndDone(PortAxis.Buffer_OP_Y, Teaching_Y_Pos.FWD_Pos))
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step640_InMode_Move_OP_Y_BWD:
                            {
                                if (Y_Axis_MotionAndDone(PortAxis.Buffer_OP_Y, Teaching_Y_Pos.BWD_Pos))
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                            {
                                if (OUTPUT_DIR_STK_PIO_TR_REQ_Await())
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step710_OutMode_Check_PIO_Busy;

                                //PIOStatus_PortToSTK_Load_Req = true;
                                //if (PIOStatus_STKToPort_TR_REQ)
                                //{
                                //    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step710_OutMode_Check_PIO_Busy;
                                //    Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);
                                //}
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step710_OutMode_Check_PIO_Busy:
                            {
                                if (OUTPUT_DIR_STK_PIO_BUSY_Check())
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step720_OutMode_Check_PIO_Complete;

                                //if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
                                //{
                                //    if (!PIOStatus_STKToPort_Busy)
                                //        AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                //}

                                //if (PIOStatus_STKToPort_Busy)
                                //{
                                //    PIOStatus_PortToSTK_Ready = true;
                                //    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step720_OutMode_Check_PIO_Complete;
                                //    Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);
                                //    m_RMCSTIDRWTimer.Stop();
                                //    m_RMCSTIDRWTimer.Reset();
                                //    m_RMCSTIDRWTimer.Start();
                                //}
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step720_OutMode_Check_PIO_Complete:
                            {
                                if (OUTPUT_DIR_STK_PIO_COMPLETE_Check())
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step730_OutMode_Check_PIO_End;

                                //if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
                                //{
                                //    if (!PIOStatus_STKToPort_Busy || !PIOStatus_STKToPort_Complete)
                                //        AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
                                //}

                                //string RMCarrierID = Carrier_GetRMToPort_RecvMapCarrierID();

                                //if (PIOStatus_STKToPort_Busy && PIOStatus_STKToPort_Complete)
                                //    Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);

                                //if (Sensor_OP_CST_Detect1 &&
                                //    Sensor_OP_CST_Detect2 &&
                                //    PIOStatus_STKToPort_Busy &&
                                //    PIOStatus_STKToPort_Complete) //RMCarrierID != string.Empty (2023-08-17 임시 삭제 촬영위함)
                                //{
                                //    Carrier_ACK_PortToRM_CarrierID(RMCarrierID);

                                //    if (RMCarrierID != string.Empty) //|| m_eControlMode == ControlMode.CIMMode
                                //    {
                                //        m_RMCSTIDRWTimer.Stop();
                                //        m_RMCSTIDRWTimer.Reset();
                                //        PIOStatus_PortToSTK_Load_Req = false;
                                //        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step730_OutMode_Check_PIO_End;
                                //    }
                                //    else if(m_RMCSTIDRWTimer.Elapsed.TotalSeconds > 30.0)
                                //    {
                                //        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"RM -> Port CST ID Read Timeout");
                                //        m_RMCSTIDRWTimer.Stop();
                                //        m_RMCSTIDRWTimer.Reset();
                                //        PIOStatus_PortToSTK_Load_Req = false;
                                //        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step730_OutMode_Check_PIO_End;
                                //    }
                                //}
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step730_OutMode_Check_PIO_End:
                            {
                                if (OUTPUT_DIR_STK_PIO_END_Check())
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe;

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
                                //        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe;
                                //    }
                                //    else if(OP_CarrierID == string.Empty)
                                //    {
                                //        //Timeout의 경우
                                //        OP_CarrierID = "CST_ID_READ_FAIL";
                                //        Carrier_ClearPortToRM_CarrierID();
                                //        PIOStatus_PortToSTK_Ready = false;
                                //        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe;
                                //    }
                                //}
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe:
                            {
                                if (Sensor_OP_CST_Detect1 &&
                                    Sensor_OP_CST_Detect2)
                                {
                                    if (Y_Axis_MotionAndDone(PortAxis.Buffer_OP_Y, Teaching_Y_Pos.BWD_Pos))
                                    {
                                        if(!Sensor_OP_Fork_Detect)
                                            m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step810_OutMode_Move_OP_Z_Down;
                                    }
                                    //&&
                                    //!Sensor_OP_Fork_Detect
                                    //m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step800_InMode_Move_OP_Y_BWD;
                                }
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step800_InMode_Move_OP_Y_BWD:
                            {
                                if (Y_Axis_MotionAndDone(PortAxis.Buffer_OP_Y, Teaching_Y_Pos.BWD_Pos))
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step810_OutMode_Move_OP_Z_Down;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step810_OutMode_Move_OP_Z_Down:
                            {
                                if (Z_Axis_MotionAndDone(PortAxis.Buffer_OP_Z, Teaching_Z_Pos.Down_Pos))
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_LP_Load_Req;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_LP_Load_Req:
                            {
                                if (m_eLP_CV_DIEBANK_AutoStep == LP_CV_DIEBANK_AutoStep.Step700_OutMode_Call_OP_Load_Req)
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step910_OutMode_Move_OP_CV_Rolling;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step910_OutMode_Move_OP_CV_Rolling:
                            {
                                CVFreqBWDRolling(BufferCV.Buffer_OP);

                                if (m_eLP_CV_DIEBANK_AutoStep == LP_CV_DIEBANK_AutoStep.Step710_OutMode_Move_LP_Rolling && Sensor_LP_CV_BWD_Status)
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step920_OutMode_Move_OP_CV_Stop;
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step920_OutMode_Move_OP_CV_Stop:
                            {
                                if (Carrier_CheckOP_ExistProduct(false) &&
                                    Carrier_CheckLP_ExistProduct(true) &&
                                    !IsLPOppositeAngle())
                                {
                                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"[Out Direction] OP CV Stop, Sensor Info: OP CV IN[{Sensor_OP_CV_IN}], OP CV STOP[{Sensor_OP_CV_STOP}], LP CV IN[{Sensor_LP_CV_IN}], LP CV STOP[{Sensor_LP_CV_STOP}], LP Opposite[{IsLPOppositeAngle()}]");
                                    CVStop(BufferCV.Buffer_OP);
                                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step930_OutMode_Send_OP_CST_ID;
                                }
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step930_OutMode_Send_OP_CST_ID:
                            {
                                if (Carrier_CheckOP_ExistProduct(false) &&
                                    Carrier_CheckOP_ExistID())
                                {
                                    string OPCarrierID = OP_CarrierID;
                                    LP_CarrierID = OPCarrierID;
                                    if (OPCarrierID == LP_CarrierID)
                                        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step940_OutMode_Clear_OP_CST_ID;
                                }
                            }
                            break;
                        case OP_CV_DIEBANK_AutoStep.Step940_OutMode_Clear_OP_CST_ID:
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
                                        m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step000_Check_Direction;
                                    else
                                    {
                                        if (m_CycleControlProgressCount + 1 < m_CycleControlSetCount)
                                        {
                                            //Outmode인 경우에는 LP에서 카운트를 증가
                                            //m_CycleControlProgressCount++;
                                            //-----------Direction Change 실행
                                            Port_To_RM_PIO_Init();
                                            m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose;
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

        /// <summary>
        /// OP or LP의 현재 스텝 텍스트
        /// </summary>
        /// <returns></returns>
        private string Auto_CV_Get_LP_DIEBANK_StepStr()
        {
            switch (m_eLP_CV_DIEBANK_AutoStep)
            {
                case LP_CV_DIEBANK_AutoStep.Step000_Check_Direction:
                    return $"Check Direction";

                case LP_CV_DIEBANK_AutoStep.Step100_InMode_Check_LP_CST_Load_And_Pose:
                case LP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_LP_CST_Load_And_Pose:
                    return $"Check LP CST Load and Pose";

                //case LP_CV_DIEBANK_AutoStep.Step211_InMode_Call_AGV_Load_Req:
                //case LP_CV_DIEBANK_AutoStep.Step911_OutMode_Call_AGV_Unload_Req:
                //    return $"Call AGV CST Load Req";
                case LP_CV_DIEBANK_AutoStep.Step200_InMode_Await_Check_Unload_REQ:
                    return $"Await PIO-Unload REQ";
                case LP_CV_DIEBANK_AutoStep.Step800_OutMode_Await_Check_PIO_Load_REQ:
                    return $"Await PIO-Load REQ";
                case LP_CV_DIEBANK_AutoStep.Step210_InMode_Check_PIO_Ready:
                case LP_CV_DIEBANK_AutoStep.Step810_OutMode_Check_PIO_Ready:
                    return $"Check PIO-Ready";
                case LP_CV_DIEBANK_AutoStep.Step220_InMode_Check_PIO_Complete_Wait:
                case LP_CV_DIEBANK_AutoStep.Step820_OutMode_Check_PIO_Complete_Wait:
                    return $"Check PIO-Complete Wait";
                case LP_CV_DIEBANK_AutoStep.Step230_InMode_Check_PIO_End:
                case LP_CV_DIEBANK_AutoStep.Step830_OutMode_Check_PIO_End:
                    return $"Check PIO-End Off";

                case LP_CV_DIEBANK_AutoStep.Step240_InMode_Check_CST_Load_And_Safe:
                    return $"Check LP CST Load and Safe";
                case LP_CV_DIEBANK_AutoStep.Step840_OutMode_Check_LP_CST_Unload_And_Safe:
                    return $"Check LP CST Unload and Safe";

                case LP_CV_DIEBANK_AutoStep.Step300_InMode_Await_MGV_CST_Load:
                    return $"Await MGV CST Load";
                case LP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_MGV_CST_Unload:
                    return $"Await MGV CST Unload";

                case LP_CV_DIEBANK_AutoStep.Step400_InMode_Await_OP_Load_Req:
                    return $"Await Load Req(OP)";

                case LP_CV_DIEBANK_AutoStep.Step410_InMode_Move_LP_CV_Rolling:
                case LP_CV_DIEBANK_AutoStep.Step610_OutMode_Move_LP_CV_Rolling:
                case LP_CV_DIEBANK_AutoStep.Step710_OutMode_Move_LP_Rolling:
                    return $"LP CV Rolling";
                case LP_CV_DIEBANK_AutoStep.Step420_InMode_Move_LP_CV_Stop:
                case LP_CV_DIEBANK_AutoStep.Step620_OutMode_Move_LP_CV_Stop:
                case LP_CV_DIEBANK_AutoStep.Step720_OutMode_Move_LP_CV_Stop:
                    return $"LP CV Stop";

                case LP_CV_DIEBANK_AutoStep.Step700_OutMode_Call_OP_Load_Req:
                    return $"Call Load Req(OP)";
                case LP_CV_DIEBANK_AutoStep.Step730_OutMode_Check_CST_ID:
                    return $"Check LP CST ID";
                default:
                    return "Not def step str";
            }
        }
        private string Auto_CV_Get_OP_DIEBANK_StepStr()
        {
            switch (m_eOP_CV_DIEBANK_AutoStep)
            {
                case OP_CV_DIEBANK_AutoStep.Step000_Check_Direction:
                    return $"Check Direction";

                case OP_CV_DIEBANK_AutoStep.Step100_InMode_Check_OP_CST_Load_And_Pose:
                case OP_CV_DIEBANK_AutoStep.Step600_OutMode_Check_OP_CST_Load_And_Pose:
                    return $"Check OP CST Load and Pose";


                case OP_CV_DIEBANK_AutoStep.Step110_InMode_Move_OP_Z_Up:
                case OP_CV_DIEBANK_AutoStep.Step620_OutMode_Move_OP_Z_Up:
                    return $"Move Z Up Pos (Init)";
                case OP_CV_DIEBANK_AutoStep.Step120_InMode_Move_OP_Y_FWD:
                case OP_CV_DIEBANK_AutoStep.Step630_InMode_Move_OP_Y_FWD:
                    return $"Move Y FWD Pos (Init)";
                case OP_CV_DIEBANK_AutoStep.Step130_InMode_Move_OP_Y_BWD:
                case OP_CV_DIEBANK_AutoStep.Step640_InMode_Move_OP_Y_BWD:
                    return $"Move Y BWD Pos (Init)";
                case OP_CV_DIEBANK_AutoStep.Step140_InMode_Move_OP_Z_Down:
                case OP_CV_DIEBANK_AutoStep.Step610_OutMode_Move_OP_Z_Down:
                    return $"Move Z Down Pos (Init)";
                case OP_CV_DIEBANK_AutoStep.Step150_InMode_Move_OP_Rolling:
                case OP_CV_DIEBANK_AutoStep.Step210_InMode_Move_OP_Rolling:
                case OP_CV_DIEBANK_AutoStep.Step910_OutMode_Move_OP_CV_Rolling:
                    return $"OP CV Rolling";
                case OP_CV_DIEBANK_AutoStep.Step160_InMode_Move_OP_CV_Stop:
                case OP_CV_DIEBANK_AutoStep.Step220_InMode_Move_OP_CV_Stop:
                case OP_CV_DIEBANK_AutoStep.Step920_OutMode_Move_OP_CV_Stop:
                    return $"OP CV Stop";

                case OP_CV_DIEBANK_AutoStep.Step200_InMode_Call_LP_Load_Req:
                    return $"Call Load Req (LP)";

                case OP_CV_DIEBANK_AutoStep.Step300_InMode_Ready_CST_ID_Read:
                    return $"Ready CST ID Read";
                case OP_CV_DIEBANK_AutoStep.Step310_InMode_Start_CST_ID_Read:
                    return $"Start CST ID Read";

                case OP_CV_DIEBANK_AutoStep.Step320_InMode_Move_OP_Z_Up:
                    return $"Move Z Up Pos";
                case OP_CV_DIEBANK_AutoStep.Step330_InMode_CST_InPlaceCheck:
                    return $"Inplace Check";
                case OP_CV_DIEBANK_AutoStep.Step340_InMode_Move_OP_Y_FWD:
                    return $"Move Y FWD Pos";

                case OP_CV_DIEBANK_AutoStep.Step400_InMode_Await_PIO_TR_REQ:
                case OP_CV_DIEBANK_AutoStep.Step700_OutMode_Await_PIO_TR_REQ:
                    return $"Await PIO-TR_REQ";
                case OP_CV_DIEBANK_AutoStep.Step410_InMode_Check_PIO_Busy:
                case OP_CV_DIEBANK_AutoStep.Step710_OutMode_Check_PIO_Busy:
                    return $"Check PIO-Busy";
                case OP_CV_DIEBANK_AutoStep.Step420_InMode_Check_PIO_Complete:
                case OP_CV_DIEBANK_AutoStep.Step720_OutMode_Check_PIO_Complete:
                    return $"Check PIO-Complete";
                case OP_CV_DIEBANK_AutoStep.Step430_InMode_Check_PIO_End:
                case OP_CV_DIEBANK_AutoStep.Step730_OutMode_Check_PIO_End:
                    return $"Check PIO-End off";
                case OP_CV_DIEBANK_AutoStep.Step440_InMode_Check_OP_CST_Unload_And_Safe:
                case OP_CV_DIEBANK_AutoStep.Step740_OutMode_Check_OP_CST_Load_And_Safe:
                    return $"Check OP CST Unload and Safe";

                case OP_CV_DIEBANK_AutoStep.Step800_InMode_Move_OP_Y_BWD:
                    return $"Move Y BWD Pos";
                case OP_CV_DIEBANK_AutoStep.Step810_OutMode_Move_OP_Z_Down:
                    return $"Move Z Down Pos";
                case OP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_LP_Load_Req:
                    return $"Await Load Req (LP)";
                case OP_CV_DIEBANK_AutoStep.Step930_OutMode_Send_OP_CST_ID:
                    return $"Send OP CST ID (OP -> LP)";
                case OP_CV_DIEBANK_AutoStep.Step940_OutMode_Clear_OP_CST_ID:
                    return $"Clear OP CST ID";

                default:
                    return "Not def step str";
            }
        }

        /// <summary>
        /// OP or LP의 현재 스텝 번호
        /// </summary>
        /// <returns></returns>
        private int Auto_CV_Get_LP_DIEBANK_StepNum()
        {
            return (int)m_eLP_CV_DIEBANK_AutoStep;
        }
        private int Auto_CV_Get_OP_DIEBANK_StepNum()
        {
            return (int)m_eOP_CV_DIEBANK_AutoStep;
        }
    }
}
