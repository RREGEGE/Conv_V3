using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.TCP;
using Master.ManagedFile;
using System.Threading;

namespace Master.Equipment.RackMaster
{
    /// <summary>
    /// RackMasterAction.cs는 STK Memory Map 및 통신 관련 행동 작성
    /// </summary>
    public partial class RackMaster
    {
        /// <summary>
        /// STK Memory Map 정의
        /// </summary>
        public enum ReceiveBitMapIndex
        {
            CMD_EmergencyStop = 0x00,
            CMD_RackMaster_ServoOn,
            
            CMD_RackMaster_ServoOff = 0x03,

            CMD_RackMaster_AutoMode_Run = 0x05,

            CMD_RackMaster_AutoMode_Stop = 0x07,

            CMD_RackMaster_ErrorReset = 0x09,
            CMD_SoftErrorOn,

            CMD_RackMaster_TimeSync = 0x10,

            CMD_RackMaster_AutoTeaching_Run = 0x15,
            CMD_RackMaster_AutoTeaching_Complete_ACK,
            CMD_RackMaster_AutoTeaching_ErrorComplete_ACK,
            CMD_RackMaster_AutoTeaching_Stop,

            CMD_RackMaster_From_Run = 0x20,
            CMD_RackMaster_From_Complete_ACK,
            CMD_RackMaster_To_Run,
            CMD_RackMaster_To_Complete_ACK,

            CMD_RackMaster_Maint_Run = 0x26,
            CMD_RackMaster_Maint_Complete_ACK,

            CMD_RackMaster_StoreAlt_ACK = 0x2B,
            CMD_RackMaster_SourceEmpty_ACK,
            CMD_RackMaster_DoubleStorage_ACK,
            CMD_RackMaster_ResumeRequest_ACK,

            CMD_RackMaster_Teaching_RW_Run = 0x30,

            CMD_HP_DoorOpen_Status = 0x40,
            CMD_OP_DoorOpen_Status,
            CMD_HP_EMO_Pushing,
            CMD_OP_EMO_Pushing,
            CMD_HP_Escape_Pushing,
            CMD_OP_Escape_Pushing,
            CMD_RM_Key_AutoStatus,
            CMD_RM_HP_Handy_EMO_Push,
            CMD_RM_OP_Handy_EMO_Push,

            CMD_RackMaster_X_Axis_MaxLoad_Reset = 0x66,
            CMD_RackMaster_Z_Axis_MaxLoad_Reset,
            CMD_RackMaster_A_Axis_MaxLoad_Reset,
            CMD_RackMaster_T_Axis_MaxLoad_Reset,

            CMD_RackMaster_X_Axis_MaxLoadLimit_Apply = 0x6b,
            CMD_RackMaster_Z_Axis_MaxLoadLimit_Apply,
            CMD_RackMaster_A_Axis_MaxLoadLimit_Apply,
            CMD_RackMaster_T_Axis_MaxLoadLimit_Apply,

            CMD_RackMaster_X_Axis_AutoHome_Reset = 0x76,
            CMD_RackMaster_Z_Axis_AutoHome_Reset,
            CMD_RackMaster_A_Axis_AutoHome_Reset,
            CMD_RackMaster_T_Axis_AutoHome_Reset,

            CMD_RackMaster_X_Axis_AutoHomeCount_Apply = 0x7b,
            CMD_RackMaster_Z_Axis_AutoHomeCount_Apply,
            CMD_RackMaster_A_Axis_AutoHomeCount_Apply,
            CMD_RackMaster_T_Axis_AutoHomeCount_Apply,

            CMD_PIO_L_REQ = 0xd0,
            CMD_PIO_UL_REQ,
            CMD_PIO_Ready,
            CMD_PIO_PortError
        }
        public enum SendBitMapIndex
        {
            Status_ServoOn_Ready = 0x00,
            Status_ServoOn,
            Status_ServoOff_Ready,
            Status_ServoOff,
            Status_AutoMode_Ready,
            Status_AutoMode,
            Status_ManualMode_Ready,
            Status_ManualMode,

            Status_Error = 0x09,
            Status_HomeDone,

            Status_FAN_1_ON = 0x0E,
            Status_FAN_2_ON,

            Status_AutoTeaching_RunningState = 0x14,
            Status_AutoTeaching_Run_ACK,
            Status_AutoTeaching_Complete,
            Status_AutoTeaching_ErrorComplete,

            Status_From_Run_ACK = 0x20,
            Status_From_Complete,
            Status_To_Run_ACK,
            Status_To_Run_Complete,

            Status_Maint_Run_ACK = 0x26,
            Status_Maint_Complete,

            Status_StoreAlt_Request = 0x2b,
            Status_SourceEmpty_Request,
            Status_DoubleStorage_Request,
            Status_Resume_Request,

            Status_Teaching_RW_Ready = 0x30,
            Status_Teaching_ReadComplete,

            Status_Down_From_In_Access = 0x33,
            Status_Down_To_In_Access,

            Status_FromMove_Wait = 0x3a,
            Status_ToMove_Wait,
            Status_Idle,
            Status_Active,

            Status_XZY_Going = 0x40,

            Status_MaintMoving = 0x43,
            Status_FromMoving,
            Status_ToMoving,

            Status_X_Axis_ForwardMoving = 0x48,
            Status_X_Axis_BackwardMoving,
            Status_X_Axis_SpeedMoving,
            Status_Arm_Axis_Home,
            Status_Turn_Axis_LeftPos,
            Status_Turn_Axis_RightPos,
            Status_Cassette_Not_Exist,
            Status_Cassette_RightPos,
            Status_X_Axis_PowerOn,
            Status_X_Axis_ServoOn,
            Status_X_Axis_HomeDone,
            Status_Z_Axis_PowerOn,
            Status_Z_Axis_ServoOn,
            Status_Z_Axis_HomeDone,
            Status_A_Axis_PowerOn,
            Status_A_Axis_ServoOn,
            Status_A_Axis_HomeDone,
            Status_T_Axis_PowerOn,
            Status_T_Axis_ServoOn,
            Status_T_Axis_HomeDone,

            Status_DoubleImport_2 = 0x62,
            Status_EStop_Pushing = 0x63,
            Status_GOT_EMO_Pushing,
            Status_DoubleImport_1,
            Status_Arm_CST_All_Undetected,
            Status_Arm_CST_1_Detecting,
            Status_Arm_CST_2_Detecting,
            Status_Arm_CST_Cross_Detecting,
            Status_From_Left_Sensor,
            Status_To_Left_Sensor,
            Status_From_Right_Sensor,
            Status_To_Right_Sensor,

            Status_Bulge_Left_1_Detecting,
            Status_Bulge_Left_2_Detecting,
            Status_Bulge_Right_1_Detecting,
            Status_Bulge_Right_2_Detecting,
            Status_Z_Axis_HP_HomeSensor,
            Status_Z_Axis_HP_NOTSensor,
            Status_Z_Axis_HP_POTSensor,
            Status_Maint_Stopper_1,
            Status_Maint_Stopper_2,
            Z_Axis_Wire_Cut_Status,
            Status_HP_EMO_Pushing,
            Status_OP_EMO_Pushing,

            MST_DTP_Mode_Select_1 = 0x7c,
            MST_DTP_Mode_Select_2,

            Status_GOT_Detecting = 0x80,
            Status_PositionSensor_1,
            Status_PositionSensor_2,
            Status_X_Axis_HP_HomeSensor,
            Status_X_Axis_HP_SlowSensor,
            Status_X_Axis_HP_NOTSensor,
            Status_X_Axis_OP_SlowSensor,
            Status_X_Axis_OP_POTSensor,
            Status_A_Axis_HomeSensor,
            Status_A_Axis_NOTSensor,
            Status_A_Axis_POTSensor,
            Status_A_Axis_Pos1Sensor,
            Status_A_Axis_Pos2Sensor,
            Status_A_Axis_Pos3Sensor,
            Status_T_Axis_HomeSensor,
            Status_T_Axis_NOTSensor,
            Status_T_Axis_POTSensor,
            Status_T_Axis_PosSensor,

            Status_CPS_2ND_Run = 0x9a,
            Status_CPS_2ND_Fault,

            Status_FromMove_Step0_Done = 0xa0,
            Status_FromMove_Step1_Done,
            Status_FromMove_Step2_Done,
            Status_FromMove_Step3_Done,
            Status_FromMove_Step4_Done,
            Status_ToMove_Step0_Done,
            Status_ToMove_Step1_Done,
            Status_ToMove_Step2_Done,
            Status_ToMove_Step3_Done,
            Status_ToMove_Step4_Done,

            Status_Communication = 0xab,
            Status_X_Axis_Busy,
            Status_Z_Axis_Busy,
            Status_T_Axis_Busy,
            Status_A_Axis_Busy,

            Status_PIO_TR_REQ = 0xd0,
            Status_PIO_Busy,
            Status_PIO_Complete,
            Status_PIO_STK_Error
        }
        public enum ReceiveWordMapIndex
        {
            CassetteID_PIO_Word_0,
            CassetteID_PIO_Word_1,
            CassetteID_PIO_Word_2,
            CassetteID_PIO_Word_3,
            CassetteID_PIO_Word_4,
            CassetteID_PIO_Word_5,
            CassetteID_PIO_Word_6,
            CassetteID_PIO_Word_7,
            CassetteID_PIO_Word_8,
            CassetteID_PIO_Word_9,
            CassetteID_PIO_Word_10,
            CassetteID_PIO_Word_11,
            CassetteID_PIO_Word_12,
            CassetteID_PIO_Word_13,
            CassetteID_PIO_Word_14,
            CassetteID_PIO_Word_15,
            CassetteID_PIO_Word_16,
            CassetteID_PIO_Word_17,
            CassetteID_PIO_Word_18,
            CassetteID_PIO_Word_19,
            CassetteID_PIO_Word_20,
            CassetteID_PIO_Word_21,
            CassetteID_PIO_Word_22,
            CassetteID_PIO_Word_23,
            CassetteID_PIO_Word_24,
            CassetteID_PIO_Word_25,
            CassetteID_PIO_Word_26,
            CassetteID_PIO_Word_27,
            CassetteID_PIO_Word_28,
            CassetteID_PIO_Word_29,
            CassetteID_PIO_Word_30,
            CassetteID_PIO_Word_31,
            CassetteID_PIO_Word_32,
            CassetteID_PIO_Word_33,
            CassetteID_PIO_Word_34,
            CassetteID_PIO_Word_35,
            CassetteID_PIO_Word_36,
            CassetteID_PIO_Word_37,
            CassetteID_PIO_Word_38,
            CassetteID_PIO_Word_39,
            CassetteID_PIO_Word_40,
            CassetteID_PIO_Word_41,
            CassetteID_PIO_Word_42,
            CassetteID_PIO_Word_43,
            CassetteID_PIO_Word_44,
            CassetteID_PIO_Word_45,
            CassetteID_PIO_Word_46,
            CassetteID_PIO_Word_47,
            CassetteID_PIO_Word_48,
            CassetteID_PIO_Word_49,
            CassetteID_PIO_Word_50,
            CassetteID_PIO_Word_51,
            CassetteID_PIO_Word_52,
            CassetteID_PIO_Word_53,
            CassetteID_PIO_Word_54,
            CassetteID_PIO_Word_55,
            CassetteID_PIO_Word_56,
            CassetteID_PIO_Word_57,
            //CassetteID_PIO_Word_58,
            //CassetteID_PIO_Word_59,
            //CassetteID_PIO_Word_60,
            //CassetteID_PIO_Word_61,
            //CassetteID_PIO_Word_62,
            //CassetteID_PIO_Word_63,
            Year = 64,
            Month,
            Day,
            Hour,
            Min,
            Sec,
            Day_of_Week,

            From_Shelf_ID_0 = 0x50,
            Form_Shelf_ID_1,
            To_Shelf_ID_0,
            TO_Shelf_ID_1,

            Inventry_Shelf_ID_0 = 0x58,
            Inventry_Shelf_ID_1,
            Teaching_RW_ID_0 = 0x5a,
            Teaching_RW_ID_1,
            AutoTeaching_ID_0,
            AutoTeaching_ID_1,
            AutoTeaching_X_Axis_Data,
            AutoTeaching_Z_Axis_Data,

            X_Axis_Speed = 0x6c,
            Z_Axis_Speed,
            A_Axis_Speed,
            T_Axis_Speed,

            X_Axis_MaxLoadValue = 0x71,
            Z_Axis_MaxLoadValue,
            A_Axis_MaxLoadValue,
            T_Axis_MaxLoadValue,

            WatchDog_Word = 0xa4
        }
        public enum SendWordMapIndex
        {
            CassetteID_PIO_Word_0,
            CassetteID_PIO_Word_1,
            CassetteID_PIO_Word_2,
            CassetteID_PIO_Word_3,
            CassetteID_PIO_Word_4,
            CassetteID_PIO_Word_5,
            CassetteID_PIO_Word_6,
            CassetteID_PIO_Word_7,
            CassetteID_PIO_Word_8,
            CassetteID_PIO_Word_9,
            CassetteID_PIO_Word_10,
            CassetteID_PIO_Word_11,
            CassetteID_PIO_Word_12,
            CassetteID_PIO_Word_13,
            CassetteID_PIO_Word_14,
            CassetteID_PIO_Word_15,
            CassetteID_PIO_Word_16,
            CassetteID_PIO_Word_17,
            CassetteID_PIO_Word_18,
            CassetteID_PIO_Word_19,
            CassetteID_PIO_Word_20,
            CassetteID_PIO_Word_21,
            CassetteID_PIO_Word_22,
            CassetteID_PIO_Word_23,
            CassetteID_PIO_Word_24,
            CassetteID_PIO_Word_25,
            CassetteID_PIO_Word_26,
            CassetteID_PIO_Word_27,
            CassetteID_PIO_Word_28,
            CassetteID_PIO_Word_29,
            CassetteID_PIO_Word_30,
            CassetteID_PIO_Word_31,
            CassetteID_PIO_Word_32,
            CassetteID_PIO_Word_33,
            CassetteID_PIO_Word_34,
            CassetteID_PIO_Word_35,
            CassetteID_PIO_Word_36,
            CassetteID_PIO_Word_37,
            CassetteID_PIO_Word_38,
            CassetteID_PIO_Word_39,
            CassetteID_PIO_Word_40,
            CassetteID_PIO_Word_41,
            CassetteID_PIO_Word_42,
            CassetteID_PIO_Word_43,
            CassetteID_PIO_Word_44,
            CassetteID_PIO_Word_45,
            CassetteID_PIO_Word_46,
            CassetteID_PIO_Word_47,
            CassetteID_PIO_Word_48,
            CassetteID_PIO_Word_49,
            CassetteID_PIO_Word_50,
            CassetteID_PIO_Word_51,
            CassetteID_PIO_Word_52,
            CassetteID_PIO_Word_53,
            CassetteID_PIO_Word_54,
            CassetteID_PIO_Word_55,
            CassetteID_PIO_Word_56,
            CassetteID_PIO_Word_57,
            //CassetteID_PIO_Word_58,
            //CassetteID_PIO_Word_59,
            //CassetteID_PIO_Word_60,
            //CassetteID_PIO_Word_61,
            //CassetteID_PIO_Word_62,
            //CassetteID_PIO_Word_63,

            X_Axis_CurrentPosition_0 = 0x40,
            X_Axis_CurrentPosition_1,
            X_Axis_TargetPosition_0,
            X_Axis_TargetPosition_1,
            X_Axis_CurrentSpeed_0,
            X_Axis_CurrentSpeed_1,
            X_Axis_CurrentMaxLoad,
            X_Axis_SetMaxLoad,
            X_Axis_AutoHomeCurrentCount,
            X_Axis_AutoHomeApplyValue,
            X_Axis_Accelation,
            X_Axis_Decelation,
            X_Axis_CurrentTorque,

            Z_Axis_CurrentPosition_0 = 0x50,
            Z_Axis_CurrentPosition_1,
            Z_Axis_TargetPosition_0,
            Z_Axis_TargetPosition_1,
            Z_Axis_CurrentSpeed_0,
            Z_Axis_CurrentSpeed_1,
            Z_Axis_CurrentMaxLoad,
            Z_Axis_SetMaxLoad,
            Z_Axis_Accelation = 0x5A,
            Z_Axis_Decelation,
            Z_Axis_CurrentTorque,

            A_Axis_CurrentPosition_0 = 0x60,
            A_Axis_CurrentPosition_1,
            A_Axis_TargetPosition_0,
            A_Axis_TargetPosition_1,
            A_Axis_CurrentSpeed_0,
            A_Axis_CurrentSpeed_1,
            A_Axis_CurrentMaxLoad,
            A_Axis_SetMaxLoad,
            A_Axis_Accelation = 0x6A,
            A_Axis_Decelation,
            A_Axis_CurrentTorque,

            T_Axis_CurrentPosition_0 = 0x70,
            T_Axis_CurrentPosition_1,
            T_Axis_TargetPosition_0,
            T_Axis_TargetPosition_1,
            T_Axis_CurrentSpeed_0,
            T_Axis_CurrentSpeed_1,
            T_Axis_CurrentMaxLoad,
            T_Axis_SetMaxLoad,
            T_Axis_Accelation = 0x7A,
            T_Axis_Decelation,
            T_Axis_CurrentTorque,

            X_Axis_SetSpeedPercent = 0x80,
            Z_Axis_SetSpeedPercent,
            A_Axis_SetSpeedPercent,
            T_Axis_SetSpeedPercent,
            AutoTeaching_ID_0,
            AutoTeaching_ID_1,
            AutoTeaching_X_Axis_Data,
            AutoTeaching_Z_Axis_Data,

            ErrorWord_0 = 0x90,
            ErrorWord_1,
            ErrorWord_2,
            ErrorWord_3,
            ErrorWord_4,
            ErrorWord_5,
            ErrorWord_6,
            ErrorWord_7,
            ErrorWord_8,
            ErrorWord_9,
            ErrorWord_10,
            ErrorWord_11,
            ErrorWord_12,

            X_Axis_Teaching_Data_0 = 0xA2,
            X_Axis_Teaching_Data_1,
            Z_Axis_Teaching_Data_0,
            Z_Axis_Teaching_Data_1,
            A_Axis_Teaching_Data_0,
            A_Axis_Teaching_Data_1,
            T_Axis_Teaching_Data_0,
            T_Axis_Teaching_Data_1,

            From_Shelf_ID_0 = 0xAC,
            From_Shelf_ID_1,
            To_Shelf_ID_0,
            To_Shelf_ID_1,

            X_Axis_Average_Torque = 0xB0,
            X_Axis_Peak_Torque,
            Z_Axis_Average_Torque,
            Z_Axis_Peak_Torque,
            A_Axis_Average_Torque,
            A_Axis_Peak_Torque,
            T_Axis_Average_Torque,
            T_Axis_Peak_Torque,

            WatchDog_WordData = 0xBA,
            X_Axis_Current_Limit_Torque,
            Z_Axis_Current_Limit_Torque,
            A_Axis_Current_Limit_Torque,
            T_Axis_Current_Limit_Torque,

            AutoStep_Number = 0xC0,

            Access_Shelf_ID_0 = 0xC2,
            Access_Shelf_ID_1,
            X_Axis_Cumulative_Distance_0,
            X_Axis_Cumulative_Distance_1,
            Z_Axis_Cumulative_Distance_0,
            Z_Axis_Cumulative_Distance_1,
            A_Axis_Cumulative_Distance_0,
            A_Axis_Cumulative_Distance_1,
            T_Axis_Cumulative_Distance_0,
            T_Axis_Cumulative_Distance_1,

            CPS_2_Regulator_InputVoltage = 0xD0,
            CPS_2_Regulator_OutputVoltage,
            CPS_2_Regulator_OutputCurrent,
            CPS_2_Regulator_Pickup_Temp,
            CPS_2_Regulator_HeatSink_Temp,
            CPS_2_Regulator_Inner_Temp
        }

        
        /// <summary>
        /// Master에서 STK 통신 패킷 수신 시 동작 행위
        /// </summary>
        /// <param name="receivePackets"></param>
        void ReceiveAcition(byte[] receivePackets)
        {
            Master.RMWatchDog.ReStartWatchdog(); //RM Packet 미 수신 시 와치독 알람 생성

            sbyte DataType = ProtocolRoles.GetValue_DataType(receivePackets);
            short DataMapAddress = ProtocolRoles.GetValue_DataMapAddress(receivePackets);


            switch ((ProtocolRoles.DataType)DataType)
            {
                case ProtocolRoles.DataType.Controller_2_STK_CIM_RM_Bit_Data:
                    Parsing_RackMaster_2_CIM_Bit_Data(DataMapAddress, receivePackets);
                    break;
                case ProtocolRoles.DataType.Controller_2_STK_CIM_RM_Word_Data:
                    Parsing_RackMaster_2_CIM_Word_Data(DataMapAddress, receivePackets);
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// STK -> Master Packet 수신 시 파싱
        /// STK에서 사용되는 Bit 변수에 값 적용
        /// </summary>
        /// <param name="DataMapAddress"></param>
        /// <param name="receivePackets"></param>
        void Parsing_RackMaster_2_CIM_Bit_Data(short DataMapAddress, byte[] receivePackets)
        {
            if (DataMapAddress >= Master.m_RackMaster_SendBitMap.Length) //맵 주소가 메모리 맵 사이즈 보다 큰 경우 리턴
                return;

            if (DataMapAddress < GetParam().SendBitMapStartAddr)
                DataMapAddress += (short)GetParam().SendBitMapStartAddr;

            byte[] receive_values = ProtocolRoles.GetReceiveDataArray(receivePackets, false);
            Buffer.BlockCopy(receive_values, 0, Master.m_RackMaster_SendBitMap, DataMapAddress, receivePackets.Length - ProtocolRoles.Send_TCPHeaderLen);

            RackMasterBitUpdateEvent(); //STK Bit 변수에 값 적용
        }

        /// <summary>
        /// STK -> Master Packet 수신 시 파싱
        /// STK에서 사용되는 Word 변수에 값 적용
        /// </summary>
        /// <param name="DataMapAddress"></param>
        /// <param name="receivePackets"></param>
        void Parsing_RackMaster_2_CIM_Word_Data(short DataMapAddress, byte[] receivePackets)
        {
            if (DataMapAddress >= Master.m_RackMaster_SendWordMap.Length) //맵 주소가 메모리 맵 사이즈 보다 큰 경우 리턴
                return;

            if (DataMapAddress < GetParam().SendWordMapStartAddr)
                DataMapAddress += (short)GetParam().SendWordMapStartAddr;

            byte[] receive_values = ProtocolRoles.GetReceiveDataArray(receivePackets);
            byte[] current_values = ProtocolRoles.GetCurrentWordDataToByteArray(DataMapAddress, receivePackets.Length - ProtocolRoles.Send_TCPHeaderLen, Master.m_RackMaster_SendWordMap);

            if (!ProtocolRoles.IsByteArrayCompare(receive_values, current_values))
                Buffer.BlockCopy(receive_values, 0, Master.m_RackMaster_SendWordMap, DataMapAddress * sizeof(short), receivePackets.Length - ProtocolRoles.Send_TCPHeaderLen);

            RackMasterWordUpdateEvent(); //STK Word 변수에 값 적용
        }

        
        /// <summary>
        /// CIM -> Master 수신한 경우 Master -> STK 전달 동작
        /// 대부분의 경우 바이패스 진행하지만 AutoMode Run의 경우 인터락 체크 진행
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        public void Action_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex eReceiveBitMap)
        {
            eReceiveBitMap = (ReceiveBitMapIndex)((int)eReceiveBitMap - GetParam().RecvBitMapStartAddr);

            switch (eReceiveBitMap)
            {
                case ReceiveBitMapIndex.CMD_EmergencyStop:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_ServoOn:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_ServoOff:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_AutoMode_Run:
                    if (Master.CMD_DoorOpen_REQ)
                    {
                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.RackMasterAutoModeInterlock, $"Door Open Command Is On");
                        return;
                    }
                    if(Master.Sensor_HPDoorOpen)
                    {
                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.RackMasterAutoModeInterlock, $"HP Door Is Open");
                        return;
                    }
                    if (Master.Sensor_OPDoorOpen)
                    {
                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.RackMasterAutoModeInterlock, $"OP Door Is Open");
                        return;
                    }
                    if (!Master.Sensor_HPAutoKey)
                    {
                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.RackMasterAutoModeInterlock, $"HP Master Key Is Manual");
                        return;
                    }
                    if (Master.mHPInnerEscape_EStop.IsEStop())
                    {
                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.RackMasterAutoModeInterlock, $"HP EMO Escape Is On");
                        return;
                    }
                    if (Master.mOPInnerEscape_EStop.IsEStop())
                    {
                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.RackMasterAutoModeInterlock, $"OP EMO Escape Is On");
                        return;
                    }
                    if (Master.mHPOutSide_EStop.IsEStop())
                    {
                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.RackMasterAutoModeInterlock, $"HP EMO Is On");
                        return;
                    }
                    if (Master.mOPOutSide_EStop.IsEStop())
                    {
                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.RackMasterAutoModeInterlock, $"OP EMO Is On");
                        return;
                    }
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_AutoMode_Stop:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_ErrorReset:
                    break;
                case ReceiveBitMapIndex.CMD_SoftErrorOn:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_TimeSync:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_AutoTeaching_Run:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_AutoTeaching_Complete_ACK:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_AutoTeaching_ErrorComplete_ACK:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_From_Run:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_From_Complete_ACK:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_To_Run:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_To_Complete_ACK:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_Maint_Run:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_Maint_Complete_ACK:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_StoreAlt_ACK:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_SourceEmpty_ACK:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_DoubleStorage_ACK:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_ResumeRequest_ACK:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_Teaching_RW_Run:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_X_Axis_MaxLoad_Reset:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_Z_Axis_MaxLoad_Reset:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_A_Axis_MaxLoad_Reset:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_T_Axis_MaxLoad_Reset:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_X_Axis_MaxLoadLimit_Apply:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_Z_Axis_MaxLoadLimit_Apply:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_A_Axis_MaxLoadLimit_Apply:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_T_Axis_MaxLoadLimit_Apply:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_X_Axis_AutoHome_Reset:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_Z_Axis_AutoHome_Reset:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_A_Axis_AutoHome_Reset:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_T_Axis_AutoHome_Reset:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_X_Axis_AutoHomeCount_Apply:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_Z_Axis_AutoHomeCount_Apply:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_A_Axis_AutoHomeCount_Apply:
                    break;
                case ReceiveBitMapIndex.CMD_RackMaster_T_Axis_AutoHomeCount_Apply:
                    break;
                case ReceiveBitMapIndex.CMD_PIO_L_REQ:
                    break;
                case ReceiveBitMapIndex.CMD_PIO_UL_REQ:
                    break;
                case ReceiveBitMapIndex.CMD_PIO_Ready:
                    break;
                case ReceiveBitMapIndex.CMD_PIO_PortError:
                    break;
                default:
                    LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->R] BitMap Number: {(int)eReceiveBitMap}");
                    break;
            }

            if (IsPassing(eReceiveBitMap))
            {
                Send_CIM_2_RackMaster_Bit_Data(eReceiveBitMap);
            }
        }
        
        /// <summary>
        /// CIM -> Master 수신한 경우 Master -> STK 전달 동작
        /// Packet Array 바이패스 (초기 단일 Word로 전송되는 줄 알았지만 메모리 맵 통째로 전송되어 추가 된 구조)
        /// </summary>
        /// <param name="eReceiveWordMapIndex"></param>
        /// <param name="DtArray"></param>
        public void Action_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex eReceiveWordMapIndex, byte[] DtArray)
        {
            eReceiveWordMapIndex = (ReceiveWordMapIndex)((int)eReceiveWordMapIndex - GetParam().RecvWordMapStartAddr);

            switch (eReceiveWordMapIndex)
            {
                case ReceiveWordMapIndex.CassetteID_PIO_Word_0:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_1:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_2:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_3:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_4:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_5:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_6:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_7:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_8:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_9:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_10:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_11:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_12:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_13:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_14:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_15:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_16:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_17:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_18:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_19:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_20:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_21:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_22:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_23:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_24:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_25:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_26:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_27:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_28:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_29:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_30:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_31:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_32:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_33:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_34:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_35:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_36:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_37:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_38:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_39:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_40:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_41:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_42:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_43:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_44:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_45:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_46:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_47:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_48:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_49:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_50:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_51:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_52:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_53:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_54:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_55:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_56:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_57:
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_58:
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_59:
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_60:
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_61:
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_62:
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_63:
                    break;
                case ReceiveWordMapIndex.Year:
                    break;
                case ReceiveWordMapIndex.Month:
                    break;
                case ReceiveWordMapIndex.Day:
                    break;
                case ReceiveWordMapIndex.Hour:
                    break;
                case ReceiveWordMapIndex.Min:
                    break;
                case ReceiveWordMapIndex.Sec:
                    break;
                case ReceiveWordMapIndex.Day_of_Week:
                    break;
                case ReceiveWordMapIndex.From_Shelf_ID_0:
                    break;
                case ReceiveWordMapIndex.Form_Shelf_ID_1:
                    break;
                case ReceiveWordMapIndex.To_Shelf_ID_0:
                    break;
                case ReceiveWordMapIndex.TO_Shelf_ID_1:
                    break;
                case ReceiveWordMapIndex.Inventry_Shelf_ID_0:
                    break;
                case ReceiveWordMapIndex.Inventry_Shelf_ID_1:
                    break;
                case ReceiveWordMapIndex.Teaching_RW_ID_0:
                    break;
                case ReceiveWordMapIndex.Teaching_RW_ID_1:
                    break;
                case ReceiveWordMapIndex.AutoTeaching_ID_0:
                    break;
                case ReceiveWordMapIndex.AutoTeaching_ID_1:
                    break;
                case ReceiveWordMapIndex.AutoTeaching_X_Axis_Data:
                    break;
                case ReceiveWordMapIndex.AutoTeaching_Z_Axis_Data:
                    break;
                case ReceiveWordMapIndex.X_Axis_Speed:
                    break;
                case ReceiveWordMapIndex.Z_Axis_Speed:
                    break;
                case ReceiveWordMapIndex.A_Axis_Speed:
                    break;
                case ReceiveWordMapIndex.T_Axis_Speed:
                    break;
                case ReceiveWordMapIndex.WatchDog_Word:
                    break;
                default:
                    LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->R] WordMap Number: {(int)eReceiveWordMapIndex}");
                    break;
            }

            if (IsPassing(eReceiveWordMapIndex))
            {
                SendData(DtArray);
            }
        }

        /// <summary>
        /// CIM -> Master 수신한 경우 Master -> STK 전달 동작
        /// 단일 워드 전송 구조
        /// </summary>
        /// <param name="eReceiveWordMapIndex"></param>
        public void Action_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex eReceiveWordMapIndex)
        {
            eReceiveWordMapIndex = (ReceiveWordMapIndex)((int)eReceiveWordMapIndex - GetParam().RecvWordMapStartAddr);

            switch (eReceiveWordMapIndex)
            {
                case ReceiveWordMapIndex.CassetteID_PIO_Word_0:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_1:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_2:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_3:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_4:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_5:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_6:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_7:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_8:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_9:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_10:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_11:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_12:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_13:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_14:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_15:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_16:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_17:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_18:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_19:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_20:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_21:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_22:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_23:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_24:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_25:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_26:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_27:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_28:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_29:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_30:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_31:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_32:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_33:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_34:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_35:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_36:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_37:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_38:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_39:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_40:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_41:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_42:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_43:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_44:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_45:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_46:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_47:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_48:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_49:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_50:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_51:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_52:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_53:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_54:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_55:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_56:
                case ReceiveWordMapIndex.CassetteID_PIO_Word_57:
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_58:
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_59:
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_60:
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_61:
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_62:
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_63:
                    break;
                case ReceiveWordMapIndex.Year:
                    break;
                case ReceiveWordMapIndex.Month:
                    break;
                case ReceiveWordMapIndex.Day:
                    break;
                case ReceiveWordMapIndex.Hour:
                    break;
                case ReceiveWordMapIndex.Min:
                    break;
                case ReceiveWordMapIndex.Sec:
                    break;
                case ReceiveWordMapIndex.Day_of_Week:
                    break;
                case ReceiveWordMapIndex.From_Shelf_ID_0:
                    break;
                case ReceiveWordMapIndex.Form_Shelf_ID_1:
                    break;
                case ReceiveWordMapIndex.To_Shelf_ID_0:
                    break;
                case ReceiveWordMapIndex.TO_Shelf_ID_1:
                    break;
                case ReceiveWordMapIndex.Inventry_Shelf_ID_0:
                    break;
                case ReceiveWordMapIndex.Inventry_Shelf_ID_1:
                    break;
                case ReceiveWordMapIndex.Teaching_RW_ID_0:
                    break;
                case ReceiveWordMapIndex.Teaching_RW_ID_1:
                    break;
                case ReceiveWordMapIndex.AutoTeaching_ID_0:
                    break;
                case ReceiveWordMapIndex.AutoTeaching_ID_1:
                    break;
                case ReceiveWordMapIndex.AutoTeaching_X_Axis_Data:
                    break;
                case ReceiveWordMapIndex.AutoTeaching_Z_Axis_Data:
                    break;
                case ReceiveWordMapIndex.X_Axis_Speed:
                    break;
                case ReceiveWordMapIndex.Z_Axis_Speed:
                    break;
                case ReceiveWordMapIndex.A_Axis_Speed:
                    break;
                case ReceiveWordMapIndex.T_Axis_Speed:
                    break;
                case ReceiveWordMapIndex.WatchDog_Word:
                    break;
                default:
                    LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->R] WordMap Number: {(int)eReceiveWordMapIndex}");
                    break;
            }

            if (IsPassing(eReceiveWordMapIndex))
            {
                Send_CIM_2_RackMaster_Word_Data(eReceiveWordMapIndex);
            }
        }


        /// <summary>
        /// CIM -> Master로 전송한 Packet Data의 Bit 값을 리턴
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <returns></returns>
        public bool Get_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex eReceiveBitMap)
        {
            if ((int)eReceiveBitMap + GetParam().RecvBitMapStartAddr >= Master.m_RackMaster_RecvBitMap.Length || (int)eReceiveBitMap + GetParam().RecvBitMapStartAddr < 0)
            {
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[C->R] BitMap Index: {eReceiveBitMap} + Start: {GetParam().RecvBitMapStartAddr} >= Length: {Master.m_RackMaster_RecvBitMap.Length}");

                return false;
            }

            return Master.m_RackMaster_RecvBitMap[(int)eReceiveBitMap + GetParam().RecvBitMapStartAddr];
        }

        /// <summary>
        /// CIM -> Master로 전송한 Packet Data의 Word 값을 리턴
        /// </summary>
        /// <param name="eReceiveWordMapIndex"></param>
        /// <returns></returns>
        public object Get_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex eReceiveWordMapIndex)
        {
            if ((int)eReceiveWordMapIndex + GetParam().RecvWordMapStartAddr >= Master.m_RackMaster_RecvWordMap.Length || (int)eReceiveWordMapIndex + GetParam().RecvWordMapStartAddr < 0)
            {
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[C->R] WordMap Index: {eReceiveWordMapIndex} + Start: {GetParam().RecvWordMapStartAddr} >= Length: {Master.m_RackMaster_RecvWordMap.Length}");
                return (short)-1;
            }

            switch (eReceiveWordMapIndex)
            {
                default:
                    {
                        if (eReceiveWordMapIndex >= ReceiveWordMapIndex.CassetteID_PIO_Word_0 &&
                            eReceiveWordMapIndex <= ReceiveWordMapIndex.CassetteID_PIO_Word_57)
                            eReceiveWordMapIndex = ReceiveWordMapIndex.CassetteID_PIO_Word_0;
                    }
                    break;
            }

            switch(eReceiveWordMapIndex)
            {
                case ReceiveWordMapIndex.CassetteID_PIO_Word_0:
                    {
                        int stringSize = 58 * sizeof(short);
                        byte[] stringData = new byte[stringSize];
                        Buffer.BlockCopy(Master.m_RackMaster_RecvWordMap, ((int)eReceiveWordMapIndex + GetParam().RecvWordMapStartAddr) * sizeof(short), stringData, 0, stringSize);
                        return ((string)Encoding.Default.GetString(stringData).Trim('\0')).Replace(" ", string.Empty);
                    }
                default:
                    return Master.m_RackMaster_RecvWordMap[(int)eReceiveWordMapIndex + GetParam().RecvWordMapStartAddr];
            }
        }
        
        /// <summary>
        /// STK -> Master로 전송한 Packet Data의 Bit 값을 리턴
        /// </summary>
        /// <param name="eSendBitMap"></param>
        /// <returns></returns>
        public bool Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex eSendBitMap)
        {
            if ((int)eSendBitMap + GetParam().SendBitMapStartAddr >= Master.m_RackMaster_SendBitMap.Length || (int)eSendBitMap + GetParam().SendBitMapStartAddr < 0)
            {
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[R->C] BitMap Index: {eSendBitMap} + Start: {GetParam().SendBitMapStartAddr} >= Length: {Master.m_RackMaster_SendBitMap.Length}");
                return false;
            }

            return Master.m_RackMaster_SendBitMap[(int)eSendBitMap + GetParam().SendBitMapStartAddr];
        }
        
        /// <summary>
        /// STK -> Master로 전송한 Packet Data의 Word 값을 리턴
        /// </summary>
        /// <param name="eSendWordMapIndex"></param>
        /// <returns></returns>
        public object Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex eSendWordMapIndex)
        {
            if ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr >= Master.m_RackMaster_SendWordMap.Length || (int)eSendWordMapIndex + GetParam().SendWordMapStartAddr < 0)
            {
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[R->C] WordMap Index: {eSendWordMapIndex} + Start: {GetParam().SendWordMapStartAddr} >= Length: {Master.m_RackMaster_SendWordMap.Length}");
                return (short)- 1;
            }

            //Multi Word인 경우 자리 조정
            switch (eSendWordMapIndex)
            {
                case SendWordMapIndex.X_Axis_TargetPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.X_Axis_TargetPosition_0;
                    break;
                case SendWordMapIndex.Z_Axis_TargetPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.Z_Axis_TargetPosition_0;
                    break;
                case SendWordMapIndex.A_Axis_TargetPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.A_Axis_TargetPosition_0;
                    break;
                case SendWordMapIndex.T_Axis_TargetPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.T_Axis_TargetPosition_0;
                    break;

                case SendWordMapIndex.X_Axis_CurrentPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.X_Axis_CurrentPosition_0;
                    break;
                case SendWordMapIndex.Z_Axis_CurrentPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.Z_Axis_CurrentPosition_0;
                    break;
                case SendWordMapIndex.A_Axis_CurrentPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.A_Axis_CurrentPosition_0;
                    break;
                case SendWordMapIndex.T_Axis_CurrentPosition_1:
                    eSendWordMapIndex = SendWordMapIndex.T_Axis_CurrentPosition_0;
                    break;

                case SendWordMapIndex.X_Axis_CurrentSpeed_1:
                    eSendWordMapIndex = SendWordMapIndex.X_Axis_CurrentSpeed_0;
                    break;
                case SendWordMapIndex.Z_Axis_CurrentSpeed_1:
                    eSendWordMapIndex = SendWordMapIndex.Z_Axis_CurrentSpeed_0;
                    break;
                case SendWordMapIndex.A_Axis_CurrentSpeed_1:
                    eSendWordMapIndex = SendWordMapIndex.A_Axis_CurrentSpeed_0;
                    break;
                case SendWordMapIndex.T_Axis_CurrentSpeed_1:
                    eSendWordMapIndex = SendWordMapIndex.T_Axis_CurrentSpeed_0;
                    break;

                case SendWordMapIndex.From_Shelf_ID_1:
                    eSendWordMapIndex = SendWordMapIndex.From_Shelf_ID_0;
                    break;
                case SendWordMapIndex.To_Shelf_ID_1:
                    eSendWordMapIndex = SendWordMapIndex.To_Shelf_ID_0;
                    break;
                case SendWordMapIndex.Access_Shelf_ID_1:
                    eSendWordMapIndex = SendWordMapIndex.Access_Shelf_ID_0;
                    break;

                case SendWordMapIndex.AutoTeaching_ID_1:
                    eSendWordMapIndex = SendWordMapIndex.AutoTeaching_ID_0;
                    break;

                case SendWordMapIndex.X_Axis_Teaching_Data_1:
                    eSendWordMapIndex = SendWordMapIndex.X_Axis_Teaching_Data_0;
                    break;
                case SendWordMapIndex.Z_Axis_Teaching_Data_1:
                    eSendWordMapIndex = SendWordMapIndex.Z_Axis_Teaching_Data_0;
                    break;


                default:
                    {
                        if (eSendWordMapIndex >= SendWordMapIndex.CassetteID_PIO_Word_0 &&
                            eSendWordMapIndex <= SendWordMapIndex.CassetteID_PIO_Word_57)
                            eSendWordMapIndex = SendWordMapIndex.CassetteID_PIO_Word_0;
                    }
                    break;
            }

            //-------OK
            //float[] test = new float[1] { 500.42f };
            //Buffer.BlockCopy(test, 0, Master.m_RackMaster_SendWordMap, ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr) * sizeof(short), 4);
            //-------


            //-------Error Test
            //eSendWordMapIndex = SendWordMapIndex.X_Axis_TargetPosition_0;

            //Error
            //float test = 5.143f;
            //byte[] testArray1 = BitConverter.GetBytes(test);
            //Buffer.BlockCopy(testArray1, 0, Master.m_RackMaster_SendWordMap, ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr) * sizeof(short), 4);

            //Compare
            //byte[] testArray2 = new byte[4] { 0x11, 0x22, 0x33, 0x44 };
            //Buffer.BlockCopy(testArray2, 0, Master.m_RackMaster_SendWordMap, ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr) * sizeof(short), 4);
            //-------

            switch (eSendWordMapIndex)
            {
                case SendWordMapIndex.X_Axis_TargetPosition_0:
                case SendWordMapIndex.Z_Axis_TargetPosition_0:
                case SendWordMapIndex.A_Axis_TargetPosition_0:
                case SendWordMapIndex.T_Axis_TargetPosition_0:
                case SendWordMapIndex.X_Axis_CurrentPosition_0:
                case SendWordMapIndex.Z_Axis_CurrentPosition_0:
                case SendWordMapIndex.A_Axis_CurrentPosition_0:
                case SendWordMapIndex.T_Axis_CurrentPosition_0:
                case SendWordMapIndex.X_Axis_CurrentSpeed_0:
                case SendWordMapIndex.Z_Axis_CurrentSpeed_0:
                case SendWordMapIndex.A_Axis_CurrentSpeed_0:
                case SendWordMapIndex.T_Axis_CurrentSpeed_0:
                case SendWordMapIndex.X_Axis_Teaching_Data_0:
                case SendWordMapIndex.Z_Axis_Teaching_Data_0:
                    {
                    byte[] floatData = new byte[4];
                    Buffer.BlockCopy(Master.m_RackMaster_SendWordMap, ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr) * sizeof(short), floatData, 0, 4);
                    return (float)BitConverter.ToSingle(floatData, 0);
                }
                case SendWordMapIndex.From_Shelf_ID_0:
                case SendWordMapIndex.To_Shelf_ID_0:
                case SendWordMapIndex.Access_Shelf_ID_0:
                case SendWordMapIndex.AutoTeaching_ID_0:
                    {
                        byte[] intData = new byte[4];
                        Buffer.BlockCopy(Master.m_RackMaster_SendWordMap, ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr) * sizeof(short), intData, 0, 4);
                        return (int)BitConverter.ToInt32(intData, 0);
                    }
                case SendWordMapIndex.CassetteID_PIO_Word_0:
                    {
                        int stringSize = 58 * sizeof(short);
                        byte[] stringData = new byte[stringSize];
                        Buffer.BlockCopy(Master.m_RackMaster_SendWordMap, ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr) * sizeof(short), stringData, 0, stringSize);
                        return ((string)Encoding.Default.GetString(stringData).Trim('\0')).Replace(" ", string.Empty);
                    }
                default:
                    return Master.m_RackMaster_SendWordMap[(int)eSendWordMapIndex + GetParam().SendWordMapStartAddr];
            }
        }


        /// <summary>
        /// CIM -> Master, Master -> STK 영역 Bit Map에 값 적용
        /// STK CIM Mode인 경우 CIM이 BitMap 관리
        /// STK Master Mode인 경우 Master가 BitMap 관리
        /// 최종 적으로 STK로 보내고자 하는 Bit Map 영역
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex eReceiveBitMap, bool value)
        {
            if ((int)eReceiveBitMap + GetParam().RecvBitMapStartAddr >= Master.m_RackMaster_RecvBitMap.Length || (int)eReceiveBitMap + GetParam().RecvBitMapStartAddr < 0)
            {
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[C->R] BitMap Index: {eReceiveBitMap} + Start: {GetParam().RecvBitMapStartAddr} >= Length: {Master.m_RackMaster_RecvBitMap.Length}");

                return false;
            }

            Master.m_RackMaster_RecvBitMap[(int)eReceiveBitMap + GetParam().RecvBitMapStartAddr] = value;
            return true;
        }

        /// <summary>
        /// CIM -> Master, Master -> STK 영역 Word Map에 값 적용
        /// STK CIM Mode인 경우 CIM이 Word Map 관리
        /// STK Master Mode인 경우 Master가 Word Map 관리
        /// 최종 적으로 STK로 보내고자 하는 Word Map 영역
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex eReceiveWordMap, object value)
        {
            if ((int)eReceiveWordMap + GetParam().RecvWordMapStartAddr >= Master.m_RackMaster_RecvWordMap.Length || (int)eReceiveWordMap + GetParam().RecvWordMapStartAddr < 0)
            {
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[C->R] WordMap Index: {eReceiveWordMap} + Start: {GetParam().RecvWordMapStartAddr} >= Length: {Master.m_RackMaster_RecvWordMap.Length}");
                return false;
            }

            switch (eReceiveWordMap)
            {
                case ReceiveWordMapIndex.To_Shelf_ID_0:
                case ReceiveWordMapIndex.From_Shelf_ID_0:
                case ReceiveWordMapIndex.AutoTeaching_ID_0:
                case ReceiveWordMapIndex.Teaching_RW_ID_0:
                    {
                        byte[] DataArray = BitConverter.GetBytes((int)value);
                        Buffer.BlockCopy(DataArray, 0, Master.m_RackMaster_RecvWordMap, ((int)eReceiveWordMap + GetParam().RecvWordMapStartAddr) * sizeof(short), 4);
                    }
                    break;
                case ReceiveWordMapIndex.CassetteID_PIO_Word_0:
                    {
                        string str = Convert.ToString(value);
                        if (string.IsNullOrEmpty(str))
                        {
                            byte[] DataArray = new byte[58 * sizeof(short)];
                            int WriteSize = DataArray.Length;
                            Buffer.BlockCopy(DataArray, 0, Master.m_RackMaster_RecvWordMap, ((int)eReceiveWordMap + GetParam().RecvWordMapStartAddr) * sizeof(short), WriteSize);
                        }
                        else
                        {
                            byte[] DataArray = Encoding.UTF8.GetBytes(((string)value).Replace(" ", string.Empty));
                            int WriteSize = DataArray.Length;
                            if (DataArray.Length > 58 * sizeof(short))
                            {
                                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.CasseteIDOverFlow, $"{DataArray.Length} > {58 * sizeof(short)}");
                                WriteSize = 58 * sizeof(short); //Max
                            }
                            Buffer.BlockCopy(DataArray, 0, Master.m_RackMaster_RecvWordMap, ((int)eReceiveWordMap + GetParam().RecvWordMapStartAddr) * sizeof(short), WriteSize);
                        }
                    }
                    break;
                default:
                    Master.m_RackMaster_RecvWordMap[(int)eReceiveWordMap + GetParam().RecvWordMapStartAddr] = (short)value;
                    break;
            }

            return true;
        }


        /// <summary>
        /// CIM -> Master, Master -> STK 영역 Bit Map
        /// Bit Map의 특정 영역만 STK에 전송
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex eReceiveBitMapIndex)
        {
            try
            {
                if (!IsMapDefineIndex(eReceiveBitMapIndex))
                    return;

                int DataArrayLength = sizeof(byte);
                short BitMapOffset = (short)eReceiveBitMapIndex;
                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.STK_CIM_2_Controller_RM_Bit_Data };
                byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);

                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_RackMaster_RecvBitMap, BitMapOffset + GetParam().RecvBitMapStartAddr, Data, 0, DataArrayLength);


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
                SendData(send.ToArray());
            }
            catch
            {

            }
        }

        /// <summary>
        /// CIM -> Master, Master -> STK 영역 Bit Map
        /// 지정된 Size만큼 Bit Map을 STK에 전송
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex eReceiveBitMapIndex, int BitMapSize)
        {
            try
            {
                int DataArrayLength = sizeof(byte) * BitMapSize;
                short BitMapOffset = (short)eReceiveBitMapIndex;
                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.STK_CIM_2_Controller_RM_Bit_Data };
                byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);

                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_RackMaster_RecvBitMap, BitMapOffset + GetParam().RecvBitMapStartAddr, Data, 0, DataArrayLength);


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
                SendData(send.ToArray());
            }
            catch
            {

            }
        }

        /// <summary>
        /// CIM -> Master, Master -> STK 영역 Word Map
        /// Word Map의 특정 영역만 STK에 전송
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex eReceiveWordMapIndex)
        {
            try
            {
                if (!IsMapDefineIndex(eReceiveWordMapIndex))
                    return;

                int DataArrayLength = sizeof(short);
                short WordMapOffset = (short)eReceiveWordMapIndex;

                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.STK_CIM_2_Controller_RM_Word_Data };
                byte[] DataOffset = BitConverter.GetBytes(WordMapOffset);

                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_RackMaster_RecvWordMap, (WordMapOffset + GetParam().RecvWordMapStartAddr) * sizeof(short), Data, 0, DataArrayLength);


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
                SendData(send.ToArray());
            }
            catch
            {

            }
        }

        /// <summary>
        /// CIM -> Master, Master -> STK 영역 Word Map
        /// 지정된 Size만큼 Word Map을 STK에 전송
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex eReceiveWordMapIndex, int WordMapSize)
        {
            try
            {
                int DataArrayLength = sizeof(short) * WordMapSize;
                short WordMapOffset = (short)eReceiveWordMapIndex;

                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.STK_CIM_2_Controller_RM_Word_Data };
                byte[] DataOffset = BitConverter.GetBytes(WordMapOffset);

                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_RackMaster_RecvWordMap, (WordMapOffset + GetParam().RecvWordMapStartAddr) * sizeof(short), Data, 0, DataArrayLength);


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
                SendData(send.ToArray());
            }
            catch
            {

            }
        }

        
        /// <summary>
        /// CIM -> Master, Master -> STK 전달 시 특정 영역 전송 막기 위한 용도로 작성
        /// 현재는 통째로 전달되는 구조이므로 사용하지 않음.
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <returns></returns>
        bool IsPassing(ReceiveBitMapIndex eReceiveBitMap)
        {
            switch (eReceiveBitMap)
            {
                //전달하지 않아야 하는 패킷이 있다면 추가
                //case ReceiveBitMapIndex.CMD_EmergencyStop:
                //    return false;
                default:
                    return true;
            }
        }
        bool IsPassing(ReceiveWordMapIndex eReceiveWordMapIndex)
        {
            switch (eReceiveWordMapIndex)
            {
                //전달하지 않아야 하는 패킷이 있다면 추가
                //case ReceiveWordMapIndex.CassetteID_PIO_Word_0:
                //    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// STK 전송 전 Memory Map 영역에 정의된 데이터 인지 유효 판단하기 위함
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <returns></returns>
        bool IsMapDefineIndex(ReceiveBitMapIndex eReceiveBitMap)
        {
            if (Enum.IsDefined(typeof(ReceiveBitMapIndex), eReceiveBitMap))
                return true;

            LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->R] BitMap Number: {(int)eReceiveBitMap}");
            return false;
        }
        bool IsMapDefineIndex(ReceiveWordMapIndex eReceiveWordMapIndex)
        {
            if (Enum.IsDefined(typeof(ReceiveWordMapIndex), eReceiveWordMapIndex))
                return true;

            LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->R] WordMap Number: {(int)eReceiveWordMapIndex}");
            return false;
        }
        
    }
}
