using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Master.Equipment.CIM;

namespace Master
{
    /// <summary>
    /// [CIM -> Master] Bit Memory Map에 사용되는 변수 기재
    /// </summary>
    static partial class Master
    {
        static Stopwatch st_DoorOpen = new Stopwatch();
        static Stopwatch st_DoorClose = new Stopwatch();
        static bool m_bDoor_Open_Relay = false;
        static bool m_bDoor_Bypass_Relay = false;
        static bool m_bDoorOpen_REQ = false;
        static bool m_bTowerLampLED_RED = false;
        static bool m_bTowerLampLED_GREEN = false;
        static bool m_bTowerLampLED_YELLOW = false;
        static bool m_bBuzzer_MasterAlarm = false;
        static bool m_bBuzzer_RackMasterActive = false;
        static bool m_bBuzzer_PortAlarm = false;
        static bool m_bBuzzer_Mute = false;

        static bool m_bCPSPowerOn = false;
        static bool m_bCPSErrorReset = false;

        /// <summary>
        /// [CIM -> Master] Door 제어에 따라 On/Off 되는 출력 변수
        /// CIM -> Master Door Open REQ에 따라 제어 됨
        /// Master UI에서 Door Open Click에 따라 제어 됨
        /// </summary>
        static public bool CMD_DoorOpen_Relay
        {
            get { return m_bDoor_Open_Relay; }
            set
            {
                m_bDoor_Open_Relay = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] Door 제어에 따라 On/Off 되는 출력 변수
        /// CIM -> Master Door Open REQ에 따라 제어 됨
        /// Master UI에서 Door Open Click에 따라 제어 됨
        /// </summary>
        static public bool CMD_DoorBypass_Relay
        {
            get { return m_bDoor_Bypass_Relay; }
            set
            {
                m_bDoor_Bypass_Relay = value;
            }
        }

        /// <summary>
        /// [CIM -> Master] Door 제어 명령
        /// 해당 REQ 상태에 따라 Door Relay가 제어 됨
        /// </summary>
        static public bool CMD_DoorOpen_REQ
        {
            get { return m_bDoorOpen_REQ; }
            set
            {
                m_bDoorOpen_REQ = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_CIM_2_Master_Bit_Data(CIM.ReceiveBitMapIndex.DoorOpen, m_bDoorOpen_REQ);
            }
        }

        /// <summary>
        /// [CIM -> Master] Tower Lamp Red On/Off 명령
        /// 해당 REQ 상태에 따라 Tower Lamp에 Output 출력 진행
        /// CIM -> Master Bit Map이지만 Master에서 제어 됨
        /// </summary>
        static public bool CMD_TowerLamp_LED_RED_REQ
        {
            get { return m_bTowerLampLED_RED; }
            set
            {
                m_bTowerLampLED_RED = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_CIM_2_Master_Bit_Data(CIM.ReceiveBitMapIndex.TowerLamp_LED_Red, m_bTowerLampLED_RED);
            }
        }

        /// <summary>
        /// [CIM -> Master] Tower Lamp Yellow On/Off 명령
        /// 해당 REQ 상태에 따라 Tower Lamp에 Output 출력 진행
        /// CIM -> Master Bit Map이지만 Master에서 제어 됨
        /// </summary>
        static public bool CMD_TowerLamp_LED_YELLOW_REQ
        {
            get { return m_bTowerLampLED_YELLOW; }
            set
            {
                m_bTowerLampLED_YELLOW = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_CIM_2_Master_Bit_Data(CIM.ReceiveBitMapIndex.TowerLamp_LED_Yellow, m_bTowerLampLED_YELLOW);
            }
        }

        /// <summary>
        /// [CIM -> Master] Tower Lamp Green On/Off 명령
        /// 해당 REQ 상태에 따라 Tower Lamp에 Output 출력 진행
        /// CIM -> Master Bit Map이지만 Master에서 제어 됨
        /// </summary>
        static public bool CMD_TowerLamp_LED_GREEN_REQ
        {
            get { return m_bTowerLampLED_GREEN; }
            set
            {
                m_bTowerLampLED_GREEN = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_CIM_2_Master_Bit_Data(CIM.ReceiveBitMapIndex.TowerLamp_LED_Green, m_bTowerLampLED_GREEN);
            }
        }

        /// <summary>
        /// [CIM -> Master] Buzzer On/Off 명령 (마스터 알람 시 발생하는 부저 음)
        /// 해당 REQ 상태에 따라 Buzzer에 Output 출력 진행
        /// CIM -> Master Bit Map이지만 Master에서 제어 됨
        /// </summary>
        static public bool CMD_Buzzer_MasterAlarm_REQ
        {
            get { return m_bBuzzer_MasterAlarm; }
            set
            {
                m_bBuzzer_MasterAlarm = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_CIM_2_Master_Bit_Data(CIM.ReceiveBitMapIndex.TowerLamp_BUZZER_MasterAlarm, m_bBuzzer_MasterAlarm);
            }
        }

        /// <summary>
        /// [CIM -> Master] Buzzer On/Off 명령 (스토커 액티브 상태시 발생하는 부저 음)
        /// 해당 REQ 상태에 따라 Buzzer에 Output 출력 진행
        /// CIM -> Master Bit Map이지만 Master에서 제어 됨
        /// </summary>
        static public bool CMD_Buzzer_RackMasterActive_REQ
        {
            get { return m_bBuzzer_RackMasterActive; }
            set
            {
                m_bBuzzer_RackMasterActive = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_CIM_2_Master_Bit_Data(CIM.ReceiveBitMapIndex.TowerLamp_BUZZER_RackMasterActive, m_bBuzzer_RackMasterActive);
            }
        }

        /// <summary>
        /// [CIM -> Master] Buzzer On/Off 명령 (포트 알람 시 발생하는 부저 음)
        /// 해당 REQ 상태에 따라 Buzzer에 Output 출력 진행
        /// CIM -> Master Bit Map이지만 Master에서 제어 됨
        /// </summary>
        static public bool CMD_Buzzer_PortAlarm_REQ
        {
            get { return m_bBuzzer_PortAlarm; }
            set
            {
                m_bBuzzer_PortAlarm = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_CIM_2_Master_Bit_Data(CIM.ReceiveBitMapIndex.TowerLamp_BUZZER_PortAlarm, m_bBuzzer_PortAlarm);
            }
        }

        /// <summary>
        /// [CIM -> Master] Buzzer Off 명령
        /// 해당 REQ 상태에 따라 Buzzer의 Output을 모두 Off 진행
        /// CIM -> Master Bit Map이지만 Master에서 제어 됨(화면 상단 부저 버튼)
        /// </summary>
        static public bool CMD_Buzzer_Mute_REQ
        {
            get { return m_bBuzzer_Mute; }
            set
            {
                m_bBuzzer_Mute = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_CIM_2_Master_Bit_Data(CIM.ReceiveBitMapIndex.TowerLamp_BUZZER_MUTE, m_bBuzzer_Mute);
            }
        }

        /// <summary>
        /// [CIM -> Master] CPS Power On 명령
        /// 해당 REQ 상태에 따라 CPS Power On에 맵핑된 출력이 제어 됨
        /// 실제 사용하지는 않고 있음 (현재 실리콘박스 CPS 하드웨어 상 On/Off 제어가 불가능, 기능만 구현, Output 연동 X)
        /// </summary>
        static public bool CMD_CPS_Power_On_REQ
        {
            get { return m_bCPSPowerOn; }
            set
            {
                m_bCPSPowerOn = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_CIM_2_Master_Bit_Data(CIM.ReceiveBitMapIndex.CPS_Power_On_Request, m_bCPSPowerOn);
            }
        }

        /// <summary>
        /// [CIM -> Master] CPS Error Reset 명령
        /// 해당 REQ 상태에 따라 CPS Error Reset에 맵핑된 출력이 제어 됨
        /// 실제 사용하지는 않고 있음 (현재 실리콘박스 CPS 하드웨어 상 On/Off 제어가 불가능, 기능만 구현, Output 연동 X)
        /// </summary>
        static public bool CMD_CPS_Error_Reset_REQ
        {
            get { return m_bCPSErrorReset; }
            set
            {
                m_bCPSErrorReset = value;

                if (m_CIM == null)
                    return;

                m_CIM.Set_CIM_2_Master_Bit_Data(CIM.ReceiveBitMapIndex.CPS_Error_Reset_Request, m_bCPSErrorReset);
            }
        }
    }
}
