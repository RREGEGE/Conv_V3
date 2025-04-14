using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;
using System.Threading;
using MovenCore;
using Master.Equipment.CIM;
using Master.Equipment.Port;
using Master.Equipment.RackMaster;
using Master.Interface.Alarm;
using System.Windows.Forms;
using Master.GlobalForm;

namespace Master
{
    /// <summary>
    /// Master_WMXIO.cs 는 WMX IO를 통해 Input, Output 제어 할 목록을 생성하고 맵핑하는 영역 입니다.
    /// </summary>
    static partial class Master
    {
        /// <summary>
        /// Master에서 사용되는 Input Item 항목
        /// </summary>
        public enum MasterInputItem
        {
            Main_MC_On,
            RM_Power,
            MCUL_Fault,

            //Door
            HP_Door_Open,
            OP_Door_Open,
            Maint_Door_Open,
            Maint_Door_Open2,

            //Key
            HP_AutoManual_Select_Key,
            OP_AutoManual_Select_Key,

            //EMO
            HP_Outside_EMO,//
            HP_Inside_EMO,//
            OP_Outside_EMO,//
            OP_Inside_EMO,//

            //HandyTouch
            Port_DTP_EMO,
            Port_DTP_Dead_Man,
            Port_DTP_Mode1,
            Port_DTP_Mode2,
            HP_DTP_EMO,
            HP_DTP_Dead_Man,
            HP_DTP_Mode1,
            HP_DTP_Mode2,
            OP_DTP_EMO,
            OP_DTP_Dead_Man,
            OP_DTP_Mode1,
            OP_DTP_Mode2,

            //CPS
            CPS_Run,
            CPS_Fault,
            CPS_Power_On_Enable_Lamp,
            CPS_Power_On_Req_Lamp,
            CPS_Error_Reset_Req_Lamp,
            CPS_Error_Lamp,

            //DieBank
            Inner_EMO_1,
            Inner_EMO_2,
            Inner_EMO_3,
            Inner_EMO_4,

            //N2
            Oxygen_Saturation_Warning,
        }

        /// <summary>
        /// Master에서 사용되는 Output Item 항목
        /// </summary>
        public enum MasterOutputItem
        {
            Door_Bypass_Relay_On,
            Door_Open_Relay_On,

            Main_Reset_SW_Lamp,

            Tower_Lamp_Red,
            Tower_Lamp_Yellow,
            Tower_Lamp_Green,
            Buzzer_MasterError,
            Buzzer_PortError,
            Buzzer_RackMasterActive,
        }

        /// <summary>
        /// Master에서 제어되는 Tower Lamp의 점등 항목
        /// </summary>
        public enum TowerLampOutput
        {
            STK_Error,
            STK_Auto,
            STK_Manuel,
            STK_Disconnection,
            None
        }

        static private WMXIO m_WMXIO            = new WMXIO();              //WMX IO 제어를 진행하기 위한 클래스
        static private Thread m_IOUpdateThread  = new Thread(IOUpdate);     //IO 상시 업데이트를 위한 스레드
        static private object UpdateLock        = new object();             //IO 업데이트와 파라미터 적용시 접근 충돌을 막기위한 Lock Obj

        /// <summary>
        /// 현재 Lock 객체를 얻어 옴
        /// </summary>
        /// <returns></returns>
        static public object GetIOUpdateLock()
        {
            return UpdateLock;
        }

        /// <summary>
        /// Master I/O 상시 업데이트 부분
        /// </summary>
        static private void IOUpdate()
        {
            //1. 최초 진입 시 WMX I/O 상태와 Prog Data와 동기화 처리
            if (Master.m_CIM_SendBitMap.Length > 0)
                InitOutputValueToBitMap();

            //2. 상시 업데이트 진행
            while (true)
            {
                if (Master.m_CIM_SendBitMap.Length > 0)
                {
                    //3. 파라미터 Apply or Save 하고있는 상황이 아니라면
                    lock (UpdateLock)
                    {
                        //4. WMX Input 업데이트 후 상태 변수에 반영
                        WMX_IO_InputToBitMap();
                        //5. 상황에 따른 Door Open Flag 업데이트
                        Output_DoorOpenUpdate();
                        //6. 상황에 따른 Tower Lamp Flag 업데이트
                        Output_TowerLampUpdate();
                        //7. 상황에 따른 Buzzer Flag 업데이트
                        Output_BuzzerUpdate();
                        //8. 플래그에 따라 Output 업데이트 
                        WMX_IO_BitMapToOutput();
                    }

                    //9. DeadMan 상태에 따른 정지 명령
                    EquipStopCMDFromDeadManStatus();

                    //10. Master -> CIM Memory Map Update (Master는 별도 Status Update 스레드를 생성하지 않고 I/O 영역에서 업데이트 진행)
                    MasterToCIMBitMapUpdate();
                }
                Thread.Sleep(Master.StatusUpdateTime);
            }
        }

        /// <summary>
        /// Master I/O Update Thread 진행 전 I/O 상태 <-> Flag, 변수 동기화 하는 부분
        /// </summary>
        static private void InitOutputValueToBitMap()
        {
            var OutputMap = GetMotionParam().Ctrl_IO.OutputMap;

            //1. Output 현재 상태 업데이트 및 Flag에 연동
            for (int nCount = 0; nCount < OutputMap.Length; nCount++)
            {
                var IOMap = OutputMap[nCount];

                if (!string.IsNullOrEmpty(IOMap.Name) &&
                    Enum.IsDefined(typeof(MasterOutputItem), IOMap.Name))
                {
                    MasterOutputItem Item = (MasterOutputItem)Enum.Parse(typeof(MasterOutputItem), IOMap.Name);

                    int StartAddr = IOMap.StartAddr;
                    int Bit = IOMap.Bit;

                    if (StartAddr < 0 || Bit < 0)
                        continue;

                    bool bEnable = GetOutBit(StartAddr, Bit);
                    WMX_IO_ItemToMapAction(Item, (IOMap.bInvert ? !bEnable : bEnable));
                }
            }

            //2. Door Open Relay I/O 상태에 따른 Door Open REQ 상태 동기화
            if (CMD_DoorOpen_Relay && CMD_DoorBypass_Relay)
            {
                //3. Relay가 둘다 들어와 있는 경우 Door가 Open된 상태며 Door Open 명령도 동기화 진행
                CMD_DoorOpen_REQ = true;
            }
            else
            {
                //4. Relay가 둘중 하나라도 안들어와있는 상태면 Door는 닫힌 상태이므로 출력 및 Flag 초기화
                CMD_DoorOpen_Relay = false;
                CMD_DoorBypass_Relay = false;
                CMD_DoorOpen_REQ = false;
            }
        }

        /// <summary>
        /// Master Input Update
        /// WMX Input 값을 얻은 후 센서 변수에 반영
        /// </summary>
        static private void WMX_IO_InputToBitMap()
        {
            var Ctrl_IOMap = GetMotionParam().Ctrl_IO;
            var InputMap = Ctrl_IOMap.InputMap;

            for (int nCount = 0; nCount < InputMap.Length; nCount++)
            {
                var IOMap = InputMap[nCount];
                if (!string.IsNullOrEmpty(IOMap.Name) &&
                Enum.IsDefined(typeof(MasterInputItem), IOMap.Name))
                {
                    MasterInputItem Item = (MasterInputItem)Enum.Parse(typeof(MasterInputItem), IOMap.Name);

                    int StartAddr = IOMap.StartAddr;
                    int Bit = IOMap.Bit;

                    if (StartAddr < 0 || Bit < 0)
                        continue;

                    bool bEnable = IOMap.bInvert ? !GetInputBit(StartAddr, Bit) : GetInputBit(StartAddr, Bit);

                    WMX_IO_ItemToMapAction(Item, bEnable);
                }
            }
        }

        /// <summary>
        /// Door Open Update 제어
        /// Door Open시 BypassRelay On -> 3sec -> OpenRelay On이 되어야 함
        /// Door Close시 OpenRelay Off -> 3sec -> BypassRelay Off가 되어야 함
        /// </summary>
        static private void Output_DoorOpenUpdate()
        {
            //Door Open 시나리오 1 (Door Open 스탑 워치 실행 및 Bypass Relay On)
            if (CMD_DoorOpen_REQ && !CMD_DoorBypass_Relay)
            {
                if (!st_DoorOpen.IsRunning)
                {
                    st_DoorOpen.Reset();
                    st_DoorOpen.Start();

                    st_DoorClose.Stop();
                    st_DoorClose.Reset();
                }
                CMD_DoorBypass_Relay = true;
            }
            else
            {
                //Door Close 시나리오 2 (Door Close 스탑 워치 3초 지났으면서 DoorBypass Relay가 On인 경우 Off)
                if (!CMD_DoorOpen_REQ && st_DoorClose.ElapsedMilliseconds > 3000 && CMD_DoorBypass_Relay)
                {
                    st_DoorClose.Stop();
                    CMD_DoorBypass_Relay = false;
                }
                //Door Abnormal 시나리오 (Door Open 명령이 들어왔는데 Bypass Relay가 켜져 있고 Close 스탑워치가 돌고 있는 경우 일단 Off하여 Door Open 시나리오 1로 진입하도록 유도)
                else if (CMD_DoorOpen_REQ && st_DoorClose.IsRunning && st_DoorClose.ElapsedMilliseconds > 3000 && CMD_DoorBypass_Relay)
                {
                    st_DoorClose.Stop();
                    CMD_DoorBypass_Relay = false;
                }
            }

            //Door Close 시나리오 1 (Door Open Relay On인 경우 Close 스탑 워치 실행 및 DoorOpenRelay Off)
            if (!CMD_DoorOpen_REQ && CMD_DoorOpen_Relay)
            {
                if (!st_DoorOpen.IsRunning)
                {
                    st_DoorClose.Reset();
                    st_DoorClose.Start();

                    st_DoorOpen.Stop();
                    st_DoorOpen.Reset();
                }
                CMD_DoorOpen_Relay = false;
            }
            else
            {
                //Door Open 시나리오 2 (Door Open 스탑 워치 3초 지났으면서 DoorOpen Relay Off인 경우 DoorOpen Relay On)
                if (CMD_DoorOpen_REQ && st_DoorOpen.ElapsedMilliseconds > 3000 && !CMD_DoorOpen_Relay)
                {
                    st_DoorOpen.Stop();
                    CMD_DoorOpen_Relay = true;
                }
            }
        }

        /// <summary>
        /// Tower Lamp는 Master가 직접 업데이트
        /// </summary>
        static private void Output_TowerLampUpdate()
        {
            foreach (var rackMaster in m_RackMasters)
            {
                if (rackMaster.Value.Status_Error)
                {
                    SetTowerLampOutput(TowerLampOutput.STK_Error);
                }
                else if (rackMaster.Value.Status_AutoMode)
                {
                    SetTowerLampOutput(TowerLampOutput.STK_Auto);
                }
                else if (rackMaster.Value.Status_ManualMode)
                {
                    SetTowerLampOutput(TowerLampOutput.STK_Manuel);
                }
                else if (!rackMaster.Value.IsConnected())
                {
                    SetTowerLampOutput(TowerLampOutput.STK_Disconnection);
                }
                else
                    SetTowerLampOutput(TowerLampOutput.None);

                break;
            }
        }

        /// <summary>
        /// Tower Lamp명령에 따른 출력 플래그 업데이트
        /// </summary>
        static private void SetTowerLampOutput(TowerLampOutput eTowerLampOutput)
        {
            switch (eTowerLampOutput)
            {
                case TowerLampOutput.STK_Error:         //RED Color
                    CMD_TowerLamp_LED_RED_REQ       = true;
                    CMD_TowerLamp_LED_YELLOW_REQ    = false;
                    CMD_TowerLamp_LED_GREEN_REQ     = false;
                    break;
                case TowerLampOutput.STK_Auto:          //GREEN Color
                    CMD_TowerLamp_LED_RED_REQ       = false;
                    CMD_TowerLamp_LED_YELLOW_REQ    = false;
                    CMD_TowerLamp_LED_GREEN_REQ     = true;
                    break;
                case TowerLampOutput.STK_Manuel:        //YELLOW Color
                    CMD_TowerLamp_LED_RED_REQ       = false;
                    CMD_TowerLamp_LED_YELLOW_REQ    = true;
                    CMD_TowerLamp_LED_GREEN_REQ     = false;
                    break;
                case TowerLampOutput.STK_Disconnection: //RED <-> YELLOW
                    CMD_TowerLamp_LED_RED_REQ       = ErrorIntervalColor == Color.Red ? false : true;
                    CMD_TowerLamp_LED_YELLOW_REQ    = ErrorIntervalColor == Color.Red ? true : false;
                    CMD_TowerLamp_LED_GREEN_REQ     = false;
                    break;
                default:                                //None
                    CMD_TowerLamp_LED_RED_REQ       = false;
                    CMD_TowerLamp_LED_YELLOW_REQ    = false;
                    CMD_TowerLamp_LED_GREEN_REQ     = false;
                    break;
            }
        }

        /// <summary>
        /// Buzzer는 Master가 직접 업데이트
        /// </summary>
        static private void Output_BuzzerUpdate()
        {
            //음소거 명령인 경우 전체 Buzzer Flag Off
            if (CMD_Buzzer_Mute_REQ)
            {
                CMD_Buzzer_MasterAlarm_REQ = false;
                CMD_Buzzer_RackMasterActive_REQ = false;
                CMD_Buzzer_PortAlarm_REQ = false;
            }
            else
            {
                //상황에 따른 Buzzer Flag Update
                CMD_Buzzer_MasterAlarm_REQ = GetAlarmLevel() == AlarmLevel.Error ? true : false;

                bool bRackMasterMoving = false;
                foreach (var rackMaster in m_RackMasters)
                {
                    if (rackMaster.Value.Status_Active)
                    {
                        CMD_Buzzer_RackMasterActive_REQ = true;
                        bRackMasterMoving = true;
                    }
                }

                if (!bRackMasterMoving)
                    CMD_Buzzer_RackMasterActive_REQ = false;

                bool bPortAlarm = false;
                foreach (var port in m_Ports)
                {
                    if (port.Value.GetAlarmLevel() == Interface.Alarm.AlarmLevel.Error)
                    {
                        CMD_Buzzer_PortAlarm_REQ = true;
                        bPortAlarm = true;
                    }
                }

                if (!bPortAlarm)
                    CMD_Buzzer_PortAlarm_REQ = false;
            }
        }

        /// <summary>
        /// Master Output Update
        /// 프로그램 제어 상황에 따라 반영된 Flag를 WMX Output으로 업데이트 진행
        /// </summary>
        static private void WMX_IO_BitMapToOutput()
        {
            var Ctrl_IOMap = GetMotionParam().Ctrl_IO;

            var OutputMap = Ctrl_IOMap.OutputMap;

            for (int nCount = 0; nCount < OutputMap.Length; nCount++)
            {
                var IOMap = OutputMap[nCount];
                if (!string.IsNullOrEmpty(IOMap.Name) &&
                    Enum.IsDefined(typeof(MasterOutputItem), IOMap.Name))
                {
                    MasterOutputItem Item = (MasterOutputItem)Enum.Parse(typeof(MasterOutputItem), IOMap.Name);

                    int StartAddr = IOMap.StartAddr;
                    int Bit = IOMap.Bit;

                    if (StartAddr < 0 || Bit < 0)
                        continue;

                    if (WMX_IO_ItemToMapData(Item) != null)
                    {
                        bool bEnable = IOMap.bInvert ? !(bool)WMX_IO_ItemToMapData(Item) : (bool)WMX_IO_ItemToMapData(Item);
                        m_WMXIO.SetOutputBit(StartAddr, Bit, bEnable);
                    }
                }
            }
        }

        /// <summary>
        /// WMX Input의 Raw 상태를 가져옴
        /// </summary>
        static public bool GetInputBit(int StartAddr, int Bit)
        {
            return m_WMXIO.GetInputBit(StartAddr, Bit);
        }

        /// <summary>
        /// WMX Output의 Raw 상태를 가져옴
        /// </summary>
        static public bool GetOutBit(int StartAddr, int Bit)
        {
            return m_WMXIO.GetOutputBit(StartAddr, Bit);
        }

        /// <summary>
        /// WMX Input에서 Update된 값을 Prog 센서 변수에 적용
        /// </summary>
        static public void WMX_IO_ItemToMapAction(MasterInputItem eMasterInputItem, bool bEnable)
        {
            switch (eMasterInputItem)
            {
                case MasterInputItem.Main_MC_On:    Sensor_MainMC       = bEnable; break;
                case MasterInputItem.RM_Power:      Sensor_RMPower      = bEnable; break;
                case MasterInputItem.MCUL_Fault:    Sensor_MCULFault    = bEnable; break;

                case MasterInputItem.HP_Door_Open:      Sensor_HPDoorOpen       = bEnable;  break;
                case MasterInputItem.OP_Door_Open:      Sensor_OPDoorOpen       = bEnable;  break;
                case MasterInputItem.Maint_Door_Open:   Sensor_MaintDoorOpen    = bEnable;  break;
                case MasterInputItem.Maint_Door_Open2:  Sensor_MaintDoorOpen2   = bEnable;  break;

                case MasterInputItem.HP_AutoManual_Select_Key: Sensor_HPAutoKey = bEnable;  break;
                case MasterInputItem.OP_AutoManual_Select_Key: Sensor_OPAutoKey = bEnable;  break;


                case MasterInputItem.HP_Outside_EMO:    Sensor_HP_Outside_EMO       = bEnable;  break;
                case MasterInputItem.HP_Inside_EMO:     Sensor_HP_InnerEscape_EMO   = bEnable;  break;
                case MasterInputItem.OP_Outside_EMO:    Sensor_OP_Outside_EMO       = bEnable;  break;
                case MasterInputItem.OP_Inside_EMO:     Sensor_OP_InnerEscape_EMO   = bEnable;  break;

                case MasterInputItem.Port_DTP_EMO:      Sensor_PortHandyTouchEMO        = !bEnable; break; //B접 처리
                case MasterInputItem.Port_DTP_Dead_Man: Sensor_PortHandyTouchDeadMan    = bEnable;  break;
                case MasterInputItem.Port_DTP_Mode1:    Sensor_PortHandyTouchMode1      = bEnable;  break;
                case MasterInputItem.Port_DTP_Mode2:    Sensor_PortHandyTouchMode2      = bEnable;  break;

                case MasterInputItem.HP_DTP_EMO:        Sensor_HPHandyTouchEMO          = !bEnable; break; //B접 처리
                case MasterInputItem.HP_DTP_Dead_Man:   Sensor_HPHandyTouchDeadMan      = bEnable;  break;
                case MasterInputItem.HP_DTP_Mode1:      Sensor_HPHandyTouchMode1        = bEnable;  break;
                case MasterInputItem.HP_DTP_Mode2:      Sensor_HPHandyTouchMode2        = bEnable;  break;

                case MasterInputItem.OP_DTP_EMO:        Sensor_OPHandyTouchEMO          = !bEnable; break; //B접 처리
                case MasterInputItem.OP_DTP_Dead_Man:   Sensor_OPHandyTouchDeadMan      = bEnable;  break;
                case MasterInputItem.OP_DTP_Mode1:      Sensor_OPHandyTouchMode1        = bEnable;  break;
                case MasterInputItem.OP_DTP_Mode2:      Sensor_OPHandyTouchMode2        = bEnable;  break;

                case MasterInputItem.CPS_Run:                   Sensor_CPSRun               = bEnable;  break;
                case MasterInputItem.CPS_Fault:                 Sensor_CPSError             = !bEnable; break; //B접 처리
                case MasterInputItem.CPS_Power_On_Enable_Lamp:  Sensor_CPSPowerOnEnableLamp = bEnable;  break;
                case MasterInputItem.CPS_Power_On_Req_Lamp:     Sensor_CPSPowerOnReqLamp    = bEnable;  break;
                case MasterInputItem.CPS_Error_Reset_Req_Lamp:  Sensor_CPSErrorResetLamp    = bEnable;  break;
                case MasterInputItem.CPS_Error_Lamp:            Sensor_CPSErrorLamp         = bEnable;  break;

                case MasterInputItem.Inner_EMO_1:                   Sensor_DieBank_Inner_EMO_Status     = !bEnable;    break; //B접 처리
                case MasterInputItem.Inner_EMO_2:                   Sensor_DieBank_Inner_EMO_Status2    = !bEnable;    break; //B접 처리
                case MasterInputItem.Inner_EMO_3:                   Sensor_DieBank_Inner_EMO_Status3    = !bEnable;    break; //B접 처리
                case MasterInputItem.Inner_EMO_4:                   Sensor_DieBank_Inner_EMO_Status4    = !bEnable;    break; //B접 처리

                case MasterInputItem.Oxygen_Saturation_Warning:     Sensor_Oxygen_Saturation_Warning_Status = bEnable;  break;
            }
        }

        /// <summary>
        /// Master Output Initialize를 위해 필요
        /// </summary>
        /// <param name="eMasterOutputItem"></param>
        /// <param name="bEnable"></param>
        static public void WMX_IO_ItemToMapAction(MasterOutputItem eMasterOutputItem, bool bEnable)
        {
            switch (eMasterOutputItem)
            {
                case MasterOutputItem.Door_Bypass_Relay_On:
                    CMD_DoorOpen_Relay = bEnable;
                    break;
                case MasterOutputItem.Door_Open_Relay_On:
                    CMD_DoorBypass_Relay = bEnable;
                    break;
                case MasterOutputItem.Main_Reset_SW_Lamp:
                    break;
                case MasterOutputItem.Tower_Lamp_Red:
                    CMD_TowerLamp_LED_RED_REQ = bEnable;
                    break;
                case MasterOutputItem.Tower_Lamp_Yellow:
                    CMD_TowerLamp_LED_YELLOW_REQ = bEnable;
                    break;
                case MasterOutputItem.Tower_Lamp_Green:
                    CMD_TowerLamp_LED_GREEN_REQ = bEnable;
                    break;
                //case MasterOutputItem.Tower_Lamp_Blue:
                //    return null;
                case MasterOutputItem.Buzzer_MasterError:
                    CMD_Buzzer_MasterAlarm_REQ = bEnable;
                    break;
                case MasterOutputItem.Buzzer_PortError:
                    CMD_Buzzer_PortAlarm_REQ = bEnable;
                    break;
                case MasterOutputItem.Buzzer_RackMasterActive:
                    CMD_Buzzer_RackMasterActive_REQ = bEnable;
                    break;
                //case MasterOutputItem.Buzzer_Sound:
                //    return null;
                //case MasterOutputItem.CPS_Power_On:
                //    return General_Set_CPSPowerOnReq_FromCIM;
                //case MasterOutputItem.CPS_Error_Reset:
                //    return General_Set_CPSErrorReset_FromCIM;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Prog의 센서 변수 값을 리턴
        /// </summary>
        /// <param name="eMasterInputItem"></param>
        /// <returns></returns>
        static public object WMX_IO_ItemToMapData(MasterInputItem eMasterInputItem)
        {
            switch (eMasterInputItem)
            {
                case MasterInputItem.Main_MC_On:                return Sensor_MainMC;
                case MasterInputItem.RM_Power:                  return Sensor_RMPower;
                case MasterInputItem.MCUL_Fault:                return Sensor_MCULFault;

                case MasterInputItem.HP_Door_Open:              return Sensor_HPDoorOpen;
                case MasterInputItem.OP_Door_Open:              return Sensor_OPDoorOpen;
                case MasterInputItem.Maint_Door_Open:           return Sensor_MaintDoorOpen;
                case MasterInputItem.Maint_Door_Open2:          return Sensor_MaintDoorOpen2;

                case MasterInputItem.HP_AutoManual_Select_Key:  return Sensor_HPAutoKey;
                case MasterInputItem.OP_AutoManual_Select_Key:  return Sensor_OPAutoKey;

                case MasterInputItem.HP_Outside_EMO:            return Sensor_HP_Outside_EMO;
                case MasterInputItem.HP_Inside_EMO:             return Sensor_HP_InnerEscape_EMO;
                case MasterInputItem.OP_Outside_EMO:            return Sensor_OP_Outside_EMO;
                case MasterInputItem.OP_Inside_EMO:             return Sensor_OP_InnerEscape_EMO;

                case MasterInputItem.Port_DTP_EMO:              return Sensor_PortHandyTouchEMO;
                case MasterInputItem.Port_DTP_Dead_Man:         return Sensor_PortHandyTouchDeadMan;
                case MasterInputItem.Port_DTP_Mode1:            return Sensor_PortHandyTouchMode1;
                case MasterInputItem.Port_DTP_Mode2:            return Sensor_PortHandyTouchMode2;

                case MasterInputItem.HP_DTP_EMO:                return Sensor_HPHandyTouchEMO;
                case MasterInputItem.HP_DTP_Dead_Man:           return Sensor_HPHandyTouchDeadMan;
                case MasterInputItem.HP_DTP_Mode1:              return Sensor_HPHandyTouchMode1;
                case MasterInputItem.HP_DTP_Mode2:              return Sensor_HPHandyTouchMode2;

                case MasterInputItem.OP_DTP_EMO:                return Sensor_OPHandyTouchEMO;
                case MasterInputItem.OP_DTP_Dead_Man:           return Sensor_OPHandyTouchDeadMan;
                case MasterInputItem.OP_DTP_Mode1:              return Sensor_OPHandyTouchMode1;
                case MasterInputItem.OP_DTP_Mode2:              return Sensor_OPHandyTouchMode2;

                case MasterInputItem.CPS_Run:                   return Sensor_CPSRun;
                case MasterInputItem.CPS_Fault:                 return Sensor_CPSError;
                case MasterInputItem.CPS_Power_On_Enable_Lamp:  return Sensor_CPSPowerOnEnableLamp;
                case MasterInputItem.CPS_Power_On_Req_Lamp:     return Sensor_CPSPowerOnReqLamp;
                case MasterInputItem.CPS_Error_Reset_Req_Lamp:  return Sensor_CPSErrorResetLamp;
                case MasterInputItem.CPS_Error_Lamp:            return Sensor_CPSErrorLamp;

                case MasterInputItem.Inner_EMO_1:               return Sensor_DieBank_Inner_EMO_Status;
                case MasterInputItem.Inner_EMO_2:               return Sensor_DieBank_Inner_EMO_Status2;                
                case MasterInputItem.Inner_EMO_3:               return Sensor_DieBank_Inner_EMO_Status3;
                case MasterInputItem.Inner_EMO_4:               return Sensor_DieBank_Inner_EMO_Status4;

                case MasterInputItem.Oxygen_Saturation_Warning: return Sensor_Oxygen_Saturation_Warning_Status;
                default: return null;
            }
        }

        /// <summary>
        /// Prog의 출력 Flag 값을 리턴
        /// </summary>
        /// <param name="eMasterOutputItem"></param>
        /// <returns></returns>
        static public object WMX_IO_ItemToMapData(MasterOutputItem eMasterOutputItem)
        {
            switch (eMasterOutputItem)
            {
                case MasterOutputItem.Door_Bypass_Relay_On:
                    return CMD_DoorOpen_Relay;
                case MasterOutputItem.Door_Open_Relay_On:
                    return CMD_DoorBypass_Relay;
                case MasterOutputItem.Main_Reset_SW_Lamp:
                    return null;
                case MasterOutputItem.Tower_Lamp_Red:
                    return CMD_TowerLamp_LED_RED_REQ;
                case MasterOutputItem.Tower_Lamp_Yellow:
                    return CMD_TowerLamp_LED_YELLOW_REQ;
                case MasterOutputItem.Tower_Lamp_Green:
                    return CMD_TowerLamp_LED_GREEN_REQ;
                case MasterOutputItem.Buzzer_MasterError:
                    return CMD_Buzzer_MasterAlarm_REQ;
                case MasterOutputItem.Buzzer_PortError:
                    return CMD_Buzzer_PortAlarm_REQ;
                case MasterOutputItem.Buzzer_RackMasterActive:
                    return CMD_Buzzer_RackMasterActive_REQ;
                default:
                    return null;
            }
        }
    }
}
