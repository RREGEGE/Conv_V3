using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.TCP;
using Master.Interface.Math;

namespace Master.Equipment.Port
{
    /// <summary>
    /// PortAction.cs는 Port Memory Map 및 통신 관련 행동 작성
    /// </summary>
    public partial class Port
    {
        /// <summary>
        /// Port Memory Map 정의
        /// </summary>
        public enum ReceiveBitMapIndex
        {
            Run_REQ = 17,
            Stop_REQ,

            PowerOn_REQ = 20,
            PowerOff_REQ,

            InMode_REQ = 23,
            OutMode_REQ,

            AGVType_REQ = 26,
            MGVType_REQ,

            Move_Reserved = 29,
            Error_Reset
        }
        public enum SendBitMapIndex
        {
            L_REQ,
            UL_REQ,
            Ready,
            Port_Error,

            TR_REQ = 8,
            Busy,
            Complete,
            STK_Auto,
            STK_EStop,
            STK_Error,

            AutoRunStatus = 16,
            Run_Enable,
            Stop_Enable,
            PowerOn_Status,
            PowerOn_Enable,
            PowerOff_Enable,
            MGV_Status,
            AGV_Status,
            Type_Change_Enable,
            Input_Status,
            Output_Status,
            Mode_Change_Enable,

            CIM_Mode_Status = 30,


            Buffer1_CST_ON = 32,
            Buffer2_CST_ON,

            //OHT Only
            OHT_To_CV_Valid = 48,
            OHT_To_CV_CS0,

            OHT_To_CV_TR_REQ = 53,
            OHT_To_CV_Busy,
            OHT_To_CV_Complete,
            CV_To_OHT_Load_REQ,
            CV_To_OHT_Unload_REQ,
            CV_To_OHT_ES,
            CV_To_OHT_Ready,
            CV_To_OHT_HO_AVBL,

            OHT_Door_Close = 62,
            OHT_Port_Key_Auto_Status,

            //AGV Only
            AGV_To_CV_Valid = 64,
            AGV_To_CV_CS0,

            AGV_To_CV_TR_REQ = 69,
            AGV_To_CV_Busy,
            AGV_To_CV_Complete,
            CV_To_AGV_Load_REQ,
            CV_To_AGV_Unload_REQ,
            CV_To_AGV_ES,
            CV_To_AGV_Ready
        }
        public enum ReceiveWordMapIndex
        {
            Buffer1_Control_0 = 0,
            Buffer2_Control_0 = 2,

            Buffer_SyncMove = 10,


            Carrier_ID_1 = 0x10,
            Carrier_ID_2,
            Carrier_ID_3,
            Carrier_ID_4,
            Carrier_ID_5,
            Carrier_ID_6,
            Carrier_ID_7,
            Carrier_ID_8,
            Carrier_ID_9,
            Carrier_ID_10,
            Carrier_ID_11,
            Carrier_ID_12,
            Carrier_ID_13,
            Carrier_ID_14,
            Carrier_ID_15,
            Carrier_ID_16,
            Carrier_ID_17,
            Carrier_ID_18,
            Carrier_ID_19,
            Carrier_ID_20,
            Carrier_ID_21,
            Carrier_ID_22,
            Carrier_ID_23,
            Carrier_ID_24,
            Carrier_ID_25,
            Carrier_ID_26,
            Carrier_ID_27,
            Carrier_ID_28,
            Carrier_ID_29,
            Carrier_ID_30,
            Carrier_ID_31,
            Carrier_ID_32,
            Carrier_ID_33,
            Carrier_ID_34,
            Carrier_ID_35,
            Carrier_ID_36,
            Carrier_ID_37,
            Carrier_ID_38,
            Carrier_ID_39,
            Carrier_ID_40,
            Carrier_ID_41,
            Carrier_ID_42,
            Carrier_ID_43,
            Carrier_ID_44,
            Carrier_ID_45,
            Carrier_ID_46,
            Carrier_ID_47,
            Carrier_ID_48,
            Carrier_ID_49,
            Carrier_ID_50,
            Carrier_ID_51,
            Carrier_ID_52,
            Carrier_ID_53,
            Carrier_ID_54,
            Carrier_ID_55,
            Carrier_ID_56,
            Carrier_ID_WriteFlag
        }
        public enum SendWordMapIndex
        {
            ErrorCode_0 = 0x00,
            ErrorCode_1,
            ErrorCode_2,
            ErrorCode_3,
            ErrorCode_4,

            X_Axis_CurrentPosition_0 = 0x10,
            X_Axis_CurrentPosition_1,
            X_Axis_TargetPosition_0,
            X_Axis_TargetPosition_1,
            X_Axis_CurrentSpeed,
            X_Axis_SettingSpeed,
            X_Axis_CurrentTorque,
            X_Axis_PeakTorque,
            X_Axis_AverageTorque,
            X_Axis_SensorMonitoring,
            X_Axis_CurrentDetectedMaxLoad,
            X_Axis_CurrentSettingMaxLoad,

            Z_Axis_CurrentPosition_0 = 0x20,
            Z_Axis_CurrentPosition_1,
            Z_Axis_TargetPosition_0,
            Z_Axis_TargetPosition_1,
            Z_Axis_CurrentSpeed,
            Z_Axis_SettingSpeed,
            Z_Axis_CurrentTorque,
            Z_Axis_PeakTorque,
            Z_Axis_AverageTorque,
            Z_Axis_SensorMonitoring,
            Z_Axis_CurrentDetectedMaxLoad,
            Z_Axis_CurrentSettingMaxLoad,

            T_Axis_CurrentPosition_0 = 0x30,
            T_Axis_CurrentPosition_1,
            T_Axis_TargetPosition_0,
            T_Axis_TargetPosition_1,
            T_Axis_CurrentSpeed,
            T_Axis_SettingSpeed,
            T_Axis_CurrentTorque,
            T_Axis_PeakTorque,
            T_Axis_AverageTorque,
            T_Axis_SensorMonitoring,
            T_Axis_CurrentDetectedMaxLoad,
            T_Axis_CurrentSettingMaxLoad,

            Shuttle_SensorStatus = 0x3f,
            Buffer1_Carrier_ID_01 = 0x40,
            Buffer1_Carrier_ID_02,
            Buffer1_Carrier_ID_03,
            Buffer1_Carrier_ID_04,
            Buffer1_Carrier_ID_05,
            Buffer1_Carrier_ID_06,
            Buffer1_Carrier_ID_07,
            Buffer1_Carrier_ID_08,
            Buffer1_Carrier_ID_09,
            Buffer1_Carrier_ID_10,
            Buffer1_Carrier_ID_11,
            Buffer1_Carrier_ID_12,
            Buffer1_Carrier_ID_13,
            Buffer1_Carrier_ID_14,
            Buffer1_Carrier_ID_15, 
            Buffer1_Carrier_ID_16,
            Buffer1_Carrier_ID_17,
            Buffer1_Carrier_ID_18,
            Buffer1_Carrier_ID_19,
            Buffer1_Carrier_ID_20,
            Buffer1_Carrier_ID_21,
            Buffer1_Carrier_ID_22,
            Buffer1_Carrier_ID_23,
            Buffer1_Carrier_ID_24,
            Buffer1_Carrier_ID_25,
            Buffer1_Carrier_ID_26,
            Buffer1_Carrier_ID_27,
            Buffer1_Carrier_ID_28,
            Buffer1_Carrier_ID_29,
            Buffer1_Carrier_ID_30,
            Buffer1_Carrier_ID_31,
            Buffer1_Carrier_ID_32,
            Buffer1_Carrier_ID_33,
            Buffer1_Carrier_ID_34,
            Buffer1_Carrier_ID_35,
            Buffer1_Carrier_ID_36,
            Buffer1_Carrier_ID_37,
            Buffer1_Carrier_ID_38,
            Buffer1_Carrier_ID_39,
            Buffer1_Carrier_ID_40,
            Buffer1_Carrier_ID_41,
            Buffer1_Carrier_ID_42,
            Buffer1_Carrier_ID_43,
            Buffer1_Carrier_ID_44,
            Buffer1_Carrier_ID_45,
            Buffer1_Carrier_ID_46,
            Buffer1_Carrier_ID_47,
            Buffer1_Carrier_ID_48,
            Buffer1_Carrier_ID_49,
            Buffer1_Carrier_ID_50,
            Buffer1_Carrier_ID_51,
            Buffer1_Carrier_ID_52,
            Buffer1_Carrier_ID_53,
            Buffer1_Carrier_ID_54,
            Buffer1_Carrier_ID_55,
            Buffer1_Carrier_ID_56,
            Buffer1_Carrier_ID_57,
            Buffer1_Carrier_ID_58,

            Buffer1_AutoStep = 0x7c,
            Buffer1_SensorStatus1,
            Buffer1_SensorStatus2,

            Buffer2_Carrier_ID_01 = 0x80,
            Buffer2_Carrier_ID_02,
            Buffer2_Carrier_ID_03,
            Buffer2_Carrier_ID_04,
            Buffer2_Carrier_ID_05,
            Buffer2_Carrier_ID_06,
            Buffer2_Carrier_ID_07,
            Buffer2_Carrier_ID_08,
            Buffer2_Carrier_ID_09,
            Buffer2_Carrier_ID_10,
            Buffer2_Carrier_ID_11,
            Buffer2_Carrier_ID_12,
            Buffer2_Carrier_ID_13,
            Buffer2_Carrier_ID_14,
            Buffer2_Carrier_ID_15,
            Buffer2_Carrier_ID_16,
            Buffer2_Carrier_ID_17,
            Buffer2_Carrier_ID_18,
            Buffer2_Carrier_ID_19,
            Buffer2_Carrier_ID_20,
            Buffer2_Carrier_ID_21,
            Buffer2_Carrier_ID_22,
            Buffer2_Carrier_ID_23,
            Buffer2_Carrier_ID_24,
            Buffer2_Carrier_ID_25,
            Buffer2_Carrier_ID_26,
            Buffer2_Carrier_ID_27,
            Buffer2_Carrier_ID_28,
            Buffer2_Carrier_ID_29,
            Buffer2_Carrier_ID_30,
            Buffer2_Carrier_ID_31,
            Buffer2_Carrier_ID_32,
            Buffer2_Carrier_ID_33,
            Buffer2_Carrier_ID_34,
            Buffer2_Carrier_ID_35,
            Buffer2_Carrier_ID_36,
            Buffer2_Carrier_ID_37,
            Buffer2_Carrier_ID_38,
            Buffer2_Carrier_ID_39,
            Buffer2_Carrier_ID_40,
            Buffer2_Carrier_ID_41,
            Buffer2_Carrier_ID_42,
            Buffer2_Carrier_ID_43,
            Buffer2_Carrier_ID_44,
            Buffer2_Carrier_ID_45,
            Buffer2_Carrier_ID_46,
            Buffer2_Carrier_ID_47,
            Buffer2_Carrier_ID_48,
            Buffer2_Carrier_ID_49,
            Buffer2_Carrier_ID_50,
            Buffer2_Carrier_ID_51,
            Buffer2_Carrier_ID_52,
            Buffer2_Carrier_ID_53,
            Buffer2_Carrier_ID_54,
            Buffer2_Carrier_ID_55,
            Buffer2_Carrier_ID_56,
            Buffer2_Carrier_ID_57,
            Buffer2_Carrier_ID_58,

            Buffer2_AutoStep = 0xbc,
            Buffer2_SensorStatus1,
            Buffer2_SensorStatus2
        }
        
        /// <summary>
        /// Port Memory Map에서 Word 영역 중 Bit Data 구분
        /// </summary>
        public enum Buffer2_ControlStatusBitMap
        {
            CV_FWD_HighSpeed_Move,
            CV_FWD_LowSpeed_Move,
            CV_BWD_HighSpeed_Move,
            CV_BWD_LowSpeed_Move,

            CV_Up_HighSpeed_Move,
            CV_Up_LowSpeed_Move,
            CV_Down_HighSpeed_Move,
            CV_Down_LowSpeed_Move
        }
        public enum Buffer1_ControlStatusBitMap
        {
            CV_FWD_HighSpeed_Move,
            CV_FWD_LowSpeed_Move,
            CV_BWD_HighSpeed_Move,
            CV_BWD_LowSpeed_Move
        }
        public enum BufferSync_ControlStatusBitMap
        {
            Buffer1_Select,
            Buffer2_Select,

            Selected_Buffer_FWD_Move = 6,
            Selected_Buffer_BWD_Move
        }

        public enum X_Axis_SensorStatusBitMap
        {
            NOT_Sensor,
            POT_Sensor,
            HOME_Sensor_LP,
            POS_Sensor_OP,
            Busy,
            OriginOK,
            WaitPosSensor
        }
        public enum Z_Axis_SensorStatusBitMap
        {
            NOT_Sensor,
            POT_Sensor,
            HOME_Sensor,
            POS_Sensor,
            Busy,
            OriginOK,
            
            Cylinder_BWD_Pos = 0x08,
            Cylinder_FWD_Pos
        }
        public enum T_Axis_SensorStatusBitMap
        {
            NOT_Sensor,
            POT_Sensor,
            HOME_Sensor,
            POS_Sensor,
            Busy,
            OriginOK,
            Degree_0_Position,
            Degree_180_Position
        }
        public enum Shuttle_SensorStatusBitMap
        {
            CST_InPlace_1,
            CST_InPlace_2
        }

        /// <summary>
        /// Buffer 2 - LP
        /// </summary>
        public enum Buffer2_SensorStatus1_BitIndex
        {
            CST_Detect_1,
            CST_Detect_2,
            CST_Presence,

            Hoist_Detect = 0x06,
            Cart_Detect_1,
            Cart_Detect_2,

            LED_Green = 0x0c,
            LED_Red
        }
        public enum Buffer2_SensorStatus2_BitIndex
        {
            Buffer_CV_IN = 0,

            Buffer_CV_OUT = 2,

            Buffer_Z_Axis_NOT = 4,
            Buffer_Z_Axis_POS1,
            Buffer_Z_Axis_POS2,
            Buffer_Z_Axis_POT,

            Buffer_CV_Forwarding = 10,
            Buffer_CV_Backwarding,
            Buffer_CV_Error,
            Buffer_Z_Axis_Error
        }
        
        /// <summary>
        /// Buffer 1 - OP
        /// </summary>
        public enum Buffer1_SensorStatus1_BitIndex
        {
            CST_Detect_1,
            CST_Detect_2,
            CST_Presence,

            Fork_Detect = 0x05,
        }
        public enum Buffer1_SensorStatus2_BitIndex
        {
            Buffer_CV_IN = 0,
            Buffer_CV_SLOW = 1,
            Buffer_CV_OUT = 2,

            Buffer_Z_Axis_NOT = 4,
            Buffer_Z_Axis_POS1,
            Buffer_Z_Axis_POS2,
            Buffer_Z_Axis_POT,

            Buffer_CV_Forwarding = 10,
            Buffer_CV_Backwarding,
            Buffer_CV_Error,
            Buffer_Z_Axis_Error
        }

        
        /// <summary>
        /// CIM -> Port Bit Map 패킷 수신 시 동작 행위
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        public void Action_CIM_2_Port_Bit_Data(ReceiveBitMapIndex eReceiveBitMap)
        {
            eReceiveBitMap = (ReceiveBitMapIndex)((int)eReceiveBitMap - GetParam().RecvBitMapStartAddr);
            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPPacketReceive, $"[C->P] Receive BitMap: {eReceiveBitMap}, Data :{Get_CIM_2_Port_Bit_Data(eReceiveBitMap)}");

            switch (eReceiveBitMap)
            {
                case ReceiveBitMapIndex.Run_REQ:
                    CMD_Run_REQ = Get_CIM_2_Port_Bit_Data(ReceiveBitMapIndex.Run_REQ);
                    break;
                case ReceiveBitMapIndex.Stop_REQ:
                    CMD_Stop_REQ = Get_CIM_2_Port_Bit_Data(ReceiveBitMapIndex.Stop_REQ);
                    break;
                case ReceiveBitMapIndex.PowerOn_REQ:
                    CMD_PowerOn_REQ = Get_CIM_2_Port_Bit_Data(ReceiveBitMapIndex.PowerOn_REQ);
                    break;
                case ReceiveBitMapIndex.PowerOff_REQ:
                    CMD_PowerOff_REQ = Get_CIM_2_Port_Bit_Data(ReceiveBitMapIndex.PowerOff_REQ);
                    break;
                case ReceiveBitMapIndex.InMode_REQ:
                    CMD_InMode_REQ = Get_CIM_2_Port_Bit_Data(ReceiveBitMapIndex.InMode_REQ);
                    break;
                case ReceiveBitMapIndex.OutMode_REQ:
                    CMD_OutMode_REQ = Get_CIM_2_Port_Bit_Data(ReceiveBitMapIndex.OutMode_REQ);
                    break;
                case ReceiveBitMapIndex.AGVType_REQ:
                    CMD_AGVorOHTMode_REQ = Get_CIM_2_Port_Bit_Data(ReceiveBitMapIndex.AGVType_REQ);
                    break;
                case ReceiveBitMapIndex.MGVType_REQ:
                    CMD_MGVMode_REQ = Get_CIM_2_Port_Bit_Data(ReceiveBitMapIndex.MGVType_REQ);
                    break;
                case ReceiveBitMapIndex.Move_Reserved:
                    CMD_MoveReserved = Get_CIM_2_Port_Bit_Data(ReceiveBitMapIndex.Move_Reserved);
                    break;
                case ReceiveBitMapIndex.Error_Reset:
                    CMD_ErrorReset = Get_CIM_2_Port_Bit_Data(ReceiveBitMapIndex.Error_Reset);
                    break;
                default:
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->P] BitMap Number: {(int)eReceiveBitMap}");
                    break;
            }
        }
        
        /// <summary>
        /// CIM -> Port Word Map 패킷 수신 시 동작 행위 (사실상 없음)
        /// </summary>
        /// <param name="eReceiveWordMapIndex"></param>
        public void Action_CIM_2_Port_Word_Data(ReceiveWordMapIndex eReceiveWordMapIndex)
        {
            eReceiveWordMapIndex = (ReceiveWordMapIndex)((int)eReceiveWordMapIndex - GetParam().RecvWordMapStartAddr);
            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPPacketReceive, $"[C->P] Receive WordMap: {eReceiveWordMapIndex}, Data :{Get_CIM_2_Port_Word_Data(eReceiveWordMapIndex)}");
            
            switch (eReceiveWordMapIndex)
            {
                case ReceiveWordMapIndex.Buffer1_Control_0:
                    break;
                case ReceiveWordMapIndex.Buffer2_Control_0:
                    break;
                case ReceiveWordMapIndex.Buffer_SyncMove:
                    break;
                case ReceiveWordMapIndex.Carrier_ID_1:
                case ReceiveWordMapIndex.Carrier_ID_2:
                case ReceiveWordMapIndex.Carrier_ID_3:
                case ReceiveWordMapIndex.Carrier_ID_4:
                case ReceiveWordMapIndex.Carrier_ID_5:
                case ReceiveWordMapIndex.Carrier_ID_6:
                case ReceiveWordMapIndex.Carrier_ID_7:
                case ReceiveWordMapIndex.Carrier_ID_8:
                case ReceiveWordMapIndex.Carrier_ID_9:
                case ReceiveWordMapIndex.Carrier_ID_10:
                case ReceiveWordMapIndex.Carrier_ID_11:
                case ReceiveWordMapIndex.Carrier_ID_12:
                case ReceiveWordMapIndex.Carrier_ID_13:
                case ReceiveWordMapIndex.Carrier_ID_14:
                case ReceiveWordMapIndex.Carrier_ID_15:
                case ReceiveWordMapIndex.Carrier_ID_16:
                case ReceiveWordMapIndex.Carrier_ID_17:
                case ReceiveWordMapIndex.Carrier_ID_18:
                case ReceiveWordMapIndex.Carrier_ID_19:
                case ReceiveWordMapIndex.Carrier_ID_20:
                case ReceiveWordMapIndex.Carrier_ID_21:
                case ReceiveWordMapIndex.Carrier_ID_22:
                case ReceiveWordMapIndex.Carrier_ID_23:
                case ReceiveWordMapIndex.Carrier_ID_24:
                case ReceiveWordMapIndex.Carrier_ID_25:
                case ReceiveWordMapIndex.Carrier_ID_26:
                case ReceiveWordMapIndex.Carrier_ID_27:
                case ReceiveWordMapIndex.Carrier_ID_28:
                case ReceiveWordMapIndex.Carrier_ID_29:
                case ReceiveWordMapIndex.Carrier_ID_30:
                case ReceiveWordMapIndex.Carrier_ID_31:
                case ReceiveWordMapIndex.Carrier_ID_32:
                case ReceiveWordMapIndex.Carrier_ID_33:
                case ReceiveWordMapIndex.Carrier_ID_34:
                case ReceiveWordMapIndex.Carrier_ID_35:
                case ReceiveWordMapIndex.Carrier_ID_36:
                case ReceiveWordMapIndex.Carrier_ID_37:
                case ReceiveWordMapIndex.Carrier_ID_38:
                case ReceiveWordMapIndex.Carrier_ID_39:
                case ReceiveWordMapIndex.Carrier_ID_40:
                case ReceiveWordMapIndex.Carrier_ID_41:
                case ReceiveWordMapIndex.Carrier_ID_42:
                case ReceiveWordMapIndex.Carrier_ID_43:
                case ReceiveWordMapIndex.Carrier_ID_44:
                case ReceiveWordMapIndex.Carrier_ID_45:
                case ReceiveWordMapIndex.Carrier_ID_46:
                case ReceiveWordMapIndex.Carrier_ID_47:
                case ReceiveWordMapIndex.Carrier_ID_48:
                case ReceiveWordMapIndex.Carrier_ID_49:
                case ReceiveWordMapIndex.Carrier_ID_50:
                case ReceiveWordMapIndex.Carrier_ID_51:
                case ReceiveWordMapIndex.Carrier_ID_52:
                case ReceiveWordMapIndex.Carrier_ID_53:
                case ReceiveWordMapIndex.Carrier_ID_54:
                case ReceiveWordMapIndex.Carrier_ID_55:
                case ReceiveWordMapIndex.Carrier_ID_56:
                    break;
                case ReceiveWordMapIndex.Carrier_ID_WriteFlag:
                    break;
                default:
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->P] WordMap Number: {(int)eReceiveWordMapIndex}");
                    break;
            }
        }


        /// <summary>
        /// CIM -> Port Bit Map의 데이터를 가져옴
        /// CIM에서 패킷 수신 시 CIM -> Port Bit Map에 우선 적용 되며 이후 Action 함수 호출을 통해 해당 Bit의 값을 얻어옴
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <returns></returns>
        public bool Get_CIM_2_Port_Bit_Data(ReceiveBitMapIndex eReceiveBitMap)
        {
            if ((int)eReceiveBitMap + GetParam().RecvBitMapStartAddr >= Master.m_Port_RecvBitMap.Length || (int)eReceiveBitMap + GetParam().RecvBitMapStartAddr < 0)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[C->P] BitMap Index: {eReceiveBitMap} + Start: {GetParam().RecvBitMapStartAddr} >= Length: {Master.m_Port_RecvBitMap.Length}");

                return false;
            }

            return Master.m_Port_RecvBitMap[(int)eReceiveBitMap + GetParam().RecvBitMapStartAddr];
        }
        
        /// <summary>
        /// CIM -> Port Word Map의 데이터를 가져옴
        /// CIM에서 패킷 수신 시 CIM -> Port Word Map에 우선 적용
        /// 주로 CST ID Read 관련 기능들이 추가되어 있으나 CST ID는 Master가 관리하므로 기능만 있고 사용 안함
        /// </summary>
        /// <param name="eReceiveWordMapIndex"></param>
        /// <returns></returns>
        public object Get_CIM_2_Port_Word_Data(ReceiveWordMapIndex eReceiveWordMapIndex)
        {
            if ((int)eReceiveWordMapIndex + GetParam().RecvWordMapStartAddr >= Master.m_Port_RecvWordMap.Length || (int)eReceiveWordMapIndex + GetParam().RecvWordMapStartAddr < 0)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[C->P] WordMap Index: {eReceiveWordMapIndex} + Start: {GetParam().RecvWordMapStartAddr} >= Length: {Master.m_Port_RecvWordMap.Length}");

                return (short)-1;
            }

            switch (eReceiveWordMapIndex)
            {
                default:
                    {
                        if (eReceiveWordMapIndex >= ReceiveWordMapIndex.Carrier_ID_1 &&
                            eReceiveWordMapIndex <= ReceiveWordMapIndex.Carrier_ID_56)
                            eReceiveWordMapIndex = ReceiveWordMapIndex.Carrier_ID_1;
                    }
                    break;
            }

            switch (eReceiveWordMapIndex)
            {
                case ReceiveWordMapIndex.Carrier_ID_1:
                    {
                        int stringSize = 56 * sizeof(short);
                        byte[] stringData = new byte[stringSize];
                        Buffer.BlockCopy(Master.m_Port_RecvWordMap, ((int)eReceiveWordMapIndex + GetParam().RecvWordMapStartAddr) * sizeof(short), stringData, 0, stringSize);
                        return (string)Encoding.Default.GetString(stringData).Trim('\0');
                    }
                default:
                    return Master.m_Port_RecvWordMap[(int)eReceiveWordMapIndex + GetParam().RecvWordMapStartAddr];

            }
        }
        
        /// <summary>
        /// Port -> CIM Bit Map의 데이터를 얻어옴
        /// Port의 Status or CMD 객체에서 값을 직접 얻어오므로 굳이 메모리 맵에서 획득할 필요 없음
        /// </summary>
        /// <param name="eSendBitMap"></param>
        /// <returns></returns>
        public bool Get_Port_2_CIM_Bit_Data(SendBitMapIndex eSendBitMap)
        {
            if ((int)eSendBitMap + GetParam().SendBitMapStartAddr >= Master.m_Port_SendBitMap.Length || (int)eSendBitMap + GetParam().SendBitMapStartAddr < 0)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[P->C] BitMap Index: {eSendBitMap} + Start: {GetParam().SendBitMapStartAddr} >= Length: {Master.m_Port_SendBitMap.Length}");
                return false;
            }

            return Master.m_Port_SendBitMap[(int)eSendBitMap + GetParam().SendBitMapStartAddr];
        }

        /// <summary>
        /// Port -> CIM Word Map의 데이터를 얻어옴
        /// Port의 Status or CMD 객체에서 값을 직접 얻어오므로 굳이 메모리 맵에서 획득할 필요 없음
        /// </summary>
        /// <param name="eSendWordMapIndex"></param>
        /// <returns></returns>
        public object Get_Port_2_CIM_Word_Data(SendWordMapIndex eSendWordMapIndex)
        {
            if ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr >= Master.m_Port_SendWordMap.Length || (int)eSendWordMapIndex + GetParam().SendWordMapStartAddr < 0)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[P->C] WordMap Index: {eSendWordMapIndex} + Start: {GetParam().SendWordMapStartAddr} >= Length: {Master.m_Port_SendWordMap.Length}");

                return (short)-1;
            }

            //Multi Word인 경우 자리 조정
            switch (eSendWordMapIndex)
            {
                case SendWordMapIndex.X_Axis_CurrentPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.X_Axis_CurrentPosition_0;
                    break;
                case SendWordMapIndex.X_Axis_TargetPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.X_Axis_TargetPosition_0;
                    break;
                case SendWordMapIndex.Z_Axis_CurrentPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.Z_Axis_CurrentPosition_0;
                    break;
                case SendWordMapIndex.Z_Axis_TargetPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.Z_Axis_TargetPosition_0;
                    break;
                case SendWordMapIndex.T_Axis_CurrentPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.T_Axis_CurrentPosition_0;
                    break;
                case SendWordMapIndex.T_Axis_TargetPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.T_Axis_TargetPosition_0;
                    break;

                default:
                    {
                        if (eSendWordMapIndex >= SendWordMapIndex.Buffer1_Carrier_ID_01 &&
                            eSendWordMapIndex <= SendWordMapIndex.Buffer1_Carrier_ID_58)
                            eSendWordMapIndex = SendWordMapIndex.Buffer1_Carrier_ID_01;

                        if (eSendWordMapIndex >= SendWordMapIndex.Buffer2_Carrier_ID_01 &&
                            eSendWordMapIndex <= SendWordMapIndex.Buffer2_Carrier_ID_58)
                            eSendWordMapIndex = SendWordMapIndex.Buffer2_Carrier_ID_01;
                    }
                    break;
            }

            switch (eSendWordMapIndex)
            {
                case SendWordMapIndex.Buffer1_Carrier_ID_01:
                case SendWordMapIndex.Buffer2_Carrier_ID_01:
                    {
                        int stringSize = 58 * sizeof(short);
                        byte[] stringData = new byte[stringSize];
                        Buffer.BlockCopy(Master.m_Port_SendWordMap, ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr) * sizeof(short), stringData, 0, stringSize);
                        return ((string)Encoding.Default.GetString(stringData).Trim('\0')).Replace(" ", string.Empty);
                    }
                case SendWordMapIndex.X_Axis_CurrentPosition_0:
                case SendWordMapIndex.X_Axis_TargetPosition_0:
                case SendWordMapIndex.Z_Axis_CurrentPosition_0:
                case SendWordMapIndex.Z_Axis_TargetPosition_0:
                case SendWordMapIndex.T_Axis_CurrentPosition_0:
                case SendWordMapIndex.T_Axis_TargetPosition_0:
                    {
                        byte[] floatData = new byte[4];
                        Buffer.BlockCopy(Master.m_Port_SendWordMap, ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr) * sizeof(short), floatData, 0, 4);
                        return (float)BitConverter.ToSingle(floatData, 0);
                    }
                default:
                    return Master.m_Port_SendWordMap[(int)eSendWordMapIndex + GetParam().SendWordMapStartAddr];

            }
        }

        /// <summary>
        /// CIM -> Port의 Bit Map 영역이므로 관리하지 않음
        /// </summary>
        /// <param name="eRecvBitMap"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set_CIM_2_Port_Bit_Data(ReceiveBitMapIndex eRecvBitMap, bool value)
        {
            if ((int)eRecvBitMap + GetParam().RecvBitMapStartAddr >= Master.m_Port_RecvBitMap.Length || (int)eRecvBitMap + GetParam().RecvBitMapStartAddr < 0)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[C->P] BitMap Index: {eRecvBitMap} + Start: {GetParam().RecvBitMapStartAddr} >= Length: {Master.m_Port_RecvBitMap.Length}");

                return false;
            }

            Master.m_Port_RecvBitMap[(int)eRecvBitMap + GetParam().RecvBitMapStartAddr] = value;
            return true;
        }

        /// <summary>
        /// CIM -> Port의 Word Map 영역이지만 CV 제어 시 일부 영역 활용 중
        /// </summary>
        /// <param name="eReceiveWordMapIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set_CIM_2_Port_Word_Data(ReceiveWordMapIndex eReceiveWordMapIndex, object value)
        {
            try
            {
                if ((int)eReceiveWordMapIndex + GetParam().RecvWordMapStartAddr >= Master.m_Port_RecvWordMap.Length || (int)eReceiveWordMapIndex + GetParam().RecvWordMapStartAddr < 0)
                {
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                        $"[C->P] WordMap Index: {eReceiveWordMapIndex} + Start: {GetParam().RecvWordMapStartAddr} >= Length: {Master.m_Port_RecvWordMap.Length}");
                    return false;
                }

                switch (eReceiveWordMapIndex)
                {
                    default:
                        Master.m_Port_RecvWordMap[(int)eReceiveWordMapIndex + GetParam().RecvWordMapStartAddr] = Convert.ToInt16(value);
                        break;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Port -> CIM Bit Map에 Data를 적용
        /// Get, Set 구조의 매개 변수에서 Set시 적용 되도록 구성
        /// </summary>
        /// <param name="eSendBitMapIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set_Port_2_CIM_Bit_Data(SendBitMapIndex eSendBitMapIndex, bool value)
        {
            if ((int)eSendBitMapIndex + GetParam().SendBitMapStartAddr >= Master.m_Port_SendBitMap.Length || (int)eSendBitMapIndex + GetParam().SendBitMapStartAddr < 0)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[P->C] BitMap Index: {eSendBitMapIndex} + Start: {GetParam().SendBitMapStartAddr} >= Length: {Master.m_Port_SendBitMap.Length}");

                return false;
            }

            Master.m_Port_SendBitMap[(int)eSendBitMapIndex + GetParam().SendBitMapStartAddr] = value;
            return true;
        }

        /// <summary>
        /// Port -> CIM Word Map에 Data를 적용
        /// Get, Set 구조의 매개 변수에서 Set시 적용 되도록 구성
        /// </summary>
        /// <param name="eSendWordMapIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set_Port_2_CIM_Word_Data(SendWordMapIndex eSendWordMapIndex, object value)
        {
            try
            {
                if ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr >= Master.m_Port_SendWordMap.Length || (int)eSendWordMapIndex + GetParam().SendWordMapStartAddr < 0)
                {
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[P->C] WordMap Index: {eSendWordMapIndex} + Start: {GetParam().SendWordMapStartAddr} >= Length: {Master.m_Port_SendWordMap.Length}");
                    return false;
                }

                switch (eSendWordMapIndex)
                {
                    case SendWordMapIndex.Buffer1_Carrier_ID_01:
                    case SendWordMapIndex.Buffer2_Carrier_ID_01:
                        {
                            string str = Convert.ToString(value);
                            str = str.Replace(" ", string.Empty);
                            if (string.IsNullOrEmpty(str))
                            {
                                byte[] DataArray = new byte[58 * sizeof(short)];
                                int WriteSize = DataArray.Length;
                                Buffer.BlockCopy(DataArray, 0, Master.m_Port_SendWordMap, ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr) * sizeof(short), WriteSize);
                            }
                            else
                            {
                                byte[] DataArray = Encoding.UTF8.GetBytes(str);
                                int WriteSize = DataArray.Length;
                                if (DataArray.Length > 58 * sizeof(short))
                                {
                                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.CasseteIDOverFlow, $"{DataArray.Length} > {58 * sizeof(short)}");
                                    WriteSize = 58 * sizeof(short); //Max
                                }
                                Buffer.BlockCopy(DataArray, 0, Master.m_Port_SendWordMap, ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr) * sizeof(short), WriteSize);
                            }
                        }
                        break;
                    case SendWordMapIndex.X_Axis_TargetPosition_0:
                    case SendWordMapIndex.X_Axis_CurrentPosition_0:
                    case SendWordMapIndex.Z_Axis_TargetPosition_0:
                    case SendWordMapIndex.Z_Axis_CurrentPosition_0:
                    case SendWordMapIndex.T_Axis_TargetPosition_0:
                    case SendWordMapIndex.T_Axis_CurrentPosition_0:
                        {
                            byte[] DataArray = BitConverter.GetBytes((float)value);
                            Buffer.BlockCopy(DataArray, 0, Master.m_Port_SendWordMap, ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr) * sizeof(short), DataArray.Length);
                        }
                        break;
                    default:
                        Master.m_Port_SendWordMap[(int)eSendWordMapIndex + GetParam().SendWordMapStartAddr] = Convert.ToInt16(value);
                        break;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Port -> CIM Bit, Word Send는 사용 X
        /// CIM에서 Control Msg 수신 시 전체 메모리 맵 반복 일괄 전송
        /// </summary>
        /// <param name="eSendBitMapIndex"></param>
        public void Send_Port_2_CIM_Bit_Data(SendBitMapIndex eSendBitMapIndex)
        {
            try
            {
                if (!IsMapDefineIndex(eSendBitMapIndex))
                    return;

                int DataArrayLength = sizeof(byte);
                short BitMapOffset = (short)((int)eSendBitMapIndex + GetParam().SendBitMapStartAddr);
                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Port_Bit_Data };
                byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);

                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_Port_SendBitMap, BitMapOffset, Data, 0, DataArrayLength);


                if (ProtocolRoles.Send_LittleEndian)
                {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < Data.Length / sizeof(byte); nCount++)
                    {
                        Array.Reverse(Data, nCount * sizeof(byte), sizeof(byte));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                Master.GetCIMPt()?.SendData(send.ToArray());
            }
            catch
            {

            }
        }
        public void Send_Port_2_CIM_Bit_Data(SendBitMapIndex eSendBitMapIndex, int BitMapSize)
        {
            try
            {
                int DataArrayLength = sizeof(byte) * BitMapSize;
                short BitMapOffset = (short)((int)eSendBitMapIndex + GetParam().SendBitMapStartAddr);
                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Port_Bit_Data };
                byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);

                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_Port_SendBitMap, BitMapOffset, Data, 0, DataArrayLength);


                if (ProtocolRoles.Send_LittleEndian)
                {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < Data.Length / sizeof(byte); nCount++)
                    {
                        Array.Reverse(Data, nCount * sizeof(byte), sizeof(byte));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                Master.GetCIMPt()?.SendData(send.ToArray());
            }
            catch
            {

            }
        }
        public void Send_Port_2_CIM_Word_Data(SendWordMapIndex eSendWordMapIndex)
        {
            try
            {
                if (!IsMapDefineIndex(eSendWordMapIndex))
                    return;

                int DataArrayLength = sizeof(short);
                short WordMapOffset = (short)((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr);

                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Port_Word_Data };
                byte[] DataOffset = BitConverter.GetBytes(WordMapOffset);

                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_Port_SendWordMap, WordMapOffset * sizeof(short), Data, 0, DataArrayLength);


                if (ProtocolRoles.Send_LittleEndian)
                {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < Data.Length / sizeof(short); nCount++)
                    {
                        Array.Reverse(Data, nCount * sizeof(short), sizeof(short));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                Master.GetCIMPt()?.SendData(send.ToArray());
            }
            catch
            {

            }
        }
        public void Send_Port_2_CIM_Word_Data(SendWordMapIndex eSendWordMapIndex, int WordMapSize)
        {
            try
            {
                int DataArrayLength = sizeof(short) * WordMapSize;
                short WordMapOffset = (short)((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr);

                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Port_Word_Data };
                byte[] DataOffset = BitConverter.GetBytes(WordMapOffset);

                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_Port_SendWordMap, WordMapOffset * sizeof(short), Data, 0, DataArrayLength);


                if (ProtocolRoles.Send_LittleEndian)
                {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < Data.Length / sizeof(short); nCount++)
                    {
                        Array.Reverse(Data, nCount * sizeof(short), sizeof(short));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                Master.GetCIMPt()?.SendData(send.ToArray());
            }
            catch
            {

            }
        }


        /// <summary>
        /// CIM -> Port Memory Map 영역이지만 CV Control 명령이 따로 CIM에서 전송되지는 않으므로 Master에서 해당 맵에 적용 중이며 삭제해도 무방
        /// </summary>
        /// <param name="eSensorMapIndex"></param>
        /// <param name="SensorBitNum"></param>
        /// <param name="SetValue"></param>
        private void BufferCtrl_SetBitInWord(ReceiveWordMapIndex eSensorMapIndex, int SensorBitNum, bool SetValue)
        {
            switch (eSensorMapIndex)
            {
                case ReceiveWordMapIndex.Buffer1_Control_0:
                case ReceiveWordMapIndex.Buffer2_Control_0:
                case ReceiveWordMapIndex.Buffer_SyncMove:
                    break;
                default:
                    return;
            }

            short SensorValue = (short)Get_CIM_2_Port_Word_Data(eSensorMapIndex);

            BitOperation.SetBit(ref SensorValue, SensorBitNum, SetValue);

            Set_CIM_2_Port_Word_Data(eSensorMapIndex, SensorValue);
        }

        /// <summary>
        /// Port Data Send 시 Enum Define 체크 영역이지만 Port에서 Send하지 않으므로 미사용
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <returns></returns>
        bool IsMapDefineIndex(ReceiveBitMapIndex eReceiveBitMap)
        {
            if (Enum.IsDefined(typeof(ReceiveBitMapIndex), eReceiveBitMap))
                return true;

            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->P] BitMap Number: {(int)eReceiveBitMap}");
            return false;
        }
        bool IsMapDefineIndex(ReceiveWordMapIndex eReceiveWordMapIndex)
        {
            if (Enum.IsDefined(typeof(ReceiveWordMapIndex), eReceiveWordMapIndex))
                return true;

            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->P] WordMap Number: {(int)eReceiveWordMapIndex}");
            return false;
        }
        bool IsMapDefineIndex(SendBitMapIndex eSendBitMapIndex)
        {
            if (Enum.IsDefined(typeof(SendBitMapIndex), eSendBitMapIndex))
                return true;

            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[P->C] BitMap Number: {(int)eSendBitMapIndex}");
            return false;
        }
        bool IsMapDefineIndex(SendWordMapIndex eSendWordMapIndex)
        {
            if (Enum.IsDefined(typeof(SendWordMapIndex), eSendWordMapIndex))
                return true;

            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[P->C] WordMap Number: {(int)eSendWordMapIndex}");
            return false;
        }
    }
}
