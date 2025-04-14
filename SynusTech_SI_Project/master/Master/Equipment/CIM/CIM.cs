using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.TCP;
using Master.ManagedFile;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Master.Equipment.CIM
{
    /// <summary>
    /// CIM.cs는 Equpment Network Setting에서 Port를 설정하며 Port 번호 유무와 상관 없이 기본 할당되는 객체
    /// </summary>
    public partial class CIM
    {
        TCPServer m_CIMServer;                              //마스터에서 CIM은 Server로 운용되며 Server 객체를 생성하여 들어오는 연결 대기
        TCPSocket m_Socket;                                 //Server 통해 연결 수락된 Client 정보
        byte[]  m_PacketBuffer      = new byte[409600];     //Server 운용과 함께 사용되는 버퍼
        int     m_CurrentPacketLen  = 0;                    //Packet Buffer에 저장된 현재 Packet 길이 추적

        EquipNetworkParam.CIMNetworkParam m_CIMParameter;   //Initialize 과정에서 전달 받는 CIM 파라미터(네트워크 주소, 패킷 사이즈 등)

        public CIM(EquipNetworkParam.CIMNetworkParam _CIMParameter)
        {
            //1. 전달 받은 CIM 파라미터 맵핑
            m_CIMParameter = _CIMParameter;

            m_Socket = new TCPSocket();                                 //소켓 생성
            m_Socket.ReceiveEvent += ReceiveData;                       //소켓 Packet Receive Event 연동

            m_CIMServer = new TCPServer(m_CIMParameter.ServerPort);     //Server 생성
            m_CIMServer.AcceptEvent += Server_AcceptEvent;              //Server 연결 수락 이벤트 연동
            m_CIMServer.Start();                                        //Server 시작
        }

        /// <summary>
        /// CIM 객체에 적용되어 있는 Parameter 정보를 리턴
        /// </summary>
        /// <returns></returns>
        public EquipNetworkParam.CIMNetworkParam GetParam()
        {
            return m_CIMParameter;
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
                LogMsg.AddCIMLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPDisconnection, $"{m_Socket.GetSocket().RemoteEndPoint}");
                //3. 기존 소켓 종료 및 메모리 정리
                m_Socket.GetSocket().Close();
                m_Socket.GetSocket().Dispose();
            }
            //4. 신규 소켓으로 재 할당
            m_Socket.SetSocket(socket);
            ClearPacketBuffer();

            //5. 연결 성공 로그 작성
            LogMsg.AddCIMLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPConnection, $"{m_Socket.GetSocket().RemoteEndPoint}");
        }

        /// <summary>
        /// Server 종료 및 이벤트 연결 해제
        /// </summary>
        public void CloseCIM()
        {
            m_CIMServer.Close();
            m_CIMServer.AcceptEvent -= Server_AcceptEvent;
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
        /// CIM TCP/IP 연결 상태 리턴
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
                if (m_CurrentPacketLen + bytes.Length > m_PacketBuffer.Length)
                {
                    LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPPacketOverFlow, $"Buffer Length: {m_CurrentPacketLen} + Read Length: {bytes.Length} > {m_PacketBuffer.Length}");
                    ClearPacketBuffer();
                }
                else
                {
                    Array.Copy(bytes, 0, m_PacketBuffer, m_CurrentPacketLen, bytes.Length);
                    m_CurrentPacketLen += bytes.Length;
                }
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"[C->M] ReceiveData");
            }

            while (m_CurrentPacketLen >= ProtocolRoles.Recv_TCPHeaderLen)
            {
                bool bValidPacket = ProtocolRoles.IsPacketValid(ref m_PacketBuffer, ref m_CurrentPacketLen, out ProtocolRoles.ErrorType eErrorType);

                if (bValidPacket)
                {
                    int value_DataLen = ProtocolRoles.GetValue_DataLen(m_PacketBuffer);
                    int PacketLength = ProtocolRoles.Recv_DataLen + value_DataLen;

                    byte[] receivePackets = new byte[PacketLength];
                    Array.Copy(m_PacketBuffer, 0, receivePackets, 0, receivePackets.Length);

                    ReceiveAcition(receivePackets);

                    m_CurrentPacketLen -= (PacketLength);
                    if (m_CurrentPacketLen > 0)
                        Array.Copy(m_PacketBuffer, PacketLength, m_PacketBuffer, 0, m_CurrentPacketLen);
                    else
                        ClearPacketBuffer();
                }
                else
                {
                    if(eErrorType != ProtocolRoles.ErrorType.ReadDataNotEnough ||
                       eErrorType != ProtocolRoles.ErrorType.None)
                    {
                        int value_DataLen = ProtocolRoles.GetValue_DataLen(m_PacketBuffer);
                        sbyte value_DataType = ProtocolRoles.GetValue_DataType(m_PacketBuffer);
                        short value_DataAddress = ProtocolRoles.GetValue_DataMapAddress(m_PacketBuffer);

                        LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPInvalidPacket, $"Error: {eErrorType} / Len: {value_DataLen}, Type: {value_DataType}, Addr: {value_DataAddress}, PLen: {m_CurrentPacketLen}, Packet: {BitConverter.ToString(bytes)}");
                        ClearPacketBuffer();
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// CIM으로 패킷 전송
        /// </summary>
        /// <param name="bytes"></param>
        public void SendData(byte[] bytes)
        {
            if (m_Socket.IsConnected())
                m_Socket.DataSend(bytes);
            else
                LogMsg.AddCIMLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotConnection, $"Send Array: {BitConverter.ToString(bytes)}");
        }
    }
}
