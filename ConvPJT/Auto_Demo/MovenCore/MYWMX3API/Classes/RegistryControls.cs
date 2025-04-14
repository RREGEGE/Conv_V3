using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MYWMX3API.Classes
{
    static public class RegistryControls
    {
        static public class RTX
        {
            public static string GetRTXVersion()
            {
                RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\IntervalZero\RTX64", false);

                if (rkey != null)
                {
                    Object val = rkey.GetValue("ProductVersion");
                    if (val != null)
                    {
                        return Convert.ToString(val);
                    }
                }

                return string.Empty;
            }
        }

        static public class WMX
        {
            public static string GetWMXDirectoryPath()
            {
                RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SoftServo\WMX3", false);

                if (rkey != null)
                {
                    Object val = rkey.GetValue("WMX3Dir");
                    if (val != null)
                    {
                        return Convert.ToString(val);
                    }
                }

                return null;
            }

            public static string GetInstallProductVersion()
            {
                RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SoftServo\WMX3", false);

                if (rkey != null)
                {
                    Object val = rkey.GetValue("ProductVersion");
                    if (val != null)
                    {
                        return Convert.ToString(val);
                    }
                }

                return null;
            }

            public static string GetEtherCATDirectoryPath()
            {
                RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SoftServo\WMX3", false);

                if (rkey != null)
                {
                    Object val = rkey.GetValue("ECATDir");
                    if (val != null)
                    {
                        return Convert.ToString(val);
                    }
                }

                return null;
            }
            public static string GetCCLinkDirectoryPath()
            {
                RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SoftServo\WMX3\CCLink", false);

                if (rkey != null)
                {
                    Object val = rkey.GetValue("CCLinkDir");
                    if (val != null)
                    {
                        return Convert.ToString(val);
                    }
                }

                return null;
            }
            public static string GetM4DirectoryPath()
            {
                RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SoftServo\WMX3\M4", false);

                if (rkey != null)
                {
                    Object val = rkey.GetValue("M4Dir");
                    if (val != null)
                    {
                        return Convert.ToString(val);
                    }
                }

                return null;
            }
        }
        static public class Service
        {
            public enum ServiceStartMode
            {
                AUTO_DELAYEDSTART,
                AUTO,
                MANUAL,
                NOTUSED
            }
            public static bool IsServiceValid(string ServiceName)
            {
                RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + ServiceName, false);

                if (rkey != null)
                {
                    return true;
                }

                return false;
            }
            public static string GetServiceUtilityPath(string ServiceName)
            {
                RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + ServiceName, false);

                if (rkey != null)
                {
                    Object val = rkey.GetValue("ImagePath");
                    if (val != null)
                    {
                        return Convert.ToString(val);
                    }
                }

                return null;
            }

            public static string GetServiceStartType(string ServiceName)
            {
                RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + ServiceName, false);

                if (rkey != null)
                {
                    Object val = rkey.GetValue("Start");
                    if (val != null)
                    {
                        return DWORD_To_StartType(ServiceName, Convert.ToInt32(val));
                    }
                }

                return null;
            }
            public static bool SetServiceStartType(string ServiceName, ServiceStartMode serviceStartMode)
            {
                RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + ServiceName, true);

                if (rkey != null)
                {
                    try
                    {
                        switch (serviceStartMode)
                        {
                            case ServiceStartMode.AUTO_DELAYEDSTART:
                                {
                                    ulong DelayedAutostart = 60;
                                    ulong Start = 2;

                                    rkey.SetValue("Start", Start, RegistryValueKind.DWord);
                                    rkey.SetValue("DelayedAutostart", DelayedAutostart, RegistryValueKind.DWord);
                                }
                                break;
                            case ServiceStartMode.AUTO:
                                {
                                    ulong DelayedAutostart = 0;
                                    ulong Start = 2;

                                    rkey.SetValue("DelayedAutostart", DelayedAutostart, RegistryValueKind.DWord);
                                    rkey.SetValue("Start", Start, RegistryValueKind.DWord);
                                }
                                break;
                            case ServiceStartMode.MANUAL:
                                {
                                    ulong Start = 3;

                                    rkey.SetValue("Start", Start, RegistryValueKind.DWord);
                                }
                                break;
                            case ServiceStartMode.NOTUSED:
                                {
                                    ulong Start = 4;

                                    rkey.SetValue("Start", Start, RegistryValueKind.DWord);
                                }
                                break;
                            default:
                                return false;
                        }
                        return true;
                    }
                    catch { return false; }
                }

                return false;
            }
            public static string GetServiceDelayedAutoStart(string ServiceName)
            {
                RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + ServiceName, false);

                if (rkey != null)
                {
                    Object val = rkey.GetValue("DelayedAutostart");
                    if (val != null)
                    {
                        return Convert.ToString(val);
                    }
                }

                return null;
            }
            private static string DWORD_To_StartType(string ServiceName, int value)
            {
                switch (value)
                {
                    case 2:
                        {
                            string DelayedAutoStart = GetServiceDelayedAutoStart(ServiceName);
                            if (DelayedAutoStart != null)
                            {
                                if (DelayedAutoStart != "0")
                                    return "0";
                                else
                                    return "1";
                            }
                            else
                                return "1";
                        }
                    case 3:
                        return "2";
                    case 4:
                        return "3";
                    default:
                        return null;
                }

                return null;
            }
        }
    }
}
