using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;

namespace RackMaster.SEQ.CLS {
    public static class CpuUsage {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetSystemTimes(out FILETIME lpIdleTime, out FILETIME lpKernelTime, out FILETIME lpUserTime);

        private static FILETIME _prevSysKernel;
        private static FILETIME _prevSysUser;
        private static FILETIME _prevSysIdle;

        private static TimeSpan _prevProcTotal = TimeSpan.MinValue;

        /// <summary>
        /// 현재 실행되고 있는 이 프로그램의 CPU 사용량을 반환하는 함수
        /// </summary>
        /// <returns></returns>
        public static float GetUsage() {
            FILETIME sysIdle, sysKernel, sysUser;
            float processCpuUsage = 0.0f;

            Process process = Process.GetCurrentProcess();
            TimeSpan procTime = process.TotalProcessorTime;

            if(!GetSystemTimes(out sysIdle, out sysKernel, out sysUser)) {
                return 0.0f;
            }

            if (_prevProcTotal != TimeSpan.MinValue) {
                ulong sysKernelDiff = SubtractTimes(sysKernel, _prevSysKernel);
                ulong sysUserDiff = SubtractTimes(sysUser, _prevSysUser);
                ulong sysIdleDiff = SubtractTimes(sysIdle, _prevSysIdle);

                ulong sysTotal = sysKernelDiff + sysUserDiff;
                long kernelTotal = (long)(sysKernelDiff - sysIdleDiff);

                if(kernelTotal < 0) {
                    kernelTotal = 0;
                }

                long procTotal = (procTime.Ticks - _prevProcTotal.Ticks);

                if(sysTotal > 0) {
                    processCpuUsage = (float)(100.0 * procTotal) / sysTotal;
                }
            }

            _prevProcTotal = procTime;
            _prevSysKernel = sysKernel;
            _prevSysUser = sysUser;
            _prevSysIdle = sysIdle;

            return processCpuUsage;
        }

        public static UInt64 SubtractTimes(FILETIME a, FILETIME b) {
            ulong aInt = ((ulong)a.dwHighDateTime << 32) | (uint)a.dwLowDateTime;
            ulong bInt = ((ulong)b.dwHighDateTime << 32) | (uint)b.dwLowDateTime;

            return aInt - bInt;
        }
    }

    public static class MemoryUsage {
        public enum MemoryUnit {
            Byte,
            KByte,
            MByte,
            GByte
        }

        private static PerformanceCounter counter;
        
        /// <summary>
        /// 메모리 타입에 따른 사이즈를 얻는 함수(byte)
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static double MemoryUnitSize(MemoryUnit unit) {
            switch (unit) {
                case MemoryUnit.Byte:
                    return 1.0;

                case MemoryUnit.KByte:
                    return 1.0 * 1024.0;

                case MemoryUnit.MByte:
                    return 1.0 * 1024.0 * 1024.0;

                case MemoryUnit.GByte:
                    return 1.0 * 1024.0 * 1024.0 * 1024.0;

                default:
                    return 1.0;
            }
        }
        /// <summary>
        /// 현재 실행되고 있는 이 프로그램의 메모리 사용량을 반환하는 함수
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static double GetMemoryUsage(MemoryUnit unit) {
            counter = new PerformanceCounter("Process", "Working Set - Private", Process.GetCurrentProcess().ProcessName);

            return counter.RawValue / MemoryUnitSize(unit);
        }
    }
}
