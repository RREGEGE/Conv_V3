using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovenCore;
using RackMaster.SEQ.COMMON;
using RackMaster.SEQ.CLS;

namespace RackMaster.SEQ.PART {
    public enum AlarmList {
        HP_EStop = 1,
        OP_EStop,
        HP_Key_EStop,
        OP_Key_EStop,
        GOT_EStop,
        Power_Off_Error,
        MST_HP_EStop,
        MST_OP_EStop,
        MST_HP_Escape,
        MST_OP_Escape,
        HP_Door_Open,
        OP_Door_Open,
        MST_DisConnected,

        Test_Mode_Double_Storage_Error = 16,
        Test_Mode_CST_Presence_Error,
        CST_Detect_Sensor_Check_Error,
        CST_Abnormal_Detect_Sensor_Check_Error,
        Double_Storage,
        Source_Empty,
        Resume_Request,
        Store_Alt,

        //X_Servo_Battery_Low_Alarm = 33,
        //Z_Servo_Battery_Low_Alarm,
        //A_Servo_Battery_Low_Alarm,
        //T_Servo_Battery_Low_Alarm,

        X_Servo_Pack_Error = 48,
        X_Axis_POT,
        X_Axis_NOT,
        //X_Axis_Origin_Search_Fail_Home_Sensor,
        X_Axis_Soft_Limit_Error,
        //X_Axis_InPosition_Error,
        //X_Axis_Absolute_Origin_Loss,
        X_Axis_Over_Load_Error = 56,
        X_Axis_Home_Sensor_Always_On_Error,
        X_Home_Move_Time_Out_Error,
        Pick_Position_Error,
        Place_Position_Error,

        X_Axis_Barcode_Sensor_Error = 65,
        X_Axis_Free_Run,

        Z_Servo_Pack_Error = 80,
        Z_Axis_POT,
        Z_Axis_NOT,
        //Z_Axis_Origin_Search_Fail_Home_Sensor,
        Z_Axis_Soft_Limit_Error,
        //Z_Axis_InPosition_Error,
        //Z_Axis_Absolute_Origin_Loss,
        Z_Axis_Over_Load_Error = 88,
        Z_Axis_Home_Sensor_Always_On_Error,
        Z_Home_Move_Time_Out_Error,
        Z_Axis_Wire_Cut_Sensor_Error,

        A_Servo_Pack_Error = 96,
        A_Axis_POT,
        A_Axis_NOT,
        //A_Axis_Origin_Search_Fail_Home_Sensor,
        A_Axis_Soft_Limit_Error,
        //A_Axis_InPosition_Error,
        //A_Axis_Absolute_Origin_Loss,
        A_Axis_Over_Load_Error = 104,
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
        //T_Axis_Origin_Search_Fail_Home_Sensor,
        T_Axis_Soft_Limit_Error,
        //T_Axis_InPosition_Error,
        //T_Axis_Absolute_Origin_Loss,
        T_Axis_Over_Load_Error = 120,
        T_Axis_Home_Sensor_Always_On_Error,
        T_Home_Move_Time_Out_Error,

        Command_ID_Range_Error = 128,
        TO_Mode_Command_Over_Time_Error,
        PIO_Over_Time_Error,
        PIO_Ready_Off_Error,
        //Stay_Complete_ACK_Over_Time_Error,
        Complete_ACK_Over_Time_Error,
        Store_ACK_Over_Time_Error,
        Source_Empty_ACK_Over_Time_Error,
        Double_Storage_ACK_Over_Time_Error,
        Resume_Request_ACK_Over_Time_Error,
        Idle_Status_CST_Error,

        Step_Timeover_Error = 145,
        Arm_Position_Sensor_Detect_Error_1,
        Arm_Position_Sensor_Detect_Error_2,
        Arm_Position_Sensor_Detect_Error_3,
        From_To_Software_Limit_Over_Error_X,
        From_To_Software_Limit_Over_Error_Z,
        From_To_Software_Limit_Over_Error_A,
        From_To_Software_Limit_Over_Error_T,
        Turn_Position_Sensor_Detect_Error,

        Regulator_Boost_PeackVoltage = 160,
        Regulator_Boost_OverVoltage,
        Regulator_Input_LowVoltage,
        Regulator_Output_OverVoltage,
        Regulator_Output_LowVoltage,
        Regulator_Output_PeakCurrent,
        Regulator_Output_OverCurrent,
        Regulator_REG_ERR_Gate_Fault,
        Regulator_Boost_PeackCurrent,
        Regulator_Boost_OverCurrent,
        Regulator_Pickup_Input_OverCurrent,
        Regulator_Overload,
        Regulator_HeatSink_OverHeat_Contact,
        Regulator_HeatSink_OverHeat_NTC,
        Regulator_Pickup_OverHeat_Contact,
        Regulator_Pickup_OverHeat_NTC,
        Regulator_Inside_OverHeat_NTC,
        Regulator_AL_CAP_OverHeat_NTC,
        Regulator_AL_CAP_OverHeat_ThermalWire,
        Regulator_Output_Fuse_Open,
        Regulator_AL_CAP_Fuse_Open,
        Regulator_Protect_Board_Open,
        Regulator_Voltage_Detection_Error,
        Regulator_Current_Detection_Error,
        Regulator_Controller_Defect,

        MAX = 207,
    }

    public enum RegulatorAlarmList {
        Boost_PeackVoltage = 1,
        Boost_OverVoltage,
        Input_LowVoltage,
        Output_OverVoltage,
        Output_LowVoltage,
        Output_PeakCurrent,
        Output_OverCurrent,
        REG_ERR_Gate_Fault,
        Boost_PeackCurrent,
        Boost_OverCurrent,
        Pickup_Input_OverCurrent,
        Overload,
        HeatSink_OverHeat_Contact,
        HeatSink_OverHeat_NTC,
        Pickup_OverHeat_Contact,
        Pickup_OverHeat_NTC,
        AL_CAP_OverHeat_NTC,
        AL_CAP_OverHeat_ThermalWire,
        Output_Fuse_Open,
        AL_CAP_Fuse_Open,
        Inside_OverHeat_NTC,

        Protect_Board_Open = 23,
        Voltage_Detection_Error,
        Current_Detection_Error,

        Warning_HeatSink_OverTemp,
        Warning_HeatSink_OverHeat,
        Wraning_Pickup_OverHeat,
    }

    public enum AlarmState {
        Idle,
        Alarm,
    }

    public partial class RackMasterMain {
        public class RackMasterAlarm {
            private enum TimerType {
                Home_Move_Timeout_X = 0,
                Home_Move_Timeout_Z,
                Home_Move_Timeout_A,
                Home_Move_Timeout_T,
                To_Start_Timeout,
                PIO_Timeout,
                PIO_Ready_Off_Timeout,
                Stay_Complete_ACK_Timeout,
                Compelete_ACK_Timeout,
                StoreAlt_ACK_Timeout,
                SourceEmpty_ACK_Timeout,
                DoubleStorage_ACK_Timeout,
                ResumeRequest_ACK_Timeout,
                ResumeRequest,
                StoreAlt,
                Step,
                DTP_EMO,
                Regulator_Disconnected,
                Test_Mode_Presence_Error,
                CST_Detect_Sensor,

                MAX_COUNT,
            }

            private List<short> m_alarm;
            private Timer[] m_timer;
            private AutoStep m_prevStep;
            private RackMasterMain m_main;
            private RackMasterMotion m_motion;
            private RackMasterParam m_param;
            private TeachingData m_teaching;

            private int m_DTP_EMO_Timeout = 1000;
            private int m_disConnected_Timeout = 3000;

            private short m_currentOverloadX;
            private short m_currentOverloadZ;
            private short m_currentOverloadA;
            private short m_currentOverloadT;
            /// <summary>
            /// Alarm List Enum값에 해당하는 문자열 반환
            /// </summary>
            /// <param name="alarm"></param>
            /// <returns></returns>
            public string GetAlarmString(AlarmList alarm) {
                switch (alarm) {
                    case AlarmList.HP_EStop:
                        return "HP EMO";

                    case AlarmList.OP_EStop:
                        return "OP EMO";

                    case AlarmList.HP_Key_EStop:
                        return "HP Key EMO";

                    case AlarmList.OP_Key_EStop:
                        return "OP Key EMO";

                    case AlarmList.GOT_EStop:
                        return "GOT EMO";

                    case AlarmList.Power_Off_Error:
                        return "Power Off";

                    case AlarmList.MST_HP_EStop:
                        return "Master HP EMO";

                    case AlarmList.MST_OP_EStop:
                        return "Master OP EMO";

                    case AlarmList.MST_HP_Escape:
                        return "Master HP Escape";

                    case AlarmList.MST_OP_Escape:
                        return "Master OP Escape";

                    case AlarmList.HP_Door_Open:
                        return "HP Door Open";

                    case AlarmList.OP_Door_Open:
                        return "OP Door Open";

                    case AlarmList.MST_DisConnected:
                        return "Master TCP Disconnected";

                    case AlarmList.Test_Mode_Double_Storage_Error:
                        return "Test Mode Double Storage Error";

                    case AlarmList.Test_Mode_CST_Presence_Error:
                        return "Test Mode CST Presence Error";

                    case AlarmList.CST_Detect_Sensor_Check_Error:
                        return "CST Detect Sensor Check Error";

                    case AlarmList.CST_Abnormal_Detect_Sensor_Check_Error:
                        return "CST Abnormal Detect Sensor Check Error";

                    case AlarmList.Double_Storage:
                        return "Dobule Storage";

                    case AlarmList.Source_Empty:
                        return "Source Empty";

                    case AlarmList.Resume_Request:
                        return "Resume Request";

                    case AlarmList.Store_Alt:
                        return "Sotre Alt";

                    //case AlarmList.X_Servo_Battery_Low_Alarm:
                    //    return "X Servo Battery Low Alarm";

                    //case AlarmList.Z_Servo_Battery_Low_Alarm:
                    //    return "Z Servo Battery Low Alarm";

                    //case AlarmList.A_Servo_Battery_Low_Alarm:
                    //    return "A Servo Battery Low Alarm";

                    //case AlarmList.T_Servo_Battery_Low_Alarm:
                    //    return "T Servo Battery Low Alarm";

                    case AlarmList.X_Servo_Pack_Error:
                        return "X Servo Alarm";

                    case AlarmList.X_Axis_POT:
                        return "X Axis POT";

                    case AlarmList.X_Axis_NOT:
                        return "X Axis NOT";

                    //case AlarmList.X_Axis_Origin_Search_Fail_Home_Sensor:
                    //    return "X Axis Origin Search Fail";

                    case AlarmList.X_Axis_Soft_Limit_Error:
                        return "X Axis Software Limit";

                    //case AlarmList.X_Axis_InPosition_Error:
                    //    return "X Axis Inposition Error";

                    //case AlarmList.X_Axis_Absolute_Origin_Loss:
                    //    return "X Axis Absolute Origin Loss";

                    case AlarmList.X_Axis_Over_Load_Error:
                        return "X Axis Overload Error";

                    case AlarmList.X_Axis_Home_Sensor_Always_On_Error:
                        return "X Axis Home Sensor Always On Error";

                    case AlarmList.X_Home_Move_Time_Out_Error:
                        return "X Axis Home Move Time Out";

                    case AlarmList.Pick_Position_Error:
                        return "Pick Sensor Error";

                    case AlarmList.Place_Position_Error:
                        return "Place Sensor Error";

                    case AlarmList.X_Axis_Barcode_Sensor_Error:
                        return "X Axis Barcode Sensor Error";

                    case AlarmList.X_Axis_Free_Run:
                        return "X Axis Free Run";

                    case AlarmList.Z_Servo_Pack_Error:
                        return "Z Servo Alarm";

                    case AlarmList.Z_Axis_POT:
                        return "Z Axis POT";

                    case AlarmList.Z_Axis_NOT:
                        return "Z Axis NOT";

                    //case AlarmList.Z_Axis_Origin_Search_Fail_Home_Sensor:
                    //    return "Z Axis Origin Search Fail";

                    case AlarmList.Z_Axis_Soft_Limit_Error:
                        return "Z Axis Software Limit";

                    //case AlarmList.Z_Axis_InPosition_Error:
                    //    return "Z Axis Inposition Error";

                    //case AlarmList.Z_Axis_Absolute_Origin_Loss:
                    //    return "Z Axis Absolute Origin Loss";

                    case AlarmList.Z_Axis_Over_Load_Error:
                        return "Z Axis Overload Error";

                    case AlarmList.Z_Axis_Home_Sensor_Always_On_Error:
                        return "Z Axis Home Sensor Always On Error";

                    case AlarmList.Z_Home_Move_Time_Out_Error:
                        return "Z Axis Home Move Time Out";

                    case AlarmList.Z_Axis_Wire_Cut_Sensor_Error:
                        return "Z Axis Wire Cut Sensor Error";

                    case AlarmList.A_Servo_Pack_Error:
                        return "A Servo Alarm";

                    case AlarmList.A_Axis_POT:
                        return "A Axis POT";

                    case AlarmList.A_Axis_NOT:
                        return "A Axis NOT";

                    //case AlarmList.A_Axis_Origin_Search_Fail_Home_Sensor:
                    //    return "A Axis Origin Search Fail";

                    case AlarmList.A_Axis_Soft_Limit_Error:
                        return "A Axis Software Limit";

                    //case AlarmList.A_Axis_InPosition_Error:
                    //    return "A Axis Inposition Error";

                    //case AlarmList.A_Axis_Absolute_Origin_Loss:
                    //    return "A Axis Absolute Origin Loss";

                    case AlarmList.A_Axis_Over_Load_Error:
                        return "A Axis Overload Error";

                    case AlarmList.A_Axis_Home_Sensor_Always_On_Error:
                        return "A Axis Home Sensor Always On Error";

                    case AlarmList.A_Home_Move_Time_Out_Error:
                        return "A Axis Home Move Time Out";

                    case AlarmList.RM_Moving_Arm_Home_Sensor_Dont_Detect_Error:
                        return "RM Moving Arm Home Sensor Don't Detect Error";

                    case AlarmList.Arm_Table_CST_Detect_Error_To_Complete:
                        return "Arm Table CST Detect Error To Complete";

                    case AlarmList.Cassette_Fail_To_X_Move:
                        return "Cassette Fail To X Move";

                    case AlarmList.Arm_Table_Cross_Sensor_Error:
                        return "Arm Table Cross Sensor Error";

                    case AlarmList.Storage_Failure_Error_To_Complete:
                        return "Storage Failure Error To Complete";

                    case AlarmList.T_Servo_Pack_Error:
                        return "T Servo Alarm";

                    case AlarmList.T_Axis_POT:
                        return "T Axis POT";

                    case AlarmList.T_Axis_NOT:
                        return "T Axis NOT";

                    //case AlarmList.T_Axis_Origin_Search_Fail_Home_Sensor:
                    //    return "T Axis Origin Search Fail";

                    case AlarmList.T_Axis_Soft_Limit_Error:
                        return "T Axis Software Limit";

                    //case AlarmList.T_Axis_InPosition_Error:
                    //    return "T Axis Inposition Error";

                    //case AlarmList.T_Axis_Absolute_Origin_Loss:
                    //    return "T Axis Absolute Origin Loss";

                    case AlarmList.T_Axis_Over_Load_Error:
                        return "T Axis Overload Error";

                    case AlarmList.T_Axis_Home_Sensor_Always_On_Error:
                        return "T Axis Home Sensor Always On Error";

                    case AlarmList.T_Home_Move_Time_Out_Error:
                        return "T Axis Home Move Time Out";

                    case AlarmList.Command_ID_Range_Error:
                        return "Command ID Range Error";

                    case AlarmList.TO_Mode_Command_Over_Time_Error:
                        return "To Mode Command Time Over Error";

                    case AlarmList.PIO_Over_Time_Error:
                        return "PIO Over Time Error";

                    case AlarmList.PIO_Ready_Off_Error:
                        return "PIO Ready Off Error";

                    //case AlarmList.Stay_Complete_ACK_Over_Time_Error:
                    //    return "Stay Complete ACK Time Over Error";

                    case AlarmList.Complete_ACK_Over_Time_Error:
                        return "Complete ACK Time Over Error";

                    case AlarmList.Store_ACK_Over_Time_Error:
                        return "Store Alt ACK Time Over Error";

                    case AlarmList.Source_Empty_ACK_Over_Time_Error:
                        return "Source Empty ACK Time Over Error";

                    case AlarmList.Double_Storage_ACK_Over_Time_Error:
                        return "Double Storage ACK Time Over Error";

                    case AlarmList.Resume_Request_ACK_Over_Time_Error:
                        return "Resume Request ACK Time Over Error";

                    case AlarmList.Idle_Status_CST_Error:
                        return "Idle Status CST Error";

                    case AlarmList.Step_Timeover_Error:
                        return "Step Timeover Error";

                    case AlarmList.Arm_Position_Sensor_Detect_Error_1:
                        return "Arm Position Sensor Detect Error 1";

                    case AlarmList.Arm_Position_Sensor_Detect_Error_2:
                        return "Arm Position Sensor Detect Error 2";

                    case AlarmList.Arm_Position_Sensor_Detect_Error_3:
                        return "Arm Position Sensor Detect Error 3";

                    case AlarmList.From_To_Software_Limit_Over_Error_X:
                        return "From To Software Limit Over Error X";

                    case AlarmList.From_To_Software_Limit_Over_Error_Z:
                        return "From To Software Limit Over Error Z";

                    case AlarmList.From_To_Software_Limit_Over_Error_A:
                        return "From To Software Limit Over Error A";

                    case AlarmList.From_To_Software_Limit_Over_Error_T:
                        return "From To Software Limit Over Error T";

                    case AlarmList.Turn_Position_Sensor_Detect_Error:
                        return "Turn Position Sensor Detect Error";

                    case AlarmList.Regulator_AL_CAP_Fuse_Open:
                        return "Regulator AL CAP Fuse Open";

                    case AlarmList.Regulator_AL_CAP_OverHeat_NTC:
                        return "Regulator AL CAP OverHeat NTC";

                    case AlarmList.Regulator_AL_CAP_OverHeat_ThermalWire:
                        return "Regulator AL CAP OverHeat ThermalWire";

                    case AlarmList.Regulator_Boost_OverCurrent:
                        return "Regulator Boost Over Current";

                    case AlarmList.Regulator_Boost_OverVoltage:
                        return "Regulator Boost Over Voltage";

                    case AlarmList.Regulator_Boost_PeackCurrent:
                        return "Regulator Boost Peack Current";

                    case AlarmList.Regulator_Boost_PeackVoltage:
                        return "Regulator Boost Peack Voltage";

                    case AlarmList.Regulator_Controller_Defect:
                        return "Regulator Controller Defect";

                    case AlarmList.Regulator_Current_Detection_Error:
                        return "Regulator Current Detection Error";

                    case AlarmList.Regulator_HeatSink_OverHeat_Contact:
                        return "Regulator HeatSink OverHeat Contact";

                    case AlarmList.Regulator_HeatSink_OverHeat_NTC:
                        return "Regulator HeatSink OverHeat NTC";

                    case AlarmList.Regulator_Input_LowVoltage:
                        return "Regulator Input Low Voltage";

                    case AlarmList.Regulator_Inside_OverHeat_NTC:
                        return "Regulator Inside OverHeat NTC";

                    case AlarmList.Regulator_Output_Fuse_Open:
                        return "Regulator Output Fuse Open";

                    case AlarmList.Regulator_Output_LowVoltage:
                        return "Regulator Output Low Voltage";

                    case AlarmList.Regulator_Output_OverCurrent:
                        return "Regulator Output Over Current";

                    case AlarmList.Regulator_Output_OverVoltage:
                        return "Regulator Output Over Voltage";

                    case AlarmList.Regulator_Output_PeakCurrent:
                        return "Regulator Output Peak Current";

                    case AlarmList.Regulator_Overload:
                        return "Regulator Overload";

                    case AlarmList.Regulator_Pickup_Input_OverCurrent:
                        return "Regulator Pickup Input Over Current";

                    case AlarmList.Regulator_Pickup_OverHeat_Contact:
                        return "Regulator Pickup OverHeat Contact";

                    case AlarmList.Regulator_Pickup_OverHeat_NTC:
                        return "Regulator Pickup OverHeat NTC";

                    case AlarmList.Regulator_Protect_Board_Open:
                        return "Regulator Protect Board Open";

                    case AlarmList.Regulator_REG_ERR_Gate_Fault:
                        return "Regulator REG ERR Gate Fault";

                    case AlarmList.Regulator_Voltage_Detection_Error:
                        return "Regulator Voltage Dtection Error";
                }

                return "";
            }

            public RackMasterAlarm(RackMasterMain main) {
                m_alarm = new List<short>();
                m_timer = new Timer[(int)TimerType.MAX_COUNT];
                m_prevStep = AutoStep.Step0_Idle;
                m_main = main;
                m_motion = m_main.m_motion;
                m_param = m_main.m_param;
                m_teaching = m_main.m_teaching;

                m_currentOverloadX = 0;
                m_currentOverloadZ = 0;
                m_currentOverloadA = 0;
                m_currentOverloadT = 0;

                for (int i = 0; i < (int)TimerType.MAX_COUNT; i++) {
                    m_timer[i] = new Timer();
                    m_timer[i].Stop();
                }
            }
            /// <summary>
            /// 알람 발생 시 알람 리스트에 특정 알람 추가 및 알람 상태로 변환
            /// </summary>
            /// <param name="alarmCode"></param>
            /// <param name="state"></param>
            public void AddAlarm(AlarmList alarmCode, AlarmState state) {
                if (m_alarm.Contains((short)alarmCode))
                    return;

                m_alarm.Add((short)alarmCode);

                // Amp Alarm 발생 시 Alarm Code도 Log에 작성
                if (state == AlarmState.Alarm) {
                    if(alarmCode == AlarmList.X_Servo_Pack_Error) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterAlarmOccurred, $"{GetAlarmString(alarmCode)}, Code=0x{(int)alarmCode:X2}, Amp Alarm=0x{m_motion.GetAxisAlarmCode(AxisList.X_Axis):X2}"));
                    }
                    else if(alarmCode == AlarmList.Z_Servo_Pack_Error) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterAlarmOccurred, $"{GetAlarmString(alarmCode)}, Code=0x{(int)alarmCode:X2}, Amp Alarm=0x{m_motion.GetAxisAlarmCode(AxisList.Z_Axis):X2}"));
                    }
                    else if(alarmCode == AlarmList.A_Servo_Pack_Error) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterAlarmOccurred, $"{GetAlarmString(alarmCode)}, Code=0x{(int)alarmCode:X2}, Amp Alarm=0x{m_motion.GetAxisAlarmCode(AxisList.A_Axis):X2}"));
                    }
                    else if(alarmCode == AlarmList.T_Servo_Pack_Error) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterAlarmOccurred, $"{GetAlarmString(alarmCode)}, Code=0x{(int)alarmCode:X2}, Amp Alarm=0x{m_motion.GetAxisAlarmCode(AxisList.T_Axis):X2}"));
                    }
                    else {
                        Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterAlarmOccurred, $"{GetAlarmString(alarmCode)}, Code=0x{(int)alarmCode:X2}"));
                    }
                }
                else {
                    string logMsg = $"[{state}]{GetAlarmString(alarmCode)}";
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, logMsg));
                }

                return;
            }
            /// <summary>
            /// 알람 클리어
            /// </summary>
            public void ClearAlarm() {
                if (m_alarm.Count > 0) {
                    m_alarm.Clear();
                    foreach(AxisList axis in Enum.GetValues(typeof(AxisList))) {
                        m_motion.AlarmClear(axis);
                    }
                    m_main.FullClosedAbnormalClear();

                    m_main.SetSendBit(SendBitMap.Accessing_RM_Down_From, false);
                    m_main.SetSendBit(SendBitMap.Accessing_RM_Down_To, false);

                    m_currentOverloadX = 0;
                    m_currentOverloadZ = 0;
                    m_currentOverloadA = 0;
                    m_currentOverloadT = 0;
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterAlarmClear));
                    return;
                }
            }
            /// <summary>
            /// Master Disconnection 알람의 경우 Master와 통신이 성공할 때 해당 알람만 클리어
            /// </summary>
            public void ClearAlarm_MSTDisconnection() {
                if(m_alarm.Count > 0) {
                    if (m_alarm.Contains((short)AlarmList.MST_DisConnected)) {
                        m_alarm.Remove((short)AlarmList.MST_DisConnected);
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterMSTDisconnectionAlarmClear));
                    }
                }

                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.CIM, $"CIM Connected"));
            }
            /// <summary>
            /// 현재 알람상태인지 판단
            /// </summary>
            /// <returns></returns>
            public bool IsAlarmState() {
                //return m_currentState == AlarmState.Alarm ? true : false;
                return m_alarm.Count > 0;
            }
            /// <summary>
            /// 현재 알람 발생 리스트 중 특정 알람이 포함되어있는지 판단
            /// </summary>
            /// <param name="alarm"></param>
            /// <returns></returns>
            public bool IsCurrentAlarmContainAt(AlarmList alarm) {
                for(int i = 0; i < m_alarm.Count; i++) {
                    if (m_alarm[i] == (short)alarm)
                        return true;
                }

                return false;
            }
            /// <summary>
            /// 현재 몇 개의 알람이 발생하였는지 반환
            /// </summary>
            /// <returns></returns>
            public int GetCurrentAlarmCount() {
                return m_alarm.Count;
            }
            /// <summary>
            /// 현재 발생한 알람 리스트 중 특정 Index 알람 반환
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public short GetAlarmCode(int index) {
                try {
                    return m_alarm[index];
                }catch(Exception ex) {
                    return 0;
                }
            }
            /// <summary>
            /// Alarm Check시 필요한 특정 타이머 Stop
            /// 주로 I/O 체크, PIO나 ACK가 안들어 올 때의 Timeout에 사용
            /// </summary>
            /// <param name="timer"></param>
            private void TimerStop(TimerType timer) {
                m_timer[(int)timer].Reset();
                m_timer[(int)timer].Stop();
            }
            /// <summary>
            /// Alarm Check시 필요한 특정 타이머 Start
            /// </summary>
            /// <param name="timer"></param>
            private void TimerStart(TimerType timer) {
                if (!m_timer[(int)timer].IsTimerStarted())
                    m_timer[(int)timer].Start();
            }
            /// <summary>
            /// Alarm Check시 필요한 특정 타이머 Delay
            /// 예를 들어, 10초동안 ACK가 없을 때 알람이 발생하여야 하는데, 해당 함수에 10초 값을 입력 후 해당 시간이 경과하면 True를 반환
            /// </summary>
            /// <param name="timer"></param>
            /// <param name="delayMilliseconds"></param>
            /// <returns></returns>
            private bool TimerDelay(TimerType timer, long delayMilliseconds) {
                if (m_timer[(int)timer].IsTimerStarted()) {
                    return m_timer[(int)timer].Delay(delayMilliseconds);
                }
                else {
                    m_timer[(int)timer].Start();
                }

                return false;
            }
            /// <summary>
            /// 특정 타이머가 시작되었는지 판단
            /// </summary>
            /// <param name="timer"></param>
            /// <returns></returns>
            private bool IsTimerStarted(TimerType timer) {
                return m_timer[(int)timer].IsTimerStarted();
            }

            // Error_Fuction은 true면 Error발생 false면 미알람

            //private bool Error_OriginSearchFail(AxisList axis) {
            //    if (m_motion.GetAxisFlag(AxisFlagType.Homing, axis)) {
            //        return (m_motion.GetAxisSensor(SensorType.Negative_Limit, axis) || m_motion.GetAxisSensor(SensorType.Positive_Limit, axis));
            //    }

            //    return false;
            //}
            /// <summary>
            /// 특정 축에 S/W Limit이 감지되었는지 판단
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            private bool Error_SoftwareLimit(AxisList axis) {
                return (m_motion.GetAxisSensor(AxisSensorType.SW_Negative_Limit, axis) || m_motion.GetAxisSensor(AxisSensorType.SW_Positive_Limit, axis));
            }
            /// <summary>
            /// 특정 축의 Inposition이 완료되었는지 판단
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            private bool Error_Inposition(AxisList axis) {
                if (!m_motion.GetAxisFlag(AxisFlagType.Run, axis)) {
                    if (!m_motion.GetAxisFlag(AxisFlagType.Poset, axis)) {
                        return true;
                    }
                }

                return false;
            }
            /// <summary>
            /// 특정 축의 토크가 설정된 Overload 값 이상을 넘어갈 시 알람
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            private bool Error_Overload(AxisList axis) {
                float overload = m_param.GetAxisParameter(axis).maxOverload;
                float current_overload = (float)Math.Abs(m_motion.GetAxisStatus(AxisStatusType.trq_act, axis));

                if (current_overload > overload) {
                    return true;
                }

                return false;
            }
            /// <summary>
            /// 설정된 Home Position Range 범위를 벗어났음에도 Home Sensor가 항상 들어올 때 알람
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            private bool Error_HomeSensorAlwaysOn(AxisList axis) {
                if (m_motion.GetAxisFlag(AxisFlagType.Homing, axis))
                    return false;

                float originRange = m_param.GetAxisParameter(axis).homePositionRange;

                if (m_motion.GetAxisStatus(AxisStatusType.pos_act, axis) > originRange) {
                    if (m_motion.GetAxisSensor(AxisSensorType.Home, axis))
                        return true;
                }

                return false;
            }
            /// <summary>
            /// 호밍 진행 시간이 설정된 시간을 벗어난 경우 발생
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            private bool Error_HomeMoveTimeOut(AxisList axis) {
                TimerType t_type = TimerType.Home_Move_Timeout_A;

                if (axis == AxisList.X_Axis) {
                    t_type = TimerType.Home_Move_Timeout_X;
                }
                else if (axis == AxisList.Z_Axis) {
                    t_type = TimerType.Home_Move_Timeout_Z;
                }
                else if (axis == AxisList.A_Axis) {
                    t_type = TimerType.Home_Move_Timeout_A;
                }
                else if (axis == AxisList.T_Axis) {
                    t_type = TimerType.Home_Move_Timeout_T;
                }

                if (m_motion.GetAxisFlag(AxisFlagType.Homing, axis)) {
                    if (!IsTimerStarted(t_type)) {
                        TimerStart(t_type);
                    }

                    if (TimerDelay(t_type, m_param.GetTimerParam().HOME_MOVE_TIMEOVER)) {
                        return true;
                    }
                }
                else {
                    TimerStop(t_type);
                }

                return false;
            }
            /// <summary>
            /// 설정된 알람 리스트들 전부 알람조건 체크
            /// </summary>
            public void AlarmCheck() {
                TeachingValueData target = null;

                if (m_prevStep != m_motion.GetCurrentAutoStep())
                    m_prevStep = m_motion.GetCurrentAutoStep();

                if (m_motion.GetCurrentAutoStep() >= AutoStep.Step101_From_CST_And_Fork_Home_Check && m_motion.GetCurrentAutoStep() <= AutoStep.Step145_From_Complete) {
                    target = m_teaching.GetTeachingData(m_motion.GetCurrentTargetID(MotionMode.From));
                }
                else if (m_motion.GetCurrentAutoStep() >= AutoStep.Step201_To_CST_And_Fork_Home_Check && m_motion.GetCurrentAutoStep() <= AutoStep.Step220_To_Complete) {
                    target = m_teaching.GetTeachingData(m_motion.GetCurrentTargetID(MotionMode.To));
                }

                try {
                    foreach (AlarmList item in Enum.GetValues(typeof(AlarmList))) {
                        switch (item) {
                            case AlarmList.HP_EStop:
                                if (m_main.GetInputBit(InputList.EMO_HP)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.OP_EStop:
                                if (m_main.GetInputBit(InputList.EMO_OP)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.HP_Key_EStop:
                                if(m_main.IsAutoState() && m_main.m_motion.IsAutoMotionRun()) {
                                    if (!m_main.GetReceiveBit(ReceiveBitMap.RM_Key_Auto)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }

                                break;

                            case AlarmList.OP_Key_EStop:
                                //
                                break;

                            case AlarmList.GOT_EStop:
                                if(m_main.GetInputBit(InputList.HP_DTP_Mode_Select_SW_1) || m_main.GetInputBit(InputList.HP_DTP_Mode_Select_SW_2) ||
                                    m_main.GetInputBit(InputList.OP_DTP_Mode_Select_SW_1) || m_main.GetInputBit(InputList.OP_DTP_Mode_Select_SW_2)) {
                                    if (m_main.GetInputBit(InputList.HP_DTP_Mode_Select_SW_1) || m_main.GetInputBit(InputList.HP_DTP_Mode_Select_SW_2)) {
                                        if (TimerDelay(TimerType.DTP_EMO, m_DTP_EMO_Timeout)) {
                                            if (m_main.GetInputBit(InputList.HP_DTP_EMS_SW)) {
                                                AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                    }

                                    if (m_main.GetInputBit(InputList.OP_DTP_Mode_Select_SW_1) || m_main.GetInputBit(InputList.OP_DTP_Mode_Select_SW_2)) {
                                        if (TimerDelay(TimerType.DTP_EMO, m_DTP_EMO_Timeout)) {
                                            if (m_main.GetInputBit(InputList.OP_DTP_EMS_SW)) {
                                                AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                    }
                                }
                                else {
                                    TimerStop(TimerType.DTP_EMO);
                                }
                                break;

                            case AlarmList.Power_Off_Error:
                                if (m_motion.GetCurrentAutoStep() != AutoStep.Step0_Idle && m_motion.GetCurrentAutoStep() != AutoStep.Step500_Error) {
                                    if (!m_motion.IsAllServoOn()) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                break;

                            case AlarmList.MST_HP_EStop:
                                if (m_main.GetReceiveBit(ReceiveBitMap.RM_HP_EMO)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.MST_OP_EStop:
                                if (m_main.GetReceiveBit(ReceiveBitMap.RM_OP_EMO)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.MST_HP_Escape:
                                if (m_main.GetReceiveBit(ReceiveBitMap.RM_HP_Escape)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.MST_OP_Escape:
                                if (m_main.GetReceiveBit(ReceiveBitMap.RM_OP_Escape)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.HP_Door_Open:
                                if (m_main.GetReceiveBit(ReceiveBitMap.RM_HP_Door_Open)) {
                                    if (m_main.IsAutoState()) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                break;

                            case AlarmList.OP_Door_Open:
                                if (m_main.GetReceiveBit(ReceiveBitMap.RM_OP_Door_Open)) {
                                    if (m_main.IsAutoState()) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                break;

                            case AlarmList.MST_DisConnected:
                                if (!m_main.IsConnected_Master()) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Test_Mode_Double_Storage_Error:
                                if (!m_main.IsAutoState()) {
                                    if (m_motion.GetCurrentAutoStep() == AutoStep.Step206_To_Double_Storage_Check ||
                                        m_motion.GetCurrentAutoStep() == AutoStep.Step209_To_Place_Sensor_Check ||
                                        m_motion.GetCurrentAutoStep() == AutoStep.Step820_Double_Storage) {
                                        if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                                            if (target.direction == (int)PortDirection_HP.Left) {
                                                if (m_main.IsDoubleStorage1_Off()) {
                                                    AddAlarm(item, AlarmState.Alarm);
                                                }
                                            }
                                            else if (target.direction == (int)PortDirection_HP.Right) {
                                                if (m_main.IsDoubleStorage2_Off()) {
                                                    AddAlarm(item, AlarmState.Alarm);
                                                }
                                            }
                                            else {
                                                AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                        else {
                                            if (m_main.IsDoubleStorage1_Off()) {
                                                AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                    }
                                }
                                break;

                            case AlarmList.Test_Mode_CST_Presence_Error:
                                if (!m_main.IsAutoState()) {
                                    if (!m_main.IsPresenseAllDisabled()) {
                                        if (m_param.GetMotionParam().presenseConditionType == SensorConditionType.Always) {
                                            if (m_motion.GetCurrentAutoStep() >= AutoStep.Step112_From_XZT_From_Move && m_motion.GetCurrentAutoStep() <= AutoStep.Step113_From_XZT_From_Complete) {
                                                if (!m_main.IsPresense_Off()) {
                                                    AddAlarm(item, AlarmState.Alarm);
                                                }
                                            }
                                        }
                                        else if (m_param.GetMotionParam().presenseConditionType == SensorConditionType.BeforeStepIn) {
                                            if (m_motion.GetCurrentAutoStep() == AutoStep.Step112_From_XZT_From_Move || m_motion.GetCurrentAutoStep() == AutoStep.Step114_From_Shelf_Port_Check) {
                                                if (!m_main.IsPresense_Off()) {
                                                    AddAlarm(item, AlarmState.Alarm);
                                                }
                                            }
                                        }
                                    }
                                }

                                break;

                            case AlarmList.CST_Detect_Sensor_Check_Error:
                                if (!m_main.IsPresenseAllDisabled()) {
                                    if (m_param.GetMotionParam().presenseConditionType == SensorConditionType.Always) {
                                        if (m_motion.GetCurrentAutoStep() >= AutoStep.Step100_From_ID_Check &&
                                            m_motion.GetCurrentAutoStep() <= AutoStep.Step113_From_XZT_From_Complete) {
                                            if (!m_main.IsPresense_Off()) {
                                                if(TimerDelay(TimerType.CST_Detect_Sensor, m_param.GetTimerParam().IO_TIMER)) {
                                                    AddAlarm(item, AlarmState.Alarm);
                                                }
                                            }
                                            else {
                                                TimerStop(TimerType.CST_Detect_Sensor);
                                            }
                                        }
                                        else if (m_motion.GetCurrentAutoStep() >= AutoStep.Step204_To_XZT_To_Move &&
                                           m_motion.GetCurrentAutoStep() <= AutoStep.Step205_To_XZT_To_Complete) {
                                            if (!m_main.IsPresense_On()) {
                                                if (TimerDelay(TimerType.CST_Detect_Sensor, m_param.GetTimerParam().IO_TIMER)) {
                                                    AddAlarm(item, AlarmState.Alarm);
                                                }
                                            }
                                            else {
                                                TimerStop(TimerType.CST_Detect_Sensor);
                                            }
                                        }
                                    }
                                    else if (m_param.GetMotionParam().presenseConditionType == SensorConditionType.BeforeStepIn) {
                                        if (m_motion.GetCurrentAutoStep() == AutoStep.Step112_From_XZT_From_Move || m_motion.GetCurrentAutoStep() == AutoStep.Step114_From_Shelf_Port_Check) {
                                            if (!m_main.IsPresense_Off())
                                                AddAlarm(item, AlarmState.Alarm);
                                        }
                                        else if (m_motion.GetCurrentAutoStep() == AutoStep.Step204_To_XZT_To_Move) {
                                            if (!m_main.IsPresense_On())
                                                AddAlarm(item, AlarmState.Alarm);
                                        }
                                    }
                                    else {
                                        TimerStop(TimerType.CST_Detect_Sensor);
                                    }
                                }

                                if (!m_main.IsInplaceAllDisabled()) {
                                    if (m_param.GetMotionParam().inPlaceType == InPlaceSensorType.Normal) {
                                        if (m_param.GetMotionParam().inPlaceConditionType == SensorConditionType.Always) {
                                            if (m_motion.GetCurrentAutoStep() >= AutoStep.Step100_From_ID_Check &&
                                                m_motion.GetCurrentAutoStep() <= AutoStep.Step113_From_XZT_From_Complete) {
                                                if (!m_main.IsInPlace_Off())
                                                    AddAlarm(item, AlarmState.Alarm);
                                            }
                                            else if (m_motion.GetCurrentAutoStep() >= AutoStep.Step204_To_XZT_To_Move &&
                                               m_motion.GetCurrentAutoStep() <= AutoStep.Step205_To_XZT_To_Complete) {
                                                if (!m_main.IsInPlace_On())
                                                    AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                        else if (m_param.GetMotionParam().inPlaceConditionType == SensorConditionType.BeforeStepIn) {
                                            if (m_motion.GetCurrentAutoStep() >= AutoStep.Step100_From_ID_Check &&
                                                m_motion.GetCurrentAutoStep() <= AutoStep.Step112_From_XZT_From_Move) {
                                                if (!m_main.IsInPlace_Off())
                                                    AddAlarm(item, AlarmState.Alarm);
                                            }
                                            else if (m_motion.GetCurrentAutoStep() == AutoStep.Step204_To_XZT_To_Move) {
                                                if (!m_main.IsInPlace_On())
                                                    AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                    }
                                }

                                break;

                            case AlarmList.CST_Abnormal_Detect_Sensor_Check_Error:
                                if (!m_main.IsInplaceAllDisabled()) {
                                    if (m_param.GetMotionParam().inPlaceType == InPlaceSensorType.Normal) {
                                        if (m_param.GetMotionParam().inPlaceConditionType == SensorConditionType.Always) {
                                            if (m_motion.GetCurrentAutoStep() >= AutoStep.Step142_From_Fork_BWD_Move && m_motion.GetCurrentAutoStep() <= AutoStep.Step145_From_Complete) {
                                                if (!m_main.IsInPlace_On()) {
                                                    AddAlarm(item, AlarmState.Alarm);
                                                }
                                            }
                                            else if (m_motion.GetCurrentAutoStep() >= AutoStep.Step210_To_Fork_FWD_Move && m_motion.GetCurrentAutoStep() <= AutoStep.Step211_To_Fork_FWD_Check) {
                                                if (!m_main.IsInPlace_On())
                                                    AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                        else if (m_param.GetMotionParam().inPlaceConditionType == SensorConditionType.BeforeStepIn) {
                                            if (m_motion.GetCurrentAutoStep() == AutoStep.Step142_From_Fork_BWD_Move || m_motion.GetCurrentAutoStep() == AutoStep.Step210_To_Fork_FWD_Move) {
                                                if (!m_main.IsInPlace_On())
                                                    AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                    }
                                }
                                break;

                            case AlarmList.Double_Storage:
                                // Auto Step 에서 처리
                                break;

                            case AlarmList.Source_Empty:
                                // Auto Step 에서 처리
                                break;

                            case AlarmList.Resume_Request:
                                if (m_motion.GetCurrentAutoStep() == AutoStep.Step114_From_Shelf_Port_Check) {
                                    if (TimerDelay(TimerType.ResumeRequest, m_main.m_param.GetTimerParam().EVENT_TIMEROVER)) {
                                        m_motion.SetAutoMotionStep(AutoStep.Step801_Resume_Request);
                                    }
                                }
                                else {
                                    TimerStop(TimerType.ResumeRequest);
                                }

                                break;

                            case AlarmList.Store_Alt:
                                if (m_motion.GetCurrentAutoStep() == AutoStep.Step207_To_Shelf_Port_Check) {
                                    if (TimerDelay(TimerType.StoreAlt, m_main.m_param.GetTimerParam().EVENT_TIMEROVER)) {
                                        m_motion.SetAutoMotionStep(AutoStep.Step800_Store_Alt);
                                    }
                                }
                                else {
                                    TimerStop(TimerType.StoreAlt);
                                }

                                break;

                            //case AlarmList.X_Servo_Battery_Low_Alarm:
                            //    break;

                            //case AlarmList.Z_Servo_Battery_Low_Alarm:
                            //    break;

                            //case AlarmList.A_Servo_Battery_Low_Alarm:
                            //    break;

                            //case AlarmList.T_Servo_Battery_Low_Alarm:
                            //    break;

                            case AlarmList.X_Servo_Pack_Error:
                                if (m_motion.GetAxisFlag(AxisFlagType.Alarm, AxisList.X_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.X_Axis_POT:
                                if (m_motion.GetAxisSensor(AxisSensorType.Positive_Limit, AxisList.X_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.X_Axis_NOT:
                                if (m_motion.GetAxisSensor(AxisSensorType.Negative_Limit, AxisList.X_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            //case AlarmList.X_Axis_Origin_Search_Fail_Home_Sensor:
                            //    if (Error_OriginSearchFail(AxisList.X_Axis)) {
                            //        AddAlarm(item, AlarmState.Alarm);
                            //    }

                            //    break;

                            case AlarmList.X_Axis_Soft_Limit_Error:
                                if (Error_SoftwareLimit(AxisList.X_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            //case AlarmList.X_Axis_InPosition_Error:
                            //    // 조건보고 구성
                            //    break;

                            //case AlarmList.X_Axis_Absolute_Origin_Loss:
                            //    // 확인하고 구성
                            //    break;

                            case AlarmList.X_Axis_Over_Load_Error:
                                if (Error_Overload(AxisList.X_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }

                                break;

                            case AlarmList.X_Axis_Home_Sensor_Always_On_Error:
                                //if (Error_HomeSensorAlwaysOn(AxisList.X_Axis)) {
                                //    AddAlarm(item, AlarmState.Alarm);
                                //}
                                break;

                            case AlarmList.X_Home_Move_Time_Out_Error:
                                if (Error_HomeMoveTimeOut(AxisList.X_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }

                                break;

                            case AlarmList.Pick_Position_Error:
                                if (m_motion.GetCurrentAutoStep() == AutoStep.Step123_From_Pick_Sensor_Cehck) {
                                    if (!m_main.IsInputEnabled(InputList.Fork_Pick_Sensor_Left) && !m_main.GetInputBit(InputList.Fork_Pick_Sensor_Right)) {
                                        //AddAlarm(item, AlarmState.Alarm);
                                    }
                                    else {
                                        if (target.direction == (int)PortDirection_HP.Left) {
                                            if (!m_main.GetInputBit(InputList.Fork_Pick_Sensor_Left) && m_main.IsInputEnabled(InputList.Fork_Pick_Sensor_Left))
                                                AddAlarm(item, AlarmState.Alarm);
                                        }
                                        else if (target.direction == (int)PortDirection_HP.Right) {
                                            if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                                                if (!m_main.GetInputBit(InputList.Fork_Pick_Sensor_Left) && m_main.IsInputEnabled(InputList.Fork_Pick_Sensor_Left))
                                                    AddAlarm(item, AlarmState.Alarm);
                                            }
                                            else {
                                                if (!m_main.GetInputBit(InputList.Fork_Pick_Sensor_Right) && m_main.IsInputEnabled(InputList.Fork_Pick_Sensor_Right))
                                                    AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                    }
                                }

                                break;

                            case AlarmList.Place_Position_Error:
                                if (m_motion.GetCurrentAutoStep() == AutoStep.Step209_To_Place_Sensor_Check) {
                                    if (!m_main.IsInputEnabled(InputList.Fork_Place_Sensor_Left) && !m_main.GetInputBit(InputList.Fork_Place_Sensor_Right)) {
                                        //AddAlarm(item, AlarmState.Alarm);
                                    }
                                    else {
                                        if (target.direction == (int)PortDirection_HP.Left) {
                                            if (!m_main.GetInputBit(InputList.Fork_Place_Sensor_Left) && m_main.IsInputEnabled(InputList.Fork_Place_Sensor_Left))
                                                AddAlarm(item, AlarmState.Alarm);
                                        }
                                        else if (target.direction == (int)PortDirection_HP.Right) {
                                            if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                                                if (!m_main.GetInputBit(InputList.Fork_Place_Sensor_Left) && m_main.IsInputEnabled(InputList.Fork_Place_Sensor_Left))
                                                    AddAlarm(item, AlarmState.Alarm);
                                            }
                                            else {
                                                if (!m_main.GetInputBit(InputList.Fork_Place_Sensor_Right) && m_main.IsInputEnabled(InputList.Fork_Place_Sensor_Right))
                                                    AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                    }
                                }

                                break;

                            case AlarmList.X_Axis_Barcode_Sensor_Error:
                                if (m_main.Interlock_FullClosedAbnormal()) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.X_Axis_Free_Run:
                                
                                break;

                            case AlarmList.Z_Servo_Pack_Error:
                                if (m_motion.GetAxisFlag(AxisFlagType.Alarm, AxisList.Z_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Z_Axis_POT:
                                if (m_motion.GetAxisSensor(AxisSensorType.Positive_Limit, AxisList.Z_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Z_Axis_NOT:
                                if (m_motion.GetAxisSensor(AxisSensorType.Negative_Limit, AxisList.Z_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            //case AlarmList.Z_Axis_Origin_Search_Fail_Home_Sensor:
                            //    if (Error_OriginSearchFail(AxisList.Z_Axis)) {
                            //        AddAlarm(item, AlarmState.Alarm);
                            //    }

                            //    break;

                            case AlarmList.Z_Axis_Soft_Limit_Error:
                                if (Error_SoftwareLimit(AxisList.Z_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            //case AlarmList.Z_Axis_InPosition_Error:
                            //    break;

                            //case AlarmList.Z_Axis_Absolute_Origin_Loss:
                            //    break;

                            case AlarmList.Z_Axis_Over_Load_Error:
                                if (Error_Overload(AxisList.Z_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }

                                break;

                            case AlarmList.Z_Axis_Home_Sensor_Always_On_Error:
                                //if (Error_HomeSensorAlwaysOn(AxisList.Z_Axis)) {
                                //    AddAlarm(item, AlarmState.Alarm);
                                //}

                                break;

                            case AlarmList.Z_Home_Move_Time_Out_Error:
                                if (Error_HomeMoveTimeOut(AxisList.Z_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }

                                break;

                            case AlarmList.Z_Axis_Wire_Cut_Sensor_Error:
                                if (m_main.IsInputEnabled(InputList.Z_Axis_Wire_Cut_Sensor) && m_main.GetInputBit(InputList.Z_Axis_Wire_Cut_Sensor)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.A_Servo_Pack_Error:
                                if (m_motion.GetAxisFlag(AxisFlagType.Alarm, AxisList.A_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.A_Axis_POT:
                                if (m_motion.GetAxisSensor(AxisSensorType.Positive_Limit, AxisList.A_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.A_Axis_NOT:
                                if (m_motion.GetAxisSensor(AxisSensorType.Negative_Limit, AxisList.A_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            //case AlarmList.A_Axis_Origin_Search_Fail_Home_Sensor:
                            //    if (Error_OriginSearchFail(AxisList.A_Axis)) {
                            //        AddAlarm(item, AlarmState.Alarm);
                            //    }

                            //    break;

                            case AlarmList.A_Axis_Soft_Limit_Error:
                                if (Error_SoftwareLimit(AxisList.A_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            //case AlarmList.A_Axis_InPosition_Error:
                            //    break;

                            //case AlarmList.A_Axis_Absolute_Origin_Loss:
                            //    break;

                            case AlarmList.A_Axis_Over_Load_Error:
                                if (Error_Overload(AxisList.A_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }

                                break;

                            case AlarmList.A_Axis_Home_Sensor_Always_On_Error:
                                // 음성공장에서 Home Sensor가 이상함
                                //if (Error_HomeSensorAlwaysOn(RMParameters.Servo.AXIS_A)) {
                                //    AddAlarm(item, AlarmState.Error);
                                //}

                                break;

                            case AlarmList.A_Home_Move_Time_Out_Error:
                                if (Error_HomeMoveTimeOut(AxisList.A_Axis)) {
                                    AddAlarm(item, AlarmState.Alarm);
                                }

                                break;

                            case AlarmList.RM_Moving_Arm_Home_Sensor_Dont_Detect_Error:
                                if (m_motion.GetCurrentAutoStep() == AutoStep.Step112_From_XZT_From_Move ||
                                    m_motion.GetCurrentAutoStep() == AutoStep.Step113_From_XZT_From_Complete ||
                                    m_motion.GetCurrentAutoStep() == AutoStep.Step204_To_XZT_To_Move ||
                                    m_motion.GetCurrentAutoStep() == AutoStep.Step205_To_XZT_To_Complete) {
                                    if (!m_motion.GetAxisSensor(AxisSensorType.Home, AxisList.A_Axis)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                break;

                            case AlarmList.Arm_Table_CST_Detect_Error_To_Complete:
                                if (m_motion.GetCurrentAutoStep() == AutoStep.Step220_To_Complete) {
                                    if (!m_main.IsPresenseAllDisabled()) {
                                        if (!m_main.IsPresense_Off()) {
                                            AddAlarm(item, AlarmState.Alarm);
                                        }
                                    }

                                    if (!m_main.IsInplaceAllDisabled()) {
                                        if (m_param.GetMotionParam().inPlaceType == InPlaceSensorType.Normal) {
                                            if (!m_main.IsInPlace_Off()) {
                                                AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                    }
                                }

                                break;

                            case AlarmList.Cassette_Fail_To_X_Move:
                                if (!m_main.IsPresenseAllDisabled()) {
                                    if (m_param.GetMotionParam().presenseConditionType == SensorConditionType.Always) {
                                        if (m_motion.GetCurrentAutoStep() == AutoStep.Step204_To_XZT_To_Move ||
                                            m_motion.GetCurrentAutoStep() == AutoStep.Step205_To_XZT_To_Complete ||
                                            m_motion.GetCurrentAutoStep() == AutoStep.Step209_To_Place_Sensor_Check ||
                                            m_motion.GetCurrentAutoStep() == AutoStep.Step145_From_Complete) {
                                            if (!m_main.IsPresense_On()) {
                                                AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                    }
                                    else if (m_param.GetMotionParam().presenseConditionType == SensorConditionType.BeforeStepIn) {
                                        if (m_motion.GetCurrentAutoStep() == AutoStep.Step204_To_XZT_To_Move ||
                                            m_motion.GetCurrentAutoStep() == AutoStep.Step209_To_Place_Sensor_Check ||
                                            m_motion.GetCurrentAutoStep() == AutoStep.Step145_From_Complete) {
                                            if (!m_main.IsPresense_On())
                                                AddAlarm(item, AlarmState.Alarm);
                                        }
                                    }
                                }

                                if (!m_main.IsInplaceAllDisabled()) {
                                    if (m_param.GetMotionParam().inPlaceType == InPlaceSensorType.Normal) {
                                        if (m_param.GetMotionParam().inPlaceConditionType == SensorConditionType.Always) {
                                            if (m_motion.GetCurrentAutoStep() == AutoStep.Step204_To_XZT_To_Move ||
                                            m_motion.GetCurrentAutoStep() == AutoStep.Step205_To_XZT_To_Complete ||
                                            m_motion.GetCurrentAutoStep() == AutoStep.Step209_To_Place_Sensor_Check ||
                                            m_motion.GetCurrentAutoStep() == AutoStep.Step145_From_Complete) {
                                                if (!m_main.IsInPlace_On()) {
                                                    AddAlarm(item, AlarmState.Alarm);
                                                }
                                            }
                                        }
                                        else if (m_param.GetMotionParam().inPlaceConditionType == SensorConditionType.BeforeStepIn) {
                                            if (m_motion.GetCurrentAutoStep() == AutoStep.Step204_To_XZT_To_Move ||
                                            m_motion.GetCurrentAutoStep() == AutoStep.Step209_To_Place_Sensor_Check ||
                                            m_motion.GetCurrentAutoStep() == AutoStep.Step145_From_Complete) {
                                                if (!m_main.IsInPlace_On())
                                                    AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                    }
                                }

                                if (m_motion.GetCurrentAutoStep() == AutoStep.Step145_From_Complete ||
                                    m_motion.GetCurrentAutoStep() == AutoStep.Step112_From_XZT_From_Move ||
                                    m_motion.GetCurrentAutoStep() == AutoStep.Step204_To_XZT_To_Move ||
                                        m_motion.GetCurrentAutoStep() == AutoStep.Step209_To_Place_Sensor_Check) {
                                    if (m_main.IsStickDetecSensorOff()) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }

                                break;

                            case AlarmList.Arm_Table_Cross_Sensor_Error:
                                if (!m_main.IsPresenseAllDisabled()) {
                                    if (m_motion.GetCurrentAutoStep() == AutoStep.Step145_From_Complete) {
                                        if (!m_main.IsPresense_On())
                                            AddAlarm(item, AlarmState.Alarm);
                                    }
                                    else if (m_motion.GetCurrentAutoStep() == AutoStep.Step220_To_Complete) {
                                        if (!m_main.IsPresense_Off())
                                            AddAlarm(item, AlarmState.Alarm);
                                    }
                                }

                                break;

                            case AlarmList.Storage_Failure_Error_To_Complete:
                                if (!m_main.IsPresenseAllDisabled()) {
                                    if (m_motion.GetCurrentAutoStep() == AutoStep.Step220_To_Complete) {
                                        if (!m_main.IsPresense_Off())
                                            AddAlarm(item, AlarmState.Alarm);
                                    }
                                }

                                break;

                            case AlarmList.T_Servo_Pack_Error:
                                if (m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                                    if (m_motion.GetAxisFlag(AxisFlagType.Alarm, AxisList.T_Axis)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                break;

                            case AlarmList.T_Axis_POT:
                                if (m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                                    if (m_motion.GetAxisSensor(AxisSensorType.Positive_Limit, AxisList.T_Axis)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                break;

                            case AlarmList.T_Axis_NOT:
                                if (m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                                    if (m_motion.GetAxisSensor(AxisSensorType.Negative_Limit, AxisList.T_Axis)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                break;

                            //case AlarmList.T_Axis_Origin_Search_Fail_Home_Sensor:
                            //    if (m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                            //        if (Error_OriginSearchFail(AxisList.T_Axis)) {
                            //            AddAlarm(item, AlarmState.Alarm);
                            //        }
                            //    }
                            //    break;

                            case AlarmList.T_Axis_Soft_Limit_Error:
                                if (m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                                    if (Error_SoftwareLimit(AxisList.T_Axis)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                break;

                            //case AlarmList.T_Axis_InPosition_Error:
                            //    break;

                            //case AlarmList.T_Axis_Absolute_Origin_Loss:
                            //    break;

                            case AlarmList.T_Axis_Over_Load_Error:
                                if (m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                                    if (Error_Overload(AxisList.T_Axis)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                break;

                            case AlarmList.T_Axis_Home_Sensor_Always_On_Error:
                                //if (Error_HomeSensorAlwaysOn(RMParam.RMAxis.T_Axis)) {
                                //    AddAlarm(item, AlarmState.Error);
                                //}
                                break;

                            case AlarmList.T_Home_Move_Time_Out_Error:
                                if (m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                                    if (Error_HomeMoveTimeOut(AxisList.T_Axis)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                break;

                            case AlarmList.Command_ID_Range_Error:
                                if (m_main.IsAutoState()) {
                                    if (m_motion.GetCurrentAutoStep() == AutoStep.Step100_From_ID_Check ||
                                    m_motion.GetCurrentAutoStep() == AutoStep.Step200_To_ID_Check) {
                                        int id = 0;
                                        if (m_motion.GetCurrentAutoStep() == AutoStep.Step100_From_ID_Check)
                                            id = m_motion.GetCurrentTargetID(MotionMode.From);
                                        else if (m_motion.GetCurrentAutoStep() == AutoStep.Step200_To_ID_Check)
                                            id = m_motion.GetCurrentTargetID(MotionMode.To);

                                        if (!m_main.Interlock_PortOrShelfReady(id)) {
                                            AddAlarm(item, AlarmState.Alarm);
                                            break;
                                        }
                                    }
                                }
                                break;

                            case AlarmList.TO_Mode_Command_Over_Time_Error:
                                if (m_main.IsAutoState()) {
                                    if (m_main.GetSendBit(SendBitMap.To_Ready)) {
                                        if (TimerDelay(TimerType.To_Start_Timeout, m_param.GetTimerParam().CIM_TIMEOVER)) {
                                            AddAlarm(item, AlarmState.Alarm);
                                        }
                                    }
                                    else {
                                        TimerStop(TimerType.To_Start_Timeout);
                                    }
                                }
                                else {
                                    TimerStop(TimerType.To_Start_Timeout);
                                }

                                break;

                            case AlarmList.PIO_Over_Time_Error:
                                if (m_main.IsAutoState() && m_motion.IsPIOState()) {
                                    if (TimerDelay(TimerType.PIO_Timeout, m_param.GetTimerParam().CIM_TIMEOVER)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                else {
                                    TimerStop(TimerType.PIO_Timeout);
                                }

                                break;

                            case AlarmList.PIO_Ready_Off_Error:
                                if (m_motion.IsPIOState()) {
                                    if ((m_motion.GetCurrentAutoStep() > AutoStep.Step122_From_Port_Ready_Check && m_motion.GetCurrentAutoStep() < AutoStep.Step144_From_Port_Ready_Off_Check) ||
                                        (m_motion.GetCurrentAutoStep() > AutoStep.Step208_To_Port_Ready_Check && m_motion.GetCurrentAutoStep() < AutoStep.Step219_To_Port_Ready_Off_Check)) {
                                        if (!m_main.GetReceiveBit(ReceiveBitMap.PIO_Ready)) {
                                            AddAlarm(item, AlarmState.Alarm);
                                        }
                                    }
                                }

                                break;

                            //case AlarmList.Stay_Complete_ACK_Over_Time_Error:
                            //    break;

                            case AlarmList.Complete_ACK_Over_Time_Error:
                                if (m_motion.GetCurrentAutoStep() == AutoStep.Step145_From_Complete ||
                                    m_motion.GetCurrentAutoStep() == AutoStep.Step220_To_Complete) {
                                    if (TimerDelay(TimerType.Compelete_ACK_Timeout, m_param.GetTimerParam().CIM_TIMEOVER)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                else
                                    TimerStop(TimerType.Compelete_ACK_Timeout);
                                break;

                            case AlarmList.Store_ACK_Over_Time_Error:
                                if (m_motion.GetCurrentEvent() == EventList.StoreAlt) {
                                    if (TimerDelay(TimerType.StoreAlt_ACK_Timeout, m_param.GetTimerParam().CIM_TIMEOVER)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                else {
                                    TimerStop(TimerType.StoreAlt_ACK_Timeout);
                                }

                                break;

                            case AlarmList.Source_Empty_ACK_Over_Time_Error:
                                if (m_motion.GetCurrentEvent() == EventList.SourceEmpty) {
                                    if (TimerDelay(TimerType.SourceEmpty_ACK_Timeout, m_param.GetTimerParam().CIM_TIMEOVER)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                else {
                                    TimerStop(TimerType.SourceEmpty_ACK_Timeout);
                                }

                                break;

                            case AlarmList.Double_Storage_ACK_Over_Time_Error:
                                if (m_motion.GetCurrentEvent() == EventList.DoubleStorage) {
                                    if (TimerDelay(TimerType.DoubleStorage_ACK_Timeout, m_param.GetTimerParam().CIM_TIMEOVER)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                else {
                                    TimerStop(TimerType.DoubleStorage_ACK_Timeout);
                                }

                                break;

                            case AlarmList.Resume_Request_ACK_Over_Time_Error:
                                if (m_motion.GetCurrentEvent() == EventList.ResumeRequest) {
                                    if (TimerDelay(TimerType.ResumeRequest_ACK_Timeout, m_param.GetTimerParam().CIM_TIMEOVER)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                else {
                                    TimerStop(TimerType.ResumeRequest_ACK_Timeout);
                                }
                                break;

                            case AlarmList.Idle_Status_CST_Error:
                                if (m_main.IsAutoState()) {
                                    if (m_motion.GetCurrentAutoStep() == AutoStep.Step0_Idle) {
                                        if (m_main.IsCassetteAbnormal())
                                            AddAlarm(item, AlarmState.Alarm);
                                    }
                                }

                                break;

                            case AlarmList.Step_Timeover_Error:
                                if (m_motion.GetCurrentAutoStep() != AutoStep.Step0_Idle && m_motion.GetCurrentAutoStep() != AutoStep.Step500_Error) {
                                    if (m_prevStep == m_motion.GetCurrentAutoStep()) {
                                        if (TimerDelay(TimerType.Step, m_param.GetTimerParam().STEP_TEIMOVER)) {
                                            AddAlarm(item, AlarmState.Alarm);
                                        }
                                    }
                                    else {
                                        m_prevStep = m_motion.GetCurrentAutoStep();
                                        TimerStop(TimerType.Step);
                                    }
                                }
                                else {
                                    TimerStop(TimerType.Step);
                                }

                                break;

                                // Home Sensor Check
                            case AlarmList.Arm_Position_Sensor_Detect_Error_1:
                                if (m_motion.GetCurrentAutoStep() == AutoStep.Step101_From_CST_And_Fork_Home_Check ||
                                    m_motion.GetCurrentAutoStep() == AutoStep.Step201_To_CST_And_Fork_Home_Check) {
                                    if (m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.A_Axis) > (m_param.GetAxisParameter(AxisList.A_Axis).homePositionRange * 1000)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                break;

                                // Position Sensor 1
                            case AlarmList.Arm_Position_Sensor_Detect_Error_2:
                                if(m_motion.GetCurrentAutoStep() == AutoStep.Step133_From_Z_Up || m_motion.GetCurrentAutoStep() == AutoStep.Step212_To_Z_Down) {
                                    //if(target.portType != (int)PortType.OVEN_PORT) {
                                    //    if (m_param.GetAxisParameter(AxisList.A_Axis).posSensorEnabled) {
                                    //        if (!m_motion.GetPosSensor(AxisList.A_Axis)) {
                                    //            AddAlarm(item, AlarmState.Alarm);
                                    //        }
                                    //    }
                                    //}
                                    if(m_param.GetPortParameter_UseForkPositionSensor((int)target.id) && 
                                        m_param.GetPortParameter_ForkPositionSensorType((int)target.id) == PositionSensorType.PositionSensor_1) {
                                        if (m_param.GetAxisParameter(AxisList.A_Axis).posSensorEnabled) {
                                            if (!m_motion.GetPosSensor(AxisList.A_Axis)) {
                                                AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                    }
                                }
                                break;

                                // Position Sensor 2
                            case AlarmList.Arm_Position_Sensor_Detect_Error_3:
                                if(m_motion.GetCurrentAutoStep() == AutoStep.Step133_From_Z_Up || m_motion.GetCurrentAutoStep() == AutoStep.Step212_To_Z_Down) {
                                    //if(target.portType == (int)PortType.OVEN_PORT) {
                                    //    if (m_param.GetAxisParameter(AxisList.A_Axis).posSensor2Enabled) {
                                    //        if (!m_motion.GetPosSensor2(AxisList.A_Axis)) {
                                    //            AddAlarm(item, AlarmState.Alarm);
                                    //        }
                                    //    }
                                    //}
                                    if(m_param.GetPortParameter_UseForkPositionSensor((int)target.id) && 
                                        m_param.GetPortParameter_ForkPositionSensorType((int)target.id) == PositionSensorType.PositionSensor_2) {
                                        if (m_param.GetAxisParameter(AxisList.A_Axis).posSensor2Enabled) {
                                            if (!m_motion.GetPosSensor2(AxisList.A_Axis)) {
                                                AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                    }
                                }
                                break;

                            case AlarmList.From_To_Software_Limit_Over_Error_X:
                                if(m_motion.GetCurrentAutoStep() == AutoStep.Step112_From_XZT_From_Move ||
                                    m_motion.GetCurrentAutoStep() == AutoStep.Step204_To_XZT_To_Move) {
                                    if(target != null && target.valX != null) {
                                        if(target.valX > m_param.GetAxisParameter(AxisList.X_Axis).swLimitPositive ||
                                            target.valX < m_param.GetAxisParameter(AxisList.X_Axis).swLimitNegative) {
                                            AddAlarm(item, AlarmState.Alarm);
                                        }
                                    }
                                }else if (m_motion.GetCurrentAutoTeachingStep() == AutoTeachingStep.Step2_ForkHomePositionCheck) {
                                    if(m_motion.GetAutoTeachingTargetX() > (m_param.GetAxisParameter(AxisList.X_Axis).swLimitPositive) ||
                                        m_motion.GetAutoTeachingTargetX() < (m_param.GetAxisParameter(AxisList.X_Axis).swLimitNegative)) {
                                        AddAlarm(item, AlarmState.Alarm);
                                    }
                                }
                                break;

                            case AlarmList.From_To_Software_Limit_Over_Error_Z:
                                if (m_motion.GetCurrentAutoStep() == AutoStep.Step112_From_XZT_From_Move ||
                                    m_motion.GetCurrentAutoStep() == AutoStep.Step204_To_XZT_To_Move) {
                                    if (target != null && target.valZ != null) {
                                        if (target.valZ > m_param.GetWMXParameter(AxisList.Z_Axis).m_softLimitPosValue ||
                                                target.valX < m_param.GetWMXParameter(AxisList.Z_Axis).m_softLimitNegValue) {
                                            AddAlarm(item, AlarmState.Alarm);
                                        }
                                    }
                                }
                                else if (m_motion.GetCurrentAutoTeachingStep() == AutoTeachingStep.Step2_ForkHomePositionCheck) {
                                    //if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                                    //    if (m_motion.GetAutoTeachingTargetZ() > m_param.GetWMXParameter(AxisList.Z_Axis).m_softLimitPosValue ||
                                    //        m_motion.GetAutoTeachingTargetZ() < m_param.GetWMXParameter(AxisList.Z_Axis).m_softLimitNegValue) {
                                    //        AddAlarm(item, AlarmState.Alarm);
                                    //    }
                                    //}
                                    //else {
                                    //    if (m_motion.RadianToCalculateDistance(m_motion.GetAutoTeachingTargetZ()) > (m_param.GetAxisParameter(AxisList.Z_Axis).swLimitPositive) ||
                                    //        m_motion.RadianToCalculateDistance(m_motion.GetAutoTeachingTargetZ()) < (m_param.GetAxisParameter(AxisList.Z_Axis).swLimitNegative)) {
                                    //        AddAlarm(item, AlarmState.Alarm);
                                    //    }
                                    //}
                                    if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                                        if (m_motion.GetAutoTeachingTargetZ() > m_param.GetWMXParameter(AxisList.Z_Axis).m_softLimitPosValue ||
                                            m_motion.GetAutoTeachingTargetZ() < m_param.GetWMXParameter(AxisList.Z_Axis).m_softLimitNegValue) {
                                            AddAlarm(item, AlarmState.Alarm);
                                        }
                                    }
                                    else {
                                        double tempZ = m_motion.DistanceToCalculatedRadian(m_motion.GetAutoTeachingTargetZ());
                                        if (tempZ > m_param.GetWMXParameter(AxisList.Z_Axis).m_softLimitPosValue ||
                                            tempZ < m_param.GetWMXParameter(AxisList.Z_Axis).m_softLimitNegValue) {
                                            AddAlarm(item, AlarmState.Alarm);
                                        }
                                    }
                                }
                                break;

                            case AlarmList.From_To_Software_Limit_Over_Error_A:
                                // 사용 안함
                                break;

                            case AlarmList.From_To_Software_Limit_Over_Error_T:
                                // 사용 안함
                                break;

                            case AlarmList.Turn_Position_Sensor_Detect_Error:
                                if (m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                                    if (m_motion.GetCurrentAutoStep() == AutoStep.Step114_From_Shelf_Port_Check || m_motion.GetCurrentAutoStep() == AutoStep.Step207_To_Shelf_Port_Check) {
                                        if (target.direction == (int)PortDirection_HP.Left) {
                                            if (!m_motion.GetAxisSensor(AxisSensorType.Home, AxisList.T_Axis) && !m_motion.GetPosSensor(AxisList.T_Axis)) {
                                                AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                        else if (target.direction == (int)PortDirection_HP.Right) {
                                            if (!m_motion.GetPosSensor(AxisList.T_Axis) && !m_motion.GetAxisSensor(AxisSensorType.Home, AxisList.T_Axis)) {
                                                AddAlarm(item, AlarmState.Alarm);
                                            }
                                        }
                                        else {
                                            AddAlarm(item, AlarmState.Alarm);
                                        }
                                    }
                                }
                                break;

                            case AlarmList.Regulator_AL_CAP_Fuse_Open:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.AL_CAP_Fuse_Open)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_AL_CAP_OverHeat_NTC:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.AL_CAP_OverHeat_NTC)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_AL_CAP_OverHeat_ThermalWire:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.AL_CAP_OverHeat_ThermalWire)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Boost_OverCurrent:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Boost_OverCurrent)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Boost_OverVoltage:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Boost_OverVoltage)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Boost_PeackCurrent:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Boost_PeackCurrent)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Boost_PeackVoltage:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Boost_PeackVoltage)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Controller_Defect:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (!m_main.IsConnected_Regulator()) {
                                        if (TimerDelay(TimerType.Regulator_Disconnected, m_disConnected_Timeout)) {
                                            AddAlarm(item, AlarmState.Alarm);
                                        }
                                    }
                                    else {
                                        TimerStop(TimerType.Regulator_Disconnected);
                                    }
                                }
                                else {
                                    TimerStop(TimerType.Regulator_Disconnected);
                                }
                                break;

                            case AlarmList.Regulator_Current_Detection_Error:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Current_Detection_Error)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_HeatSink_OverHeat_Contact:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.HeatSink_OverHeat_Contact)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_HeatSink_OverHeat_NTC:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.HeatSink_OverHeat_NTC)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Input_LowVoltage:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Input_LowVoltage)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Inside_OverHeat_NTC:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Inside_OverHeat_NTC)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Output_Fuse_Open:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Output_Fuse_Open)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Output_LowVoltage:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Output_LowVoltage)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Output_OverCurrent:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Output_OverCurrent)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Output_OverVoltage:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Output_OverVoltage)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Output_PeakCurrent:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Output_PeakCurrent)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Overload:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Overload)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Pickup_Input_OverCurrent:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Pickup_Input_OverCurrent)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Pickup_OverHeat_Contact:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Pickup_OverHeat_Contact)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Pickup_OverHeat_NTC:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Pickup_OverHeat_NTC)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Protect_Board_Open:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Protect_Board_Open)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_REG_ERR_Gate_Fault:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.REG_ERR_Gate_Fault)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;

                            case AlarmList.Regulator_Voltage_Detection_Error:
                                if (m_param.GetMotionParam().useRegulator) {
                                    if (m_main.Regulator_GetData(TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode) == (short)RegulatorAlarmList.Voltage_Detection_Error)
                                        AddAlarm(item, AlarmState.Alarm);
                                }
                                break;
                        }
                    }
                }
                catch(Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, $"Alarm Check Error", ex));
                }
            }
        }
    }
}
