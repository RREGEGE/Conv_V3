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
using RackMaster.SEQ.COMMON;

namespace RackMaster.SUBFORM.StatusPage {
    public partial class StatusPage_FullClosed : Form {
        public enum DgvBarcodeRow {
            PositionValue,
            VelocityValue,
            HardwareError,
            LaserStatus,
            Intensity,
            Temperature,
            Laser,
            Plausibility,
            VelocityMeasurementError,
        }

        private enum DgvStateColumn {
            Name,
            State,
        }

        private UICtrl.DataGridViewCtrl m_dgvCtrl;
        private RackMasterMain m_rackMaster;
        private RackMasterMain.RackMasterMotion m_motion;

        private bool m_isViewFullClosed = false;

        public StatusPage_FullClosed(RackMasterMain rackMaster) {
            m_rackMaster = rackMaster;
            m_motion = m_rackMaster.m_motion;
            m_dgvCtrl = new UICtrl.DataGridViewCtrl();

            InitializeComponent();

            InitDataGridView_Barcode();
            InitDataGridView_FullClosed();

            viewFullClosed.Click += ViewFullClosed_Click;
            gboxFullClosedCommand.Visible = false;
            gboxFullClosedFeedback.Visible = false;
        }

        private void InitDataGridView_Barcode() {
            try {
                foreach (DgvBarcodeRow row in Enum.GetValues(typeof(DgvBarcodeRow))) {
                    switch (row) {
                        case DgvBarcodeRow.PositionValue:
                            m_dgvCtrl.AddRow(ref dgvBarcodeStatus, "Position", (int)DgvStateColumn.Name);
                            break;

                        case DgvBarcodeRow.VelocityValue:
                            m_dgvCtrl.AddRow(ref dgvBarcodeStatus, "Velocity", (int)DgvStateColumn.Name);
                            break;

                        case DgvBarcodeRow.HardwareError:
                            m_dgvCtrl.AddRow(ref dgvBarcodeStatus, "Hardwre Error", (int)DgvStateColumn.Name);
                            break;

                        case DgvBarcodeRow.LaserStatus:
                            m_dgvCtrl.AddRow(ref dgvBarcodeStatus, "Laser Status", (int)DgvStateColumn.Name);
                            break;

                        case DgvBarcodeRow.Intensity:
                            m_dgvCtrl.AddRow(ref dgvBarcodeStatus, "Intensity", (int)DgvStateColumn.Name);
                            break;

                        case DgvBarcodeRow.Temperature:
                            m_dgvCtrl.AddRow(ref dgvBarcodeStatus, "Temperature", (int)DgvStateColumn.Name);
                            break;

                        case DgvBarcodeRow.Laser:
                            m_dgvCtrl.AddRow(ref dgvBarcodeStatus, "Laser", (int)DgvStateColumn.Name);
                            break;

                        case DgvBarcodeRow.Plausibility:
                            m_dgvCtrl.AddRow(ref dgvBarcodeStatus, "Plausibility", (int)DgvStateColumn.Name);
                            break;

                        case DgvBarcodeRow.VelocityMeasurementError:
                            m_dgvCtrl.AddRow(ref dgvBarcodeStatus, "Velocity Measurement Error", (int)DgvStateColumn.Name);
                            break;
                    }
                }

                foreach (DataGridViewColumn column in dgvBarcodeStatus.Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    column.ReadOnly = true;
                }
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, "Init Barcode DataGridView Fail", ex));
            }
        }

        private void InitDataGridView_FullClosed() {
            try {
                foreach (FullClosedStatus_Feedback state in Enum.GetValues(typeof(FullClosedStatus_Feedback))) {
                    switch (state) {
                        case FullClosedStatus_Feedback.OpState:
                            m_dgvCtrl.AddRow(ref dgvFullClosedFeedback, "OP State", (int)DgvStateColumn.Name);
                            break;

                        case FullClosedStatus_Feedback.TargetDistance:
                            m_dgvCtrl.AddRow(ref dgvFullClosedFeedback, "Target Distance", (int)DgvStateColumn.Name);
                            break;

                        case FullClosedStatus_Feedback.CommandDistance:
                            m_dgvCtrl.AddRow(ref dgvFullClosedFeedback, "Command Distance", (int)DgvStateColumn.Name);
                            break;

                        case FullClosedStatus_Feedback.ActualDistance:
                            m_dgvCtrl.AddRow(ref dgvFullClosedFeedback, "Actual Distance", (int)DgvStateColumn.Name);
                            break;

                        case FullClosedStatus_Feedback.RawDistance:
                            m_dgvCtrl.AddRow(ref dgvFullClosedFeedback, "Raw Distance", (int)DgvStateColumn.Name);
                            break;

                        case FullClosedStatus_Feedback.Torque:
                            m_dgvCtrl.AddRow(ref dgvFullClosedFeedback, "Torque", (int)DgvStateColumn.Name);
                            break;

                        case FullClosedStatus_Feedback.Velocity:
                            m_dgvCtrl.AddRow(ref dgvFullClosedFeedback, "Velocity", (int)DgvStateColumn.Name);
                            break;

                        case FullClosedStatus_Feedback.Pos:
                            m_dgvCtrl.AddRow(ref dgvFullClosedFeedback, "Position", (int)DgvStateColumn.Name);
                            break;

                        case FullClosedStatus_Feedback.AlarmCode:
                            m_dgvCtrl.AddRow(ref dgvFullClosedFeedback, "Alarm Code", (int)DgvStateColumn.Name);
                            break;
                    }
                }

                foreach (FullClosedStatus_Command state in Enum.GetValues(typeof(FullClosedStatus_Command))) {
                    switch (state) {
                        case FullClosedStatus_Command.TargetDistance:
                            m_dgvCtrl.AddRow(ref dgvFullClosedCommand, "Target Distance", (int)DgvStateColumn.Name);
                            break;

                        case FullClosedStatus_Command.Torque:
                            m_dgvCtrl.AddRow(ref dgvFullClosedCommand, "Torque", (int)DgvStateColumn.Name);
                            break;

                        case FullClosedStatus_Command.Velocity:
                            m_dgvCtrl.AddRow(ref dgvFullClosedCommand, "Velocity", (int)DgvStateColumn.Name);
                            break;

                        case FullClosedStatus_Command.Pos:
                            m_dgvCtrl.AddRow(ref dgvFullClosedCommand, "Position", (int)DgvStateColumn.Name);
                            break;
                    }
                }

                foreach (DataGridViewColumn column in dgvFullClosedFeedback.Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    column.ReadOnly = true;
                }

                foreach (DataGridViewColumn column in dgvFullClosedCommand.Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    column.ReadOnly = true;
                }
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, $"Init Full Closed Feedback DataGridView Fail", ex));
            }
        }

        public void UpdateFormUI() {
            if (m_isViewFullClosed) {
                UpdateDataGridView_FullClosed();
            }
            UpdateDataGridView_Barcode();
        }

        private void UpdateDataGridView_FullClosed() {
            foreach (FullClosedStatus_Feedback state in Enum.GetValues(typeof(FullClosedStatus_Feedback))) {
                switch (state) {
                    case FullClosedStatus_Feedback.OpState:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedFeedback, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_fbStatus.m_op}");
                        break;

                    case FullClosedStatus_Feedback.TargetDistance:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedFeedback, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_fbStatus.m_targetDistance:F3}");
                        break;

                    case FullClosedStatus_Feedback.CommandDistance:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedFeedback, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_fbStatus.m_commandDistance:F3}");
                        break;

                    case FullClosedStatus_Feedback.ActualDistance:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedFeedback, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_fbStatus.m_actualDistance:F3}");
                        break;

                    case FullClosedStatus_Feedback.RawDistance:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedFeedback, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_fbStatus.m_rawDistance:F3}");
                        break;

                    case FullClosedStatus_Feedback.Torque:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedFeedback, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_fbStatus.m_torque:F3}");
                        break;

                    case FullClosedStatus_Feedback.Velocity:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedFeedback, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_fbStatus.m_velocity:F3}");
                        break;

                    case FullClosedStatus_Feedback.Pos:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedFeedback, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_fbStatus.m_pos:F3}");
                        break;

                    case FullClosedStatus_Feedback.AlarmCode:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedFeedback, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_fbStatus.m_alarmCode}");
                        break;
                }
            }

            foreach (FullClosedStatus_Command state in Enum.GetValues(typeof(FullClosedStatus_Command))) {
                switch (state) {
                    case FullClosedStatus_Command.TargetDistance:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedCommand, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_cmdStatus.m_targetDistance}");
                        break;

                    case FullClosedStatus_Command.Torque:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedCommand, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_cmdStatus.m_torque}");
                        break;

                    case FullClosedStatus_Command.Velocity:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedCommand, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_cmdStatus.m_velocity}");
                        break;

                    case FullClosedStatus_Command.Pos:
                        m_dgvCtrl.SetCellText(ref dgvFullClosedCommand, (int)state, (int)DgvStateColumn.State, $"{m_motion.GetFullClosedStatus().m_cmdStatus.m_pos}");
                        break;
                }
            }
        }

        private void UpdateDataGridView_Barcode() {
            foreach (DgvBarcodeRow row in Enum.GetValues(typeof(DgvBarcodeRow))) {
                switch (row) {
                    case DgvBarcodeRow.PositionValue:
                        m_dgvCtrl.SetCellText(ref dgvBarcodeStatus, (int)row, (int)DgvStateColumn.State, $"{m_motion.GetDetectSensor_PositionlValue()}");
                        break;

                    case DgvBarcodeRow.VelocityValue:
                        m_dgvCtrl.SetCellText(ref dgvBarcodeStatus, (int)row, (int)DgvStateColumn.State, $"{m_motion.GetDetectSensor_VelocityValue()}");
                        break;

                    case DgvBarcodeRow.HardwareError:
                        m_dgvCtrl.SetErrorCell(ref dgvBarcodeStatus, (int)row, (int)DgvStateColumn.State, m_motion.GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus.HardwareError));
                        break;

                    case DgvBarcodeRow.LaserStatus:
                        m_dgvCtrl.SetErrorCell(ref dgvBarcodeStatus, (int)row, (int)DgvStateColumn.State, m_motion.GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus.LaserStatus));
                        break;

                    case DgvBarcodeRow.Intensity:
                        m_dgvCtrl.SetErrorCell(ref dgvBarcodeStatus, (int)row, (int)DgvStateColumn.State, m_motion.GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus.Intensity));
                        break;

                    case DgvBarcodeRow.Temperature:
                        m_dgvCtrl.SetErrorCell(ref dgvBarcodeStatus, (int)row, (int)DgvStateColumn.State, m_motion.GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus.Temperature));
                        break;

                    case DgvBarcodeRow.Laser:
                        m_dgvCtrl.SetErrorCell(ref dgvBarcodeStatus, (int)row, (int)DgvStateColumn.State, m_motion.GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus.Laser));
                        break;

                    case DgvBarcodeRow.Plausibility:
                        m_dgvCtrl.SetErrorCell(ref dgvBarcodeStatus, (int)row, (int)DgvStateColumn.State, m_motion.GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus.Plausibility));
                        break;

                    case DgvBarcodeRow.VelocityMeasurementError:
                        m_dgvCtrl.SetErrorCell(ref dgvBarcodeStatus, (int)row, (int)DgvStateColumn.State, m_motion.GetDetectSensor_VelocityStatus(DistanceDetectSensorVelocityStatus.MeasurementError));
                        break;
                }
            }
        }

        private void ViewFullClosed_Click(object sender, EventArgs e) {
            if (m_isViewFullClosed) {
                viewFullClosed.Text = "View Full Closed On";
                m_isViewFullClosed = false;
                gboxFullClosedFeedback.Visible = false;
                gboxFullClosedCommand.Visible = false;
            }
            else {
                viewFullClosed.Text = "View Full Closed Off";
                m_isViewFullClosed = true;
                gboxFullClosedFeedback.Visible = true;
                gboxFullClosedCommand.Visible = true;
            }
        }
    }
}
