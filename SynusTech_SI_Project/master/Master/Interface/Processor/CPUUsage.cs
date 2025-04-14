using System;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;

namespace Master.Interface.Processor
{
    /// <summary>
    /// CPU 사용량 관련 클래스
    /// </summary>
    class CpuUsage
    {
        public enum CPUUsageType
        {
            Total,
            MyProcess
        }
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetSystemTimes(out FILETIME lpIdleTime,
                    out FILETIME lpKernelTime, out FILETIME lpUserTime);

        static FILETIME _prevSysKernel;
        static FILETIME _prevSysUser;
        static FILETIME _prevSysIdle;

        static TimeSpan _prevProcTotal = TimeSpan.MinValue;

        static public float[] GetCPUUsage()
        {
            float[] CPUUsage = new float[Enum.GetValues(typeof(CPUUsageType)).Length];

            CPUUsage[(int)CPUUsageType.Total] = 0.0f;
            CPUUsage[(int)CPUUsageType.MyProcess] = 0.0f;

            FILETIME sysIdle, sysKernel, sysUser;

            TimeSpan procTime = Process.GetCurrentProcess().TotalProcessorTime;

            if (!GetSystemTimes(out sysIdle, out sysKernel, out sysUser))
            {
                return CPUUsage;
            }

            if (_prevProcTotal != TimeSpan.MinValue)
            {
                ulong sysKernelDiff = SubtractTimes(sysKernel, _prevSysKernel);
                ulong sysUserDiff = SubtractTimes(sysUser, _prevSysUser);
                ulong sysIdleDiff = SubtractTimes(sysIdle, _prevSysIdle);

                //ulong sysTotal = sysKernelDiff + sysUserDiff;
                long kernelTotal = (long)(sysKernelDiff - sysIdleDiff);
                ulong sysTotal = sysKernelDiff + sysUserDiff - sysIdleDiff;

                if (kernelTotal < 0)
                {
                    kernelTotal = 0;
                }

                CPUUsage[(int)CPUUsageType.Total] = (float)((((ulong)kernelTotal + sysUserDiff) * 100.0) / sysTotal);
                long procTotal = (procTime.Ticks - _prevProcTotal.Ticks);
                if (sysTotal > 0)
                {
                    CPUUsage[(int)CPUUsageType.MyProcess] = (float)((100.0 * procTotal) / sysTotal);
                }
            }

            _prevProcTotal = procTime;
            _prevSysKernel = sysKernel;
            _prevSysUser = sysUser;
            _prevSysIdle = sysIdle;

            return CPUUsage;
        }

        static private UInt64 SubtractTimes(FILETIME a, FILETIME b)
        {
            ulong aInt = (ulong)((a.dwHighDateTime << 32) | a.dwLowDateTime);
            ulong bInt = (ulong)((b.dwHighDateTime << 32) | b.dwLowDateTime);

            return aInt - bInt;
        }
    }
}