using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Master.Equipment.RackMaster;
using Master.Equipment.Port;
using Master.ManagedFile;
using System.Threading;
using Master.SubForm.RMTPForm.RMTPSubForm;
using Master;

namespace Master.SubForm.RMTPForm
{
    public partial class Frm_RackMasterTPScreen : Form
    {
        enum MenuButtonIndex : int
        {
            Main,
            Teaching,
            Settings,
            MappingInfo
        }

        Frm_RackMasterTPMain frm_RackMasterTPMain;
        Frm_RackMasterTPTeaching frm_RackMasterTPTeaching;
        Frm_RackMasterTPSettings frm_RackMasterTPSettings;
        Frm_RackMasterTPMappingInfo frm_RackMasterTPMappingInfo;

        private MenuButtonIndex eCurrentWindowIndex = (MenuButtonIndex)(-1);
        Button[] m_MenuButtonGroup;
        Form[] m_MenuFormGroup;
        Button[] m_RMSelectButtonGroup;

        DataGridView LogDGV;
        static public object g_CurrentRM = null;

        public Frm_RackMasterTPScreen()
        {
            InitializeComponent();
            ControlItemInit();
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

                frm_RackMasterTPMain.Visible = false;
                frm_RackMasterTPTeaching.Visible = false;

                frm_RackMasterTPSettings.Visible = false;
                frm_RackMasterTPMappingInfo.Visible = false;

                panel_WorkWindow.Controls.Clear();

                frm_RackMasterTPMain.Close();
                frm_RackMasterTPTeaching.Close();

                frm_RackMasterTPSettings.Close();
                frm_RackMasterTPMappingInfo.Close();
            };

            this.Disposed += (object sender, EventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;

                Log.logInsertEvent -= UpdateLogDGV;

                frm_RackMasterTPMain.Dispose();
                frm_RackMasterTPTeaching.Dispose();

                frm_RackMasterTPSettings.Dispose();
                frm_RackMasterTPMappingInfo.Dispose();

                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
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

        private void ControlItemInit()
        {
            LogDGVInit();
            frm_RackMasterTPMain = new Frm_RackMasterTPMain() { TopLevel = false };
            frm_RackMasterTPTeaching = new Frm_RackMasterTPTeaching() { TopLevel = false };
            frm_RackMasterTPMappingInfo = new Frm_RackMasterTPMappingInfo() { TopLevel = false };
            frm_RackMasterTPSettings = new Frm_RackMasterTPSettings() { TopLevel = false };

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

            g_CurrentRM = null;

            m_RMSelectButtonGroup = new Button[] { btn_RMSelect1, btn_RMSelect2, btn_RMSelect3, btn_RMSelect4, btn_RMSelect5 };

            for (int nCount = 0; nCount < m_RMSelectButtonGroup.Length; nCount++)
            {
                m_RMSelectButtonGroup[nCount].Text = string.Empty;
                m_RMSelectButtonGroup[nCount].Visible = false;
                m_RMSelectButtonGroup[nCount].Tag = null;
            }
            tableLayoutPanel_CommanderButton.Tag = 0;

            ContextMenuInit();
        }
        private void ContextMenuInit()
        {
            ContextMenu cm = new ContextMenu();

            MenuItem item = new MenuItem();
            item.Name = "ContextMenuExit";
            item.Text = "Clear";
            item.Click += btn_RMErrorClear_Click;
            cm.MenuItems.Add(item);

            lbl_Alarm.ContextMenu = cm;

            LaguageCheck();
        }

        private void btn_RMErrorClear_Click(object sender, EventArgs e)
        {
            if (g_CurrentRM == null)
                return;

            RackMaster rackMaster = (RackMaster)g_CurrentRM;

            rackMaster.Interlock_SetAlarmClear(RackMaster.InterlockFrom.UI_Event);
        }

        private void NavigationMapping()
        {
            m_MenuButtonGroup = new Button[]
            {
                btn_Main,
                btn_Teaching,
                btn_Settings,
                btn_MappingInfo
            };

            m_MenuFormGroup = new Form[]
            {
                frm_RackMasterTPMain,
                frm_RackMasterTPTeaching,
                frm_RackMasterTPSettings,
                frm_RackMasterTPMappingInfo
            };
        }
        private MenuButtonIndex GetMenuButtonIndex(Button btn)
        {
            for (int nCount = 0; nCount < m_MenuButtonGroup.Length; nCount++)
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

            PnlNav.Height = m_MenuButtonGroup[nCurrentButtonIndex].Height;
            PnlNav.Top = m_MenuButtonGroup[nCurrentButtonIndex].Top;
            PnlNav.Left = m_MenuButtonGroup[nCurrentButtonIndex].Left;

            m_MenuButtonGroup[nCurrentButtonIndex].BackColor = ChangeBackColor;
            m_MenuButtonGroup[nCurrentButtonIndex].ForeColor = ChangeForeColor;
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
                    panel_WorkWindow.Controls[nCount].Hide();
                }
                panel_WorkWindow.Controls.Clear();
                panel_WorkWindow.Controls.Add(m_MenuFormGroup[(int)eCurrentWindowIndex]);
                m_MenuFormGroup[(int)eCurrentWindowIndex].Dock = DockStyle.Fill;
                m_MenuFormGroup[(int)eCurrentWindowIndex].Show();

                //if(eCurrentWindowIndex == MenuButtonIndex.Main)
                //    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Main Menu Click");
                //else if (eCurrentWindowIndex == MenuButtonIndex.Teaching)
                //    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Teaching Menu Click");
                //else if (eCurrentWindowIndex == MenuButtonIndex.Settings)
                //    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Settings Menu Click");
                //else if (eCurrentWindowIndex == MenuButtonIndex.MappingInfo)
                //    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Mapping Info Menu Click");
            }
        }


        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                //if (this.WindowState != FormWindowState.Maximized)
                //    this.WindowState = FormWindowState.Maximized;

                UIUpdateTimer.Interval = Master.UIUpdateIntervalTime;
                NowTimeUpdate();
                LogOnTimeCheck();
                RMSelectButtonUpdate();

                LabelFunc.SetText(lbl_CPUUsage, Frm_Main.CPUUsageStr);
                LabelFunc.SetText(lbl_RAMUsage, Frm_Main.RAMUsageStr);

                if (g_CurrentRM != null)
                {
                    RackMaster rackMaster = (RackMaster)g_CurrentRM;

                    rackMaster.Update_Lbl_RackMasterID(ref lbl_RackMasterID);
                    rackMaster.Update_Lbl_RackMasterOnline(ref lbl_RackMasterOnline);
                    rackMaster.Update_Lbl_AutoStatus(ref lbl_AutoStatus);
                    rackMaster.Update_Lbl_AutoStep(ref lbl_AutoStep);
                    rackMaster.Update_Lbl_FromID(ref lbl_FromID);
                    rackMaster.Update_Lbl_ToID(ref lbl_ToID);
                    rackMaster.Update_Lbl_AccessID(ref lbl_AccessID);
                    rackMaster.Update_Lbl_CarrierID(ref lbl_CarrierID);
                    rackMaster.Update_Lbl_Alarm(ref lbl_Alarm);
                    rackMaster.Update_Lbl_AlarmText(ref lbl_AlarmText);
                    rackMaster.Update_Btn_RMEStop(ref btn_EMO);
                    rackMaster.Update_Btn_RMAlarmClear(ref btn_AlarmClear);
                }

                Master.Update_Btn_BuzzerStop(ref btn_BuzzerStop);
                Master.Update_Lbl_CIMOnline(ref lbl_CIMOnline);
            }
            catch{ }
            finally
            {
            }
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
            ButtonFunc.SetText(btn_MappingInfo, SynusLangPack.GetLanguage("Btn_MappingInfo"));

            GroupBoxFunc.SetText(groupBox_PortEquipments, SynusLangPack.GetLanguage("GorupBox_RMInfo"));

            LabelFunc.SetText(lbl_CPUUsageTitle, SynusLangPack.GetLanguage("Label_CPUUsage") + " :");
            LabelFunc.SetText(lbl_RAMUsageTitle, SynusLangPack.GetLanguage("Label_RAMUsage") + " :");
        }

        private void NowTimeUpdate()
        {
            LabelFunc.SetText(lbl_NowTime, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
        }
        private void LogOnTimeCheck()
        {
            LogIn.LogOnRemaningTimeLabelUpdate(ref lbl_RemainingTime);
            LogIn.LogOnExtendButtonUpdate(ref btn_LogInExtend);
        }

        private void RMSelectButtonUpdate()
        {
            object CommanderPage = tableLayoutPanel_CommanderButton.Tag;

            if (CommanderPage == null || (int)CommanderPage < 0)
                CommanderPage = 0;

            int StartPage = (int)CommanderPage * 5;

            int PageStep = 0;
            int ButtonStep = 0;

            if (Master.m_RackMasters != null)
            {
                foreach (var rackMaster in Master.m_RackMasters)
                {
                    if (PageStep >= StartPage)
                    {
                        if (ButtonStep < m_RMSelectButtonGroup.Length)
                        {
                            ButtonFunc.SetText(m_RMSelectButtonGroup[ButtonStep], rackMaster.Value.GetFocusButtonStr());
                            if (m_RMSelectButtonGroup[ButtonStep].Tag != rackMaster.Value)
                            {
                                m_RMSelectButtonGroup[ButtonStep].Tag = rackMaster.Value;
                                m_RMSelectButtonGroup[ButtonStep].Visible = true;
                            }

                        }

                        if (ButtonStep < 5)
                            ButtonStep++;
                    }
                    PageStep++;
                }
            }


            int EquipmentCount = (Master.m_RackMasters?.Count ?? 0);

            ButtonFunc.SetEnable(btn_CommanderNext, ((int)CommanderPage * 5 + 5 >= EquipmentCount) ? false : true);
            ButtonFunc.SetEnable(btn_CommanderPrev, ((int)CommanderPage <= 0) ? false : true);

            if (g_CurrentRM == null && m_RMSelectButtonGroup.Length > 0 && m_RMSelectButtonGroup[0].Tag != null)
            {
                btn_RMSelectButton_Click(m_RMSelectButtonGroup[0], new EventArgs());
            }

            for (int nCount = 0; nCount < m_RMSelectButtonGroup.Length; nCount++)
            {
                if (m_RMSelectButtonGroup[nCount].Tag == g_CurrentRM)
                    m_RMSelectButtonGroup[nCount].BackColor = Color.Lime;
                else
                    m_RMSelectButtonGroup[nCount].BackColor = Color.White;
            }
        }
        private void btn_RMSelectButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Tag == null)
                return;

            g_CurrentRM = (RackMaster)btn.Tag;
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RM[{((RackMaster)btn.Tag).GetParam().ID}] Select Click");
        }

        private void btn_CommanderPrev_Click(object sender, EventArgs e)
        {
            object CommandButtonPage = tableLayoutPanel_CommanderButton.Tag;

            if (CommandButtonPage == null || (int)CommandButtonPage < 0)
                CommandButtonPage = 0;

            if ((int)CommandButtonPage > 0)
            {
                for (int nCount = 0; nCount < m_RMSelectButtonGroup.Length; nCount++)
                {
                    m_RMSelectButtonGroup[nCount].Text = string.Empty;
                    m_RMSelectButtonGroup[nCount].Visible = false;
                    m_RMSelectButtonGroup[nCount].Tag = null;
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
                for (int nCount = 0; nCount < m_RMSelectButtonGroup.Length; nCount++)
                {
                    m_RMSelectButtonGroup[nCount].Text = string.Empty;
                    m_RMSelectButtonGroup[nCount].Visible = false;
                    m_RMSelectButtonGroup[nCount].Tag = null;
                }
                tableLayoutPanel_CommanderButton.Tag = (int)CommandButtonPage + 1;
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Close Click");
            this.Close();
        }

        private void btn_EMO_Click(object sender, EventArgs e)
        {
            if (g_CurrentRM == null)
                return;

            RackMaster rackMaster = (RackMaster)g_CurrentRM;

            if (!rackMaster.IsConnected())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_TCPIPDisconnection"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!rackMaster.CMD_EmergencyStop_REQ)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] SW E-Stop Click");
                rackMaster.CMD_SetEMO();
            }
            else
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] SW E-Stop Release Click");
                rackMaster.CMD_ReleaseEMO();
            }
        }

        private void btn_BuzzerStop_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, Master.CMD_Buzzer_Mute_REQ ? $"RackMaster Control Form-Buzzer Release Click" : $"RackMaster Control Form-Buzzer Stop Click");
            Master.CMD_Buzzer_Mute_REQ = !Master.CMD_Buzzer_Mute_REQ;
        }

        private void btn_AlarmClear_Click(object sender, EventArgs e)
        {
            if (g_CurrentRM == null)
                return;

            RackMaster rackMaster = (RackMaster)g_CurrentRM;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Alarm Clear Click");
            rackMaster.Interlock_SetAlarmClear(RackMaster.InterlockFrom.UI_Event);
        }

        private void btn_LogInExtend_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Login Extend Click");
            if (LogIn.IsLogIn())
            {
                LogIn.SetLogInExtend();
            }
        }

        private void btn_Minimize_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Minimize Click");
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
