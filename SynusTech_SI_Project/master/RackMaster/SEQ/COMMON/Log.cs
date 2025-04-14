using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using RackMaster.SEQ.CLS;
using System.Windows.Forms;

namespace RackMaster.SEQ.COMMON
{
    public static class Log
    {
        public enum LogMessage_Main {
            ProgramStart,
            ProgramStartFail,
            ProgramStop,
            ProgramStopFail,

            InitLIB,
            InitConfig,
            InitAxis,
            InitIO,
            InitCLS,
            InitComplete,
            StartRun,

            InitLIBFail,
            InitConfigFail,
            InitAxisFail,
            InitIOFail,
            InitCLSFail,

            RackMasterAlarmOccurred,
            RackMasterAutoTeachingAlarmOccurred,
            RackMasterAlarmClear,
            RackMasterMSTDisconnectionAlarmClear,
            RackMasterAutoStep,
            RackMasterEvent,

            PortDataLoadSuccess,
            AppSettingLoadSuccess,
            LanguagePackLoadSuccess,
            AxisParameterLoadSuccess,
            RackMasterParameterLoadSuccess,

            PortDataLoadFail,
            AppSettingLoadFail,
            LanguagePackLoadFail,
            AxisParameterLoadFail,
            RackMasterParameterLoadFail,

            PortDataSaveSuccess,
            AppSettingFileSaveSuccess,
            AxisParameterSaveSuccess,
            RackMasterParameterSaveSuccess,

            PortDataSaveFail,
            AppSettingFileSaveFail,
            AxisParameterSaveFail,
            RackMasterParameterSaveFail,

            PortDataNotFound,
            AppSettingFileNotFound,
            LanguagePackFileNotFound,
            AxisParameterFileNotFound,
            RackMasterParameterFileNotFound,

            GetParameterFail,
            GetStateFail,

            WMX_CreateDevice,
            WMX_CreateDeviceFail,
            WMX_StartCommunication,
            WMX_StartCommunicationFail,
            WMX_StopCommunication,
            WMX_ParameterLoadFail,
            WMX_ParameterLoadSuccess,
            WMX_ParameterSaveFail,
            WMX_ParameterSaveSuccess,
            WMX_RefreshParameter,

            TCP_BeginServer,
            TCP_BeginServerFail,
            TCP_StartConnection,
            TCP_StartConnectionReady_Server,
            TCP_StartFail,
            TCP_StopConnection,
            TCP_ConnectionFail,
            TCP_ReceiveDataFail,
            TCP_ControlMessageParsingFail,
            TCP_UpdateStart,
            TCP_ReceiveDataOutOfRange,

            CIM_ReceiveBit,
            CIM_ReceiveWord,
            CIM_ActionFail,
            CIM_SetSystemTime,

            Interlock_NotFromReady,
            Interlock_NotToReady,
            Interlock_NotAutoCondition,
            Interlock_NotMaintReady,

            UI_ButtonClick,
            UI_LabelClick,
            UI_MousePoint,
            UI_DataGridViewClick,
            UI_DataGridViewValueChanged,
            UI_PageChanged,

            UI_DataGridViewExceptionErrorOccurred,

            Log_LoadLogFileListFail,
            Log_LoadLogFileListSuccess,
            Log_CheckLogFileFail,
            Log_CheckLogFileSuccess,
            Log_GetLogDataFail,

            Utility_PasswordDoesNotMatch,
            Utility_PasswordChange,
            Utility_LanguageFileLoadFail,
            Utility_LanguageFileLoadSuccess,

            ObjectIsNull,
            ArrayArgumentOutOfRange,
            FileIsNotFound,
            EnumIsNotDefined,
            ParsingFail,
        }

        public enum LogLevel {
            Normal,
            Error,
            Warning,
            Exception,
        }

        public enum LogType {
            RackMaster,
            TCP,
            CIM,
            WMX,
            UIControl,
            Utility,
        }

        public enum LoggingOrder {
            Time,
            Level,
            Type,
            Message,
        }

        public static string GetLogMessage(LogMessage_Main log) {
            switch (log) {
                case LogMessage_Main.ProgramStart:
                    return "Program Start";

                case LogMessage_Main.ProgramStartFail:
                    return "Program Start Fail";

                case LogMessage_Main.ProgramStop:
                    return "Program Stop";

                case LogMessage_Main.ProgramStopFail:
                    return "Program Stop Fail";


                case LogMessage_Main.InitLIB:
                    return "Initialize Library";

                case LogMessage_Main.InitConfig:
                    return "Initialize Config";

                case LogMessage_Main.InitAxis:
                    return "Initialize Axis";

                case LogMessage_Main.InitIO:
                    return "Initialize IO";

                case LogMessage_Main.InitCLS:
                    return "Initialize Class";

                case LogMessage_Main.InitComplete:
                    return "Initialize Complete";

                case LogMessage_Main.StartRun:
                    return "Start Main Run";


                case LogMessage_Main.InitLIBFail:
                    return "Initialize LIB Fail";

                case LogMessage_Main.InitConfigFail:
                    return "Initialize Config Fail";

                case LogMessage_Main.InitAxisFail:
                    return "Initialize Axis Fail";

                case LogMessage_Main.InitIOFail:
                    return "Initialize IO Fail";

                case LogMessage_Main.InitCLSFail:
                    return "Initialize CLS Fail";


                case LogMessage_Main.RackMasterAlarmOccurred:
                    return "Rackmaster Alarm Occurred";

                case LogMessage_Main.RackMasterAutoTeachingAlarmOccurred:
                    return "Auto Teaching Alarm Occurred";

                case LogMessage_Main.RackMasterAlarmClear:
                    return "Alarm Clear";

                case LogMessage_Main.RackMasterMSTDisconnectionAlarmClear:
                    return "MST Disconnection Alarm Clear";

                case LogMessage_Main.RackMasterAutoStep:
                    return "Current Auto Step";

                case LogMessage_Main.RackMasterEvent:
                    return "Event";


                case LogMessage_Main.PortDataLoadSuccess:
                    return "Port Data Load Success";

                case LogMessage_Main.AppSettingLoadSuccess:
                    return "App Setting Load Success";

                case LogMessage_Main.LanguagePackLoadSuccess:
                    return "Language Pack Load Success";

                case LogMessage_Main.AxisParameterLoadSuccess:
                    return "Axis Parameter Load Success";

                case LogMessage_Main.RackMasterParameterLoadSuccess:
                    return "Axis Parameter Load Success";


                case LogMessage_Main.PortDataLoadFail:
                    return "Port Parameter Load Fail";

                case LogMessage_Main.AppSettingLoadFail:
                    return "App Setting Load Fail";

                case LogMessage_Main.LanguagePackLoadFail:
                    return "Language Pack Load Fail";

                case LogMessage_Main.AxisParameterLoadFail:
                    return "Axis Parameter Load Fail";

                case LogMessage_Main.RackMasterParameterLoadFail:
                    return "Rackmaster Parameter Load Fail";


                case LogMessage_Main.PortDataSaveSuccess:
                    return "Port Data Save Success";

                case LogMessage_Main.AppSettingFileSaveSuccess:
                    return "App Setting File Save Success";

                case LogMessage_Main.AxisParameterSaveSuccess:
                    return "Axis Parameter Save Success";

                case LogMessage_Main.RackMasterParameterSaveSuccess:
                    return "Rackmaster Parameter Save Success";


                case LogMessage_Main.PortDataSaveFail:
                    return "Port Data Save Fail";

                case LogMessage_Main.AppSettingFileSaveFail:
                    return "App Setting Save Fail";

                case LogMessage_Main.AxisParameterSaveFail:
                    return "Axis Parameter Save Fail";

                case LogMessage_Main.RackMasterParameterSaveFail:
                    return "Rackmaster Parameter Save Fail";


                case LogMessage_Main.PortDataNotFound:
                    return "Port Data File Not Found";

                case LogMessage_Main.AppSettingFileNotFound:
                    return "App Setting File Not Found";

                case LogMessage_Main.LanguagePackFileNotFound:
                    return "Language Pack File Not Found";

                case LogMessage_Main.AxisParameterFileNotFound:
                    return "Axis Parameter File Not Found";

                case LogMessage_Main.RackMasterParameterFileNotFound:
                    return "Rackmaster Parameter File Not Found";


                case LogMessage_Main.GetParameterFail:
                    return "Get Parameter Fail";

                case LogMessage_Main.GetStateFail:
                    return "Get State Fail";


                case LogMessage_Main.WMX_CreateDevice:
                    return "Create Device";

                case LogMessage_Main.WMX_CreateDeviceFail:
                    return "Create Device Fail";

                case LogMessage_Main.WMX_StartCommunication:
                    return "Start Communication";

                case LogMessage_Main.WMX_StartCommunicationFail:
                    return "Start Communication Fail";

                case LogMessage_Main.WMX_StopCommunication:
                    return "Stop Communication";

                case LogMessage_Main.WMX_ParameterLoadFail:
                    return "WMX Parameter Load Fail";

                case LogMessage_Main.WMX_ParameterLoadSuccess:
                    return "WMX Parameter Load Success";

                case LogMessage_Main.WMX_ParameterSaveFail:
                    return "WMX Parameter Save Fail";

                case LogMessage_Main.WMX_ParameterSaveSuccess:
                    return "WMX Parameter Save Success";

                case LogMessage_Main.WMX_RefreshParameter:
                    return "Refresh WMX Parameters";


                case LogMessage_Main.TCP_BeginServer:
                    return "TCP Begin Server";

                case LogMessage_Main.TCP_BeginServerFail:
                    return "TCP Begin Server Fail";

                case LogMessage_Main.TCP_StartConnection:
                    return "TCP Start Connection";

                case LogMessage_Main.TCP_StartConnectionReady_Server:
                    return "TCP Connection Ready(Server)";

                case LogMessage_Main.TCP_StartFail:
                    return "TCP Start Connection Fail";

                case LogMessage_Main.TCP_StopConnection:
                    return "TCP Stop Connection";

                case LogMessage_Main.TCP_ConnectionFail:
                    return "TCP Connection Fail";

                case LogMessage_Main.TCP_ReceiveDataFail:
                    return "TCP Receive Data Fail";

                case LogMessage_Main.TCP_ControlMessageParsingFail:
                    return "TCP Control Message Parsing Fail";

                case LogMessage_Main.TCP_UpdateStart:
                    return "TCP Bit/Word Update Start";

                case LogMessage_Main.TCP_ReceiveDataOutOfRange:
                    return "TCP Receive Data Array Argument is out of range";


                case LogMessage_Main.CIM_ReceiveBit:
                    return "CIM ReceiveBit";

                case LogMessage_Main.CIM_ReceiveWord:
                    return "CIM ReceiveWord";

                case LogMessage_Main.CIM_ActionFail:
                    return "CIM Action Fail";

                case LogMessage_Main.CIM_SetSystemTime:
                    return "CIM Set SystemTime";


                case LogMessage_Main.Interlock_NotFromReady:
                    return "Not From Ready";

                case LogMessage_Main.Interlock_NotToReady:
                    return "Not To Ready";

                case LogMessage_Main.Interlock_NotAutoCondition:
                    return "Not Auto Condition";

                case LogMessage_Main.Interlock_NotMaintReady:
                    return "Not Maint Ready";


                case LogMessage_Main.UI_ButtonClick:
                    return "Button Click";

                case LogMessage_Main.UI_LabelClick:
                    return "Label Click";

                case LogMessage_Main.UI_MousePoint:
                    return "Mouse Point";

                case LogMessage_Main.UI_DataGridViewClick:
                    return "DataGridView Click";

                case LogMessage_Main.UI_DataGridViewValueChanged:
                    return "DataGridView Value Changed";

                case LogMessage_Main.UI_PageChanged:
                    return "Page Changed";


                case LogMessage_Main.UI_DataGridViewExceptionErrorOccurred:
                    return "DataGridView Exception Error Occured";


                case LogMessage_Main.Log_LoadLogFileListFail:
                    return "Log File Load Fail";

                case LogMessage_Main.Log_LoadLogFileListSuccess:
                    return "Log File Load Success";

                case LogMessage_Main.Log_CheckLogFileFail:
                    return "Log File Compress and Delete Fail";

                case LogMessage_Main.Log_CheckLogFileSuccess:
                    return "Log File Compress and Delete Success";

                case LogMessage_Main.Log_GetLogDataFail:
                    return "Get Log File Data Fail";


                case LogMessage_Main.Utility_PasswordDoesNotMatch:
                    return "Passowrds Do Not Match";

                case LogMessage_Main.Utility_PasswordChange:
                    return "Password is Changed";

                case LogMessage_Main.Utility_LanguageFileLoadFail:
                    return "Language File Load Fail";

                case LogMessage_Main.Utility_LanguageFileLoadSuccess:
                    return "Language File Load Success";


                case LogMessage_Main.ObjectIsNull:
                    return "Object Is Null";

                case LogMessage_Main.ArrayArgumentOutOfRange:
                    return "Array Argument is out of range";

                case LogMessage_Main.FileIsNotFound:
                    return "File is Not Found";

                case LogMessage_Main.EnumIsNotDefined:
                    return "Enum is Not Defined";

                case LogMessage_Main.ParsingFail:
                    return "Parsing Fail";

                default:
                    return "Not Define Message";
            }
        }

        public class LogItem {
            public DateTime dateTime;
            public LogLevel logLevel;
            public LogType logType;
            public string message;

            public LogItem() {

            }

            public LogItem(LogLevel logLevel, LogType logType, string message) {
                this.dateTime       = DateTime.Now;
                this.logLevel       = logLevel;
                this.logType        = logType;
                this.message        = message;
            }

            public LogItem(LogLevel logLevel, LogType logType, LogMessage_Main message) {
                this.dateTime       = DateTime.Now;
                this.logLevel       = logLevel;
                this.logType        = logType;
                this.message        = GetLogMessage(message);
            }

            public LogItem(LogLevel logLevel, LogType logType, LogMessage_Main message, string description) {
                this.dateTime       = DateTime.Now;
                this.logLevel       = logLevel;
                this.logType        = logType;
                this.message        = $"{GetLogMessage(message)}, Desc: {description}";
            }

            public LogItem(LogLevel logLevel, LogType logType, string message, Exception exception) {
                this.dateTime       = DateTime.Now;
                this.logLevel       = logLevel;
                this.logType        = logType;
                this.message        = $"Message: {message}\n Exception: {exception.Message}\n Stack Trace: {exception.StackTrace}";
            }

            public LogItem(LogLevel logLevel, LogType logType, LogMessage_Main message, Exception exception) {
                this.dateTime       = DateTime.Now;
                this.logLevel       = logLevel;
                this.logType        = logType;
                this.message        = $"Message: {GetLogMessage(message)}\n Exception: {exception.Message}\n Stack Trace: {exception.StackTrace}";
            }
        }

        private static object lockObj = new object();
        private static List<LogItem> m_logs;
        private static int m_compressDay = 90;
        private static int m_deleteDay = 180;

        public static DataGridView dgvLog;

        public static void InitLog() {
            m_logs = new List<LogItem>();
            dgvLog = new DataGridView();
            InitDataGridView();
        }
        /// <summary>
        /// Log 폴더 중 연도에 해당하는 폴더 리스트 반환
        /// </summary>
        /// <returns></returns>
        public static int[] GetLogFileList_Year() {
            try {
                List<int> yearList = new List<int>();
                string logPath = $"{ManagedFileInfo.LogDirectory}";
                DirectoryInfo years = new DirectoryInfo(logPath);
                foreach(var year in years.GetDirectories()) {
                    int nYear = 0;
                    if(int.TryParse(year.Name, out nYear)) {
                        yearList.Add(nYear);
                    }
                }
                return yearList.ToArray();
            }catch(Exception ex) {
                Add(new LogItem(LogLevel.Exception, LogType.Utility, LogMessage_Main.Log_LoadLogFileListFail, ex));
                return null;
            }
        }
        /// <summary>
        /// Log 폴더 중 월에 해당하는 폴더 리스트 반환
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int[] GetLogFileList_Month(int year) {
            try {
                List<int> monthList = new List<int>();
                string logPath = $"{ManagedFileInfo.LogDirectory}\\{year}"; 
                DirectoryInfo months = new DirectoryInfo(logPath);
                foreach(var month in months.GetDirectories()) {
                    if (HasSubfolders(month.FullName)) {
                        int nMonth = 0;
                        if (int.TryParse(month.Name, out nMonth)) {
                            monthList.Add(nMonth);
                        }
                    }
                }
                return monthList.ToArray();
            }catch(Exception ex) {
                Add(new LogItem(LogLevel.Exception, LogType.Utility, LogMessage_Main.Log_LoadLogFileListFail, ex));
                return null;
            }
        }
        /// <summary>
        /// Log 폴더 중 일에 해당하는 폴더 리스트 반환
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int[] GetLogFileList_Day(int year, int month) {
            try {
                List<int> dayList = new List<int>();
                string logPath = $"{ManagedFileInfo.LogDirectory}\\{year}\\{month}";
                DirectoryInfo days = new DirectoryInfo(logPath);
                foreach(var day in days.GetDirectories()) {
                    if (!IsCompressFile(day.FullName)) {
                        int nDay = 0;
                        if (int.TryParse(day.Name, out nDay)) {
                            dayList.Add(nDay);
                        }
                    }
                }
                return dayList.ToArray();
            }catch(Exception ex) {
                Add(new LogItem(LogLevel.Exception, LogType.Utility, LogMessage_Main.Log_LoadLogFileListFail, ex));
                return null;
            }
        }
        /// <summary>
        /// 실시간 Log 상태를 표현하는 Data Grid View 초기화
        /// </summary>
        private static void InitDataGridView() {
            dgvLog.Columns.Add("Time", "Time");
            dgvLog.Columns.Add("Level", "Level");
            dgvLog.Columns.Add("Type", "Type");
            dgvLog.Columns.Add("Message", "Message");

            dgvLog.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvLog.Columns[0].Width = 200;
            dgvLog.Columns[1].Width = 200;
            dgvLog.Columns[2].Width = 200;

            dgvLog.AllowUserToAddRows = false;
            dgvLog.AllowUserToResizeRows = false;
            dgvLog.AllowUserToDeleteRows = false;
            dgvLog.AllowUserToOrderColumns = false;
            dgvLog.AllowUserToResizeColumns = false;

            dgvLog.EnableHeadersVisualStyles = false;
            dgvLog.RowHeadersVisible = false;
            dgvLog.ReadOnly = true;

            dgvLog.BackgroundColor = System.Drawing.Color.White;
            dgvLog.EnableHeadersVisualStyles = false;
            dgvLog.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.DeepSkyBlue;

            foreach(DataGridViewColumn col in dgvLog.Columns) {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dgvLog.ClearSelection();
        }
        delegate void DataGridViewUpdate(LogItem itme);
        /// <summary>
        /// Log 데이터 추가
        /// </summary>
        /// <param name="logItem"></param>
        public static void Add(LogItem logItem) {
            lock (lockObj) {
                m_logs.Add(logItem);
                SaveLog(logItem);
            }
            if (dgvLog.InvokeRequired) {
                dgvLog.Invoke(new DataGridViewUpdate(SetDgvLog), logItem);
            }
            else {
                SetDgvLog(logItem);
            }
        }
        /// <summary>
        /// 추가된 Log를 Data Grid View 표시
        /// </summary>
        /// <param name="logItem"></param>
        private static void SetDgvLog(LogItem logItem) {
            int rowIdx = dgvLog.Rows.Add();
            dgvLog.Rows[rowIdx].Cells[(int)LoggingOrder.Time].Value       = logItem.dateTime;
            dgvLog.Rows[rowIdx].Cells[(int)LoggingOrder.Level].Value      = $"{logItem.logLevel}";
            dgvLog.Rows[rowIdx].Cells[(int)LoggingOrder.Type].Value       = $"{logItem.logType}";
            dgvLog.Rows[rowIdx].Cells[(int)LoggingOrder.Message].Value    = logItem.message;
            dgvLog.CurrentCell                                            = dgvLog.Rows[rowIdx].Cells[0];

            if(rowIdx >= 100) {
                dgvLog.Rows.RemoveAt(0);
                m_logs.RemoveAt(0);
            }
        }
        /// <summary>
        /// 추가된 Log를 저장
        /// </summary>
        /// <param name="logItem"></param>
        /// <returns></returns>
        private static bool SaveLog(LogItem logItem) {
            try {
                string folderPath;
                string logPath;

                if (logItem.logLevel == LogLevel.Exception) {
                    folderPath = $"{ManagedFileInfo.ExceptionLogDirectory}\\{logItem.dateTime.Year}\\{logItem.dateTime.Month}\\{logItem.dateTime.Day}";
                    logPath = $"{folderPath}\\{logItem.dateTime.Year}{logItem.dateTime.Month}{logItem.dateTime.Day}_{logItem.dateTime.Hour}_{ManagedFileInfo.ExceptionLogFileName}";
                }
                else {
                    folderPath = $"{ManagedFileInfo.LogDirectory}\\{logItem.dateTime.Year}\\{logItem.dateTime.Month}\\{logItem.dateTime.Day}";
                    logPath = $"{folderPath}\\{logItem.dateTime.Year}{logItem.dateTime.Month}{logItem.dateTime.Day}_{logItem.dateTime.Hour}_{ManagedFileInfo.LogFileName}";
                }

                DirectoryInfo di = new DirectoryInfo(folderPath);
                if(di.Exists == false) {
                    di.Create();
                }

                using(var fs = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)) {
                    //using (StreamWriter sw = File.AppendText(logPath)) {
                    //    sw.WriteLine($"{logItem.dateTime}\t{logItem.logLevel}\t{logItem.logType}\t{logItem.message}");
                    //    sw.Close();
                    //}
                    using(StreamWriter sw = new StreamWriter(fs)) {
                        sw.WriteLine($"{logItem.dateTime}\t{logItem.logLevel}\t{logItem.logType}\t{logItem.message}");
                    }
                }

                return true;
            }catch(Exception ex) {
                return false;
            }
        }
        /// <summary>
        /// 압축날짜 및 삭제 날짜를 설정
        /// </summary>
        /// <param name="compress"></param>
        /// <param name="delete"></param>
        public static void SetLogUtility(int compress, int delete) {
            m_compressDay = compress;
            m_deleteDay = delete;
        }
        /// <summary>
        /// 현재 Log 폴더 내의 존재하는 파일들을 압축/삭제 하는 함수
        /// </summary>
        public static void CheckLogFile() {
            int compDay = m_compressDay * (-1);
            int delDay = m_deleteDay * (-1);

            DateTime compressDateTime = DateTime.Now.AddDays(compDay);
            DateTime deleteDateTime = DateTime.Now.AddDays(delDay);

            string compDirectory = $"{ManagedFileInfo.LogDirectory}";

            try {
                if (Directory.Exists(compDirectory)) {
                    string zipPath;
                    DirectoryInfo years = new DirectoryInfo(compDirectory);

                    foreach (var year in years.GetDirectories()) {
                        int nYear = 0;
                        if (int.TryParse(year.Name, out nYear)) {
                            compDirectory = $"{ManagedFileInfo.LogDirectory}\\{nYear}";
                            if (nYear < deleteDateTime.Year && nYear < compressDateTime.Year) {
                                Directory.Delete(compDirectory, true);
                            }
                            else {
                                DirectoryInfo months = new DirectoryInfo(compDirectory);
                                foreach (var month in months.GetDirectories()) {
                                    int nMonth = 0;
                                    if (int.TryParse(month.Name, out nMonth)) {
                                        compDirectory = $"{ManagedFileInfo.LogDirectory}\\{nYear}\\{nMonth}";
                                        if (nMonth < deleteDateTime.Month && nYear <= deleteDateTime.Year) {
                                            Directory.Delete(compDirectory, true);
                                        }
                                        else {
                                            DirectoryInfo days = new DirectoryInfo(compDirectory);
                                            foreach (var day in days.GetDirectories()) {
                                                int nDay = 0;
                                                if (int.TryParse(day.Name, out nDay)) {
                                                    compDirectory = $"{ManagedFileInfo.LogDirectory}\\{nYear}\\{nMonth}\\{nDay}";
                                                    if (nDay <= deleteDateTime.Day && nMonth <= deleteDateTime.Month && nYear <= deleteDateTime.Year) {
                                                        Directory.Delete(compDirectory, true);
                                                    }
                                                    else if ((nMonth < compressDateTime.Month) || (nDay <= compressDateTime.Day && nMonth == compressDateTime.Month)) {
                                                        zipPath = $"{compDirectory}.zip";

                                                        ZipFile.CreateFromDirectory(compDirectory, zipPath);

                                                        while (true) {
                                                            if (File.Exists(zipPath))
                                                                break;

                                                            System.Threading.Thread.Sleep(1000);
                                                        }
                                                        Directory.Delete(compDirectory, true);
                                                    }
                                                }
                                            }
                                            compDirectory = $"{ManagedFileInfo.LogDirectory}\\{nYear}\\{nMonth}";
                                            string[] dayFiles = Directory.GetFiles(compDirectory);
                                            foreach (string dayFile in dayFiles) {
                                                string fileName = Path.GetFileNameWithoutExtension(dayFile);
                                                int nDay = 0;
                                                if (int.TryParse(fileName, out nDay)) {
                                                    if (nYear <= deleteDateTime.Year && nMonth <= deleteDateTime.Month && nDay <= deleteDateTime.Day) {
                                                        File.Delete(dayFile);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else {
                    Add(new LogItem(LogLevel.Error, LogType.Utility, LogMessage_Main.FileIsNotFound));
                }
                Add(new LogItem(LogLevel.Normal, LogType.Utility, LogMessage_Main.Log_CheckLogFileSuccess));
            }catch(Exception ex) {
                Add(new LogItem(LogLevel.Exception, LogType.Utility, LogMessage_Main.Log_CheckLogFileFail, ex));
            }
        }
        /// <summary>
        /// 현재 Log 클리어
        /// </summary>
        public static void ClearLog() {
            m_logs.Clear();
        }
        /// <summary>
        /// 특정 연/월/일에 해당하는 Log History 로드
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static List<string[]> GetLogHistory(int year, int month, int day) {
            try {
                string logPath = $"{ManagedFileInfo.LogDirectory}\\{year}\\{month}\\{day}";
                List<string[]> history = new List<string[]>();
                string[] files = Directory.GetFiles(logPath);

                foreach (string file in files) {
                    //string[] lines = File.ReadAllLines(file);
                    //var lines = File.ReadLines(file);


                    //foreach(string line in lines) {
                    //    string[] items = line.Split('\t');
                    //    history.Add(items);
                    //}

                    using(var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                        using(StreamReader sr = new StreamReader(fs)) {
                            string[] lines = sr.ReadToEnd().Split('\n');

                            foreach(string line in lines) {
                                string[] items = line.Split('\t');
                                if (items.Length <= 1)
                                    continue;
                                history.Add(items);
                            }
                        }
                    }
                }

                return history;
            }
            catch(DirectoryNotFoundException diEx) {
                Add(new LogItem(LogLevel.Exception, LogType.Utility, LogMessage_Main.FileIsNotFound, diEx));
                MessageBox.Show(diEx.Message);

                return null;
            }catch(Exception ex) {
                Add(new LogItem(LogLevel.Exception, LogType.Utility, LogMessage_Main.Log_GetLogDataFail, ex));
                MessageBox.Show(ex.Message);

                return null;
            }
        }

        private static bool HasTextFileInFolder(string folderPath) {
            try {
                string[] files = Directory.GetFiles(folderPath);

                return files.Any(file => IsTextFile(file));
            }catch(Exception ex) {
                return false;
            }
        }

        private static bool HasSubfolders(string folderPath) {
            try {
                string[] subFolders = Directory.GetDirectories(folderPath);

                return subFolders.Length > 0;
            }catch(Exception ex) {
                return false;
            }
        }

        private static bool IsCompressFile(string filePath) {
            string extension = GetFileExtension(filePath, StringComparison.OrdinalIgnoreCase);

            string[] compressedExtensions = { ".zip", ".rar", ".7z" };

            return Array.Exists(compressedExtensions, ext => ext.Equals(extension));
        }

        private static bool IsTextFile(string filePath) {
            string extension = GetFileExtension(filePath, StringComparison.OrdinalIgnoreCase);

            return extension == "txt";
        }

        private static string GetFileExtension(string filePath, StringComparison comparisonType) {
            string extension = System.IO.Path.GetExtension(filePath);

            if (!string.IsNullOrEmpty(extension)) {
                extension = extension.TrimStart('.');
            }

            return extension;
        }
    }
}
