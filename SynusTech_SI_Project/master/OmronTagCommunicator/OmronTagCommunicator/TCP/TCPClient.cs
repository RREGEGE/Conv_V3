using System;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace Master.Interface.TCP
{
    class TCPClient
    {
        public delegate void ReceiveHandler(int MsgNum, byte[] bytes);
        public event ReceiveHandler ReceiveEvent;

        public delegate void ConnectionEventHandler(bool bConnect);
        public event ConnectionEventHandler ConnectEvent;
        Thread ConnectThread;

        Socket  m_MainSock;
        public string  m_ServerIP;
        public int     m_ServerPort;
        string  m_ThreadName;
        bool    bClose = false;

        public TCPClient(string _serverIP, int _serverPort, string _ThreadName)
        {
            m_ServerIP = _serverIP;
            m_ServerPort = _serverPort;
            m_ThreadName = _ThreadName;
        }
        
        void AutoConnectThread()
        {
            while (!bClose)
            {
                try
                {
                    if (!IsConnected())
                    {
                        if (string.IsNullOrEmpty(m_ServerIP) || m_ServerPort < 0 || m_ServerPort > 65535)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }

                        m_MainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(m_ServerIP), m_ServerPort);
                        m_MainSock.Connect(serverEP);

                        if (!m_MainSock.Connected)
                            Thread.Sleep(1000);
                    }
                    else
                    {
                        int nReceiveCount = 0;
                        ConnectEvent(true);

                        while (m_MainSock.Connected)
                        {
                            try
                            {
                                byte[] bytes = new byte[409600];
                                int nCount = m_MainSock.Receive(bytes);

                                Array.Resize(ref bytes, nCount);
                                if (bytes.Length != 0)
                                    ReceiveEvent(++nReceiveCount, bytes);

                                Thread.Sleep(10);
                            }
                            catch
                            {
                                //LogMsg.AddExceptionLog(ex, $"Server IP : {m_ServerIP} / Server Port : {m_ServerPort}");
                            }
                        }

                        ConnectEvent(false);
                        m_MainSock.Close();
                        m_MainSock.Dispose();
                    }
                }
                catch
                {
                    if(m_MainSock != null)
                    {
                        m_MainSock.Close();
                        m_MainSock.Dispose();
                    }
                    Thread.Sleep(1000);
                }
            }
        }
        public void Connect()
        {
            if(ConnectThread?.IsAlive ?? false)
            {
                ConnectThread.Abort();
                ConnectThread.Join();
            }

            if(!(ConnectThread?.IsAlive ?? false))
            {
                ConnectThread = new Thread(AutoConnectThread);
                ConnectThread.IsBackground = true;
                ConnectThread.Name = m_ThreadName;
                ConnectThread.Start();
            }
        }
        public void Close()
        {
            try
            {
                if (m_MainSock != null)
                {
                    if (m_MainSock.Connected)
                        ConnectEvent(false);

                    bClose = true;
                    m_MainSock.Close();
                    m_MainSock.Dispose();
                }

                if ((ConnectThread?.IsAlive ?? false))
                {
                    ConnectThread.Abort();
                    ConnectThread.Join();
                }
            }
            catch
            {

            }
        }
        public void Send(byte[] bytes)
        {
            try
            {
                if (m_MainSock != null && m_MainSock.Connected)
                    m_MainSock.Send(bytes);
            }
            catch
            {

            }
        }
        public bool IsConnected()
        {
            return m_MainSock?.Connected ?? false;
        }
        
    }
}
