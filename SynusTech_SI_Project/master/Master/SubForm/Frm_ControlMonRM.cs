using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.Equipment.RackMaster;

namespace Master.SubForm
{
    public partial class Frm_ControlMonRM : Form
    {
        RackMaster m_CurrentRackMaster = null;
        public Frm_ControlMonRM()
        {
            InitializeComponent();
            ControlItemInit();
            ButtonLogOnStatusCheck();

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
        }
        public void SetRackMaster(RackMaster rackMaster)
        {
            m_CurrentRackMaster = rackMaster;
        }
        public RackMaster GetRackMaster()
        {
            return m_CurrentRackMaster;
        }

        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UIUpdateTimer.Interval = Master.UIUpdateIntervalTime;

                LaguageCheck();
                ButtonLogOnStatusCheck();

                if(m_CurrentRackMaster != null)
                {
                    //return;
                    m_CurrentRackMaster.Update_Btn_AutoRun(ref btn_AutoRun);
                    m_CurrentRackMaster.Update_Btn_AutoStop(ref btn_AutoStop);
                    m_CurrentRackMaster.Update_Btn_PowerOn(ref btn_PowerOn);
                    m_CurrentRackMaster.Update_Btn_PowerOff(ref btn_PowerOff);
                    m_CurrentRackMaster.Update_Btn_CIMMode(ref btn_CIMMode);
                    m_CurrentRackMaster.Update_Btn_MasterMode(ref btn_MasterMode);
                    m_CurrentRackMaster.Update_DGV_ErrorInfo(ref DGV_ErrorInfo);
                    m_CurrentRackMaster.Update_DGV_ErrorDetailInfo(ref DGV_ErrorDetailInfo, DGV_ErrorInfo.CurrentRow.Index);
                    m_CurrentRackMaster.Update_DGV_RMStatusInfo(ref DGV_RMStatusInfo);
                    m_CurrentRackMaster.Update_DGV_InterlockInfo(ref DGV_InterlockInfo);
                    m_CurrentRackMaster.Update_Btn_RMAlarmClear(ref btn_ErrorReset);
                }
                else
                {
                    //Clear?
                }
            }
            catch { }
            finally
            {
            }
        }
        private void LaguageCheck()
        {
            GroupBoxFunc.SetText(groupBox_ErrorInfo, SynusLangPack.GetLanguage("GorupBox_ErrorInfo"));
            GroupBoxFunc.SetText(groupBox_ErrorDetail, SynusLangPack.GetLanguage("GorupBox_ErrorDetailInfo"));
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
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-RackMaster[{m_CurrentRackMaster.GetParam().ID}] Auto Run Click");
            m_CurrentRackMaster.Interlock_AutoModeEnable(RackMaster.InterlockFrom.UI_Event);
        }

        private void btn_AutoStop_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-RackMaster[{m_CurrentRackMaster.GetParam().ID}] Auto Stop Click");
            m_CurrentRackMaster.Interlock_AutoModeDisable(RackMaster.InterlockFrom.UI_Event);
        }

        private void btn_CIMMode_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-RackMaster[{m_CurrentRackMaster.GetParam().ID}] CIM Mode Click");
            m_CurrentRackMaster.Interlock_SetControlMode(RackMaster.ControlMode.CIMMode, RackMaster.InterlockFrom.UI_Event);
        }

        private void btn_MasterMode_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-RackMaster[{m_CurrentRackMaster.GetParam().ID}] Master Mode Click");
            m_CurrentRackMaster.Interlock_SetControlMode(RackMaster.ControlMode.MasterMode, RackMaster.InterlockFrom.UI_Event);
        }

        private void btn_PowerOn_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-RackMaster[{m_CurrentRackMaster.GetParam().ID}] Power On Click");
            m_CurrentRackMaster.Interlock_SetPowerOn(RackMaster.InterlockFrom.UI_Event);
        }

        private void btn_PowerOff_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-RackMaster[{m_CurrentRackMaster.GetParam().ID}] Power Off Click");
            m_CurrentRackMaster.Interlock_SetPowerOff(RackMaster.InterlockFrom.UI_Event);
        }

        private void btn_ErrorReset_Click(object sender, EventArgs e)
        {
            if (m_CurrentRackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Main Form-RackMaster[{m_CurrentRackMaster.GetParam().ID}] Alarm Clear Click");
            m_CurrentRackMaster.Interlock_SetAlarmClear(RackMaster.InterlockFrom.UI_Event);
        }
    }
}
