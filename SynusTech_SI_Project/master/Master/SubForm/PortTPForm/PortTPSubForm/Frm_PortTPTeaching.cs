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
using Master.ManagedFile;
using Master.SubForm.PortTPForm.PortTPSubForm.ControlForm;
using Master.SubForm.PortTPForm.PortTPSubForm.StatusForm;

namespace Master.SubForm.PortTPForm.PortTPSubForm
{
    public partial class Frm_PortTPTeaching : Form
    {
        Frm_PortStatus frm_PortStatus;
        Frm_MotionSelect frm_MotionSelect;
        public Frm_PortTPTeaching(Frm_PortStatus _Frm_PortStatus, Frm_MotionSelect _Frm_MotionSelect)
        {
            InitializeComponent();
            frm_PortStatus = _Frm_PortStatus;
            frm_MotionSelect = _Frm_MotionSelect;
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

            tableLayoutPanel8.RowStyles[1].Height = 70.0f;
        }
        private void TeachingParameterClear()
        {
            if (DGV_ParameterValue.Rows.Count > 0)
                DGV_ParameterValue.Rows.Clear();

            DGV_ParameterValue.Tag = null;
            if (groupBox_TeachingParameter.Visible)
                groupBox_TeachingParameter.Visible = false;

            tableLayoutPanel1.RowStyles[0].Height = 100.0f;
            tableLayoutPanel1.RowStyles[1].Height = 0.0f;
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

                if (!pnl_PortControl.Controls.Contains(frm_MotionSelect))
                {
                    pnl_PortControl.Controls.Clear();
                    pnl_PortControl.Controls.Add(frm_MotionSelect);
                    pnl_PortControl.Controls[0].Dock = DockStyle.Fill;
                    pnl_PortControl.Controls[0].Show();
                }

                ButtonFunc.SetText(btn_ParamSave, SynusLangPack.GetLanguage("Btn_ParamSave"));

                frm_PortStatus.SetCurrentFocus(frm_MotionSelect.GetCurrentTag());
                frm_PortStatus.HidePIOStatus();

                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort != null)
                {
                    object PortAxisobj = frm_MotionSelect.GetCurrentPortAxis();
                    if (PortAxisobj == null)
                    {
                        TeachingParameterClear();
                        return;
                    }


                    Port.PortAxis ePortAxis = (Port.PortAxis)PortAxisobj;

                    if (CurrentPort.GetMotionParam().IsValidServo(ePortAxis))
                    {
                        string Key = $"{CurrentPort.GetParam().ID}_{ePortAxis}";
                        if ((string)DGV_ParameterValue.Tag != Key)
                        {
                            DGV_ParameterValue.Rows.Clear();
                            DGV_ParameterValue.Tag = Key;

                            CurrentPort.LoadTeachingParamDataGridView(ref DGV_ParameterValue, ref groupBox_TeachingParameter, ePortAxis);

                            if (!groupBox_TeachingParameter.Visible)
                                groupBox_TeachingParameter.Visible = true;

                            tableLayoutPanel1.RowStyles[0].Height = 50.0f;
                            tableLayoutPanel1.RowStyles[1].Height = 50.0f;
                        }

                        DataGridViewFunc.AutoRowSize(DGV_ParameterValue, 20, 20, 30);
                    }
                    else
                    {
                        TeachingParameterClear();
                    }
                }
                else
                {
                    TeachingParameterClear();
                }
            }
            catch{ }
            finally
            {

            }
        }
        

        private void DGV_ParameterValue_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                DataGridViewCell DGV_Cell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];

                string value = (string)DGV_Cell.Value;

                if(value.Contains("Set Cur"))
                {
                    Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                    if (CurrentPort == null)
                        return;

                    object PortAxisobj = frm_MotionSelect.GetCurrentPortAxis();
                    if (PortAxisobj == null)
                    {
                        return;
                    }
                    Port.PortAxis ePortAxis = (Port.PortAxis)PortAxisobj;


                    if (CurrentPort.GetMotionParam().IsValidServo(ePortAxis))
                    {
                        float CurPos = (float)CurrentPort.Motion_CurrentPosition(ePortAxis);

                        senderGrid.Rows[e.RowIndex].Cells[(int)Port.DGV_TeachingParamColumn.SetValue].Value = CurPos.ToString("0.0");

                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Set Current Pos({CurPos.ToString("0.0")}) Click");
                    }
                }
            }
        }

        private void btn_ParamSave_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Teaching Parameter Save Click");

            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (CurrentPort.IsAutoControlRun())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InAutoControl"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] is Auto Running State");
                return;
            }

            DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_ApplyPortResult"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result != DialogResult.OK)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] Teaching Parameter Save Cancel");
                return;
            }

            object PortAxisobj = frm_MotionSelect.GetCurrentPortAxis();
            if (PortAxisobj == null)
            {
                return;
            }
            Port.PortAxis ePortAxis = (Port.PortAxis)PortAxisobj;

            if (CurrentPort.GetMotionParam().IsValidServo(ePortAxis))
            {
                if (ePortAxis == Port.PortAxis.Shuttle_X ||
                    ePortAxis == Port.PortAxis.Buffer_LP_X ||
                    ePortAxis == Port.PortAxis.Buffer_OP_X)
                {
                    CurrentPort.Apply_X_AxisTeachingParam(ref DGV_ParameterValue, ePortAxis);
                }
                else if (ePortAxis == Port.PortAxis.Buffer_LP_Y ||
                    ePortAxis == Port.PortAxis.Buffer_OP_Y)
                {
                    CurrentPort.Apply_Y_AxisTeachingParam(ref DGV_ParameterValue, ePortAxis);
                }
                else if (ePortAxis == Port.PortAxis.Shuttle_Z ||
                    ePortAxis == Port.PortAxis.Buffer_LP_Z ||
                    ePortAxis == Port.PortAxis.Buffer_OP_Z)
                {
                    CurrentPort.Apply_Z_AxisTeachingParam(ref DGV_ParameterValue, ePortAxis);
                }
                else if (ePortAxis == Port.PortAxis.Shuttle_T ||
                    ePortAxis == Port.PortAxis.Buffer_LP_T ||
                    ePortAxis == Port.PortAxis.Buffer_OP_T)
                {
                    CurrentPort.Apply_T_AxisTeachingParam(ref DGV_ParameterValue, ePortAxis);
                }
                else
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] {ePortAxis} Apply Teaching Param");

                CurrentPort.GetMotionParam().Save(CurrentPort.GetParam().ID, CurrentPort.GetMotionParam());
                DGV_ParameterValue.Tag = null;
            }
        }
    }
}
