using System;
using System.IO;
using MYWMX3API.Classes;
using WMX3ApiCLR;
using WMX3ApiCLR.EcApiCLR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MYWMX3API
{
    partial class WMXLib
    {
        static public class Controls
        {
            static public class Engine
            {
                static public void Start()
                {
                    int ret = WMX3Api.StartEngine(Datas.Engine.WMXInstallDirectory);
                    if(ret != ErrorCode.None)
                        FormControls.RichTextBox_Text_Handler(ref Global.gLogTextBox, $"{WMX3Api.ErrorToString(ret)}, ErrorCode :{ret}");
                }
                static public void Stop()
                {
                    int ret = WMX3Api.StopEngine();
                    if (ret != ErrorCode.None)
                        FormControls.RichTextBox_Text_Handler(ref Global.gLogTextBox, $"{WMX3Api.ErrorToString(ret)}, ErrorCode :{ret}");
                }
                static public void Restart()
                {
                    int ret = WMX3Api.RestartEngine(Datas.Engine.WMXInstallDirectory);
                    if (ret != ErrorCode.None)
                        FormControls.RichTextBox_Text_Handler(ref Global.gLogTextBox, $"{WMX3Api.ErrorToString(ret)}, ErrorCode :{ret}");
                }
                static public void StartComm()
                {
                    if (!WMX3Api.IsDeviceValid())
                        return;

                    int ret = WMX3Api.StartCommunication();
                    if (ret != ErrorCode.None)
                        FormControls.RichTextBox_Text_Handler(ref Global.gLogTextBox, $"{WMX3Api.ErrorToString(ret)}, ErrorCode :{ret}");
                }
                static public void StopComm()
                {
                    if (!WMX3Api.IsDeviceValid())
                        return;

                    int ret = WMX3Api.StopCommunication();
                    if (ret != ErrorCode.None)
                        FormControls.RichTextBox_Text_Handler(ref Global.gLogTextBox, $"{WMX3Api.ErrorToString(ret)}, ErrorCode :{ret}");
                }
                static public void TryCreateDevice()
                {
                    try
                    {
                        if (WMX3Api != null)
                        {
                            if (!WMX3Api.IsDeviceValid())
                            {
                                int ret = WMX3Api.CreateDevice(Datas.Engine.WMXInstallDirectory, WMX3ApiCLR.DeviceType.DeviceTypeLowPriority);
                                if (ret != ErrorCode.None)
                                {
                                    FormControls.RichTextBox_Text_Handler(ref Global.gLogTextBox, $"{WMX3Api.ErrorToString(ret)}, ErrorCode :{ret}");
                                    return;
                                }
                                WMX3Api.SetDeviceName("WMX Slave Editor");
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            static public class EtherCAT
            {
                static public void Scan()
                {
                    if (!WMX3Api.IsDeviceValid())
                        return;

                    if (Datas.Engine.EngineStatus.State != EngineState.Communicating && Datas.Engine.EngineStatus.State == EngineState.Running)
                    {
                        for (int nMasterNum = 0; nMasterNum < Datas.EtherCAT.EcMasterInfoList.NumOfMasters; nMasterNum++)
                        {
                            int ret = WMX3Ecat.ScanNetwork(nMasterNum);
                            if (ret != ErrorCode.None)
                                FormControls.RichTextBox_Text_Handler(ref Global.gLogTextBox, $"{WMX3Api.ErrorToString(ret)}, ErrorCode :{ret}");
                        }

                    }
                }

                static public class SDO
                {
                    static public class Upload
                    {
                        static private bool IsSDOUploadBusy = false;
                        static private EcSdoUploadCallBackFunc func = EcSdoUploadCallBackFunc;
                        static public int m_result;
                        static public int m_masterId;
                        static public int m_slaveId;
                        static public int m_index;
                        static public int m_subindex;
                        static public EcSdoType m_EcSdoType;
                        static public int m_len;
                        static public byte[] m_data;
                        static public uint m_errorcode;
                        static public bool Send(int nMasterNum, int nSlaveNum, int index, int subindex, EcSdoType Type, uint waitTime)
                        {
                            bool bCallback = NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.SDOCallBackFunction] == "1" ? true : false;

                            if (bCallback)
                            {
                                if (IsSDOUploadBusy) return false;

                                Stopwatch stopwatch = new Stopwatch();
                                IsSDOUploadBusy = true;
                                stopwatch.Start();
                                WMX3Ecat.SdoUpload(nMasterNum, nSlaveNum, index, subindex, Type, func, waitTime);

                                while (IsSDOUploadBusy)
                                {
                                    if (stopwatch.ElapsedMilliseconds > waitTime)
                                    {
                                        IsSDOUploadBusy = false;
                                        return false;
                                    }
                                    Thread.Sleep(1);
                                }

                                return true;
                            }
                            else
                            {
                                int Delay = Convert.ToInt32(NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.SDOReadWriteDelay]);

                                uint length = 4096;
                                byte[] data = new byte[length];

                                m_result = WMX3Ecat.SdoUpload(nMasterNum, nSlaveNum, index, subindex, Type, data, ref length, ref m_errorcode, waitTime);
                                m_masterId = nMasterNum;
                                m_slaveId = nSlaveNum;
                                m_index = index;
                                m_subindex = subindex;
                                m_EcSdoType = Type;
                                m_data = new byte[length];
                                m_len = Convert.ToInt32(length);
                                Buffer.BlockCopy(data, 0, m_data, 0, m_len);

                                Thread.Sleep(Delay);
                                return true;
                            }
                        }

                        static private int EcSdoUploadCallBackFunc(int result, int masterId, int slaveId, int index, int subindex, EcSdoType sdoType, int len, byte[] data, uint errCode)
                        {
                            m_result = result;
                            m_masterId = masterId;
                            m_slaveId = slaveId;
                            m_index = index;
                            m_subindex = subindex;
                            m_EcSdoType = sdoType;
                            m_len = len;
                            m_data = new byte[data.Length];
                            m_data = data;
                            m_errorcode = errCode;

                            IsSDOUploadBusy = false;
                            return 0;
                        }
                    }
                    static public class Download
                    {
                        static private bool IsSDODownloadBusy = false;
                        static private EcSdoDownloadCallBackFunc func = EcSdoDownloadCallBackFunc;
                        static public int m_result;
                        static public int m_masterId;
                        static public int m_slaveId;
                        static public int m_index;
                        static public int m_subindex;
                        static public EcSdoType m_EcSdoType;
                        static public int m_len;
                        static public byte[] m_data;
                        static public uint m_errorcode;
                        static public bool Send(int nMasterNum, int nSlaveNum, int index, int subindex, byte[] data, EcSdoType Type, uint waitTime)
                        {
                            bool bCallback = NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.SDOCallBackFunction] == "1" ? true : false;

                            if (bCallback)
                            {
                                if (IsSDODownloadBusy) return false;

                                Stopwatch stopwatch = new Stopwatch();
                                IsSDODownloadBusy = true;
                                stopwatch.Start();
                                WMX3Ecat.SdoDownload(nMasterNum, nSlaveNum, index, subindex, Type, data, func, waitTime);

                                while (IsSDODownloadBusy)
                                {
                                    if (stopwatch.ElapsedMilliseconds > waitTime)
                                    {
                                        IsSDODownloadBusy = false;
                                        return false;
                                    }
                                    Thread.Sleep(1);
                                }

                                return true;
                            }
                            else
                            {
                                int Delay = Convert.ToInt32(NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.SDOReadWriteDelay]);

                                m_result = WMX3Ecat.SdoDownload(nMasterNum, nSlaveNum, index, subindex, Type, data, ref m_errorcode, waitTime);
                                m_masterId = nMasterNum;
                                m_slaveId = nSlaveNum;
                                m_index = index;
                                m_subindex = subindex;
                                m_EcSdoType = Type;
                                m_data = data;
                                m_len = data.Length;

                                Thread.Sleep(Delay);
                                return true;
                            }
                        }

                        static private int EcSdoDownloadCallBackFunc(int result, int masterId, int slaveId, int index, int subindex, EcSdoType sdoType, int len, byte[] data, uint errCode)
                        {
                            m_result = result;
                            m_masterId = masterId;
                            m_slaveId = slaveId;
                            m_index = index;
                            m_subindex = subindex;
                            m_EcSdoType = sdoType;
                            m_len = len;
                            m_data = new byte[data.Length];
                            m_data = data;
                            m_errorcode = errCode;

                            IsSDODownloadBusy = false;
                            return 0;
                        }
                    }
                }
                static public class ESC
                {
                    static public byte ESCFlag = 0;

                    static public void Clear()
                    {
                        ESCFlag = 0;
                    }
                }
                static public class Register
                {
                    public class Read
                    {
                        private bool IsRegisterReadBusy = false;
                        //public EcRegisterReadCallBackFunc func = EcRegisterReadCallBackFunc;
                        public int m_result;
                        public int m_masterId;
                        public int m_slaveId;
                        public int m_regAddr;
                        public int m_len;
                        public byte[] m_data;

                        public Read()
                        {
                            IsRegisterReadBusy = false;
                            m_result = 0;
                            m_masterId = -1;
                            m_slaveId = -1;
                            m_regAddr = -1;
                            m_len = -1;
                            m_data = new byte[0];
                        }
                        public bool Send(int nMasterNum, int nSlaveNum, int regAddr, int len, uint waitTime)
                        {
                            bool bCallback = NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.RegisterCallBackFunction] == "1" ? true : false;

                            if (bCallback)
                            {
                                Stopwatch stopwatch = new Stopwatch();
                                IsRegisterReadBusy = true;
                                stopwatch.Start();
                                EcRegisterReadCallBackFunc func = EcRegisterReadCallBackFunc;
                                m_result = Datas.EtherCAT.EcMasterInfoList.Masters[nMasterNum].Slaves[nSlaveNum].RegisterRead(regAddr, len, func, waitTime);

                                while (IsRegisterReadBusy)
                                {
                                    long Elapsed = stopwatch.ElapsedMilliseconds;
                                    if (Elapsed > waitTime)
                                    {
                                        IsRegisterReadBusy = false;
                                        return false;
                                    }
                                    Thread.Sleep(1);
                                }
                                stopwatch.Stop();

                                return true;
                            }
                            else
                            {
                                int Delay = Convert.ToInt32(NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.RegisterReadWriteDelay]);

                                byte[] buff = new byte[len];
                                m_result = Datas.EtherCAT.EcMasterInfoList.Masters[nMasterNum].Slaves[nSlaveNum].RegisterRead(regAddr, buff);
                                m_masterId = nMasterNum;
                                m_slaveId = nSlaveNum;
                                m_regAddr = regAddr;
                                m_len = len;
                                m_data = new byte[len];
                                m_data = buff;

                                Thread.Sleep(Delay);
                                return true;
                            }
                        }

                        private int EcRegisterReadCallBackFunc(int result, int masterId, int slaveId, int regAddr, int len, byte[] data)
                        {
                            m_result = result;
                            m_masterId = masterId;
                            m_slaveId = slaveId;
                            m_regAddr = regAddr;
                            m_len = len;
                            m_data = new byte[data.Length];
                            m_data = data;

                            IsRegisterReadBusy = false;
                            return 0;
                        }
                    }

                    public class BroadcastRead
                    {
                        private bool IsRegisterBroadcastReadBusy = false;
                        //public EcRegisterReadCallBackFunc func = EcRegisterReadCallBackFunc;
                        public int m_result;
                        public int m_masterId;
                        public int m_regAddr;
                        public int m_len;
                        public ushort m_wkc;
                        public byte[] m_data;

                        public BroadcastRead()
                        {
                            IsRegisterBroadcastReadBusy = false;
                            m_result = 0;
                            m_masterId = -1;
                            m_regAddr = -1;
                            m_len = -1;
                            m_wkc = 0;
                            m_data = new byte[0];
                        }
                        public bool Send(int nMasterNum, int regAddr, int len, uint waitTime)
                        {
                            bool bCallback = NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.RegisterCallBackFunction] == "1" ? true : false;

                            if (bCallback)
                            {
                                Stopwatch stopwatch = new Stopwatch();
                                IsRegisterBroadcastReadBusy = true;
                                stopwatch.Start();
                                EcRegisterBroadcastReadCallBackFunc func = EcRegisterBroadcastReadCallBackFunc;
                                m_result = WMX3Ecat.RegisterBroadcastRead(nMasterNum, regAddr, len, func, waitTime);

                                while (IsRegisterBroadcastReadBusy)
                                {
                                    long Elapsed = stopwatch.ElapsedMilliseconds;
                                    if (Elapsed > waitTime)
                                    {
                                        IsRegisterBroadcastReadBusy = false;
                                        return false;
                                    }
                                    Thread.Sleep(1);
                                }
                                stopwatch.Stop();

                                return true;
                            }
                            else
                            {
                                int Delay = Convert.ToInt32(NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.RegisterReadWriteDelay]);

                                byte[] buff = new byte[len];
                                m_result = WMX3Ecat.RegisterBroadcastRead(regAddr, buff, ref m_wkc);
                                m_masterId = nMasterNum;
                                m_regAddr = regAddr;
                                m_len = len;
                                m_data = new byte[len];
                                m_data = buff;

                                Thread.Sleep(Delay);
                                return true;
                            }
                        }

                        private int EcRegisterBroadcastReadCallBackFunc(int result, int masterId, int regAddr, int len, byte[] data, ushort wkc)
                        {
                            m_result = result;
                            m_masterId = masterId;
                            m_regAddr = regAddr;
                            m_len = len;
                            m_data = new byte[data.Length];
                            m_data = data;
                            m_wkc = wkc;

                            IsRegisterBroadcastReadBusy = false;
                            return 0;
                        }
                    }

                    public class Write
                    {
                        private bool IsRegisterWriteBusy = false;
                        //public EcRegisterWriteCallBackFunc func = EcRegisterWriteCallBackFunc;
                        public int m_result;
                        public int m_masterId;
                        public int m_slaveId;
                        public int m_regAddr;
                        public int m_len;
                        public byte[] m_data;
                        public bool Send(int nMasterNum, int nSlaveNum, int regAddr, byte[] data, uint waitTime)
                        {
                            bool bCallback = NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.RegisterCallBackFunction] == "1" ? true : false;

                            if (bCallback)
                            {
                                Stopwatch stopwatch = new Stopwatch();
                                IsRegisterWriteBusy = true;
                                stopwatch.Start();
                                EcRegisterWriteCallBackFunc func = EcRegisterWriteCallBackFunc;
                                m_result = Datas.EtherCAT.EcMasterInfoList.Masters[nMasterNum].Slaves[nSlaveNum].RegisterWrite(regAddr, data, func, waitTime);

                                while (IsRegisterWriteBusy)
                                {
                                    if (stopwatch.ElapsedMilliseconds > waitTime)
                                    {
                                        IsRegisterWriteBusy = false;
                                        return false;
                                    }
                                    Thread.Sleep(1);
                                }
                                stopwatch.Stop();

                                return true;
                            }
                            else
                            {
                                int Delay = Convert.ToInt32(NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.RegisterReadWriteDelay]);

                                m_result = Datas.EtherCAT.EcMasterInfoList.Masters[nMasterNum].Slaves[nSlaveNum].RegisterWrite(regAddr, data);
                                m_masterId = nMasterNum;
                                m_slaveId = nSlaveNum;
                                m_regAddr = regAddr;
                                m_len = data.Length;
                                m_data = new byte[data.Length];
                                m_data = data;

                                Thread.Sleep(Delay);
                                return true;
                            }
                        }

                        private int EcRegisterWriteCallBackFunc(int result, int masterId, int slaveId, int regAddr, int len, byte[] data)
                        {
                            m_result = result;
                            m_masterId = masterId;
                            m_slaveId = slaveId;
                            m_regAddr = regAddr;
                            m_len = len;
                            m_data = new byte[data.Length];
                            m_data = data;

                            IsRegisterWriteBusy = false;
                            return 0;
                        }
                    }

                    public class BroadcastWrite
                    {
                        private bool IsRegisterBroadcastWriteBusy = false;
                        //public EcRegisterWriteCallBackFunc func = EcRegisterWriteCallBackFunc;
                        public int m_result;
                        public int m_masterId;
                        public int m_regAddr;
                        public int m_len;
                        public ushort m_wkc;
                        public byte[] m_data;
                        public bool Send(int nMasterNum, int regAddr, byte[] data, uint waitTime)
                        {
                            bool bCallback = NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.RegisterCallBackFunction] == "1" ? true : false;

                            if (bCallback)
                            {
                                Stopwatch stopwatch = new Stopwatch();
                                IsRegisterBroadcastWriteBusy = true;
                                stopwatch.Start();
                                EcRegisterBroadcastWriteCallBackFunc func = EcRegisterBroadcastWriteCallBackFunc;
                                m_result = WMX3Ecat.RegisterBroadcastWrite(nMasterNum, regAddr, data, func, waitTime);

                                while (IsRegisterBroadcastWriteBusy)
                                {
                                    if (stopwatch.ElapsedMilliseconds > waitTime)
                                    {
                                        IsRegisterBroadcastWriteBusy = false;
                                        return false;
                                    }
                                    Thread.Sleep(1);
                                }
                                stopwatch.Stop();

                                return true;
                            }
                            else
                            {
                                int Delay = Convert.ToInt32(NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.RegisterReadWriteDelay]);

                                m_result = WMX3Ecat.RegisterBroadcastWrite(nMasterNum, regAddr, data, ref m_wkc);
                                m_masterId = nMasterNum;
                                m_regAddr = regAddr;
                                m_len = data.Length;
                                m_data = new byte[data.Length];
                                m_data = data;

                                Thread.Sleep(Delay);
                                return true;
                            }
                        }

                        private int EcRegisterBroadcastWriteCallBackFunc(int result, int masterId, int regAddr, int len, byte[] data, ushort wkc)
                        {
                            m_result = result;
                            m_masterId = masterId;
                            m_regAddr = regAddr;
                            m_len = len;
                            m_data = new byte[data.Length];
                            m_data = data;
                            m_wkc = wkc;

                            IsRegisterBroadcastWriteBusy = false;
                            return 0;
                        }
                    }
                }
                static public class SII
                {
                    static public class Read
                    {
                        static private bool IsSIIReadBusy = false;
                        static public EcSIIReadCallBackFunc func = EcSIIReadCallBackFunc;
                        static public int m_result;
                        static public int m_masterId;
                        static public int m_slaveId;
                        static public int m_siiAddr;
                        static public int m_len;
                        static public byte[] m_data;
                        static public bool Send(int nMasterNum, int nSlaveNum, int siiAddr, int len, uint waitTime)
                        {
                            bool bCallback = NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.SIICallBackFunction] == "1" ? true : false;

                            if (bCallback)
                            {
                                Stopwatch stopwatch = new Stopwatch();
                                IsSIIReadBusy = true;
                                stopwatch.Start();

                                m_result = Datas.EtherCAT.EcMasterInfoList.Masters[nMasterNum].Slaves[nSlaveNum].SIIRead(siiAddr, len, func, waitTime);

                                while (IsSIIReadBusy)
                                {
                                    long Elapsed = stopwatch.ElapsedMilliseconds;
                                    if (Elapsed > waitTime)
                                    {
                                        IsSIIReadBusy = false;
                                        return false;
                                    }
                                    Thread.Sleep(1);
                                }
                                stopwatch.Stop();

                                return true;
                            }
                            else
                            {
                                int Delay = Convert.ToInt32(NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.SIIReadWriteDelay]);

                                byte[] data = new byte[len];

                                m_result = Datas.EtherCAT.EcMasterInfoList.Masters[nMasterNum].Slaves[nSlaveNum].SIIRead(siiAddr, data);
                                m_masterId = nMasterNum;
                                m_slaveId = nSlaveNum;
                                m_siiAddr = siiAddr;
                                m_data = new byte[len];
                                m_len = len;
                                Buffer.BlockCopy(data, 0, m_data, 0, m_len);

                                Thread.Sleep(Delay);
                                return true;
                            }
                        }

                        static private int EcSIIReadCallBackFunc(int result, int masterId, int slaveId, int siiAddr, int len, byte[] data)
                        {
                            m_result = result;
                            m_masterId = masterId;
                            m_slaveId = slaveId;
                            m_siiAddr = siiAddr;
                            m_len = len;
                            m_data = new byte[data.Length];
                            m_data = data;

                            IsSIIReadBusy = false;
                            return 0;
                        }
                    }
                    static public class Write
                    {
                        static private bool IsSIIWriteBusy = false;
                        static public EcSIIWriteCallBackFunc func = EcSIIWriteCallBackFunc;
                        static public int m_result;
                        static public int m_masterId;
                        static public int m_slaveId;
                        static public int m_siiAddr;
                        static public int m_len;
                        static public byte[] m_data;
                        static public bool Send(int nMasterNum, int nSlaveNum, int siiAddr, byte[] data, uint waitTime)
                        {
                            bool bCallback = NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.SIICallBackFunction] == "1" ? true : false;

                            if (bCallback)
                            {
                                Stopwatch stopwatch = new Stopwatch();
                                IsSIIWriteBusy = true;
                                stopwatch.Start();

                                m_result = Datas.EtherCAT.EcMasterInfoList.Masters[nMasterNum].Slaves[nSlaveNum].SIIWrite(siiAddr, data, func, waitTime);

                                while (IsSIIWriteBusy)
                                {
                                    if (stopwatch.ElapsedMilliseconds > waitTime)
                                    {
                                        IsSIIWriteBusy = false;
                                        return false;
                                    }
                                    Thread.Sleep(1);
                                }
                                stopwatch.Stop();

                                return true;
                            }
                            else
                            {
                                int Delay = Convert.ToInt32(NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.Key[(int)NetConfigurator.ManagedFile.INI.WMXSlaveEditorLoadSetup.eKeys.SIIReadWriteDelay]);

                                m_result = Datas.EtherCAT.EcMasterInfoList.Masters[nMasterNum].Slaves[nSlaveNum].SIIWrite(siiAddr, data);
                                m_masterId = nMasterNum;
                                m_slaveId = nSlaveNum;
                                m_siiAddr = siiAddr;
                                m_data = new byte[data.Length];
                                m_len = data.Length;
                                Buffer.BlockCopy(data, 0, m_data, 0, m_len);

                                Thread.Sleep(Delay);
                                return true;
                            }
                        }

                        static private int EcSIIWriteCallBackFunc(int result, int masterId, int slaveId, int siiAddr, int len, byte[] data)
                        {
                            m_result = result;
                            m_masterId = masterId;
                            m_slaveId = slaveId;
                            m_siiAddr = siiAddr;
                            m_len = len;
                            m_data = new byte[data.Length];
                            m_data = data;

                            IsSIIWriteBusy = false;
                            return 0;
                        }
                    }
                }

                static public class FoE
                {
                    static public class Read
                    {
                        static private bool IsFoEReadBusy = false;
                        static public EcFoEReadCallBackFunc func = EcFoEReadCallBackFunc;
                        static public int m_result;
                        static public int m_masterId;
                        static public int m_slaveId;
                        static public string m_filePath;
                        static public string m_fileName;
                        static public uint m_password;
                        static public uint m_errCode;
                        static public bool Send(int nMasterNum, int nSlaveNum, string FilePath, string FileName, uint Password, uint waitTime)
                        {
                            Stopwatch stopwatch = new Stopwatch();
                            IsFoEReadBusy = true;
                            stopwatch.Start();

                            m_result = Datas.EtherCAT.EcMasterInfoList.Masters[nMasterNum].Slaves[nSlaveNum].FoERead(FilePath, FileName, Password, func, waitTime);

                            while (IsFoEReadBusy)
                            {
                                long Elapsed = stopwatch.ElapsedMilliseconds;
                                if (Elapsed > waitTime)
                                {
                                    IsFoEReadBusy = false;
                                    return false;
                                }
                                Thread.Sleep(1);
                            }
                            stopwatch.Stop();

                            return true;
                        }

                        static private int EcFoEReadCallBackFunc(int result, int masterId, int slaveId, string filePath, string fileName, uint password, uint errCode)
                        {
                            m_result = result;
                            m_masterId = masterId;
                            m_slaveId = slaveId;

                            m_filePath = filePath;
                            m_fileName = fileName;
                            m_password = password;
                            m_errCode = errCode;

                            IsFoEReadBusy = false;
                            return 0;
                        }
                    }

                    static public class Write
                    {
                        static private bool IsFoEWriteBusy = false;
                        static public EcFoEWriteCallBackFunc func = EcFoEWriteCallBackFunc;
                        static public int m_result;
                        static public int m_masterId;
                        static public int m_slaveId;
                        static public string m_filePath;
                        static public string m_fileName;
                        static public uint m_password;
                        static public uint m_errCode;
                        static public bool Send(int nMasterNum, int nSlaveNum, string FilePath, string FileName, uint Password, uint waitTime)
                        {
                            Stopwatch stopwatch = new Stopwatch();
                            IsFoEWriteBusy = true;
                            stopwatch.Start();

                            m_result = Datas.EtherCAT.EcMasterInfoList.Masters[nMasterNum].Slaves[nSlaveNum].FoEWrite(FilePath, FileName, Password, func, waitTime);

                            while (IsFoEWriteBusy)
                            {
                                long Elapsed = stopwatch.ElapsedMilliseconds;
                                if (Elapsed > waitTime)
                                {
                                    IsFoEWriteBusy = false;
                                    return false;
                                }
                                Thread.Sleep(1);
                            }
                            stopwatch.Stop();

                            return true;
                        }

                        static private int EcFoEWriteCallBackFunc(int result, int masterId, int slaveId, string filePath, string fileName, uint password, uint errCode)
                        {
                            m_result = result;
                            m_masterId = masterId;
                            m_slaveId = slaveId;

                            m_filePath = filePath;
                            m_fileName = fileName;
                            m_password = password;
                            m_errCode = errCode;

                            IsFoEWriteBusy = false;
                            return 0;
                        }
                    }
                }
            }
            static public class ModuleINI
            {
                static public void DefaultModuleINICreate()
                {
                    string ModuleINIPath = Global.ModuleINIPath;

                    try
                    {
                        StreamWriter sw = new StreamWriter(ModuleINIPath);
                        if (sw != null)
                        {
                            int nNoneSectionLength = Enum.GetValues(typeof(WMXLib.Datas.ModuleINI.ModuleININoneSectionKey)).Length;

                            for (int nCount = 0; nCount < nNoneSectionLength; nCount++)
                            {
                                WMXLib.Datas.ModuleINI.ModuleININoneSectionKey eNoneSectionKey = (WMXLib.Datas.ModuleINI.ModuleININoneSectionKey)nCount;

                                switch (eNoneSectionKey)
                                {
                                    case Datas.ModuleINI.ModuleININoneSectionKey.MessageLevel:
                                        sw.WriteLine(eNoneSectionKey.ToString() + "=0");
                                        break;
                                    case Datas.ModuleINI.ModuleININoneSectionKey.PrintLog:
                                        sw.WriteLine(eNoneSectionKey.ToString() + "=0");
                                        break;
                                    case Datas.ModuleINI.ModuleININoneSectionKey.NumOfInterrupt:
                                        sw.WriteLine(eNoneSectionKey.ToString() + "=1");
                                        break;
                                    case Datas.ModuleINI.ModuleININoneSectionKey.StdOut:
                                        sw.WriteLine(eNoneSectionKey.ToString() + "=2");
                                        break;
                                    case Datas.ModuleINI.ModuleININoneSectionKey.Location:
                                        sw.WriteLine(eNoneSectionKey.ToString() + @"=.\motion\");
                                        break;
                                }
                            }
                            sw.Close();
                        }

                        WMXLib.Datas.ModuleINI.DefaultPlatformLoad();
                        WMXLib.Datas.ModuleINI.DefaultModuleLoad();
                    }
                    catch (Exception ex) { MessageBoxEx.ShowErrMessage(ex.Message); }
                }

                static public void ModuleINIReadAndSettings()
                {
                    string ModuleINIPath = Global.ModuleINIPath;

                    try
                    {
                        if (!File.Exists(ModuleINIPath))
                        {
                            DefaultModuleINICreate();
                            return;
                        }

                        File.Copy(ModuleINIPath, ModuleINIPath + ".bak", true);


                        //Read Start
                        string[] sectionlist = FileControls.INIFile.GetSectionNames(ModuleINIPath);

                        WMXLib.Datas.ModuleINI.Module.ModuleInit();
                        WMXLib.Datas.ModuleINI.Platform.PlatformInit();

                        //Module Data Load 후 삭제 (Location Name 겹치는 문제)

                        if (sectionlist.Length > 0)
                        {
                            for (int nCount = 0; nCount < sectionlist.Length; nCount++)
                            {
                                if (sectionlist[nCount].Contains("Module"))
                                {
                                    WMXLib.Datas.ModuleINI.Module.ModuleDllName[WMXLib.Datas.ModuleINI.Module.NumOfModule] = FileControls.INIFile.Read("Module " + WMXLib.Datas.ModuleINI.Module.NumOfModule.ToString(), "DllName", ModuleINIPath).ToString();
                                    WMXLib.Datas.ModuleINI.Module.ModuleLocation[WMXLib.Datas.ModuleINI.Module.NumOfModule] = FileControls.INIFile.Read("Module " + WMXLib.Datas.ModuleINI.Module.NumOfModule.ToString(), "Location", ModuleINIPath).ToString();
                                    WMXLib.Datas.ModuleINI.Module.Moduledisable[WMXLib.Datas.ModuleINI.Module.NumOfModule] = FileControls.INIFile.Read("Module " + WMXLib.Datas.ModuleINI.Module.NumOfModule.ToString(), "disable", ModuleINIPath).ToString();
                                    WMXLib.Datas.ModuleINI.Module.NumOfModule++;
                                }
                                else if (sectionlist[nCount].Contains("Platform"))
                                {
                                    WMXLib.Datas.ModuleINI.Platform.PlatformDllName[WMXLib.Datas.ModuleINI.Platform.NumOfPlatform] = FileControls.INIFile.Read("Platform " + WMXLib.Datas.ModuleINI.Platform.NumOfPlatform.ToString(), "DllName", ModuleINIPath).ToString();
                                    WMXLib.Datas.ModuleINI.Platform.PlatformLocation[WMXLib.Datas.ModuleINI.Platform.NumOfPlatform] = FileControls.INIFile.Read("Platform " + WMXLib.Datas.ModuleINI.Platform.NumOfPlatform.ToString(), "Location", ModuleINIPath).ToString();
                                    WMXLib.Datas.ModuleINI.Platform.Platformdisable[WMXLib.Datas.ModuleINI.Platform.NumOfPlatform] = FileControls.INIFile.Read("Platform " + WMXLib.Datas.ModuleINI.Platform.NumOfPlatform.ToString(), "disable", ModuleINIPath).ToString();
                                    WMXLib.Datas.ModuleINI.Platform.PlatformMasterNum[WMXLib.Datas.ModuleINI.Platform.NumOfPlatform] = FileControls.INIFile.Read("Platform " + WMXLib.Datas.ModuleINI.Platform.NumOfPlatform.ToString(), "NumOfMaster", ModuleINIPath).ToString();
                                    WMXLib.Datas.ModuleINI.Platform.NumOfPlatform++;
                                }
                                FileControls.INIFile.DelSection(sectionlist[nCount], ModuleINIPath);
                            }
                        }

                        int nNoneSectionLength = Enum.GetValues(typeof(WMXLib.Datas.ModuleINI.ModuleININoneSectionKey)).Length;
                        string[] NoneSectionData = new string[nNoneSectionLength];

                        for (int nCount = 0; nCount < nNoneSectionLength; nCount++)
                        {
                            NoneSectionData[nCount] = string.Empty;
                            WMXLib.Datas.ModuleINI.ModuleININoneSectionKey eNoneSectionKey = (WMXLib.Datas.ModuleINI.ModuleININoneSectionKey)nCount;
                            NoneSectionData[nCount] = FileControls.INIFile.ReadKeyValue(eNoneSectionKey.ToString(), ModuleINIPath);
                        }
                        //Read End

                        StreamWriter sw = new StreamWriter(ModuleINIPath);
                        if (sw != null)
                        {
                            sw.Write(string.Empty);
                            //Write Start
                            for (int nCount = 0; nCount < nNoneSectionLength; nCount++)
                            {
                                WMXLib.Datas.ModuleINI.ModuleININoneSectionKey eNoneSectionKey = (WMXLib.Datas.ModuleINI.ModuleININoneSectionKey)nCount;

                                switch (eNoneSectionKey)
                                {
                                    case Datas.ModuleINI.ModuleININoneSectionKey.MessageLevel:
                                        sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), "0"));
                                        break;
                                    case Datas.ModuleINI.ModuleININoneSectionKey.PrintLog:
                                        sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), "0"));
                                        break;
                                    case Datas.ModuleINI.ModuleININoneSectionKey.NumOfInterrupt:
                                        if (NoneSectionData[nCount] == string.Empty || NoneSectionData[nCount] == null)
                                            sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), "1"));
                                        else if (!(NoneSectionData[nCount] == "1" || NoneSectionData[nCount] == "2"))
                                            sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), "1"));
                                        else
                                            sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), NoneSectionData[nCount]));
                                        break;
                                    case Datas.ModuleINI.ModuleININoneSectionKey.InterruptDll:
                                        if (!(NoneSectionData[nCount] == string.Empty || NoneSectionData[nCount] == null))
                                            sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), NoneSectionData[nCount]));
                                        break;
                                    case Datas.ModuleINI.ModuleININoneSectionKey.ImaliveTimeout:
                                        if (!(NoneSectionData[nCount] == string.Empty || NoneSectionData[nCount] == null))
                                            sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), NoneSectionData[nCount]));
                                        break;
                                    case Datas.ModuleINI.ModuleININoneSectionKey.StdOut:
                                        sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), "2"));
                                        break;
                                    case Datas.ModuleINI.ModuleININoneSectionKey.Location:
                                        if (!(NoneSectionData[nCount] == string.Empty || NoneSectionData[nCount] == null))
                                        {
                                            if (NoneSectionData[nCount].Contains("platform"))
                                                sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), @".\motion\"));
                                            else
                                                sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), NoneSectionData[nCount]));
                                        }
                                        else
                                            sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), @".\motion\"));
                                        break;
                                    case Datas.ModuleINI.ModuleININoneSectionKey.ServoIoInputAddr:
                                        if (!(NoneSectionData[nCount] == string.Empty || NoneSectionData[nCount] == null))
                                            sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), NoneSectionData[nCount]));
                                        break;
                                    case Datas.ModuleINI.ModuleININoneSectionKey.ServoIoOutputAddr:
                                        if (!(NoneSectionData[nCount] == string.Empty || NoneSectionData[nCount] == null))
                                            sw.WriteLine(string.Format("{0}={1}", eNoneSectionKey.ToString(), NoneSectionData[nCount]));
                                        break;
                                }
                            }
                            sw.Close();
                        }


                        if (WMXLib.Datas.ModuleINI.Platform.NumOfPlatform == 0)
                        {
                            WMXLib.Datas.ModuleINI.DefaultPlatformLoad();
                        }
                        else
                        {
                            int nModuleINIPlatformNum = WMXLib.Datas.ModuleINI.Platform.NumOfPlatform;

                            for (int nCount = 0; nCount < nModuleINIPlatformNum; nCount++)
                            {
                                string DllName = WMXLib.Datas.ModuleINI.Platform.PlatformDllName[nCount];

                                if (DllName != string.Empty)
                                {
                                    if (!(WMXLib.Datas.ModuleINI.Platform.PlatformLocation[nCount] == string.Empty || WMXLib.Datas.ModuleINI.Platform.PlatformLocation[nCount] == null))
                                        FileControls.INIFile.Write("Platform " + nCount.ToString(), "Location", WMXLib.Datas.ModuleINI.Platform.PlatformLocation[nCount], ModuleINIPath);
                                    else
                                    {
                                        if (DllName == Global.ModuleINISimulationPlatformDllName)
                                        {
                                            FileControls.INIFile.Write("Platform " + nCount.ToString(), "Location", @".\platform\simu\", ModuleINIPath);
                                        }
                                        else if (DllName == Global.ModuleINIEcPlatformDllName)
                                        {
                                            FileControls.INIFile.Write("Platform " + nCount.ToString(), "Location", @".\platform\ethercat\", ModuleINIPath);
                                        }
                                        else if (DllName == Global.ModuleINICCLinkPlatformDllName)
                                        {
                                            FileControls.INIFile.Write("Platform " + nCount.ToString(), "Location", @".\platform\cclink\", ModuleINIPath);
                                        }
                                        else if (DllName == Global.ModuleINIM4PlatformDllName)
                                        {
                                            FileControls.INIFile.Write("Platform " + nCount.ToString(), "Location", @".\platform\m4\", ModuleINIPath);
                                        }
                                    }

                                    FileControls.INIFile.Write("Platform " + nCount.ToString(), "DllName", WMXLib.Datas.ModuleINI.Platform.PlatformDllName[nCount], ModuleINIPath);

                                    bool bEnable = false;
                                    if (DllName == Global.ModuleINISimulationPlatformDllName)
                                    {
                                        if (WMXLib.Datas.Engine.IsSimuInstalled())
                                            bEnable = true;
                                    }
                                    else if (DllName == Global.ModuleINIEcPlatformDllName)
                                    {
                                        if (WMXLib.Datas.Engine.IsEtherCATInstalled())
                                            bEnable = true;
                                    }
                                    else if (DllName == Global.ModuleINICCLinkPlatformDllName)
                                    {
                                        if (WMXLib.Datas.Engine.IsCCLinkTSNInstalled())
                                            bEnable = true;
                                    }
                                    else if (DllName == Global.ModuleINIM4PlatformDllName)
                                    {
                                        if (WMXLib.Datas.Engine.IsMechatrolink4Installed())
                                            bEnable = true;
                                    }

                                    if (bEnable)
                                        FileControls.INIFile.Write("Platform " + nCount.ToString(), "disable", "0", ModuleINIPath);
                                    else
                                        FileControls.INIFile.Write("Platform " + nCount.ToString(), "disable", "1", ModuleINIPath);

                                    if (!(WMXLib.Datas.ModuleINI.Platform.PlatformMasterNum[nCount] == null || WMXLib.Datas.ModuleINI.Platform.PlatformMasterNum[nCount] == string.Empty))
                                        FileControls.INIFile.Write("Platform " + nCount.ToString(), "NumOfMaster", WMXLib.Datas.ModuleINI.Platform.PlatformMasterNum[nCount], ModuleINIPath);
                                }
                                else
                                    WMXLib.Datas.ModuleINI.Platform.NumOfPlatform--;
                            }

                            //for loop 종료 후 재검사
                            if (WMXLib.Datas.ModuleINI.Platform.NumOfPlatform == 0)
                                WMXLib.Datas.ModuleINI.DefaultPlatformLoad();
                        }

                        if (WMXLib.Datas.ModuleINI.Module.NumOfModule == 0)
                        {
                            WMXLib.Datas.ModuleINI.DefaultModuleLoad();
                        }
                        else
                        {
                            int nModuleINIModuleNum = WMXLib.Datas.ModuleINI.Module.NumOfModule;
                            for (int nCount = 0; nCount < nModuleINIModuleNum; nCount++)
                            {
                                string DllName = WMXLib.Datas.ModuleINI.Module.ModuleDllName[nCount];

                                if (DllName != string.Empty)
                                {
                                    FileControls.INIFile.Write("Module " + nCount.ToString(), "DllName", WMXLib.Datas.ModuleINI.Module.ModuleDllName[nCount], ModuleINIPath);

                                    if (!(WMXLib.Datas.ModuleINI.Module.ModuleLocation[nCount] == string.Empty || WMXLib.Datas.ModuleINI.Module.ModuleLocation[nCount] == null))
                                        FileControls.INIFile.Write("Module " + nCount.ToString(), "Location", WMXLib.Datas.ModuleINI.Module.ModuleLocation[nCount], ModuleINIPath);

                                    bool bEnable = false;
                                    if (DllName == "CoreMotion")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.CoreMotion))
                                            bEnable = true;
                                    }
                                    else if (DllName == "Log")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.Log))
                                            bEnable = true;
                                    }
                                    else if (DllName == "ApiBuffer")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.ApiBuffer))
                                            bEnable = true;
                                    }
                                    else if (DllName == "CyclicBuffer")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.CyclicBuffer))
                                            bEnable = true;
                                    }
                                    else if (DllName == "CoreMotion")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.CoreMotion))
                                            bEnable = true;
                                    }
                                    else if (DllName == "IO")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.IO))
                                            bEnable = true;
                                    }
                                    else if (DllName == "Compensation")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.Compensation))
                                            bEnable = true;
                                    }
                                    else if (DllName == "Event")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.Event))
                                            bEnable = true;
                                    }
                                    else if (DllName == "AdvancedMotion")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.AdvancedMotion))
                                            bEnable = true;
                                    }
                                    else if (DllName == "UserMemory")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.UserMemory))
                                            bEnable = true;
                                    }
                                    else if (DllName == "PMMotion")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.PMMotion))
                                            bEnable = true;
                                    }
                                    else if (DllName == "Coordinate")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.Coordinate))
                                            bEnable = true;
                                    }
                                    else if (DllName == "Kinematics")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.Kinematics))
                                            bEnable = true;
                                    }
                                    else if (DllName == "SenseIT")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.SenseIT))
                                            bEnable = true;
                                    }
                                    else if (DllName == "ForceControlDll")
                                    {
                                        if (WMXLib.Datas.Engine.IsModuleInstalled(Datas.Engine.ModuleList.ForceControlDll))
                                            bEnable = true;
                                    }

                                    if (bEnable)
                                        FileControls.INIFile.Write("Module " + nCount.ToString(), "disable", "0", ModuleINIPath);
                                    else
                                        FileControls.INIFile.Write("Module " + nCount.ToString(), "disable", "1", ModuleINIPath);
                                }
                                else
                                    WMXLib.Datas.ModuleINI.Module.NumOfModule--;
                            }

                            //for loop 종료 후 재검사
                            if (WMXLib.Datas.ModuleINI.Module.NumOfModule == 0)
                                WMXLib.Datas.ModuleINI.DefaultPlatformLoad();
                        }
                    }
                    catch (Exception ex) { MessageBoxEx.ShowErrMessage(ex.Message); }
                }
            }
        }
    }
}
