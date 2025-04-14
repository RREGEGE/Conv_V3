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
using MovenCore;

namespace RackMaster.SUBFORM.SettingPage {
    public partial class SettingPage_WMX : Form {
        private enum DgvServoSettingsColumnIndex {
            Parameter,
            STK_X,
            STK_Z,
            STK_A,
            STK_T
        }

        private enum CboxEnalbed {
            Enabled,
            Disabled,
        }

        private enum SaveTimerType {
            Refresh,
            Reload,
            Save,
        }

        private UICtrl.DataGridViewCtrl m_dgvCtrl;
        private RackMasterMain m_rackMaster;
        private RackMasterMain.RackMasterParam m_param;
        private SEQ.CLS.Timer m_saveStopwatch;

        private SaveTimerType m_currentSaveType = SaveTimerType.Save;

        public SettingPage_WMX(RackMasterMain rackMaster) {
            InitializeComponent();

            m_dgvCtrl = new UICtrl.DataGridViewCtrl();
            m_rackMaster = rackMaster;
            m_param = m_rackMaster.m_param;
            m_saveStopwatch = new SEQ.CLS.Timer();
            m_saveStopwatch.Stop();

            InitDataGridView_WMX();

            btnRefreshWMXParam.MouseDown    += SaveMouseDown;
            btnReloadWMXParam.MouseDown     += SaveMouseDown;
            btnSaveWMXParam.MouseDown       += SaveMouseDown;

            btnRefreshWMXParam.MouseUp      += SaveMouseUp;
            btnReloadWMXParam.MouseUp       += SaveMouseUp;
            btnSaveWMXParam.MouseUp         += SaveMouseUp;
        }

        private void InitDataGridView_WMX() {
            try {
                foreach (DgvServoSettingsColumnIndex col in Enum.GetValues(typeof(DgvServoSettingsColumnIndex))) {
                    switch (col) {
                        case DgvServoSettingsColumnIndex.Parameter:
                            m_dgvCtrl.AddColumn(ref dgvWMXParam, $"WMX{col}", "Parameter");
                            break;

                        case DgvServoSettingsColumnIndex.STK_X:
                            m_dgvCtrl.AddColumn(ref dgvWMXParam, $"WMX{col}", "X Axis");
                            break;

                        case DgvServoSettingsColumnIndex.STK_Z:
                            m_dgvCtrl.AddColumn(ref dgvWMXParam, $"WMX{col}", "Z Axis");
                            break;

                        case DgvServoSettingsColumnIndex.STK_A:
                            m_dgvCtrl.AddColumn(ref dgvWMXParam, $"WMX{col}", "A Axis");
                            break;

                        case DgvServoSettingsColumnIndex.STK_T:
                            m_dgvCtrl.AddColumn(ref dgvWMXParam, $"WMX{col}", "T Axis");
                            break;
                    }
                }

                foreach (DgvServoSettingsColumnIndex col in Enum.GetValues(typeof(DgvServoSettingsColumnIndex))) {
                    AxisList axis = AxisList.X_Axis;
                    if (col == DgvServoSettingsColumnIndex.STK_X)
                        axis = AxisList.X_Axis;
                    else if (col == DgvServoSettingsColumnIndex.STK_Z)
                        axis = AxisList.Z_Axis;
                    else if (col == DgvServoSettingsColumnIndex.STK_A)
                        axis = AxisList.A_Axis;
                    else if (col == DgvServoSettingsColumnIndex.STK_T) {
                        //if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                        //    continue;
                        //}
                        //else {
                        //    axis = AxisList.T_Axis;
                        //}
                        axis = AxisList.T_Axis;
                    }

                    WMXMotion.AxisParameter param = new WMXMotion.AxisParameter();
                    m_param.GetWMXParameter(axis, ref param);

                    foreach (WMXParameterList row in Enum.GetValues(typeof(WMXParameterList))) {
                        switch (row) {
                            case WMXParameterList.GearRatioNumerator:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Gear Ratio Numerator", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_gearRatioNum}");
                                }
                                break;

                            case WMXParameterList.GearRatioDenominator:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Gear Ratio Denominator", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_gearRatioDen}");
                                }
                                break;

                            case WMXParameterList.AbsoluteEncoderMode:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Absolute Encoder Mode", (int)col);
                                }
                                else {
                                    DataGridViewComboBoxCell cboxAbsEncodModeType = new DataGridViewComboBoxCell();
                                    cboxAbsEncodModeType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                    foreach (CboxEnalbed type in Enum.GetValues(typeof(CboxEnalbed))) {
                                        cboxAbsEncodModeType.Items.Add($"{type}");
                                    }
                                    if (param.m_absEncoderMode) {
                                        cboxAbsEncodModeType.Value = $"{CboxEnalbed.Enabled}";
                                    }
                                    else {
                                        cboxAbsEncodModeType.Value = $"{CboxEnalbed.Disabled}";
                                    }
                                    m_dgvCtrl.SetCellStyle(ref dgvWMXParam, cboxAbsEncodModeType, (int)row, (int)col);
                                }
                                break;

                            case WMXParameterList.AbsoluteEncoderHomeOffset:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Absolute Encoder Home Offset", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_absEncoderHomeOffset}");
                                }
                                break;

                            case WMXParameterList.PosSetWidth:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Pos Set Width", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_posSetWidth}");
                                }
                                break;

                            case WMXParameterList.HomeType:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Home Type", (int)col);
                                }
                                else {
                                    DataGridViewComboBoxCell cboxHomeType = new DataGridViewComboBoxCell();
                                    cboxHomeType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                    foreach (WMXParam.m_homeType type in Enum.GetValues(typeof(WMXParam.m_homeType))) {
                                        cboxHomeType.Items.Add($"{type}");
                                    }
                                    cboxHomeType.Value = $"{param.m_homeType}";
                                    m_dgvCtrl.SetCellStyle(ref dgvWMXParam, cboxHomeType, (int)row, (int)col);
                                }
                                break;

                            case WMXParameterList.HomeDirection:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Home Direction", (int)col);
                                }
                                else {
                                    DataGridViewComboBoxCell cboxHomeDirection = new DataGridViewComboBoxCell();
                                    cboxHomeDirection.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                    foreach (WMXParam.m_homeDirection dir in Enum.GetValues(typeof(WMXParam.m_homeDirection))) {
                                        cboxHomeDirection.Items.Add($"{dir}");
                                    }
                                    cboxHomeDirection.Value = $"{param.m_homeDirection}";
                                    m_dgvCtrl.SetCellStyle(ref dgvWMXParam, cboxHomeDirection, (int)row, (int)col);
                                }
                                break;

                            case WMXParameterList.HomeSlowVelocity:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Home Slow Velocity", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeSlowVelocity}");
                                }
                                break;

                            case WMXParameterList.HomeSlowAcc:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Home Slow Acc", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeSlowAcc}");
                                }
                                break;

                            case WMXParameterList.HomeSlowDec:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Home Slow Dec", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeSlowDec}");
                                }
                                break;

                            case WMXParameterList.HomeFastVelocity:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Home Fast Velocity", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeFastVelocity}");
                                }
                                break;

                            case WMXParameterList.HomeFastAcc:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Home Fast Acc", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeFastAcc}");
                                }
                                break;

                            case WMXParameterList.HomeFastDec:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Home Fast Dec", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeFastDec}");
                                }
                                break;

                            case WMXParameterList.HomeShiftVelocity:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Home Shift Velocity", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeShiftVelocity}");
                                }
                                break;

                            case WMXParameterList.HomeShiftAcc:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Home Shift Acc", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeShiftAcc}");
                                }
                                break;

                            case WMXParameterList.HomeShiftDec:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Home Shift Dec", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeShiftDec}");
                                }
                                break;

                            case WMXParameterList.HomeShiftDistance:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Home Shift Distance", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeShiftDistance}");
                                }
                                break;

                            case WMXParameterList.LimitSwitchType:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Limit Switch Type", (int)col);
                                }
                                else {
                                    DataGridViewComboBoxCell cboxLimitSwitchType = new DataGridViewComboBoxCell();
                                    cboxLimitSwitchType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                    foreach (WMXParam.m_limitSwitchType type in Enum.GetValues(typeof(WMXParam.m_limitSwitchType))) {
                                        cboxLimitSwitchType.Items.Add($"{type}");
                                    }
                                    cboxLimitSwitchType.Value = $"{param.m_limitSwitchType}";
                                    m_dgvCtrl.SetCellStyle(ref dgvWMXParam, cboxLimitSwitchType, (int)row, (int)col);
                                }
                                break;

                            case WMXParameterList.SoftLimitSwitchType:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Soft Limit Switch Type", (int)col);
                                }
                                else {
                                    DataGridViewComboBoxCell cboxSwLimitSwitchType = new DataGridViewComboBoxCell();
                                    cboxSwLimitSwitchType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                    foreach (WMXParam.m_limitSwitchType type in Enum.GetValues(typeof(WMXParam.m_limitSwitchType))) {
                                        cboxSwLimitSwitchType.Items.Add($"{type}");
                                    }
                                    cboxSwLimitSwitchType.Value = $"{param.m_softLimitSwitchType}";
                                    m_dgvCtrl.SetCellStyle(ref dgvWMXParam, cboxSwLimitSwitchType, (int)row, (int)col);
                                }
                                break;

                            case WMXParameterList.SoftLimitSwitchPosValue:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Soft Limit Switch Pos Value", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_softLimitPosValue}");
                                }
                                break;

                            case WMXParameterList.SoftLimitSwitchNegValue:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Soft Limit Switch Neg Value", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_softLimitNegValue}");
                                }
                                break;

                            case WMXParameterList.LimitDec:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Soft Limit Switch Dec", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_limitDec}");
                                }
                                break;

                            case WMXParameterList.LinearInterpolationCalcMode:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Linear Intpl Calc Mode", (int)col);
                                }
                                else {
                                    DataGridViewComboBoxCell cboxLinIntplCalcMode = new DataGridViewComboBoxCell();
                                    cboxLinIntplCalcMode.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                    foreach (WMXParam.m_linIntplCalcMode mode in Enum.GetValues(typeof(WMXParam.m_linIntplCalcMode))) {
                                        cboxLinIntplCalcMode.Items.Add($"{mode}");
                                    }
                                    cboxLinIntplCalcMode.Value = $"{param.m_linintplCalcMode}";
                                    m_dgvCtrl.SetCellStyle(ref dgvWMXParam, cboxLinIntplCalcMode, (int)row, (int)col);
                                }
                                break;

                            case WMXParameterList.QuickStopDec:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvWMXParam, $"Quick Stop Decel", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_quickStopDecel}");
                                }
                                break;
                        }
                    }
                }
                m_dgvCtrl.DisableUserControl(ref dgvWMXParam);
                foreach (DataGridViewColumn column in dgvWMXParam.Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgvWMXParam.Columns[(int)DgvServoSettingsColumnIndex.Parameter].ReadOnly = true;
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, Log.LogMessage_Main.UI_DataGridViewExceptionErrorOccurred, ex));
            }
        }

        private bool CheckDataGridView_WMX() {
            double tempData = 0;

            foreach (DgvServoSettingsColumnIndex col in Enum.GetValues(typeof(DgvServoSettingsColumnIndex))) {
                if (col == DgvServoSettingsColumnIndex.Parameter)
                    continue;

                foreach (WMXParameterList row in Enum.GetValues(typeof(WMXParameterList))) {
                    if (row == WMXParameterList.AbsoluteEncoderMode || row == WMXParameterList.HomeType || row == WMXParameterList.HomeDirection ||
                        row == WMXParameterList.LimitSwitchType || row == WMXParameterList.SoftLimitSwitchType || row == WMXParameterList.LinearInterpolationCalcMode) {
                        continue;
                    }

                    if (!double.TryParse($"{dgvWMXParam[(int)col, (int)row].Value}", out tempData))
                        return false;
                }
            }

            return true;
        }

        private void SaveWMXParameter() {
            try {
                foreach (DgvServoSettingsColumnIndex col in Enum.GetValues(typeof(DgvServoSettingsColumnIndex))) {
                    AxisList axis = AxisList.X_Axis;

                    if (col == DgvServoSettingsColumnIndex.Parameter)
                        continue;
                    else if (col == DgvServoSettingsColumnIndex.STK_X)
                        axis = AxisList.X_Axis;
                    else if (col == DgvServoSettingsColumnIndex.STK_Z)
                        axis = AxisList.Z_Axis;
                    else if (col == DgvServoSettingsColumnIndex.STK_A)
                        axis = AxisList.A_Axis;
                    else if (col == DgvServoSettingsColumnIndex.STK_T) {
                        //if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn)
                        //    continue;
                        //else
                        //    axis = AxisList.T_Axis;
                        axis = AxisList.T_Axis;
                    }

                    WMXMotion.AxisParameter wmxParameter = new WMXMotion.AxisParameter();
                    m_rackMaster.m_param.GetWMXParameter(axis, ref wmxParameter);

                    wmxParameter.m_motorDirection = m_rackMaster.m_param.GetWMXParameter(axis).m_motorDirection;

                    foreach (WMXParameterList wmx in Enum.GetValues(typeof(WMXParameterList))) {
                        switch (wmx) {
                            case WMXParameterList.GearRatioNumerator:
                                wmxParameter.m_gearRatioNum = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.GearRatioDenominator:
                                wmxParameter.m_gearRatioDen = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.AbsoluteEncoderMode:
                                DataGridViewComboBoxCell cboxAbsEncoderMode = (DataGridViewComboBoxCell)dgvWMXParam.Rows[(int)wmx].Cells[(int)col];
                                if (CboxEnalbed.Enabled == (CboxEnalbed)cboxAbsEncoderMode.Items.IndexOf(cboxAbsEncoderMode.Value)) {
                                    wmxParameter.m_absEncoderMode = true;
                                }
                                else {
                                    wmxParameter.m_absEncoderMode = false;
                                }
                                break;

                            case WMXParameterList.AbsoluteEncoderHomeOffset:
                                wmxParameter.m_absEncoderHomeOffset = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.PosSetWidth:
                                wmxParameter.m_posSetWidth = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.HomeType:
                                DataGridViewComboBoxCell cboxHomeType = (DataGridViewComboBoxCell)dgvWMXParam.Rows[(int)wmx].Cells[(int)col];
                                wmxParameter.m_homeType = (WMXParam.m_homeType)cboxHomeType.Items.IndexOf(cboxHomeType.Value);
                                break;

                            case WMXParameterList.HomeDirection:
                                DataGridViewComboBoxCell cboxHomeDirection = (DataGridViewComboBoxCell)dgvWMXParam.Rows[(int)wmx].Cells[(int)col];
                                wmxParameter.m_homeDirection = (WMXParam.m_homeDirection)cboxHomeDirection.Items.IndexOf(cboxHomeDirection.Value);
                                break;

                            case WMXParameterList.HomeSlowVelocity:
                                wmxParameter.m_homeSlowVelocity = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.HomeSlowAcc:
                                wmxParameter.m_homeSlowAcc = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.HomeSlowDec:
                                wmxParameter.m_homeSlowDec = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.HomeFastVelocity:
                                wmxParameter.m_homeFastVelocity = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.HomeFastAcc:
                                wmxParameter.m_homeFastAcc = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.HomeFastDec:
                                wmxParameter.m_homeFastDec = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.HomeShiftVelocity:
                                wmxParameter.m_homeShiftVelocity = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.HomeShiftAcc:
                                wmxParameter.m_homeShiftAcc = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.HomeShiftDec:
                                wmxParameter.m_homeShiftDec = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.HomeShiftDistance:
                                wmxParameter.m_homeShiftDistance = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.LimitSwitchType:
                                DataGridViewComboBoxCell cboxLimitSwitchType = (DataGridViewComboBoxCell)dgvWMXParam.Rows[(int)wmx].Cells[(int)col];
                                wmxParameter.m_limitSwitchType = (WMXParam.m_limitSwitchType)cboxLimitSwitchType.Items.IndexOf(cboxLimitSwitchType.Value);
                                break;

                            case WMXParameterList.SoftLimitSwitchType:
                                DataGridViewComboBoxCell cboxSwLimitType = (DataGridViewComboBoxCell)dgvWMXParam.Rows[(int)wmx].Cells[(int)col];
                                wmxParameter.m_softLimitSwitchType = (WMXParam.m_limitSwitchType)cboxSwLimitType.Items.IndexOf(cboxSwLimitType.Value);
                                break;

                            case WMXParameterList.SoftLimitSwitchPosValue:
                                wmxParameter.m_softLimitPosValue = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.SoftLimitSwitchNegValue:
                                wmxParameter.m_softLimitNegValue = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.LimitDec:
                                wmxParameter.m_limitDec = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;

                            case WMXParameterList.LinearInterpolationCalcMode:
                                DataGridViewComboBoxCell cboxLinIntplCalcMode = (DataGridViewComboBoxCell)dgvWMXParam.Rows[(int)wmx].Cells[(int)col];
                                wmxParameter.m_linintplCalcMode = (WMXParam.m_linIntplCalcMode)cboxLinIntplCalcMode.Items.IndexOf(cboxLinIntplCalcMode.Value);
                                break;

                            case WMXParameterList.QuickStopDec:
                                wmxParameter.m_quickStopDecel = Convert.ToDouble($"{dgvWMXParam[(int)col, (int)wmx].Value}");
                                break;
                        }
                    }
                    if (!m_param.SetWMXParameter(axis, wmxParameter)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.WMX, $"WMX Parameter Save Failed"));
                        MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                        return;
                    }
                }

                if (m_param.SaveWMXParameter()) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.WMX, $"WMX Parameter Save Success"));
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));

                }
                else {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.WMX, $"WMX Parameter Save Failed"));
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                }
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, Log.LogMessage_Main.UI_DataGridViewExceptionErrorOccurred, ex));
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }
        }

        private void dgvWMXParam_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == (int)DgvServoSettingsColumnIndex.Parameter)
                return;

            if (e.RowIndex == (int)WMXParameterList.AbsoluteEncoderMode || e.RowIndex == (int)WMXParameterList.HomeType || e.RowIndex == (int)WMXParameterList.HomeDirection ||
                e.RowIndex == (int)WMXParameterList.LimitSwitchType || e.RowIndex == (int)WMXParameterList.SoftLimitSwitchType || e.RowIndex == (int)WMXParameterList.LinearInterpolationCalcMode)
                return;

            double tempData = 0;

            if (!double.TryParse($"{dgvWMXParam[e.ColumnIndex, e.RowIndex].Value}", out tempData)) {
                dgvWMXParam[e.ColumnIndex, e.RowIndex].Value = 0;
            }
        }

        public bool RefreshWMXParameter(bool isMessageBoxPopUp) {
            //m_param.RefreshWMXParameter();
            try {
                foreach (DgvServoSettingsColumnIndex col in Enum.GetValues(typeof(DgvServoSettingsColumnIndex))) {
                    AxisList axis = AxisList.X_Axis;
                    if (col == DgvServoSettingsColumnIndex.Parameter)
                        continue;
                    else if (col == DgvServoSettingsColumnIndex.STK_X)
                        axis = AxisList.X_Axis;
                    else if (col == DgvServoSettingsColumnIndex.STK_Z)
                        axis = AxisList.Z_Axis;
                    else if (col == DgvServoSettingsColumnIndex.STK_A)
                        axis = AxisList.A_Axis;
                    else if (col == DgvServoSettingsColumnIndex.STK_T) {
                        //if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn)
                        //    continue;
                        //else
                        //    axis = AxisList.T_Axis;
                        axis = AxisList.T_Axis;
                    }

                    WMXMotion.AxisParameter param = new WMXMotion.AxisParameter();
                    m_param.GetWMXParameter(axis, ref param);

                    foreach (WMXParameterList row in Enum.GetValues(typeof(WMXParameterList))) {
                        switch (row) {
                            case WMXParameterList.GearRatioNumerator:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_gearRatioNum}");
                                break;

                            case WMXParameterList.GearRatioDenominator:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_gearRatioDen}");
                                break;

                            case WMXParameterList.AbsoluteEncoderMode:
                                DataGridViewComboBoxCell cboxAbsEncoderMode = (DataGridViewComboBoxCell)dgvWMXParam.Rows[(int)row].Cells[(int)col];
                                if (param.m_absEncoderMode)
                                    dgvWMXParam.Rows[(int)row].Cells[(int)col].Value = cboxAbsEncoderMode.Items[(int)CboxEnalbed.Enabled];
                                else
                                    dgvWMXParam.Rows[(int)row].Cells[(int)col].Value = cboxAbsEncoderMode.Items[(int)CboxEnalbed.Disabled];
                                break;

                            case WMXParameterList.AbsoluteEncoderHomeOffset:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_absEncoderHomeOffset}");
                                break;

                            case WMXParameterList.PosSetWidth:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_posSetWidth}");
                                break;

                            case WMXParameterList.HomeType:
                                DataGridViewComboBoxCell cboxHomeType = (DataGridViewComboBoxCell)dgvWMXParam.Rows[(int)row].Cells[(int)col];
                                dgvWMXParam.Rows[(int)row].Cells[(int)col].Value = cboxHomeType.Items[(int)param.m_homeType];
                                break;

                            case WMXParameterList.HomeDirection:
                                DataGridViewComboBoxCell cboxHomeDirection = (DataGridViewComboBoxCell)dgvWMXParam.Rows[(int)row].Cells[(int)col];
                                dgvWMXParam.Rows[(int)row].Cells[(int)col].Value = cboxHomeDirection.Items[(int)param.m_homeDirection];
                                break;

                            case WMXParameterList.HomeSlowVelocity:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeSlowVelocity}");
                                break;

                            case WMXParameterList.HomeSlowAcc:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeSlowAcc}");
                                break;

                            case WMXParameterList.HomeSlowDec:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeSlowDec}");
                                break;

                            case WMXParameterList.HomeFastVelocity:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeFastVelocity}");
                                break;

                            case WMXParameterList.HomeFastAcc:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeFastAcc}");
                                break;

                            case WMXParameterList.HomeFastDec:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeFastDec}");
                                break;

                            case WMXParameterList.HomeShiftVelocity:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeShiftVelocity}");
                                break;

                            case WMXParameterList.HomeShiftAcc:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeShiftAcc}");
                                break;

                            case WMXParameterList.HomeShiftDec:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeShiftDec}");
                                break;

                            case WMXParameterList.HomeShiftDistance:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_homeShiftDistance}");
                                break;

                            case WMXParameterList.LimitSwitchType:
                                DataGridViewComboBoxCell cboxLimitSwitchType = (DataGridViewComboBoxCell)dgvWMXParam.Rows[(int)row].Cells[(int)col];
                                dgvWMXParam.Rows[(int)row].Cells[(int)col].Value = cboxLimitSwitchType.Items[(int)param.m_limitSwitchType];
                                break;

                            case WMXParameterList.SoftLimitSwitchType:
                                DataGridViewComboBoxCell cboxSwLimitSwitchType = (DataGridViewComboBoxCell)dgvWMXParam.Rows[(int)row].Cells[(int)col];
                                dgvWMXParam.Rows[(int)row].Cells[(int)col].Value = cboxSwLimitSwitchType.Items[(int)param.m_softLimitSwitchType];
                                break;

                            case WMXParameterList.SoftLimitSwitchPosValue:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_softLimitPosValue}");
                                break;

                            case WMXParameterList.SoftLimitSwitchNegValue:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_softLimitNegValue}");
                                break;

                            case WMXParameterList.LimitDec:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_limitDec}");
                                break;

                            case WMXParameterList.LinearInterpolationCalcMode:
                                DataGridViewComboBoxCell cboxLinearIntplCalcMode = (DataGridViewComboBoxCell)dgvWMXParam.Rows[(int)row].Cells[(int)col];
                                dgvWMXParam.Rows[(int)row].Cells[(int)col].Value = cboxLinearIntplCalcMode.Items[(int)param.m_linintplCalcMode];
                                break;

                            case WMXParameterList.QuickStopDec:
                                m_dgvCtrl.SetCellText(ref dgvWMXParam, (int)row, (int)col, $"{param.m_quickStopDecel}");
                                break;
                        }
                    }
                }
                if(isMessageBoxPopUp)
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.RefreshedParameterSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
                return true;
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, Log.LogMessage_Main.UI_DataGridViewExceptionErrorOccurred, ex));
                if(isMessageBoxPopUp)
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.RefreshedParameterFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
                return false;
            }
        }

        private void ReloadWMXParameter() {
            bool ret = m_param.LoadWMXParameterFile();

            if (!ret) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterLoadFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
            }
            else {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterLoadSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
            }
        }

        private void SaveMouseDown(object sender, MouseEventArgs e) {
            if (!CheckDataGridView_WMX()) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.DataGridViewInvalidValue}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            Button btn = sender as Button;

            if (btn == btnRefreshWMXParam)
                m_currentSaveType = SaveTimerType.Refresh;
            else if (btn == btnReloadWMXParam)
                m_currentSaveType = SaveTimerType.Reload;
            else if (btn == btnSaveWMXParam)
                m_currentSaveType = SaveTimerType.Save;
            else {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.NotExistFunction}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            saveTimer.Start();
        }

        private void SaveMouseUp(object sender, MouseEventArgs e) {
            saveTimer.Stop();
            m_saveStopwatch.Stop();
        }

        private void saveTimer_Tick(object sender, EventArgs e) {
            if (!m_saveStopwatch.IsTimerStarted())
                m_saveStopwatch.Restart();

            if (m_saveStopwatch.Delay(UICtrl.m_saveDelayTime)) {
                saveTimer.Stop();
                m_saveStopwatch.Stop();
                switch (m_currentSaveType) {
                    case SaveTimerType.Refresh:
                        if (DialogResult.Yes == MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureRefreshParameter}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.YesNo)) {
                            RefreshWMXParameter(true);
                        }
                        break;

                    case SaveTimerType.Reload:
                        if (DialogResult.Yes == MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureLoadParameter}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.YesNo)) {
                            ReloadWMXParameter();
                        }
                        break;

                    case SaveTimerType.Save:
                        if (DialogResult.Yes == MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureParameterSave}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.YesNo)) {
                            SaveWMXParameter();
                        }
                        break;
                }
            }
        }
    }
}