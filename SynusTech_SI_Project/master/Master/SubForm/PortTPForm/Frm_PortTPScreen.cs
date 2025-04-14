using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.SubForm.PortTPForm.PortTPSubForm;
using Master.SubForm.PortTPForm.PortTPSubForm.ControlForm;
using Master.SubForm.PortTPForm.PortTPSubForm.StatusForm;
using Master.Equipment.Port;
using Master.GlobalForm;

namespace Master.SubForm.PortTPForm
{
    public partial class Frm_PortTPScreen : Form
    {
        enum MenuButtonIndex : int
        {
            Main,
            IO,
            Motor,
            Teaching,
            Settings,
            MappingInfo,
            CSTInfo,
            Log,
            AlarmInfo
        }

        private MenuButtonIndex eCurrentWindowIndex = (MenuButtonIndex)(-1);
        Button[] m_MenuButtonGroup;
        Form[] m_MenuFormGroup;
        Button[] m_PortSelectButtonGroup;

        Frm_IOTable frm_IOTable;

        Frm_PortStatus frm_PortStatus;
        Frm_MotionSelect frm_MotionSelect;

        Frm_PortTPMain frm_PortTPMain;
        Frm_PortTPIO frm_PortTPIO;
        Frm_PortTPMotor frm_PortTPMotor;
        Frm_PortTPTeaching frm_PortTPTeaching;
        Frm_PortTPSettings frm_PortSettings;
        Frm_PortTPMappingInfo frm_PortTPMappingInfo;
        Frm_PortCSTInfo frm_PortCSTInfo;
        Frm_PortTPLog frm_PortTPLog;
        Frm_PortAlarmInfo frm_PortAlarmInfo;


        DataGridView LogDGV;
        static public object g_CurrentPort = null;

        public Frm_PortTPScreen(bool bTPScreen = false)
        {
            InitializeComponent();
            ControlItemInit();

            if(!bTPScreen)
                btn_Navigation_Click(btn_Main, new EventArgs());

            this.VisibleChanged += (object sender, EventArgs e) =>
            {
                if (this.Visible)
                {
                    UIUpdateTimer.Enabled = true;
                }
                else
                {
                    UIUpdateTimer.Enabled = false;
                }
            };

            this.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;

                frm_PortStatus.Visible = false;
                frm_MotionSelect.Visible = false;

                frm_PortTPMain.Visible = false;
                frm_PortTPIO.Visible = false;
                frm_PortTPMotor.Visible = false;
                frm_PortTPTeaching.Visible = false;
                frm_PortSettings.Visible = false;
                frm_PortTPMappingInfo.Visible = false;
                frm_PortCSTInfo.Visible = false;
                frm_PortTPLog.Visible = false;
                frm_PortAlarmInfo.Visible = false;

                frm_PortStatus.Close();
                frm_MotionSelect.Close();

                frm_PortTPMain.Close();
                frm_PortTPIO.Close();
                frm_PortTPMotor.Close();
                frm_PortTPTeaching.Close();
                frm_PortSettings.Close();
                frm_PortTPMappingInfo.Close();
                frm_PortCSTInfo.Close();
                frm_PortTPLog.Close();
                frm_PortAlarmInfo.Close();

                if (frm_IOTable != null)
                {
                    frm_IOTable.Close();
                }
            };

            this.Disposed += (object sender, EventArgs e) =>
            {
                Log.logInsertEvent -= UpdateLogDGV;

                frm_PortStatus.Dispose();
                frm_MotionSelect.Dispose();

                frm_PortTPMain.Dispose();
                frm_PortTPIO.Dispose();
                frm_PortTPMotor.Dispose();
                frm_PortTPTeaching.Dispose();
                frm_PortSettings.Dispose();
                frm_PortTPMappingInfo.Dispose();
                frm_PortCSTInfo.Dispose();
                frm_PortTPLog.Dispose();
                frm_PortAlarmInfo.Dispose();

                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }

        public void SetAutoScale(float FactorX, float FactorY)
        {
            this.AutoScaleMode = AutoScaleMode.None;
            this.Scale(new SizeF(FactorX, FactorY));

            foreach (Control item in this.Controls)
                ControlFunc.ResizeFont(item, FactorY);

            tableLayoutPanel1.RowStyles[0].Height = 70.0f;

            frm_PortStatus.SetAutoScale(FactorX, FactorY);
            frm_MotionSelect.SetAutoScale(FactorX, FactorY);
            frm_PortTPMain.SetAutoScale(FactorX, FactorY);
            frm_PortTPIO.SetAutoScale(FactorX, FactorY);
            frm_PortTPMotor.SetAutoScale(FactorX, FactorY);
            frm_PortTPTeaching.SetAutoScale(FactorX, FactorY);
            frm_PortSettings.SetAutoScale(FactorX, FactorY);
            frm_PortTPMappingInfo.SetAutoScale(FactorX, FactorY);
            frm_PortCSTInfo.SetAutoScale(FactorX, FactorY);
            frm_PortTPLog.SetAutoScale(FactorX, FactorY);
            frm_PortAlarmInfo.SetAutoScale(FactorX, FactorY);

            btn_Navigation_Click(btn_Main, new EventArgs());
            LogDGV.Font = new Font(LogDGV.Font.FontFamily, LogDGV.Font.Size + 2.0f, LogDGV.Font.Style);
        }

        private void LogDGVInit()
        {
            LogDGV = Log.CreateLogGridView();
            Log.LogDGVReload(LogDGV);
            Log.logInsertEvent += UpdateLogDGV;

            if (!pnl_LogDGVPanel.Controls.Contains(LogDGV))
            {
                pnl_LogDGVPanel.Controls.Clear();
                pnl_LogDGVPanel.Controls.Add(LogDGV);
                pnl_LogDGVPanel.Controls[0].Dock = DockStyle.Fill;
            }
        }
        private void UpdateLogDGV(LogMsg _LogMsg)
        {
            if (!this.IsDisposed)
                Log.InsertLogDGV(LogDGV, _LogMsg);
        }

        private void ContextMenuInit()
        {
            ContextMenu cm = new ContextMenu();

            MenuItem item = new MenuItem();
            item.Name = "ContextMenuExit";
            item.Text = "Clear";
            item.Click += btn_PortErrorClear_Click;
            cm.MenuItems.Add(item);

            lbl_Alarm.ContextMenu = cm;

            ContextMenu pm = new ContextMenu();

            MenuItem pitem = new MenuItem();
            pitem.Name = "Open I/O Map";
            pitem.Text = "Open I/O Map";
            pitem.Click += btn_OpenIOMap_Click;
            pm.MenuItems.Add(pitem);

            panel_Top.ContextMenu = pm;
        }

        private void btn_OpenIOMap_Click(object sender, EventArgs e)
        {
            if(frm_IOTable == null)
            {
                frm_IOTable = new Frm_IOTable();
                //frm_IOTable.TopLevel = false;
                frm_IOTable.Visible = true;
                frm_IOTable.Show();
                frm_IOTable.FormClosed += Frm_IOTable_FormClosed;
            }
            else
            {
                frm_IOTable.WindowState = FormWindowState.Normal;
                frm_IOTable.Visible = true;
                frm_IOTable.Show();
                frm_IOTable.BringToFront();
            }
        }

        private void Frm_IOTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_IOTable.Dispose();
            frm_IOTable = null;
        }

        private void btn_PortErrorClear_Click(object sender, EventArgs e)
        {
            if (g_CurrentPort == null)
                return;

            Port port = (Port)g_CurrentPort;

            port.Interlock_PortAmpAlarmClear(Port.InterlockFrom.UI_Event);
            port.Interlock_PortAlarmClear(Port.InterlockFrom.UI_Event);
        }

        private void NavigationMapping()
        {
            m_MenuButtonGroup = new Button[]
            {
                btn_Main,
                btn_IO,
                btn_Motor,
                btn_Teaching,
                btn_Settings,
                btn_MappingInfo,
                btn_CSTInfo,
                btn_Log,
                btn_AlarmInfo
            };

            m_MenuFormGroup = new Form[]
            {
                frm_PortTPMain,
                frm_PortTPIO,
                frm_PortTPMotor,
                frm_PortTPTeaching,
                frm_PortSettings,
                frm_PortTPMappingInfo,
                frm_PortCSTInfo,
                frm_PortTPLog,
                frm_PortAlarmInfo
            };
        }

        private void ControlItemInit()
        {
            LogDGVInit();

            frm_PortStatus = new Frm_PortStatus() { TopLevel = false };
            frm_MotionSelect = new Frm_MotionSelect() { TopLevel = false };

            frm_PortTPMain = new Frm_PortTPMain(frm_PortStatus) { TopLevel = false };
            frm_PortTPIO = new Frm_PortTPIO() { TopLevel = false };
            frm_PortTPMotor = new Frm_PortTPMotor(frm_PortStatus, frm_MotionSelect) { TopLevel = false };
            frm_PortTPTeaching = new Frm_PortTPTeaching(frm_PortStatus, frm_MotionSelect) { TopLevel = false };
            frm_PortSettings = new Frm_PortTPSettings() { TopLevel = false };
            frm_PortTPMappingInfo = new Frm_PortTPMappingInfo() { TopLevel = false };
            frm_PortCSTInfo = new Frm_PortCSTInfo() { TopLevel = false };
            frm_PortTPLog = new Frm_PortTPLog() { TopLevel = false };
            frm_PortAlarmInfo = new Frm_PortAlarmInfo() { TopLevel = false };

            try
            {
                string filePath = ManagedFile.ManagedFileInfo.StartUpPath + "\\logo.png";

                if (System.IO.File.Exists(filePath))
                    pictureBox1.BackgroundImage = Image.FromFile(filePath);
                else
                    pictureBox1.BackgroundImage = Properties.Resources.logo;
            }
            catch
            {
                pictureBox1.BackgroundImage = Properties.Resources.logo;
            }

            FormFunc.SetDoubleBuffer(this);

            NavigationMapping();

            g_CurrentPort = null;

            m_PortSelectButtonGroup = new Button[] { btn_PortSelect1, btn_PortSelect2, btn_PortSelect3, btn_PortSelect4, btn_PortSelect5 };

            for (int nCount = 0; nCount < m_PortSelectButtonGroup.Length; nCount++)
            {
                m_PortSelectButtonGroup[nCount].Text = string.Empty;
                m_PortSelectButtonGroup[nCount].Visible = false;
                m_PortSelectButtonGroup[nCount].Tag = null;
            }
            tableLayoutPanel_CommanderButton.Tag = 0;

            ContextMenuInit();
        }

        public void Btn_Disable()
        {
            btn_Close.Enabled = false;
            btn_Minimize.Enabled = false;
        }

        private MenuButtonIndex GetMenuButtonIndex(Button btn)
        {
            for(int nCount = 0; nCount < m_MenuButtonGroup.Length; nCount++)
            {
                if (btn == m_MenuButtonGroup[nCount])
                    return (MenuButtonIndex)nCount;
            }

            return MenuButtonIndex.Main;
        }

        private void btn_FocusHandler(MenuButtonIndex eCurrentMenu)
        {
            //Button Back Color
            Color BaseBackColor = Color.Transparent;
            Color ChangeBackColor = Color.LightSteelBlue;

            //Button Fore Color
            Color BaseForeColor = Color.Black;
            Color ChangeForeColor = Color.Black;

            for (int nCount = 0; nCount < Enum.GetValues(typeof(MenuButtonIndex)).Length; nCount++)
            {
                m_MenuButtonGroup[nCount].BackColor = BaseBackColor;
                m_MenuButtonGroup[nCount].ForeColor = BaseForeColor;
            }

            int nCurrentButtonIndex = (int)eCurrentMenu;

            m_MenuButtonGroup[nCurrentButtonIndex].BackColor = ChangeBackColor;
            m_MenuButtonGroup[nCurrentButtonIndex].ForeColor = ChangeForeColor;
        }
        private void Pnl_NaviBarUpdate()
        {
            int nCurrentButtonIndex = (int)eCurrentWindowIndex;

            if (nCurrentButtonIndex < 0 || nCurrentButtonIndex >= m_MenuButtonGroup.Length)
            {
                nCurrentButtonIndex = 0;
                return;
            }

            PnlNav.Height = m_MenuButtonGroup[nCurrentButtonIndex].Height;
            PnlNav.Top = m_MenuButtonGroup[nCurrentButtonIndex].Top;
            PnlNav.Left = m_MenuButtonGroup[nCurrentButtonIndex].Left;
        }
        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                //if (this.WindowState != FormWindowState.Maximized)
                //    this.WindowState = FormWindowState.Maximized;
                
                UIUpdateTimer.Interval = Master.UIUpdateIntervalTime;
                NowTimeUpdate();
                LaguageCheck();
                LogOnTimeCheck();
                PortSelectButtonUpdate();
                Pnl_NaviBarUpdate();

                LabelFunc.SetText(lbl_CPUUsageTitle, SynusLangPack.GetLanguage("Label_CPUUsage") + " :");
                LabelFunc.SetText(lbl_RAMUsageTitle, SynusLangPack.GetLanguage("Label_RAMUsage") + " :");
                LabelFunc.SetText(lbl_CPUUsage, Frm_Main.CPUUsageStr);
                LabelFunc.SetText(lbl_RAMUsage, Frm_Main.RAMUsageStr);

                if (g_CurrentPort != null)
                {
                    Port port = (Port)g_CurrentPort;

                    port.Update_Btn_PortEStop(ref btn_EMO);
                    port.Update_Btn_PortAlarmClear(ref btn_AlarmClear);

                    port.Update_Lbl_OperationMode(ref lbl_Mode);
                    port.Update_Lbl_PortDirection(ref lbl_PortDirection);
                    port.Update_Lbl_AutoControlStatus(ref lbl_AutoControl);
                    port.Update_Lbl_AlarmStatus(ref lbl_Alarm);
                    port.Update_Lbl_AlarmText(ref lbl_AlarmText);

                    //ButtonFunc.SetVisible(btn_IO, !port.IsEQPort());
                    ButtonFunc.SetVisible(btn_Motor, !port.IsEQPort());
                    ButtonFunc.SetVisible(btn_Teaching, !port.IsEQPort());
                    ButtonFunc.SetVisible(btn_CSTInfo, !port.IsEQPort());

                    if (eCurrentWindowIndex == MenuButtonIndex.Motor || 
                        eCurrentWindowIndex == MenuButtonIndex.Teaching || 
                        eCurrentWindowIndex == MenuButtonIndex.CSTInfo)
                    {
                        if(port.IsEQPort())
                            btn_Navigation_Click(btn_Main, new EventArgs());
                    }
                }

                Master.Update_Btn_BuzzerStop(ref btn_BuzzerStop);
            }
            catch { }
            finally
            {

            }
        }
        private void NowTimeUpdate()
        {
            LabelFunc.SetText(lbl_NowTime, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
        }
        private void LaguageCheck()
        {
            ButtonFunc.SetText(btn_Close, SynusLangPack.GetLanguage("Btn_Close"));
            ButtonFunc.SetText(btn_Minimize, SynusLangPack.GetLanguage("Btn_Minimize"));

            LabelFunc.SetText(lbl_NowTimeTitle, SynusLangPack.GetLanguage("Label_NowTime") + " :");
            LabelFunc.SetText(lbl_RemainingTimeTitle, SynusLangPack.GetLanguage("Label_RemainingTime") + " :");

            LabelFunc.SetText(label_Monitoring, SynusLangPack.GetLanguage("Label_Monitoring"));
            LabelFunc.SetText(label_Log, SynusLangPack.GetLanguage("Label_Log"));

            ButtonFunc.SetText(btn_Main, SynusLangPack.GetLanguage("Btn_Main"));
            ButtonFunc.SetText(btn_Teaching, SynusLangPack.GetLanguage("Btn_Teaching"));
            ButtonFunc.SetText(btn_Settings, SynusLangPack.GetLanguage("Btn_Settings"));

            ButtonFunc.SetText(btn_IO, SynusLangPack.GetLanguage("Btn_IO"));
            ButtonFunc.SetText(btn_Motor, SynusLangPack.GetLanguage("Btn_Motor"));
            ButtonFunc.SetText(btn_MappingInfo, SynusLangPack.GetLanguage("Btn_MappingInfo"));
            ButtonFunc.SetText(btn_CSTInfo, SynusLangPack.GetLanguage("Btn_CSTInfo"));
            ButtonFunc.SetText(btn_Log, SynusLangPack.GetLanguage("Btn_Log"));
            ButtonFunc.SetText(btn_AlarmInfo, SynusLangPack.GetLanguage("Btn_AlarmInfo"));

            GroupBoxFunc.SetText(groupBox_PortEquipments, SynusLangPack.GetLanguage("GorupBox_PortInfo"));
        }
        private void LogOnTimeCheck()
        {
            LogIn.LogOnRemaningTimeLabelUpdate(ref lbl_RemainingTime);
            LogIn.LogOnExtendButtonUpdate(ref btn_LogInExtend);
        }

        private void PortSelectButtonUpdate()
        {
            object CommanderPage = tableLayoutPanel_CommanderButton.Tag;

            if (CommanderPage == null || (int)CommanderPage < 0)
                CommanderPage = 0;

            int StartPage = (int)CommanderPage * 5;

            int PageStep = 0;
            int ButtonStep = 0;

            if (Master.m_Ports != null)
            {
                foreach (var port in Master.m_Ports)
                {
                    if (PageStep >= StartPage)
                    {
                        if (ButtonStep < m_PortSelectButtonGroup.Length)
                        {
                            ButtonFunc.SetText(m_PortSelectButtonGroup[ButtonStep], port.Value.GetFocusButtonStr());
                            if (m_PortSelectButtonGroup[ButtonStep].Tag != port.Value)
                            {
                                m_PortSelectButtonGroup[ButtonStep].Tag = port.Value;
                                m_PortSelectButtonGroup[ButtonStep].Visible = true;
                            }

                        }

                        if (ButtonStep < 5)
                            ButtonStep++;
                    }
                    PageStep++;
                }
            }


            int EquipmentCount = (Master.m_Ports?.Count ?? 0);

            ButtonFunc.SetEnable(btn_CommanderNext, ((int)CommanderPage * 5 + 5 >= EquipmentCount) ? false : true);
            ButtonFunc.SetEnable(btn_CommanderPrev, ((int)CommanderPage <= 0) ? false : true);

            if (g_CurrentPort == null && m_PortSelectButtonGroup.Length > 0 && m_PortSelectButtonGroup[0].Tag != null)
            {
                btn_PortSelectButton_Click(m_PortSelectButtonGroup[0], new EventArgs());
            }

            for(int nCount = 0; nCount < m_PortSelectButtonGroup.Length; nCount++)
            {
                if (m_PortSelectButtonGroup[nCount].Tag == g_CurrentPort)
                    m_PortSelectButtonGroup[nCount].BackColor = Color.Lime;
                else
                    m_PortSelectButtonGroup[nCount].BackColor = Color.White;
            }
        }

        private void btn_PortSelectButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Tag == null)
                return;

            g_CurrentPort = (Port)btn.Tag;
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{((Port)btn.Tag).GetParam().ID}] Select Click");
        }
        private void btn_Navigation_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            MenuButtonIndex eWindowButtonIndex = GetMenuButtonIndex(btn);

            if (eCurrentWindowIndex != eWindowButtonIndex)
            {
                eCurrentWindowIndex = eWindowButtonIndex;
                btn_FocusHandler(eCurrentWindowIndex);

                for (int nCount = 0; nCount < panel_WorkWindow.Controls.Count; nCount++)
                {
                    panel_WorkWindow.Controls[nCount].Visible = false;
                }
                panel_WorkWindow.Controls.Clear();
                panel_WorkWindow.Controls.Add(m_MenuFormGroup[(int)eCurrentWindowIndex]);
                m_MenuFormGroup[(int)eCurrentWindowIndex].Dock = DockStyle.Fill;
                m_MenuFormGroup[(int)eCurrentWindowIndex].Visible = true;

                //if(eCurrentWindowIndex == MenuButtonIndex.Main)
                //    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Main Menu Click");
                //else if (eCurrentWindowIndex == MenuButtonIndex.IO)
                //    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-IO Menu Click");
                //else if (eCurrentWindowIndex == MenuButtonIndex.Motor)
                //    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Motor Menu Click");
                //else if (eCurrentWindowIndex == MenuButtonIndex.Teaching)
                //    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Teaching Menu Click");
                //else if (eCurrentWindowIndex == MenuButtonIndex.Settings)
                //    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Settings Menu Click");
                //else if (eCurrentWindowIndex == MenuButtonIndex.MappingInfo)
                //    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Mapping Info Menu Click");
                //else if (eCurrentWindowIndex == MenuButtonIndex.Log)
                //    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Log Menu Click");
            }
        }
        private void btn_CommanderPrev_Click(object sender, EventArgs e)
        {
            object CommandButtonPage = tableLayoutPanel_CommanderButton.Tag;

            if (CommandButtonPage == null || (int)CommandButtonPage < 0)
                CommandButtonPage = 0;

            if ((int)CommandButtonPage > 0)
            {
                for (int nCount = 0; nCount < m_PortSelectButtonGroup.Length; nCount++)
                {
                    m_PortSelectButtonGroup[nCount].Text = string.Empty;
                    m_PortSelectButtonGroup[nCount].Visible = false;
                    m_PortSelectButtonGroup[nCount].Tag = null;
                }
                tableLayoutPanel_CommanderButton.Tag = (int)CommandButtonPage - 1;
            }
        }
        private void btn_CommanderNext_Click(object sender, EventArgs e)
        {
            object CommandButtonPage = tableLayoutPanel_CommanderButton.Tag;

            if (CommandButtonPage == null || (int)CommandButtonPage < 0)
                CommandButtonPage = 0;

            int EquipmentCount = (Master.m_Ports?.Count ?? 0);

            if ((int)CommandButtonPage * 5 + 5 < EquipmentCount)
            {
                for (int nCount = 0; nCount < m_PortSelectButtonGroup.Length; nCount++)
                {
                    m_PortSelectButtonGroup[nCount].Text = string.Empty;
                    m_PortSelectButtonGroup[nCount].Visible = false;
                    m_PortSelectButtonGroup[nCount].Tag = null;
                }
                tableLayoutPanel_CommanderButton.Tag = (int)CommandButtonPage + 1;
            }
        }
        private void btn_Close_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Close Click");
            this.Close();
            g_CurrentPort = null;
        }
        private void btn_EMO_Click(object sender, EventArgs e)
        {
            if (g_CurrentPort == null)
                return;

            Port port = (Port)g_CurrentPort;

            if (port.GetSWEStopState() == Interface.Safty.EStopState.Idle)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{port.GetParam().ID}] SW E-Stop Click");
                port.CMD_PortSetSWEStop();
            }
            else
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{port.GetParam().ID}] SW E-Stop Release Click");
                port.CMD_PortReleaseSWEStop();
            }
        }
        private void btn_BuzzerStop_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, Master.CMD_Buzzer_Mute_REQ ? $"Port Control Form-Buzzer Release Click" : $"Port Control Form-Buzzer Stop Click");
            Master.CMD_Buzzer_Mute_REQ = !Master.CMD_Buzzer_Mute_REQ;
        }

        private void btn_AlarmClear_Click(object sender, EventArgs e)
        {
            if (g_CurrentPort == null)
                return;

            Port port = (Port)g_CurrentPort;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{port.GetParam().ID}] Alarm Clear Click");
            port.Interlock_PortAmpAlarmClear(Port.InterlockFrom.UI_Event);
            port.Interlock_PortAlarmClear(Port.InterlockFrom.UI_Event);
        }

        private void btn_LogInExtend_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Login Extend Click");
            if (LogIn.IsLogIn())
            {
                LogIn.SetLogInExtend();
            }
        }

        private void btn_Minimize_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Minimize Click");
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
