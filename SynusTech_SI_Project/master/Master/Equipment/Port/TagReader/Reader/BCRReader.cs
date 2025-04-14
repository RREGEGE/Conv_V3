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
    /// BCR Reading 관련 클래스 및 기능
    /// 제공 받은 문서 토대로 만들어진 기능
    /// </summary>
    public class BCRReader
    {
        public enum SendType
        {
            None,
            AutoSettings,
            AlignMode,
            TeachIN,
            Read,
            Status
        }

        public enum CACommandResponse
        {
            Valid = 0,
            Invalid = 1,
            DoNotEnable = 2,
            None
        }

        public enum RSResponse
        {
            Valid,
            InvalidCommand,
            InvalidSaveLocation,
            CodeNotSave,
            CodeIsInvalid,
            None
        }

        public enum CodeTypeIndex
        {
            InterLeave = 1,
            Code39 = 2,
            UPC = 6,
            EAN = 7,
            Code128EAN128 = 8,
            Phamacode = 9,
            EANAppendix = 10,
            Codabar = 11,
            Code93 = 12,
            GS1_DataBar_AllDirection = 13,
            GS1_DataBar_Limited = 14,
            GS1_DataBar_Expanded = 15,
            GS1_Databar_Truncated = 20,
            DataMatrixECC200 = 32,
            QRCode = 33,
            Aztec = 34,
            PDF417 = 48,
            GS1_DataBar_Stacked = 52,
            GS1_DataBar_Stacked_Omni = 53,
            GS1_DataBar_Stacked_Expanded = 54
        }
        public enum TestReadyState
        {
            None,
            Ready,
            Error
        }

        public enum RunningMode
        {
            None,
            ProcessMode,
            ServiceMode
        }

        public enum EquipmentState
        {
            None,
            Error,
            Idle
        }

        TCPClient m_BCRClient;
        byte[] m_PacketBuffer = new byte[409600];
        int m_CurrentPacketLen = 0;
        string PortID;
        SendType eSendType = SendType.None;
        CACommandResponse eCACommandResponse = CACommandResponse.None;
        RSResponse eRSResponse = RSResponse.None;

        TestReadyState eTestReadyState  = TestReadyState.None;
        RunningMode eRunningMode        = RunningMode.None;
        EquipmentState eEquipmentState  = EquipmentState.None;

        string m_ReadingQuality = string.Empty;
        string m_CodeInfo = string.Empty;

        string m_CodeType = string.Empty;
        int m_CodeLength = 0;

        public int n_BCRReadCount = 0;
        bool m_bSendState = false;

        Stopwatch TimeoutSt = new Stopwatch();

        public BCRReader(string _PortID, string _ServerIP, int _PortNum)
        {
            PortID = _PortID;

            m_BCRClient = new TCPClient(_ServerIP, _PortNum, $"Port[{PortID}] BCR Client");
            m_BCRClient.ConnectEvent += ClientConnectEvent;
            m_BCRClient.ReceiveEvent += ReceiveData;
            m_BCRClient.Connect();
        }

        public string GetReadingQuality()
        {
            return m_ReadingQuality;
        }

        public string GetCodeInfo()
        {
            return m_CodeInfo;
        }
        public string GetCodeType()
        {
            int nCodeTypeNum = 0;

            try
            {
                if(!string.IsNullOrEmpty(m_CodeType))
                    nCodeTypeNum = Convert.ToInt32(m_CodeType);
            }
            catch
            {
                nCodeTypeNum = 0;
            }
            string CodeType = string.Empty;
            if (Enum.IsDefined(typeof(CodeTypeIndex), nCodeTypeNum))
            {
                CodeType = $"{(CodeTypeIndex)nCodeTypeNum}";
            }
            else
                CodeType = "None";

            return CodeType;
        }

        public string GetCodeLength()
        {
            return m_CodeLength.ToString();
        }

        public CACommandResponse GetCAResponse()
        {
            return eCACommandResponse;
        }

        public RSResponse GetRSResponse()
        {
            return eRSResponse;
        }

        public TestReadyState GetTestReadyState()
        {
            return eTestReadyState;
        }
        public RunningMode GetRunningMode()
        {
            return eRunningMode;
        }
        public EquipmentState GetEquipmentState()
        {
            return eEquipmentState;
        }

        public string GetReadyState()
        {
            return $"{eRSResponse}";
        }

        public bool IsTagReadSuccess()
        {
            return GetCodeInfo() != string.Empty;
        }

        public void Close()
        {
            if (m_BCRClient != null)
            {
                m_BCRClient.Close();
                m_BCRClient.ReceiveEvent -= ReceiveData;
                m_BCRClient.ConnectEvent -= ClientConnectEvent;
            }
        }
        private void ClientConnectEvent(bool bConnect)
        {
            if (bConnect)
            {
                BCRClearPacketBuffer();
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPConnection, $"BCR Server IP:{m_BCRClient.m_ServerIP} / Server Port:{m_BCRClient.m_ServerPort}");
            }
            else
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPDisconnection, $"BCR Server IP:{m_BCRClient.m_ServerIP} / Server Port:{m_BCRClient.m_ServerPort}");
            }
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
                        BCRClearPacketBuffer();
                    }
                    else
                    {
                        Array.Copy(bytes, 0, m_PacketBuffer, m_CurrentPacketLen, bytes.Length);
                        m_CurrentPacketLen += bytes.Length;
                    }
                }
                catch (Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port[{PortID}] BCR Receive");
                }

                if(eSendType == SendType.AlignMode)
                {
                    if(m_CurrentPacketLen >= 3)
                    {
                        byte[] ReadingQualityDt = new byte[3];
                        Buffer.BlockCopy(m_PacketBuffer, 0, ReadingQualityDt, 0, 3);
                        string Quality =  ((string)Encoding.Default.GetString(ReadingQualityDt).Trim('\0')).Replace(" ", string.Empty);
                        m_ReadingQuality = Quality;

                        if(m_CurrentPacketLen - 3 > 0)
                        {
                            byte[] CodeInfoDt = new byte[m_CurrentPacketLen - 3];
                            Buffer.BlockCopy(m_PacketBuffer, 3, CodeInfoDt, 0, CodeInfoDt.Length);
                            string CodeInfo = ((string)Encoding.Default.GetString(CodeInfoDt).Trim('\0')).Replace(" ", string.Empty);
                            m_CodeInfo = CodeInfo;
                        }

                        BCRClearPacketBuffer();
                    }
                }
                else if(eSendType == SendType.AutoSettings)
                {
                    if(m_CurrentPacketLen >= 5 && eCACommandResponse == CACommandResponse.None)
                    {
                        byte[] ResponseDt = new byte[5];
                        Buffer.BlockCopy(m_PacketBuffer, 0, ResponseDt, 0, 5);
                        string Response = ((string)Encoding.Default.GetString(ResponseDt).Trim('\0')).Replace(" ", string.Empty);

                        if (Response == "CS=00")
                            eCACommandResponse = CACommandResponse.Valid;
                        else if(Response == "CS=02")
                            eCACommandResponse = CACommandResponse.DoNotEnable;
                        else
                            eCACommandResponse = CACommandResponse.Invalid;

                        m_CurrentPacketLen -= (5);
                        if (m_CurrentPacketLen > 0)
                            Array.Copy(m_PacketBuffer, 5, m_PacketBuffer, 0, m_CurrentPacketLen);
                        else
                            BCRClearPacketBuffer();
                    }
                    else if(eCACommandResponse == CACommandResponse.Valid)
                    {
                        byte[] CodeTypeDt = new byte[2];
                        Buffer.BlockCopy(m_PacketBuffer, 0, CodeTypeDt, 0, 2);
                        string CodeType = ((string)Encoding.Default.GetString(CodeTypeDt).Trim('\0')).Replace(" ", string.Empty);

                        m_CodeType = CodeType;

                        byte[] CodeLengthDt = new byte[4];
                        Buffer.BlockCopy(m_PacketBuffer, 3, CodeLengthDt, 0, 4);
                        string CodeLength = ((string)Encoding.Default.GetString(CodeLengthDt).Trim('\0')).Replace(" ", string.Empty);

                        try
                        {
                            m_CodeLength = Convert.ToInt32(CodeLength);
                        }
                        catch
                        {
                            m_CodeLength = 0;
                        }

                        byte[] CodeInfoDt = new byte[m_CurrentPacketLen - 8];
                        Buffer.BlockCopy(m_PacketBuffer, 8, CodeInfoDt, 0, CodeInfoDt.Length);
                        string CodeInfo = ((string)Encoding.Default.GetString(CodeInfoDt).Trim('\0')).Replace(" ", string.Empty);
                        m_CodeInfo = CodeInfo;
                    }
                    else
                    {
                        BCRClearPacketBuffer();
                    }
                }
                else if (eSendType == SendType.TeachIN || eSendType == SendType.Read)
                {
                    int nPos = 0;

                    while(nPos < m_CurrentPacketLen)
                    {
                        if (nPos >= m_PacketBuffer.Length || nPos >= m_CurrentPacketLen)
                        {
                            BCRClearPacketBuffer();
                            return;
                        }

                        if (m_PacketBuffer[nPos] == 'R')
                        {
                            if (m_PacketBuffer[nPos + 1] == 'S')
                            {
                                byte[] ResponseDt = new byte[2];
                                Buffer.BlockCopy(m_PacketBuffer, nPos+3, ResponseDt, 0, 2);
                                string Response = ((string)Encoding.Default.GetString(ResponseDt).Trim('\0')).Replace(" ", string.Empty);

                                if (Response == "00")
                                    eRSResponse = RSResponse.Valid;
                                else if (Response == "01")
                                    eRSResponse = RSResponse.InvalidCommand;
                                else if (Response == "02")
                                    eRSResponse = RSResponse.InvalidSaveLocation;
                                else if (Response == "03")
                                    eRSResponse = RSResponse.CodeNotSave;
                                else if (Response == "04")
                                    eRSResponse = RSResponse.CodeIsInvalid;

                                m_CurrentPacketLen -= (nPos + 5);
                                if (m_CurrentPacketLen > 0)
                                    Array.Copy(m_PacketBuffer, nPos + 5, m_PacketBuffer, 0, m_CurrentPacketLen);
                                else
                                    BCRClearPacketBuffer();

                                nPos = 0;
                            }
                            else if (m_PacketBuffer[nPos + 1] == 'C')
                            {
                                byte[] CodeTypeDt = new byte[2];
                                Buffer.BlockCopy(m_PacketBuffer, nPos + 4 , CodeTypeDt, 0, 2);
                                string CodeType = ((string)Encoding.Default.GetString(CodeTypeDt).Trim('\0')).Replace(" ", string.Empty);
                                
                                if(eSendType == SendType.TeachIN)
                                    m_CodeType = CodeType;

                                if (m_CurrentPacketLen - (nPos + 6) > 0)
                                {
                                    byte[] CodeInfoDt = new byte[m_CurrentPacketLen - (nPos + 6)];
                                    Buffer.BlockCopy(m_PacketBuffer, nPos + 6, CodeInfoDt, 0, CodeInfoDt.Length);
                                    string CodeInfo = ((string)Encoding.Default.GetString(CodeInfoDt).Trim('\0')).Replace(" ", string.Empty);
                                    m_CodeInfo = CodeInfo;
                                }
                                else
                                    m_CodeInfo = string.Empty;

                                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RFIDInfo, $"Tag Read Result : {IsTagReadSuccess()}, {m_CodeInfo}");
                                BCRClearPacketBuffer();
                            }
                        }
                        else
                            nPos++;
                    }
                }
                else if (eSendType == SendType.Status)
                {
                    if (m_CurrentPacketLen >= 12)
                    {
                        byte[] CMDDt = new byte[4];
                        Buffer.BlockCopy(m_PacketBuffer, 0, CMDDt, 0, 4);
                        string CMD = ((string)Encoding.Default.GetString(CMDDt).Trim('\0')).Replace(" ", string.Empty);
                        if(CMD == "SST=")
                        {
                            eTestReadyState = m_PacketBuffer[11] == '1' ? TestReadyState.Ready : TestReadyState.Error;
                            eRunningMode = m_PacketBuffer[10] == '1' ? RunningMode.ProcessMode : RunningMode.ServiceMode;
                            eEquipmentState = m_PacketBuffer[9] == '0' ? EquipmentState.Idle : EquipmentState.Error;
                        }
                        else
                        {
                            eTestReadyState = TestReadyState.Error;
                            eRunningMode = RunningMode.None;
                            eEquipmentState = EquipmentState.Error;
                        }
                    }
                    BCRClearPacketBuffer();
                }
                else
                    BCRClearPacketBuffer();
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

        public bool IsBusy()
        {
            return m_bSendState;
        }

        public void TagRead()
        {
            if (IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.BCRBusy, $"BCR Set Tag Read Error");
                return;
            }

            if (!IsConnected())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPDisconnection, $"BCR TCP/IP is Disconnection");
                return;
            }

            if(!Master.m_Ports.ContainsKey(PortID))
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.BCRPortIDError, $"BCR Port ID Error");
                return;
            }

            TeachIn();
            //m_CodeInfo = string.Empty;
            //eSendType = SendType.Read;
            //byte[] CMD = Encoding.UTF8.GetBytes("RR");
            //IEnumerable<byte> send = CMD;
            //SendData(send.ToArray());

            while (IsBusy() && IsConnected())
            {
                Thread.Sleep(1);

                if (Master.m_Ports.ContainsKey(PortID))
                {
                    if (Master.m_Ports[PortID].GetAlarmLevel() == Interface.Alarm.AlarmLevel.Error)
                        break;
                }

                if (TimeoutSt.Elapsed.TotalSeconds > 10)
                {
                    TimeoutSt.Stop();
                    TimeoutSt.Reset();
                    m_bSendState = false;
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.BCRReadTimeOut, $"BCR Read Time out");
                    break;
                }
            }
        }

        public void AutoSettings()
        {
            eCACommandResponse = CACommandResponse.None;
            eSendType = SendType.AutoSettings;
            byte[] CMD = Encoding.UTF8.GetBytes("CA+");

            IEnumerable<byte> send = CMD;
            SendData(send.ToArray());
        }

        public void AlignOn()
        {
            eSendType = SendType.AlignMode;
            byte[] CMD = Encoding.UTF8.GetBytes("JP+");

            IEnumerable<byte> send = CMD;
            SendData(send.ToArray());
        }
        public void AlignOff()
        {
            eSendType = SendType.None;
            byte[] CMD = Encoding.UTF8.GetBytes("JP-");

            IEnumerable<byte> send = CMD;
            SendData(send.ToArray());
        }
        public void TeachIn()
        {
            m_CodeInfo = string.Empty;
            eRSResponse = RSResponse.None;
            eSendType = SendType.TeachIN;
            byte[] CMD = Encoding.UTF8.GetBytes("RT1");

            IEnumerable<byte> send = CMD;
            SendData(send.ToArray());
        }
        public void GetStatus()
        {
            eTestReadyState = TestReadyState.None;
            eRunningMode = RunningMode.None;
            eEquipmentState = EquipmentState.None;
            eSendType = SendType.Status;
            byte[] CMD = Encoding.UTF8.GetBytes("SST?");

            IEnumerable<byte> send = CMD;
            SendData(send.ToArray());
        }

        private void BCRClearPacketBuffer()
        {
            m_CurrentPacketLen = 0;
            Array.Clear(m_PacketBuffer, 0, m_PacketBuffer.Length);
        }

        public void SendData(byte[] bytes)
        {
            if (m_BCRClient.IsConnected())
            {
                m_bSendState = true;
                if (!TimeoutSt.IsRunning)
                {
                    TimeoutSt.Restart();
                }
                m_BCRClient.Send(bytes);
            }
            else
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotConnection, $"BCR Send Array: {BitConverter.ToString(bytes)}");
        }

        public bool IsConnected()
        {
            return m_BCRClient.IsConnected();
        }
    }
}
