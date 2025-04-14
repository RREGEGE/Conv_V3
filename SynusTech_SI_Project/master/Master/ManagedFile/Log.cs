using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Master
{
    public class LogMsg
    {
        //Log는 아래와 같은 구조로 저장됨
        //시간 + 로그 타입 + 로그 레벨 + 메세지 리스트 또는 직접 입력된 Text

        /// <summary>
        /// 저장할 LogType에 관련된 Enum
        /// </summary>
        public enum LogType
        {
            Application,
            Exception,
            Port,
            RackMaster,
            CIM,
            Master,
            WMX,
            CPS
        }


        /// <summary>
        /// 저장할 Log Level에 관련된 Enum
        /// </summary>
        public enum LogLevel
        {
            Normal,
            Warning,
            Error
        }


        /// <summary>
        /// 출력할 MsgList Enum
        /// </summary>
        public enum MsgList
        {
            ProgramStart,
            ProgramClose,
            TCPIPDisconnection,
            TCPIPConnection,
            TCPIPNotConnection,
            TCPIPPacketOverFlow,
            TCPIPInvalidPacket,
            TCPIPNotDefineMemoryMap,
            TCPIPMemoryMapOutofRange,
            TCPIPCheckSUMError,
            TCPIPSTXError,
            TCPIPETXError,
            TCPIPPacketReceive,
            TCPIPSTXETXPosError,
            TCPIPRequestProcessEnd,

            MemoryMapSendStart,
            MemoryMapSendEnd,
            
            CycleControlAlarmStop,
            AutoControlAlarmStop,
            AutoControlStopCommand,
            AutoControlAndCycleCrash,
            
            FilePathIsEmpty,
            FileIsNotExist,
            FileBackupSuccess,
            FileLoadSuccess,
            FileLoadFail,
            FileSaveSuccess,
            FileSaveFail,
            FileCreate,
            FileMove,
            FileLineIsEmpty,
            FileLineIsInvalid,
            
            MasterEquipmentInitializeStart,
            MasterEquipmentInitializeEnd,
            
            InvalidParameter,
            InvalidOperationType,
            InvalidControlType,
            InvalidState,
            
            PIOCurrentFlagKeeping,
            PIOKeepingFlagReload,
            PIOInformation,

            AlarmDetailInformation,

            DeadManSwitchReleaseStop,
            DTPConnectionInfo,

            WMXParameterApplyFail,
            WMXParameterApplySuccess,
            WMXParameterGetFail,
            WMXParameterGetSuccess,
            
            AlarmClear,
            AlarmCreate,
            
            SafetySensorDetectError,

            IODuple,
            CasseteIDOverFlow,
            Interlock,
            UnableOpenTheDoor,
            AcceptLogIn,
            LogInTimeout,
            LogInExtend,
            LogInExtendReject,
            LogIn,
            LogOut,
            LogOutReject,
            ButtonClick,
            Process,
            ParameterSaveAdminNameFail,
            ParameterSaveAccept,
            ParameterSavePasswordFail,

            NoneTargetRMFromReceivePacket,
            NoneTargetPortFromReceivePacket,
            RackMasterIsNotCIMMode,
            PortIsNotCIMMode,

            RackMasterAutoModeInterlock,
            RackMasterTeachingRWSuccess,
            RackMasterCSTInfo,
            RackMasterCycleStepInfo,
            RackMasterTeachingOffsetApply,
            RackMasterFromInfo,
            RackMasterToInfo,
            RackMasterTeachingInfo,
            RackMasterMotionInfo,
            RackMasterControlInfo,
            RackMasterInfo,

            PortCSTInfo,
            PortStepInfo,
            PortEStopInfo,
            PortStatusInfo,
            PortServoAmpOffline,

            TagReaderInfo,
            RFIDBusy,
            RFIDReadTimeOut,
            RFIDPortIDError,
            RFIDInfo,
            BCRBusy,
            BCRReadTimeOut,
            BCRPortIDError,
            BCRInfo,
            CanTops_LM21_Info,
            CanTops_LM21_PortID_Error,

            OMRON_Info,

        }

        public DateTime dt;
        public LogType eLogType;
        public LogLevel eLogLevel;

        public string MsgLogType;
        public string MsgLogLevel;
        public string MsgTitle;           //Title
        public string MsgDescription;     //Description
        public string MsgStack;

        /// <summary>
        /// Exception 관련 로그를 저장합니다.
        /// Exception Log는 일반 Log 폴더가 아닌 Exception Log 폴더에 저장됩니다.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="_MsgDescription"></param>
        static public void AddExceptionLog(Exception ex, string _MsgDescription)
        {
            LogMsg msg = new LogMsg();
            msg.dt              = DateTime.Now;
            msg.eLogType        = LogType.Exception;
            msg.MsgLogType      = LogType.Exception.ToString();
            msg.eLogLevel       = LogLevel.Error;
            msg.MsgLogLevel     = LogLevel.Error.ToString();
            msg.MsgTitle        = ex.Message;
            msg.MsgStack        = ex.StackTrace;
            msg.MsgDescription  = _MsgDescription;

            Log.Add(msg);
        }
        
        /// <summary>
        /// Application에서 발생한 로그를 저장합니다.
        /// 일반적으로 버튼, 화면 출력 등 유저 행동과 관련된 이벤트를 기록합니다.
        /// </summary>
        /// <param name="_LogLevel"></param>
        /// <param name="_MsgList"></param>
        /// <param name="_MsgDescription"></param>
        static public void AddApplicationLog(LogLevel _LogLevel, MsgList _MsgList, string _MsgDescription)
        {
            LogMsg msg = new LogMsg();
            msg.dt              = DateTime.Now;
            msg.eLogType        = LogType.Application;
            msg.MsgLogType      = LogType.Application.ToString();
            msg.eLogLevel       = _LogLevel;
            msg.MsgLogLevel     = _LogLevel.ToString();
            msg.MsgTitle        = MsgListToString(_MsgList);
            msg.MsgDescription  = _MsgDescription;

            Log.Add(msg);
        }
        
        /// <summary>
        /// CIM 과 관련되어 발생한 로그를 저장합니다.
        /// 통신 및 명령 전송 상태를 기록합니다.
        /// </summary>
        /// <param name="_LogLevel"></param>
        /// <param name="_MsgList"></param>
        /// <param name="_MsgDescription"></param>
        static public void AddCIMLog(LogLevel _LogLevel, MsgList _MsgList, string _MsgDescription)
        {
            LogMsg msg = new LogMsg();
            msg.dt              = DateTime.Now;
            msg.eLogType        = LogType.CIM;
            msg.MsgLogType      = LogType.CIM.ToString();
            msg.eLogLevel       = _LogLevel;
            msg.MsgLogLevel     = _LogLevel.ToString();
            msg.MsgTitle        = MsgListToString(_MsgList);
            msg.MsgDescription  = _MsgDescription;

            Log.Add(msg);
        }
        
        /// <summary>
        /// Master와 관련되어 발생한 로그를 저장합니다.
        /// 마스터 설정 및 제어와 관련된 상태를 기록합니다.
        /// </summary>
        /// <param name="_LogLevel"></param>
        /// <param name="_MsgList"></param>
        /// <param name="_MsgDescription"></param>
        static public void AddMasterLog(LogLevel _LogLevel, MsgList _MsgList, string _MsgDescription)
        {
            LogMsg msg = new LogMsg();
            msg.dt              = DateTime.Now;
            msg.eLogType        = LogType.Master;
            msg.MsgLogType      = LogType.Master.ToString();
            msg.eLogLevel       = _LogLevel;
            msg.MsgLogLevel     = _LogLevel.ToString();
            msg.MsgTitle        = MsgListToString(_MsgList);
            msg.MsgDescription  = _MsgDescription;

            Log.Add(msg);
        }
        
        /// <summary>
        /// Port와 관련되어 발생한 로그를 저장합니다.
        /// Memory Map Data 변경 및 스텝 등의 상태를 기록합니다.
        /// </summary>
        /// <param name="PortID"></param>
        /// <param name="_LogLevel"></param>
        /// <param name="_MsgList"></param>
        /// <param name="_MsgDescription"></param>
        static public void AddPortLog(string PortID, LogLevel _LogLevel, MsgList _MsgList, string _MsgDescription)
        {
            LogMsg msg = new LogMsg();
            msg.dt                  = DateTime.Now;
            msg.eLogType            = LogType.Port;
            msg.MsgLogType          = $"{LogType.Port} {PortID}";
            msg.eLogLevel           = _LogLevel;
            msg.MsgLogLevel         = _LogLevel.ToString();
            msg.MsgTitle            = MsgListToString(_MsgList);
            msg.MsgDescription      = _MsgDescription;

            Log.Add(msg);
        }
        
        /// <summary>
        /// STK와 관련되어 발생한 로그를 저장합니다.
        /// Memory Map Data 변경 및 스텝, 통신 연결 상태등을 기록합니다.
        /// STK Motion Log의 경우 별도 STK 폴더에 저장됩니다.
        /// </summary>
        /// <param name="RackMasterID"></param>
        /// <param name="_LogLevel"></param>
        /// <param name="_MsgList"></param>
        /// <param name="_MsgDescription"></param>
        static public void AddRackMasterLog(string RackMasterID, LogLevel _LogLevel, MsgList _MsgList, string _MsgDescription)
        {
            LogMsg msg = new LogMsg();
            msg.dt              = DateTime.Now;
            msg.eLogType        = LogType.RackMaster;
            msg.MsgLogType      = $"{LogType.RackMaster} {RackMasterID}";
            msg.eLogLevel       = _LogLevel;
            msg.MsgLogLevel     = _LogLevel.ToString();
            msg.MsgTitle        = MsgListToString(_MsgList);
            msg.MsgDescription  = _MsgDescription;

            if(_MsgList == MsgList.RackMasterMotionInfo)
                Log.Add(msg, false);
            else
                Log.Add(msg);
        }
        
        /// <summary>
        /// CPS와 관련되어 발생한 로그를 저장합니다.
        /// Memory Map Data 변경 및 통신 연결 상태 등을 기록합니다.
        /// </summary>
        /// <param name="_LogLevel"></param>
        /// <param name="_MsgList"></param>
        /// <param name="_MsgDescription"></param>
        static public void AddCPSLog(LogLevel _LogLevel, MsgList _MsgList, string _MsgDescription)
        {
            LogMsg msg = new LogMsg();
            msg.dt = DateTime.Now;
            msg.eLogType = LogType.CPS;
            msg.MsgLogType = LogType.CPS.ToString();
            msg.eLogLevel = _LogLevel;
            msg.MsgLogLevel = _LogLevel.ToString();
            msg.MsgTitle = MsgListToString(_MsgList);
            msg.MsgDescription = _MsgDescription;

            Log.Add(msg);
        }
        
        /// <summary>
        /// WMX Library 사용과 관련된 로그를 기록합니다.
        /// </summary>
        /// <param name="_LogLevel"></param>
        /// <param name="_MsgTitle"></param>
        /// <param name="_MsgDescription"></param>
        static public void AddWMXLog(LogLevel _LogLevel, string _MsgTitle, string _MsgDescription)
        {
            LogMsg msg = new LogMsg();
            msg.dt              = DateTime.Now;
            msg.eLogType        = LogType.WMX;
            msg.MsgLogType      = LogType.WMX.ToString();
            msg.eLogLevel       = _LogLevel;
            msg.MsgLogLevel     = _LogLevel.ToString();
            msg.MsgTitle        = _MsgTitle;
            msg.MsgDescription  = _MsgDescription;

            Log.Add(msg);
        }


        /// <summary>
        /// Add Log에서 인자로 받은 Msg List Enum 인자를 Text로 변환하는데 사용됩니다.
        /// </summary>
        /// <param name="_MsgList"></param>
        /// <returns></returns>
        static private string MsgListToString(MsgList _MsgList)
        {
            switch(_MsgList)
            {
                case MsgList.ProgramStart:
                    return "Program Start";
                case MsgList.ProgramClose:
                    return "Program Close";
                case MsgList.TCPIPDisconnection:
                    return "TCP/IP Disconnection";
                case MsgList.TCPIPConnection:
                    return "TCP/IP Connection";
                case MsgList.TCPIPNotConnection:
                    return "TCP/IP not Connection";
                case MsgList.TCPIPPacketOverFlow:
                    return "TCP/IP Packet OverFlow";
                case MsgList.TCPIPInvalidPacket:
                    return "TCP/IP Invalid Packet";
                case MsgList.TCPIPNotDefineMemoryMap:
                    return "TCP/IP Not Define Memory Map";
                case MsgList.TCPIPMemoryMapOutofRange:
                    return "TCP/IP Memory Map Out of Range";
                case MsgList.TCPIPCheckSUMError:
                    return "TCP/IP Check SUM Error";
                case MsgList.TCPIPSTXError:
                    return "TCP/IP STX Error";
                case MsgList.TCPIPETXError:
                    return "TCP/IP ETX Error";
                case MsgList.TCPIPPacketReceive:
                    return "TCP/IP Packet Receive";
                case MsgList.TCPIPSTXETXPosError:
                    return "TCP/IP STX ETX Pos Error";
                case MsgList.TCPIPRequestProcessEnd:
                    return "TCP/IP Request Process End";

                case MsgList.MemoryMapSendStart:
                    return "Memory Map Send Start";
                case MsgList.MemoryMapSendEnd:
                    return "Moemory Map Send End";

                case MsgList.CycleControlAlarmStop:
                    return "Cycle Control Alarm Stop";
                case MsgList.AutoControlAlarmStop:
                    return "Auto Control Alarm Stop";
                case MsgList.AutoControlStopCommand:
                    return "Auto Control Stop REQ";
                case MsgList.AutoControlAndCycleCrash:
                    return "Auto Control and Cycle Control CMD Crash";

                case MsgList.FilePathIsEmpty:
                    return "File Path is Empty";
                case MsgList.FileIsNotExist:
                    return "File is not Exist";
                case MsgList.FileBackupSuccess:
                    return "File Backup Success";
                case MsgList.FileLoadSuccess:
                    return "File Load Success";
                case MsgList.FileLoadFail:
                    return "File Load Fail";
                case MsgList.FileSaveSuccess:
                    return "File Save Success";
                case MsgList.FileSaveFail:
                    return "File Save Fail";
                case MsgList.FileCreate:
                    return "File Create";
                case MsgList.FileMove:
                    return "File Move";
                case MsgList.FileLineIsEmpty:
                    return "File Line is Empty";
                case MsgList.FileLineIsInvalid:
                    return "File Line is Invalid";

                case MsgList.MasterEquipmentInitializeStart:
                    return "Master Equipment Initialize Start";
                case MsgList.MasterEquipmentInitializeEnd:
                    return "Master Equipment Initialize End";

                case MsgList.InvalidParameter:
                    return "Invalid Parameter";
                case MsgList.InvalidOperationType:
                    return "Invalid Operation Type";
                case MsgList.InvalidControlType:
                    return "Invalid Control Type";
                case MsgList.InvalidState:
                    return "Invalid State";

                case MsgList.PIOCurrentFlagKeeping:
                    return "PIO Current Flag Keeping";
                case MsgList.PIOKeepingFlagReload:
                    return "PIO Keeping Flag Reload";
                case MsgList.PIOInformation:
                    return "PIO Information";

                case MsgList.AlarmDetailInformation:
                    return "Alarm Detail Information";

                case MsgList.DeadManSwitchReleaseStop:
                    return "DeadMan Switch Release Stop";
                case MsgList.DTPConnectionInfo:
                    return "Handy Touch is Connected";

                case MsgList.WMXParameterApplyFail:
                    return "WMX Parameter Apply Fail";
                case MsgList.WMXParameterApplySuccess:
                    return "WMX Parameter Apply Success";
                case MsgList.WMXParameterGetFail:
                    return "WMX Parameter Get Fail";
                case MsgList.WMXParameterGetSuccess:
                    return "WMX Parameter Get Success";

                case MsgList.AlarmClear:
                    return "Alarm Clear";
                case MsgList.AlarmCreate:
                    return "Alarm Create";

                case MsgList.SafetySensorDetectError:
                    return "Safety Sensor Detect Error";

                case MsgList.IODuple:
                    return "IO Duple";
                case MsgList.CasseteIDOverFlow:
                    return "Cassette ID Over Flow";
                case MsgList.Interlock:
                    return "Interlock";
                
                case MsgList.UnableOpenTheDoor:
                    return "Unable Open the door";
                case MsgList.AcceptLogIn:
                    return "Accept Login";
                case MsgList.LogInTimeout:
                    return "LogIn Timeout";
                case MsgList.LogInExtend:
                    return "Login Extend";
                case MsgList.LogInExtendReject:
                    return "Login Extend Reject";
                case MsgList.LogIn:
                    return "LogIn";
                case MsgList.LogOut:
                    return "Logout";
                case MsgList.LogOutReject:
                    return "Logout Reject";
                case MsgList.ButtonClick:
                    return "Button Click";
                case MsgList.Process:
                    return "Process";
                case MsgList.ParameterSaveAdminNameFail:
                    return "Parameter Save Admin Name Fail";
                case MsgList.ParameterSaveAccept:
                    return "Parameter Save Accept";
                case MsgList.ParameterSavePasswordFail:
                    return "Parameter Save Password Fail";

                case MsgList.NoneTargetRMFromReceivePacket:
                    return "None Target RM From Receive Packet";
                case MsgList.NoneTargetPortFromReceivePacket:
                    return "None Target Port From Receive Packet";
                case MsgList.RackMasterIsNotCIMMode:
                    return "RackMaster Is Not CIM Mode";
                case MsgList.PortIsNotCIMMode:
                    return "Port Is Not CIM Mode";
                case MsgList.RackMasterAutoModeInterlock:
                    return "RackMaster Auto Mode Interlock";
                case MsgList.RackMasterTeachingRWSuccess:
                    return "RackMaster Teaching Data Read Success";
                case MsgList.RackMasterCSTInfo:
                    return "RackMaster CST ID Info";
                case MsgList.RackMasterCycleStepInfo:
                    return "RackMaster Cycle Step Info";
                case MsgList.RackMasterTeachingOffsetApply:
                    return "RackMaster Teaching Offset Apply";
                case MsgList.RackMasterFromInfo:
                    return "RackMaster From ID Info";
                case MsgList.RackMasterToInfo:
                    return "RackMaster To ID Info";
                case MsgList.RackMasterTeachingInfo:
                    return "RackMaster Teaching Info";
                case MsgList.RackMasterMotionInfo:
                    return "RackMaster Motion Info";
                case MsgList.RackMasterControlInfo:
                    return "RackMaster Control Info";
                case MsgList.RackMasterInfo:
                    return "RackMaster Info";


                case MsgList.PortCSTInfo:
                    return "Port CST ID Info";
                case MsgList.PortStepInfo:
                    return "Port Step Info";
                case MsgList.PortEStopInfo:
                    return "Port E-Stop Info";
                case MsgList.PortStatusInfo:
                    return "Port Status Info";
                case MsgList.PortServoAmpOffline:
                    return "Port Servo Amp Offline";

                case MsgList.TagReaderInfo:
                    return "Tag Reader Info";
                case MsgList.RFIDBusy:
                    return "RFID Busy";
                case MsgList.RFIDReadTimeOut:
                    return "RFID Read Time out";
                case MsgList.RFIDPortIDError:
                    return "RFID Port Equipment ID Error";
                case MsgList.RFIDInfo:
                    return "RFID Info";
                case MsgList.BCRBusy:
                    return "BCR Busy";
                case MsgList.BCRReadTimeOut:
                    return "BCR Read Time out";
                case MsgList.BCRPortIDError:
                    return "BCR Port Equipment ID Error";
                case MsgList.BCRInfo:
                    return "BCR Info";
                case MsgList.CanTops_LM21_Info:
                    return "CanTops LM21 Info";
                case MsgList.CanTops_LM21_PortID_Error:
                    return "CanTops LM21 Port ID Error";
                case MsgList.OMRON_Info:
                    return "OMRON Info";
                default:
                    return $"Not Define Message";
            }
        }
    }

    class Log
    {
        /// <summary>
        /// Main Log -> Data Grid View 출력과 관련된 옵션 파라미터 입니다. (true시 해당 Type의 Log Showing)
        /// Application Param의 값으로 덮어 쓰기 진행 됨
        /// </summary>
        static public Dictionary<LogMsg.LogType, bool> ShowType = new Dictionary<LogMsg.LogType, bool>() {
            { LogMsg.LogType.Application, true },
            { LogMsg.LogType.Exception, true },
            { LogMsg.LogType.Port, true },
            { LogMsg.LogType.RackMaster, true },
            { LogMsg.LogType.CIM, true },
            { LogMsg.LogType.Master, true },
            { LogMsg.LogType.WMX, true },
            { LogMsg.LogType.CPS, true }
        };

        /// <summary>
        /// Main Log -> Data Grid View 출력과 관련된 옵션 파라미터 입니다. (true시 해당 Level의 Log Showing)
        /// Application Param의 값으로 덮어 쓰기 진행 됨
        /// </summary>
        static public Dictionary<LogMsg.LogLevel, bool> ShowLevel = new Dictionary<LogMsg.LogLevel, bool>()
        {   { LogMsg.LogLevel.Normal, true },
            { LogMsg.LogLevel.Warning, true },
            { LogMsg.LogLevel.Error, true },
        };

        /// <summary>
        /// 로그 넣고 뺄 때 동기화를 위함
        /// </summary>
        static private object LogLock = new object();

        /// <summary>
        /// 기록된 Log의 List 객체
        /// </summary>
        static private List<LogMsg> LogMsgList = new List<LogMsg>();

        /// <summary>
        /// Log Insert 관련 이벤트 및 동작
        /// </summary>
        /// <param name="_LogMsg"></param>
        public delegate void LogInsertEvent(LogMsg _LogMsg);
        static public event LogInsertEvent logInsertEvent;

        /// <summary>
        /// Log용 지정된 DataGridView의 객체를 리턴
        /// </summary>
        /// <returns></returns>
        static public DataGridView CreateLogGridView()
        {
            DataGridView LogDGV = new DataGridView();
            DataGridViewCellStyle DGV_ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            DataGridViewCellStyle DGV_DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            DataGridViewCellStyle DGV_RowHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();

            DataGridViewCellStyle TbxColumn0CellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            DataGridViewCellStyle TbxColumn1CellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            DataGridViewCellStyle TbxColumn2CellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            DataGridViewCellStyle TbxColumn3CellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            DataGridViewCellStyle TbxColumn4CellStyle = new System.Windows.Forms.DataGridViewCellStyle();

            DataGridViewTextBoxColumn TbxColumn0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn TbxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn TbxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn TbxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn TbxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            // 
            // dataGridViewTextBoxColumn10
            // 
            TbxColumn0.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            TbxColumn0CellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            TbxColumn0.DefaultCellStyle = TbxColumn0CellStyle;
            TbxColumn0.FillWeight = 40F;
            TbxColumn0.HeaderText = "Time";
            TbxColumn0.Name = "dataGridViewTextBoxColumn10";
            TbxColumn0.ReadOnly = true;
            TbxColumn0.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            TbxColumn0.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            TbxColumn0.Width = 200;
            // 
            // dataGridViewTextBoxColumn12
            // 
            TbxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            TbxColumn1CellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            TbxColumn1.DefaultCellStyle = TbxColumn1CellStyle;
            TbxColumn1.FillWeight = 40F;
            TbxColumn1.HeaderText = "Type";
            TbxColumn1.Name = "dataGridViewTextBoxColumn12";
            TbxColumn1.ReadOnly = true;
            TbxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            TbxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            TbxColumn1.Width = 120;
            // 
            // dataGridViewTextBoxColumn11
            // 
            TbxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            TbxColumn2CellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            TbxColumn2.DefaultCellStyle = TbxColumn2CellStyle;
            TbxColumn2.FillWeight = 30F;
            TbxColumn2.HeaderText = "Level";
            TbxColumn2.Name = "dataGridViewTextBoxColumn11";
            TbxColumn2.ReadOnly = true;
            TbxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            TbxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            TbxColumn2.Width = 100;
            // 
            // Column7
            // 
            TbxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            TbxColumn3CellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            TbxColumn3.DefaultCellStyle = TbxColumn3CellStyle;
            TbxColumn3.FillWeight = 100F;
            TbxColumn3.HeaderText = "Title";
            TbxColumn3.Name = "Column7";
            TbxColumn3.ReadOnly = true;
            TbxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            TbxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn13
            // 
            TbxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            TbxColumn4CellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            TbxColumn4.DefaultCellStyle = TbxColumn4CellStyle;
            TbxColumn4.FillWeight = 350F;
            TbxColumn4.HeaderText = "Comment";
            TbxColumn4.Name = "dataGridViewTextBoxColumn13";
            TbxColumn4.ReadOnly = true;
            TbxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            TbxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            LogDGV.AllowUserToAddRows = false;
            LogDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            LogDGV.BackgroundColor = System.Drawing.Color.AliceBlue;
            LogDGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            LogDGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            DGV_ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            DGV_ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            DGV_ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            DGV_ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            DGV_ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            DGV_ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            DGV_ColumnHeadersDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            LogDGV.ColumnHeadersDefaultCellStyle = DGV_ColumnHeadersDefaultCellStyle;
            LogDGV.ColumnHeadersHeight = 20;
            LogDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            LogDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            TbxColumn0,
            TbxColumn1,
            TbxColumn2,
            TbxColumn3,
            TbxColumn4});
            DGV_DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DGV_DefaultCellStyle.BackColor = System.Drawing.Color.White;
            DGV_DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            DGV_DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            DGV_DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            DGV_DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            DGV_DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            LogDGV.DefaultCellStyle = DGV_DefaultCellStyle;
            LogDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            LogDGV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            LogDGV.EnableHeadersVisualStyles = false;
            LogDGV.GridColor = System.Drawing.Color.LightGray;
            LogDGV.Location = new System.Drawing.Point(0, 30);
            LogDGV.Margin = new System.Windows.Forms.Padding(0);
            LogDGV.MultiSelect = false;
            LogDGV.Name = "DGV_Log";
            LogDGV.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            DGV_RowHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DGV_RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            DGV_RowHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            DGV_RowHeadersDefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            DGV_RowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            DGV_RowHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            DGV_RowHeadersDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            LogDGV.RowHeadersDefaultCellStyle = DGV_RowHeadersDefaultCellStyle;
            LogDGV.RowHeadersVisible = false;
            LogDGV.RowTemplate.Height = 18;
            LogDGV.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            LogDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            LogDGV.Size = new System.Drawing.Size(1654, 78);
            LogDGV.TabIndex = 35;

            return LogDGV;
        }

        /// <summary>
        /// 전달 받은 DataGridView에 현재 로그를 기준으로 다시 로드
        /// </summary>
        /// <param name="LogDGV"></param>
        static public void LogDGVReload(DataGridView LogDGV)
        {
            ClearLogDGV(LogDGV);
            lock (LogLock)
            {
                foreach (var value in LogMsgList)
                    InsertLogDGV(LogDGV, value);
            }
        }

        /// <summary>
        /// Log LogMsgList의 객체 추가 및 DataGridView Event 발생하여 출력 진행
        /// </summary>
        /// <param name="_LogMsg"></param>
        /// <param name="bLogGridInsert"></param>
        static public void Add(LogMsg _LogMsg, bool bLogGridInsert = true)
        {
            bool bAdd = true;
            lock (LogLock)
            {
                for(int nCount =0; nCount < LogMsgList.Count; nCount++)
                {
                    if((_LogMsg.MsgTitle == LogMsgList[nCount].MsgTitle) &&
                        (_LogMsg.MsgLogType == LogMsgList[nCount].MsgLogType) &&
                        (_LogMsg.MsgDescription == LogMsgList[nCount].MsgDescription))
                    {
                        double TotalSeconds = (_LogMsg.dt - LogMsgList[nCount].dt).TotalSeconds;
                        if (TotalSeconds < 1)
                        {
                            bAdd = false;
                            break;
                        }
                    }
                }

                if (bAdd)
                {
                    if(bLogGridInsert)
                        LogMsgList.Add(_LogMsg);
                    
                    while (LogMsgList.Count > 40)
                    {
                        LogMsgList.RemoveAt(0); //LogMsgList.Count - 1
                    }

                    LogFileWrite(_LogMsg);
                }
            }

            if (bAdd && logInsertEvent != null && bLogGridInsert)
                logInsertEvent(_LogMsg);
        }


        /// <summary>
        /// Log LogMsgList의 모든 로그 클리어
        /// </summary>
        static public void Clear()
        {
            lock (LogLock)
            {
                LogMsgList.Clear();
            }
        }

        /// <summary>
        /// Log가 출력되는 DataGridView를 Clear
        /// </summary>
        /// <param name="LogDGV"></param>
        static private void ClearLogDGV(DataGridView LogDGV)
        {
            if (LogDGV.IsDisposed)
                return;

            if (LogDGV.InvokeRequired)
            {
                LogDGV.Invoke(new MethodInvoker(delegate ()
                {
                    ClearLogDGV(LogDGV);
                }));
            }
            else
            {
                LogDGV.Rows.Clear();
            }
        }

        /// <summary>
        /// AddLog영역에서 logInsertEvent 발생 -> InsertLogDGV가 재 호출 되며 DataGridView에 로그 삽입
        /// </summary>
        /// <param name="LogDGV"></param>
        /// <param name="_LogMsg"></param>
        static public void InsertLogDGV(DataGridView LogDGV, LogMsg _LogMsg)
        {
            if (LogDGV.IsDisposed)
                return;

            if (LogDGV.InvokeRequired)
            {
                LogDGV.Invoke(new MethodInvoker(delegate ()
                {
                    InsertLogDGV(LogDGV, _LogMsg);
                }));
            }
            else
            {
                for (int nCount = 0; nCount < LogDGV.Columns.Count; nCount++)
                {
                    switch (nCount)
                    {
                        case 0:
                            if (LogDGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_GenerateTime"))
                                LogDGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_GenerateTime");
                            break;
                        case 1:
                            if (LogDGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LogType"))
                                LogDGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LogType");
                            break;
                        case 2:
                            if (LogDGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LogLevel"))
                                LogDGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LogLevel");
                            break;
                        case 3:
                            if (LogDGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LogTitle"))
                                LogDGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LogTitle");
                            break;
                        case 4:
                            if (LogDGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LogComment"))
                                LogDGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LogComment");
                            break;
                    }
                }

                if (ShowType[_LogMsg.eLogType] && ShowLevel[_LogMsg.eLogLevel])
                {
                    LogDGV.Rows.Add(new string[] { $"{_LogMsg.dt.ToString("G")}.{_LogMsg.dt.ToString("fff")}", _LogMsg.MsgLogType, _LogMsg.MsgLogLevel, _LogMsg.MsgTitle, _LogMsg.MsgDescription });
                    int LastRow = LogDGV.Rows.Count - 1;

                    if (_LogMsg.eLogLevel == LogMsg.LogLevel.Error)
                    {
                        Font font = new Font(LogDGV.DefaultCellStyle.Font, FontStyle.Bold);
                        LogDGV.Rows[LastRow].Cells[2].Style.Font = font;
                        LogDGV.Rows[LastRow].Cells[2].Style.ForeColor = Color.Red;
                    }
                    else if (_LogMsg.eLogLevel == LogMsg.LogLevel.Warning)
                        LogDGV.Rows[LastRow].Cells[2].Style.ForeColor = Color.Orange;
                    else if (_LogMsg.eLogLevel == LogMsg.LogLevel.Normal)
                        LogDGV.Rows[LastRow].Cells[2].Style.ForeColor = Color.Black;

                    if (LastRow == 0)
                        LogDGV.Rows[LastRow].DefaultCellStyle.BackColor = Color.White;
                    else if(LastRow > 0)
                        LogDGV.Rows[LastRow].DefaultCellStyle.BackColor = LogDGV.Rows[LastRow-1].DefaultCellStyle.BackColor == Color.White ? Color.LightCyan : Color.White;

                    LogDGV.CurrentCell = LogDGV.Rows[LastRow].Cells[0];
                }

                while(LogDGV.Rows.Count > 80)
                {
                    LogDGV.Rows.RemoveAt(0);
                }

                LogDGV.CurrentCell = null;
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Log Type에 따른 로그 경로 얻기
        /// </summary>
        /// <param name="eLogType"></param>
        /// <returns></returns>
        private static string GetLogDirectory(LogMsg.LogType eLogType)
        {
            if (eLogType == LogMsg.LogType.Exception)
                return ManagedFile.ManagedFileInfo.ExceptionLogDirectory;
            else if (eLogType == LogMsg.LogType.RackMaster)
                return ManagedFile.ManagedFileInfo.STKLogDirectory;
            else
                return ManagedFile.ManagedFileInfo.LogDirectory;
        }

        /// <summary>
        /// Log Type에 따른 로그 파일 이름 얻기
        /// </summary>
        /// <param name="eLogType"></param>
        /// <returns></returns>
        private static string GetLogFileName(LogMsg.LogType eLogType)
        {
            if (eLogType == LogMsg.LogType.Exception)
                return ManagedFile.ManagedFileInfo.ExceptionLogFileName;
            else if (eLogType == LogMsg.LogType.RackMaster)
                return ManagedFile.ManagedFileInfo.STKLogFileName;
            else
                return ManagedFile.ManagedFileInfo.LogFileName;
        }

        /// <summary>
        /// Log Type에 따라 지정 경로에 Log 작성 진행
        /// 연, 월, 일, 시간 기준으로 작성 됨
        /// </summary>
        /// <param name="_LogMsg"></param>
        public static void LogFileWrite(LogMsg _LogMsg)
        {
            DateTime dt = _LogMsg.dt;
            string CurrentTime      = dt.ToString("yyyyMMdd_HH_");
            string LogDirectory     = GetLogDirectory(_LogMsg.eLogType);
            string YearDirectory    = LogDirectory + @"\" + dt.ToString("yyyy");
            string MonthDirectory   = YearDirectory + @"\" + dt.ToString("MM");
            string DayDirectory     = MonthDirectory + @"\" + dt.ToString("dd");

            string FileName = CurrentTime + GetLogFileName(_LogMsg.eLogType);
            string FilePath = DayDirectory + @"\" + FileName;


            if (!Directory.Exists(LogDirectory))           //폴더가 없다면
            {
                Directory.CreateDirectory(LogDirectory);   //폴더 생성
            }
            if (!Directory.Exists(YearDirectory))           //폴더가 없다면
            {
                Directory.CreateDirectory(YearDirectory);   //폴더 생성
            }
            if (!Directory.Exists(MonthDirectory))           //폴더가 없다면
            {
                Directory.CreateDirectory(MonthDirectory);   //폴더 생성
            }
            if (!Directory.Exists(DayDirectory))           //폴더가 없다면
            {
                Directory.CreateDirectory(DayDirectory);   //폴더 생성
            }

            using (var fs = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    if (_LogMsg.eLogType == LogMsg.LogType.Exception)
                        sw.WriteLine($"[{dt.ToString("G")}.{dt.ToString("fff")}], [{_LogMsg.MsgLogType}], [{_LogMsg.MsgLogLevel}], [{_LogMsg.MsgDescription}], [{_LogMsg.MsgTitle}], [{_LogMsg.MsgStack}]");
                    else if (!string.IsNullOrEmpty(_LogMsg.MsgDescription))
                        sw.WriteLine($"[{dt.ToString("G")}.{dt.ToString("fff")}], [{_LogMsg.MsgLogType}], [{_LogMsg.MsgLogLevel}], [{_LogMsg.MsgTitle}], [{_LogMsg.MsgDescription}]");
                    else
                        sw.WriteLine($"[{dt.ToString("G")}.{dt.ToString("fff")}], [{_LogMsg.MsgLogType}], [{_LogMsg.MsgLogLevel}], [{_LogMsg.MsgTitle}]");

                    //Flush, Close 처리 하는 경우 열기 영역에서 지연 발생
                    //sw.Flush();
                    //sw.Close();
                }
                //fs.Close();
            }
        }
    }
}
