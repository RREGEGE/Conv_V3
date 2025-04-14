using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Equipment.Port.TagReader.ReaderEquip;
using Master.ManagedFile;

namespace Master.Equipment.Port.TagReader
{
    /// <summary>
    /// CST Tag를 Read하는 Hardware Type
    /// 향후 Model Name으로 가져갈 필요 있어보임
    /// </summary>
    public enum TagReaderType
    {
        None,
        RFID,
        BCR,
        CanTops_LM21
    }

    /// <summary>
    /// CST ID Reader Class는 Port에 할당 되며 Port에서 Tag Read 명령을 호출하면 할당 된 Tag Reader Type에 따라 명령 분기
    /// </summary>
    public class CSTIDReader
    {
        TagReaderType m_eReaderType = TagReaderType.None;
        RFIDReader m_RFIDReader = null;
        BCRReader m_BCRReader = null;
        CanTops_LM21 m_CantTops_LM21 = null;

        public CSTIDReader(EquipNetworkParam.PortNetworkParam _NetParam, EquipPortMotionParam.PortMotionParameter _MotionParam)
        {
            m_eReaderType = _NetParam.eTagReaderType; //_NetParam.eTagReaderType
            if (m_eReaderType == TagReaderType.RFID) //_NetParam.eTagReaderType
            {
                m_RFIDReader = new RFIDReader(_NetParam.ID, _NetParam.TagEquipServerIP, _NetParam.TagEquipServerPort);
                m_RFIDReader.SetModel(_MotionParam.eRFIDModel);
                m_RFIDReader.SetTagReadSize((byte)_MotionParam.TagReadSize);
            }
            else if (m_eReaderType == TagReaderType.BCR) //_NetParam.eTagReaderType
            {
                m_BCRReader = new BCRReader(_NetParam.ID, _NetParam.TagEquipServerIP, _NetParam.TagEquipServerPort);
            }
            else if (m_eReaderType == TagReaderType.CanTops_LM21) //_NetParam.eTagReaderType
            {
                m_CantTops_LM21 = new CanTops_LM21(_NetParam.ID, _NetParam.TagEquipServerIP, _NetParam.TagEquipServerPort);
            }
        }

        public TagReaderType GetReaderEquipType()
        {
            return m_eReaderType;
        }

        public void Close()
        {
            if (m_RFIDReader != null)
                m_RFIDReader.Close();
            if (m_BCRReader != null)
                m_BCRReader.Close();
            if (m_CantTops_LM21 != null)
                m_CantTops_LM21.Close();
        }

        public bool IsConnected()
        {
            if (m_RFIDReader != null)
                return m_RFIDReader.IsConnected();
            else if (m_BCRReader != null)
                return m_BCRReader.IsConnected();
            else if (m_CantTops_LM21 != null)
                return m_CantTops_LM21.IsConnected();
            else
                return false;
        }

        public void TagRead()
        {
            if (m_RFIDReader != null)
                m_RFIDReader.TagRead();
            else if (m_BCRReader != null)
                m_BCRReader.TagRead();
            else if (m_CantTops_LM21 != null)
                m_CantTops_LM21.TagRead();
        }

        public string GetTag()
        {
            if (m_RFIDReader != null)
                return m_RFIDReader.GetTag();
            else if (m_BCRReader != null)
                return m_BCRReader.GetCodeInfo();
            else if (m_CantTops_LM21 != null)
                return m_CantTops_LM21.GetTag();
            else
                return string.Empty;
        }

        public void TagReadCountUp()
        {
            if (m_RFIDReader != null)
                m_RFIDReader.n_RFIDReadCount++;
            else if (m_BCRReader != null)
                m_BCRReader.n_BCRReadCount++;
            else if (m_CantTops_LM21 != null)
                m_CantTops_LM21.n_RFIDReadCount++;
        }

        public int GetTagReadCount()
        {
            if (m_RFIDReader != null)
                return m_RFIDReader.n_RFIDReadCount;
            else if (m_BCRReader != null)
                return m_BCRReader.n_BCRReadCount;
            else if (m_CantTops_LM21 != null)
                return m_CantTops_LM21.n_RFIDReadCount;
            else
                return -1;
        }

        public string GetCurrentTagReader()
        {
            if (m_RFIDReader != null)
                return "RFID";
            else if (m_BCRReader != null)
                return "BCR";
            else if (m_CantTops_LM21 != null)
                return "CanTops LM21";
            else
                return "None";
        }

        public bool IsTagReadSuccess()
        {
            if (m_RFIDReader != null)
                return m_RFIDReader.IsTagReadSuccess();
            else if (m_BCRReader != null)
                return m_BCRReader.IsTagReadSuccess();
            else if (m_CantTops_LM21 != null)
                return m_CantTops_LM21.IsTagReadSuccess();
            else
                return false;
        }

        public RFIDReader GetRFIDReader()
        {
            return m_RFIDReader;
        }

        public BCRReader GetBCRReader()
        {
            return m_BCRReader;
        }
        public CanTops_LM21 GetCanTopsLM21Reader()
        {
            return m_CantTops_LM21;
        }
    }
}
