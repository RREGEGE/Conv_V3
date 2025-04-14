using WMX3ApiCLR;
using WMX3ApiCLR.EcApiCLR;
using System.Threading;
using System;

namespace Synustech
{
    public static class WMX3
    {
        public enum EtherCATStateMachine {
            None        = EcStateMachine.None,
            Init        = EcStateMachine.Init,
            PreOp       = EcStateMachine.PreOp,
            SafeOp      = EcStateMachine.SafeOp,
            Op          = EcStateMachine.Op,
            Boot        = EcStateMachine.Boot,
        }

        public enum PlatformType {
            EtherCAT        = 0,
            Simulation      = 1,
        }

        public class SlaveInformation {
            public int id;
            public uint alias;
            public uint vendorId;
            public uint productCode;
            public int axisCount;
            public int[] axisNumber;
            public EtherCATStateMachine ESM;

            public SlaveInformation() {
                id              = 0;
                alias           = 0;
                vendorId        = 0;
                productCode     = 0;
                axisCount       = 0;
                ESM             = EtherCATStateMachine.None;
                axisNumber      = new int[WMX3ApiCLR.EcApiCLR.EcConstants.MaxSlaveAxes];
            }
        }

        private static WMX3Api m_wmx3Api;
        private static Ecat m_ecat;
        private static CoreMotion m_CoreMotion;
        private static EngineStatus m_engineStatus;
        private static EcMasterInfo m_masterInfo;
        public static Io m_wmx3io;
        private static ModulesInfo modulesInfo;
        public static CoreMotionStatus m_coreMotionStatus;
        public static WMXParam.m_engineState m_engineState;
        public static SlaveInformation m_slaveInfo;
        static public object m_IOLock = new object();
        private static Thread UpdateThread;

        public static int CreateWMX3Device(string deviceName)
        {
            m_wmx3Api = new WMX3Api();

            int err;

            if (m_wmx3Api.IsDeviceValid())
                m_wmx3Api.CloseDevice();

            try
            {
                if (UpdateThread != null)
                    if (UpdateThread.IsAlive)
                    {
                        UpdateThread.Abort();
                        UpdateThread.Join();
                        UpdateThread = null;
                    }
            }
            catch
            {

            }
            Thread.Sleep(100);

            err = m_wmx3Api.CreateDevice(@"C:\Program Files\SoftServo\WMX3", DeviceType.DeviceTypeNormal, 30000);

            if (err == WMXParam.ErrorCode_None)
                m_wmx3Api.SetDeviceName(deviceName);

            m_ecat = new Ecat(m_wmx3Api);
            m_CoreMotion = new CoreMotion(m_wmx3Api);
            m_wmx3io = new Io(m_wmx3Api);

            m_engineStatus = new EngineStatus();
            m_masterInfo = new EcMasterInfo();
            m_coreMotionStatus = new CoreMotionStatus();
            m_slaveInfo = new SlaveInformation();
            modulesInfo = new ModulesInfo();

            UpdateThread = new Thread(Update);
            UpdateThread.IsBackground = true;
            UpdateThread.Name = $"MovenCore Main Updater [{deviceName}]";
            UpdateThread.Start();

            return err;
        }

        public static bool IsUpdateThreadStarted() {
            return UpdateThread.IsAlive;
        }

        public static Io GetIOPt()
        {
            if (m_wmx3io.IsDeviceValid())
                return m_wmx3io;
            else
                return null;
        }

        private static void Update()
        {
            while (m_wmx3Api.IsDeviceValid())
            {
                m_wmx3Api.GetEngineStatus(ref m_engineStatus);
                Thread.Sleep(1);
                m_engineState = (WMXParam.m_engineState)m_engineStatus.State;

                if (m_engineStatus.State != EngineState.Shutdown &&
                    m_engineStatus.State != EngineState.Unknown)
                {
                    m_wmx3Api.GetModulesInfo(ref modulesInfo);

                    m_CoreMotion.GetStatus(ref m_coreMotionStatus);
                    Thread.Sleep(1);

                    if (modulesInfo != null)
                    {
                        foreach (var module in modulesInfo.Modules)
                        {
                            if (module.ModuleName.ToLower().Contains("ethercat") && m_ecat != null)
                            {
                                UpdateMasterInfo();
                                break;
                            }
                        }

                        Thread.Sleep(1);
                    }
                }
            }
        }

        public static int EmergencyStop()
        {
            int err = 0;
            if (!m_coreMotionStatus.EmergencyStop)
                err = m_CoreMotion.ExecEStop(EStopLevel.Final);

            return err;
        }
        public static int EmergencyStopRelease()
        {
            int err = 0;
            if (m_coreMotionStatus.EmergencyStop)
                err = m_CoreMotion.ReleaseEStop();

            return err;
        }

        public static bool IsEmergencyStop()
        {
            return m_coreMotionStatus.EmergencyStop;
        }


        public static bool IsDeviceValid()
        {
            try
            {
                if (m_wmx3Api == null)
                    return false;

                if (m_wmx3Api.IsDeviceValid())
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
        public static int StartWMX3Engine()
        {
            int err;

            err = m_wmx3Api.StartEngine(@"C:\Program Files\SoftServo\WMX3");
            return err;
        }
        public static int CloseDevice()
        {
            int err;

            err = m_wmx3Api.CloseDevice();
            return err;
        }

        public static int RestartWMX3Engine()
        {
            int err;

            err = m_wmx3Api.RestartEngine(@"C:\Program Files\SoftServo\WMX3");
            return err;
        }

        public static int StartCommunicate()
        {
            int err;
            err = m_wmx3Api.StartCommunication(1000);
            return err;
        }

        public static int StopCommunicate()
        {
            int err;
            err = m_wmx3Api.StopCommunication();
            return err;
        }

        public static WMX3Api GetWMXLib()
        {
            return m_wmx3Api;
        }

        public static bool IsEngineRunning()
        {
            if (m_engineState == WMXParam.m_engineState.Idle || m_engineState == WMXParam.m_engineState.Shutdown || m_engineState == WMXParam.m_engineState.Unknown)
                return false;
            else
                return true;
        }

        public static bool IsEngineCommunicating()
        {
            if (IsEngineRunning())
            {
                if (m_engineState == WMXParam.m_engineState.Communicating)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public static bool IsDeviceExist()
        {
            bool err;

            err = m_wmx3Api.IsDeviceValid();
            return err;
        }
        public static void UpdateMasterInfo()
        {
            m_ecat.GetMasterInfo(0, m_masterInfo);

            WMXParam.CommStatus.m_cyclePeriod = m_masterInfo.StatisticsInfo.CommPeriod;
            WMXParam.CommStatus.m_cycleCount = m_masterInfo.StatisticsInfo.CycleCounter;
            WMXParam.CommStatus.m_DCDiffAvg = m_masterInfo.StatisticsInfo.DiffFromNextDcClock;
            WMXParam.CommStatus.m_DCDiffMax = m_masterInfo.StatisticsInfo.MaxDiffFromNextDcClock;
            WMXParam.CommStatus.m_DCDiffMin = m_masterInfo.StatisticsInfo.MinDiffFromNextDcClock;
            WMXParam.CommStatus.m_TxDelayAvg = m_masterInfo.StatisticsInfo.TxDelay;
            WMXParam.CommStatus.m_TxDelayMax = m_masterInfo.StatisticsInfo.MaxTxDelay;
            WMXParam.CommStatus.m_TxDelayMin = m_masterInfo.StatisticsInfo.MinTxDelay;
            WMXParam.CommStatus.m_packetIntervalAvg = m_masterInfo.StatisticsInfo.AvgRefClockInterval;
            WMXParam.CommStatus.m_packetIntervalMax = m_masterInfo.StatisticsInfo.MaxRefClockInterval;
            WMXParam.CommStatus.m_packetIntervalMin = m_masterInfo.StatisticsInfo.MinRefClockInterval;
            WMXParam.CommStatus.m_packetLoss = m_masterInfo.StatisticsInfo.PacketLoss;
            WMXParam.CommStatus.m_packetTimeOut = m_masterInfo.StatisticsInfo.PacketTimeout;
        }

        public static SlaveInformation GetSlaveInformation(int slaveNum) {
            try {
                UpdateMasterInfo();

                m_slaveInfo.id              = m_masterInfo.Slaves[slaveNum].Id;
                m_slaveInfo.alias           = m_masterInfo.Slaves[slaveNum].Alias;
                m_slaveInfo.vendorId        = m_masterInfo.Slaves[slaveNum].VendorId;
                m_slaveInfo.productCode     = m_masterInfo.Slaves[slaveNum].ProductCode;
                m_slaveInfo.axisCount       = (int)m_masterInfo.Slaves[slaveNum].NumOfAxes;
                m_slaveInfo.ESM = (EtherCATStateMachine)m_masterInfo.Slaves[slaveNum].State;
                for (int i = 0; i < m_slaveInfo.axisCount; i++) {
                    m_slaveInfo.axisNumber[i] = m_masterInfo.Slaves[slaveNum].AxisInfo[i].AxisIndex;
                }

                return m_slaveInfo;
            }
            catch (Exception ex) {
                return null;
            }
        }

        public static bool SlaveNotOpState()
        {
            if (m_masterInfo == null)
                return false;

            if (m_engineState != WMXParam.m_engineState.Communicating)
                return false;

            for (int nCount = 0; nCount < m_masterInfo.NumOfSlaves; nCount++)
            {
                if (m_masterInfo.Slaves[nCount].State != EcStateMachine.Op)
                    return true;
            }

            return false;
        }

        public static bool AxisIsNotOpState(int nAxisNum)
        {
            if (m_masterInfo == null)
                return false;

            if (m_engineState != WMXParam.m_engineState.Communicating)
                return false;

            for (int nCount = 0; nCount < m_masterInfo.NumOfSlaves; nCount++)
            {
                for (int nAxisCount = 0; nAxisCount < m_masterInfo.Slaves[nCount].NumOfAxes; nAxisCount++)
                {
                    int AxisIndex = m_masterInfo.Slaves[nCount].AxisInfo[nAxisCount].AxisIndex;

                    if (AxisIndex == nAxisNum)
                    {
                        if (m_masterInfo.Slaves[nCount].State != EcStateMachine.Op)
                            return true;
                    }
                }
            }

            return false;
        }

        public static uint GetSlaveCount() {
            return m_masterInfo.NumOfSlaves;
        }

        public static int GetOnlineSlaveCount() {
            return m_masterInfo.GetOnlineSlaveCount();
        }

        public static int GetOfflineSlaveCount() {
            return m_masterInfo.GetOfflineSlaveCount();
        }

        public static int ScanSlave()
        {
            int ret = -1;
            ret = m_ecat.ScanNetwork();
            return ret;
        }

        public static int StartHotConnect()
        {
            int ret = -1;
            ret = m_ecat.StartHotconnect();
            return ret;
        }

        public static int SDOUploadExpedited(int nSlaveID, int nIndex, int nSubIndex, ref byte[] bDataBuff, ref uint uActualSize, ref uint uErrorCode)
        {
            int ret = -1;

            ret = m_ecat.SdoUpload(nSlaveID, nIndex, nSubIndex, EcSdoType.Expedited, bDataBuff, ref uActualSize, ref uErrorCode, 3000);
            return ret;
        }

        public static int SDOUploadNormal(int nSlaveID, int nIndex, int nSubIndex, ref string sData, ref uint uActualSize, ref uint uErrorCode)
        {
            int ret = -1;

            byte[] DataBuff = new byte[256];
            ret = m_ecat.SdoUpload(nSlaveID, nIndex, nSubIndex, EcSdoType.Normal, DataBuff, ref uActualSize, ref uErrorCode, 3000);
            sData = System.Text.Encoding.ASCII.GetString(DataBuff);
            return ret;
        }

        public static int SDODownloadExpedited(int nSlaveID, int nIndex, int nSubIndex, int nDataSize, byte[] DataBuff, ref uint uErrorCode)
        {
            int ret = -1;
            ret = m_ecat.SdoDownload(nSlaveID, nIndex, nSubIndex, EcSdoType.Expedited, DataBuff, ref uErrorCode, 3000);
            return ret;
        }

        public static int SDODownloadNormal(int nSlaveID, int nIndex, int nSubIndex, byte[] dataBuff, ref uint uErrorCode) {
            int ret = -1;
            ret = m_ecat.SdoDownload(nSlaveID, nIndex, nSubIndex, dataBuff, ref uErrorCode);
            return ret;
        }

        public static Ecat GetEcatLib()
        {
            return m_ecat;
        }

        public static string ErrorCodeToString(int err)
        {
            if (err >= 0x100 && err < 0x1000)
                return WMX3Api.ErrorToString(err);
            else if (err >= 0x2000 && err < 0x3000)
                return Ecat.ErrorToString(err);
            else if (err >= 0x10000 && err < 0x11000)
                return CoreMotion.ErrorToString(err);
            else if (err >= 0x14000 && err < 0x15000)
                return Io.ErrorToString(err);
            else
                return string.Format("It's not defined error code(0x{0:X})", err);
        }

        public static PlatformType GetCurrentPlatofrmType() {
            foreach (var module in modulesInfo.Modules) {
                if (module.ModuleName.ToLower().Contains("ethercat") && m_ecat != null) {
                    return PlatformType.EtherCAT;
                }
            }

            return PlatformType.Simulation;
        }
    }
}
