using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SEQ.TCP {
    public class TCPServer {
        public delegate void AcceptHandler(Socket socket);
        public event AcceptHandler AcceptEvent;
        public delegate void TCPConnectionAcceptEvent();
        public event TCPConnectionAcceptEvent ConnectionEvent;

        Socket m_MainSock;
        int m_ServerPort;

        public TCPServer(int _serverPort) {
            m_MainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_ServerPort = _serverPort;
        }
        /// <summary>
        /// 서버 동작 시작
        /// </summary>
        public void Start() {
            try {
                if (m_ServerPort < 0)
                    return;

                IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, m_ServerPort);
                m_MainSock.Bind(serverEP);
                m_MainSock.Listen(10);
                m_MainSock.BeginAccept(AcceptCallback, null);
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.TCP, Log.LogMessage_Main.TCP_BeginServerFail, ex));
            }
        }
        /// <summary>
        /// 서버 소켓 사용 중인 경우 닫기, 메모리 정리
        /// </summary>
        public void Close() {
            if (m_MainSock != null) {
                m_MainSock.Close();
                m_MainSock.Dispose();
            }
        }
        /// <summary>
        /// 지정된 서버 포트로 연결 시도가 오는 경우 아래 이벤트 발생
        /// </summary>
        /// <param name="ar"></param>
        void AcceptCallback(IAsyncResult ar) {
            try {
                Socket acceptClient = m_MainSock.EndAccept(ar);
                AcceptEvent(acceptClient);
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.TCP, $"{Log.GetLogMessage(Log.LogMessage_Main.TCP_StartConnection)}, Client: {acceptClient.RemoteEndPoint}"));
                ConnectionEvent();
                m_MainSock.BeginAccept(AcceptCallback, null);
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.TCP, Log.LogMessage_Main.TCP_ConnectionFail, ex));
            }
        }
    }
}
