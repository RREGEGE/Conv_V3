using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Equipment.RackMaster
{
    /// <summary>
    /// STK -> Master Word Memory Map에 사용되는 변수 기재
    /// </summary>
    public partial class RackMaster
    {
        string m_STKToCIM_CarrierID     = string.Empty;
        int m_STKToCIM_AccessID         = 0;
        int m_STKToCIM_FromID           = 0;
        int m_STKToCIM_ToID             = 0;
        int m_STKToCIM_TeachingID       = 0;
        short m_STKToCIM_TeachingXPos   = 0;
        short m_STKToCIM_TeachingZPos   = 0;

        /// <summary>
        /// STK -> Master로 전송하는 CST ID
        /// STK에서 Word Packet 수신 시 업데이트 진행
        /// </summary>
        public string Status_STK_To_CIM_CarrierID
        {
            get
            {
                return m_STKToCIM_CarrierID;
            }
            set
            {
                if (m_STKToCIM_CarrierID != value)
                {
                    //현재 저장된 CST ID와 다른 경우 로그 작성
                    if(value == string.Empty)
                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterCSTInfo, $"RM -> Master(CIM) CST ID : {m_STKToCIM_CarrierID} -> Clear");
                    else
                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterCSTInfo, $"RM -> Master(CIM) CST ID : {m_STKToCIM_CarrierID} -> {value}");

                    m_STKToCIM_CarrierID = value;
                }
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Access Shelf ID
        /// STK에서 Word Packet 수신 시 업데이트 진행
        /// </summary>
        public int Status_STK_To_CIM_AccessID
        {
            get
            {
                return m_STKToCIM_AccessID;
            }
            set
            {
                if (m_STKToCIM_AccessID != value)
                {
                    m_STKToCIM_AccessID = value;
                    LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterControlInfo, 
                        $"Access ID : {(m_STKToCIM_AccessID == 0 ? "Clear" : value.ToString())}");
                }
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 From Shelf ID
        /// STK에서 Word Packet 수신 시 업데이트 진행
        /// </summary>
        public int Status_STK_To_CIM_FromID
        {
            get
            {
                return m_STKToCIM_FromID;
            }
            set
            {
                if (m_STKToCIM_FromID != value)
                {
                    m_STKToCIM_FromID = value;
                    LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterControlInfo, 
                        $"From ID : {(m_STKToCIM_FromID == 0 ? "Clear" : value.ToString())}");
                }
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 To Shelf ID
        /// STK에서 Word Packet 수신 시 업데이트 진행
        /// </summary>
        public int Status_STK_To_CIM_ToID
        {
            get
            {
                return m_STKToCIM_ToID;
            }
            set
            {
                if (m_STKToCIM_ToID != value)
                {
                    m_STKToCIM_ToID = value;
                    LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterControlInfo, 
                        $"To ID : {(m_STKToCIM_ToID == 0 ? "Clear" : value.ToString())}");
                }
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Teaching Shelf ID
        /// STK에서 Word Packet 수신 시 업데이트 진행
        /// </summary>
        public int Status_STK_To_CIM_TeachingID
        {
            get
            {
                return m_STKToCIM_TeachingID;
            }
            set
            {
                if (m_STKToCIM_TeachingID != value)
                {
                    m_STKToCIM_TeachingID = value;
                    LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterControlInfo, 
                        $"Auto Teaching ID : {(m_STKToCIM_TeachingID == 0 ? "Clear" : value.ToString())}");
                }
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Teaching Shelf X Pos
        /// STK에서 Word Packet 수신 시 업데이트 진행
        /// </summary>
        public short Status_STK_To_CIM_Teaching_X_Pos
        {
            get
            {
                return m_STKToCIM_TeachingXPos;
            }
            set
            {
                m_STKToCIM_TeachingXPos = value;
            }
        }

        /// <summary>
        /// STK -> Master로 전송하는 Teaching Shelf Z Pos
        /// STK에서 Word Packet 수신 시 업데이트 진행
        /// </summary>
        public short Status_STK_To_CIM_Teaching_Z_Pos
        {
            get
            {
                return m_STKToCIM_TeachingZPos;
            }
            set
            {
                m_STKToCIM_TeachingZPos = value;
            }
        }

        /// <summary>
        /// STK Word Map Packet 수신 시 업데이트
        /// </summary>
        public void RackMasterWordUpdateEvent()
        {
            //Packet 수신 시 업데이트 진행
            Status_STK_To_CIM_CarrierID         = (string)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.CassetteID_PIO_Word_0);
            Status_STK_To_CIM_AccessID          = (int)(Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.Access_Shelf_ID_0));
            Status_STK_To_CIM_FromID            = (int)(Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.From_Shelf_ID_0));
            Status_STK_To_CIM_ToID              = (int)(Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.To_Shelf_ID_0));
            Status_STK_To_CIM_TeachingID        = (int)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.AutoTeaching_ID_0);
            Status_STK_To_CIM_Teaching_X_Pos    = (short)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.AutoTeaching_X_Axis_Data);
            Status_STK_To_CIM_Teaching_Z_Pos    = (short)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.AutoTeaching_Z_Axis_Data);

            //STK Motion 동작 중인 경우 실시간 로그 작성
            if (Status_STK_To_CIM_AccessID != 0 ||
                Status_STK_To_CIM_FromID != 0 ||
                Status_STK_To_CIM_ToID != 0 ||
                Status_STK_To_CIM_TeachingID != 0)
            {
                string TagetPos_X = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.X_Axis_TargetPosition_0)).ToString("0.0")} mm";
                string TagetPos_Z = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_TargetPosition_0)).ToString("0.0")} mm";
                string TagetPos_A = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.A_Axis_TargetPosition_0)).ToString("0.0")} mm";
                string TagetPos_T = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.T_Axis_TargetPosition_0)).ToString("0.0")} °";

                string ActualPos_X = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentPosition_0)).ToString("0.0")} mm";
                string ActualPos_Z = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentPosition_0)).ToString("0.0")} mm";
                string ActualPos_A = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.A_Axis_CurrentPosition_0)).ToString("0.0")} mm";
                string ActualPos_T = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentPosition_0)).ToString("0.0")} °";

                string ActualVel_X = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentSpeed_0)).ToString("0")} m/min";
                string ActualVel_Z = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentSpeed_0)).ToString("0")} m/min";
                string ActualVel_A = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.A_Axis_CurrentSpeed_0)).ToString("0")} m/min";
                string ActualVel_T = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentSpeed_0)).ToString("0")} °/min";

                string ActualTrq_X = $"{Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentTorque)} %";
                string ActualTrq_Z = $"{Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentTorque)} %";
                string ActualTrq_A = $"{Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.A_Axis_CurrentTorque)} %";
                string ActualTrq_T = $"{Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentTorque)} %";

                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterMotionInfo,
                    $"TarPos [{TagetPos_X}, {TagetPos_Z}, {TagetPos_A}, {TagetPos_T}], " +
                    $"ActPos [{ActualPos_X}, {ActualPos_Z}, {ActualPos_A}, {ActualPos_T}], " +
                    $"ActVel [{ActualVel_X}, {ActualVel_Z}, {ActualVel_A}, {ActualVel_T}], " +
                    $"ActTrq [{ActualTrq_X}, {ActualTrq_Z}, {ActualTrq_A}, {ActualTrq_T}]");
            }
        }
    }
}
