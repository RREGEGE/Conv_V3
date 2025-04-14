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
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SUBFORM.SettingPage {
    public partial class SettingPage_Teaching : Form {
        private enum DgvStateColumn {
            Name,
            State,
        }

        private enum DgvShelfDataRow {
            ID,
            X_Data,
            Z_Data,
            A_Data,
            T_Data,
            Z_UpData,
            Z_DownData,
        }

        private enum DgvAutoTeachingRow {
            AutoTeachingStep,
            SensorFindStep,
            X_Position,
            X_Velocity,
            Z_Position,
            Z_Velocity,
            SensorDataX,
            SensorDataZ,
            Width,
            Height,
            SensorType,
            PickSensor,
            PlaceSensor,
        }

        private enum SaveType {
            OneShelf,
            T_AxisAll,
            A_AxisAll,
            Z_AxisUpDown,
            X_Z_Axis,
            Maint,
        }

        private int? m_portId = 0;
        private float? m_xPos = 0;
        private float? m_zPos = 0;
        private float? m_aPos = 0;
        private float? m_tPos = 0;
        private float? m_zUpPos = 0;
        private float? m_zdownPos = 0;

        private int? m_curPortIndex = 0;

        private int m_autoTeachingId = 0;
        private float m_autoTeachingTargetX = 0;
        private float m_autoTeachingTargetZ = 0;

        private const string MAINT_HP_CODE = "001";
        private const string MAINT_OP_CODE = "002";

        private RackMasterMain m_rackMaster;
        private RackMasterMain.RackMasterParam m_param;
        private RackMasterMain.RackMasterMotion m_motion;
        private RackMasterMain.TeachingData m_teaching;
        private UICtrl.DataGridViewCtrl m_dgvCtrl;
        private UICtrl.LabelCtrl m_labelCtrl;
        private UICtrl.ButtonCtrl m_btnCtrl;

        private SaveType m_saveType;

        private SEQ.CLS.Timer m_saveStopwatch;
        private SEQ.CLS.Timer m_autoTeachingStopwatch;

        public SettingPage_Teaching(RackMasterMain rackMaster) {
            InitializeComponent();

            m_rackMaster = rackMaster;
            m_teaching = m_rackMaster.m_teaching;
            m_param = m_rackMaster.m_param;
            m_motion = m_rackMaster.m_motion;
            m_dgvCtrl = new UICtrl.DataGridViewCtrl();
            m_labelCtrl = new UICtrl.LabelCtrl();
            m_btnCtrl = new UICtrl.ButtonCtrl();

            m_saveStopwatch = new SEQ.CLS.Timer();
            m_autoTeachingStopwatch = new SEQ.CLS.Timer();
            m_saveStopwatch.Stop();
            m_autoTeachingStopwatch.Stop();

            btnOneShelfTeaching.MouseDown       += SaveMouseDown;
            btnTAllTeacing.MouseDown            += SaveMouseDown;
            btnAAllTeaching.MouseDown           += SaveMouseDown;
            btnZUpDownTeaching.MouseDown        += SaveMouseDown;
            btnXZTeaching.MouseDown             += SaveMouseDown;
            btnMaintTeaching.MouseDown          += SaveMouseDown;

            btnOneShelfTeaching.MouseUp         += SaveMouseUp;
            btnTAllTeacing.MouseUp              += SaveMouseUp;
            btnAAllTeaching.MouseUp             += SaveMouseUp;
            btnZUpDownTeaching.MouseUp          += SaveMouseUp;
            btnXZTeaching.MouseUp               += SaveMouseUp;
            btnMaintTeaching.MouseUp            += SaveMouseUp;

            btnAutoTeachingStart.MouseDown      += AutoTeachingMouseDown;
            btnAutoTeachingStart.MouseUp        += AutoTeachingMouseUp;

            InitDataGridView_ShelfData();
            InitDataGridView_AutoTeachingStatus();
        }

        private void InitDataGridView_ShelfData() {
            foreach(DgvShelfDataRow row in Enum.GetValues(typeof(DgvShelfDataRow))) {
                switch (row) {
                    case DgvShelfDataRow.ID:
                        m_dgvCtrl.AddRow(ref dgvShelfData, "Shelf ID", (int)DgvStateColumn.Name);
                        break;

                    case DgvShelfDataRow.X_Data:
                        m_dgvCtrl.AddRow(ref dgvShelfData, "X Data(mm)", (int)DgvStateColumn.Name);
                        break;

                    case DgvShelfDataRow.Z_Data:
                        m_dgvCtrl.AddRow(ref dgvShelfData, "Z Data(mm)", (int)DgvStateColumn.Name);
                        break;

                    case DgvShelfDataRow.A_Data:
                        m_dgvCtrl.AddRow(ref dgvShelfData, "A Data(mm or deg)", (int)DgvStateColumn.Name);
                        break;

                    case DgvShelfDataRow.T_Data:
                        m_dgvCtrl.AddRow(ref dgvShelfData, "T Data(deg)", (int)DgvStateColumn.Name);
                        break;

                    case DgvShelfDataRow.Z_UpData:
                        m_dgvCtrl.AddRow(ref dgvShelfData, "Z UP Data(mm)", (int)DgvStateColumn.Name);
                        break;

                    case DgvShelfDataRow.Z_DownData:
                        m_dgvCtrl.AddRow(ref dgvShelfData, "Z Down Data(mm)", (int)DgvStateColumn.Name);
                        break;
                }
            }

            foreach(DataGridViewColumn column in dgvShelfData.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
        }

        private void InitDataGridView_AutoTeachingStatus() {
            foreach(DgvAutoTeachingRow row in Enum.GetValues(typeof(DgvAutoTeachingRow))) {
                switch (row) {
                    case DgvAutoTeachingRow.AutoTeachingStep:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "Auto Teaching Step", (int)DgvStateColumn.Name);
                        break;

                    case DgvAutoTeachingRow.SensorFindStep:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "Sensor Find Step", (int)DgvStateColumn.Name);
                        break;

                    case DgvAutoTeachingRow.X_Position:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "X Position(mm)", (int)DgvStateColumn.Name);
                        break;

                    case DgvAutoTeachingRow.X_Velocity:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "X Velocity(mm/s)", (int)DgvStateColumn.Name);
                        break;

                    case DgvAutoTeachingRow.Z_Position:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "Z Position(mm)", (int)DgvStateColumn.Name);
                        break;

                    case DgvAutoTeachingRow.Z_Velocity:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "Z Velocity(mm/s)", (int)DgvStateColumn.Name);
                        break;

                    case DgvAutoTeachingRow.SensorDataX:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "Sensor Data X(mm)", (int)DgvStateColumn.Name);
                        break;

                    case DgvAutoTeachingRow.SensorDataZ:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "Sensor Data Z(mm)", (int)DgvStateColumn.Name);
                        break;

                    case DgvAutoTeachingRow.Width:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "Width(mm)", (int)DgvStateColumn.Name);
                        break;

                    case DgvAutoTeachingRow.Height:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "Height(mm)", (int)DgvStateColumn.Name);
                        break;

                    case DgvAutoTeachingRow.SensorType:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "Sensor Type", (int)DgvStateColumn.Name);
                        break;

                    case DgvAutoTeachingRow.PickSensor:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "Pick Sensor", (int)DgvStateColumn.Name);
                        break;

                    case DgvAutoTeachingRow.PlaceSensor:
                        m_dgvCtrl.AddRow(ref dgvAutoTeachingStatus, "Place Sensor", (int)DgvStateColumn.Name);
                        break;
                }
            }
            foreach (DataGridViewColumn column in dgvAutoTeachingStatus.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
        }

        public void UpdateFormUI() {
            if (NullableFunction.TryParseNullable(txtID.Text, out m_portId)) {
                if (m_teaching.IsExistPortOrShelf((int)m_portId)) {
                    BlinkingPortIDError(false);

                    m_curPortIndex = m_teaching.GetTeachingDataIndex((int)m_portId);

                    if (m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valX != null)
                        m_xPos = m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valX / 1000;
                    else
                        m_xPos = 0;

                    if (m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valZ != null) {
                        if (m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                            m_zPos = m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valZ / 1000;
                        }
                        else {
                            m_zPos = (float)(m_rackMaster.m_motion.RadianToCalculateDistance((double)m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valZ) / 1000);
                        }
                    }
                    else
                        m_zPos = 0;

                    if (m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valFork_A != null)
                        m_aPos = m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valFork_A / 1000;
                    else
                        m_aPos = 0;

                    if (m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valT != null)
                        m_tPos = m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valT / 1000;
                    else
                        m_tPos = 0;

                    if (m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valZUp != null)
                        m_zUpPos = m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valZUp / 1000;
                    else
                        m_zUpPos = 0;

                    if (m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valZDown != null)
                        m_zdownPos = m_teaching.GetTeachingDataList()[(int)m_curPortIndex].valZDown / 1000;
                    else
                        m_zdownPos = 0;

                    txtTeachXPos.Text = $"{m_xPos:F3}";
                    txtTeachZPos.Text = $"{m_zPos:F3}";
                    txtTeachAPos.Text = $"{m_aPos:F3}";
                    txtTeachTPos.Text = $"{m_tPos:F3}";
                    txtTeachZUp.Text = $"{m_zUpPos:F3}";
                    txtTeachZDown.Text = $"{m_zdownPos:F3}";
                }
                else if (txtID.Text.Equals(MAINT_HP_CODE) || txtID.Text.Equals(MAINT_OP_CODE)) {
                    BlinkingPortIDError(false);
                }
                else {
                    BlinkingPortIDError(true);
                }
            }
            else {
                BlinkingPortIDError(true);
            }

            if(int.TryParse(txtAutoTeachingID.Text, out m_autoTeachingId)) {
                if (m_teaching.IsExistPortOrShelf((int)m_autoTeachingId)) {
                    BlinkingAutoTeachingIDError(false);
                    if (float.TryParse(txtAutoTeachingTargetX.Text, out m_autoTeachingTargetX) && float.TryParse(txtAutoTeachingTargetZ.Text, out m_autoTeachingTargetZ)) {
                        if (!m_rackMaster.IsAutoState()) {
                            if (m_motion.IsAutoTeachingRun()) {
                                m_btnCtrl.SetOnOffButtonStyle(ref btnAutoTeachingStart, true);
                                m_btnCtrl.EnabledButton(ref btnAutoTeachingStop, true);
                            }
                            else {
                                m_btnCtrl.EnabledButton(ref btnAutoTeachingStart, true);
                                m_btnCtrl.EnabledButton(ref btnAutoTeachingStop, true);
                            }
                        }
                        else {
                            m_btnCtrl.EnabledButton(ref btnAutoTeachingStart, false);
                            m_btnCtrl.EnabledButton(ref btnAutoTeachingStop, false);
                        }
                    }
                    else {
                        m_btnCtrl.EnabledButton(ref btnAutoTeachingStart, false);
                    }
                }
                else {
                    BlinkingAutoTeachingIDError(true);
                }
            }
            else {
                BlinkingAutoTeachingIDError(true);
            }

            if (m_param.GetMotionParam().useMaint) {
                m_btnCtrl.BlinkingButton(ref btnMaintTeaching, !m_teaching.IsMaintAllTeachingComplete());
            }
            else {
                m_btnCtrl.EnabledButton(ref btnMaintTeaching, false);
            }

            if (m_motion.IsAutoTeachingRun()) {
            txtAutoTeachingID.Enabled = false;
            txtAutoTeachingTargetX.Enabled = false;
            txtAutoTeachingTargetZ.Enabled = false;
            }
            else {
                txtAutoTeachingID.Enabled = true;
                txtAutoTeachingTargetX.Enabled = true;
                txtAutoTeachingTargetZ.Enabled = true;
            }

            txtCurXPos.Text = $"{(m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.X_Axis) / 1000):F3}";
            if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum) {
                txtCurZPos.Text = $"{(m_rackMaster.m_motion.GetDrumBeltZAxisActualPosition() / 1000):F3}";
            }
            else {
                txtCurZPos.Text = $"{(m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.Z_Axis) / 1000):F3}";
            }
            txtCurAPos.Text = $"{(m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.A_Axis) / 1000):F3}";
            if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                txtCurTPos.Text = $"{((m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.T_Axis) - m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.A_Axis)) / 1000):F3}";
            }else if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                txtCurTPos.Text = $"0";
            }
            else {
                txtCurTPos.Text = $"{(m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.T_Axis) / 1000):F3}";
            }

            UpdateDataGridView_ShelfData();
            UpdateDataGridView_AutoTeachingStep();
        }

        // Port ID가 존재하지 않으면 점등
        private void BlinkingPortIDError(bool isError) {
            if (isError) {
                ButtonEnabledTeachingPanel(false);
                m_labelCtrl.BlinkingLabel(ref lblTeachingID);

                txtTeachXPos.Text = "";
                txtTeachZPos.Text = "";
                txtTeachAPos.Text = "";
                txtTeachTPos.Text = "";
                txtTeachZUp.Text = "";
                txtTeachZDown.Text = "";
            }
            else {
                ButtonEnabledTeachingPanel(true);
                m_labelCtrl.BlinkingLabel(ref lblTeachingID, false);
            }
        }

        private void BlinkingAutoTeachingIDError(bool isError) {
            if (isError) {
                m_labelCtrl.BlinkingLabel(ref lblAutoTeachingIDError);
                m_btnCtrl.EnabledButton(ref btnAutoTeachingStart, false);
            }
            else {
                m_labelCtrl.BlinkingLabel(ref lblAutoTeachingIDError, false);
            }
        }

        private void ButtonEnabledTeachingPanel(bool enabled) {
            if (enabled) {
                btnAAllTeaching.Enabled = true;
                btnOneShelfTeaching.Enabled = true;
                btnTAllTeacing.Enabled = true;
                btnXZTeaching.Enabled = true;
                btnZUpDownTeaching.Enabled = true;
            }
            else {
                btnAAllTeaching.Enabled = false;
                btnOneShelfTeaching.Enabled = false;
                btnTAllTeacing.Enabled = false;
                btnXZTeaching.Enabled = false;
                btnZUpDownTeaching.Enabled = false;
            }
        }

        private void UpdateDataGridView_ShelfData() {
            if (m_teaching.IsExistPortOrShelf((int)m_autoTeachingId)) {
                TeachingValueData shelf = m_teaching.GetTeachingData((int)m_autoTeachingId);

                foreach(DgvShelfDataRow row in Enum.GetValues(typeof(DgvShelfDataRow))) {
                    switch (row) {
                        case DgvShelfDataRow.ID:
                            m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, $"{(int)m_autoTeachingId}");
                            break;

                        case DgvShelfDataRow.X_Data:
                            if (shelf.valX == null)
                                m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, "0");
                            else
                                m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, $"{(shelf.valX / 1000):F0}");
                            break;

                        case DgvShelfDataRow.Z_Data:
                            if(shelf.valZ == null)
                                m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, "0");
                            else {
                                if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum) {
                                    m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, $"{(m_motion.RadianToCalculateDistance((double)shelf.valZ)):F0}");
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, $"{(shelf.valZ / 1000):F0}");
                                }
                            }
                            break;

                        case DgvShelfDataRow.A_Data:
                            if(shelf.valFork_A == null)
                                m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, "0");
                            else
                                m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, $"{(shelf.valFork_A / 1000):F0}");
                            break;

                        case DgvShelfDataRow.T_Data:
                            if(shelf.valT == null)
                                m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, "0");
                            else
                                m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, $"{(shelf.valT / 1000):F0}");
                            break;

                        case DgvShelfDataRow.Z_UpData:
                            if(shelf.valZUp == null)
                                m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, "0");
                            else
                                m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, $"{(shelf.valZUp / 1000):F0}");
                            break;

                        case DgvShelfDataRow.Z_DownData:
                            if(shelf.valZDown == null)
                                m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, "0");
                            else
                                m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, $"{(shelf.valZDown / 1000):F0}");
                            break;
                    }
                }
            }
            else {
                foreach(DgvShelfDataRow row in Enum.GetValues(typeof(DgvShelfDataRow))) {
                    m_dgvCtrl.SetCellText(ref dgvShelfData, (int)row, (int)DgvStateColumn.State, "0");
                }
            }
        }

        private void UpdateDataGridView_AutoTeachingStep() {
            foreach(DgvAutoTeachingRow row in Enum.GetValues(typeof(DgvAutoTeachingRow))) {
                switch (row) {
                    case DgvAutoTeachingRow.AutoTeachingStep:
                        m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{m_motion.GetCurrentAutoTeachingStep()}");
                        break;

                    case DgvAutoTeachingRow.SensorFindStep:
                        m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{m_motion.GetCurrentSensorFindStep()}");
                        break;

                    case DgvAutoTeachingRow.X_Position:
                        m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{(m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.X_Axis) / 1000):F0}");
                        break;

                    case DgvAutoTeachingRow.X_Velocity:
                        m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{(m_motion.GetAxisStatus(AxisStatusType.vel_act, AxisList.X_Axis) / 1000):F0}");
                        break;

                    case DgvAutoTeachingRow.Z_Position:
                        if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum) {
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{(m_motion.RadianToCalculateDistance(m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.Z_Axis)) / 1000):F0}");
                        }
                        else {
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{(m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.Z_Axis) / 1000):F0}");
                        }
                        break;

                    case DgvAutoTeachingRow.Z_Velocity:
                        if (m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum) {
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{(m_motion.RadianToCalculateDistance(m_motion.GetAxisStatus(AxisStatusType.vel_act, AxisList.Z_Axis)) / 1000):F0}");
                        }
                        else {
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{(m_motion.GetAxisStatus(AxisStatusType.vel_act, AxisList.Z_Axis) / 1000):F0}");
                        }
                        break;

                    case DgvAutoTeachingRow.SensorDataX:
                        m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{(m_motion.GetCurrentSensorData().x_centerPos / 1000):F0}");
                        break;

                    case DgvAutoTeachingRow.SensorDataZ:
                        m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{(m_motion.GetCurrentSensorData().z_centerPos / 1000):F0}");
                        break;

                    case DgvAutoTeachingRow.Width:
                        m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{(m_motion.GetCurrentSensorData().width / 1000):F0}");
                        break;

                    case DgvAutoTeachingRow.Height:
                        m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{(m_motion.GetCurrentSensorData().height / 1000):F0}");
                        break;

                    case DgvAutoTeachingRow.SensorType:
                        m_dgvCtrl.SetCellText(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, $"{m_motion.GetAutoTeachingSensorType()}");
                        break;

                    case DgvAutoTeachingRow.PickSensor:
                        if (m_teaching.IsExistPortOrShelf(m_motion.GetAutoTeachingTargetID())) {
                            if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                                m_dgvCtrl.SetOnOffCell(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetInputBit(InputList.Fork_Pick_Sensor_Left));
                            }
                            else {
                                if (m_teaching.GetTeachingData(m_motion.GetAutoTeachingTargetID()).direction != null && m_teaching.GetTeachingData(m_motion.GetAutoTeachingTargetID()).direction == (int)PortDirection_HP.Left) {
                                    m_dgvCtrl.SetOnOffCell(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetInputBit(InputList.Fork_Pick_Sensor_Left));
                                }
                                else if (m_teaching.GetTeachingData(m_motion.GetAutoTeachingTargetID()).direction != null && m_teaching.GetTeachingData(m_motion.GetAutoTeachingTargetID()).direction == (int)PortDirection_HP.Right) {
                                    m_dgvCtrl.SetOnOffCell(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetInputBit(InputList.Fork_Pick_Sensor_Right));
                                }
                            }
                        }
                        else {
                            m_dgvCtrl.SetOnOffCell(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, false);
                        }
                        break;

                    case DgvAutoTeachingRow.PlaceSensor:
                        if (m_teaching.IsExistPortOrShelf(m_motion.GetAutoTeachingTargetID())) {
                            if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                                m_dgvCtrl.SetOnOffCell(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetInputBit(InputList.Fork_Place_Sensor_Left));
                            }
                            else {
                                if (m_teaching.GetTeachingData(m_motion.GetAutoTeachingTargetID()).direction != null && m_teaching.GetTeachingData(m_motion.GetAutoTeachingTargetID()).direction == (int)PortDirection_HP.Left) {
                                    m_dgvCtrl.SetOnOffCell(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetInputBit(InputList.Fork_Place_Sensor_Left));
                                }
                                else if (m_teaching.GetTeachingData(m_motion.GetAutoTeachingTargetID()).direction != null && m_teaching.GetTeachingData(m_motion.GetAutoTeachingTargetID()).direction == (int)PortDirection_HP.Right) {
                                    m_dgvCtrl.SetOnOffCell(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetInputBit(InputList.Fork_Place_Sensor_Right));
                                }
                            }
                        }
                        else {
                            m_dgvCtrl.SetOnOffCell(ref dgvAutoTeachingStatus, (int)row, (int)DgvStateColumn.State, false);
                        }
                        break;
                }
            }
        }

        private void SaveMouseDown(object sender, MouseEventArgs e) {
            Button btn = sender as Button;

            if(btn == btnOneShelfTeaching) {
                if (!CheckZUpDownTextValue()) {
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.TeachingZUpDownValueIsInvalid}"), $"{UI_MessageBoxTitleLangPackList.Warning}");
                    return;
                }

                m_saveType = SaveType.OneShelf;
            }else if(btn == btnTAllTeacing) {
                m_saveType = SaveType.T_AxisAll;
            }else if(btn == btnAAllTeaching) {
                m_saveType = SaveType.A_AxisAll;
            }else if(btn == btnZUpDownTeaching) {
                if (!CheckZUpDownTextValue()) {
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.TeachingZUpDownValueIsInvalid}"), $"{UI_MessageBoxTitleLangPackList.Warning}");
                    return;
                }

                m_saveType = SaveType.Z_AxisUpDown;
            }else if(btn == btnXZTeaching) {
                m_saveType = SaveType.X_Z_Axis;
            }else if(btn == btnMaintTeaching) {
                m_saveType = SaveType.Maint;
            }

            saveTimer.Start();
        }

        private void SaveMouseUp(object sender, MouseEventArgs e) {
            saveTimer.Stop();
            m_saveStopwatch.Stop();
        }

        private bool CheckZUpDownTextValue() {
            float zUpData = 0;
            float zDownData = 0;

            if (!float.TryParse(txtCurZUp.Text, out zUpData) || !float.TryParse(txtCurZDown.Text, out zDownData)) {
                return false;
            }

            return true;
        }

        private void SaveOneShelfTeaching() {
            float zUpData = 0;
            float zDownData = 0;

            if (!float.TryParse(txtCurZUp.Text, out zUpData) || !float.TryParse(txtCurZDown.Text, out zDownData)) {
                return;
            }

            float xData = (float)m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.X_Axis);
            float zData = (float)m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.Z_Axis);
            float forkAData = (float)m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.A_Axis);
            float forkTData = (float)m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.T_Axis);
            float tData = 0;
            if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                tData = forkTData - forkAData;
            }else if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                forkTData = 0;
                tData = 0;

                m_teaching.SetAllTurn((int)m_teaching.GetTeachingData((int)m_portId).direction, 0);
            }
            else {
                tData = (float)m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.T_Axis);
            }
            zUpData *= 1000;
            zDownData *= 1000;

            m_teaching.SetAllValue((int)m_portId, xData, tData, zData, forkAData, forkTData);

            m_curPortIndex = m_teaching.GetTeachingDataIndex((int)m_portId);
            m_teaching.SetValZ_UpDown((int)m_curPortIndex, zUpData, zDownData);
            m_teaching.SaveDataFile();

            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.TeachingDataSaveSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
            return;
        }

        private void SaveTAxisAllTeaching() {
            float forkAData = (float)m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.A_Axis);
            float forkTData = (float)m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.T_Axis);
            float tData = 0;
            if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                tData = forkTData - forkAData;
            }
            else {
                tData = (float)m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.T_Axis);
            }

            int direction = (int)m_teaching.GetTeachingDataList()[(int)m_curPortIndex].direction;

            m_teaching.SetAllTurn(direction, tData);
            m_teaching.SaveDataFile();

            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.TeachingDataSaveSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
            return;
        }

        private void SaveAAxisAllTeaching() {
            float forkAData = (float)m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.A_Axis);
            float forkTData = (float)m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.T_Axis);

            int direction = (int)m_teaching.GetTeachingDataList()[(int)m_curPortIndex].direction;

            m_teaching.SetAllFork(direction, forkAData, forkTData);
            m_teaching.SaveDataFile();

            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.TeachingDataSaveSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
            return;
        }

        private void SaveZUpDownTeaching() {
            float zUpData = 0;
            float zDownData = 0;

            if (!float.TryParse(txtCurZUp.Text, out zUpData) || !float.TryParse(txtCurZDown.Text, out zDownData)) {
                return;
            }
            zUpData *= 1000;
            zDownData *= 1000;

            m_teaching.SetAllValZ_UpDown(zUpData, zDownData);
            m_teaching.SaveDataFile();

            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.TeachingDataSaveSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
            return;
        }

        private void SaveXZAxisTeaching() {
            float xData = (float)m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.X_Axis);
            float zData = (float)m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.Z_Axis);

            m_teaching.SetValXZ((int)m_curPortIndex, xData, zData);
            m_teaching.SaveDataFile();

            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.TeachingDataSaveSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
            return;
        }

        private void SaveMaintTeaching() {
            float xData = (float)m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.X_Axis);
            float tData = (float)m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.T_Axis);

            if(txtID.Text.Equals(MAINT_HP_CODE))
                m_teaching.SetMaintTeaching(xData, tData, MaintTarget.HP);
            else if(txtID.Text.Equals(MAINT_OP_CODE))
                m_teaching.SetMaintTeaching(xData, tData, MaintTarget.OP);
            m_teaching.SaveDataFile();

            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.TeachingDataSaveSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
            return;
        }

        private void saveTimer_Tick(object sender, EventArgs e) {
            if (!m_saveStopwatch.IsTimerStarted())
                m_saveStopwatch.Restart();

            if (m_saveStopwatch.Delay(UICtrl.m_saveDelayTime)) {
                saveTimer.Stop();
                m_saveStopwatch.Stop();
                if (DialogResult.No == MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureSaveTeachingData}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.YesNo)) {
                    return;
                }

                switch (m_saveType) {
                    case SaveType.OneShelf:
                        SaveOneShelfTeaching();
                        break;

                    case SaveType.T_AxisAll:
                        SaveTAxisAllTeaching();
                        break;

                    case SaveType.A_AxisAll:
                        SaveAAxisAllTeaching();
                        break;

                    case SaveType.Z_AxisUpDown:
                        SaveZUpDownTeaching();
                        break;

                    case SaveType.X_Z_Axis:
                        SaveXZAxisTeaching();
                        break;

                    case SaveType.Maint:
                        SaveMaintTeaching();
                        break;
                }
            }
        }

        private bool CheckAutoTeachingData() {
            if (!int.TryParse(txtAutoTeachingID.Text, out m_autoTeachingId) || !float.TryParse(txtAutoTeachingTargetX.Text, out m_autoTeachingTargetX) ||
                !float.TryParse(txtAutoTeachingTargetZ.Text, out m_autoTeachingTargetZ))
                return false;

            return true;
        }

        private void btnAutoTeachingStop_Click(object sender, EventArgs e) {
            m_motion.AutoTeachingManualStop();
        }

        private void AutoTeachingMouseDown(object sender, MouseEventArgs e) {
            if (!CheckAutoTeachingData()) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ValidValueCheck}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            autoTeachingTimer.Start();
        }

        private void AutoTeachingMouseUp(object sender, MouseEventArgs e) {
            m_autoTeachingStopwatch.Stop();
            autoTeachingTimer.Stop();
        }

        private void autoTeachingTimer_Tick(object sender, EventArgs e) {
            if (!m_autoTeachingStopwatch.IsTimerStarted())
                m_autoTeachingStopwatch.Restart();

            if (m_autoTeachingStopwatch.Delay(UICtrl.m_saveDelayTime)) {
                autoTeachingTimer.Stop();
                m_autoTeachingStopwatch.Stop();

                if (DialogResult.No == MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureStartAutoTeaching}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.YesNo)) {
                    return;
                }

                m_motion.AutoTeachingManualStart(m_autoTeachingId, m_autoTeachingTargetX, m_autoTeachingTargetZ);
            }
        }

        public void AccessLoginType_User() {
            gboxAutoTeaching.Visible = false;
        }

        public void AccessLoginType_Admin() {
            gboxAutoTeaching.Visible = true;
        }
    }
}
