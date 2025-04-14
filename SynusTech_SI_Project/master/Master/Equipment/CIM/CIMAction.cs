using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.TCP;
using System.Runtime;
using System.Threading;
using System.Collections;

namespace Master.Equipment.CIM
{
    /// <summary>
    /// CIMAction.cs는 CIM Memory Map 및 통신 관련 행동 작성
    /// CIM MemoryMap은 CIM <-> Master 간 메모리 맵
    /// </summary>
    public partial class CIM
    {
        /// <summary>
        /// CIM Memory Map 정의
        /// </summary>
        public enum ReceiveBitMapIndex
        {
            DoorOpen = 0x00,

            OnOffline_Status = 0x02,
            CSTFull,

            TowerLamp_LED_Green = 0x05,
            TowerLamp_LED_Yellow,
            TowerLamp_LED_Red,
            TowerLamp_BUZZER_MasterAlarm,
            TowerLamp_BUZZER_PortAlarm,
            TowerLamp_BUZZER_RackMasterActive,
            TowerLamp_BUZZER_MUTE,

            Master_EMO_Request = 0x0e,
            Master_Error_Reset_Request,

            FluLamp_ON = 0x10,
            FluLamp_OFF,
            FluLamp_SetTimer,
            
            CPS_Power_On_Enable = 0x21,


            CPS_Power_On_Request = 0x34,
            CPS_Error_Reset_Request = 0x37
        }
        public enum SendBitMapIndex
        {
            HP_DoorOpen = 0x00,
            HP_EMO_Pushing,
            HP_MasterKey_AutoMode_Status,
            //Reserve
            //Reserve
            HP_EMO_Escape_Status = 0x05,
            //Reserve
            OP_DoorOpen = 0x07,
            OP_EMO_Pushing,
            HP_Handy_Touch_EMO_Pushing,
            OP_Handy_Touch_EMO_Pushing,
            CIM_Mode_Status,
            OP_EMO_Escape_Status = 0x0c,
            RM_HP_EMO_Pushing,
            RM_OP_EMO_Pushing,

            CPS_RUN_Status = 0x18,
            CPS_Error_Status,

            CPS_Power_On_Enable_Lamp = 0x21,

            CPS_Power_On_Request_Lamp = 0x34,
            CPS_Error_Reset_Request_Lamp = 0x37,
            CPS_Error_Lamp = 0x3a,


            //FAN_Intake_Error = 0x50,
            //FAN_Exhaust_Error
        }
        public enum ReceiveWordMapIndex
        {
            FluLamp_OnTime = 0x2E,
            WatchDog = 0x30,
        }
        public enum SendWordMapIndex
        {
            Master_ErrorWord0 = 0x00,
            Master_ErrorWord1,
            Master_ErrorWord2,
            Master_ErrorWord3,
            Master_ErrorWord4,
            Master_ErrorWord5,
            Master_ErrorWord6,

            CPS_ErrorWord0 = 0x08,
            CPS_ErrorWord1,
            CPS_ErrorWord2,
            CPS_ErrorWord3,
            CPS_ErrorWord4,
            CPS_ErrorWord5,
            CPS_ErrorWord6,

            FluLamp_CurrentApplyTime = 0x2e,

            WatchDog = 0x30,

            CPS_Vdc = 0x32,
            CPS_DC_Current,
            CPS_IGBT_Current,
            CPS_Track_Current,
            CPS_Output_Frequency1,
            CPS_Output_Frequency2,
            CPS_HeatsinkTemp1,
            CPS_HeatsinkTemp2,
            CPS_InnerTemp1,
            CPS_InnerTemp2
        }

        object LocalThreadobj = null; //기존 패킷 전송 스레드 종료하기 위함

        /// <summary>
        /// Master에서 CIM 통신 패킷 수신 시 동작 행위
        /// </summary>
        /// <param name="receivePackets"></param>
        void ReceiveAcition(byte[] receivePackets)
        {
            sbyte DataType    = ProtocolRoles.GetValue_DataType(receivePackets);
            short DataMapAddress = ProtocolRoles.GetValue_DataMapAddress(receivePackets);

            switch ((ProtocolRoles.DataType)DataType)
            {
                case ProtocolRoles.DataType.STK_CIM_2_Controller_Master_Bit_Data:       //CIM -> Master Bit Map 패킷
                    Parsing_CIM_2_Master_Bit_Data(DataMapAddress, receivePackets);
                    break;
                case ProtocolRoles.DataType.STK_CIM_2_Controller_Master_Word_Data:      //CIM -> Master Word Map 패킷
                    Parsing_CIM_2_Master_Word_Data(DataMapAddress, receivePackets);
                    break;
                case ProtocolRoles.DataType.STK_CIM_2_Controller_RM_Bit_Data:           //CIM -> STK Bit Map 패킷
                    Parsing_CIM_2_RackMaster_Bit_Data(DataMapAddress, receivePackets);
                    break;
                case ProtocolRoles.DataType.STK_CIM_2_Controller_RM_Word_Data:          //CIM -> STK Word Map 패킷
                    Parsing_CIM_2_RackMaster_Word_Data(DataMapAddress, receivePackets);
                    break;
                case ProtocolRoles.DataType.STK_CIM_2_Controller_Port_Bit_Data:         //CIM -> Port Bit Map 패킷
                    Parsing_CIM_2_Port_Bit_Data(DataMapAddress, receivePackets);
                    break;
                case ProtocolRoles.DataType.STK_CIM_2_Controller_Port_Word_Data:        //CIM -> Port Word Map 패킷
                    Parsing_CIM_2_Port_Word_Data(DataMapAddress, receivePackets);
                    break;
                case ProtocolRoles.DataType.ControlMSG:                                 //Data 전송 요청 메세지(해당 메세지 수신시 CIM으로 패킷 반복 전달)
                    Parsing_CIM_2_Master_ControlMsg(DataMapAddress, receivePackets);
                    break;
                default:
                    return;
            }
        }


        /// <summary>
        /// CIM -> Master Bit Map Data 수신한 경우 파싱 행위
        /// </summary>
        /// <param name="DataMapAddress"></param>
        /// <param name="receivePackets"></param>
        void Parsing_CIM_2_Master_Bit_Data(short DataMapAddress, byte[] receivePackets)
        {
            //1. 메모리 맵 사이즈를 벗어나는 경우 리턴
            if (DataMapAddress >= Master.m_CIM_RecvBitMap.Length)
                return;

            //2. 수신 값, 현재 값 판단
            bool receive_value = Convert.ToBoolean(receivePackets[ProtocolRoles.Recv_TCPHeaderLen]);
            bool current_value = Convert.ToBoolean(Master.m_CIM_RecvBitMap[DataMapAddress]);

            //3. 수신 값, 현재 값 다른 경우 값 적용
            if (receive_value != current_value)
            {
                //수신 값이 DoorOpen이며 마스터 키가 Auto 상태인 경우 문 열림 인터락
                if ((ReceiveBitMapIndex)DataMapAddress == ReceiveBitMapIndex.DoorOpen &&
                    receive_value == true)
                {
                    foreach (var rackmaster in Master.m_RackMasters)
                    {
                        if (rackmaster.Value.Status_AutoMode)
                        {
                            LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.UnableOpenTheDoor, $"RackMaster[{rackmaster.Value.GetParam().ID}] Auto Running State.");
                            return;
                        }
                    }

                    if (Master.Sensor_HPAutoKey)
                    {
                        LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.UnableOpenTheDoor, $"HP Key Auto State.");
                        return;
                    }
                    else if (Master.Sensor_Oxygen_Saturation_Warning_Status)
                    {
                        LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.UnableOpenTheDoor, $"Oxygen saturation warning status inside door.");
                        return;
                    }
                    else
                        Master.m_CIM_RecvBitMap[DataMapAddress] = receive_value;
                }
                else
                    Master.m_CIM_RecvBitMap[DataMapAddress] = receive_value; //값 적용
            }

            //수신 값에 대한 동작
            Action_CIM_2_Master_Bit_Data((ReceiveBitMapIndex)DataMapAddress);
        }

        /// <summary>
        /// CIM -> Master Word Map Data 수신한 경우 파싱 행위
        /// </summary>
        /// <param name="DataMapAddress"></param>
        /// <param name="receivePackets"></param>
        void Parsing_CIM_2_Master_Word_Data(short DataMapAddress, byte[] receivePackets)
        {
            //1. 메모리 맵 사이즈를 벗어나는 경우 리턴
            if (DataMapAddress >= Master.m_CIM_RecvWordMap.Length)
                return;

            //2. 수신 값, 현재 값 판단
            byte[] receive_values = ProtocolRoles.GetReceiveDataArray(receivePackets);
            byte[] current_values = ProtocolRoles.GetCurrentWordDataToByteArray(DataMapAddress, receivePackets.Length - ProtocolRoles.Recv_TCPHeaderLen, Master.m_CIM_RecvWordMap);

            //3. 수신 값, 현재 값 다른 경우 값 적용
            if (!ProtocolRoles.IsByteArrayCompare(receive_values, current_values))
                Buffer.BlockCopy(receive_values, 0, Master.m_CIM_RecvWordMap, DataMapAddress * sizeof(short), receivePackets.Length - ProtocolRoles.Recv_TCPHeaderLen);

            //수신 값에 대한 동작
            Action_CIM_2_Master_Word_Data((ReceiveWordMapIndex)DataMapAddress);
        }

        /// <summary>
        /// CIM -> STK Bit Map Data 수신한 경우 파싱 행위
        /// </summary>
        /// <param name="DataMapAddress"></param>
        /// <param name="receivePackets"></param>
        void Parsing_CIM_2_RackMaster_Bit_Data(short DataMapAddress, byte[] receivePackets)
        {
            //1. 메모리 맵 사이즈를 벗어나는 경우 리턴
            if (DataMapAddress >= Master.m_RackMaster_RecvBitMap.Length)
                return;

            //2. 수신 값, 현재 값 판단
            bool receive_value = Convert.ToBoolean(receivePackets[ProtocolRoles.Recv_TCPHeaderLen]);
            bool current_value = Convert.ToBoolean(Master.m_RackMaster_RecvBitMap[DataMapAddress]);

            //3. 메모리 맵 주소에 해당하는 STK 객체 리턴
            var rackMaster = Master.ConvertMemoryAddressToRackMasterPt(Master.MapType.ReceiveBitMap, DataMapAddress);

            //4. 메모리 맵 주소에 해당하는 STK 객체 없는 경우 에러 로그
            if (rackMaster == null)
            {
                LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.NoneTargetRMFromReceivePacket, $"None Target RackMaster, Receive Data(Bit) : {BitConverter.ToString(receivePackets)}");
                return;
            }

            //5. STK CIM Mode 확인
            if (rackMaster.m_eControlMode == RackMaster.RackMaster.ControlMode.CIMMode)
            {
                //6. CIM Mode인 경우 값이 다른 경우 적용
                if (receive_value != current_value)
                    Master.m_RackMaster_RecvBitMap[DataMapAddress] = receive_value;

                //7. 수신 값에 대한 동작 (Auto Mode run 명령 제외하고는 바이패스 진행)
                rackMaster?.Action_CIM_2_RackMaster_Bit_Data((RackMaster.RackMaster.ReceiveBitMapIndex)DataMapAddress);
            }
            else
                LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.RackMasterIsNotCIMMode, $"RackMaster Is Not CIM Mode, Receive Data(Bit) : {BitConverter.ToString(receivePackets)}");
        }

        /// <summary>
        /// CIM -> STK Word Map Data 수신한 경우 파싱 행위
        /// </summary>
        /// <param name="DataMapAddress"></param>
        /// <param name="receivePackets"></param>
        void Parsing_CIM_2_RackMaster_Word_Data(short DataMapAddress, byte[] receivePackets)
        {
            //1. 메모리 맵 사이즈를 벗어나는 경우 리턴
            if (DataMapAddress >= Master.m_RackMaster_RecvWordMap.Length)
                return;

            //2. 수신 값, 현재 값 판단
            byte[] receive_values = ProtocolRoles.GetReceiveDataArray(receivePackets);
            byte[] current_values = ProtocolRoles.GetCurrentWordDataToByteArray(DataMapAddress, receivePackets.Length - ProtocolRoles.Recv_TCPHeaderLen, Master.m_RackMaster_RecvWordMap);

            //3. 메모리 맵 주소에 해당하는 STK 객체 리턴
            var rackMaster = Master.ConvertMemoryAddressToRackMasterPt(Master.MapType.ReceiveBitMap, DataMapAddress);

            if (rackMaster == null)
            {
                LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.NoneTargetRMFromReceivePacket, $"None Target RackMaster, Receive Data(Word) : {BitConverter.ToString(receivePackets)}");
                return;
            }

            //5. STK CIM Mode 확인
            if (rackMaster?.m_eControlMode == RackMaster.RackMaster.ControlMode.CIMMode)
            {
                //6. CIM Mode인 경우 값이 다른 경우 적용
                if (!ProtocolRoles.IsByteArrayCompare(receive_values, current_values))
                    Buffer.BlockCopy(receive_values, 0, Master.m_RackMaster_RecvWordMap, DataMapAddress * sizeof(short), receivePackets.Length - ProtocolRoles.Recv_TCPHeaderLen);

                //7. 수신 값에 대한 동작 (모든 패킷 바이패스 진행)
                rackMaster?.Action_CIM_2_RackMaster_Word_Data((RackMaster.RackMaster.ReceiveWordMapIndex)DataMapAddress, receivePackets);
            }
            else
                LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.RackMasterIsNotCIMMode, $"RackMaster Is Not CIM Mode, Receive Data(Word) : {BitConverter.ToString(receivePackets)}");
        }


        /// <summary>
        /// CIM -> Port Bit Map Data 수신한 경우 파싱 행위
        /// </summary>
        /// <param name="DataMapAddress"></param>
        /// <param name="receivePackets"></param>
        void Parsing_CIM_2_Port_Bit_Data(short DataMapAddress, byte[] receivePackets)
        {
            //1. 메모리 맵 사이즈를 벗어나는 경우 리턴
            if (DataMapAddress >= Master.m_Port_RecvBitMap.Length)
                return;

            //2. 수신 값, 현재 값 판단
            bool receive_value = Convert.ToBoolean(receivePackets[ProtocolRoles.Recv_TCPHeaderLen]);
            bool current_value = Convert.ToBoolean(Master.m_Port_RecvBitMap[DataMapAddress]);

            //3. 메모리 맵 주소에 해당하는 Port 객체 리턴
            var port = Master.ConvertMemoryAddressToPortPt(Master.MapType.ReceiveBitMap, DataMapAddress);

            //4. 메모리 맵 주소에 해당하는 Port 객체 없는 경우 에러 로그
            if (port == null)
            {
                LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.NoneTargetPortFromReceivePacket, $"None Target Port, Receive Data(Bit) : {BitConverter.ToString(receivePackets)}");
                return;
            }

            //5. Port CIM Mode 확인
            if (port?.m_eControlMode == Port.Port.ControlMode.CIMMode)
            {
                //6. CIM Mode인 경우 값이 다른 경우 적용
                if (receive_value != current_value)
                    Master.m_Port_RecvBitMap[DataMapAddress] = receive_value;

                //7. 수신 값에 대한 동작
                port.Action_CIM_2_Port_Bit_Data((Equipment.Port.Port.ReceiveBitMapIndex)DataMapAddress);
            }
            else
                LogMsg.AddPortLog(port.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PortIsNotCIMMode, $"Port Is Not CIM Mode, Receive Data(Bit) : {BitConverter.ToString(receivePackets)}");
        }
        void Parsing_CIM_2_Port_Word_Data(short DataMapAddress, byte[] receivePackets)
        {
            if (DataMapAddress >= Master.m_Port_RecvWordMap.Length)
                return;

            byte[] receive_values = ProtocolRoles.GetReceiveDataArray(receivePackets);
            byte[] current_values = ProtocolRoles.GetCurrentWordDataToByteArray(DataMapAddress, receivePackets.Length - ProtocolRoles.Recv_TCPHeaderLen, Master.m_Port_RecvWordMap);

            var port = Master.ConvertMemoryAddressToPortPt(Master.MapType.ReceiveBitMap, DataMapAddress);

            if (port == null)
            {
                LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.NoneTargetPortFromReceivePacket, $"None Target Port, Receive Data(Word) : {BitConverter.ToString(receivePackets)}");
                return;
            }

            if (port?.m_eControlMode == Port.Port.ControlMode.CIMMode)
            {
                if (!ProtocolRoles.IsByteArrayCompare(receive_values, current_values))
                    Buffer.BlockCopy(receive_values, 0, Master.m_Port_RecvWordMap, DataMapAddress * sizeof(short), receivePackets.Length - ProtocolRoles.Recv_TCPHeaderLen);

                port.Action_CIM_2_Port_Word_Data((Equipment.Port.Port.ReceiveWordMapIndex)DataMapAddress);
            }
            else
                LogMsg.AddPortLog(port.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PortIsNotCIMMode, $"Port Is Not CIM Mode, Receive Data(Word) : {BitConverter.ToString(receivePackets)}");
        }
        
        /// <summary>
        /// CIM -> Master Control Msg 수신 시 반복 전송 동작
        /// </summary>
        /// <param name="DataMapAddress"></param>
        /// <param name="receivePackets"></param>
        void Parsing_CIM_2_Master_ControlMsg(short DataMapAddress, byte[] receivePackets)
        {
            int DataByteLength = receivePackets.Length - ProtocolRoles.Recv_TCPHeaderLen;

            byte[] receive_values = new byte[DataByteLength];
            Buffer.BlockCopy(receivePackets, ProtocolRoles.Recv_TCPHeaderLen, receive_values, 0, DataByteLength);

            if (ProtocolRoles.Recv_LittleEndian)
                Array.Reverse(receive_values);

            short ControlTime = BitConverter.ToInt16(receive_values, 0);

            foreach (var rackmaster in Master.m_RackMasters)
            {
                rackmaster.Value.SendData(receivePackets);
            }

            if (LocalThreadobj != null)
            {
                Thread ThreadPt = (Thread)LocalThreadobj;

                if (ThreadPt.IsAlive)
                {
                    ThreadPt.Abort();
                    ThreadPt.Join();
                }
                LocalThreadobj = null;
            }

            Thread LocalThread = new Thread(delegate ()
            {
                LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.MemoryMapSendStart, $"Master Send Start, Send Time : {ControlTime} msec");

                while (IsConnected())
                {
                    SendAll_Master_2_CIM_Bit_Data();
                    SendAll_Master_2_CIM_Word_Data();
                    SendAll_RackMaster_2_CIM_Bit_Data();
                    SendAll_RackMaster_2_CIM_Word_Data();
                    SendAll_Port_2_CIM_Bit_Data();
                    SendAll_Port_2_CIM_Word_Data();
                    Thread.Sleep(ControlTime);
                }

                LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.MemoryMapSendStart, $"Master Send Start, Send End");
            });
            LocalThread.IsBackground = true;
            LocalThread.Start();

            LocalThreadobj = LocalThread;
        }


        /// <summary>
        /// CIM -> Master Bit, Word 맵 수신 시 동작 행위
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        void Action_CIM_2_Master_Bit_Data(ReceiveBitMapIndex eReceiveBitMap)
        {
            switch(eReceiveBitMap)
            {
                case ReceiveBitMapIndex.DoorOpen:
                    Master.CMD_DoorOpen_REQ = Get_CIM_2_Master_Bit_Data(eReceiveBitMap);
                    break;
                case ReceiveBitMapIndex.OnOffline_Status:
                    break;
                case ReceiveBitMapIndex.CSTFull:
                    break;
                case ReceiveBitMapIndex.TowerLamp_LED_Green:
                    //Master.CMD_TowerLamp_LED_GREEN_REQ = Get_CIM_2_Master_Bit_Data(eReceiveBitMap);
                    break;
                case ReceiveBitMapIndex.TowerLamp_LED_Yellow:
                    //Master.CMD_TowerLamp_LED_YELLOW_REQ = Get_CIM_2_Master_Bit_Data(eReceiveBitMap);
                    break;
                case ReceiveBitMapIndex.TowerLamp_LED_Red:
                    //Master.CMD_TowerLamp_LED_RED_REQ = Get_CIM_2_Master_Bit_Data(eReceiveBitMap);
                    break;
                case ReceiveBitMapIndex.TowerLamp_BUZZER_MasterAlarm:
                    //Master.CMD_Buzzer_MasterAlarm_REQ = Get_CIM_2_Master_Bit_Data(eReceiveBitMap);
                    break;
                case ReceiveBitMapIndex.TowerLamp_BUZZER_PortAlarm:
                    //Master.CMD_Buzzer_PortAlarm_REQ = Get_CIM_2_Master_Bit_Data(eReceiveBitMap);
                    break;
                case ReceiveBitMapIndex.TowerLamp_BUZZER_RackMasterActive:
                    //Master.CMD_Buzzer_RackMasterActive_REQ = Get_CIM_2_Master_Bit_Data(eReceiveBitMap);
                    break;
                case ReceiveBitMapIndex.TowerLamp_BUZZER_MUTE:
                    //Master.CMD_Buzzer_Mute_REQ = Get_CIM_2_Master_Bit_Data(eReceiveBitMap);
                    break;

                case ReceiveBitMapIndex.Master_EMO_Request:
                    Master.AlarmInsert(Master.MasterAlarm.CIM_E_Stop);
                    break;
                case ReceiveBitMapIndex.Master_Error_Reset_Request:
                    {
                        Master.Do_MasterRecovery(false);
                        Master.AlarmAllClear();

                        foreach (var rackMaster in Master.m_RackMasters)
                        {
                            if (rackMaster.Value.m_eControlMode == Equipment.RackMaster.RackMaster.ControlMode.CIMMode && rackMaster.Value.IsConnected())
                                rackMaster.Value.Interlock_SetAlarmClear(Equipment.RackMaster.RackMaster.InterlockFrom.TCPIP);
                        }

                        foreach (var port in Master.m_Ports)
                        {
                            if (port.Value.m_eControlMode == Equipment.Port.Port.ControlMode.CIMMode)
                            {
                                port.Value.Interlock_PortAmpAlarmClear(Equipment.Port.Port.InterlockFrom.TCPIP);
                                port.Value.Interlock_PortAlarmClear(Equipment.Port.Port.InterlockFrom.TCPIP);
                            }
                        }
                    }
                    break;

                case ReceiveBitMapIndex.FluLamp_ON:
                    break;
                case ReceiveBitMapIndex.FluLamp_OFF:
                    break;
                case ReceiveBitMapIndex.FluLamp_SetTimer:
                    break;

                case ReceiveBitMapIndex.CPS_Power_On_Enable:
                    break;
                case ReceiveBitMapIndex.CPS_Power_On_Request:
                    Master.CMD_CPS_Power_On_REQ = Get_CIM_2_Master_Bit_Data(eReceiveBitMap);
                    break;
                case ReceiveBitMapIndex.CPS_Error_Reset_Request:
                    Master.CMD_CPS_Error_Reset_REQ = Get_CIM_2_Master_Bit_Data(eReceiveBitMap);
                    break;

                default:
                    LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->M] BitMap Number: {(int)eReceiveBitMap}");
                    break;
            }
        }
        void Action_CIM_2_Master_Word_Data(ReceiveWordMapIndex eReceiveWordMapIndex)
        {
            switch (eReceiveWordMapIndex)
            {
                case ReceiveWordMapIndex.WatchDog:
                    break;
                default:
                    LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->M] WordMap Number: {(int)eReceiveWordMapIndex}");
                    break;
            }
        }


        /// <summary>
        /// CIM -> Master의 메모리 맵의 현재 데이터를 가져옴
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <returns></returns>
        public bool Get_CIM_2_Master_Bit_Data(ReceiveBitMapIndex eReceiveBitMap)
        {
            if ((int)eReceiveBitMap >= Master.m_CIM_RecvBitMap.Length || (int)eReceiveBitMap < 0)
            {
                LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange, 
                    $"[C->M] BitMap Index: {eReceiveBitMap} + Start: {GetParam().RecvBitMapStartAddr} >= Length: {Master.m_CIM_RecvBitMap.Length}");
                return false;
            }

            return Master.m_CIM_RecvBitMap[(int)eReceiveBitMap];
        }
        public short Get_CIM_2_Master_Word_Data(ReceiveWordMapIndex eReceiveWordMapIndex)
        {
            if ((int)eReceiveWordMapIndex >= Master.m_CIM_RecvWordMap.Length || (int)eReceiveWordMapIndex < 0)
            {
                LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[C->M] WordMap Index: {eReceiveWordMapIndex} + Start: {GetParam().RecvWordMapStartAddr} >= Length: {Master.m_CIM_RecvWordMap.Length}");
                return -1;
            }

            return Master.m_CIM_RecvWordMap[(int)eReceiveWordMapIndex];
        }
        
        /// <summary>
        /// Master -> CIM의 메모리 맵의 현재 데이터를 가져옴
        /// </summary>
        /// <param name="eSendBitMap"></param>
        /// <returns></returns>
        public bool Get_Master_2_CIM_Bit_Data(SendBitMapIndex eSendBitMap)
        {
            if ((int)eSendBitMap >= Master.m_CIM_SendBitMap.Length || (int)eSendBitMap < 0)
            {
                LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[M->C] BitMap Index: {eSendBitMap} + Start: {GetParam().SendBitMapStartAddr} >= Length: {Master.m_CIM_SendBitMap.Length}");
                return false;
            }

            return Master.m_CIM_SendBitMap[(int)eSendBitMap];
        }
        public short Get_Master_2_CIM_Word_Data(SendWordMapIndex eSendWordMapIndex)
        {
            if ((int)eSendWordMapIndex >= Master.m_CIM_SendWordMap.Length || (int)eSendWordMapIndex < 0)
            {
                LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[M->C] WordMap Index: {eSendWordMapIndex} + Start: {GetParam().SendWordMapStartAddr} >= Length: {Master.m_CIM_SendWordMap.Length}");
                return -1;
            }

            return Master.m_CIM_SendWordMap[(int)eSendWordMapIndex];
        }


        /// <summary>
        /// CIM -> Master의 Bit 메모리 맵에 데이터 작성 (Master Mode인 경우)
        /// </summary>
        /// <param name="eRecvBitMap"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set_CIM_2_Master_Bit_Data(ReceiveBitMapIndex eRecvBitMap, bool value)
        {
            if ((int)eRecvBitMap >= Master.m_CIM_RecvBitMap.Length || (int)eRecvBitMap < 0)
            {
                LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[C->M] BitMap Index: {eRecvBitMap} + Start: {GetParam().RecvBitMapStartAddr} >= Length: {Master.m_CIM_RecvBitMap.Length}");
                return false;
            }
            Master.m_CIM_RecvBitMap[(int)eRecvBitMap] = value;
            return true;
        }
        
        /// <summary>
        /// Master -> CIM의 Bit 메모리 맵에 데이터 작성
        /// </summary>
        /// <param name="eSendBitMap"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set_Master_2_CIM_Bit_Data(SendBitMapIndex eSendBitMap, bool value)
        {
            if ((int)eSendBitMap >= Master.m_CIM_SendBitMap.Length || (int)eSendBitMap < 0)
            {
                LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                    $"[M->C] BitMap Index: {eSendBitMap} + Start: {GetParam().SendBitMapStartAddr} >= Length: {Master.m_CIM_SendBitMap.Length}");
                return false;
            }
            Master.m_CIM_SendBitMap[(int)eSendBitMap] = value;
            return true;
        }

        /// <summary>
        /// Master -> CIM의 Word 메모리 맵에 데이터 작성
        /// </summary>
        /// <param name="eSendWordMapIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set_Master_2_CIM_Word_Data(SendWordMapIndex eSendWordMapIndex, object value)
        {
            try
            {
                if ((int)eSendWordMapIndex >= Master.m_CIM_SendWordMap.Length || (int)eSendWordMapIndex < 0)
                {
                    LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                        $"[M->C] WordMap Index: {eSendWordMapIndex} + Start: {GetParam().SendWordMapStartAddr} >= Length: {Master.m_CIM_SendWordMap.Length}");
                    return false;
                }

                switch (eSendWordMapIndex)
                {
                    case SendWordMapIndex.CPS_Output_Frequency1:
                    case SendWordMapIndex.CPS_InnerTemp1:
                    case SendWordMapIndex.CPS_HeatsinkTemp1:
                        {
                            byte[] DataArray = BitConverter.GetBytes((float)value);
                            Buffer.BlockCopy(DataArray, 0, Master.m_CIM_SendWordMap, ((int)eSendWordMapIndex + GetParam().SendWordMapStartAddr) * sizeof(short), DataArray.Length);
                        }
                        break;
                    default:
                        Master.m_CIM_SendWordMap[(int)eSendWordMapIndex + GetParam().SendWordMapStartAddr] = Convert.ToInt16(value);
                        break;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Master -> CIM Send (미사용, 상시 전송 ControlMsg 참고)
        /// </summary>
        /// <param name="eSendBitMapIndex"></param>
        public void Send_Master_2_CIM_Bit_Data(SendBitMapIndex eSendBitMapIndex)
        {
            try
            {
                if (!IsMapDefineIndex(eSendBitMapIndex))
                    return;

                int DataArrayLength = sizeof(byte);
                short BitMapOffset = (short)eSendBitMapIndex;
                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Master_Bit_Data };
                byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);

                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_CIM_SendBitMap, BitMapOffset, Data, 0, DataArrayLength);


                if (ProtocolRoles.Send_LittleEndian)
                {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < Data.Length / sizeof(byte); nCount++)
                    {
                        Array.Reverse(Data, nCount * sizeof(byte), sizeof(byte));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                SendData(send.ToArray());
            }
            catch
            {

            }
        }
        public void Send_Master_2_CIM_Bit_Data(SendBitMapIndex eSendBitMapIndex, int BitMapSize)
        {
            try
            {
                int DataArrayLength = sizeof(byte) * BitMapSize;
                short BitMapOffset = (short)eSendBitMapIndex;
                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Master_Bit_Data };
                byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);

                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_CIM_SendBitMap, BitMapOffset, Data, 0, DataArrayLength);


                if (ProtocolRoles.Send_LittleEndian)
                {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < Data.Length / sizeof(byte); nCount++)
                    {
                        Array.Reverse(Data, nCount * sizeof(byte), sizeof(byte));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                SendData(send.ToArray());
            }
            catch
            {

            }
        }
        public void Send_Master_2_CIM_Word_Data(SendWordMapIndex eSendWordMapIndex)
        {
            try
            {
                if (!IsMapDefineIndex(eSendWordMapIndex))
                    return;

                int DataArrayLength = sizeof(short);
                short WordMapOffset = (short)eSendWordMapIndex;

                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Master_Word_Data };
                byte[] DataOffset = BitConverter.GetBytes(WordMapOffset);

                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_CIM_SendWordMap, WordMapOffset * sizeof(short), Data, 0, DataArrayLength);


                if (ProtocolRoles.Send_LittleEndian)
                {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < Data.Length / sizeof(short); nCount++)
                    {
                        Array.Reverse(Data, nCount * sizeof(short), sizeof(short));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                SendData(send.ToArray());
            }
            catch
            {

            }
        }
        public void Send_Master_2_CIM_Word_Data(SendWordMapIndex eSendWordMapIndex, int WordMapSize)
        {
            try
            {
                int DataArrayLength = sizeof(short) * WordMapSize;
                short WordMapOffset = (short)eSendWordMapIndex;

                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Master_Word_Data };
                byte[] DataOffset = BitConverter.GetBytes(WordMapOffset);

                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_CIM_SendWordMap, WordMapOffset * sizeof(short), Data, 0, DataArrayLength);


                if (ProtocolRoles.Send_LittleEndian)
                {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < Data.Length / sizeof(short); nCount++)
                    {
                        Array.Reverse(Data, nCount * sizeof(short), sizeof(short));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                SendData(send.ToArray());
            }
            catch
            {

            }
        }

        /// <summary>
        /// Enum에 등록된 Index인지 체크 
        /// 개발자가 enum 매개 변수에 다른 타입으로 강제 할당 시 발생되는 오류 방지
        /// </summary>
        /// <param name="eReceiveBitMap"></param>
        /// <returns></returns>
        bool IsMapDefineIndex(ReceiveBitMapIndex eReceiveBitMap)
        {
            if (Enum.IsDefined(typeof(ReceiveBitMapIndex), eReceiveBitMap))
                return true;

            LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->M] BitMap Number: {(int)eReceiveBitMap}");
            return false;
        }
        bool IsMapDefineIndex(ReceiveWordMapIndex eReceiveWordMapIndex)
        {
            if (Enum.IsDefined(typeof(ReceiveWordMapIndex), eReceiveWordMapIndex))
                return true;

            LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[C->M] WordMap Number: {(int)eReceiveWordMapIndex}");
            return false;
        }
        bool IsMapDefineIndex(SendBitMapIndex eSendBitMapIndex)
        {
            if (Enum.IsDefined(typeof(SendBitMapIndex), eSendBitMapIndex))
                return true;

            LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[M->C] BitMap Number: {(int)eSendBitMapIndex}");
            return false;
        }
        bool IsMapDefineIndex(SendWordMapIndex eSendWordMapIndex)
        {
            if (Enum.IsDefined(typeof(SendWordMapIndex), eSendWordMapIndex))
                return true;

            LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotDefineMemoryMap, $"[M->C] WordMap Number: {(int)eSendWordMapIndex}");
            return false;
        }

        /// <summary>
        /// Master -> CIM Bit 메모리 맵 전체 전송
        /// </summary>
        void SendAll_Master_2_CIM_Bit_Data()
        {
            try
            {
                int DataArrayLength = Master.m_CIM?.GetParam()?.SendBitMapSize ?? -1;
                short BitMapOffset = 0;

                if (DataArrayLength <= -1 || BitMapOffset <= -1)
                    return;

                bool[] SendMap = new bool[DataArrayLength];

                for (int nCount = 0; nCount < DataArrayLength; nCount++)
                {
                    SendMap[nCount] = Master.m_CIM_SendBitMap[nCount];
                }

                BitArray bitarray = new BitArray(SendMap);
                byte[] DtArray = BitArrayToByteArray(bitarray);

                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DtArray.Length);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Master_Bit_Data };
                byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);

                if (ProtocolRoles.Send_LittleEndian)
                {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < DtArray.Length; nCount++)
                    {
                        Array.Reverse(DtArray, nCount * sizeof(byte), sizeof(byte));
                    }
                }
                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(DtArray);
                SendData(send.ToArray());
            }
            catch
            {

            }
        }
        
        /// <summary>
        /// Master -> CIM Word 메모리 맵 전체 전송
        /// </summary>
        void SendAll_Master_2_CIM_Word_Data()
        {
            try
            {
                int DataArrayLength = (Master.m_CIM?.GetParam()?.SendWordMapSize * sizeof(short)) ?? -1;
                short BitMapOffset = 0;

                if (DataArrayLength == -1)
                    return;

                //for (int nCount = 0; nCount < (Master.m_CIM?.GetParam()?.SendWordMapSize ?? 0); nCount++)
                //{
                //    Master.m_CIM_SendWordMap[BitMapOffset + nCount] = (short)(nCount % 2 == 0 ? 0x1234 : 0x6789);
                //}

                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Master_Word_Data };
                byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);
                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_CIM_SendWordMap, BitMapOffset, Data, 0, DataArrayLength);


                if (ProtocolRoles.Send_LittleEndian)
                {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < Master.m_CIM?.GetParam()?.SendWordMapSize; nCount++)
                    {
                        Array.Reverse(Data, nCount * sizeof(short), sizeof(short));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                SendData(send.ToArray());
            }
            catch
            {

            }
        }

        /// <summary>
        /// Master -> CIM, STK Bit 메모리 맵 전체 전송
        /// </summary>
        void SendAll_RackMaster_2_CIM_Bit_Data()
        {
            try
            {
                foreach (var rackMaster in Master.m_RackMasters)
                {
                    int DataArrayLength = rackMaster.Value?.GetParam()?.SendBitMapSize ?? -1;
                    short BitMapOffset = (short)(rackMaster.Value?.GetParam()?.SendBitMapStartAddr ?? -1);

                    if (DataArrayLength <= -1 || BitMapOffset <= -1)
                        return;

                    bool[] SendMap = new bool[DataArrayLength];

                    for(int nCount = 0; nCount < DataArrayLength; nCount++)
                    {
                        SendMap[nCount] = Master.m_RackMaster_SendBitMap[nCount + rackMaster.Value.GetParam().SendBitMapStartAddr];
                    }

                    BitArray bitarray = new BitArray(SendMap);
                    byte[] DtArray = BitArrayToByteArray(bitarray);

                    byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DtArray.Length);
                    byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_RM_Bit_Data };
                    byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);

                    if (ProtocolRoles.Send_LittleEndian)
                    {
                        Array.Reverse(DataLength);
                        Array.Reverse(DataOffset);

                        for (int nCount = 0; nCount < DtArray.Length; nCount++)
                        {
                            Array.Reverse(DtArray, nCount * sizeof(byte), sizeof(byte));
                        }
                    }

                    IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(DtArray);
                    SendData(send.ToArray());
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Master -> CIM, STK Word 메모리 맵 전체 전송
        /// </summary>
        void SendAll_RackMaster_2_CIM_Word_Data()
        {
            try
            {
                foreach (var rackMaster in Master.m_RackMasters)
                {
                    int DataArrayLength = (rackMaster.Value?.GetParam()?.SendWordMapSize * sizeof(short)) ?? -1;
                    short BitMapOffset = (short)rackMaster.Value?.GetParam().SendWordMapStartAddr;

                    if (DataArrayLength == -1)
                        return;

                    //for (int nCount = 0; nCount < (rackMaster.Value?.GetParam()?.SendWordMapSize ?? 0); nCount++)
                    //{
                    //    Master.m_RackMaster_SendWordMap[BitMapOffset + nCount] = (short)(nCount % 2 == 0 ? 0x1234 : 0x6789);
                    //}

                    byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                    byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_RM_Word_Data };
                    byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);
                    byte[] Data = new byte[DataArrayLength];
                    Buffer.BlockCopy(Master.m_RackMaster_SendWordMap, BitMapOffset, Data, 0, DataArrayLength);

                    if (ProtocolRoles.Send_LittleEndian)
                    {
                        Array.Reverse(DataLength);
                        Array.Reverse(DataOffset);

                        for (int nCount = 0; nCount < rackMaster.Value?.GetParam()?.SendWordMapSize; nCount++)
                        {
                            Array.Reverse(Data, nCount * sizeof(short), sizeof(short));
                        }
                    }

                    IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                    SendData(send.ToArray());
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Master -> CIM, Port Bit 메모리 맵 전체 전송
        /// </summary>
        void SendAll_Port_2_CIM_Bit_Data()
        {
            try
            {
                if (Master.m_Ports.Count == 0)
                    return;

                short BitMapOffset = (short)Master.m_Ports.ElementAt(0).Value.GetParam().SendBitMapSize;
                BitArray bitarray = new BitArray(Master.m_Port_SendBitMap);
                byte[] DtArray = BitArrayToByteArray(bitarray);
                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DtArray.Length);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Port_Bit_Data };
                byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);

                if (ProtocolRoles.Send_LittleEndian)
                {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < DtArray.Length; nCount++)
                    {
                        Array.Reverse(DtArray, nCount * sizeof(byte), sizeof(byte));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(DtArray);
                SendData(send.ToArray());

                //foreach (var port in Master.m_Ports)
                //{
                //    int DataArrayLength = port.Value?.GetParam()?.SendBitMapSize ?? -1;
                //    short BitMapOffset = (short)(port.Value?.GetParam()?.SendBitMapStartAddr ?? -1);

                //    if (DataArrayLength <= -1 || BitMapOffset <= -1)
                //        return;

                //    bool[] SendMap = new bool[DataArrayLength];

                //    for (int nCount = 0; nCount < DataArrayLength; nCount++)
                //    {
                //        SendMap[nCount] = Master.m_Port_SendBitMap[nCount + port.Value.GetParam().SendBitMapStartAddr];
                //    }

                //    BitArray bitarray = new BitArray(SendMap);
                //    byte[] DtArray = BitArrayToByteArray(bitarray);

                //    byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DtArray.Length);
                //    byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Port_Bit_Data };
                //    byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);

                //    if (ProtocolRoles.Send_LittleEndian)
                //    {
                //        Array.Reverse(DataLength);
                //        Array.Reverse(DataOffset);

                //        for (int nCount = 0; nCount < DtArray.Length; nCount++)
                //        {
                //            Array.Reverse(DtArray, nCount * sizeof(byte), sizeof(byte));
                //        }
                //    }

                //    IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(DtArray);
                //    SendData(send.ToArray());
                //}
            }
            catch
            {

            }
        }

        /// <summary>
        /// Master -> CIM, Port Word 메모리 맵 전체 전송
        /// </summary>
        void SendAll_Port_2_CIM_Word_Data()
        {
            try
            {
                if (Master.m_Ports.Count == 0)
                    return;

                short WordMapOffset = (short)Master.m_Ports.ElementAt(0).Value.GetParam().SendWordMapSize;
                int DataArrayLength = Master.m_Port_SendWordMap.Length * sizeof(short);
                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Port_Word_Data };
                byte[] DataOffset = BitConverter.GetBytes(WordMapOffset);
                byte[] Data = new byte[DataArrayLength];
                Buffer.BlockCopy(Master.m_Port_SendWordMap, 0, Data, 0, DataArrayLength);

                if (ProtocolRoles.Send_LittleEndian)
                {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < Master.m_Port_SendWordMap.Length; nCount++)
                    {
                        Array.Reverse(Data, nCount * sizeof(short), sizeof(short));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                SendData(send.ToArray());

                //foreach (var port in Master.m_Ports)
                //{
                //    int DataArrayLength = (port.Value?.GetParam()?.SendWordMapSize * sizeof(short)) ?? -1;
                //    short BitMapOffset = (short)port.Value?.GetParam().SendWordMapStartAddr;

                //    if (DataArrayLength == -1)
                //        return;

                //    //for (int nCount = 0; nCount < (port.Value?.GetParam()?.SendWordMapSize ?? 0); nCount++)
                //    //{
                //    //    Master.m_Port_SendWordMap[BitMapOffset + nCount] = (short)(nCount % 2 == 0 ? 0x1234 : 0x6789);
                //    //}

                //    byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + DataArrayLength);
                //    byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_Port_Word_Data };
                //    byte[] DataOffset = BitConverter.GetBytes(BitMapOffset);
                //    byte[] Data = new byte[DataArrayLength];
                //    Buffer.BlockCopy(Master.m_Port_SendWordMap, BitMapOffset, Data, 0, DataArrayLength);

                //    if (ProtocolRoles.Send_LittleEndian)
                //    {
                //        Array.Reverse(DataLength);
                //        Array.Reverse(DataOffset);

                //        for (int nCount = 0; nCount < port.Value?.GetParam()?.SendWordMapSize; nCount++)
                //        {
                //            Array.Reverse(Data, nCount * sizeof(short), sizeof(short));
                //        }
                //    }

                //    IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                //    SendData(send.ToArray());
                //}
            }
            catch
            {

            }
        }

        /// <summary>
        /// Bit 개수에 따른 바이트 배열 생성
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }
}
