using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Equipment.RackMaster
{
    /// <summary>
    /// STK -> Master Bit Memory Map에 사용되는 변수 기재
    /// </summary>
    public partial class RackMaster
    {
        bool m_bAutoMode = false;
        bool m_bAutoModeReady = false;
        bool m_bServoOn = false;
        bool m_bServoOnEnable = false;
        bool m_bServoOff = false;
        bool m_bServoOffEnable = false;
        bool m_bManualMode = false;
        bool m_bManualModeEnable = false;
        bool m_bActive = false;
        bool m_bIDLE = false;
        bool m_bCassetteOn = false;
        bool m_bCassettePos = false;
        bool m_bErrorState = false;
        bool m_bHomeDone = false;
        bool m_bHWEStopSwitchState = false;
        bool m_bGOTEStopSwitchState = false;
        bool m_bHPEStopSwitchState = false;
        bool m_bOPEStopSwitchState = false;
        bool m_bGOTMode = false;
        bool m_bTeachingRWEnable = false;
        bool m_bTeachingRWComplete = false;

        bool m_bFromRunACK      = false;
        bool m_bFromComplete    = false;
        bool m_bToRunACK        = false;
        bool m_bToComplete      = false;

        bool m_bMaintMove = false;
        bool m_bMaintMoveRunACK = false;
        bool m_bMaintMoveComplete = false;

        bool m_bFromMoveWait = false;
        bool m_bToMoveWait = false;

        bool m_bAutoTeachingRunACK = false;
        bool m_bAutoTeachingRunningState = false;
        bool m_bAutoTeachingComplete = false;
        bool m_bAutoTeachingErrorComplete = false;

        bool m_bXAxis_Busy = false;
        bool m_bZAxis_Busy = false;
        bool m_bAAxis_Busy = false;
        bool m_bTAxis_Busy = false;

        /// <summary>
        /// STK -> Master로 전송하는 Auto Mode 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_AutoMode
        {
            get { return m_bAutoMode; }
            set
            {
                m_bAutoMode = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Auto Mode 가능 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_AutoModeReady
        {
            get { return m_bAutoModeReady; }
            set
            {
                m_bAutoModeReady = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Servo On 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_ServoOn
        {
            get { return m_bServoOn; }
            set
            {
                m_bServoOn = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Servo On 가능 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_ServoOnEnable
        {
            get { return m_bServoOnEnable; }
            set
            {
                m_bServoOnEnable = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Servo Off 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_ServoOff
        {
            get { return m_bServoOff; }
            set
            {
                m_bServoOff = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Servo Off 가능 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_ServoOffEnable
        {
            get { return m_bServoOffEnable; }
            set
            {
                m_bServoOffEnable = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Manual Mode 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_ManualMode
        {
            get { return m_bManualMode; }
            set
            {
                m_bManualMode = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Manual Mode 가능 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_ManualModeEnable
        {
            get { return m_bManualModeEnable; }
            set
            {
                m_bManualModeEnable = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Active 상태, Active 상태에 따라 마스터 부저음 출력
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_Active
        {
            get { return m_bActive; }
            set
            {
                m_bActive = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 IDLE 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_IDLE
        {
            get { return m_bIDLE; }
            set
            {
                m_bIDLE = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 CST 적재 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_CassetteOn
        {
            get { return m_bCassetteOn; }
            set
            {
                m_bCassetteOn = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 CST 적재 위치 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_CassettePos
        {
            get { return m_bCassettePos; }
            set
            {
                m_bCassettePos = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Error 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_Error
        {
            get { return m_bErrorState; }
            set
            {
                if (m_bErrorState != value)
                    LogMsg.AddRackMasterLog(GetParam().ID, value ? LogMsg.LogLevel.Error : LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterInfo, $"RackMaster Status Error : {value}");
                m_bErrorState = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Home Done 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_HomeDone
        {
            get { return m_bHomeDone; }
            set
            {
                m_bHomeDone = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 HW EStop 눌림 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_HWEStopSwitch
        {
            get { return m_bHWEStopSwitchState; }
            set
            {
                m_bHWEStopSwitchState = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 GOT EStop 눌림 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_GOTEStopSwitch
        {
            get { return m_bGOTEStopSwitchState; }
            set
            {
                m_bGOTEStopSwitchState = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 HP EStop 눌림 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_HPEStopSwitch
        {
            get { return m_bHPEStopSwitchState; }
            set
            {
                m_bHPEStopSwitchState = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 OP EStop 눌림 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_OPEStopSwitch
        {
            get { return m_bOPEStopSwitchState; }
            set
            {
                m_bOPEStopSwitchState = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 GOT 연결 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_GOTMode
        {
            get { return m_bGOTMode; }
            set
            {
                m_bGOTMode = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Teaching Read/Write 가능 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_TeachingRWEnable
        {
            get { return m_bTeachingRWEnable; }
            set
            {
                m_bTeachingRWEnable = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Teaching Read/Write 완료 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_TeachingRWComplete
        {
            get { return m_bTeachingRWComplete; }
            set
            {
                m_bTeachingRWComplete = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 From Run 명령에 대한 ACK 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_FromRunACK
        {
            get { return m_bFromRunACK; }
            set
            {
                m_bFromRunACK = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 From Run 명령에 대한 완료 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_FromRunComplete
        {
            get { return m_bFromComplete; }
            set
            {
                m_bFromComplete = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 To Run 명령에 대한 ACK 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_ToRunACK
        {
            get { return m_bToRunACK; }
            set
            {
                m_bToRunACK = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 To Run 명령에 대한 완료
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_ToRunComplete
        {
            get { return m_bToComplete; }
            set
            {
                m_bToComplete = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Maint Move 동작 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_MaintMove
        {
            get { return m_bMaintMove; }
            set
            {
                m_bMaintMove = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Maint Move 동작 ACK 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_MaintMoveRunAck
        {
            get { return m_bMaintMoveRunACK; }
            set
            {
                m_bMaintMoveRunACK = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Maint Move 동작 완료 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_MaintMoveComplete
        {
            get { return m_bMaintMoveComplete; }
            set
            {
                m_bMaintMoveComplete = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 From 동작 대기 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_FromMoveWait
        {
            get { return m_bFromMoveWait; }
            set
            {
                m_bFromMoveWait = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 To 동작 대기 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_ToMoveWait
        {
            get { return m_bToMoveWait; }
            set
            {
                m_bToMoveWait = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Auto Teaching Run ACK 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_AutoTeachingRunACK
        {
            get { return m_bAutoTeachingRunACK; }
            set
            {
                m_bAutoTeachingRunACK = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Auto Teaching 동작 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_AutoTeachingRunningState
        {
            get { return m_bAutoTeachingRunningState; }
            set
            {
                m_bAutoTeachingRunningState = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Auto Teaching 완료 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_AutoTeachingComplete
        {
            get { return m_bAutoTeachingComplete; }
            set
            {
                m_bAutoTeachingComplete = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Auto Teaching 에러 완료 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_AutoTeachingErrorComplete
        {
            get { return m_bAutoTeachingErrorComplete; }
            set
            {
                m_bAutoTeachingErrorComplete = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 X Axis Busy 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_X_Axis_Busy
        {
            get { return m_bXAxis_Busy; }
            set
            {
                m_bXAxis_Busy = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Z Axis Busy 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_Z_Axis_Busy
        {
            get { return m_bZAxis_Busy; }
            set
            {
                m_bZAxis_Busy = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 A Axis Busy 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_A_Axis_Busy
        {
            get { return m_bAAxis_Busy; }
            set
            {
                m_bAAxis_Busy = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 T Axis Busy 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool Status_T_Axis_Busy
        {
            get { return m_bTAxis_Busy; }
            set
            {
                m_bTAxis_Busy = value;
            }
        }
        

        bool m_bSTK_TR_REQ = false;
        bool m_bSTK_Busy = false;
        bool m_bSTK_Complete = false;
        bool m_bSTK_Error = false;

        /// <summary>
        /// STK -> Master로 전송하는 PIO 중 TR_REQ 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool PIOStatus_STK_TR_REQ
        {
            get { return m_bSTK_TR_REQ; }
            set
            {
                m_bSTK_TR_REQ = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 PIO 중 Busy 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool PIOStatus_STK_Busy
        {
            get { return m_bSTK_Busy; }
            set
            {
                m_bSTK_Busy = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 PIO 중 Complete 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool PIOStatus_STK_Complete
        {
            get { return m_bSTK_Complete; }
            set
            {
                m_bSTK_Complete = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 PIO 중 Error 상태
        /// STK에서 Bit Packet 수신 시 업데이트 진행
        /// </summary>
        public bool PIOStatus_STK_Error
        {
            get { return m_bSTK_Error; }
            set
            {
                m_bSTK_Error = value;
            }
        }

        /// <summary>
        /// STK Bit Map Packet 수신 시 업데이트
        /// </summary>
        public void RackMasterBitUpdateEvent()
        {
            Status_AutoMode         = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_AutoMode);
            Status_AutoModeReady    = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_AutoMode_Ready);
            Status_ServoOn          = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_ServoOn);
            Status_ServoOnEnable    = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_ServoOn_Ready);
            Status_ServoOff         = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_ServoOff);
            Status_ServoOffEnable   = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_ServoOff_Ready);
            Status_ManualMode       = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_ManualMode);
            Status_ManualModeEnable = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_ManualMode_Ready);
            Status_Active           = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Active);
            Status_IDLE             = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Idle);
            Status_CassetteOn       = !Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Cassette_Not_Exist);
            Status_CassettePos      = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Cassette_RightPos);
            Status_Error            = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Error);
            Status_HomeDone         = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_HomeDone);
            Status_HWEStopSwitch    = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_EStop_Pushing);
            Status_GOTEStopSwitch   = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_GOT_EMO_Pushing);
            Status_HPEStopSwitch    = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_HP_EMO_Pushing);
            Status_OPEStopSwitch    = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_OP_EMO_Pushing);
            Status_GOTMode          = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_GOT_Detecting);
            Status_TeachingRWEnable = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Teaching_RW_Ready);
            Status_TeachingRWComplete = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Teaching_ReadComplete);
            Status_X_Axis_Busy      = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_X_Axis_Busy);
            Status_Z_Axis_Busy      = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Z_Axis_Busy);
            Status_A_Axis_Busy      = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_A_Axis_Busy);
            Status_T_Axis_Busy      = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_T_Axis_Busy);
            Status_FromMoveWait     = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_FromMove_Wait);
            Status_ToMoveWait       = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_ToMove_Wait);
            Status_FromRunACK       = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_From_Run_ACK);
            Status_FromRunComplete  = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_From_Complete);
            Status_ToRunACK         = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_To_Run_ACK);
            Status_ToRunComplete    = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_To_Run_Complete);

            Status_MaintMove        = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_MaintMoving);
            Status_MaintMoveRunAck  = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Maint_Run_ACK);
            Status_MaintMoveComplete = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Maint_Complete);

            if(Status_MaintMoveComplete)
            {
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Maint_Complete_ACK, true);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Maint_Complete_ACK);
            }

            Status_AutoTeachingRunACK           = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_AutoTeaching_Run_ACK);
            Status_AutoTeachingRunningState     = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_AutoTeaching_RunningState);
            Status_AutoTeachingComplete         = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_AutoTeaching_Complete);
            Status_AutoTeachingErrorComplete    = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_AutoTeaching_ErrorComplete);


            PIOStatus_STK_TR_REQ    = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_PIO_TR_REQ);
            PIOStatus_STK_Busy      = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_PIO_Busy);
            PIOStatus_STK_Complete  = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_PIO_Complete);
            PIOStatus_STK_Error     = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_PIO_STK_Error);
        }
    }
}
