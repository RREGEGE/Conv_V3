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

namespace Master.SubForm.PortTPForm.PortTPSubForm.StatusForm
{
    public partial class Frm_PortStatus : Form
    {
        object CurrentTag = null;
        public Frm_PortStatus()
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
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                GroupBoxFunc.SetText(groupBox_PortStatusMonitor, SynusLangPack.GetLanguage("GorupBox_PortStatus") + $" ({CurrentPort.GetPortOperationMode()})");
                GroupBoxFunc.SetText(groupBox_PortMotionStatus, SynusLangPack.GetLanguage("GorupBox_MotionStatus"));
                GroupBoxFunc.SetText(groupBox_PortSensorStatus, SynusLangPack.GetLanguage("GorupBox_SensorStatus"));
                GroupBoxFunc.SetText(groupBox_PortPIOStatus, SynusLangPack.GetLanguage("GorupBox_PIOStatus"));
                tableLayoutPanel_PIOStatus.ColumnStyles[0].Width = CurrentPort.IsEQPort() ? 0.0F : 50.0F;
                tableLayoutPanel_PIOStatus.ColumnStyles[1].Width = CurrentPort.IsEQPort() ? 100.0F : 50.0F;

                if (CurrentPort != null)
                {
                    ///Motion Status
                    if (CurrentPort.IsShuttleControlPort() || CurrentPort.IsBufferControlPort())
                    {
                        CurrentPort.Update_DGV_MotionServoStatus(ref DGV_Motion_ServoStatus, CurrentTag);
                        CurrentPort.Update_DGV_MotionInverterStatus(ref DGV_Motion_InverterStatus, CurrentTag);
                        CurrentPort.Update_DGV_MotionCylinderStatus(ref DGV_Motion_CylinderStatus, CurrentTag);
                        CurrentPort.Update_DGV_MotionConveyorStatus(ref DGV_Motion_ConveyorStatus, CurrentTag);
                        CurrentPort.Update_DGV_MotionCVOptionStatus(ref DGV_Motion_CVOptionStatus, CurrentTag);

                        GroupBoxFunc.SetVisible(groupBox_PortMotionStatus, true);
                        int PortMotionStatusTotalHeight = DGV_Motion_ServoStatus.Height + DGV_Motion_InverterStatus.Height + DGV_Motion_CylinderStatus.Height + DGV_Motion_ConveyorStatus.Height + DGV_Motion_CVOptionStatus.Height;
                        GroupBoxFunc.SetHeight(groupBox_PortMotionStatus, PortMotionStatusTotalHeight + 30);
                    }
                    else
                    {
                        GroupBoxFunc.SetVisible(groupBox_PortMotionStatus, false);
                    }

                    ///Sensor Status
                    if (CurrentPort.IsShuttleControlPort() || CurrentPort.IsBufferControlPort())
                    {
                        CurrentPort.Update_DGV_PortSensorStatus(ref DGV_PortSensorStatus);

                        GroupBoxFunc.SetVisible(groupBox_PortSensorStatus, true);
                        int PortSensorStatusTotalHeight = DGV_PortSensorStatus.Height;
                        GroupBoxFunc.SetHeight(groupBox_PortSensorStatus, PortSensorStatusTotalHeight + 30);
                    }
                    else
                    {
                        GroupBoxFunc.SetVisible(groupBox_PortSensorStatus, false);
                    }


                    ///PIO Status
                    if (!CurrentPort.IsEQPort())
                    {
                        if (!DGV_Buffer1PIOStatus.Visible)
                            DGV_Buffer1PIOStatus.Visible = true;

                        CurrentPort.Update_DGV_Buffer1PIOStatus(ref DGV_Buffer1PIOStatus);
                    }
                    else
                    {
                        if (DGV_Buffer1PIOStatus.Visible)
                            DGV_Buffer1PIOStatus.Visible = false;

                        DGV_Buffer1PIOStatus.Height = 0;
                    }
                    CurrentPort.Update_DGV_Buffer2PIOStatus(ref DGV_Buffer2PIOStatus);

                    int PortPIOStatusTotalHeight = (DGV_Buffer1PIOStatus.Height >= DGV_Buffer2PIOStatus.Height ? DGV_Buffer1PIOStatus.Height : DGV_Buffer2PIOStatus.Height) + 6;
                    TableLayoutPanelFunc.SetHeight(tableLayoutPanel_PIOStatus, PortPIOStatusTotalHeight);
                    GroupBoxFunc.SetHeight(groupBox_PortPIOStatus, PortPIOStatusTotalHeight + 30);
                }
            }
            catch{ }
            finally
            {

            }
        }

        public void HidePIOStatus()
        {
            if (groupBox_PortPIOStatus.Visible)
                groupBox_PortPIOStatus.Visible = false;
        }
        public void ShowPIOStatus()
        {
            if (!groupBox_PortPIOStatus.Visible)
                groupBox_PortPIOStatus.Visible = true;
        }

        private void DGV_Motion_ServoStatus_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort != null)
                CurrentPort.Event_DGV_ShuttleAxisStatus_CellContentClick(sender, e);
        }

        public void SetCurrentFocus(object obj)
        {
            CurrentTag = obj;
        }
    }
}
