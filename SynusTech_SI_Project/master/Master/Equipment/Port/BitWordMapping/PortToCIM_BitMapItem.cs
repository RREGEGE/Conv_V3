using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Equipment.Port
{
    /// <summary>
    /// [Port -> CIM] Bit Memory Map에 사용되는 변수 기재
    /// Sensor : 센서 상태
    /// Status : 모션, 공정 관련 상태
    /// PIOStatus : PIO 상태
    /// </summary>
    public partial class Port
    {
        public class ManualPIOKeeper
        {
            public bool bInit = false;
            public bool[] bFlag;

            public ManualPIOKeeper(int bFlagSize)
            {
                bFlag = new bool[bFlagSize];
            }
        }


        bool m_bHWEStopSwitchStatus = false;
        bool m_bLightCurtainStatus = false;
        bool m_bBuzzerStatus = false;

        public bool Sensor_HWEStop
        {
            get { return m_bHWEStopSwitchStatus; }
            set { 
                m_bHWEStopSwitchStatus = value;
                if (m_bHWEStopSwitchStatus)
                    mHW_EStop.PushEStop();
                else
                    mHW_EStop.ReleaseEStop();
            }
        }
        public bool Sensor_LightCurtain
        {
            get { return m_bLightCurtainStatus; }
            set { m_bLightCurtainStatus = value; }
        }
        public bool Sensor_Buzzer
        {
            get { return m_bBuzzerStatus; }
            set { m_bBuzzerStatus = value; }
        }

        /// <summary>
        /// General Status
        /// </summary>
        bool m_bEStop = false;
        bool m_bAutoRunStatus = false;
        bool m_bRunEnable = false;
        bool m_bStopEnable = false;
        bool m_bPowerOnStatus = false;
        bool m_bPowerOnEnable = false;
        bool m_bPowerOffEnable = false;
        bool m_bMGVStatus = false;
        bool m_bAGVorOHTStatus = false;
        bool m_bTypeChangeEnable = false;
        bool m_bInputStatus = false;
        bool m_bOutputStatus = false;
        bool m_bModeChangeEnable = false;
        bool m_bCIMModeStatus = false;
        bool m_bBuffer1_CST_Status = false;
        bool m_bBuffer2_CST_Status = false;
        bool m_bMaintDoorOpen = false;
        bool m_bMaintDoorOpen2 = false;
        public bool Status_Maint_Door_Open_Status
        {
            get { return m_bMaintDoorOpen; }
            set
            {
                m_bMaintDoorOpen = value;
            }
        }
        public bool Status_Maint_Door_Open_Status2
        {
            get { return m_bMaintDoorOpen2; }
            set
            {
                m_bMaintDoorOpen2 = value;
            }
        }
        public bool Status_EStop
        {
            get { return m_bEStop; }
            set { 
                if(m_bEStop != value && value == true)
                {
                    bool bMasterEStop   = Master.mPort_EStop.GetEStopState() == Interface.Safty.EStopState.EStop;
                    bool bWMXEStop      = Master.IsWMXEStopState();
                    bool bPortSWEStop   = mSW_EStop.GetEStopState() == Interface.Safty.EStopState.EStop;
                    bool bPortHWEStop   = mHW_EStop.GetEStopState() == Interface.Safty.EStopState.EStop;
                    bool bDTPEStop      = Master.mPortHandyTouch_EStop.GetEStopState() == Interface.Safty.EStopState.EStop;


                    //Estop 상황의 경우 5가지를 조합해서 보므로 어떤 센서가 원인으로 EStop 됐는지 로그 작성
                    string Info = $"Port is E-stop State, " +
                        $"Master EStop:{bMasterEStop}, " +
                        $"WMX EStop:{bWMXEStop}, " +
                        $"Port SW_EStop:{bPortSWEStop}, " +
                        $"Port HW_EStop:{bPortHWEStop}, " +
                        $"Port DTP_EStop:{bDTPEStop}";

                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortEStopInfo, Info);

                }
                m_bEStop = value; 
            }
        }
        public bool Status_AutoRun
        {
            get { return m_bAutoRunStatus; }
            set { 
                m_bAutoRunStatus = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.AutoRunStatus, m_bAutoRunStatus);
            }
        }
        public bool Status_RunEnable
        {
            get { return m_bRunEnable; }
            set { 
                m_bRunEnable = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Run_Enable, m_bRunEnable);
            }
        }
        public bool Status_StopEnable
        {
            get { return m_bStopEnable; }
            set
            {
                m_bStopEnable = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Stop_Enable, m_bStopEnable);
            }
        }
        public bool Status_PowerOn
        {
            get { return m_bPowerOnStatus; }
            set
            {
                m_bPowerOnStatus = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.PowerOn_Status, m_bPowerOnStatus);
            }
        }
        public bool Status_PowerOnEnable
        {
            get { return m_bPowerOnEnable; }
            set
            {
                m_bPowerOnEnable = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.PowerOn_Enable, m_bPowerOnEnable);
            }
        }
        public bool Status_PowerOffEnable
        {
            get { return m_bPowerOffEnable; }
            set
            {
                m_bPowerOffEnable = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.PowerOff_Enable, m_bPowerOffEnable);
            }
        }
        public bool Status_MGV
        {
            get { return m_bMGVStatus; }
            set
            {
                m_bMGVStatus = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.MGV_Status, m_bMGVStatus);
            }
        }
        public bool Status_AGVorOHT
        {
            get { return m_bAGVorOHTStatus; }
            set
            {
                m_bAGVorOHTStatus = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.AGV_Status, m_bAGVorOHTStatus);
            }
        }
        public bool Status_TypeChangeEnable
        {
            get { return m_bTypeChangeEnable; }
            set
            {
                m_bTypeChangeEnable = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Type_Change_Enable, m_bTypeChangeEnable);
            }
        }
        public bool Status_Input
        {
            get { return m_bInputStatus; }
            set
            {
                m_bInputStatus = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Input_Status, m_bInputStatus);
            }
        }
        public bool Status_Output
        {
            get { return m_bOutputStatus; }
            set
            {
                m_bOutputStatus = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Output_Status, m_bOutputStatus);
            }
        }
        public bool Status_DirectionChangeEnable
        {
            get { return m_bModeChangeEnable; }
            set
            {
                m_bModeChangeEnable = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Mode_Change_Enable, m_bModeChangeEnable);
            }
        }

        public bool Status_CIMMode
        {
            get { return m_bCIMModeStatus; }
            set
            {
                m_bCIMModeStatus = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.CIM_Mode_Status, m_bCIMModeStatus);
            }
        }

        public bool Status_Buffer1_CST_Status
        {
            get { return m_bBuffer1_CST_Status; }
            set
            {
                m_bBuffer1_CST_Status = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Buffer1_CST_ON, m_bBuffer1_CST_Status);
            }
        }
        public bool Status_Buffer2_CST_Status
        {
            get { return m_bBuffer2_CST_Status; }
            set
            {
                m_bBuffer2_CST_Status = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Buffer2_CST_ON, m_bBuffer2_CST_Status);
            }
        }

        /// <summary>
        /// Port <-> RM
        /// </summary>
        bool m_bPort_To_STK_Load_REQ = false;
        bool m_bPort_To_STK_Unload_REQ = false;
        bool m_bPort_To_STK_Ready = false;
        bool m_bPort_To_STK_PortError = false;
        bool m_bSTK_To_Port_TR_REQ = false;
        bool m_bSTK_To_Port_Busy = false;
        bool m_bSTK_To_Port_Complete = false;
        bool m_bSTK_To_Port_STKError = false;
        ManualPIOKeeper m_Port_To_STK_PIOKeeper = new ManualPIOKeeper(4);
        ManualPIOKeeper m_STK_To_EQ_PIOKeeper = new ManualPIOKeeper(4);

        public bool PIOStatus_PortToSTK_Load_Req
        {
            get { return m_bPort_To_STK_Load_REQ; }
            set { 
                m_bPort_To_STK_Load_REQ = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.L_REQ, m_bPort_To_STK_Load_REQ);
            }
        }
        public bool PIOStatus_PortToSTK_Unload_Req
        {
            get { return m_bPort_To_STK_Unload_REQ; }
            set { 
                m_bPort_To_STK_Unload_REQ = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.UL_REQ, m_bPort_To_STK_Unload_REQ);
            }
        }
        public bool PIOStatus_PortToSTK_Ready
        {
            get { return m_bPort_To_STK_Ready; }
            set { 
                m_bPort_To_STK_Ready = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Ready, m_bPort_To_STK_Ready);
            }
        }
        public bool PIOStatus_PortToSTK_Error
        {
            get { return m_bPort_To_STK_PortError; }
            set { 
                m_bPort_To_STK_PortError = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Port_Error, m_bPort_To_STK_PortError);
            }
        }
        public bool PIOStatus_STKToPort_TR_REQ
        {
            get { return m_bSTK_To_Port_TR_REQ; }
            set { 
                m_bSTK_To_Port_TR_REQ = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.TR_REQ, m_bSTK_To_Port_TR_REQ);
            }
        }
        public bool PIOStatus_STKToPort_Busy
        {
            get { return m_bSTK_To_Port_Busy; }
            set { 
                m_bSTK_To_Port_Busy = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Busy, m_bSTK_To_Port_Busy);
            }
        }
        public bool PIOStatus_STKToPort_Complete
        {
            get { return m_bSTK_To_Port_Complete; }
            set { 
                m_bSTK_To_Port_Complete = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Complete, m_bSTK_To_Port_Complete);
            }
        }
        public bool PIOStatus_STKToPort_STKError
        {
            get { return m_bSTK_To_Port_STKError; }
            set { 
                m_bSTK_To_Port_STKError = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.STK_Error, m_bSTK_To_Port_STKError);
            }
        }
        public void PIOStatus_ManualSaveSTKToEQPIO()
        {
            if (!IsAutoControlRun() &&
                !IsAutoManualCycleRun() &&
                m_eControlMode != ControlMode.CIMMode &&
                m_STK_To_EQ_PIOKeeper.bInit == false)
            {
                m_STK_To_EQ_PIOKeeper.bInit = true;
                m_STK_To_EQ_PIOKeeper.bFlag[0] = PIOStatus_STKToPort_TR_REQ;
                m_STK_To_EQ_PIOKeeper.bFlag[1] = PIOStatus_STKToPort_Busy;
                m_STK_To_EQ_PIOKeeper.bFlag[2] = PIOStatus_STKToPort_Complete;
                m_STK_To_EQ_PIOKeeper.bFlag[3] = PIOStatus_STKToPort_STKError;
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PIOCurrentFlagKeeping,
                    $"[STK->EQ]: " +
                    $"TR_REQ({m_STK_To_EQ_PIOKeeper.bFlag[0]}) / " +
                    $"BUSY({m_STK_To_EQ_PIOKeeper.bFlag[1]}) / " +
                    $"Complete({m_STK_To_EQ_PIOKeeper.bFlag[2]}) / " +
                    $"Error({m_STK_To_EQ_PIOKeeper.bFlag[3]})");
            }
        }
        public void PIOStatus_ManualReleaseSTKToEQPIO()
        {
            if (m_STK_To_EQ_PIOKeeper.bInit)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PIOKeepingFlagReload,
                    $"[STK->EQ]: " +
                    $"TR_REQ({m_STK_To_EQ_PIOKeeper.bFlag[0]}) / " +
                    $"BUSY({m_STK_To_EQ_PIOKeeper.bFlag[1]}) / " +
                    $"Complete({m_STK_To_EQ_PIOKeeper.bFlag[2]}) / " +
                    $"Error({m_STK_To_EQ_PIOKeeper.bFlag[3]})");

                if (PIOStatus_STKToPort_TR_REQ != m_STK_To_EQ_PIOKeeper.bFlag[0])
                    Interlock_Manual_PIO_STKToEQ_TR_REQ(m_STK_To_EQ_PIOKeeper.bFlag[0], InterlockFrom.ApplicationLoop);
                if (PIOStatus_STKToPort_Busy != m_STK_To_EQ_PIOKeeper.bFlag[1])
                    Interlock_Manual_PIO_STKToEQ_BUSY(m_STK_To_EQ_PIOKeeper.bFlag[1], InterlockFrom.ApplicationLoop);
                if (PIOStatus_STKToPort_Complete != m_STK_To_EQ_PIOKeeper.bFlag[2])
                    Interlock_Manual_PIO_STKToEQ_Complete(m_STK_To_EQ_PIOKeeper.bFlag[2], InterlockFrom.ApplicationLoop);
                if (PIOStatus_STKToPort_STKError != m_STK_To_EQ_PIOKeeper.bFlag[3])
                    Interlock_Manual_PIO_STKToEQ_Error(m_STK_To_EQ_PIOKeeper.bFlag[3], InterlockFrom.ApplicationLoop);

                m_STK_To_EQ_PIOKeeper.bInit = false;
            }
        }
        public void PIOStatus_ManualSavePortToRMPIO()
        {
            if (!IsAutoControlRun() &&
                !IsAutoManualCycleRun() &&
                m_eControlMode != ControlMode.CIMMode &&
                m_Port_To_STK_PIOKeeper.bInit == false)
            {
                m_Port_To_STK_PIOKeeper.bInit = true;
                m_Port_To_STK_PIOKeeper.bFlag[0] = PIOStatus_PortToSTK_Load_Req;
                m_Port_To_STK_PIOKeeper.bFlag[1] = PIOStatus_PortToSTK_Unload_Req;
                m_Port_To_STK_PIOKeeper.bFlag[2] = PIOStatus_PortToSTK_Ready;
                m_Port_To_STK_PIOKeeper.bFlag[3] = PIOStatus_PortToSTK_Error;
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PIOCurrentFlagKeeping,
                    $"[Port->RM]: " +
                    $"Load_REQ({m_Port_To_STK_PIOKeeper.bFlag[0]}) / " +
                    $"Unload_REQ({m_Port_To_STK_PIOKeeper.bFlag[1]}) / " +
                    $"Ready({m_Port_To_STK_PIOKeeper.bFlag[2]}) / " +
                    $"Error({m_Port_To_STK_PIOKeeper.bFlag[3]})");
            }
        }
        public void PIOStatus_ManualReleasePortToRMPIO()
        {
            if (m_Port_To_STK_PIOKeeper.bInit)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PIOKeepingFlagReload,
                    $"[Port->RM]: " +
                    $"Load_REQ({m_Port_To_STK_PIOKeeper.bFlag[0]}) / " +
                    $"Unload_REQ({m_Port_To_STK_PIOKeeper.bFlag[1]}) / " +
                    $"Ready({m_Port_To_STK_PIOKeeper.bFlag[2]}) / " +
                    $"Error({m_Port_To_STK_PIOKeeper.bFlag[3]})");

                if (PIOStatus_PortToSTK_Load_Req != m_Port_To_STK_PIOKeeper.bFlag[0])
                    Interlock_Manual_PIO_PortToRM_LoadREQ(m_Port_To_STK_PIOKeeper.bFlag[0], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToSTK_Unload_Req != m_Port_To_STK_PIOKeeper.bFlag[1])
                    Interlock_Manual_PIO_PortToRM_UnloadREQ(m_Port_To_STK_PIOKeeper.bFlag[1], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToSTK_Ready != m_Port_To_STK_PIOKeeper.bFlag[2])
                    Interlock_Manual_PIO_PortToRM_Ready(m_Port_To_STK_PIOKeeper.bFlag[2], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToSTK_Error != m_Port_To_STK_PIOKeeper.bFlag[3])
                    Interlock_Manual_PIO_PortToRM_Error(m_Port_To_STK_PIOKeeper.bFlag[3], InterlockFrom.ApplicationLoop);

                m_Port_To_STK_PIOKeeper.bInit = false;
            }
        }

        /// <summary>
        /// AGV <-> Port 
        /// </summary>
        bool m_bAGV_To_Port_Valid = false;
        bool m_bAGV_To_Port_CS0 = false;
        bool m_bAGV_To_Port_TR_REQ = false;
        bool m_bAGV_To_Port_Busy = false;
        bool m_bAGV_To_Port_Complete = false;
        bool m_bPort_To_AGV_Load_REQ = false;
        bool m_bPort_To_AGV_Unload_REQ = false;
        bool m_bPort_To_AGV_ES = false;
        bool m_bPort_To_AGV_Ready = false;
        ManualPIOKeeper m_Port_To_AGV_PIOKeeper = new ManualPIOKeeper(4);

        public bool PIOStatus_AGVToPort_Valid
        {
            get { return m_bAGV_To_Port_Valid; }
            set { 
                m_bAGV_To_Port_Valid = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.AGV_To_CV_Valid, m_bAGV_To_Port_Valid);
            }
        }
        public bool PIOStatus_AGVToPort_CS0
        {
            get { return m_bAGV_To_Port_CS0; }
            set { 
                m_bAGV_To_Port_CS0 = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.AGV_To_CV_CS0, m_bAGV_To_Port_CS0);
            }
        }
        public bool PIOStatus_AGVToPort_TR_Req
        {
            get { return m_bAGV_To_Port_TR_REQ; }
            set { 
                m_bAGV_To_Port_TR_REQ = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.AGV_To_CV_TR_REQ, m_bAGV_To_Port_TR_REQ);
            }
        }
        public bool PIOStatus_AGVToPort_Busy
        {
            get { return m_bAGV_To_Port_Busy; }
            set { 
                m_bAGV_To_Port_Busy = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.AGV_To_CV_Busy, m_bAGV_To_Port_Busy);
            }
        }
        public bool PIOStatus_AGVToPort_Complete
        {
            get { return m_bAGV_To_Port_Complete; }
            set { 
                m_bAGV_To_Port_Complete = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.AGV_To_CV_Complete, m_bAGV_To_Port_Complete);
            }
        }

        public bool PIOStatus_PortToAGV_Load_Req
        {
            get { return m_bPort_To_AGV_Load_REQ; }
            set { 
                m_bPort_To_AGV_Load_REQ = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.CV_To_AGV_Load_REQ, m_bPort_To_AGV_Load_REQ);
            }
        }
        public bool PIOStatus_PortToAGV_Unload_Req
        {
            get { return m_bPort_To_AGV_Unload_REQ; }
            set { 
                m_bPort_To_AGV_Unload_REQ = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.CV_To_AGV_Unload_REQ, m_bPort_To_AGV_Unload_REQ);
            }
        }
        public bool PIOStatus_PortToAGV_Ready
        {
            get { return m_bPort_To_AGV_Ready; }
            set { 
                m_bPort_To_AGV_Ready = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.CV_To_AGV_Ready, m_bPort_To_AGV_Ready);
            }
        }
        public bool PIOStatus_PortToAGV_ES
        {
            get { return m_bPort_To_AGV_ES; }
            set { 
                m_bPort_To_AGV_ES = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.CV_To_AGV_ES, m_bPort_To_AGV_ES);
            }
        }
        public void PIOStatus_ManualSavePortToAGVPIO()
        {
            if (!IsAutoControlRun() &&
                !IsAutoManualCycleRun() &&
                m_eControlMode != ControlMode.CIMMode &&
                m_Port_To_AGV_PIOKeeper.bInit == false)
            {
                m_Port_To_AGV_PIOKeeper.bInit = true;
                m_Port_To_AGV_PIOKeeper.bFlag[0] = PIOStatus_PortToAGV_Load_Req;
                m_Port_To_AGV_PIOKeeper.bFlag[1] = PIOStatus_PortToAGV_Unload_Req;
                m_Port_To_AGV_PIOKeeper.bFlag[2] = PIOStatus_PortToAGV_Ready;
                m_Port_To_AGV_PIOKeeper.bFlag[3] = PIOStatus_PortToAGV_ES;

                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PIOCurrentFlagKeeping,
                    $"[Port->AGV]: " +
                    $"Load_REQ({m_Port_To_AGV_PIOKeeper.bFlag[0]}) / " +
                    $"Unload_REQ({m_Port_To_AGV_PIOKeeper.bFlag[1]}) / " +
                    $"Ready({m_Port_To_AGV_PIOKeeper.bFlag[2]}) / " +
                    $"ES({m_Port_To_AGV_PIOKeeper.bFlag[3]})");
            }
        }
        public void PIOStatus_ManualReleasePortToAGVPIO()
        {
            if (m_Port_To_AGV_PIOKeeper.bInit)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PIOKeepingFlagReload,
                    $"[Port->AGV]: " +
                    $"Load_REQ({m_Port_To_AGV_PIOKeeper.bFlag[0]}) / " +
                    $"Unload_REQ({m_Port_To_AGV_PIOKeeper.bFlag[1]}) / " +
                    $"Ready({m_Port_To_AGV_PIOKeeper.bFlag[2]}) / " +
                    $"ES({m_Port_To_AGV_PIOKeeper.bFlag[3]})");

                if (PIOStatus_PortToAGV_Load_Req != m_Port_To_AGV_PIOKeeper.bFlag[0])
                    Interlock_Manual_PIO_PortToAGV_LoadREQ(m_Port_To_AGV_PIOKeeper.bFlag[0], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToAGV_Unload_Req != m_Port_To_AGV_PIOKeeper.bFlag[1])
                    Interlock_Manual_PIO_PortToAGV_UnloadREQ(m_Port_To_AGV_PIOKeeper.bFlag[1], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToAGV_Ready != m_Port_To_AGV_PIOKeeper.bFlag[2])
                    Interlock_Manual_PIO_PortToAGV_Ready(m_Port_To_AGV_PIOKeeper.bFlag[2], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToAGV_ES != m_Port_To_AGV_PIOKeeper.bFlag[3])
                    Interlock_Manual_PIO_PortToAGV_ES(m_Port_To_AGV_PIOKeeper.bFlag[3], InterlockFrom.ApplicationLoop);

                m_Port_To_AGV_PIOKeeper.bInit = false;
            }
        }

        /// <summary>
        /// Port <-> OHT
        /// </summary>
        bool m_bOHT_To_Port_Valid = false;
        bool m_bOHT_To_Port_CS0 = false;
        bool m_bOHT_To_Port_TR_REQ = false;
        bool m_bOHT_To_Port_Busy = false;
        bool m_bOHT_To_Port_Complete = false;
        bool m_bPort_To_OHT_Load_REQ = false;
        bool m_bPort_To_OHT_Unoad_REQ = false;
        bool m_bPort_To_OHT_HO_AVBL = false;
        bool m_bPort_To_OHT_ES = false;
        bool m_bPort_To_OHT_Ready = false;
        bool m_bOHT_Door_Open = false;
        bool m_bOHT_Door_Close = false;
        bool m_bOHT_Key_Auto_Status = false;
        ManualPIOKeeper m_Port_To_OHT_PIOKeeper = new ManualPIOKeeper(5);

        public bool PIOStatus_OHTToPort_Valid
        {
            get { return m_bOHT_To_Port_Valid; }
            set { 
                m_bOHT_To_Port_Valid = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.OHT_To_CV_Valid, m_bOHT_To_Port_Valid);
            }
        }
        public bool PIOStatus_OHTToPort_CS0
        {
            get { return m_bOHT_To_Port_CS0; }
            set { 
                m_bOHT_To_Port_CS0 = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.OHT_To_CV_CS0, m_bOHT_To_Port_CS0);
            }
        }
        public bool PIOStatus_OHTToPort_TR_Req
        {
            get { return m_bOHT_To_Port_TR_REQ; }
            set { 
                m_bOHT_To_Port_TR_REQ = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.OHT_To_CV_TR_REQ, m_bOHT_To_Port_TR_REQ);
            }
        }
        public bool PIOStatus_OHTToPort_Busy
        {
            get { return m_bOHT_To_Port_Busy; }
            set { 
                m_bOHT_To_Port_Busy = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.OHT_To_CV_Busy, m_bOHT_To_Port_Busy);
            }
        }
        public bool PIOStatus_OHTToPort_Complete
        {
            get { return m_bOHT_To_Port_Complete; }
            set { 
                m_bOHT_To_Port_Complete = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.OHT_To_CV_Complete, m_bOHT_To_Port_Complete);
            }
        }

        public bool PIOStatus_PortToOHT_Load_Req
        {
            get { return m_bPort_To_OHT_Load_REQ; }
            set { 
                m_bPort_To_OHT_Load_REQ = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.CV_To_OHT_Load_REQ, m_bPort_To_OHT_Load_REQ);
            }
        }
        public bool PIOStatus_PortToOHT_Unload_Req
        {
            get { return m_bPort_To_OHT_Unoad_REQ; }
            set { 
                m_bPort_To_OHT_Unoad_REQ = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.CV_To_OHT_Unload_REQ, m_bPort_To_OHT_Unoad_REQ);
            }
        }
        public bool PIOStatus_PortToOHT_Ready
        {
            get { return m_bPort_To_OHT_Ready; }
            set { 
                m_bPort_To_OHT_Ready = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.CV_To_OHT_Ready, m_bPort_To_OHT_Ready);
            }
        }
        public bool PIOStatus_PortToOHT_HO_AVBL
        {
            get { return m_bPort_To_OHT_HO_AVBL; }
            set {
                m_bPort_To_OHT_HO_AVBL = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.CV_To_OHT_HO_AVBL, m_bPort_To_OHT_HO_AVBL);
            }
        }
        public bool PIOStatus_PortToOHT_ES
        {
            get { return m_bPort_To_OHT_ES; }
            set { 
                m_bPort_To_OHT_ES = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.CV_To_OHT_ES, m_bPort_To_OHT_ES);
            }
        }

        public bool CMD_OHT_Door_Open
        {
            get { return m_bOHT_Door_Open; }
            set { m_bOHT_Door_Open = value; }
        }
        public bool Status_OHT_Door_Close
        {
            get { return m_bOHT_Door_Close; }
            set {
                m_bOHT_Door_Close = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.OHT_Door_Close, m_bOHT_Door_Close);
            }
        }

        public bool Status_OHT_Key_Open_Status
        {
            get { return m_bOHT_Key_Auto_Status; }
            set
            {
                m_bOHT_Key_Auto_Status = value;
                CMD_OHT_Door_Open = value;
                Set_Port_2_CIM_Bit_Data(SendBitMapIndex.OHT_Port_Key_Auto_Status, m_bOHT_Key_Auto_Status);
            }
        }

        public void PIOStatus_ManualSavePortToOHTPIO()
        {
            if (!IsAutoControlRun() &&
                !IsAutoManualCycleRun() &&
                m_eControlMode != ControlMode.CIMMode &&
                m_Port_To_OHT_PIOKeeper.bInit == false)
            {
                m_Port_To_OHT_PIOKeeper.bInit = true;
                m_Port_To_OHT_PIOKeeper.bFlag[0] = PIOStatus_PortToOHT_Load_Req;
                m_Port_To_OHT_PIOKeeper.bFlag[1] = PIOStatus_PortToOHT_Unload_Req;
                m_Port_To_OHT_PIOKeeper.bFlag[2] = PIOStatus_PortToOHT_Ready;
                m_Port_To_OHT_PIOKeeper.bFlag[3] = PIOStatus_PortToOHT_ES;
                m_Port_To_OHT_PIOKeeper.bFlag[4] = PIOStatus_PortToOHT_HO_AVBL;

                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PIOCurrentFlagKeeping,
                    $"[Port->OHT]: " +
                    $"Load_REQ({m_Port_To_OHT_PIOKeeper.bFlag[0]}) / " +
                    $"Unload_REQ({m_Port_To_OHT_PIOKeeper.bFlag[1]}) / " +
                    $"Ready({m_Port_To_OHT_PIOKeeper.bFlag[2]}) / " +
                    $"ES({m_Port_To_OHT_PIOKeeper.bFlag[3]}) /" +
                    $"HO_AVBL({m_Port_To_OHT_PIOKeeper.bFlag[4]}) /");
            }
        }
        public void PIOStatus_ManualReleasePortToOHTPIO()
        {
            if (m_Port_To_OHT_PIOKeeper.bInit)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PIOKeepingFlagReload,
                    $"[Port->OHT]: " +
                    $"Load_REQ({m_Port_To_OHT_PIOKeeper.bFlag[0]}) / " +
                    $"Unload_REQ({m_Port_To_OHT_PIOKeeper.bFlag[1]}) / " +
                    $"Ready({m_Port_To_OHT_PIOKeeper.bFlag[2]}) / " +
                    $"ES({m_Port_To_OHT_PIOKeeper.bFlag[3]}) / " +
                    $"HO_AVBL({m_Port_To_OHT_PIOKeeper.bFlag[4]})");


                if (PIOStatus_PortToOHT_Load_Req != m_Port_To_OHT_PIOKeeper.bFlag[0])
                    Interlock_Manual_PIO_PortToOHT_LoadREQ(m_Port_To_OHT_PIOKeeper.bFlag[0], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToOHT_Unload_Req != m_Port_To_OHT_PIOKeeper.bFlag[1])
                    Interlock_Manual_PIO_PortToOHT_UnloadREQ(m_Port_To_OHT_PIOKeeper.bFlag[1], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToOHT_Ready != m_Port_To_OHT_PIOKeeper.bFlag[2])
                    Interlock_Manual_PIO_PortToOHT_Ready(m_Port_To_OHT_PIOKeeper.bFlag[2], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToOHT_ES != m_Port_To_OHT_PIOKeeper.bFlag[3])
                    Interlock_Manual_PIO_PortToOHT_ES(m_Port_To_OHT_PIOKeeper.bFlag[3], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToOHT_HO_AVBL != m_Port_To_OHT_PIOKeeper.bFlag[4])
                    Interlock_Manual_PIO_PortToOHT_HO_AVBL(m_Port_To_OHT_PIOKeeper.bFlag[4], InterlockFrom.ApplicationLoop);

                m_Port_To_OHT_PIOKeeper.bInit = false;
            }
        }

        /// <summary>
        /// EQ <-> RM
        /// </summary>
        bool m_bEQ_LoadReq = false;
        bool m_bEQ_UnloadReq = false;
        bool m_bEQ_Ready = false;

        private bool PIOStatus_EQToRM_Load_Req
        {
            get { return m_bEQ_LoadReq; }
            set { m_bEQ_LoadReq = value; }
        }
        private bool PIOStatus_EQToRM_Unload_Req
        {
            get { return m_bEQ_UnloadReq; }
            set { m_bEQ_UnloadReq = value; }
        }
        private bool PIOStatus_EQToRM_Ready
        {
            get { return m_bEQ_Ready; }
            set { m_bEQ_Ready = value; }
        }


        public void PortToCIMBitMapUpdate()
        {
            //상시 업데이트 영역
            bool bBusy = IsPortBusy();
            bool bAutoManualCycle = IsAutoManualCycleRun();
            bool bAutoRun = IsAutoControlRun();
            bool bAlarm = GetAlarmLevel() == Interface.Alarm.AlarmLevel.Error ? true : false;
            bool bPortServoOn = IsPortPowerOn();
            bool bEcat = MovenCore.WMX3.IsEngineCommunicating();
            bool bMoveReserve = m_eControlMode == ControlMode.CIMMode ? CMD_MoveReserved : false;
            bool bTwoBuffer = GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer;

            Status_EStop            = IsEQPort() ? false : IsPortEmergencyState();
            Status_AutoRun          = IsEQPort() ? false : bAutoRun;
            Status_RunEnable        = IsEQPort() ? false : (!bBusy && !bAutoManualCycle && !bAutoRun && !bAlarm && IsPortHomeDone());
            Status_StopEnable       = IsEQPort() ? false : (bAutoManualCycle || bAutoRun) && IsStopEnable();
            Status_PowerOnEnable    = IsEQPort() ? false : !bPortServoOn && bEcat;
            Status_PowerOffEnable   = IsEQPort() ? false : bPortServoOn && bEcat && !bAutoRun && !bAutoManualCycle;

            Status_PowerOn          = IsEQPort() ? false : (bPortServoOn && bEcat);
            Status_MGV              = IsEQPort() ? false : GetPortOperationMode() == PortOperationMode.MGV;
            Status_AGVorOHT         = IsEQPort() ? false : (GetPortOperationMode() == PortOperationMode.AGV || GetPortOperationMode() == PortOperationMode.OHT);
            Status_Input            = IsEQPort() ? false : GetOperationDirection() == PortDirection.Input;
            Status_Output           = IsEQPort() ? false : GetOperationDirection() == PortDirection.Output;
            Status_CIMMode          = IsEQPort() ? false : m_eControlMode == ControlMode.CIMMode;

            if (!IsEQPort())
            {
                if (GetOperationDirection() == PortDirection.Input)
                {
                    //Status_Buffer1_CST_Status = OP
                    //Status_Buffer2_CST_Status = LP

                    //OP Update
                    if (IsShuttleControlPort())
                        Status_Buffer1_CST_Status = (Sensor_OP_CST_Detect1 && Sensor_OP_CST_Detect2 && Sensor_OP_CST_Presence); //OP Input인 경우 랙마와 PIO 직전 상황으로 3개의 센서가 다 On 되야 자재 있는 것으로 판단.
                    else if (IsBufferControlPort())
                    {
                        if (IsZAxisPos_UP(PortAxis.Buffer_OP_Z))
                            Status_Buffer1_CST_Status = (Sensor_OP_CST_Detect1 && Sensor_OP_CST_Detect2);
                        else if (IsZAxisPos_DOWN(PortAxis.Buffer_OP_Z))
                            Status_Buffer1_CST_Status = (Sensor_OP_CV_IN && Sensor_OP_CV_STOP);
                    }

                    //LP Update
                    if(IsShuttleControlPort())
                    {
                        if(bTwoBuffer)
                        {
                            bool bSensor_LP_CST_Detect2 = IsOHT() && !IsValidInputItemMapping(OHT_InputItem.LP_Placement_Detect_2.ToString()) ? false : Sensor_LP_CST_Detect2;

                            //기존 Step
                            Status_Buffer2_CST_Status = Sensor_LP_CST_Detect1 || bSensor_LP_CST_Detect2 || Sensor_LP_CST_Presence; //LP Input의 경우 장비와 PIO 직전 상황으로 센서가 1개라도 On인 경우 있다고 판단.

                        }
                        else
                        {
                            Status_Buffer2_CST_Status = Sensor_Shuttle_CSTDetect1 || Sensor_Shuttle_CSTDetect2 || Sensor_LP_CST_Presence;
                        }
                    }
                    else if (IsBufferControlPort())
                        Status_Buffer2_CST_Status = Sensor_LP_CV_IN || Sensor_LP_CV_STOP || Sensor_LP_CST_Presence;
                }
                else
                {
                    //OP Update
                    if (IsShuttleControlPort())
                        Status_Buffer1_CST_Status = (Sensor_OP_CST_Detect1 || Sensor_OP_CST_Detect2 || Sensor_OP_CST_Presence); //OP Output인 경우 랙마와 PIO 직전 상황으로 3개의 센서가 다 Off 되야 자재 없는 것으로 판단.
                    else if (IsBufferControlPort())
                    {
                        if (IsZAxisPos_UP(PortAxis.Buffer_OP_Z))
                            Status_Buffer1_CST_Status = (Sensor_OP_CST_Detect1 || Sensor_OP_CST_Detect2);
                        else if (IsZAxisPos_DOWN(PortAxis.Buffer_OP_Z))
                            Status_Buffer1_CST_Status = (Sensor_OP_CV_IN || Sensor_OP_CV_STOP);
                    }

                    //LP Update

                    if(IsShuttleControlPort())
                    {
                        if(bTwoBuffer)
                        {
                            bool bSensor_LP_CST_Detect2 = IsOHT() && !IsValidInputItemMapping(OHT_InputItem.LP_Placement_Detect_2.ToString()) ? true : Sensor_LP_CST_Detect2;

                            Status_Buffer2_CST_Status = Sensor_LP_CST_Detect1 && bSensor_LP_CST_Detect2 && Sensor_LP_CST_Presence; //LP Output인 경우 장비와 PIO 직전 상황으로 센서가 모두 On인 경우 있다고 판단.
                        }
                        else
                        {
                            Status_Buffer2_CST_Status = Sensor_Shuttle_CSTDetect1 && Sensor_Shuttle_CSTDetect2 && Sensor_LP_CST_Presence;
                        }
                    }
                    else if (IsBufferControlPort())
                        Status_Buffer2_CST_Status = Sensor_LP_CV_IN && Sensor_LP_CV_STOP;
                }
            }
            else
            {
                Status_Buffer1_CST_Status = false;
                Status_Buffer2_CST_Status = false;
            }


            Status_TypeChangeEnable         = IsEQPort() ? false : (!bBusy && !bAutoManualCycle && !bAutoRun && !bMoveReserve);
            Status_DirectionChangeEnable    = IsEQPort() ? false : (!bBusy && !bAutoManualCycle && !bAutoRun && !bMoveReserve);

            PIOStatus_PortToSTK_Error       = IsEQPort() ? false : bAlarm;

            if ((GetPortOperationMode() == PortOperationMode.AGV || GetPortOperationMode() == PortOperationMode.OHT) && bAutoRun)
            {
                PIOStatus_PortToOHT_ES = bAlarm;
                PIOStatus_PortToOHT_HO_AVBL = PIOStatus_PortToOHT_ES; //ES신호 동기되도록 구성 
                PIOStatus_PortToAGV_ES = bAlarm;
            }


            if (Master.m_Omron?.m_OmronEnable ?? false)
                PIOStatus_PortToOMRON_Error = PIOStatus_PortToSTK_Error;

            Sensor_Buzzer = !Master.CMD_Buzzer_Mute_REQ && bAlarm;
        }

        bool m_bOMRON_To_Port_Load_REQ = false;
        bool m_bOMRON_To_Port_Unload_REQ = false;
        bool m_bOMRON_To_Port_Ready = false;
        bool m_bOMRON_To_Port_Auto = false;
        bool m_bOMRON_To_Port_Error = false;
        bool m_bPort_To_OMRON_TR_REQ = false;
        bool m_bPort_To_OMRON_Busy_REQ = false;
        bool m_bPort_To_OMRON_Complete = false;
        bool m_bPort_To_OMRON_Auto = false;
        bool m_bPort_To_OMRON_Error = false;
        ManualPIOKeeper m_Port_To_OMRON_PIOKeeper = new ManualPIOKeeper(4);

        public bool PIOStatus_OMRONToPort_Load_REQ
        {
            get
            {
                if (GetParam().ID == "30301")
                    m_bOMRON_To_Port_Load_REQ = Master.m_ReadOmronBitMap[0];
                else if (GetParam().ID == "30302")
                    m_bOMRON_To_Port_Load_REQ = Master.m_ReadOmronBitMap[64];
                else if (GetParam().ID == "30303")
                    m_bOMRON_To_Port_Load_REQ = Master.m_ReadOmronBitMap[128];
                else if (GetParam().ID == "30304")
                    m_bOMRON_To_Port_Load_REQ = Master.m_ReadOmronBitMap[192];

                return m_bOMRON_To_Port_Load_REQ;
            }
        }
        public bool PIOStatus_OMRONToPort_Unload_REQ
        {
            get
            {
                if (GetParam().ID == "30301")
                    m_bOMRON_To_Port_Unload_REQ = Master.m_ReadOmronBitMap[0 + 1];
                else if (GetParam().ID == "30302")
                    m_bOMRON_To_Port_Unload_REQ = Master.m_ReadOmronBitMap[64 + 1];
                else if (GetParam().ID == "30303")
                    m_bOMRON_To_Port_Unload_REQ = Master.m_ReadOmronBitMap[128 + 1];
                else if (GetParam().ID == "30304")
                    m_bOMRON_To_Port_Unload_REQ = Master.m_ReadOmronBitMap[192 + 1];

                return m_bOMRON_To_Port_Unload_REQ;
            }
        }
        public bool PIOStatus_OMRONToPort_Ready
        {
            get
            {
                if (GetParam().ID == "30301")
                    m_bOMRON_To_Port_Ready = Master.m_ReadOmronBitMap[0 + 2];
                else if (GetParam().ID == "30302")
                    m_bOMRON_To_Port_Ready = Master.m_ReadOmronBitMap[64 + 2];
                else if (GetParam().ID == "30303")
                    m_bOMRON_To_Port_Ready = Master.m_ReadOmronBitMap[128 + 2];
                else if (GetParam().ID == "30304")
                    m_bOMRON_To_Port_Ready = Master.m_ReadOmronBitMap[192 + 2];

                return m_bOMRON_To_Port_Ready;
            }
        }
        public bool PIOStatus_OMRONToPort_Auto
        {
            get
            {
                if (GetParam().ID == "30301")
                    m_bOMRON_To_Port_Auto = Master.m_ReadOmronBitMap[0 + 3];
                else if (GetParam().ID == "30302")
                    m_bOMRON_To_Port_Auto = Master.m_ReadOmronBitMap[64 + 3];
                else if (GetParam().ID == "30303")
                    m_bOMRON_To_Port_Auto = Master.m_ReadOmronBitMap[128 + 3];
                else if (GetParam().ID == "30304")
                    m_bOMRON_To_Port_Auto = Master.m_ReadOmronBitMap[192 + 3];

                return m_bOMRON_To_Port_Auto;
            }
        }
        public bool PIOStatus_OMRONToPort_Error
        {
            get
            {
                if (GetParam().ID == "30301")
                    m_bOMRON_To_Port_Error = Master.m_ReadOmronBitMap[0 + 4];
                else if (GetParam().ID == "30302")
                    m_bOMRON_To_Port_Error = Master.m_ReadOmronBitMap[64 + 4];
                else if (GetParam().ID == "30303")
                    m_bOMRON_To_Port_Error = Master.m_ReadOmronBitMap[128 + 4];
                else if (GetParam().ID == "30304")
                    m_bOMRON_To_Port_Error = Master.m_ReadOmronBitMap[192 + 4];

                return m_bOMRON_To_Port_Error;
            }
        }

        public bool PIOStatus_PortToOMRON_TR_REQ
        {
            get { return m_bPort_To_OMRON_TR_REQ; }
            set
            {
                m_bPort_To_OMRON_TR_REQ = value;

                if (GetParam().ID == "30301")
                    Master.m_WriteOmronBitMap[0] = m_bPort_To_OMRON_TR_REQ;
                else if (GetParam().ID == "30302")
                    Master.m_WriteOmronBitMap[64] = m_bPort_To_OMRON_TR_REQ;
                else if (GetParam().ID == "30303")
                    Master.m_WriteOmronBitMap[128] = m_bPort_To_OMRON_TR_REQ;
                else if (GetParam().ID == "30304")
                    Master.m_WriteOmronBitMap[192] = m_bPort_To_OMRON_TR_REQ;
            }
        }
        public bool PIOStatus_PortToOMRON_Busy_REQ
        {
            get { return m_bPort_To_OMRON_Busy_REQ; }
            set
            {
                m_bPort_To_OMRON_Busy_REQ = value;

                if (GetParam().ID == "30301")
                    Master.m_WriteOmronBitMap[0 + 1] = m_bPort_To_OMRON_Busy_REQ;
                else if (GetParam().ID == "30302")
                    Master.m_WriteOmronBitMap[64 + 1] = m_bPort_To_OMRON_Busy_REQ;
                else if (GetParam().ID == "30303")
                    Master.m_WriteOmronBitMap[128 + 1] = m_bPort_To_OMRON_Busy_REQ;
                else if (GetParam().ID == "30304")
                    Master.m_WriteOmronBitMap[192 + 1] = m_bPort_To_OMRON_Busy_REQ;
            }
        }
        public bool PIOStatus_PortToOMRON_Complete
        {
            get { return m_bPort_To_OMRON_Complete; }
            set
            {
                m_bPort_To_OMRON_Complete = value;

                if (GetParam().ID == "30301")
                    Master.m_WriteOmronBitMap[0 + 2] = m_bPort_To_OMRON_Complete;
                else if (GetParam().ID == "30302")
                    Master.m_WriteOmronBitMap[64 + 2] = m_bPort_To_OMRON_Complete;
                else if (GetParam().ID == "30303")
                    Master.m_WriteOmronBitMap[128 + 2] = m_bPort_To_OMRON_Complete;
                else if (GetParam().ID == "30304")
                    Master.m_WriteOmronBitMap[192 + 2] = m_bPort_To_OMRON_Complete;
            }
        }
        public bool PIOStatus_PortToOMRON_Auto
        {
            get { return m_bPort_To_OMRON_Auto; }
            set
            {
                m_bPort_To_OMRON_Auto = value;

                if (GetParam().ID == "30301")
                    Master.m_WriteOmronBitMap[0 + 3] = m_bPort_To_OMRON_Auto;
                else if (GetParam().ID == "30302")
                    Master.m_WriteOmronBitMap[64 + 3] = m_bPort_To_OMRON_Auto;
                else if (GetParam().ID == "30303")
                    Master.m_WriteOmronBitMap[128 + 3] = m_bPort_To_OMRON_Auto;
                else if (GetParam().ID == "30304")
                    Master.m_WriteOmronBitMap[192 + 3] = m_bPort_To_OMRON_Auto;
            }
        }
        public bool PIOStatus_PortToOMRON_Error
        {
            get { return m_bPort_To_OMRON_Error; }
            set
            {
                m_bPort_To_OMRON_Error = value;

                if (GetParam().ID == "30301")
                    Master.m_WriteOmronBitMap[0 + 4] = m_bPort_To_OMRON_Error;
                else if (GetParam().ID == "30302")
                    Master.m_WriteOmronBitMap[64 + 4] = m_bPort_To_OMRON_Error;
                else if (GetParam().ID == "30303")
                    Master.m_WriteOmronBitMap[128 + 4] = m_bPort_To_OMRON_Error;
                else if (GetParam().ID == "30304")
                    Master.m_WriteOmronBitMap[192 + 4] = m_bPort_To_OMRON_Error;
            }
        }
        public void PIOStatus_ManualSavePortToOMRONPIO()
        {
            if (!IsAutoControlRun() &&
                !IsAutoManualCycleRun() &&
                m_eControlMode != ControlMode.CIMMode &&
                m_Port_To_OMRON_PIOKeeper.bInit == false)
            {
                m_Port_To_OMRON_PIOKeeper.bInit = true;
                m_Port_To_OMRON_PIOKeeper.bFlag[0] = PIOStatus_PortToOMRON_TR_REQ;
                m_Port_To_OMRON_PIOKeeper.bFlag[1] = PIOStatus_PortToOMRON_Busy_REQ;
                m_Port_To_OMRON_PIOKeeper.bFlag[2] = PIOStatus_PortToOMRON_Complete;
                m_Port_To_OMRON_PIOKeeper.bFlag[3] = PIOStatus_PortToOMRON_Error;

                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PIOCurrentFlagKeeping,
                    $"[Port->OMRON]: " +
                    $"TR_REQ({m_Port_To_OMRON_PIOKeeper.bFlag[0]}) / " +
                    $"Busy_REQ({m_Port_To_OMRON_PIOKeeper.bFlag[1]}) / " +
                    $"Complete({m_Port_To_OMRON_PIOKeeper.bFlag[2]}) / " +
                    $"Error({m_Port_To_OMRON_PIOKeeper.bFlag[3]})");
            }
        }
        public void PIOStatus_ManualReleasePortToOMRONPIO()
        {
            if (m_Port_To_OMRON_PIOKeeper.bInit)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PIOKeepingFlagReload,
                    $"[Port->OMRON]: " +
                    $"TR_REQ({m_Port_To_OMRON_PIOKeeper.bFlag[0]}) / " +
                    $"Busy_REQ({m_Port_To_OMRON_PIOKeeper.bFlag[1]}) / " +
                    $"Complete({m_Port_To_OMRON_PIOKeeper.bFlag[2]}) / " +
                    $"Error({m_Port_To_OMRON_PIOKeeper.bFlag[3]})");

                if (PIOStatus_PortToOMRON_TR_REQ != m_Port_To_OMRON_PIOKeeper.bFlag[0])
                    Interlock_Manual_PIO_PortToOMRON_TR_REQ(m_Port_To_OMRON_PIOKeeper.bFlag[0], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToOMRON_Busy_REQ != m_Port_To_OMRON_PIOKeeper.bFlag[1])
                    Interlock_Manual_PIO_PortToOMRON_Busy_REQ(m_Port_To_OMRON_PIOKeeper.bFlag[1], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToOMRON_Complete != m_Port_To_OMRON_PIOKeeper.bFlag[2])
                    Interlock_Manual_PIO_PortToOMRON_Complete(m_Port_To_OMRON_PIOKeeper.bFlag[2], InterlockFrom.ApplicationLoop);
                if (PIOStatus_PortToOMRON_Error != m_Port_To_OMRON_PIOKeeper.bFlag[3])
                    Interlock_Manual_PIO_PortToOMRON_Error(m_Port_To_OMRON_PIOKeeper.bFlag[3], InterlockFrom.ApplicationLoop);

                m_Port_To_OMRON_PIOKeeper.bInit = false;
            }
        }
    }
}
