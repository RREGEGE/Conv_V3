using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Globalization;
using Master.Equipment;
using Master.ManagedFile;
using Master.SubForm;
using Master.SubForm.MasterForm;
using Master.SubForm.PortTPForm;
using Master.SubForm.RMTPForm;
using Master.Interface.Processor;
using System.IO.Compression;
using Master.GlobalForm;


namespace Master
{
    public partial class Frm_Main : Form
    {
        //---Monitor 접근 인터락용 Form Class---//
        Frm_Lock[] frm_Lock = new Frm_Lock[]
        {
            new Frm_Lock(),
            new Frm_Lock()
        };
        //

        //---Main form에서 출력되는 Form List---//
        Frm_MasterSafetyMapSettings frm_SafetyMapSettings   = null; //Main 화면에서 I/O Map Setting 버튼 클릭 시 출력되는 설정 화면
        Frm_ControlMonRM frm_ControlMonRM                   = new Frm_ControlMonRM() { TopLevel = false };  //Main 화면에서 Port Btn 클릭 시 출력되는 모니터링 화면
        Frm_ControlMonPort frm_ControlMonPort               = new Frm_ControlMonPort() { TopLevel = false }; //Main 화면에서 STK Btn 클릭 시 출력되는 모니터링 화면
        Frm_PortTPScreen frm_SecondPortTPScreen             = null; //신시누 향으로 추가 된 것, 모니터 3개인 경우 3번 모니터 포트 화면 고정 출력 용
        //

        //---Main form에서 출력되는 Form List---//
        MasterSafetyImageInfo safetyImageInfo               = new MasterSafetyImageInfo(); //Main form에서 관리되는 I/O Map 사진, 위치 정보
        TableLayoutPanel[] SafetyLayoutPanel; //MainForm 내의 I/O Map 영역에 표시되는 상태 패널
        Label[] SafetyLabel; //MainForm 내의 I/O Map 영역에 표시되는 상태 라벨
        //

        //---Main form 하단 로그가 출력되는 DataGridView---//
        DataGridView LogDGV;
        //

        //---Main form 우측 장비 정보 표시를 위한 라벨---//
        Label[] m_BasicInfoLabel;
        //

        //---Main form 중앙 장비 상태 폼 변경을 위한 장비 선택 버튼---//
        Button[] m_CommanderButton;
        //

        //---즉시 종료를 막기 위한 플래그(notifyicon 용)---//
        bool m_bShutdown = false;
        //

        //---CPU, Memory Monitoring을 위한 변수---//
        float ProcessCPU = 0.0f;
        double ProcessMemory = 0.0;
        static public string CPUUsageStr = string.Empty;
        static public string RAMUsageStr = string.Empty;
        //

        public Frm_Main()
        {
            //1. C# 기본 함수
            InitializeComponent();
            
            //2. Main Form에서 사용되는 UI 객체 할당
            ControlItemInit();

            //3. 프로그램에서 사용되는 시스템 파일 로드, App, Lang, Network 구성 정보, Safety Image
            SystemFilesLoad();

            //4. 장비에서 사용될 파라미터 로드 및 Init
            if (SystemParamsInit())
            {
                //5.Init 성공 시 UI 갱신 및 각종 타이머 시작
                RefreshAndMainStart();
                UIUpdateTimer.Enabled = true;
                IntervalColorUpdateTimer.Enabled = true;
            }
            else
            {
                //6.Init 실패 시 화면 invisible 처리 및 종료 타이머 시작
                this.Visible = false;
                Form_CloseTimer.Enabled = true;
            }
        }

        /// <summary>
        /// Master Program에서 사용되는 File Load
        /// 1. ApplicationParam (SysAppInfo.ini)
        /// 2. LangPack (MasterLangPack.ini)
        /// 3. EquipNetworkParam (SysNetworkParamInfo.ini)
        /// 4. SafetyImageInfo (SafetyItemInfo.xml)
        /// </summary>
        private void SystemFilesLoad()
        {
            if (ApplicationParam.LoadFile(ManagedFileInfo.AppSettingsDirectory, ManagedFileInfo.AppSettingsFileName))
            {
                //Load 성공 시 값을 로그 클래스에 적용
                Log.ShowType[LogMsg.LogType.Application]    = ApplicationParam.m_LogParam.LogShowType_Application;
                Log.ShowType[LogMsg.LogType.Exception]      = ApplicationParam.m_LogParam.LogShowType_Exception;
                Log.ShowType[LogMsg.LogType.Port]           = ApplicationParam.m_LogParam.LogShowType_Port;
                Log.ShowType[LogMsg.LogType.RackMaster]     = ApplicationParam.m_LogParam.LogShowType_RackMaster;
                Log.ShowType[LogMsg.LogType.CIM]            = ApplicationParam.m_LogParam.LogShowType_CIM;
                Log.ShowType[LogMsg.LogType.Master]         = ApplicationParam.m_LogParam.LogShowType_Master;
                Log.ShowType[LogMsg.LogType.WMX]            = ApplicationParam.m_LogParam.LogShowType_WMX;
                Log.ShowType[LogMsg.LogType.CPS]            = ApplicationParam.m_LogParam.LogShowType_CPS;

                Log.ShowLevel[LogMsg.LogLevel.Normal]       = ApplicationParam.m_LogParam.LogShowType_Normal;
                Log.ShowLevel[LogMsg.LogLevel.Warning]      = ApplicationParam.m_LogParam.LogShowType_Warning;
                Log.ShowLevel[LogMsg.LogLevel.Error]        = ApplicationParam.m_LogParam.LogShowType_Error;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileLoadSuccess, $"App Param");
            }
            else
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.FileLoadFail, $"App Param");

            if (SynusLangPack.LoadFile(ManagedFileInfo.LangPackDirectory, ManagedFileInfo.LangPackFileName))
            {
                //Load 성공 시 값을 SynusLangPack 클래스에 적용
                SynusLangPack.SetLanguageType(ApplicationParam.m_ApplicationParam.eLangType);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileLoadSuccess, $"Lang Pack: {SynusLangPack.GetLanguagePackVersion()}");
            }
            else
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.FileLoadFail, $"Error: {SynusLangPack.GetErrorMessage()}");

            if (EquipNetworkParam.LoadFile(ManagedFileInfo.EquipNetworkParamDirectory, ManagedFileInfo.EquipNetworkParamFileName))
            {
                //Load 성공 시 이후 장비 Initialize 과정에서 사용
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileLoadSuccess, $"Network Param");
            }
            else
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.FileLoadFail, $"Network Param");

            if (!MasterSafetyImageInfo.Load(ref safetyImageInfo))
            {
                //Load 실패 시 Item을 I/O Map 현재 위치로 초기화 및 다시 저장
                safetyImageInfo.WorkZoneImagePath = string.Empty;

                foreach (var eSafetyItemList in Enum.GetValues(typeof(Master.DGV_SaftyIOStatusRow)))
                {
                    safetyImageInfo.SafetyItems[(int)eSafetyItemList].Text = $"{eSafetyItemList}";
                    safetyImageInfo.SafetyItems[(int)eSafetyItemList].MapVisible = false;
                    safetyImageInfo.SafetyItems[(int)eSafetyItemList].GridVisible = false;
                    safetyImageInfo.SafetyItems[(int)eSafetyItemList].X = SafetyLayoutPanel[(int)eSafetyItemList].Location.X;
                    safetyImageInfo.SafetyItems[(int)eSafetyItemList].Y = SafetyLayoutPanel[(int)eSafetyItemList].Location.Y;
                }

                SafetyInfoSync(safetyImageInfo);
                MasterSafetyImageInfo.Save(safetyImageInfo);
            }
            else
            {
                //Load 성공 시 I/O Map을 Item에 저장된 위치로 조정
                SafetyInfoSync(safetyImageInfo);
            }
        }

        /// <summary>
        /// SafetyItemInfo 클래스의 정보를 기준으로 I/O Map 위치를 조정
        /// SafetyItemInfo -> UI Item
        /// </summary>
        /// <param name="_SafetyImageInfo"></param>
        private void SafetyInfoSync(MasterSafetyImageInfo _SafetyImageInfo)
        {
            try
            {
                string ImagePath = _SafetyImageInfo.WorkZoneImagePath;
                Image image = Image.FromFile(ImagePath);

                if (image != null)
                {
                    pictureBox2.BackgroundImage = image;
                }
            }
            catch
            {
                pictureBox2.BackgroundImage = _SafetyImageInfo.GetDefaultImage();
            }

            if(_SafetyImageInfo.SafetyItems.Length < Enum.GetValues(typeof(Master.DGV_SaftyIOStatusRow)).Length)
            {
                Array.Resize(ref _SafetyImageInfo.SafetyItems, Enum.GetValues(typeof(Master.DGV_SaftyIOStatusRow)).Length);
            }

            foreach (var eSafetyItemList in Enum.GetValues(typeof(Master.DGV_SaftyIOStatusRow)))
            {
                if (_SafetyImageInfo.SafetyItems[(int)eSafetyItemList] == null)
                    _SafetyImageInfo.SafetyItems[(int)eSafetyItemList] = new MasterSafetyImageInfo.SafetyItem();

                int X = _SafetyImageInfo.SafetyItems[(int)eSafetyItemList].X;
                int Y = _SafetyImageInfo.SafetyItems[(int)eSafetyItemList].Y;

                SafetyLayoutPanel[(int)eSafetyItemList].Visible = _SafetyImageInfo.SafetyItems[(int)eSafetyItemList].MapVisible;
                SafetyLayoutPanel[(int)eSafetyItemList].Location = new Point(X, Y);
            }
        }

        /// <summary>
        /// I/O Map의 위치 정보를 SafetyItemInfo 클래스에 저장
        /// UI Item -> SafetyItemInfo
        /// </summary>
        private void SafetyInfoItemLocationSync()
        {
            foreach (var eSafetyItemList in Enum.GetValues(typeof(Master.DGV_SaftyIOStatusRow)))
            {
                safetyImageInfo.SafetyItems[(int)eSafetyItemList].X = SafetyLayoutPanel[(int)eSafetyItemList].Location.X;
                safetyImageInfo.SafetyItems[(int)eSafetyItemList].Y = SafetyLayoutPanel[(int)eSafetyItemList].Location.Y;
            }
        }
        
        /// <summary>
        /// 장비에서 사용되는 파라미터 및 장비 클래스 Intialize 진행
        /// </summary>
        /// <returns></returns>
        private bool SystemParamsInit()
        {
            try
            {
                int err = Master.MasterInit();
                if (err != 0)
                {
                    if (err == 268)
                        MessageBox.Show($"{MovenCore.WMX3.ErrorCodeToString(err)}\nRTX SubSystem Restart Please.", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show($"{MovenCore.WMX3.ErrorCodeToString(err)}", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(System.TypeInitializationException))
                    MessageBox.Show(SynusLangPack.GetLanguage("RTXSubsystemException"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    LogMsg.AddExceptionLog(ex, $"Main Initialize");
                    MessageBox.Show("Master Load Exception.\nPlease Log File Check.", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
        }
        
        /// <summary>
        /// 장비 로드 성공 시 로드 된 장비 기준으로 Initialize 및 이벤트 연동, 각종 업데이트 시작
        /// </summary>
        private void RefreshAndMainStart()
        {
            Load_MonitoringPortMap();
            Load_MonitoringRackMasterMap();
            LogFileCompressAndRemove();
            CPURAMUsageUpdate();
            LogIn.LogInExtendMessageEvent += LogInExtendMessageEvent;

            this.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                if (!m_bShutdown)
                {
                    //종료가 아닌 경우 최소화
                    e.Cancel = true;
                    this.Visible = false;
                }
            };

            this.Disposed += (object sender, EventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;

                frm_ControlMonRM.Visible = false;
                frm_ControlMonPort.Visible = false;
                frm_ControlMonRM.Close();
                frm_ControlMonPort.Close();

                frm_ControlMonRM.Dispose();
                frm_ControlMonPort.Dispose();

                if(frm_SecondPortTPScreen != null)
                {
                    frm_SecondPortTPScreen.Close();
                    if(frm_SecondPortTPScreen != null && 
                        !frm_SecondPortTPScreen.IsDisposed)
                        frm_SecondPortTPScreen.Dispose();
                }

                Master.MasterClose();
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ProgramClose, string.Empty);
            };
        }
        
        /// <summary>
        /// CPU, RAM 업데이트 관련 함수
        /// CPU 부하에 따라 전체적인 시퀀스 Delay 조정
        /// </summary>
        private void CPURAMUsageUpdate()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                while (true)
                {
                    if (!m_bShutdown)
                    {
                        if (chx_CPUUpdate.Checked)
                        {
                            var CPUUsage = CpuUsage.GetCPUUsage();
                            ProcessCPU = CPUUsage[(int)CpuUsage.CPUUsageType.MyProcess];

                            if (ProcessCPU < 5.0)
                            {
                                Master.StepUpdateTime = 5;
                                Master.StatusUpdateTime = 5;
                            }
                            else if (ProcessCPU >= 5.0 && ProcessCPU < 10)
                            {
                                Master.StepUpdateTime = 10;
                                Master.StatusUpdateTime = 5;
                            }
                            else if (ProcessCPU >= 10.0 && ProcessCPU < 30)
                            {
                                Master.StepUpdateTime = 20;
                                Master.StatusUpdateTime = 10;
                            }
                            else if (ProcessCPU >= 30.0 && ProcessCPU < 50)
                            {
                                Master.StepUpdateTime = 20;
                                Master.StatusUpdateTime = 20;
                            }
                            else if (ProcessCPU >= 50.0)
                            {
                                Master.StepUpdateTime = 50;
                                Master.StatusUpdateTime = 50;
                            }
                        }
                    }
                    Thread.Sleep(1000);
                };
            });
            LocalThread.IsBackground = true;
            LocalThread.Priority = ThreadPriority.Highest;
            LocalThread.Name = $"CPU Usage Update";
            LocalThread.Start();

            Thread LocalThread2 = new Thread(delegate ()
            {
                while (true)
                {
                    if (!m_bShutdown)
                    {
                        if (chx_RAMUpdate.Checked)
                        {
                            //var SysMemory = MemoryUsage.GetSystemMemory();
                            //ProcessMemory = SysMemory[(int)MemoryUsage.MemoryUnit.MByte].CurrentProcessMemory;
                            //ProcessMemory = MemoryUsage.GetSystemMemory(MemoryUsage.MemoryUnit.MByte).CurrentProcessMemory;
                            ProcessMemory = MemoryUsage.GetMyProcessMemoryUsage(MemoryUsage.MemoryUnit.MByte);
                        }
                    }
                    Thread.Sleep(1000);
                };
            });
            LocalThread2.IsBackground = true;
            LocalThread2.Priority = ThreadPriority.Highest;
            LocalThread2.Name = $"RAM Usage Update";
            LocalThread2.Start();
        }
        
        /// <summary>
        /// Log Update Event
        /// </summary>
        /// <param name="_LogMsg"></param>
        private void UpdateLogDGV(LogMsg _LogMsg)
        {
            if(!this.IsDisposed)
                Log.InsertLogDGV(LogDGV, _LogMsg);
        }

        /// <summary>
        /// Login 관련 시간 연장 메세지 출력
        /// </summary>
        private void LogInExtendMessageEvent()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () 
                {
                    LogInExtendMessage();
                }));
            }
            else
            {
                LogInExtendMessage();
            }
        }

        /// <summary>
        /// Login 관련 시간 연장 메세지
        /// </summary>
        private void LogInExtendMessage()
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.LogIn, $"LogIn Extend Message");
            DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_Login_ExtendLogin"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                if (LogIn.IsLogIn())
                {
                    LogIn.SetLogInExtend();
                }
            }
            else
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.LogInExtendReject, $"LogIn Extend Reject");
        }
        
        /// <summary>
        /// Main form에서 사용되는 UI Item관련 1회성 Update 또는 할당
        /// </summary>
        private void ControlItemInit()
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ProgramStart, string.Empty);
            FormFunc.SetDoubleBuffer(this);
            BuildVersionUpdate();
            LogDGVInit();
            InfoLabelAndEquipSelectButtonInit();
            ContextMenuItemInit();

            try
            {
                string filePath = ManagedFile.ManagedFileInfo.StartUpPath + "\\logo.png";

                if (File.Exists(filePath))
                    pictureBox1.BackgroundImage = Image.FromFile(filePath);
                else
                    pictureBox1.BackgroundImage = Properties.Resources.logo;
            }
            catch
            {
                pictureBox1.BackgroundImage = Properties.Resources.logo;
            }

            SafetyLayoutPanel = new TableLayoutPanel[]
            {
                TPnl_SafetyStatus_Index0,
                TPnl_SafetyStatus_Index1,
                TPnl_SafetyStatus_Index2,
                TPnl_SafetyStatus_Index3,
                TPnl_SafetyStatus_Index4,
                TPnl_SafetyStatus_Index5,
                TPnl_SafetyStatus_Index6,
                TPnl_SafetyStatus_Index7,
                TPnl_SafetyStatus_Index8,
                TPnl_SafetyStatus_Index9,
                TPnl_SafetyStatus_Index10,
                TPnl_SafetyStatus_Index11,
                TPnl_SafetyStatus_Index12,
                TPnl_SafetyStatus_Index13,
                TPnl_SafetyStatus_Index14,
                TPnl_SafetyStatus_Index15,
                TPnl_SafetyStatus_Index16,
                TPnl_SafetyStatus_Index17,
                TPnl_SafetyStatus_Index18,
                TPnl_SafetyStatus_Index19,
                TPnl_SafetyStatus_Index20,
                TPnl_SafetyStatus_Index21,
                TPnl_SafetyStatus_Index22
            };

            SafetyLabel = new Label[]
            {
                lbl__SafetyStatus_Index0,
                lbl__SafetyStatus_Index1,
                lbl__SafetyStatus_Index2,
                lbl__SafetyStatus_Index3,
                lbl__SafetyStatus_Index4,
                lbl__SafetyStatus_Index5,
                lbl__SafetyStatus_Index6,
                lbl__SafetyStatus_Index7,
                lbl__SafetyStatus_Index8,
                lbl__SafetyStatus_Index9,
                lbl__SafetyStatus_Index10,
                lbl__SafetyStatus_Index11,
                lbl__SafetyStatus_Index12,
                lbl__SafetyStatus_Index13,
                lbl__SafetyStatus_Index14,
                lbl__SafetyStatus_Index15,
                lbl__SafetyStatus_Index16,
                lbl__SafetyStatus_Index17,
                lbl__SafetyStatus_Index18,
                lbl__SafetyStatus_Index19,
                lbl__SafetyStatus_Index20,
                lbl__SafetyStatus_Index21,
                lbl__SafetyStatus_Index22
            };
        }
        
        /// <summary>
        /// 마지막 빌드 날짜를 화면에 표시
        /// </summary>
        private void BuildVersionUpdate()
        {
            //1. Assembly.GetExecutingAssembly().FullName의 값은     
            //'ApplicationName, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'    
            //와 같다.     
            string strVersionText = Assembly.GetExecutingAssembly().FullName
                .Split(',')[1]
                .Trim()
                .Split('=')[1];

            //2. Version Text의 세번째 값(Build Number)은 2000년 1월 1일부터     
            //Build된 날짜까지의 총 일(Days) 수 이다.    
            int intDays = Convert.ToInt32(strVersionText.Split('.')[2]);
            DateTime refDate = new DateTime(2000, 1, 1);
            DateTime dtBuildDate = refDate.AddDays(intDays);

            ////3. Verion Text의 네번째 값(Revision NUmber)은 자정으로부터 Build된    
            ////시간까지의 지나간 초(Second) 값 이다.    
            int intSeconds = Convert.ToInt32(strVersionText.Split('.')[3]);
            intSeconds = intSeconds * 2;
            dtBuildDate = dtBuildDate.AddSeconds(intSeconds);

            ////4. 시차조정    
            DaylightTime daylingTime = TimeZone.CurrentTimeZone.GetDaylightChanges(dtBuildDate.Year);
            if (TimeZone.IsDaylightSavingTime(dtBuildDate, daylingTime))
                dtBuildDate = dtBuildDate.Add(daylingTime.Delta);

            string AppVersionFirstValue = strVersionText.Split('.').Length >= 1 ? strVersionText.Split('.')[0] : "None";
            string AppVersionSecondValue = strVersionText.Split('.').Length >= 2 ? strVersionText.Split('.')[1] : "None";

            lbl_MasterVer.Text = $"v{AppVersionFirstValue}.{AppVersionSecondValue} " +
                $"({dtBuildDate.ToString("yyyy.MM.dd HH:mm:ss")})";
        }
        
        /// <summary>
        /// Log GridView를 Log Class와 연동
        /// </summary>
        private void LogDGVInit()
        {
            LogDGV = Log.CreateLogGridView();
            FormFunc.SetDoubleBuffer(LogDGV);
            Log.LogDGVReload(LogDGV);
            Log.logInsertEvent += UpdateLogDGV;

            if (!pnl_LogDGVPanel.Controls.Contains(LogDGV))
            {
                pnl_LogDGVPanel.Controls.Clear();
                pnl_LogDGVPanel.Controls.Add(LogDGV);
                pnl_LogDGVPanel.Controls[0].Dock = DockStyle.Fill;
            }
        }

        /// <summary>
        /// 우측 장비 정보 라벨과 중앙 모니터링용 장비 객체 버튼 할당 
        /// </summary>
        private void InfoLabelAndEquipSelectButtonInit()
        {
            m_BasicInfoLabel = new Label[5] { label_EquipInfo1, label_EquipInfo2, label_EquipInfo3, label_EquipInfo4, label_EquipInfo5 };
            m_CommanderButton = new Button[5] { btn_CommanderEquip1, btn_CommanderEquip2, btn_CommanderEquip3, btn_CommanderEquip4, btn_CommanderEquip5 };

            for (int nCount = 0; nCount < m_BasicInfoLabel.Length; nCount++)
            {
                m_BasicInfoLabel[nCount].Text = string.Empty;
                m_BasicInfoLabel[nCount].Visible = false;
                FormFunc.SetDoubleBuffer(m_BasicInfoLabel[nCount]);
            }
            tableLayoutPanel_BasicInfo.Tag = 0;

            for (int nCount = 0; nCount < m_CommanderButton.Length; nCount++)
            {
                m_CommanderButton[nCount].Text = string.Empty;
                m_CommanderButton[nCount].Visible = false;
                m_CommanderButton[nCount].Tag = null;
                FormFunc.SetDoubleBuffer(m_CommanderButton[nCount]);
            }
            tableLayoutPanel_FocusButton.Tag = 0;
        }
        
        /// <summary>
        /// 마우스 우 클릭시 출력되는 이벤트 및 메뉴 객체에 연동(notifyIcon)
        /// </summary>
        private void ContextMenuItemInit()
        {
            ContextMenu ctx = new ContextMenu();

            MenuItem item = new MenuItem();
            item.Text = SynusLangPack.GetLanguage("Open");
            item.Click += btn_Show_Click;

            ctx.MenuItems.Add(item);
            ctx.MenuItems.Add("-");

            MenuItem item2 = new MenuItem();
            item2.Text = SynusLangPack.GetLanguage("Close");
            item2.Click += btn_Exit_Click;
            ctx.MenuItems.Add(item2);

            notifyIcon1.ContextMenu = ctx;

            //ContextMenu TopContextMenu = new ContextMenu();

            //MenuItem SafetyMapSetting = new MenuItem();
            //SafetyMapSetting.Name = "SafetyMap Settings";
            //SafetyMapSetting.Text = "SafetyMap Settings";
            //SafetyMapSetting.Click += btn_SafetyMapSettings_Click;
            //TopContextMenu.MenuItems.Add(SafetyMapSetting);

            //panel_Top.ContextMenu = TopContextMenu;

        }

        /// <summary>
        /// 실시간 화면 업데이트 영역
        /// 객체에 대한 정보를 지속 적으로 업데이트 하기위해선 해당 영역에 작성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UIUpdateTimer.Interval = Master.UIUpdateIntervalTime;

                LaguageCheck();
                NowTimeUpdate();
                LogOnTimeCheck();
                TopPanelStatusUpdate();
                FocusButtonUpdate();
                BasicInfoLabelUpdate();
                RackMasterAndPortValidUpdate();

                Update_MonitoringPortMap();
                Update_MonitoringRackMasterMap();

                CPUUsageStr = $"{ProcessCPU.ToString("0.0")} %" + (chx_CPUUpdate.Checked ? string.Empty : " (Last Value)");
                RAMUsageStr = $"{ProcessMemory.ToString("0.0")} Mbytes" + (chx_RAMUpdate.Checked ? string.Empty : " (Last Value)");
                LabelFunc.SetText(lbl_CPUUsage, CPUUsageStr);
                LabelFunc.SetText(lbl_RAMUsage, RAMUsageStr);

                Master.Update_Lbl_MasterAlarm(ref lbl_MasterAlarm);
                Master.Update_Lbl_MasterAlarmText(ref lbl_AlarmText);
                Master.Update_Lbl_WMXEngineStatus(ref lbl_WMXEngineStatusValue);
                Master.Update_Btn_BuzzerStop(ref btn_BuzzerStop);
                Master.Update_Btn_MasterAlarmClear(ref btn_MasterAlarmReset);
                Master.Update_Btn_AllAlarmClear(ref btn_AllAlarmReset);
                Master.Update_Btn_AllEquipPowerOn(ref btn_AllEquipPowerOn);
                Master.Update_Btn_AllEquipPowerOff(ref btn_AllEquipPowerOff);
                Master.Update_Btn_AllEquipCIMMode(ref btn_AllEquipCIMMode);
                Master.Update_Btn_AllEquipMasterMode(ref btn_AllEquipMasterMode);
                Master.Update_Btn_AllEquipAutoRun(ref btn_AllEquipAutoRun);
                Master.Update_Btn_AllEquipAutoStop(ref btn_AllEquipAutoStop);
                Master.Update_DGV_SaftyIOStatus(ref DGV_SaftyIOStatus, safetyImageInfo);
                Master.Update_DGV_EquipmentAutoRunStatus(ref DGV_EquipmentStatus);

                btn_Settings_IOMap.Visible = LogIn.GetLogInLevel() == LogIn.LogInLevel.Admin;
                btn_Save_IOMap.Visible = LogIn.GetLogInLevel() == LogIn.LogInLevel.Admin;

                SaftyImagePanelUpdate();
                GOTEventCheck();
            }
            catch{ }
            finally
            {
                //UIUpdateTimer.Enabled = true;
            }
        }
        
        /// <summary>
        /// I/O Map 객체에 대한 상태 업데이트
        /// </summary>
        private void SaftyImagePanelUpdate()
        {
            int nSaftyPanelHeight = panel_PictureBackPanel.Height;
            int nSaftyPanelWidth = panel_PictureBackPanel.Width;

            panel_Picture.Location = new Point((nSaftyPanelWidth - panel_Picture.Width) / 2, (nSaftyPanelHeight - panel_Picture.Height) / 2);

            foreach (var eSafetyItemList in Enum.GetValues(typeof(Master.DGV_SaftyIOStatusRow)))
            {
                int SafetyItemIndex = (int)eSafetyItemList;
                switch (eSafetyItemList)
                {
                    case Master.DGV_SaftyIOStatusRow.RM_GOT_EStop:
                        {
                            bool bErrorStatus = Master.Sensor_STK_Body_GOT_EMO;
                            LabelFunc.SetText(SafetyLabel[SafetyItemIndex], bErrorStatus ? "On" : "Off");
                            LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], bErrorStatus ? Master.ErrorIntervalColor : Color.Lime);
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.RM_HP_EMO:
                        {
                            bool bErrorStatus = Master.Sensor_STK_Body_HP_EMO;
                            LabelFunc.SetText(SafetyLabel[SafetyItemIndex], bErrorStatus ? "On" : "Off");
                            LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], bErrorStatus ? Master.ErrorIntervalColor : Color.Lime);
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.RM_OP_EMO:
                        {
                            bool bErrorStatus = Master.Sensor_STK_Body_OP_EMO;
                            LabelFunc.SetText(SafetyLabel[SafetyItemIndex], bErrorStatus ? "On" : "Off");
                            LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], bErrorStatus ? Master.ErrorIntervalColor : Color.Lime);
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_HP_DoorOpen:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.HP_Door_Open);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                bool bErrorStatus = Master.Sensor_HPDoorOpen;
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], bErrorStatus ? "On" : "Off");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], bErrorStatus ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_OP_DoorOpen:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.OP_Door_Open);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                bool bErrorStatus = Master.Sensor_OPDoorOpen;
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], bErrorStatus ? "On" : "Off");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], bErrorStatus ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_HP_EMO_Pushing:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.HP_Outside_EMO);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.mHPOutSide_EStop.GetEStopStateToStr());
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.mHPOutSide_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_HP_EMO_Escape_Status:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.HP_Inside_EMO);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.mHPInnerEscape_EStop.GetEStopStateToStr());
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.mHPInnerEscape_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_OP_EMO_Pushing:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.OP_Outside_EMO);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.mOPOutSide_EStop.GetEStopStateToStr());
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.mOPOutSide_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_OP_EMO_Escape_Status:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.OP_Inside_EMO);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.mOPInnerEscape_EStop.GetEStopStateToStr());
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.mOPInnerEscape_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_HP_MasterKey_AutoMode_Status:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.HP_AutoManual_Select_Key);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                bool bAutoStatus = Master.Sensor_HPAutoKey;
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], bAutoStatus ? "Auto" : "Manual");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], bAutoStatus ? Color.Lime : Color.Orange);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Bypass_Relay:
                        {
                            bool bEnable = Master.IsValidOutputItemMapping(Master.MasterOutputItem.Door_Bypass_Relay_On);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                bool bRelay = Master.CMD_DoorOpen_Relay;
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], bRelay ? "On" : "Off");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], bRelay ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Door_Open_Relay:
                        {
                            bool bEnable = Master.IsValidOutputItemMapping(Master.MasterOutputItem.Door_Open_Relay_On);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                bool bRelay = Master.CMD_DoorBypass_Relay;
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], bRelay ? "On" : "Off");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], bRelay ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_Maint_Door_Open:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.Maint_Door_Open) ||
                                            Master.IsValidInputItemMapping(Master.MasterInputItem.Maint_Door_Open2);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                bool bErrorStatus = Master.Sensor_MaintDoorOpen || Master.Sensor_MaintDoorOpen2;
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], bErrorStatus ? "On" : "Off");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], bErrorStatus ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_Port_Handy_Touch_EMO:
                        {
                            bool bEnable =
                                  Master.IsValidInputItemMapping(Master.MasterInputItem.Port_DTP_EMO) &&
                                  (Master.IsValidInputItemMapping(Master.MasterInputItem.Port_DTP_Mode1) ||
                                  Master.IsValidInputItemMapping(Master.MasterInputItem.Port_DTP_Mode2));
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.mPortHandyTouch_EStop.GetEStopStateToStr());
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.mPortHandyTouch_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_HP_Handy_Touch_EMO:
                        {
                            bool bEnable =
                               Master.IsValidInputItemMapping(Master.MasterInputItem.HP_DTP_EMO) &&
                               (Master.IsValidInputItemMapping(Master.MasterInputItem.HP_DTP_Mode1) ||
                               Master.IsValidInputItemMapping(Master.MasterInputItem.HP_DTP_Mode2));
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else 
                            { 
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.mHPHandyTouch_EStop.GetEStopStateToStr());
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.mHPHandyTouch_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_OP_Handy_Touch_EMO:
                        {
                            bool bEnable = 
                                Master.IsValidInputItemMapping(Master.MasterInputItem.OP_DTP_EMO) && 
                                (Master.IsValidInputItemMapping(Master.MasterInputItem.OP_DTP_Mode1) ||
                                Master.IsValidInputItemMapping(Master.MasterInputItem.OP_DTP_Mode2));
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.mOPHandyTouch_EStop.GetEStopStateToStr());
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.mOPHandyTouch_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_CPS_Run:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.CPS_Run);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.Sensor_CPSRun ? "On" : "Off");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.Sensor_CPSRun ? Color.Lime : Master.ErrorIntervalColor);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_CPS_Fault:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.CPS_Fault);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.Sensor_CPSError ? "On" : "Off");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.Sensor_CPSError ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_MCUL_Fault:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.MCUL_Fault);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.Sensor_MCULFault ? "On" : "Off");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.Sensor_MCULFault ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_Inner_EMO1:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.Inner_EMO_1);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.mDieBankInnerEMO_EStop.GetEStopStateToStr());
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.mDieBankInnerEMO_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_Inner_EMO2:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.Inner_EMO_2);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.mDieBankInnerEMO_EStop2.GetEStopStateToStr());
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.mDieBankInnerEMO_EStop2.IsEStop() ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_Inner_EMO3:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.Inner_EMO_3);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.mDieBankInnerEMO_EStop3.GetEStopStateToStr());
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.mDieBankInnerEMO_EStop3.IsEStop() ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                    case Master.DGV_SaftyIOStatusRow.Master_Inner_EMO4:
                        {
                            bool bEnable = Master.IsValidInputItemMapping(Master.MasterInputItem.Inner_EMO_4);
                            if (!bEnable)
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], "X");
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Color.DarkGray);
                            }
                            else
                            {
                                LabelFunc.SetText(SafetyLabel[SafetyItemIndex], Master.mDieBankInnerEMO_EStop4.GetEStopStateToStr());
                                LabelFunc.SetBackColor(SafetyLabel[SafetyItemIndex], Master.mDieBankInnerEMO_EStop4.IsEStop() ? Master.ErrorIntervalColor : Color.Lime);
                            }
                        }
                        break;
                }
            }
        }
        
        /// <summary>
        /// GOT 연결 상태, 화면 개수에 따라 잠금 화면 출력 조정 
        /// </summary>
        private void GOTEventCheck()
        {
            bool bScreen1Lock = false;

            if (Master.IsPortHandyTouchMainMonLock || Master.IsHPHandyTouchMainMonLock || Master.IsOPHandyTouchMainMonLock)
            {
                bScreen1Lock = true;
            }

            Screen[] screens = Screen.AllScreens;

            if (bScreen1Lock)
            {
                if (LogIn.GetLogInLevel() != LogIn.LogInLevel.GOT)
                {
                    LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.DTPConnectionInfo,
                        $"Port DTP:{Master.Sensor_PortHandyTouchMode2}, " +
                        $"HP DTP:{Master.Sensor_HPHandyTouchMode2}, " +
                        $"OP DTP:{Master.Sensor_OPHandyTouchMode2}," +
                        $"Screen Count :{screens.Length}");

                    
                    Screen scr = screens.First();

                    if (scr != null && screens.Length == 1)
                    {
                        //화면이 한개인 경우 최대화만 진행
                        LogIn.SetLogIn(LogIn.LogInLevel.GOT);
                        this.FormBorderStyle = FormBorderStyle.None;
                        this.Visible = true;
                        this.WindowState = FormWindowState.Maximized;
                        this.BringToFront();
                        this.Show();
                    }
                    else if (screens.Length == 2)
                    {
                        //화면이 두개인 경우 두번째 화면을 위치로 잡음

                        Screen GOTMonitor = GetScreenInfo(2);

                        if (GOTMonitor == null)
                            return;

                        LogIn.SetLogIn(LogIn.LogInLevel.GOT);
                        this.FormBorderStyle = FormBorderStyle.None;
                        this.Visible = true;

                        this.Location = new Point(GOTMonitor.Bounds.X, GOTMonitor.Bounds.Y);
                        this.Size = new Size(GOTMonitor.Bounds.Width, GOTMonitor.Bounds.Height);

                        this.WindowState = FormWindowState.Maximized;
                        this.BringToFront();
                        this.Show();

                        //if (btn_EquipNetworkSettings.Tag != null)
                        //{
                        //    Frm_EquipNetworkSettings frm_EquipNetworkSettings = (Frm_EquipNetworkSettings)btn_EquipNetworkSettings.Tag;
                        //    frm_EquipNetworkSettings.WindowState = FormWindowState.Normal;

                        //    frm_EquipNetworkSettings.Location = new Point(GOTMonitor.Bounds.X, GOTMonitor.Bounds.Y);
                        //    frm_EquipNetworkSettings.Size = new Size(GOTMonitor.Bounds.Width, GOTMonitor.Bounds.Height);
                        //    frm_EquipNetworkSettings.BringToFront();
                        //}

                        //if (btn_EquipMotionSettings.Tag != null)
                        //{
                        //    Frm_EquipMotionSettings frm_EquipMotionSettings = (Frm_EquipMotionSettings)btn_EquipMotionSettings.Tag;
                        //    frm_EquipMotionSettings.WindowState = FormWindowState.Normal;

                        //    frm_EquipMotionSettings.Location = new Point(GOTMonitor.Bounds.X, GOTMonitor.Bounds.Y);
                        //    frm_EquipMotionSettings.Size = new Size(GOTMonitor.Bounds.Width, GOTMonitor.Bounds.Height);
                        //    frm_EquipMotionSettings.BringToFront();
                        //}


                        if (btn_EquipNetworkSettings.Tag != null)
                        {
                            Frm_EquipNetworkSettings frm_EquipNetworkSettings = (Frm_EquipNetworkSettings)btn_EquipNetworkSettings.Tag;
                            frm_EquipNetworkSettings.Close();
                            frm_EquipNetworkSettings.Dispose();
                            btn_EquipNetworkSettings.Tag = null;

                            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Network Settings Form Closing [DTP Connection]");
                        }

                        if (btn_EquipMotionSettings.Tag != null)
                        {
                            Frm_EquipMotionSettings frm_EquipMotionSettings = (Frm_EquipMotionSettings)btn_EquipMotionSettings.Tag;
                            frm_EquipMotionSettings.Close();
                            frm_EquipMotionSettings.Dispose();
                            btn_EquipMotionSettings.Tag = null;

                            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Motion Settings Form Closing [DTP Connection]");
                        }

                        if (btn_MasterCtrlForm.Tag != null)
                        {
                            Frm_MasterCtrlScreen frm_MasterCtrlScreen = (Frm_MasterCtrlScreen)btn_MasterCtrlForm.Tag;
                            frm_MasterCtrlScreen.WindowState = FormWindowState.Normal;

                            frm_MasterCtrlScreen.Location = new Point(GOTMonitor.Bounds.X, GOTMonitor.Bounds.Y);
                            frm_MasterCtrlScreen.Size = new Size(GOTMonitor.Bounds.Width, GOTMonitor.Bounds.Height);
                            frm_MasterCtrlScreen.BringToFront();
                        }

                        if (btn_RMTPForm.Tag != null)
                        {
                            Frm_RackMasterTPScreen frm_RMTPScreen = (Frm_RackMasterTPScreen)btn_RMTPForm.Tag;
                            frm_RMTPScreen.WindowState = FormWindowState.Normal;
                            frm_RMTPScreen.Location = new Point(GOTMonitor.Bounds.X, GOTMonitor.Bounds.Y);
                            frm_RMTPScreen.Size = new Size(GOTMonitor.Bounds.Width, GOTMonitor.Bounds.Height);
                            frm_RMTPScreen.BringToFront();
                        }

                        if (btn_PortTPForm.Tag != null)
                        {
                            Frm_PortTPScreen frm_PortTPScreen = (Frm_PortTPScreen)btn_PortTPForm.Tag;
                            frm_PortTPScreen.WindowState = FormWindowState.Normal;
                            frm_PortTPScreen.Location = new Point(GOTMonitor.Bounds.X, GOTMonitor.Bounds.Y);
                            frm_PortTPScreen.Size = new Size(GOTMonitor.Bounds.Width, GOTMonitor.Bounds.Height);
                            frm_PortTPScreen.BringToFront();
                        }
                    }
                }
            }
            else
            {
                if (LogIn.GetLogInLevel() == LogIn.LogInLevel.GOT)
                {
                    ////주 모니터 위치로 복귀
                    ///
                    Screen MainMonitor = GetScreenInfo(1);

                    if (MainMonitor == null)
                        return;

                    this.WindowState = FormWindowState.Normal;
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.Location = new Point(MainMonitor.Bounds.X, MainMonitor.Bounds.Y);
                    this.Size = new Size(MainMonitor.Bounds.Width, MainMonitor.Bounds.Height);
                    this.BringToFront();
                    this.Show();
                    LogIn.SetLogout();
                }
            }

            if (screens.Length >= 2)
            {
                if (bScreen1Lock && frm_Lock[0] == null)
                {
                    ////주 모니터 위치에 락
                    Screen MainMonitor = GetScreenInfo(1);

                    if (MainMonitor == null)
                        return;

                    frm_Lock[0] = new Frm_Lock();

                    frm_Lock[0].Location = new Point(MainMonitor.Bounds.X, MainMonitor.Bounds.Y);
                    frm_Lock[0].Size = new Size(MainMonitor.Bounds.Width, MainMonitor.Bounds.Height);
                    frm_Lock[0].TopMost = true;
                    frm_Lock[0].Show();
                }
                else if(!bScreen1Lock && frm_Lock[0] != null)
                {
                    frm_Lock[0].CloseEnable = true;
                    frm_Lock[0].TopMost = false;
                    frm_Lock[0].Close();
                    frm_Lock[0].Dispose();
                    frm_Lock[0] = null;
                }
            }
            else
            {
                //스크린이 1개인 경우
                if (frm_Lock[0] != null)
                {
                    frm_Lock[0].CloseEnable = true;
                    frm_Lock[0].TopMost = false;
                    frm_Lock[0].Close();
                    frm_Lock[0].Dispose();
                    frm_Lock[0] = null;
                }
            }

            if(screens.Length == 3)
            {
                Screen TPMonitor = GetScreenInfo(3);
                if (TPMonitor == null)
                    return;

                //Monitor 3개인 경우
                if (frm_SecondPortTPScreen == null)
                {
                    frm_SecondPortTPScreen = new Frm_PortTPScreen(true);
                    frm_SecondPortTPScreen.Disposed += Frm_SecondPortTPScreen_Disposed;
                    frm_SecondPortTPScreen.FormBorderStyle = FormBorderStyle.None;
                    frm_SecondPortTPScreen.Visible = true;

                    frm_SecondPortTPScreen.Location = new Point(TPMonitor.Bounds.X, TPMonitor.Bounds.Y);
                    frm_SecondPortTPScreen.Size = new Size(TPMonitor.Bounds.Width, TPMonitor.Bounds.Height);
                    frm_SecondPortTPScreen.SetAutoScale(TPMonitor.Bounds.Width / 1920.0f, TPMonitor.Bounds.Height / 1080.0f);
                    frm_SecondPortTPScreen.WindowState = FormWindowState.Maximized;
                    frm_SecondPortTPScreen.BringToFront();
                    frm_SecondPortTPScreen.Btn_Disable();
                    frm_SecondPortTPScreen.Show();
                }
                else
                {
                    bool bSecondPortScreenLock = btn_PortTPForm.Tag != null || btn_EquipMotionSettings.Tag != null || btn_EquipNetworkSettings.Tag != null;
                    frm_SecondPortTPScreen.Enabled = !bSecondPortScreenLock;

                    if (!frm_SecondPortTPScreen.Visible)
                    {
                        frm_SecondPortTPScreen.Visible = true;
                        frm_SecondPortTPScreen.BringToFront();
                    }
                }
            }
            else
            {
                if (frm_SecondPortTPScreen != null)
                {
                    frm_SecondPortTPScreen.Close();
                    if(frm_SecondPortTPScreen != null && !frm_SecondPortTPScreen.IsDisposed)
                        frm_SecondPortTPScreen.Dispose();
                    frm_SecondPortTPScreen = null;
                }
            }
        }

        private Screen GetScreenInfo(int ScreenNum)
        {
            Screen[] screens = Screen.AllScreens;

            List<Screen> screenLists = new List<Screen>();

            foreach (var screen in screens)
            {
                screenLists.Add(screen);
            }

            screenLists.Sort((X1, X2) => X1.Bounds.X.CompareTo(X2.Bounds.X));

            if (ScreenNum > screenLists.Count)
                return null;
            else
                return screenLists.ElementAt(ScreenNum-1);
        }
        /// <summary>
        /// Second Port 제어 폼 종료 시 dispose 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_SecondPortTPScreen_Disposed(object sender, EventArgs e)
        {
            frm_SecondPortTPScreen.Disposed -= Frm_SecondPortTPScreen_Disposed;
            frm_SecondPortTPScreen = null;
        }

        /// <summary>
        /// Login 시간 체크 및 Login Level에 따른 버튼 활성화
        /// Logoff시 활성 폼 종료
        /// </summary>
        private void LogOnTimeCheck()
        {
            LogIn.LogOnRemaningTimeLabelUpdate(ref lbl_RemainingTime);
            LogIn.LogOnExtendButtonUpdate(ref btn_LogInExtend);
            
            ButtonFunc.SetImage(btn_LogIn, LogIn.IsLogIn() ? Properties.Resources.icons8_unlock_48 : Properties.Resources.icons8_lock_48);
            ButtonFunc.SetBackColor(btn_LogIn, LogIn.IsLogIn() ? Color.LightGreen : Color.White);

            if (LogIn.IsLogIn())
            {
                ButtonFunc.SetEnable(btn_EquipNetworkSettings, LogIn.GetLogInLevel() == LogIn.LogInLevel.Admin ? true : false);
                ButtonFunc.SetEnable(btn_EquipMotionSettings, LogIn.GetLogInLevel() == LogIn.LogInLevel.Admin ? true : false);
                ButtonFunc.SetEnable(btn_MasterCtrlForm, LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint ? true : false);

                ButtonFunc.SetEnable(btn_LogSet, LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint ? true : false);
                ButtonFunc.SetEnable(btn_AllEquipPowerOn, LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint ? true : false);
                ButtonFunc.SetEnable(btn_AllEquipPowerOff, LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint ? true : false);
                ButtonFunc.SetEnable(btn_AllEquipCIMMode, LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint ? true : false);
                ButtonFunc.SetEnable(btn_AllEquipMasterMode, LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint ? true : false);
                ButtonFunc.SetEnable(btn_AllEquipAutoRun, LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint ? true : false);
                ButtonFunc.SetEnable(btn_AllEquipAutoStop, LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint ? true : false);
            }
            else
            {
                ButtonFunc.SetEnable(btn_EquipNetworkSettings, false);
                ButtonFunc.SetEnable(btn_EquipMotionSettings, false);
                ButtonFunc.SetEnable(btn_MasterCtrlForm, false);
                ButtonFunc.SetEnable(btn_LogSet, false);

                ButtonFunc.SetEnable(btn_AllEquipPowerOn, false);
                ButtonFunc.SetEnable(btn_AllEquipPowerOff, false);
                ButtonFunc.SetEnable(btn_AllEquipCIMMode, false);
                ButtonFunc.SetEnable(btn_AllEquipMasterMode, false);
                ButtonFunc.SetEnable(btn_AllEquipAutoRun, false);
                ButtonFunc.SetEnable(btn_AllEquipAutoStop, false);

                if (btn_EquipNetworkSettings.Tag != null)
                {
                    Frm_EquipNetworkSettings frm_EquipNetworkSettings = (Frm_EquipNetworkSettings)btn_EquipNetworkSettings.Tag;
                    frm_EquipNetworkSettings.Close();
                    frm_EquipNetworkSettings.Dispose();
                    btn_EquipNetworkSettings.Tag = null;

                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Network Settings Form Logout Closing");
                }

                if (btn_EquipMotionSettings.Tag != null)
                {
                    Frm_EquipMotionSettings frm_EquipMotionSettings = (Frm_EquipMotionSettings)btn_EquipMotionSettings.Tag;
                    frm_EquipMotionSettings.Close();
                    frm_EquipMotionSettings.Dispose();
                    btn_EquipMotionSettings.Tag = null;

                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Motion Settings Form Logout Closing");
                }

                if (btn_MasterCtrlForm.Tag != null)
                {
                    Frm_MasterCtrlScreen frm_MasterCtrlScreen = (Frm_MasterCtrlScreen)btn_MasterCtrlForm.Tag;
                    frm_MasterCtrlScreen.Close();
                    frm_MasterCtrlScreen.Dispose();
                    btn_MasterCtrlForm.Tag = null;

                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Master Control Form Logout Closing");
                }

                if (btn_PortTPForm.Tag != null)
                {
                    Frm_PortTPScreen frm_PortTPScreen = (Frm_PortTPScreen)btn_PortTPForm.Tag;
                    frm_PortTPScreen.Close();
                    frm_PortTPScreen.Dispose();
                    btn_PortTPForm.Tag = null;

                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Port Control Form Logout Closing");
                }

                if (btn_RMTPForm.Tag != null)
                {
                    Frm_RackMasterTPScreen frm_RackMasterTPScreen = (Frm_RackMasterTPScreen)btn_RMTPForm.Tag;
                    frm_RackMasterTPScreen.Close();
                    frm_RackMasterTPScreen.Dispose();
                    btn_RMTPForm.Tag = null;

                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"RackMaster Control Form Logout Closing");
                }

                if (btn_LogSet.Tag != null)
                {
                    Frm_LogSet frm_LogSet = (Frm_LogSet)btn_LogSet.Tag;
                    frm_LogSet.Close();
                    frm_LogSet.Dispose();
                    btn_LogSet.Tag = null;

                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Log Settings Form Logout Closing");
                }
            }
        }
        
        /// <summary>
        /// Language Type에 따라 언어 변경
        /// </summary>
        private void LaguageCheck()
        {
            FormFunc.SetText(this, SynusLangPack.GetLanguage("Frm_Main_FormTitle"));

            LabelFunc.SetText(lbl_MasterVersionTitle, SynusLangPack.GetLanguage("Label_MasterVersion") + " :");
            LabelFunc.SetText(lbl_NowTimeTitle, SynusLangPack.GetLanguage("Label_NowTime") + " :");
            LabelFunc.SetText(lbl_RemainingTimeTitle, SynusLangPack.GetLanguage("Label_RemainingTime") + " :");

            LabelFunc.SetText(lbl_Monitoring, SynusLangPack.GetLanguage("Label_Monitoring"));
            LabelFunc.SetText(lbl_Log, SynusLangPack.GetLanguage("Label_Log"));

            LabelFunc.SetText(lbl_MasterAlarmCodeTitle, SynusLangPack.GetLanguage("Label_MasterAlarmCode") + " :");
            LabelFunc.SetText(lbl_MasterAlarmTextTitle, SynusLangPack.GetLanguage("Label_MasterAlarmText") + " :");
            LabelFunc.SetText(lbl_CPUUsageTitle, SynusLangPack.GetLanguage("Label_CPUUsage") + " :");
            LabelFunc.SetText(lbl_RAMUsageTitle, SynusLangPack.GetLanguage("Label_RAMUsage") + " :");
            LabelFunc.SetText(lbl_WMXEngineStatusTitle, SynusLangPack.GetLanguage("Label_WMXEngineStatus") + " :");

            ButtonFunc.SetText(btn_LogIn, LogIn.IsLogIn() ? SynusLangPack.GetLanguage("Btn_Logout") : SynusLangPack.GetLanguage("Btn_LogIn"));
            ButtonFunc.SetText(btn_Language, SynusLangPack.GetLanguage("Btn_LanguageSetting"));
            ButtonFunc.SetText(btn_EquipNetworkSettings, SynusLangPack.GetLanguage("Btn_EquipSettings"));
            ButtonFunc.SetText(btn_EquipMotionSettings, SynusLangPack.GetLanguage("Btn_MotionSettings"));
            ButtonFunc.SetText(btn_MasterCtrlForm, SynusLangPack.GetLanguage("Btn_MasterCtrlForm"));
            ButtonFunc.SetText(btn_RMTPForm, SynusLangPack.GetLanguage("Btn_RMTPForm"));
            ButtonFunc.SetText(btn_PortTPForm, SynusLangPack.GetLanguage("Btn_PortTPForm"));

            GroupBoxFunc.SetText(groupBox_BasicInfo, SynusLangPack.GetLanguage("GorupBox_BasicInfo"));
            GroupBoxFunc.SetText(groupBox_MasterStatus, SynusLangPack.GetLanguage("GorupBox_MasterStatus"));
            GroupBoxFunc.SetText(groupBox_MasterEquipment, SynusLangPack.GetLanguage("GorupBox_MasterEquipment"));
            GroupBoxFunc.SetText(groupBox_EquipmentControls, SynusLangPack.GetLanguage("GorupBox_EquipmentControls"));
            GroupBoxFunc.SetText(groupBox_EquipmentStatus, SynusLangPack.GetLanguage("GorupBox_MasterEquipmentStatus"));
            GroupBoxFunc.SetText(groupBox_SafetyIOStatus, SynusLangPack.GetLanguage("GroupBox_SafetyIOStatus"));
            GroupBoxFunc.SetText(groupBox_SafetyIOMap, SynusLangPack.GetLanguage("GroupBox_SafetyIOMap"));

            GroupBoxFunc.SetText(groupBox_RackMasterMap, SynusLangPack.GetLanguage("GroupBox_RackMasterMap"));
            GroupBoxFunc.SetText(groupBox_PortMap, SynusLangPack.GetLanguage("GroupBox_PortMap"));

            TabPageFunc.SetText(tabPage_Safty, SynusLangPack.GetLanguage("TabPage_EquipmentSaftyStates"));
            TabPageFunc.SetText(tabPage_Equipments, SynusLangPack.GetLanguage("TabPage_EquipmentControls"));
            TabPageFunc.SetText(tabPage_MonitoringMap, SynusLangPack.GetLanguage("TabPage_MonitoringMap"));

            ButtonFunc.SetText(btn_Safty, SynusLangPack.GetLanguage("TabPage_EquipmentSaftyStates"));
            ButtonFunc.SetText(btn_Monitoring, SynusLangPack.GetLanguage("TabPage_MonitoringMap"));

            if (notifyIcon1.ContextMenu.MenuItems.Count == 3)
            {
                notifyIcon1.ContextMenu.MenuItems[0].Text = SynusLangPack.GetLanguage("Open");
                notifyIcon1.ContextMenu.MenuItems[2].Text = SynusLangPack.GetLanguage("Close");
            }
        }
        
        /// <summary>
        /// 현재 시간 업데이트
        /// </summary>
        private void NowTimeUpdate()
        {
            LabelFunc.SetText(lbl_NowTime, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
        }

        /// <summary>
        /// Main Form 최 상단 TCP/IP 관련 상태 패널 업데이트
        /// </summary>
        private void TopPanelStatusUpdate()
        {
            Master.Update_Lbl_CIMOnline(ref lbl_CIMOnline);

            if (Master.m_RackMasters == null)
            {
                PanelFunc.SetVisible(pnl_RM0Online, false);
                PanelFunc.SetVisible(pnl_RM1Online, false);
            }
            else
            {
                int nRMVisibleCount = pnl_RM0Online.Visible && pnl_RM1Online.Visible ? 2 : pnl_RM0Online.Visible || pnl_RM1Online.Visible ? 1 : 0;
                if (Master.m_RackMasters.Count != nRMVisibleCount)
                {
                    PanelFunc.SetVisible(pnl_RM0Online, false);
                    PanelFunc.SetVisible(pnl_RM1Online, false);

                    foreach (var rackMaster in Master.m_RackMasters.Select((value, index) => (value, index)))
                    {
                        if (rackMaster.index == 0)
                            PanelFunc.SetVisible(pnl_RM0Online, true);
                        if (rackMaster.index == 1)
                            PanelFunc.SetVisible(pnl_RM1Online, true);
                    }
                }
                else
                {
                    foreach(var rackMaster in Master.m_RackMasters.Select((value, index) => (value, index)))
                    {
                        string ID = rackMaster.value.Value.GetParam().ID;
                        bool bRMConnection = rackMaster.value.Value.IsConnected();

                        if (rackMaster.index == 0)
                        {
                            rackMaster.value.Value.Update_Lbl_RackMasterTitleID(ref lbl_RM0ID);
                            rackMaster.value.Value.Update_Lbl_RackMasterOnline(ref lbl_RM0Online);
                            PanelFunc.SetVisible(pnl_RM0Online, true);
                        }
                        if (rackMaster.index == 1)
                        {
                            rackMaster.value.Value.Update_Lbl_RackMasterTitleID(ref lbl_RM1ID);
                            rackMaster.value.Value.Update_Lbl_RackMasterOnline(ref lbl_RM1Online);
                            PanelFunc.SetVisible(pnl_RM1Online, true);
                        }
                    }
                }
            }

            if(Master.m_CPS?.m_CPSEnable ?? false)
            {
                PanelFunc.SetVisible(pnl_CPSOnline, true);
                LabelFunc.SetVisible(lbl_CPSPowerOnEnableLamp, true);
                LabelFunc.SetVisible(lbl_CPSPowerOnReqLamp, true);
                LabelFunc.SetVisible(lbl_CPSErrorResetReqLamp, true);
                LabelFunc.SetVisible(lbl_CPSErrorLamp, true);

                Master.Update_Lbl_CPSOnline(ref lbl_CPSOnline);
                Master.Update_Lbl_CPSPowerOnEnableLamp(ref lbl_CPSPowerOnEnableLamp);
                Master.Update_Lbl_CPSPowerOnReqLamp(ref lbl_CPSPowerOnReqLamp);
                Master.Update_Lbl_CPSErrorResetReqLamp(ref lbl_CPSErrorResetReqLamp);
                Master.Update_Lbl_CPSErrorLamp(ref lbl_CPSErrorLamp);
            }
            else
            {
                PanelFunc.SetVisible(pnl_CPSOnline, false);
                LabelFunc.SetVisible(lbl_CPSPowerOnEnableLamp, false);
                LabelFunc.SetVisible(lbl_CPSPowerOnReqLamp, false);
                LabelFunc.SetVisible(lbl_CPSErrorResetReqLamp, false);
                LabelFunc.SetVisible(lbl_CPSErrorLamp, false);
            }

            if (Master.m_Omron?.m_OmronEnable ?? false)
            {
                PanelFunc.SetVisible(pnl_OmronOnline, true);
                Master.Update_Lbl_OMRONOnline(ref lbl_OMRON);
            }
            else
            {
                PanelFunc.SetVisible(pnl_OmronOnline, false);
            }
        }

        /// <summary>
        /// 장비 모니터링용 버튼 업데이트(우클릭, 좌클릭에 따른)
        /// </summary>
        private void FocusButtonUpdate()
        {
            object FocusPage = tableLayoutPanel_FocusButton.Tag;

            if (FocusPage == null || (int)FocusPage < 0)
                FocusPage = 0;

            int StartPage = (int)FocusPage * 5;

            int PageStep = 0;
            int ButtonStep = 0;

            if (Master.m_RackMasters != null)
            {
                foreach (var rackMaster in Master.m_RackMasters)
                {
                    if (PageStep >= StartPage)
                    {
                        if (ButtonStep < m_CommanderButton.Length)
                        {
                            ButtonFunc.SetText(m_CommanderButton[ButtonStep], rackMaster.Value.GetFocusButtonStr());
                            if (m_CommanderButton[ButtonStep].Tag != rackMaster.Value)
                            {
                                m_CommanderButton[ButtonStep].Tag = rackMaster.Value;
                                if(!m_CommanderButton[ButtonStep].Visible)
                                    m_CommanderButton[ButtonStep].Visible = true;
                            }
                        }

                        if (ButtonStep < 5)
                            ButtonStep++;
                    }
                    PageStep++;
                }
            }
            if (Master.m_Ports != null)
            {
                foreach (var port in Master.m_Ports)
                {
                    if (PageStep >= StartPage)
                    {
                        if (ButtonStep < m_CommanderButton.Length)
                        {
                            ButtonFunc.SetText(m_CommanderButton[ButtonStep], port.Value.GetFocusButtonStr());
                            if (m_CommanderButton[ButtonStep].Tag != port.Value)
                            {
                                m_CommanderButton[ButtonStep].Tag = port.Value;

                                if (!m_CommanderButton[ButtonStep].Visible)
                                    m_CommanderButton[ButtonStep].Visible = true;
                            }

                        }

                        if (ButtonStep < 5)
                            ButtonStep++;
                    }
                    PageStep++;
                }
            }


            int EquipmentCount = (Master.m_RackMasters?.Count ?? 0) + (Master.m_Ports?.Count ?? 0);

            ButtonFunc.SetEnable(btn_CommanderNext, ((int)FocusPage * 5 + 5 >= EquipmentCount) ? false : true);
            ButtonFunc.SetEnable(btn_CommanderPrev, ((int)FocusPage <= 0) ? false : true);

            if (panel_ControlMonitorWork.Tag == null && m_CommanderButton.Length > 0 && m_CommanderButton[0].Tag != null)
            {
                btn_CommanderEquip_Click(m_CommanderButton[0], new EventArgs());
            }

            for (int nCount = 0; nCount < m_CommanderButton.Length; nCount++)
            {
                if (panel_ControlMonitorWork.Tag != null)
                {
                    if (panel_ControlMonitorWork.Tag == frm_ControlMonRM)
                    {
                        if (m_CommanderButton[nCount].Tag == frm_ControlMonRM.GetRackMaster())
                            m_CommanderButton[nCount].BackColor = Color.Lime;
                        else
                            m_CommanderButton[nCount].BackColor = Color.White;
                    }
                    else if (panel_ControlMonitorWork.Tag == frm_ControlMonPort)
                    {
                        if (m_CommanderButton[nCount].Tag == frm_ControlMonPort.GetPort())
                            m_CommanderButton[nCount].BackColor = Color.Lime;
                        else
                            m_CommanderButton[nCount].BackColor = Color.White;
                    }
                }
                else
                    m_CommanderButton[nCount].BackColor = Color.White;
            }
        }
        
        /// <summary>
        /// 장비 라벨 업데이트(우클릭, 좌클릭에 따른)
        /// </summary>
        private void BasicInfoLabelUpdate()
        {
            object BasicInfoPage = tableLayoutPanel_BasicInfo.Tag;

            if (BasicInfoPage == null || (int)BasicInfoPage < 0)
                BasicInfoPage = 0;

            int StartPage = (int)BasicInfoPage * 5;

            int PageStep = 0;
            int LabelStep = 0;

            if (Master.m_RackMasters != null)
            {
                foreach (var rackMaster in Master.m_RackMasters)
                {
                    if (PageStep >= StartPage)
                    {
                        if (LabelStep < m_CommanderButton.Length)
                        {
                            rackMaster.Value.Update_Lbl_RackMasterInfoLabel(ref m_BasicInfoLabel[LabelStep], Equipment.RackMaster.RackMaster.RackMasterInfoType.Detail);
                        }

                        if (LabelStep < 5)
                            LabelStep++;
                    }
                    PageStep++;
                }
            }
            if (Master.m_Ports != null)
            {
                foreach (var port in Master.m_Ports)
                {
                    if (PageStep >= StartPage)
                    {
                        if (LabelStep < m_CommanderButton.Length)
                        {
                            port.Value.Update_Lbl_PortInfoLabel(ref m_BasicInfoLabel[LabelStep], Equipment.Port.Port.PortInfoType.Detail);
                        }

                        if (LabelStep < 5)
                            LabelStep++;
                    }
                    PageStep++;
                }
            }


            int EquipmentCount = (Master.m_RackMasters?.Count ?? 0) + (Master.m_Ports?.Count ?? 0);

            ButtonFunc.SetEnable(btn_BasicInfoNext, ((int)BasicInfoPage * 5 + 5 >= EquipmentCount) ? false : true);
            ButtonFunc.SetEnable(btn_BasicInfoPrev, ((int)BasicInfoPage <= 0) ? false : true);
        }
        
        /// <summary>
        /// 장비 할당 상태에 따라 제어 폼 출력 버튼 enable 조정
        /// </summary>
        private void RackMasterAndPortValidUpdate()
        {
            if ((Master.m_RackMasters?.Count ?? 0) > 0)
            {
                ButtonFunc.SetEnable(btn_RMTPForm, LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint);
            }
            else
            {
                ButtonFunc.SetEnable(btn_RMTPForm, false);

                if (btn_RMTPForm.Tag != null)
                {
                    Frm_RackMasterTPScreen frm_RackMasterTPScreen = (Frm_RackMasterTPScreen)btn_RMTPForm.Tag;
                    frm_RackMasterTPScreen.Close();
                    frm_RackMasterTPScreen.Dispose();
                    btn_RMTPForm.Tag = null;
                }
            }

            if((Master.m_Ports?.Count ?? 0) > 0)
                ButtonFunc.SetEnable(btn_PortTPForm, LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint);
            else
            {
                ButtonFunc.SetEnable(btn_PortTPForm, false);

                if (btn_PortTPForm.Tag != null)
                {
                    Frm_PortTPScreen frm_PortTPScreen = (Frm_PortTPScreen)btn_PortTPForm.Tag;
                    frm_PortTPScreen.Close();
                    frm_PortTPScreen.Dispose();
                    btn_PortTPForm.Tag = null;
                }
            }
        }
        
        /// <summary>
        /// Mainform에서 Login 버튼 클릭 시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LogIn_Click(object sender, EventArgs e)
        {
            if (!LogIn.IsLogIn())
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Login Click");

                Frm_Login frm_Login = new Frm_Login();
                frm_Login.StartPosition = FormStartPosition.CenterParent;
                DialogResult result = frm_Login.ShowDialog();
                frm_Login.Dispose();

                if(result != DialogResult.OK)
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Login Cancel Click");
            }
            else
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Logout Click");
                if(LogIn.GetLogInLevel() != LogIn.LogInLevel.GOT)
                    LogIn.SetLogout();
                else
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Warning, LogMsg.MsgList.LogOutReject, $"Cannot log out of got mode.");
            }
        }
        
        /// <summary>
        /// Mainform에서 Languege 버튼 클릭 시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Language_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Language Settings Click");

            Frm_Language frm_Language = new Frm_Language();
            frm_Language.StartPosition = FormStartPosition.CenterParent;
            DialogResult result = frm_Language.ShowDialog();

            if (result == DialogResult.OK)
            {
                SynusLangPack.SetLanguageType(ApplicationParam.m_ApplicationParam.eLangType);
            }
            else
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Language Type Settings Cancle");

            frm_Language.Dispose();
        }

        /// <summary>
        /// Mainform에서 장비 네트워크 설정 버튼 클릭 시 이벤트
        /// 스토커 및 포트가 공정 중인 경우 불가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_EquipmentSettings_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Network Settings Click");

            if (LogIn.IsLogIn())
            {
                foreach (var rackMaster in Master.m_RackMasters)
                {
                    if (rackMaster.Value.Status_AutoMode ||
                        rackMaster.Value.IsAutoCycleRun() ||
                        rackMaster.Value.IsAutoTeachingRun())
                    {
                        MessageBox.Show( $"RackMaster ID = [{rackMaster.Value.GetParam().ID}]" + SynusLangPack.GetLanguage("Message_RejectCommandInWorking"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                foreach (var port in Master.m_Ports)
                {
                    if (port.Value.IsAutoControlRun() ||
                        port.Value.IsAutoManualCycleRun() ||
                        port.Value.IsPortBusy())
                    {
                        MessageBox.Show($"Port ID = [{port.Value.GetParam().ID}]" + SynusLangPack.GetLanguage("Message_RejectCommandInWorking"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                Frm_EquipNetworkSettings frm_EquipNetworkSettings = new Frm_EquipNetworkSettings();
                frm_EquipNetworkSettings.StartPosition = FormStartPosition.CenterParent;
                btn_EquipNetworkSettings.Tag = frm_EquipNetworkSettings;
                DialogResult result = frm_EquipNetworkSettings.ShowDialog();
                frm_EquipNetworkSettings.Dispose();
                btn_EquipNetworkSettings.Tag = null;

                if (result == DialogResult.Yes)
                {
                    if(EquipNetworkParam.SaveFile(ManagedFileInfo.EquipNetworkParamDirectory, ManagedFileInfo.EquipNetworkParamFileName))
                    {
                        //1. 장비 Init 전 default Parameter file 생성
                        AutomaticCreateParameterFile();
                        //2. Init Sequence 재 실행
                        Master.EquipmentInit();
                        //3. Load된 Equipment 기준으로 Form Load
                        MonitoringEquipmentFormReset();
                        //4. 사용하지 않는 장비 파라미터 파일 cleanup directory에 backup
                        AutomaticCleanupParameterFiles();
                        //5. 없는 포트의 CST ID 삭제
                        CassetteInfo.AutomaticCleanupCSTInfoFile();
                        //6. 포트 및 스토커 모니터링 맵 조정
                        Load_MonitoringPortMap();
                        Load_MonitoringRackMasterMap();
                    }
                }
            }
        }
        
        /// <summary>
        /// 사용하지 않는 포트 파일을 정리함으로 이동
        /// </summary>
        /// <param name="filePath"></param>
        private void CleanupFile(string filePath)
        {
            string CleanupDirectoryName = "CleanupPortParamFile";
            string CleanupdirectoryPath = ManagedFileInfo.EquipMotionParamDirectory + $"\\{CleanupDirectoryName}";
            if (!Directory.Exists(CleanupdirectoryPath))
                Directory.CreateDirectory(CleanupdirectoryPath);

            string FileName = Path.GetFileName(filePath);
            string MoveName = DateTime.Now.ToString("yyyyMMdd_HH_mm_ss") + $"_{FileName}";
            string MoveFilePath = CleanupdirectoryPath + $"\\{MoveName}";

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileMove, $"Un Used Port File : {FileName}");
            File.Move(filePath, MoveFilePath);
        }

        /// <summary>
        /// 장비 Initialize 이후 Parameter File이 없는 경우 Default File 생성
        /// </summary>
        private void AutomaticCreateParameterFile()
        {
            foreach (var port in ManagedFile.EquipNetworkParam.m_PortNetworkParams)
            {
                string MotionfilePath = ManagedFileInfo.EquipMotionParamDirectory + @"\" + $"{port.Value.ID}_{ManagedFileInfo.EquipMotionParamFileName}";

                if (!File.Exists(MotionfilePath))
                {
                    EquipPortMotionParam.PortMotionParameter PortMotionParameter = new EquipPortMotionParam.PortMotionParameter();
                    PortMotionParameter.Save(port.Value.ID, PortMotionParameter);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileCreate, $"New Port[{port.Value.ID}, {port.Value.ePortType}], Default Motion Parameter File Create");
                }

                string UIfilePath = ManagedFileInfo.PortUIParamDirectory + @"\" + $"{port.Value.ID}_{ManagedFileInfo.PortUIParamFileName}"; 

                if (!File.Exists(UIfilePath))
                {
                    EquipPortMotionParam.Port_UIParam PortUIParameter = new EquipPortMotionParam.Port_UIParam();
                    PortUIParameter.Save(port.Value.ID, PortUIParameter);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileCreate, $"New Port[{port.Value.ID}, {port.Value.ePortType}], Default UI Parameter File Create");
                }
            }
        }
        
        /// <summary>
        /// 장비 Initialize 이후 사용하지 않는 Port의 Parameter File을 정리
        /// </summary>
        private void AutomaticCleanupParameterFiles()
        {
            List<string> UsePortID = new List<string>();
            foreach(var port in Master.m_Ports)
            {
                UsePortID.Add(port.Value.GetParam().ID);
            }

            DirectoryInfo di = new DirectoryInfo(ManagedFileInfo.EquipMotionParamDirectory);

            string[] files = Directory.GetFiles(di.FullName, "*.*", SearchOption.AllDirectories);

            foreach(var file in files)
            {
                string fileName = Path.GetFileName(file);
                if (fileName.Split('_').Length == 2)
                {
                    string filePortID = fileName.Split('_')[0];
                    string fileType = fileName.Split('_')[1];

                    if (!UsePortID.Contains(filePortID))
                    {
                        if((Path.GetFileNameWithoutExtension(fileType) == Path.GetFileNameWithoutExtension(ManagedFileInfo.EquipMotionParamFileName)) ||
                            (Path.GetFileNameWithoutExtension(fileType) == Path.GetFileNameWithoutExtension(ManagedFileInfo.PortUIParamFileName)))
                        {
                            CleanupFile(file);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 포트 축 설정 관련 폼 출력
        /// 포트가 공정 중인 경우 불가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_EquipMotionSettings_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Motion Settings Click");

            if (LogIn.IsLogIn())
            {
                foreach (var port in Master.m_Ports)
                {
                    if (port.Value.IsAutoControlRun() ||
                        port.Value.IsAutoManualCycleRun() ||
                        port.Value.IsPortBusy())
                    {
                        MessageBox.Show($"Port ID = [{port.Value.GetParam().ID}]" + SynusLangPack.GetLanguage("Message_RejectCommandInWorking"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                Frm_EquipMotionSettings frm_EquipMotionSettings = new Frm_EquipMotionSettings();
                frm_EquipMotionSettings.StartPosition = FormStartPosition.CenterParent;
                btn_EquipMotionSettings.Tag = frm_EquipMotionSettings;
                DialogResult result = frm_EquipMotionSettings.ShowDialog();
                frm_EquipMotionSettings.Dispose();
                btn_EquipMotionSettings.Tag = null;

                if (result == DialogResult.Yes)
                {
                    foreach (var port in Master.m_Ports)
                    {
                        if (port.Value.GetMotionParam().Save(port.Value.GetParam().ID, port.Value.GetMotionParam()))
                            port.Value.InitMotionParam();
                    }
                }
            }
        }

        /// <summary>
        /// Buzzer Stop 버튼 클릭시 상황에 따라 Mute 제어
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BuzzerStop_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, Master.CMD_Buzzer_Mute_REQ ? $"MainForm-Buzzer Release Click" : $"MainForm-Buzzer Stop Click");
            Master.CMD_Buzzer_Mute_REQ = !Master.CMD_Buzzer_Mute_REQ;
        }

        /// <summary>
        /// 우측 장비 상태 라벨 Prev, Next 이동 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BasicInfoNext_Click(object sender, EventArgs e)
        {
            object BasicInfoPage = tableLayoutPanel_BasicInfo.Tag;

            if (BasicInfoPage == null || (int)BasicInfoPage < 0)
                BasicInfoPage = 0;

            int EquipmentCount = (Master.m_RackMasters?.Count ?? 0) + (Master.m_Ports?.Count ?? 0);

            if ((int)BasicInfoPage * 5 + 5 < EquipmentCount)
            {
                for (int nCount = 0; nCount < m_BasicInfoLabel.Length; nCount++)
                {
                    m_BasicInfoLabel[nCount].Text = string.Empty;
                    m_BasicInfoLabel[nCount].Visible = false;
                }
                tableLayoutPanel_BasicInfo.Tag = (int)BasicInfoPage + 1;
            }
        }
        private void btn_BasicInfoPrev_Click(object sender, EventArgs e)
        {
            object BasicInfoPage = tableLayoutPanel_BasicInfo.Tag;

            if (BasicInfoPage == null || (int)BasicInfoPage < 0)
                BasicInfoPage = 0;

            if ((int)BasicInfoPage > 0)
            {
                for (int nCount = 0; nCount < m_BasicInfoLabel.Length; nCount++)
                {
                    m_BasicInfoLabel[nCount].Text = string.Empty;
                    m_BasicInfoLabel[nCount].Visible = false;
                }
                tableLayoutPanel_BasicInfo.Tag = (int)BasicInfoPage - 1;
            }
        }

        /// <summary>
        /// 중앙 장비 모니터링용 객체 버튼 Prev, Next 이동 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CommanderNext_Click(object sender, EventArgs e)
        {
            object CommandButtonPage = tableLayoutPanel_FocusButton.Tag;

            if (CommandButtonPage == null || (int)CommandButtonPage < 0)
                CommandButtonPage = 0;

            int EquipmentCount = (Master.m_RackMasters?.Count ?? 0) + (Master.m_Ports?.Count ?? 0);

            if ((int)CommandButtonPage * 5 + 5 < EquipmentCount)
            {
                for (int nCount = 0; nCount < m_CommanderButton.Length; nCount++)
                {
                    m_CommanderButton[nCount].Text = string.Empty;
                    m_CommanderButton[nCount].Visible = false;
                    m_CommanderButton[nCount].Tag = null;
                }
                tableLayoutPanel_FocusButton.Tag = (int)CommandButtonPage + 1;
            }
        }
        private void btn_CommanderPrev_Click(object sender, EventArgs e)
        {
            object CommandButtonPage = tableLayoutPanel_FocusButton.Tag;

            if (CommandButtonPage == null || (int)CommandButtonPage < 0)
                CommandButtonPage = 0;

            if ((int)CommandButtonPage > 0)
            {
                for (int nCount = 0; nCount < m_CommanderButton.Length; nCount++)
                {
                    m_CommanderButton[nCount].Text = string.Empty;
                    m_CommanderButton[nCount].Visible = false;
                    m_CommanderButton[nCount].Tag = null;
                }
                tableLayoutPanel_FocusButton.Tag = (int)CommandButtonPage - 1;
            }
        }

        /// <summary>
        /// 중앙 장비 모니터링 화면 초기화
        /// </summary>
        private void MonitoringEquipmentFormReset()
        {
            for(int nCount = 0; nCount < panel_ControlMonitorWork.Controls.Count; nCount++)
                panel_ControlMonitorWork.Controls[nCount].Hide();
            panel_ControlMonitorWork.Controls.Clear();

            panel_ControlMonitorWork.Tag = null;
            frm_ControlMonRM.SetRackMaster(null);
            frm_ControlMonPort.SetPort(null);

            for (int nCount = 0; nCount < m_BasicInfoLabel.Length; nCount++)
            {
                m_BasicInfoLabel[nCount].Text = string.Empty;
                m_BasicInfoLabel[nCount].Visible = false;
            }
            tableLayoutPanel_BasicInfo.Tag = 0;

            for (int nCount = 0; nCount < m_CommanderButton.Length; nCount++)
            {
                m_CommanderButton[nCount].Text = string.Empty;
                m_CommanderButton[nCount].Visible = false;
                m_CommanderButton[nCount].Tag = null;
            }
            tableLayoutPanel_FocusButton.Tag = 0;
        }

        /// <summary>
        /// 중앙 장비 모니터링용 버튼 클릭 시 이벤트(폼 출력 및 장비 객체 전달)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CommanderEquip_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Tag == null)
                return;

            if (panel_ControlMonitorWork.Controls.Count != 1)
            {
                for (int nCount = 0; nCount < panel_ControlMonitorWork.Controls.Count; nCount++)
                    panel_ControlMonitorWork.Controls[nCount].Hide();
                panel_ControlMonitorWork.Controls.Clear();

                if (btn.Tag.GetType() == typeof(Equipment.RackMaster.RackMaster))
                {
                    Equipment.RackMaster.RackMaster rackMaster = (Equipment.RackMaster.RackMaster)btn.Tag;
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-RackMaster[{rackMaster.GetParam().ID}] Control View Click");
                    panel_ControlMonitorWork.Controls.Add(frm_ControlMonRM);
                    panel_ControlMonitorWork.Tag = frm_ControlMonRM;
                    frm_ControlMonRM.SetRackMaster(rackMaster);
                }
                else if (btn.Tag.GetType() == typeof(Equipment.Port.Port))
                {
                    Equipment.Port.Port port = (Equipment.Port.Port)btn.Tag;
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Port[{port.GetParam().ID}] Control View Click");
                    panel_ControlMonitorWork.Controls.Add(frm_ControlMonPort);
                    panel_ControlMonitorWork.Tag = frm_ControlMonPort;
                    frm_ControlMonPort.SetPort(port);
                }
                else
                {
                    panel_ControlMonitorWork.Tag = null;
                    frm_ControlMonRM.SetRackMaster(null);
                    frm_ControlMonPort.SetPort(null);
                }

                panel_ControlMonitorWork.Controls[0].Dock = DockStyle.Fill;
                panel_ControlMonitorWork.Controls[0].Show();
            }
            else
            {
                panel_ControlMonitorWork.Controls[0].Hide();
                panel_ControlMonitorWork.Controls.Clear();
                btn_CommanderEquip_Click(sender, e);
            }
        }

        /// <summary>
        /// Master 제어 화면 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MasterIO_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Master Control Form Click");

            if (LogIn.IsLogIn())
            {
                Frm_MasterCtrlScreen frm_MasterCtrlScreen = new Frm_MasterCtrlScreen();
                frm_MasterCtrlScreen.Location = this.Location;
                frm_MasterCtrlScreen.StartPosition = FormStartPosition.CenterScreen;
                frm_MasterCtrlScreen.WindowState = FormWindowState.Maximized;
                btn_MasterCtrlForm.Tag = frm_MasterCtrlScreen;
                frm_MasterCtrlScreen.ShowDialog();
                frm_MasterCtrlScreen.Dispose();
                btn_MasterCtrlForm.Tag = null;
            }
        }

        /// <summary>
        /// STK 제어 화면 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RMTPForm_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-RackMaster Control Form Click");

            if (LogIn.IsLogIn())
            {
                if (frm_ControlMonRM.GetRackMaster() != null)
                {
                    Frm_RackMasterTPScreen frm_RackMasterTPScreen = new Frm_RackMasterTPScreen();
                    frm_RackMasterTPScreen.Location = this.Location;
                    frm_RackMasterTPScreen.StartPosition = FormStartPosition.CenterScreen;
                    frm_RackMasterTPScreen.WindowState = FormWindowState.Maximized;
                    btn_RMTPForm.Tag = frm_RackMasterTPScreen;
                    frm_RackMasterTPScreen.ShowDialog();
                    frm_RackMasterTPScreen.Dispose();
                    btn_RMTPForm.Tag = null;
                }
            }
        }

        /// <summary>
        /// Port 제어 화면 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PortTPForm_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Port Control Form Click");

            if (LogIn.IsLogIn())
            {
                Frm_PortTPScreen frm_PortTPScreen = new Frm_PortTPScreen();
                frm_PortTPScreen.Location = this.Location;
                frm_PortTPScreen.StartPosition = FormStartPosition.CenterScreen;
                frm_PortTPScreen.WindowState = FormWindowState.Maximized;
                btn_PortTPForm.Tag = frm_PortTPScreen;
                frm_PortTPScreen.ShowDialog();
                frm_PortTPScreen.Dispose();
                btn_PortTPForm.Tag = null;
            }
        }

        /// <summary>
        /// Main Form 중앙의 Master Alarm Reset 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MasterAlarmReset_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Master Alarm Reset Click");

            Master.Do_MasterRecovery();     //마스터 슬레이브 통신 복구 시나리오
            Master.AlarmAllClear();         //마스터 알람 초기화
        }

        /// <summary>
        /// Main Form 중앙의 Master All Alarm Reset 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AllAlarmReset_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-All Alarm Reset Click");

            Master.Do_MasterRecovery();     //마스터 슬레이브 통신 복구 시나리오
            Master.AlarmAllClear();         //마스터 알람 초기화

            foreach (var rackMaster in Master.m_RackMasters)
            {
                if (rackMaster.Value.m_eControlMode != Equipment.RackMaster.RackMaster.ControlMode.CIMMode && rackMaster.Value.IsConnected())
                    rackMaster.Value.Interlock_SetAlarmClear(Equipment.RackMaster.RackMaster.InterlockFrom.UI_Event); //STK 알람 초기화
            }

            foreach (var port in Master.m_Ports)
            {
                if (port.Value.m_eControlMode != Equipment.Port.Port.ControlMode.CIMMode)
                {
                    port.Value.Interlock_PortAmpAlarmClear(Equipment.Port.Port.InterlockFrom.UI_Event);
                    port.Value.Interlock_PortAlarmClear(Equipment.Port.Port.InterlockFrom.UI_Event); //Port Alarm 초기화
                }
            }
        }

        /// <summary>
        /// Main Form 하단의 로그 상태 초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LogClear_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Log Clear Click");
            Log.Clear();
            Log.LogDGVReload(LogDGV);
        }

        /// <summary>
        /// 로그 폴더를 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OpenFolder_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Log Folder Open Click");
            try
            {
                if (!Directory.Exists(ManagedFileInfo.LogDirectory))
                    Directory.CreateDirectory(ManagedFileInfo.LogDirectory);

                Process.Start(Application.StartupPath + @"\\Log");
            }
            catch
            {

            }
        }

        /// <summary>
        /// 로그 그리드 뷰의 영역을 확장, 축소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LogSizeChange_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.RowStyles[2].Height == 120.0f)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Log Panel Size Up Click");
                tableLayoutPanel1.RowStyles[2].Height = 240.0f;
                ButtonFunc.SetImage(btn_LogSizeChange, Properties.Resources.icons8_thick_arrow_pointing_down_24);
            }
            else
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Log Panel Size Down Click");
                tableLayoutPanel1.RowStyles[2].Height = 120.0f;
                ButtonFunc.SetImage(btn_LogSizeChange, Properties.Resources.icons8_thick_arrow_pointing_up_24);
            }
        }

        /// <summary>
        /// notifyIcon 더블 클릭 시 화면 출력 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-NotifyIcon Click");
            m_bShutdown = false;
            this.Visible = true;
        }

        /// <summary>
        /// notifyIcon show 메뉴 클릭 시 화면 출력 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void btn_Show_Click(object sender, EventArgs eventArgs)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Show Click");
            m_bShutdown = false;
            this.Visible = true;
            this.BringToFront();
        }
        
        /// <summary>
        /// notifyIcon exit 메뉴 클릭 시 프로그램 종료 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void btn_Exit_Click(object sender, EventArgs eventArgs)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Close Click");
            m_bShutdown = true;
            this.Close();
        }

        /// <summary>
        /// 점멸 출력을 위한 Color Update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IntervalColorUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (Master.ErrorIntervalColor == Color.Red)
                Master.ErrorIntervalColor = Color.White;
            else
                Master.ErrorIntervalColor = Color.Red;

            if(Master.FocusIntervalColor == Color.Lime)
                Master.FocusIntervalColor = Color.White;
            else
                Master.FocusIntervalColor = Color.Lime;
        }

        /// <summary>
        /// Main form 중앙의 모든 장비 파워 온 버튼 클릭시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AllEquipPowerOn_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-All Equipment Power On Click");

            foreach (var rackMaster in Master.m_RackMasters)
            {
                rackMaster.Value.Interlock_SetPowerOn(Equipment.RackMaster.RackMaster.InterlockFrom.ApplicationLoop);
            }

            foreach (var port in Master.m_Ports)
            {
                if (port.Value.GetParam().ePortType == Equipment.Port.Port.PortType.EQ)
                    continue;

                port.Value.Interlock_PortPowerOn(Equipment.Port.Port.InterlockFrom.ApplicationLoop);
            }
        }

        /// <summary>
        /// Main form 중앙의 모든 장비 파워 오프 버튼 클릭시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AllEquipPowerOff_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-All Equipment Power Off Click");

            foreach (var rackMaster in Master.m_RackMasters)
            {
                rackMaster.Value.Interlock_SetPowerOff(Equipment.RackMaster.RackMaster.InterlockFrom.ApplicationLoop);
            }

            foreach (var port in Master.m_Ports)
            {
                if (port.Value.GetParam().ePortType == Equipment.Port.Port.PortType.EQ)
                    continue;

                port.Value.Interlock_PortPowerOff(Equipment.Port.Port.InterlockFrom.ApplicationLoop);
            }
        }

        /// <summary>
        /// Main form 중앙의 모든 장비 CIM Mode 버튼 클릭시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AllEquipCIMMode_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-All Equipment CIM Mode Click");

            foreach (var rackMaster in Master.m_RackMasters)
            {
                rackMaster.Value.Interlock_SetControlMode(Equipment.RackMaster.RackMaster.ControlMode.CIMMode ,Equipment.RackMaster.RackMaster.InterlockFrom.ApplicationLoop);
            }

            foreach (var port in Master.m_Ports)
            {
                if (port.Value.GetParam().ePortType == Equipment.Port.Port.PortType.EQ)
                    continue;

                port.Value.Interlock_SetControlMode(Equipment.Port.Port.ControlMode.CIMMode,Equipment.Port.Port.InterlockFrom.ApplicationLoop);
            }
        }

        /// <summary>
        /// Main form 중앙의 모든 장비 Master Mode 버튼 클릭시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AllEquipMasterMode_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-All Equipment Master Mode Click");

            foreach (var rackMaster in Master.m_RackMasters)
            {
                rackMaster.Value.Interlock_SetControlMode(Equipment.RackMaster.RackMaster.ControlMode.MasterMode, Equipment.RackMaster.RackMaster.InterlockFrom.ApplicationLoop);
            }

            foreach (var port in Master.m_Ports)
            {
                if (port.Value.GetParam().ePortType == Equipment.Port.Port.PortType.EQ)
                    continue;

                port.Value.Interlock_SetControlMode(Equipment.Port.Port.ControlMode.MasterMode, Equipment.Port.Port.InterlockFrom.ApplicationLoop);
            }
        }

        /// <summary>
        /// Main form 중앙의 모든 장비 Auto Run 버튼 클릭시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AllEquipAutoRun_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-All Equipment Auto Run Click");

            foreach (var rackMaster in Master.m_RackMasters)
            {
                rackMaster.Value.Interlock_AutoModeEnable(Equipment.RackMaster.RackMaster.InterlockFrom.ApplicationLoop);
            }

            foreach (var port in Master.m_Ports)
            {
                if (port.Value.GetParam().ePortType == Equipment.Port.Port.PortType.EQ)
                    continue;

                port.Value.Interlock_StartAutoControl(Equipment.Port.Port.InterlockFrom.ApplicationLoop);
            }
        }

        /// <summary>
        /// Main form 중앙의 모든 장비 Auto Stop 버튼 클릭시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AllEquipAutoStop_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-All Equipment Auto Stop Click");

            foreach (var rackMaster in Master.m_RackMasters)
            {
                rackMaster.Value.Interlock_AutoModeDisable(Equipment.RackMaster.RackMaster.InterlockFrom.ApplicationLoop);
            }

            foreach (var port in Master.m_Ports)
            {
                if (port.Value.GetParam().ePortType == Equipment.Port.Port.PortType.EQ)
                    continue;

                port.Value.Interlock_StopAutoControl(Equipment.Port.Port.InterlockFrom.ApplicationLoop);
            }
        }

        /// <summary>
        /// Main form Log 영역 Log 관련 설정 폼 출력 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LogSet_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Log Settings Click");

            if (LogIn.IsLogIn())
            {
                Frm_LogSet frm_LogSet = new Frm_LogSet();
                frm_LogSet.StartPosition = FormStartPosition.CenterParent;
                btn_LogSet.Tag = frm_LogSet;
                DialogResult result = frm_LogSet.ShowDialog();
                frm_LogSet.Dispose();
                frm_LogSet.Tag = null;

                if(result == DialogResult.OK)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Log Settings Apply");
                    Log.LogDGVReload(LogDGV);
                    LogFileCompressAndRemove();
                }
                else
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Log Settings Cancel");
            }
        }

        /// <summary>
        /// 로그 압축, 삭제 타이머 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogRemoveTimer_Tick(object sender, EventArgs e)
        {
            LogFileCompressAndRemove();
        }

        /// <summary>
        /// 로그 압축, 삭제 진행
        /// </summary>
        private void LogFileCompressAndRemove()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                try
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Log file Compress and Zip Remove");
                    LogCompress(ManagedFileInfo.LogDirectory);
                    ZipRemove(ManagedFileInfo.LogDirectory);
                    LogCompress(ManagedFileInfo.ExceptionLogDirectory);
                    ZipRemove(ManagedFileInfo.ExceptionLogDirectory);
                    LogCompress(ManagedFileInfo.STKLogDirectory);
                    ZipRemove(ManagedFileInfo.STKLogDirectory);
                }
                catch { }
            });
            LocalThread.IsBackground = true;
            LocalThread.Name = "Log RemoveThread";
            LocalThread.Start();
        }

        /// <summary>
        /// 로그 압축 주기 확인
        /// </summary>
        /// <param name="Directory"></param>
        /// <returns></returns>
        private int GetLogCompressCycle(string Directory)
        {
            if (Directory == ManagedFileInfo.STKLogDirectory)
                return 7;
            else
                return ApplicationParam.m_LogParam.CompressionCycle;
        }

        /// <summary>
        /// 로그 삭제 주기 확인
        /// </summary>
        /// <param name="Directory"></param>
        /// <returns></returns>
        private int GetZipRemoveCycle(string Directory)
        {
            if (Directory == ManagedFileInfo.STKLogDirectory)
                return 7;
            else
                return ApplicationParam.m_LogParam.ZIPDeleteCycle;
        }

        /// <summary>
        /// 지정 경로의 로그 압축 함수
        /// </summary>
        /// <param name="LogDirectory"></param>
        private void LogCompress(string LogDirectory)
        {
            int CompressCycle = GetLogCompressCycle(LogDirectory);
            DateTime CompressDate = DateTime.Now.AddDays(Math.Abs(CompressCycle) * -1);
            string IDate = CompressDate.ToString("yyyyMMdd");
            //string LogDirectory = ManagedFileInfo.LogDirectory;

            DirectoryInfo di = new DirectoryInfo(LogDirectory);
            DirectoryInfo[] YearDirectories = di.GetDirectories("*.*", SearchOption.TopDirectoryOnly);

            foreach (DirectoryInfo YearDirectory in YearDirectories)
            {
                if (YearDirectory.Name.Length != 4)
                    continue;

                string Year;
                string Month;
                string Day;
                try
                {
                    int nYear = Convert.ToInt32(YearDirectory.Name);
                    if (nYear >= 2000 && nYear <= 3000)
                        Year = nYear.ToString("0000");
                    else
                        continue;
                }
                catch
                {
                    continue;
                }

                DirectoryInfo[] MonthDirectories = YearDirectory.GetDirectories("*.*", SearchOption.TopDirectoryOnly);
                foreach (DirectoryInfo MonthDirectory in MonthDirectories)
                {
                    if (MonthDirectory.Name.Length != 2)
                        continue;

                    try
                    {
                        int nMonth = Convert.ToInt32(MonthDirectory.Name);
                        if (nMonth >= 1 && nMonth <= 12)
                            Month = nMonth.ToString("00");
                        else
                            continue;
                    }
                    catch
                    {
                        continue;
                    }

                    DirectoryInfo[] DayDirectories = MonthDirectory.GetDirectories("*.*", SearchOption.TopDirectoryOnly);
                    foreach (DirectoryInfo DayDirectory in DayDirectories)
                    {
                        if (DayDirectory.Name.Length != 2)
                            continue;

                        try
                        {
                            int nDay = Convert.ToInt32(DayDirectory.Name);
                            if (nDay >= 1 && nDay <= 31)
                                Day = nDay.ToString("00");
                            else
                                continue;
                        }
                        catch
                        {
                            continue;
                        }

                        string DirectoryDate = $"{Year}{Month}{Day}";

                        if (IDate.CompareTo(DirectoryDate) > 0)
                        {
                            ZipFile.CreateFromDirectory(DayDirectory.FullName, $"{DayDirectory.FullName}.zip");
                            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Create Zip: {DayDirectory.FullName}.zip");
                            Directory.Delete(DayDirectory.FullName, true);
                            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Delete Directory: {DayDirectory.FullName}");
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 지정 경로의 Zip 삭제 함수
        /// </summary>
        /// <param name="LogDirectory"></param>
        private void ZipRemove(string LogDirectory)
        {
            int RemoveCycle = GetZipRemoveCycle(LogDirectory);
            DateTime DeleteDate = DateTime.Now.AddDays(Math.Abs(RemoveCycle) * -1);
            string IDate = DeleteDate.ToString("yyyyMMdd");

            //string LogDirectory = ManagedFileInfo.LogDirectory;

            DirectoryInfo di = new DirectoryInfo(LogDirectory);
            DirectoryInfo[] dirs = di.GetDirectories("*.*", SearchOption.AllDirectories);

            foreach (DirectoryInfo info in dirs)
            {
                string[] file = Directory.GetFiles(info.FullName, "*.*", SearchOption.AllDirectories);

                if (file != null && file.Length > 0)
                {
                    foreach (string filepath in file)
                    {
                        DateTime fileCreationTime = File.GetCreationTime(filepath);
                        string fileDate = fileCreationTime.ToString("yyyyMMdd");

                        if (IDate.CompareTo(fileDate) > 0 && Path.GetExtension(filepath).ToLower() == ".zip")
                        {
                            //파일 삭제 후
                            File.Delete(filepath);
                            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Delete Zip: {filepath}");
                        }
                    }
                }

                //폴더에 남은 파일이 없으면 폴더도 삭제
                string[] Refresh = Directory.GetFiles(info.FullName, "*.*", SearchOption.AllDirectories);

                if (Refresh != null && Refresh.Length == 0)
                {
                    info.Attributes = FileAttributes.Normal;
                    info.Delete(true);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Delete Directory: {info.FullName}");
                }
            }
        }

        /// <summary>
        /// Master Program 실행 후 File Initialize에 실패 시 해당 Event 호출되며 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_CloseTimer_Tick(object sender, EventArgs e)
        {
            Form_CloseTimer.Enabled = false;
            m_bShutdown = true;
            this.Close();
        }

        /// <summary>
        /// 장비 모니터링 Grid 또는 장비 맵의 상태 Grid에서 클릭 시 Status View로 이동 및 객체 선택
        /// </summary>
        /// <param name="tag"></param>
        private void FindControlEquipmentPageAndFocus(object tag)
        {
            tabControl1.SelectedTab = tabPage_Equipments;

            int EquipmentCount = (Master.m_RackMasters?.Count ?? 0) + (Master.m_Ports?.Count ?? 0);

            bool bFind = false;

            for (int nLoop = 0; nLoop < (EquipmentCount / 5) + (EquipmentCount % 5 == 0 ? 0 : 1); nLoop++)
            {
                tableLayoutPanel_FocusButton.Tag = nLoop;

                for (int nCount = 0; nCount < m_CommanderButton.Length; nCount++)
                {
                    m_CommanderButton[nCount].Tag = null;
                    m_CommanderButton[nCount].Text = string.Empty;
                    m_CommanderButton[nCount].Visible = false;
                }

                FocusButtonUpdate();

                for (int nCount = 0; nCount < m_CommanderButton.Length; nCount++)
                {
                    if (m_CommanderButton[nCount].Tag == tag)
                    {
                        btn_CommanderEquip_Click(m_CommanderButton[nCount], new EventArgs());
                        bFind = true;
                        break;
                    }
                }

                if (bFind)
                    break;
            }
        }
        
        /// <summary>
        /// 장비 맵의 상태 Grid에서 장비 제어 버튼 클릭시 발생 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_EquipmentStatus_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex >= 0 &&
                senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {                
                if (e.ColumnIndex == (int)Master.DGV_EquipmentStatusColumn.Control)
                {
                    DataGridViewCell DGV_EquipCell = senderGrid.Rows[e.RowIndex].Cells[(int)Master.DGV_EquipmentStatusColumn.Control];
                    FindControlEquipmentPageAndFocus(DGV_EquipCell.Tag);
                }
            }
            senderGrid.CurrentCell = null;
        }

        /// <summary>
        /// 장비 상태 탭에서 Safety 버튼 클릭 시 발생 이벤트
        /// 클릭 시 Safety Tab Page로 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Safty_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Safety Status Click");
            tabControl1.SelectedTab = tabPage_Safty;
            //tabPage_Safty.Show();
        }

        /// <summary>
        /// 장비 상태 탭에서 Monitoring 버튼 클릭 시 발생 이벤트
        /// 클릭 시 Monitoring Tab Page로 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Monitoring_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Monitoring Status Click");
            tabControl1.SelectedTab = tabPage_MonitoringMap;
        }

        /// <summary>
        /// 로그인 연장 버튼 클릭 시 시간 연장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LogInExtend_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Login Extend Click");
            if (LogIn.IsLogIn())
            {
                LogIn.SetLogInExtend();
            }
        }
        
        /// <summary>
        /// 화면 중앙 Monitoring Tab의 Port 정보 업데이트
        /// </summary>
        private void Update_MonitoringPortMap()
        {
            foreach (var control in tableLayoutPanel_PortMap.Controls)
            {
                if (control.GetType() == typeof(Label))
                {
                    Label lbl = (Label)control;
                    Equipment.Port.Port port = (Equipment.Port.Port)lbl.Tag;
                    port.Update_Lbl_PortInfoLabel(ref lbl, Equipment.Port.Port.PortInfoType.Map);
                }
            }
        }
        
        /// <summary>
        /// 화면 중앙 Monitoring Tab의 STK 정보 업데이트
        /// </summary>
        private void Update_MonitoringRackMasterMap()
        {
            foreach (var control in tableLayoutPanel_RackMasterMap.Controls)
            {
                if (control.GetType() == typeof(Label))
                {
                    Label lbl = (Label)control;
                    Equipment.RackMaster.RackMaster rackmaster = (Equipment.RackMaster.RackMaster)lbl.Tag;
                    rackmaster.Update_Lbl_RackMasterInfoLabel(ref lbl, Equipment.RackMaster.RackMaster.RackMasterInfoType.Map);
                }
            }
        }
        
        /// <summary>
        /// 화면 중앙 Monitoring Tab의 Label 클릭 시 화면 전환 및 장비 객체 추적 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_To_EquipmentControlView(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            MouseEventArgs me = (MouseEventArgs)e;
            if(me.Button == MouseButtons.Left)
                FindControlEquipmentPageAndFocus(lbl.Tag);
        }
        
        /// <summary>
        /// 화면 중앙 Monitoring Tab에서 TableLayout에 Port 정보 로드 및 라벨 객체 할당 이벤트
        /// </summary>
        private void Load_MonitoringPortMap()
        {
            tableLayoutPanel_PortMap.Controls.Clear();

            List<Label> PortList = new List<Label>();

            foreach (var port in Master.m_Ports)
            {
                Label lbl = new Label();
                lbl.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, lbl, new object[] { true });
                lbl.Click += Change_To_EquipmentControlView;
                lbl.BorderStyle = BorderStyle.Fixed3D;
                port.Value.Update_Lbl_PortInfoLabel(ref lbl, Equipment.Port.Port.PortInfoType.Map);

                float fontsize = 8f;

                if (Master.m_Ports.Count < 10)
                    fontsize = 9f;
                else if (Master.m_Ports.Count < 20)
                    fontsize = 8.5f;
                else if (Master.m_Ports.Count < 30)
                    fontsize = 8f;
                else if (Master.m_Ports.Count < 40)
                    fontsize = 7f;
                else
                    fontsize = 7.5f;
                lbl.AutoSize = true;
                lbl.Font = new Font("Segoe UI Semibold", fontsize, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                lbl.Margin = new Padding(1, 1, 1, 1);
                lbl.Tag = port.Value;

                if (port.Value.GetParam().ePortType != Equipment.Port.Port.PortType.EQ)
                {
                    ContextMenu ctx = new ContextMenu();

                    MenuItem item = new MenuItem();
                    item.Text = "Error Clear";
                    item.Tag = port.Value;
                    item.Click += btn_PortErrorClear_Click;

                    ctx.MenuItems.Add(item);
                    ctx.MenuItems.Add("-");

                    MenuItem item2 = new MenuItem();
                    item2.Text = "Power On";
                    item2.Tag = port.Value;
                    item2.Click += btn_PortPowerOn_Click;
                    ctx.MenuItems.Add(item2);

                    MenuItem item3 = new MenuItem();
                    item3.Text = "Power Off";
                    item3.Tag = port.Value;
                    item3.Click += btn_PortPowerOff_Click;
                    ctx.MenuItems.Add(item3);
                    ctx.MenuItems.Add("-");

                    MenuItem item4 = new MenuItem();
                    item4.Text = "Auto Run";
                    item4.Tag = port.Value;
                    item4.Click += btn_PortAutoRun_Click;
                    ctx.MenuItems.Add(item4);

                    MenuItem item5 = new MenuItem();
                    item5.Text = "Auto Stop";
                    item5.Tag = port.Value;
                    item5.Click += btn_PortAutoStop_Click;
                    ctx.MenuItems.Add(item5);
                    ctx.MenuItems.Add("-");


                    MenuItem item6 = new MenuItem();
                    item6.Text = "Cycle Run";
                    item6.Tag = port.Value;
                    item6.Click += btn_PortCycleRun_Click;
                    ctx.MenuItems.Add(item6);

                    MenuItem item7 = new MenuItem();
                    item7.Text = "Cycle Stop";
                    item7.Tag = port.Value;
                    item7.Click += btn_PortCycleStop_Click;
                    ctx.MenuItems.Add(item7);
                    ctx.MenuItems.Add("-");

                    MenuItem item8 = new MenuItem();
                    item8.Text = "Direction Change";
                    item8.Tag = port.Value;
                    item8.Click += btn_PortDirectionChange_Click;
                    ctx.MenuItems.Add(item8);


                    lbl.ContextMenu = ctx;
                }
                PortList.Add(lbl);
            }

            int PortCount = Master.m_Ports?.Count ?? 0;
            int ColumnCount = PortCount <= 15 ? 5 : PortCount <= 20 ? 6 : PortCount <= 30 ? 7 : PortCount <= 40 ? 8 : 9;
            int RowCount = (PortCount % ColumnCount == 0 ? PortCount / ColumnCount : PortCount / ColumnCount + 1);
            if (RowCount < 3)
                RowCount = 3;

            tableLayoutPanel_PortMap.ColumnStyles.Clear();
            tableLayoutPanel_PortMap.RowStyles.Clear();

            tableLayoutPanel_PortMap.ColumnCount = ColumnCount;
            tableLayoutPanel_PortMap.RowCount = RowCount;
            for (int nColumnCount = 0; nColumnCount < tableLayoutPanel_PortMap.ColumnCount; nColumnCount++)
            {
                tableLayoutPanel_PortMap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            }
            for (int nRowCount = 0; nRowCount < tableLayoutPanel_PortMap.RowCount; nRowCount++)
            {
                tableLayoutPanel_PortMap.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            }
            tableLayoutPanel_PortMap.Dock = DockStyle.Fill;
            tableLayoutPanel_PortMap.Refresh();


            int nInsertCount = 0;
            for (int nRowCount = 0; nRowCount < tableLayoutPanel_PortMap.RowCount; nRowCount++)
            {
                for (int nColumnCount = 0; nColumnCount < tableLayoutPanel_PortMap.ColumnCount; nColumnCount++)
                {
                    if (nInsertCount >= PortList.Count)
                        continue;

                    tableLayoutPanel_PortMap.Controls.Add(PortList[nInsertCount], nColumnCount, nRowCount);
                    PortList[nInsertCount].Dock = DockStyle.Fill;
                    nInsertCount++;
                }
            }
        }
        
        /// <summary>
        /// 화면 중앙 Monitoring Tab에서 TableLayout에 STK 정보 로드 및 라벨 객체 할당 이벤트
        /// </summary>
        private void Load_MonitoringRackMasterMap()
        {
            tableLayoutPanel_RackMasterMap.Controls.Clear();

            List<Label> RackMasterList = new List<Label>();

            foreach (var rackmaster in Master.m_RackMasters)
            {
                Label lbl = new Label();
                lbl.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, lbl, new object[] { true });
                lbl.BorderStyle = BorderStyle.Fixed3D;
                lbl.Click += Change_To_EquipmentControlView;
                rackmaster.Value.Update_Lbl_RackMasterInfoLabel(ref lbl, Equipment.RackMaster.RackMaster.RackMasterInfoType.Map);

                //btn.Text = $"Port [ {port.Value.GetParam().ID} ]";
                lbl.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                lbl.Margin = new Padding(1, 1, 1, 1);
                lbl.Tag = rackmaster.Value;

                ContextMenu ctx = new ContextMenu();

                MenuItem item = new MenuItem();
                item.Text = "Error Clear";
                item.Tag = rackmaster.Value;
                item.Click += btn_RackMasterErrorClear_Click;

                ctx.MenuItems.Add(item);
                ctx.MenuItems.Add("-");

                MenuItem item2 = new MenuItem();
                item2.Text = "Power On";
                item2.Tag = rackmaster.Value;
                item2.Click += btn_RackMasterPowerOn_Click;
                ctx.MenuItems.Add(item2);

                MenuItem item3 = new MenuItem();
                item3.Text = "Power Off";
                item3.Tag = rackmaster.Value;
                item3.Click += btn_RackMasterPowerOff_Click;
                ctx.MenuItems.Add(item3);
                ctx.MenuItems.Add("-");

                MenuItem item4 = new MenuItem();
                item4.Text = "Auto";
                item4.Tag = rackmaster.Value;
                item4.Click += btn_RackMasterAuto_Click;
                ctx.MenuItems.Add(item4);

                MenuItem item5 = new MenuItem();
                item5.Text = "Auto Stop";
                item5.Tag = rackmaster.Value;
                item5.Click += btn_RackMasterAutoStop_Click;
                ctx.MenuItems.Add(item5);

                lbl.ContextMenu = ctx;
                RackMasterList.Add(lbl);
            }

            tableLayoutPanel_RackMasterMap.ColumnCount = 1;
            for (int nColumnCount = 0; nColumnCount < tableLayoutPanel_RackMasterMap.ColumnCount; nColumnCount++)
            {
                tableLayoutPanel_RackMasterMap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            }
            //Row Setting
            tableLayoutPanel_RackMasterMap.RowCount = Master.m_RackMasters?.Count ?? 0;
            for (int nRowCount = 0; nRowCount < tableLayoutPanel_RackMasterMap.RowCount; nRowCount++)
            {
                tableLayoutPanel_RackMasterMap.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            }

            int nRackMasterCount = 0;
            for (int nRowCount = 0; nRowCount < tableLayoutPanel_RackMasterMap.RowCount; nRowCount++)
            {
                for (int nColumnCount = 0; nColumnCount < tableLayoutPanel_RackMasterMap.ColumnCount; nColumnCount++)
                {
                    if (nRackMasterCount >= RackMasterList.Count)
                        continue;

                    tableLayoutPanel_RackMasterMap.Controls.Add(RackMasterList[nRackMasterCount], nColumnCount, nRowCount);
                    RackMasterList[nRackMasterCount].Dock = DockStyle.Fill;
                    nRackMasterCount++;
                }
            }
        }

        /// <summary>
        /// SafetyMapSetting Form 내에서 Apply 버튼 클릭 시 이벤트
        /// </summary>
        /// <param name="_SafetyImageInfo"></param>
        private void Frm_SafetyMapSettings_ApplyEvent(MasterSafetyImageInfo _SafetyImageInfo)
        {
            safetyImageInfo = _SafetyImageInfo;
            SafetyInfoSync(safetyImageInfo);
        }

        /// <summary>
        /// SafetyMapSetting 폼 종료시 객체 이벤트 해제 및 메모리 정리 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_SafetyMapSettings_Disposed(object sender, EventArgs e)
        {
            frm_SafetyMapSettings.ApplyEvent -= Frm_SafetyMapSettings_ApplyEvent;
            frm_SafetyMapSettings.Disposed -= Frm_SafetyMapSettings_Disposed;
            frm_SafetyMapSettings = null;
        }

        /// <summary>
        /// 화면 중앙 Monitoring Map의 장비 객체 Label 우 클릭 시 출력되는 장비 동작 관련 메뉴 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void btn_PortErrorClear_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            Equipment.Port.Port port = (Equipment.Port.Port)item.Tag;

            if (port == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map Port[{port.GetParam().ID}] Alarm Clear Click");
            port.Interlock_PortAmpAlarmClear(Equipment.Port.Port.InterlockFrom.UI_Event);
            port.Interlock_PortAlarmClear(Equipment.Port.Port.InterlockFrom.UI_Event);
        }
        private void btn_PortPowerOn_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            Equipment.Port.Port port = (Equipment.Port.Port)item.Tag;

            if (port == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map Port[{port.GetParam().ID}] Power On Click");
            port.Interlock_PortPowerOn(Equipment.Port.Port.InterlockFrom.UI_Event);
        }
        private void btn_PortPowerOff_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            Equipment.Port.Port port = (Equipment.Port.Port)item.Tag;

            if (port == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map Port[{port.GetParam().ID}] Power Off Click");
            port.Interlock_PortPowerOff(Equipment.Port.Port.InterlockFrom.UI_Event);
        }
        private void btn_PortAutoRun_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;

            Equipment.Port.Port port = (Equipment.Port.Port)item.Tag;

            if (port == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map Port[{port.GetParam().ID}] Auto Run Click");
            port.Interlock_StartAutoControl(Equipment.Port.Port.InterlockFrom.UI_Event);
        }
        private void btn_PortAutoStop_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            Equipment.Port.Port port = (Equipment.Port.Port)item.Tag;

            if (port == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map Port[{port.GetParam().ID}] Auto Stop Click");
            port.Interlock_StopAutoControl(Equipment.Port.Port.InterlockFrom.UI_Event);
        }

        private void btn_PortCycleRun_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;

            Equipment.Port.Port port = (Equipment.Port.Port)item.Tag;

            if (port == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map Port[{port.GetParam().ID}] Cycle Run Click");
            port.Interlock_StartAutoManualCycleControl(1, Equipment.Port.Port.InterlockFrom.UI_Event);
        }
        private void btn_PortCycleStop_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            Equipment.Port.Port port = (Equipment.Port.Port)item.Tag;

            if (port == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map Port[{port.GetParam().ID}] Cycle Stop Click");
            port.Interlock_StopAutoManualCycleControl(Equipment.Port.Port.InterlockFrom.UI_Event);
        }
        private void btn_PortDirectionChange_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;

            Equipment.Port.Port port = (Equipment.Port.Port)item.Tag;

            if (port == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (port.GetOperationDirection() == Equipment.Port.Port.PortDirection.Input)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map Port[{port.GetParam().ID}] Direction Change(Output) Click");
                port.Interlock_AutoControlDirectionChange(Equipment.Port.Port.PortDirection.Output, Equipment.Port.Port.InterlockFrom.UI_Event);
            }
            else
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map Port[{port.GetParam().ID}] Direction Change(Input) Click");
                port.Interlock_AutoControlDirectionChange(Equipment.Port.Port.PortDirection.Input, Equipment.Port.Port.InterlockFrom.UI_Event);
            }
        }

        private void btn_RackMasterErrorClear_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            Equipment.RackMaster.RackMaster rackmaster = (Equipment.RackMaster.RackMaster)item.Tag;

            if (rackmaster == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map RackMaster[{rackmaster.GetParam().ID}] Alarm Clear Click");
            rackmaster.Interlock_SetAlarmClear(Equipment.RackMaster.RackMaster.InterlockFrom.UI_Event);
        }
        private void btn_RackMasterPowerOn_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            Equipment.RackMaster.RackMaster rackmaster = (Equipment.RackMaster.RackMaster)item.Tag;

            if (rackmaster == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map RackMaster[{rackmaster.GetParam().ID}] Power On Click");
            rackmaster.Interlock_SetPowerOn(Equipment.RackMaster.RackMaster.InterlockFrom.UI_Event);
        }
        private void btn_RackMasterPowerOff_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            Equipment.RackMaster.RackMaster rackmaster = (Equipment.RackMaster.RackMaster)item.Tag;

            if (rackmaster == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map RackMaster[{rackmaster.GetParam().ID}] Power Off Click");
            rackmaster.Interlock_SetPowerOff(Equipment.RackMaster.RackMaster.InterlockFrom.UI_Event);
        }
        private void btn_RackMasterAuto_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;

            Equipment.RackMaster.RackMaster rackmaster = (Equipment.RackMaster.RackMaster)item.Tag;

            if (rackmaster == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map RackMaster[{rackmaster.GetParam().ID}] Auto Click");
            rackmaster.Interlock_AutoModeEnable(Equipment.RackMaster.RackMaster.InterlockFrom.UI_Event);
        }
        private void btn_RackMasterAutoStop_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            Equipment.RackMaster.RackMaster rackmaster = (Equipment.RackMaster.RackMaster)item.Tag;

            if (rackmaster == null)
                return;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port Map RackMaster[{rackmaster.GetParam().ID}] Auto Stop Click");
            rackmaster.Interlock_AutoModeDisable(Equipment.RackMaster.RackMaster.InterlockFrom.UI_Event);
        }

        /// <summary>
        /// 화면 상단 OMRON 통신 상태 관련 라벨 클릭시 출력되는 메뉴 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_OMRON_Click(object sender, EventArgs e)
        {
            Frm_OmronTable frm_OmronTable = new Frm_OmronTable();
            frm_OmronTable.Location = this.Location;
            frm_OmronTable.StartPosition = FormStartPosition.CenterScreen;
            frm_OmronTable.ShowDialog();
            frm_OmronTable.Dispose();
        }

        /// <summary>
        /// IO Map Label 이동 관련 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_StatusLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                return;

            Label lbl = (Label)sender;
            lbl.Parent.Tag = "On";
        }

        private void lbl_StatusLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                return;

            Label lbl = (Label)sender;

            if ((string)lbl.Parent.Tag == "On" && e.Button == MouseButtons.Left)
                lbl.Parent.Location = new Point(e.X + lbl.Parent.Location.X - (lbl.Size.Width / 2), e.Y + lbl.Parent.Location.Y - (lbl.Size.Height / 2));
        }

        private void lbl_StatusLabel_MouseUp(object sender, MouseEventArgs e)
        {
            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                return;

            Label lbl = (Label)sender;
            lbl.Parent.Tag = "Off";
        }

        /// <summary>
        /// IO Map 설정 버튼 클릭 시 이벤트
        /// SafetyMapSetting 폼 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Settings_IOMap_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Safety Map Settings Click");

            if (frm_SafetyMapSettings == null)
            {
                SafetyInfoItemLocationSync();
                frm_SafetyMapSettings = new Frm_MasterSafetyMapSettings(ref safetyImageInfo);
                frm_SafetyMapSettings.Show();
                frm_SafetyMapSettings.Disposed += Frm_SafetyMapSettings_Disposed;
                frm_SafetyMapSettings.ApplyEvent += Frm_SafetyMapSettings_ApplyEvent;
            }
            else
            {
                frm_SafetyMapSettings.BringToFront();
            }
        }

        /// <summary>
        /// IO Map 저장 버튼 클릭 시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_IOMap_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;

            if (!LogIn.IsLogIn())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_LoginRequest"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Interlock, $"Cannot be ordered. Currently Logoff State.");
                return;
            }

            SafetyInfoItemLocationSync();

            if (MasterSafetyImageInfo.Save(safetyImageInfo))
                MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
