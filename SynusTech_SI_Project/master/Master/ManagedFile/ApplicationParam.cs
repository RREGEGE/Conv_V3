using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Master.Interface.MyFileIO;

namespace Master.ManagedFile
{
    public static class ApplicationParam
    {
        // ApplicationParam은 ini File 구조로 저장됨
        // ini File은 아래와 같은 구조
        // 자세한 내용은 EquipNetworkParam.cs 파일 참고
        // [Section]
        // Key = Value
        // 

        public enum Sections
        {
            Application,
            Log,
            PortParameterInterlock
        }

        public enum ApplicationParameterKey
        {
            LoginDuration,
            LangType
        }
        public enum LogParameterKey
        {
            LogCompressionCycle,
            ZIPDeleteCycle,

            LogShowType_Application,
            LogShowType_Exception,
            LogShowType_Port,
            LogShowType_RackMaster,
            LogShowType_CIM,
            LogShowType_Master,
            LogShowType_WMX,
            LogShowType_CPS,

            LogShowType_Normal,
            LogShowType_Warning,
            LogShowType_Error
        }
        public enum PortParameterInterlockKey
        {
            Port_X_Axis_Vel_Limit_mm,
            Port_X_Axis_Acc_Limit_mm,
            Port_X_Axis_Dec_Limit_mm,
            Port_X_Axis_Teaching_OP_Limit_mm,
            Port_X_Axis_Teaching_LP_Limit_mm,

            Port_Z_Axis_Vel_Limit_mm,
            Port_Z_Axis_Acc_Limit_mm,
            Port_Z_Axis_Dec_Limit_mm,
            Port_Z_Axis_Teaching_Up_Limit_mm,
            Port_Z_Axis_Teaching_Down_Limit_mm,

            Port_T_Axis_Vel_Limit_deg,
            Port_T_Axis_Acc_Limit_deg,
            Port_T_Axis_Dec_Limit_deg,
            Port_T_Axis_Teaching_180Deg_Limit_deg,
            Port_T_Axis_Teaching_0Deg_Limit_deg
        }

        /// <summary>
        /// 로그인 시 유지 시간 지정
        /// 언어 설정
        /// </summary>
        public class ApplicationParameter
        {
            public double LoginDuration = 60.0;
            public SynusLangPack.LanguageType eLangType = SynusLangPack.LanguageType.Korean;
        }

        /// <summary>
        /// 로그 압축, 삭제 주기 지정
        /// Log DataGridView에 출력될 레벨 및 타입 지정
        /// </summary>
        public class LogParameter
        {
            public int CompressionCycle = 1;
            public int ZIPDeleteCycle = 1;

            public bool LogShowType_Application = true;
            public bool LogShowType_Exception = true;
            public bool LogShowType_Port = true;
            public bool LogShowType_RackMaster = true;
            public bool LogShowType_CIM = true;
            public bool LogShowType_Master = true;
            public bool LogShowType_WMX = true;
            public bool LogShowType_CPS = true;

            public bool LogShowType_Normal = true;
            public bool LogShowType_Warning = true;
            public bool LogShowType_Error = true;
        }

        /// <summary>
        /// 포트 파라미터 설정 관련 인터락
        /// 해당 파라미터 값 범위 내에서 설정 진행 됨(파라미터 설정 진행 시 메세지 출력되며 안되는 경우 ApplicationParam 파일 열어 수정 진행해야 함
        /// </summary>
        public class PortParameterInterLock
        {
            //Default Value
            public double Port_X_Axis_Vel_Limit_mm              = 50.0;
            public double Port_X_Axis_Acc_Limit_mm              = 50.0;
            public double Port_X_Axis_Dec_Limit_mm              = 50.0;
            public double Port_X_Axis_Teaching_OP_Limit_mm      = 0.0;
            public double Port_X_Axis_Teaching_LP_Limit_mm      = 0.0;

            public double Port_Z_Axis_Vel_Limit_mm              = 15.0;
            public double Port_Z_Axis_Acc_Limit_mm              = 15.0;
            public double Port_Z_Axis_Dec_Limit_mm              = 15.0;
            public double Port_Z_Axis_Teaching_Up_Limit_mm      = 30.0;
            public double Port_Z_Axis_Teaching_Down_Limit_mm    = 0.0;

            public double Port_T_Axis_Vel_Limit_deg             = 3600.0;
            public double Port_T_Axis_Acc_Limit_deg             = 3600.0;
            public double Port_T_Axis_Dec_Limit_deg             = 3600.0;
            public double Port_T_Axis_Teaching_180Deg_Limit_deg = 180.0;
            public double Port_T_Axis_Teaching_0Deg_Limit_deg   = 0.0;
        }


        static public ApplicationParameter m_ApplicationParam = new ApplicationParameter();
        static public LogParameter m_LogParam = new LogParameter();
        static public PortParameterInterLock m_PortParameterInterLock = new PortParameterInterLock();

        /// <summary>
        /// EquipNetworkParam.cs 파일의 Load, Save 과정 참고
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public bool LoadFile(string path, string fileName)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = path + "\\" + fileName;

                if (File.Exists(filePath))
                {
                    if (!ReadFile(filePath))
                        return false;
                }
                else
                {
                    SaveDefaultFile(filePath);
                }


                return true;
            }
            catch
            {
                return false;
            }
        }
        static public bool SaveFile(string path, string fileName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = path + "\\" + fileName;

            if (File.Exists(filePath))
            {
                if (MyFile.BackupAndRemove(filePath))
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileBackupSuccess, $"AppParam");
            }

            if(File.Exists(filePath))
            {
                //backup 과정에서 file 삭제되는 경우도 있으므로 재 검사
                File.SetAttributes(filePath, File.GetAttributes(filePath) & FileAttributes.Archive);
            }

            foreach(ApplicationParameterKey key in Enum.GetValues(typeof(ApplicationParameterKey)))
                WriteValue(filePath, key);

            foreach (LogParameterKey key in Enum.GetValues(typeof(LogParameterKey)))
                WriteValue(filePath, key);

            foreach (PortParameterInterlockKey key in Enum.GetValues(typeof(PortParameterInterlockKey)))
                WriteValue(filePath, key);

            File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileSaveSuccess, $"AppParam");
            return true;
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

                if (!Sections.Contains($"{ApplicationParam.Sections.Application}"))
                {
                    foreach (ApplicationParameterKey key in Enum.GetValues(typeof(ApplicationParameterKey)))
                        WriteValue(filePath, key);
                }

                if (!Sections.Contains($"{ApplicationParam.Sections.Log}"))
                {
                    foreach (LogParameterKey key in Enum.GetValues(typeof(LogParameterKey)))
                        WriteValue(filePath, key);
                }

                if (!Sections.Contains($"{ApplicationParam.Sections.PortParameterInterlock}"))
                {
                    foreach (PortParameterInterlockKey key in Enum.GetValues(typeof(PortParameterInterlockKey)))
                        WriteValue(filePath, key);
                }



                foreach (var Section in Sections)
                {
                    if (Section.Contains($"{ApplicationParam.Sections.Application}"))
                    {
                        m_ApplicationParam.LoginDuration = Convert.ToDouble(ReadValue(filePath, ApplicationParameterKey.LoginDuration));
                        m_ApplicationParam.eLangType = (SynusLangPack.LanguageType)Enum.Parse(typeof(SynusLangPack.LanguageType), Convert.ToString(ReadValue(filePath, ApplicationParameterKey.LangType)));
                    }
                    else if (Section.Contains($"{ApplicationParam.Sections.Log}"))
                    {
                        foreach (LogParameterKey key in Enum.GetValues(typeof(LogParameterKey)))
                        {
                            try
                            {
                                switch (key)
                                {
                                    case LogParameterKey.LogCompressionCycle:
                                        m_LogParam.CompressionCycle = Convert.ToInt32(ReadValue(filePath, key));
                                        break;
                                    case LogParameterKey.ZIPDeleteCycle:
                                        m_LogParam.ZIPDeleteCycle = Convert.ToInt32(ReadValue(filePath, key));
                                        break;

                                    case LogParameterKey.LogShowType_Application:
                                        m_LogParam.LogShowType_Application = Convert.ToBoolean(ReadValue(filePath, key));
                                        break;
                                    case LogParameterKey.LogShowType_Exception:
                                        m_LogParam.LogShowType_Exception = Convert.ToBoolean(ReadValue(filePath, key));
                                        break;
                                    case LogParameterKey.LogShowType_Port:
                                        m_LogParam.LogShowType_Port = Convert.ToBoolean(ReadValue(filePath, key));
                                        break;
                                    case LogParameterKey.LogShowType_RackMaster:
                                        m_LogParam.LogShowType_RackMaster = Convert.ToBoolean(ReadValue(filePath, key));
                                        break;
                                    case LogParameterKey.LogShowType_CIM:
                                        m_LogParam.LogShowType_CIM = Convert.ToBoolean(ReadValue(filePath, key));
                                        break;
                                    case LogParameterKey.LogShowType_Master:
                                        m_LogParam.LogShowType_Master = Convert.ToBoolean(ReadValue(filePath, key));
                                        break;
                                    case LogParameterKey.LogShowType_WMX:
                                        m_LogParam.LogShowType_WMX = Convert.ToBoolean(ReadValue(filePath, key));
                                        break;
                                    case LogParameterKey.LogShowType_CPS:
                                        m_LogParam.LogShowType_CPS = Convert.ToBoolean(ReadValue(filePath, key));
                                        break;

                                    case LogParameterKey.LogShowType_Normal:
                                        m_LogParam.LogShowType_Normal = Convert.ToBoolean(ReadValue(filePath, key));
                                        break;
                                    case LogParameterKey.LogShowType_Warning:
                                        m_LogParam.LogShowType_Warning = Convert.ToBoolean(ReadValue(filePath, key));
                                        break;
                                    case LogParameterKey.LogShowType_Error:
                                        m_LogParam.LogShowType_Error = Convert.ToBoolean(ReadValue(filePath, key));
                                        break;
                                }
                            }
                            catch
                            {
                                WriteValue(filePath, key);
                            }
                        }
                    }
                    else if (Section.Contains($"{ApplicationParam.Sections.PortParameterInterlock}"))
                    {
                        foreach (PortParameterInterlockKey key in Enum.GetValues(typeof(PortParameterInterlockKey)))
                        {
                            try
                            {
                                switch (key)
                                {
                                    case PortParameterInterlockKey.Port_X_Axis_Vel_Limit_mm:
                                        m_PortParameterInterLock.Port_X_Axis_Vel_Limit_mm = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                    case PortParameterInterlockKey.Port_X_Axis_Acc_Limit_mm:
                                        m_PortParameterInterLock.Port_X_Axis_Acc_Limit_mm = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                    case PortParameterInterlockKey.Port_X_Axis_Dec_Limit_mm:
                                        m_PortParameterInterLock.Port_X_Axis_Dec_Limit_mm = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                    case PortParameterInterlockKey.Port_X_Axis_Teaching_OP_Limit_mm:
                                        m_PortParameterInterLock.Port_X_Axis_Teaching_OP_Limit_mm = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                    case PortParameterInterlockKey.Port_X_Axis_Teaching_LP_Limit_mm:
                                        m_PortParameterInterLock.Port_X_Axis_Teaching_LP_Limit_mm = Convert.ToDouble(ReadValue(filePath, key));
                                        break;

                                    case PortParameterInterlockKey.Port_Z_Axis_Vel_Limit_mm:
                                        m_PortParameterInterLock.Port_Z_Axis_Vel_Limit_mm = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                    case PortParameterInterlockKey.Port_Z_Axis_Acc_Limit_mm:
                                        m_PortParameterInterLock.Port_Z_Axis_Acc_Limit_mm = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                    case PortParameterInterlockKey.Port_Z_Axis_Dec_Limit_mm:
                                        m_PortParameterInterLock.Port_Z_Axis_Dec_Limit_mm = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                    case PortParameterInterlockKey.Port_Z_Axis_Teaching_Up_Limit_mm:
                                        m_PortParameterInterLock.Port_Z_Axis_Teaching_Up_Limit_mm = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                    case PortParameterInterlockKey.Port_Z_Axis_Teaching_Down_Limit_mm:
                                        m_PortParameterInterLock.Port_Z_Axis_Teaching_Down_Limit_mm = Convert.ToDouble(ReadValue(filePath, key));
                                        break;

                                    case PortParameterInterlockKey.Port_T_Axis_Vel_Limit_deg:
                                        m_PortParameterInterLock.Port_T_Axis_Vel_Limit_deg = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                    case PortParameterInterlockKey.Port_T_Axis_Acc_Limit_deg:
                                        m_PortParameterInterLock.Port_T_Axis_Acc_Limit_deg = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                    case PortParameterInterlockKey.Port_T_Axis_Dec_Limit_deg:
                                        m_PortParameterInterLock.Port_T_Axis_Dec_Limit_deg = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                    case PortParameterInterlockKey.Port_T_Axis_Teaching_180Deg_Limit_deg:
                                        m_PortParameterInterLock.Port_T_Axis_Teaching_180Deg_Limit_deg = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                    case PortParameterInterlockKey.Port_T_Axis_Teaching_0Deg_Limit_deg:
                                        m_PortParameterInterLock.Port_T_Axis_Teaching_0Deg_Limit_deg = Convert.ToDouble(ReadValue(filePath, key));
                                        break;
                                }
                            }
                            catch 
                            {
                                WriteValue(filePath, key);
                            }
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        static private void SaveDefaultFile(string filePath)
        {
            foreach (ApplicationParameterKey key in Enum.GetValues(typeof(ApplicationParameterKey)))
                WriteValue(filePath, key);

            foreach (LogParameterKey key in Enum.GetValues(typeof(LogParameterKey)))
                WriteValue(filePath, key);

            foreach (PortParameterInterlockKey key in Enum.GetValues(typeof(PortParameterInterlockKey)))
                WriteValue(filePath, key);
        }

        static public void WriteValue(string filePath, ApplicationParameterKey eApplicationParameterKey)
        {
            switch (eApplicationParameterKey)
            {
                case ApplicationParameterKey.LoginDuration:
                    MyINI.Write($"{Sections.Application}", $"{eApplicationParameterKey}", $"{m_ApplicationParam.LoginDuration.ToString("0.0")}", filePath);
                    break;
                case ApplicationParameterKey.LangType:
                    MyINI.Write($"{Sections.Application}", $"{eApplicationParameterKey}", $"{m_ApplicationParam.eLangType}", filePath);
                    break;
            }
        }
        static public void WriteValue(string filePath, LogParameterKey eLogParameterKey)
        {
            switch (eLogParameterKey)
            {
                case LogParameterKey.LogCompressionCycle:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.CompressionCycle}", filePath);
                    break;
                case LogParameterKey.ZIPDeleteCycle:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.ZIPDeleteCycle}", filePath);
                    break;

                case LogParameterKey.LogShowType_Application:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.LogShowType_Application}", filePath);
                    break;
                case LogParameterKey.LogShowType_Exception:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.LogShowType_Exception}", filePath);
                    break;
                case LogParameterKey.LogShowType_Port:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.LogShowType_Port}", filePath);
                    break;
                case LogParameterKey.LogShowType_RackMaster:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.LogShowType_RackMaster}", filePath);
                    break;
                case LogParameterKey.LogShowType_CIM:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.LogShowType_CIM}", filePath);
                    break;
                case LogParameterKey.LogShowType_Master:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.LogShowType_Master}", filePath);
                    break;
                case LogParameterKey.LogShowType_WMX:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.LogShowType_WMX}", filePath);
                    break;
                case LogParameterKey.LogShowType_CPS:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.LogShowType_CPS}", filePath);
                    break;

                case LogParameterKey.LogShowType_Normal:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.LogShowType_Normal}", filePath);
                    break;
                case LogParameterKey.LogShowType_Warning:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.LogShowType_Warning}", filePath);
                    break;
                case LogParameterKey.LogShowType_Error:
                    MyINI.Write($"{Sections.Log}", $"{eLogParameterKey}", $"{m_LogParam.LogShowType_Error}", filePath);
                    break;
            }
        }
        static public void WriteValue(string filePath, PortParameterInterlockKey ePortParameterInterlockKey)
        {
            switch (ePortParameterInterlockKey)
            {
                case PortParameterInterlockKey.Port_X_Axis_Vel_Limit_mm:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_X_Axis_Vel_Limit_mm}", filePath);
                    break;
                case PortParameterInterlockKey.Port_X_Axis_Acc_Limit_mm:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_X_Axis_Acc_Limit_mm}", filePath);
                    break;
                case PortParameterInterlockKey.Port_X_Axis_Dec_Limit_mm:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_X_Axis_Dec_Limit_mm}", filePath);
                    break;
                case PortParameterInterlockKey.Port_X_Axis_Teaching_OP_Limit_mm:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_X_Axis_Teaching_OP_Limit_mm}", filePath);
                    break;
                case PortParameterInterlockKey.Port_X_Axis_Teaching_LP_Limit_mm:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_X_Axis_Teaching_LP_Limit_mm}", filePath);
                    break;

                case PortParameterInterlockKey.Port_Z_Axis_Vel_Limit_mm:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_Z_Axis_Vel_Limit_mm}", filePath);
                    break;
                case PortParameterInterlockKey.Port_Z_Axis_Acc_Limit_mm:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_Z_Axis_Acc_Limit_mm}", filePath);
                    break;
                case PortParameterInterlockKey.Port_Z_Axis_Dec_Limit_mm:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_Z_Axis_Dec_Limit_mm}", filePath);
                    break;
                case PortParameterInterlockKey.Port_Z_Axis_Teaching_Up_Limit_mm:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_Z_Axis_Teaching_Up_Limit_mm}", filePath);
                    break;
                case PortParameterInterlockKey.Port_Z_Axis_Teaching_Down_Limit_mm:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_Z_Axis_Teaching_Down_Limit_mm}", filePath);
                    break;

                case PortParameterInterlockKey.Port_T_Axis_Vel_Limit_deg:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_T_Axis_Vel_Limit_deg}", filePath);
                    break;
                case PortParameterInterlockKey.Port_T_Axis_Acc_Limit_deg:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_T_Axis_Acc_Limit_deg}", filePath);
                    break;
                case PortParameterInterlockKey.Port_T_Axis_Dec_Limit_deg:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_T_Axis_Dec_Limit_deg}", filePath);
                    break;
                case PortParameterInterlockKey.Port_T_Axis_Teaching_180Deg_Limit_deg:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_T_Axis_Teaching_180Deg_Limit_deg}", filePath);
                    break;
                case PortParameterInterlockKey.Port_T_Axis_Teaching_0Deg_Limit_deg:
                    MyINI.Write($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", $"{m_PortParameterInterLock.Port_T_Axis_Teaching_0Deg_Limit_deg}", filePath);
                    break;
            }
        }
        static public object ReadValue(string filePath, ApplicationParameterKey eApplicationParameterKey)
        {
            return MyINI.Read($"{Sections.Application}", $"{eApplicationParameterKey}", filePath).ToString();
        }
        static public object ReadValue(string filePath, LogParameterKey eLogParameterKey)
        {
            return MyINI.Read($"{Sections.Log}", $"{eLogParameterKey}", filePath).ToString();
        }
        static public object ReadValue(string filePath, PortParameterInterlockKey ePortParameterInterlockKey)
        {
            return MyINI.Read($"{Sections.PortParameterInterlock}", $"{ePortParameterInterlockKey}", filePath).ToString();
        }
    }
}
