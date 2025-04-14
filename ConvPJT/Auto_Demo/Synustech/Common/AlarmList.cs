using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMX3ApiCLR;
using static Synustech.G_Var;
using System.IO;

namespace Synustech
{
    public enum AlarmLevel
    {
        None,
        Warning,
        Error
    }
    //추후 알람리스트 분리 등 필요할 때 사용
    //public enum AlarmType 
    //{
    //    Master,
    //    Conveyor,

    //    MAX_COUNT,
    //}
    public enum ConveyorAlarm : int
    {
        // Global 부분
        MainPanel_EMO = 0x00,
        Conveyor_E_Stop,
        WMX_E_Stop,
        Software_E_Stop,
        Power_Off_Error,
        Master_Mode_Change_Error,
        //CNV 부분
        Power_Off = 0x20,
        Disconnection_EtherCAT_Error,
        EtherCAT_Connecting_Fail,
        Over_Current,
        Over_Voltage,
        Open_Temperature,
        Oper_Motor_Winding,
        Internal_Voltage_Bad,
        Position_Limit,
        Voltage_Low,
        Current_limit,
        Step_Drive_Error,
        CST_Empty = 0x30,
        Unkown_CST_Detected,
        IN_Step_Time_Over_Error,
        OUT_Step_Time_Over_Error,
        STEP1_Error_Detect,
        Process_Run_Time_Over_Error,
        //Turn 부분
        Origin_Search_Fail = 0x40,
        HomeSensor_Always_On,
        Move_To_Home_Timeout,
        Move_To_POS1_Timeout,
        Move_To_POS2_Timeout,
        Move_To_POS3_Timeout,
        Soft_POT_detection_,
        Soft_NOT_detection,
        Command_exceeded_Soft_POT_,
        Command_exceeded_Soft_NOT_,
        No_Teaching_Value,
        Sensor_not_detected,
        Teaching_value_error,
        CST_Over_Run,
        Turn_Step_Time_Over_Error,
        //Interface 부분
        Reading_CheckSum_Error = 0x50,
        Reading_ID_Error,
        Reading_Tag_not_recognized,
        Reading_Tag_type_error,
        Reading_Tag_Data_CheckSum_Error,
        Reading_Tag_communication_error,
        Reading_Antenna_Error,
        Light_Curtain_Detect_Error,
        Hoist_Detect_Error,
        PIO1_Valid_Wait_Time_Out_Error,
        PIO1_Busy_Wait_Time_Out_Error,
        PIO1_Busy_ON_Time_Out_Error,
        PIO1_Complete_Wait_Time_Out_Error,
        PIO1,
        PIO2_PIO_Ready_Wait_Time_Out_Error,
        PIO2_PIO_Ready_Off_Time_Out_Error,
        PIO2_Port_Error_On_While_Busy_Error,
        PIO2_Port_Ready_Off_While_Busy_Error,
        PIO2,

        NormalConv = 0x80,
        LongConv,
        TurnConv,
    }
    public class AlarmList
    {
        int ID;

        public AlarmList(int _ID)
        {
            ID = _ID;
        }

        public AlarmList()
        {

        }

        /// <summary>
        /// 알람을 저장하기 위한 List 인스턴스
        /// </summary>
        List<AlarmListParam> m_AlarmListParam = new List<AlarmListParam>();

        /// <summary>
        /// 발생한 알람을 저장하기 위한 Parameter 
        /// </summary>
        public class AlarmListParam
        {
            public string time { get; set; }
            public int code { get; set; } = 0;
            public string comment { get; set; }
            public string solution { get; set; }
            public AlarmLevel level { get; set; }
            public string EQ { get; set; }

            public bool bClear;
            public string ClearTime;


            public AlarmListParam(int _AlarmCode, string _AlarmComment, AlarmLevel _eAlarmLevel, string _AlarmEquipment, int? _ID)
            {
                time = DateTime.Now.ToString("MM.dd HH:mm:ss");
                code = _AlarmCode;
                comment = _AlarmComment;
                level = _eAlarmLevel;
                EQ = _AlarmEquipment + _ID;

            }

            public AlarmListParam()
            {

            }
        }
        /// <summary>
        /// 발생한 알람을 List에 저장
        /// </summary>
        /// <param name="_AlarmCode"></param>
        /// <param name="_AlarmComment"></param>
        /// <param name="_eAlarmLevel"></param>
        /// <returns></returns>
        public bool IsAlarmOccur(int _AlarmCode, int ID, AlarmList total)
        {
            try
            {
                foreach (AlarmListParam OccurAlarm in alarmCodes)
                {
                    if (OccurAlarm.code == _AlarmCode)
                    {
                        AlarmListParam SettingCode = new AlarmListParam(OccurAlarm.code, OccurAlarm.comment, OccurAlarm.level, OccurAlarm.EQ, ID);
                        m_AlarmListParam.Add(SettingCode);

                        if (total == G_Var.totalLogs)
                        { AppendAlarmToFile(SettingCode); }
                        break;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool IsAlarmOccur_Master(int _AlarmCode, AlarmList total)
        {
            try
            {
                foreach (AlarmListParam OccurAlarm in alarmCodes)
                {
                    if (OccurAlarm.code == _AlarmCode)
                    {
                        AlarmListParam SettingCode = new AlarmListParam(OccurAlarm.code, OccurAlarm.comment, OccurAlarm.level, OccurAlarm.EQ, null);
                        m_AlarmListParam.Add(SettingCode);

                        if (total == G_Var.totalLogs)
                        { AppendAlarmToFile(SettingCode); }
                        break;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void AppendAlarmToFile(AlarmListParam alarm)
        {
            string filePath = alarmHistoryFullPath;
            string line = $"{alarm.time}, {alarm.code}, {alarm.comment}, {alarm.level}, {alarm.EQ}";

            var alarms = new List<string>();
            if (File.Exists(filePath))
            {
                // 파일에서 기존 데이터를 읽어옴
                alarms = File.ReadAllLines(filePath, Encoding.UTF8).ToList();
            }

            // Step 2: 새 알람을 추가
            alarms.Add(line);

            // Step 3: 알람 리스트를 시간 순으로 정렬
            var sortedAlarms = alarms.OrderByDescending(x => DateTime.Parse(x.Split(',')[0])).ToList();

            // Step 4: 정렬된 알람들을 텍스트 파일에 저장
            File.WriteAllLines(filePath, sortedAlarms, Encoding.UTF8);

            //// 파일에 문자열 추가
            //File.AppendAllText(filePath, line + Environment.NewLine, Encoding.UTF8);
        }
        /// <summary>
        /// List에 저장된 알람을 전체 삭제
        /// </summary>
        public void ClearAlarm()
        {
            m_AlarmListParam.Clear();
        }

        // List의 알람을 Return 합니다.
        public List<AlarmListParam> GetAlarmList()
        {
            return m_AlarmListParam;
        }

        public string AlarmSolutionPrint(int alarmCode)
        {
            switch (alarmCode)
            {
                case 48:
                    return "- Verify data and physical consistency\r\n- Check CST Detect Sensor input status";
                case 50:
                    return "- Verify data and physical consistency\r\n- Check CST Detect Sensor input status\r\n- Check motor status";
                case 51:
                    return "- Verify data and physical consistency\r\n- Check CST Detect Sensor input status\r\n- Check motor status";
                case 77:
                    return "- Verify data and physical consistency\r\n- Check CST Detect Sensor input status";
                case 78:
                    return "- Check for motor abnormalities and modify WatchDog";
                default:
                    return "Error";
            }
        }
    }
}
