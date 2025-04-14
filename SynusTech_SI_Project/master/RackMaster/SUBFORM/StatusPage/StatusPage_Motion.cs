using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RackMaster.SEQ.PART;

namespace RackMaster.SUBFORM.StatusPage {
    public partial class StatusPage_Motion : Form {
        private enum DgvCycleColumn {
            DataName,
            Data,
        }

        private enum DgvAxisDataColumn {
            DataName,
            Axis,
            Data
        }

        private UICtrl.DataGridViewCtrl m_dgvCtrl;
        private RackMasterMain.RackMasterData m_data;

        public StatusPage_Motion(RackMasterMain rackMaster) {
            m_dgvCtrl = new UICtrl.DataGridViewCtrl();
            m_data = rackMaster.m_data;

            InitializeComponent();

            InitDataGridView_CycleData();
            InitDataGridView_AxisData();
        }

        private void InitDataGridView_CycleData() {
            foreach(CycleDataList data in Enum.GetValues(typeof(CycleDataList))) {
                switch (data) {
                    case CycleDataList.CycleCount:
                        m_dgvCtrl.AddRow(ref dgvCycle, "Total Cycle Count", (int)DgvCycleColumn.DataName);
                        break;

                    case CycleDataList.AutoHomingCount:
                        m_dgvCtrl.AddRow(ref dgvCycle, "Auto Homing Count", (int)DgvCycleColumn.DataName);
                        break;

                    case CycleDataList.LastMode:
                        m_dgvCtrl.AddRow(ref dgvCycle, "Last Mode", (int)DgvCycleColumn.DataName);
                        break;

                    case CycleDataList.From_CycleTime:
                        m_dgvCtrl.AddRow(ref dgvCycle, "From Cycle Time(s)", (int)DgvCycleColumn.DataName);
                        break;

                    case CycleDataList.From_XZT_Move_Time:
                        m_dgvCtrl.AddRow(ref dgvCycle, "From XZT Move Time(s)", (int)DgvCycleColumn.DataName);
                        break;

                    case CycleDataList.From_ForkFWD_Time:
                        m_dgvCtrl.AddRow(ref dgvCycle, "From Fork FWD Move Time(s)", (int)DgvCycleColumn.DataName);
                        break;

                    case CycleDataList.From_Z_Up_Time:
                        m_dgvCtrl.AddRow(ref dgvCycle, "From Z Axis Up Time(s)", (int)DgvCycleColumn.DataName);
                        break;

                    case CycleDataList.From_ForkBWD_Time:
                        m_dgvCtrl.AddRow(ref dgvCycle, "From Fork BWD Move Time(s)", (int)DgvCycleColumn.DataName);
                        break;

                    case CycleDataList.To_CycleTime:
                        m_dgvCtrl.AddRow(ref dgvCycle, "To Cycle Time(s)", (int)DgvCycleColumn.DataName);
                        break;

                    case CycleDataList.To_XZT_Move_Time:
                        m_dgvCtrl.AddRow(ref dgvCycle, "To XZT Move Time(s)", (int)DgvCycleColumn.DataName);
                        break;

                    case CycleDataList.To_ForkFWD_Time:
                        m_dgvCtrl.AddRow(ref dgvCycle, "To Fork FWD Move Time(s)", (int)DgvCycleColumn.DataName);
                        break;

                    case CycleDataList.To_Z_Down_Time:
                        m_dgvCtrl.AddRow(ref dgvCycle, "To Z Axis Down Time(s)", (int)DgvCycleColumn.DataName);
                        break;

                    case CycleDataList.To_ForkBWD_Time:
                        m_dgvCtrl.AddRow(ref dgvCycle, "To Fork BWD Move Time(s)", (int)DgvCycleColumn.DataName);
                        break;
                }
            }

            foreach (DataGridViewColumn column in dgvCycle.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
        }

        private void InitDataGridView_AxisData() {
            foreach(AxisDataList data in Enum.GetValues(typeof(AxisDataList))) {
                int rowIdx = 0;

                switch (data) {
                    case AxisDataList.From_XZT_AverageTorque_X:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "From XZT Move Average Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "X");
                        break;

                    case AxisDataList.From_XZT_AverageTorque_Z:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "From XZT Move Average Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "Z");
                        break;

                    case AxisDataList.From_XZT_AverageTorque_T:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "From XZT Move Average Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "T");
                        break;

                    case AxisDataList.From_XZT_MaxTorque_X:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "From XZT Move Max Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "X");
                        break;

                    case AxisDataList.From_XZT_MaxTorque_Z:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "From XZT Move Max Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "Z");
                        break;

                    case AxisDataList.From_XZT_MaxTorque_T:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "From XZT Move Max Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "T");
                        break;

                    case AxisDataList.From_ForkFWD_AverageTorque_A:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "From Fork FWD Average Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "A");
                        break;

                    case AxisDataList.From_ForkFWD_MaxTorque_A:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "From Fork FWD Max Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "A");
                        break;

                    case AxisDataList.From_Z_Up_AverageTorque_Z:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "From Z Up Average Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "Z");
                        break;

                    case AxisDataList.From_Z_Up_MaxTorque_Z:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "From Z Up Max Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "Z");
                        break;

                    case AxisDataList.From_ForkBWD_AverageTorque_A:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "From Fork BWD Average Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "A");
                        break;

                    case AxisDataList.From_ForkBWD_MaxTorque_A:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "From Fork BWD Max Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "A");
                        break;

                    case AxisDataList.To_XZT_AverageTorque_X:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "To XZT Move Average Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "X");
                        break;

                    case AxisDataList.To_XZT_AverageTorque_Z:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "To XZT Move Average Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "Z");
                        break;

                    case AxisDataList.To_XZT_AverageTorque_T:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "To XZT Move Average Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "T");
                        break;

                    case AxisDataList.To_XZT_MaxTorque_X:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "To XZT Move Max Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "X");
                        break;

                    case AxisDataList.To_XZT_MaxTorque_Z:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "To XZT Move Max Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "Z");
                        break;

                    case AxisDataList.To_XZT_MaxTorque_T:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "To XZT Move Max Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "T");
                        break;

                    case AxisDataList.To_ForkFWD_AverageTorque_A:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "To Fork FWD Average Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "A");
                        break;

                    case AxisDataList.To_ForkFWD_MaxTorque_A:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "To Fork FWD Max Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "A");
                        break;

                    case AxisDataList.To_Z_Down_AverageTorque_Z:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "To Z Down Average Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "Z");
                        break;

                    case AxisDataList.To_Z_Down_MaxTorque_Z:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "To Z Down Max Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "Z");
                        break;

                    case AxisDataList.To_ForkBWD_AverageToruqe_A:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "To Fork BWD Average Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "A");
                        break;

                    case AxisDataList.To_ForkBWD_MaxTorque_A:
                        rowIdx = m_dgvCtrl.AddRow(ref dgvAxisData, "To Fork BWD Max Torque(%)", (int)DgvAxisDataColumn.DataName);
                        m_dgvCtrl.SetCellText(ref dgvAxisData, rowIdx, (int)DgvAxisDataColumn.Axis, "A");
                        break;
                }
            }

            foreach (DataGridViewColumn column in dgvAxisData.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
        }

        public void UpdateFormUI() {
            UpdateDataGridView_CycleData();
            UpdateDataGridView_AxisData();
        }

        private void UpdateDataGridView_CycleData() {
            foreach (CycleDataList data in Enum.GetValues(typeof(CycleDataList))) {
                switch (data) {
                    case CycleDataList.CycleCount:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().cycleCount}");
                        break;

                    case CycleDataList.AutoHomingCount:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().autoHomingCount}");
                        break;

                    case CycleDataList.LastMode:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().lastMode}");
                        break;

                    case CycleDataList.From_CycleTime:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().fromCycleTime / 1000.0:F3}");
                        break;

                    case CycleDataList.From_XZT_Move_Time:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().fromXZTMoveTime / 1000.0:F3}");
                        break;

                    case CycleDataList.From_ForkFWD_Time:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().fromForkFWDMoveTime / 1000.0:F3}");
                        break;

                    case CycleDataList.From_Z_Up_Time:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().fromZUpTime / 1000.0:F3}");
                        break;

                    case CycleDataList.From_ForkBWD_Time:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().fromForkBWDMoveTime / 1000.0:F3}");
                        break;

                    case CycleDataList.To_CycleTime:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().toCycleTime / 1000.0:F3}");
                        break;

                    case CycleDataList.To_XZT_Move_Time:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().toXZTMoveTime / 1000.0:F3}");
                        break;

                    case CycleDataList.To_ForkFWD_Time:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().toForkFWDMoveTime / 1000.0:F3}");
                        break;

                    case CycleDataList.To_Z_Down_Time:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().toZDownTime / 1000.0:F3}");
                        break;

                    case CycleDataList.To_ForkBWD_Time:
                        m_dgvCtrl.SetCellText(ref dgvCycle, (int)data, (int)DgvCycleColumn.Data, $"{m_data.GetRackMasterCycleData().toForkBWDMoveTime / 1000.0:F3}");
                        break;
                }
            }
        }

        private void UpdateDataGridView_AxisData() {
            foreach(AxisDataList data in Enum.GetValues(typeof(AxisDataList))) {
                switch (data) {
                    case AxisDataList.From_XZT_AverageTorque_X:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().fromXZT_AvrTorque_X:F1}");
                        break;

                    case AxisDataList.From_XZT_AverageTorque_Z:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().fromXZT_AvrTorque_Z:F1}");
                        break;

                    case AxisDataList.From_XZT_AverageTorque_T:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().fromXZT_AvrTorque_T:F1}");
                        break;

                    case AxisDataList.From_XZT_MaxTorque_X:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().fromXZT_MaxTorque_X:F1}");
                        break;

                    case AxisDataList.From_XZT_MaxTorque_Z:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().fromXZT_MaxTorque_Z:F1}");
                        break;

                    case AxisDataList.From_XZT_MaxTorque_T:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().fromXZT_MaxTorque_T:F1}");
                        break;

                    case AxisDataList.From_ForkFWD_AverageTorque_A:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().fromForkFWD_AvrTorque_A:F1}");
                        break;

                    case AxisDataList.From_ForkFWD_MaxTorque_A:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().fromForkFWD_MaxTorque_A:F1}");
                        break;

                    case AxisDataList.From_Z_Up_AverageTorque_Z:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().fromZUp_AvrTorque_Z:F1}");
                        break;

                    case AxisDataList.From_Z_Up_MaxTorque_Z:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().fromZUp_MaxTorque_Z:F1}");
                        break;

                    case AxisDataList.From_ForkBWD_AverageTorque_A:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().fromForkBWD_AvrTorque_A:F1}");
                        break;

                    case AxisDataList.From_ForkBWD_MaxTorque_A:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().fromForkBWD_MaxTorque_A:F1}");
                        break;

                    case AxisDataList.To_XZT_AverageTorque_X:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().toXZT_AvrTorque_X:F1}");
                        break;

                    case AxisDataList.To_XZT_AverageTorque_Z:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().toXZT_AvrTorque_Z:F1}");
                        break;

                    case AxisDataList.To_XZT_AverageTorque_T:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().toXZT_AvrTorque_T:F1}");
                        break;

                    case AxisDataList.To_XZT_MaxTorque_X:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().toXZT_MaxTorque_X:F1}");
                        break;

                    case AxisDataList.To_XZT_MaxTorque_Z:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().toXZT_MaxTorque_Z:F1}");
                        break;

                    case AxisDataList.To_XZT_MaxTorque_T:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().toXZT_MaxTorque_T:F1}");
                        break;

                    case AxisDataList.To_ForkFWD_AverageTorque_A:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().toForkFWD_AvrTorque_A:F1}");
                        break;

                    case AxisDataList.To_ForkFWD_MaxTorque_A:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().toForkFWD_MaxTorque_A:F1}");
                        break;

                    case AxisDataList.To_Z_Down_AverageTorque_Z:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().toZDown_AvrTorque_Z:F1}");
                        break;

                    case AxisDataList.To_Z_Down_MaxTorque_Z:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().toZDown_MaxTorque_Z:F1}");
                        break;

                    case AxisDataList.To_ForkBWD_AverageToruqe_A:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().toForkBWD_AvrTorque_A:F1}");
                        break;

                    case AxisDataList.To_ForkBWD_MaxTorque_A:
                        m_dgvCtrl.SetCellText(ref dgvAxisData, (int)data, (int)DgvAxisDataColumn.Data, $"{m_data.GetRackMasterAxisData().toForkBWD_MaxTorque_A:F1}");
                        break;
                }
            }
        }
    }
}
