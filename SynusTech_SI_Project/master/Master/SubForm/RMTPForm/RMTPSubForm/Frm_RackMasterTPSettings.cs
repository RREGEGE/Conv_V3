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
using System.Diagnostics;
using System.Threading;

namespace Master.SubForm.RMTPForm.RMTPSubForm
{
    public partial class Frm_RackMasterTPSettings : Form
    {
        Stopwatch m_PushSt = new Stopwatch();
        public Frm_RackMasterTPSettings()
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
                GroupBoxFunc.SetText(groupBox_RackMasterSpeedSetting, SynusLangPack.GetLanguage("GorupBox_RackMasterSpeedSetting"));
                GroupBoxFunc.SetText(groupBox_RackMasterSubCommand, SynusLangPack.GetLanguage("GorupBox_RackMasterSubCommand"));
                GroupBoxFunc.SetText(groupBox_RackMasterOverLoadSetting, SynusLangPack.GetLanguage("GorupBox_RackMaster_OverloadSetting"));
                GroupBoxFunc.SetText(groupBox_RackMasterOverLoadClear, SynusLangPack.GetLanguage("GorupBox_RackMaster_OverloadClear"));

                ButtonFunc.SetText(btn_X_Axis_SpeedSettings_Send, SynusLangPack.GetLanguage("Btn_Send"));
                ButtonFunc.SetBackColor(btn_X_Axis_SpeedSettings_Send, btn_X_Axis_SpeedSettings_Send.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_Z_Axis_SpeedSettings_Send, SynusLangPack.GetLanguage("Btn_Send"));
                ButtonFunc.SetBackColor(btn_Z_Axis_SpeedSettings_Send, btn_Z_Axis_SpeedSettings_Send.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_A_Axis_SpeedSettings_Send, SynusLangPack.GetLanguage("Btn_Send"));
                ButtonFunc.SetBackColor(btn_A_Axis_SpeedSettings_Send, btn_A_Axis_SpeedSettings_Send.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_T_Axis_SpeedSettings_Send, SynusLangPack.GetLanguage("Btn_Send"));
                ButtonFunc.SetBackColor(btn_T_Axis_SpeedSettings_Send, btn_T_Axis_SpeedSettings_Send.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_All_Axis_SpeedSettings_Send, SynusLangPack.GetLanguage("Btn_AllSend"));
                ButtonFunc.SetBackColor(btn_All_Axis_SpeedSettings_Send, btn_All_Axis_SpeedSettings_Send.Tag != null ? Color.Lime : Color.White);

                ButtonFunc.SetText(btn_X_Axis_OverLoadSettings_Send, SynusLangPack.GetLanguage("Btn_Send"));
                ButtonFunc.SetBackColor(btn_X_Axis_OverLoadSettings_Send, btn_X_Axis_OverLoadSettings_Send.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_Z_Axis_OverLoadSettings_Send, SynusLangPack.GetLanguage("Btn_Send"));
                ButtonFunc.SetBackColor(btn_Z_Axis_OverLoadSettings_Send, btn_Z_Axis_OverLoadSettings_Send.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_A_Axis_OverLoadSettings_Send, SynusLangPack.GetLanguage("Btn_Send"));
                ButtonFunc.SetBackColor(btn_A_Axis_OverLoadSettings_Send, btn_A_Axis_OverLoadSettings_Send.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_T_Axis_OverLoadSettings_Send, SynusLangPack.GetLanguage("Btn_Send"));
                ButtonFunc.SetBackColor(btn_T_Axis_OverLoadSettings_Send, btn_T_Axis_OverLoadSettings_Send.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_All_Axis_OverLoadSettings_Send, SynusLangPack.GetLanguage("Btn_AllSend"));
                ButtonFunc.SetBackColor(btn_All_Axis_OverLoadSettings_Send, btn_All_Axis_OverLoadSettings_Send.Tag != null ? Color.Lime : Color.White);

                ButtonFunc.SetText(btn_X_Axis_Detected_OverLoad_Clear, SynusLangPack.GetLanguage("Btn_Clear"));
                ButtonFunc.SetBackColor(btn_X_Axis_Detected_OverLoad_Clear, btn_X_Axis_Detected_OverLoad_Clear.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_Z_Axis_Detected_OverLoad_Clear, SynusLangPack.GetLanguage("Btn_Clear"));
                ButtonFunc.SetBackColor(btn_Z_Axis_Detected_OverLoad_Clear, btn_Z_Axis_Detected_OverLoad_Clear.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_A_Axis_Detected_OverLoad_Clear, SynusLangPack.GetLanguage("Btn_Clear"));
                ButtonFunc.SetBackColor(btn_A_Axis_Detected_OverLoad_Clear, btn_A_Axis_Detected_OverLoad_Clear.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_T_Axis_Detected_OverLoad_Clear, SynusLangPack.GetLanguage("Btn_Clear"));
                ButtonFunc.SetBackColor(btn_T_Axis_Detected_OverLoad_Clear, btn_T_Axis_Detected_OverLoad_Clear.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_All_Axis_Detected_OverLoad_Clear, SynusLangPack.GetLanguage("Btn_AllClear"));
                ButtonFunc.SetBackColor(btn_All_Axis_Detected_OverLoad_Clear, btn_All_Axis_Detected_OverLoad_Clear.Tag != null ? Color.Lime : Color.White);



                ButtonFunc.SetText(btn_TimeSync, SynusLangPack.GetLanguage("Btn_TimeSync"));
                ButtonFunc.SetBackColor(btn_TimeSync, btn_TimeSync.Tag != null ? Color.Lime : Color.White);

                ButtonFunc.SetText(btn_MaintMove, SynusLangPack.GetLanguage("Btn_MaintMove"));
                ButtonFunc.SetBackColor(btn_MaintMove, btn_MaintMove.Tag != null ? Color.Lime : Color.White);

                UpdateSpeedSettingValue();
                UpdateSetMaxLoadValue();
                UpdateDetectedMaxLoadValue();
                UpdateMaintMoveStatus();
            }
            catch { }
            finally
            {

            }
        }

        private void UpdateSpeedSettingValue()
        {
            LabelFunc.SetText(lbl_XAxisSpeedTitle, SynusLangPack.GetLanguage("Label_XAxisSpeed"));
            LabelFunc.SetText(lbl_ZAxisSpeedTitle, SynusLangPack.GetLanguage("Label_ZAxisSpeed"));
            LabelFunc.SetText(lbl_AAxisSpeedTitle, SynusLangPack.GetLanguage("Label_AAxisSpeed"));
            LabelFunc.SetText(lbl_TAxisSpeedTitle, SynusLangPack.GetLanguage("Label_TAxisSpeed"));
            LabelFunc.SetText(lbl_AppliedValue, SynusLangPack.GetLanguage("Label_AppliedValue"));
            LabelFunc.SetText(lbl_SetValue, SynusLangPack.GetLanguage("Label_SetValue"));

            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LabelFunc.SetText(lbl_X_Axis_SpeedSetting, $"{rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.X_Axis_SetSpeedPercent)} %");
            LabelFunc.SetText(lbl_Z_Axis_SpeedSetting, $"{rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.Z_Axis_SetSpeedPercent)} %");
            LabelFunc.SetText(lbl_A_Axis_SpeedSetting, $"{rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.A_Axis_SetSpeedPercent)} %");
            LabelFunc.SetText(lbl_T_Axis_SpeedSetting, $"{rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.T_Axis_SetSpeedPercent)} %");
        }
        private void UpdateSetMaxLoadValue()
        {
            LabelFunc.SetText(lbl_XAxisMaxLoadTitle, SynusLangPack.GetLanguage("Label_XAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_ZAxisMaxLoadTitle, SynusLangPack.GetLanguage("Label_ZAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_AAxisMaxLoadTitle, SynusLangPack.GetLanguage("Label_AAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_TAxisMaxLoadTitle, SynusLangPack.GetLanguage("Label_TAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_AppliedValue2, SynusLangPack.GetLanguage("Label_AppliedValue"));
            LabelFunc.SetText(lbl_SetValue2, SynusLangPack.GetLanguage("Label_SetValue"));

            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LabelFunc.SetText(lbl_X_Axis_Read_SetMaxLoad, $"{rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.X_Axis_SetMaxLoad)} %");
            LabelFunc.SetText(lbl_Z_Axis_Read_SetMaxLoad, $"{rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.Z_Axis_SetMaxLoad)} %");
            LabelFunc.SetText(lbl_A_Axis_Read_SetMaxLoad, $"{rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.A_Axis_SetMaxLoad)} %");
            LabelFunc.SetText(lbl_T_Axis_Read_SetMaxLoad, $"{rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.T_Axis_SetMaxLoad)} %");
        }
        private void UpdateDetectedMaxLoadValue()
        {
            LabelFunc.SetText(lbl_XAxisMaxLoadTitle2, SynusLangPack.GetLanguage("Label_XAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_ZAxisMaxLoadTitle2, SynusLangPack.GetLanguage("Label_ZAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_AAxisMaxLoadTitle2, SynusLangPack.GetLanguage("Label_AAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_TAxisMaxLoadTitle2, SynusLangPack.GetLanguage("Label_TAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_DetectedValue, SynusLangPack.GetLanguage("Label_DetectValue"));

            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LabelFunc.SetText(lbl_X_Axis_Detected_OverLoad, $"{rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.X_Axis_CurrentMaxLoad)} %");
            LabelFunc.SetText(lbl_Z_Axis_Detected_OverLoad, $"{rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.Z_Axis_CurrentMaxLoad)} %");
            LabelFunc.SetText(lbl_A_Axis_Detected_OverLoad, $"{rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.A_Axis_CurrentMaxLoad)} %");
            LabelFunc.SetText(lbl_T_Axis_Detected_OverLoad, $"{rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.T_Axis_CurrentMaxLoad)} %");
        }

        private void UpdateMaintMoveStatus()
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;
            ButtonFunc.SetBackColor(btn_MaintMove, rackMaster.Status_MaintMove ? Color.Lime : Color.White);
        }
        private void btn_SpeedSettings_Send_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_AdminPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyResult"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result != DialogResult.OK)
                return;

            Button btn = (Button)sender;

            try
            {
                if (btn == btn_X_Axis_SpeedSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] X Axis Speed Settings Click");
                    short value = Convert.ToInt16(tbx_X_Axis_SpeedSettings.Text);
                    rackMaster.Interlock_SetAxisSpeedRatio(RackMaster.AxisType.X_Axis, value, RackMaster.InterlockFrom.UI_Event);
                }
                else if (btn == btn_Z_Axis_SpeedSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Z Axis Speed Settings Click");
                    short value = Convert.ToInt16(tbx_Z_Axis_SpeedSettings.Text);
                    rackMaster.Interlock_SetAxisSpeedRatio(RackMaster.AxisType.Z_Axis, value, RackMaster.InterlockFrom.UI_Event);
                }
                else if (btn == btn_A_Axis_SpeedSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] A Axis Speed Settings Click");
                    short value = Convert.ToInt16(tbx_A_Axis_SpeedSettings.Text);
                    rackMaster.Interlock_SetAxisSpeedRatio(RackMaster.AxisType.A_Axis, value, RackMaster.InterlockFrom.UI_Event);
                }
                else if (btn == btn_T_Axis_SpeedSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] T Axis Speed Settings Click");
                    short value = Convert.ToInt16(tbx_T_Axis_SpeedSettings.Text);
                    rackMaster.Interlock_SetAxisSpeedRatio(RackMaster.AxisType.T_Axis, value, RackMaster.InterlockFrom.UI_Event);
                }
                else if (btn == btn_All_Axis_SpeedSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] All Axis Speed Settings Click");

                    short value = Convert.ToInt16(tbx_X_Axis_SpeedSettings.Text);
                    rackMaster.Interlock_SetAxisSpeedRatio(RackMaster.AxisType.X_Axis, value, RackMaster.InterlockFrom.ApplicationLoop);

                    value = Convert.ToInt16(tbx_Z_Axis_SpeedSettings.Text);
                    rackMaster.Interlock_SetAxisSpeedRatio(RackMaster.AxisType.Z_Axis, value, RackMaster.InterlockFrom.ApplicationLoop);

                    value = Convert.ToInt16(tbx_A_Axis_SpeedSettings.Text);
                    rackMaster.Interlock_SetAxisSpeedRatio(RackMaster.AxisType.A_Axis, value, RackMaster.InterlockFrom.ApplicationLoop);

                    value = Convert.ToInt16(tbx_T_Axis_SpeedSettings.Text);
                    rackMaster.Interlock_SetAxisSpeedRatio(RackMaster.AxisType.T_Axis, value, RackMaster.InterlockFrom.ApplicationLoop);
                }
            }
            catch
            {

            }
        }

        private void btn_OverLoadSettings_Send_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_AdminPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyResult"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result != DialogResult.OK)
                return;

            Button btn = (Button)sender;

            try
            {
                if (btn == btn_X_Axis_OverLoadSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] X Axis Overload Settings Click");
                    short value = Convert.ToInt16(tbx_X_Axis_OverLoadSettings.Text);
                    rackMaster.Interlock_SetOverLoadValue(RackMaster.AxisType.X_Axis, value, RackMaster.InterlockFrom.UI_Event);
                }
                else if (btn == btn_Z_Axis_OverLoadSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Z Axis Overload Settings Click");
                    short value = Convert.ToInt16(tbx_Z_Axis_OverLoadSettings.Text);
                    rackMaster.Interlock_SetOverLoadValue(RackMaster.AxisType.Z_Axis, value, RackMaster.InterlockFrom.UI_Event);
                }
                else if (btn == btn_A_Axis_OverLoadSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] A Axis Overload Settings Click");
                    short value = Convert.ToInt16(tbx_A_Axis_OverLoadSettings.Text);
                    rackMaster.Interlock_SetOverLoadValue(RackMaster.AxisType.A_Axis, value, RackMaster.InterlockFrom.UI_Event);
                }
                else if (btn == btn_T_Axis_OverLoadSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] T Axis Overload Settings Click");
                    short value = Convert.ToInt16(tbx_T_Axis_OverLoadSettings.Text);
                    rackMaster.Interlock_SetOverLoadValue(RackMaster.AxisType.T_Axis, value, RackMaster.InterlockFrom.UI_Event);
                }
                else if (btn == btn_All_Axis_OverLoadSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] All Axis Overload Settings Click");
                    short value = Convert.ToInt16(tbx_X_Axis_OverLoadSettings.Text);
                    rackMaster.Interlock_SetOverLoadValue(RackMaster.AxisType.X_Axis, value, RackMaster.InterlockFrom.ApplicationLoop);

                    value = Convert.ToInt16(tbx_Z_Axis_OverLoadSettings.Text);
                    rackMaster.Interlock_SetOverLoadValue(RackMaster.AxisType.Z_Axis, value, RackMaster.InterlockFrom.ApplicationLoop);

                    value = Convert.ToInt16(tbx_A_Axis_OverLoadSettings.Text);
                    rackMaster.Interlock_SetOverLoadValue(RackMaster.AxisType.A_Axis, value, RackMaster.InterlockFrom.ApplicationLoop);

                    value = Convert.ToInt16(tbx_T_Axis_OverLoadSettings.Text);
                    rackMaster.Interlock_SetOverLoadValue(RackMaster.AxisType.T_Axis, value, RackMaster.InterlockFrom.ApplicationLoop);
                }
            }
            catch
            {

            }
        }

        private void btn_Detected_OverLoad_Clear_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyResult"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result != DialogResult.OK)
                return;

            Button btn = (Button)sender;

            try
            {
                if (btn == btn_X_Axis_Detected_OverLoad_Clear)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] X Axis Overload Clear Click");
                    rackMaster.Interlock_SetOverClear(RackMaster.AxisType.X_Axis, RackMaster.InterlockFrom.UI_Event);
                }
                else if (btn == btn_Z_Axis_Detected_OverLoad_Clear)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Z Axis Overload Clear Click");
                    rackMaster.Interlock_SetOverClear(RackMaster.AxisType.Z_Axis, RackMaster.InterlockFrom.UI_Event);
                }
                else if (btn == btn_A_Axis_Detected_OverLoad_Clear)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] A Axis Overload Clear Click");
                    rackMaster.Interlock_SetOverClear(RackMaster.AxisType.A_Axis, RackMaster.InterlockFrom.UI_Event);
                }
                else if (btn == btn_T_Axis_Detected_OverLoad_Clear)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] T Axis Overload Clear Click");
                    rackMaster.Interlock_SetOverClear(RackMaster.AxisType.T_Axis, RackMaster.InterlockFrom.UI_Event);
                }
                else if (btn == btn_All_Axis_Detected_OverLoad_Clear)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] All Axis Overload Clear Click");
                    rackMaster.Interlock_SetOverClear(RackMaster.AxisType.X_Axis, RackMaster.InterlockFrom.ApplicationLoop);
                    rackMaster.Interlock_SetOverClear(RackMaster.AxisType.Z_Axis, RackMaster.InterlockFrom.ApplicationLoop);
                    rackMaster.Interlock_SetOverClear(RackMaster.AxisType.A_Axis, RackMaster.InterlockFrom.ApplicationLoop);
                    rackMaster.Interlock_SetOverClear(RackMaster.AxisType.T_Axis, RackMaster.InterlockFrom.ApplicationLoop);
                }
            }
            catch
            {

            }
        }

        private void btn_TimeSync_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_CommandSendResult"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result != DialogResult.OK)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Time Sync Click");
            rackMaster.Interlock_SetTimeSync(RackMaster.InterlockFrom.UI_Event);
        }
        private void btn_MaintMove_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_CommandSendResult"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result != DialogResult.OK)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Maint Move Click");
            rackMaster.Interlock_MaintMove(RackMaster.InterlockFrom.UI_Event);
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }

        private void btn_SpeedSettings_Send_MouseDown(object sender, MouseEventArgs e)
        {
            Thread LocalThread = new Thread(delegate ()
            {
                Button btn = (Button)sender;
                m_PushSt.Restart();
                btn.Tag = m_PushSt;
                while (m_PushSt.ElapsedMilliseconds < 1000 && m_PushSt.IsRunning)
                {
                    Thread.Sleep(1);
                }
                m_PushSt.Stop();
                btn.Tag = null;

                if (m_PushSt.ElapsedMilliseconds >= 1000 && !m_PushSt.IsRunning)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            btn_SpeedSettings_Send_Click(sender, e);
                        }));
                    }
                    else
                    {
                        btn_SpeedSettings_Send_Click(sender, e);
                    }
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Start();
        }

        private void btn_Send_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            m_PushSt.Stop();
            btn.Tag = null;
        }

        private void btn_OverLoadSettings_Send_MouseDown(object sender, MouseEventArgs e)
        {
            Thread LocalThread = new Thread(delegate ()
            {
                Button btn = (Button)sender;
                m_PushSt.Restart();
                btn.Tag = m_PushSt;
                while (m_PushSt.ElapsedMilliseconds < 1000 && m_PushSt.IsRunning)
                {
                    Thread.Sleep(1);
                }
                m_PushSt.Stop();
                btn.Tag = null;

                if (m_PushSt.ElapsedMilliseconds >= 1000 && !m_PushSt.IsRunning)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            btn_OverLoadSettings_Send_Click(sender, e);
                        }));
                    }
                    else
                    {
                        btn_OverLoadSettings_Send_Click(sender, e);
                    }
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Start();
        }

        private void btn_Detected_OverLoad_Clear_MouseDown(object sender, MouseEventArgs e)
        {
            Thread LocalThread = new Thread(delegate ()
            {
                Button btn = (Button)sender;
                m_PushSt.Restart();
                btn.Tag = m_PushSt;
                while (m_PushSt.ElapsedMilliseconds < 1000 && m_PushSt.IsRunning)
                {
                    Thread.Sleep(1);
                }
                m_PushSt.Stop();
                btn.Tag = null;

                if (m_PushSt.ElapsedMilliseconds >= 1000 && !m_PushSt.IsRunning)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            btn_Detected_OverLoad_Clear_Click(sender, e);
                        }));
                    }
                    else
                    {
                        btn_Detected_OverLoad_Clear_Click(sender, e);
                    }
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Start();
        }

        private void btn_TimeSync_MouseDown(object sender, MouseEventArgs e)
        {
            Thread LocalThread = new Thread(delegate ()
            {
                Button btn = (Button)sender;
                m_PushSt.Restart();
                btn.Tag = m_PushSt;
                while (m_PushSt.ElapsedMilliseconds < 1000 && m_PushSt.IsRunning)
                {
                    Thread.Sleep(1);
                }
                m_PushSt.Stop();
                btn.Tag = null;

                if (m_PushSt.ElapsedMilliseconds >= 1000 && !m_PushSt.IsRunning)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            btn_TimeSync_Click(sender, e);
                        }));
                    }
                    else
                    {
                        btn_TimeSync_Click(sender, e);
                    }
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Start();
        }

        private void btn_MaintMove_MouseDown(object sender, MouseEventArgs e)
        {
            Thread LocalThread = new Thread(delegate ()
            {
                Button btn = (Button)sender;
                m_PushSt.Restart();
                btn.Tag = m_PushSt;
                while (m_PushSt.ElapsedMilliseconds < 1000 && m_PushSt.IsRunning)
                {
                    Thread.Sleep(1);
                }
                m_PushSt.Stop();
                btn.Tag = null;

                if (m_PushSt.ElapsedMilliseconds >= 1000 && !m_PushSt.IsRunning)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            btn_MaintMove_Click(sender, e);
                        }));
                    }
                    else
                    {
                        btn_MaintMove_Click(sender, e);
                    }
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Start();
        }
    }
}
