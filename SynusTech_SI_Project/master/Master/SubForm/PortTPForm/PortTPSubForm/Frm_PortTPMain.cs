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
using Master.Equipment.Port;
using Master.SubForm.PortTPForm.PortTPSubForm.StatusForm;
using Master.GlobalForm;

namespace Master.SubForm.PortTPForm.PortTPSubForm
{
    public partial class Frm_PortTPMain : Form
    {
        Frm_PortStatus frm_PortStatus;
        bool bTPScreenMode = false;
        public Frm_PortTPMain(Frm_PortStatus _Frm_PortStatus)
        {
            InitializeComponent();
            ControlItemInit();

            frm_PortStatus = _Frm_PortStatus;

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

            if (pnl_PortTotalStatus.Controls.Count > 0)
                pnl_PortTotalStatus.Controls.Clear();
        }
        public void SetAutoScale(float FactorX, float FactorY)
        {
            this.AutoScaleMode = AutoScaleMode.None;
            this.Scale(new SizeF(FactorX, FactorY));

            foreach (Control item in this.Controls)
                ControlFunc.ResizeFont(item, FactorY);

            tableLayoutPanel1.RowStyles[1].Height = 70.0f;
            tableLayoutPanel_Right.RowStyles[1].Height = 70.0f;
            bTPScreenMode = true;
        }
        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                UIUpdateTimer.Enabled = false;
                return;
            }

            try
            {
                UIUpdateTimer.Interval = Master.UIUpdateIntervalTime;
                if (!pnl_PortTotalStatus.Controls.Contains(frm_PortStatus))
                {
                    pnl_PortTotalStatus.Controls.Clear();
                    pnl_PortTotalStatus.Controls.Add(frm_PortStatus);
                    pnl_PortTotalStatus.Controls[0].Dock = DockStyle.Fill;
                    pnl_PortTotalStatus.Controls[0].Show();
                }

                LaguageCheck();

                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                GroupBoxFunc.SetText(groupBox_AutoRunStatus, SynusLangPack.GetLanguage("GroupBox_AutoRunStatus"));

                frm_PortStatus.SetCurrentFocus(null);
                frm_PortStatus.ShowPIOStatus();

                if (CurrentPort != null)
                {
                    GroupBoxFunc.SetVisible(groupBox_AutoRunStatus, !CurrentPort.IsEQPort());
                    CurrentPort.Update_Btn_AutoRun(ref btn_AutoRun);
                    CurrentPort.Update_Btn_AutoStop(ref btn_AutoStop);
                    CurrentPort.Update_Btn_PowerOn(ref btn_PowerOn);
                    CurrentPort.Update_Btn_PowerOff(ref btn_PowerOff);
                    CurrentPort.Update_Btn_CIMMode(ref btn_CIMMode);
                    CurrentPort.Update_Btn_MasterMode(ref btn_MasterMode);
                    CurrentPort.Update_Btn_ModeChange(ref btn_ModeChange);
                    CurrentPort.Update_Btn_DirectionChange(ref btn_DirectionChange);

                    //CurrentPort.Update_Btn_DoorOpen(ref btn_DoorOpen);

                    CurrentPort.Update_Btn_CycleRun(ref btn_CycleRun);
                    CurrentPort.Update_Btn_CycleStop(ref btn_CycleStop);
                    CurrentPort.Update_DGV_AutoRunStatus(ref DGV_AutoRunStatus);
                    CurrentPort.Update_GBx_CycleGroup(ref groupBox_CycleSettings);

                    tableLayoutPanel_Right.Visible = !CurrentPort.IsEQPort();
                }
            }
            catch{ }
            finally
            {

            }
        }
        private void LaguageCheck()
        {

        }

        private void btn_ModeChange_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (CurrentPort.GetParam().ePortType == Port.PortType.MGV_AGV)
            {
                if (CurrentPort.GetPortOperationMode() == Port.PortOperationMode.MGV)
                    CurrentPort.Interlock_OperationModeChange(Port.PortOperationMode.AGV, Port.InterlockFrom.UI_Event);
                else if (CurrentPort.GetPortOperationMode() == Port.PortOperationMode.AGV)
                    CurrentPort.Interlock_OperationModeChange(Port.PortOperationMode.MGV, Port.InterlockFrom.UI_Event);
            }
            else if(CurrentPort.GetParam().ePortType == Port.PortType.MGV_OHT)
            {
                if (CurrentPort.GetPortOperationMode() == Port.PortOperationMode.MGV)
                    CurrentPort.Interlock_OperationModeChange(Port.PortOperationMode.OHT, Port.InterlockFrom.UI_Event);
                else if (CurrentPort.GetPortOperationMode() == Port.PortOperationMode.OHT)
                    CurrentPort.Interlock_OperationModeChange(Port.PortOperationMode.MGV, Port.InterlockFrom.UI_Event);
            }
        }

        private void btn_AutoRun_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Auto Run Click");
            CurrentPort.Interlock_StartAutoControl(Port.InterlockFrom.UI_Event);
        }

        private void btn_AutoStop_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Auto Stop Click");
            CurrentPort.Interlock_StopAutoControl(Port.InterlockFrom.UI_Event);
        }

        private void btn_CycleRun_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            try
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Cycle Run Click");
                int CycleCount = Convert.ToInt32(tbx_CycleCount.Text);
                CurrentPort.Interlock_StartAutoManualCycleControl(CycleCount, Port.InterlockFrom.UI_Event);
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] Cycle Run");
            }
        }

        private void btn_CycleStop_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Cycle Stop Click");
            CurrentPort.Interlock_StopAutoManualCycleControl(Port.InterlockFrom.UI_Event);
        }

        private void btn_CIMMode_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] CIM Mode Click");
            CurrentPort.Interlock_SetControlMode(Port.ControlMode.CIMMode, Port.InterlockFrom.UI_Event);
        }

        private void btn_MasterMode_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Master Mode Click");
            CurrentPort.Interlock_SetControlMode(Port.ControlMode.MasterMode, Port.InterlockFrom.UI_Event);
        }

        private void btn_DirectionChange_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (CurrentPort.GetOperationDirection() == Port.PortDirection.Input)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Direction Change(Output) Click");
                CurrentPort.Interlock_AutoControlDirectionChange(Port.PortDirection.Output, Port.InterlockFrom.UI_Event);
            }
            else
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Direction Change(Input) Click");
                CurrentPort.Interlock_AutoControlDirectionChange(Port.PortDirection.Input, Port.InterlockFrom.UI_Event);
            }
        }

        private void btn_PowerOn_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Power On Click");
            CurrentPort.Interlock_PortPowerOn(Port.InterlockFrom.UI_Event);
        }

        private void btn_PowerOff_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Power Off Click");
            CurrentPort.Interlock_PortPowerOff(Port.InterlockFrom.UI_Event);
        }

        private void tbx_CycleCount_MouseClick(object sender, MouseEventArgs e)
        {
            if (!bTPScreenMode)
                return;

            try
            {
                TextBox tbx = (TextBox)sender;

                if (tbx == null)
                    return;

                string OrgData = tbx.Text;

                Frm_Keypad frmKeypad = new Frm_Keypad(OrgData, false);
                frmKeypad.StartPosition = FormStartPosition.CenterParent;
                DialogResult result = frmKeypad.ShowDialog();

                if (result == DialogResult.OK)
                    tbx.Text = frmKeypad.InsertResult.ToString();

                frmKeypad.Dispose();
            }
            catch
            {

            }
        }

        //private void btn_DoorOpen_Click(object sender, EventArgs e)
        //{
        //    Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

        //    if (CurrentPort == null)
        //        return;

        //    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Door Open Click");
        //    CurrentPort.Interlock_PortDoorOpen(Port.InterlockFrom.UI_Event, !CurrentPort.CMD_OHT_Door_Open);
        //}
    }
}
