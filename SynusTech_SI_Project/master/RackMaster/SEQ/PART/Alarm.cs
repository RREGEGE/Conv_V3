using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SEQ.PART
{
    public static class Alarm
    {
        public enum AlarmList {
            HP_EStop,
            OP_EStop,
            HP_Key_Stop,
            OP_Key_Stop,
            GOT_EStop,
            Power_Off_Error,

            Test_Mode_Double_Storage_Error = 16,
            Test_Mode_CST_Presence_Error,
            Double_Sensor_Check_Error,
            Double_Sensor_Error,
            Source_Empty,
            Port_Interface_ULD_Event,
            Port_Interface_LD_Event,

            PLC_Battery_Low = 32,
            X_Servo_Battery_Low_Alarm,
            Z_Servo_Battery_Low_Alarm,
            A_Servo_Battery_Low_Alarm,
            T_Servo_Battery_Low_Alarm,

            X_Servo_Pack_Error = 48,
            X_Axis_POT,
            X_Axis_NOT,
            X_Axis_Origin_Search_Fail_Home_Sensor,
            X_Axis_Soft_Limit_Error,
            X_Axis_Dest_Position_Error,
            X_Axis_Absolute_Origin_Loss,
            X_Axis_Dest_Positioning_Complete_Error,
            X_Axis_Over_Load_Error,
            X_Axis_Home_Sensor_Always_On_Error,
            X_Home_Move_Time_Out_Error,
            XZ_Axis_Dest_Position_Error,

            X_Axis_Origin_Search_Fail_BCR = 64,
            X_Axis_Barcode_Sensor_Error,
            X_Axis_Free_Run,

            Z_Servo_Pack_Error = 80,
            Z_Axis_POT,
            Z_Axis_NOT,
            Z_Axis_Origin_Search_Fail_Home_Sensor,
            Z_Axis_Soft_Limit_Error,
            Z_Axis_Dest_Position_Error,
            Z_Axis_Absolute_Origin_Loss,
            Z_Axis_Dest_Positioning_Complete_Error,
            Z_Axis_Over_Load_Error,
            Z_Axis_Home_Sensor_Always_On_Error,
            Z_Home_Move_Time_Out_Error,
            Z_Axis_Unbalance_Error,

            A_Servo_Pack_Error = 96,
            A_Axis_POT,
            A_Axis_NOT,
            A_Axis_Origin_Search_Fail_Home_Sensor,
            A_Axis_Soft_Limit_Error,
            A_Axis_Dest_Position_Error,
            A_Axis_Absolute_Origin_Loss,
            A_Axis_Dest_Positioning_Complete_Error,
            A_Axis_Over_Load_Error,
            A_Axis_Home_Sensor_Always_On_Error,
            A_Home_Move_Time_Out_Error,
            RM_Moving_Arm_Home_Sensor_Dont_Detect_Error,
            Arm_Table_CST_Detect_Error_To_Complete,
            Cassette_Fail_To_X_Move,
            Arm_Table_Cross_Sensor_Error,
            Storage_Failure_Error_To_Complete,

            T_Servo_Pack_Error,
            T_Axis_POT,
            T_Axis_NOT,
            T_Axis_Origin_Search_Fail_Home_Sensor,
            T_Axis_Soft_Limit_Error,
            T_Axis_Dest_Position_Error,
            T_Axis_Absolute_Origin_Loss,
            T_Axis_Dest_Positioning_Complete_Error,
            T_Axis_Over_Load_Error,
            T_Axis_Home_Sensor_Always_On_Error,
            T_Home_Move_Time_Out_Error,

            Cammand_ID_Range_Error = 128,
            TO_Mode_Command_Over_Time_Error,
            PIO_Over_Time_Error,
            PIO_Ready_Off_Error,
            Stay_Complete_ACK_Over_Time_Error,
            Complete_ACK_Over_Time_Error,
            Store_ACK_Over_Time_Error,
            Source_Empty_ACK_Over_Time_Error,
            Double_Storage_ACK_Over_Time_Error,
            Idle_Status_CST_Error,
            A_Dest_FWD_Move_Time_Error,
            Z_Dest_Up_Down_Move_Time_Error,
            Z_Dest_Down_Move_Time_Error,
            A_Dest_BWD_Move_Time_Error,
            CST_Inplace_Sensor_1_Abnormal_Error,
            CST_Inplace_Sensor_2_Abnormal_Error,
        }

        public enum AlarmState {
            Idle,
            Error,
            Warning,
            Event
        }

        private static List<byte> m_alarm;
        private static Motor m_motor;

        public static AlarmState m_currentState;

        public static void Init() {
            m_alarm = new List<byte>();
            m_motor = Motor.Instance;
            m_currentState = AlarmState.Idle;
        }

        public static void AddAlarm(byte alarmCode, AlarmState state) {
            if (m_alarm.Contains(alarmCode))
                return;

            m_alarm.Add(alarmCode);
            m_currentState = state;

            if(state == AlarmState.Error) {
                RackMasterSEQ.SetStep((int)RackMasterSEQ.RM_STEP.Error);
            }

            return;
        }

        public static void ClearAlarm() {
            if(m_alarm.Count > 0) {
                m_alarm.Clear();
                return;
            }

            return;
        }

        public static int GetCurrentAlarmCount() {
            return m_alarm.Count;
        }

        public static byte GetAlarmCode(int index) {
            return m_alarm[index];
        }

        public static void AlarmCheck() {
            foreach(AlarmList item in Enum.GetValues(typeof(AlarmList))) {
                switch (item) {
                    case AlarmList.HP_EStop:
                        if (!Io.GetInputBit((int)IoList.InputList.EMO_HP)) {
                            AddAlarm((byte)item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.OP_EStop:
                        if (!Io.GetInputBit((int)IoList.InputList.EMO_OP)) {
                            AddAlarm((byte)item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.HP_Key_Stop:
                        break;

                    case AlarmList.OP_Key_Stop:
                        break;

                    case AlarmList.GOT_EStop:
                        if (Io.GetInputBit((int)IoList.InputList.Touch_EMO_SW)) {
                            AddAlarm((byte)item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.Power_Off_Error:
                        if (!Io.GetInputBit((int)IoList.InputList.RM_MC_On)) {
                            AddAlarm((byte)item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.Test_Mode_Double_Storage_Error:
                        break;

                    case AlarmList.Test_Mode_CST_Presence_Error:
                        break;

                    case AlarmList.Double_Sensor_Check_Error:
                        break;

                    case AlarmList.Double_Sensor_Error:
                        break;

                    case AlarmList.Source_Empty:
                        break;

                    case AlarmList.Port_Interface_ULD_Event:
                        break;

                    case AlarmList.Port_Interface_LD_Event:
                        break;

                    case AlarmList.PLC_Battery_Low:
                        break;

                    case AlarmList.X_Servo_Battery_Low_Alarm:
                        break;

                    case AlarmList.Z_Servo_Battery_Low_Alarm:
                        break;

                    case AlarmList.A_Servo_Battery_Low_Alarm:
                        break;

                    case AlarmList.T_Servo_Battery_Low_Alarm:
                        break;

                    case AlarmList.X_Servo_Pack_Error:
                        if (m_motor.m_status[(int)MtList.STK_X].m_alarm) {
                            AddAlarm((byte)item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.X_Axis_POT:
                        if (m_motor.m_status[(int)MtList.STK_X].m_pLimit) {
                            AddAlarm((byte)item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.X_Axis_NOT:
                        if (m_motor.m_status[(int)MtList.STK_X].m_nLimit) {
                            AddAlarm((byte)item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.X_Axis_Origin_Search_Fail_Home_Sensor:
                        break;

                    case AlarmList.X_Axis_Soft_Limit_Error:

                        break;
                }
            }
        }
    }
}
