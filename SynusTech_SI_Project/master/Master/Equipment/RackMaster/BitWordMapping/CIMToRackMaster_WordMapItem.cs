using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Equipment.RackMaster
{
    /// <summary>
    /// [CIM -> Master] -> STK Word Memory Map에 사용되는 변수 기재
    /// </summary>
    public partial class RackMaster
    {
        string m_CIMToRM_CarrierID      = string.Empty;
        
        int m_CIMToRM_FromShelfID       = 0;
        int m_CIMToRM_ToShelfID         = 0;

        int m_CIMToRM_TeachingRWID      = 0;
        int m_CIMToRM_TeachingID        = 0;
        short m_CIMToRM_TeachingXPos    = 0;
        short m_CIMToRM_TeachingZPos    = 0;

        short m_X_Axis_Speed_Ratio      = 100;
        short m_Z_Axis_Speed_Ratio      = 100;
        short m_A_Axis_Speed_Ratio      = 100;
        short m_T_Axis_Speed_Ratio      = 100;

        short m_X_Axis_TorqueLimit      = 200;
        short m_Z_Axis_TorqueLimit      = 200;
        short m_A_Axis_TorqueLimit      = 200;
        short m_T_Axis_TorqueLimit      = 200;

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 CST ID
        /// 포트 공정중 OP PIO 진행 영역 port -> STK CST 이재 Step에서 값 적용 (Send or ACK 용으로 확인)
        /// </summary>
        public string CMD_CIM_To_STK_CarrierID
        {
            get
            {
                return m_CIMToRM_CarrierID;
            }
            set
            {
                if (m_CIMToRM_CarrierID != value)
                {
                    if (value != string.Empty)
                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterCSTInfo, $"Master(CIM) -> RM CST ID : {m_CIMToRM_CarrierID} => {value}");
                    else
                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterCSTInfo, $"Master(CIM) -> RM CST ID : {m_CIMToRM_CarrierID} => Clear");
                }

                m_CIMToRM_CarrierID = value;
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.CassetteID_PIO_Word_0, value);

                if(m_CIMToRM_CarrierID != string.Empty)
                    Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.CassetteID_PIO_Word_0, 58);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Shelf ID
        /// Master에서는 Teaching Shelf의 정보를 불러오는데 사용
        /// </summary>
        public int CMD_CIM_To_STK_Teaching_RW_ID
        {
            get
            {
                return m_CIMToRM_TeachingRWID;
            }
            set
            {
                m_CIMToRM_TeachingRWID = value;
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterCSTInfo, $"Master(CIM) -> RM Teaching RW ID Send : {m_CIMToRM_TeachingRWID}");
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Teaching_RW_ID_0, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Teaching_RW_ID_0, 2);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 From 동작 Shelf ID
        /// STK Cycle 공정에서 호출
        /// </summary>
        public int CMD_CIM_To_STK_From_Shelf_ID
        {
            get
            {
                return m_CIMToRM_FromShelfID;
            }
            set
            {
                m_CIMToRM_FromShelfID = value;
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterFromInfo, $"Master(CIM) -> RM From ID Send : {m_CIMToRM_FromShelfID}");
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.From_Shelf_ID_0, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.From_Shelf_ID_0, 2);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 To 동작 Shelf ID
        /// STK Cycle 공정에서 호출
        /// </summary>
        public int CMD_CIM_To_STK_To_Shelf_ID
        {
            get
            {
                return m_CIMToRM_ToShelfID;
            }
            set
            {
                m_CIMToRM_ToShelfID = value;
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterToInfo, $"Master(CIM) -> RM To ID Send : {m_CIMToRM_ToShelfID}");
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.To_Shelf_ID_0, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.To_Shelf_ID_0, 2);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Teaching 동작 Shelf ID
        /// STK Auto Teaching 공정에서 호출
        /// </summary>
        public int CMD_CIM_To_STK_Teaching_Shelf_ID
        {
            get
            {
                return m_CIMToRM_TeachingID;
            }
            set
            {
                m_CIMToRM_TeachingID = value;
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterTeachingInfo, $"Master(CIM) -> RM Teaching ID Send : {m_CIMToRM_TeachingID}");
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.AutoTeaching_ID_0, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.AutoTeaching_ID_0, 2);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Teaching Shelf의 X 기준 위치
        /// STK Auto Teaching 공정에서 호출
        /// </summary>
        public short CMD_CIM_To_STK_Teaching_X_Pos
        {
            get
            {
                return m_CIMToRM_TeachingXPos;
            }
            set
            {
                m_CIMToRM_TeachingXPos = value;
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterTeachingInfo, $"Master(CIM) -> RM Teaching X Pos Send : {m_CIMToRM_TeachingXPos} mm");
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.AutoTeaching_X_Axis_Data, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.AutoTeaching_X_Axis_Data);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Teaching Shelf의 Z 기준 위치
        /// STK Auto Teaching 공정에서 호출
        /// </summary>
        public short CMD_CIM_To_STK_Teaching_Z_Pos
        {
            get
            {
                return m_CIMToRM_TeachingZPos;
            }
            set
            {
                m_CIMToRM_TeachingZPos = value;
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterTeachingInfo, $"Master(CIM) -> RM Teaching Z Pos Send : {m_CIMToRM_TeachingZPos} mm");
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.AutoTeaching_Z_Axis_Data, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.AutoTeaching_Z_Axis_Data);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 X Axis 속도 비율
        /// STK Setting 영역에서 호출
        /// </summary>
        public short CMD_CIM_To_STK_X_Axis_SpeedRatio
        {
            get
            {
                return m_X_Axis_Speed_Ratio;
            }
            set
            {
                m_X_Axis_Speed_Ratio = value;
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.X_Axis_Speed, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.X_Axis_Speed);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Z Axis 속도 비율
        /// STK Setting 영역에서 호출
        /// </summary>
        public short CMD_CIM_To_STK_Z_Axis_SpeedRatio
        {
            get
            {
                return m_Z_Axis_Speed_Ratio;
            }
            set
            {
                m_Z_Axis_Speed_Ratio = value;
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Z_Axis_Speed, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Z_Axis_Speed);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 A Axis 속도 비율
        /// STK Setting 영역에서 호출
        /// </summary>
        public short CMD_CIM_To_STK_A_Axis_SpeedRatio
        {
            get
            {
                return m_A_Axis_Speed_Ratio;
            }
            set
            {
                m_A_Axis_Speed_Ratio = value;
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.A_Axis_Speed, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.A_Axis_Speed);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 T Axis 속도 비율
        /// STK Setting 영역에서 호출
        /// </summary>
        public short CMD_CIM_To_STK_T_Axis_SpeedRatio
        {
            get
            {
                return m_T_Axis_Speed_Ratio;
            }
            set
            {
                m_T_Axis_Speed_Ratio = value;
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.T_Axis_Speed, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.T_Axis_Speed);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 X Axis 토크 제한 비율
        /// STK Setting 영역에서 호출
        /// </summary>
        public short CMD_CIM_To_STK_X_Axis_TorqueLimit
        {
            get
            {
                return m_X_Axis_TorqueLimit;
            }
            set
            {
                m_X_Axis_TorqueLimit = value;
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.X_Axis_MaxLoadValue, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.X_Axis_MaxLoadValue);

                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_X_Axis_MaxLoadLimit_Apply, true);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_X_Axis_MaxLoadLimit_Apply);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 Z Axis 토크 제한 비율
        /// STK Setting 영역에서 호출
        /// </summary>
        public short CMD_CIM_To_STK_Z_Axis_TorqueLimit
        {
            get
            {
                return m_Z_Axis_TorqueLimit;
            }
            set
            {
                m_Z_Axis_TorqueLimit = value;
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Z_Axis_MaxLoadValue, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Z_Axis_MaxLoadValue);

                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Z_Axis_MaxLoadLimit_Apply, true);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Z_Axis_MaxLoadLimit_Apply);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 A Axis 토크 제한 비율
        /// STK Setting 영역에서 호출
        /// </summary>
        public short CMD_CIM_To_STK_A_Axis_TorqueLimit
        {
            get
            {
                return m_A_Axis_TorqueLimit;
            }
            set
            {
                m_A_Axis_TorqueLimit = value;
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.A_Axis_MaxLoadValue, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.A_Axis_MaxLoadValue);

                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_A_Axis_MaxLoadLimit_Apply, true);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_A_Axis_MaxLoadLimit_Apply);
            }
        }

        /// <summary>
        /// [CIM -> Master] -> STK로 전송하는 T Axis 토크 제한 비율
        /// STK Setting 영역에서 호출
        /// </summary>
        public short CMD_CIM_To_STK_T_Axis_TorqueLimit
        {
            get
            {
                return m_T_Axis_TorqueLimit;
            }
            set
            {
                m_T_Axis_TorqueLimit = value;
                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.T_Axis_MaxLoadValue, value);
                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.T_Axis_MaxLoadValue);

                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_T_Axis_MaxLoadLimit_Apply, true);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_T_Axis_MaxLoadLimit_Apply);
            }
        }
    }
}
