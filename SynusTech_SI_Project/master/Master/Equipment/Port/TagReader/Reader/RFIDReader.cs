using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.TCP;
using System.Threading;
using System.Diagnostics;

namespace Master.Equipment.Port.TagReader.ReaderEquip
{
    /// <summary>
    /// CRE1356 - RFID 제품 관련 클래스 및 기능
    /// 제공 받은 문서 토대로 만들어진 기능
    /// </summary>
    public class RFIDReader
    {
        public enum ErrorCode
        {
            None,
            Unknown_CommandID,
            Not_Yet_Implemented_CommandID,
            Invalid_Destination_Address_DeviceID,
            Invalid_System_Register_Address,
            Timeout_Error,
            Invalid_SLRC_Register_Address,
            Out_of_System_Register_Address_Range,
            Out_of_SLRC_Register_Address_Range,
            Out_of_RF_Channel_Number,
            Out_of_Bit_Range,
            Invalid_Bit_Value,
            Check_Sum_Error,
            Write_Command_Fail,
            Read_Command_Fail,
            Long_Data_Length_MAX32Bytes,
            RF_Channel_Disabled,
            SLRC_Reset_Error,
            SLRC_Parallel_Bus_Error,
            Max_Time_Slot_Error_MAX255,
            Not_Supported_RF_Protocol,
            ICODE_Wrong_Command_Parameter,
            ICODE_Timeout,
            ICODE_No_Tag,
            ICODE_CRC_Error,
            ICODE_Collision_Error,
            ICODE_SNR_Error,
            ICODE_Count_Error,
            RFU_0,
            ICODE_Invalid_Quit_Value,
            ICODE_Weak_Collision_Error,
            ICODE_Write_Fail,
            ICODE_Halt_Fail,
            ICODE_Not_Implemented_Error,
            RFU_1,
            RFU_2,
            RFU_3,
            RFU_4,
            RFU_5,
            Family_Code_Mismatch,
            Application_Code_Mismatch,
            ICODE_Framing_Error,
            Carrier_Disabled,

            ReadOrWriteReceive_In_WriteAction = 0xa1,
            ReadOrWriteReceive_In_ReadAction,
            WriteData_112BytesOver,
            Length_Data_Mismatch
        }
        public enum ReaderOperationMode
        {
            None,
            Verbose,
            AutoRead
        }

        public enum RequestResult
        {
            None,
            ResponseofReadCommand,
            ResponseofWriteCommand,
            AbnormalCommand
        }
        public enum ANT : byte
        {
            ANT1,
            ANT2,
            ANT3,
            ANT4
        }
        public enum Model
        {
            CH2,
            CH4
        }

        TCPClient m_RFIDClient;
        byte[] m_PacketBuffer = new byte[409600];
        int m_CurrentPacketLen = 0;

        ReaderOperationMode eReaderOperationMode;
        RequestResult eRequestResult;

        bool m_bSendState = false;
        bool m_bTagReadSuccess = false;

        Model m_eModel = Model.CH2;
        byte TagReadSize = 32;
        string PortID;
        string ErrorStr = string.Empty;
        string Tag;

        Stopwatch TimeoutSt = new Stopwatch();

        public int n_RFIDReadCount = 0;

        public RFIDReader(string _PortID, string _ServerIP, int _PortNum)
        {
            PortID = _PortID;

            m_RFIDClient = new TCPClient(_ServerIP, _PortNum, $"Port[{PortID}] RFID Client");
            m_RFIDClient.ConnectEvent += ClientConnectEvent;
            m_RFIDClient.ReceiveEvent += ReceiveData;
            m_RFIDClient.Connect();
        }
        public void Close()
        {
            if (m_RFIDClient != null)
            {
                m_RFIDClient.Close();
                m_RFIDClient.ReceiveEvent -= ReceiveData;
                m_RFIDClient.ConnectEvent -= ClientConnectEvent;
            }
        }
        private void ClientConnectEvent(bool bConnect)
        {
            if (bConnect)
            {
                RFIDClearPacketBuffer();
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPConnection, $"RFID Server IP:{m_RFIDClient.m_ServerIP} / Server Port:{m_RFIDClient.m_ServerPort}");
            }
            else
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPDisconnection, $"RFID Server IP:{m_RFIDClient.m_ServerIP} / Server Port:{m_RFIDClient.m_ServerPort}");
            }
        }
        private void RFIDClearPacketBuffer()
        {
            m_CurrentPacketLen = 0;
            Array.Clear(m_PacketBuffer, 0, m_PacketBuffer.Length);
        }

        public bool IsConnected()
        {
            return m_RFIDClient.IsConnected();
        }

        public bool IsBusy()
        {
            return m_bSendState;
        }
        public bool IsTagReadSuccess()
        {
            return m_bTagReadSuccess;
        }
        public string GetTag()
        {
            return Tag;
        }
        public void TagClear()
        {
            Tag = string.Empty;
        }
        public string GetErrorStr()
        {
            return ErrorStr;
        }
        public string GetOperationMode()
        {
            return Convert.ToString(eReaderOperationMode);
        }
        public string GetRequestResult()
        {
            return Convert.ToString(eRequestResult);
        }
        public void SetModel(Model eModel)
        {
            m_eModel = eModel;
        }
        public void SetTagReadSize(byte size)
        {
            TagReadSize = size;
        }
        public void SendData(byte[] bytes)
        {
            if (m_RFIDClient.IsConnected())
            {
                if(IsBusy())
                {
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"RFID Send Data Error");
                    return;
                }

                ErrorStr = $"{ErrorCode.None}";
                eRequestResult = RequestResult.None;
                m_bSendState = true;

                if (!TimeoutSt.IsRunning)
                {
                    TimeoutSt.Restart();
                }

                m_RFIDClient.Send(bytes);
            }
            else
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotConnection, $"RFID Send Array: {BitConverter.ToString(bytes)}");
        }
        public void TagRead(ANT eANT, byte ReadSize)
        {
            byte StartPoint = 0x00;
            byte ANT = (byte)(0x80 + (byte)eANT);
            short CheckSum = (short)(0x05 + 0x01 + ANT + StartPoint + ReadSize);
            byte[] CS = BitConverter.GetBytes(CheckSum);

            SendData(new byte[] { 0x05, 0x01, ANT, StartPoint, ReadSize, CS[0] });
        }

        public void BusyCheckAndRead(ANT eANT, byte ReadSize)
        {
            while (true)
            {
                if (!IsBusy() && !m_bTagReadSuccess)
                {
                    TagRead(eANT, ReadSize);
                    break;
                }
                else if (m_bTagReadSuccess)
                    break;

                Thread.Sleep(1);
            }
        }
        public void TagRead()
        {
            if(IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"RFID Set Tag Read Error");
                return;
            }

            if(!IsConnected())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPDisconnection, $"RFID TCP/IP is Disconnection");
                return;
            }

            if(!Master.m_Ports.ContainsKey(PortID))
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDPortIDError, $"RFID Port ID Error");
                return;
            }

            if (m_eModel == Model.CH2)
            {
                TagClear();
                m_bTagReadSuccess = false;
                BusyCheckAndRead(ANT.ANT1, TagReadSize);
                BusyCheckAndRead(ANT.ANT2, TagReadSize);

            }
            else if (m_eModel == Model.CH4)
            {
                TagClear();
                m_bTagReadSuccess = false;
                BusyCheckAndRead(ANT.ANT1, TagReadSize);
                BusyCheckAndRead(ANT.ANT2, TagReadSize);
                BusyCheckAndRead(ANT.ANT3, TagReadSize);
                BusyCheckAndRead(ANT.ANT4, TagReadSize);
            }

            while (IsBusy() && IsConnected())
            {
                Thread.Sleep(1);

                if(Master.m_Ports.ContainsKey(PortID))
                {
                    if (Master.m_Ports[PortID].GetAlarmLevel() == Interface.Alarm.AlarmLevel.Error)
                        break;
                }

                if (TimeoutSt.Elapsed.TotalSeconds > 10)
                {
                    TimeoutSt.Stop();
                    TimeoutSt.Reset();
                    m_bSendState = false;
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDReadTimeOut, $"RFID Read Time out");
                    break;
                }
            }
        }
        public void AutoRead()
        {
            if (IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"RFID Set Auto Read Error");
                return;
            }

            SendData(new byte[] { 0x05, 0x01, 0x18, 0x0b, 0x01, 0xdc, 0x06 });
        }
        public void VerboseMode()
        {
            if (IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"RFID Set Verbose Mode Error");
                return;
            }

            SendData(new byte[] { 0x05, 0x01, 0x18, 0x0b, 0x01, 0xde, 0x08 });
        }
        public void VerboseTimeOutSet(byte msec_100)
        {
            if (IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"RFID Set Verbose Time out Error");
                return;
            }

            short CheckSum = (short)(0x05 + 0x01 + 0x18 + 0x1d + 0x01 + msec_100);
            byte[] CS = BitConverter.GetBytes(CheckSum);

            SendData(new byte[] { 0x05, 0x01, 0x18, 0x1d, 0x01, msec_100, CS[0] });
        }
        public void OnlyANT1Enable()
        {
            if (IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"RFID Set ANT1 Enable Error");
                return;
            }

            SendData(new byte[] { 0x05, 0x01, 0x18, 0x03, 0x01, 0x01, 0x23 });
        }
        public void OnlyANT2Enable()
        {
            if (IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"RFID Set ANT2 Enable Error");
                return;
            }

            SendData(new byte[] { 0x05, 0x01, 0x18, 0x03, 0x01, 0x02, 0x24 });
        }
        public void ANT1ANT2Enable()
        {
            if (IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"RFID Set ANT1, ANT2 Enable Error");
                return;
            }

            SendData(new byte[] { 0x05, 0x01, 0x18, 0x03, 0x01, 0x03, 0x25 });
        }
        public void SetRTA(byte StartAddress)
        {
            if (IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"RFID Set RTA Error");
                return;
            }

            short CheckSum = (short)(0x05 + 0x01 + 0x18 + 0x1c + 0x01 + StartAddress);
            byte[] CS = BitConverter.GetBytes(CheckSum);

            SendData(new byte[] { 0x05, 0x01, 0x18, 0x1c, 0x01, StartAddress, CS[0] });
        }
        public void SetRTB(byte Size)
        {
            if (IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"RFID Set RTB Error");
                return;
            }

            short CheckSum = (short)(0x05 + 0x01 + 0x18 + 0x1b + 0x01 + Size);
            byte[] CS = BitConverter.GetBytes(CheckSum);

            SendData(new byte[] { 0x05, 0x01, 0x18, 0x1b, 0x01, Size, CS[0] });
        }
        public void Save()
        {
            if (IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"RFID Save Error");
                return;
            }

            SendData(new byte[] { 0x05, 0x01, 0x18, 0x18, 0x01, 0x20, 0x57 });
        }
        public void SystemRegisterRead()
        {
            if (IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"RFID System Register Read Error");
                return;
            }

            SendData(new byte[] { 0x05, 0x01, 0x08, 0x0b, 0x01, 0x1a });
        }

        public void ReceiveData(int MsgNum, byte[] bytes)
        {
            try
            {
                try
                {
                    if (m_CurrentPacketLen + bytes.Length > m_PacketBuffer.Length)
                    {
                        LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPPacketOverFlow, $"Buffer Length: {m_CurrentPacketLen} + Read Length: {bytes.Length} > {m_PacketBuffer.Length}");
                        RFIDClearPacketBuffer();
                    }
                    else
                    {
                        Array.Copy(bytes, 0, m_PacketBuffer, m_CurrentPacketLen, bytes.Length);
                        m_CurrentPacketLen += bytes.Length;
                    }
                }
                catch (Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port[{PortID}] RFID Receive");
                }

                for (int nPos = 0; nPos < m_CurrentPacketLen; nPos++)
                {
                    if (m_PacketBuffer[nPos] == 0x02)
                    {
                        int StartByteAddr = nPos;
                        //Normally Response of Read Command
                        if (m_PacketBuffer.Length >= 4)
                        {
                            for (; nPos < m_PacketBuffer.Length; nPos++)
                            {
                                if (m_PacketBuffer[nPos] == 0x03)
                                {
                                    int EndByteAddr = nPos;

                                    if (EndByteAddr - StartByteAddr < 0)
                                        break;

                                    int PacketLength = EndByteAddr - StartByteAddr + 1;
                                    byte[] receivePackets = new byte[PacketLength];
                                    Array.Copy(m_PacketBuffer, StartByteAddr, receivePackets, 0, receivePackets.Length);
                                    if (receivePackets[2] == 0x08)
                                    {
                                        eRequestResult = RequestResult.ResponseofReadCommand;
                                        //Operation Mode
                                        if (receivePackets[3] == 0xde)
                                            eReaderOperationMode = ReaderOperationMode.Verbose;
                                        else if (receivePackets[3] == 0xdc)
                                            eReaderOperationMode = ReaderOperationMode.AutoRead;
                                        else
                                            eReaderOperationMode = ReaderOperationMode.None;
                                    }

                                    else if (((receivePackets[2] == 0xa0 || receivePackets[2] == 0x80)) ||
                                            ((receivePackets[2] == 0xa1 || receivePackets[2] == 0x81)) ||
                                            ((receivePackets[2] == 0xa2 || receivePackets[2] == 0x82)) ||
                                            ((receivePackets[2] == 0xa3 || receivePackets[2] == 0x83)))
                                    {
                                        eRequestResult = RequestResult.ResponseofReadCommand;
                                        //Tag Read
                                        if (PacketLength - 4 > 0)
                                        {
                                            m_bTagReadSuccess = true;
                                            byte[] TagData = new byte[PacketLength - 4];
                                            Array.Copy(receivePackets, 3, TagData, 0, TagData.Length);
                                            Tag = (string)Encoding.Default.GetString(TagData).Trim('\0');
                                        }
                                        else
                                        {
                                            Tag = string.Empty;
                                        }
                                        LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RFIDInfo, $"Tag Read Result : {IsTagReadSuccess()}, {Tag}");
                                    }

                                    m_CurrentPacketLen -= (StartByteAddr + PacketLength);
                                    nPos = -1;
                                    if (m_CurrentPacketLen > 0)
                                        Array.Copy(m_PacketBuffer, (StartByteAddr + PacketLength), m_PacketBuffer, 0, m_CurrentPacketLen);
                                    else
                                        RFIDClearPacketBuffer();
                                    break;
                                }
                            }
                        }
                    }
                    else if (m_PacketBuffer[nPos] == 0x06)
                    {
                        int StartByteAddr = nPos;
                        //Normally Response of Write Command
                        if (m_PacketBuffer.Length >= 4)
                        {
                            if (m_PacketBuffer[3] == 0x03)
                            {
                                eRequestResult = RequestResult.ResponseofWriteCommand;
                                int PacketLength = 4;
                                byte[] receivePackets = new byte[PacketLength];
                                Array.Copy(m_PacketBuffer, StartByteAddr, receivePackets, 0, receivePackets.Length);

                                m_CurrentPacketLen -= (StartByteAddr + PacketLength);
                                nPos = -1;
                                if (m_CurrentPacketLen > 0)
                                    Array.Copy(m_PacketBuffer, (StartByteAddr + PacketLength), m_PacketBuffer, 0, m_CurrentPacketLen);
                                else
                                    RFIDClearPacketBuffer();
                            }
                        }
                    }
                    else if (m_PacketBuffer[nPos] == 0x15)
                    {
                        int StartByteAddr = nPos;
                        //Abnormal Response
                        if (m_PacketBuffer.Length >= 5)
                        {
                            if (m_PacketBuffer[4] == 0x03)
                            {
                                eRequestResult = RequestResult.AbnormalCommand;
                                int PacketLength = 5;
                                byte[] receivePackets = new byte[PacketLength];
                                Array.Copy(m_PacketBuffer, StartByteAddr, receivePackets, 0, receivePackets.Length);

                                ErrorStr = $"{(ErrorCode)receivePackets[3]}";
                                m_CurrentPacketLen -= (StartByteAddr + PacketLength);
                                nPos = -1;
                                if (m_CurrentPacketLen > 0)
                                    Array.Copy(m_PacketBuffer, (StartByteAddr + PacketLength), m_PacketBuffer, 0, m_CurrentPacketLen);
                                else
                                    RFIDClearPacketBuffer();
                            }
                        }
                    }
                    else
                    {
                        if (nPos == m_PacketBuffer.Length - 1)
                        {
                            //종점까지 왔지만 유효 패킷이 없는 경우
                            LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPInvalidPacket, $"Invalid Packet Clear: {BitConverter.ToString(bytes)}");
                            RFIDClearPacketBuffer();
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {
                TimeoutSt.Stop();
                TimeoutSt.Reset();
                m_bSendState = false;
            }
        }
    }
}
