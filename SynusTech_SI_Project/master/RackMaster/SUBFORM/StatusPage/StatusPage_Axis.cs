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
    public partial class StatusPage_Axis : Form {
        private enum DgvStateColumn {
            Name,
            State,
        }

        private enum DgvAxisStatusRow {
            ServoOn,
            HomeDone,
            Alarm,
            AlarmCode,
            AxisStep,
            Run,
            CommandPosition,
            ActualPosition,
            CommandVelocity,
            ActualVelocity,
            ActualTorque,
            Inposition,
            Temperature,
        }

        private DataGridView[] m_dgvArray;
        private RackMasterMain m_main;
        private UICtrl.DataGridViewCtrl m_dgvCtrl;

        public StatusPage_Axis(RackMasterMain rackMaster) {
            InitializeComponent();

            m_main = rackMaster;
            m_dgvArray = new DataGridView[m_main.m_param.GetAxisCount()];
            m_dgvArray[(int)AxisList.X_Axis] = dgvXAxis;
            m_dgvArray[(int)AxisList.Z_Axis] = dgvZAxis;
            m_dgvArray[(int)AxisList.A_Axis] = dgvAAxis;
            m_dgvArray[(int)AxisList.T_Axis] = dgvTAxis;
            m_dgvCtrl = new UICtrl.DataGridViewCtrl();

            InitDataGridView_AxisStatus();
        }

        private void InitDataGridView_AxisStatus() {
            foreach(AxisList axis in Enum.GetValues(typeof(AxisList))) {
                foreach(DgvAxisStatusRow row in Enum.GetValues(typeof(DgvAxisStatusRow))) {
                    switch (row) {
                        case DgvAxisStatusRow.ServoOn:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Servo On", (int)DgvStateColumn.Name);
                            break;

                        case DgvAxisStatusRow.HomeDone:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Home Done", (int)DgvStateColumn.Name);
                            break;

                        case DgvAxisStatusRow.Alarm:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Alarm State", (int)DgvStateColumn.Name);
                            break;

                        case DgvAxisStatusRow.AlarmCode:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Alarm Code", (int)DgvStateColumn.Name);
                            break;

                        case DgvAxisStatusRow.AxisStep:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Axis Step", (int)DgvStateColumn.Name);
                            break;

                        case DgvAxisStatusRow.Run:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Run State", (int)DgvStateColumn.Name);
                            break;

                        case DgvAxisStatusRow.CommandPosition:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Command Position(mm or deg)", (int)DgvStateColumn.Name);
                            break;

                        case DgvAxisStatusRow.ActualPosition:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Actual Position(mm or deg)", (int)DgvStateColumn.Name);
                            break;

                        case DgvAxisStatusRow.CommandVelocity:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Command Velocity(mm/s or deg/s)", (int)DgvStateColumn.Name);
                            break;

                        case DgvAxisStatusRow.ActualVelocity:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Actual Velocity(mm/s or deg/s)", (int)DgvStateColumn.Name);
                            break;

                        case DgvAxisStatusRow.ActualTorque:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Actual Torque(%)", (int)DgvStateColumn.Name);
                            break;

                        case DgvAxisStatusRow.Inposition:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Inposition", (int)DgvStateColumn.Name);
                            break;

                        case DgvAxisStatusRow.Temperature:
                            m_dgvCtrl.AddRow(ref m_dgvArray[(int)axis], $"Temperature", (int)DgvStateColumn.Name);
                            break;
                    }
                }

                foreach(DataGridViewColumn column in m_dgvArray[(int)axis].Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    column.ReadOnly = true;
                }
            }
        }

        public void UpdateFormUI() {
            if(m_main.m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                if (gboxTAxis.Visible) {
                    gboxTAxis.Visible = false;
                }
            }
            else {
                if (!gboxTAxis.Visible) {
                    gboxTAxis.Visible = true;
                }
            }
            UpdateDataGridView_AxisStatus();
        }

        private void UpdateDataGridView_AxisStatus() {
            foreach(AxisList axis in Enum.GetValues(typeof(AxisList))) {
                if (m_main.m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn && axis == AxisList.T_Axis)
                    continue;

                foreach(DgvAxisStatusRow row in Enum.GetValues(typeof(DgvAxisStatusRow))) {
                    switch (row) {
                        case DgvAxisStatusRow.ServoOn:
                            m_dgvCtrl.SetOnOffCell(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, m_main.m_motion.GetAxisFlag(AxisFlagType.Servo_On, axis));
                            break;

                        case DgvAxisStatusRow.HomeDone:
                            m_dgvCtrl.SetOnOffCell(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, m_main.m_motion.GetAxisFlag(AxisFlagType.HomeDone, axis));
                            break;

                        case DgvAxisStatusRow.Alarm:
                            m_dgvCtrl.SetErrorCell(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, m_main.m_motion.GetAxisFlag(AxisFlagType.Alarm, axis));
                            break;

                        case DgvAxisStatusRow.AlarmCode:
                            m_dgvCtrl.SetErrorCell(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, m_main.m_motion.GetAxisFlag(AxisFlagType.Alarm, axis), true, $"{m_main.m_motion.GetAxisAlarmCode(axis):X}");
                            break;

                        case DgvAxisStatusRow.AxisStep:
                            m_dgvCtrl.SetCellText(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, $"{m_main.m_motion.GetAxisStep(axis)}");
                            break;

                        case DgvAxisStatusRow.Run:
                            m_dgvCtrl.SetOnOffCell(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, m_main.m_motion.GetAxisFlag(AxisFlagType.Run, axis));
                            break;

                        case DgvAxisStatusRow.CommandPosition:
                            if(m_main.m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum && axis == AxisList.Z_Axis) {
                                double positionCmd = m_main.m_motion.RadianToCalculateDistance(m_main.m_motion.GetAxisStatus(AxisStatusType.pos_cmd, axis));
                                m_dgvCtrl.SetCellText(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, $"{positionCmd:F0}");
                            }
                            else {
                                m_dgvCtrl.SetCellText(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, $"{(m_main.m_motion.GetAxisStatus(AxisStatusType.pos_cmd, axis) / 1000):F0}");
                            }
                            break;

                        case DgvAxisStatusRow.ActualPosition:
                            if(m_main.m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum && axis == AxisList.Z_Axis) {
                                double positionAct = m_main.m_motion.RadianToCalculateDistance(m_main.m_motion.GetAxisStatus(AxisStatusType.pos_act, axis));
                                m_dgvCtrl.SetCellText(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, $"{positionAct:F0}");
                            }
                            else {
                                m_dgvCtrl.SetCellText(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, $"{(m_main.m_motion.GetAxisStatus(AxisStatusType.pos_act, axis) / 1000):F0}");
                            }
                            break;

                        case DgvAxisStatusRow.CommandVelocity:
                            if(m_main.m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum && axis == AxisList.Z_Axis) {
                                double velocityCmd = m_main.m_motion.RadianToCalculateDistance(m_main.m_motion.GetAxisStatus(AxisStatusType.vel_cmd, axis));
                                m_dgvCtrl.SetCellText(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, $"{velocityCmd:F0}");
                            }
                            else {
                                m_dgvCtrl.SetCellText(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, $"{(m_main.m_motion.GetAxisStatus(AxisStatusType.vel_cmd, axis) / 1000):F0}");
                            }
                            break;

                        case DgvAxisStatusRow.ActualVelocity:
                            if(m_main.m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum && axis == AxisList.Z_Axis) {
                                double velocityAct = m_main.m_motion.RadianToCalculateDistance(m_main.m_motion.GetAxisStatus(AxisStatusType.vel_act, axis));
                                m_dgvCtrl.SetCellText(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, $"{velocityAct:F0}");
                            }
                            else {
                                m_dgvCtrl.SetCellText(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, $"{(m_main.m_motion.GetAxisStatus(AxisStatusType.vel_act, axis) / 1000):F0}");
                            }
                            break;

                        case DgvAxisStatusRow.ActualTorque:
                            m_dgvCtrl.SetCellText(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, $"{m_main.m_motion.GetAxisStatus(AxisStatusType.trq_act, axis):F1}");
                            break;

                        case DgvAxisStatusRow.Inposition:
                            m_dgvCtrl.SetOnOffCell(ref m_dgvArray[(int)axis], (int)row, (int)DgvStateColumn.State, m_main.m_motion.GetAxisFlag(AxisFlagType.Poset, axis));
                            break;
                    }
                }
            }
        }

        public void UpdateAmpTemperature(AxisList axis, int temp) {
            switch (axis) {
                case AxisList.X_Axis:
                    m_dgvCtrl.SetCellText(ref dgvXAxis, (int)DgvAxisStatusRow.Temperature, (int)DgvStateColumn.State, $"{temp}");
                    break;

                case AxisList.Z_Axis:
                    m_dgvCtrl.SetCellText(ref dgvZAxis, (int)DgvAxisStatusRow.Temperature, (int)DgvStateColumn.State, $"{temp}");
                    break;

                case AxisList.A_Axis:
                    m_dgvCtrl.SetCellText(ref dgvAAxis, (int)DgvAxisStatusRow.Temperature, (int)DgvStateColumn.State, $"{temp}");
                    break;

                case AxisList.T_Axis:
                    m_dgvCtrl.SetCellText(ref dgvTAxis, (int)DgvAxisStatusRow.Temperature, (int)DgvStateColumn.State, $"{temp}");
                    break;
            }
        }
    }
}
