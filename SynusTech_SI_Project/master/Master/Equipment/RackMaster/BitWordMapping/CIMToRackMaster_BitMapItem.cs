using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Equipment.RackMaster
{
    /// <summary>
    /// [CIM -> Master] -> STK Bit Memory Map에 사용되는 변수 기재
    /// </summary>
    public partial class RackMaster
    {
        bool m_bCMDEmergencyStop = false;
        bool m_bCMDAutoModeRun = false;
        bool m_bCMDAutoModeStop = false;
        bool m_bCMDServoOn = false;
        bool m_bCMDServoOff = false;
        bool m_bCMDErrorReset = false;
        bool m_bCMDTeachingRWRun = false;

        bool m_bCMDAutoTeachingRun = false;
        bool m_bCMDAutoTeachingCompleteACK = false;
        bool m_bCMDAutoTeachingErrorCompleteACK = false;
        bool m_bCMDAutoTeachingStop = false;

        bool m_bCMDFromRun = false;
        bool m_bCMDFromCompleteACK = false;
        bool m_bCMDToRun = false;
        bool m_bCMDToCompleteACK = false;

        bool m_bX_Axis_MaxLoadReset = false;
        bool m_bZ_Axis_MaxLoadReset = false;
        bool m_bA_Axis_MaxLoadReset = false;
        bool m_bT_Axis_MaxLoadReset = false;

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 EMO Bit
        /// 마스터 알람 또는 STK SW EMO Button 누름 상황에 따라 전송
        /// </summary>
        public bool CMD_EmergencyStop_REQ
        {
            get
            {
                return m_bCMDEmergencyStop;
            }
            set
            {
                if (m_bCMDEmergencyStop != value)
                {
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_EmergencyStop, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_EmergencyStop);
                }
                else
                {
                    if(Status_AutoMode)
                        Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_EmergencyStop);

                }

                m_bCMDEmergencyStop = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Auto Mode Run Bit
        /// 오토 Run, 오토 Stop 버튼 누름에 따라 전송 
        /// </summary>
        public bool CMD_AutoModeRun_REQ
        {
            get
            {
                return m_bCMDAutoModeRun;
            }
            set
            {
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_AutoMode_Run, value);
                if (IsConnected()) //이니셜 라이즈 상황 로그 작성 막기위해 추가
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_AutoMode_Run);
                m_bCMDAutoModeRun = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Auto Mode Stop Bit
        /// 오토 Run, 오토 Stop 버튼 누름에 따라 전송 
        /// </summary>
        public bool CMD_AutoModeStop_REQ
        {
            get
            {
                return m_bCMDAutoModeStop;
            }
            set
            {
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_AutoMode_Stop, value);
                if (IsConnected()) //이니셜 라이즈 상황 로그 작성 막기위해 추가
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_AutoMode_Stop);
                m_bCMDAutoModeStop = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Servo On Bit
        /// 파워 On, 파워 Off 버튼에 따라 전송 
        /// </summary>
        public bool CMD_ServoOn_REQ
        {
            get
            {
                return m_bCMDServoOn;
            }
            set
            {
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_ServoOn, value);
                if (IsConnected()) //이니셜 라이즈 상황 로그 작성 막기위해 추가
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_ServoOn);
                m_bCMDServoOn = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Servo Off Bit
        /// 파워 On, 파워 Off 버튼에 따라 전송 
        /// </summary>
        public bool CMD_ServoOff_REQ
        {
            get
            {
                return m_bCMDServoOff;
            }
            set
            {
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_ServoOff, value);
                if (IsConnected()) //이니셜 라이즈 상황 로그 작성 막기위해 추가
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_ServoOff);
                m_bCMDServoOff = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Error Reset Bit
        /// 알람 클리어 버튼에 따라 전송 
        /// </summary>
        public bool CMD_ErrorReset_REQ
        {
            get
            {
                return m_bCMDErrorReset;
            }
            set
            {
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_ErrorReset, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_ErrorReset);
                m_bCMDErrorReset = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Teaching RW Run Bit
        /// STK Teaching 페이지에서 기준 정보 얻기 버튼 또는 타임아웃에 의해 조작 됨 
        /// </summary>
        public bool CMD_TeachingRWRun_REQ
        {
            get
            {
                return m_bCMDTeachingRWRun;
            }
            set
            {
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Teaching_RW_Run, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Teaching_RW_Run);
                m_bCMDTeachingRWRun = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Auto Teaching Run Bit
        /// STK Auto Teaching 공정에서 조작 진행
        /// </summary>
        public bool CMD_AutoTeachingRun_REQ
        {
            get
            {
                return m_bCMDAutoTeachingRun;
            }
            set
            {
                m_bCMDAutoTeachingRun = value;
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_AutoTeaching_Run, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_AutoTeaching_Run);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Auto Teaching Complete ACK Bit
        /// STK Auto Teaching 공정에서 Complete 수신 시 조작
        /// </summary>
        public bool CMD_AutoTeachingCompleteACK_REQ
        {
            get
            {
                return m_bCMDAutoTeachingCompleteACK;
            }
            set
            {
                m_bCMDAutoTeachingCompleteACK = value;
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_AutoTeaching_Complete_ACK, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_AutoTeaching_Complete_ACK);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Auto Teaching Error Complete ACK Bit
        /// STK Auto Teaching 공정에서 Error Complete 수신 시 조작
        /// </summary>
        public bool CMD_AutoTeachingErrorCompleteACK_REQ
        {
            get
            {
                return m_bCMDAutoTeachingErrorCompleteACK;
            }
            set
            {
                m_bCMDAutoTeachingErrorCompleteACK = value;
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_AutoTeaching_ErrorComplete_ACK, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_AutoTeaching_ErrorComplete_ACK);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Auto Teaching Stop Bit
        /// STK Auto Teaching 공정에서 완료 또는 유저가 정지 버튼 누른 경우 조작
        /// </summary>
        public bool CMD_AutoTeachingStop_REQ
        {
            get
            {
                return m_bCMDAutoTeachingStop;
            }
            set
            {
                m_bCMDAutoTeachingStop = value;
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_AutoTeaching_Stop, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_AutoTeaching_Stop);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 From Run Bit
        /// STK Auto Cycle 공정에서 조작
        /// </summary>
        public bool CMD_FromRun_REQ
        {
            get
            {
                return m_bCMDFromRun;
            }
            set
            {
                m_bCMDFromRun = value;
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_From_Run, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_From_Run);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 From Complete ACK Bit
        /// STK Auto Cycle 공정에서 조작
        /// </summary>
        public bool CMD_FromCompleteACK_REQ
        {
            get
            {
                return m_bCMDFromCompleteACK;
            }
            set
            {
                m_bCMDFromCompleteACK = value;
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_From_Complete_ACK, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_From_Complete_ACK);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 To Run Bit
        /// STK Auto Cycle 공정에서 조작
        /// </summary>
        public bool CMD_ToRun_REQ
        {
            get
            {
                return m_bCMDToRun;
            }
            set
            {
                m_bCMDToRun = value;
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_To_Run, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_To_Run);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 To Complete ACK Bit
        /// STK Auto Cycle 공정에서 조작
        /// </summary>
        public bool CMD_ToCompleteACK_REQ
        {
            get
            {
                return m_bCMDToCompleteACK;
            }
            set
            {
                m_bCMDToCompleteACK = value;
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_To_Complete_ACK, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_To_Complete_ACK);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 과부하 초기화 Bit
        /// STK Setting 화면에서 조작
        /// </summary>
        public bool CMD_X_Axis_MaxLoad_Clear
        {
            get
            {
                return m_bX_Axis_MaxLoadReset;
            }
            set
            {
                m_bX_Axis_MaxLoadReset = value;
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_X_Axis_MaxLoad_Reset, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_X_Axis_MaxLoad_Reset);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 과부하 초기화 Bit
        /// STK Setting 화면에서 조작
        /// </summary>
        public bool CMD_Z_Axis_MaxLoad_Clear
        {
            get
            {
                return m_bZ_Axis_MaxLoadReset;
            }
            set
            {
                m_bZ_Axis_MaxLoadReset = value;
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Z_Axis_MaxLoad_Reset, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Z_Axis_MaxLoad_Reset);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 과부하 초기화 Bit
        /// STK Setting 화면에서 조작
        /// </summary>
        public bool CMD_A_Axis_MaxLoad_Clear
        {
            get
            {
                return m_bA_Axis_MaxLoadReset;
            }
            set
            {
                m_bA_Axis_MaxLoadReset = value;
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_A_Axis_MaxLoad_Reset, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_A_Axis_MaxLoad_Reset);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 과부하 초기화 Bit
        /// STK Setting 화면에서 조작
        /// </summary>
        public bool CMD_T_Axis_MaxLoad_Clear
        {
            get
            {
                return m_bT_Axis_MaxLoadReset;
            }
            set
            {
                m_bT_Axis_MaxLoadReset = value;
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_T_Axis_MaxLoad_Reset, value);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_T_Axis_MaxLoad_Reset);
            }
        }

        /// <summary>
        /// Interlock State
        /// Init은 초기 1회 전송을 위함
        /// </summary>
        bool m_bHPDoorOpenStateInit     = false;
        bool m_bHPDoorOpenState         = false;

        bool m_bOPDoorOpenStateInit     = false;
        bool m_bOPDoorOpenState         = false;

        bool m_bHPEMOPushInit           = false;
        bool m_bHPEMOPush               = false;

        bool m_bOPEMOPushInit           = false;
        bool m_bOPEMOPush               = false;

        bool m_bHPEscapeStateInit       = false;
        bool m_bHPEscapeState           = false;

        bool m_bOPEscapeStateInit       = false;
        bool m_bOPEscapeState           = false;

        bool m_bHPAutoKeyStateInit      = false;
        bool m_bHPAutoKeyState          = false;

        bool m_bHPHandyEMOStateInit     = false;
        bool m_bHPHandyEMOState         = false;

        bool m_bOPHandyEMOStateInit     = false;
        bool m_bOPHandyEMOState         = false;

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 HP Door Open 상태
        /// 마스터 I/O에서 인식한 접점 정보를 STK로 전달
        /// </summary>
        public bool Status_CIM_To_STK_HPDoorOpen
        {
            get
            {
                return m_bHPDoorOpenState;
            }
            set
            {
                if (m_bHPDoorOpenState != value || !m_bHPDoorOpenStateInit)
                {
                    m_bHPDoorOpenStateInit = true;
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_HP_DoorOpen_Status, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_HP_DoorOpen_Status);
                }
                m_bHPDoorOpenState = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 OP Door Open 상태
        /// 마스터 I/O에서 인식한 접점 정보를 STK로 전달
        /// </summary>
        public bool Status_CIM_To_STK_OPDoorOpen
        {
            get
            {
                return m_bOPDoorOpenState;
            }
            set
            {
                if (m_bOPDoorOpenState != value || !m_bOPDoorOpenStateInit)
                {
                    m_bOPDoorOpenStateInit = true;
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_OP_DoorOpen_Status, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_OP_DoorOpen_Status);
                }
                m_bOPDoorOpenState = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 HP EMO Push 상태
        /// 마스터 I/O에서 인식한 접점 정보를 STK로 전달
        /// </summary>
        public bool Status_CIM_To_STK_HP_EMO_Push
        {
            get
            {
                return m_bHPEMOPush;
            }
            set
            {
                if (m_bHPEMOPush != value || !m_bHPEMOPushInit)
                {
                    m_bHPEMOPushInit = true;
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_HP_EMO_Pushing, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_HP_EMO_Pushing);
                }
                m_bHPEMOPush = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 OP EMO Push 상태
        /// 마스터 I/O에서 인식한 접점 정보를 STK로 전달
        /// </summary>
        public bool Status_CIM_To_STK_OP_EMO_Push
        {
            get
            {
                return m_bOPEMOPush;
            }
            set
            {
                if (m_bOPEMOPush != value || !m_bOPEMOPushInit)
                {
                    m_bOPEMOPushInit = true;
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_OP_EMO_Pushing, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_OP_EMO_Pushing);
                }
                m_bOPEMOPush = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 HP Escape 상태
        /// 마스터 I/O에서 인식한 접점 정보를 STK로 전달
        /// </summary>
        public bool Status_CIM_To_STK_HP_Escape
        {
            get
            {
                return m_bHPEscapeState;
            }
            set
            {
                if (m_bHPEscapeState != value || !m_bHPEscapeStateInit)
                {
                    m_bHPEscapeStateInit = true;
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_HP_Escape_Pushing, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_HP_Escape_Pushing);
                }
                m_bHPEscapeState = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 OP Escape 상태
        /// 마스터 I/O에서 인식한 접점 정보를 STK로 전달
        /// </summary>
        public bool Status_CIM_To_STK_OP_Escape
        {
            get
            {
                return m_bOPEscapeState;
            }
            set
            {
                if (m_bOPEscapeState != value || !m_bOPEscapeStateInit)
                {
                    m_bOPEscapeStateInit = true;
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_OP_Escape_Pushing, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_OP_Escape_Pushing);
                }
                m_bOPEscapeState = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 HP Auto Key 상태
        /// 마스터 I/O에서 인식한 접점 정보를 STK로 전달
        /// </summary>
        public bool Status_CIM_To_STK_HPAutoKeyState
        {
            get
            {
                return m_bHPAutoKeyState;
            }
            set
            {
                if (m_bHPAutoKeyState != value || !m_bHPAutoKeyStateInit)
                {
                    m_bHPAutoKeyStateInit = true;
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RM_Key_AutoStatus, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RM_Key_AutoStatus);
                }
                m_bHPAutoKeyState = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 HP Handy EMO 상태
        /// 마스터 I/O에서 인식한 접점 정보를 STK로 전달
        /// </summary>
        public bool Status_CIM_To_STK_HPHandyEMO
        {
            get
            {
                return m_bHPHandyEMOState;
            }
            set
            {
                if (m_bHPHandyEMOState != value || !m_bHPHandyEMOStateInit)
                {
                    m_bHPHandyEMOStateInit = true;
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RM_HP_Handy_EMO_Push, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RM_HP_Handy_EMO_Push);
                }
                m_bHPHandyEMOState = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 OP Handy EMO 상태
        /// 마스터 I/O에서 인식한 접점 정보를 STK로 전달
        /// </summary>
        public bool Status_CIM_To_STK_OPHandyEMO
        {
            get
            {
                return m_bOPHandyEMOState;
            }
            set
            {
                if (m_bOPHandyEMOState != value || !m_bOPHandyEMOStateInit)
                {
                    m_bOPHandyEMOStateInit = true;
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RM_OP_Handy_EMO_Push, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RM_OP_Handy_EMO_Push);
                }
                m_bOPHandyEMOState = value;
            }
        }

        bool m_bPort_L_REQ = false;
        bool m_bPort_UL_REQ = false;
        bool m_bPort_Ready = false;
        bool m_bPort_Error = false;

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Port Load REQ 상태
        /// STK의 Access ID가 Master에서 보유한 Port ID와 일치하는 경우 업데이트 진행
        /// </summary>
        public bool PIOStatus_Port_L_REQ
        {
            get { return m_bPort_L_REQ; }
            set
            {
                if(m_bPort_L_REQ != value)
                {
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_PIO_L_REQ, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_PIO_L_REQ);
                }
                m_bPort_L_REQ = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Port Unload REQ 상태
        /// STK의 Access ID가 Master에서 보유한 Port ID와 일치하는 경우 업데이트 진행
        /// </summary>
        public bool PIOStatus_Port_UL_REQ
        {
            get { return m_bPort_UL_REQ; }
            set
            {
                if(m_bPort_UL_REQ != value)
                {
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_PIO_UL_REQ, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_PIO_UL_REQ);
                }
                m_bPort_UL_REQ = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Port Ready 상태
        /// STK의 Access ID가 Master에서 보유한 Port ID와 일치하는 경우 업데이트 진행
        /// </summary>
        public bool PIOStatus_Port_Ready
        {
            get { return m_bPort_Ready; }
            set
            {
                if(m_bPort_Ready != value)
                {
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_PIO_Ready, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_PIO_Ready);
                }
                m_bPort_Ready = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Port Error 상태
        /// STK의 Access ID가 Master에서 보유한 Port ID와 일치하는 경우 업데이트 진행
        /// </summary>
        public bool PIOStatus_Port_Error
        {
            get { return m_bPort_Error; }
            set
            {
                if(m_bPort_Error != value)
                {
                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_PIO_PortError, value);
                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_PIO_PortError);
                }
                m_bPort_Error = value;
            }
        }
    }
}
