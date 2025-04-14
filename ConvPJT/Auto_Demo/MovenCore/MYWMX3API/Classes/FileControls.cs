using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MYWMX3API.Classes
{
    static public class FileControls
    {
        static public class INIFile
        {
            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            [DllImport("kernel32.dll")]
            private static extern int GetPrivateProfileSection(string section, byte[] Keys, int nSize, string filePath);
            [DllImport("kernel32.dll")]
            private static extern uint GetPrivateProfileSectionNames(byte[] sections, uint size, String filePath);
            [DllImport("kernel32")] 
            private static extern int GetPrivateProfileString(string Section, int Key, string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result, int Size, string FileName);
            static public bool IsExist(string path)
            {
                if (File.Exists(path))
                    return true;

                return false;
            }

            static public void Write(string section, string key, string val, string path)
            {
                WritePrivateProfileString(section, key, val, path);
            }

            static public StringBuilder Read(string section, string key, string path)
            {
                StringBuilder str = new StringBuilder(4096);
                GetPrivateProfileString(section, key, "", str, str.Capacity, path);
                return str;
            }

            static public StringBuilder Read(string section, string key, string def, string path)
            {
                StringBuilder str = new StringBuilder(4096);
                GetPrivateProfileString(section, key, def, str, str.Capacity, path);
                return str;
            }

            static public string[] GetSectionNames(string _filePath)
            {
                byte[] bytes = new byte[4096];
                uint Flag = GetPrivateProfileSectionNames(bytes, 4096, _filePath);
                return Encoding.Default.GetString(bytes).Split(new char[1] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }
            static public string[] GetEntryNames(string section, string _filePath)
            {
                byte[] bytes = new byte[4096];
                GetPrivateProfileSection(section, bytes, 4096, _filePath);

                return Encoding.Default.GetString(bytes).Split(new char[1] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }

            static public void DelSection(string Section, string avsPath)
            {
                string Key = null;
                string Value = null;
                WritePrivateProfileString(Section, Key, Value, avsPath);
            }

            static public void DelKey(string Section, string Key, string avsPath)
            {
                string Value = null;
                WritePrivateProfileString(Section, Key, Value, avsPath);
            }
            static public void WriteKeyValue(string _key, string _value, string _filePath)
            {
                //section이 없는 경우
                try
                {
                    using (var stream = new FileStream(_filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        using (var reader = new StreamReader(stream))
                        using (var writer = new StreamWriter(stream))
                        {
                            string texts = reader.ReadToEnd();

                            string[] textArray = texts.Split(new[] { "\r\n" }, StringSplitOptions.None);

                            if (textArray.Length > 0)
                                stream.SetLength(0);

                            bool bKeyContain = false;

                            for (int nCount = 0; nCount < textArray.Length; nCount++)
                            {
                                if (textArray[nCount].Contains(_key))
                                {
                                    bKeyContain = true;
                                    break;
                                }
                            }

                            if (bKeyContain == false)
                                writer.WriteLine(string.Format("{0}={1}", _key, _value));

                            for (int nCount = 0; nCount < textArray.Length; nCount++)
                            {
                                if (bKeyContain == true)
                                {
                                    if (textArray[nCount].Contains(_key))
                                    {
                                        textArray[nCount] = string.Format("{0}={1}", _key, _value);
                                    }
                                }
                                writer.WriteLine(textArray[nCount]);
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            }

            static public string ReadKeyValue(string _key, string _filePath)
            {
                //section이 없는 경우
                try
                {
                    using (var reader = new StreamReader(_filePath))
                    {
                        string texts = reader.ReadToEnd();
                        string[] textArray = texts.Split(new[] { "\r\n" }, StringSplitOptions.None);

                        for (int nCount = 0; nCount < textArray.Length; nCount++)
                        {
                            if (textArray[nCount].Contains(_key))
                            {
                                string[] strLine = textArray[nCount].Split('=');

                                if(strLine.Length > 0)
                                {
                                    return strLine[strLine.Length - 1];
                                }
                            }
                        }

                        return string.Empty;
                    }
                }
                catch (Exception ex) { return string.Empty; }
            }
        }

        static public class CSVFile
        {

        }

        static public class XMLFile
        {

        }

        static public class BATFile
        {
            static public void InstallCreateFile(string ServiceName, string path)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(path);
                    if(sw != null)
                    {
                        sw.WriteLine("sc " + "create " + ServiceName + " binPath=" + "\"%~dp0"+ ServiceName + ".exe\"");
                        sw.Close();
                    }
                }
                catch { }
            }
            static public void UnInstallCreateFile(string ServiceName, string path)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(path);
                    if (sw != null)
                    {
                        sw.WriteLine("sc " + "delete " + ServiceName);
                        sw.Close();
                    }
                }
                catch { }
            }
            static public String ReadAll(string path)
            {
                try
                {
                    StreamReader sr = new StreamReader(path);
                    if (sr != null)
                    {
                        string text = sr.ReadToEnd();
                        sr.Close();
                        return text;
                    }
                }
                catch {
                    return null;
                }

                return null;
            }
        }

        static public class Folder
        {
            static public bool IsExist(string path)
            {
                if (Directory.Exists(path))
                    return true;

                return false;
            }

            static public void CreateFolder(string path)
            {
                Directory.CreateDirectory(path);
            }
        }

        static public class DefCtrl
        {
            static public void WriteSectionKeyValue(string _section, string _key, string _value, string _filePath)
            {
                try
                {
                    using (var stream = new FileStream(_filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        using (var reader = new StreamReader(stream))
                        using (var writer = new StreamWriter(stream))
                        {
                            string texts = reader.ReadToEnd();

                            string[] textArray = texts.Split(new[] {"\r\n"}, StringSplitOptions.None);

                            if (textArray.Length > 0)
                                stream.SetLength(0);

                            for (int nCount = 0; nCount < textArray.Length; nCount++)
                            {
                                if (textArray[nCount].Contains(_section))
                                {
                                    if (nCount + 1 < textArray.Length)
                                    {
                                        textArray[nCount + 1] = string.Format("{0}={1}", _key, _value);
                                    }
                                }
                                writer.WriteLine(textArray[nCount]);
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            }
            static public void WriteKeyValue(string _key, string _value, string _filePath)
            {
                try
                {
                    using (var stream = new FileStream(_filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        using (var reader = new StreamReader(stream))
                        using (var writer = new StreamWriter(stream))
                        {
                            string texts = reader.ReadToEnd();

                            string[] textArray = texts.Split(new[] { "\r\n" }, StringSplitOptions.None);

                            if (textArray.Length > 0)
                                stream.SetLength(0);

                            bool bKeyContain = false;

                            for (int nCount = 0; nCount < textArray.Length; nCount++)
                            {
                                if (textArray[nCount].Contains(_key))
                                {
                                    bKeyContain = true;
                                    break;
                                }
                            }

                            if (bKeyContain == false)
                                writer.WriteLine(string.Format("{0}={1}", _key, _value));

                            for (int nCount = 0; nCount < textArray.Length; nCount++)
                            {
                                if (bKeyContain == true)
                                {
                                    if (textArray[nCount].Contains(_key))
                                    {
                                        textArray[nCount] = string.Format("{0}={1}", _key, _value);
                                    }
                                }
                                writer.WriteLine(textArray[nCount]);
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            }
        }

        static public class StringKeyCtrl
        {
            static public void WriteKeyValue(string _key, string _writeData, string _filePath)
            {
                var lines = File.ReadAllLines(_filePath);
                var pos = Array.FindIndex(lines, row => row.Contains(_key));
                if (pos >= 0)
                {
                    string data = "";
                    string[] stringSeparators = new string[] { "=" };
                    string[] lineArr = lines[pos].Split(stringSeparators, StringSplitOptions.None);
                    lineArr[1] = _writeData;
                    for (int i = 0; i < lineArr.Count(); i++)
                    {
                        data += lineArr[i];
                        if (i != lineArr.Count() - 1) data += "=";
                    }
                    lines[pos] = data;
                    File.WriteAllLines(_filePath, lines);
                }
                else
                {
                    StreamWriter sw = File.AppendText(_filePath);
                    sw.WriteLine(_key + "=" + _writeData);
                    sw.Close();
                }
            }

            static public void WriteStrLine(string _strLine, string _filePath)
            {
                StreamWriter sw = File.AppendText(_filePath);
                sw.WriteLine(_strLine);
                sw.Close();
            }

            static public string ReadKeyValue(string _key, string _defValue, string _filePath)
            {
                var lines = File.ReadAllLines(_filePath);
                var pos = Array.FindIndex(lines, row => row.Contains(_key));
                if (pos < 0) return _defValue;

                string[] stringSeparators = new string[] { "=" };
                string[] lineArr = lines[pos].Split(stringSeparators, StringSplitOptions.None);

                return lineArr[1].Trim();
            }

            static public bool CheckKeyExist(string _key, string _filePath)
            {
                var lines = File.ReadAllLines(_filePath);
                var pos = Array.FindIndex(lines, row => row.Contains(_key));

                if (pos < 0) return false;
                return true;
            }

            static public void DelKey(string _key, string _filePath)
            {
                var lines = File.ReadAllLines(_filePath);
                var pos = Array.FindIndex(lines, row => row.Contains(_key));
                if (pos > 0)
                {
                    List<string> listStr = new List<string>(lines);
                    listStr.RemoveAt((int)pos);
                    File.WriteAllLines(_filePath, listStr);
                }
            }
        }

        public class RtxTcpipInICtrl
        {
            string rtxTcpipInIPath = string.Empty;

            string ecPlatformPath = @"\Platform\EtherCAT\";
            string cclinkPlatformPath = @"\Platform\CCLink\";
            string mecha4PlatformPath = @"\Platform\M4\";

            string[] iniStrKey = new string[(int)eStrKey.EndOfIndex];

            //public uint timerPriority;
            public uint numOfInterfaces;
            private string lastErrorString;

            ePlatformType platformType;

            const string defTimerPri = "121";
            const string defInterupPri = "126";
            const string defRecvPri = "124";

            public enum eDriverType
            {
                None = 0,
                RtIGB,
                RtIPCH,
                RtE1000,
                Rt8257X,
                Rt82580,
                RtRTL8168
            }

            public enum ePlatformType
            {
                EtherCAT = 0,
                CCLink_IE_TSN,
                Mechatrolink4
            }

            public enum eStrKey
            {
                //RtxTcpIp.ini - TCP/IP Section
                TimerPriority = 0,
                NumOfInterfaces,

                //RtxTcpIp.ini - rtnd0 or 1 Section
                Driver,
                InterruptPriority,
                ReceivePriority,
                Location,
                LinkStatus,

                EndOfIndex
            }

            public RtxTcpipInICtrl(ePlatformType _platformType)
            {
                platformType = _platformType;
                InitializeClass();
            }

            public void InitializeClass()
            {
                //timerPriority = defTimerPri;
                numOfInterfaces = 1;
                InitInIStrKey();

                if (platformType == ePlatformType.EtherCAT)
                    rtxTcpipInIPath = MYWMX3API.WMXLib.Datas.Engine.WMXInstallDirectory + ecPlatformPath + "RtxTcpIp.ini";
                else if (platformType == ePlatformType.CCLink_IE_TSN)
                    rtxTcpipInIPath = MYWMX3API.WMXLib.Datas.Engine.WMXInstallDirectory + cclinkPlatformPath + "RtxTcpIp.ini";
                else if (platformType == ePlatformType.Mechatrolink4)
                    rtxTcpipInIPath = MYWMX3API.WMXLib.Datas.Engine.WMXInstallDirectory + mecha4PlatformPath + "RtxTcpIp.ini";
            }

            private void InitInIStrKey()
            {
                iniStrKey = Enum.GetNames(typeof(eStrKey));
            }

            public bool CheckForSection(string checkSectionName)
            {
                string[] sectionNames = INIFile.GetSectionNames(rtxTcpipInIPath);

                int sectionCount = sectionNames.Count();
                for (int i = 0; i < sectionCount; i++)
                {
                    if (sectionNames[i] == checkSectionName)
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool WriteTcpIpSection(string timerPriority, string numOfInterfaces)
            {
                if (!File.Exists(rtxTcpipInIPath))
                {
                    lastErrorString = string.Format("RtxTcpIp.ini file is not exist.(Platform:{0})", platformType.ToString());
                    return false;
                }

                string tcpIpSection = "TCP/IP";
                try
                {
                    if (File.Exists(rtxTcpipInIPath))
                        File.Copy(rtxTcpipInIPath, rtxTcpipInIPath + ".bak", true);

                    INIFile.Write(tcpIpSection, iniStrKey[(int)eStrKey.TimerPriority], timerPriority, rtxTcpipInIPath);
                    INIFile.Write(tcpIpSection, iniStrKey[(int)eStrKey.NumOfInterfaces], numOfInterfaces, rtxTcpipInIPath);
                }
                catch (Exception ex)
                {
                    lastErrorString = ex.Message;
                    return false;
                }

                return true;
            }

            public bool ReadTcpIpSection(ref string timerPriority, ref string numOfInterfaces)
            {
                if (!File.Exists(rtxTcpipInIPath))
                {
                    lastErrorString = string.Format("RtxTcpIp.ini file is not exist.(Platform:{0})", platformType.ToString());
                    return false;
                }

                string tcpIpSection = "TCP/IP";
                try
                {
                    timerPriority = INIFile.Read(tcpIpSection, iniStrKey[(int)eStrKey.TimerPriority], defTimerPri, rtxTcpipInIPath).ToString();
                    numOfInterfaces = INIFile.Read(tcpIpSection, iniStrKey[(int)eStrKey.NumOfInterfaces], "1", rtxTcpipInIPath).ToString();
                }
                catch (Exception ex)
                {
                    lastErrorString = ex.Message;
                    return false;
                }

                return true;
            }

            public bool WriteRtndSection(int rtndNo, string driverName, string interruptPri,
                                         string receivePri, string location, bool linkStatus)
            {
                if (!File.Exists(rtxTcpipInIPath))
                {
                    lastErrorString = string.Format("RtxTcpIp.ini file is not exist.(Platform:{0})", platformType.ToString());
                    return false;
                }

                string rtndSection = "rtnd" + rtndNo.ToString();
                try
                {
                    INIFile.Write(rtndSection, iniStrKey[(int)eStrKey.Driver], driverName, rtxTcpipInIPath);
                    INIFile.Write(rtndSection, iniStrKey[(int)eStrKey.InterruptPriority], interruptPri, rtxTcpipInIPath);
                    INIFile.Write(rtndSection, iniStrKey[(int)eStrKey.ReceivePriority], receivePri, rtxTcpipInIPath);
                    INIFile.Write(rtndSection, iniStrKey[(int)eStrKey.Location], location, rtxTcpipInIPath);

                    if (linkStatus)
                        INIFile.Write(rtndSection, iniStrKey[(int)eStrKey.LinkStatus], "1", rtxTcpipInIPath);
                    else
                        INIFile.DelKey(rtndSection, iniStrKey[(int)eStrKey.LinkStatus], rtxTcpipInIPath);
                }
                catch (Exception ex)
                {
                    lastErrorString = ex.Message;
                    return false;
                }
                return true;
            }

            public bool ReadRtndSection(int rtndNo, ref string driverName, ref string interruptPri,
                                        ref string receivePri, ref string location, ref bool linkStatus)
            {
                if (!File.Exists(rtxTcpipInIPath))
                {
                    lastErrorString = string.Format("RtxTcpIp.ini file is not exist.(Platform:{0})", platformType.ToString());
                    return false;
                }

                string rtndSection = "rtnd" + rtndNo.ToString();
                try
                {
                    driverName = INIFile.Read(rtndSection, iniStrKey[(int)eStrKey.Driver], "", rtxTcpipInIPath).ToString();
                    interruptPri = INIFile.Read(rtndSection, iniStrKey[(int)eStrKey.InterruptPriority], defInterupPri, rtxTcpipInIPath).ToString();
                    receivePri = INIFile.Read(rtndSection, iniStrKey[(int)eStrKey.ReceivePriority], defRecvPri, rtxTcpipInIPath).ToString();
                    location = INIFile.Read(rtndSection, iniStrKey[(int)eStrKey.Location], "1;0;0", rtxTcpipInIPath).ToString();

                    string strKeyVal = INIFile.Read(rtndSection, iniStrKey[(int)eStrKey.LinkStatus], "0", rtxTcpipInIPath).ToString();
                    if (strKeyVal == "1")
                        linkStatus = true;
                    else
                        linkStatus = false;
                }
                catch (Exception ex)
                {
                    lastErrorString = ex.Message;
                    return false;
                }

                return true;
            }

            public void BackUpRtxTcpIpFile()
            {
                if (File.Exists(rtxTcpipInIPath))
                {
                    File.Copy(rtxTcpipInIPath, rtxTcpipInIPath + ".bak", true);
                    File.Delete(rtxTcpipInIPath);
                    Thread.Sleep(10);
                }
            }

            public bool GenerateRtxTcpIpFile(int nicDriveNum, string pciLocation, string driveType)
            {
                if (platformType == ePlatformType.EtherCAT)
                {
                    try
                    {
                        string tcpIpSection = "TCP/IP";
                        INIFile.Write(tcpIpSection, "MemoryInK", "1024", rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "TickInterval", "200", rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "MaxSockets", "30", rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "TimerPriority", defTimerPri, rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "NumStartupEvents", "20", rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "NumOfInterfaces", string.Format("{0}", nicDriveNum)
                                                                                    + Environment.NewLine, rtxTcpipInIPath);

                        string rtndSection = string.Format("rtnd{0}", nicDriveNum);
                        INIFile.Write(rtndSection, "Driver", driveType, rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "IPAddr", "192.168.4.8", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "Netmask", "255.255.0.0", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "InterruptPriority", defInterupPri, rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "ReceivePriority", defRecvPri, rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "NumRecvBuffers", "128", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "NumXmitBuffers", "128", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "LatencyRecvUpdated", "0", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "LineBasedOnly", "0", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "Location", pciLocation, rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "Filter", "EcFilter", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "LinkStatus", "1", rtxTcpipInIPath);
                        //if (nicDriveNum == 0)
                        //    INIFile.Write(rtndSection, "LinkStatus", "1" + Environment.NewLine, rtxTcpipInIPath);
                        //else
                        //    INIFile.Write(rtndSection, "LinkStatus", "1", rtxTcpipInIPath);
                    }
                    catch (Exception ex)
                    {
                        lastErrorString = ex.Message;
                        return false;
                    }
                }
                else if (platformType == ePlatformType.CCLink_IE_TSN)
                {
                    try
                    {
                        if (File.Exists(rtxTcpipInIPath))
                        {
                            File.Copy(rtxTcpipInIPath, rtxTcpipInIPath + ".bak", true);
                            File.Delete(rtxTcpipInIPath);
                            Thread.Sleep(10);
                        }
                        string tcpIpSection = "TCP/IP";
                        INIFile.Write(tcpIpSection, "MemoryInK", "1024", rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "TickInterval", "200", rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "MaxSockets", "30", rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "TimerPriority", defTimerPri, rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "NumStartupEvents", "20", rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "NumOfInterfaces", "1" + Environment.NewLine, rtxTcpipInIPath);

                        string rtndSection = "rtnd0";
                        INIFile.Write(rtndSection, "Driver", "RtIGB_TSN", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "InterruptPriority", defInterupPri, rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "ReceivePriority", defRecvPri, rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "NumRecvBuffers", "512", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "NumXmitBuffers", "512", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "LatencyRecvUpdated", "0", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "LineBasedOnly", "0", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "Location", pciLocation, rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "LinkStatus", "1", rtxTcpipInIPath);
                    }
                    catch (Exception ex)
                    {
                        lastErrorString = ex.Message;
                        return false;
                    }
                }
                else if (platformType == ePlatformType.Mechatrolink4)
                {
                    try
                    {
                        if (File.Exists(rtxTcpipInIPath))
                        {
                            File.Copy(rtxTcpipInIPath, rtxTcpipInIPath + ".bak", true);
                            File.Delete(rtxTcpipInIPath);
                            Thread.Sleep(10);
                        }
                        string tcpIpSection = "TCP/IP";
                        INIFile.Write(tcpIpSection, "MemoryInK", "1024", rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "TickInterval", "200", rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "MaxSockets", "30", rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "TimerPriority", defTimerPri, rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "NumStartupEvents", "20", rtxTcpipInIPath);
                        INIFile.Write(tcpIpSection, "NumOfInterfaces", "1" + Environment.NewLine, rtxTcpipInIPath);

                        string rtndSection = "rtnd0";
                        INIFile.Write(rtndSection, "Driver", driveType, rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "IPAddr", "192.168.4.8", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "Netmask", "255.255.0.0", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "InterruptPriority", defInterupPri, rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "ReceivePriority", defRecvPri, rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "LatencyRecvUpdated", "0", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "LineBasedOnly", "0", rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "Location", pciLocation, rtxTcpipInIPath);
                        INIFile.Write(rtndSection, "LinkStatus", "1", rtxTcpipInIPath);
                    }
                    catch (Exception ex)
                    {
                        lastErrorString = ex.Message;
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
