using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Master.Interface.TCP
{
    /// <summary>
    /// TCP Server 운용 시 연결 허용된 Socket 정보를 저장, 통신 운용하기 위한 클래스
    /// </summary>
    class TCPSocket
    {
        public delegate void ReceiveHandler(int MsgNum, byte[] bytes);
        public event ReceiveHandler ReceiveEvent;

        Socket m_Socket;
        byte[] m_ReceiveBuffer;
        int m_ReceiveBufferSize;
        int m_MsgNum;

        public TCPSocket()
        {
            m_ReceiveBufferSize = 409600;
            m_ReceiveBuffer = new byte[m_ReceiveBufferSize];
        }

        /// <summary>
        /// Socket 닫기, 서버 운용 시 새로 연결 요청이 들어온 경우 기존 것을 닫고 새로 할당
        /// </summary>
        public void Close()
        {
            if (m_Socket != null)
            {
                m_Socket.Close();
                m_Socket.Dispose();
            }
        }

        /// <summary>
        /// Socket 연결 상태
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return m_Socket?.Connected ?? false;
        }

        /// <summary>
        /// Server에 Socket 연결이 새로 들어온 경우 기존 것을 Close, Dispose 후 재 할당
        /// </summary>
        /// <param name="socket"></param>
        public void SetSocket(Socket socket)
        {
            m_Socket = new Socket(SocketType.Stream, ProtocolType.Tcp); 
            m_Socket = socket;                                                                              //신규 소켓 할당
            m_MsgNum = 0;
            m_Socket.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, 0, DataReceived, m_Socket);   //Data Receive 대기 상태 진입
        }

        /// <summary>
        /// 현재 할당된 소켓 정보를 얻음
        /// </summary>
        /// <returns></returns>
        public Socket GetSocket()
        {
            return m_Socket;
        }

        /// <summary>
        /// Packet Buffer 초기화
        /// </summary>
        private void ReceiveBufferClear()
        {
            m_ReceiveBuffer = new byte[m_ReceiveBufferSize];
        }

        /// <summary>
        /// 소켓의 Data Receive Event
        /// </summary>
        /// <param name="ar"></param>
        void DataReceived(IAsyncResult ar)
        {
            try
            {
                Socket Sock = (Socket)ar.AsyncState;
                if (Sock.Connected) //소켓이 연결 상태인 경우
                {
                    int received = m_Socket.EndReceive(ar); //Receive 된 값의 길이를 얻어옴
                    byte[] buffer = new byte[received];
                    Array.Copy(m_ReceiveBuffer, 0, buffer, 0, received); //Receive Buffer에서 Receive된 길이 만큼 생성 및 전달

                    ReceiveEvent(++m_MsgNum, buffer); //Receive Packet 전달 이벤트

                    ReceiveBufferClear(); //Receive Packet 전달 후 기존 버퍼 클리어
                    m_Socket.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, 0, DataReceived, m_Socket); //다시 Receive 대기 상태 진입
                }
            }
            catch
            {
                //끊어짐
                //Log
            }
        }

        /// <summary>
        /// Socket Data 전송 함수
        /// </summary>
        /// <param name="bytes"></param>
        public void DataSend(byte[] bytes)
        {
            try
            {
                if (m_Socket != null && m_Socket.Connected)
                {
                    m_Socket.Send(bytes);
                }
            }
            catch
            {
                //끊어짐
                //Log
            }
        }
    }
}
