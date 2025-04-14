using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.TCP;
using System.Threading;
using System.Diagnostics;
using System.Threading;

namespace Master.Equipment.Port.TagReader.ReaderEquip
{
    /// <summary>
    /// CanTops LM21 제품 관련 클래스 및 기능
    /// 제공 받은 문서 토대로 만들어진 기능
    /// </summary>
    public class CanTops_LM21
    {
        enum CMDType
        {
            Upper, //String 기반
            Lower //Hex 기반
        }
        TCPClient m_RFIDClient;

        string PortID;
        bool m_bSendState = false;
        string Tag;

        public int n_RFIDReadCount = 0;

        CMDType m_eCMDType = CMDType.Upper;
        Stopwatch TimeoutSt = new Stopwatch();

        int         RWPageIndex = -1;
        string[]    PageStr = new string[17];
        bool[]      PageRW = new bool[17];

        public CanTops_LM21(string _PortID, string _ServerIP, int _PortNum)
        {
            PortID = _PortID;

            for (int nCount = 0; nCount < PageStr.Length; nCount++)
            {
                PageStr[nCount] = string.Empty;
                PageRW[nCount] = false;
            }

            m_RFIDClient = new TCPClient(_ServerIP, _PortNum, $"Port[{PortID}] CanTops RFID Client");
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
                TagClear();
                m_bSendState = false;
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Normal, LogMsg.MsgList.TCPIPConnection, $"CanTops RFID Server IP:{m_RFIDClient.m_ServerIP} / Server Port:{m_RFIDClient.m_ServerPort}");
            }
            else
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPDisconnection, $"CanTops RFID Server IP:{m_RFIDClient.m_ServerIP} / Server Port:{m_RFIDClient.m_ServerPort}");
            }
        }
        
        public bool IsTagReadSuccess()
        {
            for (int nCount = 0; nCount < PageRW.Length; nCount++)
            {
                if (nCount < GetPageCount())
                {
                    if (!PageRW[nCount])
                        return false;
                }
            }

            return true;
        }

        public string GetTag()
        {
            string Text = string.Empty;
            for (int nCount = 0; nCount < PageStr.Length; nCount++)
            {
                Text += PageStr[nCount];
            }
            Tag = Text;
            return Tag;
        }
        
        public void TagClear()
        {
            for (int nCount = 0; nCount < PageStr.Length; nCount++)
            {
                PageStr[nCount] = string.Empty;
                PageRW[nCount] = false;
            }

            Tag = string.Empty;
        }

        
        public void TagRead(int PageIndex)
        {
            if(PageIndex < 0 || PageIndex > 17)
            {
                RWPageIndex = -1;
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"CanTops RFID Page Range Error");
                return;
            }
            byte ANT = (byte)(0x31);
            byte CMD = m_eCMDType == CMDType.Upper ? (byte)'R' : (byte)'r';
            byte Page = (byte)(0x30 + PageIndex);
            RWPageIndex = PageIndex;

            byte CheckSum = 0x00;
            CheckSum ^= ANT;
            CheckSum ^= CMD;
            CheckSum ^= Page;
            CheckSum &= 0x0f;
            CheckSum += 0x30;

            SendData(new byte[] { ANT, CMD, Page, CheckSum });
        }
        public void BusyCheckAndRead(int PageIndex)
        {
            while (true)
            {
                if (!IsBusy())
                {
                    TagRead(PageIndex);
                    break;
                }

                Thread.Sleep(1);
            }
        }
        public void TagRead()
        {
            if (IsBusy())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"CanTops RFID Set Tag Read Error");
                return;
            }

            if (!IsConnected())
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPDisconnection, $"CanTops RFID TCP/IP is Disconnection");
                return;
            }

            if (!Master.m_Ports.ContainsKey(PortID))
            {
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.CanTops_LM21_PortID_Error, $"CanTops LM21 Port ID Error");
                return;
            }

            TagClear();
            TimeoutSt.Reset();
            TimeoutSt.Start();

            Thread LocalThread = new Thread(delegate ()
            {
                for (int nPageIndex = 1; nPageIndex <= GetPageCount(); nPageIndex++)
                    BusyCheckAndRead(nPageIndex);
            });
            LocalThread.IsBackground = true;
            LocalThread.Name = "CanTops RFID Tag Read";
            LocalThread.Start();


            while(true)
            {
                if (Master.m_Ports.ContainsKey(PortID))
                {
                    if (Master.m_Ports[PortID].GetAlarmLevel() == Interface.Alarm.AlarmLevel.Error)
                        break;
                }

                if (TimeoutSt.Elapsed.Seconds > (GetPageCount() + 1) ||
                    !TimeoutSt.IsRunning ||
                    IsTagReadSuccess())
                    break;

                Thread.Sleep(10);
            }

            TimeoutSt.Stop();
        }

        public bool IsBusy()
        {
            return m_bSendState;
        }

        public void SendData(byte[] bytes)
        {
            if (m_RFIDClient.IsConnected())
            {
                if (IsBusy())
                {
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.RFIDBusy, $"CanTops RFID Send Data Error");
                    return;
                }

                m_bSendState = true;
                m_RFIDClient.Send(bytes);
            }
            else
                LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPNotConnection, $"CanTops RFID Send Array: {BitConverter.ToString(bytes)}");
        }

        public void ReceiveData(int MsgNum, byte[] bytes)
        {
            try
            {
                const byte ANT = (byte)(0x31); //ANT 고정
                byte CMD = m_eCMDType == CMDType.Upper ? (byte)'R' : (byte)'r';
                const byte CR = 0x0d; //STX
                const byte LF = 0x0a; //ETX

                if(bytes.Length == 14)
                {
                    //리드 정상 패킷
                    if (bytes[0] == ANT &&
                        bytes[12] == CR &&
                        bytes[13] == LF)
                    {
                        if (bytes[1] == (byte)'R' ||
                            bytes[1] == (byte)'r')
                        {
                            byte Status = bytes[2];

                            if(Status == (byte)'0')
                            {
                                //정상 상태 패킷
                                byte[] TagArray = new byte[8];
                                Array.Copy(bytes, 3, TagArray, 0, TagArray.Length);

                                if (RWPageIndex - 1 >= 0)
                                {
                                    PageStr[RWPageIndex - 1] = ((string)Encoding.Default.GetString(TagArray).Trim('\0')).Replace(" ", string.Empty);
                                    PageRW[RWPageIndex - 1] = true;
                                }

                                StatusLog((char)Status);

                                if (RWPageIndex == GetPageCount())
                                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RFIDInfo, $"Tag Read Result : {IsTagReadSuccess()}, {GetTag()}");
                            }
                            else
                            {
                                StatusLog((char)Status);
                            }
                        }
                    }
                }
                else
                {
                    if (bytes.Length > 3)
                    {
                        byte Status = bytes[2];
                        StatusLog((char)Status);
                    }
                }
            }
            catch
            {

            }
            finally
            {
                m_bSendState = false;
            }
        }

        private void StatusLog(char ErrorCode)
        {
            switch(ErrorCode)
            {
                case '0':
                    {
                        if (RWPageIndex - 1 >= 0)
                        {
                            LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Normal, LogMsg.MsgList.CanTops_LM21_Info, $"Read Success : Page[{RWPageIndex}], TagText -> {PageStr[RWPageIndex - 1]}");
                        }
                    }
                    break;
                case '1':
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.CanTops_LM21_Info, $"Read Fail : Page[{RWPageIndex}], Serial CS or Parity Error");
                    break;
                case '2':
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.CanTops_LM21_Info, $"Read Fail : Page[{RWPageIndex}], Serial ID or Command Error");
                    break;
                case '3':
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.CanTops_LM21_Info, $"Read Fail : Page[{RWPageIndex}], Serial Setting value Invalid Range");
                    break;
                case '4':
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.CanTops_LM21_Info, $"Write Fail : Page[{RWPageIndex}], Tag ID Write Fail");
                    break;
                case '5':
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.CanTops_LM21_Info, $"Read Fail : Page[{RWPageIndex}], Tag Identification Fail");
                    break;
                case '6':
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.CanTops_LM21_Info, $"Read Fail : Page[{RWPageIndex}], Tag type Error");
                    break;
                case '7':
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.CanTops_LM21_Info, $"Read Fail : Page[{RWPageIndex}], Tag Data CS Error");
                    break;
                case '8':
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.CanTops_LM21_Info, $"Read Fail : Page[{RWPageIndex}], Tag Communication Error(Tag is On)");
                    break;
                case '9':
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.CanTops_LM21_Info, $"Read Fail : Page[{RWPageIndex}], ANT Error");
                    break;
                default:
                    LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.CanTops_LM21_Info, $"Read Fail : Page[{RWPageIndex}], Not Define Error -> {ErrorCode}");
                    break;
            }
        }

        public bool IsConnected()
        {
            return m_RFIDClient.IsConnected();
        }
        private int GetPageCount()
        {
            int PageCount = 1;
            try
            {
                PageCount = Convert.ToInt32(ManagedFile.CassetteInfo.CanTopsReadPageCount());

                if (PageCount < 1)
                    PageCount = 1;

                if (PageCount > 17)
                    PageCount = 17;
            }
            catch
            {
                PageCount = 1;
            }

            return PageCount;
        }
    }
}
