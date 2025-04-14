using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.Equipment.Port;

namespace Master.SubForm.PortTPForm.PortTPSubForm
{
    public partial class Frm_PortCSTInfo : Form
    {
        public Frm_PortCSTInfo()
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
        }
        public void SetAutoScale(float FactorX, float FactorY)
        {
            this.AutoScaleMode = AutoScaleMode.None;
            //this.Scale(new SizeF(FactorX, FactorY));

            foreach (Control item in this.Controls)
                ControlFunc.ResizeFont(item,FactorY);
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
                GroupBoxFunc.SetText(groupBox_CSTInfo, SynusLangPack.GetLanguage("GorupBox_CSTInfo"));
                ButtonFunc.SetText(btn_LP_CST_ID_Set, SynusLangPack.GetLanguage("Btn_Apply"));
                ButtonFunc.SetText(btn_OP_CST_ID_Set, SynusLangPack.GetLanguage("Btn_Apply"));
                ButtonFunc.SetText(btn_BP1_CST_ID_Set, SynusLangPack.GetLanguage("Btn_Apply"));
                ButtonFunc.SetText(btn_BP2_CST_ID_Set, SynusLangPack.GetLanguage("Btn_Apply"));
                ButtonFunc.SetText(btn_BP3_CST_ID_Set, SynusLangPack.GetLanguage("Btn_Apply"));
                ButtonFunc.SetText(btn_BP4_CST_ID_Set, SynusLangPack.GetLanguage("Btn_Apply"));

                ButtonFunc.SetText(btn_LP_CST_ID_Clear, SynusLangPack.GetLanguage("Btn_Delete"));
                ButtonFunc.SetText(btn_OP_CST_ID_Clear, SynusLangPack.GetLanguage("Btn_Delete"));
                ButtonFunc.SetText(btn_BP1_CST_ID_Clear, SynusLangPack.GetLanguage("Btn_Delete"));
                ButtonFunc.SetText(btn_BP2_CST_ID_Clear, SynusLangPack.GetLanguage("Btn_Delete"));
                ButtonFunc.SetText(btn_BP3_CST_ID_Clear, SynusLangPack.GetLanguage("Btn_Delete"));
                ButtonFunc.SetText(btn_BP4_CST_ID_Clear, SynusLangPack.GetLanguage("Btn_Delete"));

                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort != null)
                {
                    if (CurrentPort.IsShuttleControlPort())
                    {
                        bool bLPCSTState = CurrentPort.Carrier_CheckLP_ExistProduct(true);
                        bool bOPCSTState = CurrentPort.Carrier_CheckOP_ExistProduct(true);
                        bool bShuttleCSTState = CurrentPort.Carrier_CheckShuttle_ExistProduct(true);

                        if(CurrentPort.GetMotionParam().eBufferType == Port.ShuttleCtrlBufferType.One_Buffer)
                        {
                            lbl_LP_CST_ID.Text = string.Empty;
                            lbl_LP_CST_State.Text = "Off";
                        }
                        else
                        {
                            lbl_LP_CST_ID.Text = CurrentPort.LP_CarrierID;
                            lbl_LP_CST_State.Text = bLPCSTState ? "On" : "Off";
                        }

                        lbl_OP_CST_ID.Text      = CurrentPort.OP_CarrierID;
                        lbl_OP_CST_State.Text   = bOPCSTState ? "On" : "Off";

                        lbl_BP1_CST_ID.Text     = CurrentPort.Carrier_GetBP_CarrierID(0);
                        lbl_BP1_CST_State.Text  = bShuttleCSTState ? "On" : "Off";
                        lbl_BP2_CST_State.Text  = "Off";
                        lbl_BP3_CST_State.Text  = "Off";
                        lbl_BP4_CST_State.Text  = "Off";
                    }
                    else if(CurrentPort.IsBufferControlPort())
                    {
                        bool bLPCSTState = CurrentPort.Carrier_CheckLP_ExistProduct(true);
                        bool bOPCSTState = false;

                        if (CurrentPort.GetParam().ePortType == Port.PortType.Conveyor_AGV)
                        {
                            bOPCSTState = CurrentPort.Carrier_CheckOP_ExistProduct(true);
                        }
                        else if (CurrentPort.GetParam().ePortType == Port.PortType.Conveyor_OMRON)
                        {
                            if (CurrentPort.IsZAxisPos_UP(Port.PortAxis.Buffer_OP_Z))
                            {
                                bOPCSTState = CurrentPort.Sensor_OP_CST_Detect1 && CurrentPort.Sensor_OP_CST_Detect2;
                            }
                            else if (CurrentPort.IsZAxisPos_DOWN(Port.PortAxis.Buffer_OP_Z))
                            {
                                bOPCSTState = CurrentPort.Carrier_CheckOP_ExistProduct(true);
                            }
                        }

                        lbl_LP_CST_ID.Text = CurrentPort.LP_CarrierID;
                        lbl_LP_CST_State.Text = bLPCSTState ? "On" : "Off";

                        lbl_OP_CST_ID.Text = CurrentPort.OP_CarrierID;
                        lbl_OP_CST_State.Text = bOPCSTState ? "On" : "Off";

                        lbl_BP1_CST_ID.Text = CurrentPort.Carrier_GetBP_CarrierID(0);
                        lbl_BP2_CST_ID.Text = CurrentPort.Carrier_GetBP_CarrierID(1);
                        lbl_BP3_CST_ID.Text = CurrentPort.Carrier_GetBP_CarrierID(2);
                        lbl_BP4_CST_ID.Text = CurrentPort.Carrier_GetBP_CarrierID(3);

                        if (CurrentPort.GetMotionParam().IsCVUsed(Port.BufferCV.Buffer_BP1) &&
                            CurrentPort.GetMotionParam().IsCSTDetectSensorEnable(Port.BufferCV.Buffer_BP1) &&
                            CurrentPort.GetMotionParam().GetBufferCVParam(Port.BufferCV.Buffer_BP1).GetCSTDetectStatus())
                            lbl_BP1_CST_State.Text = "On";
                        else
                            lbl_BP1_CST_State.Text = "Off";

                        if (CurrentPort.GetMotionParam().IsCVUsed(Port.BufferCV.Buffer_BP2) &&
                            CurrentPort.GetMotionParam().IsCSTDetectSensorEnable(Port.BufferCV.Buffer_BP2) &&
                            CurrentPort.GetMotionParam().GetBufferCVParam(Port.BufferCV.Buffer_BP2).GetCSTDetectStatus())
                            lbl_BP2_CST_State.Text = "On";
                        else
                            lbl_BP2_CST_State.Text = "Off";

                        if (CurrentPort.GetMotionParam().IsCVUsed(Port.BufferCV.Buffer_BP3) &&
                            CurrentPort.GetMotionParam().IsCSTDetectSensorEnable(Port.BufferCV.Buffer_BP3) &&
                            CurrentPort.GetMotionParam().GetBufferCVParam(Port.BufferCV.Buffer_BP3).GetCSTDetectStatus())
                            lbl_BP3_CST_State.Text = "On";
                        else
                            lbl_BP3_CST_State.Text = "Off";

                        if (CurrentPort.GetMotionParam().IsCVUsed(Port.BufferCV.Buffer_BP4) &&
                            CurrentPort.GetMotionParam().IsCSTDetectSensorEnable(Port.BufferCV.Buffer_BP4) &&
                            CurrentPort.GetMotionParam().GetBufferCVParam(Port.BufferCV.Buffer_BP4).GetCSTDetectStatus())
                            lbl_BP4_CST_State.Text = "On";
                        else
                            lbl_BP4_CST_State.Text = "Off";
                    }
                    else
                    {
                        lbl_BP1_CST_State.Text = "Off";
                        lbl_BP2_CST_State.Text = "Off";
                        lbl_BP3_CST_State.Text = "Off";
                        lbl_BP4_CST_State.Text = "Off";
                    }

                    lbl_OP_CST_State.BackColor = lbl_OP_CST_State.Text == "On" ? Color.Lime : Color.Red;
                    lbl_LP_CST_State.BackColor = lbl_LP_CST_State.Text == "On" ? Color.Lime : Color.Red;
                    lbl_BP1_CST_State.BackColor = lbl_BP1_CST_State.Text == "On" ? Color.Lime : Color.Red;
                    lbl_BP2_CST_State.BackColor = lbl_BP2_CST_State.Text == "On" ? Color.Lime : Color.Red;
                    lbl_BP3_CST_State.BackColor = lbl_BP3_CST_State.Text == "On" ? Color.Lime : Color.Red;
                    lbl_BP4_CST_State.BackColor = lbl_BP4_CST_State.Text == "On" ? Color.Lime : Color.Red;

                    bool bEnable = !(CurrentPort.IsAutoControlRun() || CurrentPort.IsAutoManualCycleRun());
                    bool bLPEnable = (CurrentPort.GetMotionParam().eBufferType == Port.ShuttleCtrlBufferType.Two_Buffer && CurrentPort.IsShuttleControlPort()) || CurrentPort.IsBufferControlPort();

                    bool BP1Enable = (CurrentPort.GetMotionParam().IsCVUsed(Port.BufferCV.Buffer_BP1) && CurrentPort.GetMotionParam().IsCSTDetectSensorEnable(Port.BufferCV.Buffer_BP1) && CurrentPort.IsBufferControlPort()) || CurrentPort.IsShuttleControlPort();
                    bool BP2Enable = (CurrentPort.GetMotionParam().IsCVUsed(Port.BufferCV.Buffer_BP2) && CurrentPort.GetMotionParam().IsCSTDetectSensorEnable(Port.BufferCV.Buffer_BP2) && CurrentPort.IsBufferControlPort());
                    bool BP3Enable = (CurrentPort.GetMotionParam().IsCVUsed(Port.BufferCV.Buffer_BP3) && CurrentPort.GetMotionParam().IsCSTDetectSensorEnable(Port.BufferCV.Buffer_BP3) && CurrentPort.IsBufferControlPort());
                    bool BP4Enable = (CurrentPort.GetMotionParam().IsCVUsed(Port.BufferCV.Buffer_BP4) && CurrentPort.GetMotionParam().IsCSTDetectSensorEnable(Port.BufferCV.Buffer_BP4) && CurrentPort.IsBufferControlPort());
                    btn_LP_CST_ID_Set.Enabled = bEnable && bLPEnable;
                    btn_OP_CST_ID_Set.Enabled = bEnable;
                    btn_BP1_CST_ID_Set.Enabled = bEnable && BP1Enable;
                    btn_BP2_CST_ID_Set.Enabled = bEnable && BP2Enable;
                    btn_BP3_CST_ID_Set.Enabled = bEnable && BP3Enable;
                    btn_BP4_CST_ID_Set.Enabled = bEnable && BP4Enable;

                    btn_LP_CST_ID_Clear.Enabled = bEnable && bLPEnable;
                    btn_OP_CST_ID_Clear.Enabled = bEnable;
                    btn_BP1_CST_ID_Clear.Enabled = bEnable && BP1Enable;
                    btn_BP2_CST_ID_Clear.Enabled = bEnable && BP2Enable;
                    btn_BP3_CST_ID_Clear.Enabled = bEnable && BP3Enable;
                    btn_BP4_CST_ID_Clear.Enabled = bEnable && BP4Enable;

                    btn_LP_CST_ID_Set.BackColor = btn_LP_CST_ID_Set.Enabled ? Color.White : Color.DarkGray;
                    btn_OP_CST_ID_Set.BackColor = btn_OP_CST_ID_Set.Enabled ? Color.White : Color.DarkGray;
                    btn_BP1_CST_ID_Set.BackColor = btn_BP1_CST_ID_Set.Enabled ? Color.White : Color.DarkGray;
                    btn_BP2_CST_ID_Set.BackColor = btn_BP2_CST_ID_Set.Enabled ? Color.White : Color.DarkGray;
                    btn_BP3_CST_ID_Set.BackColor = btn_BP3_CST_ID_Set.Enabled ? Color.White : Color.DarkGray;
                    btn_BP4_CST_ID_Set.BackColor = btn_BP4_CST_ID_Set.Enabled ? Color.White : Color.DarkGray;

                    btn_LP_CST_ID_Clear.BackColor = btn_LP_CST_ID_Clear.Enabled ? Color.White : Color.DarkGray;
                    btn_OP_CST_ID_Clear.BackColor = btn_OP_CST_ID_Clear.Enabled ? Color.White : Color.DarkGray;
                    btn_BP1_CST_ID_Clear.BackColor = btn_BP1_CST_ID_Clear.Enabled ? Color.White : Color.DarkGray;
                    btn_BP2_CST_ID_Clear.BackColor = btn_BP2_CST_ID_Clear.Enabled ? Color.White : Color.DarkGray;
                    btn_BP3_CST_ID_Clear.BackColor = btn_BP3_CST_ID_Clear.Enabled ? Color.White : Color.DarkGray;
                    btn_BP4_CST_ID_Clear.BackColor = btn_BP4_CST_ID_Clear.Enabled ? Color.White : Color.DarkGray;
                }
                else
                {
                    lbl_LP_CST_ID.Text = string.Empty;
                    lbl_OP_CST_ID.Text = string.Empty;
                    lbl_BP1_CST_ID.Text = string.Empty;
                    lbl_BP2_CST_ID.Text = string.Empty;
                    lbl_BP3_CST_ID.Text = string.Empty;
                    lbl_BP4_CST_ID.Text = string.Empty;
                }
            }
            catch { }
            finally
            {

            }
        }

        private void btn_CST_ID_Set_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port CST Info Form-Port[{CurrentPort.GetParam().ID}] CST Set Click");

            if (CurrentPort.IsAutoControlRun())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InAutoControl"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] is Auto Running State");
                return;
            }

            if(CurrentPort.IsAutoManualCycleRun())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InCycleControl"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] is Cycle Running State");
                return;
            }

            Button btn = (Button)sender;

            if (btn == btn_LP_CST_ID_Set)
            {
                if (lbl_LP_CST_State.Text != "On")
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Not_Exist_CST"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMsg.AddPortLog(CurrentPort.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PortCSTInfo, $"LP ID Set Fail. LP CST Not Exist");
                    return;
                }
                CurrentPort.LP_CarrierID = tbx_LP_CST_ID.Text;
            }
            else if (btn == btn_OP_CST_ID_Set)
            {
                if (lbl_OP_CST_State.Text != "On")
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Not_Exist_CST"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMsg.AddPortLog(CurrentPort.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PortCSTInfo, $"OP ID Set Fail. OP CST Not Exist");
                    return;
                }

                CurrentPort.OP_CarrierID = tbx_OP_CST_ID.Text;
            }
            else if (btn == btn_BP1_CST_ID_Set)
            {
                if (lbl_BP1_CST_State.Text != "On")
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Not_Exist_CST"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMsg.AddPortLog(CurrentPort.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PortCSTInfo, $"Shuttle(BP1) ID Set Fail. Shuttle(BP1) CST Not Exist");
                    return;
                }

                CurrentPort.Carrier_SetBP_CarrierID(0, tbx_BP1_CST_ID.Text);
            }
            else if (btn == btn_BP2_CST_ID_Set)
            {
                if (lbl_BP1_CST_State.Text != "On")
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Not_Exist_CST"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMsg.AddPortLog(CurrentPort.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PortCSTInfo, $"BP2 ID Set Fail. BP2 CST Not Exist");
                    return;
                }

                CurrentPort.Carrier_SetBP_CarrierID(1, tbx_BP2_CST_ID.Text);
            }
            else if (btn == btn_BP3_CST_ID_Set)
            {
                if (lbl_BP1_CST_State.Text != "On")
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Not_Exist_CST"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMsg.AddPortLog(CurrentPort.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PortCSTInfo, $"BP3 ID Set Fail. BP3 CST Not Exist");
                    return;
                }

                CurrentPort.Carrier_SetBP_CarrierID(2, tbx_BP3_CST_ID.Text);
            }
            else if (btn == btn_BP4_CST_ID_Set)
            {
                if (lbl_BP1_CST_State.Text != "On")
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Not_Exist_CST"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogMsg.AddPortLog(CurrentPort.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PortCSTInfo, $"BP4 ID Set Fail. BP4 CST Not Exist");
                    return;
                }

                CurrentPort.Carrier_SetBP_CarrierID(3, tbx_BP4_CST_ID.Text);
            }
        }

        private void btn_CST_ID_Clear_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port CST Info Form-Port[{CurrentPort.GetParam().ID}] CST Clear Click");

            if (CurrentPort.IsAutoControlRun())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InAutoControl"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] is Auto Running State");
                return;
            }

            if (CurrentPort.IsAutoManualCycleRun())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InCycleControl"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] is Cycle Running State");
                return;
            }

            Button btn = (Button)sender;

            if (btn == btn_LP_CST_ID_Clear)
                CurrentPort.LP_CarrierID = string.Empty;
            if (btn == btn_OP_CST_ID_Clear)
                CurrentPort.OP_CarrierID = string.Empty;
            if (btn == btn_BP1_CST_ID_Clear)
                CurrentPort.Carrier_SetBP_CarrierID(0, string.Empty);
            if (btn == btn_BP2_CST_ID_Clear)
                CurrentPort.Carrier_SetBP_CarrierID(1, string.Empty);
            if (btn == btn_BP3_CST_ID_Clear)
                CurrentPort.Carrier_SetBP_CarrierID(2, string.Empty);
            if (btn == btn_BP4_CST_ID_Clear)
                CurrentPort.Carrier_SetBP_CarrierID(3, string.Empty);
        }
    }
}
