using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RackMaster.SEQ {
    public static class Global
    {
        public enum ForkType {
            Slide,
            Slide_NoTurn,
            SCARA,
        }

        public static bool IS_CONNECTED_TCP = false;
        public static bool IS_CONNECTED_ETHERCAT = false;
        public static ForkType FORK_TYPE = ForkType.SCARA;

        public static bool AUTO_STATE = false;
        public static bool MANUAL_STATE = true;
        public static bool ERROR_STATE = false;
        public static bool PIO_STATE = false;
        public static bool AUTO_TEACHING_STATE = false;

        public static bool CST_ON = false;

        public static bool MANUAL_RUN = false;

        // 임시 Password
        public const int PASSWORD = 5190;
    }
}
