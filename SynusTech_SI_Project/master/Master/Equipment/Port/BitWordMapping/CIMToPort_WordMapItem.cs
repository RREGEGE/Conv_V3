using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Equipment.Port
{
    /// <summary>
    /// [CIM -> Master] -> Port Word Memory Map에 사용되는 변수 기재
    /// </summary>
    public partial class Port
    {
        short m_nBuffer1_Control = 0;
        short m_nBuffer2_Control = 0;
        short m_nBufferSyncControl = 0;

        string m_CIMToPort_CarrierID = string.Empty;
        short m_nCIMToPort_CarrierID_WriteFlag = 0;

        /// <summary>
        /// 현재 CST ID는 모두 마스터에서 관리하므로 CIM에서 보내주는 경우 없음
        /// 아래 두 기능은 맵핑만 해뒀고 실제로 
        /// </summary>
        public string CMD_CIMToPortCarrierID
        {
            get {
                m_CIMToPort_CarrierID = (string)Get_CIM_2_Port_Word_Data(ReceiveWordMapIndex.Carrier_ID_1);
                return m_CIMToPort_CarrierID;
            }
            set { m_CIMToPort_CarrierID = value; }
        }
        public short CMD_CarrierID_WriteFlag
        {
            get {
                m_nCIMToPort_CarrierID_WriteFlag = (short)Get_CIM_2_Port_Word_Data(ReceiveWordMapIndex.Carrier_ID_WriteFlag);
                return m_nCIMToPort_CarrierID_WriteFlag; 
            }
            set { m_nCIMToPort_CarrierID_WriteFlag = value; }
        }
    }
}
