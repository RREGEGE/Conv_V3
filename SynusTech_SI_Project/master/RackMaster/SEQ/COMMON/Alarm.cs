using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.PART;
using RackMaster.SEQ.CLS;
using MovenCore;

namespace RackMaster.SEQ.COMMON
{
    public static class Alarm
    {
        public enum AlarmList {
            HP_EStop,
            OP_EStop,
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
            Port_Interface_ULD_Event,
            Port_Interface_LD_Event,

            X_Servo_Battery_Low_Alarm = 33,
            Z_Servo_Battery_Low_Alarm,
            A_Servo_Battery_Low_Alarm,
            T_Servo_Battery_Low_Alarm,

            X_Servo_Pack_Error = 48,
            X_Axis_POT,
            X_Axis_NOT,
            X_Axis_Origin_Search_Fail_Home_Sensor,
            X_Axis_Soft_Limit_Error,
            X_Axis_InPosition_Error,
            X_Axis_Absolute_Origin_Loss,
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
            Z_Axis_Origin_Search_Fail_Home_Sensor,
            Z_Axis_Soft_Limit_Error,
            Z_Axis_InPosition_Error,
            Z_Axis_Absolute_Origin_Loss,
            Z_Axis_Over_Load_Error = 88,
            Z_Axis_Home_Sensor_Always_On_Error,
            Z_Home_Move_Time_Out_Error,

            A_Servo_Pack_Error = 96,
            A_Axis_POT,
            A_Axis_NOT,
            A_Axis_Origin_Search_Fail_Home_Sensor,
            A_Axis_Soft_Limit_Error,
            A_Axis_InPosition_Error,
            A_Axis_Absolute_Origin_Loss,
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
            T_Axis_Origin_Search_Fail_Home_Sensor,
            T_Axis_Soft_Limit_Error,
            T_Axis_InPosition_Error,
            T_Axis_Absolute_Origin_Loss,
            T_Axis_Over_Load_Error = 120,
            T_Axis_Home_Sensor_Always_On_Error,
            T_Home_Move_Time_Out_Error,

            Command_ID_Range_Error = 128,
            TO_Mode_Command_Over_Time_Error,
            PIO_Over_Time_Error,
            PIO_Ready_Off_Error,
            Stay_Complete_ACK_Over_Time_Error,
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

            MAX = 191,
        }

        public enum AlarmState {
            Idle,
            Error,
            Warning,
            Event,
            WMX3_Call_Error,
        }

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

            MAX_COUNT,
        }

        private static List<short> m_alarm;
        private static List<int> m_wmxErrorCode;
        private static string[] m_alarmNameList;
        private static Motor m_motor;
        private static Timer[] m_timer;
        private static RackMasterMotion.RM_STEP m_currentStep;

        public static AlarmState m_currentState;
        

        public static void Init() {
            m_alarm = new List<short>();
            m_wmxErrorCode = new List<int>();
            m_alarmNameList = new string[(int)AlarmList.MAX];
            m_motor = Motor.Instance;
            m_timer = new Timer[(int)TimerType.MAX_COUNT];
            m_currentStep = RackMasterMotion.RM_STEP.Idle;

            for (int i = 0; i < (int)TimerType.MAX_COUNT; i++) {
                m_timer[i] = new Timer();
                m_timer[i].Stop();
            }

            m_currentState = AlarmState.Idle;

            foreach(AlarmList item in Enum.GetValues(typeof(AlarmList))) {
                if (item == AlarmList.MAX)
                    return;

                m_alarmNameList[(int)item] = item.ToString();
            }
        }

        public static void AddAlarm(AlarmList alarmCode, AlarmState state) {
            if (m_alarm.Contains((short)alarmCode))
                return;

            m_alarm.Add((short)alarmCode);
            m_currentState = state;

            if (state == AlarmState.Error) {
                Global.ERROR_STATE = true;
                Global.AUTO_STATE = false;
                Global.MANUAL_STATE = true;
                Log.Add(new Log.LogItem(Log.LogType.Alarm, m_alarmNameList[(int)alarmCode]));
            }
            else {
                string logMsg = $"[{state}]{m_alarmNameList[(int)alarmCode]}";
                Log.Add(new Log.LogItem(Log.LogType.Alarm, logMsg));
            }

            if (Global.PIO_STATE)
                Global.PIO_STATE = false;

            return;
        }

        public static void AddWMX3ErrorCode(int errorCode, AlarmState state) {
            return;

            if (m_wmxErrorCode.Contains(errorCode))
                return;

            m_wmxErrorCode.Add(errorCode);
            m_currentState = state;

            Log.Add(new Log.LogItem(Log.LogType.WMX, WMX3.ErrorCodeToString(errorCode)));

            Global.ERROR_STATE = true;
            if (state == AlarmState.WMX3_Call_Error) {
                RackMasterMotion.SetStep(RackMasterMotion.RM_STEP.WMX3_Call_Error);
            }
        }

        public static void ClearAlarm() {
            if(m_alarm.Count > 0 || m_wmxErrorCode.Count > 0) {
                m_alarm.Clear();
                m_wmxErrorCode.Clear();
                m_currentState = AlarmState.Idle;
                Global.ERROR_STATE = false;
                RackMasterMotion.SetStep(RackMasterMotion.RM_STEP.Idle);
                return;
            }
        }

        public static void ClearWMX3Error() {
            if(m_wmxErrorCode.Count > 0) {
                m_wmxErrorCode.Clear();
                return;
            }
        }

        public static int GetCurrentAlarmCount() {
            return m_alarm.Count;
        }

        public static short GetAlarmCode(int index) {
            return m_alarm[index];
        }

        private static void TimerStop(TimerType timer) {
            m_timer[(int)timer].Reset();
            m_timer[(int)timer].Stop();
        }

        private static void TimerStart(TimerType timer) {
            if (!m_timer[(int)timer].IsTimerStarted())
                m_timer[(int)timer].Start();
        }

        private static bool TimerDelay(TimerType timer, long delayMilliseconds) {
            if (m_timer[(int)timer].IsTimerStarted()) {
                return m_timer[(int)timer].Delay(delayMilliseconds);
            }
            else {
                m_timer[(int)timer].Start();
            }

            return false;
        }

        private static bool IsTimerStarted(TimerType timer) {
            return m_timer[(int)timer].IsTimerStarted();
        }

        // Error_Fuction은 true면 Error발생 false면 미알람

        private static bool Error_ServoPackAlarm(RMParam.RMAxis axis) {
            return m_motor.GetServoFlag(Motor.ServoFlagType.Alarm, axis);
        }

        private static bool Error_POT(RMParam.RMAxis axis) {
            return m_motor.GetSensorStatus(Motor.SensorType.Positive_Limit, axis);
        }

        private static bool Error_NOT(RMParam.RMAxis axis) {
            return m_motor.GetSensorStatus(Motor.SensorType.Negative_Limit, axis);
        }

        private static bool Error_OriginSearchFail(RMParam.RMAxis axis) {
            if (m_motor.IsHoming(axis)) {
                if(m_motor.GetSensorStatus(Motor.SensorType.Negative_Limit, axis) ||
                    m_motor.GetSensorStatus(Motor.SensorType.Positive_Limit, axis)){
                    return true;
                }
            }

            return false;
        }

        private static bool Error_SoftwareLimit(RMParam.RMAxis axis) {
            if(m_motor.GetSensorStatus(Motor.SensorType.SW_Negative_Limit, axis) ||
                m_motor.GetSensorStatus(Motor.SensorType.SW_Positive_Limit, axis)) {
                return true;
            }

            return false;
        }

        private static bool Error_Inposition(RMParam.RMAxis axis) {
            if(!m_motor.GetServoFlag(Motor.ServoFlagType.Run, axis)) {
                if(!m_motor.GetServoFlag(Motor.ServoFlagType.Poset, axis)) {
                    return true;
                }
            }

            return false;
        }

        private static bool Error_AbsoluteOriginLoss(RMParam.RMAxis axis) {
            return false;
        }

        private static bool Error_Overload(RMParam.RMAxis axis) {
            double overload = RMParam.GetServoParam(RMParam.ServoParam.Max_Overload, axis);
            double current_overload = Math.Abs(m_motor.GetServoStatus(Motor.ServoStatusType.trq_act, axis));

            if (current_overload > overload) {
                m_motor.Overload_Alarm(axis);
                return true;
            }

            return false;
        }

        private static bool Error_HomeSensorAlwaysOn(RMParam.RMAxis axis) {
            if (m_motor.IsHoming(axis))
                return false;

            double originRange = RMParam.GetServoParam(RMParam.ServoParam.Home_Position_Range, axis);

            if(m_motor.GetServoStatus(Motor.ServoStatusType.pos_act, axis) > originRange) {
                if (m_motor.GetSensorStatus(Motor.SensorType.Home, axis))
                    return true;
            }

            return false;
        }

        private static bool Error_HomeMoveTimeOut(RMParam.RMAxis axis) {
            TimerType t_type = TimerType.Home_Move_Timeout_A;

            if(axis == RMParam.RMAxis.X_Axis) {
                t_type = TimerType.Home_Move_Timeout_X;
            }else if(axis == RMParam.RMAxis.Z_Axis) {
                t_type = TimerType.Home_Move_Timeout_Z;
            }else if(axis == RMParam.RMAxis.A_Axis) {
                t_type = TimerType.Home_Move_Timeout_A;
            }else if(axis == RMParam.RMAxis.T_Axis) {
                t_type = TimerType.Home_Move_Timeout_T;
            }

            if (m_motor.IsHoming(axis)) {
                if (!IsTimerStarted(t_type)) {
                    TimerStart(t_type);
                }

                if(TimerDelay(t_type, RMParam.StepTime.HOME_MOVE_TIMEOVER)) {
                    return true;
                }
            }
            else {
                TimerStop(t_type);
            }

            return false;
        }

        public static void AlarmCheck() {
            if (m_currentState == AlarmState.Error) {
                Global.ERROR_STATE = true;
            }
            else {
                Global.ERROR_STATE = false;
            }

            PortData port = null;

            if(RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_CST_And_Fork_Home_Check) {
                port = Port.GetPortData(RackMasterMotion.GetFromID());
            }else if(RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_CST_And_Fork_Home_Check) {
                port = Port.GetPortData(RackMasterMotion.GetToID());
            }

            foreach(AlarmList item in Enum.GetValues(typeof(AlarmList))) {
                switch (item) {
                    case AlarmList.HP_EStop:
                        if (Io.GetInputBit((int)IoList.InputList.EMO_HP)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.OP_EStop:
                        if (Io.GetInputBit((int)IoList.InputList.EMO_OP)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.GOT_EStop:
                        if (Io.GetInputBit((int)IoList.InputList.Touch_EMO_SW)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.Power_Off_Error:
                        //if (RackMasterMotion.GetCurrentStep() != RackMasterMotion.RM_STEP.Idle)
                        //    if (!Io.GetInputBit((int)IoList.InputList.RM_MC_On)) {
                        //        AddAlarm(item, AlarmState.Error);
                        //    }
                        break;

                    case AlarmList.MST_HP_EStop:
                        if (RackMasterTCP.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_HP_EMO)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.MST_OP_EStop:
                        if (RackMasterTCP.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_OP_EMO)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.MST_HP_Escape:
                        if (RackMasterTCP.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_HP_Escape)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.MST_OP_Escape:
                        if (RackMasterTCP.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_OP_Escape)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.HP_Door_Open:
                        if (RackMasterTCP.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_HP_Door_Open)) {
                            if (Global.AUTO_STATE) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        break;

                    case AlarmList.OP_Door_Open:
                        if (RackMasterTCP.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_OP_Door_Open)) {
                            if (Global.AUTO_STATE) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        break;

                    case AlarmList.MST_DisConnected:
                        if (!RackMasterTCP.isConnected) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.Test_Mode_Double_Storage_Error:
                        //if (Global.MANUAL_STATE && !Global.AUTO_STATE)
                        //{
                        //    if (RackMasterMotion.GetCurrentStep() > RackMasterMotion.RM_STEP.To_CST_And_Fork_Home_Check &&
                        //        RackMasterMotion.GetCurrentStep() < RackMasterMotion.RM_STEP.To_Complete)
                        //    {
                        //        if (Io.GetInputBit((int)IoList.InputList.Fork_Double_Storage_Sensor))
                        //        {
                        //            AddAlarm(item, AlarmState.Error);
                        //        }
                        //    }
                        //}
                        //if (Global.AUTO_TEACHING_STATE)
                        //{
                        //    if (Io.GetInputBit(IoList.InputList.Fork_Double_Storage_Sensor))
                        //    {
                        //        AddAlarm(item, AlarmState.Error);
                        //    }
                        //}
                        break;

                    case AlarmList.Test_Mode_CST_Presence_Error:
                        if (Global.MANUAL_STATE && !Global.AUTO_STATE) {
                            if (RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.From_ID_Check &&
                                RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.From_XZT_From_Complete) {
                                if (!Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_1) ||
                                    !Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_2) ||
                                    Io.GetInputBit(IoList.InputList.Fork_Presence_Sensor)) {
                                    AddAlarm(item, AlarmState.Error);
                                }
                            }
                        }

                        break;

                    case AlarmList.CST_Detect_Sensor_Check_Error:
                        if (!Global.MANUAL_STATE && Global.AUTO_STATE) {
                            if (RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.From_ID_Check &&
                                RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.From_XZT_From_Complete) {
                                if (!Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_1) ||
                                    !Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_2) ||
                                    Io.GetInputBit(IoList.InputList.Fork_Presence_Sensor)) {
                                    AddAlarm(item, AlarmState.Error);
                                }
                            }
                        }
                        else {
                            if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.Teaching_Fork_Home_Check &&
                                RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.Teaching_XZT_InpositionCheck) {
                                if(!Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_1) ||
                                    !Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_2) ||
                                    Io.GetInputBit(IoList.InputList.Fork_Presence_Sensor)) {
                                    AddAlarm(item, AlarmState.Error);
                                }
                            }
                        }

                        break;

                    case AlarmList.CST_Abnormal_Detect_Sensor_Check_Error:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_CST_Check_Sensor) {
                            if (Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_1) || Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_2)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        break;

                    case AlarmList.Double_Storage:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Double_Storage) {
                            AddAlarm(item, AlarmState.Event);
                        }
                        break;

                    case AlarmList.Source_Empty:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Source_Empty) {
                            AddAlarm(item, AlarmState.Event);
                        }

                        break;

                    case AlarmList.Port_Interface_ULD_Event:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_Port_Ready_Check || RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_Shelf_Port_Check ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_Port_Ready_Off_Check) {
                            if (TimerDelay(TimerType.ResumeRequest, RMParam.StepTime.PIO_READY_TIMOVER)) {
                                AddAlarm(item, AlarmState.Event);
                                RackMasterMotion.SetStep(RackMasterMotion.RM_STEP.Resume_Request);
                            }
                        }
                        else {
                            TimerStop(TimerType.ResumeRequest);
                        }

                        break;

                    case AlarmList.Port_Interface_LD_Event:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_Port_Ready_Check || RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_Shelf_Port_Check ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_Port_Ready_Off_Check) {
                            if (TimerDelay(TimerType.StoreAlt, RMParam.StepTime.PIO_READY_TIMOVER)) {
                                AddAlarm(item, AlarmState.Event);
                                RackMasterMotion.SetStep(RackMasterMotion.RM_STEP.Store_Alt);
                            }
                        }
                        else {
                            TimerStop(TimerType.StoreAlt);
                        }

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
                        if (Error_ServoPackAlarm(RMParam.RMAxis.X_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.X_Axis_POT:
                        if (Error_POT(RMParam.RMAxis.X_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.X_Axis_NOT:
                        if (Error_NOT(RMParam.RMAxis.X_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.X_Axis_Origin_Search_Fail_Home_Sensor:
                        if (Error_OriginSearchFail(RMParam.RMAxis.X_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }

                        break;

                    case AlarmList.X_Axis_Soft_Limit_Error:
                        if (Error_SoftwareLimit(RMParam.RMAxis.X_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.X_Axis_InPosition_Error:
                        // 조건보고 구성
                        break;

                    case AlarmList.X_Axis_Absolute_Origin_Loss:
                        // 확인하고 구성
                        break;

                    case AlarmList.X_Axis_Over_Load_Error:
                        if (Error_Overload(RMParam.RMAxis.X_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }

                        break;

                    case AlarmList.X_Axis_Home_Sensor_Always_On_Error:
                        if (Error_HomeSensorAlwaysOn(RMParam.RMAxis.X_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.X_Home_Move_Time_Out_Error:
                        if (Error_HomeMoveTimeOut(RMParam.RMAxis.X_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }

                        break;

                    case AlarmList.Pick_Position_Error:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_Pick_Sensor_Cehck) {
                            if (!Io.GetInputBit(IoList.InputList.Fork_Pick_Sensor)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }

                        break;

                    case AlarmList.Place_Position_Error:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_Place_Sensor_Check) {
                            if (!Io.GetInputBit(IoList.InputList.Fork_Place_Sensor)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }

                        break;

                    case AlarmList.X_Axis_Barcode_Sensor_Error:
                        break;

                    case AlarmList.X_Axis_Free_Run:
                        break;

                    case AlarmList.Z_Servo_Pack_Error:
                        if (Error_ServoPackAlarm(RMParam.RMAxis.Z_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.Z_Axis_POT:
                        if (Error_POT(RMParam.RMAxis.Z_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.Z_Axis_NOT:
                        if (Error_NOT(RMParam.RMAxis.Z_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.Z_Axis_Origin_Search_Fail_Home_Sensor:
                        if (Error_OriginSearchFail(RMParam.RMAxis.Z_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }

                        break;

                    case AlarmList.Z_Axis_Soft_Limit_Error:
                        if (Error_SoftwareLimit(RMParam.RMAxis.Z_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.Z_Axis_InPosition_Error:
                        break;

                    case AlarmList.Z_Axis_Absolute_Origin_Loss:
                        break;

                    case AlarmList.Z_Axis_Over_Load_Error:
                        if (Error_Overload(RMParam.RMAxis.Z_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }

                        break;

                    case AlarmList.Z_Axis_Home_Sensor_Always_On_Error:
                        if (Error_HomeSensorAlwaysOn(RMParam.RMAxis.Z_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }

                        break;

                    case AlarmList.Z_Home_Move_Time_Out_Error:
                        if (Error_HomeMoveTimeOut(RMParam.RMAxis.Z_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }

                        break;

                    case AlarmList.A_Servo_Pack_Error:
                        if (Error_ServoPackAlarm(RMParam.RMAxis.A_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.A_Axis_POT:
                        if (Error_POT(RMParam.RMAxis.A_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.A_Axis_NOT:
                        if (Error_NOT(RMParam.RMAxis.A_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.A_Axis_Origin_Search_Fail_Home_Sensor:
                        if (Error_OriginSearchFail(RMParam.RMAxis.A_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }

                        break;

                    case AlarmList.A_Axis_Soft_Limit_Error:
                        if (Error_SoftwareLimit(RMParam.RMAxis.A_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.A_Axis_InPosition_Error:
                        break;

                    case AlarmList.A_Axis_Absolute_Origin_Loss:
                        break;

                    case AlarmList.A_Axis_Over_Load_Error:
                        if (Error_Overload(RMParam.RMAxis.A_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }

                        break;

                    case AlarmList.A_Axis_Home_Sensor_Always_On_Error:
                        // 음성공장에서 Home Sensor가 이상함
                        //if (Error_HomeSensorAlwaysOn(RMParameters.Servo.AXIS_A)) {
                        //    AddAlarm(item, AlarmState.Error);
                        //}

                        break;

                    case AlarmList.A_Home_Move_Time_Out_Error:
                        if (Error_HomeMoveTimeOut(RMParam.RMAxis.A_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }

                        break;

                    case AlarmList.RM_Moving_Arm_Home_Sensor_Dont_Detect_Error:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_XZT_From_Move ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_XZT_From_Complete ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_XZT_To_Move ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_XZT_To_Complete) {
                            if (!m_motor.GetSensorStatus(Motor.SensorType.Home, RMParam.RMAxis.A_Axis)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        break;

                    case AlarmList.Arm_Table_CST_Detect_Error_To_Complete:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_Complete) {
                            if (!Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_1) ||
                                !Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_2)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }

                        break;

                    case AlarmList.Cassette_Fail_To_X_Move:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_XZT_To_Move ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_XZT_To_Complete) {
                            if (!Global.CST_ON) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }

                        break;

                    case AlarmList.Arm_Table_Cross_Sensor_Error:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_Complete) {
                            if (!Global.CST_ON)
                                AddAlarm(item, AlarmState.Error);
                        } else if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_Complete) {
                            if (Io.GetInputBit(IoList.InputList.Fork_Presence_Sensor) ||
                                !Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_1) ||
                                !Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_2)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }

                        break;

                    case AlarmList.Storage_Failure_Error_To_Complete:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_Complete) {
                            if (Io.GetInputBit((int)IoList.InputList.Fork_Presence_Sensor)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }

                        break;

                    case AlarmList.T_Servo_Pack_Error:
                        if (Error_ServoPackAlarm(RMParam.RMAxis.T_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.T_Axis_POT:
                        if (Error_POT(RMParam.RMAxis.T_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.T_Axis_NOT:
                        if (Error_NOT(RMParam.RMAxis.T_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.T_Axis_Origin_Search_Fail_Home_Sensor:
                        if (Error_OriginSearchFail(RMParam.RMAxis.T_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.T_Axis_Soft_Limit_Error:
                        if (Error_SoftwareLimit(RMParam.RMAxis.T_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.T_Axis_InPosition_Error:
                        break;

                    case AlarmList.T_Axis_Absolute_Origin_Loss:
                        break;

                    case AlarmList.T_Axis_Over_Load_Error:
                        if (Error_Overload(RMParam.RMAxis.T_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.T_Axis_Home_Sensor_Always_On_Error:
                        //if (Error_HomeSensorAlwaysOn(RMParam.RMAxis.T_Axis)) {
                        //    AddAlarm(item, AlarmState.Error);
                        //}
                        break;

                    case AlarmList.T_Home_Move_Time_Out_Error:
                        if (Error_HomeMoveTimeOut(RMParam.RMAxis.T_Axis)) {
                            AddAlarm(item, AlarmState.Error);
                        }
                        break;

                    case AlarmList.Command_ID_Range_Error:
                        if (Global.AUTO_STATE) {
                            if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_ID_Check ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_ID_Check) {
                                int id = 0;
                                if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_ID_Check)
                                    id = RackMasterMotion.GetFromID();
                                else if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_ID_Check)
                                    id = RackMasterMotion.GetToID();

                                if (!Port.IsExistPort(id)) {
                                    // 잘못된 ID Error 처리
                                    AddAlarm(item, AlarmState.Error);
                                    break;
                                }

                                if (!Port.IsExistPortData(id)) {
                                    // Teaching이 완료되지 않음
                                    AddAlarm(item, AlarmState.Error);
                                    break;
                                }

                                if (!Port.IsEnablePort(id)) {
                                    // 비활성화된 ID임
                                    AddAlarm(item, AlarmState.Error);
                                    break;
                                }
                            }
                        }
                        break;

                    case AlarmList.TO_Mode_Command_Over_Time_Error:
                        if (Global.AUTO_STATE) {
                            if (RackMasterTCP.GetSendBit(TCP.TcpDataDef.SendBitMap.To_Ready)) {
                                if (TimerDelay(TimerType.To_Start_Timeout, RMParam.StepTime.CIM_TIMEOVER)) {
                                    AddAlarm(item, AlarmState.Error);
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
                        if (Global.AUTO_STATE && Global.PIO_STATE) {
                            if (TimerDelay(TimerType.PIO_Timeout, RMParam.StepTime.CIM_TIMEOVER)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        else {
                            TimerStop(TimerType.PIO_Timeout);
                        }

                        break;

                    case AlarmList.PIO_Ready_Off_Error:
                        if (Global.PIO_STATE) {
                            if ((RackMasterMotion.GetCurrentStep() > RackMasterMotion.RM_STEP.From_Port_Ready_Check && RackMasterMotion.GetCurrentStep() < RackMasterMotion.RM_STEP.From_Port_Ready_Off_Check) ||
                                (RackMasterMotion.GetCurrentStep() > RackMasterMotion.RM_STEP.To_Port_Ready_Check && RackMasterMotion.GetCurrentStep() < RackMasterMotion.RM_STEP.To_Port_Ready_Off_Check)) {
                                if (!RackMasterTCP.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.PIO_Ready)) {
                                    AddAlarm(item, AlarmState.Error);
                                }
                            }
                        }

                        break;

                    case AlarmList.Stay_Complete_ACK_Over_Time_Error:
                        break;

                    case AlarmList.Complete_ACK_Over_Time_Error:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_Complete ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_Complete) {
                            if (TimerDelay(TimerType.Compelete_ACK_Timeout, RMParam.StepTime.CIM_TIMEOVER)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        else
                            TimerStop(TimerType.Compelete_ACK_Timeout);
                        break;

                    case AlarmList.Store_ACK_Over_Time_Error:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Store_Alt) {
                            if (TimerDelay(TimerType.StoreAlt_ACK_Timeout, RMParam.StepTime.CIM_TIMEOVER)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        else {
                            TimerStop(TimerType.StoreAlt_ACK_Timeout);
                        }

                        break;

                    case AlarmList.Source_Empty_ACK_Over_Time_Error:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Source_Empty) {
                            if (TimerDelay(TimerType.SourceEmpty_ACK_Timeout, RMParam.StepTime.CIM_TIMEOVER)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        else {
                            TimerStop(TimerType.SourceEmpty_ACK_Timeout);
                        }

                        break;

                    case AlarmList.Double_Storage_ACK_Over_Time_Error:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Double_Storage) {
                            if (TimerDelay(TimerType.DoubleStorage_ACK_Timeout, RMParam.StepTime.CIM_TIMEOVER)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        else {
                            TimerStop(TimerType.DoubleStorage_ACK_Timeout);
                        }

                        break;

                    case AlarmList.Resume_Request_ACK_Over_Time_Error:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Resume_Request) {
                            if (TimerDelay(TimerType.ResumeRequest_ACK_Timeout, RMParam.StepTime.CIM_TIMEOVER)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        else {
                            TimerStop(TimerType.ResumeRequest_ACK_Timeout);
                        }
                        break;

                    case AlarmList.Idle_Status_CST_Error:
                        if (Global.AUTO_STATE) {
                            if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Idle) {
                                if (Io.GetInputBit(IoList.InputList.Fork_Presence_Sensor)) {
                                    if (Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_1) || Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_2)) {
                                        AddAlarm(item, AlarmState.Error);
                                    }
                                }
                                else if (!Io.GetInputBit(IoList.InputList.Fork_Presence_Sensor)) {
                                    if (!Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_1) || !Io.GetInputBit(IoList.InputList.Fork_Placement_Sensor_2)) {
                                        AddAlarm(item, AlarmState.Error);
                                    }
                                }
                            }
                        }

                        break;

                    case AlarmList.Step_Timeover_Error:
                        if (RackMasterMotion.GetCurrentStep() != RackMasterMotion.RM_STEP.Idle && RackMasterMotion.GetCurrentStep() != RackMasterMotion.RM_STEP.Error) {
                            if (m_currentStep == RackMasterMotion.GetCurrentStep()) {
                                if (TimerDelay(TimerType.Step, RMParam.StepTime.STEP_TEIMOVER)) {
                                    AddAlarm(item, AlarmState.Error);
                                }
                            }
                            else {
                                m_currentStep = RackMasterMotion.GetCurrentStep();
                                TimerStop(TimerType.Step);
                            }
                        }
                        else {
                            TimerStop(TimerType.Step);
                        }

                        break;

                    case AlarmList.Arm_Position_Sensor_Detect_Error_1:
                    case AlarmList.Arm_Position_Sensor_Detect_Error_2:
                    case AlarmList.Arm_Position_Sensor_Detect_Error_3:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_CST_And_Fork_Home_Check ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_CST_And_Fork_Home_Check){
                            if (m_motor.GetServoStatus(Motor.ServoStatusType.pos_act, RMParam.RMAxis.A_Axis) > RMParam.GetParam(RMParam.Param.Arm_Home_Position)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        break;

                    case AlarmList.From_To_Software_Limit_Over_Error_X:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_CST_And_Fork_Home_Check ||
                                RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_CST_And_Fork_Home_Check) {
                            if (port != null) {
                                if (port.valX > RMParam.GetServoParam(RMParam.ServoParam.Soft_Limit_Positive, RMParam.RMAxis.X_Axis) ||
                                    port.valX < RMParam.GetServoParam(RMParam.ServoParam.Soft_Limit_Negative, RMParam.RMAxis.X_Axis)) {
                                    AddAlarm(item, AlarmState.Error);
                                }
                            }
                        }
                        else if (Global.AUTO_TEACHING_STATE) {
                            if (RackMasterMotion.GetAutoTeachingTargetX() > RMParam.GetServoParam(RMParam.ServoParam.Soft_Limit_Positive, RMParam.RMAxis.X_Axis) ||
                                RackMasterMotion.GetAutoTeachingTargetX() < RMParam.GetServoParam(RMParam.ServoParam.Soft_Limit_Negative, RMParam.RMAxis.X_Axis)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        break;

                    case AlarmList.From_To_Software_Limit_Over_Error_Z:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_CST_And_Fork_Home_Check ||
                                RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_CST_And_Fork_Home_Check) {
                            if (port != null) {
                                if ((port.valZ + port.valZUp) > RMParam.GetServoParam(RMParam.ServoParam.Soft_Limit_Positive, RMParam.RMAxis.Z_Axis) ||
                                    (port.valZ - port.valZDown) < RMParam.GetServoParam(RMParam.ServoParam.Soft_Limit_Negative, RMParam.RMAxis.Z_Axis)) {
                                    AddAlarm(item, AlarmState.Error);
                                }
                            }
                        }
                        else if (Global.AUTO_TEACHING_STATE) {
                            if (RackMasterMotion.GetAutoTeachingTargetZ() > RMParam.GetServoParam(RMParam.ServoParam.Soft_Limit_Positive, RMParam.RMAxis.Z_Axis) ||
                                RackMasterMotion.GetAutoTeachingTargetZ() < RMParam.GetServoParam(RMParam.ServoParam.Soft_Limit_Negative, RMParam.RMAxis.Z_Axis)) {
                                AddAlarm(item, AlarmState.Error);
                            }
                        }
                        break;

                    case AlarmList.From_To_Software_Limit_Over_Error_A:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_CST_And_Fork_Home_Check ||
                                RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_CST_And_Fork_Home_Check) {
                            if (port != null) {
                                if (port.valFork_A > RMParam.GetServoParam(RMParam.ServoParam.Soft_Limit_Positive, RMParam.RMAxis.A_Axis) ||
                                    port.valFork_A < RMParam.GetServoParam(RMParam.ServoParam.Soft_Limit_Negative, RMParam.RMAxis.A_Axis)) {
                                    AddAlarm(item, AlarmState.Error);
                                }
                            }
                        }
                        break;

                    case AlarmList.From_To_Software_Limit_Over_Error_T:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_CST_And_Fork_Home_Check ||
                                RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_CST_And_Fork_Home_Check) {
                            if (port != null) {
                                if (port.valT > RMParam.GetServoParam(RMParam.ServoParam.Soft_Limit_Positive, RMParam.RMAxis.T_Axis) ||
                                    port.valT < RMParam.GetServoParam(RMParam.ServoParam.Soft_Limit_Negative, RMParam.RMAxis.T_Axis)) {
                                    AddAlarm(item, AlarmState.Error);
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}
