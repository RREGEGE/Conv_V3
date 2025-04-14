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
using Master.SubForm.MasterForm.MasterSubForm;
using Master.GlobalForm;

namespace Master.SubForm.MasterForm
{
    public partial class Frm_MasterCtrlScreen : Form
    {
        enum MenuButtonIndex : int
        {
            Main,
            Settings,
            MappingInfo,
            CPSInfo,
            AlarmInfo
        }

        Frm_IOTable frm_IOTable;

        Frm_MasterCtrlMain frm_MasterCtrlMain = new Frm_MasterCtrlMain() { TopLevel = false };
        Frm_MasterSettings frm_MasterSettings = new Frm_MasterSettings() { TopLevel = false };
        Frm_MasterCtrlMappingInfo frm_MasterCtrlMappingInfo = new Frm_MasterCtrlMappingInfo() { TopLevel = false };
        Frm_MasterCtrlCPS frm_MasterCtrlCPSInfo = new Frm_MasterCtrlCPS() { TopLevel = false };
        Frm_MasterCtrlAlarmInfo frm_MasterCtrlAlarmInfo = new Frm_MasterCtrlAlarmInfo() { TopLevel = false };

        private MenuButtonIndex eCurrentWindowIndex = (MenuButtonIndex)(-1);
        Button[] m_MenuButtonGroup;
        Form[] m_MenuFormGroup;
        DataGridView LogDGV;

        public Frm_MasterCtrlScreen()
        {
            InitializeComponent();
            LogDGVInit();
            NavigationMapping();
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

                frm_MasterCtrlMain.Visible = false;
                frm_MasterSettings.Visible = false;
                frm_MasterCtrlMappingInfo.Visible = false;
                frm_MasterCtrlCPSInfo.Visible = false;
                frm_MasterCtrlAlarmInfo.Visible = false;

                frm_MasterCtrlMain.Close();
                frm_MasterSettings.Close();
                frm_MasterCtrlMappingInfo.Close();
                frm_MasterCtrlCPSInfo.Close();
                frm_MasterCtrlAlarmInfo.Close();

                if (frm_IOTable != null)
                {
                    frm_IOTable.Close();
                }
            };

            this.Disposed += (object sender, EventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;

                Log.logInsertEvent -= UpdateLogDGV;

                frm_MasterCtrlMain.Dispose();
                frm_MasterSettings.Dispose();
                frm_MasterCtrlMappingInfo.Dispose();
                frm_MasterCtrlCPSInfo.Dispose();
                frm_MasterCtrlAlarmInfo.Dispose();

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

            ContextMenuInit();
        }

        private void ContextMenuInit()
        {
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
            if (frm_IOTable == null)
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
        private void NavigationMapping()
        {
            m_MenuButtonGroup = new Button[]
            {
                btn_Main,
                btn_Settings,
                btn_MappingInfo,
                btn_CPSInfo,
                btn_AlarmInfo
            };

            m_MenuFormGroup = new Form[]
            {
                frm_MasterCtrlMain,
                frm_MasterSettings,
                frm_MasterCtrlMappingInfo,
                frm_MasterCtrlCPSInfo,
                frm_MasterCtrlAlarmInfo
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
            }
        }


        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                //if (this.WindowState != FormWindowState.Maximized)
                //    this.WindowState = FormWindowState.Maximized;

                UIUpdateTimer.Interval = Master.UIUpdateIntervalTime;

                btn_CPSInfo.Visible = Master.m_CPS?.m_CPSEnable ?? false;

                NowTimeUpdate();
                LogOnTimeCheck();
                LaguageCheck();

                Master.Update_Btn_NormalMasterAlarmClear(ref btn_AlarmClear);

                LabelFunc.SetText(lbl_CPUUsageTitle, SynusLangPack.GetLanguage("Label_CPUUsage") + " :");
                LabelFunc.SetText(lbl_RAMUsageTitle, SynusLangPack.GetLanguage("Label_RAMUsage") + " :");
                LabelFunc.SetText(lbl_CPUUsage, Frm_Main.CPUUsageStr);
                LabelFunc.SetText(lbl_RAMUsage, Frm_Main.RAMUsageStr);

                Master.Update_Btn_BuzzerStop(ref btn_BuzzerStop);
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
            ButtonFunc.SetText(btn_Settings, SynusLangPack.GetLanguage("Btn_Settings"));
            ButtonFunc.SetText(btn_MappingInfo, SynusLangPack.GetLanguage("Btn_MappingInfo"));
            ButtonFunc.SetText(btn_CPSInfo, SynusLangPack.GetLanguage("Btn_CPSInfo"));
            ButtonFunc.SetText(btn_AlarmInfo, SynusLangPack.GetLanguage("Btn_AlarmInfo"));
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

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_BuzzerStop_Click(object sender, EventArgs e)
        {
            Master.CMD_Buzzer_Mute_REQ = !Master.CMD_Buzzer_Mute_REQ;
        }

        private void btn_LogInExtend_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Master Control Form-Login Extend Click");
            if (LogIn.IsLogIn())
            {
                LogIn.SetLogInExtend();
            }
        }

        private void btn_AlarmClear_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Master Control Form-Master Alarm Reset Click");

            Master.Do_MasterRecovery();
            Master.AlarmAllClear();
        }

        private void btn_Minimize_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Master Control Form-Minimize Click");
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
