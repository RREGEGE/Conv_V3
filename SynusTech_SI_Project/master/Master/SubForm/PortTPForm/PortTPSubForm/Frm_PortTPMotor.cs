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
using Master.SubForm.PortTPForm.PortTPSubForm.ControlForm;
using Master.SubForm.PortTPForm.PortTPSubForm.StatusForm;
using System.Threading;

namespace Master.SubForm.PortTPForm.PortTPSubForm
{
    public partial class Frm_PortTPMotor : Form
    {
        Frm_PortStatus frm_PortStatus;
        Frm_MotionSelect frm_MotionSelect;
        public Frm_PortTPMotor(Frm_PortStatus _Frm_PortStatus, Frm_MotionSelect _Frm_MotionSelect)
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
                    foreach(var port in Master.m_Ports)
                    {
                        port.Value.PIOStatus_ManualReleasePortToRMPIO();
                    }
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

                GroupBoxFunc.SetText(groupBox_RFID, SynusLangPack.GetLanguage("GorupBox_RFID"));
                GroupBoxFunc.SetText(groupBox_RackMasterManualPIO, SynusLangPack.GetLanguage("GorupBox_RackMasterManualPIO"));

                frm_PortStatus.SetCurrentFocus(frm_MotionSelect.GetCurrentTag());
                frm_PortStatus.ShowPIOStatus();

                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort != null)
                {
                    LabelFunc.SetText(lbl_Tag, $"Tag : {CurrentPort.m_TagReader_Interface.GetTag()}");

                    ButtonFunc.SetBackColor(btn_RM_Load_REQ, CurrentPort.PIOStatus_PortToSTK_Load_Req ? Color.Lime : Color.White);
                    ButtonFunc.SetBackColor(btn_RM_Unload_REQ, CurrentPort.PIOStatus_PortToSTK_Unload_Req ? Color.Lime : Color.White);
                    ButtonFunc.SetBackColor(btn_RM_Ready, CurrentPort.PIOStatus_PortToSTK_Ready ? Color.Lime : Color.White);
                    ButtonFunc.SetBackColor(btn_RM_Error, CurrentPort.PIOStatus_PortToSTK_Error ? Color.Lime : Color.White);
                }
            }
            catch{ }
            finally
            {

            }
        }

        private void btn_RFIDRead_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Tag Read Click");

            Thread LocalThread = new Thread(delegate ()
            {
                try
                {
                    if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
                        CurrentPort.m_TagReader_Interface.GetRFIDReader().TagRead();
                    else if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.BCR)
                        CurrentPort.m_TagReader_Interface.GetBCRReader().TagRead();
                    else if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.CanTops_LM21)
                        CurrentPort.m_TagReader_Interface.GetCanTopsLM21Reader().TagRead();
                }
                catch (Exception ex)
                {
                    LogMsg.AddPortLog(CurrentPort.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.TagReaderInfo, $"CST ID Read Exception");
                    LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}], CST ID Read Exception");
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Name = $"Port[{CurrentPort.GetParam().ID}] CST ID Reader Read";
            LocalThread.Start();
        }

        private void btn_RM_TR_REQ_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Manual PIO TR-REQ Click");

            CurrentPort.PIOStatus_ManualSavePortToRMPIO();
            CurrentPort.Interlock_Manual_PIO_PortToRM_LoadREQ(!CurrentPort.PIOStatus_PortToSTK_Load_Req, Port.InterlockFrom.UI_Event);
        }

        private void btn_RM_BUSY_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Manual PIO Busy Click");

            CurrentPort.PIOStatus_ManualSavePortToRMPIO();
            CurrentPort.Interlock_Manual_PIO_PortToRM_UnloadREQ(!CurrentPort.PIOStatus_PortToSTK_Unload_Req, Port.InterlockFrom.UI_Event);
        }

        private void btn_RM_Complete_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Manual PIO Complete Click");

            CurrentPort.PIOStatus_ManualSavePortToRMPIO();
            CurrentPort.Interlock_Manual_PIO_PortToRM_Ready(!CurrentPort.PIOStatus_PortToSTK_Ready, Port.InterlockFrom.UI_Event);
        }

        private void btn_RM_Error_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Manual PIO Error Click");

            CurrentPort.PIOStatus_ManualSavePortToRMPIO();
            CurrentPort.Interlock_Manual_PIO_PortToRM_Error(!CurrentPort.PIOStatus_PortToSTK_Error, Port.InterlockFrom.UI_Event);
        }
    }
}
