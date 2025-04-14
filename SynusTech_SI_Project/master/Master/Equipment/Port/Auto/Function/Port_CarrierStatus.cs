using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Master.ManagedFile;

namespace Master.Equipment.Port
{
    /// <summary>
    /// Port_CarrierStatus.cs 는 CST ID 이전 및 존재 유무를 체크하는 기능 작성
    /// Carrier 의 네이밍으로 작성
    /// </summary>
    public partial class Port
    {
        /// <summary>
        /// Shuttle, Conveyor 연결 물류에 대한 ID
        /// 메모리 맵등 별도 관리 포인트가 없어 프로그램에서 직접 관리
        /// </summary>
        private string[] BP_CarrierID = Enumerable.Repeat(string.Empty, 4).ToArray();

        /// <summary>
        /// OP -> Shuttle ID 이전 함수
        /// </summary>
        /// <returns></returns>
        public bool Carrier_OP_To_Shuttle_CST_ID_Transfer()
        {
            if (Carrier_CheckOP_ExistID() &&
                Carrier_CheckShuttle_ExistProduct(true))
            {
                //OP에 ID가 있지만 제품은 BP로 와있는 경우
                string OPCarrierID = OP_CarrierID;
                Carrier_SetBP_CarrierID(0, OPCarrierID);

                if (Carrier_CheckBP_ExistID(0) &&
                    Carrier_CheckOP_ExistProduct(false, false))
                {
                    //BP에 ID가 Set 되고 OP에는 제품이 없는게 확인 됐다면 OP ID는 Clear
                    OP_CarrierID = string.Empty;
                }

                return false;
            }
            else if (!Carrier_CheckOP_ExistID() &&
                    Carrier_CheckBP_ExistID(0))
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// LP -> Shuttle ID 이전 함수
        /// </summary>
        /// <returns></returns>
        public bool Carrier_LP_To_Shuttle_CST_ID_Transfer()
        {
            if (Carrier_CheckLP_ExistID() &&
                Carrier_CheckShuttle_ExistProduct(true))
            {
                string LPCarrierID = LP_CarrierID;
                Carrier_SetBP_CarrierID(0, LPCarrierID);

                if (Carrier_CheckBP_ExistID(0) &&
                    Carrier_CheckLP_ExistProduct(false, false))
                {
                    //BP에 ID가 Set 되고 LP에는 제품이 없는게 확인 됐다면 LP ID는 Clear
                    LP_CarrierID = string.Empty;
                }

                return false;
            }
            else if (!Carrier_CheckLP_ExistID() &&
                    Carrier_CheckBP_ExistID(0))
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Shuttle -> LP ID 이전 함수
        /// </summary>
        /// <returns></returns>
        public bool Carrier_Shuttle_To_LP_CST_ID_Transfer()
        {
            if (Carrier_CheckBP_ExistID(0) &&
                Carrier_CheckLP_ExistProduct(true, false))
            {
                string BPCarrierID = Carrier_GetBP_CarrierID(0);
                LP_CarrierID = BPCarrierID;

                if (Carrier_CheckLP_ExistID() &&
                    Carrier_CheckShuttle_ExistProduct(false))
                {
                    Carrier_ClearBP_CarrierID(0);
                }

                return false;
            }
            else if (!Carrier_CheckBP_ExistID(0) &&
                    Carrier_CheckLP_ExistID())
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Shuttle -> OP ID 이전 함수
        /// </summary>
        /// <returns></returns>
        public bool Carrier_Shuttle_To_OP_CST_ID_Transfer()
        {
            if (Carrier_CheckBP_ExistID(0) &&
                Carrier_CheckOP_ExistProduct(true, false))
            {
                string BPCarrierID = Carrier_GetBP_CarrierID(0);
                OP_CarrierID = BPCarrierID;

                if (Carrier_CheckOP_ExistID() &&
                    Carrier_CheckShuttle_ExistProduct(false))
                {
                    Carrier_ClearBP_CarrierID(0);
                }

                return false;
            }
            else if (!Carrier_CheckBP_ExistID(0) &&
                    Carrier_CheckOP_ExistID())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 연결 물류 N번의 CST ID를 가져옴 
        /// </summary>
        /// <param name="BPIndex"></param>
        /// <returns></returns>
        public string Carrier_GetBP_CarrierID(int BPIndex)
        {
            if (BPIndex >= BP_CarrierID.Length)
                return string.Empty;

            return BP_CarrierID[BPIndex];
        }

        /// <summary>
        /// OP or LP or Shuttle의 자재 유무 확인
        /// bWithPresence : true (대각 까지 체크)
        /// bWithPresence : false (대각 제외 체크)
        /// 셔틀이 자재를 뜨거나 내려놓는 경우 눌림 센서와 대각과의 불일치 상황이 존재
        /// Diebank의 경우 Z축이 하강 상태인 경우 CV IN, STOP으로 감지하며 Z축이 상승 상태인 경우 Toggle을 통해 감지해야 함
        /// </summary>
        /// <param name="bProductExist"></param>
        /// <param name="bWithPresence"></param>
        /// <returns></returns>
        public bool Carrier_CheckOP_ExistProduct(bool bProductExist, bool bWithPresence = true)
        {
            if (bProductExist)
            {
                if (GetParam().ePortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
                {
                    if (Sensor_OP_CV_IN ||
                        Sensor_OP_CV_STOP)
                        return true;
                }
                else
                {
                    if (Sensor_OP_CST_Detect1 &&
                        Sensor_OP_CST_Detect2 &&
                        (bWithPresence ? Sensor_OP_CST_Presence : true))
                        return true;
                }

                return false;
            }
            else
            {
                if (GetParam().ePortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
                {
                    if (!Sensor_OP_CV_IN &&
                        !Sensor_OP_CV_STOP)
                        return true;
                }
                else
                {
                    if (!Sensor_OP_CST_Detect1 &&
                        !Sensor_OP_CST_Detect2 &&
                        (bWithPresence ? !Sensor_OP_CST_Presence : true))
                        return true;
                }

                return false;
            }
        }
        public bool Carrier_CheckLP_ExistProduct(bool bProductExist, bool bWithPresence = true)
        {
            if (bProductExist)
            {
                if (GetParam().ePortType == PortType.MGV_OHT || GetParam().ePortType == PortType.OHT)
                {
                    bool bLP_CST_Detect2 = !IsValidInputItemMapping(OHT_InputItem.LP_Placement_Detect_2.ToString()) ? true : Sensor_LP_CST_Detect2;

                    if (Sensor_LP_CST_Detect1 && bLP_CST_Detect2 &&
                        (bWithPresence ? Sensor_LP_CST_Presence : true))
                        return true;
                }
                else if (GetParam().ePortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
                {
                    if (Sensor_LP_CV_IN ||
                        Sensor_LP_CV_STOP)
                        return true;
                }
                else
                {
                    if (Sensor_LP_CST_Detect1 &&
                        Sensor_LP_CST_Detect2 &&
                        (bWithPresence ? Sensor_LP_CST_Presence : true))
                        return true;
                }

                return false;
            }
            else
            {
                if (GetParam().ePortType == PortType.MGV_OHT || GetParam().ePortType == PortType.OHT)
                {
                    bool bLP_CST_Detect2 = !IsValidInputItemMapping(OHT_InputItem.LP_Placement_Detect_2.ToString()) ? false : Sensor_LP_CST_Detect2;

                    if (!Sensor_LP_CST_Detect1 && !bLP_CST_Detect2 &&
                        (bWithPresence ? !Sensor_LP_CST_Presence : true))
                        return true;
                }
                else if (GetParam().ePortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
                {
                    if (!Sensor_LP_CV_IN &&
                        !Sensor_LP_CV_STOP)
                        return true;
                }
                else
                {
                    if (!Sensor_LP_CST_Detect1 &&
                        !Sensor_LP_CST_Detect2 &&
                        (bWithPresence ? !Sensor_LP_CST_Presence : true))
                        return true;
                }

                return false;
            }
        }
        public bool Carrier_CheckShuttle_ExistProduct(bool bProductExist)
        {
            if (bProductExist)
            {
                if (Sensor_Shuttle_CSTDetect1 &&
                    Sensor_Shuttle_CSTDetect2)
                    return true;

                return false;
            }
            else
            {
                if (!Sensor_Shuttle_CSTDetect1 &&
                    !Sensor_Shuttle_CSTDetect2)
                    return true;

                return false;
            }
        }
        
        /// <summary>
        /// 컨베이어 연결 물류에서의 자재 상태 확인
        /// 기능만 추가하고 사용한적 없음(연결 물류 상황 검증 필요)
        /// </summary>
        /// <param name="BPIndex"></param>
        /// <param name="bProductExist"></param>
        /// <returns></returns>
        public bool Carrier_CheckCVBP_ExistProduct(int BPIndex, bool bProductExist)
        {
            BufferCV eBufferCV = (BufferCV)(BPIndex + 2);

            if (bProductExist)
            {
                if (BufferCtrl_BP_CSTDetect_Status(eBufferCV))
                    return true;

                return false;
            }
            else
            {
                if (!BufferCtrl_BP_CSTDetect_Status(eBufferCV))
                    return true;

                return false;
            }
        }

        /// <summary>
        /// CST ID 존재 여부 확인
        /// </summary>
        /// <returns></returns>
        private bool Carrier_CheckOP_ExistID()
        {
            if (OP_CarrierID != string.Empty)
                return true;

            return false;
        }
        private bool Carrier_CheckLP_ExistID()
        {
            if (LP_CarrierID != string.Empty)
                return true;

            return false;
        }
        private bool Carrier_CheckBP_ExistID(int BPIndex)
        {
            if (BPIndex >= BP_CarrierID.Length)
                return false;

            if (BP_CarrierID[BPIndex] != string.Empty)
                return true;

            return false;
        }

        /// <summary>
        /// Shuttle or 연결 물류 CST ID Clear
        /// One Buffer Type의 경우 Shuttle이 LP라고 가정하여 Buffer2 CST ID 메모리 맵에 맵핑
        /// </summary>
        /// <param name="BPIndex"></param>
        public void Carrier_ClearBP_CarrierID(int BPIndex)
        {
            if (BPIndex >= BP_CarrierID.Length)
                return;

            BP_CarrierID[BPIndex] = string.Empty;

            if (BPIndex == 0 && IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
            {
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer2_Carrier_ID_01, BP_CarrierID[BPIndex]);
            }
        }
        public void Carrier_SetBP_CarrierID(int BPIndex, string CarrierID)
        {
            if (BPIndex >= BP_CarrierID.Length)
                return;

            if (BP_CarrierID[BPIndex] != CarrierID)
            {
                if(CarrierID != string.Empty)
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"BP[{BPIndex}] CST ID : {BP_CarrierID[BPIndex]} => {CarrierID}");
                else
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"BP[{BPIndex}] CST ID : {BP_CarrierID[BPIndex]} => Clear");

                BP_CarrierID[BPIndex] = CarrierID;

                if (BPIndex == 0 && IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                {
                    Set_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer2_Carrier_ID_01, BP_CarrierID[BPIndex]);
                }

                if (BPIndex == 0)
                    CassetteInfo.WriteCSTID(GetParam().ID, CassetteInfo.CassetteInfoKey.BP_CST_ID1, BP_CarrierID[BPIndex]);
                else if (BPIndex == 1)
                    CassetteInfo.WriteCSTID(GetParam().ID, CassetteInfo.CassetteInfoKey.BP_CST_ID2, BP_CarrierID[BPIndex]);
                else if (BPIndex == 2)
                    CassetteInfo.WriteCSTID(GetParam().ID, CassetteInfo.CassetteInfoKey.BP_CST_ID3, BP_CarrierID[BPIndex]);
                else if (BPIndex == 3)
                    CassetteInfo.WriteCSTID(GetParam().ID, CassetteInfo.CassetteInfoKey.BP_CST_ID4, BP_CarrierID[BPIndex]);
            }
        }

        /// <summary>
        /// Port -> STK에 CST ID 전송 [Set 기능에 존재]
        /// </summary>
        /// <param name="CarrierID"></param>
        private void Carrier_ACK_PortToRM_CarrierID(string CarrierID)
        {
            try
            {
                foreach (var rackMaster in Master.m_RackMasters)
                {
                    string AccessID = Convert.ToString(rackMaster.Value.Status_STK_To_CIM_AccessID);

                    if (GetParam().ID == AccessID)
                    {
                        //1. RM에서 받은 경우 RM->CIM 의 Carrier ID를 CIM -> RM Carrier ID 영역에 복사 후 Send (Ack 개념)
                        //2. RM으로 보내는 경우 Op Carrier ID를 CIM-> RM Carrier ID 영역에 복사 후 Send
                        rackMaster.Value.CMD_CIM_To_STK_CarrierID = CarrierID;
                        break;
                    }
                }
            }
            catch { }
        }
        
        /// <summary>
        /// STK -> Port에 전송한 CST ID 가져옴
        /// </summary>
        /// <returns></returns>
        public string Carrier_GetRMToPort_RecvMapCarrierID()
        {
            try
            {
                foreach (var rackMaster in Master.m_RackMasters)
                {
                    string AccessID = Convert.ToString(rackMaster.Value.Status_STK_To_CIM_AccessID);

                    if (GetParam().ID == AccessID)
                    {
                        return rackMaster.Value.Status_STK_To_CIM_CarrierID;
                    }
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        
        /// <summary>
        /// Port -> STK에 보낼 CST ID를 가져옴
        /// </summary>
        /// <returns></returns>
        public string Carrier_GetPortToRM_SendMapCarrierID()
        {
            try
            {
                foreach (var rackMaster in Master.m_RackMasters)
                {
                    string AccessID = Convert.ToString(rackMaster.Value.Status_STK_To_CIM_AccessID);

                    if (GetParam().ID == AccessID)
                    {
                        return rackMaster.Value.CMD_CIM_To_STK_CarrierID;
                    }
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Port -> STK에 보낼 CST ID를 초기화 [Set과 동일]
        /// </summary>
        private void Carrier_ClearPortToRM_CarrierID()
        {
            try
            {
                foreach (var rackMaster in Master.m_RackMasters)
                {
                    string FromID   = Convert.ToString(rackMaster.Value.Status_STK_To_CIM_FromID);
                    string ToID     = Convert.ToString(rackMaster.Value.Status_STK_To_CIM_ToID);

                    if (GetParam().ID == FromID || GetParam().ID == ToID)
                    {
                        rackMaster.Value.CMD_CIM_To_STK_CarrierID = string.Empty;
                        break;
                    }
                }
            }
            catch
            {

            }
        }


    }
}
