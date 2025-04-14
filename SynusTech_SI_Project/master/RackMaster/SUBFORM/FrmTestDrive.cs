using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RackMaster.SEQ;
using RackMaster.SEQ.PART;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SUBFORM {
    public partial class FrmTestDrive : Form {
        private enum DgvSettingValueColumn {
            Setting,
            X_Axis,
            Z_Axis,
            A_Axis,
            T_Axis,
        }

        private enum DgvSettingValueRow {
            ManualHighSpeed,
            ManualHighAccDec,
            ManualLowSpeed,
            ManualLowAccDec,
        }

        private enum DgvStatusColumn {
            Name,
            State,
        }

        private enum DgvServoColumn {
            AxisName,
            ServoOn,
            HomeDone,
            Position,
            Velocity,
            Torque,
        }

        private enum DgvTestDriveStatusRow {
            Step,
            Error,
            SourceEmpty,
            ManualAutoCycleStep,
            ManualAutoCycleCount,
            MST_EMO,
            Z_Axis_MaintStopper,
            CST_Abnormal,
        }

        private enum DgvServoRow {
            X_Axis,
            Z_Axis,
            A_Axis,
            T_Axis,
        }

        private enum DgvIoRow {
            ForkPresenceSensor_1,
            ForkPresenceSensor_2,
            ForkDoubleStorageSensor_1,
            ForkDoubleStorageSensor_2,
            ForkPickSensorLeft,
            ForkPickSensorRight,
            ForkPlaceSensorLeft,
            ForkPlaceSensorRight,
            ForkInPlace_1,
            ForkInPlace_2,
            StickDetectSensor_1,
            StickDetectSensor_2,
            StickDetectSensor_3,
            StickDetectSensor_4,
        }

        private RackMasterMain m_rackMaster;
        private RackMasterMain.RackMasterMotion m_motion;
        private RackMasterMain.TeachingData m_teaching;
        private UICtrl.LabelCtrl m_lblCtrl;
        private UICtrl.DataGridViewCtrl m_dgvCtrl;
        private UICtrl.ButtonCtrl m_btnCtrl;

        private int m_currentId;

        public FrmTestDrive(RackMasterMain rackMaster) {
            InitializeComponent();

            // Auto Cycle
            gboxAutoCycle.Visible = false;

            m_rackMaster = rackMaster;
            m_motion = m_rackMaster.m_motion;
            m_teaching = m_rackMaster.m_teaching;
            m_lblCtrl = new UICtrl.LabelCtrl();
            m_dgvCtrl = new UICtrl.DataGridViewCtrl();
            m_btnCtrl = new UICtrl.ButtonCtrl();

            m_currentId = 0;

            InitDataGridView_Setting();
            InitDataGridView_IO();
            InitDataGridView_TD();
            InitDataGridView_Servo();

            btnFromRun.MouseDown    += ManualRun_MouseDown;
            btnToRun.MouseDown      += ManualRun_MouseDown;

            btnFromRun.MouseUp      += ManualRun_MouseUp;
            btnToRun.MouseUp        += ManualRun_MouseUp;

            //LanguageChanged();

            this.VisibleChanged     += ThisFormVisibleChangedEvent;
        }

        private void ThisFormVisibleChangedEvent(object sender, EventArgs e) {
            m_rackMaster.m_motion.ResetManualRun();
        }

        private void InitDataGridView_Setting() {
            foreach(DgvSettingValueColumn col in Enum.GetValues(typeof(DgvSettingValueColumn))) {
                switch (col) {
                    case DgvSettingValueColumn.Setting:
                        m_dgvCtrl.AddColumn(ref dgvSettingValue, $"{col}", "Setting");
                        break;

                    case DgvSettingValueColumn.X_Axis:
                        m_dgvCtrl.AddColumn(ref dgvSettingValue, $"{col}", "X Axis");
                        break;

                    case DgvSettingValueColumn.Z_Axis:
                        m_dgvCtrl.AddColumn(ref dgvSettingValue, $"{col}", "Z Axis");
                        break;

                    case DgvSettingValueColumn.A_Axis:
                        m_dgvCtrl.AddColumn(ref dgvSettingValue, $"{col}", "A Axis");
                        break;

                    case DgvSettingValueColumn.T_Axis:
                        m_dgvCtrl.AddColumn(ref dgvSettingValue, $"{col}", "T Axis");
                        break;
                }
            }

            foreach(DgvSettingValueRow row in Enum.GetValues(typeof(DgvSettingValueRow))) {
                switch (row) {
                    case DgvSettingValueRow.ManualHighSpeed:
                        m_dgvCtrl.AddRow(ref dgvSettingValue, $"Manual High Speed(u/min)", (int)DgvSettingValueColumn.Setting);
                        break;

                    case DgvSettingValueRow.ManualHighAccDec:
                        m_dgvCtrl.AddRow(ref dgvSettingValue, $"Manual High Acc/Dec(s)", (int)DgvSettingValueColumn.Setting);
                        break;

                    case DgvSettingValueRow.ManualLowSpeed:
                        m_dgvCtrl.AddRow(ref dgvSettingValue, $"Manual Low Speed(u/min)", (int)DgvSettingValueColumn.Setting);
                        break;

                    case DgvSettingValueRow.ManualLowAccDec:
                        m_dgvCtrl.AddRow(ref dgvSettingValue, $"Manual Low Acc/Dec(s)", (int)DgvSettingValueColumn.Setting);
                        break;
                }
            }
            m_dgvCtrl.DisableUserControl(ref dgvSettingValue);
            m_dgvCtrl.SetReadonly(ref dgvSettingValue);
            foreach (DataGridViewColumn column in dgvSettingValue.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dgvSettingValue.Columns[(int)DgvSettingValueColumn.Setting].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void InitDataGridView_IO() {
            m_dgvCtrl.DisableUserControl(ref dgvIOStatus);

            foreach(DgvIoRow row in Enum.GetValues(typeof(DgvIoRow))) {
                switch (row) {
                    case DgvIoRow.ForkPresenceSensor_1:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Presence Sensor 1", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.ForkPresenceSensor_2:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Presence Sensor 2", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.ForkDoubleStorageSensor_1:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Double Storage Sensor 1", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.ForkDoubleStorageSensor_2:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Double Storage Sensor 2", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.ForkPickSensorLeft:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Pick Sensor Left", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.ForkPickSensorRight:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Pick Sensor Right", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.ForkPlaceSensorLeft:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Place Sensor Left", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.ForkPlaceSensorRight:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Place Sensor Right", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.ForkInPlace_1:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Placement Sensor 1", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.ForkInPlace_2:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Placement Sensor 2", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.StickDetectSensor_1:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Stick Detect Sensor 1", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.StickDetectSensor_2:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Stick Detect Sensor 2", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.StickDetectSensor_3:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Stick Detect Sensor 3", (int)DgvStatusColumn.Name);
                        break;

                    case DgvIoRow.StickDetectSensor_4:
                        m_dgvCtrl.AddRow(ref dgvIOStatus, $"Stick Detect Sensor 4", (int)DgvStatusColumn.Name);
                        break;
                }
            }
            m_dgvCtrl.DisableUserControl(ref dgvIOStatus);
            m_dgvCtrl.SetReadonly(ref dgvIOStatus);
            foreach (DataGridViewColumn column in dgvIOStatus.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void InitDataGridView_TD() {
            foreach(DgvTestDriveStatusRow row in Enum.GetValues(typeof(DgvTestDriveStatusRow))) {
                switch (row) {
                    case DgvTestDriveStatusRow.Step:
                        m_dgvCtrl.AddRow(ref dgvTDStatus, "Step", (int)DgvStatusColumn.Name);
                        break;

                    case DgvTestDriveStatusRow.Error:
                        m_dgvCtrl.AddRow(ref dgvTDStatus, "Error", (int)DgvStatusColumn.Name);
                        break;

                    case DgvTestDriveStatusRow.SourceEmpty:
                        m_dgvCtrl.AddRow(ref dgvTDStatus, "Source Empty", (int)DgvStatusColumn.Name);
                        break;

                    case DgvTestDriveStatusRow.ManualAutoCycleStep:
                        m_dgvCtrl.AddRow(ref dgvTDStatus, "Manual Auto Cycle Step", (int)DgvStatusColumn.Name);
                        break;

                    case DgvTestDriveStatusRow.ManualAutoCycleCount:
                        m_dgvCtrl.AddRow(ref dgvTDStatus, "Manual Auto Cycle Count", (int)DgvStatusColumn.Name);
                        break;

                    case DgvTestDriveStatusRow.MST_EMO:
                        m_dgvCtrl.AddRow(ref dgvTDStatus, "Master EMO", (int)DgvStatusColumn.Name);
                        break;

                    case DgvTestDriveStatusRow.Z_Axis_MaintStopper:
                        m_dgvCtrl.AddRow(ref dgvTDStatus, "Z Axis Maint Stopper", (int)DgvStatusColumn.Name);
                        break;

                    case DgvTestDriveStatusRow.CST_Abnormal:
                        m_dgvCtrl.AddRow(ref dgvTDStatus, "CST Abnormal", (int)DgvStatusColumn.Name);
                        break;
                }
            }

            m_dgvCtrl.DisableUserControl(ref dgvTDStatus);
            m_dgvCtrl.SetReadonly(ref dgvTDStatus);
            foreach (DataGridViewColumn column in dgvTDStatus.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void InitDataGridView_Servo() {
            m_dgvCtrl.DisableUserControl(ref dgvServoStatus);

            foreach(DgvServoColumn col in Enum.GetValues(typeof(DgvServoColumn))) {
                switch (col) {
                    case DgvServoColumn.AxisName:
                        m_dgvCtrl.AddColumn(ref dgvServoStatus, $"{col}", $"Axis Name");
                        break;

                    case DgvServoColumn.ServoOn:
                        m_dgvCtrl.AddColumn(ref dgvServoStatus, $"{col}", $"Servo On");
                        break;

                    case DgvServoColumn.HomeDone:
                        m_dgvCtrl.AddColumn(ref dgvServoStatus, $"{col}", $"Home Done");
                        break;

                    case DgvServoColumn.Position:
                        m_dgvCtrl.AddColumn(ref dgvServoStatus, $"{col}", $"Position(mm)");
                        break;

                    case DgvServoColumn.Velocity:
                        m_dgvCtrl.AddColumn(ref dgvServoStatus, $"{col}", $"Velocity(mm/min)");
                        break;

                    case DgvServoColumn.Torque:
                        m_dgvCtrl.AddColumn(ref dgvServoStatus, $"{col}", $"Torque(%)");
                        break;
                }
            }

            foreach(DgvServoRow row in Enum.GetValues(typeof(DgvServoRow))) {
                switch (row) {
                    case DgvServoRow.X_Axis:
                        m_dgvCtrl.AddRow(ref dgvServoStatus, $"X-Axis", (int)DgvServoColumn.AxisName);
                        break;

                    case DgvServoRow.Z_Axis:
                        m_dgvCtrl.AddRow(ref dgvServoStatus, $"Z-Axis", (int)DgvServoColumn.AxisName);
                        break;

                    case DgvServoRow.A_Axis:
                        m_dgvCtrl.AddRow(ref dgvServoStatus, $"A-Axis", (int)DgvServoColumn.AxisName);
                        break;

                    case DgvServoRow.T_Axis:
                        m_dgvCtrl.AddRow(ref dgvServoStatus, $"T-Axis", (int)DgvServoColumn.AxisName);
                        break;
                }
            }
            m_dgvCtrl.DisableUserControl(ref dgvServoStatus);
            m_dgvCtrl.SetReadonly(ref dgvServoStatus);
            foreach (DataGridViewColumn column in dgvServoStatus.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void UpdateFormUI() {
            dgvServoStatus.ClearSelection();
            dgvIOStatus.ClearSelection();
            dgvSettingValue.ClearSelection();
            dgvTDStatus.ClearSelection();

            UpdateServoStatus();
            UpdateIOStatus();
            UpdateSettingValue();
            UpdateTestDriveStatus();

            if (int.TryParse(txtID.Text, out m_currentId)) {
                if(!m_teaching.IsEnablePort(m_currentId) || !m_teaching.IsExistPortOrShelf(m_currentId) || !m_teaching.IsExistTeachingData(m_currentId)) {
                    m_lblCtrl.BlinkingLabel(ref lblIDError);
                    btnFromRun.Enabled = false;
                    btnToRun.Enabled = false;
                }else if (m_rackMaster.m_alarm.IsAlarmState() && !m_rackMaster.Interlock_OnlyMSTDisconnectedAlarmOccurred()) {
                    m_lblCtrl.SetErrorLabelStyle(ref lblIDError, false);
                    btnFromRun.Enabled = false;
                    btnToRun.Enabled = false;
                }else if (m_rackMaster.GetReceiveBit(ReceiveBitMap.MST_Emo_Request)) {
                    m_lblCtrl.SetErrorLabelStyle(ref lblIDError, false);
                    btnFromRun.Enabled = false;
                    btnToRun.Enabled = false;
                }else if(!m_rackMaster.Interlock_ZAxisMaintStopper()) {
                    m_lblCtrl.SetErrorLabelStyle(ref lblIDError, false);
                    btnFromRun.Enabled = false;
                    btnToRun.Enabled = false;
                }
                else {
                    m_lblCtrl.SetErrorLabelStyle(ref lblIDError, false);
                    //if (m_rackMaster.IsCassetteAbnormal() && m_rackMaster.m_motion.GetCurerntManualStep() == ManualStep.Idle) {
                    //    btnFromRun.Enabled = false;
                    //    btnToRun.Enabled = false;
                    //}
                    //else {
                    //    if (m_rackMaster.m_motion.GetCurerntManualStep() == ManualStep.Idle) {
                    //        if (m_rackMaster.IsCassetteOn()) {
                    //            btnFromRun.Enabled = false;
                    //            btnToRun.Enabled = true;
                    //        }
                    //        else if(m_rackMaster.IsCassetteEmpty()) {
                    //            btnFromRun.Enabled = true;
                    //            btnToRun.Enabled = false;
                    //        }
                    //    }
                    //    else if (m_rackMaster.m_motion.GetCurerntManualStep() >= ManualStep.From_XZTGoing && m_rackMaster.m_motion.GetCurerntManualStep() <= ManualStep.From_ForkBWD) {
                    //        btnFromRun.Enabled = true;
                    //        btnToRun.Enabled = false;
                    //    }
                    //    else if (m_rackMaster.m_motion.GetCurerntManualStep() >= ManualStep.To_XZTGoing && m_rackMaster.m_motion.GetCurerntManualStep() <= ManualStep.To_ForkBWD) {
                    //        btnFromRun.Enabled = false;
                    //        btnToRun.Enabled = true;
                    //    }
                    //}
                    if (m_rackMaster.m_motion.GetCurerntManualStep() == ManualStep.Idle) {
                        if (m_rackMaster.IsCassetteOn()) {
                            btnFromRun.Enabled = false;
                            btnToRun.Enabled = true;
                        }
                        else if (m_rackMaster.IsCassetteEmpty()) {
                            btnFromRun.Enabled = true;
                            btnToRun.Enabled = false;
                        }
                        else {
                            btnFromRun.Enabled = false;
                            btnToRun.Enabled = false;
                        }
                    }
                    else if (m_rackMaster.m_motion.GetCurerntManualStep() >= ManualStep.From_XZTGoing && m_rackMaster.m_motion.GetCurerntManualStep() <= ManualStep.From_ForkBWD) {
                        btnFromRun.Enabled = true;
                        btnToRun.Enabled = false;
                    }
                    else if (m_rackMaster.m_motion.GetCurerntManualStep() >= ManualStep.To_XZTGoing && m_rackMaster.m_motion.GetCurerntManualStep() <= ManualStep.To_ForkBWD) {
                        btnFromRun.Enabled = false;
                        btnToRun.Enabled = true;
                    }
                }
            }
            else {
                m_lblCtrl.BlinkingLabel(ref lblIDError);
                btnFromRun.Enabled = false;
                btnToRun.Enabled = false;
            }

            if (m_rackMaster.m_alarm.IsAlarmState() && !m_rackMaster.Interlock_OnlyMSTDisconnectedAlarmOccurred())
            {
                m_btnCtrl.BlinkingButton(ref btnErrorReset);
                btnForkHoming.Enabled = false;
            }
            else
            {
                m_btnCtrl.SetButtonIdleStatus(ref btnErrorReset);
                btnForkHoming.Enabled = true;
            }

            switch (m_rackMaster.m_motion.GetCurerntManualStep()) {
                case ManualStep.Idle:
                    m_lblCtrl.SetOnOffLabelStyle(ref lblXZTGoing,   false);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblForkFWD,    false);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblZMove,      false);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblForkBWD,    false);
                    break;

                case ManualStep.From_XZTGoing:
                case ManualStep.To_XZTGoing:
                    m_lblCtrl.SetOnOffLabelStyle(ref lblXZTGoing,   true);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblForkFWD,    false);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblZMove,      false);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblForkBWD,    false);
                    break;

                case ManualStep.From_ForkFWD:
                case ManualStep.To_ForkFWD:
                    m_lblCtrl.SetOnOffLabelStyle(ref lblXZTGoing,   false);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblForkFWD,    true);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblZMove,      false);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblForkBWD,    false);
                    break;

                case ManualStep.From_ZUp:
                case ManualStep.To_ZDown:
                    m_lblCtrl.SetOnOffLabelStyle(ref lblXZTGoing,   false);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblForkFWD,    false);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblZMove,      true);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblForkBWD,    false);
                    break;

                case ManualStep.From_ForkBWD:
                case ManualStep.To_ForkBWD:
                    m_lblCtrl.SetOnOffLabelStyle(ref lblXZTGoing,   false);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblForkFWD,    false);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblZMove,      false);
                    m_lblCtrl.SetOnOffLabelStyle(ref lblForkBWD,    true);
                    break;
            }

            switch (m_rackMaster.m_motion.GetCurrentManualSpeedType()) {
                case ManualSpeedType.High:
                    m_btnCtrl.SetOnOffButtonStyle(ref btnHighSpeed,     true);
                    m_btnCtrl.SetOnOffButtonStyle(ref btnLowSpeed,      false);
                    break;

                case ManualSpeedType.Low:
                    m_btnCtrl.SetOnOffButtonStyle(ref btnHighSpeed,     false);
                    m_btnCtrl.SetOnOffButtonStyle(ref btnLowSpeed,      true);
                    break;
            }

            m_btnCtrl.SetOnOffButtonStyle(ref btnAutoMode, m_motion.IsManualAutoCylceMode());
            m_btnCtrl.SetOnOffButtonStyle(ref btnAutoCylceStart, m_motion.IsManualAutoCycleRun());
        }

        private void UpdateTestDriveStatus() {
            foreach(DgvTestDriveStatusRow row in Enum.GetValues(typeof(DgvTestDriveStatusRow))) {
                switch (row) {
                    case DgvTestDriveStatusRow.Step:
                        m_dgvCtrl.SetCellText(ref dgvTDStatus, (int)row, (int)DgvStatusColumn.State, $"{m_rackMaster.m_motion.GetRackMasterAutoMotionStepString(m_rackMaster.m_motion.GetCurrentAutoStep())}");
                        break;

                    case DgvTestDriveStatusRow.Error:
                        if (m_rackMaster.m_alarm.IsAlarmState()) {
                            m_dgvCtrl.SetErrorCell(ref dgvTDStatus, (int)row, (int)DgvStatusColumn.State, true);
                            //m_dgvCtrl.SetCellText(ref dgvTDStatus, (int)row, (int)DgvStatusColumn.State, $"Error Occurred");
                        }
                        else {
                            m_dgvCtrl.SetErrorCell(ref dgvTDStatus, (int)row, (int)DgvStatusColumn.State, false);
                            //m_dgvCtrl.SetCellText(ref dgvTDStatus, (int)row, (int)DgvStatusColumn.State, $"None");
                        }
                        break;

                    case DgvTestDriveStatusRow.SourceEmpty:
                        if(m_rackMaster.m_motion.GetCurerntManualStep() == ManualStep.SourceEmpty) {
                            m_dgvCtrl.SetErrorCell(ref dgvTDStatus, (int)row, (int)DgvStatusColumn.State, true);
                        }
                        else {
                            m_dgvCtrl.SetErrorCell(ref dgvTDStatus, (int)row, (int)DgvStatusColumn.State, false);
                        }
                        break;

                    case DgvTestDriveStatusRow.ManualAutoCycleStep:
                        m_dgvCtrl.SetCellText(ref dgvTDStatus, (int)row, (int)DgvStatusColumn.State, $"{m_motion.GetManualAutoCylceStep()}");
                        break;

                    case DgvTestDriveStatusRow.ManualAutoCycleCount:
                        m_dgvCtrl.SetCellText(ref dgvTDStatus, (int)row, (int)DgvStatusColumn.State, $"{m_motion.GetManualAutoCycleCount()}");
                        break;

                    case DgvTestDriveStatusRow.MST_EMO:
                        m_dgvCtrl.SetErrorCell(ref dgvTDStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetReceiveBit(ReceiveBitMap.MST_Emo_Request));
                        break;

                    case DgvTestDriveStatusRow.Z_Axis_MaintStopper:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvTDStatus, (int)row, (int)DgvStatusColumn.State, !m_rackMaster.Interlock_ZAxisMaintStopper());
                        break;

                    case DgvTestDriveStatusRow.CST_Abnormal:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvTDStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.IsCassetteAbnormal());
                        break;
                }
            }
        }

        private void UpdateServoStatus() {
            foreach(DgvServoRow row in Enum.GetValues(typeof(DgvServoRow))) {
                AxisList axis = AxisList.X_Axis;
                if (row == DgvServoRow.Z_Axis)
                    axis = AxisList.Z_Axis;
                else if (row == DgvServoRow.A_Axis)
                    axis = AxisList.A_Axis;
                else if (row == DgvServoRow.T_Axis)
                    axis = AxisList.T_Axis;

                foreach(DgvServoColumn col in Enum.GetValues(typeof(DgvServoColumn))) {
                    if(m_rackMaster.m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn && axis == AxisList.T_Axis) {
                        if(col == DgvServoColumn.AxisName) {
                            m_dgvCtrl.SetCellStyle(ref dgvServoStatus, (int)row, (int)col, UICtrl.m_disableColor);
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvServoStatus, (int)row, (int)col);
                        }
                        continue;
                    }
                    else {
                        if (col == DgvServoColumn.AxisName) {
                            m_dgvCtrl.SetCellStyle(ref dgvServoStatus, (int)row, (int)col, UICtrl.m_idleColor);
                        }
                    }

                    switch (col) {
                        case DgvServoColumn.ServoOn:
                            m_dgvCtrl.SetOnOffCell(ref dgvServoStatus, (int)row, (int)col, m_rackMaster.m_motion.GetAxisFlag(AxisFlagType.Servo_On, axis));
                            break;

                        case DgvServoColumn.HomeDone:
                            m_dgvCtrl.SetOnOffCell(ref dgvServoStatus, (int)row, (int)col, m_rackMaster.m_motion.GetAxisFlag(AxisFlagType.HomeDone, axis));
                            break;

                        case DgvServoColumn.Position:
                            if(m_rackMaster.m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum && axis == AxisList.Z_Axis) {
                                m_dgvCtrl.SetCellStyle(ref dgvServoStatus, (int)row, (int)col, $"{(m_motion.GetDrumBeltZAxisActualPosition() / 1000):F3}", UICtrl.m_idleColor);
                            }
                            else {
                                m_dgvCtrl.SetCellStyle(ref dgvServoStatus, (int)row, (int)col, $"{(m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.pos_act, axis) / 1000):F3}", UICtrl.m_idleColor);
                            }
                            break;

                        case DgvServoColumn.Velocity:
                            if(axis == AxisList.X_Axis) {
                                m_dgvCtrl.SetCellStyle(ref dgvServoStatus, (int)row, (int)col, $"{(m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.vel_act, axis) / 1000000 * 60):F3}", UICtrl.m_idleColor);
                            }else if(axis == AxisList.Z_Axis) {
                                if(m_rackMaster.m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                                    m_dgvCtrl.SetCellStyle(ref dgvServoStatus, (int)row, (int)col, $"{(m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.vel_act, axis) / 1000000 * 60):F3}", UICtrl.m_idleColor);
                                }
                                else {
                                    m_dgvCtrl.SetCellStyle(ref dgvServoStatus, (int)row, (int)col, $"{(m_motion.GetDrumBeltZAxisActualVelocity() / 1000000 * 60):F3}", UICtrl.m_idleColor);
                                }
                            }
                            else if(axis == AxisList.A_Axis || axis == AxisList.T_Axis) {
                                m_dgvCtrl.SetCellStyle(ref dgvServoStatus, (int)row, (int)col, $"{(m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.vel_act, axis) / 1000 * 60):F3}", UICtrl.m_idleColor);
                            }
                            break;

                        case DgvServoColumn.Torque:
                            m_dgvCtrl.SetCellStyle(ref dgvServoStatus, (int)row, (int)col, $"{m_rackMaster.m_motion.GetAxisStatus(AxisStatusType.trq_act, axis)}", UICtrl.m_idleColor);
                            break;
                    }
                }
            }
        }

        private void UpdateIOStatus() {
            foreach(DgvIoRow row in Enum.GetValues(typeof(DgvIoRow))) {
                switch (row) {
                    case DgvIoRow.ForkPresenceSensor_1:
                        if (m_rackMaster.IsInputEnabled(InputList.Presense_Detection_1))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.Presense_Detection_1, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.ForkPresenceSensor_2:
                        if (m_rackMaster.IsInputEnabled(InputList.Presense_Detection_2))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.Presense_Detection_2, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.ForkDoubleStorageSensor_1:
                        if (m_rackMaster.IsInputEnabled(InputList.Double_Storage_Sensor_1))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.Double_Storage_Sensor_1, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.ForkDoubleStorageSensor_2:
                        if (m_rackMaster.IsInputEnabled(InputList.Double_Storage_Sensor_2))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.Double_Storage_Sensor_2, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.ForkPickSensorLeft:
                        if (m_rackMaster.IsInputEnabled(InputList.Fork_Pick_Sensor_Left))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.Fork_Pick_Sensor_Left, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.ForkPickSensorRight:
                        if (m_rackMaster.IsInputEnabled(InputList.Fork_Pick_Sensor_Right))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.Fork_Pick_Sensor_Right, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.ForkPlaceSensorLeft:
                        if (m_rackMaster.IsInputEnabled(InputList.Fork_Place_Sensor_Left))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.Fork_Place_Sensor_Left, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.ForkPlaceSensorRight:
                        if (m_rackMaster.IsInputEnabled(InputList.Fork_Place_Sensor_Right))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.Fork_Place_Sensor_Right, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.ForkInPlace_1:
                        if (m_rackMaster.IsInputEnabled(InputList.Fork_In_Place_1))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.Fork_In_Place_1, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.ForkInPlace_2:
                        if (m_rackMaster.IsInputEnabled(InputList.Fork_In_Place_2))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.Fork_In_Place_2, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.StickDetectSensor_1:
                        if (m_rackMaster.IsInputEnabled(InputList.StickDetectSensor_1))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.StickDetectSensor_1, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.StickDetectSensor_2:
                        if (m_rackMaster.IsInputEnabled(InputList.StickDetectSensor_2))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.StickDetectSensor_2, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.StickDetectSensor_3:
                        if (m_rackMaster.IsInputEnabled(InputList.StickDetectSensor_3))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.StickDetectSensor_3, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;

                    case DgvIoRow.StickDetectSensor_4:
                        if (m_rackMaster.IsInputEnabled(InputList.StickDetectSensor_4))
                            m_dgvCtrl.SetOnOffCell(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State, m_rackMaster.GetInputBit(InputList.StickDetectSensor_4, UICtrl.UI_IsIORawData()));
                        else
                            m_dgvCtrl.SetCellDisableStyle(ref dgvIOStatus, (int)row, (int)DgvStatusColumn.State);
                        break;
                }
            }
        }

        private void UpdateSettingValue() {
            foreach(DgvSettingValueColumn col in Enum.GetValues(typeof(DgvSettingValueColumn))) {
                if (col == DgvSettingValueColumn.Setting)
                    continue;
                foreach(DgvSettingValueRow row in Enum.GetValues(typeof(DgvSettingValueRow))) {
                    AxisList axis = AxisList.X_Axis;
                    if (col == DgvSettingValueColumn.Z_Axis)
                        axis = AxisList.Z_Axis;
                    else if (col == DgvSettingValueColumn.A_Axis)
                        axis = AxisList.A_Axis;
                    else if (col == DgvSettingValueColumn.T_Axis)
                        axis = AxisList.T_Axis;

                    switch (row) {
                        case DgvSettingValueRow.ManualHighSpeed:
                            m_dgvCtrl.SetCellText(ref dgvSettingValue, (int)row, (int)col, $"{m_rackMaster.m_param.GetAxisParameter(axis).manualHighSpeed}");
                            break;

                        case DgvSettingValueRow.ManualHighAccDec:
                            m_dgvCtrl.SetCellText(ref dgvSettingValue, (int)row, (int)col, $"{m_rackMaster.m_param.GetAxisParameter(axis).manualHighAccDec}");
                            break;

                        case DgvSettingValueRow.ManualLowSpeed:
                            m_dgvCtrl.SetCellText(ref dgvSettingValue, (int)row, (int)col, $"{m_rackMaster.m_param.GetAxisParameter(axis).manualLowSpeed}");
                            break;

                        case DgvSettingValueRow.ManualLowAccDec:
                            m_dgvCtrl.SetCellText(ref dgvSettingValue, (int)row, (int)col, $"{m_rackMaster.m_param.GetAxisParameter(axis).manualLowAccDec}");
                            break;
                    }
                }
            }
        }

        private void btnErrorReset_Click(object sender, EventArgs e) {
            if (m_rackMaster.m_alarm.IsAlarmState()) {
                m_rackMaster.m_alarm.ClearAlarm();
            }
        }

        private void btnLowSpeed_Click(object sender, EventArgs e) {
            m_rackMaster.m_motion.SetManualSpeedType(ManualSpeedType.Low);
        }

        private void btnHighSpeed_Click(object sender, EventArgs e) {
            m_rackMaster.m_motion.SetManualSpeedType(ManualSpeedType.High);
        }

        private void ManualRun_MouseDown(object sender, MouseEventArgs e) {
            if (!m_motion.IsAllServoOn()) {
                MessageBox.Show("Not Servo On!");
                return;
            }

            Button btn = sender as Button;

            if(btn == btnFromRun) {
                m_rackMaster.m_motion.StartManualRun(m_currentId, MotionMode.From);
            }
            else if(btn == btnToRun) {
                m_rackMaster.m_motion.StartManualRun(m_currentId, MotionMode.To);
            }
        }

        private void ManualRun_MouseUp(object sender, MouseEventArgs e) {
            m_rackMaster.m_motion.StopManualRun();
        }

        private void btnResetStep_Click(object sender, EventArgs e) {
            m_rackMaster.m_motion.ResetManualRun();
        }

        private void txtID_TextChanged(object sender, EventArgs e) {
            if (m_rackMaster.m_alarm.IsAlarmState())
                return;
            m_rackMaster.m_motion.ResetManualRun();
        }

        public void LanguageChanged() {
            //gboxTestModeStep.Text           = SynusLangPack.GetLanguage(gboxTestModeStep.Name);
            //gboxStatus.Text                 = SynusLangPack.GetLanguage(gboxStatus.Name);
            //gboxTestDriveStatus.Text        = SynusLangPack.GetLanguage(gboxTestDriveStatus.Name);
            //gboxIOStatus.Text               = SynusLangPack.GetLanguage(gboxIOStatus.Name);
            //gboxAxisStatus.Text             = SynusLangPack.GetLanguage(gboxAxisStatus.Name);
            //gboxSettingValue.Text           = SynusLangPack.GetLanguage(gboxSettingValue.Name);

            //m_btnCtrl.SetButtonText(ref btnFromRun, SynusLangPack.GetLanguage(btnFromRun.Name));
            //m_btnCtrl.SetButtonText(ref btnToRun, SynusLangPack.GetLanguage(btnToRun.Name));
            //m_btnCtrl.SetButtonText(ref btnForkHoming, SynusLangPack.GetLanguage(btnForkHoming.Name));
            //m_btnCtrl.SetButtonText(ref btnErrorReset, SynusLangPack.GetLanguage(btnErrorReset.Name));
            //m_btnCtrl.SetButtonText(ref btnResetStep, SynusLangPack.GetLanguage(btnResetStep.Name));
            //m_btnCtrl.SetButtonText(ref btnLowSpeed, SynusLangPack.GetLanguage(btnLowSpeed.Name));
            //m_btnCtrl.SetButtonText(ref btnHighSpeed, SynusLangPack.GetLanguage(btnHighSpeed.Name));
        }

        private void btnAutoMode_Click(object sender, EventArgs e) {
            if(m_motion.IsManualAutoCylceMode())
                m_motion.SetManualAutoCycleMode(false);
            else
                m_motion.SetManualAutoCycleMode(true);
        }

        private void btnAutoCylceStart_Click(object sender, EventArgs e) {
            if (!m_motion.IsManualAutoCylceMode()) {
                MessageBox.Show("Auto Cycle Mode 상태가 아닙니다.");
                return;
            }

            if(m_rackMaster.m_alarm.IsAlarmState() && !m_rackMaster.Interlock_OnlyMSTDisconnectedAlarmOccurred()) {
                MessageBox.Show("Alarm 상태입니다.");
                return;
            }

            if (m_motion.IsManualAutoCycleRun()) {
                MessageBox.Show("이미 동작중입니다.");
                return;
            }

            int fromId = 0;
            int toId = 0;
            int cycleCount = 0;

            try {
                fromId = int.Parse($"{txtAutoManualFromID.Text}");
                toId = int.Parse($"{txtAutoManualToID.Text}");
                cycleCount = int.Parse($"{txtAutoManualCycleCount.Text}");

                m_motion.StartManualAutoCycle(cycleCount, fromId, toId);
            }catch(Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, "Int Parsing Error", ex));
                return;
            }
        }

        private void btnAutoCycleStop_Click(object sender, EventArgs e) {
            m_motion.StopManualAutoCycle();
        }

        private void btnForkHoming_MouseDown(object sender, MouseEventArgs e) {
            if(!m_motion.GetAxisFlag(AxisFlagType.Servo_On, AxisList.A_Axis)) {
                MessageBox.Show("Not Servo On!");
                return;
            }

            m_rackMaster.m_motion.ForkHoming();
        }

        private void btnForkHoming_MouseUp(object sender, MouseEventArgs e) {
            m_rackMaster.m_motion.ForkHomingStop();
        }

        public bool IsTestDriveRun() {
            if (m_motion.GetCurrentAutoStep() != AutoStep.Step0_Idle && m_motion.GetCurrentAutoStep() != AutoStep.Step500_Error)
                return true;

            return false;
        }
    }
}
