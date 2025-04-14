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
using Master.Equipment.RackMaster;

namespace Master.SubForm.RMTPForm.RMTPSubForm
{
    public partial class Frm_RackMasterTPMain : Form
    {
        Label[] PortInfoLabel = null;

        public Frm_RackMasterTPMain()
        {
            InitializeComponent();
            ControlItemInit();

            this.VisibleChanged += (object sender, EventArgs e) =>
            {
                if (this.Visible)
                {
                    RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;
                    if (rackMaster != null)
                    {
                        tbx_CycleCount.Text = rackMaster.m_CycleCount.ToString();
                        tbx_FromID.Text = rackMaster.m_nFromID.ToString();
                        tbx_ToID.Text = rackMaster.m_nToID.ToString();
                    }

                    UIUpdateTimer.Enabled = true;
                }
                else
                {
                    RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;
                    if (rackMaster != null)
                    {
                        if (!rackMaster.IsAutoCycleRun())
                            rackMaster.m_CycleRunningTime.Reset();
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

            //Port Label Create
            PortInfoLabel = new Label[Master.m_Ports.Count];
            for(int nCount = 0; nCount < PortInfoLabel.Length; nCount++)
            {
                var port = Master.m_Ports.ElementAt(nCount);

                PortInfoLabel[nCount] = new Label();
                PortInfoLabel[nCount].TextAlign = ContentAlignment.MiddleCenter;
                PortInfoLabel[nCount].Margin = new Padding(1, 1, 1, 1);
                PortInfoLabel[nCount].Tag = port.Key;
                PortInfoLabel[nCount].Click += btn_PortLabel_Click;

                ContextMenu ctx = new ContextMenu();

                MenuItem item = new MenuItem();
                item.Text = "Error Clear";
                item.Tag = port.Key;
                item.Click += btn_PortErrorClear_Click;

                ctx.MenuItems.Add(item);
                ctx.MenuItems.Add("-");

                MenuItem item2 = new MenuItem();
                item2.Text = "Power On";
                item2.Tag = port.Key;
                item2.Click += btn_PortPowerOn_Click;
                ctx.MenuItems.Add(item2);

                MenuItem item3 = new MenuItem();
                item3.Text = "Power Off";
                item3.Tag = port.Key;
                item3.Click += btn_PortPowerOff_Click;
                ctx.MenuItems.Add(item3);
                ctx.MenuItems.Add("-");

                MenuItem item4 = new MenuItem();
                item4.Text = "Auto Run";
                item4.Tag = port.Key;
                item4.Click += btn_PortAutoRun_Click;
                ctx.MenuItems.Add(item4);

                MenuItem item5 = new MenuItem();
                item5.Text = "Auto Stop";
                item5.Tag = port.Key;
                item5.Click += btn_PortAutoStop_Click;
                ctx.MenuItems.Add(item5);
                ctx.MenuItems.Add("-");

                MenuItem item7 = new MenuItem();
                item7.Text = "Direction Change";
                item7.Tag = port.Key;
                item7.Click += btn_PortDirectionChange_Click;
                ctx.MenuItems.Add(item7);

                PortInfoLabel[nCount].ContextMenu = ctx;
            }

            //Column Setting
            tableLayoutPanel_PortInfo.ColumnCount = 4;
            for (int nColumnCount = 0; nColumnCount < tableLayoutPanel_PortInfo.ColumnCount; nColumnCount++)
            {
                tableLayoutPanel_PortInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            }
            //Row Setting
            tableLayoutPanel_PortInfo.RowCount = (Master.m_Ports.Count % 4 == 0 ? Master.m_Ports.Count / 4 : Master.m_Ports.Count / 4 + 1);
            for (int nRowCount = 0; nRowCount < tableLayoutPanel_PortInfo.RowCount; nRowCount++)
            {
                tableLayoutPanel_PortInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            }

            tableLayoutPanel_PortInfo.Dock = DockStyle.Fill;

            //Grid Add
            int nLabelCount = 0;
            for (int nRowCount = 0; nRowCount < tableLayoutPanel_PortInfo.RowCount; nRowCount++)
            {
                for (int nColumnCount = 0; nColumnCount < tableLayoutPanel_PortInfo.ColumnCount; nColumnCount++)
                {
                    if (nLabelCount >= PortInfoLabel.Length)
                        continue;

                    tableLayoutPanel_PortInfo.Controls.Add(PortInfoLabel[nLabelCount], nColumnCount, nRowCount);
                    PortInfoLabel[nLabelCount].Dock = DockStyle.Fill;
                    nLabelCount++;
                }
            }
            //Table Size
            tableLayoutPanel_PortPanel.RowStyles[0].Height = tableLayoutPanel_PortInfo.RowCount * 50 + 40;
        }

        private void PortInfoUpdate()
        {
            for(int nCount =0; nCount < Master.m_Ports.Count; nCount++)
            {
                if (nCount >= PortInfoLabel.Length)
                    continue;

                Master.m_Ports.ElementAt(nCount).Value.Update_Lbl_PortInfoLabel(ref PortInfoLabel[nCount], Port.PortInfoType.Simple);
            }

            //foreach (var port in Master.m_Ports.Select((value, index) => (value, index)))
            //{
            //    if (port.index >= PortInfoLabel.Length)
            //        continue;

            //    port.value.Value.Update_Lbl_PortInfoLabel(ref PortInfoLabel[port.index], Port.PortInfoType.Simple);
            //}
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
                LabelFunc.SetText(label_CycleCount, SynusLangPack.GetLanguage("Label_CycleCount") + " :");
                GroupBoxFunc.SetText(groupBox_RackMasterStatus, SynusLangPack.GetLanguage("GorupBox_RackMasterStatus"));
                GroupBoxFunc.SetText(groupBox_RackMasterMotionStatus, SynusLangPack.GetLanguage("GorupBox_MotionStatus"));
                GroupBoxFunc.SetText(groupBox_RackMasterSensorStatus, SynusLangPack.GetLanguage("GorupBox_SensorStatus"));
                GroupBoxFunc.SetText(groupBox_RegulatorStatus, SynusLangPack.GetLanguage("GroupBox_RegulatorStatus"));
                GroupBoxFunc.SetText(groupBox_PortInfo, SynusLangPack.GetLanguage("GorupBox_PortInfo"));
                GroupBoxFunc.SetText(groupBox_ErrorInfo, SynusLangPack.GetLanguage("GorupBox_ErrorInfo"));
                GroupBoxFunc.SetText(groupBox_StatusInfo, SynusLangPack.GetLanguage("GorupBox_StatusInfo"));
                GroupBoxFunc.SetText(groupBox_InterlockInfo, SynusLangPack.GetLanguage("GorupBox_Interlock"));
                GroupBoxFunc.SetText(groupBox_PIOStatus, SynusLangPack.GetLanguage("GorupBox_PIOStatus"));

                UpdateEquipmentItem();

                PortInfoUpdate();
            }
            catch{ }
            finally
            {

            }
        }

        private void UpdateEquipmentItem()
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;
            
            if (rackMaster != null)
            {
                rackMaster.Update_DGV_RMMotionStatus(ref DGV_RackMasterMotionStatus);
                rackMaster.Update_DGV_RMSensorStatus(ref DGV_RackMasterSensorStatus);
                rackMaster.Update_DGV_RegulatorStatus(ref DGV_RackMasterRegulatorStatus);
                rackMaster.Update_Btn_AutoRun(ref btn_AutoRun);
                rackMaster.Update_Btn_AutoStop(ref btn_AutoStop);
                rackMaster.Update_Btn_CIMMode(ref btn_CIMMode);
                rackMaster.Update_Btn_MasterMode(ref btn_MasterMode);
                rackMaster.Update_Btn_CycleRun(ref btn_CycleRun);
                rackMaster.Update_Btn_CycleStop(ref btn_CycleStop);
                rackMaster.Update_GBx_CycleGroup(ref groupBox_CycleSettings);

                rackMaster.Update_Lbl_AutoCycleStep(ref lbl_CycleStep);
                rackMaster.Update_Lbl_AutoCycleError(ref lbl_CycleError);
                rackMaster.Update_Lbl_AutoCycleProgressPercent(ref lbl_CycleProgressCount);
                rackMaster.Update_Lbl_AutoCycleProgressTime(ref lbl_CycleProgressTime);
                rackMaster.Update_Lbl_AutoCyclePIOProgressTime(ref lbl_PIOTimer);

                string AccessID = Convert.ToString(rackMaster.Status_STK_To_CIM_AccessID);
                if (Master.m_Ports.ContainsKey(AccessID))
                {
                    var port = Master.m_Ports[AccessID];
                    GroupBoxFunc.SetText(groupBox_AccessPort, SynusLangPack.GetLanguage("GorupBox_AccessPortInfo") + $"[{rackMaster.Status_STK_To_CIM_AccessID}]");
                    GroupBoxFunc.SetVisible(groupBox_AccessPort, true);
                    rackMaster.Update_DGV_RMPIOStatus(ref DGV_RackMasterPIO);
                    port.Update_DGV_PortStatusInfo(ref DGV_PortStatusInfo);
                    port.Update_DGV_ErrorInfo(ref DGV_ErrorInfo);
                    port.Update_DGV_InterlockInfo(ref DGV_InterlockInfo);
                }
                else
                {
                    string SelectedID = (string)groupBox_AccessPort.Tag;
                    if (Master.m_Ports.ContainsKey(SelectedID ?? string.Empty))
                    {
                        var port = Master.m_Ports[SelectedID];
                        GroupBoxFunc.SetText(groupBox_AccessPort, SynusLangPack.GetLanguage("GorupBox_SelectedPortInfo") + $"[{SelectedID}]");
                        GroupBoxFunc.SetVisible(groupBox_AccessPort, true);
                        rackMaster.Update_DGV_RMPIOStatus(ref DGV_RackMasterPIO);
                        port.Update_DGV_PortStatusInfo(ref DGV_PortStatusInfo);
                        port.Update_DGV_ErrorInfo(ref DGV_ErrorInfo);
                        port.Update_DGV_InterlockInfo(ref DGV_InterlockInfo);
                    }
                    else
                    {
                        if (PortInfoLabel.Length > 0)
                            groupBox_AccessPort.Tag = PortInfoLabel[0].Tag;
                        else
                        {
                            GroupBoxFunc.SetText(groupBox_AccessPort, SynusLangPack.GetLanguage("GorupBox_SelectedPortInfo") + $"[None]");
                            GroupBoxFunc.SetVisible(groupBox_AccessPort, true);
                            DGV_RackMasterPIO.Rows.Clear();
                            DGV_PortStatusInfo.Rows.Clear();
                            DGV_ErrorInfo.Rows.Clear();
                            DGV_InterlockInfo.Rows.Clear();
                        }
                    }
                }
            }
        }

        private void btn_AutoRun_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Auto Run Click");
            rackMaster.Interlock_AutoModeEnable(RackMaster.InterlockFrom.UI_Event);
        }

        private void btn_AutoStop_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Auto Stop Click");
            rackMaster.Interlock_AutoModeDisable(RackMaster.InterlockFrom.UI_Event);
        }

        private void btn_CycleRun_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Cycle Run Click");

            try
            {
                int CycleCount = Convert.ToInt32(tbx_CycleCount.Text);
                int FromID = Convert.ToInt32(tbx_FromID.Text);
                int ToID = Convert.ToInt32(tbx_ToID.Text);

                if(Master.m_Ports.ContainsKey(tbx_FromID.Text))
                {
                    var port = Master.m_Ports[tbx_FromID.Text];
                    if (port.GetParam().ePortType != Port.PortType.EQ && port.GetOperationDirection() == Port.PortDirection.Output)
                    {
                        //Error
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InvalidDirection"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"From Port Direction Error / From Port ID: {tbx_FromID.Text} / Port Direction: {port.GetOperationDirection()}");
                        return;
                    }
                    else if (port.GetParam().ePortType == Port.PortType.EQ)
                    {
                        //Error
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InvalidCycle_EQPort"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Invalid Port Type Error / From Port ID: {tbx_FromID.Text} / Port Type: {port.GetParam().ePortType}");
                        return;
                    }
                }

                if (Master.m_Ports.ContainsKey(tbx_ToID.Text))
                {
                    var port = Master.m_Ports[tbx_ToID.Text];
                    if (port.GetParam().ePortType != Port.PortType.EQ && port.GetOperationDirection() == Port.PortDirection.Input)
                    {
                        //Error
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InvalidDirection"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"To Port Direction Error / To Port ID: {tbx_ToID.Text} / Port Direction: {port.GetOperationDirection()}");
                        return;
                    }
                    else if (port.GetParam().ePortType == Port.PortType.EQ)
                    {
                        //Error
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InvalidCycle_EQPort"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Invalid Port Type Error / To Port ID: {tbx_ToID.Text} / Port Type: {port.GetParam().ePortType}");
                        return;
                    }
                }

                rackMaster.Interlock_StartAutoCycleControl(CycleCount, FromID, ToID, RackMaster.InterlockFrom.UI_Event);
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"RackMaster[{rackMaster.GetParam().ID}] Cycle Run");
            }
        }

        private void btn_CycleStop_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Cycle Stop Click");
            rackMaster.AutoCycleStop();
        }

        private void btn_CIMMode_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] CIM Mode Click");
            rackMaster.Interlock_SetControlMode(RackMaster.ControlMode.CIMMode, RackMaster.InterlockFrom.UI_Event);
        }

        private void btn_MasterMode_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Master Mode Click");
            rackMaster.Interlock_SetControlMode(RackMaster.ControlMode.MasterMode, RackMaster.InterlockFrom.UI_Event);
        }

        private void btn_PortLabel_Click(object sender, EventArgs eventArgs)
        {
            Label item = (Label)sender;
            string PortNumber = Convert.ToString(item.Tag);

            if (Master.m_Ports.ContainsKey(PortNumber))
            {
                groupBox_AccessPort.Tag = PortNumber;
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Port[{PortNumber}] Click");
            }
            else
                groupBox_AccessPort.Tag = null;
        }

        private void btn_PortErrorClear_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            string PortNumber = Convert.ToString(item.Tag);

            if(Master.m_Ports.ContainsKey(PortNumber))
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Port[{PortNumber}] Alarm Clear Click");
                Master.m_Ports[PortNumber].Interlock_PortAmpAlarmClear(Equipment.Port.Port.InterlockFrom.UI_Event);
                Master.m_Ports[PortNumber].Interlock_PortAlarmClear(Port.InterlockFrom.UI_Event);
            }
        }
        private void btn_PortPowerOn_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            string PortNumber = Convert.ToString(item.Tag);

            if (Master.m_Ports.ContainsKey(PortNumber))
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Port[{PortNumber}] Power On Click");
                Master.m_Ports[PortNumber].Interlock_PortPowerOn(Port.InterlockFrom.UI_Event);
            }
        }
        private void btn_PortPowerOff_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            string PortNumber = Convert.ToString(item.Tag);

            if (Master.m_Ports.ContainsKey(PortNumber))
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Port[{PortNumber}] Power Off Click");
                Master.m_Ports[PortNumber].Interlock_PortPowerOff(Port.InterlockFrom.UI_Event);
            }
        }

        private void btn_PortAutoRun_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            string PortNumber = Convert.ToString(item.Tag);

            if (Master.m_Ports.ContainsKey(PortNumber))
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Port[{PortNumber}] Auto Run Click");
                Master.m_Ports[PortNumber].Interlock_StartAutoControl(Port.InterlockFrom.UI_Event);
            }
        }

        private void btn_PortAutoStop_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            string PortNumber = Convert.ToString(item.Tag);

            if (Master.m_Ports.ContainsKey(PortNumber))
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Port[{PortNumber}] Auto Stop Click");
                Master.m_Ports[PortNumber].Interlock_StopAutoControl(Port.InterlockFrom.UI_Event);
            }
        }

        private void btn_PortDirectionChange_Click(object sender, EventArgs eventArgs)
        {
            MenuItem item = (MenuItem)sender;
            string PortNumber = Convert.ToString(item.Tag);

            if (Master.m_Ports.ContainsKey(PortNumber))
            {
                if (Master.m_Ports[PortNumber].GetOperationDirection() == Port.PortDirection.Input)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Port[{PortNumber}] Direction Change(Output) Click");
                    Master.m_Ports[PortNumber].Interlock_AutoControlDirectionChange(Port.PortDirection.Output, Port.InterlockFrom.UI_Event);
                }
                else
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Port[{PortNumber}] Direction Change(Input) Click");
                    Master.m_Ports[PortNumber].Interlock_AutoControlDirectionChange(Port.PortDirection.Input, Port.InterlockFrom.UI_Event);
                }
            }
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
    }
}
