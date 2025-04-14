using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using Master.ManagedFile;
using Master.Interface.TCP;
using System.Collections;
using System.Runtime;
using System.Net;

namespace Master.Equipment.Omron
{
    /// <summary>
    /// Omron.cs는 Port Type이 Diebank_Omron인 경우 자동 할당 됨.
    /// Diebank는 Master.Omron <-> OmronCommunicator.exe <-> Omron PLC 방식으로 소통 진행 됨
    /// Master.Omron <-(TCP/IP)-> OmronCommunicator.exe <-(Omron Library)-> Omron PLC
    /// </summary>
    public partial class Omron
    {
        public bool m_OmronEnable   = false;    //OMRON 사용 상태
        public bool m_OmronValid    = false;    //Omron으로 부터 얻어온 값의 유효 상태

        TCPServer m_OmronServer;                    //마스터에서 OMRON은 Server로 운용되며 Server 객체를 생성하여 들어오는 연결 대기
        TCPSocket m_Socket;                         //Server 통해 연결 수락된 Client 정보
        byte[] m_PacketBuffer = new byte[409600];   //Server 운용과 함께 사용되는 버퍼
        int m_CurrentPacketLen = 0;                 //Packet Buffer에 저장된 현재 Packet 길이 추적

        public Omron(bool bEnable)
        {
            m_OmronEnable = bEnable;                            //할당 되는 경우 사용 중 변수 활성화

            m_Socket = new TCPSocket();                         //소켓 생성
            m_Socket.ReceiveEvent += ReceiveData;               //소켓 Packet Receive Event 연동

            m_OmronServer = new TCPServer(60001);               //Server 생성(고정 포트)
            m_OmronServer.AcceptEvent += Server_AcceptEvent;    //Server 연결 수락 이벤트 연동

            if(m_OmronEnable)
                m_OmronServer.Start();                          //Omron 사용 한다면 Server 시작
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
                LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPDisconnection, $"Omron Communicator {m_Socket.GetSocket().RemoteEndPoint}");
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

                while (Mysocket != null &&
                        Mysocket.Connected)
                {
                    WriteTagMap(); //데이터 요청 전송
                    Thread.Sleep(100);
                }

                LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPRequestProcessEnd, $"Omron {RemoteEndPoint}");
            });
            LocalThread.Name = $"Omron {socket.RemoteEndPoint} Tag Write Thread";
            LocalThread.IsBackground = true;
            LocalThread.Start();

            //6. 연결 성공 로그 작성
            LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPConnection, $"Omron Communicator {m_Socket.GetSocket().RemoteEndPoint}");
        }

        /// <summary>
        /// Server 종료 및 이벤트 연결 해제
        /// </summary>
        public void CloseOmron()
        {
            m_OmronServer.Close();
            m_OmronServer.AcceptEvent -= Server_AcceptEvent;
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
        /// OMRON TCP/IP 연결 상태 리턴
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
                        LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPPacketOverFlow, $"Omron Buffer Length: {m_CurrentPacketLen} + Read Length: {bytes.Length} > {m_PacketBuffer.Length}");
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
                    LogMsg.AddExceptionLog(ex, $"Omron->M Receive");
                }


                // 
                // StartByte(1) + DataType(1) + State(1) + DataArray(32 or 512) + EndByte(1)
                // Data Type -> 5 : Bit Map, 6 : Word Map
                // State -> 0 : Error, 1 : Valid
                //BitMap Size = 64 * 4 / 8
                //WordMap Size = 64 * 2 * 4

                const int PacketHeaderLen = 36; //최소 수량
                int nPos = 0;
                while (m_CurrentPacketLen >= PacketHeaderLen)
                {
                    if(m_PacketBuffer[nPos] == 0x02)
                    {
                        int StartPos = nPos;

                        if (StartPos + 1 < m_CurrentPacketLen)
                        {
                            if (m_PacketBuffer[StartPos + 1] == 0x05)
                            {
                                //BitMap
                                if (StartPos + 35 < m_CurrentPacketLen)
                                {
                                    if(m_PacketBuffer[StartPos + 35] == 0x03)
                                    {
                                        if (m_PacketBuffer[StartPos + 2] == 0x01)
                                        {
                                            //Write
                                            m_OmronValid = true;
                                            byte[] OnlyDataPackets = new byte[32];
                                            Array.Copy(m_PacketBuffer, StartPos + 3, OnlyDataPackets, 0, OnlyDataPackets.Length);
                                            BitArray ba = new BitArray(OnlyDataPackets);

                                            if(Master.m_ReadOmronBitMap.Length != ba.Length)
                                                break;

                                            for (int nCount =0; nCount < ba.Length; nCount++)
                                            {
                                                Master.m_ReadOmronBitMap[nCount] = ba[nCount];
                                            }
                                        }
                                        else
                                            m_OmronValid = false;

                                        m_CurrentPacketLen -= (StartPos + 35);
                                        if (m_CurrentPacketLen > 0)
                                            Array.Copy(m_PacketBuffer, StartPos + 35, m_PacketBuffer, 0, m_CurrentPacketLen);
                                        else
                                            ClearPacketBuffer();
                                    }
                                    else
                                    {
                                        m_CurrentPacketLen -= (StartPos + 35);
                                        if (m_CurrentPacketLen > 0)
                                            Array.Copy(m_PacketBuffer, StartPos + 35, m_PacketBuffer, 0, m_CurrentPacketLen);
                                        else
                                            ClearPacketBuffer();
                                    }
                                }
                                else
                                    break;

                            }
                            else if (m_PacketBuffer[StartPos + 1] == 0x06)
                            {
                                //WordMap
                                if (StartPos + 515 < m_CurrentPacketLen)
                                {
                                    if (m_PacketBuffer[StartPos + 515] == 0x03)
                                    {
                                        if (m_PacketBuffer[StartPos + 2] == 0x01)
                                        {
                                            //Write
                                            byte[] OnlyDataPackets = new byte[4 * 64 * 2];
                                            Array.Copy(m_PacketBuffer, StartPos + 3, OnlyDataPackets, 0, OnlyDataPackets.Length);

                                            if (Master.m_ReadOmronWordMap.Length * 2 != OnlyDataPackets.Length)
                                                break;

                                            Buffer.BlockCopy(OnlyDataPackets, 0, Master.m_ReadOmronWordMap, 0, Master.m_ReadOmronWordMap.Length * 2);
                                        }
                                        else
                                            m_OmronValid = false;

                                        m_CurrentPacketLen -= (StartPos + 515);
                                        if (m_CurrentPacketLen > 0)
                                            Array.Copy(m_PacketBuffer, StartPos + 515, m_PacketBuffer, 0, m_CurrentPacketLen);
                                        else
                                            ClearPacketBuffer();
                                    }
                                    else
                                    {
                                        m_CurrentPacketLen -= (StartPos + 35);
                                        if (m_CurrentPacketLen > 0)
                                            Array.Copy(m_PacketBuffer, StartPos + 35, m_PacketBuffer, 0, m_CurrentPacketLen);
                                        else
                                            ClearPacketBuffer();
                                    }
                                }
                                else
                                    break;
                            }
                            else
                                nPos++;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if(nPos + 1 < m_CurrentPacketLen)
                            nPos++;
                    }
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        /// <summary>
        /// Tag Map 정보 요청
        /// </summary>
        public void WriteTagMap()
        {
            WriteBitMap();
            WriteWordMap();
        }

        /// <summary>
        /// Tag Map 중 Bit Map 정보 요청(PIO)
        /// </summary>
        public void WriteBitMap()
        {
            byte[] HeaderByte = new byte[] { 0x02, 0x05, 0x01 };
            BitArray ba = new BitArray(Master.m_WriteOmronBitMap);
            byte[] DataByte = new byte[32];
            ba.CopyTo(DataByte, 0);
            byte[] EndByte = new byte[] { 0x03 };


            IEnumerable<byte> send = HeaderByte.Concat(DataByte).Concat(EndByte);
            SendData(send.ToArray());
        }

        /// <summary>
        /// Tag Map 중 Word Map 정보 요청(CST ID)
        /// </summary>
        public void WriteWordMap()
        {
            byte[] HeaderByte = new byte[] { 0x02, 0x06, 0x01 };
            byte[] DataByte = new byte[64 * 4 * 2];
            Buffer.BlockCopy(Master.m_WriteOmronWordMap, 0, DataByte, 0, DataByte.Length);
            byte[] EndByte = new byte[] { 0x03 };


            IEnumerable<byte> send = HeaderByte.Concat(DataByte).Concat(EndByte);
            SendData(send.ToArray());
        }

        /// <summary>
        /// Omron Communicator로 패킷 전송
        /// </summary>
        /// <param name="bytes"></param>
        public void SendData(byte[] bytes)
        {
            if (m_Socket.IsConnected())
                m_Socket.DataSend(bytes);
            else
                LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotConnection, $"Send Array: {BitConverter.ToString(bytes)}");
        }
    }
}
