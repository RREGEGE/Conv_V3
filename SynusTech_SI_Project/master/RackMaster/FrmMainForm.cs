using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using RackMaster.SUBFORM;
using RackMaster.SEQ.COMMON;
using RackMaster.SEQ.PART;
using RackMaster.SEQ.CLS;
using System.Threading;
using System.IO;

namespace RackMaster {
    public partial class FrmMainForm : Form {
        private FrmMain frmMain;
        private FrmMotor frmMotor;
        private FrmSettings frmSettings;
        private FrmStatus frmStatus;
        private FrmTestDrive frmTestDrive;
        private FrmLogin frmLogin;
        private FrmHistory frmHistory;
        private FrmAlarmList frmAlarmList;
        private FrmUtilitySettings frmUtilitySettings;
        private FrmLoading frmLoading;
        private FrmMemoryMap frmMemoryMap;
        private UICtrl.ButtonCtrl m_btnCtrl;

        private SEQ.CLS.Timer m_serialProgramTimer;
        private SEQ.CLS.Timer m_temperatureTimer;
        private SEQ.CLS.Timer m_loginDurationTimer;
        private System.Timers.Timer m_logTimer;

        private const int m_temperatureTimerCount = 5000;

        private int m_loginDuration = 0;

        private RackMasterMain m_rackMaster;
        private Thread m_processUsageThread;

        private string loginButton;
        private const string serialProcName = "serial";

        private bool isStartedSuccess = false;
        private bool isMute = false;

        private float m_cpuUsage = 0.0f;
        private float m_memUsage = 0.0f;

        public FrmMainForm() {
            Thread loadingThread = new Thread(new ThreadStart(ShowLoadingScreen));
            loadingThread.Start();

            Log.InitLog();

            m_rackMaster = new RackMasterMain();
            m_rackMaster.Start();

            int count = 0;
            while (true) {
                if (count > 15) {
                    break;
                }

                if (m_rackMaster.GetMainSequenceStep() == MainSequenceStep.Run) {
                    isStartedSuccess = true;
                    System.Threading.Thread.Sleep(500);
                    break;
                }
                System.Threading.Thread.Sleep(1000);
                count++;
            }

            if (SynusLangPack.LoadFile(ManagedFileInfo.LangPackDirectory, ManagedFileInfo.RackMasterLangFileName)) {
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.Utility, Log.LogMessage_Main.Utility_LanguageFileLoadSuccess, $"Ver={SynusLangPack.GetLanguagePackVersion()}"));
            }
            else {
                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.Utility, Log.LogMessage_Main.Utility_LanguageFileLoadFail, $"{SynusLangPack.GetErrorMessage()}"));
            }

            if (loadingThread != null)
                frmLoading.Invoke(new Action(frmLoading.Close));

            m_btnCtrl = new UICtrl.ButtonCtrl();
            m_serialProgramTimer = new SEQ.CLS.Timer();
            m_temperatureTimer = new SEQ.CLS.Timer();
            m_loginDurationTimer = new SEQ.CLS.Timer();
            m_loginDurationTimer.Stop();
            InitializeComponent();

            if (File.Exists($"{ManagedFileInfo.StartUpPath}\\{ManagedFileInfo.LogoFileName}")) {
                pictureLogo.Load($"{ManagedFileInfo.StartUpPath}\\{ManagedFileInfo.LogoFileName}");
                pictureLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            LanguageChanged();

            DataGridView dgvLog = Log.dgvLog;
            pnlBottom.Controls.Add(dgvLog);
            dgvLog.Dock = DockStyle.Fill;

            frmMain = new FrmMain(m_rackMaster);
            frmMotor = new FrmMotor(m_rackMaster);
            frmSettings = new FrmSettings(m_rackMaster);
            frmStatus = new FrmStatus(m_rackMaster);
            frmTestDrive = new FrmTestDrive(m_rackMaster);
            frmHistory = new FrmHistory();
            frmUtilitySettings = new FrmUtilitySettings();
            frmAlarmList = new FrmAlarmList(m_rackMaster);

            frmMain.TopLevel = false;
            frmMotor.TopLevel = false;
            frmSettings.TopLevel = false;
            frmStatus.TopLevel = false;
            frmTestDrive.TopLevel = false;
            frmHistory.TopLevel = false;
            frmAlarmList.TopLevel = false;

            pnlMain.Controls.Add(frmMain);
            pnlMain.Controls.Add(frmMotor);
            pnlMain.Controls.Add(frmSettings);
            pnlMain.Controls.Add(frmStatus);
            pnlMain.Controls.Add(frmTestDrive);
            pnlMain.Controls.Add(frmHistory);
            pnlMain.Controls.Add(frmAlarmList);

            HideAllPage();

            frmMain.Visible = true;
            m_btnCtrl.SetOnOffButtonStyle(ref btnPageMain, true);

            UpdateProcessUsage();
            updateTimer.Start();

            frmUtilitySettings.LangChangeEvent += LanguageChanged;
            frmUtilitySettings.LangChangeEvent += frmMain.LanguageChanged;
            frmUtilitySettings.LangChangeEvent += frmSettings.LanguageChanged;
            frmUtilitySettings.LangChangeEvent += frmMotor.LanguageChanged;
            frmUtilitySettings.LangChangeEvent += frmTestDrive.LanguageChanged;
            frmUtilitySettings.LangChangeEvent += frmHistory.LanguageChanged;

            btnPageMain.Click += PageButtonClick;
            btnPageStatus.Click += PageButtonClick;
            btnPageMotor.Click += PageButtonClick;
            btnPageSetting.Click += PageButtonClick;
            btnPageTestDrive.Click += PageButtonClick;
            btnPageHistory.Click += PageButtonClick;
            btnPageAlarmList.Click += PageButtonClick;

            m_rackMaster.ReceiveBitEvent += AutoChangedEvent;

            CheckLogFileSetting();

            lblSWVersion.Text = $"Build : {BuildTime.GetBuidTime().ToString("yyyy/MM/dd HH:mm:ss")}";

            toolMemoryMap.Click += ToolMemoryMap_Click;
        }

        private void CheckLogFileSetting() {
            Log.CheckLogFile();

            DateTime now = DateTime.Now;
            DateTime tomorrowMidnight = now.Date.AddDays(1);
            TimeSpan timeUntilTomorrow = tomorrowMidnight - now;

            m_logTimer = new System.Timers.Timer();
            m_logTimer.Interval = timeUntilTomorrow.TotalMilliseconds;
            m_logTimer.Elapsed += LogTimerElapsed;
        }

        private void ShowLoadingScreen() {
            frmLoading = new FrmLoading(LoadingType.StartLoading);
            Application.Run(frmLoading);
        }

        private void UpdateProcessUsage() {
            if (m_processUsageThread != null) {
                m_processUsageThread.Join();
                m_processUsageThread.Abort();
                m_processUsageThread = null;
            }

            m_processUsageThread = new Thread(delegate () {
                while (m_rackMaster.GetMainSequenceStep() != MainSequenceStep.Stop) {
                    m_cpuUsage = CpuUsage.GetUsage();
                    m_memUsage = (float)MemoryUsage.GetMemoryUsage(MemoryUsage.MemoryUnit.MByte);
                    Thread.Sleep(3000);
                }
            });
            m_processUsageThread.IsBackground = true;
            m_processUsageThread.Name = $"Process Usage Thread";
            m_processUsageThread.Start();
        }

        private void HideAllPage() {
            frmMain.Visible = false;
            frmMotor.Visible = false;
            frmSettings.Visible = false;
            frmStatus.Visible = false;
            frmTestDrive.Visible = false;
            frmHistory.Visible = false;
            frmAlarmList.Visible = false;

            m_btnCtrl.SetOnOffButtonStyle(ref btnPageMain, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnPageMotor, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnPageStatus, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnPageSetting, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnPageTestDrive, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnPageHistory, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnPageAlarmList, false);
        }

        private void PageButtonClick(object sender, EventArgs e) {
            Button btn = sender as Button;

            if (btn == btnPageMain) {
                HideAllPage();
                frmMain.Visible = true;
                m_btnCtrl.SetOnOffButtonStyle(ref btn, true);
            }
            else if (btn == btnPageStatus) {
                HideAllPage();
                frmStatus.Visible = true;
                m_btnCtrl.SetOnOffButtonStyle(ref btn, true);
            }
            else if (btn == btnPageMotor) {
                HideAllPage();
                frmMotor.Visible = true;
                m_btnCtrl.SetOnOffButtonStyle(ref btn, true);
            }
            else if (btn == btnPageSetting) {
                frmLogin = new FrmLogin(true);

                frmLogin.loginEventWithDuration += AccessLogin;
                loginButton = btn.Text;

                frmLogin.ShowDialog();
            }
            else if (btn == btnPageTestDrive) {
                frmLogin = new FrmLogin(true);

                frmLogin.loginEventWithDuration += AccessLogin;
                loginButton = btn.Text;

                frmLogin.ShowDialog();
            }
            else if (btn == btnPageHistory) {
                HideAllPage();
                frmHistory.Visible = true;
                m_btnCtrl.SetOnOffButtonStyle(ref btn, true);
            }
            else if (btn == btnPageAlarmList) {
                HideAllPage();
                frmAlarmList.Visible = true;
                m_btnCtrl.SetOnOffButtonStyle(ref btn, true);
            }

            if (btn != btnPageSetting && btn != btnPageTestDrive) {
                m_loginDurationTimer.Stop();
                m_loginDurationTimer.Reset();
            }
        }

        private void AccessLogin(Utility.PasswordType type, int durationMilliseconds) {
            if (loginButton.Equals(btnPageSetting.Text)) {
                HideAllPage();

                if (type == Utility.PasswordType.Admin) {
                    frmSettings.AccessLoginType_Admin();
                } else if (type == Utility.PasswordType.User) {
                    frmSettings.AccessLoginType_User();
                }

                frmSettings.Visible = true;
                m_btnCtrl.SetOnOffButtonStyle(ref btnPageSetting, true);

                m_loginDuration = durationMilliseconds;
                if (m_loginDurationTimer.IsTimerStarted()) {
                    m_loginDurationTimer.Stop();
                }
                m_loginDurationTimer.Reset();
                m_loginDurationTimer.Start();

                return;
            }

            if (loginButton.Equals(btnPageTestDrive.Text)) {
                //if(type == Utility.PasswordType.User) {
                //    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.DoNotHaveAccess}"));
                //    return;
                //}

                HideAllPage();
                frmTestDrive.Visible = true;
                m_btnCtrl.SetOnOffButtonStyle(ref btnPageTestDrive, true);

                m_loginDuration = durationMilliseconds;
                if (m_loginDurationTimer.IsTimerStarted()) {
                    m_loginDurationTimer.Stop();
                }
                m_loginDurationTimer.Reset();
                m_loginDurationTimer.Start();

                return;
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e) {
            frmMain.UpdateFormUI();
            frmMotor.UpdateFormUI();
            frmSettings.UpdateFormUI();
            frmStatus.UpdateFormUI();
            frmTestDrive.UpdateFormUI();
            frmAlarmList.UpdateFormUI();

            if (m_rackMaster.IsConnected_EtherCAT()) {
                lblEtherCAT.BackColor = Color.GreenYellow;
                lblEtherCAT.Text = "Online";
            }
            else {
                lblEtherCAT.BackColor = Color.OrangeRed;
                lblEtherCAT.Text = "Offline";
            }

            if (m_rackMaster.IsConnected_Master()) {
                lblMaster.BackColor = Color.GreenYellow;
                lblMaster.Text = "Online";
            }
            else {
                lblMaster.BackColor = Color.OrangeRed;
                lblMaster.Text = "Offline";
            }

            if (m_rackMaster.IsAutoState()) {
                btnPageMotor.Enabled = false;
                btnPageTestDrive.Enabled = false;
                btnAlarmClear.Enabled = false;
            }
            else {
                if (!m_rackMaster.Interlock_IsSettingSuccessParamter() || !m_rackMaster.Interlock_IsParameterValueCorrect()) {
                    btnPageMotor.Enabled = false;
                    btnPageTestDrive.Enabled = false;
                    m_btnCtrl.BlinkingButton(ref btnPageSetting);
                }
                else {
                    btnPageMotor.Enabled = true;
                    btnPageTestDrive.Enabled = true;
                    if (frmSettings.Visible) {
                        m_btnCtrl.SetOnOffButtonStyle(ref btnPageSetting, true);
                    }
                    else {
                        m_btnCtrl.BlinkingButton(ref btnPageSetting, false);
                    }
                }
                btnAlarmClear.Enabled = true;
            }

            if (!isMute) {
                if (m_rackMaster.m_alarm.IsAlarmState()) {
                    m_rackMaster.OnRackMasterAlarmSpeaker(true);
                    m_rackMaster.OnRackMasterRunSpeaker(false);
                }
                else if (m_rackMaster.m_motion.IsAutoMotionRun()) {
                    m_rackMaster.OnRackMasterAlarmSpeaker(false);
                    m_rackMaster.OnRackMasterRunSpeaker(true);
                }
                else {
                    m_rackMaster.OnRackMasterAlarmSpeaker(false);
                    m_rackMaster.OnRackMasterRunSpeaker(false);
                }
            }

            if (m_loginDurationTimer.IsTimerStarted()) {
                lblLoginDuration.Text = $"Login Duration : {m_loginDurationTimer.GetRemainingTimeSpan((long)m_loginDuration):mm\\:ss}";
                if (frmTestDrive.IsTestDriveRun()) {
                    m_loginDurationTimer.Restart();
                }

                if (m_loginDurationTimer.Delay(m_loginDuration)) {
                    if ((frmTestDrive.Visible && !frmTestDrive.IsTestDriveRun()) || frmSettings.Visible) {
                        ChangeMainForm();
                        m_loginDurationTimer.Stop();
                    }
                }
            }
            else {
                lblLoginDuration.Text = string.Empty;
            }

            lblCurrentTime.Text = $"Time : {DateTime.Now:u}";
            lblCPUUsage.Text = $"{m_cpuUsage:F1}%";
            lblRamUsage.Text = $"{m_memUsage:F1}MByte";

            m_btnCtrl.BlinkingButton(ref btnAlarmClear, m_rackMaster.m_alarm.IsAlarmState());

            SerialRun();
            UpdateAmpTemperature();
        }

        private void UpdateAmpTemperature() {
            if (m_rackMaster.IsConnected_EtherCAT() && m_rackMaster.m_motion.IsAllAxisIsNotAlarmState()) {
                if (!m_temperatureTimer.IsTimerStarted()) {
                    m_temperatureTimer.Start();
                }

                if (m_temperatureTimer.Delay(m_temperatureTimerCount)) {
                    int ampTemp_X = m_rackMaster.m_motion.GetAmpTemperature(AxisList.X_Axis);
                    int ampTemp_Z = m_rackMaster.m_motion.GetAmpTemperature(AxisList.Z_Axis);
                    int ampTemp_A = m_rackMaster.m_motion.GetAmpTemperature(AxisList.A_Axis);

                    frmMain.UpdateAmpTemperature(AxisList.X_Axis, ampTemp_X);
                    frmMain.UpdateAmpTemperature(AxisList.Z_Axis, ampTemp_Z);
                    frmMain.UpdateAmpTemperature(AxisList.A_Axis, ampTemp_A);

                    frmStatus.UpdateAmpTemperature(AxisList.X_Axis, ampTemp_X);
                    frmStatus.UpdateAmpTemperature(AxisList.Z_Axis, ampTemp_Z);
                    frmStatus.UpdateAmpTemperature(AxisList.A_Axis, ampTemp_A);

                    if (m_rackMaster.m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                        int ampTemp_T = m_rackMaster.m_motion.GetAmpTemperature(AxisList.T_Axis);

                        frmMain.UpdateAmpTemperature(AxisList.T_Axis, ampTemp_T);
                        frmStatus.UpdateAmpTemperature(AxisList.T_Axis, ampTemp_T);
                    }

                    m_temperatureTimer.Stop();
                    m_temperatureTimer.Reset();
                    m_temperatureTimer.Start();
                }
            }
            else {
                m_temperatureTimer.Stop();
                m_temperatureTimer.Reset();
            }
        }

        private void FrmMainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (!isStartedSuccess) {
                //return;
            }

            if (DialogResult.Yes != MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ExitProgram}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.ExitProgramTitle}"), MessageBoxButtons.YesNo)) {
                e.Cancel = true;
            }
            else {
                updateTimer.Stop();
                m_rackMaster.Stop();
                while (true) {
                    if (!m_rackMaster.IsThreadRun()) {
                        System.Threading.Thread.Sleep(100);
                        break;
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnAlarmClear_Click(object sender, EventArgs e) {
            if (m_rackMaster.IsAutoState()) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.IsAutoState}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            foreach (AxisList axis in Enum.GetValues(typeof(AxisList))) {
                m_rackMaster.m_motion.AlarmClear(axis);
            }
        }

        private void btnUtilitySettings_Click(object sender, EventArgs e) {
            frmUtilitySettings.ShowDialog();
        }

        private void LanguageChanged() {
            //m_btnCtrl.SetButtonText(ref btnUtilitySettings, SynusLangPack.GetLanguage(btnUtilitySettings.Name));
            //m_btnCtrl.SetButtonText(ref btnBuzzerStop, SynusLangPack.GetLanguage(btnBuzzerStop.Name));
            //m_btnCtrl.SetButtonText(ref btnPageMain, SynusLangPack.GetLanguage(btnPageMain.Name));
            //m_btnCtrl.SetButtonText(ref btnPageMotor, SynusLangPack.GetLanguage(btnPageMotor.Name));
            //m_btnCtrl.SetButtonText(ref btnPageStatus, SynusLangPack.GetLanguage(btnPageStatus.Name));
            //m_btnCtrl.SetButtonText(ref btnPageSetting, SynusLangPack.GetLanguage(btnPageSetting.Name));
            //m_btnCtrl.SetButtonText(ref btnPageTestDrive, SynusLangPack.GetLanguage(btnPageTestDrive.Name));
            //m_btnCtrl.SetButtonText(ref btnPageHistory, SynusLangPack.GetLanguage(btnPageHistory.Name));
            //m_btnCtrl.SetButtonText(ref btnExit, SynusLangPack.GetLanguage(btnExit.Name));
            //m_btnCtrl.SetButtonText(ref btnUtilitySettings, SynusLangPack.GetLanguage(btnUtilitySettings.Name));
        }

        private void AutoChangedEvent(ReceiveBitMap bitMap) {
            if (bitMap == ReceiveBitMap.RM_Auto_Request) {
                if (frmMotor.Visible || frmTestDrive.Visible || frmSettings.Visible) {
                    ChangeMainForm();
                    m_loginDurationTimer.Stop();
                }
            }
        }

        private void ChangeMainForm() {
            HideAllPage();
            frmMain.Visible = true;
            m_btnCtrl.SetOnOffButtonStyle(ref btnPageMain, true);
        }

        private void btnAlarmClear_Click_1(object sender, EventArgs e) {
            m_rackMaster.m_alarm.ClearAlarm();
        }

        private void SerialRun() {
            try {
                if (ProcessControl.IsRunningProcess(serialProcName)) {
                    return;
                }
                else {
                    if (m_serialProgramTimer.Delay(10000)) {
                        if (m_rackMaster.GetInputBit(InputList.HP_DTP_Mode_Select_SW_1) || m_rackMaster.GetInputBit(InputList.HP_DTP_Mode_Select_SW_2) ||
                        m_rackMaster.GetInputBit(InputList.OP_DTP_Mode_Select_SW_1) || m_rackMaster.GetInputBit(InputList.OP_DTP_Mode_Select_SW_2)) {
                            if (ProcessControl.RunAsAdministrator($"C:", $"{serialProcName}.exe")) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.Utility, "Serial Program Start Success!"));
                            }
                        }
                        m_serialProgramTimer.Restart();
                    }
                }
            } catch (Exception ex) {
                m_serialProgramTimer.Restart();
            }
        }

        private void LogTimerElapsed(object sender, ElapsedEventArgs e) {
            Log.CheckLogFile();

            System.Timers.Timer timer = (System.Timers.Timer)sender;
            timer.Interval = 24 * 60 * 60 * 1000;
        }

        private void btnBuzzerStop_Click(object sender, EventArgs e) {
            isMute = isMute ? false : true;

            if (!isMute) {
                this.btnBuzzerStop.Image = global::RackMaster.Properties.Resources.icons8_no_audio_48;
            }
            else {
                this.btnBuzzerStop.Image = global::RackMaster.Properties.Resources.icons8_speaker_48;
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e) {
            this.WindowState = FormWindowState.Minimized;
        }

        private void ToolMemoryMap_Click(object sender, EventArgs e) {
            if (frmMemoryMap == null || frmMemoryMap.IsDisposed) {
                frmMemoryMap = new FrmMemoryMap(m_rackMaster);
                frmMemoryMap.Show();
            }
            else {
                frmMemoryMap.WindowState = FormWindowState.Normal;
                frmMemoryMap.Focus();
            }
        }
    }
}
