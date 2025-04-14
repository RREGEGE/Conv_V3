using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using WMX3ApiCLR;

namespace Conv_Step_Simulation
{
    public enum Mode
    {
        Manual,
        Auto,
        Alarm
    }
    public enum SensorOnOff
    {
        Off,
        On
    }
    public enum ServoOnOff
    {
        Off,
        On
    }
    public enum AlarmStatus
    {
        OK,
        NG
    }
    public enum InOutMode
    {
        InMode,
        OutMode
    }
    public enum Auto
    {
        Enable,
        Disable
    }
    internal class G_Var
    {
        // 파일 경로
        public static string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public static string lineParamFilePath = @".\Conv\Setting\LineParam.xml";
        public static string lineFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, lineParamFilePath));
        //public static string lineFullPath = @"C:\Users\240604\Desktop\Conv\Setting\LineParam.xml";

        public static string rectParamFilePath = @".\Conv\Setting\RectParam.xml";
        public static string rectFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, rectParamFilePath));
        //public static string rectFullPath = @"C:\Users\240604\Desktop\Conv\Setting\RectParam.xml";

        public static string convParamFilePath = @".\Conv\Setting\ConvParam.xml";
        public static string convFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, convParamFilePath));
        //public static string convFullPath = @"C:\Users\240604\Desktop\Conv\Setting\ConvParam.xml";

        public static string logFilePath = @".\Conv\Setting\LogParam.xml";
        public static string logFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, logFilePath));

        public static string profileFilePath = @".\Conv\Setting\Profile.xml";
        public static string profileFullPath = Path.GetFullPath(Path.Combine(solutionDirectory,profileFilePath));
        //public static string profileFullPath = @"C:\Users\240604\Desktop\Conv\Setting\Profile.xml";

        public static string paramFilePath = @".\Conv\Setting\wmx_parameters.xml";
        public static string paramFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, paramFilePath));
        //public static string paramFullPath = @"C:\Users\240604\Desktop\Conv\Setting\wmx_parameters.xml";

        public static string alarmFilePath = @".\Conv\Setting\AlarmListParam.xml";
        //public static string alarmFullPath = @"C:\Users\240604\Desktop\Conv\Setting\AlarmListParam.xml";
        public static string alarmFullPath = Path.GetFullPath(Path.Combine(solutionDirectory , alarmFilePath));

        public static string alarmHistoryFilePath = @".\Conv\Conv\AlarmHistory\AlarmHistory.xml";
        public static string alarmHistoryFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, alarmHistoryFilePath));
        //public static string alarmHistoryFullPath = @"C:\Users\240604\Desktop\Conv\AlarmHistory\AlarmHistory.xml";

        //WMX3
        public static WMXMotion w_motion;
        public static CoreMotionStatus cmStatus = new CoreMotionStatus();

        public static byte[] byInput = new byte[255];
        public static byte[] byOutput = new byte[255];

        // IO Group
        public enum MyInput
        {
            CST_Detect1 = 2,
            CST_Detect2,
        }
        public enum TurnInput
        {
            Home = 6,
            POS1 = 2,
            POS2,
        }
        public enum TurnInput2
        {
            POS3 = 6
        }
    }
    class StopWatchFunc
    {
        /// <summary>
        /// Stop Watch 경과 시간 출력
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        static public string GetRunningTime(Stopwatch st)
        {
            var RunningTime = st.Elapsed;

            return $"{RunningTime.TotalHours.ToString("00")}:{RunningTime.Minutes.ToString("00")}:{RunningTime.Seconds.ToString("00")}";
        }
    }
}