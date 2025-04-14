using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RackMaster.SEQ.COMMON {
    public class RMParam {
        public enum ServoParam {
            Max_Speed,
            Max_Acc_Dec,
            Min_Acc_Dec,
            Max_Overload,
            JerkRatio,
            Soft_Limit_Positive,
            Soft_Limit_Negative,
            Auto_Speed_Percent,
            Home_Position_Range,
            Jog_HighSpeed_Limit,
            Jog_LowSpeed_Limit,
            Inching_Limit,
            Manual_High_Speed,
            Manual_High_Acc_Dec,
            Manual_Low_Speed,
            Manual_Low_Acc_Dec,
        }

        public enum Param {
            Z_Override_Percent,
            Z_Override_Down_Distance,
            Z_Override_Up_Distance,
            Turn_Left_Right_Degree,
            Arm_Home_Position,
            Use_Interpolation,
        }

        public enum RMAxis {
            X_Axis,
            Z_Axis,
            A_Axis,
            T_Axis,

            MAX_COUNT
        }

        public enum SyncTimeType {
            Year,
            Month,
            Day,
            Hour,
            Minute,
            Second,
        }

        private class Kinematics {
            public double RZ_LENGTH;
            public double RX_LENGTH;
            public double RY_LENGTH;

            public Kinematics() {
                RX_LENGTH = 220;
                RY_LENGTH = 220;
                RZ_LENGTH = 420;
            }
        }


        private class Servo {
            public int axisNumber;
            public int enable;
            public double maxSpeed;
            public double maxAccDec;
            public double minAccDec;
            public double maxOverload;
            public double jerkRatio;
            public double swPositiveLimit;
            public double swNegativeLimit;
            public double autoSpeedPercent;
            public double homePositionRange;
            public double jogHighSpeedLimit;
            public double jogLowSpeedLimit;
            public double inchingLimit;
            public double manualHighSpeed;
            public double manualHighAccDec;
            public double manualLowSpeed;
            public double manualLowAccDec;

            public Servo() {
                axisNumber = 0;
                enable = 0;
                maxSpeed = 0;
                maxAccDec = 0;
                maxOverload = 0;
                jerkRatio = 0;
                swPositiveLimit = 0;
                swNegativeLimit = 0;
                autoSpeedPercent = 0;
                homePositionRange = 1000000;
                jogHighSpeedLimit = 0;
                jogLowSpeedLimit = 0;
                inchingLimit = 0;
                manualHighSpeed = 0;
                manualHighAccDec = 0;
                manualLowSpeed = 0;
                manualLowAccDec = 0;
            }
        }

        private class GeneralParam {
            public int axisCount;
            public double Z_OverridePercent;
            public double Z_OverrideUpDist; // Z축 Up 할 때
            public double Z_OverrideDownDist;  // Z축 Down 할 때
            public double turnLeftRightPosition;
            public double armHomePosition;
            public bool useInterpolation;

            public GeneralParam() {
                axisCount = 4;
                Z_OverridePercent = 0.3;
                Z_OverrideUpDist = 5000;
                Z_OverrideDownDist = 10000;
                turnLeftRightPosition = 90000;
                armHomePosition = 0;
                useInterpolation = false;
            }
        }

        public static class StepTime {
            public const int CIM_TIMEOVER = 600000;
            public const int STEP_TEIMOVER = 700000;
            public const int PIO_READY_TIMOVER = 300000;
            public const int HOME_MOVE_TIMEOVER = 600000;
        }

        private static class SyncTime {
            public static short year = 0;
            public static short month = 0;
            public static short day = 0;
            public static short hour = 0;
            public static short minute = 0;
            public static short second = 0;
        }

        private static long m_syncTimeOffset = 0;

        private static Servo[] m_servoParam;
        private static GeneralParam m_generalParam;
        private static Kinematics m_kinematics;

        public static void Init() {
            m_generalParam = new GeneralParam();
            m_kinematics = new Kinematics();
            m_servoParam = new Servo[(int)RMAxis.MAX_COUNT];
            for(int i = 0; i < (int)RMAxis.MAX_COUNT; i++) {
                m_servoParam[i] = new Servo();
            }
        }


        public static int GetAxisNumber(RMAxis axis) {            
            return m_servoParam[(int)axis].axisNumber;
        }

        public static void SetAxisNumber(RMAxis axis, int number) {
            m_servoParam[(int)axis].axisNumber = number;
        }

        public static int GetAxisCount() {
            return m_generalParam.axisCount;
        }
        
        public static void SetUseInterpoation(bool value) {
            m_generalParam.useInterpolation = value;
        }

        public static bool GetUseInterpolation() {
            return m_generalParam.useInterpolation;
        }

        public static void SetSyncTimeData(SyncTimeType syncType, short value) {
            switch (syncType) {
                case SyncTimeType.Year:
                    SyncTime.year = value;
                    return;

                case SyncTimeType.Month:
                    SyncTime.month = value;
                    return;

                case SyncTimeType.Day:
                    SyncTime.day = value;
                    return;

                case SyncTimeType.Hour:
                    SyncTime.hour = value;
                    return;

                case SyncTimeType.Minute:
                    SyncTime.minute = value;
                    return;

                case SyncTimeType.Second:
                    SyncTime.second = value;
                    return;
            }
        }

        public static void UpdateSyncTime() {
            int yearOffset = 0;
            int monthOffset = 0;
            int dayOffset = 0;
            int hourOffset = 0;
            int minuteOffset = 0;
            int secondeOffset = 0;

            if (DateTime.Now.Year != SyncTime.year)
                yearOffset = SyncTime.year - DateTime.Now.Year;
            if (DateTime.Now.Month != SyncTime.month)
                monthOffset = SyncTime.month - DateTime.Now.Month;
            if (DateTime.Now.Day != SyncTime.day)
                dayOffset = SyncTime.day - DateTime.Now.Day;
            if (DateTime.Now.Hour != SyncTime.hour)
                hourOffset = SyncTime.hour - DateTime.Now.Hour;
            if (DateTime.Now.Minute != SyncTime.minute)
                minuteOffset = SyncTime.minute - DateTime.Now.Minute;
            if (DateTime.Now.Second != SyncTime.second)
                secondeOffset = SyncTime.second - DateTime.Now.Second;

            // 계산 다시...
            m_syncTimeOffset += yearOffset * 365 * 24 * 60 * 60;
            m_syncTimeOffset += monthOffset * 31 * 24 * 60 * 60;
            m_syncTimeOffset += dayOffset * 24 * 60 * 60;
            m_syncTimeOffset += hourOffset * 60 * 60;
            m_syncTimeOffset += minuteOffset * 60;
            m_syncTimeOffset += secondeOffset;
        }

        public static DateTime GetDateTimeNow() {
            return DateTime.Now.AddSeconds(m_syncTimeOffset);
        }

        public static void SetServoParam(ServoParam parameter, RMAxis axis, double value) {
            switch (parameter) {
                case ServoParam.Max_Speed:
                    m_servoParam[(int)axis].maxSpeed = value;
                    break;

                case ServoParam.Max_Acc_Dec:
                    m_servoParam[(int)axis].maxAccDec = value;
                    break;

                case ServoParam.Max_Overload:
                    m_servoParam[(int)axis].maxOverload = value;
                    break;

                case ServoParam.Min_Acc_Dec:
                    m_servoParam[(int)axis].minAccDec = value;
                    break;

                case ServoParam.JerkRatio:
                    m_servoParam[(int)axis].jerkRatio = value;
                    break;

                case ServoParam.Soft_Limit_Positive:
                    m_servoParam[(int)axis].swPositiveLimit = value;
                    break;

                case ServoParam.Soft_Limit_Negative:
                    m_servoParam[(int)axis].swNegativeLimit = value;
                    break;

                case ServoParam.Auto_Speed_Percent:
                    m_servoParam[(int)axis].autoSpeedPercent = value;
                    break;

                case ServoParam.Home_Position_Range:
                    m_servoParam[(int)axis].autoSpeedPercent = value;
                    break;

                case ServoParam.Jog_HighSpeed_Limit:
                    m_servoParam[(int)axis].jogHighSpeedLimit = value;
                    break;

                case ServoParam.Jog_LowSpeed_Limit:
                    m_servoParam[(int)axis].jogLowSpeedLimit = value;
                    break;

                case ServoParam.Inching_Limit:
                    m_servoParam[(int)axis].inchingLimit = value;
                    break;

                case ServoParam.Manual_High_Speed:
                    m_servoParam[(int)axis].manualHighSpeed = value;
                    break;

                case ServoParam.Manual_High_Acc_Dec:
                    m_servoParam[(int)axis].manualHighAccDec = value;
                    break;

                case ServoParam.Manual_Low_Speed:
                    m_servoParam[(int)axis].manualLowSpeed = value;
                    break;

                case ServoParam.Manual_Low_Acc_Dec:
                    m_servoParam[(int)axis].manualLowAccDec = value;
                    break;
            }
        }

        public static void SetParam(Param parameter, double value) {
            switch (parameter) {
                case Param.Z_Override_Percent:
                    m_generalParam.Z_OverridePercent = value;
                    break;

                case Param.Z_Override_Down_Distance:
                    m_generalParam.Z_OverrideDownDist = value;
                    break;

                case Param.Z_Override_Up_Distance:
                    m_generalParam.Z_OverrideUpDist = value;
                    break;

                case Param.Turn_Left_Right_Degree:
                    m_generalParam.turnLeftRightPosition = value;
                    break;

                case Param.Arm_Home_Position:
                    m_generalParam.armHomePosition = value;
                    break;
            }
        }

        public static double GetServoParam(ServoParam parameter, RMAxis axis) {
            switch (parameter) {
                case ServoParam.Max_Speed:
                    return m_servoParam[(int)axis].maxSpeed;

                case ServoParam.Max_Acc_Dec:
                    return m_servoParam[(int)axis].maxAccDec;

                case ServoParam.Max_Overload:
                    return m_servoParam[(int)axis].maxOverload;

                case ServoParam.Min_Acc_Dec:
                    return m_servoParam[(int)axis].minAccDec;

                case ServoParam.JerkRatio:
                    return m_servoParam[(int)axis].jerkRatio;

                case ServoParam.Soft_Limit_Positive:
                    return m_servoParam[(int)axis].swPositiveLimit;

                case ServoParam.Soft_Limit_Negative:
                    return m_servoParam[(int)axis].swNegativeLimit;

                case ServoParam.Auto_Speed_Percent:
                    return m_servoParam[(int)axis].autoSpeedPercent;

                case ServoParam.Home_Position_Range:
                    return m_servoParam[(int)axis].homePositionRange;

                case ServoParam.Jog_HighSpeed_Limit:
                    return m_servoParam[(int)axis].jogHighSpeedLimit;

                case ServoParam.Jog_LowSpeed_Limit:
                    return m_servoParam[(int)axis].jogLowSpeedLimit;

                case ServoParam.Inching_Limit:
                    return m_servoParam[(int)axis].inchingLimit;

                case ServoParam.Manual_High_Speed:
                    return m_servoParam[(int)axis].manualHighSpeed;

                case ServoParam.Manual_High_Acc_Dec:
                    return m_servoParam[(int)axis].manualHighAccDec;

                case ServoParam.Manual_Low_Speed:
                    return m_servoParam[(int)axis].manualLowSpeed;

                case ServoParam.Manual_Low_Acc_Dec:
                    return m_servoParam[(int)axis].manualLowAccDec;
            }

            return 0;
        }

        public static double GetParam(Param parameter) {
            switch (parameter) {
                case Param.Z_Override_Percent:
                    return m_generalParam.Z_OverridePercent;

                case Param.Z_Override_Down_Distance:
                    return m_generalParam.Z_OverrideDownDist;

                case Param.Z_Override_Up_Distance:
                    return m_generalParam.Z_OverrideUpDist;

                case Param.Turn_Left_Right_Degree:
                    return m_generalParam.turnLeftRightPosition;

                case Param.Arm_Home_Position:
                    return m_generalParam.armHomePosition;
            }

            return 0;
        }
    }
}