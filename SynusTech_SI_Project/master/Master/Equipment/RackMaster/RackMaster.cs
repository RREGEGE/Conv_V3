using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.Interface.TCP;
using Master.ManagedFile;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Master.Interface.Alarm;
using Master.Interface.Safty;

namespace Master.Equipment.RackMaster
{
    /// <summary>
    /// RackMaster.cs는 이니셜라이징 및 주요 처리 부 작성
    /// </summary>
    public partial class RackMaster
    {
        /// <summary>
        /// STK Control Mode
        /// </summary>
        public enum ControlMode
        {
            MasterMode,
            CIMMode
        }

        /// <summary>
        /// STK 축 타입
        /// </summary>
        public enum AxisType
        {
            X_Axis,
            Z_Axis,
            A_Axis,
            T_Axis
        }

        TCPClient m_Client;                                                 //STK는 Server로 운용되며 Master에서 Client 객체를 생성하여 계속 연결 시도 
        byte[] m_PacketBuffer               = new byte[409600];             //Client 운용과 함께 사용되는 버퍼
        int m_CurrentPacketLen              = 0;                            //Packet Buffer에 저장된 현재 Packet 길이 추적
        EquipNetworkParam.RackMasterNetworkParam m_RackMasterParameter;     //Initialize 과정에서 전달 받는 STK 파라미터(ID, 네트워크 주소, 메모리맵 사이즈 등)
        public ControlMode m_eControlMode   = ControlMode.MasterMode;       //STK 운용 모드
        EStop mSW_EStop                     = new EStop();                  //STK Form에서 EStop 버튼 연동 변수

        /// <summary>
        /// STK 객체 이니셜라이즈
        /// Master 장비 이니셜 과정에서 진행 됨
        /// </summary>
        /// <param name="_RackMasterParameter"></param>
        public RackMaster(EquipNetworkParam.RackMasterNetworkParam _RackMasterParameter)
        {
            //1. 전달 받은 STK 파라미터 맵핑
            m_RackMasterParameter = _RackMasterParameter;

            //2. Memory Map 디폴트 값 Init
            InitializeDefaultMapData();

            //3. Word Alarm 초기화
            m_RackMasterWordAlarm = new WordAlarm[Enum.GetValues(typeof(RackMasterAlarmWordMap)).Length];
            for (int nCount = 0; nCount < m_RackMasterWordAlarm.Length; nCount++)
                m_RackMasterWordAlarm[nCount] = new WordAlarm();

            //4. Client 객체 생성 후 연결 시도
            m_Client = new TCPClient(m_RackMasterParameter.ServerIP, m_RackMasterParameter.ServerPort, $"RackMaster [{m_RackMasterParameter.ID} Client]");
            m_Client.ConnectEvent += ClientConnectEvent;    //연결 성공 이벤트
            m_Client.ReceiveEvent += ReceiveData;           //데이터 수신 이벤트
            m_Client.Connect();
        }

        /// <summary>
        /// STK Memory Map중 일부 Default 값으로 초기화
        /// STK 와 통신이 연결되면 STK 값 기반으로 동기 됨
        /// STK 와 통신 중이지 않은 경우 색상, 상태 표현을 위해 Default 값 적용
        /// </summary>
        private void InitializeDefaultMapData()
        {
            CMD_AutoModeRun_REQ     = false;
            CMD_AutoModeStop_REQ    = true;
            CMD_ServoOn_REQ         = false;
            CMD_ServoOff_REQ        = true;
        }

        /// <summary>
        /// STK 객체에 적용되어 있는 Parameter 정보를 리턴
        /// </summary>
        /// <returns></returns>
        public EquipNetworkParam.RackMasterNetworkParam GetParam()
        {
            return m_RackMasterParameter;
        }
        
        /// <summary>
        /// TCPClient에서 연결 성공/해제 시 호출되는 이벤트
        /// </summary>
        /// <param name="bConnect"></param>
        private void ClientConnectEvent(bool bConnect)
        {
            if (bConnect) //연결 성공한 경우
            {
                //1. 기존 패킷 버퍼 초기화
                ClearPacketBuffer();

                //2. STK에서 최초 필요한 Interlock Packet 정보 전달을 위해 전송 플래그 초기화
                InitPacket();

                //3. STK 정보 Update 관련 Thread 동작 (Master Interlock, Port PIO 중개, Alarm 처리)
                Thread LocalThread = new Thread(delegate ()
                {
                    while (IsConnected())
                    {
                        UpdateStatus();
                        Thread.Sleep(Master.StatusUpdateTime);
                    }
                });
                LocalThread.Name = $"RackMaster {GetParam().ID} Update Status";
                LocalThread.IsBackground = true;
                LocalThread.Start();

                //4. 연결 성공 로그 작성
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPConnection, $"Server IP:{m_Client.m_ServerIP} / Server Port:{m_Client.m_ServerPort}");

                //5. STK 상태 업데이트 명령 패킷 전송
                int nSTKCommunicationTime = 50; //msec
                byte[] CommTimeArray = BitConverter.GetBytes(nSTKCommunicationTime);

                //500msec
                //SendData(new byte[] { 0x0, 0x0, 0x0, 0x05, 0x64, 0x0, 0x0, 0x1, 0xf4 });

                //100msec
                //SendData(new byte[] { 0x0, 0x0, 0x0, 0x05, 0x64, 0x0, 0x0, 0x0, 0x64 });

                //50msec
                //SendData(new byte[] { 0x0, 0x0, 0x0, 0x05, 0x64, 0x0, 0x0, 0x0, 0x32 });

                SendData(new byte[] { 0x0, 0x0, 0x0, 0x05, 0x64, CommTimeArray[3], CommTimeArray[2], CommTimeArray[1], CommTimeArray[0] });

                //6. STK Watchdog 시작 (패킷 전달 못받는 시간이 길어지면 알람)
                Master.RMWatchDog.StartWatchdog();
            }
            else //연결 해제된 경우
            {
                //1. 알람 발생
                Master.AlarmInsert(Master.MasterAlarm.RM_TCP_Disconnection);
                //2. RM Watchdog 정지
                Master.RMWatchDog.StopWatchdog(true);
                //3. 연결 해제 로그 작성
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPDisconnection, $"Server IP:{m_Client.m_ServerIP} / Server Port:{m_Client.m_ServerPort}");
            }
        }

        /// <summary>
        /// STK 통신 연결 시 인터락 관련 메모리 맵 최초 1회 전송을 위한 플래그
        /// </summary>
        private void InitPacket()
        {
            m_bHPDoorOpenStateInit  = false;
            m_bOPDoorOpenStateInit  = false;
            m_bHPEMOPushInit        = false;
            m_bOPEMOPushInit        = false;
            m_bHPEscapeStateInit    = false;
            m_bOPEscapeStateInit    = false;
            m_bHPAutoKeyStateInit   = false;
            m_bHPHandyEMOStateInit  = false;
            m_bOPHandyEMOStateInit  = false;
        }

        /// <summary>
        /// Client 종료 및 이벤트 연결 해제
        /// </summary>
        public void CloseRackMaster()
        {
            if (m_Client != null)
            {
                m_Client.Close();
                m_Client.ReceiveEvent -= ReceiveData;
                m_Client.ConnectEvent -= ClientConnectEvent;
            }
        }
        
        /// <summary>
        /// Packet Buffer 정리(데이터 이상한 경우, 통신 시작하는 경우)
        /// </summary>
        private void ClearPacketBuffer()
        {
            m_CurrentPacketLen = 0;
            Array.Clear(m_PacketBuffer, 0, m_PacketBuffer.Length);
        }

        /// <summary>
        /// Client에서 발생한 Packet 수신 이벤트 처리
        /// </summary>
        /// <param name="MsgNum"></param>
        /// <param name="bytes"></param>
        public void ReceiveData(int MsgNum, byte[] bytes)
        {
            try
            {
                if (m_CurrentPacketLen + bytes.Length > m_PacketBuffer.Length) //패킷 버퍼를 넘어가는 경우
                {
                    //오버 플로우 메세지 및 패킷 정리
                    LogMsg.AddRackMasterLog(GetParam().ID,LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPPacketOverFlow, $"Buffer Length: {m_CurrentPacketLen} + Read Length: {bytes.Length} > {m_PacketBuffer.Length}");
                    ClearPacketBuffer();
                }
                else
                {
                    //패킷 버퍼에 누적 및 길이 증가
                    Array.Copy(bytes, 0, m_PacketBuffer, m_CurrentPacketLen, bytes.Length);
                    m_CurrentPacketLen += bytes.Length;
                }
            }
            catch(Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"RM[{GetParam().ID}]->M ReceiveData");
            }

            //Packet이 해석에 필요한 최소길이 이상인 경우 패킷 해석 진행 
            while (m_CurrentPacketLen >= ProtocolRoles.Recv_TCPHeaderLen)
            {
                //패킷 유효성 판단
                bool bValidPacket = ProtocolRoles.IsPacketValid(ref m_PacketBuffer, ref m_CurrentPacketLen, out ProtocolRoles.ErrorType eErrorType);

                if (bValidPacket)
                {
                    //패킷 유효한 경우 패킷에 대한 동작 처리
                    int value_DataLen = ProtocolRoles.GetValue_DataLen(m_PacketBuffer); //Packet의 Data Length 영역을 읽어 길이 정보 획득
                    int PacketLength = ProtocolRoles.Recv_DataLen + value_DataLen;      //Packet의 Len + 실제 데이터 길이 영역을 합쳐 실제 패킷 길이 획득

                    byte[] receivePackets = new byte[PacketLength];
                    Array.Copy(m_PacketBuffer, 0, receivePackets, 0, receivePackets.Length); //Buffer에서 실제 패킷 정보를 복사

                    ReceiveAcition(receivePackets); //동작 처리

                    //동작 후 처리된 패킷에 대해 버퍼 처리 및 버퍼 유효 패킷 길이 증감 처리
                    m_CurrentPacketLen -= (PacketLength);
                    if (m_CurrentPacketLen > 0)
                        Array.Copy(m_PacketBuffer, PacketLength, m_PacketBuffer, 0, m_CurrentPacketLen);
                    else
                        ClearPacketBuffer();
                }
                else
                {
                    //에러인 경우 로그 작성 및 특정 에러인 경우 패킷 정리
                    if (eErrorType != ProtocolRoles.ErrorType.ReadDataNotEnough ||
                       eErrorType != ProtocolRoles.ErrorType.None)
                    {
                        int value_DataLen = ProtocolRoles.GetValue_DataLen(m_PacketBuffer);
                        sbyte value_DataType = ProtocolRoles.GetValue_DataType(m_PacketBuffer);
                        short value_DataAddress = ProtocolRoles.GetValue_DataMapAddress(m_PacketBuffer);

                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPInvalidPacket, $"Error: {eErrorType} / Len: {value_DataLen}, Type: {value_DataType}, Addr: {value_DataAddress}, PLen: {m_CurrentPacketLen}, Packet: {BitConverter.ToString(bytes)}");
                        ClearPacketBuffer();
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// STK로 패킷 전송
        /// </summary>
        /// <param name="bytes"></param>
        public void SendData(byte[] bytes)
        {
            if(m_Client.IsConnected())
                m_Client.Send(bytes);
            else
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotConnection, $"Send Array: {BitConverter.ToString(bytes)}");
        }

        /// <summary>
        /// STK TCP/IP 연결 상태 리턴
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return m_Client?.IsConnected() ?? false;
        }

        /// <summary>
        /// STK 상태 업데이트 진행 (통신 연결 시 스레드 동작)
        /// </summary>
        private void UpdateStatus()
        {
            //1. Master의 인터락 정보를 STK 쪽으로 복사
            UpdateMasterInterlockStatus();

            //2. 공정 상황에 따라서 Port의 PIO Data를 STK Memory Map으로 복사 (STK의 Access ID가 일치하는 포트)
            UpdatePortAndRackMasterPIO();

            //3. STK의 Alarm 상태 업데이트
            UpdateAlarmStatus();

            //4. STK의 Maint Move 상태에 따른 ACK 업데이트
            if ((Status_MaintMoveRunAck || Status_MaintMove) && Get_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Maint_Run))
            {
                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Maint_Run, false);
                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Maint_Run);
            }
        }

        /// <summary>
        /// STK Busy 상태
        /// 전 축을 조합해서 확인
        /// 한 축이라도 동작 중이면 Busy
        /// </summary>
        /// <returns></returns>
        public bool IsBusy()
        {
            if (IsAxisBusy(AxisType.X_Axis) ||
                IsAxisBusy(AxisType.Z_Axis) ||
                IsAxisBusy(AxisType.A_Axis) ||
                IsAxisBusy(AxisType.T_Axis))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Master의 Interlock 정보를 STK 제어 변수에 적용(동기화)
        /// 값이 바뀌는 경우 자동 Send
        /// </summary>
        private void UpdateMasterInterlockStatus()
        {
            Status_CIM_To_STK_HPDoorOpen        = Master.Sensor_HPDoorOpen;
            Status_CIM_To_STK_OPDoorOpen        = Master.Sensor_OPDoorOpen;
            Status_CIM_To_STK_HP_EMO_Push       = Master.mHPOutSide_EStop.IsEStop();
            Status_CIM_To_STK_OP_EMO_Push       = Master.mOPOutSide_EStop.IsEStop();
            Status_CIM_To_STK_HP_Escape         = Master.mHPInnerEscape_EStop.IsEStop();
            Status_CIM_To_STK_OP_Escape         = Master.mOPInnerEscape_EStop.IsEStop();
            Status_CIM_To_STK_HPAutoKeyState    = Master.Sensor_HPAutoKey;
            Status_CIM_To_STK_HPHandyEMO        = Master.AlarmContains(Master.MasterAlarm.HP_Handy_Touch_E_Stop);
            Status_CIM_To_STK_OPHandyEMO        = Master.AlarmContains(Master.MasterAlarm.OP_Handy_Touch_E_Stop);
        }

        /// <summary>
        /// STK에서 Access 중인 Shelf가 Master에서 보유중인 Port의 ID와 일치하는 경우 중개 처리
        /// </summary>
        private void UpdatePortAndRackMasterPIO()
        {
            try
            {
                string AccessID = Convert.ToString(Status_STK_To_CIM_AccessID);
                if (Master.m_Ports.ContainsKey(AccessID)) //STK에서 Access 하려는 ID가 Master에서 보유중인 Port ID라면
                {
                    var port = Master.m_Ports[AccessID];

                    //STK PIO상태를 Port Memory Map에 적용
                    port.PIOStatus_STKToPort_TR_REQ     = PIOStatus_STK_TR_REQ;
                    port.PIOStatus_STKToPort_Busy       = PIOStatus_STK_Busy;
                    port.PIOStatus_STKToPort_Complete   = PIOStatus_STK_Complete;
                    port.PIOStatus_STKToPort_STKError   = PIOStatus_STK_Error;


                    //Port PIO 상태를 STK Memory Map에 적용
                    PIOStatus_Port_L_REQ                = port.PIOStatus_PortToSTK_Load_Req;
                    PIOStatus_Port_UL_REQ               = port.PIOStatus_PortToSTK_Unload_Req;
                    PIOStatus_Port_Ready                = port.PIOStatus_PortToSTK_Ready;
                    PIOStatus_Port_Error                = port.PIOStatus_PortToSTK_Error;
                }
                else //STK에서 Access 하려는 ID가 Master에 없는 경우
                {
                        //모든 Port PIO 초기화
                    foreach (var port in Master.m_Ports)
                    {
                        if (port.Value.GetParam().ePortType == Port.Port.PortType.EQ)
                        {
                            if (Status_AutoMode)
                            {
                                port.Value.PIOStatus_STKToPort_TR_REQ = false;
                                port.Value.PIOStatus_STKToPort_Busy = false;
                                port.Value.PIOStatus_STKToPort_Complete = false;
                                port.Value.PIOStatus_STKToPort_STKError = false;
                            }
                        }
                        else
                        {
                            port.Value.PIOStatus_STKToPort_TR_REQ = false;
                            port.Value.PIOStatus_STKToPort_Busy = false;
                            port.Value.PIOStatus_STKToPort_Complete = false;
                            port.Value.PIOStatus_STKToPort_STKError = false;
                        }
                    }

                    //STK Memory Map Clear
                    PIOStatus_Port_L_REQ = false;
                    PIOStatus_Port_UL_REQ = false;
                    PIOStatus_Port_Ready = false;
                    PIOStatus_Port_Error = false;
                }
            }
            catch
            {

            }
        }
        
        /// <summary>
        /// Auto Mode Enable 시 명령 수행
        /// </summary>
        private void CMD_AutoModeEnable()
        {
            CMD_AutoModeStop_REQ    = false;
            CMD_AutoModeRun_REQ     = true;
        }

        /// <summary>
        /// Auto Mode Disable 시 명령 수행
        /// </summary>
        private void CMD_AutoModeDisable()
        {
            CMD_AutoModeRun_REQ     = false;
            CMD_AutoModeStop_REQ    = true;
        }

        /// <summary>
        /// STK Control Mode 변경 시 동작 수행
        /// </summary>
        /// <param name="econtrolMode"></param>
        private void CMD_SetControlMode(ControlMode econtrolMode)
        {
            m_eControlMode = econtrolMode;
        }

        /// <summary>
        /// STK 제어 화면의 EMO 버튼 누름 시 동작 수행
        /// </summary>
        public void CMD_SetEMO()
        {
            mSW_EStop.PushEStop();
            CMD_EmergencyStop();
        }

        /// <summary>
        /// STK 제어 화면의 EMO 버튼 해제 시 동작 수행
        /// </summary>
        public void CMD_ReleaseEMO()
        {
            mSW_EStop.ReleaseEStop();
            CMD_EmergencyStop();
        }

        /// <summary>
        /// STK Emergency Stop 전송 관리
        /// 1. Master 에서 EStop 상황을 호출 한 경우
        /// 2. STK 제어 화면에서 EStop 버튼을 누른 경우
        /// </summary>
        public void CMD_EmergencyStop()
        {
            CMD_EmergencyStop_REQ = (Master.mRM_EStop.GetEStopState() == Interface.Safty.EStopState.EStop) || (mSW_EStop.GetEStopState() == Interface.Safty.EStopState.EStop);
        }

        /// <summary>
        /// STK Power On 시 명령 수행
        /// </summary>
        private void CMD_PowerOn()
        {
            CMD_ServoOff_REQ    = false;
            CMD_ServoOn_REQ     = true;
        }

        /// <summary>
        /// STK Power Off 시 명령 수행
        /// </summary>
        private void CMD_PowerOff()
        {
            CMD_ServoOn_REQ     = false;
            CMD_ServoOff_REQ    = true;
        }

        /// <summary>
        /// STK Alarm Clear 시 명령 수행
        /// </summary>
        private void CMD_AlarmClear()
        {
            CMD_ErrorReset_REQ  = true;
        }

        /// <summary>
        /// STK Axis Busy 상태 리턴
        /// </summary>
        public bool IsAxisBusy(AxisType eAxisType)
        {
            switch (eAxisType)
            {
                case AxisType.X_Axis:
                    return Status_X_Axis_Busy;
                case AxisType.Z_Axis:
                    return Status_Z_Axis_Busy;
                case AxisType.A_Axis:
                    return Status_A_Axis_Busy;
                case AxisType.T_Axis:
                    return Status_T_Axis_Busy;
                default:
                    return false;
            }
        }

        /// <summary>
        /// STK Axis Speed 비율 적용
        /// </summary>
        private void CMD_SetAxisSpeedRatio(AxisType eAxisType, short value)
        {
            if (eAxisType == AxisType.X_Axis)
                CMD_CIM_To_STK_X_Axis_SpeedRatio = value;
            else if (eAxisType == AxisType.Z_Axis)
                CMD_CIM_To_STK_Z_Axis_SpeedRatio = value;
            else if (eAxisType == AxisType.A_Axis)
                CMD_CIM_To_STK_A_Axis_SpeedRatio = value;
            else if (eAxisType == AxisType.T_Axis)
                CMD_CIM_To_STK_T_Axis_SpeedRatio = value;
        }

        /// <summary>
        /// STK Over load 비율 적용
        /// </summary>
        private void CMD_SetOverLoadValue(AxisType eAxisType, short value)
        {
            if (eAxisType == AxisType.X_Axis)
                CMD_CIM_To_STK_X_Axis_TorqueLimit = value;
            else if (eAxisType == AxisType.Z_Axis)
                CMD_CIM_To_STK_Z_Axis_TorqueLimit = value;
            else if (eAxisType == AxisType.A_Axis)
                CMD_CIM_To_STK_A_Axis_TorqueLimit = value;
            else if (eAxisType == AxisType.T_Axis)
                CMD_CIM_To_STK_T_Axis_TorqueLimit = value;
        }

        /// <summary>
        /// STK Over load 클리어 명령 수행
        /// </summary>
        private void CMD_SetOverLoadClear(AxisType eAxisType)
        {
            if (eAxisType == AxisType.X_Axis)
                CMD_X_Axis_MaxLoad_Clear = true;
            else if (eAxisType == AxisType.Z_Axis)
                CMD_Z_Axis_MaxLoad_Clear = true;
            else if (eAxisType == AxisType.A_Axis)
                CMD_A_Axis_MaxLoad_Clear = true;
            else if (eAxisType == AxisType.T_Axis)
                CMD_T_Axis_MaxLoad_Clear = true;
        }

        /// <summary>
        /// STK 시간 동기화 명령 수행
        /// Master PC의 시간 정보와 동기화 됨
        /// </summary>
        private void CMD_SetTimeSync()
        {
            DateTime CurrentDt = DateTime.Now;

            short Year = Convert.ToInt16(CurrentDt.Year);
            short Month = Convert.ToInt16(CurrentDt.Month);
            short Day = Convert.ToInt16(CurrentDt.Day);
            short Hour = Convert.ToInt16(CurrentDt.Hour);
            short Min = Convert.ToInt16(CurrentDt.Minute);
            short Sec = Convert.ToInt16(CurrentDt.Second);
            short DayOfWeek = (short)CurrentDt.DayOfWeek;

            Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Year, Year);
            Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Month, Month);
            Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Day, Day);
            Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Hour, Hour);
            Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Min, Min);
            Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Sec, Sec);
            Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Day_of_Week, DayOfWeek);
            Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.Year, 7); //Year 부터 7 Size Send

            Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_TimeSync, true);
            Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_TimeSync);
        }

        /// <summary>
        /// STK 메인트 무브 동작 명령 수행
        /// </summary>
        private void CMD_MaintMoveRun()
        {
            Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Maint_Run, true);
            Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_Maint_Run);
        }
    }
}
