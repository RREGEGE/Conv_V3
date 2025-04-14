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


namespace Master.SubForm.PortTPForm.PortTPSubForm
{
    public partial class Frm_PortAlarmInfo : Form
    {
        public Frm_PortAlarmInfo()
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

                GroupBoxFunc.SetText(groupBox_Alarmnfo, SynusLangPack.GetLanguage("GroupBox_Alarmnfo"));
                GroupBoxFunc.SetText(groupBox_PortAlarmList, SynusLangPack.GetLanguage("GroupBox_PortAlarmList"));
                GroupBoxFunc.SetText(groupBox_PortAlarmState, SynusLangPack.GetLanguage("GroupBox_PortAlarmState"));
                GroupBoxFunc.SetText(groupBox_PortAlarmSolution, SynusLangPack.GetLanguage("GroupBox_PortAlarmSolution"));

                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort != null)
                {
                    CurrentPort.Update_DGV_AlarmList(ref DGV_PortAlarmList);
                    CurrentPort.Update_DGV_ErrorInfo(ref DGV_ErrorInfo);

                    DataGridViewCell CurrentCell = DGV_PortAlarmList.CurrentCell;
                    int SelectedRowIndex = CurrentCell == null ? -1 : DGV_PortAlarmList.CurrentCell.RowIndex;

                    if (SelectedRowIndex > 0)
                    {
                        short AlarmCode = Convert.ToInt16(DGV_PortAlarmList.Rows[SelectedRowIndex].Cells[0].Value);
                        Port.PortAlarm ePortAlarm = (Port.PortAlarm)AlarmCode;
                        string AlarmSolution = CurrentPort.GetAlarmSolutionText(ePortAlarm);

                        if (richTextBox_Solution.Text != AlarmSolution)
                            richTextBox_Solution.Text = AlarmSolution;
                    }
                    else
                        richTextBox_Solution.Clear();

                    if (CurrentCell == null && DGV_PortAlarmList.Rows.Count > 0)
                        DGV_PortAlarmList.CurrentCell = DGV_PortAlarmList.Rows[0].Cells[2];
                }
                else
                {
                    if (DGV_PortAlarmList.Rows.Count > 0)
                        DGV_PortAlarmList.Rows.Clear();

                    if (richTextBox_Solution.Text != string.Empty)
                        richTextBox_Solution.Clear();
                }
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
                    Port.DGV_AlarmListColumn eDGV_AlarmListColumn = (Port.DGV_AlarmListColumn)ColumnIndex;

                    if (eDGV_AlarmListColumn == Port.DGV_AlarmListColumn.AlarmName)
                    {
                        DataGridView grid = sender as DataGridView;
                        ContextMenu cm = new ContextMenu();

                        MenuItem item1 = new MenuItem();
                        item1.Name = "Alarm Manual Create";
                        item1.Text = "Alarm Manual Create";
                        item1.Tag = Convert.ToInt16(grid.Rows[e.RowIndex].Cells[(int)Port.DGV_AlarmListColumn.Index].Value);
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
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                MenuItem item = (MenuItem)sender;
                short AlarmCode = Convert.ToInt16(item.Tag);

                CurrentPort.Interlock_AlarmCreate(AlarmCode, Port.InterlockFrom.UI_Event);
            }
            catch
            {

            }
        }
    }
}
