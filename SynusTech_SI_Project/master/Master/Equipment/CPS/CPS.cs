using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.TCP;
using Master.Interface.Alarm;
using Master.ManagedFile;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Master.Equipment.CPS
{
    /// <summary>
    /// CPS.cs는 Equpment Network Setting에서 Server port를 설정하는 경우 자동 할당 됨.
    /// </summary>
    public partial class CPS
    {
        /// <summary>
        /// CPS 상태 Enum 정의 (전달 받은 File 기준으로 작성)
        /// </summary>
        public enum CPSStatus
        {
            Stop,
            Run,
            Fault,
            Warning,
            FailOver
        }
        public bool m_CPSEnable = false;                //CPS 사용 상태

        TCPServer m_CPSServer;                          //마스터에서 CPS는 Server로 운용되며 Server 객체를 생성하여 들어오는 연결 대기
        TCPSocket m_Socket;                             //Server 통해 연결 수락된 Client 정보
        byte[] m_PacketBuffer = new byte[409600];       //Server 운용과 함께 사용되는 버퍼
        int m_CurrentPacketLen = 0;                     //Packet Buffer에 저장된 현재 Packet 길이 추적

        public CPSStatus eCPSStatus = CPSStatus.Stop;   //CPS 상태(통신을 통해 업데이트)

        public int nRequestCount = 0;                   //CPS 데이터 요청 카운트
        public int nReceiveCount = 0;                   //CPS 데이터 수신 카운트

        EquipNetworkParam.CPSNetworkParam m_CPSParameter;   //Initialize 과정에서 전달 받는 CPS 파라미터(네트워크 주소, 패킷 사이즈 등)

        public CPS(EquipNetworkParam.CPSNetworkParam _CPSParameter)
        {
            //1. 전달 받은 CPS 파라미터 맵핑
            m_CPSParameter = _CPSParameter;

            //2. Server Port 유효 판단 및 사용 유무 설정
            if (_CPSParameter.ServerPort < 0 || _CPSParameter.ServerPort > 65535)
                m_CPSEnable = false;
            else
                m_CPSEnable = true;

            m_Socket = new TCPSocket();                             //소켓 생성
            m_Socket.ReceiveEvent += ReceiveData;                   //소켓 Packet Receive Event 연동

            m_CPSServer = new TCPServer(_CPSParameter.ServerPort);  //Server 생성
            m_CPSServer.AcceptEvent += Server_AcceptEvent;          //Server 연결 수락 이벤트 연동

            if(m_CPSEnable)
                m_CPSServer.Start();                                //CPS 사용 한다면 Server 시작
        }

        /// <summary>
        /// CPS 객체에 적용되어 있는 Parameter 정보를 리턴
        /// </summary>
        /// <returns></returns>
        public EquipNetworkParam.CPSNetworkParam GetParam()
        {
            return m_CPSParameter;
        }

        /// <summary>
        /// TCP Server에서 연결 허용시 호출되는 이벤트
        /// Server에서 연결 수락한 소켓 정보가 전달 됨(연결 시도한 Client의 정보)
        /// </summary>
        /// <param name="socket"></param>
        private void Server_AcceptEvent(Socket socket)
        {
            //1. 현재 사용중인 소켓이 있었다면
            if (m_Socket.GetSocket() != null)
            {
                //2. 연결 해제 로그 작성
                LogMsg.AddCPSLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPDisconnection, $"{m_Socket.GetSocket().RemoteEndPoint}");
                //3. 기존 소켓 종료 및 메모리 정리
                m_Socket.GetSocket().Close();
                m_Socket.GetSocket().Dispose();
            }
            //4. 신규 소켓으로 재 할당
            m_Socket.SetSocket(socket);
            ClearPacketBuffer();

            //5. 업데이트 관련 스레드 진행
            Thread LocalThread = new Thread(delegate ()
            {
                //7. 스레드는 별도 프로세스 영역으로 객체를 별도 저장 후 관리 진행(null Exception 관리)
                Socket Mysocket = socket;
                EndPoint RemoteEndPoint = Mysocket.RemoteEndPoint;
                nRequestCount = 0;
                nReceiveCount = 0;

                while (Mysocket != null &&
                        Mysocket.Connected)
                {
                    DataRequest();      //데이터 요청 전송
                    nRequestCount++;    //요청 카운트 증가
                    Thread.Sleep(500);
                }

                LogMsg.AddCPSLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPRequestProcessEnd, $"{RemoteEndPoint}");
            });
            LocalThread.Name = $"CPS {socket.RemoteEndPoint} Data Request Thread";
            LocalThread.IsBackground = true;
            LocalThread.Start();

            //6. 연결 성공 로그 작성
            LogMsg.AddCPSLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPConnection, $"{m_Socket.GetSocket().RemoteEndPoint}");
        }

        /// <summary>
        /// Server 종료 및 이벤트 연결 해제
        /// </summary>
        public void CloseCPS()
        {
            m_CPSServer.Close();
            m_CPSServer.AcceptEvent -= Server_AcceptEvent;
            m_Socket.Close();
            m_Socket.ReceiveEvent -= ReceiveData;
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
        /// CPS TCP/IP 연결 상태 리턴
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return m_Socket.IsConnected();
        }

        /// <summary>
        /// Socket에서 발생한 Packet 수신 이벤트 처리
        /// </summary>
        /// <param name="MsgNum"></param>
        /// <param name="bytes"></param>
        public void ReceiveData(int MsgNum, byte[] bytes)
        {
            try
            {
                try
                {
                    if (m_CurrentPacketLen + bytes.Length > m_PacketBuffer.Length)
                    {
                        LogMsg.AddCPSLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPPacketOverFlow, $"Buffer Length: {m_CurrentPacketLen} + Read Length: {bytes.Length} > {m_PacketBuffer.Length}");
                        ClearPacketBuffer();
                        return;
                    }
                    else
                    {
                        Array.Copy(bytes, 0, m_PacketBuffer, m_CurrentPacketLen, bytes.Length);
                        m_CurrentPacketLen += bytes.Length;
                    }
                }
                catch (Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"CPS->M Receive");
                }

                const byte PKT_STX = 0x02; //STX
                const byte PKT_ETX = 0x03; //ETX
                const int PacketHeaderLen = 98;
                int STXPos = -1;
                int ETXPos = -1;
                int index = 0;

                while (m_CurrentPacketLen >= PacketHeaderLen) //98개 이상이라면 While 루프 실행
                {
                    //0, 1, (2), [3, 4 ---- , 97, 98], (99)
                    if (m_PacketBuffer[index] == PKT_STX)
                        STXPos = index; //0, 1, 2
                    else if (m_PacketBuffer[index] == PKT_ETX)
                        ETXPos = index; //97, 98, 99

                    if (STXPos != -1 && ETXPos != -1)
                    {
                        int validPacketLength = ETXPos - STXPos + 1; //98

                        if(validPacketLength != PacketHeaderLen)
                        {
                            byte[] ErrorData = new byte[validPacketLength];
                            Array.Copy(m_PacketBuffer, STXPos, ErrorData, 0, validPacketLength);
                            LogMsg.AddCPSLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPSTXETXPosError, $"Recv STXPos: {STXPos}, ETXPos: {ETXPos}, Packet: {BitConverter.ToString(ErrorData)}");
                        }
                        else
                        {
                            byte[] validData = new byte[validPacketLength - 2]; //96
                            Array.Copy(m_PacketBuffer, STXPos + 1, validData, 0, validPacketLength - 2); //STX, ETX를 제외한 패킷
                            //2
                            byte CheckSumValue = validData[validData.Length - 1]; //95
                            int CheckSum = 0;
                            for (int nCount = 0; nCount < validData.Length - 1; nCount++)
                            {
                                CheckSum += validData[nCount];
                            }

                            byte[] receivePackets = new byte[PacketHeaderLen];
                            Array.Copy(m_PacketBuffer, STXPos, receivePackets, 0, receivePackets.Length);

                            if (CheckSumValue == (byte)(CheckSum & 0xff))
                            {
                                try
                                {
                                    ReceiveAcition(receivePackets);
                                    UpdateStatus();
                                }
                                catch(Exception ex) {
                                    LogMsg.AddExceptionLog(ex, $"CPS Packet Parsing Exception");
                                }
                                nReceiveCount++;
                            }
                            else
                            {
                                //Check SUM Error
                                LogMsg.AddCPSLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPCheckSUMError, $"Recv SUM: {CheckSumValue}, Cal SUM: {(byte)(CheckSum & 0xff)}, Packet: {BitConverter.ToString(receivePackets)}");
                            }
                        }
                        

                        m_CurrentPacketLen -= (ETXPos+1);

                        if (m_CurrentPacketLen > 0)
                            Array.Copy(m_PacketBuffer, ETXPos + 1, m_PacketBuffer, 0, m_CurrentPacketLen);
                        else
                            ClearPacketBuffer();

                        STXPos = -1;
                        ETXPos = -1;
                        index = 0;
                    }
                    else
                        index++;

                    if (STXPos == -1 && index >= m_CurrentPacketLen) //STX Pos를 찾지 못한 상태로 패킷이 끝나는 경우(쓰레기 패킷)
                        ClearPacketBuffer();
                    else if (STXPos != -1 && index >= m_CurrentPacketLen) //STX Pos는 찾았는데 ETX Pos가 없는 경우(이어 붙여야 함)
                        break;
                    else if (STXPos == -1 && ETXPos != -1) //STX Pos도 없고 ETX Pos도 없는 경우(쓰레기 패킷)
                    {
                        ClearPacketBuffer();
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"CPS Packet Processing Exception");
            }
            finally
            {

            }
        }

        /// <summary>
        /// CPS Data 요청 전송
        /// </summary>
        public void DataRequest()
        {
            try
            {
                ////EX : 02 52 30 31 32 33 30 36 31 35 31 35 31 35 30 31 11 03
                //byte[] TestArray = { 0x02, 0x52, 0x30, 0x31, 0x32, 0x33, 0x30, 0x36, 0x31, 0x35, 0x31, 0x35, 0x31, 0x35, 0x30, 0x31 };
                //byte[] SendExam = { 0x02, 0x52, 0x30, 0x31, 0x32, 0x33, 0x30, 0x36, 0x31, 0x35, 0x31, 0x35, 0x31, 0x35, 0x30, 0x31, 0x11, 0x03 };
                //SendData(SendExam);

                byte[] DataRequest = new byte[4];
                
                DataRequest[0] = 0x02;
                DataRequest[1] = (byte)'R';
                DataRequest[2] = (byte)'0';
                DataRequest[3] = (byte)'1';

                DateTime CurrentDt = DateTime.Now;
                string DateData  = CurrentDt.ToString("yyMMddhhmmss");
                byte[] DtArray = Encoding.UTF8.GetBytes(DateData);

                IEnumerable<byte> send = DataRequest.Concat(DtArray);

                byte[] AddPacket = send.ToArray();
                byte[] LastPacket = new byte[2];
                int CheckSum = 0;

                for (int nCount = 1; nCount < AddPacket.Length; nCount++)
                {
                    CheckSum += AddPacket[nCount];
                }


                LastPacket[0] = (byte)(CheckSum & 0xff);
                LastPacket[1] = 0x03;

                send = AddPacket.Concat(LastPacket);

                SendData(send.ToArray());
            }
            catch
            {

            }
        }

        /// <summary>
        /// CPS로 패킷 전송
        /// </summary>
        /// <param name="bytes"></param>
        public void SendData(byte[] bytes)
        {
            if (m_Socket.IsConnected())
                m_Socket.DataSend(bytes);
            else
                LogMsg.AddCPSLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotConnection, $"Send Array: {BitConverter.ToString(bytes)}");
        }

        /// <summary>
        /// CPS에서 패킷 수신 후 데이터 파싱 작업거쳐 업데이트 진행
        /// </summary>
        private void UpdateStatus()
        {
            UpdateMasterBitWord();
            AlarmCheck();
        }

        /// <summary>
        /// CPS의 Data Map에서 각 영역의 타입에 맞는 데이터 형 변환 진행
        /// </summary>
        private void UpdateMasterBitWord()
        {
            string Status = (string)Get_CPS_Data(PacketStruct.Status);

            if (Status == "000")
                eCPSStatus = CPSStatus.Stop;
            else if (Status == "001")
                eCPSStatus = CPSStatus.Run;
            else if (Status == "002")
                eCPSStatus = CPSStatus.Fault;
            else if (Status == "003")
                eCPSStatus = CPSStatus.Warning;
            else if (Status == "004")
                eCPSStatus = CPSStatus.FailOver;
            
            //Master.m_CIM.Set_Master_2_CIM_Bit_Data(CIM.CIM.SendBitMapIndex.CPS_RUN_Status, Master.Sensor_Is_CPS_Run_Status());
            //Master.m_CIM.Set_Master_2_CIM_Bit_Data(CIM.CIM.SendBitMapIndex.CPS_Error_Status, Master.Sensor_Is_CPS_Error_Status());


            Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_Vdc, (short)Get_CPS_Data(PacketStruct.Voltage));
            Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_DC_Current, (short)Get_CPS_Data(PacketStruct.DC_Current));
            Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_IGBT_Current, (short)Get_CPS_Data(PacketStruct.IGBT_Current));
            Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_Track_Current, (short)Get_CPS_Data(PacketStruct.Track_Current));
            Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_Output_Frequency1, (float)Get_CPS_Data(PacketStruct.OutputFreq));
            Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_HeatsinkTemp1, (float)Get_CPS_Data(PacketStruct.Converter_Heatsink_Temp));
            Master.m_CIM.Set_Master_2_CIM_Word_Data(CIM.CIM.SendWordMapIndex.CPS_InnerTemp1, (float)Get_CPS_Data(PacketStruct.Covnerter_Inner_Temp));
        }
    }
}
