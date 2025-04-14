using System;
using System.Net;
using System.Net.Sockets;

namespace Master.Interface.TCP
{
    class TCPServer
    {
        public delegate void AcceptHandler(Socket socket);
        public event AcceptHandler AcceptEvent;

        Socket m_MainSock;
        int m_ServerPort;

        public TCPServer(int _serverPort)
        {
            m_MainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_ServerPort = _serverPort;
        }
        
        public void Start()
        {
            try
            {
                if (m_ServerPort < 0 || m_ServerPort > 65535)
                    return;

                IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, m_ServerPort);
                m_MainSock.Bind(serverEP);
                m_MainSock.Listen(10);
                m_MainSock.BeginAccept(AcceptCallback, null);
            }
            catch
            {
            }
        }
        public void Close()
        {
            if (m_MainSock != null)
            {
                m_MainSock.Close();
                m_MainSock.Dispose();
            }
        }
        void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket acceptClient = m_MainSock.EndAccept(ar);
                AcceptEvent(acceptClient);
                m_MainSock.BeginAccept(AcceptCallback, null);
            }
            catch
            { }
        }
    }
}
