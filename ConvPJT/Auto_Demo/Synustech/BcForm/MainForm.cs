using Synustech.ucPanel;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using Synustech.ucPanel.BcStatus;
using Synustech.ucPanel.BcStatus.Setting;
using Synustech.ucPanel.BcStatus.IO;
using Synustech.ucPanel.BcStatus.RFID;
using Synustech.ucPanel.BcAlarm;
using Synustech.ucPanel.BcMotion;
using Synustech.ucPanel.BcSetting;
using Synustech.BcForm.ucPanel.BcSetting;
using Synustech.BcForm.ucPanel.BcMain;
using System.ComponentModel;
using System.Threading;
using static Synustech.G_Var;
using Synustech.BcForm;
using WMX3ApiCLR;

namespace Synustech
{
    public partial class MainForm : Form
    {
      
        UserLog      ucLog  = new UserLog();
        UserMainStatusdg ucMainStatus = new UserMainStatusdg();
        UserCVStatusdg ucCvStatus = new UserCVStatusdg();
        UserSafety userSafety = new UserSafety();
        UserAutoCondition userAutoCondition = new UserAutoCondition();
        UserEQstatus userEQstatus = new UserEQstatus();
        UserAlarmHistory userAlarmHistory = new UserAlarmHistory();
        StorageProgress prograss = new StorageProgress();
        UserTopComm ucTopComm = new UserTopComm();

        //Status
        UserSafetyCondition ucSap = new UserSafetyCondition();
        UserAutoConditionSub ucAuto = new UserAutoConditionSub();
        UserEqStatusSub ucEqCondition = new UserEqStatusSub();
        UserTopComm_Status ucSatus = new UserTopComm_Status();
        UserPIOStatus ucPIOStatus = new UserPIOStatus();

        //Status-IO
        TableLayoutPanel IOPanel = new TableLayoutPanel();
        UserInput ucInput = new UserInput();
        UserOutput ucOutput = new UserOutput();
        ConvIDSelect ucIDSelect = new ConvIDSelect();

        //Status-Setting
        TableLayoutPanel Status_SettingPanel = new TableLayoutPanel();
        UserMotion ucMotion = new UserMotion();
        UserSetting ucSetting = new UserSetting();
        UserStep ucStep = new UserStep();

        //Status-RFID
        TableLayoutPanel RFIDPanel = new TableLayoutPanel();
        UserRFIDStatus ucRFIDStatus = new UserRFIDStatus();
        UserRFIDSetting ucRFIDSetting = new UserRFIDSetting();

        //Alarm
        TableLayoutPanel alarmPanel = new TableLayoutPanel();
        UserCurrentAlarm userCurrentAlarm = new UserCurrentAlarm();
        UserSolution userSolution = new UserSolution();

        //Motion
        TableLayoutPanel MotionPanel = new TableLayoutPanel();
        UserIO_Motion ucIO_Motion = new UserIO_Motion();
        UserPowerOn ucPowerOn = new UserPowerOn();
        UserTopComm_Motion ucTopComm_Motion = new UserTopComm_Motion();
        UserConvControl_Re ucConvControl_Re = new UserConvControl_Re();
        UserConvTeaching_Re ucConvTeaching_Re = new UserConvTeaching_Re();
        UserCycleStatus ucCycleStatus = new UserCycleStatus();
        UserCycleTest ucCycleTest = new UserCycleTest();
        UserConvLineView ucConvLineView = new UserConvLineView();
       
        //Setting
        TableLayoutPanel SettingPanel = new TableLayoutPanel();
        UserTopComm_Setting ucTopComm_Setting = new UserTopComm_Setting();

        Parameter ucParameter = new Parameter();
        Operation ucOperation = new Operation();
        Watchdog  ucWatchdog = new Watchdog();
        UserCVCreate ucCVCreate = new UserCVCreate();
        LineCreate ucLineCreate = new LineCreate();

        public MainForm()
        {
 
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            InitializeUi();
            isMainFormOpen = true;

            G_Var.conveyors  = new List<Conveyor>();
            G_Var.rectangles = new List<CustomRectangle>();
            G_Var.lines      = new List<Line>();
            xmlControl.GetList(alarmFullPath);
            Init();

            SetDoubleBuffer(Dep_03_Pn_MainMenu);

            // 각 패널에 더블 버퍼링 적용
            SetDoubleBuffer(IOPanel);
            SetDoubleBuffer(Status_SettingPanel);
            SetDoubleBuffer(alarmPanel);
            SetDoubleBuffer(MotionPanel);
            SetDoubleBuffer(SettingPanel);

            //Main_SubMenu
            ucTopComm.del_CommunicationWMX += Init;
            ucTopComm.del_CloseWMX         += DisConnectWMX;
            ucTopComm.del_ServoOn          += w_motion.AllServoOn;
            ucTopComm.del_ServoOff         += w_motion.AllServoOff;

            //MainbMenu
            Dep_03_Pn_MainMenu.del_PanelMain += MonitorMain;
            Dep_03_Pn_MainMenu.del_PanelStatus += MonitorStatus;
            Dep_03_Pn_MainMenu.del_PanelAlarm += MonitorAlarm;
            Dep_03_Pn_MainMenu.del_PanelMotion += MonitorMotion;
            Dep_03_Pn_MainMenu.del_PanelSetting += MonitorSetting;
            Dep_03_Pn_MainMenu.del_PanelChange += initDefult;

            //Status_SubMenu
            ucSatus.del_PanelMain += MonitorStatus_main;
            ucSatus.del_PanelIO += MonitorIO;
            ucSatus.del_PanelSetting += MonitorStatusSetting;

            ucSatus.del_PanelRFID += MonitorRFID;

            //Motion_SubMenu
            ucTopComm_Motion.del_PanelManual        += MonitorManual;
            ucTopComm_Motion.del_PanelTeaching      += MonitorTeaching;
            ucTopComm_Motion.del_PanelCycleTest     += MonitorCycleTest;

            //Setting_SubMenu
            ucTopComm_Setting.del_PanelWMXPara_Setting += MonitorPara; 
            ucTopComm_Setting.del_PanelOperation       += MonitorOperation;
            ucTopComm_Setting.del_PanelTime        += MonitorWachdogTime;
            ucTopComm_Setting.del_PanelCnvCreate   += MonitorCnvCreate;
            ucTopComm_Setting.del_PanelLine        += MonitorLine;

            Dep_03_Pn_MainMenu.del_Subreset_Status += ucSatus.ResetButtonStates;
            Dep_03_Pn_MainMenu.del_Subreset_Status += ucSatus.init;
            Dep_03_Pn_MainMenu.del_Subreset_Motion += ucTopComm_Motion.ResetButtonStates;
            Dep_03_Pn_MainMenu.del_Subreset_Motion += ucTopComm_Motion.init;
            Dep_03_Pn_MainMenu.del_Subreset_Setting += ucTopComm_Setting.ResetButtonStates;
            Dep_03_Pn_MainMenu.del_Subreset_Setting += ucTopComm_Setting.init;

            ucIDSelect.del_MasterUpdate             += ucInput.IsMaster;
            ucIDSelect.del_ConvUpdate               += ucInput.IsConv;

            userCurrentAlarm.del_SendCode += userSolution.UcCurrentAlarm_SendCode;

            ucConvControl_Re.del_Jogpos += w_motion.StartJogPos;
            ucConvControl_Re.del_Jogneg += w_motion.StartJogNeg;
            ucConvControl_Re.del_Jogstop += w_motion.StopJog;

            del_lineAdd += ucCVCreate.InitializeLines;

            del_param += ucParameter.InitialParameterGridView;

            xmlControl.LoadLineFromXml(lineFullPath);
            xmlControl.LoadConveyorFromXML(convFullPath);
            xmlControl.LoadRectanglesFromXML(rectFullPath);
            xmlControl.GetProfileParameter(profileFullPath);
            Thread.Sleep(1000);
            ucParameter.Header_Update_Timer.Enabled = true;
        }
        // 화면 전환 시 호출할 메서드

        private void InitializeUi()
        {
            InitializeIOPanel();
            InitializeStatusSettingPanel();
            //InitializeDrivePanel();
            InitializeRFIDPanel();
            InitializeAlarmPanel();
            InitializeMotionPanel();
            InitializeSettingPanel();
        }

        private void Init()
        {
            WMX3.CreateWMX3Device("Conveyor");
            w_motion = new WMXMotion("ConvMotion", true);
            WMX3.StartCommunicate();
            Task.Delay(500);
            w_motion.LoadAndSetAxisParameter(paramFullPath);
            w_motion.GetAndSaveAxisParameter(paramFullPath);
            ThreadStart();
        }
        private void ThreadStart()
        {
            threadControl.IoThread();
            threadControl.ServoCheckThread();
            threadControl.LineCheckThread();
        }
        static public void SetDoubleBuffer(Control control)
        {
            try
            {
                if (control.Controls.Count > 0)
                {
                    foreach (Control item in control.Controls)
                    {
                        SetDoubleBuffer((Control)item);
                    }
                    control.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, control, new object[] { true });
                }
                else
                    control.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, control, new object[] { true });
            }
            catch (Exception ex)
            {
                // 예외가 발생할 경우 처리
            }
        }
        private void DisConnectWMX()
        {
            WMX3.StopCommunicate();
            w_motion.CloseWMXMotion();
            WMX3.CloseDevice();
        }
        private void InitializeIOPanel()
        {
            IOPanel.RowCount = 2; // 수정
            IOPanel.ColumnCount = 2;

            IOPanel.RowStyles.Clear();
            IOPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 8F));
            IOPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 92F));

            IOPanel.ColumnStyles.Clear();
            IOPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            IOPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        }
        private void InitializeStatusSettingPanel() 
        {
            Status_SettingPanel.RowCount = 1;
            Status_SettingPanel.ColumnCount = 3;

            // 각 행과 열에 비례적으로 크기를 설정 (50%씩)
            Status_SettingPanel.RowStyles.Clear();
            Status_SettingPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // 전체를 차지하는 하나의 행

            // 열 스타일 설정 (각 열을 동일한 비율로 설정)
            Status_SettingPanel.ColumnStyles.Clear();
            Status_SettingPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F)); // 첫 번째 열 33.33%
            Status_SettingPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F)); // 두 번째 열 33.33%
            Status_SettingPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F)); // 세 번째 열 33.33%
        }
        private void InitializeRFIDPanel()
        {
            RFIDPanel.RowCount = 2; //수정
            RFIDPanel.ColumnCount = 2;

            // 각 행과 열에 비례적으로 크기를 설정 (50%씩)
            RFIDPanel.RowStyles.Clear();
            RFIDPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            RFIDPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            RFIDPanel.ColumnStyles.Clear();
            RFIDPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            RFIDPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        }
        private void InitializeAlarmPanel()
        {
            // alarmPanel을 2행 2열로 설정
            alarmPanel.RowCount = 2;
            alarmPanel.ColumnCount = 2;

            // 각 행과 열에 비례적으로 크기를 설정 (50%씩)
            alarmPanel.RowStyles.Clear();
            alarmPanel.ColumnStyles.Clear();
            alarmPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            alarmPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 65F));


            alarmPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            alarmPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        }
        // Motion 화면을 표현하기 위한 레이아웃 조절
        private void InitializeMotionPanel()
        {
            // MotionPanel을 1행 3열로 설정
            MotionPanel.RowCount = 1;
            MotionPanel.ColumnCount = 3;
            MotionPanel.RowStyles.Clear();
            MotionPanel.ColumnStyles.Clear();

            // MotionPanel의 행 분할 비율 설정
            MotionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // MotionPanel의 열 분할 비율 설정

            MotionPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            MotionPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            MotionPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
              
        }

        private void InitializeSettingPanel()
        {
            // SettingPanel을 1행 0열로 설정
            SettingPanel.RowCount = 1;
            SettingPanel.ColumnCount = 2;
            SettingPanel.RowStyles.Clear();
            SettingPanel.ColumnStyles.Clear();

            // SettingPanel의 행 분할 비율 설정
            SettingPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // SettingPanel의 열 분할 비율 설정

            SettingPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            SettingPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            //SettingPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

        }
        //Main
        private void MonitorMain()
        {
            ClearRow(Tb_Main_Dep_01, 1);
            ClearRow(Tb_Main_Dep_03_Monitor, 1);
            ClearRow(Tb_Main_Dep_03_Monitor, 0);

            SplitMain.Panel1.Controls.Clear();
            SplitLog.Panel1.Controls.Clear();
            SplitLog.Panel2.Controls.Clear();
            // AlarmPanel의 Dock 스타일 설정
            

            // AlarmPanel을 SplitMain.Panel1에 추가
            SplitMain.Panel1.Controls.Add(Tb_Main_Dep_03_Monitor);

            StorageProgress prograss    = new StorageProgress();
            Tb_Main_Dep_03_Monitor.Dock = DockStyle.Fill;
            prograss.Dock               = DockStyle.Fill;
            ucConvLineView.Dock         = DockStyle.Fill;
            ucTopComm.Dock              = DockStyle.Fill;
            userAlarmHistory.Dock       = DockStyle.Fill;
            userSafety.Dock             = DockStyle.Fill;
            userAutoCondition.Dock      = DockStyle.Fill;
            userEQstatus.Dock           = DockStyle.Fill;
 
            Tb_Main_Dep_03_Monitor.Controls.Add(userSafety, 0, 0);
            Tb_Main_Dep_03_Monitor.Controls.Add(userAutoCondition, 1, 0);
            Tb_Main_Dep_03_Monitor.Controls.Add(userEQstatus, 2, 0);
            Tb_Main_Dep_03_Monitor.Controls.Add(userAlarmHistory, 3, 0);
            Tb_Main_Dep_03_Monitor.SetRowSpan(userAlarmHistory, 2);
            Tb_Main_Dep_03_Monitor.Controls.Add(prograss, 0, 1);
            Tb_Main_Dep_03_Monitor.Controls.Add(ucConvLineView, 1, 1);
            Tb_Main_Dep_03_Monitor.SetColumnSpan(ucConvLineView, 2);
            Tb_Main_Dep_01.Controls.Add(ucTopComm, 0, 1);
            SplitLog.Panel2.Controls.Add(ucLog);
            SplitLog.Panel1.Controls.Add(ucMainStatus);
        }
        //Status
        private void MonitorStatus()
        {
            ClearRow(Tb_Main_Dep_01, 1);
            ClearRow(Tb_Main_Dep_03_Monitor, 1);
            ClearRow(Tb_Main_Dep_03_Monitor, 0);

            SplitMain.Panel1.Controls.Clear();
            SplitLog.Panel1.Controls.Clear();
            SplitLog.Panel2.Controls.Clear();

            // AlarmPanel의 Dock 스타일 설정
            Tb_Main_Dep_03_Monitor.Dock = DockStyle.Fill;

            // AlarmPanel을 SplitMain.Panel1에 추가
            SplitMain.Panel1.Controls.Add(Tb_Main_Dep_03_Monitor);

            userSafety.Dock = DockStyle.Fill;
            userAutoCondition.Dock = DockStyle.Fill;
            userEQstatus.Dock = DockStyle.Fill;
            ucSap.Dock = DockStyle.Fill;
            ucAuto.Dock = DockStyle.Fill;
            ucEqCondition.Dock = DockStyle.Fill;
            ucSatus.Dock = DockStyle.Fill;
            ucPIOStatus.Dock = DockStyle.Fill;
            ucCvStatus.Dock = DockStyle.Fill;

            Tb_Main_Dep_03_Monitor.Controls.Add(userSafety, 0, 0);
            Tb_Main_Dep_03_Monitor.Controls.Add(userAutoCondition, 1, 0);
            Tb_Main_Dep_03_Monitor.Controls.Add(userEQstatus, 2, 0);
            Tb_Main_Dep_03_Monitor.Controls.Add(ucSap, 0, 1);
            Tb_Main_Dep_03_Monitor.Controls.Add(ucAuto, 1, 1);
            Tb_Main_Dep_03_Monitor.Controls.Add(ucEqCondition, 2, 1);
            Tb_Main_Dep_03_Monitor.Controls.Add(ucPIOStatus, 3, 0);
            Tb_Main_Dep_03_Monitor.SetRowSpan(ucPIOStatus, 2);

            Tb_Main_Dep_01.Controls.Add(ucSatus, 0, 1);

            SplitLog.Panel2.Controls.Add(ucLog);
            SplitLog.Panel1.Controls.Add(ucCvStatus);
        }
        private void MonitorStatus_main()
        {
            ClearRow(Tb_Main_Dep_03_Monitor, 1);
            ClearRow(Tb_Main_Dep_03_Monitor, 0);

            SplitMain.Panel1.Controls.Clear();

            // AlarmPanel의 Dock 스타일 설정
            Tb_Main_Dep_03_Monitor.Dock = DockStyle.Fill;

            // AlarmPanel을 SplitMain.Panel1에 추가
            SplitMain.Panel1.Controls.Add(Tb_Main_Dep_03_Monitor);

            userSafety.Dock = DockStyle.Fill;
            userAutoCondition.Dock = DockStyle.Fill;
            userEQstatus.Dock = DockStyle.Fill;
            ucSap.Dock = DockStyle.Fill;
            ucAuto.Dock = DockStyle.Fill;
            ucEqCondition.Dock = DockStyle.Fill;
            ucSatus.Dock = DockStyle.Fill;
            ucPIOStatus.Dock = DockStyle.Fill;

            Tb_Main_Dep_03_Monitor.Controls.Add(userSafety, 0, 0);
            Tb_Main_Dep_03_Monitor.Controls.Add(userAutoCondition, 1, 0);
            Tb_Main_Dep_03_Monitor.Controls.Add(userEQstatus, 2, 0);
            Tb_Main_Dep_03_Monitor.Controls.Add(ucSap, 0, 1);
            Tb_Main_Dep_03_Monitor.Controls.Add(ucAuto, 1, 1);
            Tb_Main_Dep_03_Monitor.Controls.Add(ucEqCondition, 2, 1);
            Tb_Main_Dep_03_Monitor.Controls.Add(ucPIOStatus, 3, 0);
            Tb_Main_Dep_03_Monitor.SetRowSpan(ucPIOStatus, 2);

            Tb_Main_Dep_01.Controls.Add(ucSatus, 0, 1);
        }

        private void MonitorIO()
        {
            // SplitMain.Panel1의 컨트롤을 초기화
            SplitMain.Panel1.Controls.Clear();

            IOPanel.Dock = DockStyle.Fill;
              // AlarmPanel을 SplitMain.Panel1에 추가
            SplitMain.Panel1.Controls.Add(IOPanel);

            ucIDSelect.Dock = DockStyle.Fill;
            ucInput.Dock = DockStyle.Fill;
            ucOutput.Dock = DockStyle.Fill;

            IOPanel.Controls.Add(ucIDSelect,0,0);
            IOPanel.SetColumnSpan(ucIDSelect, 2);
            IOPanel.Controls.Add(ucInput,0,1);
            IOPanel.Controls.Add(ucOutput,1,1);
        }
        private void MonitorStatusSetting() 
        {
            // SplitMain.Panel1의 컨트롤을 초기화
            SplitMain.Panel1.Controls.Clear();

            Status_SettingPanel.Dock = DockStyle.Fill;

            // AlarmPanel을 SplitMain.Panel1에 추가
            SplitMain.Panel1.Controls.Add(Status_SettingPanel);

            ClearRow(Tb_Main_Dep_03_Monitor, 1);
            ClearRow(Tb_Main_Dep_03_Monitor, 0);

            ucStep.Dock = DockStyle.Fill;
            ucMotion.Dock = DockStyle.Fill;
            ucSetting.Dock = DockStyle.Fill;

            Status_SettingPanel.Controls.Add(ucStep, 0, 0);
            Status_SettingPanel.Controls.Add(ucMotion, 1, 0);
            Status_SettingPanel.Controls.Add(ucSetting, 2, 0);
        }

        private void MonitorRFID()
        {
            // SplitMain.Panel1의 컨트롤을 초기화
            SplitMain.Panel1.Controls.Clear();

            RFIDPanel.Dock = DockStyle.Fill;

            // AlarmPanel을 SplitMain.Panel1에 추가
            SplitMain.Panel1.Controls.Add(RFIDPanel);

            ucRFIDStatus.Dock = DockStyle.Fill;
            ucRFIDSetting.Dock = DockStyle.Fill;

            RFIDPanel.Controls.Add(ucRFIDStatus,0,0);
            RFIDPanel.Controls.Add(ucRFIDSetting,1,0);
        }
        //Alarm
        private void MonitorAlarm()
        {
            ClearRow(Tb_Main_Dep_03_Monitor, 1);
            ClearRow(Tb_Main_Dep_01, 1);
            ClearRow(Tb_Main_Dep_03_Monitor, 0);
            SplitLog.Panel1.Controls.Clear();
            SplitLog.Panel2.Controls.Clear();

            // SplitMain.Panel1의 컨트롤을 초기화
            SplitMain.Panel1.Controls.Clear();

            // AlarmPanel의 Dock 스타일 설정
            alarmPanel.Dock = DockStyle.Fill;

            // AlarmPanel을 SplitMain.Panel1에 추가
            SplitMain.Panel1.Controls.Add(alarmPanel);
            userCurrentAlarm.Dock = DockStyle.Fill;
            userSolution.Dock = DockStyle.Fill;
            userAlarmHistory.Dock = DockStyle.Fill;
            ucCvStatus.Dock = DockStyle.Fill;
            alarmPanel.Controls.Add(userCurrentAlarm, 0, 0);
            alarmPanel.SetRowSpan(userCurrentAlarm, 2);
            alarmPanel.Controls.Add(userSolution, 1, 0);
            alarmPanel.Controls.Add(userAlarmHistory, 1, 1);
            alarmPanel.SetRowSpan(userAlarmHistory, 1);

            SplitLog.Panel2.Controls.Add(ucLog);
            SplitLog.Panel1.Controls.Add(ucCvStatus);
        }
        //Motion
        private void MonitorMotion()
        {
            SplitLog.Panel1.Controls.Clear();
            SplitLog.Panel2.Controls.Clear();
            SplitMain.Panel1.Controls.Clear();

            //SplitMain.SplitterDistance = (int)(0.60f * SplitMain.Height);

            //Table Main 초기화
            ClearRow(Tb_Main_Dep_01, 1);
            ClearRow(MotionPanel, 1);
            ClearRow(MotionPanel, 0);

            //SplitMain.Panel1의 컨트롤을 초기화
            SplitMain.Panel1.Controls.Add(MotionPanel);

            ucConvLineView.Dock = DockStyle.Fill;
            MotionPanel.Dock = DockStyle.Fill;
            ucTopComm_Motion.Dock = DockStyle.Fill;
            ucIO_Motion.Dock = DockStyle.Fill;
            ucPowerOn.Dock = DockStyle.Fill;
            ucConvControl_Re.Dock = DockStyle.Fill;
            ucCvStatus.Dock = DockStyle.Fill;

            MotionPanel.Controls.Add(ucConvLineView, 0, 0);
            MotionPanel.SetColumnSpan(ucConvLineView, 1);
            MotionPanel.SetRowSpan(ucConvLineView, 1);
            MotionPanel.Controls.Add(ucIO_Motion, 2, 0);
            MotionPanel.Controls.Add(ucPowerOn, 1, 0);

            Tb_Main_Dep_01.Controls.Add(ucTopComm_Motion, 0, 1);
            SplitLog.Panel1.Controls.Add(ucCvStatus);
            SplitLog.Panel2.Controls.Add(ucConvControl_Re);

        }
        private void MonitorManual()
        {
            ClearRow(MotionPanel, 1);
            ClearRow(MotionPanel, 0);
            SplitLog.Panel2.Controls.Clear();
            //SplitMain.Panel1.Controls.Clear();


            MotionPanel.Controls.Add(ucConvLineView, 0, 0);
            MotionPanel.SetColumnSpan(ucConvLineView, 1);
            MotionPanel.SetRowSpan(ucConvLineView, 1);

            MotionPanel.Controls.Add(ucIO_Motion, 2, 0);
            MotionPanel.Controls.Add(ucPowerOn, 1, 0);

            SplitLog.Panel2.Controls.Add(ucConvControl_Re);

            ucConvLineView.Dock = DockStyle.Fill;
            ucIO_Motion.Dock = DockStyle.Fill;
            ucPowerOn.Dock = DockStyle.Fill;
            ucConvControl_Re.Dock = DockStyle.Fill;
        }
        private void MonitorTeaching()
        {
            ClearRow(MotionPanel, 1);
            ClearRow(MotionPanel, 0);
            SplitLog.Panel2.Controls.Clear();

            MotionPanel.Controls.Add(ucConvLineView, 0, 0);
            MotionPanel.SetColumnSpan(ucConvLineView, 1);
            MotionPanel.SetRowSpan(ucConvLineView, 1);

            MotionPanel.Controls.Add(ucIO_Motion, 2, 0);
            MotionPanel.Controls.Add(ucPowerOn, 1, 0);

            SplitLog.Panel2.Controls.Add(ucConvTeaching_Re);

            ucIO_Motion.Dock = DockStyle.Fill;
            ucPowerOn.Dock = DockStyle.Fill;
            ucConvTeaching_Re.Dock = DockStyle.Fill;
        }
        private void MonitorCycleTest()
        {
            ClearRow(MotionPanel, 1);
            ClearRow(MotionPanel, 0);
            SplitLog.Panel2.Controls.Clear();

            MotionPanel.Controls.Add(ucConvLineView, 0, 0);
            MotionPanel.SetColumnSpan(ucConvLineView, 1);
            MotionPanel.SetRowSpan(ucConvLineView, 1);

            MotionPanel.Controls.Add(ucIO_Motion, 2, 0);
            MotionPanel.Controls.Add(ucPowerOn, 1, 0);
            MotionPanel.Controls.Add(ucCycleStatus, 2, 0);

            SplitLog.Panel2.Controls.Add(ucCycleTest);

            ucCycleStatus.Dock = DockStyle.Fill;
            ucPowerOn.Dock = DockStyle.Fill;
            ucCycleTest.Dock = DockStyle.Fill;

        }
        //Setting
        private void MonitorSetting()
        {
            SplitLog.Panel1.Controls.Clear();
            SplitLog.Panel2.Controls.Clear();
            SplitMain.Panel1.Controls.Clear();
            SettingPanel.Controls.Clear();

            SplitMain.SplitterDistance = (int)(0.45f * this.Height);
            SplitLog.Panel1Collapsed = false;

            ClearRow(Tb_Main_Dep_01, 1);
            SettingPanel.Dock = DockStyle.Fill;

            SettingPanel.Controls.Add(ucParameter);
            SettingPanel.SetColumnSpan(ucParameter, 2);
            SplitMain.Panel1.Controls.Add(SettingPanel);

            Tb_Main_Dep_01.Controls.Add(ucTopComm_Setting, 0, 1);
            SplitLog.Panel1.Controls.Add(ucCvStatus);
            SplitLog.Panel2.Controls.Add(ucLog);

            ucTopComm_Setting.Dock = DockStyle.Fill;
            ucParameter.Dock = DockStyle.Fill;
            ucCvStatus.Dock = DockStyle.Fill;
            ucLog.Dock = DockStyle.Fill;
        }

        private void MonitorPara()
        {
            // SettingPanel 초기화
            SettingPanel.Controls.Clear();
            SplitMain.Panel1.Controls.Clear();
            SettingPanel.SetColumnSpan(ucParameter, 2);

            // Panel 크기 및 레이아웃 설정
            SettingPanel.Dock = DockStyle.Fill;

            // Parameter 컨트롤 추가
            SettingPanel.Controls.Add(ucParameter);
            SplitMain.Panel1.Controls.Add(SettingPanel);

            // 모든 컨트롤 DockStyle 설정
            ucParameter.Dock = DockStyle.Fill;
        }

        private void MonitorOperation()
        {
            SettingPanel.Controls.Clear();
            SettingPanel.Dock = DockStyle.Fill;

            SettingPanel.Controls.Add(ucOperation);
            SettingPanel.SetColumnSpan(ucOperation, 2);
            SplitMain.Panel1.Controls.Add(SettingPanel);

            ucOperation.Dock = DockStyle.Fill;
        }

        private void MonitorWachdogTime()
        {
            SettingPanel.Controls.Clear();
            SplitMain.SplitterDistance = (int)(0.45f * this.Height);
            SplitLog.Panel1Collapsed = false;

            SettingPanel.Dock = DockStyle.Fill;
            SettingPanel.Controls.Add(ucWatchdog);
            SettingPanel.SetColumnSpan(ucWatchdog, 2);
            SplitMain.Panel1.Controls.Add(SettingPanel);

            ucWatchdog.Dock = DockStyle.Fill;
        }
        private void MonitorLine()
        {
            SettingPanel.Controls.Clear();
            SplitMain.SplitterDistance = (int)(0.45f * this.Height);
            SplitLog.Panel1Collapsed = false;


            SettingPanel.Controls.Add(ucConvLineView, 0, 0);
            SettingPanel.SetColumnSpan(ucConvLineView, 1);
            SettingPanel.Controls.Add(ucLineCreate, 1, 0);

            SplitMain.Panel1.Controls.Add(SettingPanel);

            SettingPanel.Dock = DockStyle.Fill;
            ucLineCreate.Dock = DockStyle.Fill;

        }
        private void MonitorCnvCreate()
        {
            SettingPanel.Controls.Clear();
            SplitMain.SplitterDistance = (int)(0.45f * this.Height);
            SplitLog.Panel1Collapsed = false;


            SettingPanel.Controls.Add(ucConvLineView, 0, 0);
            SettingPanel.SetColumnSpan(ucConvLineView, 1);
            SettingPanel.Controls.Add(ucCVCreate,1,0);
            //SettingPanel.SetColumnSpan(ucCVCreate, 1);
            SplitMain.Panel1.Controls.Add(SettingPanel);
            //ucConvLineView.Dock = DockStyle.Fill;
            SettingPanel.Dock = DockStyle.Fill;
            ucCVCreate.Dock = DockStyle.Fill;

        }
        // 화면 설정 돌리기
        private void initDefult()
        {
            // 기본 화면 비율 SplitPanel을 조정
            SplitLog.Panel1Collapsed = false;
            SplitMain.SplitterDistance = (int)((472f/792f)*SplitMain.Height);
            Tb_Main_Dep_03_Monitor.RowStyles.Clear();
            Tb_Main_Dep_03_Monitor.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
            Tb_Main_Dep_03_Monitor.RowStyles.Add(new RowStyle(SizeType.Percent, 72F));
        }
        private void ClearRow(TableLayoutPanel panel, int row)
        {
            // 주어진 행에 있는 모든 컨트롤을 리스트로 임시 저장
            List<Control> controlsToRemove = new List<Control>();

            foreach (Control control in panel.Controls)
            {
                TableLayoutPanelCellPosition position = panel.GetCellPosition(control);
                if (position.Row == row)
                {
                    controlsToRemove.Add(control);
                }
            }
            // 컨트롤을 panel에서 제거
            foreach (Control control in controlsToRemove)
            {
                panel.Controls.Remove(control);
            }
        }
        private void ClearRow(SplitterPanel splitterpanel)
        {
            // 컨트롤을 Split에서 제거
            splitterpanel.Controls.Clear();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            UserSafety ucSafety = new UserSafety();
            UserAutoCondition ucAutCondition = new UserAutoCondition();
            UserEQstatus ucEQstatus = new UserEQstatus();

            //버전관리
            Version oVersion = Assembly.GetEntryAssembly().GetName().Version;
            this.Text = string.Format("{0} Ver.{1}.{2} / Build Time({3}) {4}", "Automation", oVersion.Major, oVersion.Minor, GetBuildDataTime(oVersion), "");

            GetBuildDataTime(oVersion);

            ucLog.Dock          = DockStyle.Fill;
            ucMainStatus.Dock   = DockStyle.Fill;
            ucSafety.Dock       = DockStyle.Fill;
            ucAutCondition.Dock = DockStyle.Fill;
            ucEQstatus.Dock     = DockStyle.Fill;

            SplitLog.Panel2.Controls.Add(ucLog);
            SplitLog.Panel1.Controls.Add(ucMainStatus);
            MonitorMain();


        }
        private void InitializePanel(TableLayoutPanel panel, int rowCount, int colCount, float[] rowPercentages, float[] colPercentages)
        {
            panel.RowCount = rowCount;
            panel.ColumnCount = colCount;
            panel.RowStyles.Clear();
            panel.ColumnStyles.Clear();

            for (int i = 0; i < rowCount; i++)
            {
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, rowPercentages[i]));
            }
            for (int i = 0; i < colCount; i++)
            {
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, colPercentages[i]));
            }
        }

        public DateTime GetBuildDataTime(Version oVersion)
        {
            string strVersion = oVersion.ToString();

         
            //날짜 등록
            int jDays = Convert.ToInt32(strVersion.Split('.')[2]);
            DateTime refData = new DateTime(2000, 1, 1);
            //일수 계산
            DateTime dtBuildData = refData.AddDays(jDays);

            //초 등록
            int iSeconds = Convert.ToInt32(strVersion.Split('.')[3]);
            iSeconds = iSeconds * 2;
            dtBuildData = dtBuildData.AddSeconds(iSeconds);

            //시차 조정
            DaylightTime daylightTime = TimeZone.CurrentTimeZone.GetDaylightChanges(dtBuildData.Year);

            if (TimeZone.IsDaylightSavingTime(dtBuildData, daylightTime))
            {
                dtBuildData = dtBuildData.Add(daylightTime.Delta);
            }
            return dtBuildData;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isMainFormOpen = false;

            DisConnectWMX();
        }

        private void UIUpdate_Timer_Tick_300(object sender, EventArgs e)
        {
            userSafety.UpdateSafetyUI();
            userEQstatus.CheckAuto();
            userAutoCondition.UpdateAutoEnable();
            ucMainStatus.SetLineStatusDg();
            ucTopComm.IsAllServoOn();
            ucTopComm.btnEtherCatColor_Change();
            ucIO_Motion.SetConvDginput();
            ucOutput.UpdateOutputGrid();
            ucAuto.AutoConditionUpdate();
            ucSap.SafetyDataGridUpdate();
        }

    }
}
