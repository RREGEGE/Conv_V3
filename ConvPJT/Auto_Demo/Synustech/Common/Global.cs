using Synustech.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using WMX3ApiCLR;
using static Synustech.AlarmList;

namespace Synustech
{
    public enum Safety
    {
        NG,
        OK
    }
    public enum Mode
    {
        Manual,
        Auto,
        Alarm
    }
    public enum Status
    {
        Idle,
        NG
    }
    public enum SensorOnOff
    {
        Off,
        On
    }
    public enum ServoOnOff
    {
        Off,
        On
    }
    public enum AlarmStatus
    {
        OK,
        NG
    }
    public enum InOutMode
    {
        InMode,
        OutMode
    }
    public enum Auto
    {
        Enable,
        Disable
    }
    public enum PIOType
    {
        None,
        AGV,
        OHT,
        Equipment
    }
    internal class G_Var
    {
        // UIFunction 초기화
        public static UIFunction UIFunction = new UIFunction();

        public static event delMonitorLogView del_ELogSender_Global;

        public static AlarmList m_ConveyorAlarmList = new AlarmList();
        public static delAlarmHistory del_alarm;
        public static delLine del_lineAdd;
        public static delParam del_param;
        // Alarm
        //전체 알람을 저장할 테이블
        public static AlarmList totalLogs = new AlarmList();

        public static List<AlarmListParam> alarmCodes = new List<AlarmListParam>();  // 알람 XML 을 읽어올 List 만들기

        public static void AddAlarm_Conveyor(Conveyor conveyor, ConveyorAlarm conveyorAlarm)
        {
            int transForm = (int)conveyorAlarm;
            int ID = conveyor.ID;

            conveyor.m_ConveyorAlarmList.IsAlarmOccur(transForm, ID, conveyor.m_ConveyorAlarmList);
            totalLogs.IsAlarmOccur(transForm, ID, totalLogs);
            //interlock(TransForm); // 인터락 기능 추가 시 사용 
        }
        public static void AddAlarm_Master(ConveyorAlarm masterAlarm)
        {
            int transForm = (int)masterAlarm;
            m_ConveyorAlarmList.IsAlarmOccur_Master(transForm, m_ConveyorAlarmList);
            totalLogs.IsAlarmOccur_Master(transForm, totalLogs);
        }
        public static DataTable ConvertListToDataTable<T>(List<T> list)
        {
            // 새로운 DataTable 생성
            DataTable dataTable = new DataTable(typeof(T).Name);

            // 클래스의 모든 속성(Property) 가져오기
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // DataTable에 열(Column) 추가
            foreach (PropertyInfo property in properties)
            {
                // 속성 이름과 타입으로 DataColumn을 생성 후 추가
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            // 리스트의 각 객체를 DataTable의 행(Row)으로 추가
            foreach (T item in list)
            {
                // 새로운 DataRow 생성
                DataRow row = dataTable.NewRow();
                foreach (PropertyInfo property in properties)
                {
                    // 속성 값을 DataRow의 해당 열에 설정
                    row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                }

                // DataTable에 DataRow 추가
                //dataTable.Rows.Add(row);

                // DataTable의 맨 앞에 DataRow 추가
                dataTable.Rows.InsertAt(row, 0);
            }

            return dataTable;
        }

        // 파일 경로
        public static string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public static string lineParamFilePath = @".\Conv\Setting\LineParam.xml";
        public static string lineFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, lineParamFilePath));
        //public static string lineFullPath = @"C:\Users\240604\Desktop\Conv\Setting\LineParam.xml";

        public static string rectParamFilePath = @".\Conv\Setting\RectParam.xml";
        public static string rectFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, rectParamFilePath));
        //public static string rectFullPath = @"C:\Users\240604\Desktop\Conv\Setting\RectParam.xml";

        public static string convParamFilePath = @".\Conv\Setting\ConvParam.xml";
        public static string convFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, convParamFilePath));
        //public static string convFullPath = @"C:\Users\240604\Desktop\Conv\Setting\ConvParam.xml";

        public static string logFilePath = @".\Conv\Setting\LogParam.xml";
        public static string logFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, logFilePath));

        public static string profileFilePath = @".\Conv\Setting\Profile.xml";
        public static string profileFullPath = Path.GetFullPath(Path.Combine(solutionDirectory,profileFilePath));
        //public static string profileFullPath = @"C:\Users\240604\Desktop\Conv\Setting\Profile.xml";

        public static string paramFilePath = @".\Conv\Setting\wmx_parameters.xml";
        public static string paramFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, paramFilePath));
        //public static string paramFullPath = @"C:\Users\240604\Desktop\Conv\Setting\wmx_parameters.xml";

        public static string alarmFilePath = @".\Conv\Setting\AlarmListParam.xml";
        //public static string alarmFullPath = @"C:\Users\240604\Desktop\Conv\Setting\AlarmListParam.xml";
        public static string alarmFullPath = Path.GetFullPath(Path.Combine(solutionDirectory , alarmFilePath));

        public static string alarmHistoryFilePath = @".\Conv\Conv\AlarmHistory\AlarmHistory.xml";
        public static string alarmHistoryFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, alarmHistoryFilePath));
        //public static string alarmHistoryFullPath = @"C:\Users\240604\Desktop\Conv\AlarmHistory\AlarmHistory.xml";

        //WMX3
        public static WMXMotion w_motion;
        public static CoreMotionStatus cmStatus = new CoreMotionStatus();

        // Error 선언
        public const int MASTER_ALAMR_BYTE_SIZE = 14;
        public const int CONVEYOR_ALARM_BYTE_SIZE = 26;

        // Thread 선언
        public static ThreadControl threadControl = new ThreadControl();
        //public static LampThread lampThread = new LampThread();
        //public static SafetyThread safetyThread = new SafetyThread();
        //public static RectThread rectThread = new RectThread();
        //public static ConvThread convThread = new ConvThread();
        //public static LineThread lineThread = new LineThread();

        // XMLControl 선언
        public static XMLControl xmlControl = new XMLControl();

        // 프로그램 동작 bool 변수
        public static bool isMainFormOpen;
        public static bool isEthercatCommunicate;

        // Cycle 관련 변수
        public static bool stopCycle;
        public static int targetCycle;
        public static int currentCycle;
        public static bool isInput;

        public static bool bMouse;
        public static int totalCycle = 0;
        // Safety 관련 변수
        public static bool isSafety = false;
        public static bool isAutoEnable = false;
        public static bool isAutoRun = false;
        public static bool isCycleRun = false;
        public static bool isAlarm = false;
        public static bool isBuzzerSound = true;

        // 프로그램에 생성된 객체들을 모아놓은 리스트
        public static List<Conveyor> conveyors;              // 컨베이어 리스트
        public static List<CustomRectangle> rectangles;      // 사각형 리스트
        public static List<Line> lines;
        public static CustomRectangle selectedRectangle;     // 선택된 사각형
        public static int? selectedConvID;                   // 선택된 컨베이어의 ID 

        public static byte[] byInput = new byte[255];
        public static byte[] byOutput = new byte[255];

        // Safety Input
        public static SensorOnOff Safety;
        public static SensorOnOff MainPower;
        public static SensorOnOff EMO;
        public static SensorOnOff EMS_1;
        public static SensorOnOff EMS_2;
        public static SensorOnOff Mode_Change;
        public static SensorOnOff[] OHTPIOSend = new SensorOnOff[4];
        public static SensorOnOff[] AGVPIOSend = new SensorOnOff[3];

        // Alarm 변수
        public static bool isSafetyAlarm = false;
        public static bool isMainPowerAlarm = false;
        public static bool isEMOAlarm = false;
        public static bool isEMSAlarm = false;
        public static bool isWMX_E_Stop = false;
        public static bool Software_E_Stop = false;
        public static bool Master_Mode_Change_Error = false;

        // Master Input IO 주소 변수
        public static int addrSafetyIn = 0;
        public static int addrOHTPIO = 2;
        public static int addrAGVPIO = 3;
        public static int addrEMS_1 = 24;
        public static int addrEMS_2 = 56;

        // Master Input IO bit 변수
        public static int bitSafetyIn = 0;
        public static int bitMainPower = 1;
        public static int bitModeChange = 5;
        public static int bitEMO = 6;
        public static int bitEMS = 6;

        // 외부 PIO Input bit 변수
        public static List<int> OHTSendBits = new List<int>() { 0, 1, 3, 6 };
        public static List<int> AGVSendBits = new List<int>() { 0, 1, 2 };

        // Safety OutPut
        public static SensorOnOff SafetyReset;
        public static SensorOnOff[] Lamp_Buzz = new SensorOnOff[8];
        public static SensorOnOff[] OHTPIOReceive = new SensorOnOff[7];
        public static SensorOnOff[] AGVPIOReceive = new SensorOnOff[3];

        //OHT PIO Input
        public static SensorOnOff L_REQ = SensorOnOff.Off;
        public static SensorOnOff UL_REQ = SensorOnOff.Off;
        public static SensorOnOff READY = SensorOnOff.Off;
        public static  SensorOnOff HO_AVBL = SensorOnOff.Off;
        public static SensorOnOff ES = SensorOnOff.Off;

        //OHT PIO Output
        public static SensorOnOff VALID = SensorOnOff.Off;
        public static SensorOnOff CS_0 = SensorOnOff.Off;
        public static SensorOnOff TR_REQ = SensorOnOff.Off;
        public static SensorOnOff BUSY = SensorOnOff.Off;
        public static SensorOnOff COMPT = SensorOnOff.Off;

        // Master Output IO 주소 변수
        public static int addrSafetyReset = 0;
        public static int addrLamp_Buzz = 1;

        // Master Output IO bit 변수
        public static int bitSafetyReset = 3;

        // Lamp 및 외부 PIO Output bit 변수
        public static List<int> bitsLamp = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        public static List<int> OHTReceiveBits = new List<int>() { 0, 1, 2, 4, 5, 6, 7 };
        public static List<int> AGVReceiveBits = new List<int>() { 0, 1, 2 };

        // IO Group
        public enum MyInput
        {
            CST_Detect1 = 2,
            CST_Detect2,
        }
        public enum LongInput
        {
            CST_Detect1 = 2,
            CST_Detect3,
            CST_Detect2 = 6,
        }
        public enum TurnInput
        {
            Home = 6,
            POS1 = 2,
            POS2,
        }
        public enum TurnInput2
        {
            POS3 = 6
        }
        public enum Master
        {
            Safety_On,
            Main_Power,
            Auto_Key = 5,
            EMO = 6,
            EMS_1 = 24,
            EMS_2 = 56
        }
        public enum OHTPIO_Passive
        {
            OHT_PIO_Load_Req,
            OHT_PIO_Unload_Req,
            OHT_PIO_Ready = 3,
            OHT_PIO_HOAVBL = 6,
            OHT_PIO_ES
        }
        public enum AGVPIO_Passive
        {
            AGV_PIO_Load_Req,
            AGV_PIO_Unload_Req,
            AGV_PIO_Ready
        }
        public enum SafetyOut
        {
            SafetyReset = 3
        }
        public enum LampAndBuzz
        {
            Lamp_Red,
            Lamp_Yellow,
            Lamp_Green,
            Lamp_Buzz,
            Buzzer1,
            Buzzer2,
            Buzzer3,
            Buzzer4,
        }
        public enum OHTPIO_Active
        {
            OHT_PIO_Valid,
            OHT_PIO_CS_0,
            OHT_PIO_CS_2,

            OHT_PIO_TR_Req = 4,
            OHT_PIO_Busy,
            OHT_PIO_Comp,
            OHT_PIO_CONT
        }
        public enum AGVPIO_Active
        {
            AGV_PIO_TR_Req,
            AGV_PIO_Busy,
            AGV_PIO_Comp
        }
        /// <summary>
        /// Auto 및 Cycle Mode로 전환 시 실행되는 Initialize 동작 함수
        /// </summary>
        /// <returns></returns>
        public static async Task ConvInit()
        {
            int batchSize = 5;
            foreach(var conv in conveyors)
            {
                conv.initComp = false;
                if(conv.type == "Turn")
                {
                    conv.turn_CV_InitComp = false;
                }
            }
            for (int i = 0; i < conveyors.Count; i += batchSize)
            {
                var turnItems = new List<Conveyor>();
                for (int j = i; j < i + batchSize && j < conveyors.Count; j++)
                {
                    if (conveyors[j].type == "Turn")
                    {
                        conveyors[j].turn_CV_InitComp = false;
                        turnItems.Add(conveyors[j]);
                    }
                }
                // type == "Turn"인 요소가 있다면 Turn_Init() 실행 및 대기
                if (turnItems.Count > 0)
                {
                    foreach (var turnItem in turnItems)
                    {
                        turnItem.Turn_Init();
                        del_ELogSender_Global(enLogType.Application, enLogLevel.Normal, enLogTitle.TurnInit, $"Conveyor ID: {turnItem}");
                    }
                    while (true)
                    {
                        int count = 0;
                        await Task.Delay(100);
                        //if (turnItems.All(turnItem => turnItem.TCV_InitComp))
                        //{
                        //    allTurnInitComplete = true;
                        //    return;
                        //}
                        foreach(var turnItem in turnItems)
                        {
                            if(turnItem.turn_CV_InitComp == false)
                            {
                                count++;
                            }
                            else
                            {
                                del_ELogSender_Global(enLogType.Application, enLogLevel.Normal, enLogTitle.TurnInitDone, $"Conveyor ID: {turnItem}");
                            }
                        }
                        Console.WriteLine(count);
                        if(count == 0) { break; }
                    }
                }
                Console.WriteLine("Turn Done");
                // 모든 요소에 Init() 실행
                for (int j = i; j < i + batchSize && j < conveyors.Count; j++)
                {
                    conveyors[j].initComp = false;
                    conveyors[j].Init();
                    del_ELogSender_Global(enLogType.Application, enLogLevel.Normal, enLogTitle.RollingInit, $"Conveyor ID: {conveyors[j].ID}");
                }
                // Init() 완료 대기
                bool allInitComplete;
                do
                {
                    await Task.Delay(100);  // 100ms 간격으로 상태 확인
                    allInitComplete = true;

                    for (int j = i; j < i + batchSize && j < conveyors.Count; j++)
                    {
                        if (!conveyors[j].initComp)  // 모든 요소의 InitComp 상태 확인
                        {
                            allInitComplete = false;
                            break;
                        }
                    }
                    if (allInitComplete)
                    {
                        for (int j = i; j < i + batchSize && j < conveyors.Count; j++)
                        {
                            del_ELogSender_Global(enLogType.Application, enLogLevel.Normal, enLogTitle.RollingInitDone, $"Conveyor ID: {conveyors[j].ID}");
                        }
                    }
                } while (!allInitComplete);  // 모든 InitComp가 true가 될 때까지 반복
            }
            Console.WriteLine("Init Done");
            foreach (var conv in conveyors)
            {
                conv.initComp = false;
                if (conv.type == "Turn")
                {
                    conv.turn_CV_InitComp = false;
                }
            }
            // Auto 동작
            foreach (var line in lines)
            {
                //line.StartTime = DateTime.Now;
                line.cycleStopWatch.Restart();
                for (int i = 0; i < line.conveyors.Count; i++)
                {
                    if (line.conveyors[i].mode != Mode.Auto)
                    {
                        line.conveyors[i].normal_CV_CycleRunning = true;
                        line.conveyors[i].Auto_Start_CV_Control();
                    }
                }
            }
            Console.WriteLine("asdadfacnh----------------");
            //MonitorConveyors.StartMonitoring();
        }
    }
    // Log 작성
    public enum enLogType
    {
        Application,
        Process

    }
    public enum enLogLevel
    {
        Normal,
        Warning,
        Error,
    }
    public enum enLogTitle
    {
        ButtonClick,
        NcvAutoStep,
        TcvAutoStep,
        LcvAutoStep,
        TurnInit,
        TurnInitDone,
        RollingInit,
        RollingInitDone,
        CycleStep,
        JogPositiveStart,
        JogNegativeStart,
        JogStop,
        TurnCWStart,
        TurnCCWStart,
        TurnStop,
        CycleStart,
        CycleStop,
        CycleEnd
    }
    public enum LoginPassward
    {
        User,
        Admin,
        Maker,
    }
    public enum MotionStatus
    {
        ID,
        Axis,
        Step,
        Home,
        Busy,
        Commend_Vel,
        Actual_Vel,
        Actual_Torque,
    }
    public delegate void delMenuBtnVisible(string buttonName, bool visible);
    public delegate void delLoginlblIdChanged(string text);

    public delegate void delMonitorLogView(enLogType eType1, enLogLevel eLevel, enLogTitle eTitle1, string strLog);

    public delegate void delMonitorRemove();
    public delegate void delMonitorMain();
    public delegate void delMonitorStatus();
    public delegate void delMonitorMotion();
    public delegate void delMonitorAlarm();
    public delegate void delMonitorTeach();
    public delegate void delMonitorChange();
    public delegate void delAlarmHistory();

    public delegate void delLine(List<Synustech.Line> lines);
    public delegate void delParam();

    class StopWatchFunc
    {
        /// <summary>
        /// Stop Watch 경과 시간 출력
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        static public string GetRunningTime(Stopwatch st)
        {
            var RunningTime = st.Elapsed;

            return $"{RunningTime.TotalHours.ToString("00")}:{RunningTime.Minutes.ToString("00")}:{RunningTime.Seconds.ToString("00")}";
        }
    }
}