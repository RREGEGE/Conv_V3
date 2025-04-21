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
    // <summary>
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
    internal class G_Var
    {
        public static AlarmList m_ConveyorAlarmList = new AlarmList();

        // 파일 경로
        public static string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public static string LineParamFilePath = @".\Conv\Setting\LineParam.xml";
        //public static string LineFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, LineParamFilePath));
        //public static string LineFullPath = @"C:\Users\240604\source\repos\ConPJT\ConvPJT\Auto\Setting\LineParam.xml";
        public static string LineFullPath = @"C:\Users\240604\Desktop\241105_Merge\Setting\LineParam.xml";

        public static string RectParamFilePath = @".\Conv\Setting\RectParam.xml";
        //public static string RectFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, RectParamFilePath));
        //public static string RectFullPath = @"C:\Users\240604\source\repos\ConPJT\ConvPJT\Auto\Setting\RectParam.xml";
        public static string RectFullPath = @"C:\Users\240604\Desktop\241105_Merge\Setting\RectParam.xml";

        public static string ConvParamFilePath = @".\Conv\Setting\ConvParam.xml";
        //public static string ConvFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, ConvParamFilePath));
        //public static string ConvFullPath = @"C:\Users\240604\source\repos\ConPJT\ConvPJT\Auto\Setting\ConvParam.xml";
        public static string ConvFullPath = @"C:\Users\240604\Desktop\241105_Merge\Setting\ConvParam.xml";

        public static string LogFilePath = @".\Conv\Setting\LogParam.xml";
        public static string LogFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, LogFilePath));

        public static string ProfileFilePath = @".\Conv\Setting\Profile.xml";
        //public static string ProfileFullPath = Path.GetFullPath(Path.Combine(solutionDirectory,ProfileFilePath));
        public static string ProfileFullPath = @"C:\Users\240604\Desktop\241105_Merge\Setting\Profile.xml";

        public static string ParamFilePath = @".\Conv\Setting\wmx_parameters.xml";
        //public static string ParamFullPath = Path.GetFullPath(Path.Combine(solutionDirectory, ParamFilePath));
        public static string ParamFullPath = @"C:\Users\240604\Desktop\241105_Merge\Setting\wmx_parameters.xml";

        public static WMXMotion m_WMXMotion;
        public static CoreMotionStatus cmStatus = new CoreMotionStatus();
        // Error 선언
        public const int MASTER_ALAMR_BYTE_SIZE = 14;
        public const int CONVEYOR_ALARM_BYTE_SIZE = 26;


        // Thread 선언
        public static AnyThread iOCheck = new AnyThread();
        public static LampThread lampThread = new LampThread();
        public static SafetyThread safetyThread = new SafetyThread();
        public static RectThread rectThread = new RectThread();
        public static ConvThread convThread = new ConvThread();
        public static LineThread lineThread = new LineThread();

        public static XMLControl _xml = new XMLControl();

        public static bool IsMainForm;
        public static bool bThreadEnd;
        public static bool bIOThreadEnd;
        public static bool AnalogData_Check;
        public static bool bRun;
        public static bool bOupIoRun;
        public static bool IsCommunication;

        public static bool bMouse;
        public static bool StepDrection = false;

        public static int nStep;

        public static bool IsSafety = false;
        public static bool IsAuto = false;
        public static bool IsAutoRun = false;
        public static bool IsBuzzerSound = true;

        public static int? selectedConvID;
        public static bool auto;
        public static int LIneNum = 1;

        public static List<Conveyor> conveyors;              // 컨베이어 리스트
        public static List<CustomRectangle> rectangles;      // 사각형 리스크
        public static List<Line> lines;
        public static CustomRectangle selectedRectangle;     // 선택된 사각형


        public static byte[] byInput = new byte[255];
        public static byte[] byOutput = new byte[255];

        // Safety InPut
        public static SensorOnOff Safety;
        public static SensorOnOff MainPower;
        public static SensorOnOff EMO;
        public static SensorOnOff EMS_1;
        public static SensorOnOff EMS_2;
        public static SensorOnOff[] OHTPIOin = new SensorOnOff[4];
        public static SensorOnOff[] AGVPIOin = new SensorOnOff[3];

        public static int addrSafetyin = 0;
        public static int addrOHTin = 2;
        public static int addrAGVin = 3;
        public static int addrEMS_1 = 22;
        public static int addrEMS_2 = 38;

        public static int Safetyinbit = 0;
        public static int MainPowerbit = 1;
        public static int EMObit = 6;
        public static List<int> OHTINbits = new List<int>() { 0, 1, 3, 6 };
        public static List<int> AGVINbits = new List<int>() { 0, 1, 2 };

        //Safety OutPut
        public static SensorOnOff SafetyReset;
        public static SensorOnOff[] Lamp_Buzz = new SensorOnOff[8];
        public static SensorOnOff[] OHTpio = new SensorOnOff[7];
        public static SensorOnOff[] AGVpio = new SensorOnOff[3];

        public static int addrSafety = 0;
        public static int addrLamp_Buzz = 1;
        public static int addrOHTPIO = 2;
        public static int addrAGVPIO = 3;

        public static int bitSafety = 3;
        public static int bitEMS = 2;

        //public static bool EMS_1;
        //public static bool EMS_2;

        public static List<int> Lampbits = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        public static List<int> OHTbits = new List<int>() { 0, 1, 2, 4, 5, 6, 7 };
        public static List<int> AGVbits = new List<int>() { 0, 1, 2 };

        public static bool Isauto = false;
        public static bool IsManual = false;
        public static bool IsAlarm = false;
        // IO Group
        public enum MyInput
        {
            Foup_Detect1 = 2,
            Foup_Detect2,
        }
        public enum LongInput
        {
            Foup_Detect1 = 2,
            Foup_Detect3,
            Foup_Detect2 = 6,
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
            EMO = 6,
            EMS_1 = 22,
            EMS_2 = 38
        }
        public enum OHTPIO_in
        {
            OHT_PIO_Load_Req,
            OHT_Unload_Req,
            OHT_PIO_Ready = 3,
            OHT_PIO_HOAVBL = 6
        }
        public enum AGVPIO_in
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

        public enum OHTPIO_out
        {
            OHT_PIO_Valid,
            OHT_PIO_CS_0,
            OHT_PIO_CS_2,

            OHT_PIO_TR_Req = 4,
            OHT_PIO_Busy,
            OHT_PIO_Comp,
            OHT_PIO_CONT
        }
        public enum AGVPIO_out
        {
            AGV_PIO_TR_Req,
            AGV_PIO_Busy,
            AGV_PIO_Comp
        }

        public enum MyOutput
        {
            DIGITAL_OUTPUT_00 = 0,
            DIGITAL_OUTPUT_01,
            DIGITAL_OUTPUT_02,
            DIGITAL_OUTPUT_03,
            DIGITAL_OUTPUT_04,
            DIGITAL_OUTPUT_05,
            DIGITAL_OUTPUT_06,
            DIGITAL_OUTPUT_07,
            DIGITAL_OUTPUT_08,
            DIGITAL_OUTPUT_09,
            DIGITAL_OUTPUT_10,
            DIGITAL_OUTPUT_11,
            DIGITAL_OUTPUT_12,
            DIGITAL_OUTPUT_13,
            DIGITAL_OUTPUT_14,
            DIGITAL_OUTPUT_15,
        }

        // Analog Data
        public static bool bAnalogIoRun;
        public static bool bAnalogIOThreadEnd;

        public static byte[] inputData_Ch1 = new byte[2];
        public static byte[] inputData_Ch2 = new byte[2];

        public static int AXIS = 0;
        public static int SERVO_ON = 1;
        public static int SERVO_OFF = 0;

    }
    public class MyProfile
    {
        public double dVelocity;
        public double dAcc;
        public double dDec;
        public double dJerkAccRatio;
        public double dJerkDecRatio;
    }

    public class ServoMonitor
    {
        public bool bServoon;
        public bool bAlarm;
        public bool bPls;
        public bool bNls;
        public bool bOrgine;
        public bool bHomedone;


        public double dCmdpos;
        public double dFedpos;
        public double dCmdvel;
        public double dFedvel;
        public double dCmdtrq;
        public double dFedtrq;
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
        CycleStep
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




    public delegate void delMonitorLogView(object oSender, enLogType eType1, enLogLevel eLevel, enLogTitle eTitle1, string strLog);
    public delegate void delMonitorRemove();
    public delegate void delMonitorMain();
    public delegate void delMonitorStatus();
    public delegate void delMonitorMotion();
    public delegate void delMonitorAlarm();
    public delegate void delMonitorTeach();
    public delegate void delMonitorChange();

}