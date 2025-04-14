using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Interface.Alarm
{
    /// <summary>
    /// Word 형태 알람 제어시 사용되는 클래스
    /// 사용되는 Word 수 만큼 할당하여 사용
    /// CPS, Master, RackMaster 워드 알람 사용
    /// </summary>
    public class WordAlarm
    {
        public enum UpdateResult
        {
            None,
            Clear,
            Create
        }
        public string GenerateTime = string.Empty;
        public short AlarmWord = 0;
        public short ClearAlarmWord = 0;
        public string ClearTime = string.Empty;

        public WordAlarm()
        {
            GenerateTime = string.Empty;
            AlarmWord = 0;
            ClearAlarmWord = 0;
            ClearTime = string.Empty;
        }

        public UpdateResult Update(short _AlarmCode)
        {
            UpdateResult eUpdateResult = UpdateResult.None;

            if (_AlarmCode == 0 && AlarmWord != 0)
            {
                ClearTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss") + $" [0x{AlarmWord.ToString("x4")}]";
                ClearAlarmWord = AlarmWord;
                AlarmWord = _AlarmCode;
                eUpdateResult = UpdateResult.Clear;
            }
            else if (AlarmWord == 0 && _AlarmCode != 0)
            {
                AlarmWord = _AlarmCode;
                GenerateTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
                ClearTime = string.Empty;
                eUpdateResult = UpdateResult.Create;
            }
            else if (AlarmWord != 0 && _AlarmCode != 0 && AlarmWord != _AlarmCode)
            {
                AlarmWord = _AlarmCode;
                GenerateTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
                ClearTime = string.Empty;
                eUpdateResult = UpdateResult.Create;
            }

            return eUpdateResult;
        }
    }
}
