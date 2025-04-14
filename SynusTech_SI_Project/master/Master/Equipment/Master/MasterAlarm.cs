using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Master.Equipment.RackMaster;
using Master.Equipment.CIM;
using Master.Interface.Alarm;
using MovenCore;
using Master.Interface.Watchdog;
using Master.Interface.Math;

namespace Master
{
    /// <summary>
    /// MasterAlarm.cs는 Master Alarm 정의 및 처리 관련 동작 작성
    /// </summary>
    static partial class Master
    {
        /// <summary>
        /// Master Word Alarm Array Index Enum
        /// </summary>
        public enum MasterAlarmWordMap
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
        /// Master Word Alarm 표
        /// 16개 단위로 WordMap Array에 맵핑
        /// </summary>
        public enum MasterAlarm : int
        {
            None,
            HP_Door_E_Stop,
            OP_Door_E_Stop,
            HP_STK_Key_Stop,
            OP_STK_Key_Stop,
            HP_Door_Open,
            OP_Door_Open,
            MC_On_Time_Out,
            Master_Mode_Change_Error = 0x08,

            Maint_Door_Open = 0x0e,

            CIM_E_Stop = 0x13,
            Port_Handy_Touch_E_Stop,
            STK_HP_Handy_Touch_E_Stop,
            STK_OP_Handy_Touch_E_Stop,
            HP_Handy_Touch_E_Stop,
            OP_Handy_Touch_E_Stop,

            RM_Watch_Dog_Time_Out = 0x20,
            CIM_Watch_Dog_Time_Over,
            RM_TCP_Disconnection,
            EtherCAT_Communication_Error,
            Slave_Not_Op_State_Error,
            WMX_Engine_Stop_Error,

            CPS_Run_Error = 0x31,
            RM_Regulator_Fault,

            HP_Escape_E_Stop = 0x40,
            OP_Escape_E_Stop,

            Inner_EMO_1 = 0x50,
            Inner_EMO_2,
            Inner_EMO_3,
            Inner_EMO_4,

        }

        static public Watchdog RMWatchDog   = new Watchdog(10000);  //STK 통신 시 10sec 이상 수신하지 못하는 경우 WatchDog Alarm
        static public Watchdog CIMWatchDog  = new Watchdog(10000);  //CIM 통신 시 와치독으로 활용하려 했으나 CIM에서 상시 보내주는 패킷이 없음

        static Watchdog m_HPDTPDelay        = new Watchdog(500);    //HP DTP EMO 인식 시 해당 초 이상 감지되어야 알람 발생(꽂는 순간 알람 방지)
        static Watchdog m_OPDTPDelay        = new Watchdog(500);    //OP DTP EMO 인식 시 해당 초 이상 감지되어야 알람 발생(꽂는 순간 알람 방지)
        static Watchdog m_PortDTPDelay      = new Watchdog(500);    //Port DTP EMO 인식 시 해당 초 이상 감지되어야 알람 발생(꽂는 순간 알람 방지)

        /// <summary>
        /// Master Alarm Word Map 배열
        /// </summary>
        static WordAlarm[] m_MasterWordAlarm = new WordAlarm[]
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
        /// Master Alarm Update 스레드
        /// </summary>
        static private Thread m_MasterAlarmUpdateThread = new Thread(MasterAlarmUpdateThread);
        
        /// <summary>
        /// Master Alarm Check 스레드
        /// </summary>
        static private void MasterAlarmUpdateThread()
        {
            while (true)
            {
                //1. Alarm Check 진행
                AlarmCheck();
                //2. Check 후 Word Map에 반영
                AlarmListWordMapUpdate();

                Thread.Sleep(Master.StatusUpdateTime);
            }
        }
        
        /// <summary>
        /// Alarm Word map의 모든 값을 초기화
        /// </summary>
        static public void AlarmAllClear()
        {
            foreach (MasterAlarm eMasterAlarm in Enum.GetValues(typeof(MasterAlarm)))
            {
                AlarmClear(eMasterAlarm);
            }
        }

        /// <summary>
        /// 지정한 Alarm을 초기화 (조건에 의한 특정 알람 자동 클리어 기능 활용 위함)
        /// </summary>
        /// <param name="eMasterAlarm"></param>
        /// <returns></returns>
        static public bool AlarmClear(MasterAlarm eMasterAlarm)
        {
            int WordIndex = (int)eMasterAlarm / 16;
            int BitIndex = (int)eMasterAlarm % 16;

            short AlarmWord = m_MasterWordAlarm[WordIndex].AlarmWord;

            if (BitOperation.GetBit(AlarmWord, BitIndex))
            {
                if(eMasterAlarm == MasterAlarm.RM_Watch_Dog_Time_Out)
                {
                    foreach (var rackmaster in m_RackMasters)
                    {
                        if (rackmaster.Value.IsConnected())
                        {
                            RMWatchDog.ReStartWatchdog();
                        }
                        else
                        {
                            RMWatchDog.StopWatchdog(true);
                        }
                        break;
                    }
                }
                BitOperation.SetBit(ref AlarmWord, BitIndex, false);
                m_MasterWordAlarm[WordIndex].AlarmWord = AlarmWord;

                if(m_MasterWordAlarm[WordIndex].AlarmWord == 0)
                    m_MasterWordAlarm[WordIndex].ClearTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
                LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.AlarmClear, $"Alarm Word[{WordIndex}]: {m_MasterWordAlarm[WordIndex].AlarmWord.ToString("x4")} / Alarm Name: {eMasterAlarm} Clear.");


                if (!Check_RM_EStop())
                {
                    mRM_EStop.ReleaseEStop();

                    foreach (var rackmaster in m_RackMasters)
                    {
                        if (rackmaster.Value.IsConnected())
                        {
                            rackmaster.Value.CMD_EmergencyStop();
                        }
                    }
                }

                if(!Check_Port_EStop())
                    mPort_EStop.ReleaseEStop();

                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Master Alarm 발생 시 RM E-Stop 전송 Case
        /// </summary>
        /// <returns></returns>
        static private bool Check_RM_EStop()
        {
            if (AlarmContains(MasterAlarm.HP_Door_E_Stop) ||
                AlarmContains(MasterAlarm.OP_Door_E_Stop) ||
                AlarmContains(MasterAlarm.HP_STK_Key_Stop) ||
                AlarmContains(MasterAlarm.OP_STK_Key_Stop) ||
                AlarmContains(MasterAlarm.Maint_Door_Open) ||
                AlarmContains(MasterAlarm.HP_Door_Open) ||
                AlarmContains(MasterAlarm.OP_Door_Open) ||
                AlarmContains(MasterAlarm.CIM_E_Stop) ||
                AlarmContains(MasterAlarm.Port_Handy_Touch_E_Stop) ||
                AlarmContains(MasterAlarm.STK_HP_Handy_Touch_E_Stop) ||
                AlarmContains(MasterAlarm.STK_OP_Handy_Touch_E_Stop) ||
                AlarmContains(MasterAlarm.HP_Handy_Touch_E_Stop) ||
                AlarmContains(MasterAlarm.OP_Handy_Touch_E_Stop) ||
                AlarmContains(MasterAlarm.HP_Escape_E_Stop) ||
                AlarmContains(MasterAlarm.OP_Escape_E_Stop) ||
                AlarmContains(MasterAlarm.CPS_Run_Error))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Master Alarm 발생 시 Port E-Stop 전송 Case
        /// </summary>
        /// <returns></returns>
        static private bool Check_Port_EStop()
        {
            if (AlarmContains(MasterAlarm.HP_Door_E_Stop) ||
                AlarmContains(MasterAlarm.OP_Door_E_Stop) ||
                AlarmContains(MasterAlarm.Maint_Door_Open) ||
                AlarmContains(MasterAlarm.HP_Door_Open) ||
                AlarmContains(MasterAlarm.OP_Door_Open) ||
                AlarmContains(MasterAlarm.CIM_E_Stop) ||
                AlarmContains(MasterAlarm.Port_Handy_Touch_E_Stop) ||
                AlarmContains(MasterAlarm.STK_HP_Handy_Touch_E_Stop) ||
                AlarmContains(MasterAlarm.STK_OP_Handy_Touch_E_Stop) ||
                AlarmContains(MasterAlarm.HP_Handy_Touch_E_Stop) ||
                AlarmContains(MasterAlarm.OP_Handy_Touch_E_Stop) ||
                AlarmContains(MasterAlarm.HP_Escape_E_Stop) ||
                AlarmContains(MasterAlarm.OP_Escape_E_Stop) ||
                AlarmContains(MasterAlarm.Slave_Not_Op_State_Error) ||
                AlarmContains(MasterAlarm.WMX_Engine_Stop_Error))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Master 상황에 따른 알람 생성
        /// </summary>
        /// <param name="eMasterAlarm"></param>
        static public void AlarmInsert(MasterAlarm eMasterAlarm)
        {
            int WordIndex = (int)eMasterAlarm / 16;
            int BitIndex = (int)eMasterAlarm % 16;

            short AlarmWord = m_MasterWordAlarm[WordIndex].AlarmWord;

            if (!BitOperation.GetBit(AlarmWord, BitIndex))
            {
                BitOperation.SetBit(ref AlarmWord, BitIndex, true);
                m_MasterWordAlarm[WordIndex].AlarmWord = AlarmWord;
                m_MasterWordAlarm[WordIndex].GenerateTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");

                if (m_MasterWordAlarm[WordIndex].ClearTime != string.Empty)
                    m_MasterWordAlarm[WordIndex].ClearTime = string.Empty;

                //if (Result == WordAlarm.UpdateResult.Create)
                LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmCreate, $"Alarm Word[{WordIndex}]: 0x{m_MasterWordAlarm[WordIndex].AlarmWord.ToString("x4")} / Alarm Name: {eMasterAlarm} Create.");

                if (Check_RM_EStop() && (mRM_EStop.GetEStopState() != Interface.Safty.EStopState.EStop))
                { 
                    mRM_EStop.PushEStop();
                    foreach (var rackmaster in m_RackMasters)
                    {
                        if (rackmaster.Value.IsConnected())
                        {
                            rackmaster.Value.CMD_EmergencyStop();
                        }
                    }
                    LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"ReackMaster Emergency Stop On.");
                }

                if (Check_Port_EStop() && (mPort_EStop.GetEStopState() != Interface.Safty.EStopState.EStop))
                {
                    mPort_EStop.PushEStop();
                    LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Port Emergency Stop On.");
                }
            }
        }

        /// <summary>
        /// Master I/O Mapping 상황 체크
        /// </summary>
        /// <param name="eMasterInputItem"></param>
        /// <returns></returns>
        public static bool IsValidInputItemMapping(MasterInputItem eMasterInputItem)
        {
            var InputMap = GetMotionParam().Ctrl_IO.InputMap;

            for (int nCount = 0; nCount < InputMap.Length; nCount++)
            {
                var IOMap = InputMap[nCount];

                if ($"{eMasterInputItem}" != IOMap.Name)
                    continue;

                if (!string.IsNullOrEmpty(IOMap.Name) &&
                    !string.IsNullOrEmpty(eMasterInputItem.ToString()) &&
                    Enum.IsDefined(typeof(MasterInputItem), eMasterInputItem.ToString()) &&
                    Enum.IsDefined(typeof(MasterInputItem), IOMap.Name))
                {
                    return GetMotionParam().IsValidIO(IOMap);
                }
            }
            return false;
        }
        public static bool IsValidOutputItemMapping(MasterOutputItem eMasterOutputItem)
        {
            var OutputMap = GetMotionParam().Ctrl_IO.OutputMap;

            for (int nCount = 0; nCount < OutputMap.Length; nCount++)
            {
                var IOMap = OutputMap[nCount];

                if ($"{eMasterOutputItem}" != IOMap.Name)
                    continue;

                if (!string.IsNullOrEmpty(IOMap.Name) &&
                    !string.IsNullOrEmpty(eMasterOutputItem.ToString()) &&
                    Enum.IsDefined(typeof(MasterOutputItem), eMasterOutputItem.ToString()) &&
                    Enum.IsDefined(typeof(MasterOutputItem), IOMap.Name))
                {
                    return GetMotionParam().IsValidIO(IOMap);
                }
            }
            return false;
        }
        
        /// <summary>
        /// Master Alarm Case 별 체크 조건
        /// </summary>
        static private void AlarmCheck()
        {
            try
            {
                foreach (MasterAlarm eAlarm in Enum.GetValues(typeof(MasterAlarm)))
                {
                    switch (eAlarm)
                    {
                        case MasterAlarm.None:
                            break;
                        case MasterAlarm.HP_Door_E_Stop:
                            if (mHPOutSide_EStop.IsEStop())
                                AlarmInsert(eAlarm);
                            break;
                        case MasterAlarm.OP_Door_E_Stop:
                            if (mOPOutSide_EStop.IsEStop())
                                AlarmInsert(eAlarm);
                            break;
                        case MasterAlarm.HP_STK_Key_Stop:
                            foreach (var rackmaster in m_RackMasters)
                            {
                                if (rackmaster.Value.Status_AutoMode &&
                                    !Sensor_HPAutoKey)
                                {
                                    AlarmInsert(eAlarm);
                                    break;
                                }
                            }
                            break;
                        case MasterAlarm.OP_STK_Key_Stop:
                            break;
                        case MasterAlarm.HP_Door_Open:
                            {
                                bool bStop = false;
                                foreach (var rackmaster in m_RackMasters)
                                {
                                    if (rackmaster.Value.Status_AutoMode &&
                                        Sensor_HPDoorOpen)
                                    {
                                        bStop = true;
                                    }
                                }

                                foreach (var port in m_Ports)
                                {
                                    if (port.Value.IsAutoControlRun() &&
                                        Sensor_HPDoorOpen)
                                    {
                                        bStop = true;
                                    }
                                }

                                if (bStop && IsValidInputItemMapping(MasterInputItem.HP_Door_Open))
                                {
                                    AlarmInsert(eAlarm);
                                }
                            }
                            break;
                        case MasterAlarm.OP_Door_Open:
                            {
                                bool bStop = false;
                                foreach (var rackmaster in m_RackMasters)
                                {
                                    if (rackmaster.Value.Status_AutoMode &&
                                        Sensor_OPDoorOpen)
                                    {
                                        bStop = true;
                                    }
                                }

                                foreach (var port in m_Ports)
                                {
                                    if (port.Value.IsAutoControlRun() &&
                                        Sensor_OPDoorOpen)
                                    {
                                        bStop = true;
                                    }
                                }

                                if (bStop && IsValidInputItemMapping(MasterInputItem.OP_Door_Open))
                                {
                                    AlarmInsert(eAlarm);
                                }
                            }
                            break;
                        case MasterAlarm.Master_Mode_Change_Error:
                            //Insert Case Alarm
                            break;
                        case MasterAlarm.Maint_Door_Open:
                            {
                                if (Sensor_MaintDoorOpen && IsValidInputItemMapping(MasterInputItem.Maint_Door_Open))
                                    AlarmInsert(eAlarm);

                                if (Sensor_MaintDoorOpen2 && IsValidInputItemMapping(MasterInputItem.Maint_Door_Open2))
                                    AlarmInsert(eAlarm);
                            }
                            break;
                        case MasterAlarm.CIM_E_Stop:
                            //Insert Case Alarm
                            break;
                        case MasterAlarm.Port_Handy_Touch_E_Stop:
                            {
                                //DTP Mode 2를 커넥션 신호로 보기로 합의
                                if (Sensor_PortHandyTouchEMO && IsPortHandyTouchConnection)
                                {
                                    m_PortDTPDelay.StartWatchdog();
                                    if (m_PortDTPDelay.IsDetecting())
                                    {
                                        mPortHandyTouch_EStop.PushEStop();
                                        AlarmInsert(eAlarm);
                                    }
                                }
                                else
                                {
                                    m_PortDTPDelay.StopWatchdog(true);
                                    mPortHandyTouch_EStop.ReleaseEStop();
                                }
                            }
                            break;
                        case MasterAlarm.HP_Handy_Touch_E_Stop:
                            {
                                if (Sensor_HPHandyTouchEMO && IsHPHandyTouchConnection)
                                {
                                    m_HPDTPDelay.StartWatchdog();
                                    if (m_HPDTPDelay.IsDetecting())
                                    {
                                        mHPHandyTouch_EStop.PushEStop();
                                        AlarmInsert(eAlarm);
                                    }
                                }
                                else
                                {
                                    m_HPDTPDelay.StopWatchdog(true);
                                    mHPHandyTouch_EStop.ReleaseEStop();
                                }
                            }
                            break;
                        case MasterAlarm.OP_Handy_Touch_E_Stop:
                            {
                                if (Sensor_OPHandyTouchEMO && IsOPHandyTouchConnection)
                                {
                                    m_OPDTPDelay.StartWatchdog();
                                    if (m_OPDTPDelay.IsDetecting())
                                    {
                                        mOPHandyTouch_EStop.PushEStop();
                                        AlarmInsert(eAlarm);
                                    }
                                }
                                else
                                {
                                    m_OPDTPDelay.StopWatchdog(true);
                                    mOPHandyTouch_EStop.ReleaseEStop();
                                }
                            }
                            break;
                        case MasterAlarm.STK_HP_Handy_Touch_E_Stop:
                        case MasterAlarm.STK_OP_Handy_Touch_E_Stop:
                            {
                                if (m_RackMasters == null)
                                    break;

                                foreach (var rackMaster in m_RackMasters)
                                {
                                    bool bEStop = eAlarm == MasterAlarm.STK_HP_Handy_Touch_E_Stop ? rackMaster.Value.Status_HPEStopSwitch : rackMaster.Value.Status_OPEStopSwitch;
                                    
                                    if (bEStop)
                                    {
                                        AlarmInsert(eAlarm);
                                        break;
                                    }
                                }
                            }
                            break;
                        case MasterAlarm.RM_Watch_Dog_Time_Out:
                            {
                                if(RMWatchDog.IsDetecting())
                                    AlarmInsert(eAlarm);
                            }
                            break;
                        case MasterAlarm.CIM_Watch_Dog_Time_Over:
                            {
                                //Not Start
                                if (CIMWatchDog.IsDetecting())
                                    AlarmInsert(eAlarm);
                            }
                            break;
                        case MasterAlarm.RM_TCP_Disconnection:
                            {
                                bool bConnect = true;
                                
                                if (m_RackMasters == null)
                                    break;

                                foreach(var rackMaster in m_RackMasters)
                                {
                                    if (!rackMaster.Value.IsConnected())
                                        bConnect = false;
                                }

                                if(AlarmContains(eAlarm) && bConnect)
                                    AlarmClear(eAlarm);
                            }
                            //Insert Case Alarm
                            break;
                        case MasterAlarm.EtherCAT_Communication_Error:
                            {
                                if (!WMX3.IsEngineCommunicating() && WMX3.IsEngineRunning())
                                {
                                    AlarmInsert(eAlarm);
                                }
                                else
                                    AlarmClear(eAlarm);
                            }
                            break;
                        case MasterAlarm.Slave_Not_Op_State_Error:
                            {
                                if (WMX3.SlaveNotOpState())
                                {
                                    AlarmInsert(eAlarm);
                                }
                                else
                                    AlarmClear(eAlarm);
                            }
                            break;
                        case MasterAlarm.WMX_Engine_Stop_Error:
                            {
                                if(!WMX3.IsEngineRunning())
                                {
                                    AlarmInsert(eAlarm);
                                }
                                else
                                    AlarmClear(eAlarm);
                            }
                            break;
                        case MasterAlarm.CPS_Run_Error:
                            {
                                if ((m_CPS?.m_CPSEnable ?? false) && !Sensor_CPSRun)
                                    AlarmInsert(eAlarm);
                                else if((m_CPS?.m_CPSEnable ?? false) && Sensor_CPSError)
                                    AlarmInsert(eAlarm);
                                else
                                    AlarmClear(eAlarm);
                            }
                            break;
                        case MasterAlarm.RM_Regulator_Fault:
                            {
                                foreach(var rackMaster in m_RackMasters)
                                {
                                    if(rackMaster.Value.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_CPS_2ND_Fault))
                                    {
                                        AlarmInsert(eAlarm);
                                    }
                                }
                            }
                            break;
                        case MasterAlarm.HP_Escape_E_Stop:
                            {
                                if (mHPInnerEscape_EStop.IsEStop())
                                    AlarmInsert(eAlarm);
                            }
                            break;
                        case MasterAlarm.OP_Escape_E_Stop:
                            {
                                if (mOPInnerEscape_EStop.IsEStop())
                                    AlarmInsert(eAlarm);
                            }
                            break;
                        case MasterAlarm.Inner_EMO_1:
                            {
                                if (mDieBankInnerEMO_EStop.IsEStop() &&
                                    IsValidInputItemMapping(MasterInputItem.Inner_EMO_1))
                                    AlarmInsert(eAlarm);
                            }
                            break;
                        case MasterAlarm.Inner_EMO_2:
                            {
                                if (mDieBankInnerEMO_EStop2.IsEStop() &&
                                    IsValidInputItemMapping(MasterInputItem.Inner_EMO_2))
                                    AlarmInsert(eAlarm);
                            }
                            break;
                        case MasterAlarm.Inner_EMO_3:
                            {
                                if (mDieBankInnerEMO_EStop3.IsEStop() &&
                                    IsValidInputItemMapping(MasterInputItem.Inner_EMO_3))
                                    AlarmInsert(eAlarm);
                            }
                            break;
                        case MasterAlarm.Inner_EMO_4:
                            {
                                if (mDieBankInnerEMO_EStop4.IsEStop() &&
                                    IsValidInputItemMapping(MasterInputItem.Inner_EMO_4))
                                    AlarmInsert(eAlarm);
                            }
                            break;
                    }
                }
            }
            catch
            {

            }
        }
        
        /// <summary>
        /// 현재 Alarm Word Map에 따라 Master -> CIM Word 메모리 맵의 알람 영역에 값 적용
        /// </summary>
        static private void AlarmListWordMapUpdate()
        {
            if (Master.m_CIM_SendWordMap.Length <= 0)
                return;

            int StartMapIndex = (int)CIM.SendWordMapIndex.Master_ErrorWord0;
            for (int nCount = 0; nCount < m_MasterWordAlarm.Length; nCount++)
            {
                m_CIM.Set_Master_2_CIM_Word_Data((CIM.SendWordMapIndex)StartMapIndex, m_MasterWordAlarm[nCount].AlarmWord);
                StartMapIndex++;
            }
        }
        
        /// <summary>
        /// 현재 마스터 알람 상태에 따라 Alarm Level 리턴
        /// </summary>
        /// <returns></returns>
        static public AlarmLevel GetAlarmLevel()
        {
            foreach(var WordAlarm in m_MasterWordAlarm)
            {
                if (WordAlarm.AlarmWord != 0)
                    return AlarmLevel.Error;
            }

            return AlarmLevel.None;
        }

        /// <summary>
        /// 현재 마스터에 특정 알람이 발생되어 있는지 확인
        /// </summary>
        /// <param name="eMasterAlarm"></param>
        /// <returns></returns>
        static public bool AlarmContains(MasterAlarm eMasterAlarm)
        {
            int WordIndex = (int)eMasterAlarm / 16;
            int BitIndex = (int)eMasterAlarm % 16;

            short AlarmWord = m_MasterWordAlarm[WordIndex].AlarmWord;

            return BitOperation.GetBit(AlarmWord, BitIndex);
        }

        /// <summary>
        /// 가장 최근 발생한 Error 정보를 리턴
        /// </summary>
        /// <param name="WordIndex"></param>
        /// <param name="AlarmWord"></param>
        /// <param name="AlarmStr"></param>
        static public void GetRecentErrorInfo(out int WordIndex, out short AlarmWord, out string AlarmStr)
        {
            WordIndex = 0;
            AlarmWord = 0;
            AlarmStr = string.Empty;

            for (int nWordIndex = 0; nWordIndex < m_MasterWordAlarm.Length; nWordIndex++)
            {
                var WordAlarm = m_MasterWordAlarm[nWordIndex];
                if (WordAlarm.AlarmWord != 0)
                {
                    WordIndex = nWordIndex;
                    AlarmWord = WordAlarm.AlarmWord;

                    for (int nBitCount = 0; nBitCount < 16; nBitCount++)
                    {
                        MasterAlarm eMasterAlarm = (MasterAlarm)(nWordIndex * 16) + nBitCount;
                        if (AlarmContains(eMasterAlarm))
                        {
                            if (AlarmStr != string.Empty)
                                AlarmStr += "/ ";
                            AlarmStr += GetMasterAlarmComment(eMasterAlarm);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Alarm Word Map의 특정 영역의 값을 가져옴
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        static public WordAlarm GetAlarmAt(int index)
        {
            if (index < m_MasterWordAlarm.Length)
                return m_MasterWordAlarm[index];
            else
                return null;
        }

        /// <summary>
        /// Alarm에 따른 솔루션 텍스트를 리턴
        /// </summary>
        /// <param name="eMasterAlarm"></param>
        /// <returns></returns>
        static public string GetAlarmSolutionText(MasterAlarm eMasterAlarm)
        {
            return SynusLangPack.GetLanguage($"MasterAlarm_{eMasterAlarm}_Solution");
        }

        /// <summary>
        /// Alarm Code에 따른 Alarm Enum을 리턴 (Index 형태의 반복 루프에 활용)
        /// </summary>
        /// <param name="AlarmCode"></param>
        /// <returns></returns>
        static public MasterAlarm IndexToAlarmEnum(short AlarmCode)
        {
            try
            {
                if (Enum.IsDefined(typeof(MasterAlarm), (int)AlarmCode))
                    return (MasterAlarm)AlarmCode;
                else
                    return (MasterAlarm)(-1);
            }
            catch
            {
                return (MasterAlarm)(-1);
            }
        }

        /// <summary>
        /// Master Enum에 따른 텍스트를 출력
        /// </summary>
        /// <param name="eMasterAlarm"></param>
        /// <returns></returns>
        static public string GetMasterAlarmComment(MasterAlarm eMasterAlarm)
        {
            switch (eMasterAlarm)
            {
                case MasterAlarm.None:
                    return "None";
                case MasterAlarm.HP_Door_E_Stop:
                    return "HP Door E-Stop Error";
                case MasterAlarm.OP_Door_E_Stop:
                    return "OP Door E-Stop Error";
                case MasterAlarm.HP_STK_Key_Stop:
                    return "HP STK Key Stop Error";
                case MasterAlarm.OP_STK_Key_Stop:
                    return "OP STK Key Stop Error";
                case MasterAlarm.HP_Door_Open:
                    return "HP Door Open Error";
                case MasterAlarm.OP_Door_Open:
                    return "OP Door Open Error";
                case MasterAlarm.MC_On_Time_Out:
                    return "MC On Time Out";
                case MasterAlarm.Master_Mode_Change_Error:
                    return "Master Mode Change Error";

                case MasterAlarm.RM_Watch_Dog_Time_Out:
                    return "RM Watch Dog Time Out Error";
                case MasterAlarm.CIM_Watch_Dog_Time_Over:
                    return "CIM Watch Dog Time Out Error";
                case MasterAlarm.RM_TCP_Disconnection:
                    return "RM TCP Disconnection Error";
                case MasterAlarm.EtherCAT_Communication_Error:
                    return "EtherCAT Communication Error";
                case MasterAlarm.Slave_Not_Op_State_Error:
                    return "EtherCAT Slave Not Op State Error";
                case MasterAlarm.WMX_Engine_Stop_Error:
                    return "WMX Engine Stop Error";

                case MasterAlarm.Maint_Door_Open:
                    return "Maint Door Open Error";
                case MasterAlarm.CIM_E_Stop:
                    return "CIM E Stop Error";
                case MasterAlarm.Port_Handy_Touch_E_Stop:
                    return "Port Handy Touch E-Stop Error";
                case MasterAlarm.STK_HP_Handy_Touch_E_Stop:
                    return "STK HP Handy Touch E-Stop Error";
                case MasterAlarm.STK_OP_Handy_Touch_E_Stop:
                    return "STK OP Handy Touch E-Stop Error";
                case MasterAlarm.HP_Handy_Touch_E_Stop:
                    return "HP Handy Touch E-Stop Error";
                case MasterAlarm.OP_Handy_Touch_E_Stop:
                    return "OP Handy Touch E-Stop Error";
                

                case MasterAlarm.CPS_Run_Error:
                    return "CPS Run Error";
                case MasterAlarm.RM_Regulator_Fault:
                    return "RM Regulator Fault Error";

                case MasterAlarm.HP_Escape_E_Stop:
                    return "HP Escape E-Stop Error";
                case MasterAlarm.OP_Escape_E_Stop:
                    return "OP Escape E-Stop Error";
                case MasterAlarm.Inner_EMO_1:
                    return "Inner E-Stop 1 Error";
                case MasterAlarm.Inner_EMO_2:
                    return "Inner E-Stop 2 Error";
                case MasterAlarm.Inner_EMO_3:
                    return "Inner E-Stop 3 Error";
                case MasterAlarm.Inner_EMO_4:
                    return "Inner E-Stop 4 Error";
                default:
                    return "Reserve";
            }
        }

    }
}
