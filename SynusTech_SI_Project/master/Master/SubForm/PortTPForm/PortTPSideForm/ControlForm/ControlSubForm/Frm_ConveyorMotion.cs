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
using System.Threading;

namespace Master.SubForm.PortTPForm.PortTPSubForm.ControlForm
{
    public partial class Frm_ConveyorMotion : Form
    {
        Port.BufferCV m_eSelectedBufferCV;
        public Frm_ConveyorMotion()
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
        public void SetControlType(Port.BufferCV type)
        {
            m_eSelectedBufferCV = type;
        }
        public Port.BufferCV GetControlType()
        {
            return m_eSelectedBufferCV;
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
                GroupBoxFunc.SetText(groupBox_CylinderCommand, SynusLangPack.GetLanguage("GorupBox_CylinderCommand"));
                GroupBoxFunc.SetText(groupBox_ConveyorCommand, SynusLangPack.GetLanguage("GorupBox_ConveyorCommand"));
                GroupBoxFunc.SetText(groupBox_Stopper, SynusLangPack.GetLanguage("GorupBox_Stopper"));
                GroupBoxFunc.SetText(groupBox_Centering, SynusLangPack.GetLanguage("GorupBox_Centering"));
                GroupBoxFunc.SetText(groupBox_Flag, SynusLangPack.GetLanguage("GorupBox_Flag"));

                GroupBoxFunc.SetText(groupBox_Move, SynusLangPack.GetLanguage("GorupBox_Move"));
                GroupBoxFunc.SetText(groupBox_SyncMove, SynusLangPack.GetLanguage("GorupBox_SyncMove"));

                

                LabelFunc.SetText(lbl_SyncBWD, SynusLangPack.GetLanguage("Label_BWD"));
                LabelFunc.SetText(lbl_SyncFWD, SynusLangPack.GetLanguage("Label_FWD"));
                LabelFunc.SetText(lbl_StopperBWD, SynusLangPack.GetLanguage("Label_BWD"));
                LabelFunc.SetText(lbl_StopperFWD, SynusLangPack.GetLanguage("Label_FWD"));
                LabelFunc.SetText(lbl_CenteringBWD, SynusLangPack.GetLanguage("Label_BWD"));
                LabelFunc.SetText(lbl_CenteringFWD, SynusLangPack.GetLanguage("Label_FWD"));

                ButtonFunc.SetText(btn_Buffer_LP, SynusLangPack.GetLanguage("Btn_Buffer") + $"LP");
                ButtonFunc.SetText(btn_Buffer_OP, SynusLangPack.GetLanguage("Btn_Buffer") + $"OP");
                ButtonFunc.SetText(btn_Buffer_BP1, SynusLangPack.GetLanguage("Btn_Buffer") + $"BP1");
                ButtonFunc.SetText(btn_Buffer_BP2, SynusLangPack.GetLanguage("Btn_Buffer") + $"BP2");
                ButtonFunc.SetText(btn_Buffer_BP3, SynusLangPack.GetLanguage("Btn_Buffer") + $"BP3");
                ButtonFunc.SetText(btn_Buffer_BP4, SynusLangPack.GetLanguage("Btn_Buffer") + $"BP4");

                ButtonFunc.SetText(btn_StopperStop, SynusLangPack.GetLanguage("Btn_StopperStop"));
                ButtonFunc.SetText(btn_CenteringStop, SynusLangPack.GetLanguage("Btn_CenteringStop"));
                ButtonFunc.SetText(btn_ConveyorStop, SynusLangPack.GetLanguage("Btn_ConveyorStop"));
                ButtonFunc.SetText(btn_ConveyorReset, SynusLangPack.GetLanguage("Btn_CVReset"));

                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort != null)
                {
                    foreach(Port.BufferCV eBufferCV in Enum.GetValues(typeof(Port.BufferCV)))
                    {
                        Button btn = null;
                        if (eBufferCV == Port.BufferCV.Buffer_LP)
                            btn = btn_Buffer_LP;
                        else if (eBufferCV == Port.BufferCV.Buffer_OP)
                            btn = btn_Buffer_OP;
                        else if (eBufferCV == Port.BufferCV.Buffer_BP1)
                            btn = btn_Buffer_BP1;
                        else if (eBufferCV == Port.BufferCV.Buffer_BP2)
                            btn = btn_Buffer_BP2;
                        else if (eBufferCV == Port.BufferCV.Buffer_BP3)
                            btn = btn_Buffer_BP3;
                        else if (eBufferCV == Port.BufferCV.Buffer_BP4)
                            btn = btn_Buffer_BP4;

                        if (btn != null)
                        {
                            ButtonFunc.SetEnable(btn, CurrentPort.GetMotionParam().IsCVUsed(eBufferCV) ? true : false);
                            ButtonFunc.SetBackColor(btn, !btn.Enabled ? Color.DarkGray : CurrentPort.BufferCtrl_CV_Is_SyncMoveSelect(eBufferCV) ? Color.Lime : Color.White);
                        }
                    }

                    bool bCenteringUse = CurrentPort.GetMotionParam().IsCenteringEnable(m_eSelectedBufferCV);
                    bool bStopperUse = CurrentPort.GetMotionParam().IsStopperEnable(m_eSelectedBufferCV);

                    GroupBoxFunc.SetVisible(groupBox_CylinderCommand, bCenteringUse || bStopperUse);
                    groupBox_CylinderCommand.Height = bCenteringUse && bStopperUse ? 225 : bCenteringUse || bStopperUse ? 125 : 0;
                    GroupBoxFunc.SetVisible(groupBox_Centering, bCenteringUse);
                    GroupBoxFunc.SetVisible(groupBox_Stopper, bStopperUse);
                    ButtonFunc.SetEnable(btn_StopperStop, bStopperUse);
                    ButtonFunc.SetEnable(btn_CenteringStop, bCenteringUse);
                    ButtonFunc.SetVisible(btn_StopperStop, bStopperUse);
                    ButtonFunc.SetVisible(btn_CenteringStop, bCenteringUse);


                    if (CurrentPort.GetMotionParam().GetBufferCtrl_CVParam(m_eSelectedBufferCV).InvCtrlMode == Port.InvCtrlMode.IOControl)
                    {
                        btn_LowSpeed_FWD.Visible = true;
                        btn_LowSpeed_BWD.Visible = true;
                        LabelFunc.SetVisible(lbl_LowSpeed_BWD, true);
                        LabelFunc.SetVisible(lbl_LowSpeed_FWD, true);

                        LabelFunc.SetText(lbl_HighSpeed_BWD, SynusLangPack.GetLanguage("Label_HighSpeedBWD"));
                        LabelFunc.SetText(lbl_LowSpeed_BWD, SynusLangPack.GetLanguage("Label_LowSpeedBWD"));
                        LabelFunc.SetText(lbl_LowSpeed_FWD, SynusLangPack.GetLanguage("Label_LowSpeedFWD"));
                        LabelFunc.SetText(lbl_HighSpeed_FWD, SynusLangPack.GetLanguage("Label_HighSpeedFWD"));

                        ButtonFunc.SetBackColor(btn_HighSpeed_FWD, CurrentPort.BufferCtrl_CV_GetRunStatus(m_eSelectedBufferCV, Port.InvCtrlType.HighSpeedFWD) ? Color.Lime : Color.White);
                        ButtonFunc.SetBackColor(btn_LowSpeed_FWD, CurrentPort.BufferCtrl_CV_GetRunStatus(m_eSelectedBufferCV, Port.InvCtrlType.LowSpeedFWD) ? Color.Lime : Color.White);
                        ButtonFunc.SetBackColor(btn_HighSpeed_BWD, CurrentPort.BufferCtrl_CV_GetRunStatus(m_eSelectedBufferCV, Port.InvCtrlType.HighSpeedBWD) ? Color.Lime : Color.White);
                        ButtonFunc.SetBackColor(btn_LowSpeed_BWD, CurrentPort.BufferCtrl_CV_GetRunStatus(m_eSelectedBufferCV, Port.InvCtrlType.LowSpeedBWD) ? Color.Lime : Color.White);
                    }
                    else
                    {
                        btn_LowSpeed_FWD.Visible = false;
                        btn_LowSpeed_BWD.Visible = false;
                        LabelFunc.SetVisible(lbl_LowSpeed_BWD, false);
                        LabelFunc.SetVisible(lbl_LowSpeed_FWD, false);
                        LabelFunc.SetText(lbl_HighSpeed_BWD, SynusLangPack.GetLanguage("Label_BWD"));
                        LabelFunc.SetText(lbl_HighSpeed_FWD, SynusLangPack.GetLanguage("Label_FWD"));

                        ButtonFunc.SetBackColor(btn_HighSpeed_FWD, CurrentPort.BufferCtrl_CV_GetRunStatus(m_eSelectedBufferCV, Port.InvCtrlType.FreqFWD) ? Color.Lime : Color.White);
                        ButtonFunc.SetBackColor(btn_HighSpeed_BWD, CurrentPort.BufferCtrl_CV_GetRunStatus(m_eSelectedBufferCV, Port.InvCtrlType.FreqBWD) ? Color.Lime : Color.White);
                    }
                    ButtonFunc.SetBackColor(btn_ConveyorReset, CurrentPort.BufferCtrl_CV_GetResetFlag(m_eSelectedBufferCV) ? Color.Lime : Color.White);
                }
                else
                {
                    foreach (Port.BufferCV eBufferCV in Enum.GetValues(typeof(Port.BufferCV)))
                    {
                        Button btn = null;
                        if (eBufferCV == Port.BufferCV.Buffer_LP)
                            btn = btn_Buffer_LP;
                        else if (eBufferCV == Port.BufferCV.Buffer_OP)
                            btn = btn_Buffer_OP;
                        else if (eBufferCV == Port.BufferCV.Buffer_BP1)
                            btn = btn_Buffer_BP1;
                        else if (eBufferCV == Port.BufferCV.Buffer_BP2)
                            btn = btn_Buffer_BP2;
                        else if (eBufferCV == Port.BufferCV.Buffer_BP3)
                            btn = btn_Buffer_BP3;
                        else if (eBufferCV == Port.BufferCV.Buffer_BP4)
                            btn = btn_Buffer_BP4;

                        if (btn != null)
                        {
                            ButtonFunc.SetEnable(btn, false);
                            ButtonFunc.SetBackColor(btn, Color.DarkGray);
                        }
                    }
                }
            }
            catch{ }
            finally
            {

            }
        }

        private void btn_ConveyorMotion_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            bool bIOControl = CurrentPort.GetMotionParam().GetBufferCtrl_CVParam(m_eSelectedBufferCV).InvCtrlMode == Port.InvCtrlMode.IOControl;

            if (btn == btn_HighSpeed_BWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor High Speed BWD Run Click");

                CurrentPort.Interlock_SetConveyorMove(m_eSelectedBufferCV, bIOControl ? Port.InvCtrlType.HighSpeedBWD : Port.InvCtrlType.FreqBWD, true, false, Port.InterlockFrom.UI_Event);
            }
            else if (btn == btn_LowSpeed_BWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor Low Speed BWD Run Click");
                CurrentPort.Interlock_SetConveyorMove(m_eSelectedBufferCV, Port.InvCtrlType.LowSpeedBWD, true, false, Port.InterlockFrom.UI_Event);
            }
            else if (btn == btn_LowSpeed_FWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor Low Speed FWD Run Click");
                CurrentPort.Interlock_SetConveyorMove(m_eSelectedBufferCV, Port.InvCtrlType.LowSpeedFWD, true, false, Port.InterlockFrom.UI_Event);
            }
            else if (btn == btn_HighSpeed_FWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor High Speed FWD Run Click");
                CurrentPort.Interlock_SetConveyorMove(m_eSelectedBufferCV, bIOControl ? Port.InvCtrlType.HighSpeedFWD : Port.InvCtrlType.FreqFWD, true, false, Port.InterlockFrom.UI_Event);
            }
        }

        private void btn_ConveyorMotion_MouseUp(object sender, MouseEventArgs e)
        {
            btn_ConveyorStop_Click(sender, e);
        }

        private void btn_SyncBuffer_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;

            Port.BufferCV eBufferCV = Port.BufferCV.Buffer_LP;

            if (btn == btn_Buffer_LP)
                eBufferCV = Port.BufferCV.Buffer_LP;
            else if (btn == btn_Buffer_OP)
                eBufferCV = Port.BufferCV.Buffer_OP;
            else if (btn == btn_Buffer_BP1)
                eBufferCV = Port.BufferCV.Buffer_BP1;
            else if (btn == btn_Buffer_BP2)
                eBufferCV = Port.BufferCV.Buffer_BP2;
            else if (btn == btn_Buffer_BP3)
                eBufferCV = Port.BufferCV.Buffer_BP3;
            else if (btn == btn_Buffer_BP4)
                eBufferCV = Port.BufferCV.Buffer_BP4;
            else
                return;

            bool bEnable = CurrentPort.BufferCtrl_CV_Is_SyncMoveSelect(eBufferCV);
            CurrentPort.BufferCtrl_CV_SetSyncEnable(eBufferCV, !bEnable);

            if(bEnable)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {eBufferCV} Sync Enable Click");
            else
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {eBufferCV} Sync Disable Click");
        }

        private void btn_SyncMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            if (btn == btn_SyncMoveBWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Conveyor Sync Motion BWD Run Click");
                CurrentPort.BufferCtrl_CV_Set_SyncMoveBackward(true);
            }
            else if (btn == btn_SyncMoveFWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Conveyor Sync Motion FWD Run Click");
                CurrentPort.BufferCtrl_CV_Set_SyncMoveForward(true);
            }
        }

        private void btn_SyncMove_MouseUp(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;
            Button btn = (Button)sender;
            btn.Tag = null;

            if (btn == btn_SyncMoveBWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Conveyor Sync Motion BWD Stop Click");
            }
            else if (btn == btn_SyncMoveFWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Conveyor Sync Motion FWD Stop Click");
            }

            CurrentPort.BufferCtrl_CV_SyncMotionStop();
        }

        private void btn_Stopper_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            if (btn == btn_Stopper_FWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Stopper FWD Run Click");
                CurrentPort.Interlock_SetStopperMove(m_eSelectedBufferCV, Port.CylCtrlList.FWD, true, false, Port.InterlockFrom.UI_Event);
            }
            else if (btn == btn_Stopper_BWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Stopper BWD Run Click");
                CurrentPort.Interlock_SetStopperMove(m_eSelectedBufferCV, Port.CylCtrlList.BWD, true, false, Port.InterlockFrom.UI_Event);
            }
        }

        private void btn_Stopper_MouseUp(object sender, MouseEventArgs e)
        {
            btn_StopperStop_Click(sender, e);
        }

        private void btn_Centering_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            if (btn == btn_Centering_FWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Centering FWD Run Click");
                CurrentPort.Interlock_SetCenteringMove(m_eSelectedBufferCV, Port.CylCtrlList.FWD, true, false, Port.InterlockFrom.UI_Event);
            }
            else if (btn == btn_Centering_BWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Centering BWD Run Click");
                CurrentPort.Interlock_SetCenteringMove(m_eSelectedBufferCV, Port.CylCtrlList.BWD, true, false, Port.InterlockFrom.UI_Event);
            }
        }

        private void btn_Centering_MouseUp(object sender, MouseEventArgs e)
        {
            btn_CenteringStop_Click(sender, e);
        }

        private void btn_StopperStop_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = null;

            if (btn == btn_Stopper_FWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Stopper FWD Stop Click");
            }
            else if (btn == btn_Stopper_BWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Stopper BWD Stop Click");
            }
            else if (btn == btn_StopperStop)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Stopper Stop Click");
            }

            CurrentPort.Interlock_StopperMotionStop(m_eSelectedBufferCV, false, Port.InterlockFrom.UI_Event);
        }

        private void btn_CenteringStop_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = null;

            if (btn == btn_Centering_FWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Centering FWD Stop Click");
            }
            else if (btn == btn_Centering_BWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Centering BWD Stop Click");
            }
            else if (btn == btn_CenteringStop)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Centering Stop Click");
            }

            CurrentPort.Interlock_CenteringMotionStop(m_eSelectedBufferCV, false, Port.InterlockFrom.UI_Event);
        }

        private void btn_ConveyorStop_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = null;

            if (btn == btn_HighSpeed_BWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor High Speed BWD Stop Click");
            }
            else if (btn == btn_LowSpeed_BWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor Low Speed BWD Stop Click");
            }
            else if (btn == btn_LowSpeed_FWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor Low Speed FWD Stop Click");
            }
            else if (btn == btn_HighSpeed_FWD)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor High Speed FWD Stop Click");
            }
            else if (btn == btn_ConveyorStop)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor Stop Click");
            }

            CurrentPort.Interlock_ConveyorMotionStop(m_eSelectedBufferCV, false, Port.InterlockFrom.UI_Event);
        }

        private void btn_ConveyorReset_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor Reset Push");

            CurrentPort.Interlock_ConveyorReset(m_eSelectedBufferCV, true, Port.InterlockFrom.UI_Event);
        }

        private void btn_ConveyorReset_MouseUp(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor Reset Release");

            CurrentPort.Interlock_ConveyorReset(m_eSelectedBufferCV, false, Port.InterlockFrom.UI_Event);
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

                if (btn == btn_Centering_FWD || btn == btn_Centering_BWD)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Centering Stop (Mouse Leave)");
                    CurrentPort.Interlock_CenteringMotionStop(m_eSelectedBufferCV, false, Port.InterlockFrom.UI_Event);
                }
                else if (btn == btn_HighSpeed_FWD || btn == btn_HighSpeed_BWD || btn == btn_LowSpeed_FWD || btn == btn_LowSpeed_FWD)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor Stop (Mouse Leave)");
                    CurrentPort.Interlock_ConveyorMotionStop(m_eSelectedBufferCV, false, Port.InterlockFrom.UI_Event);
                }
                else if (btn == btn_Stopper_FWD || btn == btn_Stopper_BWD)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Stopper Stop (Mouse Leave)");
                    CurrentPort.Interlock_StopperMotionStop(m_eSelectedBufferCV, false, Port.InterlockFrom.UI_Event);
                }
                else if (btn == btn_SyncMoveFWD || btn == btn_SyncMoveBWD)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedBufferCV} Conveyor Stop (Mouse Leave)");
                    CurrentPort.BufferCtrl_CV_SyncMotionStop();
                }
            }
        }
    }
}
