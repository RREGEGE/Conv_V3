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

        public void Close()
        {
            if (m_Socket != null)
            {
                m_Socket.Close();
                m_Socket.Dispose();
            }
        }

        public bool IsConnected()
        {
            return m_Socket?.Connected ?? false;
        }

        public void SetSocket(Socket socket)
        {
            m_Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            m_Socket = socket;
            m_MsgNum = 0;
            m_Socket.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, 0, DataReceived, m_Socket);
        }

        public Socket GetSocket()
        {
            return m_Socket;
        }

        private void ReceiveBufferClear()
        {
            m_ReceiveBuffer = new byte[m_ReceiveBufferSize];
        }


        void DataReceived(IAsyncResult ar)
        {
            try
            {
                Socket Sock = (Socket)ar.AsyncState;
                if (Sock.Connected)
                {
                    int received = m_Socket.EndReceive(ar);
                    byte[] buffer = new byte[received];
                    Array.Copy(m_ReceiveBuffer, 0, buffer, 0, received);

                    ReceiveEvent(++m_MsgNum, buffer);

                    ReceiveBufferClear();
                    m_Socket.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, 0, DataReceived, m_Socket);
                }
            }
            catch
            {
                //끊어짐
                //Log
            }
        }
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
