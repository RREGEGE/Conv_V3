using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;
using System.Threading;

namespace RackMaster.SEQ.PART {
    // CIM -> RM BIT
    public enum ReceiveBitMap {
        MST_Emo_Request = 0,
        RM_Servo_On_Request,

        RM_Servo_Off_Request = 3,

        RM_Auto_Request = 5,

        RM_Auto_Stop_Request = 7,

        RM_Error_Reset_Request = 9,
        MST_Soft_Error_State,

        RM_Time_Sync = 16,

        RM_Auto_Teaching_Start_Request = 21,
        RM_Auto_Teaching_Complete_ACK,
        RM_Auto_Teaching_Complete_Alarm_ACK,
        RM_Auto_Teaching_Stop_Request,

        RM_From_Start_Request = 32,
        RM_From_Complete_ACK,
        RM_To_Start_Request,
        RM_To_Complete_ACK,

        RM_Maint_Start_Request = 38,
        RM_Maint_Complete_ACK,

        RM_Store_Alt_ACK = 43,
        RM_Source_Empty_ACK,
        RM_Double_Storage_ACK,
        RM_Resume_Request_ACK,

        RM_Teaching_Read_Write_Start = 48,

        RM_HP_Door_Open = 64,
        RM_OP_Door_Open,
        RM_HP_EMO,
        RM_OP_EMO,
        RM_HP_Escape,
        RM_OP_Escape,
        RM_Key_Auto,

        RM_X_Axis_Max_OverLoad_Reset = 102,
        RM_Z_Axis_Max_OverLoad_Reset,
        RM_A_Axis_Max_OverLoad_Reset,
        RM_T_Axis_Max_OverLoad_Reset,

        RM_X_Axis_Max_OverLoad_Apply = 107,
        RM_Z_Axis_Max_OverLoad_Apply,
        RM_A_Axis_Max_OverLoad_Apply,
        RM_T_Axis_Max_OverLoad_Apply,

        RM_X_Axis_Auto_Home_Cur_Reset = 118,
        RM_Z_Axis_Auto_Home_Cur_Reset,
        RM_A_Axis_Auto_Home_Cur_Reset,
        RM_T_Axis_Auto_Home_Cur_Reset,

        RM_X_Axis_Auto_Home_Count_Apply = 123,
        RM_Z_Axis_Auto_Home_Count_Apply,
        RM_A_Axis_Auto_Home_Count_Apply,
        RM_T_Axis_Auto_Home_Count_Apply,

        PIO_Load_Request = 208,
        PIO_Unload_Request,
        PIO_Ready,
        PIO_Port_Error,

        MAX_COUNT,
    }

    // RM -> CIM BIT
    public enum SendBitMap {
        Servo_On_Ready = 0,
        Servo_On_State,
        Servo_Off_Ready,
        Servo_Off_State,
        Auto_Ready,
        Auto_State,
        Manual_Ready,
        Manual_State,

        Error_State = 9,
        Home_State,

        Fan1_On_State = 14,
        Fan2_On_State,

        Auto_Teaching_State = 20,
        Auto_Teaching_Start_ACK,
        Auto_Teaching_Complete,
        Auto_Teaching_Alarm,

        From_Start_ACK = 32,
        From_Complete,
        To_Start_ACK,
        To_Complete,

        Maint_Move_Start_ACK = 38,
        Maint_Move_Complete,

        Store_Alt_Request = 43,
        Source_Empty_Request,
        Double_Storage_Request,
        Resume_Request_Request,

        Teaching_RW_Ready = 48,
        Teaching_Read_Complete,

        Accessing_RM_Down_From = 51,
        Accessing_RM_Down_To,

        From_Ready = 58,
        To_Ready,
        Idle_State,
        Active_State,

        XZT_Going_State = 64,

        Maint_Move_State = 67,
        From_State,
        To_State,

        X_Axis_FWD_State = 72,
        X_Axis_BWD_State,
        X_Axis_Max_Speed_State,
        Arm_Home_Position_State,
        Turn_Left_Position_State,
        Turn_Right_Position_State,
        CST_Empty,
        CST_ON,

        X_Axis_Power_On,
        X_Axis_Servo_On,
        X_Axis_HomeDone,
        Z_Axis_Power_On,
        Z_Axis_Servo_On,
        Z_Axis_HomeDone,
        A_Axis_Power_On,
        A_Axis_Servo_On,
        A_Axis_HomeDone,
        T_Axis_Power_On,
        T_Axis_Servo_On,
        T_Axis_HomeDone,

        EMO_On = 99,
        GOT_EMO_On,
        Double_Storage_Sensor,
        Arm_CST_All_Undetected,
        Arm_CST_1_Detected_Sensor,
        Arm_CST_2_Detected_Sensor,
        Arm_CST_Diagonal_Detected_Sensor,
        Left_From_Sensor,
        Left_To_Sensor,
        Right_From_Sensor,
        Right_To_Sensor,
        Left_1_Projecting_Sensor_Front,
        Left_2_Projecting_Sensor_Rear,
        Right_1_Projecting_Sensor_Front,
        Right_2_Projecting_Sensor_Rear,
        Z_Axis_HP_Home_Sensor,
        Z_Axis_HP_NOT_Sensor,
        Z_Axis_HP_POT_Sensor,

        HP_EMO_Push = 120,
        OP_EMO_Push,

        GOT_Detect = 128,
        Position_Sensor_1,
        Position_Sensor_2,
        X_Axis_HP_Home_Sensor,
        X_Axis_HP_Slow_Sensor,
        X_Axis_HP_NOT_Sensor,
        X_Axis_OP_Slow_Sensor,
        X_Axis_OP_POT_Sensor,
        A_Axis_Home_Sensor,
        A_Axis_NOT_Sensor,
        A_Axis_POT_Sensor,
        A_Axis_POS_1_Sensor,
        A_Axis_POS_2_Sensor,
        A_Axis_POS_3_Sensor,
        T_Axis_Home_Sensor,
        T_Axis_NOT_Sensor,
        T_Axis_POT_Sensor,
        T_Axis_POS_Sensor,

        CPS_Second_Run = 154,
        CPS_Second_Fault,

        From_Step_0_Complete = 160,
        From_Step_1_Complete,
        From_Step_2_Complete,
        From_Step_3_Complete,
        From_Step_4_Complete,
        To_Step_0_Complete,
        To_Step_1_Complete,
        To_Step_2_Complete,
        To_Step_3_Complete,
        To_Step_4_Complete,

        Communication_Check = 171,
        X_Axis_Busy,
        Z_Axis_Busy,
        T_Axis_Busy,
        A_Axis_Busy,

        PIO_TR_Request = 208,
        PIO_Busy,
        PIO_Complete,
        PIO_STK_Error,

        MAX_COUNT,
    }

    // CIM -> RM WORD
    public enum ReceiveWordMap {
        CST_ID_PIO_Word_0,
        CST_ID_PIO_Word_1,
        CST_ID_PIO_Word_2,
        CST_ID_PIO_Word_3,
        CST_ID_PIO_Word_4,
        CST_ID_PIO_Word_5,
        CST_ID_PIO_Word_6,
        CST_ID_PIO_Word_7,
        CST_ID_PIO_Word_8,
        CST_ID_PIO_Word_9,
        CST_ID_PIO_Word_10,
        CST_ID_PIO_Word_11,
        CST_ID_PIO_Word_12,
        CST_ID_PIO_Word_13,
        CST_ID_PIO_Word_14,
        CST_ID_PIO_Word_15,
        CST_ID_PIO_Word_16,
        CST_ID_PIO_Word_17,
        CST_ID_PIO_Word_18,
        CST_ID_PIO_Word_19,
        CST_ID_PIO_Word_20,
        CST_ID_PIO_Word_21,
        CST_ID_PIO_Word_22,
        CST_ID_PIO_Word_23,
        CST_ID_PIO_Word_24,
        CST_ID_PIO_Word_25,
        CST_ID_PIO_Word_26,
        CST_ID_PIO_Word_27,
        CST_ID_PIO_Word_28,
        CST_ID_PIO_Word_29,
        CST_ID_PIO_Word_30,
        CST_ID_PIO_Word_31,
        CST_ID_PIO_Word_32,
        CST_ID_PIO_Word_33,
        CST_ID_PIO_Word_34,
        CST_ID_PIO_Word_35,
        CST_ID_PIO_Word_36,
        CST_ID_PIO_Word_37,
        CST_ID_PIO_Word_38,
        CST_ID_PIO_Word_39,
        CST_ID_PIO_Word_40,
        CST_ID_PIO_Word_41,
        CST_ID_PIO_Word_42,
        CST_ID_PIO_Word_43,
        CST_ID_PIO_Word_44,
        CST_ID_PIO_Word_45,
        CST_ID_PIO_Word_46,
        CST_ID_PIO_Word_47,
        CST_ID_PIO_Word_48,
        CST_ID_PIO_Word_49,
        CST_ID_PIO_Word_50,
        CST_ID_PIO_Word_51,
        CST_ID_PIO_Word_52,
        CST_ID_PIO_Word_53,
        CST_ID_PIO_Word_54,
        CST_ID_PIO_Word_55,
        CST_ID_PIO_Word_56,
        CST_ID_PIO_Word_57,

        Year = 64,
        Month,
        Day,
        Hour,
        Minute,
        Second,
        Day_Of_The_Week,

        RM_FROM_SHELF_ID_0 = 80,
        RM_FROM_SHELF_ID_1,
        RM_TO_SHELF_ID_0,
        RM_TO_SHELF_ID_1,

        Teaching_RW_ID_0 = 90,
        Teaching_RW_ID_1,
        Auto_Teaching_ID_0,
        Auto_Teaching_ID_1,
        Auto_Teaching_X_Axis_Data,
        Auto_Teaching_Z_Axis_Data,

        RM_Speed_X_Axis_Percent = 108,
        RM_Speed_Z_Axis_Percent,
        RM_Speed_A_Axis_Percent,
        RM_Speed_T_Axis_Percent,

        X_Axis_Max_OverLoad_Limit = 113,
        Z_Axis_Max_OverLoad_Limit,
        A_Axis_Max_OverLoad_Limit,
        T_Axis_Max_OverLoad_Limit,

        Watchdog_Word_Data = 164,

        MAX_COUNT,
    }

    // RM -> CIM WORD
    public enum SendWordMap {
        CST_ID_PIO_Word_0,
        CST_ID_PIO_Word_1,
        CST_ID_PIO_Word_2,
        CST_ID_PIO_Word_3,
        CST_ID_PIO_Word_4,
        CST_ID_PIO_Word_5,
        CST_ID_PIO_Word_6,
        CST_ID_PIO_Word_7,
        CST_ID_PIO_Word_8,
        CST_ID_PIO_Word_9,
        CST_ID_PIO_Word_10,
        CST_ID_PIO_Word_11,
        CST_ID_PIO_Word_12,
        CST_ID_PIO_Word_13,
        CST_ID_PIO_Word_14,
        CST_ID_PIO_Word_15,
        CST_ID_PIO_Word_16,
        CST_ID_PIO_Word_17,
        CST_ID_PIO_Word_18,
        CST_ID_PIO_Word_19,
        CST_ID_PIO_Word_20,
        CST_ID_PIO_Word_21,
        CST_ID_PIO_Word_22,
        CST_ID_PIO_Word_23,
        CST_ID_PIO_Word_24,
        CST_ID_PIO_Word_25,
        CST_ID_PIO_Word_26,
        CST_ID_PIO_Word_27,
        CST_ID_PIO_Word_28,
        CST_ID_PIO_Word_29,
        CST_ID_PIO_Word_30,
        CST_ID_PIO_Word_31,
        CST_ID_PIO_Word_32,
        CST_ID_PIO_Word_33,
        CST_ID_PIO_Word_34,
        CST_ID_PIO_Word_35,
        CST_ID_PIO_Word_36,
        CST_ID_PIO_Word_37,
        CST_ID_PIO_Word_38,
        CST_ID_PIO_Word_39,
        CST_ID_PIO_Word_40,
        CST_ID_PIO_Word_41,
        CST_ID_PIO_Word_42,
        CST_ID_PIO_Word_43,
        CST_ID_PIO_Word_44,
        CST_ID_PIO_Word_45,
        CST_ID_PIO_Word_46,
        CST_ID_PIO_Word_47,
        CST_ID_PIO_Word_48,
        CST_ID_PIO_Word_49,
        CST_ID_PIO_Word_50,
        CST_ID_PIO_Word_51,
        CST_ID_PIO_Word_52,
        CST_ID_PIO_Word_53,
        CST_ID_PIO_Word_54,
        CST_ID_PIO_Word_55,
        CST_ID_PIO_Word_56,
        CST_ID_PIO_Word_57,

        X_Axis_Cur_Position_0 = 64,
        X_Axis_Cur_Position_1,
        X_Axis_Target_Position_0,
        X_Axis_Target_Position_1,
        X_Axis_Cur_Velocity_0,
        X_Axis_Cur_Velocity_1,
        X_Axis_Cur_Max_OverLoad,
        X_Axis_Setting_Max_OverLoad,

        X_Axis_ACC = 74,
        X_Axis_DEC,
        X_Axis_Cur_Torque,

        Z_Axis_Cur_Position_0 = 80,
        Z_Axis_Cur_Position_1,
        Z_Axis_Target_Position_0,
        Z_Axis_Target_Position_1,
        Z_Axis_Cur_Velocity_0,
        Z_Axis_Cur_Velocity_1,
        Z_Axis_Cur_Max_OverLoad,
        Z_Axis_Setting_Max_OverLoad,

        Z_Axis_ACC = 90,
        Z_Axis_DEC,
        Z_Axis_Cur_Torque,

        A_Axis_Cur_Position_0 = 96,
        A_Axis_Cur_Position_1,
        A_Axis_Target_Position_0,
        A_Axis_Target_Position_1,
        A_Axis_Cur_Velocity_0,
        A_Axis_Cur_Velocity_1,
        A_Axis_Cur_Max_OverLoad,
        A_Axis_Setting_Max_OverLoad,

        A_Axis_ACC = 106,
        A_Axis_DEC,
        A_Axis_Cur_Torque,

        T_Axis_Cur_Position_0 = 112,
        T_Axis_Cur_Position_1,
        T_Axis_Target_Position_0,
        T_Axis_Target_Position_1,
        T_Axis_Cur_Velocity_0,
        T_Axis_Cur_Velocity_1,
        T_Axis_Cur_Max_OverLoad,
        T_Axis_Setting_Max_OverLoad,

        T_Axis_ACC = 122,
        T_Axis_DEC,
        T_Axis_Cur_Torque,

        Auto_Speed_Percent_X = 128,
        Auto_Speed_Percent_Z,
        Auto_Speed_Percent_A,
        Auto_Speed_Percent_T,
        Auto_Teaching_Id_0,
        Auto_Teaching_Id_1,
        Auto_Teaching_Target_X,
        Auto_Teaching_Target_Z,

        Error_Word_0 = 144,
        Error_Word_1,
        Error_Word_2,
        Error_Word_3,
        Error_Word_4,
        Error_Word_5,
        Error_Word_6,
        Error_Word_7,
        Error_Word_8,
        Error_Word_9,
        Error_Word_10,
        Error_Word_11,
        Error_Word_12,

        Teaching_X_Axis_Data_0 = 162,
        Teaching_X_Axis_Data_1,
        Teaching_Z_Axis_Data_0,
        Teaching_Z_Axis_Data_1,
        Teaching_A_Axis_Data_0,
        Teaching_A_Axis_Data_1,
        Teaching_T_Axis_Data_0,
        Teaching_T_Axis_Data_1,

        FROM_SHELF_ID_0 = 172,
        FROM_SHELF_ID_1,
        TO_SHELF_ID_0,
        TO_SHELF_ID_1,

        X_Axis_Avr_Torque,
        X_Axis_Peak_Torque,
        Z_Axis_Avr_Torque,
        Z_Axis_Peak_Torque,
        A_Axis_Avr_Torque,
        A_Axis_Peak_Torque,
        T_Axis_Avr_Torque,
        T_Axis_Peak_Torque,

        Watchdog_Word_Data = 186,
        X_Axis_Cur_Limit_Torque,
        Z_Axis_Cur_Limit_Torque,
        A_Axis_Cur_Limit_Torque,
        T_Axis_Cur_Limit_Torque,

        RM_Auto_Step_Number = 192,

        Access_Shelf_ID_0 = 194,
        Access_Shelf_ID_1,
        X_Axis_Cumulative_Distance_0,
        X_Axis_Cumulative_Distance_1,
        Z_Axis_Cumulative_Distance_0,
        Z_Axis_Cumulative_Distance_1,
        A_Axis_Cumulative_Distance_0,
        A_Axis_Cumulative_Distance_1,
        T_Axis_Cumulative_Distance_0,
        T_Axis_Cumulative_Distance_1,

        Regulator_BoostVoltage = 208,
        Regulator_OutputVoltage,
        Regulator_IBoostCurrent,
        Regulator_OutputCurrent,
        Regulator_PickupTemperature,
        Regulator_HitSinkTemperature,
        Regulator_InsideTemperature,

        MAX_COUNT,
    }
    public partial class RackMasterMain {
        public delegate void ReceiveBitEventHandler(ReceiveBitMap bitMap);
        public event ReceiveBitEventHandler ReceiveBitEvent;

        private RackMasterParam.AxisParam m_axisParam;

        private static TeachingValueData m_teacingData = null;

        private static short m_maxOverloadX = 0;
        private static short m_maxOverloadZ = 0;
        private static short m_maxOverloadA = 0;
        private static short m_maxOverloadT = 0;

        private static string m_preCSTID = string.Empty;
        /// <summary>
        /// Master(or CIM)으로부터 받은 Bit 값을 반환
        /// </summary>
        /// <param name="bitMap"></param>
        /// <returns></returns>
        public bool GetReceiveBit(ReceiveBitMap bitMap) {
            return m_RackMaster_RecvBitMap[(int)bitMap];
        }
        /// <summary>
        /// Master(or CIM)에게 전달하는 Bit 값을 반환
        /// </summary>
        /// <param name="bitMap"></param>
        /// <returns></returns>
        public bool GetSendBit(SendBitMap bitMap) {
            return m_RackMaster_SendBitMap[(int)bitMap];
        }
        /// <summary>
        /// Master(or CIM)에게 전달하는 Bit 값을 설정
        /// </summary>
        /// <param name="bitMap"></param>
        /// <param name="value"></param>
        public void SetSendBit(SendBitMap bitMap, bool value) {
            m_RackMaster_SendBitMap[(int)bitMap] = value;
        }
        /// <summary>
        /// Master(or CIM)에게 전달받은 Word 값을 반환
        /// </summary>
        /// <param name="wordMap"></param>
        /// <returns></returns>
        public short GetReceiveWord(ReceiveWordMap wordMap) {
            return m_RackMaster_RecvWordMap[(int)wordMap];
        }
        /// <summary>
        /// Master(or CIM)에게 전달하는 word 값을 반환
        /// </summary>
        /// <param name="wordMap"></param>
        /// <returns></returns>
        public object GetSendWord(SendWordMap wordMap) {
            return m_RackMaster_SendWordMap[(int)wordMap];
        }
        /// <summary>
        /// Master(or CIM)에게 Bit 값을 받았을 때 동작
        /// </summary>
        /// <param name="receiveBitMap"></param>
        public void Action_CIM_2_RM_Bit_Data(ReceiveBitMap receiveBitMap) {
            switch (receiveBitMap) {
                case ReceiveBitMap.MST_Emo_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        SetSendBit(SendBitMap.Auto_State, false);
                        SetSendBit(SendBitMap.Manual_State, true);
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }

                    break;

                case ReceiveBitMap.RM_Servo_On_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_motion.AllServoOn();
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_Servo_Off_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_motion.AllServoOff();
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_Auto_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        SetSendBit(SendBitMap.Auto_State, true);
                        SetSendBit(SendBitMap.Manual_State, false);
                        ReceiveBitEvent(receiveBitMap);
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_Auto_Stop_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        SetSendBit(SendBitMap.Auto_State, false);
                        SetSendBit(SendBitMap.Manual_State, true);
                        m_motion.EmergencyStop();
                        m_motion.SetAutoMotionStep(AutoStep.Step500_Error);
                        m_motion.SetPIOInitial();
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_Error_Reset_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_alarm.ClearAlarm();
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.MST_Soft_Error_State:
                    if (GetReceiveBit(receiveBitMap)) {
                        SetSendBit(SendBitMap.Auto_State, false);
                        SetSendBit(SendBitMap.Manual_State, true);
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }

                    break;

                case ReceiveBitMap.RM_Time_Sync:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                        if (Utility.GetApp_ModifySyncTime()) {
                            SetSyncTime();
                        }
                        else {
                            Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"System time cannot be set."));
                        }
                    }

                    break;

                case ReceiveBitMap.RM_Auto_Teaching_Start_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                        m_motion.AutoTeachingStart();
                    }

                    break;

                case ReceiveBitMap.RM_Auto_Teaching_Complete_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }

                    break;

                case ReceiveBitMap.RM_Auto_Teaching_Complete_Alarm_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_From_Start_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        if (Interlock_FromReady()) {
                            Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                            m_motion.AutoMotionFromStart();
                            SetSendBit(SendBitMap.Accessing_RM_Down_From, true);
                            SetSendBit(SendBitMap.Accessing_RM_Down_To, false);
                        }
                        else {
                            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.CIM, Log.LogMessage_Main.CIM_ActionFail, $"{receiveBitMap} => {Log.GetLogMessage(Log.LogMessage_Main.Interlock_NotFromReady)}"));
                        }
                    }
                    break;

                case ReceiveBitMap.RM_From_Complete_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_To_Start_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        if (Interlock_ToReady()) {
                            Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                            m_motion.AutoMotionToStart();
                            SetSendBit(SendBitMap.Accessing_RM_Down_From, false);
                            SetSendBit(SendBitMap.Accessing_RM_Down_To, true);
                        }
                        else {
                            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.CIM, Log.LogMessage_Main.CIM_ActionFail, $"{receiveBitMap} => {Log.GetLogMessage(Log.LogMessage_Main.Interlock_NotToReady)}"));
                        }
                    }
                    break;

                case ReceiveBitMap.RM_To_Complete_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_Maint_Start_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        if (Interlock_MaintReady()) {
                            Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                            m_motion.MaintMoveStart();
                            SetSendBit(SendBitMap.Maint_Move_Start_ACK, true);
                        }
                        else {
                            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.CIM, Log.LogMessage_Main.CIM_ActionFail, $"{receiveBitMap} => {Log.GetLogMessage(Log.LogMessage_Main.Interlock_NotMaintReady)}"));
                        }
                    }
                    else {
                        SetSendBit(SendBitMap.Maint_Move_Start_ACK, false);
                    }

                    break;

                // AutoMotion에서 처리
                case ReceiveBitMap.RM_Maint_Complete_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }

                    break;

                // AutoMotion에서 처리
                case ReceiveBitMap.RM_Store_Alt_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }

                    break;

                // AutoMotion에서 처리
                case ReceiveBitMap.RM_Source_Empty_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }

                    break;

                // AutoMotion에서 처리
                case ReceiveBitMap.RM_Double_Storage_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                // AutoMotion에서 처리
                case ReceiveBitMap.RM_Resume_Request_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }

                    break;

                case ReceiveBitMap.RM_Teaching_Read_Write_Start:
                    if (GetReceiveBit(receiveBitMap)) {
                        try {
                            int id = ((ushort)m_RackMaster_RecvWordMap[(int)ReceiveWordMap.Teaching_RW_ID_1] << 16) | (ushort)m_RackMaster_RecvWordMap[(int)ReceiveWordMap.Teaching_RW_ID_0];

                            m_teacingData = m_teaching.GetTeachingData(id);
                            if (m_teacingData == null) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}, Port data is not exist, id={id}"));
                            }
                            else {
                                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}, id={id}"));
                            }
                        }
                        catch (Exception ex) {
                            Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.CIM, $"Recevie Bit:{receiveBitMap},", ex));
                        }
                        if (m_teacingData != null) {
                            float xData = 0;
                            float zData = 0;
                            float aData = 0;
                            float tData = 0;
                            if (m_teacingData.valX != null) xData = (float)m_teacingData.valX / 1000;
                            if (m_teacingData.valZ != null) {
                                if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                                    zData = (float)m_teacingData.valZ / 1000;
                                }
                                else {
                                    zData = (float)m_motion.RadianToCalculateDistance((double)m_teacingData.valZ);
                                    zData /= 1000;
                                }
                            }
                            if (m_teacingData.valFork_A != null) aData = (float)m_teacingData.valFork_A / 1000;
                            if (m_teacingData.valT != null) tData = (float)m_teacingData.valT / 1000;

                            Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"R/W Complete, X Data={xData}, Z Data={zData}, A Data={aData}, T Data={tData}"));

                            UpdateTeachingData(SendWordMap.Teaching_X_Axis_Data_0, xData);
                            UpdateTeachingData(SendWordMap.Teaching_Z_Axis_Data_0, zData);
                            UpdateTeachingData(SendWordMap.Teaching_A_Axis_Data_0, aData);
                            UpdateTeachingData(SendWordMap.Teaching_T_Axis_Data_0, tData);

                            m_teacingData = null;
                        }
                        else {
                            Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"R/W Complete, X Data=0, Z Data=0, A Data=0, T Data=0"));
                            UpdateTeachingData(SendWordMap.Teaching_X_Axis_Data_0, 0);
                            UpdateTeachingData(SendWordMap.Teaching_Z_Axis_Data_0, 0);
                            UpdateTeachingData(SendWordMap.Teaching_A_Axis_Data_0, 0);
                            UpdateTeachingData(SendWordMap.Teaching_T_Axis_Data_0, 0);
                        }
                        m_RackMaster_SendBitMap[(int)SendBitMap.Teaching_Read_Complete] = true;

                    }

                    break;

                case ReceiveBitMap.RM_HP_Door_Open:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_OP_Door_Open:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_HP_EMO:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_OP_EMO:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_HP_Escape:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_OP_Escape:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_Key_Auto:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.RM_X_Axis_Max_OverLoad_Reset:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }

                    break;

                case ReceiveBitMap.RM_Z_Axis_Max_OverLoad_Reset:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }

                    break;

                case ReceiveBitMap.RM_A_Axis_Max_OverLoad_Reset:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }

                    break;

                case ReceiveBitMap.RM_T_Axis_Max_OverLoad_Reset:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }

                    break;

                case ReceiveBitMap.RM_X_Axis_Max_OverLoad_Apply:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_axisParam = m_param.GetAxisParameter(AxisList.X_Axis);
                        m_axisParam.maxOverload = (float)m_maxOverloadX;
                        m_param.SetAxisParameter(AxisList.X_Axis, m_axisParam);
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap} => {m_maxOverloadX:F1}"));
                    }

                    break;

                case ReceiveBitMap.RM_Z_Axis_Max_OverLoad_Apply:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_axisParam = m_param.GetAxisParameter(AxisList.Z_Axis);
                        m_axisParam.maxOverload = (float)m_maxOverloadZ;
                        m_param.SetAxisParameter(AxisList.Z_Axis, m_axisParam);
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap} => {m_maxOverloadZ:F1}"));
                    }

                    break;

                case ReceiveBitMap.RM_A_Axis_Max_OverLoad_Apply:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_axisParam = m_param.GetAxisParameter(AxisList.A_Axis);
                        m_axisParam.maxOverload = (float)m_maxOverloadZ;
                        m_param.SetAxisParameter(AxisList.A_Axis, m_axisParam);
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap} => {m_maxOverloadA:F1}"));
                    }
                    break;

                case ReceiveBitMap.RM_T_Axis_Max_OverLoad_Apply:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_axisParam = m_param.GetAxisParameter(AxisList.T_Axis);
                        m_axisParam.maxOverload = (float)m_maxOverloadZ;
                        m_param.SetAxisParameter(AxisList.T_Axis, m_axisParam);
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap} => {m_maxOverloadT:F1}"));
                    }
                    break;

                case ReceiveBitMap.PIO_Load_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.PIO_Unload_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.PIO_Ready:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;

                case ReceiveBitMap.PIO_Port_Error:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveBit, $"{receiveBitMap}"));
                    }
                    break;
            }
        }
        /// <summary>
        /// Master(or CIM)에게 Word 값을 받았을 때 동작
        /// </summary>
        /// <param name="receiveWordMap"></param>
        public void Action_CIM_2_RM_Word_Data(ReceiveWordMap receiveWordMap) {
            if(receiveWordMap != ReceiveWordMap.CST_ID_PIO_Word_0) {
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, Log.LogMessage_Main.CIM_ReceiveWord, $"{receiveWordMap}"));
            }

            //SetAutoSpeedPercent();
            //SetOverload();
            //SetTeachingData();
            //SetAutoTeaching();

            //byte[] fromShelfIdData = new byte[sizeof(int)];
            //Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.RM_FROM_SHELF_ID_0 * sizeof(short)), fromShelfIdData, 0, sizeof(int));
            //m_motion.SetAutoMotionFromID((int)BitConverter.ToInt32(fromShelfIdData, 0));

            //byte[] toShelfIdData = new byte[sizeof(int)];
            //Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.RM_TO_SHELF_ID_0 * sizeof(short)), toShelfIdData, 0, sizeof(int));
            //m_motion.SetAutoMotionToID((int)BitConverter.ToInt32(toShelfIdData, 0));

            switch (receiveWordMap) {
                case ReceiveWordMap.CST_ID_PIO_Word_0:
                case ReceiveWordMap.CST_ID_PIO_Word_1:
                case ReceiveWordMap.CST_ID_PIO_Word_2:
                case ReceiveWordMap.CST_ID_PIO_Word_3:
                case ReceiveWordMap.CST_ID_PIO_Word_4:
                case ReceiveWordMap.CST_ID_PIO_Word_5:
                case ReceiveWordMap.CST_ID_PIO_Word_6:
                case ReceiveWordMap.CST_ID_PIO_Word_7:
                case ReceiveWordMap.CST_ID_PIO_Word_8:
                case ReceiveWordMap.CST_ID_PIO_Word_9:
                case ReceiveWordMap.CST_ID_PIO_Word_10:
                case ReceiveWordMap.CST_ID_PIO_Word_11:
                case ReceiveWordMap.CST_ID_PIO_Word_12:
                case ReceiveWordMap.CST_ID_PIO_Word_13:
                case ReceiveWordMap.CST_ID_PIO_Word_14:
                case ReceiveWordMap.CST_ID_PIO_Word_15:
                case ReceiveWordMap.CST_ID_PIO_Word_16:
                case ReceiveWordMap.CST_ID_PIO_Word_17:
                case ReceiveWordMap.CST_ID_PIO_Word_18:
                case ReceiveWordMap.CST_ID_PIO_Word_19:
                case ReceiveWordMap.CST_ID_PIO_Word_20:
                case ReceiveWordMap.CST_ID_PIO_Word_21:
                case ReceiveWordMap.CST_ID_PIO_Word_22:
                case ReceiveWordMap.CST_ID_PIO_Word_23:
                case ReceiveWordMap.CST_ID_PIO_Word_24:
                case ReceiveWordMap.CST_ID_PIO_Word_25:
                case ReceiveWordMap.CST_ID_PIO_Word_26:
                case ReceiveWordMap.CST_ID_PIO_Word_27:
                case ReceiveWordMap.CST_ID_PIO_Word_28:
                case ReceiveWordMap.CST_ID_PIO_Word_29:
                case ReceiveWordMap.CST_ID_PIO_Word_30:
                case ReceiveWordMap.CST_ID_PIO_Word_31:
                case ReceiveWordMap.CST_ID_PIO_Word_32:
                case ReceiveWordMap.CST_ID_PIO_Word_33:
                case ReceiveWordMap.CST_ID_PIO_Word_34:
                case ReceiveWordMap.CST_ID_PIO_Word_35:
                case ReceiveWordMap.CST_ID_PIO_Word_36:
                case ReceiveWordMap.CST_ID_PIO_Word_37:
                case ReceiveWordMap.CST_ID_PIO_Word_38:
                case ReceiveWordMap.CST_ID_PIO_Word_39:
                case ReceiveWordMap.CST_ID_PIO_Word_40:
                case ReceiveWordMap.CST_ID_PIO_Word_41:
                case ReceiveWordMap.CST_ID_PIO_Word_42:
                case ReceiveWordMap.CST_ID_PIO_Word_43:
                case ReceiveWordMap.CST_ID_PIO_Word_44:
                case ReceiveWordMap.CST_ID_PIO_Word_45:
                case ReceiveWordMap.CST_ID_PIO_Word_46:
                case ReceiveWordMap.CST_ID_PIO_Word_47:
                case ReceiveWordMap.CST_ID_PIO_Word_48:
                case ReceiveWordMap.CST_ID_PIO_Word_49:
                case ReceiveWordMap.CST_ID_PIO_Word_50:
                case ReceiveWordMap.CST_ID_PIO_Word_51:
                case ReceiveWordMap.CST_ID_PIO_Word_52:
                case ReceiveWordMap.CST_ID_PIO_Word_53:
                case ReceiveWordMap.CST_ID_PIO_Word_54:
                case ReceiveWordMap.CST_ID_PIO_Word_55:
                case ReceiveWordMap.CST_ID_PIO_Word_56:
                case ReceiveWordMap.CST_ID_PIO_Word_57:
                    CopyCassetteID();
                    break;

                case ReceiveWordMap.Year:
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Year={m_RackMaster_RecvWordMap[(int)receiveWordMap]}"));
                    break;
                case ReceiveWordMap.Month:
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Month={m_RackMaster_RecvWordMap[(int)receiveWordMap]}"));
                    break;
                case ReceiveWordMap.Day:
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Day={m_RackMaster_RecvWordMap[(int)receiveWordMap]}"));
                    break;
                case ReceiveWordMap.Hour:
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Hour={m_RackMaster_RecvWordMap[(int)receiveWordMap]}"));
                    break;
                case ReceiveWordMap.Minute:
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Minute={m_RackMaster_RecvWordMap[(int)receiveWordMap]}"));
                    break;
                case ReceiveWordMap.Second:
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Second={m_RackMaster_RecvWordMap[(int)receiveWordMap]}"));
                    break;

                case ReceiveWordMap.RM_Speed_X_Axis_Percent:
                    SetAutoSpeedPercent(AxisList.X_Axis);
                    break;
                case ReceiveWordMap.RM_Speed_Z_Axis_Percent:
                    SetAutoSpeedPercent(AxisList.Z_Axis);
                    break;
                case ReceiveWordMap.RM_Speed_A_Axis_Percent:
                    SetAutoSpeedPercent(AxisList.A_Axis);
                    break;
                case ReceiveWordMap.RM_Speed_T_Axis_Percent:
                    SetAutoSpeedPercent(AxisList.T_Axis);
                    break;

                case ReceiveWordMap.X_Axis_Max_OverLoad_Limit:
                case ReceiveWordMap.Z_Axis_Max_OverLoad_Limit:
                case ReceiveWordMap.A_Axis_Max_OverLoad_Limit:
                case ReceiveWordMap.T_Axis_Max_OverLoad_Limit:
                    SetOverload();
                    break;

                case ReceiveWordMap.Teaching_RW_ID_0:
                case ReceiveWordMap.Teaching_RW_ID_1:
                    SetTeachingData();
                    break;

                case ReceiveWordMap.Auto_Teaching_ID_0:
                case ReceiveWordMap.Auto_Teaching_ID_1:
                    SetAutoTeachingID();
                    break;
                case ReceiveWordMap.Auto_Teaching_X_Axis_Data:
                    SetAutoTeachingXData();
                    break;
                case ReceiveWordMap.Auto_Teaching_Z_Axis_Data:
                    SetAutoTeachingZData();
                    break;

                case ReceiveWordMap.RM_FROM_SHELF_ID_0:
                case ReceiveWordMap.RM_FROM_SHELF_ID_1:
                    byte[] fromShelfIdData = new byte[sizeof(int)];
                    Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.RM_FROM_SHELF_ID_0 * sizeof(short)), fromShelfIdData, 0, sizeof(int));
                    m_motion.SetAutoMotionFromID((int)BitConverter.ToInt32(fromShelfIdData, 0));
                    break;

                case ReceiveWordMap.RM_TO_SHELF_ID_0:
                case ReceiveWordMap.RM_TO_SHELF_ID_1:
                    byte[] toShelfIdData = new byte[sizeof(int)];
                    Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.RM_TO_SHELF_ID_0 * sizeof(short)), toShelfIdData, 0, sizeof(int));
                    m_motion.SetAutoMotionToID((int)BitConverter.ToInt32(toShelfIdData, 0));
                    break;
            }
        }
        /// <summary>
        /// Master(or CIM)에게 전달받은 ID를 토대로 티칭 데이터를 임시 저장 후 Master(or CIM)에게 전달
        /// </summary>
        private void SetTeachingData() {
            try {
                int id = ((ushort)m_RackMaster_RecvWordMap[(int)ReceiveWordMap.Teaching_RW_ID_1] << 16) | (ushort)m_RackMaster_RecvWordMap[(int)ReceiveWordMap.Teaching_RW_ID_0];

                m_teacingData = m_teaching.GetTeachingData(id);
                if (m_teacingData == null) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.CIM, $"Get Word Data : Not Port Data, id={id}"));
                }
                else {
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Get Word Data : Port ID={id}"));
                }
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.CIM, $"SetTeachingData(),", ex));
            }
        }
        /// <summary>
        /// Master(or CIM)에게 받은 카세트 ID를 m_RackMaster_SendWordMap 배열에 복사
        /// </summary>
        private void CopyCassetteID() {
            int stringSize = 58 * sizeof(short);
            byte[] stringData = new byte[stringSize];
            Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.CST_ID_PIO_Word_0) * sizeof(short), stringData, 0, stringSize);

            string strCSTID = ((string)Encoding.Default.GetString(stringData).Trim('\0')).Replace(" ", string.Empty);

            if(strCSTID == string.Empty) {
                foreach (SendWordMap item in Enum.GetValues(typeof(SendWordMap))) {
                    if (item >= SendWordMap.CST_ID_PIO_Word_0 &&
                        item <= SendWordMap.CST_ID_PIO_Word_57) {
                        m_RackMaster_RecvWordMap[(int)item] = 0;
                    }
                }

                m_preCSTID = string.Empty;
            }
            else {
                foreach (SendWordMap item in Enum.GetValues(typeof(SendWordMap))) {
                    if (item >= SendWordMap.CST_ID_PIO_Word_0 &&
                        item <= SendWordMap.CST_ID_PIO_Word_57) {
                        m_RackMaster_SendWordMap[(int)item] = m_RackMaster_RecvWordMap[(int)item];
                    }
                }

                if (!m_preCSTID.Equals(strCSTID)) {
                    m_preCSTID = strCSTID;
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Receive CST ID : {strCSTID}"));
                }
            }
        }
        /// <summary>
        /// Master(or CIM)에게 받은 카세트 ID와 Master(or CIM)에게 전달하는 카세트 ID가 동일한지 비교
        /// </summary>
        /// <returns></returns>
        private bool CompareCassetteID() {
            foreach (SendWordMap item in Enum.GetValues(typeof(SendWordMap))) {
                if (item >= SendWordMap.CST_ID_PIO_Word_0 &&
                    item <= SendWordMap.CST_ID_PIO_Word_57) {
                    if (m_RackMaster_SendWordMap[(int)item] != m_RackMaster_RecvWordMap[(int)item]) {
                        return false;
                    }
                }
            }

            return true;
        }
        /// <summary>
        /// Master(or CIM)에게 받은 시간을 시스템 시간으로 설정
        /// </summary>
        private void SetSyncTime() {
            SystemTimeControl.SystemTime syncTime = new SystemTimeControl.SystemTime();

            syncTime.wYear      = (ushort)m_RackMaster_RecvWordMap[(int)ReceiveWordMap.Year];
            syncTime.wMonth     = (ushort)m_RackMaster_RecvWordMap[(int)ReceiveWordMap.Month];
            syncTime.wDay       = (ushort)m_RackMaster_RecvWordMap[(int)ReceiveWordMap.Day];
            syncTime.wHour      = (ushort)m_RackMaster_RecvWordMap[(int)ReceiveWordMap.Hour];
            syncTime.wMinute    = (ushort)m_RackMaster_RecvWordMap[(int)ReceiveWordMap.Minute];
            syncTime.wSecond    = (ushort)m_RackMaster_RecvWordMap[(int)ReceiveWordMap.Second];

            if (SystemTimeControl.ModifySystemTime(syncTime)) {
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"TIme Sync Success, Year={syncTime.wYear}, Month={syncTime.wMonth}, Day={syncTime.wDay}, Hour={syncTime.wHour}, Minute={syncTime.wMinute}, Second={syncTime.wSecond}"));
            }
            else {
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Time Sync Fail"));
            }
        }
        /// <summary>
        /// Master(or CIM)에게 받은 각 축별로 Auto Speed Percent 세팅
        /// </summary>
        /// <param name="axis"></param>
        private void SetAutoSpeedPercent(AxisList axis) {
            byte[] shortData = new byte[sizeof(short)];
            switch (axis) {
                case AxisList.X_Axis:
                    Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.RM_Speed_X_Axis_Percent * sizeof(short)), shortData, 0, sizeof(short));
                    short percentData_X = BitConverter.ToInt16(shortData, 0);
                    if (percentData_X <= 0) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.CIM, $"Wrong Data X={percentData_X}"));
                        percentData_X = 50;
                    }
                    else if (percentData_X > 100) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.CIM, $"Wrong Data X={percentData_X}"));
                        percentData_X = 100;
                    }
                    m_axisParam = m_param.GetAxisParameter(AxisList.X_Axis);
                    m_axisParam.autoSpeedPercent = (float)percentData_X;
                    m_param.SetAxisParameter(AxisList.X_Axis, m_axisParam);
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Set Auto Speed Percent X={percentData_X}"));
                    break;

                case AxisList.Z_Axis:
                    Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.RM_Speed_Z_Axis_Percent * sizeof(short)), shortData, 0, sizeof(short));
                    short percentData_Z = BitConverter.ToInt16(shortData, 0);
                    if (percentData_Z <= 0) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.CIM, $"Wrong Data Z={percentData_Z}"));
                        percentData_Z = 50;
                    }
                    else if (percentData_Z > 100) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.CIM, $"Wrong Data Z={percentData_Z}"));
                        percentData_Z = 100;
                    }
                    m_axisParam = m_param.GetAxisParameter(AxisList.Z_Axis);
                    m_axisParam.autoSpeedPercent = (float)percentData_Z;
                    m_param.SetAxisParameter(AxisList.Z_Axis, m_axisParam);
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Set Auto Speed Percent Z={percentData_Z}"));
                    break;

                case AxisList.A_Axis:
                    Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.RM_Speed_A_Axis_Percent * sizeof(short)), shortData, 0, sizeof(short));
                    short percentData_A = BitConverter.ToInt16(shortData, 0);
                    if (percentData_A <= 0) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.CIM, $"Wrong Data A={percentData_A}"));
                        percentData_A = 50;
                    }
                    else if (percentData_A > 100) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.CIM, $"Wrong Data A={percentData_A}"));
                        percentData_A = 100;
                    }
                    m_axisParam = m_param.GetAxisParameter(AxisList.A_Axis);
                    m_axisParam.autoSpeedPercent = (float)percentData_A;
                    m_param.SetAxisParameter(AxisList.A_Axis, m_axisParam);
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Set Auto Speed Percent A={percentData_A}"));
                    break;

                case AxisList.T_Axis:
                    Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.RM_Speed_T_Axis_Percent * sizeof(short)), shortData, 0, sizeof(short));
                    short percentData_T = BitConverter.ToInt16(shortData, 0);
                    if (percentData_T <= 0) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.CIM, $"Wrong Data T={percentData_T}"));
                        percentData_T = 50;
                    }
                    else if (percentData_T > 100) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.CIM, $"Wrong Data T={percentData_T}"));
                        percentData_T = 100;
                    }
                    m_axisParam = m_param.GetAxisParameter(AxisList.T_Axis);
                    m_axisParam.autoSpeedPercent = (float)percentData_T;
                    m_param.SetAxisParameter(AxisList.T_Axis, m_axisParam);
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"Set Auto Speed Percent T={percentData_T}"));
                    break;
            }
        }
        /// <summary>
        /// Master(or CIM)에게 받은 알람 발생 기준의 토크치를 설정
        /// </summary>
        private void SetOverload() {
            byte[] shortData = new byte[sizeof(short)];
            Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.X_Axis_Max_OverLoad_Limit * sizeof(short)), shortData, 0, sizeof(short));
            m_maxOverloadX = BitConverter.ToInt16(shortData, 0);

            Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.Z_Axis_Max_OverLoad_Limit * sizeof(short)), shortData, 0, sizeof(short));
            m_maxOverloadZ = BitConverter.ToInt16(shortData, 0);

            Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.A_Axis_Max_OverLoad_Limit * sizeof(short)), shortData, 0, sizeof(short));
            m_maxOverloadA = BitConverter.ToInt16(shortData, 0);

            Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.T_Axis_Max_OverLoad_Limit * sizeof(short)), shortData, 0, sizeof(short));
            m_maxOverloadT = BitConverter.ToInt16(shortData, 0);
        }
        /// <summary>
        /// Master(or CIM)에게 받은 Auto Teaching을 시작할 ID 설정
        /// </summary>
        private void SetAutoTeachingID() {
            byte[] intData = new byte[sizeof(int)];
            Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.Auto_Teaching_ID_0 * sizeof(short)), intData, 0, sizeof(int));
            m_motion.SetAutoTeachingTargetID((int)BitConverter.ToInt32(intData, 0));
        }
        /// <summary>
        /// Master(or CIM)에게 받은 Auto Teaching을 시작할 타겟의 X축 데이터 설정
        /// </summary>
        private void SetAutoTeachingXData() {
            byte[] shortData = new byte[sizeof(short)];
            Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.Auto_Teaching_X_Axis_Data * sizeof(short)), shortData, 0, sizeof(short));
            m_motion.SetAutoTeachingTargetX((double)BitConverter.ToInt16(shortData, 0));
        }
        /// <summary>
        /// Master(or CIM)에게 받은 Auto Teaching을 시작할 타겟의 Z축 데이터 설정
        /// </summary>
        private void SetAutoTeachingZData() {
            byte[] shortData = new byte[sizeof(short)];
            Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)ReceiveWordMap.Auto_Teaching_Z_Axis_Data * sizeof(short)), shortData, 0, sizeof(short));
            m_motion.SetAutoTeachingTargetZ((double)BitConverter.ToInt16(shortData, 0));
        }
        /// <summary>
        /// Master(or CIM)에게 전달할 Bit 정보 업데이트
        /// </summary>
        private void UpdateSendBitData() {
            foreach (SendBitMap item in Enum.GetValues(typeof(SendBitMap))) {
                switch (item) {
                    case SendBitMap.Servo_On_Ready:
                    case SendBitMap.Servo_Off_State:
                        m_RackMaster_SendBitMap[(int)item] = !m_motion.IsAllServoOn();
                        break;

                    case SendBitMap.Servo_On_State:
                    case SendBitMap.Servo_Off_Ready:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsAllServoOn();
                        break;

                    case SendBitMap.Auto_Ready:
                        if (m_RackMaster_SendBitMap[(int)SendBitMap.Auto_State]) {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = Interlock_AutoCondition();
                        }
                        break;

                    case SendBitMap.Auto_State:
                        if (!Interlock_AutoCondition()) {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case SendBitMap.Manual_Ready:
                        m_RackMaster_SendBitMap[(int)item] = IsAutoState();
                        break;

                    case SendBitMap.Manual_State:
                        m_RackMaster_SendBitMap[(int)item] = !IsAutoState();
                        if (!GetSendBit(SendBitMap.Auto_State))
                            ManualInitial();
                        break;

                    case SendBitMap.Error_State:
                        m_RackMaster_SendBitMap[(int)item] = m_alarm.IsAlarmState();
                        break;

                    case SendBitMap.Home_State:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsAllHomeDone();
                        break;

                    case SendBitMap.Fan1_On_State:
                        break;

                    case SendBitMap.Fan2_On_State:
                        break;

                    case SendBitMap.From_Start_ACK:
                        m_RackMaster_SendBitMap[(int)item] = (GetReceiveBit(ReceiveBitMap.RM_From_Start_Request) && m_motion.IsFromRun());
                        break;

                    case SendBitMap.From_Complete:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsFromComplete();
                        break;

                    case SendBitMap.To_Start_ACK:
                        m_RackMaster_SendBitMap[(int)item] = (GetReceiveBit(ReceiveBitMap.RM_To_Start_Request) && m_motion.IsToRun());
                        break;

                    case SendBitMap.To_Complete:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsToComplete();
                        break;

                    case SendBitMap.Maint_Move_Start_ACK:
                        m_RackMaster_SendBitMap[(int)item] = (GetReceiveBit(ReceiveBitMap.RM_Maint_Start_Request) && m_motion.IsMaintRun());
                        break;

                    case SendBitMap.Maint_Move_Complete:
                        //m_RackMaster_SendBitMap[(int)item] = m_motion.IsMaintComplete();
                        break;

                    case SendBitMap.Store_Alt_Request:
                        m_RackMaster_SendBitMap[(int)item] = (m_motion.GetCurrentEvent() == EventList.StoreAlt);
                        break;

                    case SendBitMap.Source_Empty_Request:
                        m_RackMaster_SendBitMap[(int)item] = (m_motion.GetCurrentEvent() == EventList.SourceEmpty);
                        break;

                    case SendBitMap.Double_Storage_Request:
                        m_RackMaster_SendBitMap[(int)item] = (m_motion.GetCurrentEvent() == EventList.DoubleStorage);
                        break;

                    case SendBitMap.Resume_Request_Request:
                        m_RackMaster_SendBitMap[(int)item] = (m_motion.GetCurrentEvent() == EventList.ResumeRequest);
                        break;

                    case SendBitMap.Teaching_RW_Ready:
                        m_RackMaster_SendBitMap[(int)item] = Interlock_TeachingRWReady();
                        break;

                    case SendBitMap.Teaching_Read_Complete:
                        if (!m_RackMaster_RecvBitMap[(int)ReceiveBitMap.RM_Teaching_Read_Write_Start]) {
                            UpdateTeachingData(SendWordMap.Teaching_X_Axis_Data_0, 0);
                            UpdateTeachingData(SendWordMap.Teaching_Z_Axis_Data_0, 0);
                            UpdateTeachingData(SendWordMap.Teaching_A_Axis_Data_0, 0);
                            UpdateTeachingData(SendWordMap.Teaching_T_Axis_Data_0, 0);
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case SendBitMap.Accessing_RM_Down_From:
                        //m_RackMaster_SendBitMap[(int)item] = m_motion.IsDownState(MotionMode.From);
                        break;

                    case SendBitMap.Accessing_RM_Down_To:
                        //m_RackMaster_SendBitMap[(int)item] = m_motion.IsDownState(MotionMode.To);
                        break;

                    case SendBitMap.From_Ready:
                        m_RackMaster_SendBitMap[(int)item] = Interlock_FromReady();
                        break;

                    case SendBitMap.To_Ready:
                        m_RackMaster_SendBitMap[(int)item] = Interlock_ToReady();
                        break;

                    case SendBitMap.Idle_State:
                        m_RackMaster_SendBitMap[(int)item] = !m_motion.IsAutoMotionRun();
                        break;

                    case SendBitMap.Active_State:
                        m_RackMaster_SendBitMap[(int)item] = (m_motion.IsAutoMotionRun() && !m_alarm.IsAlarmState() && m_motion.GetCurrentAutoStep() != AutoStep.Step500_Error);
                        break;

                    case SendBitMap.XZT_Going_State:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsXZTGoing();
                        break;

                    case SendBitMap.Maint_Move_State:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsMaintRun();
                        break;

                    case SendBitMap.From_State:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsFromRun();
                        break;

                    case SendBitMap.To_State:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsToRun();
                        break;

                    case SendBitMap.X_Axis_FWD_State:
                        m_RackMaster_SendBitMap[(int)item] = (m_motion.GetAxisStatus(AxisStatusType.vel_cmd, AxisList.X_Axis) > 0 && m_motion.GetAxisFlag(AxisFlagType.Servo_On, AxisList.X_Axis));
                        break;

                    case SendBitMap.X_Axis_BWD_State:
                        m_RackMaster_SendBitMap[(int)item] = (m_motion.GetAxisStatus(AxisStatusType.vel_cmd, AxisList.X_Axis) < 0 && m_motion.GetAxisFlag(AxisFlagType.Servo_On, AxisList.X_Axis));
                        break;

                    case SendBitMap.X_Axis_Max_Speed_State:
                        m_RackMaster_SendBitMap[(int)item] = (m_motion.GetAxisStatus(AxisStatusType.vel_cmd, AxisList.X_Axis) >= m_param.GetAxisParameter(AxisList.X_Axis).maxSpeed);
                        break;

                    case SendBitMap.Arm_Home_Position_State:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsForkHomeCheck();
                        break;

                    case SendBitMap.Turn_Left_Position_State:
                        m_RackMaster_SendBitMap[(int)item] = (m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.T_Axis) < m_param.GetMotionParam().turnCenterPosition * 1000);
                        break;

                    case SendBitMap.Turn_Right_Position_State:
                        m_RackMaster_SendBitMap[(int)item] = (m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.T_Axis) > m_param.GetMotionParam().turnCenterPosition * 1000);
                        break;

                    case SendBitMap.CST_Empty:
                        m_RackMaster_SendBitMap[(int)item] = IsCassetteEmpty();
                        break;

                    case SendBitMap.CST_ON:
                        m_RackMaster_SendBitMap[(int)item] = IsCassetteOn();
                        break;

                    case SendBitMap.X_Axis_Power_On:
                        break;

                    case SendBitMap.X_Axis_Servo_On:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisFlag(AxisFlagType.Servo_On, AxisList.X_Axis);
                        break;

                    case SendBitMap.X_Axis_HomeDone:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisFlag(AxisFlagType.HomeDone, AxisList.X_Axis);
                        break;

                    case SendBitMap.Z_Axis_Power_On:
                        break;

                    case SendBitMap.Z_Axis_Servo_On:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisFlag(AxisFlagType.Servo_On, AxisList.Z_Axis);
                        break;

                    case SendBitMap.Z_Axis_HomeDone:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisFlag(AxisFlagType.HomeDone, AxisList.Z_Axis);
                        break;

                    case SendBitMap.A_Axis_Power_On:
                        break;

                    case SendBitMap.A_Axis_Servo_On:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisFlag(AxisFlagType.Servo_On, AxisList.A_Axis);
                        break;

                    case SendBitMap.A_Axis_HomeDone:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisFlag(AxisFlagType.HomeDone, AxisList.A_Axis);
                        break;

                    case SendBitMap.T_Axis_Power_On:
                        break;

                    case SendBitMap.T_Axis_Servo_On:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisFlag(AxisFlagType.Servo_On, AxisList.T_Axis);
                        break;

                    case SendBitMap.T_Axis_HomeDone:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisFlag(AxisFlagType.HomeDone, AxisList.T_Axis);
                        break;

                    case SendBitMap.EMO_On:
                        m_RackMaster_SendBitMap[(int)item] = (GetInputBit(InputList.EMO_HP) || GetInputBit(InputList.EMO_OP));
                        break;

                    case SendBitMap.GOT_EMO_On:
                        //m_RackMaster_SendBitMap[(int)item] = GetInputBit(InputList.HP_DTP_EMS_SW);
                        m_RackMaster_SendBitMap[(int)item] = Interlock_GOTEmoOn();
                        break;

                    case SendBitMap.Double_Storage_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = GetInputBit(InputList.Double_Storage_Sensor_1);
                        break;

                    case SendBitMap.Arm_CST_All_Undetected:
                        m_RackMaster_SendBitMap[(int)item] = !IsCassetteOn();
                        break;

                    case SendBitMap.Arm_CST_1_Detected_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = !GetInputBit(InputList.Fork_In_Place_1);
                        break;

                    case SendBitMap.Arm_CST_2_Detected_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = !GetInputBit(InputList.Fork_In_Place_2);
                        break;

                    case SendBitMap.Arm_CST_Diagonal_Detected_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = GetInputBit(InputList.Presense_Detection_1);
                        break;

                    case SendBitMap.Left_From_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = GetInputBit(InputList.Fork_Pick_Sensor_Left);
                        break;

                    case SendBitMap.Left_To_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = GetInputBit(InputList.Fork_Place_Sensor_Left);
                        break;

                    case SendBitMap.Right_From_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = GetInputBit(InputList.Fork_Pick_Sensor_Left);
                        break;

                    case SendBitMap.Right_To_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = GetInputBit(InputList.Fork_Place_Sensor_Left);
                        break;

                    case SendBitMap.Left_1_Projecting_Sensor_Front:
                        break;

                    case SendBitMap.Left_2_Projecting_Sensor_Rear:
                        break;

                    case SendBitMap.Right_1_Projecting_Sensor_Front:
                        break;

                    case SendBitMap.Right_2_Projecting_Sensor_Rear:
                        break;

                    case SendBitMap.Z_Axis_HP_Home_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisSensor(AxisSensorType.Home, AxisList.Z_Axis);

                        break;

                    case SendBitMap.Z_Axis_HP_NOT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisSensor(AxisSensorType.Negative_Limit, AxisList.Z_Axis);
                        break;

                    case SendBitMap.Z_Axis_HP_POT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisSensor(AxisSensorType.Positive_Limit, AxisList.Z_Axis);

                        break;

                    case SendBitMap.HP_EMO_Push:
                        m_RackMaster_SendBitMap[(int)item] = GetInputBit(InputList.EMO_HP);
                        break;

                    case SendBitMap.OP_EMO_Push:
                        m_RackMaster_SendBitMap[(int)item] = GetInputBit(InputList.EMO_OP);
                        break;

                    case SendBitMap.GOT_Detect:
                        m_RackMaster_ErrorWord[(int)item] = IsGOTDetected_HP() || IsGOTDetected_OP();

                        break;

                    case SendBitMap.Position_Sensor_1:
                        break;

                    case SendBitMap.Position_Sensor_2:
                        break;

                    case SendBitMap.X_Axis_HP_Home_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisSensor(AxisSensorType.Home, AxisList.X_Axis);
                        break;

                    case SendBitMap.X_Axis_HP_Slow_Sensor:
                        break;

                    case SendBitMap.X_Axis_HP_NOT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisSensor(AxisSensorType.Negative_Limit, AxisList.X_Axis);
                        break;

                    case SendBitMap.X_Axis_OP_Slow_Sensor:
                        break;

                    case SendBitMap.X_Axis_OP_POT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisSensor(AxisSensorType.Positive_Limit, AxisList.X_Axis);
                        break;

                    case SendBitMap.A_Axis_Home_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisSensor(AxisSensorType.Home, AxisList.A_Axis);
                        break;

                    case SendBitMap.A_Axis_NOT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisSensor(AxisSensorType.Negative_Limit, AxisList.A_Axis);
                        break;

                    case SendBitMap.A_Axis_POT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisSensor(AxisSensorType.Positive_Limit, AxisList.A_Axis);
                        break;

                    case SendBitMap.A_Axis_POS_1_Sensor:
                        break;

                    case SendBitMap.A_Axis_POS_2_Sensor:
                        break;

                    case SendBitMap.A_Axis_POS_3_Sensor:
                        break;

                    case SendBitMap.T_Axis_Home_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisSensor(AxisSensorType.Home, AxisList.T_Axis);
                        break;

                    case SendBitMap.T_Axis_NOT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisSensor(AxisSensorType.Negative_Limit, AxisList.T_Axis);
                        break;

                    case SendBitMap.T_Axis_POT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisSensor(AxisSensorType.Positive_Limit, AxisList.T_Axis);
                        break;

                    case SendBitMap.T_Axis_POS_Sensor:
                        break;

                    case SendBitMap.CPS_Second_Run:
                        break;

                    case SendBitMap.CPS_Second_Fault:
                        break;

                    case SendBitMap.From_Step_0_Complete:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsCompleteFromStep(FromToStepNumber.Zero);
                        break;

                    case SendBitMap.From_Step_1_Complete:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsCompleteFromStep(FromToStepNumber.One);
                        break;

                    case SendBitMap.From_Step_2_Complete:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsCompleteFromStep(FromToStepNumber.Two);
                        break;

                    case SendBitMap.From_Step_3_Complete:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsCompleteFromStep(FromToStepNumber.Three);
                        break;

                    case SendBitMap.From_Step_4_Complete:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsCompleteFromStep(FromToStepNumber.Four);
                        break;

                    case SendBitMap.To_Step_0_Complete:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsCompleteToStep(FromToStepNumber.Zero);
                        break;

                    case SendBitMap.To_Step_1_Complete:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsCompleteToStep(FromToStepNumber.One);
                        break;

                    case SendBitMap.To_Step_2_Complete:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsCompleteToStep(FromToStepNumber.Two);
                        break;

                    case SendBitMap.To_Step_3_Complete:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsCompleteToStep(FromToStepNumber.Three);
                        break;

                    case SendBitMap.To_Step_4_Complete:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.IsCompleteToStep(FromToStepNumber.Four);
                        break;

                    case SendBitMap.Communication_Check:
                        break;

                    case SendBitMap.X_Axis_Busy:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisFlag(AxisFlagType.Run, AxisList.X_Axis);
                        break;

                    case SendBitMap.Z_Axis_Busy:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisFlag(AxisFlagType.Run, AxisList.Z_Axis);
                        break;

                    case SendBitMap.A_Axis_Busy:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisFlag(AxisFlagType.Run, AxisList.A_Axis);
                        break;

                    case SendBitMap.T_Axis_Busy:
                        m_RackMaster_SendBitMap[(int)item] = m_motion.GetAxisFlag(AxisFlagType.Run, AxisList.T_Axis);
                        break;
                }
            }
        }
        /// <summary>
        /// Master(or CIM)에게 전달할 Word 정보 업데이트
        /// </summary>
        public void UpdateSendWordData() {
            float curPosX = (float)(m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.X_Axis) / 1000);
            float curPosZ = 0;
            if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum) {
                curPosZ = (float)(m_motion.GetDrumBeltZAxisActualPosition() / 1000);
            }
            else {
                curPosZ = (float)(m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.Z_Axis) / 1000);
            }
            float curPosA = (float)(m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.A_Axis) / 1000);
            float curPosT = (float)(m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.T_Axis) / 1000);

            float curTargetX = (float)(m_motion.GetAxisStatus(AxisStatusType.Profile_Traget_Pisition, AxisList.X_Axis) / 1000);
            float curTargetZ = 0;
            if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum) {
                curTargetZ = (float)(m_motion.GetDrumBeltZAxisTargetPosition() / 1000);
            }
            else {
                curTargetZ = (float)(m_motion.GetAxisStatus(AxisStatusType.Profile_Traget_Pisition, AxisList.Z_Axis) / 1000);
            }
            float curTargetA = (float)(m_motion.GetAxisStatus(AxisStatusType.Profile_Traget_Pisition, AxisList.A_Axis) / 1000);
            float curTargetT = (float)(m_motion.GetAxisStatus(AxisStatusType.Profile_Traget_Pisition, AxisList.T_Axis) / 1000);

            float curVelX = (float)(m_motion.GetAxisStatus(AxisStatusType.vel_act, AxisList.X_Axis) / 1000000 * 60);
            float curVelZ = (float)(m_motion.GetAxisStatus(AxisStatusType.vel_act, AxisList.Z_Axis) / 1000000 * 60);
            if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum) {
                curVelZ = (float)(m_motion.GetDrumBeltZAxisActualVelocity() / 1000);
            }
            float curVelA = 0;
            if(m_param.GetMotionParam().forkType == ForkType.SCARA) {
                curVelA = (float)(m_motion.GetAxisStatus(AxisStatusType.vel_act, AxisList.A_Axis) / 1000 * 60);
            }
            else {
                curVelA = (float)(m_motion.GetAxisStatus(AxisStatusType.vel_act, AxisList.A_Axis) / 1000000 * 60);
            }
            float curVelT = (float)(m_motion.GetAxisStatus(AxisStatusType.vel_act, AxisList.T_Axis) / 1000 * 60);

            short torqueX = (short)(m_motion.GetAxisStatus(AxisStatusType.trq_act, AxisList.X_Axis));
            short torqueZ = (short)(m_motion.GetAxisStatus(AxisStatusType.trq_act, AxisList.Z_Axis));
            short torqueA = (short)(m_motion.GetAxisStatus(AxisStatusType.trq_act, AxisList.A_Axis));
            short torqueT = (short)(m_motion.GetAxisStatus(AxisStatusType.trq_act, AxisList.T_Axis));

            short overloadSettingX = (short)m_param.GetAxisParameter(AxisList.X_Axis).maxOverload;
            short overloadSettingZ = (short)m_param.GetAxisParameter(AxisList.Z_Axis).maxOverload;
            short overloadSettingA = (short)m_param.GetAxisParameter(AxisList.A_Axis).maxOverload;
            short overloadSettingT = (short)m_param.GetAxisParameter(AxisList.T_Axis).maxOverload;

            short overloadCurrentX = (short)m_data.GetAxisAverageTorque(AxisList.X_Axis);
            short overloadCurrentZ = (short)m_data.GetAxisAverageTorque(AxisList.Z_Axis);
            short overloadCurrentA = (short)m_data.GetAxisAverageTorque(AxisList.A_Axis);
            short overloadCurrentT = (short)m_data.GetAxisAverageTorque(AxisList.T_Axis);

            short autoSpeedPercentX = (short)m_param.GetAxisParameter(AxisList.X_Axis).autoSpeedPercent;
            short autoSpeedPercentZ = (short)m_param.GetAxisParameter(AxisList.Z_Axis).autoSpeedPercent;
            short autoSpeedPercentA = (short)m_param.GetAxisParameter(AxisList.A_Axis).autoSpeedPercent;
            short autoSpeedPercentT = (short)m_param.GetAxisParameter(AxisList.T_Axis).autoSpeedPercent;

            int autoTeachingId = m_motion.GetAutoTeachingTargetID();
            short autoTeachingTargetX = (short)m_motion.GetAutoTeachingTargetX();
            short autoTeachingTargetZ = (short)m_motion.GetAutoTeachingTargetZ();

            UpdateServoStatus(SendWordMap.X_Axis_Cur_Position_0, curPosX);
            UpdateServoStatus(SendWordMap.Z_Axis_Cur_Position_0, curPosZ);
            UpdateServoStatus(SendWordMap.A_Axis_Cur_Position_0, curPosA);
            UpdateServoStatus(SendWordMap.T_Axis_Cur_Position_0, curPosT);

            UpdateServoStatus(SendWordMap.X_Axis_Target_Position_0, curTargetX);
            UpdateServoStatus(SendWordMap.Z_Axis_Target_Position_0, curTargetZ);
            UpdateServoStatus(SendWordMap.A_Axis_Target_Position_0, curTargetA);
            UpdateServoStatus(SendWordMap.T_Axis_Target_Position_0, curTargetT);

            UpdateServoStatus(SendWordMap.X_Axis_Cur_Velocity_0, curVelX);
            UpdateServoStatus(SendWordMap.Z_Axis_Cur_Velocity_0, curVelZ);
            UpdateServoStatus(SendWordMap.A_Axis_Cur_Velocity_0, curVelA);
            UpdateServoStatus(SendWordMap.T_Axis_Cur_Velocity_0, curVelT);

            UpdateServoStatus(SendWordMap.X_Axis_Cur_Torque, torqueX);
            UpdateServoStatus(SendWordMap.Z_Axis_Cur_Torque, torqueZ);
            UpdateServoStatus(SendWordMap.A_Axis_Cur_Torque, torqueA);
            UpdateServoStatus(SendWordMap.T_Axis_Cur_Torque, torqueT);

            UpdateServoStatus(SendWordMap.X_Axis_Peak_Torque, (short)m_data.GetAxisPeakTorque(AxisList.X_Axis));
            UpdateServoStatus(SendWordMap.Z_Axis_Peak_Torque, (short)m_data.GetAxisPeakTorque(AxisList.Z_Axis));
            UpdateServoStatus(SendWordMap.A_Axis_Peak_Torque, (short)m_data.GetAxisPeakTorque(AxisList.A_Axis));
            UpdateServoStatus(SendWordMap.T_Axis_Peak_Torque, (short)m_data.GetAxisPeakTorque(AxisList.T_Axis));

            UpdateServoStatus(SendWordMap.X_Axis_Avr_Torque, (short)m_data.GetAxisAverageTorque(AxisList.X_Axis));
            UpdateServoStatus(SendWordMap.Z_Axis_Avr_Torque, (short)m_data.GetAxisAverageTorque(AxisList.Z_Axis));
            UpdateServoStatus(SendWordMap.A_Axis_Avr_Torque, (short)m_data.GetAxisAverageTorque(AxisList.A_Axis));
            UpdateServoStatus(SendWordMap.T_Axis_Avr_Torque, (short)m_data.GetAxisAverageTorque(AxisList.T_Axis));

            UpdateServoStatus(SendWordMap.X_Axis_Setting_Max_OverLoad, overloadSettingX);
            UpdateServoStatus(SendWordMap.Z_Axis_Setting_Max_OverLoad, overloadSettingZ);
            UpdateServoStatus(SendWordMap.A_Axis_Setting_Max_OverLoad, overloadSettingA);
            UpdateServoStatus(SendWordMap.T_Axis_Setting_Max_OverLoad, overloadSettingT);

            UpdateServoStatus(SendWordMap.X_Axis_Cur_Max_OverLoad, overloadCurrentX);
            UpdateServoStatus(SendWordMap.Z_Axis_Cur_Max_OverLoad, overloadCurrentZ);
            UpdateServoStatus(SendWordMap.A_Axis_Cur_Max_OverLoad, overloadCurrentA);
            UpdateServoStatus(SendWordMap.T_Axis_Cur_Max_OverLoad, overloadCurrentT);

            UpdateAutoSpeedPecent(SendWordMap.Auto_Speed_Percent_X, autoSpeedPercentX);
            UpdateAutoSpeedPecent(SendWordMap.Auto_Speed_Percent_Z, autoSpeedPercentZ);
            UpdateAutoSpeedPecent(SendWordMap.Auto_Speed_Percent_A, autoSpeedPercentA);
            UpdateAutoSpeedPecent(SendWordMap.Auto_Speed_Percent_T, autoSpeedPercentT);

            UpdateAutoTeachingID(SendWordMap.Auto_Teaching_Id_0, autoTeachingId);
            UpdateAutoTeachingData(SendWordMap.Auto_Teaching_Target_X, autoTeachingTargetX);
            UpdateAutoTeachingData(SendWordMap.Auto_Teaching_Target_Z, autoTeachingTargetZ);

            int fromId = m_motion.GetCurrentTargetFromID();
            int toID = m_motion.GetCurrentTargetToID();

            UpdateFromToID(SendWordMap.FROM_SHELF_ID_0, fromId);
            UpdateFromToID(SendWordMap.TO_SHELF_ID_0, toID);

            UpdateAlarmCode();

            UpdateAccessShelfID();

            UpdateAutoStepNumber(SendWordMap.RM_Auto_Step_Number, (short)m_motion.GetCurrentAutoStep());

            UpdateRegulatorData(SendWordMap.Regulator_BoostVoltage, Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.BoostVoltage));
            UpdateRegulatorData(SendWordMap.Regulator_OutputVoltage, Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.OutputVoltage));
            UpdateRegulatorData(SendWordMap.Regulator_IBoostCurrent, Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.BoostCurrent));
            UpdateRegulatorData(SendWordMap.Regulator_OutputCurrent, Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.OutputCurrent));
            UpdateRegulatorData(SendWordMap.Regulator_PickupTemperature, Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.PickupNTC));
            UpdateRegulatorData(SendWordMap.Regulator_HitSinkTemperature, Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.HitSinkTemperature));
            UpdateRegulatorData(SendWordMap.Regulator_InsideTemperature, Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.InsideNTC));
        }
        /// <summary>
        /// Regulator 데이터를 Word 값으로 업데이트
        /// </summary>
        /// <param name="wordMapIndex"></param>
        /// <param name="value"></param>
        private void UpdateRegulatorData(SendWordMap wordMapIndex, short value) {
            switch (wordMapIndex) {
                case SendWordMap.Regulator_BoostVoltage:
                case SendWordMap.Regulator_OutputVoltage:
                case SendWordMap.Regulator_IBoostCurrent:
                case SendWordMap.Regulator_OutputCurrent:
                case SendWordMap.Regulator_PickupTemperature:
                case SendWordMap.Regulator_HitSinkTemperature:
                case SendWordMap.Regulator_InsideTemperature:
                    if(m_param.GetMotionParam().useRegulator && IsConnected_Regulator()) {
                        byte[] shorData = BitConverter.GetBytes(value);
                        Buffer.BlockCopy(shorData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(short));
                    }
                    else {
                        m_RackMaster_SendWordMap[(int)wordMapIndex] = 0;
                    }
                    break;
            }
        }
        /// <summary>
        /// 현재 Auto Step 번호를 Word 값으로 업데이트
        /// </summary>
        /// <param name="wordMapIndex"></param>
        /// <param name="value"></param>
        private void UpdateAutoStepNumber(SendWordMap wordMapIndex, short value) {
            switch (wordMapIndex) {
                case SendWordMap.RM_Auto_Step_Number:
                    byte[] shortData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(shortData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(short));
                    break;
            }
        }
        /// <summary>
        /// 현재 축 상태를 Word 값으로 업데이트(float 값)
        /// </summary>
        /// <param name="wordMapIndex"></param>
        /// <param name="value"></param>
        private void UpdateServoStatus(SendWordMap wordMapIndex, float value) {
            switch (wordMapIndex) {
                case SendWordMap.X_Axis_Cur_Position_0:
                case SendWordMap.Z_Axis_Cur_Position_0:
                case SendWordMap.A_Axis_Cur_Position_0:
                case SendWordMap.T_Axis_Cur_Position_0:
                case SendWordMap.X_Axis_Target_Position_0:
                case SendWordMap.Z_Axis_Target_Position_0:
                case SendWordMap.A_Axis_Target_Position_0:
                case SendWordMap.T_Axis_Target_Position_0:
                case SendWordMap.X_Axis_Cur_Velocity_0:
                case SendWordMap.Z_Axis_Cur_Velocity_0:
                case SendWordMap.A_Axis_Cur_Velocity_0:
                case SendWordMap.T_Axis_Cur_Velocity_0:
                    byte[] floatData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(floatData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(float));
                    break;
            }
        }
        /// <summary>
        /// 현재 축 상태를 Word 값으로 업데이트(short 값)
        /// </summary>
        /// <param name="wordMapIndex"></param>
        /// <param name="value"></param>
        private void UpdateServoStatus(SendWordMap wordMapIndex, short value) {
            switch (wordMapIndex) {
                case SendWordMap.X_Axis_Cur_Torque:
                case SendWordMap.Z_Axis_Cur_Torque:
                case SendWordMap.A_Axis_Cur_Torque:
                case SendWordMap.T_Axis_Cur_Torque:
                case SendWordMap.X_Axis_Avr_Torque:
                case SendWordMap.Z_Axis_Avr_Torque:
                case SendWordMap.A_Axis_Avr_Torque:
                case SendWordMap.T_Axis_Avr_Torque:
                case SendWordMap.X_Axis_Peak_Torque:
                case SendWordMap.Z_Axis_Peak_Torque:
                case SendWordMap.A_Axis_Peak_Torque:
                case SendWordMap.T_Axis_Peak_Torque:
                case SendWordMap.X_Axis_Setting_Max_OverLoad:
                case SendWordMap.Z_Axis_Setting_Max_OverLoad:
                case SendWordMap.A_Axis_Setting_Max_OverLoad:
                case SendWordMap.T_Axis_Setting_Max_OverLoad:
                case SendWordMap.X_Axis_Cur_Max_OverLoad:
                case SendWordMap.Z_Axis_Cur_Max_OverLoad:
                case SendWordMap.A_Axis_Cur_Max_OverLoad:
                case SendWordMap.T_Axis_Cur_Max_OverLoad:
                    byte[] shortData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(shortData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(short));
                    break;
            }
        }
        /// <summary>
        /// Master(or CIM)에게 받은 CST ID를 Master(or CIM)에게 전달할 CST ID 값에 복사
        /// </summary>
        public void SetCassetteID() {
            for(int idx = (int)SendWordMap.CST_ID_PIO_Word_0; idx <= (int)SendWordMap.CST_ID_PIO_Word_57; idx++) {
                m_RackMaster_SendWordMap[idx] = m_RackMaster_RecvWordMap[idx];
            }
        }
        /// <summary>
        /// CST ID Clear
        /// </summary>
        public void ClearCassetteID() {
            foreach (SendWordMap item in Enum.GetValues(typeof(SendWordMap))) {
                if (item >= SendWordMap.CST_ID_PIO_Word_0 &&
                    item <= SendWordMap.CST_ID_PIO_Word_57) {
                    m_RackMaster_SendWordMap[(int)item] = 0;
                }
            }
        }
        /// <summary>
        /// Master(or CIM)에게 현재 상태의 From/To Target ID를 업데이트
        /// </summary>
        /// <param name="wordMapIndex"></param>
        /// <param name="value"></param>
        private void UpdateFromToID(SendWordMap wordMapIndex, int value) {
            switch (wordMapIndex) {
                case SendWordMap.FROM_SHELF_ID_0:
                    byte[] fromIdData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(fromIdData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(int));
                    break;

                case SendWordMap.TO_SHELF_ID_0:
                    byte[] toIdData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(toIdData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(int));
                    break;
            }
        }
        /// <summary>
        /// Master(or CIM)에게 전달할 Access ID 업데이트. Access ID의 경우 현재 Auto Step 에서의 타겟 ID
        /// </summary>
        private void UpdateAccessShelfID() {
            if (m_RackMaster_SendBitMap[(int)SendBitMap.Auto_State]) {
                if (m_motion.GetCurrentAutoStep() >= AutoStep.Step100_From_ID_Check &&
                m_motion.GetCurrentAutoStep() <= AutoStep.Step145_From_Complete) {
                    byte[] intData = BitConverter.GetBytes(m_motion.GetCurrentTargetFromID());
                    Buffer.BlockCopy(intData, 0, m_RackMaster_SendWordMap, (int)SendWordMap.Access_Shelf_ID_0 * sizeof(short), sizeof(int));
                }
                else if (m_motion.GetCurrentAutoStep() >= AutoStep.Step200_To_ID_Check &&
                    m_motion.GetCurrentAutoStep() <= AutoStep.Step220_To_Complete) {
                    byte[] intData = BitConverter.GetBytes(m_motion.GetCurrentTargetToID());
                    Buffer.BlockCopy(intData, 0, m_RackMaster_SendWordMap, (int)SendWordMap.Access_Shelf_ID_0 * sizeof(short), sizeof(int));
                }
                else {
                    ClearSendWord(SendWordMap.Access_Shelf_ID_0);
                    ClearSendWord(SendWordMap.Access_Shelf_ID_1);
                }
            }
            else {
                ClearSendWord(SendWordMap.Access_Shelf_ID_0);
                ClearSendWord(SendWordMap.Access_Shelf_ID_1);
            }
        }
        /// <summary>
        /// Master(or CIM)에게 전달할 Teaching Data 업데이트
        /// </summary>
        /// <param name="wordMapIndex"></param>
        /// <param name="value"></param>
        private void UpdateTeachingData(SendWordMap wordMapIndex, float value) {
            switch (wordMapIndex) {
                case SendWordMap.Teaching_X_Axis_Data_0:
                case SendWordMap.Teaching_Z_Axis_Data_0:
                case SendWordMap.Teaching_A_Axis_Data_0:
                case SendWordMap.Teaching_T_Axis_Data_0:
                    byte[] floatData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(floatData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(float));
                    break;
            }
        }
        /// <summary>
        /// Master(or CIM)에게 전달할 Auto Speed Percent 업데이트
        /// </summary>
        /// <param name="wordMapIndex"></param>
        /// <param name="value"></param>
        private void UpdateAutoSpeedPecent(SendWordMap wordMapIndex, short value) {
            switch (wordMapIndex) {
                case SendWordMap.Auto_Speed_Percent_X:
                case SendWordMap.Auto_Speed_Percent_Z:
                case SendWordMap.Auto_Speed_Percent_A:
                case SendWordMap.Auto_Speed_Percent_T:
                    byte[] shortData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(shortData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(short));
                    break;
            }
        }
        /// <summary>
        /// Master(or CIM)에게 전달할 현재 Auto Teaching ID 업데이트
        /// </summary>
        /// <param name="wordMapIndex"></param>
        /// <param name="value"></param>
        private void UpdateAutoTeachingID(SendWordMap wordMapIndex, int value) {
            switch (wordMapIndex) {
                case SendWordMap.Auto_Teaching_Id_0:
                    byte[] intData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(intData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(int));
                    break;
            }
        }
        /// <summary>
        /// Master(or CIM)에게 전달할 현재 Auto Teachin Target X,Z 데이터 업데이트
        /// </summary>
        /// <param name="wordMapIndex"></param>
        /// <param name="value"></param>
        private void UpdateAutoTeachingData(SendWordMap wordMapIndex, short value) {
            switch (wordMapIndex) {
                case SendWordMap.Auto_Teaching_Target_X:
                case SendWordMap.Auto_Teaching_Target_Z:
                    byte[] shortData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(shortData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(short));
                    break;
            }
        }
        /// <summary>
        /// Master(or CIM)에게 전달할 현재 Rack Master의 알람코드 Update
        /// </summary>
        private void UpdateAlarmCode() {
            if (!m_alarm.IsAlarmState()) {
                ClearAlarmCodeWord();
                return;
            }

            for(int i = 0; i < m_alarm.GetCurrentAlarmCount(); i++) {
                m_RackMaster_ErrorWord[m_alarm.GetAlarmCode(i)] = true;
            }

            int bSize = m_RackMaster_ErrorWord.Length / 8;
            byte[] errorByteArr = new byte[bSize];
            m_RackMaster_ErrorWord.CopyTo(errorByteArr, 0);
            Buffer.BlockCopy(errorByteArr, 0, m_RackMaster_SendWordMap, ((int)SendWordMap.Error_Word_0) * sizeof(short), bSize);
            int ret = 0;
        }
        /// <summary>
        /// Master(or CIM)에게 전달할 현재 Rack Master의 알람코드 클리어. 알람 클리어시 적용
        /// </summary>
        private void ClearAlarmCodeWord() {
            for(int i = 0; i < m_RackMaster_ErrorWord.Length; i++) {
                m_RackMaster_ErrorWord[i] = false;
            }

            ClearSendWord(SendWordMap.Error_Word_0);
            ClearSendWord(SendWordMap.Error_Word_1);
            ClearSendWord(SendWordMap.Error_Word_2);
            ClearSendWord(SendWordMap.Error_Word_3);
            ClearSendWord(SendWordMap.Error_Word_4);
            ClearSendWord(SendWordMap.Error_Word_5);
            ClearSendWord(SendWordMap.Error_Word_6);
            ClearSendWord(SendWordMap.Error_Word_7);
            ClearSendWord(SendWordMap.Error_Word_8);
            ClearSendWord(SendWordMap.Error_Word_9);
            ClearSendWord(SendWordMap.Error_Word_10);
            ClearSendWord(SendWordMap.Error_Word_11);
            ClearSendWord(SendWordMap.Error_Word_12);
        }
        /// <summary>
        /// Master(or CIM)에게 전달할 Word 데이터 클리어(특정 데이터만)
        /// </summary>
        /// <param name="mapIndex"></param>
        private void ClearSendWord(SendWordMap mapIndex) {
            m_RackMaster_SendWordMap[(int)mapIndex] = 0;
        }
        /// <summary>
        /// Master(or CIM)에게 전달할 Bit 데이터 클리어(특정 데이터만)
        /// </summary>
        /// <param name="mapIndex"></param>
        private void ClearSendBit(SendBitMap mapIndex) {
            m_RackMaster_SendBitMap[(int)mapIndex] = false;
        }
        /// <summary>
        /// Master(or CIM)에게 받은 Word 데이터 클리어(특정 데이터만)
        /// </summary>
        /// <param name="mapIndex"></param>
        private void ClearReceiveWord(ReceiveWordMap mapIndex) {
            if (!m_RackMaster_SendBitMap[(int)SendBitMap.Auto_State]) {
                m_RackMaster_RecvWordMap[(int)mapIndex] = 0;
            }
        }
        /// <summary>
        /// Master(or CIM)에게 받은 Bit 데이터 클리어(특정 데이터만)
        /// </summary>
        /// <param name="mapIndex"></param>
        private void ClearReceiveBit(ReceiveBitMap mapIndex) {
            if (!m_RackMaster_SendBitMap[(int)SendBitMap.Auto_State]) {
                m_RackMaster_RecvBitMap[(int)mapIndex] = false;
            }
        }
        /// <summary>
        /// Manual Mode로 진입 시 데이터 초기화
        /// </summary>
        private void ManualInitial() {
            ClearReceiveBit(ReceiveBitMap.RM_Auto_Request);
            ClearReceiveBit(ReceiveBitMap.RM_From_Start_Request);
            ClearReceiveBit(ReceiveBitMap.RM_To_Start_Request);
            ClearReceiveBit(ReceiveBitMap.RM_Maint_Start_Request);
            ClearReceiveBit(ReceiveBitMap.RM_Auto_Teaching_Start_Request);

            ClearSendBit(SendBitMap.Auto_State);
            ClearSendBit(SendBitMap.Manual_Ready);
        }
    }
}
