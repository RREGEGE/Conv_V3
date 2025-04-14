using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Master.Interface.MyFileIO;
using Master.Equipment.Port;
using Master.Equipment.Port.TagReader;

namespace Master.ManagedFile
{
    public static class EquipNetworkParam
    {
        // EquipNetworkParam은 ini File 구조로 저장됨
        // ini File은 아래와 같은 구조
        // 
        // [Section]
        // Key = Value
        // 



        /// <summary>
        /// ini에 저장되는 Section
        /// </summary>
        public enum Sections
        {
            CIM,
            RackMaster,
            Port,
            CPS
        }

        /// <summary>
        /// ini에 저장되는 CIM Section의 Key
        /// </summary>
        public enum CIMParameterKey
        {
            ServerPort,
            RecvBitMapStartAddr,
            RecvBitMapSize,
            SendBitMapStartAddr,
            SendBitMapSize,
            RecvWordMapStartAddr,
            RecvWordMapSize,
            SendWordMapStartAddr,
            SendWordMapSize,
            ALL
        }

        /// <summary>
        /// ini에 저장되는 RackMaster Section의 Key
        /// </summary>
        public enum RackMasterParameterKey
        {
            ID,
            ServerIP,
            ServerPort,
            RecvBitMapStartAddr,
            RecvBitMapSize,
            SendBitMapStartAddr,
            SendBitMapSize,
            RecvWordMapStartAddr,
            RecvWordMapSize,
            SendWordMapStartAddr,
            SendWordMapSize,
            ALL
        }

        /// <summary>
        /// ini에 저장되는 Port Section의 Key
        /// </summary>
        public enum PortParameterKey
        {
            ID,
            PortType,
            RecvBitMapStartAddr,
            RecvBitMapSize,
            SendBitMapStartAddr,
            SendBitMapSize,
            RecvWordMapStartAddr,
            RecvWordMapSize,
            SendWordMapStartAddr,
            SendWordMapSize,
            TagEquipType,
            TagEquipServerIP,
            TagEquipServerPort,
            ALL
        }

        /// <summary>
        /// ini에 저장되는 CPS Section의 Key
        /// </summary>
        public enum CPSParameterKey
        {
            ServerPort,
            PacketSize,
            ALL
        }

        /// <summary>
        /// Network 정보 관련 CIM, RackMaster, Port, CPS 파라미터 클래스
        /// </summary>
        public class CIMNetworkParam
        {
            public int ServerPort               = 50001;
            public int RecvBitMapStartAddr      = -1;
            public int RecvBitMapSize           = -1;
            public int SendBitMapStartAddr      = -1;
            public int SendBitMapSize           = -1;
            public int RecvWordMapStartAddr     = -1;
            public int RecvWordMapSize          = -1;
            public int SendWordMapStartAddr     = -1;
            public int SendWordMapSize          = -1;
        }
        public class RackMasterNetworkParam
        {
            public string ID                                = string.Empty;
            public string ServerIP                          = string.Empty;
            public int ServerPort                           = -1;
            public int RecvBitMapStartAddr                  = -1;
            public int RecvBitMapSize                       = -1;
            public int SendBitMapStartAddr                  = -1;
            public int SendBitMapSize                       = -1;
            public int RecvWordMapStartAddr                 = -1;
            public int RecvWordMapSize                      = -1;
            public int SendWordMapStartAddr                 = -1;
            public int SendWordMapSize                      = -1;
        }
        public class PortNetworkParam
        {
            public string ID                                = string.Empty;
            public Port.PortType ePortType                  = Port.PortType.MGV;
            public int RecvBitMapStartAddr                  = -1;
            public int RecvBitMapSize                       = -1;
            public int SendBitMapStartAddr                  = -1;
            public int SendBitMapSize                       = -1;
            public int RecvWordMapStartAddr                 = -1;
            public int RecvWordMapSize                      = -1;
            public int SendWordMapStartAddr                 = -1;
            public int SendWordMapSize                      = -1;
            public TagReaderType eTagReaderType             = TagReaderType.None;
            public string TagEquipServerIP                  = string.Empty;
            public int TagEquipServerPort                   = -1;
        }
        public class CPSNetworkParam
        {
            public int ServerPort = -1;
            public int PacketSize = 98;
        }

        const int PortNumMinRange = 0;
        const int PortNumMaxRange = 65535;

        static public CIMNetworkParam                              m_CIMNetworkParam = new CIMNetworkParam();
        static public CPSNetworkParam                              m_CPSNetworkParam = new CPSNetworkParam();
        static public Dictionary<int, RackMasterNetworkParam>      m_RackMasterNetworkParams = new Dictionary<int, RackMasterNetworkParam>();   //다수 스토커 대응을 위한 맵 구조
        static public Dictionary<int, PortNetworkParam>            m_PortNetworkParams = new Dictionary<int, PortNetworkParam>();               //다수 포트 대응을 위한 맵 구조

        static public bool LoadFile(string path, string fileName)
        {
            try
            {
                //1. 경로 유무 확인
                if (!Directory.Exists(path))
                {
                    //2. 경로가 없는 경우 폴더 생성
                    Directory.CreateDirectory(path);
                }

                //3. 파일 경로 지정
                string filePath = path + "\\" + fileName;

                //4. 파일 유무 확인
                if (File.Exists(filePath))
                {
                    //5.파일 있는 경우 읽기 진행
                    if (!ReadFile(filePath))
                        return false;
                }
                else
                {
                    //6. 파일 없는 경우 기본 값 파일 생성
                    SaveDefaultFile(filePath);
                }

                return true;
            }
            catch(Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"App NetParam Load Error");
                return false;
            }
        }
        static public bool SaveFile(string path, string fileName)
        {
            try
            {
                //1. 파일 경로 지정
                string filePath = path + "\\" + fileName;

                //2. 경로 유무 확인
                if (!Directory.Exists(path))
                {
                    //3. 경로가 없는 경우 폴더 생성
                    Directory.CreateDirectory(path);
                }

                //4. 파일 유무 확인
                if (File.Exists(filePath))
                {
                    //5. 파일 있는 경우 백업 진행
                    if (MyFile.BackupAndRemove(filePath))
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileBackupSuccess, $"AppNetParam");
                }

                //6. 파일 유무 확인
                if (File.Exists(filePath))
                {
                    //backup 과정에서 file 삭제되는 경우도 있으므로 재 검사
                    //File 있는 경우 히든 -> 아카이브로 변경 (편집 가능한 상태로 변환)
                    File.SetAttributes(filePath, File.GetAttributes(filePath) & FileAttributes.Archive);
                }

                //7. 현재 값 기준으로 Write 진행
                WriteValue(filePath, CIMParameterKey.ALL);

                WriteValue(filePath, CPSParameterKey.ALL);

                foreach (var rackMaster in m_RackMasterNetworkParams)
                    WriteValue(filePath, RackMasterParameterKey.ALL, rackMaster.Key);

                foreach (var port in m_PortNetworkParams)
                    WriteValue(filePath, PortParameterKey.ALL, port.Key);

                //8. 텍스트 재 정렬
                TextLineReArrage(filePath);

                //9. 파일 속성 히든으로 변경
                File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileSaveSuccess, $"AppNetParam");
                return true;
            }
            catch(Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"App Network Parameter Save Error");
                return false;
            }
        }
        static private bool ReadFile(string filePath)
        {
            try
            {
                string[] Sections = MyINI.GetSectionNames(filePath);

                if (Sections == null)
                {
                    SaveDefaultFile(filePath);
                }

                if (!Sections.Contains($"{EquipNetworkParam.Sections.CIM}"))
                    WriteValue(filePath, CIMParameterKey.ServerPort);

                if (!Sections.Contains($"{EquipNetworkParam.Sections.CPS}"))
                    WriteValue(filePath, CPSParameterKey.PacketSize);


                foreach (var Section in Sections)
                {
                    if (Section.Contains($"{EquipNetworkParam.Sections.CIM}"))
                    {
                        m_CIMNetworkParam.ServerPort = Convert.ToInt32(ReadValue(filePath, CIMParameterKey.ServerPort));
                        m_CIMNetworkParam.RecvBitMapStartAddr = Convert.ToInt32(ReadValue(filePath, CIMParameterKey.RecvBitMapStartAddr));
                        m_CIMNetworkParam.RecvBitMapSize = Convert.ToInt32(ReadValue(filePath, CIMParameterKey.RecvBitMapSize));
                        m_CIMNetworkParam.SendBitMapStartAddr = Convert.ToInt32(ReadValue(filePath, CIMParameterKey.SendBitMapStartAddr));
                        m_CIMNetworkParam.SendBitMapSize = Convert.ToInt32(ReadValue(filePath, CIMParameterKey.SendBitMapSize));
                        m_CIMNetworkParam.RecvWordMapStartAddr = Convert.ToInt32(ReadValue(filePath, CIMParameterKey.RecvWordMapStartAddr));
                        m_CIMNetworkParam.RecvWordMapSize = Convert.ToInt32(ReadValue(filePath, CIMParameterKey.RecvWordMapSize));
                        m_CIMNetworkParam.SendWordMapStartAddr = Convert.ToInt32(ReadValue(filePath, CIMParameterKey.SendWordMapStartAddr));
                        m_CIMNetworkParam.SendWordMapSize = Convert.ToInt32(ReadValue(filePath, CIMParameterKey.SendWordMapSize));
                    }
                    else if (Section.Contains($"{EquipNetworkParam.Sections.CPS}"))
                    {
                        m_CPSNetworkParam.ServerPort = Convert.ToInt32(ReadValue(filePath, CPSParameterKey.ServerPort));
                        m_CPSNetworkParam.PacketSize = Convert.ToInt32(ReadValue(filePath, CPSParameterKey.PacketSize));
                    }
                    else if (Section.Contains($"{EquipNetworkParam.Sections.RackMaster}"))
                    {
                        AddRackMasterParam(Section, filePath);
                    }
                    else if (Section.Contains($"{EquipNetworkParam.Sections.Port}"))
                    {
                        AddPortParam(Section, filePath);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        static private void TextLineReArrage(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                using (var reader = new StreamReader(stream))
                using (var writer = new StreamWriter(stream))
                {
                    string texts = reader.ReadToEnd();

                    string[] textArray = texts.Split(new[] { "\r\n" }, StringSplitOptions.None);

                    if (textArray.Length > 0)
                        stream.SetLength(0);

                    for (int nCount = 0; nCount < textArray.Length; nCount++)
                    {
                        if (nCount != 0 && textArray[nCount].Contains("[") && textArray[nCount].Contains("]"))
                        {
                            writer.WriteLine();
                            writer.WriteLine(textArray[nCount]);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(textArray[nCount]))
                                writer.WriteLine(textArray[nCount]);
                        }
                    }
                }
            }
        }
        static private void SaveDefaultFile(string filePath)
        {
            WriteValue(filePath, CIMParameterKey.ServerPort);
            WriteValue(filePath, CPSParameterKey.PacketSize);
        }
        static private void AddRackMasterParam(string section, string filePath)
        {
            int Index = GetIndex(section);

            if (Index == -1)
                return;

            if (m_RackMasterNetworkParams.ContainsKey(Index))
                return;

            RackMasterNetworkParam rackMasterParameter = new RackMasterNetworkParam();

            for(int nCount = 0; nCount < Enum.GetValues(typeof(RackMasterParameterKey)).Length; nCount++)
            {

                RackMasterParameterKey eRackMasterParameterKey = (RackMasterParameterKey)nCount;
                try
                {
                    switch (eRackMasterParameterKey)
                    {
                        case RackMasterParameterKey.ID:
                            rackMasterParameter.ID = Convert.ToString(ReadValue(filePath, eRackMasterParameterKey, Index));
                            break;
                        case RackMasterParameterKey.ServerIP:
                            rackMasterParameter.ServerIP = Convert.ToString(ReadValue(filePath, eRackMasterParameterKey, Index));
                            break;
                        case RackMasterParameterKey.ServerPort:
                            rackMasterParameter.ServerPort = Convert.ToInt32(ReadValue(filePath, eRackMasterParameterKey, Index));
                            break;

                        case RackMasterParameterKey.RecvBitMapStartAddr:
                            rackMasterParameter.RecvBitMapStartAddr = Convert.ToInt32(ReadValue(filePath, eRackMasterParameterKey, Index));
                            break;
                        case RackMasterParameterKey.RecvBitMapSize:
                            rackMasterParameter.RecvBitMapSize = Convert.ToInt32(ReadValue(filePath, eRackMasterParameterKey, Index));
                            break;

                        case RackMasterParameterKey.SendBitMapStartAddr:
                            rackMasterParameter.SendBitMapStartAddr = Convert.ToInt32(ReadValue(filePath, eRackMasterParameterKey, Index));
                            break;
                        case RackMasterParameterKey.SendBitMapSize:
                            rackMasterParameter.SendBitMapSize = Convert.ToInt32(ReadValue(filePath, eRackMasterParameterKey, Index));
                            break;

                        case RackMasterParameterKey.RecvWordMapStartAddr:
                            rackMasterParameter.RecvWordMapStartAddr = Convert.ToInt32(ReadValue(filePath, eRackMasterParameterKey, Index));
                            break;
                        case RackMasterParameterKey.RecvWordMapSize:
                            rackMasterParameter.RecvWordMapSize = Convert.ToInt32(ReadValue(filePath, eRackMasterParameterKey, Index));
                            break;

                        case RackMasterParameterKey.SendWordMapStartAddr:
                            rackMasterParameter.SendWordMapStartAddr = Convert.ToInt32(ReadValue(filePath, eRackMasterParameterKey, Index));
                            break;
                        case RackMasterParameterKey.SendWordMapSize:
                            rackMasterParameter.SendWordMapSize = Convert.ToInt32(ReadValue(filePath, eRackMasterParameterKey, Index));
                            break;
                    }
                }
                catch(Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"RackMaster NetParam[{eRackMasterParameterKey}] Read Error");

                    //Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.Equipment, Log.Equipment.RackMaster, rackMasterParameter.ID, $"Parameter File Read Error : Index = {Index}, {eRackMasterParameterKey} = {Convert.ToString(ReadValue(filePath, eRackMasterParameterKey, Index))}"));
                }
            }

            m_RackMasterNetworkParams.Add(Index, rackMasterParameter);
        }
        static private void AddPortParam(string section, string filePath)
        {
            int Index = GetIndex(section);

            if (Index == -1)
                return;

            if (m_PortNetworkParams.ContainsKey(Index))
                return;

            PortNetworkParam portParameter = new PortNetworkParam();

            for (int nCount = 0; nCount < Enum.GetValues(typeof(PortParameterKey)).Length; nCount++)
            {
                PortParameterKey ePortParameterKey = (PortParameterKey)nCount;

                try
                {
                    switch (ePortParameterKey)
                    {
                        case PortParameterKey.ID:
                            portParameter.ID = Convert.ToString(ReadValue(filePath, ePortParameterKey, Index));
                            break;
                        case PortParameterKey.PortType:
                            portParameter.ePortType = (Port.PortType)Enum.Parse(typeof(Port.PortType), Convert.ToString(ReadValue(filePath, ePortParameterKey, Index)));
                            break;

                        case PortParameterKey.RecvBitMapStartAddr:
                            portParameter.RecvBitMapStartAddr = Convert.ToInt32(ReadValue(filePath, ePortParameterKey, Index));
                            break;
                        case PortParameterKey.RecvBitMapSize:
                            portParameter.RecvBitMapSize = Convert.ToInt32(ReadValue(filePath, ePortParameterKey, Index));
                            break;

                        case PortParameterKey.SendBitMapStartAddr:
                            portParameter.SendBitMapStartAddr = Convert.ToInt32(ReadValue(filePath, ePortParameterKey, Index));
                            break;
                        case PortParameterKey.SendBitMapSize:
                            portParameter.SendBitMapSize = Convert.ToInt32(ReadValue(filePath, ePortParameterKey, Index));
                            break;

                        case PortParameterKey.RecvWordMapStartAddr:
                            portParameter.RecvWordMapStartAddr = Convert.ToInt32(ReadValue(filePath, ePortParameterKey, Index));
                            break;
                        case PortParameterKey.RecvWordMapSize:
                            portParameter.RecvWordMapSize = Convert.ToInt32(ReadValue(filePath, ePortParameterKey, Index));
                            break;

                        case PortParameterKey.SendWordMapStartAddr:
                            portParameter.SendWordMapStartAddr = Convert.ToInt32(ReadValue(filePath, ePortParameterKey, Index));
                            break;
                        case PortParameterKey.SendWordMapSize:
                            portParameter.SendWordMapSize = Convert.ToInt32(ReadValue(filePath, ePortParameterKey, Index));
                            break;
                        case PortParameterKey.TagEquipType:
                            portParameter.eTagReaderType = (TagReaderType)Enum.Parse(typeof(TagReaderType), Convert.ToString(ReadValue(filePath, ePortParameterKey, Index)));
                            break;
                        case PortParameterKey.TagEquipServerIP:
                            portParameter.TagEquipServerIP = Convert.ToString(ReadValue(filePath, ePortParameterKey, Index));
                            break;
                        case PortParameterKey.TagEquipServerPort:
                            portParameter.TagEquipServerPort = Convert.ToInt32(ReadValue(filePath, ePortParameterKey, Index));
                            break;
                    }
                }
                catch(Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port NetParam[{ePortParameterKey}] Read Error");

                    //Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.Equipment, Log.Equipment.Port, portParameter.ID, $"Parameter File Read Error : Index = {Index}, {ePortParameterKey} = {Convert.ToString(ReadValue(filePath, ePortParameterKey, Index))}"));
                }
            }

            m_PortNetworkParams.Add(Index, portParameter);
        }
        static private int GetIndex(string section)
        {
            if (section.Split('_').Length == 2)
            {
                try
                {
                    int Index = Convert.ToInt32(section.Split('_')[1]);
                    return Index;
                }
                catch
                {
                    return -1;
                }
            }

            return -1;
        }
        static public bool IsRMValidID(string ID)
        {
            if (string.IsNullOrEmpty(ID))
                return false;

            return true;
        }
        static public bool IsPortValidID(string ID)
        {
            if (string.IsNullOrEmpty(ID))
                return false;

            try
            {
                int nID = Convert.ToInt32(ID);

                if (nID >= 10000 && nID < 40000)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        static public bool IsValidIP(string IPAddress)
        {
            string[] IPNum = IPAddress.Split('.');
            if (IPNum.Length != 4)
                return false;

            try
            {
                for (int nCount = 0; nCount < IPNum.Length; nCount++)
                {
                    if (Convert.ToInt32(IPNum[nCount]) < 0 || Convert.ToInt32(IPNum[nCount]) > 255)
                        return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        static public bool IsValidPort(int portNum)
        {
            if (portNum < PortNumMinRange || portNum > PortNumMaxRange)
                return false;

            return true;
        }
        static public bool IsValidStartAddr(int startAddr)
        {
            if (startAddr < 0)
                return false;

            return true;
        }
        static public bool IsValidBitMapSize(int bitMapSize, int MinimumBitMapSize)
        {
            if (bitMapSize < MinimumBitMapSize)
                return false;

            return true;
        }
        static public bool IsValidWordMapSize(int wordMapSize, int MinimumBitMapSize)
        {
            if (wordMapSize < MinimumBitMapSize)
                return false;

            return true;
        }

        static public void WriteValue(string filePath, CIMParameterKey eCIMParameterKey)
        {
            switch (eCIMParameterKey)
            {
                case CIMParameterKey.ServerPort:
                    MyINI.Write($"{Sections.CIM}", $"{eCIMParameterKey}", $"{m_CIMNetworkParam.ServerPort}", filePath);
                    break;

                case CIMParameterKey.RecvBitMapStartAddr:
                    MyINI.Write($"{Sections.CIM}", $"{eCIMParameterKey}", $"{m_CIMNetworkParam.RecvBitMapStartAddr}", filePath);
                    break;
                case CIMParameterKey.RecvBitMapSize:
                    MyINI.Write($"{Sections.CIM}", $"{eCIMParameterKey}", $"{m_CIMNetworkParam.RecvBitMapSize}", filePath);
                    break;

                case CIMParameterKey.SendBitMapStartAddr:
                    MyINI.Write($"{Sections.CIM}", $"{eCIMParameterKey}", $"{m_CIMNetworkParam.SendBitMapStartAddr}", filePath);
                    break;
                case CIMParameterKey.SendBitMapSize:
                    MyINI.Write($"{Sections.CIM}", $"{eCIMParameterKey}", $"{m_CIMNetworkParam.SendBitMapSize}", filePath);
                    break;

                case CIMParameterKey.RecvWordMapStartAddr:
                    MyINI.Write($"{Sections.CIM}", $"{eCIMParameterKey}", $"{m_CIMNetworkParam.RecvWordMapStartAddr}", filePath);
                    break;
                case CIMParameterKey.RecvWordMapSize:
                    MyINI.Write($"{Sections.CIM}", $"{eCIMParameterKey}", $"{m_CIMNetworkParam.RecvWordMapSize}", filePath);
                    break;

                case CIMParameterKey.SendWordMapStartAddr:
                    MyINI.Write($"{Sections.CIM}", $"{eCIMParameterKey}", $"{m_CIMNetworkParam.SendWordMapStartAddr}\n", filePath);
                    break;
                case CIMParameterKey.SendWordMapSize:
                    MyINI.Write($"{Sections.CIM}", $"{eCIMParameterKey}", $"{m_CIMNetworkParam.SendWordMapSize}\n", filePath);
                    break;

                case CIMParameterKey.ALL:
                    for (int nCount = 0; nCount < (int)CIMParameterKey.ALL; nCount++)
                    {
                        CIMParameterKey eLoopKey = (CIMParameterKey)nCount;
                        WriteValue(filePath, eLoopKey);
                    }
                    break;
            }
        }
        static public void WriteValue(string filePath, CPSParameterKey eCPSParameterKey)
        {
            switch (eCPSParameterKey)
            {
                case CPSParameterKey.ServerPort:
                    MyINI.Write($"{Sections.CPS}", $"{eCPSParameterKey}", $"{m_CPSNetworkParam.ServerPort}", filePath);
                    break;
                case CPSParameterKey.PacketSize:
                    MyINI.Write($"{Sections.CPS}", $"{eCPSParameterKey}", $"{m_CPSNetworkParam.PacketSize}", filePath);
                    break;

                case CPSParameterKey.ALL:
                    for (int nCount = 0; nCount < (int)CPSParameterKey.ALL; nCount++)
                    {
                        CPSParameterKey eLoopKey = (CPSParameterKey)nCount;
                        WriteValue(filePath, eLoopKey);
                    }
                    break;
            }
        }
        static public void WriteValue(string filePath, RackMasterParameterKey eRackMasterParameterKey, int rackMasterIndex)
        {
            if (!m_RackMasterNetworkParams.ContainsKey(rackMasterIndex))
                return;

            switch (eRackMasterParameterKey)
            {
                case RackMasterParameterKey.ID:
                    MyINI.Write($"{Sections.RackMaster}_{rackMasterIndex}", $"{eRackMasterParameterKey}", $"{m_RackMasterNetworkParams[rackMasterIndex].ID}", filePath);
                    break;
                case RackMasterParameterKey.ServerIP:
                    MyINI.Write($"{Sections.RackMaster}_{rackMasterIndex}", $"{eRackMasterParameterKey}", $"{m_RackMasterNetworkParams[rackMasterIndex].ServerIP}", filePath);
                    break;
                case RackMasterParameterKey.ServerPort:
                    MyINI.Write($"{Sections.RackMaster}_{rackMasterIndex}", $"{eRackMasterParameterKey}", $"{m_RackMasterNetworkParams[rackMasterIndex].ServerPort}", filePath);
                    break;

                case RackMasterParameterKey.RecvBitMapStartAddr:
                    MyINI.Write($"{Sections.RackMaster}_{rackMasterIndex}", $"{eRackMasterParameterKey}", $"{m_RackMasterNetworkParams[rackMasterIndex].RecvBitMapStartAddr}", filePath);
                    break;
                case RackMasterParameterKey.RecvBitMapSize:
                    MyINI.Write($"{Sections.RackMaster}_{rackMasterIndex}", $"{eRackMasterParameterKey}", $"{m_RackMasterNetworkParams[rackMasterIndex].RecvBitMapSize}", filePath);
                    break;

                case RackMasterParameterKey.SendBitMapStartAddr:
                    MyINI.Write($"{Sections.RackMaster}_{rackMasterIndex}", $"{eRackMasterParameterKey}", $"{m_RackMasterNetworkParams[rackMasterIndex].SendBitMapStartAddr}", filePath);
                    break;
                case RackMasterParameterKey.SendBitMapSize:
                    MyINI.Write($"{Sections.RackMaster}_{rackMasterIndex}", $"{eRackMasterParameterKey}", $"{m_RackMasterNetworkParams[rackMasterIndex].SendBitMapSize}", filePath);
                    break;

                case RackMasterParameterKey.RecvWordMapStartAddr:
                    MyINI.Write($"{Sections.RackMaster}_{rackMasterIndex}", $"{eRackMasterParameterKey}", $"{m_RackMasterNetworkParams[rackMasterIndex].RecvWordMapStartAddr}", filePath);
                    break;
                case RackMasterParameterKey.RecvWordMapSize:
                    MyINI.Write($"{Sections.RackMaster}_{rackMasterIndex}", $"{eRackMasterParameterKey}", $"{m_RackMasterNetworkParams[rackMasterIndex].RecvWordMapSize}", filePath);
                    break;

                case RackMasterParameterKey.SendWordMapStartAddr:
                    MyINI.Write($"{Sections.RackMaster}_{rackMasterIndex}", $"{eRackMasterParameterKey}", $"{m_RackMasterNetworkParams[rackMasterIndex].SendWordMapStartAddr}\n", filePath);
                    break;
                case RackMasterParameterKey.SendWordMapSize:
                    MyINI.Write($"{Sections.RackMaster}_{rackMasterIndex}", $"{eRackMasterParameterKey}", $"{m_RackMasterNetworkParams[rackMasterIndex].SendWordMapSize}\n", filePath);
                    break;

                case RackMasterParameterKey.ALL:
                    for (int nCount = 0; nCount < (int)RackMasterParameterKey.ALL; nCount++)
                    {
                        RackMasterParameterKey eLoopKey = (RackMasterParameterKey)nCount;
                        WriteValue(filePath, eLoopKey, rackMasterIndex);
                    }
                    break;
            }
        }
        static public void WriteValue(string filePath, PortParameterKey ePortParameterKey, int portIndex)
        {
            if (!m_PortNetworkParams.ContainsKey(portIndex))
                return;

            switch (ePortParameterKey)
            {
                case PortParameterKey.ID:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].ID}", filePath);
                    break;
                case PortParameterKey.PortType:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].ePortType}", filePath);
                    break;

                case PortParameterKey.RecvBitMapStartAddr:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].RecvBitMapStartAddr}", filePath);
                    break;
                case PortParameterKey.RecvBitMapSize:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].RecvBitMapSize}", filePath);
                    break;

                case PortParameterKey.SendBitMapStartAddr:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].SendBitMapStartAddr}", filePath);
                    break;
                case PortParameterKey.SendBitMapSize:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].SendBitMapSize}", filePath);
                    break;

                case PortParameterKey.RecvWordMapStartAddr:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].RecvWordMapStartAddr}", filePath);
                    break;
                case PortParameterKey.RecvWordMapSize:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].RecvWordMapSize}", filePath);
                    break;

                case PortParameterKey.SendWordMapStartAddr:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].SendWordMapStartAddr}", filePath);
                    break;
                case PortParameterKey.SendWordMapSize:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].SendWordMapSize}", filePath);
                    break;

                case PortParameterKey.TagEquipType:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].eTagReaderType}", filePath);
                    break;
                case PortParameterKey.TagEquipServerIP:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].TagEquipServerIP}", filePath);
                    break;
                case PortParameterKey.TagEquipServerPort:
                    MyINI.Write($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", $"{m_PortNetworkParams[portIndex].TagEquipServerPort}", filePath);
                    break;

                case PortParameterKey.ALL:
                    for (int nCount = 0; nCount < (int)PortParameterKey.ALL; nCount++)
                    {
                        PortParameterKey eLoopKey = (PortParameterKey)nCount;
                        WriteValue(filePath, eLoopKey, portIndex);
                    }
                    break;
            }
        }

        static public object ReadValue(string filePath, CIMParameterKey eCIMParameterKey)
        {
            return MyINI.Read($"{Sections.CIM}", $"{eCIMParameterKey}", filePath).ToString();
        }
        static public object ReadValue(string filePath, CPSParameterKey eCPSParameterKey)
        {
            return MyINI.Read($"{Sections.CPS}", $"{eCPSParameterKey}", filePath).ToString();
        }
        static public object ReadValue(string filePath, RackMasterParameterKey eRackMasterParameterKey, int rackMasterIndex)
        {
            return MyINI.Read($"{Sections.RackMaster}_{rackMasterIndex}", $"{eRackMasterParameterKey}", filePath).ToString();
        }
        static public object ReadValue(string filePath, PortParameterKey ePortParameterKey, int portIndex)
        {
            return MyINI.Read($"{Sections.Port}_{portIndex}", $"{ePortParameterKey}", filePath).ToString();
        }
    }
}
