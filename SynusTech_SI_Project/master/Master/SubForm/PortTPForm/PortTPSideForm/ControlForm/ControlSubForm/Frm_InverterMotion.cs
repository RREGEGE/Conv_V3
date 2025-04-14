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

namespace Master.SubForm.PortTPForm.PortTPSubForm.ControlForm
{
    public partial class Frm_InverterMotion : Form
    {
        Port.PortAxis m_eSelectedPortAxis;
        public Frm_InverterMotion()
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
            this.Scale(new SizeF(FactorX, FactorY));

            foreach (Control item in this.Controls)
                ControlFunc.ResizeFont(item,FactorY);
        }
        public void SetControlType(Port.PortAxis type)
        {
            m_eSelectedPortAxis = type;
        }
        public Port.PortAxis GetControlType()
        {
            return m_eSelectedPortAxis;
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
                GroupBoxFunc.SetText(groupBox_InverterCommand, SynusLangPack.GetLanguage("GorupBox_InverterCommand"));
                GroupBoxFunc.SetText(groupBox_Move, SynusLangPack.GetLanguage("GorupBox_Move"));
                GroupBoxFunc.SetText(groupBox_InverterFlag, SynusLangPack.GetLanguage("GorupBox_Flag"));

                ButtonFunc.SetText(btn_InverterStop, SynusLangPack.GetLanguage("Btn_InverterStop"));
                ButtonFunc.SetText(btn_Reset, SynusLangPack.GetLanguage("Btn_InverterReset"));

                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                if (CurrentPort.GetMotionParam().IsInverterType(m_eSelectedPortAxis))
                {
                    if (CurrentPort.GetMotionParam().GetShuttleCtrl_InvParam(m_eSelectedPortAxis).InvCtrlMode == Port.InvCtrlMode.IOControl)
                    {
                        LabelFunc.SetText(lbl_HighSpeed_BWD, SynusLangPack.GetLanguage("Label_HighSpeedBWD"));
                        LabelFunc.SetText(lbl_LowSpeed_BWD, SynusLangPack.GetLanguage("Label_LowSpeedBWD"));
                        LabelFunc.SetText(lbl_LowSpeed_FWD, SynusLangPack.GetLanguage("Label_LowSpeedFWD"));
                        LabelFunc.SetText(lbl_HighSpeed_FWD, SynusLangPack.GetLanguage("Label_HighSpeedFWD"));

                        lbl_LowSpeed_BWD.Visible = true;
                        lbl_LowSpeed_FWD.Visible = true;
                        btn_LowSpeed_FWD.Visible = true;
                        btn_LowSpeed_BWD.Visible = true;
                        ButtonFunc.SetBackColor(btn_HighSpeed_FWD, CurrentPort.InverterCtrl_GetRunStatus(m_eSelectedPortAxis, Port.InvCtrlType.HighSpeedFWD) ? Color.Lime : Color.White);
                        ButtonFunc.SetBackColor(btn_LowSpeed_FWD, CurrentPort.InverterCtrl_GetRunStatus(m_eSelectedPortAxis, Port.InvCtrlType.LowSpeedFWD) ? Color.Lime : Color.White);
                        ButtonFunc.SetBackColor(btn_HighSpeed_BWD, CurrentPort.InverterCtrl_GetRunStatus(m_eSelectedPortAxis, Port.InvCtrlType.HighSpeedBWD) ? Color.Lime : Color.White);
                        ButtonFunc.SetBackColor(btn_LowSpeed_BWD, CurrentPort.InverterCtrl_GetRunStatus(m_eSelectedPortAxis, Port.InvCtrlType.LowSpeedBWD) ? Color.Lime : Color.White);
                    }
                    else
                    {
                        LabelFunc.SetText(lbl_HighSpeed_BWD, SynusLangPack.GetLanguage("Label_BWD"));
                        LabelFunc.SetText(lbl_HighSpeed_FWD, SynusLangPack.GetLanguage("Label_FWD"));

                        lbl_LowSpeed_BWD.Visible = false;
                        lbl_LowSpeed_FWD.Visible = false;
                        btn_LowSpeed_FWD.Visible = false;
                        btn_LowSpeed_BWD.Visible = false;
                        ButtonFunc.SetBackColor(btn_HighSpeed_FWD, CurrentPort.InverterCtrl_GetRunStatus(m_eSelectedPortAxis, Port.InvCtrlType.FreqFWD) ? Color.Lime : Color.White);
                        ButtonFunc.SetBackColor(btn_HighSpeed_BWD, CurrentPort.InverterCtrl_GetRunStatus(m_eSelectedPortAxis, Port.InvCtrlType.FreqBWD) ? Color.Lime : Color.White);
                    }

                    ButtonFunc.SetBackColor(btn_Reset, CurrentPort.InverterCtrl_GetResetFlag(m_eSelectedPortAxis) ? Color.Lime : Color.White);
                }
            }
            catch{ }
            finally
            {

            }
        }

        private void btn_InverterMotion_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            bool bIOControl = CurrentPort.GetMotionParam().GetShuttleCtrl_InvParam(m_eSelectedPortAxis).InvCtrlMode == Port.InvCtrlMode.IOControl;

            if (btn == btn_HighSpeed_BWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inv High Speed BWD Run Click");
                CurrentPort.Interlock_SetInverterMove(m_eSelectedPortAxis, bIOControl ? Port.InvCtrlType.HighSpeedBWD : Port.InvCtrlType.FreqBWD, true, false, Port.InterlockFrom.UI_Event);
            }
            else if (btn == btn_LowSpeed_BWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inv Low Speed BWD Run Click");
                CurrentPort.Interlock_SetInverterMove(m_eSelectedPortAxis, Port.InvCtrlType.LowSpeedBWD, true, false, Port.InterlockFrom.UI_Event);
            }
            else if (btn == btn_LowSpeed_FWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inv Low Speed FWD Run Click");
                CurrentPort.Interlock_SetInverterMove(m_eSelectedPortAxis, Port.InvCtrlType.LowSpeedFWD, true, false, Port.InterlockFrom.UI_Event);
            }
            else if (btn == btn_HighSpeed_FWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inv High Speed FWD Run Click");
                CurrentPort.Interlock_SetInverterMove(m_eSelectedPortAxis, bIOControl ? Port.InvCtrlType.HighSpeedFWD : Port.InvCtrlType.FreqFWD, true, false, Port.InterlockFrom.UI_Event);
            }
        }

        private void btn_InverterMotion_MouseUp(object sender, MouseEventArgs e)
        {
            btn_InverterStop_Click(sender, e);
        }

        private void btn_InverterStop_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = null;
            if (btn == btn_HighSpeed_BWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inv High Speed BWD Stop Click");
            }
            else if (btn == btn_LowSpeed_BWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inv Low Speed BWD Stop Click");
            }
            else if (btn == btn_LowSpeed_FWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inv Low Speed FWD Stop Click");
            }
            else if (btn == btn_HighSpeed_FWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inv High Speed FWD Stop Click");
            }
            else if (btn == btn_InverterStop)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inv Stop Click");
            }

            if (CurrentPort.GetMotionParam().IsInverterType(m_eSelectedPortAxis))
                CurrentPort.Interlock_InverterMotionStop(m_eSelectedPortAxis, false, Port.InterlockFrom.UI_Event);
        }

        private void btn_Reset_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inv Reset Push");

            if (CurrentPort.GetMotionParam().IsInverterType(m_eSelectedPortAxis))
                CurrentPort.Interlock_InverterReset(m_eSelectedPortAxis, true, Port.InterlockFrom.UI_Event);
        }

        private void btn_Reset_MouseUp(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inv Reset Release");

            if (CurrentPort.GetMotionParam().IsInverterType(m_eSelectedPortAxis))
                CurrentPort.Interlock_InverterReset(m_eSelectedPortAxis, false, Port.InterlockFrom.UI_Event);
        }

        private void btn_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if ((string)btn.Tag == "Push")
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                btn.Tag = null;

                if (btn == btn_HighSpeed_FWD || btn == btn_HighSpeed_BWD || btn == btn_LowSpeed_FWD || btn == btn_LowSpeed_FWD)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inv Stop (Mouse Leave)");

                    if (CurrentPort.GetMotionParam().IsInverterType(m_eSelectedPortAxis))
                        CurrentPort.Interlock_InverterMotionStop(m_eSelectedPortAxis, false, Port.InterlockFrom.UI_Event);
                }
            }
        }
    }
}
