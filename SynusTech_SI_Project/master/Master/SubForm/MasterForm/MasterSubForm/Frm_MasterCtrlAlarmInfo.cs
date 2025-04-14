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
    public partial class Frm_MasterCtrlAlarmInfo : Form
    {
        public Frm_MasterCtrlAlarmInfo()
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

                GroupBoxFunc.SetText(groupBox_Alarmnfo, SynusLangPack.GetLanguage("GroupBox_Alarmnfo"));
                GroupBoxFunc.SetText(groupBox_MasterAlarmList, SynusLangPack.GetLanguage("GroupBox_MasterAlarmList"));
                GroupBoxFunc.SetText(groupBox_MasterAlarmState, SynusLangPack.GetLanguage("GroupBox_MasterAlarmState"));
                GroupBoxFunc.SetText(groupBox_MasterAlarmSolution, SynusLangPack.GetLanguage("GroupBox_MasterAlarmSolution"));

                Master.Update_DGV_AlarmList(ref DGV_PortAlarmList);
                Master.Update_DGV_ErrorInfo(ref DGV_ErrorInfo);

                DataGridViewCell CurrentCell = DGV_PortAlarmList.CurrentCell;
                int SelectedRowIndex = CurrentCell == null ? -1 : DGV_PortAlarmList.CurrentCell.RowIndex;

                if (SelectedRowIndex > 0)
                {
                    short AlarmCode = Convert.ToInt16(DGV_PortAlarmList.Rows[SelectedRowIndex].Cells[0].Value);
                    Master.MasterAlarm eMasterAlarm = (Master.MasterAlarm)AlarmCode;
                    string AlarmSolution = Master.GetAlarmSolutionText(eMasterAlarm);

                    if (richTextBox_Solution.Text != AlarmSolution)
                        richTextBox_Solution.Text = AlarmSolution;
                }
                else
                    richTextBox_Solution.Clear();

                if (CurrentCell == null && DGV_PortAlarmList.Rows.Count > 0)
                    DGV_PortAlarmList.CurrentCell = DGV_PortAlarmList.Rows[0].Cells[2];

                
            }
            catch { }
            finally
            {

            }
        }

        private void DGV_PortAlarmList_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex < 0)
                        return;

                    int ColumnIndex = e.ColumnIndex;
                    Master.DGV_AlarmListColumn eDGV_AlarmListColumn = (Master.DGV_AlarmListColumn)ColumnIndex;

                    if (eDGV_AlarmListColumn == Master.DGV_AlarmListColumn.AlarmName)
                    {
                        DataGridView grid = sender as DataGridView;
                        ContextMenu cm = new ContextMenu();

                        MenuItem item1 = new MenuItem();
                        item1.Name = "Alarm Manual Create";
                        item1.Text = "Alarm Manual Create";
                        item1.Tag = Convert.ToInt16(grid.Rows[e.RowIndex].Cells[(int)Master.DGV_AlarmListColumn.Index].Value);
                        item1.Click += btn_AlarmManualCreate_Click;
                        cm.MenuItems.Add(item1);

                        Point pt = grid.PointToClient(Control.MousePosition);
                        cm.Show(grid, pt);
                    }
                }
            }
            catch
            {

            }
        }

        private void btn_AlarmManualCreate_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var rackMaster in Master.m_RackMasters)
                {
                    if (rackMaster.Value.Status_AutoMode ||
                        rackMaster.Value.IsAutoCycleRun() ||
                        rackMaster.Value.IsAutoTeachingRun())
                    {
                        MessageBox.Show($"RackMaster ID = [{rackMaster.Value.GetParam().ID}]" + SynusLangPack.GetLanguage("Message_RejectCommandInWorking"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                foreach (var port in Master.m_Ports)
                {
                    if (port.Value.IsAutoControlRun() ||
                        port.Value.IsAutoManualCycleRun() ||
                        port.Value.IsPortBusy())
                    {
                        MessageBox.Show($"Port ID = [{port.Value.GetParam().ID}]" + SynusLangPack.GetLanguage("Message_RejectCommandInWorking"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }


                MenuItem item = (MenuItem)sender;
                short AlarmCode = Convert.ToInt16(item.Tag);

                Master.AlarmInsert((Master.MasterAlarm)AlarmCode);
            }
            catch
            {

            }
        }
    }
}
