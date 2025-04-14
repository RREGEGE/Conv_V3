using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Interface.Alarm
{
    public enum AlarmLevel
    {
        None,
        Warning,
        Error
    }

    /// <summary>
    /// Code 형태 알람 제어시 사용되는 클래스
    /// Port에서 사용
    /// </summary>
    public class CodeAlarm
    {
        public class CodeAlarmParam
        {
            public string GenerateTime;
            public short AlarmCode = 0;
            public string AlarmComment;
            public AlarmLevel eAlarmLevel;

            public bool bClear;
            public string ClearTime;

            public CodeAlarmParam(short _AlarmCode, string _AlarmComment, AlarmLevel _eAlarmLevel)
            {
                GenerateTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
                AlarmCode = _AlarmCode;
                AlarmComment = _AlarmComment;
                eAlarmLevel = _eAlarmLevel;
            }
        }

        List<CodeAlarmParam> m_AlarmLists = new List<CodeAlarmParam>();
        private object AlarmLock = new object();

        public CodeAlarm()
        {
            m_AlarmLists = new List<CodeAlarmParam>();
        }

        public AlarmLevel GetRecentAlarmLevel()
        {
            lock (AlarmLock)
            {
                AlarmLevel eAlarmLevel = AlarmLevel.None;
                foreach (var Alarm in m_AlarmLists)
                {
                    if ((Alarm.eAlarmLevel == AlarmLevel.Error) && !Alarm.bClear)
                        eAlarmLevel = eAlarmLevel <= AlarmLevel.Error ? AlarmLevel.Error : eAlarmLevel;
                    else if ((Alarm.eAlarmLevel == AlarmLevel.Warning) && !Alarm.bClear)
                        eAlarmLevel = eAlarmLevel <= AlarmLevel.Warning ? AlarmLevel.Warning : eAlarmLevel;
                }

                return eAlarmLevel;
            }
        }
        public List<CodeAlarmParam> GetAlarmList()
        {
            lock (AlarmLock)
            {
                return m_AlarmLists;
            }
        }

        public short GetRecentAlarmCode()
        {
            lock (AlarmLock)
            {
                if (m_AlarmLists.Count > 0)
                {
                    foreach (var Alarm in m_AlarmLists)
                    {
                        if (!Alarm.bClear)
                        {
                            return Alarm.AlarmCode;
                        }
                    }
                }
                else
                    return 0;

                return 0;
            }
        }
        public List<CodeAlarmParam> Clear()
        {
            List<CodeAlarmParam> ClearList = new List<CodeAlarmParam>();

            lock (AlarmLock)
            {
                if (m_AlarmLists.Count > 0)
                {
                    foreach (var Alarm in m_AlarmLists)
                    {
                        if (!Alarm.bClear)
                        {
                            Alarm.bClear = true;
                            Alarm.ClearTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");

                            ClearList.Add(Alarm);
                        }
                    }
                }
            }

            return ClearList;
        }
        public bool Insert(short AlarmCode, string AlarmComment, AlarmLevel eAlarmLevel)
        {
            bool bInsertEnable = true;

            lock (AlarmLock)
            {
                foreach (var Alarm in m_AlarmLists)
                {
                    //이미 존재하는지 체크
                    if (!Alarm.bClear && (Alarm.AlarmCode == AlarmCode))
                    {
                        bInsertEnable = false;
                        break;
                    }
                }
            }

            if (bInsertEnable)
                m_AlarmLists.Insert(0, new CodeAlarmParam(AlarmCode, AlarmComment, eAlarmLevel));

            return bInsertEnable;
        }

        public void Remove(short AlarmCode)
        {
            lock (AlarmLock)
            {
                foreach (var Alarm in m_AlarmLists)
                {
                    if (!Alarm.bClear && (Alarm.AlarmCode == AlarmCode))
                    {
                        m_AlarmLists.Remove(Alarm);
                        break;
                    }
                }
            }
        }

        public bool Contains(short AlarmCode)
        {
            lock (AlarmLock)
            {
                foreach (var Alarm in m_AlarmLists)
                {
                    if (!Alarm.bClear && (Alarm.AlarmCode == AlarmCode))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
