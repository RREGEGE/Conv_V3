using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;
using DWORD = System.Int32;

namespace Master.Interface.Processor
{
    /// <summary>
    /// Memory 사용량 관련 클래스
    /// </summary>
    class MemoryUsage
    {
        public enum MemoryUnit : int
        {
            Byte,
            KByte,
            MByte,
            GByte
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(out ulong MemoryInKilobytes);

        [DllImport("psapi.dll", SetLastError = true)]
        public static extern bool GetPerformanceInfo(out PERFORMANCE_INFORMATION pPerformanceInformation, uint cb);

        private static PerformanceCounter freeMemory;
        private static PerformanceCounter modifiedMemory;

        static private PerformanceCounter counter = new PerformanceCounter("Process", "Working Set - Private", Process.GetCurrentProcess().ProcessName);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct PERFORMANCE_INFORMATION
        {
            public uint cb;
            public ulong CommitTotal;
            public ulong CommitLimit;
            public ulong CommitPeak;
            public ulong PhysicalTotal;
            public ulong PhysicalAvailable;
            public ulong SystemCache;
            public ulong KernelTotal;
            public ulong KernelPaged;
            public ulong KernelNonpaged;
            public ulong PageSize;
            public DWORD HandleCount;
            public DWORD ProcessCount;
            public DWORD ThreadCount;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX buffer);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }

        public struct SystemMemory
        {
            public double InstalledRAMSize;
            public double InstalledRAMUsageSize;
            public double HWReservedMemory;
            public double UsingMemory;
            public double AvailableMemory;
            public double CommitedMemory;
            public double TotalPageFile;
            public double CashedMemory;
            public double PagedPool;
            public double NonPagedPool;
            public double CurrentProcessMemory;
        }

        static private double MemoryUnitSize(MemoryUnit eMemoryUnit)
        {
            switch(eMemoryUnit)
            {
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
        /// Installed RAM Size Return
        /// </summary>
        /// <returns></returns>
        static public double GetInstalledRAMSize(MemoryUnit eMemoryUnit)
        {
            ulong Mem;

            if(!GetPhysicallyInstalledSystemMemory(out Mem))
            {
                return 0;
            }
            Mem = Mem * 1024;
            return Mem / MemoryUnitSize(eMemoryUnit);
        }

        /// <summary>
        /// Installed RAM Usage
        /// </summary>
        /// <returns></returns>
        static public double GetInstalledRAMUsageSize(MemoryUnit eMemoryUnit)
        {
            PERFORMANCE_INFORMATION pi = new PERFORMANCE_INFORMATION();
            pi.cb = (uint)Marshal.SizeOf(pi);
            GetPerformanceInfo(out pi, pi.cb);
            ulong piTotal = pi.PhysicalTotal * pi.PageSize;

            return piTotal / MemoryUnitSize(eMemoryUnit);
        }

        /// <summary>
        /// Using Memory of Installed RAM Size
        /// </summary>
        /// <returns></returns>
        static public double GetUsingMemoryofInstalledSize(MemoryUnit eMemoryUnit)
        {
            modifiedMemory = new PerformanceCounter("Memory", "Modified Page List Bytes", true);
            PERFORMANCE_INFORMATION pi = new PERFORMANCE_INFORMATION();
            pi.cb = (uint)Marshal.SizeOf(pi);
            GetPerformanceInfo(out pi, pi.cb);

            ulong modified = (ulong)modifiedMemory.RawValue;
            ulong piAvailable = pi.PhysicalAvailable * pi.PageSize;
            ulong piTotal = pi.PhysicalTotal * pi.PageSize;
            ulong inuse = piTotal - piAvailable - modified;

            return inuse / MemoryUnitSize(eMemoryUnit);
        }

        /// <summary>
        /// Available Memory of Installed RAM Size
        /// </summary>
        /// <returns></returns>
        static public double GetAvailableMemoryofInstalledSize(MemoryUnit eMemoryUnit)
        {
            PERFORMANCE_INFORMATION pi = new PERFORMANCE_INFORMATION();
            pi.cb = (uint)Marshal.SizeOf(pi);
            GetPerformanceInfo(out pi, pi.cb);

            ulong piAvailable = pi.PhysicalAvailable * pi.PageSize;
            return piAvailable / MemoryUnitSize(eMemoryUnit);
        }

        /// <summary>
        /// Commited Memory of Installed RAM Size
        /// </summary>
        /// <returns></returns>
        static public double GetCommitedMemory(MemoryUnit eMemoryUnit)
        {
            PERFORMANCE_INFORMATION pi = new PERFORMANCE_INFORMATION();
            pi.cb = (uint)Marshal.SizeOf(pi);
            GetPerformanceInfo(out pi, pi.cb);
            return (pi.CommitTotal * pi.PageSize) / MemoryUnitSize(eMemoryUnit);
        }

        /// <summary>
        /// Total Page File
        /// </summary>
        /// <returns></returns>
        static public double GetTotalPageFile(MemoryUnit eMemoryUnit)
        {
            MEMORYSTATUSEX globalMemoryStatus = new MEMORYSTATUSEX();
            globalMemoryStatus.dwLength = (uint)Marshal.SizeOf(globalMemoryStatus);
            GlobalMemoryStatusEx(ref globalMemoryStatus);

            return globalMemoryStatus.ullTotalPageFile / MemoryUnitSize(eMemoryUnit);
        }

        /// <summary>
        /// Cached Memory
        /// </summary>
        /// <returns></returns>
        static public double GetCachedMemory(MemoryUnit eMemoryUnit)
        {
            freeMemory = new PerformanceCounter("Memory", "Free & Zero Page List Bytes", true);
            modifiedMemory = new PerformanceCounter("Memory", "Modified Page List Bytes", true);
            PERFORMANCE_INFORMATION pi = new PERFORMANCE_INFORMATION();
            pi.cb = (uint)Marshal.SizeOf(pi);
            GetPerformanceInfo(out pi, pi.cb);

            ulong piAvailable = pi.PhysicalAvailable * pi.PageSize;
            ulong modified = (ulong)modifiedMemory.RawValue;
            ulong free = (ulong)freeMemory.RawValue;
            ulong standby = piAvailable - free;
            return (standby + modified) / MemoryUnitSize(eMemoryUnit);
        }

        /// <summary>
        /// Paged Pool
        /// </summary>
        /// <returns></returns>
        static public double GetPagedPool(MemoryUnit eMemoryUnit)
        {
            PERFORMANCE_INFORMATION pi = new PERFORMANCE_INFORMATION();
            pi.cb = (uint)Marshal.SizeOf(pi);
            GetPerformanceInfo(out pi, pi.cb);

            return pi.KernelPaged * pi.PageSize / MemoryUnitSize(eMemoryUnit);
        }

        /// <summary>
        /// Non-Paged Pool
        /// </summary>
        /// <returns></returns>
        static public double GetNonPagedPool(MemoryUnit eMemoryUnit)
        {
            PERFORMANCE_INFORMATION pi = new PERFORMANCE_INFORMATION();
            pi.cb = (uint)Marshal.SizeOf(pi);
            GetPerformanceInfo(out pi, pi.cb);

            return (pi.KernelNonpaged * pi.PageSize) / MemoryUnitSize(eMemoryUnit);
        }

        /// <summary>
        /// Hardware Reserved Memory
        /// </summary>
        /// <returns></returns>
        static public double GetHardwareReserved(MemoryUnit eMemoryUnit)
        {
            PERFORMANCE_INFORMATION pi = new PERFORMANCE_INFORMATION();
            pi.cb = (uint)Marshal.SizeOf(pi);
            GetPerformanceInfo(out pi, pi.cb);

            ulong piTotal = pi.PhysicalTotal * pi.PageSize;
            GetPhysicallyInstalledSystemMemory(out ulong installedMemory);
            double reserved = (installedMemory * 1024) - piTotal;
            return reserved / MemoryUnitSize(eMemoryUnit);
        }
        
        /// <summary>
        /// Private Memory Usage
        /// </summary>
        /// <returns></returns>
        static public double GetMyProcessMemoryUsage(MemoryUnit eMemoryUnit)
        {
            //var counter = new PerformanceCounter("Process", "Working Set - Private", Process.GetCurrentProcess().ProcessName);

            return counter.RawValue / MemoryUnitSize(eMemoryUnit);
        }

        static public SystemMemory[] GetSystemMemory()
        {
            SystemMemory[] systemMemory = new SystemMemory[Enum.GetValues(typeof(MemoryUnit)).Length];
            for (int nCount = 0; nCount < systemMemory.Length; nCount++)
                systemMemory[nCount] = new SystemMemory();

            freeMemory = new PerformanceCounter("Memory", "Free & Zero Page List Bytes", true);
            modifiedMemory = new PerformanceCounter("Memory", "Modified Page List Bytes", true);

            //Byte
            PERFORMANCE_INFORMATION pi = new PERFORMANCE_INFORMATION();
            pi.cb = (uint)Marshal.SizeOf(pi);
            GetPerformanceInfo(out pi, pi.cb);

            ulong piAvailable = pi.PhysicalAvailable * pi.PageSize;
            ulong piTotal = pi.PhysicalTotal * pi.PageSize;

            GetPhysicallyInstalledSystemMemory(out ulong installedMemory);
            installedMemory = installedMemory * 1024;

            double HWReserved = installedMemory - piTotal;
            ulong modified = (ulong)modifiedMemory.RawValue;
            ulong inuse = piTotal - piAvailable - modified;
            ulong free = (ulong)freeMemory.RawValue;
            ulong standby = piAvailable - free;

            MEMORYSTATUSEX globalMemoryStatus = new MEMORYSTATUSEX();
            globalMemoryStatus.dwLength = (uint)Marshal.SizeOf(globalMemoryStatus);
            GlobalMemoryStatusEx(ref globalMemoryStatus);

            //var counter = new PerformanceCounter("Process", "Working Set - Private", Process.GetCurrentProcess().ProcessName);

            foreach (MemoryUnit eMemoryUnit in Enum.GetValues(typeof(MemoryUnit)))
            {
                systemMemory[(int)eMemoryUnit].InstalledRAMSize         = installedMemory / MemoryUnitSize(eMemoryUnit);
                systemMemory[(int)eMemoryUnit].InstalledRAMUsageSize    = piTotal / MemoryUnitSize(eMemoryUnit);
                systemMemory[(int)eMemoryUnit].HWReservedMemory         = HWReserved / MemoryUnitSize(eMemoryUnit);
                systemMemory[(int)eMemoryUnit].UsingMemory              = inuse / MemoryUnitSize(eMemoryUnit);
                systemMemory[(int)eMemoryUnit].AvailableMemory          = piAvailable / MemoryUnitSize(eMemoryUnit);
                systemMemory[(int)eMemoryUnit].CommitedMemory           = pi.CommitTotal * pi.PageSize / MemoryUnitSize(eMemoryUnit);
                systemMemory[(int)eMemoryUnit].TotalPageFile            = globalMemoryStatus.ullTotalPageFile / MemoryUnitSize(eMemoryUnit);
                systemMemory[(int)eMemoryUnit].CashedMemory             = (standby + modified) / MemoryUnitSize(eMemoryUnit);
                systemMemory[(int)eMemoryUnit].PagedPool                = pi.KernelPaged * pi.PageSize / MemoryUnitSize(eMemoryUnit);
                systemMemory[(int)eMemoryUnit].NonPagedPool             = pi.KernelNonpaged * pi.PageSize / MemoryUnitSize(eMemoryUnit);
                systemMemory[(int)eMemoryUnit].CurrentProcessMemory     = counter.RawValue / MemoryUnitSize(eMemoryUnit);
            }

            return systemMemory;
        }

        //static private PerformanceCounter counter = new PerformanceCounter("Process", "Working Set - Private", Process.GetCurrentProcess().ProcessName);

        static public SystemMemory GetSystemMemory(MemoryUsage.MemoryUnit eMemoryUnit)
        {
            SystemMemory systemMemory = new SystemMemory();

            freeMemory = new PerformanceCounter("Memory", "Free & Zero Page List Bytes", true);
            modifiedMemory = new PerformanceCounter("Memory", "Modified Page List Bytes", true);

            //Byte
            PERFORMANCE_INFORMATION pi = new PERFORMANCE_INFORMATION();
            pi.cb = (uint)Marshal.SizeOf(pi);
            GetPerformanceInfo(out pi, pi.cb);

            ulong piAvailable = pi.PhysicalAvailable * pi.PageSize;
            ulong piTotal = pi.PhysicalTotal * pi.PageSize;

            GetPhysicallyInstalledSystemMemory(out ulong installedMemory);
            installedMemory = installedMemory * 1024;

            double HWReserved = installedMemory - piTotal;
            ulong modified = (ulong)modifiedMemory.RawValue;
            ulong inuse = piTotal - piAvailable - modified;
            ulong free = (ulong)freeMemory.RawValue;
            ulong standby = piAvailable - free;

            MEMORYSTATUSEX globalMemoryStatus = new MEMORYSTATUSEX();
            globalMemoryStatus.dwLength = (uint)Marshal.SizeOf(globalMemoryStatus);
            GlobalMemoryStatusEx(ref globalMemoryStatus);

            //var counter = new PerformanceCounter("Process", "Working Set - Private", Process.GetCurrentProcess().ProcessName);

            systemMemory.InstalledRAMSize           = installedMemory / MemoryUnitSize(eMemoryUnit);
            systemMemory.InstalledRAMUsageSize      = piTotal / MemoryUnitSize(eMemoryUnit);
            systemMemory.HWReservedMemory           = HWReserved / MemoryUnitSize(eMemoryUnit);
            systemMemory.UsingMemory                = inuse / MemoryUnitSize(eMemoryUnit);
            systemMemory.AvailableMemory            = piAvailable / MemoryUnitSize(eMemoryUnit);
            systemMemory.CommitedMemory             = pi.CommitTotal * pi.PageSize / MemoryUnitSize(eMemoryUnit);
            systemMemory.TotalPageFile              = globalMemoryStatus.ullTotalPageFile / MemoryUnitSize(eMemoryUnit);
            systemMemory.CashedMemory               = (standby + modified) / MemoryUnitSize(eMemoryUnit);
            systemMemory.PagedPool                  = pi.KernelPaged * pi.PageSize / MemoryUnitSize(eMemoryUnit);
            systemMemory.NonPagedPool               = pi.KernelNonpaged * pi.PageSize / MemoryUnitSize(eMemoryUnit);
            systemMemory.CurrentProcessMemory       = counter.RawValue / MemoryUnitSize(eMemoryUnit);

            return systemMemory;
        }
    }
}
