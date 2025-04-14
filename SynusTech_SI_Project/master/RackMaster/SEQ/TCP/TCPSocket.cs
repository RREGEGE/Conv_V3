using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace RackMaster.SEQ.TCP {
    class TCPSocket {
        public delegate void ReceiveHandler(int MsgNum, byte[] bytes);
        public event ReceiveHandler ReceiveEvent;

        Socket m_Socket;
        byte[] m_ReceiveBuffer;
        int m_ReceiveBufferSize;
        int m_MsgNum;

        public TCPSocket(int _receiveBufferSize, int _sendBufferSize) {
            m_ReceiveBufferSize = _receiveBufferSize;
            m_ReceiveBuffer = new byte[m_ReceiveBufferSize];
        }
        public TCPSocket() {
            m_ReceiveBufferSize = 409600;
            m_ReceiveBuffer = new byte[m_ReceiveBufferSize];
        }
        /// <summary>
        /// Socket 닫기, 서버 운용 시 새로 연결 요청이 들어온 경우 기존 것을 닫고 새로 할당
        /// </summary>
        public void Close() {
            if (m_Socket != null) {
                m_Socket.Close();
                m_Socket.Dispose();
            }
        }
        /// <summary>
        /// Socket 연결 상태
        /// </summary>
        /// <returns></returns>
        public bool IsConnected() {
            return m_Socket?.Connected ?? false;
        }
        /// <summary>
        /// Server에 Socket 연결이 새로 들어온 경우 기존 것을 Close, Dispose 후 재 할당
        /// </summary>
        /// <param name="socket"></param>
        public void SetSocket(Socket socket) {
            m_Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            m_Socket = socket;
            m_MsgNum = 0;
            m_Socket.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, 0, DataReceived, m_Socket);
        }
        /// <summary>
        /// 현재 할당된 소켓 정보를 얻음
        /// </summary>
        /// <returns></returns>
        public Socket GetSocket() {
            return m_Socket;
        }
        /// <summary>
        /// Packet Buffer 초기화
        /// </summary>
        private void ReceiveBufferClear() {
            m_ReceiveBuffer = new byte[m_ReceiveBufferSize];
        }
        /// <summary>
        /// 소켓의 Data Receive Event
        /// </summary>
        /// <param name="ar"></param>
        void DataReceived(IAsyncResult ar) {
            try {
                Socket Sock = (Socket)ar.AsyncState;
                if (Sock.Connected) {
                    int received = m_Socket.EndReceive(ar);
                    byte[] buffer = new byte[received];
                    Array.Copy(m_ReceiveBuffer, 0, buffer, 0, received);

                    ReceiveEvent(++m_MsgNum, buffer);

                    ReceiveBufferClear();
                    m_Socket.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, 0, DataReceived, m_Socket);
                }
            }
            catch (SocketException ex) {
                //끊어짐
                //Log
            }
        }
        /// <summary>
        /// Socket Data 전송 함수
        /// </summary>
        /// <param name="bytes"></param>
        public void DataSend(byte[] bytes) {
            try {
                if (m_Socket != null && m_Socket.Connected)
                    m_Socket.Send(bytes);
            }
            catch (SocketException ex) {
                //끊어짐
                //Log
            }
        }
    }
}
