using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.TCP;
using System.Threading;
using System.Collections;

namespace OmronTagCommunicator
{
    class DataSender
    {
        TCPClient m_Client;
        byte[] m_PacketBuffer = new byte[409600];
        int m_CurrentPacketLen = 0;

        public DataSender()
        {
            m_Client = new TCPClient("127.0.0.1", 60001, $"Omron Client]");
            m_Client.ConnectEvent += ClientConnectEvent;
            m_Client.ReceiveEvent += ReceiveData;
            m_Client.Connect();
        }
        private void ClearPacketBuffer()
        {
            m_CurrentPacketLen = 0;
            Array.Clear(m_PacketBuffer, 0, m_PacketBuffer.Length);
        }
        public bool IsConnected()
        {
            return m_Client?.IsConnected() ?? false;
        }

        public void CloseDataSender()
        {
            if (m_Client != null)
            {
                m_Client.Close();
                m_Client.ReceiveEvent -= ReceiveData;
                m_Client.ConnectEvent -= ClientConnectEvent;
            }
        }
        public void ReceiveData(int MsgNum, byte[] bytes)
        {
            try
            {
                if (m_CurrentPacketLen + bytes.Length > m_PacketBuffer.Length)
                {
                    ClearPacketBuffer();
                }
                else
                {
                    Array.Copy(bytes, 0, m_PacketBuffer, m_CurrentPacketLen, bytes.Length);
                    m_CurrentPacketLen += bytes.Length;
                }
            }
            catch (Exception ex)
            {
            }

            // StartByte(1) + DataType(1) + State(1) + DataArray(32 or 512) + EndByte(1)
            // Data Type -> 5 : Bit Map, 6 : Word Map
            // State -> 0 : Error, 1 : Valid
            //BitMap Size = 64 * 4 / 8
            //WordMap Size = 64 * 2 * 4

            const int PacketHeaderLen = 36; //최소 수량
            int nPos = 0;
            while (m_CurrentPacketLen >= PacketHeaderLen)
            {
                if (m_PacketBuffer[nPos] == 0x02)
                {
                    int StartPos = nPos;

                    if (StartPos + 1 < m_CurrentPacketLen)
                    {
                        if (m_PacketBuffer[StartPos + 1] == 0x05)
                        {
                            //BitMap
                            if (StartPos + 35 < m_CurrentPacketLen)
                            {
                                if (m_PacketBuffer[StartPos + 35] == 0x03)
                                {
                                    if (m_PacketBuffer[StartPos + 2] == 0x01)
                                    {
                                        //Write
                                        byte[] OnlyDataPackets = new byte[32];
                                        Array.Copy(m_PacketBuffer, StartPos + 3, OnlyDataPackets, 0, OnlyDataPackets.Length);
                                        BitArray ba = new BitArray(OnlyDataPackets);

                                        if (Form1.m_WriteOmronBitMap.Length != ba.Length)
                                            break;

                                        for (int nCount = 0; nCount < ba.Length; nCount++)
                                        {
                                            Form1.m_WriteOmronBitMap[nCount] = ba[nCount];
                                        }
                                    }

                                    m_CurrentPacketLen -= (StartPos + 35);
                                    if (m_CurrentPacketLen > 0)
                                        Array.Copy(m_PacketBuffer, StartPos + 35, m_PacketBuffer, 0, m_CurrentPacketLen);
                                    else
                                        ClearPacketBuffer();
                                }
                                else
                                {
                                    m_CurrentPacketLen -= (StartPos + 35);
                                    if (m_CurrentPacketLen > 0)
                                        Array.Copy(m_PacketBuffer, StartPos + 35, m_PacketBuffer, 0, m_CurrentPacketLen);
                                    else
                                        ClearPacketBuffer();
                                }
                            }
                            else
                                break;

                        }
                        else if (m_PacketBuffer[StartPos + 1] == 0x06)
                        {
                            //WordMap
                            if (StartPos + 515 < m_CurrentPacketLen)
                            {
                                if (m_PacketBuffer[StartPos + 515] == 0x03)
                                {
                                    if (m_PacketBuffer[StartPos + 2] == 0x01)
                                    {
                                        //Write
                                        byte[] OnlyDataPackets = new byte[4 * 64 * 2];
                                        Array.Copy(m_PacketBuffer, StartPos + 3, OnlyDataPackets, 0, OnlyDataPackets.Length);

                                        if (Form1.m_WriteOmronWordMap.Length * 2 != OnlyDataPackets.Length)
                                            break;

                                        Buffer.BlockCopy(OnlyDataPackets, 0, Form1.m_WriteOmronWordMap, 0, Form1.m_WriteOmronWordMap.Length * 2);
                                    }

                                    m_CurrentPacketLen -= (StartPos + 515);
                                    if (m_CurrentPacketLen > 0)
                                        Array.Copy(m_PacketBuffer, StartPos + 515, m_PacketBuffer, 0, m_CurrentPacketLen);
                                    else
                                        ClearPacketBuffer();
                                }
                                else
                                {
                                    m_CurrentPacketLen -= (StartPos + 35);
                                    if (m_CurrentPacketLen > 0)
                                        Array.Copy(m_PacketBuffer, StartPos + 35, m_PacketBuffer, 0, m_CurrentPacketLen);
                                    else
                                        ClearPacketBuffer();
                                }
                            }
                            else
                                break;
                        }
                        else
                            nPos++;
                    }
                    else
                        break;
                }
                else
                {
                    if (nPos + 1 < m_CurrentPacketLen)
                        nPos++;
                }
            }
        }

        private void ClientConnectEvent(bool bConnect)
        {
            if (bConnect)
            {
                ClearPacketBuffer();
                Thread LocalThread = new Thread(delegate ()
                {
                    while (IsConnected())
                    {
                        WriteTagMap();
                        Thread.Sleep(100);
                    }
                });
                LocalThread.Name = $"Omron Status";
                LocalThread.IsBackground = true;
                LocalThread.Start();
            }
        }
        private void WriteTagMap()
        {
            WriteBitMap();
            WriteWordMap();
        }
        public void WriteBitMap()
        {
            byte Invalid = Form1.m_bValidState ? (byte)0x01 : (byte)0x00;
            byte[] HeaderByte = new byte[] { 0x02, 0x05, Invalid };
            BitArray ba = new BitArray(Form1.m_ReadOmronBitMap);
            byte[] DataByte = new byte[32];
            ba.CopyTo(DataByte, 0);
            byte[] EndByte = new byte[] { 0x03 };


            IEnumerable<byte> send = HeaderByte.Concat(DataByte).Concat(EndByte);
            SendData(send.ToArray());
        }
        public void WriteWordMap()
        {
            byte Invalid = Form1.m_bValidState ? (byte)0x01 : (byte)0x00;
            byte[] HeaderByte = new byte[] { 0x02, 0x06, Invalid };
            byte[] DataByte = new byte[64 * 4 * 2];
            Buffer.BlockCopy(Form1.m_ReadOmronWordMap, 0, DataByte, 0, DataByte.Length);
            byte[] EndByte = new byte[] { 0x03 };


            IEnumerable<byte> send = HeaderByte.Concat(DataByte).Concat(EndByte);
            SendData(send.ToArray());
        }

        public void SendData(byte[] bytes)
        {
            if (m_Client.IsConnected())
                m_Client.Send(bytes);
        }
    }
}
