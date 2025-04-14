using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MovenCore;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.PART;

namespace RackMaster.SEQ.COMMON
{
    public class SeqMain
    {
        public static class Cmd
        {
            public const int INIT_LIB = 100;
            public const int INIT_CONFIG = 101;
            public const int INIT_AXIS = 102;
            public const int INIT_IO = 103;
            public const int INIT_CLS = 104;

            public const int RUN = 200;
            public const int STOP = 500;
        }

        private RackMasterTCP m_rackMasterTCP;
        private RackMasterMotion m_rackMaster;
        private Motor m_motor;

        private FSM m_fsm;
        private Thread m_thread;
        private bool m_threadRun;

        private Thread m_statusThread;
        private bool m_statusThreadRun;

        public SeqMain()
        {
            m_fsm = new FSM();
            m_thread = null;
            m_threadRun = true;

            m_statusThread = null;
            m_statusThreadRun = false;
        }

        public void Start()
        {
            if(m_thread == null) {
                m_thread = new Thread(new ThreadStart(MainThread));
                m_thread.IsBackground = true;
                m_thread.Start();
            }
        }

        public int GetFSMStatus()
        {
            return m_fsm.Get();
        }

        private void SetFSMStatus(int status) {
            m_fsm.Set(status);
        }

        public void Stop() {
            if(m_thread != null) {
                SetFSMStatus(Cmd.STOP);
            }
        }

        public bool IsThreadRun() {
            return m_threadRun;
        }

        private void MainThread()
        {
            m_fsm.Set(Cmd.INIT_LIB);

            while (m_threadRun)
            {
                switch (m_fsm.Get())
                {
                    case Cmd.INIT_LIB:
                        Log.InitLog();
                        ExceptionLog.InitExceptionLog();

                        WMX3.CreateWMX3Device("Engine");
                        WMX3.StartCommunicate();

                        int retryCount = 0;

                        while (true) {
                            if (retryCount > 15) {
                                // Error 처리
                            }

                            if (WMX3.IsEngineRunning() && WMX3.IsEngineCommunicating()) {
                                m_fsm.Set(Cmd.INIT_AXIS);
                                break;
                            }

                            retryCount++;
                            Thread.Sleep(1000);
                        }
                        m_fsm.Set(Cmd.INIT_CONFIG);
                        RMParam.Init();
                        break;

                    case Cmd.INIT_CONFIG:
                        m_fsm.Set(Cmd.INIT_AXIS);
                        break;

                    case Cmd.INIT_AXIS:
                        m_motor = Motor.Instance;
                        Thread.Sleep(10);
                        m_motor.RefreshAxisParameter();
                        m_fsm.Set(Cmd.INIT_IO);
                        break;

                    case Cmd.INIT_IO:
                        Io.Init();

                        m_fsm.Set(Cmd.INIT_CLS);

                        break;

                    case Cmd.INIT_CLS:
                        Port.InitData();
                        m_rackMasterTCP = new RackMasterTCP();
                        m_rackMaster = new RackMasterMotion();
                        m_rackMasterTCP.SetRackMasterInstance(m_rackMaster);
                        Alarm.Init();
                        m_fsm.Set(Cmd.RUN);

                        break;

                    case Cmd.RUN:
                        StatusUpdate();
                        Run();
                        UpdateConnected();

                        break;

                    case Cmd.STOP:
                        m_rackMasterTCP.CloseCIM();

                        StatusUpdateThreadStop();
                        MainThreadStop();
                        break;
                }

                Thread.Sleep(1);
            }
        }

        private void Run()
        {
            Alarm.AlarmCheck();
            m_rackMaster.AutoMotionRun();
            m_rackMaster.AutoTeachingRun();
        }

        private void UpdateConnected() {
            Global.IS_CONNECTED_TCP = m_rackMasterTCP.IsConnected();
            Global.IS_CONNECTED_ETHERCAT = WMX3.IsEngineCommunicating();
        }

        private void StatusUpdate() {
            if(m_statusThread == null) {
                if (!m_statusThreadRun) {
                    m_statusThreadRun = true;
                }

                m_statusThread = new Thread(delegate () {
                    Motor updateServo = Motor.Instance;

                    while (m_statusThreadRun) {
                        Io.Update();
                        updateServo.UpdateMotorStatus();

                        Thread.Sleep(1);
                    }
                });

                m_statusThread.IsBackground = true;
                m_statusThread.Start();
            }
        }

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

        private void MainThreadStop() {
            m_threadRun = false;
        }

        public RackMasterMotion GetRackMasterInstance() {
            return m_rackMaster;
        }
    }
}
