using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watchdog_Test;

namespace Watchdog_Test
{
    public static class MotionParam
    {
        public class LineParam
        {
            public string ID = null;
            public Line.LineDirection elineDirection = Line.LineDirection.Input;
            public Line.Auto eAuto = Line.Auto.Disable;
            public Conveyor_WatchdogParam WatchdogDetectParam = new Conveyor_WatchdogParam();
            public int ConvEA = 0;
            public int CSTEA = 0;
            public int AutoEA = 0;
            public int IdleEA = 0;
            public int ManualEA = 0;
            public int ErrorEA = 0;
            public int WarningEA = 0;
            public LineParam(string id)
            {
                this.ID = id;
            }
        }
        public class ConveyorParam
        {

        }
        public class Conveyor_WatchdogParam
        {
            public int T_Axis_HomingTimer = 1000;
            public int T_Axis_Move_To_LoadTimer = 1000;
            public int T_Axis_Move_To_UnLoadTimer = 1000;
            public int OHT_HoistDetectTimer = 1000;
            public int AGVorOHT_PIO_Timer = 1000;
            public int Init_Step_Timer = 1000;
            public int IN_Step_Timer = 1000;
            public int OUT_Step_Timer = 1000;
            public int Turn_Load_Timer = 1000;
            public int Turn_Unload_Timer = 1000;
        }
    }
}
