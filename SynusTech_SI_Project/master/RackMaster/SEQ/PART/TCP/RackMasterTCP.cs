using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.TCP;
using RackMaster.SEQ.COMMON;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace RackMaster.SEQ.PART
{
    public partial class RackMasterMain {
        TCPServer m_RMServer;   // CIM Server
        TCPSocket m_Socket;     // CIM Socket
        byte[] m_PacketBuffer = new byte[409600];
        int m_CurrentPacketLen = 0;

        int m_ServerPort = 50002;

        public static bool isConnected = false;

        private static int CIM_2_RM_BIT_SIZE = 256;
        private static int RM_2_CIM_BIT_SIZE = 256;
        private static int CIM_2_RM_WORD_SIZE = 256;
        private static int RM_2_CIM_WORD_SIZE = 256;

        private object m_localThreadObj = null;

        private static bool[] m_RackMaster_RecvBitMap;
        private static bool[] m_RackMaster_SendBitMap;
        private static short[] m_RackMaster_RecvWordMap;
        private static short[] m_RackMaster_SendWordMap;

        private static BitArray m_RackMaster_ErrorWord;
        private static int RM_ERRORWORD_BIT_SIZE = 192;

        private static int m_sendCycleTime = 0;
        /// <summary>
        /// CIM -> RM Bit Size
        /// </summary>
        /// <returns></returns>
        public int GetMemoryMapSize_CIM2RMBit() {
            return CIM_2_RM_BIT_SIZE;
        }
        /// <summary>
        /// RM -> CIM Bit Size
        /// </summary>
        /// <returns></returns>
        public int GetMemoryMapSize_RM2CIMBit() {
            return RM_2_CIM_BIT_SIZE;
        }
        /// <summary>
        /// CIM -> RM Word Size
        /// </summary>
        /// <returns></returns>
        public int GetMemoryMapSize_CIM2RMWord() {
            return CIM_2_RM_WORD_SIZE;
        }
        /// <summary>
        /// RM -> CIM Word Size
        /// </summary>
        /// <returns></returns>
        public int GetMemoryMapSize_RM2CIMWord() {
            return RM_2_CIM_WORD_SIZE;
        }
        /// <summary>
        /// Program 종료 시 Server, Socket 종료
        /// </summary>
        public void CloseCIM() {
            m_RMServer.Close();
            m_RMServer.AcceptEvent -= Server_AcceptEvent;
            m_Socket.Close();
            m_Socket.ReceiveEvent -= ReceiveData;
        }
        /// <summary>
        /// 열어두었던 서버에 연결이 시도되었을 때 호출되는 이벤트 함수
        /// Master와 통신 중이 아닐 때만 시도
        /// </summary>
        /// <param name="socket"></param>
        private void Server_AcceptEvent(Socket socket) {
            if (!IsConnected_Master()) {
                m_Socket.SetSocket(socket);
                CIMNetworkInit();
            }
        }
        /// <summary>
        /// Master와 통신이 시도될 때 Packet Buffer 내용 클리어
        /// </summary>
        private void CIMNetworkInit() {
            ClearPacketBuffer();
        }
        /// <summary>
        /// 위 함수와 동일
        /// </summary>
        private void ClearPacketBuffer() {
            m_CurrentPacketLen = 0;
            Array.Clear(m_PacketBuffer, 0, m_PacketBuffer.Length);
        }
        /// <summary>
        /// Packet 데이터가 서버로 전달될 때 호출되는 함수
        /// Byte 배열로 받은 데이터를 Protocol Role에 위배되는지 체크
        /// </summary>
        /// <param name="MsgNum"></param>
        /// <param name="bytes"></param>
        public void ReceiveData(int MsgNum, byte[] bytes) {
            try {
                if (m_CurrentPacketLen + bytes.Length > m_PacketBuffer.Length) {
                    ClearPacketBuffer();
                }
                else {
                    Array.Copy(bytes, 0, m_PacketBuffer, m_CurrentPacketLen, bytes.Length);
                    m_CurrentPacketLen += bytes.Length;
                }
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.TCP, Log.LogMessage_Main.TCP_ReceiveDataFail, ex));
            }

            while (m_CurrentPacketLen >= ProtocolRoles.Recv_TCPHeaderLen) {
                bool bValidPacket = ProtocolRoles.IsPacketValid(ref m_PacketBuffer, ref m_CurrentPacketLen, $"CIM");

                if (bValidPacket) {
                    int value_DataLen = ProtocolRoles.GetValue_DataLen(m_PacketBuffer);
                    int PacketLength = ProtocolRoles.Recv_DataLen + value_DataLen;

                    byte[] receivePackets = new byte[PacketLength];
                    Array.Copy(m_PacketBuffer, 0, receivePackets, 0, receivePackets.Length);

                    ReceiveAction(receivePackets);

                    m_CurrentPacketLen -= (PacketLength);
                    if (m_CurrentPacketLen > 0)
                        Array.Copy(m_PacketBuffer, PacketLength, m_PacketBuffer, 0, m_CurrentPacketLen);
                    else
                        ClearPacketBuffer();
                }
            }
        }
        /// <summary>
        /// Byte 배열에 담긴 데이터를 전송
        /// </summary>
        /// <param name="bytes"></param>
        public void SendData(byte[] bytes) {
            m_Socket.DataSend(bytes);
        }
        /// <summary>
        /// Packet 데이터를 Protocol Role에 맞춰 파싱(Data Type, Data Map Address 등)
        /// </summary>
        /// <param name="receivePackets"></param>
        public void ReceiveAction(byte[] receivePackets) {
            sbyte dataType = ProtocolRoles.GetValue_DataType(receivePackets);
            short dataMapAddress = ProtocolRoles.GetValue_DataAddress(receivePackets);

            //Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.TCP, $"Data Type={dataType}, DataMapAddress={dataMapAddress}"));
;
            switch ((ProtocolRoles.DataType)dataType) {
                case ProtocolRoles.DataType.STK_CIM_2_Controller_RM_Bit_Data:
                    Parsing_CIM_2_RM_Bit_Data(dataMapAddress, receivePackets);
                    break;

                case ProtocolRoles.DataType.STK_CIM_2_Controller_RM_Word_Data:
                    Parsing_CIM_2_RM_Word_Data(dataMapAddress, receivePackets);
                    break;

                case ProtocolRoles.DataType.ControlMSG:
                    Parsing_ControlMSG(receivePackets);
                    break;
            }
        }
        /// <summary>
        /// CIM에게 업데이트할 데이터 주기 설정
        /// </summary>
        /// <param name="receivePackets"></param>
        private void Parsing_ControlMSG(byte[] receivePackets) {
            try
            {
                byte[] receiveValues = ProtocolRoles.GetReceiveDataArray(receivePackets);

                m_sendCycleTime = BitConverter.ToInt16(receiveValues, 0);

                if (m_localThreadObj != null)
                {
                    Thread threadPt = (Thread)m_localThreadObj;

                    if (threadPt.IsAlive)
                    {
                        threadPt.Abort();
                        threadPt.Join();
                    }
                    m_localThreadObj = null;
                }

                Thread localThread = new Thread(delegate () {
                    while(m_motion == null)
                    {
                        Thread.Sleep(1000);
                    }

                    while (IsConnected_Master())
                    {
                        UpdateSendBitData();
                        UpdateSendWordData();
                        Send_RM_2_CIM_Bit_Data();
                        Send_RM_2_CIM_Word_Data();

                        Thread.Sleep(m_sendCycleTime);
                    }
                    ClearSendData();
                    ClearReceiveData();
                });
                localThread.IsBackground = true;
                localThread.Start();
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.TCP, Log.LogMessage_Main.TCP_UpdateStart, $"Send Cycle Time={m_sendCycleTime}"));

                m_localThreadObj = localThread;
            }catch(Exception ex)
            {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.TCP, Log.LogMessage_Main.TCP_ControlMessageParsingFail, ex));
            }
        }
        /// <summary>
        /// CIM -> RM Bit 데이터 파싱
        /// </summary>
        /// <param name="dataMapAddress"></param>
        /// <param name="receivePackets"></param>
        private void Parsing_CIM_2_RM_Bit_Data(short dataMapAddress, byte[] receivePackets) {
            if (dataMapAddress >= m_RackMaster_RecvBitMap.Length)
                return;

            bool receiveValue = Convert.ToBoolean(receivePackets[ProtocolRoles.Recv_TCPHeaderLen]);
            bool currentValue = m_RackMaster_RecvBitMap[dataMapAddress];

            if (receiveValue != currentValue) {
                m_RackMaster_RecvBitMap[dataMapAddress] = receiveValue;
            }
            Action_CIM_2_RM_Bit_Data((ReceiveBitMap)dataMapAddress);
        }
        /// <summary>
        /// CIM -> RM Word 데이터 파싱
        /// </summary>
        /// <param name="dataMapAddress"></param>
        /// <param name="receivePackets"></param>
        private void Parsing_CIM_2_RM_Word_Data(short dataMapAddress, byte[] receivePackets) {
            if (dataMapAddress >= m_RackMaster_RecvWordMap.Length)
                return;

            byte[] receive_values = ProtocolRoles.GetReceiveDataArray(receivePackets);
            byte[] current_values = ProtocolRoles.GetCurrentWordDataToByteArray(dataMapAddress, receivePackets.Length - ProtocolRoles.Recv_TCPHeaderLen, m_RackMaster_RecvWordMap);

            if (!ProtocolRoles.IsByteArrayCompare(receive_values, current_values)) {
                Buffer.BlockCopy(receive_values, 0, m_RackMaster_RecvWordMap, dataMapAddress * sizeof(short), receivePackets.Length - ProtocolRoles.Recv_TCPHeaderLen);
            }
            Action_CIM_2_RM_Word_Data((ReceiveWordMap)dataMapAddress);
        }
        /// <summary>
        /// CIM -> RM Packet 데이터를 담는 배열 클리어
        /// </summary>
        private void ClearReceiveData() {
            for (int i = 0; i < m_RackMaster_RecvBitMap.Length; i++) {
                m_RackMaster_RecvBitMap[i] = false;
            }
            for (int i = 0; i < m_RackMaster_RecvWordMap.Length; i++) {
                m_RackMaster_RecvWordMap[i] = 0;
            }
        }
        /// <summary>
        /// RM -> CIM Packet 데이터를 담는 배열 클리어
        /// </summary>
        private void ClearSendData() {
            for (int i = 0; i < m_RackMaster_SendBitMap.Length; i++) {
                m_RackMaster_SendBitMap[i] = false;
            }
            for (int i = 0; i < m_RackMaster_SendWordMap.Length; i++) {
                m_RackMaster_SendWordMap[i] = 0;
            }
        }
        /// <summary>
        /// RM -> CIM Bit 데이터 전송
        /// </summary>
        private void Send_RM_2_CIM_Bit_Data() {
            try {
                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + RM_2_CIM_BIT_SIZE);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_RM_Bit_Data };
                byte[] DataOffset = { 0, 0 };
                byte[] Data = new byte[RM_2_CIM_BIT_SIZE];
                Buffer.BlockCopy(m_RackMaster_SendBitMap, 0, Data, 0, RM_2_CIM_BIT_SIZE);

                if (ProtocolRoles.Send_LittleEndian) {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                SendData(send.ToArray());
            }
            catch(Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.TCP, "Send Bit Exception, ", ex));
            }
        }
        /// <summary>
        /// RM -> CIM Word 데이터 전송
        /// </summary>
        public void Send_RM_2_CIM_Word_Data() {
            try {
                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + (RM_2_CIM_WORD_SIZE * sizeof(short)));
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_RM_Word_Data };
                byte[] DataOffset = { 0, 0 };
                byte[] Data = new byte[RM_2_CIM_WORD_SIZE * sizeof(short)];
                Buffer.BlockCopy(m_RackMaster_SendWordMap, 0, Data, 0, RM_2_CIM_WORD_SIZE * sizeof(short));

                if (ProtocolRoles.Send_LittleEndian) {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < Data.Length / sizeof(short); nCount++) {
                        Array.Reverse(Data, nCount * sizeof(short), sizeof(short));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                SendData(send.ToArray());
            }
            catch(Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.TCP, "Send Word Exception, ", ex));
            }
        }
    }

    public partial class RackMasterMain {
        TCPServer m_regServer;      // Regulator와 통신에 필요한 서버
        TCPSocket m_regSocket;      // Regulator와 통신에 필요한 소켓
        byte[] m_regPacketBuffer = new byte[m_regPacketLength];
        byte[] m_regSendPacketData = new byte[m_regSendPacketLength];
        public static int m_regPacketLength = 40;
        public static int m_regSendPacketLength = 18;

        //string m_regIP = "127.0.0.1";
        //string m_regIP = "192.168.210.100";
        int m_regPort = 5376;

        private static bool isClientConnected = false;
        /// <summary>
        /// Regulator와 통신 종료
        /// </summary>
        public void CloseRegulator() {
            //if(m_client != null) {
            //    m_client.Close();
            //    m_client.ReceiveEvent -= Client_ReceivData;
            //    m_client.ConnectEvent -= Client_ConnectEvent;
            //}
            m_regServer.Close();
            m_regServer.AcceptEvent -= Regulator_AcceptEvent;
            m_regSocket.Close();
            m_regSocket.ReceiveEvent -= Regulator_ReceivData;
        }
        /// <summary>
        /// Regulator와 통신이 성공할 때 호출되는 이벤트 함수
        /// </summary>
        public void Regulator_ConnectEvent() {
            Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.TCP, $"Regulator Connect!"));
            int tryCount = 0;
            while (true) {
                Thread.Sleep(500);
                if (m_regSocket.IsConnected()) {
                    Regulator_SendData();
                }
                else {
                    tryCount++;
                    if(tryCount > 10) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.TCP, $"Regulator Disconnected!"));
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Regulator에 전송할 데이터 업데이트 및 전송
        /// </summary>
        public void Regulator_SendData() {
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Start] = 0x02;
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Command] = (byte)ProtocolRoles_Regulator.CharToASCII('R');
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.WPS_Regulator_ID] = 0x36;
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.WPS_Regulator_ID + 1] = 0x31;

            int year        = DateTime.Now.Year;
            int month       = DateTime.Now.Month;
            int day         = DateTime.Now.Day;
            int hour        = DateTime.Now.Hour;
            int minute      = DateTime.Now.Minute;
            int second      = DateTime.Now.Second;

            year %= 100;

            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Year]        = (byte)ProtocolRoles_Regulator.IntToASCII(year / 10);
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Year + 1]    = (byte)ProtocolRoles_Regulator.IntToASCII(year & 10);
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Month]       = (byte)ProtocolRoles_Regulator.IntToASCII(month / 10);
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Month + 1]   = (byte)ProtocolRoles_Regulator.IntToASCII(month % 10);
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Date]        = (byte)ProtocolRoles_Regulator.IntToASCII(day / 10);
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Date + 1]    = (byte)ProtocolRoles_Regulator.IntToASCII(day % 10);
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Hour]        = (byte)ProtocolRoles_Regulator.IntToASCII(hour / 10);
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Hour + 1]    = (byte)ProtocolRoles_Regulator.IntToASCII(hour % 10);
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Minute]      = (byte)ProtocolRoles_Regulator.IntToASCII(minute / 10);
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Minute + 1]  = (byte)ProtocolRoles_Regulator.IntToASCII(minute % 10);
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Second]      = (byte)ProtocolRoles_Regulator.IntToASCII(second / 10);
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.Second + 1]  = (byte)ProtocolRoles_Regulator.IntToASCII(second % 10);

            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.CheckSum] = (byte)ProtocolRoles_Regulator.GetCheckSum_Send(m_regSendPacketData);
            m_regSendPacketData[(int)ProtocolRoles_Regulator.RackMasterToRegulator.End] = 0x03;

            m_regSocket.DataSend(m_regSendPacketData);
        }
        /// <summary>
        /// Regulator -> RM Packet을 배열에 복사
        /// </summary>
        /// <param name="msgNum"></param>
        /// <param name="packets"></param>
        public void Regulator_ReceivData(int msgNum, byte[] packets) {
            if(ProtocolRoles_Regulator.IsPacketValid(packets, m_regPacketLength)) {
                //Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.TCP, $"Receive Regulator Data ! "));

                Buffer.BlockCopy(packets, 0, m_regPacketBuffer, 0, packets.Length);
            }
        }
        /// <summary>
        /// 배열에 담긴 Data 중 특정 Data 반환
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public short Regulator_GetData(ProtocolRoles_Regulator.RegulatorToRackMaster order) {
            return ProtocolRoles_Regulator.GetRegulatorData(m_regPacketBuffer, order);
        }
        /// <summary>
        /// Regulator 서버에 접속 요청이 들어왔을 때 Regulator와 통신 중이 아닌경우 통신 시도
        /// </summary>
        /// <param name="socket"></param>
        public void Regulator_AcceptEvent(Socket socket) {
            if (!m_regSocket.IsConnected()) {
                m_regSocket.SetSocket(socket);
            }
        }
    }
}
