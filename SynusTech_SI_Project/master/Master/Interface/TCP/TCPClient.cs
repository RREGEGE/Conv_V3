using System;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace Master.Interface.TCP
{
    /// <summary>
    /// TCP Client 운용 시 사용되는 클래스.
    /// </summary>
    class TCPClient
    {
        public delegate void ReceiveHandler(int MsgNum, byte[] bytes);
        public event ReceiveHandler ReceiveEvent;                       //Client Receive Event

        public delegate void ConnectionEventHandler(bool bConnect);
        public event ConnectionEventHandler ConnectEvent;               //Client Connect Event
        Thread ConnectThread;

        Socket  m_MainSock;
        public string  m_ServerIP;
        public int     m_ServerPort;
        string  m_ThreadName;
        bool    bClose = false;

        public TCPClient(string _serverIP, int _serverPort, string _ThreadName)
        {
            //객체 생성과 동시에 IP, Port, Thread Name 지정
            m_ServerIP = _serverIP;
            m_ServerPort = _serverPort;
            m_ThreadName = _ThreadName;
        }
        
        /// <summary>
        /// 자동 연결 스레드
        /// </summary>
        void AutoConnectThread()
        {
            while (!bClose) //소켓을 닫은 상태가 아닌 경우 계속 실행
            {
                try
                {
                    if (!IsConnected()) //연결 이 아닌 경우
                    {
                        if (string.IsNullOrEmpty(m_ServerIP) || m_ServerPort < 0 || m_ServerPort > 65535) //서버 IP, Port 주소 이상 있는 경우
                        {
                            Thread.Sleep(1000);
                            continue;
                        }

                        m_MainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(m_ServerIP), m_ServerPort);
                        m_MainSock.Connect(serverEP); //연결 시도

                        if (!m_MainSock.Connected) //연결 실패한 경우
                            Thread.Sleep(1000); //휴지기
                    }
                    else
                    {
                        //연결 된 경우
                        int nReceiveCount = 0; //패킷 리시브 카운트 초기화
                        ConnectEvent(true); //연결 이벤트 발생

                        while (m_MainSock.Connected) //연결 된 경우 패킷 이벤트 발생 후 대기 반복
                        {
                            try
                            {
                                byte[] bytes = new byte[409600];
                                int nCount = m_MainSock.Receive(bytes); //수신 대기, 수신 된 경우 블로킹 해제

                                Array.Resize(ref bytes, nCount);
                                if (bytes.Length != 0)
                                    ReceiveEvent(++nReceiveCount, bytes); //패킷이 있는 경우 리시브 이벤트 발생

                                Thread.Sleep(10);
                            }
                            catch
                            {
                                //LogMsg.AddExceptionLog(ex, $"Server IP : {m_ServerIP} / Server Port : {m_ServerPort}");
                            }
                        }

                        //연결 해제된 상황
                        ConnectEvent(false);    //연결 해제 이벤트 발생
                        m_MainSock.Close();     //소켓 닫기
                        m_MainSock.Dispose();   //소켓 메모리 정리
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
        
        /// <summary>
        /// TCP Client Connect 기능 -> 자동 연결 thread 실행
        /// </summary>
        public void Connect()
        {
            //객체 연결 동작 호출 시 자동 연결 스레드 실행
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
        
        /// <summary>
        /// TCP Client Close 기능 -> 연결 중인 소켓 연결 해제 이벤트 발생 및 소켓 정리, 스레드 정리
        /// </summary>
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
        
        /// <summary>
        /// TCP Client 연결 상태인 경우 패킷 전송
        /// </summary>
        /// <param name="bytes"></param>
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
        
        /// <summary>
        /// TCP Client 연결 상태 출력
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return m_MainSock?.Connected ?? false;
        }
        
    }
}
