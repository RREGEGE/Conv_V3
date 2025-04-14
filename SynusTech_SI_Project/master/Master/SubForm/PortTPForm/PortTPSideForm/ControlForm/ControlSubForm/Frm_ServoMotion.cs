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
using MovenCore;
using Master.GlobalForm;

namespace Master.SubForm.PortTPForm.PortTPSubForm.ControlForm
{
    public partial class Frm_ServoMotion : Form
    {
        Port.PortAxis m_eSelectedPortAxis;
        bool bTPScreenMode = false;
        public Frm_ServoMotion()
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
        public void SetControlType(Port.PortAxis type)
        {
            m_eSelectedPortAxis = type;
        }
        public Port.PortAxis GetControlType()
        {
            return m_eSelectedPortAxis;
        }
        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);

            btn_ManualSpeed1.Tag = 10;
            btn_ManualSpeed2.Tag = 30;
            btn_ManualSpeed3.Tag = 50;
            btn_ManualSpeed4.Tag = 70;
            btn_ManualSpeed5.Tag = 100;
        }
        public void SetAutoScale(float FactorX, float FactorY)
        {
            this.AutoScaleMode = AutoScaleMode.None;
            this.Scale(new SizeF(FactorX, FactorY));

            foreach (Control item in this.Controls)
                ControlFunc.ResizeFont(item,FactorY);

            bTPScreenMode = true;
        }
        public void UpdateAxisInfo(Port.PortAxis ePortAxis)
        {
            m_eSelectedPortAxis = ePortAxis;
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
                GroupBoxFunc.SetText(groupBox_MotionCommand, SynusLangPack.GetLanguage("GroupBox_ServoCommand"));
                GroupBoxFunc.SetText(groupBox_Servo, SynusLangPack.GetLanguage("GroupBox_Servo"));
                GroupBoxFunc.SetText(groupBox_Motion, SynusLangPack.GetLanguage("GroupBox_Motion"));
                //GroupBoxFunc.SetText(groupBox_Jog, SynusLangPack.GetLanguage("GroupBox_Jog"));
                //GroupBoxFunc.SetText(groupBox_RelandAbs, SynusLangPack.GetLanguage("GroupBox_RelandAbs"));
                //GroupBoxFunc.SetText(groupBox_TeachingPointMove, SynusLangPack.GetLanguage("GorupBox_TeachingPosMove"));
                GroupBoxFunc.SetText(groupBox_ManualMoveSpeed, SynusLangPack.GetLanguage("GorupBox_ManualMoveSpeedSetting"));

                //tabPage_Jog.Text = SynusLangPack.GetLanguage("GroupBox_Jog");
                //tabPage_InchingAbsMove.Text = SynusLangPack.GetLanguage("GroupBox_RelandAbs");
                //tabPage_Teaching.Text = SynusLangPack.GetLanguage("GorupBox_TeachingPosMove");

                if(tabControl1.ItemSize != new Size((tabControl1.Width - 30) / 3, tabControl1.ItemSize.Height))
                    tabControl1.ItemSize = new Size((tabControl1.Width - 30) / 3, tabControl1.ItemSize.Height);

                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                //tabControl1.ItemSize = new Size((tabControl1.Width - 30) / 3, tabControl1.ItemSize.Height);
                if (CurrentPort != null)
                {
                    CurrentPort.Update_Btn_ServoOn(ref btn_ServoOn, m_eSelectedPortAxis);
                    CurrentPort.Update_Btn_ServoOff(ref btn_ServoOff, m_eSelectedPortAxis);
                    CurrentPort.Update_Btn_Homing(ref btn_Home, m_eSelectedPortAxis);
                    CurrentPort.Update_Btn_AmpAlarmClear(ref btn_AlarmClear, m_eSelectedPortAxis);
                    CurrentPort.Update_Btn_MotionStop(ref btn_Stop, m_eSelectedPortAxis);

                    CurrentPort.Update_Lbl_JogLowSpeed(ref label_JogLowSpeed, m_eSelectedPortAxis);
                    CurrentPort.Update_Lbl_JogHighSpeed(ref label_JogHighSpeed, m_eSelectedPortAxis);
                    CurrentPort.Update_Lbl_RelMove(ref label_RelativeInching, m_eSelectedPortAxis);
                    CurrentPort.Update_Lbl_AbsMove(ref label_AbsoluteTargetPos, m_eSelectedPortAxis);

                    CurrentPort.Update_TeachingLabel(ref lbl_TeachingPosTitle1, ref lbl_TeachingPosValue1, ref lbl_TeachingUnit1, ref btn_TeachingPos1Move,
                                        ref lbl_TeachingPosTitle2, ref lbl_TeachingPosValue2, ref lbl_TeachingUnit2, ref btn_TeachingPos2Move,
                                        ref lbl_TeachingPosTitle3, ref lbl_TeachingPosValue3, ref lbl_TeachingUnit3, ref btn_TeachingPos3Move,
                                        m_eSelectedPortAxis);

                    ButtonFunc.SetBackColor(btn_ManualSpeed1, (int)btn_ManualSpeed1.Tag == CurrentPort.GetUIManualSpeedRatio() ? Color.Lime : Color.White);
                    ButtonFunc.SetBackColor(btn_ManualSpeed2, (int)btn_ManualSpeed2.Tag == CurrentPort.GetUIManualSpeedRatio() ? Color.Lime : Color.White);
                    ButtonFunc.SetBackColor(btn_ManualSpeed3, (int)btn_ManualSpeed3.Tag == CurrentPort.GetUIManualSpeedRatio() ? Color.Lime : Color.White);
                    ButtonFunc.SetBackColor(btn_ManualSpeed4, (int)btn_ManualSpeed4.Tag == CurrentPort.GetUIManualSpeedRatio() ? Color.Lime : Color.White);
                    ButtonFunc.SetBackColor(btn_ManualSpeed5, (int)btn_ManualSpeed5.Tag == CurrentPort.GetUIManualSpeedRatio() ? Color.Lime : Color.White);

                    if(!string.IsNullOrEmpty((string)btn_Home.Tag)) // && !CurrentPort.ServoCtrl_GetHomeDone(m_eSelectedPortAxis)
                    {
                        CurrentPort.bHomeDoneAndReloadEngineParam[(int)m_eSelectedPortAxis] = false;
                    }
                    else if(!CurrentPort.bHomeDoneAndReloadEngineParam[(int)m_eSelectedPortAxis] && string.IsNullOrEmpty((string)btn_Home.Tag)) //&& CurrentPort.ServoCtrl_GetHomeDone(m_eSelectedPortAxis)
                    {
                        CurrentPort.bHomeDoneAndReloadEngineParam[(int)m_eSelectedPortAxis] = true;
                        if (CurrentPort.GetMotionParam().GetShuttleCtrl_ServoParam(m_eSelectedPortAxis).WMXParam.m_absEncoderMode)
                        {
                            int nAxis = CurrentPort.GetMotionParam().GetServoAxisNum(m_eSelectedPortAxis);

                            WMXMotion.AxisParameter EngineAxisParameter = new WMXMotion.AxisParameter();

                            CurrentPort.WMXParam_Load_EngineToProg(nAxis, CurrentPort.GetMotionParam().GetShuttleCtrl_ServoParam(m_eSelectedPortAxis).WMXParam);
                            CurrentPort.GetMotionParam().Save(CurrentPort.GetParam().ID, CurrentPort.GetMotionParam());
                        }
                    }


                    if (CurrentPort.GetMotionParam().IsValidServo(m_eSelectedPortAxis))
                    {
                        string Key = $"{CurrentPort.GetParam().ID}_{m_eSelectedPortAxis}";

                        if ((string)tbx_JogLowSpeed.Tag != Key)
                        {
                            tbx_JogLowSpeed.Tag = Key;
                            tbx_JogLowSpeed.Text = CurrentPort.GetUIJogLowSpeed(m_eSelectedPortAxis);
                            tbx_JogHighSpeed.Text = CurrentPort.GetUIJogHighSpeed(m_eSelectedPortAxis);
                            tbx_InchingMovePos.Text = CurrentPort.GetUIInchingValue(m_eSelectedPortAxis);
                            tbx_AbsMoveTargetPos.Text = CurrentPort.GetUITargetValue(m_eSelectedPortAxis);
                        }
                    }
                    else
                    {
                        tbx_JogLowSpeed.Tag = null;
                        tbx_JogLowSpeed.Text = string.Empty;
                        tbx_JogHighSpeed.Text = string.Empty;
                        tbx_InchingMovePos.Text = string.Empty;
                        tbx_AbsMoveTargetPos.Text = string.Empty;
                    }
                }
                else
                {
                    tbx_JogLowSpeed.Tag = null;
                    tbx_JogLowSpeed.Text = string.Empty;
                    tbx_JogHighSpeed.Text = string.Empty;
                    tbx_InchingMovePos.Text = string.Empty;
                    tbx_AbsMoveTargetPos.Text = string.Empty;
                }
            }
            catch{ }
            finally
            {

            }
        }

        private void btn_ServoOn_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null) //|| m_nCurrentAxis == -1
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Servo On Click");

            if (CurrentPort.GetMotionParam().IsValidServo(m_eSelectedPortAxis))
                CurrentPort.Interlock_AxisServoOn(m_eSelectedPortAxis, Port.InterlockFrom.UI_Event);
        }

        private void btn_ServoOff_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null) //|| m_nCurrentAxis == -1
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Servo Off Click");

            if (CurrentPort.GetMotionParam().IsValidServo(m_eSelectedPortAxis))
                CurrentPort.Interlock_AxisServoOff(m_eSelectedPortAxis, Port.InterlockFrom.UI_Event);
        }

        private void btn_AlarmClear_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null) //|| m_nCurrentAxis == -1
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Amp Alarm Clear Click");

            if (CurrentPort.GetMotionParam().IsValidServo(m_eSelectedPortAxis))
                CurrentPort.Interlock_AxisAmpAlarmClear(m_eSelectedPortAxis, Port.InterlockFrom.UI_Event);
        }

        private void btn_Jog_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null) //|| m_nCurrentAxis == -1
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Jog Motion Click");

            Button btn = (Button)sender;
            btn.Tag = "Push";

            if (CurrentPort.GetMotionParam().IsValidServo(m_eSelectedPortAxis))
            {
                try
                {
                    if (btn == btn_LowSpeed_JogMinus || btn == btn_LowSpeed_JogPlus)
                    {
                        float Velocity = Convert.ToSingle(tbx_JogLowSpeed.Text);
                        CurrentPort.Interlock_AxisStartJog(ref tbx_JogLowSpeed, m_eSelectedPortAxis, true, Velocity, btn == btn_LowSpeed_JogPlus ? true : false, Port.InterlockFrom.UI_Event);
                    }
                    else if (btn == btn_HighSpeed_JogMinus || btn == btn_HighSpeed_JogPlus)
                    {
                        float Velocity = Convert.ToSingle(tbx_JogHighSpeed.Text);
                        CurrentPort.Interlock_AxisStartJog(ref tbx_JogHighSpeed, m_eSelectedPortAxis, false, Velocity, btn == btn_HighSpeed_JogPlus ? true : false, Port.InterlockFrom.UI_Event);
                    }
                }
                catch (Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] Jog Control");
                }
            }
        }

        private void btn_Jog_MouseUp(object sender, MouseEventArgs e)
        {
            btn_Stop_Click(sender, e);
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = null;

            if (btn == btn_Home)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Homing Motion Stop Click");
            else if (btn == btn_HighSpeed_JogMinus || btn == btn_HighSpeed_JogPlus || btn == btn_LowSpeed_JogMinus || btn == btn_LowSpeed_JogPlus)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Jog Motion Stop Click");
            else if (btn == btn_RelMovePlus || btn == btn_RelMoveMinus)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inching Motion Stop Click");
            else if (btn == btn_TeachingPos1Move || btn == btn_TeachingPos2Move || btn == btn_TeachingPos3Move)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Teaching Motion Stop Click");
            else if (btn == btn_AbsMove)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Absolute Motion Stop Click");

            if (CurrentPort.GetMotionParam().IsValidServo(m_eSelectedPortAxis))
                CurrentPort.Interlock_AxisStop(m_eSelectedPortAxis, false, Port.InterlockFrom.UI_Event);
        }

        private void btn_RelMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Inching Motion Click");

            Button btn = (Button)sender;
            btn.Tag = "Push";

            if (CurrentPort.GetMotionParam().IsValidServo(m_eSelectedPortAxis))
            {
                try
                {
                    double InchingPos = Convert.ToDouble(tbx_InchingMovePos.Text);

                    CurrentPort.Interlock_AxisInchingMove(ref tbx_InchingMovePos, m_eSelectedPortAxis, InchingPos, btn == btn_RelMovePlus ? true : false, Port.InterlockFrom.UI_Event);
                }
                catch (Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] Inching Control");
                }
            }
        }

        private void btn_RelMove_MouseUp(object sender, MouseEventArgs e)
        {
            btn_Stop_Click(sender, e);
        }

        private void btn_AbsMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Absolute Motion Click");

            if (CurrentPort.GetMotionParam().IsValidServo(m_eSelectedPortAxis))
            {
                try
                {
                    float Velocity = CurrentPort.GetMotionManualSpeed(m_eSelectedPortAxis);
                    float Acc = CurrentPort.GetMotionManualAcc(m_eSelectedPortAxis);
                    float Dec = CurrentPort.GetMotionManualDec(m_eSelectedPortAxis);
                    double TargetPos = Convert.ToDouble(tbx_AbsMoveTargetPos.Text);
                    CurrentPort.Interlock_AxisTargetMove(m_eSelectedPortAxis, TargetPos, Port.InterlockFrom.UI_Event);
                }
                catch (Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] Abs Control");
                }
            }
        }

        private void btn_AbsMove_MouseUp(object sender, MouseEventArgs e)
        {
            btn_Stop_Click(sender, e);
        }

        private void btn_TeachingPos_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Teaching Motion Click");

            Button btn = (Button)sender;
            btn.Tag = "Push";

            if (CurrentPort.GetMotionParam().IsValidServo(m_eSelectedPortAxis))
            {
                try
                {
                    if (m_eSelectedPortAxis == Port.PortAxis.Shuttle_X || m_eSelectedPortAxis == Port.PortAxis.Buffer_LP_X || m_eSelectedPortAxis == Port.PortAxis.Buffer_OP_X)
                    {
                        if (btn == btn_TeachingPos1Move)
                            CurrentPort.X_Axis_MotionAndDone(m_eSelectedPortAxis, Port.Teaching_X_Pos.OP_Pos, false);
                        else if (btn == btn_TeachingPos2Move)
                            CurrentPort.X_Axis_MotionAndDone(m_eSelectedPortAxis, Port.Teaching_X_Pos.Wait_Pos, false);
                        else if (btn == btn_TeachingPos3Move)
                            CurrentPort.X_Axis_MotionAndDone(m_eSelectedPortAxis, CurrentPort.IsMGV() ? Port.Teaching_X_Pos.MGV_LP_Pos : Port.Teaching_X_Pos.Equip_LP_Pos, false);
                    }
                    else if (m_eSelectedPortAxis == Port.PortAxis.Shuttle_Z || m_eSelectedPortAxis == Port.PortAxis.Buffer_LP_Z || m_eSelectedPortAxis == Port.PortAxis.Buffer_OP_Z)
                    {
                        if (btn == btn_TeachingPos1Move)
                            CurrentPort.Z_Axis_MotionAndDone(m_eSelectedPortAxis, Port.Teaching_Z_Pos.Up_Pos, false);
                        else if (btn == btn_TeachingPos2Move)
                            CurrentPort.Z_Axis_MotionAndDone(m_eSelectedPortAxis, Port.Teaching_Z_Pos.Down_Pos, false);
                    }
                    else if (m_eSelectedPortAxis == Port.PortAxis.Shuttle_T || m_eSelectedPortAxis == Port.PortAxis.Buffer_LP_T || m_eSelectedPortAxis == Port.PortAxis.Buffer_OP_T)
                    {
                        if (btn == btn_TeachingPos1Move)
                            CurrentPort.T_Axis_MotionAndDone(m_eSelectedPortAxis, Port.Teaching_T_Pos.Degree0_Pos, false);
                        else if (btn == btn_TeachingPos2Move)
                            CurrentPort.T_Axis_MotionAndDone(m_eSelectedPortAxis, Port.Teaching_T_Pos.Degree180_Pos, false);
                    }
                }
                catch (Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] Teaching Control");
                }
            }
        }

        private void btn_TeachingPos_MouseUp(object sender, MouseEventArgs e)
        {
            btn_Stop_Click(sender, e);
        }

        private void btn_Home_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Homing Motion Click");

            if (CurrentPort.GetMotionParam().IsValidServo(m_eSelectedPortAxis))
            {
                CurrentPort.Interlock_AxisStartHoming(m_eSelectedPortAxis, Port.InterlockFrom.UI_Event);
            }
        }

        private void btn_Home_MouseUp(object sender, MouseEventArgs e)
        {
            btn_Stop_Click(sender, e);
        }

        private void btn_ManualSpeed_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;

            if (btn == btn_ManualSpeed1)
                CurrentPort.SetUIManualSpeedRatio(10);
            else if (btn == btn_ManualSpeed2)
                CurrentPort.SetUIManualSpeedRatio(30);
            else if (btn == btn_ManualSpeed3)
                CurrentPort.SetUIManualSpeedRatio(50);
            else if (btn == btn_ManualSpeed4)
                CurrentPort.SetUIManualSpeedRatio(70);
            else if (btn == btn_ManualSpeed5)
                CurrentPort.SetUIManualSpeedRatio(100);
            else
                CurrentPort.SetUIManualSpeedRatio(50);

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Manual Speed({CurrentPort.GetUIManualSpeedRatio()}) Click");
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

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} All Motion Stop (Mouse Leave)");

                if (CurrentPort.GetMotionParam().IsValidServo(m_eSelectedPortAxis))
                    CurrentPort.Interlock_AxisStop(m_eSelectedPortAxis, false, Port.InterlockFrom.UI_Event);
            }
        }

        private void tbx_KeyPadPopUp_MouseClick(object sender, MouseEventArgs e)
        {
            if (!bTPScreenMode)
                return;

            try
            {
                TextBox tbx = (TextBox)sender;

                if (tbx == null)
                    return;

                string OrgData = tbx.Text;

                Frm_Keypad frmKeypad = new Frm_Keypad(OrgData, true);
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
    }
}
