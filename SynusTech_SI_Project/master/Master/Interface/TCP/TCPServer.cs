using System;
using System.Net;
using System.Net.Sockets;

namespace Master.Interface.TCP
{
    /// <summary>
    /// TCP Server 운용 시 사용되는 클래스.
    /// </summary>
    class TCPServer
    {
        public delegate void AcceptHandler(Socket socket);
        public event AcceptHandler AcceptEvent;             //연결 허용 이벤트

        Socket m_MainSock;  //연결을 받는 메인 서버 소켓
        int m_ServerPort;   //연결 대기할 포트 넘버

        public TCPServer(int _serverPort)
        {
           //서버용 소켓 생성 및 포트 지정
            m_MainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_ServerPort = _serverPort;
        }
        
        /// <summary>
        /// 서버 동작 시작
        /// </summary>
        public void Start()
        {
            try
            {
                if (m_ServerPort < 0 || m_ServerPort > 65535) //포트 유효 범위 확인
                    return;

                IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, m_ServerPort); //포트 지정
                m_MainSock.Bind(serverEP);
                m_MainSock.Listen(10);
                m_MainSock.BeginAccept(AcceptCallback, null); //연결 대기
            }
            catch
            {
            }
        }

        /// <summary>
        /// 서버 소켓 사용 중인 경우 닫기, 메모리 정리
        /// </summary>
        public void Close()
        {
            if (m_MainSock != null)
            {
                m_MainSock.Close();
                m_MainSock.Dispose();
            }
        }

        /// <summary>
        /// 지정된 서버 포트로 연결 시도가 오는 경우 아래 이벤트 발생
        /// </summary>
        /// <param name="ar"></param>
        void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                
                Socket acceptClient = m_MainSock.EndAccept(ar);     //연결을 시도한 소켓 정보를 가져옴
                AcceptEvent(acceptClient);                          //연결 수락 이벤트 발생과 함께 연결 시도한 소켓 정보를 전달
                m_MainSock.BeginAccept(AcceptCallback, null);       //다시 메인 서버는 연결 대기 상태로 진입
            }
            catch
            { }
        }
    }
}
