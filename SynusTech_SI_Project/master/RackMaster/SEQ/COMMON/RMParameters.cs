using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RackMaster.SEQ.COMMON {
    public static class RMParameters {
        // Auto Speed에 대한 비율도 필요(%)
        // Z축 오버라이드에 대한 비율도 필요 -> 이 부분은 Z축의 Auto Speed에 대한 %
        // Z축 오버라이드 시점은 Z-Center 값의 +- 5mm로 설정
        // Slide Type도 설정해야함
        public static class Servo {
            public const double RZ_LENGTH = 420;
            public const double RX_LENGTH = 220;
            public const double RY_LENGTH = 220;

            public static int AXIS_NUMBER_X = 0;
            public static int AXIS_NUMBER_Z = 1;
            public static int AXIS_NUMBER_A = 2;
            public static int AXIS_NUMBER_T = 3;

            public static int ENABLED_X = 1;
            public static int ENABLED_Z = 1;
            public static int ENABLED_A = 1;
            public static int ENABLED_T = 1;

            public static int AXIS_COUNT = 4;

            public static double MAX_SPEED_X = 1333333; // 80m/min
            public static double MAX_SPEED_Z = 1000000; // 60m/min
            public static double MAX_SPEED_T = 65000; // 3900deg/min
            public static double MAX_SPEED_A = 65000; // 3900deg/min

            public static double MAX_ACC_DEC_X = 3.5; // 초 단위
            public static double MAX_ACC_DEC_Z = 4;
            public static double MAX_ACC_DEC_T = 1;
            public static double MAX_ACC_DEC_A = 1;

            public static double MAX_OVERLOAD_X = 100;
            public static double MAX_OVERLOAD_Z = 100;
            public static double MAX_OVERLOAD_A = 100;
            public static double MAX_OVERLOAD_T = 100;

            public static double JERK_RATIO_X = 0.7;
            public static double JERK_RATIO_Z = 0.7;
            public static double JERK_RATIO_T = 0.7;
            public static double JERK_RATIO_A = 0.7;

            public static double SW_PLS_X = 5054;
            public static double SW_PLS_Z = 3000;
            public static double SW_PLS_A = 90;
            public static double SW_PLS_T = 300;

            public static double SW_NLS_X = 0;
            public static double SW_NLS_Z = 0;
            public static double SW_NLS_A = 0;
            public static double SW_NLS_T = 0;

            public static double AUTO_SPEED_PERCENT = 0.3;
            public static double Z_OVERRIDE_SPEED_PERCENT = 0.3;

            public static double Z_OVERRIDE_DIST = 5000; // 5mm

            public static double HOME_POSITION_RANGE_X = 800000;
            public static double HOME_POSITION_RANGE_Z = 800000;
            public static double HOME_POSITION_RANGE_A = 800000;
            public static double HOME_POSITION_RANGE_T = 800000;

            public static double TURN_LEFT_RIGHT_POSITION = 90000;

            public static int FORK_AXIS_COUNT = 2;
        }

        public static class StepTime {
            public static int CIM_TIMEOVER = 10000;
            public static int HOME_MOVE_TIMEOVER = 120000;
        }
    }
}
