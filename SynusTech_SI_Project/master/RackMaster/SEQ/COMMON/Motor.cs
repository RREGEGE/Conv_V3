using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovenCore;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SEQ.COMMON
{
    public class Motor {
        public enum SensorType {
            Home = 0,
            Negative_Limit,
            Positive_Limit,
            SW_Negative_Limit,
            SW_Positive_Limit
        }

        public enum ServoStatusType { 
            pos_cmd = 0,
            pos_act,
            vel_cmd,
            vel_act,
            trq_cmd,
            trq_act,
            Profile_Traget_Pisition,
        }

        public enum ServoFlagType {
            Servo_On = 0,
            Run,
            HomeDone,
            Alarm,
            Waiting_Trigger,
            Inposition,
            Poset,
        }

        public enum ParameterType {
            GearRatio_Numerator,
            GearRatio_Denominator,
            Software_Limit_Positive,
            Software_Limit_Negative,
        }

        private static class Overload {
            public static double m_alarmOverload_X = 0;
            public static double m_alarmOverload_Z = 0;
            public static double m_alarmOverload_A = 0;
            public static double m_alarmOverload_T = 0;
        }

        private static class TotalDistance {
            public static double m_totalDist_X = 0;
            public static double m_totalDist_Z = 0;
            public static double m_totalDist_A = 0;
            public static double m_totalDist_T = 0;
        }

        private WMXMotion m_motor;
        private WMXMotion.AxisStatus[] m_status;
        
        private Motor() {
            m_motor = new WMXMotion("RackMaster");
            m_status = new WMXMotion.AxisStatus[RMParam.GetAxisCount()];
            for(int i = 0; i < m_status.Length; i++) {
                m_status[i] = new WMXMotion.AxisStatus();
            }
        }

        private static readonly Lazy<Motor> instanceHolder = new Lazy<Motor>(() => new Motor());

        public static Motor Instance {
            get {
                return instanceHolder.Value;
            }
        }

        public bool ServoOn(RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            int ret = m_motor.ServoOn(axisIndex);
            if(ret != WMXParam.ErrorCode_None) {
                Alarm.AddWMX3ErrorCode(ret, Alarm.AlarmState.WMX3_Call_Error);

                return false;
            }
            return true;
        }

        public void ServoOff(RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            m_motor.ServoOff(axisIndex);
        }

        public bool IsServoOn(RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            bool res = m_motor.IsServoOn(axisIndex);

            return res;
        }

        public bool Jog(AxisProfile profile) {
            int ret = m_motor.JogMove(profile);
            if(ret != WMXParam.ErrorCode_None) {
                Alarm.AddWMX3ErrorCode(ret, Alarm.AlarmState.WMX3_Call_Error);
                return false;
            }

            return true;
        }

        public bool Jog(int axisCount, AxisProfile[] profiles) {
            int ret = m_motor.JogMove((uint)axisCount, profiles);
            if (ret != WMXParam.ErrorCode_None) {
                Alarm.AddWMX3ErrorCode(ret, Alarm.AlarmState.WMX3_Call_Error);
                return false;
            }

            return true;
        }

        public bool AbsoluteMove(AxisProfile profile) {
            int ret = m_motor.AbsoluteMove(profile);
            if(ret != WMXParam.ErrorCode_None) {
                Alarm.AddWMX3ErrorCode(ret, Alarm.AlarmState.WMX3_Call_Error);
                return false;
            }

            return true;
        }

        public bool AbsoluteMove(AxisProfile profile, TriggerCondition triggerCondition) {
            int ret = m_motor.AbsoluteMove(profile, triggerCondition);
            if(ret != WMXParam.ErrorCode_None) {
                Alarm.AddWMX3ErrorCode(ret, Alarm.AlarmState.WMX3_Call_Error);
                return false;
            }

            return true;
        }

        public bool RelativeMove(AxisProfile profile) {
            int ret = m_motor.RelativeMove(profile);
            if(ret != WMXParam.ErrorCode_None) {
                Alarm.AddWMX3ErrorCode(ret, Alarm.AlarmState.WMX3_Call_Error);
                return false;
            }
            return true;
        }

        public void Stop(RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            m_motor.StopServo(axisIndex);
        }

        public void EmergencyStop(RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            m_motor.EmergencyStop(axisIndex);
        }

        public bool HomeStart(RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            int ret = m_motor.HomeStart(axisIndex);
            if(ret != WMXParam.ErrorCode_None) {
                Alarm.AddWMX3ErrorCode(ret, Alarm.AlarmState.WMX3_Call_Error);
                return false;
            }
            return true;
        }

        public bool InterpolationMove(AxesProfile profile) {
            int ret = m_motor.AbsLinearInterpolation(profile);
            if(ret != WMXParam.ErrorCode_None) {
                Alarm.AddWMX3ErrorCode(ret, Alarm.AlarmState.WMX3_Call_Error);
                return false;
            }

            return true;
        }

        public void UpdateMotorStatus() {
            for(int i = 0; i < m_status.Length; i++) {
                m_status[i] = m_motor.m_axisStatus[i].Copy();
            }
        }

        public bool IsAllServoOn() {
            for(int i = 0; i < RMParam.GetAxisCount(); i++) {
                if (!m_status[i].m_servoOn) {
                    return false;
                }
            }

            return true;
        }

        public bool IsServoRun(RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            return m_status[axisIndex].m_servoRun;
        }

        public bool IsHoming(RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            return m_status[axisIndex].m_homing;
        }

        public bool GetSensorStatus(SensorType sensor, RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            switch (sensor) {
                case SensorType.Home:
                    return m_status[axisIndex].m_origin;

                case SensorType.Negative_Limit:
                    return m_status[axisIndex].m_nLimit;

                case SensorType.Positive_Limit:
                    return m_status[axisIndex].m_pLimit;

                case SensorType.SW_Negative_Limit:
                    return m_status[axisIndex].m_nLimitSoft;

                case SensorType.SW_Positive_Limit:
                    return m_status[axisIndex].m_pLimitSoft;
            }

            return false;
        }

        public double GetServoStatus(ServoStatusType status, RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            switch (status) {
                case ServoStatusType.pos_cmd:
                    return m_status[axisIndex].m_cmdPos;

                case ServoStatusType.pos_act:
                    return m_status[axisIndex].m_actualPos;

                case ServoStatusType.vel_cmd:
                    return m_status[axisIndex].m_cmdVelocity;

                case ServoStatusType.vel_act:
                    return m_status[axisIndex].m_actualVelocity;

                case ServoStatusType.trq_cmd:
                    return m_status[axisIndex].m_cmdTorque;

                case ServoStatusType.trq_act:
                    return m_status[axisIndex].m_actualTorque;

                case ServoStatusType.Profile_Traget_Pisition:
                    return m_status[axisIndex].m_profileTargetPos;
            }

            return 0;
        }

        public bool GetServoFlag(ServoFlagType flag, RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            switch (flag) {
                case ServoFlagType.Servo_On:
                    return m_status[axisIndex].m_servoOn;

                case ServoFlagType.Run:
                    return m_status[axisIndex].m_servoRun;

                case ServoFlagType.HomeDone:
                    return m_status[axisIndex].m_homeDone;

                case ServoFlagType.Alarm:
                    return m_status[axisIndex].m_alarm;

                case ServoFlagType.Waiting_Trigger:
                    return m_status[axisIndex].m_waitingTrigger;

                case ServoFlagType.Inposition:
                    return m_status[axisIndex].m_Inpos;

                case ServoFlagType.Poset:
                    return m_status[axisIndex].m_posSet;
            }

            return false;
        }

        public int GetAlarmCode(RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            return m_status[axisIndex].m_alarmCode;
        }

        public bool AllServoOn() {
            for(int i = 0; i < RMParam.GetAxisCount(); i++) {
                if (!ServoOn((RMParam.RMAxis)i)) {
                    return false;
                }
            }
            return true;
        }

        public void AllServoOff() {
            for (int i = 0; i < RMParam.GetAxisCount(); i++) {
                ServoOff((RMParam.RMAxis)i);
            }
        }

        public void AlarmClear(RMParam.RMAxis axis) {
            int axisIndex = RMParam.GetAxisNumber(axis);
            int ret = m_motor.AlarmClear(axisIndex);
            if(ret != WMXParam.ErrorCode_None) {
                Alarm.AddWMX3ErrorCode(ret, Alarm.AlarmState.WMX3_Call_Error);
            }
        }

        public void Overload_Alarm(RMParam.RMAxis axis) {
            if (axis == RMParam.RMAxis.X_Axis)
                Overload.m_alarmOverload_X = GetServoStatus(ServoStatusType.trq_act, axis);
            else if (axis == RMParam.RMAxis.Z_Axis)
                Overload.m_alarmOverload_Z = GetServoStatus(ServoStatusType.trq_act, axis);
            else if (axis == RMParam.RMAxis.A_Axis)
                Overload.m_alarmOverload_A = GetServoStatus(ServoStatusType.trq_act, axis);
            else if (axis == RMParam.RMAxis.T_Axis)
                Overload.m_alarmOverload_T = GetServoStatus(ServoStatusType.trq_act, axis);
        }

        public void ClearOverload_Alarm(RMParam.RMAxis axis) {
            if (axis == RMParam.RMAxis.X_Axis)
                Overload.m_alarmOverload_X = 0;
            else if (axis == RMParam.RMAxis.Z_Axis)
                Overload.m_alarmOverload_Z = 0;
            else if (axis == RMParam.RMAxis.A_Axis)
                Overload.m_alarmOverload_A = 0;
            else if (axis == RMParam.RMAxis.T_Axis)
                Overload.m_alarmOverload_T = 0;
        }

        public double GetOverload_Alarm(RMParam.RMAxis axis) {
            if (axis == RMParam.RMAxis.X_Axis)
                return Overload.m_alarmOverload_X;
            else if (axis == RMParam.RMAxis.Z_Axis)
                return Overload.m_alarmOverload_Z;
            else if (axis == RMParam.RMAxis.A_Axis)
                return Overload.m_alarmOverload_A;
            else if (axis == RMParam.RMAxis.T_Axis)
                return Overload.m_alarmOverload_T;

            return 0;
        }

        public void RefreshAxisParameter() {
            m_motor.GetAxisParam();
        }

        public double GetAxisParameter(RMParam.RMAxis axis, ParameterType type) {
            m_motor.GetAxisParam();
            int axisIndex = RMParam.GetAxisNumber(axis);
            switch (type) {
                case ParameterType.GearRatio_Numerator:
                    return m_motor.m_axisParameter[axisIndex].m_gearRatioNum;

                case ParameterType.GearRatio_Denominator:
                    return m_motor.m_axisParameter[axisIndex].m_gearRatioDen;

                case ParameterType.Software_Limit_Positive:
                    return m_motor.m_axisParameter[axisIndex].m_softLimitPosValue;

                case ParameterType.Software_Limit_Negative:
                    return m_motor.m_axisParameter[axisIndex].m_softLimitNegValue;
            }

            return 0;
        }

        public void SetAxisParameter(RMParam.RMAxis axis, ParameterType type, double value) {
            m_motor.GetAxisParam();
            int axisIndex = RMParam.GetAxisNumber(axis);
            switch (type) {
                case ParameterType.GearRatio_Numerator:
                    m_motor.m_axisParameter[axisIndex].m_gearRatioNum = value;
                    break;

                case ParameterType.GearRatio_Denominator:
                    m_motor.m_axisParameter[axisIndex].m_gearRatioDen = value;
                    break;

                case ParameterType.Software_Limit_Positive:
                    m_motor.m_axisParameter[axisIndex].m_softLimitPosValue = value;
                    break;

                case ParameterType.Software_Limit_Negative:
                    m_motor.m_axisParameter[axisIndex].m_softLimitNegValue = value;
                    break;
            }
            m_motor.SetAxisParam();
        }
    }
}
