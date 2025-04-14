using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SEQ.TCP {
    public class TCPClient {
        public delegate void ReceiveHandler(int MsgNum, byte[] bytes);
        public event ReceiveHandler ReceiveEvent;

        public delegate void ConnectionEventHandler(bool bConnect);
        public event ConnectionEventHandler ConnectEvent;
        Thread ConnectThread;

        Socket m_MainSock;
        public string m_ServerIP;
        public int m_ServerPort;
        string m_ThreadName;
        bool bClose = false;

        public TCPClient(string _serverIP, int _serverPort, string _ThreadName) {
            m_ServerIP = _serverIP;
            m_ServerPort = _serverPort;
            m_ThreadName = _ThreadName;
        }
        /// <summary>
        /// Server에 자동으로 접속 시도하는 스레드
        /// </summary>
        void AutoConnectThread() {
            while (!bClose) {
                try {
                    if (!IsConnected()) {
                        if (string.IsNullOrEmpty(m_ServerIP) || m_ServerPort < 0 || m_ServerPort > 65535) {
                            Thread.Sleep(1000);
                            continue;
                        }

                        m_MainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(m_ServerIP), m_ServerPort);
                        m_MainSock.Connect(serverEP);

                        if (!m_MainSock.Connected)
                            Thread.Sleep(1000);
                    }
                    else {
                        int nReceiveCount = 0;
                        ConnectEvent(true);

                        while (m_MainSock.Connected) {
                            try {
                                byte[] bytes = new byte[409600];
                                int nCount = m_MainSock.Receive(bytes);

                                Array.Resize(ref bytes, nCount);
                                if (bytes.Length != 0)
                                    ReceiveEvent(++nReceiveCount, bytes);

                                Thread.Sleep(1);
                            }
                            catch (Exception ex) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.TCP, $"Server IP : {m_ServerIP} / Server Port : {m_ServerPort}", ex));
                            }
                        }

                        ConnectEvent(false);
                        m_MainSock.Close();
                        m_MainSock.Dispose();
                    }
                }
                catch (Exception ex) {
                    if (m_MainSock != null) {
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
        public void Connect() {
            if (ConnectThread?.IsAlive ?? false) {
                ConnectThread.Abort();
                ConnectThread.Join();
            }

            if (!(ConnectThread?.IsAlive ?? false)) {
                ConnectThread = new Thread(AutoConnectThread);
                ConnectThread.IsBackground = true;
                ConnectThread.Name = m_ThreadName;
                ConnectThread.Start();
            }
        }
        /// <summary>
        /// TCP Client Close 기능 -> 연결 중인 소켓 연결 해제 이벤트 발생 및 소켓 정리, 스레드 정리
        /// </summary>
        public void Close() {
            try {
                if (m_MainSock != null) {
                    if (m_MainSock.Connected)
                        ConnectEvent(false);

                    bClose = true;
                    m_MainSock.Close();
                    m_MainSock.Dispose();
                }

                if ((ConnectThread?.IsAlive ?? false)) {
                    ConnectThread.Abort();
                    ConnectThread.Join();
                }
            }
            catch {

            }
        }
        /// <summary>
        /// TCP Client 연결 상태인 경우 패킷 전송
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(byte[] bytes) {
            try {
                if (m_MainSock != null && m_MainSock.Connected)
                    m_MainSock.Send(bytes);
            }
            catch {

            }
        }
        /// <summary>
        /// TCP Client 연결 상태 출력
        /// </summary>
        /// <returns></returns>
        public bool IsConnected() {
            return m_MainSock?.Connected ?? false;
        }

    }
}
