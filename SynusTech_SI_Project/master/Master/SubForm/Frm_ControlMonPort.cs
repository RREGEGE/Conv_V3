using Master.Equipment.Port;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Master.SubForm
{
    public partial class Frm_ControlMonPort : Form
    {
        Port m_CurrentPort = null;
        public Frm_ControlMonPort()
        {
            InitializeComponent();
            ControlItemInit();
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
            };

            this.Disposed += (object sender, EventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;

                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }
        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);
            tableLayoutPanel_Command.Visible = false;
        }

        public void SetPort(Port port)
        {
            m_CurrentPort = port;
        }
        public Port GetPort()
        {
            return m_CurrentPort;
        }

        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UIUpdateTimer.Interval = Master.UIUpdateIntervalTime;

                LaguageCheck();
                ButtonLogOnStatusCheck();

                if (m_CurrentPort != null)
                {
                    m_CurrentPort.Update_Btn_AutoRun(ref btn_AutoRun);
                    m_CurrentPort.Update_Btn_AutoStop(ref btn_AutoStop);
                    m_CurrentPort.Update_Btn_PowerOn(ref btn_PowerOn);
                    m_CurrentPort.Update_Btn_PowerOff(ref btn_PowerOff);
                    m_CurrentPort.Update_Btn_CIMMode(ref btn_CIMMode);
                    m_CurrentPort.Update_Btn_MasterMode(ref btn_MasterMode);
                    m_CurrentPort.Update_Btn_PortAlarmClear(ref btn_ErrorReset);
                    m_CurrentPort.Update_DGV_PortStatusInfo(ref DGV_PortStatusInfo);
                    m_CurrentPort.Update_DGV_ErrorInfo(ref DGV_ErrorInfo);
                    m_CurrentPort.Update_DGV_InterlockInfo(ref DGV_InterlockInfo);
                }
                else
                {
                    //Clear?
                }
            }
            catch (Exception ex)
            { }
            finally
            {
            }
        }
        private void LaguageCheck()
        {
            GroupBoxFunc.SetText(groupBox_ErrorInfo, SynusLangPack.GetLanguage("GorupBox_ErrorInfo"));
            GroupBoxFunc.SetText(groupBox_StatusInfo, SynusLangPack.GetLanguage("GorupBox_StatusInfo"));
            GroupBoxFunc.SetText(groupBox_InterlockInfo, SynusLangPack.GetLanguage("GorupBox_Interlock"));
        }

        private void ButtonLogOnStatusCheck()
        {
            if (LogIn.IsLogIn())
            {
                if (!tableLayoutPanel_Command.Visible)
                    tableLayoutPanel_Command.Visible = true;
            }
            else
            {
                if (tableLayoutPanel_Command.Visible)
                    tableLayoutPanel_Command.Visible = false;
            }
        }

        private void btn_AutoRun_Click(object sender, EventArgs e)
        {
            if (m_CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port[{m_CurrentPort.GetParam().ID}] Auto Run Click");
            m_CurrentPort.Interlock_StartAutoControl(Port.InterlockFrom.UI_Event);
        }
        private void btn_AutoStop_Click(object sender, EventArgs e)
        {
            if (m_CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port[{m_CurrentPort.GetParam().ID}] Auto Stop Click");
            m_CurrentPort.Interlock_StopAutoControl(Port.InterlockFrom.UI_Event);
        }
        private void btn_CIMMode_Click(object sender, EventArgs e)
        {
            if (m_CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port[{m_CurrentPort.GetParam().ID}] CIM Mode Click");
            m_CurrentPort.Interlock_SetControlMode(Port.ControlMode.CIMMode, Port.InterlockFrom.UI_Event);
        }
        private void btn_MasterMode_Click(object sender, EventArgs e)
        {
            if (m_CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port[{m_CurrentPort.GetParam().ID}] Master Mode Click");
            m_CurrentPort.Interlock_SetControlMode(Port.ControlMode.MasterMode, Port.InterlockFrom.UI_Event);
        }
        private void btn_PowerOn_Click(object sender, EventArgs e)
        {
            if (m_CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port[{m_CurrentPort.GetParam().ID}] Power On Click");
            m_CurrentPort.Interlock_PortPowerOn(Port.InterlockFrom.UI_Event);
        }
        private void btn_PowerOff_Click(object sender, EventArgs e)
        {
            if (m_CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port[{m_CurrentPort.GetParam().ID}] Power Off Click");
            m_CurrentPort.Interlock_PortPowerOff(Port.InterlockFrom.UI_Event);
        }
        private void btn_ErrorReset_Click(object sender, EventArgs e)
        {
            if (m_CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-Port[{m_CurrentPort.GetParam().ID}] Alarm Clear Click");

            m_CurrentPort.Interlock_PortAmpAlarmClear(Port.InterlockFrom.UI_Event);
            m_CurrentPort.Interlock_PortAlarmClear(Port.InterlockFrom.UI_Event);
        }
    }
}
