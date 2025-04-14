using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Master.SubForm.MasterForm.MasterSubForm
{
    public partial class Frm_MasterCtrlMain : Form
    {
        public Frm_MasterCtrlMain()
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
                GroupBoxFunc.SetText(groupBox_EquipmentStatus, SynusLangPack.GetLanguage("GorupBox_MasterEquipmentStatus"));
                GroupBoxFunc.SetText(groupBox_MasterInterlockControl, SynusLangPack.GetLanguage("GroupBox_MasterInterlockControl"));
                Master.Update_DGV_SaftyIOStatus(ref DGV_SaftyIOStatus, null);
                Master.Update_DGV_EquipmentAutoRunStatus(ref DGV_EquipmentStatus);

                bool bDoorOpenState     = Master.CMD_DoorOpen_Relay && Master.CMD_DoorBypass_Relay;
                bool bDoorCloseState    = !Master.CMD_DoorOpen_Relay && !Master.CMD_DoorBypass_Relay;
                ButtonFunc.SetBackColor(btn_DoorOpen, bDoorOpenState ? Color.Lime : Color.White);
                ButtonFunc.SetBackColor(btn_DoorClose, bDoorCloseState ? Color.Lime : Color.White);

                ButtonFunc.SetEnable(btn_DoorOpen, !bDoorOpenState && !bDoorCloseState ? false : true);
                ButtonFunc.SetEnable(btn_DoorClose, !bDoorOpenState && !bDoorCloseState ? false : true);
            }
            catch{ }
            finally
            {

            }
        }

        private void btn_DoorOpen_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Master Control Form-Door Open Click");

            foreach (var rackmaster in Master.m_RackMasters)
            {
                if (rackmaster.Value.Status_AutoMode)
                {
                    LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.UnableOpenTheDoor, $"RackMaster[{rackmaster.Value.GetParam().ID}] Auto Running State.");
                    MessageBox.Show($"RackMaster[{rackmaster.Value.GetParam().ID}] Auto Running State.", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if(Master.Sensor_HPAutoKey)
            {
                LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.UnableOpenTheDoor, $"HP Key Auto State.");
                MessageBox.Show("HP Key Auto State.", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Master.Sensor_Oxygen_Saturation_Warning_Status)
            {
                LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.UnableOpenTheDoor, $"Oxygen saturation warning status inside door.");
                MessageBox.Show("Oxygen saturation warning status inside door.", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_Master_DoorOpenWarningMessage"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result != DialogResult.OK)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Master Control Form-Door Open Cancel");
                return;
            }

            Master.CMD_DoorOpen_REQ = true;
        }

        private void btn_DoorClose_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Master Control Form-Door Close Click");
            Master.CMD_DoorOpen_REQ = false;
        }
    }
}
