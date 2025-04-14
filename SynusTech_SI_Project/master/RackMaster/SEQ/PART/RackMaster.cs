using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MovenCore;
using RackMaster.SEQ.COMMON;
using RackMaster.SEQ.TCP;

namespace RackMaster.SEQ.PART {
    public enum MainSequenceStep {
        Idle,           
        Init_LIB,       // WMX Library 초기화 및 EtherCAT 통신 시작
        Init_Config,    // Config File 로드 및 파라미터 세팅
        Init_Axis,      // Aixs 클래스 초기화
        Init_IO,        // IO 클래스 초기화
        Init_CLS,       // 기타 클래스 초기화 및 TCP 통신 시작
        Init_Complete,

        Run,
        Stop
    }

    public enum SettingMode {
        Setup,
        Demo,
    }

    public partial class RackMasterMain
    {
        public RackMasterMotion m_motion;
        public RackMasterAlarm m_alarm;
        public RackMasterParam m_param;
        public RackMasterData m_data;
        public TeachingData m_teaching;
        private RackMasterParameterInterlock m_interlockParam;
        private Axis m_axis;

        private MainSequenceStep m_mainStep;
        private SettingMode m_settingMode;
        private Thread m_thread;
        private bool m_threadRun;

        private Thread m_statusThread;
        private bool m_statusThreadRun;

        private Thread m_fullClosedThread;
        private bool m_fullClosedThreadRun;
        private SEQ.CLS.Timer m_fullClosedTimer;

        private Thread m_etherCATThread;
        private bool m_etherCATTreadRun;

        private bool m_fullClosedAbnormal;

        private const int m_fullClosedVendorID = 0x00000121;
        private const int m_fullClosedProductCode = 0x00000002;
        private const int m_fullClosedIndex = 0x2000;
        private const int m_fullClosedSubIndex = 0x3;
        private byte[] m_fullClosedValue = new byte[1];

        public RackMasterMain()
        {
            m_interlockParam = new RackMasterParameterInterlock(m_param);
            m_GOTTimer = new CLS.Timer();

            m_mainStep = MainSequenceStep.Idle;
            m_settingMode = SettingMode.Setup;
            m_thread = null;
            m_threadRun = true;

            m_statusThread = null;
            m_statusThreadRun = false;

            m_fullClosedThread = null;
            m_fullClosedThreadRun = false;
            m_fullClosedTimer = new CLS.Timer();

            m_fullClosedAbnormal = false;

            m_Socket = new TCPSocket();
            m_Socket.ReceiveEvent += ReceiveData;

            m_RMServer = new TCPServer(m_ServerPort);
            m_RMServer.AcceptEvent += Server_AcceptEvent;

            m_RackMaster_RecvBitMap = new bool[CIM_2_RM_BIT_SIZE];
            m_RackMaster_SendBitMap = new bool[RM_2_CIM_BIT_SIZE];
            m_RackMaster_RecvWordMap = new short[CIM_2_RM_WORD_SIZE];
            m_RackMaster_SendWordMap = new short[RM_2_CIM_WORD_SIZE];
            m_RackMaster_ErrorWord = new System.Collections.BitArray(RM_ERRORWORD_BIT_SIZE);

            m_regSocket = new TCPSocket();
            m_regSocket.ReceiveEvent += Regulator_ReceivData;

            m_regServer = new TCPServer(m_regPort);
            m_regServer.AcceptEvent += Regulator_AcceptEvent;
        }
        /// <summary>
        /// 메인 스레드 시작
        /// </summary>
        public void Start()
        {
            if(m_thread == null) {
                m_thread = new Thread(new ThreadStart(MainThread));
                m_thread.IsBackground = true;
                m_thread.Name = $"RackMaster Main Thread";
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.ProgramStart));
                m_thread.Start();
            }
        }
        /// <summary>
        /// 메인 스레드 시퀀스 스텝 반환
        /// </summary>
        /// <returns></returns>
        public MainSequenceStep GetMainSequenceStep()
        {
            return m_mainStep;
        }
        /// <summary>
        /// 메인 스레드 시퀀스 스텝 설정
        /// </summary>
        /// <param name="step"></param>
        private void SetMainStep(MainSequenceStep step) {
            m_mainStep = step;
        }
        /// <summary>
        /// 메인 스레드 종료
        /// </summary>
        public void Stop() {
            if(m_thread != null) {
                SetMainStep(MainSequenceStep.Stop);
            }
        }
        /// <summary>
        /// 메인 스레드 동작 여부 판단
        /// </summary>
        /// <returns></returns>
        public bool IsThreadRun() {
            return m_threadRun;
        }
        /// <summary>
        /// 메인 스레드 동작
        /// </summary>
        private void MainThread()
        {
            m_mainStep = MainSequenceStep.Init_LIB;
            int ret = 0;
            while (m_threadRun)
            {
                switch (m_mainStep)
                {
                    case MainSequenceStep.Init_LIB:
                        ret = WMX3.CreateWMX3Device("Engine");
                        if(ret != WMXParam.ErrorCode_None) {
                            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.WMX, Log.LogMessage_Main.WMX_CreateDeviceFail));
                            Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, WMX3.ErrorCodeToString(ret)));
                            m_mainStep = MainSequenceStep.Init_Config;
                        }
                        else {
                            Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.WMX, Log.LogMessage_Main.WMX_CreateDevice));
                        }

                        int retryCount = 0;
                        while (true) {
                            ret = WMX3.StartCommunicate();
                            if (ret != WMXParam.ErrorCode_None) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.WMX, Log.LogMessage_Main.WMX_StartCommunicationFail));
                                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, WMX3.ErrorCodeToString(ret)));
                            }

                            if (retryCount > 5) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.WMX, Log.LogMessage_Main.WMX_StartCommunicationFail));
                                break;
                            }

                            if (WMX3.IsEngineRunning() && WMX3.IsEngineCommunicating()) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.WMX, Log.LogMessage_Main.WMX_StartCommunication));
                                Thread.Sleep(1000);
                                break;
                            }

                            retryCount++;
                            Thread.Sleep(2000);
                        }
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.InitLIB));
                        m_mainStep = MainSequenceStep.Init_Config;
                        break;

                    case MainSequenceStep.Init_Config:
                        Utility.InitUtilitSetting();
                        m_teaching = new TeachingData();
                        m_param = new RackMasterParam(this);
                        m_data = new RackMasterData();
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.InitConfig));
                        m_mainStep = MainSequenceStep.Init_Axis;
                        break;

                    case MainSequenceStep.Init_Axis:
                        m_axis = Axis.Instance;
                        m_axis.Initialize(m_param.GetAxisCount());
                        Thread.Sleep(10);
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.InitAxis));
                        m_mainStep = MainSequenceStep.Init_IO;
                        break;

                    case MainSequenceStep.Init_IO:
                        Io.Init();
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.InitIO));
                        m_mainStep = MainSequenceStep.Init_CLS;

                        break;

                    case MainSequenceStep.Init_CLS:
                        m_motion = new RackMasterMotion(this);
                        m_alarm = new RackMasterAlarm(this);
                        m_RMServer.ConnectionEvent += m_alarm.ClearAlarm_MSTDisconnection;
                        m_RMServer.Start();
                        m_regServer.ConnectionEvent += Regulator_ConnectEvent;
                        m_regServer.Start();
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.InitCLS));
                        m_mainStep = MainSequenceStep.Init_Complete;

                        break;

                    case MainSequenceStep.Init_Complete:
                        Thread.Sleep(100);
                        m_param.SetCommandPoisitionHomeShift();
                        FullClosedSetting();

                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.InitComplete));
                        m_mainStep = MainSequenceStep.Run;
                        break;

                    case MainSequenceStep.Run:
                        try {
                            StatusUpdate();
                            FullClosedAbnormal();
                            RecoveryEtherCAT();
                            Run();
                        }
                        catch (Exception ex) {
                            Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, "Exception", ex));
                        }

                        break;

                    case MainSequenceStep.Stop:
                        CloseCIM();
                        CloseRegulator();

                        StatusUpdateThreadStop();
                        MainThreadStop();
                        break;
                }

                Thread.Sleep(1);
            }
        }
        /// <summary>
        /// Motion에 해당하는 함수 호출(Alarm, Auto Motion, Auto Teaching)
        /// </summary>
        private void Run()
        {
            m_alarm.AlarmCheck();
            m_motion.AutoMotionRun();
            m_motion.AutoTeachingRun();
        }
        /// <summary>
        /// 축 상태 및 I/O 상태 업데이트 스레드
        /// </summary>
        private void StatusUpdate() {
            if(m_statusThread == null) {
                if (!m_statusThreadRun) {
                    m_statusThreadRun = true;
                }

                m_statusThread = new Thread(delegate () {
                    Axis updateServo = Axis.Instance;

                    while (m_statusThreadRun) {
                        if (IsConnected_EtherCAT()) {
                            Io.Update();
                            updateServo.UpdateMotorStatus();
                        }

                        Thread.Sleep(1);
                    }
                }) {
                    IsBackground = true,
                    Name = $"RackMaster Status Thread"
                };
                m_statusThread.Start();
            }
        }
        /// <summary>
        /// Full Closed 사용 시 Full Closed 인터락 체크 스레드
        /// </summary>
        private void FullClosedAbnormal() {
            if(m_fullClosedThread == null) {
                if (!m_fullClosedThreadRun)
                    m_fullClosedThreadRun = true;

                m_fullClosedThread = new Thread(delegate () {
                    while (m_fullClosedThreadRun) {
                        if (m_param.GetMotionParam().useFullClosed) {
                            Interlock_FullClosedAbnormalUpdate();
                        }
                        Thread.Sleep(1);
                    }
                }) {
                    IsBackground = true,
                    Name = $"Full Closed Abnormal Thread"
                };
                m_fullClosedThread.Start();
            }
        }
        /// <summary>
        /// 상태 업데이트 스레드 종료
        /// </summary>
        private void StatusUpdateThreadStop() {
            if(m_statusThread != null) {
                m_statusThreadRun = false;
                while (true) {
                    if (!m_statusThread.IsAlive) {
                        m_statusThread = null;
                        break;
                    }
                    Thread.Sleep(30);
                }
            }
        }
        /// <summary>
        /// 메인 스레드 종료
        /// </summary>
        private void MainThreadStop() {
            m_threadRun = false;
        }
        /// <summary>
        /// Full Closed Abnormal 상황 클리어
        /// </summary>
        public void FullClosedAbnormalClear() {
            m_fullClosedAbnormal = false;
        }
        /// <summary>
        /// Full Closed에 사용되는 거리감지 센서 세팅
        /// </summary>
        public void FullClosedSetting() {
            if (WMX3.GetCurrentPlatofrmType() == WMX3.PlatformType.Simulation)
                return;

            for(int i = 0; i < WMX3.GetSlaveCount(); i++) {
                if(WMX3.GetSlaveInformation(i) == null) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.WMX, $"Get Slave Information Fail"));
                }
                else {
                    if (WMX3.GetSlaveInformation(i).vendorId == m_fullClosedVendorID &&
                    WMX3.GetSlaveInformation(i).productCode == m_fullClosedProductCode) {
                        m_fullClosedValue[0] = 1;
                        uint errCode = 0;

                        while (true) {
                            int ret = WMX3.SDODownloadNormal(WMX3.GetSlaveInformation(i).id, m_fullClosedIndex, m_fullClosedSubIndex, m_fullClosedValue, ref errCode);

                            if(ret == WMXParam.ErrorCode_None) {
                                break;
                            }

                            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}"));
                            Thread.Sleep(5000);
                        }

                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.WMX, $"Full Closed Slave SDO Setting, err={errCode}"));
                    }
                }
            }
        }
        /// <summary>
        /// EtherCAT 통신에 문제가 발생할 경우 Recovery
        /// 이더캣 통신 종료 후 재시도
        /// </summary>
        private void RecoveryEtherCAT() {
            if (m_etherCATThread == null) {
                if (!m_etherCATTreadRun)
                    m_etherCATTreadRun = true;

                m_etherCATThread = new Thread(delegate () {
                    while (m_etherCATTreadRun) {
                        if(WMX3.GetOfflineSlaveCount() > 0 || !WMX3.IsEngineCommunicating()) {
                            Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"EtherCAT Communication Error!"));

                            if (Utility.GetEtherCAT_AutoRecovery()) {
                                WMX3.StopCommunicate();
                                Thread.Sleep(100);
                            }
                            while (true) {
                                if (Utility.GetEtherCAT_AutoRecovery()) {
                                    WMX3.StartCommunicate();
                                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.WMX_StartCommunication));
                                }
                                if (IsConnected_EtherCAT()) {
                                    Thread.Sleep(1000);
                                    break;
                                }
                                Thread.Sleep(10000);
                            }

                            foreach(AxisList axis in Enum.GetValues(typeof(AxisList))) {
                                m_motion.SetHomeDoneFlag(axis, true);
                            }
                            m_param.LoadWMXParameterFile();
                            FullClosedSetting();
                            m_param.SetCommandPoisitionHomeShift();
                            Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, "EtherCAT Recovery Success"));
                            Thread.Sleep(2000);
                        }
                        Thread.Sleep(1000);
                    }
                }) {
                    IsBackground = true,
                    Name = $"EtherCAT Thread"
                };
                m_etherCATThread.Start();
            }
        }
        /// <summary>
        /// EtherCAT 통신에 연결된 모든 슬레이브가 OP 상태인지 여부 판단
        /// </summary>
        /// <returns></returns>
        public bool IsAllSlaveOPState() {
            try {
                for (int i = 0; i < WMX3.GetSlaveCount(); i++) {
                    if (WMX3.GetSlaveInformation(i) != null) {
                        if (WMX3.GetSlaveInformation(i).ESM != WMX3.EtherCATStateMachine.Op)
                            return false;
                    }
                }

                return true;
            }catch(Exception ex) {
                return true;
            }
        }
        /// <summary>
        /// Output Bit 신호 설정
        /// </summary>
        /// <param name="output"></param>
        /// <param name="value"></param>
        public void SetOutputBit(OutputList output, bool value) {
            Io.SetOutputBit((int)output, value);
        }
        /// <summary>
        /// RackMaster가 동작 중 부저 울림 Enable 설정
        /// </summary>
        /// <param name="enable"></param>
        public void OnRackMasterRunSpeaker(bool enable) {
            if (IsOutputEnabled(OutputList.Voice_Buzzer_CH1)) {
                Io.SetOutputBit((int)OutputList.Voice_Buzzer_CH1, enable);
            }
        }
        /// <summary>
        /// RackMaster가 동장 중 부저 울림 Enable 설정
        /// </summary>
        /// <param name="enable"></param>
        public void OnRackMasterAlarmSpeaker(bool enable) {
            if (IsOutputEnabled(OutputList.Voice_Buzzer_CH3)) {
                Io.SetOutputBit((int)OutputList.Voice_Buzzer_CH3, enable);
            }
        }
        /// <summary>
        /// I/O Setting 모드 반환
        /// </summary>
        /// <returns></returns>
        public SettingMode GetCurrentSettingMode() {
            return m_settingMode;
        }
        /// <summary>
        /// I/O Setting 모드 설정
        /// </summary>
        /// <param name="mode"></param>
        public void SetSettingMode(SettingMode mode) {
            m_settingMode = mode;
        }
    }
}
