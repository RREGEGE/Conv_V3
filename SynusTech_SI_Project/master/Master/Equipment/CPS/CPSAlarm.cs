using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.Alarm;
using Master.Interface.Math;

namespace Master.Equipment.CPS
{
    /// <summary>
    /// CPSAlarm.cs는 CPS Alarm 정의 및 처리 관련 동작 작성
    /// </summary>
    public partial class CPS
    {
        /// <summary>
        /// Master -> CIM WordMap중 CPS MemoryMap 수 만큼 Enum 정의
        /// </summary>
        public enum CPSAlarmWordMap
        {
            ErrorWord_0,
            ErrorWord_1,
            ErrorWord_2,
            ErrorWord_3,
            ErrorWord_4,
            ErrorWord_5,
            ErrorWord_6
        }

        /// <summary>
        /// CPS ErrorWord의 Code값 정의
        /// </summary>
        public enum CPSAlarmList
        {
            HeatSink_OverHeat_60 = 0x01,
            Panel_OverTemp,
            Diode_OverHeat_Warning,
            IGBT_OverHeat_Warning,
            RS232_Comm_Fail = 19,

            Erec_PeakVoltage = 33,
            Erec_OverVoltage,
            Erec_UnderVoltage,

            IBoost_PeakCurrent = 42,
            IBoost_OverCurrent,

            Isrc_PeakCurrent = 48,
            Isrc_OverCurrent,
            Isrc_UnderCurrent,

            Iout_PeackCurrent = 54,
            Iout_OverCurrent,

            Inv1_Gate_Fault = 59,
            Inv2_Gate_Fault,
            Initial_Charge_Fail,
            EMO_Stop_Int,
            EMO_Stop_Ext,

            Fuse_Open = 65,
            Sag_Gen_Fault,
            Main_Fan_Fault,
            Heatsink_OverHeat_80,
            IGBT_OverHeat,
            Track_Cable_OverHeat,
            Inside_Cable_OverHeat,
            Busbar_OverHeat,

            Regulator_Fault = 78,
            EEPROM_AD_Check_Sum_Fail,
            EEPROM_Sys_Check_Sum_Fail,
            EEPROM_Gain_Check_Sum_Fail,
            Watchdog_Fault,

            Input_MC1_Open = 100,
            Input_MC2_Open,

            Cap_OverHeat = 105,
            Diode_OverHeat = 107
        }

        WordAlarm[] m_CPSWordAlarm = new WordAlarm[]
        {
            new WordAlarm(),
            new WordAlarm(),
            new WordAlarm(),
            new WordAlarm(),
            new WordAlarm(),
            new WordAlarm(),
            new WordAlarm()
        };

        /// <summary>
        /// 특정 WordMap의 Alarm 정보 얻기
        /// </summary>
        /// <param name="WordMapIndex"></param>
        /// <returns></returns>
        public WordAlarm GetWordAlarm(int WordMapIndex)
        {
            return m_CPSWordAlarm[WordMapIndex];
        }

        /// <summary>
        /// /// 특정 WordMap의 Alarm 정보 얻기
        /// </summary>
        /// <param name="eCPSAlarmWordMap"></param>
        /// <returns></returns>
        public WordAlarm GetWordAlarm(CPSAlarmWordMap eCPSAlarmWordMap)
        {
            return m_CPSWordAlarm[(int)eCPSAlarmWordMap];
        }

        /// <summary>
        /// Data Packet 수신 후 파싱 작업 완료하고나서 Alarm Check 진행
        /// </summary>
        public void AlarmCheck()
        {
            try
            {
                short nErrorCode = (short)Get_CPS_Data(PacketStruct.Converter_Error_Code);
                int WordIndex = (int)(nErrorCode / 16);
                int BitIndex = (int)(nErrorCode % 16);

                for (int nCount = 0; nCount < m_CPSWordAlarm.Length; nCount++)
                {
                    short AlarmWord = 0;
                    //1. Alarm Word 값 업데이트
                    BitOperation.SetBit(ref AlarmWord, BitIndex, true);

                    //2. ErrorCode의 WordIndex와 루프의 WordMap Index가 같은 경우 업데이트 진행
                    var UpdateResult = m_CPSWordAlarm[nCount].Update(WordIndex == nCount ? AlarmWord : (short)0);

                    //3.Update 결과에 따른 로그 작성
                    if (UpdateResult == WordAlarm.UpdateResult.Clear)
                        LogMsg.AddCPSLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.AlarmClear, $"ErrorWordIndex: {WordIndex} / Alarm Word: 0x{m_CPSWordAlarm[nCount].ClearAlarmWord.ToString("x4")} Clear.");
                    else if (UpdateResult == WordAlarm.UpdateResult.Create)
                    {
                        string AlarmText = Enum.IsDefined(typeof(CPS.CPSAlarmList), (WordIndex * 16 + BitIndex)) ? $"{(CPS.CPSAlarmList)(WordIndex * 16 + BitIndex)}" : "Reserve";
                        if (WordIndex == 0 &&
                            (BitIndex == (int)CPSAlarmList.HeatSink_OverHeat_60 ||
                            BitIndex == (int)CPSAlarmList.Panel_OverTemp ||
                            BitIndex == (int)CPSAlarmList.Diode_OverHeat_Warning ||
                            BitIndex == (int)CPSAlarmList.IGBT_OverHeat_Warning))
                        {
                            LogMsg.AddCPSLog(LogMsg.LogLevel.Warning, LogMsg.MsgList.AlarmCreate, $"ErrorWordIndex: {WordIndex} / Alarm Word: 0x{AlarmWord.ToString("x4")} / {AlarmText} Create.");
                        }
                        else if (WordIndex == 1 &&
                                (BitIndex + 16) == (int)CPSAlarmList.RS232_Comm_Fail)
                        {
                            LogMsg.AddCPSLog(LogMsg.LogLevel.Warning, LogMsg.MsgList.AlarmCreate, $"ErrorWordIndex: {WordIndex} / Alarm Word: 0x{AlarmWord.ToString("x4")} / {AlarmText} Create.");
                        }
                        else
                            LogMsg.AddCPSLog(LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmCreate, $"ErrorWordIndex: {WordIndex} / Alarm Word: 0x{AlarmWord.ToString("x4")} / {AlarmText} Create.");
                    }
                }

                //4. Master -> CIM Memory Map에 업데이트된 Word Value 적용
                Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_ErrorWord0, m_CPSWordAlarm[0].AlarmWord);
                Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_ErrorWord1, m_CPSWordAlarm[1].AlarmWord);
                Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_ErrorWord2, m_CPSWordAlarm[2].AlarmWord);
                Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_ErrorWord3, m_CPSWordAlarm[3].AlarmWord);
                Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_ErrorWord4, m_CPSWordAlarm[4].AlarmWord);
                Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_ErrorWord5, m_CPSWordAlarm[5].AlarmWord);
                Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_ErrorWord6, m_CPSWordAlarm[6].AlarmWord);

            }
            catch
            {

            }
        }
    }
}
