using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using Master.Interface.Alarm;

namespace Master.Equipment.Port
{
    /// <summary>
    /// PortAutoMotion.cs는 Port의 오토 공정에서 사용할 함수의 집합
    /// 하위 폴더인 AutoStep에서 여기에 기재된 함수들을 주로 사용
    /// </summary>
    public partial class Port
    {
        private Stopwatch m_AutoRunProgressTime = new Stopwatch();
        private Stopwatch m_RMCSTIDRWTimer = new Stopwatch();

        //Auto
        private bool m_bAutoRunStopReq = false;

        //Manual
        private int m_CycleControlSetCount = 0;
        private int m_CycleControlProgressCount = 0;
        private bool m_bAutoManualCycleStopReq = false;
        private bool m_bCycleRunning = false;
        private bool AGVFullFlagOption = false;

        /// <summary>
        /// AGV가 CS를 쓰는 경우도 있고 안쓰는 경우도 있어 만든 함수
        /// AGV Full Flag Option : true (CS 체크)
        /// AGV Full Flag Option : false (Valid 부터 체크)
        /// </summary>
        /// <returns></returns>
        public bool GetAGVPIOFlagOption()
        {
            return AGVFullFlagOption;
        }

        /// <summary>
        /// 인터락 통해 들어온 최종 Auto Cycle 동작, 정지 명령 수행부
        /// </summary>
        private void CMD_PortStartAutoControl()
        {
            try
            {
                //Stop REQ를 받고 Stop Enable 상태에 따라서 Stop 진행
                m_bAutoRunStopReq = false;
                //Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Stop_Enable, true);

                AutoControlRun(true);
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"Port[{GetParam().ID}] Start Auto Control");
            }
        }
        private void CMD_PortStopAutoControl()
        {
            m_bAutoRunStopReq = true;
        }
        
        /// <summary>
        /// 인터락 통해 들어온 최종 Manual Cycle 동작, 정지 명령 수행부
        /// </summary>
        /// <param name="CycleCount"></param>
        private void CMD_PortStartAutoManualCycleControl(int CycleCount)
        {
            try
            {
                m_AutoRunProgressTime = new Stopwatch();
                m_CycleControlSetCount = CycleCount;
                m_CycleControlProgressCount = 0;
                m_bAutoManualCycleStopReq = false;
                //Set_Port_2_CIM_Bit_Data(SendBitMapIndex.Stop_Enable, true);

                AutoControlRun(false);
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"Port[{GetParam().ID}] Start Auto Cycle Control");
            }
        }
        private void CMD_PortStopAutoManualCycleControl()
        {
            m_bAutoManualCycleStopReq = true;
        }

        /// <summary>
        /// 포트의 공정 시작 명령 및 이니셜
        /// </summary>
        /// <param name="bAutoRun"></param>
        private void AutoControlRun(bool bAutoRun)
        {
            if(bAutoRun)
            {
                PIOStatus_ManualReleasePortToOHTPIO();
                PIOStatus_ManualReleasePortToAGVPIO();
                PIOStatus_ManualReleasePortToRMPIO();
                PIOStatus_ManualReleasePortToOMRONPIO();
            }

            if (IsShuttleControlPort())
            {
                //Initialize AutoControl
                if (Carrier_CheckOP_ExistProduct(false, false))
                    OP_CarrierID = string.Empty;

                if (Carrier_CheckShuttle_ExistProduct(false))
                    Carrier_ClearBP_CarrierID(0);

                if (Carrier_CheckLP_ExistProduct(false, false))
                    LP_CarrierID = string.Empty;

                if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                {
                    if (bAutoRun)
                    {
                        m_bShuttle_2BP_AutoRunning = true;
                        m_bLP_2BP_AutoRunning = true;
                        m_bOP_2BP_AutoRunning = true;

                        m_bLP_2BP_AutoStopEnable = true;
                        m_bOP_2BP_AutoStopEnable = true;
                        m_bShuttle_2BP_AutoStopEnable = true;

                        m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step000_Check_Direction;
                        m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step000_Check_Direction;
                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step000_Check_Direction;

                        Pre_LP_2BP_AutoStep = LP_2BP_AutoStep.Step000_Check_Direction;
                        Pre_OP_2BP_AutoStep = OP_2BP_AutoStep.Step000_Check_Direction;
                        Pre_Shuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step000_Check_Direction;

                        Auto_2BP_Start_LP_Control();
                        Auto_2BP_Start_OP_Control();
                        Auto_2BP_Start_Shuttle_Control();
                    }
                    else
                    {
                        m_bCycleRunning = true;
                        m_bShuttle_2BP_AutoStopEnable = true;
                        m_eLP_2BP_AutoStep = LP_2BP_AutoStep.Step000_Check_Direction;
                        m_eOP_2BP_AutoStep = OP_2BP_AutoStep.Step000_Check_Direction;
                        m_eShuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step000_Check_Direction;

                        Pre_LP_2BP_AutoStep = LP_2BP_AutoStep.Step000_Check_Direction;
                        Pre_OP_2BP_AutoStep = OP_2BP_AutoStep.Step000_Check_Direction;
                        Pre_Shuttle_2BP_AutoStep = Shuttle_2BP_AutoStep.Step000_Check_Direction;

                        Auto_2BP_Start_Shuttle_Control();
                    }
                }
                else if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                {
                    if (bAutoRun)
                    {
                        m_bShuttle_1BP_AutoRunning = true;
                        m_bOP_1BP_AutoRunning = true;

                        m_bOP_1BP_AutoStopEnable = true;
                        m_bShuttle_1BP_AutoStopEnable = true;

                        m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step000_Check_Direction;
                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step000_Check_Direction;

                        Pre_OP_1BP_AutoStep = OP_1BP_AutoStep.Step000_Check_Direction;
                        Pre_Shuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step000_Check_Direction;

                        Auto_1BP_Start_OP_Control();
                        Auto_1BP_Start_Shuttle_Control();
                    }
                    else
                    {
                        m_bCycleRunning = true;
                        m_bShuttle_1BP_AutoStopEnable = true;
                        m_eOP_1BP_AutoStep = OP_1BP_AutoStep.Step000_Check_Direction;
                        m_eShuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step000_Check_Direction;

                        Pre_OP_1BP_AutoStep = OP_1BP_AutoStep.Step000_Check_Direction;
                        Pre_Shuttle_1BP_AutoStep = Shuttle_1BP_AutoStep.Step000_Check_Direction;

                        Auto_1BP_Start_Shuttle_Control();
                    }
                }
            }
            else if (IsBufferControlPort())
            {
                //Initialize AutoControl

                if (GetParam().ePortType == PortType.Conveyor_AGV)
                {
                    if (Carrier_CheckOP_ExistProduct(false, false))
                        OP_CarrierID = string.Empty;
                }
                else if(GetParam().ePortType == PortType.Conveyor_OMRON)
                {
                    if(IsZAxisPos_UP(PortAxis.Buffer_OP_Z))
                    {
                        if(!Sensor_OP_CST_Detect1 && !Sensor_OP_CST_Detect2)
                        {
                            OP_CarrierID = string.Empty;
                        }
                    }
                    else if(IsZAxisPos_DOWN(PortAxis.Buffer_OP_Z))
                    {
                        if (Carrier_CheckOP_ExistProduct(false, false))
                            OP_CarrierID = string.Empty;
                    }
                }

                var origin = Enum.GetValues(typeof(BufferCV));
                foreach (BufferCV eBufferCV in origin)
                {
                    if (eBufferCV < BufferCV.Buffer_BP1 || eBufferCV > BufferCV.Buffer_BP4)
                        continue;

                    if (GetMotionParam().IsCVUsed(eBufferCV))
                    {
                        if (GetMotionParam().IsCSTDetectSensorEnable(eBufferCV))
                        {
                            int BPIndex = (int)eBufferCV - 2;

                            if (!BufferCtrl_BP_CSTDetect_Status(eBufferCV))
                            {
                                Carrier_ClearBP_CarrierID(BPIndex);
                            }
                        }
                    }
                }

                if (Carrier_CheckLP_ExistProduct(false, false))
                    LP_CarrierID = string.Empty;

                if (GetParam().ePortType == PortType.Conveyor_AGV)
                {
                    if (GetMotionParam().IsBPCVUsed())
                    {
                        m_bCycleRunning = !bAutoRun;
                        m_bLP_CV_AutoRunning = bAutoRun;
                        m_bOP_CV_AutoRunning = bAutoRun;
                        m_bBP_CV_AutoRunning = bAutoRun;

                        m_bLP_CV_AutoStopEnable = true;
                        m_bOP_CV_AutoStopEnable = true;
                        m_bBP_CV_AutoStopEnable = true;

                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step000_Check_Direction;
                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step000_Check_Direction;
                        m_eBP_CV_AutoStep = BP_CV_AutoStep.Step000_Check_Direction;

                        Pre_OP_CV_AutoStep = OP_CV_AutoStep.Step000_Check_Direction;
                        Pre_LP_CV_AutoStep = LP_CV_AutoStep.Step000_Check_Direction;
                        Pre_BP_CV_AutoStep = BP_CV_AutoStep.Step000_Check_Direction;

                        Auto_CV_Start_OP_Control();
                        Auto_CV_Start_LP_Control();
                        Auto_CV_Start_BP_Control();
                    }
                    else
                    {
                        m_bCycleRunning = !bAutoRun;
                        m_bLP_CV_AutoRunning = bAutoRun;
                        m_bOP_CV_AutoRunning = bAutoRun;

                        m_bLP_CV_AutoStopEnable = true;
                        m_bOP_CV_AutoStopEnable = true;

                        m_eOP_CV_AutoStep = OP_CV_AutoStep.Step000_Check_Direction;
                        m_eLP_CV_AutoStep = LP_CV_AutoStep.Step000_Check_Direction;

                        Pre_OP_CV_AutoStep = OP_CV_AutoStep.Step000_Check_Direction;
                        Pre_LP_CV_AutoStep = LP_CV_AutoStep.Step000_Check_Direction;

                        Auto_CV_Start_OP_Control();
                        Auto_CV_Start_LP_Control();
                    }
                }
                else if(GetParam().ePortType == PortType.Conveyor_OMRON)
                {
                    m_bCycleRunning = !bAutoRun;
                    m_bLP_CV_DIEBANK_AutoRunning = bAutoRun;
                    m_bOP_CV_DIEBANK_AutoRunning = bAutoRun;

                    m_bLP_CV_DIEBANK_AutoStopEnable = true;
                    m_bOP_CV_DIEBANK_AutoStopEnable = true;

                    m_eOP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step000_Check_Direction;
                    m_eLP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step000_Check_Direction;

                    Pre_OP_CV_DIEBANK_AutoStep = OP_CV_DIEBANK_AutoStep.Step000_Check_Direction;
                    Pre_LP_CV_DIEBANK_AutoStep = LP_CV_DIEBANK_AutoStep.Step000_Check_Direction;

                    Auto_CV_Start_DIEBANK_OP_Control();
                    Auto_CV_Start_DIEBANK_LP_Control();
                }
            }
        }


        /// <summary>
        /// Auto Cycle 공정 상태를 가져옴 (PIO 포함 모든 동작)
        /// </summary>
        /// <returns></returns>
        public bool IsAutoControlRun()
        {
            if (IsShuttleControlPort())
            {
                if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                {
                    return m_bLP_2BP_AutoRunning || m_bOP_2BP_AutoRunning || m_bShuttle_2BP_AutoRunning;
                }
                else if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                {
                    return m_bOP_1BP_AutoRunning || m_bShuttle_1BP_AutoRunning;
                }
                else
                    return false;
            }
            else if (IsBufferControlPort())
            {
                if (GetParam().ePortType == PortType.Conveyor_AGV)
                {
                    if (GetMotionParam().IsBPCVUsed())
                    {
                        return m_bLP_CV_AutoRunning || m_bOP_CV_AutoRunning || m_bBP_CV_AutoRunning;
                    }
                    else
                        return m_bLP_CV_AutoRunning || m_bOP_CV_AutoRunning;
                }
                else if (GetParam().ePortType == PortType.Conveyor_OMRON)
                {
                    return m_bLP_CV_DIEBANK_AutoRunning || m_bOP_CV_DIEBANK_AutoRunning;
                }
                else
                    return false;
            }
            else
                return false;
        }
        
        /// <summary>
        /// Manual Cycle 동작 상태를 가져옴 (PIO 제외 동작)
        /// </summary>
        /// <returns></returns>
        public bool IsAutoManualCycleRun()
        {
            return m_bCycleRunning;
        }
        
        /// <summary>
        /// Auto or Manual Cycle 공정의 Stop 요청 상태를 가져옴
        /// </summary>
        /// <param name="bAutoMode"></param>
        /// <returns></returns>
        private bool IsAutoStopReq(bool bAutoMode)
        {
            if (bAutoMode)
                return m_bAutoRunStopReq;
            else
                return m_bAutoManualCycleStopReq;
        }


        /// <summary>
        /// PIO 초기화 함수
        /// 공정 시작 부분에서 호출
        /// </summary>
        public void Port_To_RM_PIO_Init()
        {
            PIOStatus_PortToSTK_Load_Req = false;
            PIOStatus_PortToSTK_Unload_Req = false;
            PIOStatus_PortToSTK_Ready = false;
            PIOStatus_PortToSTK_Error = false;
        }
        public void Port_To_OMRON_PIO_Init()
        {
            PIOStatus_PortToOMRON_TR_REQ = false;
            PIOStatus_PortToOMRON_Busy_REQ = false;
            PIOStatus_PortToOMRON_Complete = false;
            PIOStatus_PortToOMRON_Error = false;
        }
        public void Port_To_AGV_PIO_Init()
        {
            PIOStatus_PortToAGV_Load_Req = false;
            PIOStatus_PortToAGV_Unload_Req = false;
            PIOStatus_PortToAGV_ES = false;
            PIOStatus_PortToAGV_Ready = false;
        }
        public void Port_To_OHT_PIO_Init()
        {
            PIOStatus_PortToOHT_Load_Req    = false;
            PIOStatus_PortToOHT_Unload_Req  = false;
            PIOStatus_PortToOHT_HO_AVBL     = false;
            PIOStatus_PortToOHT_ES          = false;
            PIOStatus_PortToOHT_Ready       = false;
        }


        /// <summary>
        /// AGV or OHT -> Port로 전송받은 PIO 상태
        /// ES, HO_AVBL 신호는 공정 중인 경우 Port의 Alarm 상태와 연동
        /// </summary>
        /// <returns></returns>
        private bool Is_EquipType_To_Port_PIO_ValidFlag()
        {
            if (GetPortOperationMode() == PortOperationMode.OHT)
                return PIOStatus_OHTToPort_Valid;
            else if (GetPortOperationMode() == PortOperationMode.AGV || GetPortOperationMode() == PortOperationMode.Conveyor)
            {
                if (GetParam().ePortType == PortType.Conveyor_OMRON)
                    return false;
                else
                    return PIOStatus_AGVToPort_Valid;
            }
            else
                return false;
        }
        private bool Is_EquipType_To_Port_PIO_CSFlag()
        {
            if (GetPortOperationMode() == PortOperationMode.OHT)
                return PIOStatus_OHTToPort_CS0;
            else if (GetPortOperationMode() == PortOperationMode.AGV || GetPortOperationMode() == PortOperationMode.Conveyor)
            {
                if (GetParam().ePortType == PortType.Conveyor_OMRON)
                    return false;
                else
                    return PIOStatus_AGVToPort_CS0;
            }
            else
                return false;
        }
        private bool Is_EquipType_To_Port_PIO_TR_ReqFlag()
        {
            if (GetPortOperationMode() == PortOperationMode.OHT)
                return PIOStatus_OHTToPort_TR_Req;
            else if (GetPortOperationMode() == PortOperationMode.AGV || GetParam().ePortType == PortType.Conveyor_AGV)
            {
                return PIOStatus_AGVToPort_TR_Req;
            }
            else
                return false;
        }
        private bool Is_EquipType_To_Port_PIO_BusyFlag()
        {
            if (GetPortOperationMode() == PortOperationMode.OHT)
                return PIOStatus_OHTToPort_Busy;
            else if (GetPortOperationMode() == PortOperationMode.AGV || GetParam().ePortType == PortType.Conveyor_AGV)
            {
                return PIOStatus_AGVToPort_Busy;
            }
            else
                return false;
        }
        private bool Is_EquipType_To_Port_PIO_Complete()
        {
            if (GetPortOperationMode() == PortOperationMode.OHT)
                return PIOStatus_OHTToPort_Complete;
            else if (GetPortOperationMode() == PortOperationMode.AGV || GetParam().ePortType == PortType.Conveyor_AGV)
            {
                return PIOStatus_AGVToPort_Complete;
            }
            else
                return false;
        }
        
        /// <summary>
        /// Load or Unload Off를 체크하기 위한 함수
        /// </summary>
        /// <returns></returns>
        private bool Is_Port_To_Equip_PIO_Load_ReqFlag()
        {
            if (GetPortOperationMode() == PortOperationMode.OHT)
                return PIOStatus_PortToOHT_Load_Req;
            else if (GetPortOperationMode() == PortOperationMode.AGV || GetParam().ePortType == PortType.Conveyor_AGV)
            {
                return PIOStatus_PortToAGV_Load_Req;
            }
            else
                return false;
        }
        private bool Is_Port_To_Equip_PIO_Unload_ReqFlag()
        {
            if (GetPortOperationMode() == PortOperationMode.OHT)
                return PIOStatus_PortToOHT_Unload_Req;
            else if (GetPortOperationMode() == PortOperationMode.AGV || GetParam().ePortType == PortType.Conveyor_AGV)
            {
                return PIOStatus_PortToAGV_Unload_Req;
            }
            else
                return false;
        }
        
        /// <summary>
        /// Port -> AGV or OHT에 전송할 PIO 상태
        /// </summary>
        /// <param name="bEnable"></param>
        private void Set_Port_To_Equip_PIO_Load_ReqFlag(bool bEnable)
        {
            if (GetPortOperationMode() == PortOperationMode.OHT)
                PIOStatus_PortToOHT_Load_Req = bEnable;
            else if (GetPortOperationMode() == PortOperationMode.AGV || GetParam().ePortType == PortType.Conveyor_AGV)
            {
                PIOStatus_PortToAGV_Load_Req = bEnable;
            }
        }
        private void Set_Port_To_Equip_PIO_Unload_ReqFlag(bool bEnable)
        {
            if (GetPortOperationMode() == PortOperationMode.OHT)
                PIOStatus_PortToOHT_Unload_Req = bEnable;
            else if (GetPortOperationMode() == PortOperationMode.AGV || GetParam().ePortType == PortType.Conveyor_AGV)
            {
                PIOStatus_PortToAGV_Unload_Req = bEnable;
            }
        }
        private void Set_Port_To_Equip_PIO_ReadyFlag(bool bEnable)
        {
            if (GetPortOperationMode() == PortOperationMode.OHT)
                PIOStatus_PortToOHT_Ready = bEnable;
            else if (GetPortOperationMode() == PortOperationMode.AGV || GetParam().ePortType == PortType.Conveyor_AGV)
            {
                PIOStatus_PortToAGV_Ready = bEnable;
            }
        }

        /// <summary>
        /// Light curtain이 감지 해제된 경우 N초 이후 동작하기 위한 함수
        /// </summary>
        /// <returns></returns>
        private bool Is_LightCurtainRelease()
        {
            if (IsValidInputItemMapping(OHT_InputItem.Port_Area_Sensor.ToString()) || IsValidInputItemMapping(AGV_MGV_InputItem.MGV_Port_Area_Sensor.ToString()))
                return Watchdog_IsDetect(WatchdogList.PortArea_Release_Timer);
            else
                return true;
        }
        
        /// <summary>
        /// 포트 타입에 따라 안전 인터락인 Light curtain or Hoist를 체크
        /// 모션 정지 및 스텝 조건에 사용
        /// </summary>
        /// <returns></returns>
        private bool Is_LightCurtain_or_Hoist_SensorCheck()
        {
            if (GetParam().ePortType == PortType.AGV ||
                GetParam().ePortType == PortType.MGV_AGV ||
                GetParam().ePortType == PortType.MGV)
                return Sensor_LightCurtain;
            else if (GetParam().ePortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
                return false;
            else if (GetParam().ePortType == PortType.MGV_OHT ||
                    GetParam().ePortType == PortType.OHT)
                return Sensor_LP_Hoist_Detect;
            else
                return false;
        }

        /// <summary>
        /// MGV 영역 CartDetect 체크 함수
        /// Cart가 Detect 되는 경우 Light Curtain의 와치독이 초기화 됨
        /// </summary>
        /// <returns></returns>
        private bool Is_CartDetct1_Check()
        {
            if (GetParam().ePortType == PortType.AGV ||
                GetParam().ePortType == PortType.MGV_AGV ||
                GetParam().ePortType == PortType.MGV)
            {
                bool bValidCart1 = IsValidInputItemMapping(AGV_MGV_InputItem.Cart_Detect1.ToString());
                bool bCartDetect1 = bValidCart1 && Sensor_LP_Cart_Detect1;
                return bCartDetect1;
            }
            else if (GetParam().ePortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
                return false;
            else if (GetParam().ePortType == PortType.MGV_OHT ||
                    GetParam().ePortType == PortType.OHT)
            {
                bool bValidCart1 = IsValidInputItemMapping(OHT_InputItem.Cart_Detect1.ToString());
                bool bCartDetect1 = bValidCart1 && Sensor_LP_Cart_Detect1;
                return bCartDetect1;
            }
            else
                return false;
        }
        private bool Is_CartDetct2_Check()
        {
            if (GetParam().ePortType == PortType.AGV ||
                GetParam().ePortType == PortType.MGV_AGV ||
                GetParam().ePortType == PortType.MGV)
            {
                bool bValidCart2 = IsValidInputItemMapping(AGV_MGV_InputItem.Cart_Detect2.ToString());
                bool bCartDetect2 = bValidCart2 && Sensor_LP_Cart_Detect2;
                return bCartDetect2;
            }
            else if (GetParam().ePortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
                return false;
            else if (GetParam().ePortType == PortType.MGV_OHT ||
                    GetParam().ePortType == PortType.OHT)
            {
                bool bValidCart2 = IsValidInputItemMapping(OHT_InputItem.Cart_Detect2.ToString());
                bool bCartDetect2 = bValidCart2 && Sensor_LP_Cart_Detect2;
                return bCartDetect2;
            }
            else
                return false;
        }

        /// <summary>
        /// OHT 영역 Door Open Sensor 체크 함수
        /// </summary>
        /// <returns></returns>
        private bool Is_OHTDoorOpen_Check()
        {
            if (GetParam().ePortType == PortType.AGV ||
                GetParam().ePortType == PortType.MGV_AGV ||
                GetParam().ePortType == PortType.MGV ||
                GetParam().ePortType == PortType.Conveyor_AGV ||
                GetParam().ePortType == PortType.Conveyor_OMRON)
            {
                return false;
            }
            else if (GetParam().ePortType == PortType.MGV_OHT ||
                    GetParam().ePortType == PortType.OHT)
            {
                bool bValid = IsValidInputItemMapping(OHT_InputItem.Door_Close_Status.ToString());
                bool bDoorOpen = bValid && !Status_OHT_Door_Close;
                return bDoorOpen;
            }
            else
                return false;
        }


        /// <summary>
        /// PIO 및 Step 분기를 위해 Operation Mode Check
        /// </summary>
        /// <returns></returns>
        public bool IsMGV()
        {
            return (GetPortOperationMode() == PortOperationMode.MGV);
        }
        private bool IsAGV()
        {
            return (GetPortOperationMode() == PortOperationMode.AGV);
        }
        private bool IsOHT()
        {
            return (GetPortOperationMode() == PortOperationMode.OHT);
        }

        /// <summary>
        /// Stop 가능 상태
        /// </summary>
        /// <returns></returns>
        private bool IsStopEnable()
        {
            if (IsShuttleControlPort())
            {
                if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                {
                    return m_bLP_2BP_AutoStopEnable && m_bOP_2BP_AutoStopEnable && m_bShuttle_2BP_AutoStopEnable;
                }
                else if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                {
                    return m_bOP_1BP_AutoStopEnable && m_bShuttle_1BP_AutoStopEnable;
                }
                else
                    return false;
            }
            else if (IsBufferControlPort())
            {
                if (GetParam().ePortType == PortType.Conveyor_AGV)
                {
                    if (GetMotionParam().IsBPCVUsed())
                    {
                        return m_bLP_CV_AutoStopEnable && m_bOP_CV_AutoStopEnable && m_bBP_CV_AutoStopEnable;
                    }
                    else
                        return m_bLP_CV_AutoStopEnable && m_bOP_CV_AutoStopEnable;
                }
                else if (GetParam().ePortType == PortType.Conveyor_OMRON)
                {
                    return m_bLP_CV_DIEBANK_AutoStopEnable && m_bOP_CV_DIEBANK_AutoStopEnable;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// X 축 관련 센서 상태
        /// Servo And Sensor : 서보의 위치와 센서 On 여부로 판단
        /// </summary>
        /// <returns></returns>
        private bool IsXAxisPos_WaitorLP()
        {
            if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                return IsXAxisPos_Wait();
            else
                return IsXAxisPos_LP();
        }
        private bool IsXAxisPos_Wait()
        {
            float Target = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.Wait_Pos);
            bool OnlyServoCheck = !IsPortAxisBusy(PortAxis.Shuttle_X) && Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(PortAxis.Shuttle_X)) && IsAxisPositionInside(PortAxis.Shuttle_X, Target, 0.1f);

            if (GetMotionParam().IsServoType(PortAxis.Shuttle_X))
                return GetMotionParam().GetPositionCheckType(PortAxis.Shuttle_X, (int)Teaching_X_Pos.Wait_Pos) == PositionCheckType.Servo_and_Sensor ? 
                    (Sensor_X_Axis_WaitPosSensor && OnlyServoCheck) : OnlyServoCheck;
            else
                return false;
        }
        private bool IsXAxisPos_LP()
        {
            float Target = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos));
            bool OnlyServoCheck = !IsPortAxisBusy(PortAxis.Shuttle_X) && Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(PortAxis.Shuttle_X)) && IsAxisPositionInside(PortAxis.Shuttle_X, Target, 0.1f);

            if (GetMotionParam().IsServoType(PortAxis.Shuttle_X))
                return GetMotionParam().GetPositionCheckType(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos)) == PositionCheckType.Servo_and_Sensor ?
                    (Sensor_X_Axis_HOME && OnlyServoCheck) : OnlyServoCheck;
            else
                return false;
        }
        private bool IsXAxisPos_OP()
        {
            float Target = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos);
            bool OnlyServoCheck = !IsPortAxisBusy(PortAxis.Shuttle_X) && Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(PortAxis.Shuttle_X)) && IsAxisPositionInside(PortAxis.Shuttle_X, Target, 0.1f);

            if (GetMotionParam().IsServoType(PortAxis.Shuttle_X))
                return GetMotionParam().GetPositionCheckType(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos) == PositionCheckType.Servo_and_Sensor ?
                    (Sensor_X_Axis_POS && OnlyServoCheck) : OnlyServoCheck;
            else

                return false;
        }

        /// <summary>
        /// Y 축 관련 센서 상태
        /// Servo And Sensor : 서보의 위치와 센서 On 여부로 판단
        /// </summary>
        /// <returns></returns>
        private bool IsYAxisPos_FWD(PortAxis ePortAxis)
        {
            if (GetMotionParam().IsCylinderType(ePortAxis))
            {
                return CylinderCtrl_GetPosSensorOn(ePortAxis, CylCtrlList.FWD);
            }
            else
                return true;
        }
        private bool IsYAxisPos_BWD(PortAxis ePortAxis)
        {
            if (GetMotionParam().IsCylinderType(ePortAxis))
            {
                return CylinderCtrl_GetPosSensorOn(ePortAxis, CylCtrlList.BWD);
            }
            else
                return true;
        }

        /// <summary>
        /// Z 축 관련 센서 상태
        /// Servo And Sensor : 서보의 위치와 센서 On 여부로 판단
        /// </summary>
        /// <returns></returns>
        public bool IsZAxisPos_UP(PortAxis ePortAxis)
        {
            if (GetMotionParam().IsServoType(ePortAxis))
            {
                float Target = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_Z_Pos.Up_Pos);
                bool OnlyServoCheck = !IsPortAxisBusy(ePortAxis) && Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(ePortAxis)) && IsAxisPositionInside(ePortAxis, Target, 0.1f);

                return GetMotionParam().GetPositionCheckType(ePortAxis, (int)Teaching_Z_Pos.Up_Pos) == PositionCheckType.Servo_and_Sensor ?
                    (Sensor_Z_Axis_POS && OnlyServoCheck) : OnlyServoCheck;
            }
            else if (GetMotionParam().IsCylinderType(ePortAxis))
            {
                return CylinderCtrl_GetPosSensorOn(ePortAxis, CylCtrlList.FWD);
            }
            else if (GetMotionParam().IsInverterType(ePortAxis))
            {
                if (ePortAxis == PortAxis.Buffer_LP_Z)
                    return Sensor_LP_Z_POS2;
                else if (ePortAxis == PortAxis.Buffer_OP_Z)
                    return Sensor_OP_Z_POS2;
                else
                    return false;
            }
            else
                return true;
        }
        public bool IsZAxisPos_DOWN(PortAxis ePortAxis)
        {
            if (GetMotionParam().IsServoType(ePortAxis))
            {
                float Target = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_Z_Pos.Down_Pos);
                bool OnlyServoCheck = !IsPortAxisBusy(ePortAxis) && Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(ePortAxis)) && IsAxisPositionInside(PortAxis.Shuttle_Z, Target, 0.1f);

                return GetMotionParam().GetPositionCheckType(ePortAxis, (int)Teaching_Z_Pos.Down_Pos) == PositionCheckType.Servo_and_Sensor ?
                    (Sensor_Z_Axis_HOME && OnlyServoCheck) : OnlyServoCheck;
            }
            else if (GetMotionParam().IsCylinderType(ePortAxis))
            {
                return CylinderCtrl_GetPosSensorOn(ePortAxis, CylCtrlList.BWD);
            }
            else if (GetMotionParam().IsInverterType(ePortAxis))
            {
                if (ePortAxis == PortAxis.Buffer_LP_Z)
                    return Sensor_LP_Z_POS1;
                else if (ePortAxis == PortAxis.Buffer_OP_Z)
                    return Sensor_OP_Z_POS1;
                else
                    return false;
            }
            else
                return true;
        }
        
        /// <summary>
        /// T 축 관련 센서 상태
        /// Servo And Sensor : 서보의 위치와 센서 On 여부로 판단
        /// </summary>
        /// <returns></returns>
        private bool IsTAxisPos_0_Deg()
        {
            float Target = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree0_Pos);
            bool OnlyServoCheck = !IsPortAxisBusy(PortAxis.Shuttle_T) && Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(PortAxis.Shuttle_T)) && IsAxisPositionInside(PortAxis.Shuttle_T, Target, 0.1f);

            if (GetMotionParam().IsServoType(PortAxis.Shuttle_T))
                return GetMotionParam().GetPositionCheckType(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree0_Pos) == PositionCheckType.Servo_and_Sensor ?
                    (Sensor_T_Axis_HOME && OnlyServoCheck) : OnlyServoCheck;
            else
                return true;
        }
        private bool IsTAxisPos_180_Deg()
        {
            float Target = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree180_Pos);
            bool OnlyServoCheck = !IsPortAxisBusy(PortAxis.Shuttle_T) && Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(PortAxis.Shuttle_T)) && IsAxisPositionInside(PortAxis.Shuttle_T, Target, 0.1f);

            if (GetMotionParam().IsServoType(PortAxis.Shuttle_T))
                return GetMotionParam().GetPositionCheckType(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree180_Pos) == PositionCheckType.Servo_and_Sensor ?
                    (Sensor_T_Axis_180DegSensor && OnlyServoCheck) : OnlyServoCheck;
            else
                return true;
        }


        /// <summary>
        /// Teaching X Motion에 대한 동작 완료 정의
        /// Light Curtain or Hoist 감지 시 정지
        /// Target이 OP 위치면서 Fork 감지 시 정지
        /// 센서 and 위치 조합인 경우 센서 On 이면서 위치 도달 시 Done 판단
        /// 위치만으로 판단하는 경우 위치 도달 시 Done 판단
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eMoveStep"></param>
        /// <param name="bFromAutoStep"></param>
        /// <returns></returns>
        public bool X_Axis_MotionAndDone(PortAxis ePortAxis, Teaching_X_Pos eMoveStep, bool bFromAutoStep = true)
        {
            if (ePortAxis != PortAxis.Shuttle_X && ePortAxis != PortAxis.Buffer_LP_X && ePortAxis != PortAxis.Buffer_OP_X)
                return false;

            bool bSensorState = false;

            if (eMoveStep == Teaching_X_Pos.OP_Pos)
                bSensorState = IsXAxisPos_OP();
            else if(eMoveStep == Teaching_X_Pos.MGV_LP_Pos || eMoveStep == Teaching_X_Pos.Equip_LP_Pos)
                bSensorState = IsXAxisPos_LP();
            else if (eMoveStep == Teaching_X_Pos.Wait_Pos)
                bSensorState = IsXAxisPos_Wait();

            if (GetMotionParam().IsServoType(ePortAxis))
            {
                //if (GetMotionParam().GetPositionCheckType(ePortAxis, (int)eMoveStep) == PositionCheckType.Servo)
                //    bSensorState = true;

                if (!Is_LightCurtain_or_Hoist_SensorCheck() && !(eMoveStep == Teaching_X_Pos.OP_Pos && Sensor_OP_Fork_Detect))
                {
                    float Target = 0;
                    if (eMoveStep == Teaching_X_Pos.OP_Pos)
                        Target = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_X_Pos.OP_Pos);
                    else if (eMoveStep == Teaching_X_Pos.Wait_Pos)
                        Target = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_X_Pos.Wait_Pos);
                    else if (eMoveStep == Teaching_X_Pos.MGV_LP_Pos || eMoveStep == Teaching_X_Pos.Equip_LP_Pos)
                        Target = GetMotionParam().GetTeachingPos(ePortAxis, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos));

                    if (!IsPortAxisBusy(ePortAxis) && 
                        bSensorState &&
                        Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(ePortAxis)) && 
                        IsAxisPositionInside(ePortAxis, Target, 0.1f))
                    {
                        return true;
                    }
                    else if (ServoCtrl_GetAxisMoveEnable(ePortAxis, bFromAutoStep) && Is_LightCurtainRelease() &&
                            (!Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(ePortAxis)) || !IsAxisPositionInside(ePortAxis, Target, 0.1f)))
                    {
                        Interlock_X_Axis_Move_To_TeachingPos(ePortAxis, eMoveStep, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event);
                    }
                }
                else
                {
                    if (!bFromAutoStep)
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.SafetySensorDetectError, $"Light Curtain:{Sensor_LightCurtain}, Hoist Sensor:{Sensor_LP_Hoist_Detect}, Fork Sensor:{Sensor_OP_Fork_Detect}");
                    Interlock_AxisStop(ePortAxis, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event); //Light Curtain이 인식된 경우 동작 정지
                }
            }
            else
                return true;

            return false;
        }

        /// <summary>
        /// Teaching Y Motion에 대한 동작 정의
        /// Light Curtain or Hoist 감지 시 정지 (실린더라 사실 상 정지 불가)
        /// FWD, BWD 센서가 들어오는 경우 Done 판단 (Diebank 한정 사용, 스토커 기구 부 문제로 인한 Y축 추가)
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eMoveStep"></param>
        /// <param name="bFromAutoStep"></param>
        /// <returns></returns>
        public bool Y_Axis_MotionAndDone(PortAxis ePortAxis, Teaching_Y_Pos eMoveStep, bool bFromAutoStep = true)
        {
            if (ePortAxis != PortAxis.Buffer_LP_Y && ePortAxis != PortAxis.Buffer_OP_Y)
                return false;

            bool bSensorState = false;

            if (eMoveStep == Teaching_Y_Pos.FWD_Pos)
                bSensorState = IsYAxisPos_FWD(ePortAxis);
            else if (eMoveStep == Teaching_Y_Pos.BWD_Pos)
                bSensorState = IsYAxisPos_BWD(ePortAxis);

            if (GetMotionParam().IsCylinderType(ePortAxis))
            {
                if (!Is_LightCurtain_or_Hoist_SensorCheck())
                {
                    if (!IsPortAxisBusy(ePortAxis) && bSensorState)
                    {
                        return true; //Next Step
                    }
                    else if (eMoveStep == Teaching_Y_Pos.FWD_Pos || eMoveStep == Teaching_Y_Pos.BWD_Pos)
                    {
                        if (!bSensorState && Is_LightCurtainRelease()) //최종 Up 위치가 아니라면
                        {
                            if (!CylinderCtrl_GetPosSensorOn(ePortAxis, eMoveStep == Teaching_Y_Pos.FWD_Pos ? CylCtrlList.FWD : CylCtrlList.BWD))
                            {
                                //실린더1의 전진 완료 위치가 아니라면
                                Interlock_SetCylinderMove(ePortAxis, CylCtrlList.BWD, eMoveStep == Teaching_Y_Pos.FWD_Pos ? false : true, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event);
                                Interlock_SetCylinderMove(ePortAxis, CylCtrlList.FWD, eMoveStep == Teaching_Y_Pos.FWD_Pos ? true : false, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event); //실린더1 전진 제어
                            }
                        }
                        else
                            Interlock_CylinderMotionStop(ePortAxis, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event); //최종 위치에 도달한 경우 Flag Off
                    }
                }
                else
                {
                    if (!bFromAutoStep)
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.SafetySensorDetectError, $"Light Curtain:{Sensor_LightCurtain}, Hoist Sensor:{Sensor_LP_Hoist_Detect}");

                    Interlock_CylinderMotionStop(ePortAxis, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event); //Light Curtain이 인식된 경우 동작 정지
                }
            }
            else
                return true;

            return false;
        }

        /// <summary>
        /// Teaching Z Motion에 대한 동작 정의
        /// Light Curtain or Hoist 감지 시 정지
        /// Z 축은 서보, 실린더, 인버터 타입으로 사용 가능
        /// 서보의 경우 센서 and 위치 조합인 경우 센서 On 이면서 위치 도달 시 Done 판단
        /// 서보의 경우 위치만으로 판단하는 경우 위치 도달 시 Done 판단
        /// 실린더의 경우 FWD, BWD 센서가 들어오는 경우 Done 판단
        /// 인버터의 경우 POS1, POS2 센서가 들어오는 경우 Done 판단
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eMoveStep"></param>
        /// <param name="bFromAutoStep"></param>
        /// <returns></returns>
        public bool Z_Axis_MotionAndDone(PortAxis ePortAxis, Teaching_Z_Pos eMoveStep, bool bFromAutoStep = true)
        {
            if (ePortAxis != PortAxis.Shuttle_Z && ePortAxis != PortAxis.Buffer_LP_Z && ePortAxis != PortAxis.Buffer_OP_Z)
                return false;

            bool bSensorState = false;

            //Axis Type에 따라 Sensor 값을 가져옴
            if (eMoveStep == Teaching_Z_Pos.Up_Pos)
                bSensorState = IsZAxisPos_UP(ePortAxis);
            else if (eMoveStep == Teaching_Z_Pos.Down_Pos)
                bSensorState = IsZAxisPos_DOWN(ePortAxis);

            if (GetMotionParam().IsServoType(ePortAxis))
            {
                if (!Is_LightCurtain_or_Hoist_SensorCheck())
                {
                    float Target = 0;
                    if (eMoveStep == Teaching_Z_Pos.Up_Pos)
                        Target = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_Z_Pos.Up_Pos);
                    else if (eMoveStep == Teaching_Z_Pos.Down_Pos)
                        Target = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_Z_Pos.Down_Pos);

                    if (!IsPortAxisBusy(ePortAxis) && 
                        bSensorState &&
                        Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(ePortAxis)) && 
                        IsAxisPositionInside(ePortAxis, Target, 0.1f))
                    {
                        return true;
                    }
                    else if (ServoCtrl_GetAxisMoveEnable(ePortAxis, bFromAutoStep) && Is_LightCurtainRelease() &&
                            (!Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(ePortAxis)) || !IsAxisPositionInside(ePortAxis, Target, 0.1f)))
                    {
                        Interlock_Z_Axis_Move_To_TeachingPos(ePortAxis, eMoveStep, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event);
                    }
                }
                else
                {
                    if (!bFromAutoStep)
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.SafetySensorDetectError, $"Light Curtain:{Sensor_LightCurtain}, Hoist Sensor:{Sensor_LP_Hoist_Detect}");
                    Interlock_AxisStop(ePortAxis, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event); //Light Curtain이 인식된 경우 동작 정지
                }
            }
            else if (GetMotionParam().IsCylinderType(ePortAxis))
            {
                if (!Is_LightCurtain_or_Hoist_SensorCheck())
                {
                    if (!IsPortAxisBusy(ePortAxis) && bSensorState)
                    {
                        return true; //Next Step
                    }
                    else if (eMoveStep == Teaching_Z_Pos.Up_Pos || eMoveStep == Teaching_Z_Pos.Down_Pos)
                    {
                        if (!bSensorState && Is_LightCurtainRelease()) //최종 Up 위치가 아니라면
                        {
                            if (!CylinderCtrl_GetPosSensorOn(ePortAxis, eMoveStep == Teaching_Z_Pos.Up_Pos ? CylCtrlList.FWD : CylCtrlList.BWD))
                            {
                                //실린더1의 전진 완료 위치가 아니라면
                                Interlock_SetCylinderMove(ePortAxis, CylCtrlList.BWD, eMoveStep == Teaching_Z_Pos.Up_Pos ? false : true, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event);
                                Interlock_SetCylinderMove(ePortAxis, CylCtrlList.FWD, eMoveStep == Teaching_Z_Pos.Up_Pos ? true : false, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event); //실린더1 전진 제어
                            }
                        }
                        else
                            Interlock_CylinderMotionStop(ePortAxis, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event); //최종 위치에 도달한 경우 Flag Off
                    }
                }
                else
                {
                    if (!bFromAutoStep)
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.SafetySensorDetectError, $"Light Curtain:{Sensor_LightCurtain}, Hoist Sensor:{Sensor_LP_Hoist_Detect}");

                    Interlock_CylinderMotionStop(ePortAxis, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event); //Light Curtain이 인식된 경우 동작 정지
                }
            }
            else if (GetMotionParam().IsInverterType(ePortAxis))
            {
                if (!Is_LightCurtain_or_Hoist_SensorCheck())
                {
                    if (!IsPortAxisBusy(ePortAxis) && bSensorState)
                    {
                        return true; //Next Step
                    }
                    else if (eMoveStep == Teaching_Z_Pos.Up_Pos || eMoveStep == Teaching_Z_Pos.Down_Pos)
                    {
                        if (!bSensorState && Is_LightCurtainRelease()) //최종 Up 위치가 아니라면
                        {
                            if (GetMotionParam().GetShuttleCtrl_InvParam(ePortAxis).InvCtrlMode == InvCtrlMode.IOControl)
                                Interlock_SetInverterMove(ePortAxis, eMoveStep == Teaching_Z_Pos.Up_Pos ? InvCtrlType.HighSpeedFWD : InvCtrlType.HighSpeedBWD, true, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event);
                            else
                                Interlock_SetInverterMove(ePortAxis, eMoveStep == Teaching_Z_Pos.Up_Pos ? InvCtrlType.FreqFWD : InvCtrlType.FreqBWD, true, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event);
                        }
                        else
                            Interlock_InverterMotionStop(ePortAxis, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event); //최종 위치에 도달한 경우 Flag Off
                    }
                }
                else
                {
                    if (!bFromAutoStep)
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.SafetySensorDetectError, $"Light Curtain:{Sensor_LightCurtain}, Hoist Sensor:{Sensor_LP_Hoist_Detect}");

                    Interlock_InverterMotionStop(ePortAxis, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event); //Light Curtain이 인식된 경우 동작 정지
                }
            }
            else
                return true;

            return false;
        }

        /// <summary>
        /// Teaching T Motion에 대한 동작 완료 정의
        /// Light Curtain or Hoist 감지 시 정지
        /// Paramter에서 Crash ID가 입력된 경우 회전 전에 지정 ID의 포트 회전 상태 확인 후 동작(충돌 방지, 실리콘 박스 RDL OVEN에서 IN, OUT Port 회전시 간섭)
        /// 센서 and 위치 조합인 경우 센서 On 이면서 위치 도달 시 Done 판단
        /// 위치만으로 판단하는 경우 위치 도달 시 Done 판단
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eMoveStep"></param>
        /// <param name="bFromAutoStep"></param>
        /// <returns></returns>
        public bool T_Axis_MotionAndDone(PortAxis ePortAxis, Teaching_T_Pos eMoveStep, bool bFromAutoStep = true)
        {
            if (ePortAxis != PortAxis.Shuttle_T && ePortAxis != PortAxis.Buffer_LP_T && ePortAxis != PortAxis.Buffer_OP_T)
                return false;

            bool bSensorState = false;

            if (eMoveStep == Teaching_T_Pos.Degree0_Pos)
                bSensorState = IsTAxisPos_0_Deg();
            else if (eMoveStep == Teaching_T_Pos.Degree180_Pos)
                bSensorState = IsTAxisPos_180_Deg();

            if (GetMotionParam().IsServoType(ePortAxis))
            {
                string CrashCheckPortID = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).CrashCheckID;
                bool bCrashInterlock = false;

                if (Master.m_Ports.ContainsKey(CrashCheckPortID))
                {
                    var port = Master.m_Ports[CrashCheckPortID];

                    if ((port.ServoCtrl_GetCurrentPosition(ePortAxis) != 0 && port.ServoCtrl_GetCurrentPosition(ePortAxis) != 180) && port.ServoCtrl_GetBusy(ePortAxis))
                        bCrashInterlock = true;
                }

                if (!Is_LightCurtain_or_Hoist_SensorCheck() && !bCrashInterlock) //Turn
                {
                    float Target = 0;
                    if (eMoveStep == Teaching_T_Pos.Degree0_Pos)
                        Target = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_T_Pos.Degree0_Pos);
                    else if (eMoveStep == Teaching_T_Pos.Degree180_Pos)
                        Target = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_T_Pos.Degree180_Pos);

                    if (!IsPortAxisBusy(ePortAxis) && 
                        bSensorState &&
                        Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(ePortAxis)) &&
                        IsAxisPositionInside(ePortAxis, Target, 0.1f))
                    {
                        return true;
                    }
                    else if (ServoCtrl_GetAxisMoveEnable(ePortAxis, bFromAutoStep) && Is_LightCurtainRelease() &&
                            (!Interface.Math.Compare.Equal(Target, ServoCtrl_GetTargetPosition(ePortAxis)) || !IsAxisPositionInside(ePortAxis, Target, 0.1f)))
                    {
                        Interlock_T_Axis_Move_To_TeachingPos(ePortAxis, eMoveStep, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event);
                    }
                }
                else
                {
                    if (!bFromAutoStep)
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.SafetySensorDetectError, $"Light Curtain:{Sensor_LightCurtain}, Hoist Sensor:{Sensor_LP_Hoist_Detect}, Crash Interlock:{bCrashInterlock}");

                    if((bCrashInterlock && Convert.ToInt32(GetParam().ID) < Convert.ToInt32(CrashCheckPortID)) ||
                        Is_LightCurtain_or_Hoist_SensorCheck())
                        Interlock_AxisStop(ePortAxis, bFromAutoStep, bFromAutoStep ? InterlockFrom.ApplicationLoop : InterlockFrom.UI_Event);
                }
            }
            else
                return true;

            return false;
        }

        /// <summary>
        /// 공정중인 Auto Step Number를 가져옴
        /// </summary>
        /// <returns></returns>
        public int Get_OP_AutoControlStep()
        {
            if (IsShuttleControlPort())
            {
                if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                {
                    return Auto_2BP_Get_OP_StepNum();
                }
                else if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                {
                    return Auto_1BP_Get_OP_StepNum();
                }
                else
                    return -1;
            }
            else if (GetParam().ePortType == PortType.Conveyor_AGV)
            {
                return Auto_CV_Get_OP_StepNum();
            }
            else if (GetParam().ePortType == PortType.Conveyor_OMRON)
            {
                return Auto_CV_Get_OP_DIEBANK_StepNum();
            }
            else
                return -1;
        }
        public int Get_BP_AutoControlStep()
        {
            if (IsShuttleControlPort())
            {
                if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                {
                    return Auto_2BP_Get_BP_StepNum();
                }
                else if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                {
                    return Auto_1BP_Get_BP_StepNum();
                }
                else
                    return -1;
            }
            else if (GetParam().ePortType == PortType.Conveyor_AGV)
            {
                if(GetMotionParam().IsBPCVUsed())
                    return Auto_CV_Get_BP_StepNum();
                else
                    return -1;
            }
            else if (GetParam().ePortType == PortType.Conveyor_OMRON)
            {
                return -1;
            }
            else
                return -1;
        }
        public int Get_LP_AutoControlStep()
        {
            if (IsShuttleControlPort())
            {
                if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                {
                    return Auto_2BP_Get_LP_StepNum();
                }
                else if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                {
                    return Auto_1BP_Get_BP_StepNum();
                }
                else
                    return -1;
            }
            else if (GetParam().ePortType == PortType.Conveyor_AGV)
            {
                return Auto_CV_Get_LP_StepNum();
            }
            else if (GetParam().ePortType == PortType.Conveyor_OMRON)
            {
                return Auto_CV_Get_LP_DIEBANK_StepNum();
            }
            else
                return -1;
        }


        /// <summary>
        /// 공정중인 Auto Step의 텍스트를 가져옴
        /// </summary>
        /// <returns></returns>
        public string Get_OP_AutoControlStepToStr()
        {
            if (IsShuttleControlPort())
            {
                if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                {
                    return Auto_2BP_Get_OP_StepStr();
                }
                else if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                {
                    return Auto_1BP_Get_OP_StepStr();
                }
                else
                    return string.Empty;
            }
            else if (GetParam().ePortType == PortType.Conveyor_AGV)
            {
                return Auto_CV_Get_OP_StepStr();
            }
            else if (GetParam().ePortType == PortType.Conveyor_OMRON)
            {
                return Auto_CV_Get_OP_DIEBANK_StepStr();
            }
            else
                return string.Empty;
        }
        public string Get_BP_AutoControlStepToStr()
        {
            if (IsShuttleControlPort())
            {
                if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                {
                    return Auto_2BP_Get_BP_StepStr();
                }
                else if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                {
                    return Auto_1BP_Get_BP_StepStr();
                }
                else
                    return string.Empty;
            }
            else if (GetParam().ePortType == PortType.Conveyor_AGV)
            {
                if (GetMotionParam().IsBPCVUsed())
                    return Auto_CV_Get_BP_StepStr();
                else
                    return string.Empty;
            }
            else if(GetParam().ePortType == PortType.Conveyor_OMRON)
            {
                return string.Empty;
            }
            else
                return string.Empty;
        }
        public string Get_LP_AutoControlStepToStr()
        {
            if (IsShuttleControlPort())
            {
                if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                {
                    return Auto_2BP_Get_LP_StepStr();
                }
                else if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                {
                    return string.Empty;
                }
                else
                    return string.Empty;
            }
            else if (GetParam().ePortType == PortType.Conveyor_AGV)
            {
                return Auto_CV_Get_LP_StepStr();
            }
            else if (GetParam().ePortType == PortType.Conveyor_OMRON)
            {
                return Auto_CV_Get_LP_DIEBANK_StepStr();
            }
            else
                return string.Empty;
        }
    }
}
