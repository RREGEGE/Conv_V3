using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.Alarm;

namespace Master.Equipment.RackMaster
{
    /// <summary>
    /// RackMasterAlarm.cs는 STK Alarm 정의 및 처리 관련 동작 작성
    /// </summary>
    public partial class RackMaster
    {
        /// <summary>
        /// STK Word Alarm Array Index Enum
        /// </summary>
        public enum RackMasterAlarmWordMap
        {
            ErrorWord_0,
            ErrorWord_1,
            ErrorWord_2,
            ErrorWord_3,
            ErrorWord_4,
            ErrorWord_5,
            ErrorWord_6,
            ErrorWord_7,
            ErrorWord_8,
            ErrorWord_9,
            ErrorWord_10,
            ErrorWord_11,
            ErrorWord_12
        }
        
        /// <summary>
        /// STK Word Alarm 표
        /// 16개 단위로 WordMap Array에 맵핑
        /// </summary>
        public enum RackMasterAlarmList
        {
            RM_HP_E_Stop = 0x01,
            RM_OP_E_Stop,
            RM_HP_Key_Stop,
            RM_OP_Key_Stop,
            GOT_E_Stop,
            RM_PowerOff_Error,
            MST_HP_EStop,
            MST_OP_EStop,
            MST_HP_Escape,
            MST_OP_Escape,
            HP_Door_Open,
            OP_Door_Open,
            MST_Disconnected,

            Test_Mode_Double_Storage_Error = 0x10,
            Test_Mode_CST_Presence_Error,
            CST_Detect_Sensor_Check_Error,
            CST_Abnormal_Detect_Check_Error,
            Double_Sensor_Error,
            Source_Empty,
            Port_Interface_ULD_Event,
            Port_Interface_LD_Event,

            PLC_Battery_Low = 0x20,
            X_Axis_ServoPack_Battery_Low_Alarm,
            Z_Axis_ServoPack_Battery_Low_Alarm,
            A_Axis_ServoPack_Battery_Low_Alarm,
            T_Axis_ServoPack_Battery_Low_Alarm,

            X_Axis_ServoPack_Error = 0x30,
            X_Axis_POT,
            X_Axis_NOT,
            X_Axis_Origin_Search_Fail,
            X_Axis_Software_Limit,
            X_Axis_Dest_Position,
            X_Axis_Absolute_Origin_Loss,
            X_Axis_Dest_Positioning_Complete,
            X_Axis_OverLoad,
            X_Axis_Home_Sensor_Always_On,
            X_Axis_HomeMove_TimeOut_Error,
            Pick_Position_Error,
            Place_Position_Error,

            X_Axis_Origin_Search_Fail_BCR = 0x40,
            X_Axis_Barcode_Sensor_Error,
            X_Axis_Free_Run,

            Z_Axis_ServoPack_Error = 0x50,
            Z_Axis_POT,
            Z_Axis_NOT,
            Z_Axis_Origin_Search_Fail,
            Z_Axis_Software_Limit,
            Z_Axis_Dest_Position,
            Z_Axis_Absolute_Origin_Loss,
            Z_Axis_Dest_Positioning_Complete,
            Z_Axis_OverLoad,
            Z_Axis_Home_Sensor_Always_On,
            Z_Axis_HomeMove_TimeOut_Error,

            A_Axis_ServoPack_Error = 0x60,
            A_Axis_POT,
            A_Axis_NOT,
            A_Axis_Origin_Search_Fail,
            A_Axis_Software_Limit,
            A_Axis_Dest_Position,
            A_Axis_Absolute_Origin_Loss,
            A_Axis_Dest_Positioning_Complete,
            A_Axis_OverLoad,
            A_Axis_Home_Sensor_Always_On,
            A_Axis_HomeMove_TimeOut_Error,
            RM_Moving_Arm_HomeSensor_Do_Not_Detect,
            Arm_Table_CST_Detect,
            Cassette_Fail,
            Arm_Table_Cross_Sensor,
            Storage_Failure,

            T_Axis_ServoPack_Error = 0x70,
            T_Axis_POT,
            T_Axis_NOT,
            T_Axis_Origin_Search_Fail,
            T_Axis_Software_Limit,
            T_Axis_Dest_Position,
            T_Axis_Absolute_Origin_Loss,
            T_Axis_Dest_Positioning_Complete,
            T_Axis_OverLoad,
            T_Axis_Home_Sensor_Always_On,
            T_Axis_HomeMove_TimeOut_Error,

            Command_ID_Error = 0x80,
            ToMode_Command_Over_Time_Error,
            PIO_Over_Time_Error,
            PIO_Ready_Off_Error,
            Stay_Complete_ACK_Over_Time_Error,
            Complete_ACK_Over_Time_Error,
            Store_ACK_Over_Time_Error,
            Source_Empty_ACK_Over_Time_Error,
            Double_Storage_ACK_Over_Time_Error,
            Resume_Request_ACK_Over_Time_Error,
            Idle_Status_CST_Error,

            Step_TimeOver_Error = 0x91,
            Arm_Position_Sensor1_Detect_Error,
            Arm_Position_Sensor2_Detect_Error,
            Arm_Position_Sensor3_Detect_Error,
            X_Axis_From_To_Software_Limit_Over_Error,
            Z_Axis_From_To_Software_Limit_Over_Error,
            A_Axis_From_To_Software_Limit_Over_Error,
            T_Axis_From_To_Software_Limit_Over_Error
        }

        /// <summary>
        /// STK Alarm Word Map변수이며 STK 객체 할당 시 생성 
        /// </summary>
        WordAlarm[] m_RackMasterWordAlarm;
        
        /// <summary>
        /// 특정 워드 영역의 알람 상태를 가져옴
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public WordAlarm GetAlarmAt(int index)
        {
            if (index < m_RackMasterWordAlarm.Length)
                return m_RackMasterWordAlarm[index];
            else
                return null;
        }

        /// <summary>
        /// STK에서 전달 받은 AlarmWordMap을 객체에 업데이트
        /// </summary>
        public void UpdateAlarmStatus()
        {
            for(int nCount = 0; nCount < m_RackMasterWordAlarm.Length; nCount++)
            {
                int ErrorWordIndex = (int)SendWordMapIndex.ErrorWord_0 + nCount;
                short ErrorCode = (short)Get_RackMaster_2_CIM_Word_Data((SendWordMapIndex)ErrorWordIndex);
                var UpdateResult = m_RackMasterWordAlarm[nCount].Update(ErrorCode);

                if(UpdateResult == WordAlarm.UpdateResult.Clear)
                    LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AlarmClear, $"ErrorWordIndex: {ErrorWordIndex} / Alarm Word: 0x{m_RackMasterWordAlarm[nCount].ClearAlarmWord.ToString("x4")} Clear.");
                else if (UpdateResult == WordAlarm.UpdateResult.Create)
                    LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmCreate, $"ErrorWordIndex: {ErrorWordIndex} / Alarm Word: 0x{ErrorCode.ToString("x4")} Create.");
            }
        }

        /// <summary>
        /// STK Alarm 상태
        /// </summary>
        /// <returns></returns>
        public bool IsAlarmState()
        {
            if (Status_Error)
                return true;

            for (int nCount = 0; nCount < m_RackMasterWordAlarm.Length; nCount++)
            {
                if (m_RackMasterWordAlarm[nCount].AlarmWord != 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// STK Alarm 개수 출력
        /// </summary>
        /// <returns></returns>
        public string GetAlarmCount()
        {
            int nAlarmCount = 0;
            for (int nCount = 0; nCount < m_RackMasterWordAlarm.Length; nCount++)
            {
                if (m_RackMasterWordAlarm[nCount].AlarmWord != 0)
                { 
                    for(int nShift = 0; nShift < 16; nShift++)
                    {
                        if ((short)(m_RackMasterWordAlarm[nCount].AlarmWord & (0x1 << nShift)) == (0x1 << nShift))
                            nAlarmCount++;
                    }
                }
            }

            if (nAlarmCount == 0)
                return "None";
            else
                return $"{nAlarmCount} Alarm";
        }
    }
}
