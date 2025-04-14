using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.Safty;
using Master.Equipment.CIM;
using Master.Equipment.RackMaster;

namespace Master
{
    /// <summary>
    /// Master -> CIM Bit Memory Map에 사용되는 변수 기재
    /// </summary>
    static partial class Master
    {

        static public EStop mHPOutSide_EStop = new EStop();
        static public EStop mHPInnerEscape_EStop = new EStop();
        static public EStop mOPOutSide_EStop = new EStop();
        static public EStop mOPInnerEscape_EStop = new EStop();

        static public EStop mPortHandyTouch_EStop = new EStop();
        static public EStop mHPHandyTouch_EStop = new EStop();
        static public EStop mOPHandyTouch_EStop = new EStop();

        static public EStop mDieBankInnerEMO_EStop = new EStop();
        static public EStop mDieBankInnerEMO_EStop2 = new EStop();
        static public EStop mDieBankInnerEMO_EStop3 = new EStop();
        static public EStop mDieBankInnerEMO_EStop4 = new EStop();

        static bool m_bHP_Outside_EMO_Status = false;
        static bool m_bHP_InnerEscape_EMO_Status = false;
        static bool m_bOP_Outside_EMO_Status = false;
        static bool m_bOP_InnerEscape_EMO_Status = false;

        static bool m_bMainMC = false;
        static bool m_bRMPower = false;
        static bool m_bMCULFault = false;

        static bool m_bHPDoorOpen = false;
        static bool m_bOPDoorOpen = false;
        static bool m_bMaintDoorOpen = false;
        static bool m_bMaintDoorOpen2 = false;

        static bool m_bHPAutoManualSelectKey = false;
        static bool m_bOPAutoManualSelectKey = false;

        static bool m_bPortHandyTouchEMOStatus = false;
        static bool m_bPortHandyTouchDeadManStatus = false;
        static bool m_bPortHandyTouchMode1Status = false;
        static bool m_bPortHandyTouchMode2Status = false;

        static bool m_bHPHandyTouchEMOStatus = false;
        static bool m_bHPHandyTouchDeadManStatus = false;
        static bool m_bHPHandyTouchMode1Status = false;
        static bool m_bHPHandyTouchMode2Status = false;

        static bool m_bOPHandyTouchEMOStatus = false;
        static bool m_bOPHandyTouchDeadManStatus = false;
        static bool m_bOPHandyTouchMode1Status = false;
        static bool m_bOPHandyTouchMode2Status = false;

        static bool m_bCPSRunStatus = false;
        static bool m_bCPSErrorStatus = false;
        static bool m_bCPSPowerOnEnableLamp = false;
        static bool m_bCPSPowerOnReqLamp = false;
        static bool m_bCPSErrorResetReqLamp = false;
        static bool m_bCPSErrorLamp = false;

        static bool m_bDieBank_Inner_EMO_Status = false;
        static bool m_bDieBank_Inner_EMO_Status2 = false;
        static bool m_bDieBank_Inner_EMO_Status3 = false;
        static bool m_bDieBank_Inner_EMO_Status4 = false;

        static bool m_bOxygen_Saturation_Warning_Status = false;

        static bool m_bSTKBodyHPEMO = false;
        static bool m_bSTKBodyOPEMO = false;
        static bool m_bSTKCIMMode = false;

        /// <summary>
        /// [Master -> CIM] HP Outside EMO 상태
        /// Master Sensor Map에 연동 된 HP_Outside_EMO Input에 따라 제어 됨
        /// </summary>
        static public bool Sensor_HP_Outside_EMO
        {
            get { return m_bHP_Outside_EMO_Status; }
            set
            {
                m_bHP_Outside_EMO_Status = value;
                if (m_bHP_Outside_EMO_Status)
                    mHPOutSide_EStop.PushEStop();
                else
                    mHPOutSide_EStop.ReleaseEStop();

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.HP_EMO_Pushing, m_bHP_Outside_EMO_Status);
            }
        }

        /// <summary>
        /// [Master -> CIM] HP Inner EMO 상태
        /// Master Sensor Map에 연동 된 HP_Inside_EMO 에 따라 제어 됨
        /// </summary>
        static public bool Sensor_HP_InnerEscape_EMO
        {
            get { return m_bHP_InnerEscape_EMO_Status; }
            set
            {
                m_bHP_InnerEscape_EMO_Status = value;
                if (m_bHP_InnerEscape_EMO_Status)
                    mHPInnerEscape_EStop.PushEStop();
                else
                    mHPInnerEscape_EStop.ReleaseEStop();

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.HP_EMO_Escape_Status, m_bHP_InnerEscape_EMO_Status);
            }
        }

        /// <summary>
        /// [Master -> CIM] OP Outside EMO 상태
        /// Master Sensor Map에 연동 된 OP_Outside_EMO Input에 따라 제어 됨
        /// </summary>
        static public bool Sensor_OP_Outside_EMO
        {
            get { return m_bOP_Outside_EMO_Status; }
            set
            {
                m_bOP_Outside_EMO_Status = value;
                if (m_bOP_Outside_EMO_Status)
                    mOPOutSide_EStop.PushEStop();
                else
                    mOPOutSide_EStop.ReleaseEStop();

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.OP_EMO_Pushing, m_bOP_Outside_EMO_Status);
            }
        }

        /// <summary>
        /// [Master -> CIM] OP Inner EMO 상태
        /// Master Sensor Map에 연동 된 OP_Inside_EMO 에 따라 제어 됨
        /// </summary>
        static public bool Sensor_OP_InnerEscape_EMO
        {
            get { return m_bOP_InnerEscape_EMO_Status; }
            set
            {
                m_bOP_InnerEscape_EMO_Status = value;
                if (m_bOP_InnerEscape_EMO_Status)
                    mOPInnerEscape_EStop.PushEStop();
                else
                    mOPInnerEscape_EStop.ReleaseEStop();

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.OP_EMO_Escape_Status, m_bOP_InnerEscape_EMO_Status);
            }
        }

        /// <summary>
        /// [Master -> CIM] MainMC 상태
        /// Master Sensor Map에 연동 된 Main_MC_On 에 따라 제어 됨
        /// 실제 센서 값은 반영 되지만 동작은 없음(구현 X)
        /// </summary>
        static public bool Sensor_MainMC
        {
            get { return m_bMainMC; }
            set
            {
                m_bMainMC = value;
            }
        }

        /// <summary>
        /// [Master -> CIM] RM Power 상태
        /// Master Sensor Map에 연동 된 RM_Power 에 따라 제어 됨
        /// 실제 센서 값은 반영 되지만 동작은 없음(구현 X)
        /// </summary>
        static public bool Sensor_RMPower
        {
            get { return m_bRMPower; }
            set
            {
                m_bRMPower = value;
            }
        }

        /// <summary>
        /// [Master -> CIM] RM Power 상태
        /// Master Sensor Map에 연동 된 MCUL_Fault 에 따라 제어 됨
        /// 실제 센서 값은 반영 되고 일부 UI에 상태는 표시하지만 동작은 없음(구현 X)
        /// </summary>
        static public bool Sensor_MCULFault
        {
            get { return m_bMCULFault; }
            set
            {
                m_bMCULFault = value;
            }
        }

        /// <summary>
        /// [Master -> CIM] HP Door Open 상태
        /// Master Sensor Map에 연동 된 HP_Door_Open 에 따라 제어 됨
        /// 알람 처리 및 인터락에 활용
        /// </summary>
        static public bool Sensor_HPDoorOpen
        {
            get { return m_bHPDoorOpen; }
            set
            {
                m_bHPDoorOpen = value;
                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.HP_DoorOpen, m_bHPDoorOpen);
            }
        }

        /// <summary>
        /// [Master -> CIM] OP Door Open 상태
        /// Master Sensor Map에 연동 된 OP_Door_Open 에 따라 제어 됨
        /// 알람 처리 및 인터락에 활용
        /// </summary>
        static public bool Sensor_OPDoorOpen
        {
            get { return m_bOPDoorOpen; }
            set
            {
                m_bOPDoorOpen = value;
                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.OP_DoorOpen, m_bOPDoorOpen);
            }
        }

        /// <summary>
        /// [Master -> CIM] Maint Door Open 상태
        /// Master Sensor Map에 연동 된 Maint_Door_Open 에 따라 제어 됨
        /// 알람 처리에 활용
        /// </summary>
        static public bool Sensor_MaintDoorOpen
        {
            get { return m_bMaintDoorOpen; }
            set
            {
                m_bMaintDoorOpen = value;
            }
        }

        /// <summary>
        /// [Master -> CIM] Maint Door Open 상태
        /// Master Sensor Map에 연동 된 Maint_Door_Open2 에 따라 제어 됨
        /// 알람 처리에 활용
        /// </summary>
        static public bool Sensor_MaintDoorOpen2
        {
            get { return m_bMaintDoorOpen2; }
            set
            {
                m_bMaintDoorOpen2 = value;
            }
        }

        /// <summary>
        /// [Master -> CIM] HP Auto Key 상태
        /// Master Sensor Map에 연동 된 HP_AutoManual_Select_Key 에 따라 제어 됨
        /// 알람 처리, 인터락에 활용
        /// </summary>
        static public bool Sensor_HPAutoKey
        {
            get { return m_bHPAutoManualSelectKey; }
            set
            {
                m_bHPAutoManualSelectKey = value;
                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.HP_MasterKey_AutoMode_Status, m_bHPAutoManualSelectKey);
            }
        }

        /// <summary>
        /// [Master -> CIM] OP Auto Key 상태
        /// Master Sensor Map에 연동 된 OP_AutoManual_Select_Key 에 따라 제어 됨
        /// 미사용, 기능만 구현
        /// </summary>
        static public bool Sensor_OPAutoKey
        {
            get { return m_bOPAutoManualSelectKey; }
            set
            {
                m_bOPAutoManualSelectKey = value;
            }
        }

        /// <summary>
        /// Port DTP 상태
        /// Master Sensor Map에 연동 된 Port_DTP_ 에 따라 제어 됨
        /// Screen 제어 및 제어 인터락, 알람 등에 활용
        /// </summary>
        static public bool Sensor_PortHandyTouchEMO
        {
            get { return m_bPortHandyTouchEMOStatus; }
            set
            {
                m_bPortHandyTouchEMOStatus = value;
            }
        }
        static public bool Sensor_PortHandyTouchDeadMan
        {
            get { return m_bPortHandyTouchDeadManStatus; }
            set
            {
                m_bPortHandyTouchDeadManStatus = value;
            }
        }
        static public bool Sensor_PortHandyTouchMode1
        {
            get { return m_bPortHandyTouchMode1Status; }
            set
            {
                m_bPortHandyTouchMode1Status = value;
            }
        }
        static public bool Sensor_PortHandyTouchMode2
        {
            get { return m_bPortHandyTouchMode2Status; }
            set
            {
                m_bPortHandyTouchMode2Status = value;
            }
        }
        static public bool IsPortHandyTouchConnection
        {
            get { return Sensor_PortHandyTouchMode1 || Sensor_PortHandyTouchMode2; }
        }
        static public bool IsPortHandyTouchMainMonLock
        {
            get { return !Sensor_PortHandyTouchMode1 && Sensor_PortHandyTouchMode2; }
        }
        static public bool IsPortHandyTouchDeadManControl
        {
            get { return Sensor_PortHandyTouchDeadMan && Sensor_PortHandyTouchMode2; }
        }
        static public bool IsPortHandyTouchDeadManControlLock
        {
            get { return !Sensor_PortHandyTouchDeadMan && Sensor_PortHandyTouchMode2; }
        }


        /// <summary>
        /// HP DTP 상태
        /// Master Sensor Map에 연동 된 HP_DTP_ 에 따라 제어 됨
        /// Screen 제어 및 제어 인터락, 알람 등에 활용
        /// </summary>
        static public bool Sensor_HPHandyTouchEMO
        {
            get { return m_bHPHandyTouchEMOStatus; }
            set
            {
                m_bHPHandyTouchEMOStatus = value;
            }
        }
        static public bool Sensor_HPHandyTouchDeadMan
        {
            get { return m_bHPHandyTouchDeadManStatus; }
            set
            {
                m_bHPHandyTouchDeadManStatus = value;
            }
        }
        static public bool Sensor_HPHandyTouchMode1
        {
            get { return m_bHPHandyTouchMode1Status; }
            set
            {
                m_bHPHandyTouchMode1Status = value;
            }
        }
        static public bool Sensor_HPHandyTouchMode2
        {
            get { return m_bHPHandyTouchMode2Status; }
            set
            {
                m_bHPHandyTouchMode2Status = value;
            }
        }
        static public bool IsHPHandyTouchConnection
        {
            get { return Sensor_HPHandyTouchMode1 || Sensor_HPHandyTouchMode2; }
        }
        static public bool IsHPHandyTouchMainMonLock
        {
            get { return !Sensor_HPHandyTouchMode1 && Sensor_HPHandyTouchMode2; }
        }
        static public bool IsHPHandyTouchDeadManControl
        {
            get { return Sensor_HPHandyTouchDeadMan && Sensor_HPHandyTouchMode2; }
        }
        static public bool IsHPHandyTouchDeadManControlLock
        {
            get { return !Sensor_HPHandyTouchDeadMan && Sensor_HPHandyTouchMode2; }
        }

        /// <summary>
        /// OP DTP 상태
        /// Master Sensor Map에 연동 된 OP_DTP_ 에 따라 제어 됨
        /// Screen 제어 및 제어 인터락, 알람 등에 활용
        /// </summary>
        static public bool Sensor_OPHandyTouchEMO
        {
            get { return m_bOPHandyTouchEMOStatus; }
            set
            {
                m_bOPHandyTouchEMOStatus = value;
            }
        }
        static public bool Sensor_OPHandyTouchDeadMan
        {
            get { return m_bOPHandyTouchDeadManStatus; }
            set
            {
                m_bOPHandyTouchDeadManStatus = value;
            }
        }
        static public bool Sensor_OPHandyTouchMode1
        {
            get { return m_bOPHandyTouchMode1Status; }
            set
            {
                m_bOPHandyTouchMode1Status = value;
            }
        }
        static public bool Sensor_OPHandyTouchMode2
        {
            get { return m_bOPHandyTouchMode2Status; }
            set
            {
                m_bOPHandyTouchMode2Status = value;
            }
        }
        static public bool IsOPHandyTouchConnection
        {
            get { return Sensor_OPHandyTouchMode1 || Sensor_OPHandyTouchMode2; }
        }
        static public bool IsOPHandyTouchMainMonLock
        {
            get { return !Sensor_OPHandyTouchMode1 && Sensor_OPHandyTouchMode2; }
        }
        static public bool IsOPHandyTouchDeadManControl
        {
            get { return Sensor_OPHandyTouchDeadMan && Sensor_OPHandyTouchMode2; }
        }
        static public bool IsOPHandyTouchDeadManControlLock
        {
            get { return !Sensor_OPHandyTouchDeadMan && Sensor_OPHandyTouchMode2; }
        }


        /// <summary>
        /// GOT Dead Man Switch를 통한 제어 잠금 상태 (GOT는 DeadMan을 누른 후 제어를 진행 해야 함)
        /// </summary>
        /// <returns></returns>
        static public bool Is_DeadMan_Status_ControlLock
        {
            get { return IsHPHandyTouchDeadManControlLock || IsOPHandyTouchDeadManControlLock || IsPortHandyTouchDeadManControlLock; }
        }

        /// <summary>
        /// GOT Dead Man Switch를 통한 제어 가능 상태 (Dead Man Switch를 누르고 있는 중)
        /// </summary>
        /// <returns></returns>
        static public bool Is_DeadMan_Status_Control
        {
            get { return IsHPHandyTouchDeadManControl || IsOPHandyTouchDeadManControl || IsPortHandyTouchDeadManControl; }
        }

        /// <summary>
        /// [Master -> CIM] CPS Run 상태
        /// Master Sensor Map에 연동 된 CPS_Run 에 따라 제어 됨
        /// 알람 처리에 사용 off 시 알람
        /// </summary>
        static public bool Sensor_CPSRun
        {
            get { return m_bCPSRunStatus; }
            set
            {
                m_bCPSRunStatus = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.CPS_RUN_Status, m_bCPSRunStatus);
            }
        }

        /// <summary>
        /// [Master -> CIM] CPS Error 상태
        /// Master Sensor Map에 연동 된 CPS_Fault 에 따라 제어 됨
        /// 알람 처리에 사용 on 시 알람 (b접)
        /// </summary>
        static public bool Sensor_CPSError
        {
            get { return m_bCPSErrorStatus; }
            set
            {
                m_bCPSErrorStatus = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.CPS_Error_Status, m_bCPSErrorStatus);
            }
        }

        /// <summary>
        /// [Master -> CIM] CPS Power on Enable Lamp 상태
        /// Master Sensor Map에 연동 된 CPS_Power_On_Enable_Lamp 에 따라 제어 됨
        /// UI 상태 표시만 하고 사용 영역 없음
        /// </summary>
        static public bool Sensor_CPSPowerOnEnableLamp
        {
            get { return m_bCPSPowerOnEnableLamp; }
            set
            {
                m_bCPSPowerOnEnableLamp = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.CPS_Power_On_Enable_Lamp, m_bCPSPowerOnEnableLamp);
            }
        }

        /// <summary>
        /// [Master -> CIM] CPS Power on Req Lamp 상태
        /// Master Sensor Map에 연동 된 CPS_Power_On_Req_Lamp 에 따라 제어 됨
        /// UI 상태 표시만 하고 사용 영역 없음
        /// </summary>
        static public bool Sensor_CPSPowerOnReqLamp
        {
            get { return m_bCPSPowerOnReqLamp; }
            set
            {
                m_bCPSPowerOnReqLamp = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.CPS_Power_On_Request_Lamp, m_bCPSPowerOnReqLamp);
            }
        }

        /// <summary>
        /// [Master -> CIM] CPS Error Reset Req Lamp 상태
        /// Master Sensor Map에 연동 된 CPS_Error_Reset_Req_Lamp 에 따라 제어 됨
        /// UI 상태 표시만 하고 사용 영역 없음
        /// </summary>
        static public bool Sensor_CPSErrorResetLamp
        {
            get { return m_bCPSErrorResetReqLamp; }
            set
            {
                m_bCPSErrorResetReqLamp = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.CPS_Error_Reset_Request_Lamp, m_bCPSErrorResetReqLamp);
            }
        }

        /// <summary>
        /// [Master -> CIM] CPS Error Lamp 상태
        /// Master Sensor Map에 연동 된 CPS_Error_Lamp 에 따라 제어 됨
        /// UI 상태 표시만 하고 사용 영역 없음
        /// </summary>
        static public bool Sensor_CPSErrorLamp
        {
            get { return m_bCPSErrorLamp; }
            set
            {
                m_bCPSErrorLamp = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.CPS_Error_Lamp, m_bCPSErrorLamp);
            }
        }

        /// <summary>
        /// DieBank에 사용되는 내부 EMO 상태
        /// Master Sensor Map에 연동 된 Inner_EMO_1 ~ Inner_EMO_4 에 따라 제어 됨
        /// 알람처리에 사용
        /// </summary>
        static public bool Sensor_DieBank_Inner_EMO_Status
        {
            get { return m_bDieBank_Inner_EMO_Status; }
            set
            {
                m_bDieBank_Inner_EMO_Status = value;
                if (m_bDieBank_Inner_EMO_Status)
                    mDieBankInnerEMO_EStop.PushEStop();
                else
                    mDieBankInnerEMO_EStop.ReleaseEStop();
            }
        }
        static public bool Sensor_DieBank_Inner_EMO_Status2
        {
            get { return m_bDieBank_Inner_EMO_Status2; }
            set
            {
                m_bDieBank_Inner_EMO_Status2 = value;
                if (m_bDieBank_Inner_EMO_Status2)
                    mDieBankInnerEMO_EStop2.PushEStop();
                else
                    mDieBankInnerEMO_EStop2.ReleaseEStop();
            }
        }
        static public bool Sensor_DieBank_Inner_EMO_Status3
        {
            get { return m_bDieBank_Inner_EMO_Status3; }
            set
            {
                m_bDieBank_Inner_EMO_Status3 = value;
                if (m_bDieBank_Inner_EMO_Status3)
                    mDieBankInnerEMO_EStop3.PushEStop();
                else
                    mDieBankInnerEMO_EStop3.ReleaseEStop();
            }
        }
        static public bool Sensor_DieBank_Inner_EMO_Status4
        {
            get { return m_bDieBank_Inner_EMO_Status4; }
            set
            {
                m_bDieBank_Inner_EMO_Status4 = value;
                if (m_bDieBank_Inner_EMO_Status4)
                    mDieBankInnerEMO_EStop4.PushEStop();
                else
                    mDieBankInnerEMO_EStop4.ReleaseEStop();
            }
        }

        /// <summary>
        /// NSYNU N2 Stocker에 사용되는 산소 포화도 상태
        /// Master Sensor Map에 연동 된 Oxygen_Saturation_Warning 에 따라 제어 됨
        /// 도어 오픈시 인터락에 사용
        /// </summary>
        static public bool Sensor_Oxygen_Saturation_Warning_Status
        {
            get { return m_bOxygen_Saturation_Warning_Status; }
            set
            {
                m_bOxygen_Saturation_Warning_Status = value;
            }
        }

        /// <summary>
        /// [Master -> CIM] Stocker HP EMO 상태
        /// Master에서 STK의 HP EMO 패킷을 전달 받아 적용
        /// 메모리 맵, UI 상태 표시만 처리
        /// </summary>
        static public bool Sensor_STK_Body_HP_EMO
        {
            get { return m_bSTKBodyHPEMO; }
            set
            {
                m_bSTKBodyHPEMO = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.RM_HP_EMO_Pushing, m_bSTKBodyHPEMO);
            }
        }

        /// <summary>
        /// [Master -> CIM] Stocker OP EMO 상태
        /// Master에서 STK의 OP EMO 패킷을 전달 받아 적용
        /// 메모리 맵, UI 상태 표시만 처리
        /// </summary>
        static public bool Sensor_STK_Body_OP_EMO
        {
            get { return m_bSTKBodyOPEMO; }
            set
            {
                m_bSTKBodyOPEMO = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.RM_OP_EMO_Pushing, m_bSTKBodyOPEMO);
            }
        }

        /// <summary>
        /// [Master -> CIM] Stocker GOT EMO 상태
        /// Master에서 STK의 GOT E Stop Switch 관련 패킷을 전달 받아 적용
        /// UI 상태 표시만 처리
        /// </summary>
        static public bool Sensor_STK_Body_GOT_EMO
        {
            get {

                if (m_RackMasters == null)
                    return false;

                foreach (var rackMaster in m_RackMasters)
                {
                    if (rackMaster.Value.Status_GOTEStopSwitch)
                        return true;
                }

                return false; }
        }

        /// <summary>
        /// [Master -> CIM] Stocker CIM Mode 상태
        /// Master에서 현재 Stocker의 제어 모드를 적용
        /// </summary>
        static public bool Status_STK_CIM_Mode
        {
            set
            {
                m_bSTKCIMMode = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.CIM_Mode_Status, m_bSTKCIMMode);
            }
        }

        static public void MasterToCIMBitMapUpdate()
        {
            //1. Master Alarm 상태에 따른 Memory Map Update
            m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.HP_Handy_Touch_EMO_Pushing, Master.AlarmContains(MasterAlarm.HP_Handy_Touch_E_Stop));
            m_CIM.Set_Master_2_CIM_Bit_Data(CIM.SendBitMapIndex.OP_Handy_Touch_EMO_Pushing, Master.AlarmContains(MasterAlarm.OP_Handy_Touch_E_Stop));

            //2. STK에서 수신된 패킷에 따라 Master -> CIM Bit Map 업데이트
            foreach (var rackMaster in m_RackMasters)
            {
                Sensor_STK_Body_HP_EMO = rackMaster.Value.Status_HPEStopSwitch;
                Sensor_STK_Body_OP_EMO = rackMaster.Value.Status_OPEStopSwitch;
                break;
            }

            //3. STK 제어 모드에 따라 Master -> CIM Bit Map 업데이트
            foreach (var rackMaster in m_RackMasters)
            {
                Status_STK_CIM_Mode = rackMaster.Value.m_eControlMode == RackMaster.ControlMode.CIMMode;
                break;
            }
        }
    }
}
