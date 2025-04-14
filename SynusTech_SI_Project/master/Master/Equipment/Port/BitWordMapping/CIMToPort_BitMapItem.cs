using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Equipment.Port
{
    /// <summary>
    /// [CIM -> Master] -> Port Bit Memory Map에 사용되는 변수 기재
    /// </summary>
    public partial class Port
    {
        //CIM 명령
        bool m_bRun_REQ = false;
        bool m_bStop_REQ = false;
        bool m_bPowerOn_REQ = false;
        bool m_bPowerOff_REQ = false;
        bool m_bInMode_REQ = false;
        bool m_bOutMode_REQ = false;
        bool m_bAGVorOHTMode_REQ = false;
        bool m_bMGVMode_REQ = false;
        bool m_bMoveReserved = false;
        bool m_bErrorReset_REQ = false;

        /// <summary>
        /// Packet을 통해 들어온 값을 확인하여 변수에 적용 (set 영역에서 변수에 적용 하며 동작 명령 수행) 
        /// </summary>
        public bool CMD_Run_REQ
        {
            get { return m_bRun_REQ; }
            set { 
                m_bRun_REQ = value;
                if (m_bRun_REQ)
                    Interlock_StartAutoControl(InterlockFrom.TCPIP);
            }
        }
        public bool CMD_Stop_REQ
        {
            get { return m_bStop_REQ; }
            set { 
                m_bStop_REQ = value;
                if (m_bStop_REQ)
                    Interlock_StopAutoControl(InterlockFrom.TCPIP);
            }
        }
        public bool CMD_PowerOn_REQ
        {
            get { return m_bPowerOn_REQ; }
            set { 
                m_bPowerOn_REQ = value;
                if (m_bPowerOn_REQ)
                    Interlock_PortPowerOn(InterlockFrom.TCPIP);
            }
        }
        public bool CMD_PowerOff_REQ
        {
            get { return m_bPowerOff_REQ; }
            set { 
                m_bPowerOff_REQ = value;
                if (m_bPowerOff_REQ)
                    Interlock_PortPowerOff(InterlockFrom.TCPIP);
            }
        }
        public bool CMD_InMode_REQ
        {
            get { return m_bInMode_REQ; }
            set { 
                m_bInMode_REQ = value;
                if (m_bInMode_REQ)
                    Interlock_AutoControlDirectionChange(PortDirection.Input, InterlockFrom.TCPIP);
            }
        }
        public bool CMD_OutMode_REQ
        {
            get { return m_bOutMode_REQ; }
            set { 
                m_bOutMode_REQ = value;
                if (m_bOutMode_REQ)
                    Interlock_AutoControlDirectionChange(PortDirection.Output, InterlockFrom.TCPIP);
            }
        }
        public bool CMD_AGVorOHTMode_REQ
        {
            get { return m_bAGVorOHTMode_REQ; }
            set { 
                m_bAGVorOHTMode_REQ = value;
                if (m_bAGVorOHTMode_REQ)
                {
                    if (GetParam().ePortType == PortType.MGV_AGV)
                        Interlock_OperationModeChange(PortOperationMode.AGV, InterlockFrom.TCPIP);
                    else if (GetParam().ePortType == PortType.MGV_OHT)
                        Interlock_OperationModeChange(PortOperationMode.OHT, InterlockFrom.TCPIP);
                }
            }
        }
        public bool CMD_MGVMode_REQ
        {
            get { return m_bMGVMode_REQ; }
            set { 
                m_bMGVMode_REQ = value;
                if (m_bMGVMode_REQ)
                {
                    if (GetParam().ePortType == PortType.MGV_AGV || GetParam().ePortType == PortType.MGV_OHT)
                        Interlock_OperationModeChange(PortOperationMode.MGV, InterlockFrom.TCPIP);
                }
            }
        }
        public bool CMD_MoveReserved
        {
            get { return m_bMoveReserved; }
            set { 
                m_bMoveReserved = value; 
            }
        }
        public bool CMD_ErrorReset
        {
            get { return m_bErrorReset_REQ; }
            set { 
                m_bErrorReset_REQ = value;
                if (m_bErrorReset_REQ)
                {
                    Interlock_PortAmpAlarmClear(InterlockFrom.TCPIP);
                    Interlock_PortAlarmClear(InterlockFrom.TCPIP);
                }
            }
        }
    }
}
