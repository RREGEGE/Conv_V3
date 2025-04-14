using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SEQ.CLS {
    public class SystemTimeControl {
        public struct SystemTime {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int SetSystemTime([In] SystemTime st);

        /// <summary>
        /// 현재 시스템의 시간을 수정하는 함수
        /// </summary>
        /// <param name="sysTime"></param>
        /// <returns></returns>
        public static bool ModifySystemTime(SystemTime sysTime) {
            try {
                TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

                int localTimeOffset = (int)localTimeZone.BaseUtcOffset.TotalHours * (-1);

                DateTime dtTime = new DateTime(sysTime.wYear, sysTime.wMonth, sysTime.wDay, sysTime.wHour, sysTime.wMinute, sysTime.wSecond);

                dtTime = dtTime.AddHours(localTimeOffset);

                SystemTime setTime = new SystemTime();
                setTime.wYear   = (ushort)dtTime.Year;
                setTime.wMonth  = (ushort)dtTime.Month;
                setTime.wDay    = (ushort)dtTime.Day;
                setTime.wHour   = (ushort)dtTime.Hour;
                setTime.wMinute = (ushort)dtTime.Minute;
                setTime.wSecond = (ushort)dtTime.Second;

                if (SetSystemTime(setTime) != 0) {
                    return true;
                }
                else {
                    return false;
                }
            }catch(Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.Utility, $"Modify System Time Fail", ex));
                return false;
            }
        }
    }
}
