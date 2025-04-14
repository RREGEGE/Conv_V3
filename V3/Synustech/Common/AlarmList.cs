using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMX3ApiCLR;


namespace Synustech
{
    /// <summary>
    /// AlarmType을 정의한다.
    /// </summary>
    public enum AlarmType
    {
        MasterAlarm,
        ConveyorAlarm,
        MasterWarning,
        ConveyorWarning,

        MAX_COUNT,
    }
    /// <summary>
    /// 각 Part에 맞는 AlarmList를 정의 한다.
    /// </summary>
    public enum MasterAlarmList
    {
        //HP_Door_EStop = 1,
        //OP_Door_EStop,
        Auto_Key_Stop,

        Main_EStop,
        Line_EStop_01,
        Line_EStop_02,
        //MC_On_Time_Out,
        Master_Mode_Change_Error,
        EtherCAT_Communication_Error,

        MAX_COUNT,
    }

    public enum ConveyorAlarmList
    {
        EStop = 1,
        Key_Stop,
        Power_Off_Error,

        Test_Mode_CST_Presence_Error,

        Line_Interface_ULD_Event,
        Line_Interface_LD_Event,
     
        Cassette_Fail,


        T_Axis_ServoPack_Error,
        T_Axis_POT,
        T_Axis_NOT,
        T_Axis_Origin_Search_Fail,
        T_Axis_Software_Limit_Error,
        T_Axis_Dest_Position_Encoder_Error,
        T_Axis_Absolute_Origin_Loss,
        T_Axis_Dest_Positioning_Complete_Error,
        T_Axis_OverLoad_Error,
        T_Axis_HomeSensor_Always_On_Error,
        T_Home_Move_Time_Out_Error,
      
        MAX_COUNT,
    }
    public static class AlarmList
    {
        private static string[] m_masterAlarmList = new string[(int)MasterAlarmList.MAX_COUNT];
        private static string[] ConveryorAlarmList = new string[(int)MasterAlarmList.MAX_COUNT];
        public static void SetAlarmList(AlarmType eAlarmType, int index, string sValue)
        {
            switch (eAlarmType)
            {
                case AlarmType.MasterAlarm:
                    m_masterAlarmList[index] = sValue;
                    break;

                case AlarmType.ConveyorAlarm:
                    ConveryorAlarmList[index] = sValue;
                    break;
            }
        }

        public static string GetAlarmDescription(AlarmType eAlarmType, int index)
        {
            string value = "";

            switch (eAlarmType)
            {
                case AlarmType.MasterAlarm:
                    value = m_masterAlarmList[index];
                    break;

                case AlarmType.ConveyorAlarm:
                    value = ConveryorAlarmList[index];
                    break;
            }

            return value;
        }
    }

    /// <summary>
    /// List class
    /// </summary>
    public static class Alarm
    {
        private static List<int> m_masterList   = new List<int>();
        private static List<int> m_conveyorList = new List<int>();

        private static byte[] m_masterByteArray    = new byte[G_Var.MASTER_ALAMR_BYTE_SIZE];
        private static byte[] m_conveyorByteArray  = new byte[G_Var.CONVEYOR_ALARM_BYTE_SIZE];
        private static BitArray m_masterBitArray   = new BitArray(m_masterByteArray);
        private static BitArray m_conveyorBitArray = new BitArray(m_conveyorByteArray);

        private static AlarmType m_curType;
        private static int m_curAlarm;

        public static void AddAlarm(AlarmType eAlarmType, int nNo)
        {
            if (0 > nNo)
                return;
            string strMsg = "[" + eAlarmType.ToString() + "]" + "Error Code : " + nNo.ToString() + " Error Desc : "
                + AlarmList.GetAlarmDescription(eAlarmType, nNo);

            switch (eAlarmType)
            {
                case AlarmType.MasterAlarm:
                    if (!m_masterList.Contains(nNo))
                    {
                        m_masterList.Add(nNo);
                        //이 부분에 로그 추가
                        m_masterBitArray[nNo] = true;
                    }
                    break;

                case AlarmType.ConveyorAlarm:
                    if (!m_conveyorList.Contains(nNo))
                    {
                        m_conveyorList.Add(nNo);
                        //이 부분에 로그 추가
                        m_conveyorBitArray[nNo] = true;
                    }
                    break;

           
            }

            m_curType = eAlarmType;
            m_curAlarm = nNo;

            return;
        }

        //public static void GetErrorShort(AlarmType eType, out short outVal)
        //{
        //    outVal = 0;

        //}

        public static void DelAll()
        {
            m_masterList.Clear();
            m_conveyorList.Clear();

            return;
        }

        public static int GetCnt(AlarmType eAlarmType)
        {
            switch (eAlarmType)
            {
                case AlarmType.MasterAlarm:
                    return m_masterList.Count;

                case AlarmType.ConveyorAlarm:
                    return m_conveyorList.Count;

            }

            return 0;
        }

        public static int Get(AlarmType eAlarmType, int index)
        {
            switch (eAlarmType)
            {
                case AlarmType.MasterAlarm:
                    return m_masterList[index];

                case AlarmType.ConveyorAlarm:
                    return m_conveyorList[index];
            }

            return 0;
        }

        public static AlarmType GetCurrentAlarmType()
        {
            return m_curType;
        }

        public static int GetCurrentAlarm()
        {
            return m_curAlarm;
        }

        public static short GetAlarmBit()
        {
            return (short)m_curAlarm;
        }
    }
}
