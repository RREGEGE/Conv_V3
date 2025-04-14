using Master.Interface.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Equipment.Port
{
    /// <summary>
    /// Port_TAG_READ_Step.cs는 Auto 공정에서 Tag Read 관련 스텝에 사용 할 기능을 함수화 하여 작성
    /// </summary>
    partial class Port
    {
        public enum TAG_ID_READ_SET_SECTION
        {
            LP,
            OP,
            BP
        }

        /// <summary>
        /// Tag Read 전 Init (시도횟수 카운트 초기화)
        /// </summary>
        /// <returns></returns>
        private bool TAG_READ_INIT()
        {
            if (m_TagReader_Interface.GetReaderEquipType() == TagReader.TagReaderType.RFID)
                m_TagReader_Interface.GetRFIDReader().n_RFIDReadCount = 0;
            else if (m_TagReader_Interface.GetReaderEquipType() == TagReader.TagReaderType.BCR)
                m_TagReader_Interface.GetBCRReader().n_BCRReadCount = 0;
            else if (m_TagReader_Interface.GetReaderEquipType() == TagReader.TagReaderType.CanTops_LM21)
                m_TagReader_Interface.GetCanTopsLM21Reader().n_RFIDReadCount = 0;

            return true;
        }

        /// <summary>
        /// TAG Read 동작
        /// </summary>
        /// <param name="eTAG_ID_READ_SET_SECTION"></param>
        /// <returns></returns>
        private bool TAG_READ_TRY(TAG_ID_READ_SET_SECTION eTAG_ID_READ_SET_SECTION)
        {
            bool ret = false;
            var TagReader = m_TagReader_Interface;

            if (TagReader.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.None)
            {
                TAG_READER_NOT_SET(eTAG_ID_READ_SET_SECTION);
                ret = true;
            }
            else if(TagReader.IsConnected())
            {
                TagReader.TagReadCountUp();
                TagReader.TagRead();

                int TagReadCount = m_TagReader_Interface.GetTagReadCount();

                LogMsg.AddPortLog(this.GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.TagReaderInfo, $"{TagReader.GetCurrentTagReader()} Read Try[{TagReadCount}] : {TagReader.GetTag()}");

                if (TagReader.IsTagReadSuccess())
                {
                    string TagValue = TagReader.GetTag();
                    TAG_READER_READ_SUCCESS_SET(eTAG_ID_READ_SET_SECTION, TagValue);
                    ret = true;
                }
                else
                {
                    if (TagReadCount >= TAG_GET_READ_TRY_COUNT())
                    {
                        if (GetMotionParam().TagReadFailError)
                        {
                            AlarmInsert((short)PortAlarm.Tag_Read_Fail, AlarmLevel.Error);
                        }
                        else
                        {
                            AlarmInsert((short)PortAlarm.Tag_Read_Fail, AlarmLevel.Warning);
                            TAG_READER_READ_FAIL_SET(eTAG_ID_READ_SET_SECTION);
                            ret = true;
                        }
                    }
                }
            }
            else
            {
                AlarmInsert((short)PortAlarm.Tag_Disconnection, AlarmLevel.Error);
            }

            return ret;
        }
        
        /// <summary>
        /// Tag Reader 사용하지 않는 경우 ID 부여 (Cycle, Output 등)
        /// </summary>
        /// <param name="eTAG_ID_READ_SET_SECTION"></param>
        private void TAG_READER_NOT_SET(TAG_ID_READ_SET_SECTION eTAG_ID_READ_SET_SECTION)
        {
            if(eTAG_ID_READ_SET_SECTION == TAG_ID_READ_SET_SECTION.LP)
                LP_CarrierID = "TAG_READER_NOT_SET";
            else if(eTAG_ID_READ_SET_SECTION == TAG_ID_READ_SET_SECTION.OP)
                OP_CarrierID = "TAG_READER_NOT_SET";
            else if(eTAG_ID_READ_SET_SECTION == TAG_ID_READ_SET_SECTION.BP)
                Carrier_SetBP_CarrierID(0, "TAG_READER_NOT_SET");
        }
        
        /// <summary>
        /// Tag Read 실패하는 경우 ID 부여
        /// CassetteInfo.ini 에서 부여 ID 지정 가능
        /// </summary>
        /// <param name="eTAG_ID_READ_SET_SECTION"></param>
        private void TAG_READER_READ_FAIL_SET(TAG_ID_READ_SET_SECTION eTAG_ID_READ_SET_SECTION)
        {
            if (eTAG_ID_READ_SET_SECTION == TAG_ID_READ_SET_SECTION.LP)
                LP_CarrierID = ManagedFile.CassetteInfo.ReadFailCSTID(); //"CST_ID_READ_FAIL";
            else if (eTAG_ID_READ_SET_SECTION == TAG_ID_READ_SET_SECTION.OP)
                OP_CarrierID = ManagedFile.CassetteInfo.ReadFailCSTID();// "CST_ID_READ_FAIL";
            else if (eTAG_ID_READ_SET_SECTION == TAG_ID_READ_SET_SECTION.BP)
                Carrier_SetBP_CarrierID(0, ManagedFile.CassetteInfo.ReadFailCSTID());
        }

        /// <summary>
        /// Tag Read 성공 시 ID 부여
        /// </summary>
        /// <param name="eTAG_ID_READ_SET_SECTION"></param>
        /// <param name="TagID"></param>
        private void TAG_READER_READ_SUCCESS_SET(TAG_ID_READ_SET_SECTION eTAG_ID_READ_SET_SECTION, string TagID)
        {
            if (eTAG_ID_READ_SET_SECTION == TAG_ID_READ_SET_SECTION.LP)
                LP_CarrierID = TagID;
            else if (eTAG_ID_READ_SET_SECTION == TAG_ID_READ_SET_SECTION.OP)
                OP_CarrierID = TagID;
            else if (eTAG_ID_READ_SET_SECTION == TAG_ID_READ_SET_SECTION.BP)
                Carrier_SetBP_CarrierID(0, TagID);
        }
        
        /// <summary>
        /// Tag Read 시도 횟수 지정
        /// </summary>
        /// <returns></returns>
        private int TAG_GET_READ_TRY_COUNT()
        {
            int ReadTrycount = 5;
            try
            {
                ReadTrycount = Convert.ToInt32(ManagedFile.CassetteInfo.ReadTryCount());

                if (ReadTrycount < 1)
                    ReadTrycount = 1;
            }
            catch
            {
                ReadTrycount = 5;
            }

            return ReadTrycount;
        }
    }
}
