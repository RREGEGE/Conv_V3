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
    public partial class SettingPage_RMParam : Form {
        private enum DgvParamSettingColumn {
            Parameter,
            Value,
        }

        private enum CboxEnalbed {
            Enabled,
            Disabled,
        }

        private enum CboxForkType {
            Slide = ForkType.Slide,
            Slide_NoTurn = ForkType.Slide_NoTurn,
            SCARA = ForkType.SCARA,
        }

        private enum DgvServoSettingsColumnIndex {
            Parameter,
            STK_X,
            STK_Z,
            STK_A,
            STK_T
        }

        private RackMasterMain m_main;
        private RackMasterMain.RackMasterParam m_param;
        private UICtrl.DataGridViewCtrl m_dgvCtrl;
        private SEQ.CLS.Timer m_saveStopwatch;
        private Utility.PasswordType m_currentAccessType = Utility.PasswordType.Admin;

        public SettingPage_RMParam(RackMasterMain rackMaster) {
            InitializeComponent();

            m_main = rackMaster;
            m_param = m_main.m_param;
            m_dgvCtrl = new UICtrl.DataGridViewCtrl();
            m_saveStopwatch = new SEQ.CLS.Timer();
            m_saveStopwatch.Stop();

            InitDataGridView_FullClosedParam();
            InitDataGridView_MotionParam();
            InitDataGridView_AutoTeachingParam();
            InitDataGridView_AxisParam();
            InitDataGridView_TimerParam();
            InitDataGridView_SCARAParam();

            btnParameterSave.MouseDown      += SaveMouseDown;
            btnParameterSave.MouseUp        += SaveMouseUp;
        }

        public void UpdateFormUI() {
            foreach(AxisParameter param in Enum.GetValues(typeof(AxisParameter))) {
                switch (param) {
                    case AxisParameter.AutoSpeedPercent:
                        if (!m_main.Interlock_AutoSpeedPercentLimit(AxisList.X_Axis) || !m_main.Interlock_AutoSpeedPercentLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_AutoSpeedPercentLimit(AxisList.A_Axis) || !m_main.Interlock_AutoSpeedPercentLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.AxisNumber:
                        if(!m_main.Interlock_ParameterAxisNumber())
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.MaxSpeed:
                        if(!m_main.Interlock_MaxSpeedLimit(AxisList.X_Axis) || !m_main.Interlock_MaxSpeedLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_MaxSpeedLimit(AxisList.A_Axis) || !m_main.Interlock_MaxSpeedLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.ManualHighSpeed:
                        if(!m_main.Interlock_ManualHighSpeedLimit(AxisList.X_Axis) || !m_main.Interlock_ManualHighSpeedLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_ManualHighSpeedLimit(AxisList.A_Axis) || !m_main.Interlock_ManualHighSpeedLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.ManualLowSpeed:
                        if(!m_main.Interlock_ManualLowSpeedLimit(AxisList.X_Axis) || !m_main.Interlock_ManualLowSpeedLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_ManualLowSpeedLimit(AxisList.A_Axis) || !m_main.Interlock_ManualLowSpeedLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.JogHighSpeedLimit:
                        if(!m_main.Interlock_JogHighSpeedLimit(AxisList.X_Axis) || !m_main.Interlock_JogHighSpeedLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_JogHighSpeedLimit(AxisList.A_Axis) || !m_main.Interlock_JogHighSpeedLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.JogLowSpeedLimit:
                        if(!m_main.Interlock_JogLowSpeedLimit(AxisList.X_Axis) || !m_main.Interlock_JogLowSpeedLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_JogLowSpeedLimit(AxisList.A_Axis) || !m_main.Interlock_JogLowSpeedLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.MaxAccDec:
                        if(!m_main.Interlock_MaxAccDecLimit(AxisList.X_Axis) || !m_main.Interlock_MaxAccDecLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_MaxAccDecLimit(AxisList.A_Axis) || !m_main.Interlock_MaxAccDecLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.ManualHighAccDec:
                        if(!m_main.Interlock_ManualHighAccDecLimit(AxisList.X_Axis) || !m_main.Interlock_ManualHighAccDecLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_ManualHighAccDecLimit(AxisList.A_Axis) || !m_main.Interlock_ManualHighAccDecLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.ManualLowAccDec:
                        if(!m_main.Interlock_ManualLowAccDecLimit(AxisList.X_Axis) || !m_main.Interlock_ManualLowAccDecLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_ManualLowAccDecLimit(AxisList.A_Axis) || !m_main.Interlock_ManualLowAccDecLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.QuickStop:
                        if(!m_main.Interlock_QuickStopDecelLimit(AxisList.X_Axis) || !m_main.Interlock_QuickStopDecelLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_QuickStopDecelLimit(AxisList.A_Axis) || !m_main.Interlock_QuickStopDecelLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.NormalStop:
                        if(!m_main.Interlock_NormalStopDecelLimit(AxisList.X_Axis) || !m_main.Interlock_NormalStopDecelLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_NormalStopDecelLimit(AxisList.A_Axis) || !m_main.Interlock_NormalStopDecelLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.SlowStop:
                        if(!m_main.Interlock_SlowStopDecelLimit(AxisList.X_Axis) || !m_main.Interlock_SlowStopDecelLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_SlowStopDecelLimit(AxisList.A_Axis) || !m_main.Interlock_SlowStopDecelLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;

                    case AxisParameter.JerkRatio:
                        if(!m_main.Interlock_JerkRatioLimit(AxisList.X_Axis) || !m_main.Interlock_JerkRatioLimit(AxisList.Z_Axis) ||
                            !m_main.Interlock_JerkRatioLimit(AxisList.A_Axis) || !m_main.Interlock_JerkRatioLimit(AxisList.T_Axis))
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, true, false);
                        else
                            m_dgvCtrl.SetErrorCell(ref dgvAxisParam, (int)param, (int)DgvServoSettingsColumnIndex.Parameter, false, false);
                        break;
                }
            }
        }

        private void InitDataGridView_FullClosedParam() {
            try {
                int rowIdx = 0;
                foreach (FullClosedParameterList param in Enum.GetValues(typeof(FullClosedParameterList))) {
                    switch (param) {
                        case FullClosedParameterList.Axis:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Axis Number", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeParam().m_axis}");
                            break;

                        case FullClosedParameterList.BarcodeStartAddress:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Barcode Start Address", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeParam().m_startAddr}");
                            break;

                        case FullClosedParameterList.BarcodeSize:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Barcode Size(byte)", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeParam().m_size}");
                            break;

                        case FullClosedParameterList.BarcodeScale:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Barcode Scale", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeParam().m_barcodeScale}");
                            break;

                        case FullClosedParameterList.HomeOffset:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Home Offset", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeParam().m_homeOffset}");
                            break;

                        case FullClosedParameterList.SpecInType:
                            DataGridViewComboBoxCell cboxSpecInType = new DataGridViewComboBoxCell();
                            cboxSpecInType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (BarcodeSpecInType fork in Enum.GetValues(typeof(BarcodeSpecInType))) {
                                cboxSpecInType.Items.Add($"{fork}");
                            }
                            cboxSpecInType.Value = $"{m_param.GetBarcodeParam().m_specInType}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, cboxSpecInType, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Parameter, "Spec In Type");
                            break;

                        case FullClosedParameterList.SpecInRange:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Spec In Range", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeParam().m_specInRange}");
                            break;

                        case FullClosedParameterList.SpecInTime:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Spec In Time(s)", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeParam().m_specInTimeSec}");
                            break;

                        case FullClosedParameterList.Error1_StartAddress:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Error1 Start Address", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_error1_StartAddr}");
                            break;

                        case FullClosedParameterList.Error1_Bit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Error1 Bit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_error1_Bit}");
                            break;

                        case FullClosedParameterList.Error2_StartAddress:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Error2 Start Address", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_error2_StartAddr}");
                            break;

                        case FullClosedParameterList.Error2_Bit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Error2 Bit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_error2_Bit}");
                            break;

                        case FullClosedParameterList.Error3_StartAddress:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Error3 Start Address", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_error3_StartAddr}");
                            break;

                        case FullClosedParameterList.Error3_Bit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Error3 Bit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_error3_Bit}");
                            break;

                        case FullClosedParameterList.Error4_StartAddress:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Error4 Start Address", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_error4_StartAddr}");
                            break;

                        case FullClosedParameterList.Error4_Bit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Error4 Bit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_error4_Bit}");
                            break;

                        case FullClosedParameterList.EMO1_StartAddress:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "EMO1 Start Address", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_EMO1_StartAddr}");
                            break;

                        case FullClosedParameterList.EMO1_Bit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "EMO1 Bit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_EMO1_Bit}");
                            break;

                        case FullClosedParameterList.EMO2_StartAddress:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "EMO2 Start Address", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_EMO2_StartAddr}");
                            break;

                        case FullClosedParameterList.EMO2_Bit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "EMO2 Bit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_EMO2_Bit}");
                            break;

                        case FullClosedParameterList.EMO3_StartAddress:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "EMO3 Start Address", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_EMO3_StartAddr}");
                            break;

                        case FullClosedParameterList.EMO3_Bit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "EMO3 Bit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_EMO3_Bit}");
                            break;

                        case FullClosedParameterList.EMO4_StartAddress:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "EMO4 Start Address", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_EMO4_StartAddr}");
                            break;

                        case FullClosedParameterList.EMO4_Bit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "EMO4 Bit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_EMO4_Bit}");
                            break;

                        case FullClosedParameterList.AlarmStopDecTimeSecond:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Alarm Stop Dec Time(s)", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_alarmStopDecTimeSec}");
                            break;

                        case FullClosedParameterList.StopDecTimeSecond:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Stop Dec Time(s)", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_stopDecTimeSec}");
                            break;

                        case FullClosedParameterList.FollowingError:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Following Error", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_followingError}");
                            break;

                        case FullClosedParameterList.VelocityLimit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Velocity Limit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_velocityLimit}");
                            break;

                        case FullClosedParameterList.AccLimit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Acc Limit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_accLimit}");
                            break;

                        case FullClosedParameterList.DecLimit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Dec Limit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_decLimit}");
                            break;

                        case FullClosedParameterList.UseBarcodePositiveLimit:
                            DataGridViewComboBoxCell cboxUsedBarcodePosLimit = new DataGridViewComboBoxCell();
                            cboxUsedBarcodePosLimit.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (CboxEnalbed posLimit in Enum.GetValues(typeof(CboxEnalbed))) {
                                cboxUsedBarcodePosLimit.Items.Add($"{posLimit}");
                            }
                            if (m_param.GetBarcodeSafetyParam().m_useBarcodePositiveLimit)
                                cboxUsedBarcodePosLimit.Value = $"{CboxEnalbed.Enabled}";
                            else
                                cboxUsedBarcodePosLimit.Value = $"{CboxEnalbed.Disabled}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Use Positive Limit", cboxUsedBarcodePosLimit, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Parameter, "Use Positive Limit");
                            break;

                        case FullClosedParameterList.BarcodePositiveLimit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Barcode Positive Limit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_barcodePositiveLimit}");
                            break;

                        case FullClosedParameterList.UseBarcodeNegativeLimit:
                            DataGridViewComboBoxCell cboxUsedBarcodeNegLimit = new DataGridViewComboBoxCell();
                            cboxUsedBarcodeNegLimit.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (CboxEnalbed negLimit in Enum.GetValues(typeof(CboxEnalbed))) {
                                cboxUsedBarcodeNegLimit.Items.Add($"{negLimit}");
                            }
                            if (m_param.GetBarcodeSafetyParam().m_useBarcodeNegativeLimit)
                                cboxUsedBarcodeNegLimit.Value = $"{CboxEnalbed.Enabled}";
                            else
                                cboxUsedBarcodeNegLimit.Value = $"{CboxEnalbed.Disabled}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Use Negative Limit", cboxUsedBarcodeNegLimit, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Parameter, "Use Negative Limit");
                            break;

                        case FullClosedParameterList.BarcodeNegativeLimit:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "Barcode Negative Limit", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetBarcodeSafetyParam().m_barcodeNegativeLimit}");
                            break;

                        case FullClosedParameterList.PGain:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "PGain", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetFullClosedPGain()}");
                            break;

                        case FullClosedParameterList.IGain:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvFullClosed, "IGain", (int)DgvParamSettingColumn.Parameter);
                            m_dgvCtrl.SetCellText(ref dgvFullClosed, rowIdx, (int)DgvParamSettingColumn.Value, $"{m_param.GetFullClosedIGain()}");
                            break;
                    }
                }

                foreach (DataGridViewColumn column in dgvFullClosed.Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, $"Initialize Full Closed Parameter Fail", ex));
            }
        }

        private void InitDataGridView_MotionParam() {
            try {
                int rowIdx = 0;
                foreach (MotionParameter row in Enum.GetValues(typeof(MotionParameter))) {
                    switch (row) {
                        case MotionParameter.ForkType:
                            DataGridViewComboBoxCell cboxForkType = new DataGridViewComboBoxCell();
                            cboxForkType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (CboxForkType fork in Enum.GetValues(typeof(CboxForkType))) {
                                cboxForkType.Items.Add($"{fork}");
                            }
                            cboxForkType.Value = $"{m_param.GetMotionParam().forkType}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, cboxForkType, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Fork Type");
                            break;

                        case MotionParameter.StopMode:
                            DataGridViewComboBoxCell cboxStopMode = new DataGridViewComboBoxCell();
                            cboxStopMode.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (AxisStopMode stop in Enum.GetValues(typeof(AxisStopMode))) {
                                cboxStopMode.Items.Add($"{stop}");
                            }
                            cboxStopMode.Value = $"{m_param.GetMotionParam().stopMode}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, cboxStopMode, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Stop Mode");
                            break;

                        case MotionParameter.ZOverridePercent:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().Z_OverridePercent}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Override Speed Percent(%)");
                            break;

                        case MotionParameter.ZOverrideFromUpDistance:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().Z_OverrideFromUpDist}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Override From Up Dist(mm)");
                            break;

                        case MotionParameter.ZOverrideFromDownDistance:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().Z_OverrideFromDownDist}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Override From Down Dist(mm)");
                            break;

                        case MotionParameter.ZOverrideToUpDistance:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().Z_OverrideToUpDist}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Override To Up Dist(mm)");
                            break;

                        case MotionParameter.ZOverrideToDownDistance:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().Z_OverrideToDownDist}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Override To Down Dist(mm)");
                            break;

                        case MotionParameter.TurnCenterPosition:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().turnCenterPosition}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Turn Center Position(°)");
                            break;

                        case MotionParameter.ArmHomePositionRange:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().armHomePositionRange}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Arm Home Position Range");
                            break;

                        case MotionParameter.UseInterpolation:
                            DataGridViewComboBoxCell cboxInterpolation = new DataGridViewComboBoxCell();
                            cboxInterpolation.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (CboxEnalbed interpolation in Enum.GetValues(typeof(CboxEnalbed))) {
                                cboxInterpolation.Items.Add($"{interpolation}");
                            }
                            if (m_param.GetMotionParam().useInterpolation)
                                cboxInterpolation.Value = $"{CboxEnalbed.Enabled}";
                            else
                                cboxInterpolation.Value = $"{CboxEnalbed.Disabled}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, "Interpolaton", cboxInterpolation, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Interpolaton");
                            break;

                        case MotionParameter.UseFullclosed:
                            DataGridViewComboBoxCell cboxFullClosed = new DataGridViewComboBoxCell();
                            cboxFullClosed.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (CboxEnalbed fullClosed in Enum.GetValues(typeof(CboxEnalbed))) {
                                cboxFullClosed.Items.Add($"{fullClosed}");
                            }
                            if (m_param.GetMotionParam().useFullClosed)
                                cboxFullClosed.Value = $"{CboxEnalbed.Enabled}";
                            else
                                cboxFullClosed.Value = $"{CboxEnalbed.Disabled}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, "Full Closed", cboxFullClosed, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Full Closed");
                            break;

                        case MotionParameter.DistanceDetectSensor:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().distDetectSensor}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Distance Detect Sensor");
                            break;

                        case MotionParameter.ZAutoHomingCount:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().Z_AutoHomingCount}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Auto Homing Count");
                            break;

                        case MotionParameter.PresenseSensorConditionType:
                            DataGridViewComboBoxCell cboxPresenseType = new DataGridViewComboBoxCell();
                            cboxPresenseType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (SensorConditionType type in Enum.GetValues(typeof(SensorConditionType))) {
                                cboxPresenseType.Items.Add($"{type}");
                            }
                            cboxPresenseType.Value = $"{m_param.GetMotionParam().presenseConditionType}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, cboxPresenseType, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Presense Condition Type");
                            break;

                        case MotionParameter.InPlaceSensorConditionType:
                            DataGridViewComboBoxCell cboxInPlaceCondType = new DataGridViewComboBoxCell();
                            cboxInPlaceCondType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (SensorConditionType type in Enum.GetValues(typeof(SensorConditionType))) {
                                cboxInPlaceCondType.Items.Add($"{type}");
                            }
                            cboxInPlaceCondType.Value = $"{m_param.GetMotionParam().inPlaceConditionType}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, cboxInPlaceCondType, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "In Place Condition Type");
                            break;

                        case MotionParameter.InPlaceSensorType:
                            DataGridViewComboBoxCell cboxInPlaceType = new DataGridViewComboBoxCell();
                            cboxInPlaceType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (InPlaceSensorType type in Enum.GetValues(typeof(InPlaceSensorType))) {
                                cboxInPlaceType.Items.Add($"{type}");
                            }
                            cboxInPlaceType.Value = $"{m_param.GetMotionParam().inPlaceType}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, cboxInPlaceType, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "In Place Sensor Type");
                            break;

                        case MotionParameter.ZAxisBeltType:
                            DataGridViewComboBoxCell cboxZBeltType = new DataGridViewComboBoxCell();
                            cboxZBeltType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (ZAxisBeltType type in Enum.GetValues(typeof(ZAxisBeltType))) {
                                cboxZBeltType.Items.Add($"{type}");
                            }
                            cboxZBeltType.Value = $"{m_param.GetMotionParam().ZAxisBeltType}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, cboxZBeltType, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Axis Belt Type");
                            break;

                        case MotionParameter.ZAxisBeltHomeOffset:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().ZAxisBeltHomeOffset}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Axis Belt Home Offset(deg)");
                            break;

                        case MotionParameter.ZAxisBeltFirstDia:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().ZAxisBeltFirstDia}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Axis Belt First DIA(mm)");
                            break;

                        case MotionParameter.ZAxisBeltIncrementDia:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().ZAxisBeltDia}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Axis Belt DIA(mm)");
                            break;

                        case MotionParameter.ToModeAutoSpeedPercent:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, $"{m_param.GetMotionParam().toModeAutoSpeedPercent}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "To Mode Fork Auto Speed Percent");
                            break;

                        case MotionParameter.UseRegulator:
                            DataGridViewComboBoxCell cboxRegulator = new DataGridViewComboBoxCell();
                            cboxRegulator.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (CboxEnalbed regulator in Enum.GetValues(typeof(CboxEnalbed))) {
                                cboxRegulator.Items.Add($"{regulator}");
                            }
                            if (m_param.GetMotionParam().useRegulator)
                                cboxRegulator.Value = $"{CboxEnalbed.Enabled}";
                            else
                                cboxRegulator.Value = $"{CboxEnalbed.Disabled}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, "Use Regulator", cboxRegulator, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Use Regulator");
                            break;

                        case MotionParameter.MaintTarget:
                            DataGridViewComboBoxCell cboxMaintTarget = new DataGridViewComboBoxCell();
                            cboxMaintTarget.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (MaintTarget type in Enum.GetValues(typeof(MaintTarget))) {
                                cboxMaintTarget.Items.Add($"{type}");
                            }
                            cboxMaintTarget.Value = $"{m_param.GetMotionParam().maintTarget}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, cboxMaintTarget, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Maint Target");
                            break;

                        case MotionParameter.UseMaint:
                            DataGridViewComboBoxCell cboxUseMaint = new DataGridViewComboBoxCell();
                            cboxUseMaint.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (CboxEnalbed maint in Enum.GetValues(typeof(CboxEnalbed))) {
                                cboxUseMaint.Items.Add($"{maint}");
                            }
                            if (m_param.GetMotionParam().useMaint)
                                cboxUseMaint.Value = $"{CboxEnalbed.Enabled}";
                            else
                                cboxUseMaint.Value = $"{CboxEnalbed.Disabled}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, "Use Maint", cboxUseMaint, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Use Maint");
                            break;

                        case MotionParameter.PresenseSensorType:
                            DataGridViewComboBoxCell cboxPresenseSensorType = new DataGridViewComboBoxCell();
                            cboxPresenseSensorType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (PresenseSensorType type in Enum.GetValues(typeof(PresenseSensorType))) {
                                cboxPresenseSensorType.Items.Add($"{type}");
                            }
                            cboxPresenseSensorType.Value = $"{m_param.GetMotionParam().presensType}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvMotionParam, cboxPresenseSensorType, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvMotionParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Presense Sensor Type");
                            break;
                    }
                }
                m_dgvCtrl.DisableUserControl(ref dgvMotionParam);
                foreach (DataGridViewColumn column in dgvMotionParam.Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgvMotionParam.Columns[(int)DgvParamSettingColumn.Parameter].ReadOnly = true;
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, Log.LogMessage_Main.UI_DataGridViewExceptionErrorOccurred, ex));
            }
        }

        private void InitDataGridView_AutoTeachingParam() {
            try {
                int rowIdx = 0;
                foreach (AutoTeachingParameter index in Enum.GetValues(typeof(AutoTeachingParameter))) {
                    switch (index) {
                        case AutoTeachingParameter.SensorType:
                            DataGridViewComboBoxCell cboxSensorType = new DataGridViewComboBoxCell();
                            cboxSensorType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (AutoTeachingSensorType fork in Enum.GetValues(typeof(AutoTeachingSensorType))) {
                                cboxSensorType.Items.Add($"{fork}");
                            }
                            cboxSensorType.Value = $"{m_param.GetAutoTeachingParam().sensorType}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, cboxSensorType, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Sensor Type");
                            break;

                        //case AutoTeachingParameter.AutoTeachingType:
                        //    DataGridViewComboBoxCell cboxAutoteachingType = new DataGridViewComboBoxCell();
                        //    cboxAutoteachingType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                        //    foreach (AutoTeachingType fork in Enum.GetValues(typeof(AutoTeachingType))) {
                        //        cboxAutoteachingType.Items.Add($"{fork}");
                        //    }
                        //    cboxAutoteachingType.Value = $"{m_param.GetAutoTeachingParam().autoTeachingType}";
                        //    rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, cboxAutoteachingType, (int)DgvParamSettingColumn.Value);
                        //    m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Auto Teaching Type");
                        //    break;

                        case AutoTeachingParameter.AutoTeachingSpeedX:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingSpeedX}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "X Speed(mm/s)");
                            break;

                        case AutoTeachingParameter.AutoTeachingSpeedZ:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingSpeedZ}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Speed(mm/s)");
                            break;

                        case AutoTeachingParameter.AutoTeachingAccDecX:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingAccDecX}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "X Acc/Dec(s)");
                            break;

                        case AutoTeachingParameter.AutoTeachingAccDecZ:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingAccDecZ}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Acc/Dec(s)");
                            break;

                        case AutoTeachingParameter.AutoTeachingDistX:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingDistX}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "X Distance(mm)");
                            break;

                        case AutoTeachingParameter.AutoTeachingDistZ:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingDistZ}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Distance(mm)");
                            break;

                        case AutoTeachingParameter.AutoTeachingCompensationX:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingCompensationX}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "X Compensation(mm)");
                            break;

                        case AutoTeachingParameter.AutoTeachingCompensationZ:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingCompensationZ}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Compensation(mm)");
                            break;

                        case AutoTeachingParameter.AutoTeachingFindSensorRangeX:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingFindSensorRangeX}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "X Find Sensor Range(mm)");
                            break;

                        case AutoTeachingParameter.AutoTeachingFindSensorRangeZ:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingFindSensorRangeZ}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Find Sensor Range(mm)");
                            break;

                        case AutoTeachingParameter.AutoTeachingFindSensorSpeedX:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingFindSensorSpeedX}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "X Find Sensor Speed(mm/s)");
                            break;

                        case AutoTeachingParameter.AutoTeachingFindSensorSpeedZ:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingFindSensorSpeedZ}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Find Sensor Speed(mm/s)");
                            break;

                        case AutoTeachingParameter.AutoTeachingFindSensorAccDecX:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingFindSensorAccDecX}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "X Find Sensor Acc/Dec(s)");
                            break;

                        case AutoTeachingParameter.AutoTeachingFindSensorAccDecZ:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingFindSensorAccDecZ}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Find Sensor Acc/Dec(s)");
                            break;

                        case AutoTeachingParameter.AutoTeachingReflectorWidth:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingReflectorWidth}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Reflector Width(mm)");
                            break;

                        case AutoTeachingParameter.AutoTeachingReflectorHeight:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingReflectorHeight}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Reflector Height(mm)");
                            break;

                        case AutoTeachingParameter.AutoTeachingReflectorWidthErrorRange:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingReflectorErrorRangeWidth}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Reflector Width Errror Range(mm)");
                            break;

                        case AutoTeachingParameter.AutoTeachingReflectorHeightErrorRange:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingReflectorErrorRangeHeight}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Reflector Height Error Range(mm)");
                            break;

                        case AutoTeachingParameter.AutoTeachingTargetSpeedX:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingTargetSpeedX}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Target Speed X(mm/s)");
                            break;

                        case AutoTeachingParameter.AutoTeachingTargetSpeedZ:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingTargetSpeedZ}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Target Speed Z(mm/s)");
                            break;

                        case AutoTeachingParameter.AutoTeachingTargetAccDecX:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingTargetAccDecX}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Target Acc/Dec X(s)");
                            break;

                        case AutoTeachingParameter.AutoTeachingTargetAccDecZ:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, $"{m_param.GetAutoTeachingParam().autoTeachingTargetAccDecZ}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Target Acc/Dec Z(s)");
                            break;

                        case AutoTeachingParameter.DoubleStorageSensorCheck:
                            DataGridViewComboBoxCell cboxDoubleStorage = new DataGridViewComboBoxCell();
                            cboxDoubleStorage.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (CboxEnalbed doubleStorage in Enum.GetValues(typeof(CboxEnalbed))) {
                                cboxDoubleStorage.Items.Add($"{doubleStorage}");
                            }
                            if (m_param.GetAutoTeachingParam().doubleStorageSensorCheck)
                                cboxDoubleStorage.Value = $"{CboxEnalbed.Enabled}";
                            else
                                cboxDoubleStorage.Value = $"{CboxEnalbed.Disabled}";
                            rowIdx = m_dgvCtrl.AddRow(ref dgvAutoTeachingParam, "Double Storage Check", cboxDoubleStorage, (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvAutoTeachingParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Double Storage Check");
                            break;
                    }
                }
                m_dgvCtrl.DisableUserControl(ref dgvAutoTeachingParam);
                foreach (DataGridViewColumn column in dgvAutoTeachingParam.Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgvAutoTeachingParam.Columns[(int)DgvParamSettingColumn.Parameter].ReadOnly = true;
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, Log.LogMessage_Main.UI_DataGridViewExceptionErrorOccurred, ex));
            }
        }

        private void InitDataGridView_AxisParam() {
            try {
                foreach (AxisParameter index in Enum.GetValues(typeof(AxisParameter))) {
                    foreach (DgvServoSettingsColumnIndex col in Enum.GetValues(typeof(DgvServoSettingsColumnIndex))) {
                        AxisList axisIndex = AxisList.X_Axis;
                        if (col == DgvServoSettingsColumnIndex.STK_X)
                            axisIndex = AxisList.X_Axis;
                        else if (col == DgvServoSettingsColumnIndex.STK_Z)
                            axisIndex = AxisList.Z_Axis;
                        else if (col == DgvServoSettingsColumnIndex.STK_A)
                            axisIndex = AxisList.A_Axis;
                        else if (col == DgvServoSettingsColumnIndex.STK_T)
                            axisIndex = AxisList.T_Axis;

                        //if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn && axisIndex == AxisList.T_Axis) {
                        //    m_dgvCtrl.SetCellDisableStyle(ref dgvAxisParam, (int)index, (int)col, true);
                        //    continue;
                        //}

                        switch (index) {
                            case AxisParameter.AxisNumber:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Axis Number", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).axisNumber}");
                                }
                                break;

                            case AxisParameter.AutoSpeedPercent:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Auto Speed Percent(%)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).autoSpeedPercent}");
                                }
                                break;

                            case AxisParameter.MaxSpeed:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Max Speed(u/min)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).maxSpeed}");
                                }
                                break;

                            case AxisParameter.MaxAccDec:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Max Acc/Dec(s)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).maxAccDec}");
                                }
                                break;

                            case AxisParameter.JerkRatio:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Jerk Ratio(%)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).jerkRatio}");
                                }
                                break;

                            case AxisParameter.JogHighSpeedLimit:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Jog High Speed Limit(u/min)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).jogHighSpeedLimit}");
                                }
                                break;

                            case AxisParameter.JogLowSpeedLimit:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Jog Low Speed Limit(u/min)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).jogLowSpeedLimit}");
                                }
                                break;

                            case AxisParameter.InchingLimit:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Inching Limit(mm)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).inchingLimit}");
                                }
                                break;

                            case AxisParameter.ManualHighSpeed:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Manual High Speed(u/min)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).manualHighSpeed}");
                                }
                                break;

                            case AxisParameter.ManualHighAccDec:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Manual High Acc/Dec(s)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).manualHighAccDec}");
                                }
                                break;

                            case AxisParameter.ManualLowSpeed:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Manual Low Speed(u/min)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).manualLowSpeed}");
                                }
                                break;

                            case AxisParameter.ManualLowAccDec:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Manual Low Acc/Dec(s)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).manualLowAccDec}");
                                }
                                break;

                            case AxisParameter.MaxOverload:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Max Overload(%)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).maxOverload}");
                                }
                                break;

                            case AxisParameter.HomePositionRange:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Home Position Range(mm)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).homePositionRange}");
                                }
                                break;

                            case AxisParameter.QuickStop:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Quick Stop(s)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).quickStop}");
                                }
                                break;

                            case AxisParameter.NormalStop:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Normal Stop(s)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).normalStop}");
                                }
                                break;

                            case AxisParameter.SlowStop:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Slow Stop(s)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).slowStop}");
                                }
                                break;

                            case AxisParameter.PosSensorByteAddr:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Pos Sensor Byte Addr", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).posSensorByteAddr}");
                                }
                                break;

                            case AxisParameter.PosSensorBitAddr:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Pos Sensor Bit Addr", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).posSensorBitAddr}");
                                }
                                break;

                            case AxisParameter.PosSensorEnabled:
                                DataGridViewComboBoxCell cboxUsePosSensor = new DataGridViewComboBoxCell();
                                cboxUsePosSensor.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                foreach (CboxEnalbed posSensor in Enum.GetValues(typeof(CboxEnalbed))) {
                                    cboxUsePosSensor.Items.Add($"{posSensor}");
                                }
                                if (m_param.GetAxisParameter(axisIndex).posSensorEnabled)
                                    cboxUsePosSensor.Value = $"{CboxEnalbed.Enabled}";
                                else
                                    cboxUsePosSensor.Value = $"{CboxEnalbed.Disabled}";
                                if(col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, $"Pos Sensor Enabled", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellStyle(ref dgvAxisParam, cboxUsePosSensor, (int)index, (int)col);
                                }
                                break;

                            case AxisParameter.PosSensor2ByteAddr:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Pos Sensor 2 Byte Addr", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).posSensor2ByteAddr}");
                                }
                                break;

                            case AxisParameter.PosSensor2BitAddr:
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, "Pos Sensor 2 Bit Addr", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellText(ref dgvAxisParam, (int)index, (int)col, $"{m_param.GetAxisParameter(axisIndex).posSensor2BitAddr}");
                                }
                                break;

                            case AxisParameter.PosSensor2Enabled:
                                DataGridViewComboBoxCell cboxUsePosSensor2 = new DataGridViewComboBoxCell();
                                cboxUsePosSensor2.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                foreach (CboxEnalbed posSensor in Enum.GetValues(typeof(CboxEnalbed))) {
                                    cboxUsePosSensor2.Items.Add($"{posSensor}");
                                }
                                if (m_param.GetAxisParameter(axisIndex).posSensor2Enabled)
                                    cboxUsePosSensor2.Value = $"{CboxEnalbed.Enabled}";
                                else
                                    cboxUsePosSensor2.Value = $"{CboxEnalbed.Disabled}";
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, $"Pos Sensor 2 Enabled", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellStyle(ref dgvAxisParam, cboxUsePosSensor2, (int)index, (int)col);
                                }
                                break;

                            case AxisParameter.SoftwareLimitPositive:
                                DataGridViewButtonCell swLimitPosBtn = new DataGridViewButtonCell();
                                swLimitPosBtn.Value = $"{m_param.GetAxisParameter(axisIndex).swLimitPositive / 1000:F0}";
                                swLimitPosBtn.Style.BackColor = UICtrl.m_idleColor;
                                if(col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, $"Sw Limit Positive(mm)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellStyle(ref dgvAxisParam, swLimitPosBtn, (int)index, (int)col);
                                }
                                break;

                            case AxisParameter.SoftwareLimitNegative:
                                DataGridViewButtonCell swLimitNegBtn = new DataGridViewButtonCell();
                                swLimitNegBtn.Value = $"{m_param.GetAxisParameter(axisIndex).swLimitNegative / 1000:F0}";
                                swLimitNegBtn.Style.BackColor = UICtrl.m_idleColor;
                                if (col == DgvServoSettingsColumnIndex.Parameter) {
                                    m_dgvCtrl.AddRow(ref dgvAxisParam, $"Sw Limit Negative(mm)", (int)col);
                                }
                                else {
                                    m_dgvCtrl.SetCellStyle(ref dgvAxisParam, swLimitNegBtn, (int)index, (int)col);
                                }
                                break;
                        }
                    }
                }
                m_dgvCtrl.DisableUserControl(ref dgvAxisParam);
                foreach (DataGridViewColumn column in dgvAxisParam.Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgvAxisParam.Columns[(int)DgvParamSettingColumn.Parameter].ReadOnly = true;
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, Log.LogMessage_Main.UI_DataGridViewExceptionErrorOccurred, ex));
            }
        }

        private void InitDataGridView_TimerParam() {
            try {
                int rowIdx = 0;
                foreach (TimerParameter index in Enum.GetValues(typeof(TimerParameter))) {
                    switch (index) {
                        case TimerParameter.CIMTimerOver:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvTimerParam, $"{m_param.GetTimerParam().CIM_TIMEOVER}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvTimerParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "CIM Timer(ms)");
                            break;

                        case TimerParameter.StepTimerOver:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvTimerParam, $"{m_param.GetTimerParam().STEP_TEIMOVER}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvTimerParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Step Timer(ms)");
                            break;

                        case TimerParameter.PIOReadyTimerOver:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvTimerParam, $"{m_param.GetTimerParam().PIO_READY_TIMOVER}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvTimerParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "PIO Ready Timer(ms)");
                            break;

                        case TimerParameter.HomeMoveTimerOver:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvTimerParam, $"{m_param.GetTimerParam().HOME_MOVE_TIMEOVER}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvTimerParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Homing Timer(ms)");
                            break;

                        case TimerParameter.IoTimer:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvTimerParam, $"{m_param.GetTimerParam().IO_TIMER}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvTimerParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "IO Timer(ms)");
                            break;

                        case TimerParameter.EventTimer:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvTimerParam, $"{m_param.GetTimerParam().EVENT_TIMEROVER}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvTimerParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Event Timer(ms)");
                            break;

                        case TimerParameter.AutoTeachingStepTimeOver:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvTimerParam, $"{m_param.GetTimerParam().AUTO_TEACHING_STEP_TIMEOVER}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvTimerParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Auto Teaching Step Timer(ms)");
                            break;
                    }
                }
                m_dgvCtrl.DisableUserControl(ref dgvTimerParam);
                foreach (DataGridViewColumn column in dgvTimerParam.Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgvTimerParam.Columns[(int)DgvParamSettingColumn.Parameter].ReadOnly = true;
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, Log.LogMessage_Main.UI_DataGridViewExceptionErrorOccurred, ex));
            }
        }

        private void InitDataGridView_SCARAParam() {
            try {
                int rowIdx = 0;
                foreach (SCARAParameter index in Enum.GetValues(typeof(SCARAParameter))) {
                    switch (index) {
                        case SCARAParameter.RZLength:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvScaraParam, $"{m_param.GetSCARAParamter().RZ_LENGTH}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvScaraParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Z Length(mm)");
                            break;

                        case SCARAParameter.RXLength:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvScaraParam, "X Length(mm)", $"{m_param.GetSCARAParamter().RX_LENGTH}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvScaraParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "X Length(mm)");
                            break;

                        case SCARAParameter.RYLength:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvScaraParam, "Y Length(mm)", $"{m_param.GetSCARAParamter().RY_LENGTH}", (int)DgvParamSettingColumn.Value);
                            m_dgvCtrl.SetCellText(ref dgvScaraParam, rowIdx, (int)DgvParamSettingColumn.Parameter, "Y Length(mm)");
                            break;
                    }
                }
                m_dgvCtrl.DisableUserControl(ref dgvScaraParam);
                foreach (DataGridViewColumn column in dgvScaraParam.Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgvScaraParam.Columns[(int)DgvParamSettingColumn.Parameter].ReadOnly = true;
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, Log.LogMessage_Main.UI_DataGridViewExceptionErrorOccurred, ex));
            }
        }

        private bool CheckDataGridView_FullClosedParameter() {
            for (int row = 0; row < dgvFullClosed.Rows.Count; row++) {
                if (row == (int)FullClosedParameterList.UseBarcodePositiveLimit || row == (int)FullClosedParameterList.UseBarcodeNegativeLimit ||
                    row == (int)FullClosedParameterList.SpecInType)
                    continue;

                double value = 0;
                if (!double.TryParse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, row].Value}", out value))
                    return false;
            }

            return true;
        }

        private bool CheckDataGridView_MotionParameter() {
            for (int row = 0; row < dgvMotionParam.Rows.Count; row++) {
                if (row == (int)MotionParameter.ForkType || row == (int)MotionParameter.UseInterpolation || row == (int)MotionParameter.StopMode ||
                    row == (int)MotionParameter.UseFullclosed || row == (int)MotionParameter.PresenseSensorConditionType ||
                    row == (int)MotionParameter.InPlaceSensorConditionType || row == (int)MotionParameter.InPlaceSensorType ||
                    row == (int)MotionParameter.ZAxisBeltType || row == (int)MotionParameter.UseRegulator || row == (int)MotionParameter.MaintTarget ||
                    row == (int)MotionParameter.UseMaint || row == (int)MotionParameter.PresenseSensorType)
                    continue;

                float tempData = 0;
                if (!float.TryParse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, row].Value}", out tempData) || tempData < 0)
                    return false;
            }
            return true;
        }

        private bool CheckDataGridView_AxisParameter() {
            foreach (DgvServoSettingsColumnIndex col in Enum.GetValues(typeof(DgvServoSettingsColumnIndex))) {
                if (col == DgvServoSettingsColumnIndex.Parameter)
                    continue;

                if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn && col == DgvServoSettingsColumnIndex.STK_T)
                    continue;

                for (int row = 0; row < dgvAxisParam.Rows.Count; row++) {
                    if (row == (int)AxisParameter.PosSensorEnabled || row == (int)AxisParameter.PosSensor2Enabled || row == (int)AxisParameter.SoftwareLimitPositive ||
                        row == (int)AxisParameter.SoftwareLimitNegative)
                        continue;

                    float tempData = 0;
                    if (!float.TryParse($"{dgvAxisParam[(int)col, row].Value}", out tempData) || tempData < 0)
                        return false;
                }
            }
            return true;
        }

        private bool CheckDataGridView_AutoTeachingParameter() {
            for (int row = 0; row < dgvAutoTeachingParam.Rows.Count; row++) {
                if (row == (int)AutoTeachingParameter.SensorType || row == (int)AutoTeachingParameter.DoubleStorageSensorCheck)
                    continue;
                float tempData = 0;
                if (!float.TryParse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, row].Value}", out tempData) || tempData < 0)
                    return false;
            }
            return true;
        }

        private bool CheckDataGridView_TimerParameter() {
            for (int row = 0; row < dgvTimerParam.Rows.Count; row++) {
                float tempData = 0;
                if (!float.TryParse($"{dgvTimerParam[(int)DgvParamSettingColumn.Value, row].Value}", out tempData) || tempData < 0)
                    return false;
            }
            return true;
        }

        private bool CheckDataGridView_ScaraParameter() {
            for (int row = 0; row < dgvScaraParam.Rows.Count; row++) {
                float tempData = 0;
                if (!float.TryParse($"{dgvScaraParam[(int)DgvParamSettingColumn.Value, row].Value}", out tempData) || tempData < 0)
                    return false;
            }
            return true;
        }

        private void SaveMouseDown(object sender, MouseEventArgs e) {
            if (!CheckDataGridView_AutoTeachingParameter() || !CheckDataGridView_AxisParameter() || !CheckDataGridView_MotionParameter() ||
                    !CheckDataGridView_ScaraParameter() || !CheckDataGridView_TimerParameter() || !CheckDataGridView_FullClosedParameter()) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.DataGridViewInvalidValue}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            saveTimer.Start();
        }

        private void SaveMouseUp(object sender, MouseEventArgs e) {
            saveTimer.Stop();
            m_saveStopwatch.Stop();
        }

        private void SaveParameter() {
            try {
                #region Motion Parameter
                if(m_currentAccessType == Utility.PasswordType.Admin) {
                    RackMasterMain.RackMasterParam.MotionParam motionParam = new RackMasterMain.RackMasterParam.MotionParam();
                    foreach (MotionParameter motion in Enum.GetValues(typeof(MotionParameter))) {
                        switch (motion) {
                            case MotionParameter.ForkType:
                                DataGridViewComboBoxCell cboxForkType = (DataGridViewComboBoxCell)dgvMotionParam.Rows[(int)motion].Cells[(int)DgvParamSettingColumn.Value];
                                motionParam.forkType = (ForkType)cboxForkType.Items.IndexOf(cboxForkType.Value);
                                break;

                            case MotionParameter.StopMode:
                                DataGridViewComboBoxCell cboxStopMode = (DataGridViewComboBoxCell)dgvMotionParam.Rows[(int)motion].Cells[(int)DgvParamSettingColumn.Value];
                                motionParam.stopMode = (AxisStopMode)cboxStopMode.Items.IndexOf(cboxStopMode.Value);
                                break;

                            case MotionParameter.ZOverridePercent:
                                motionParam.Z_OverridePercent = float.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.ZOverrideFromUpDistance:
                                motionParam.Z_OverrideFromUpDist = float.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.ZOverrideFromDownDistance:
                                motionParam.Z_OverrideFromDownDist = float.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.ZOverrideToUpDistance:
                                motionParam.Z_OverrideToUpDist = float.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.ZOverrideToDownDistance:
                                motionParam.Z_OverrideToDownDist = float.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.TurnCenterPosition:
                                motionParam.turnCenterPosition = float.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.ArmHomePositionRange:
                                motionParam.armHomePositionRange = float.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.UseInterpolation:
                                DataGridViewComboBoxCell cboxInterpolation = (DataGridViewComboBoxCell)dgvMotionParam.Rows[(int)motion].Cells[(int)DgvParamSettingColumn.Value];
                                if ((CboxEnalbed)cboxInterpolation.Items.IndexOf(cboxInterpolation.Value) == CboxEnalbed.Enabled)
                                    motionParam.useInterpolation = true;
                                else
                                    motionParam.useInterpolation = false;
                                break;

                            case MotionParameter.UseFullclosed:
                                DataGridViewComboBoxCell cboxFullClosed = (DataGridViewComboBoxCell)dgvMotionParam.Rows[(int)motion].Cells[(int)DgvParamSettingColumn.Value];
                                if ((CboxEnalbed)cboxFullClosed.Items.IndexOf(cboxFullClosed.Value) == CboxEnalbed.Enabled)
                                    motionParam.useFullClosed = true;
                                else
                                    motionParam.useFullClosed = false;
                                break;

                            case MotionParameter.DistanceDetectSensor:
                                motionParam.distDetectSensor = int.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.ZAutoHomingCount:
                                motionParam.Z_AutoHomingCount = int.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.PresenseSensorConditionType:
                                DataGridViewComboBoxCell cboxPresenseType = (DataGridViewComboBoxCell)dgvMotionParam.Rows[(int)motion].Cells[(int)DgvParamSettingColumn.Value];
                                motionParam.presenseConditionType = (SensorConditionType)cboxPresenseType.Items.IndexOf(cboxPresenseType.Value);
                                break;

                            case MotionParameter.InPlaceSensorConditionType:
                                DataGridViewComboBoxCell cboxInPlaceCondType = (DataGridViewComboBoxCell)dgvMotionParam.Rows[(int)motion].Cells[(int)DgvParamSettingColumn.Value];
                                motionParam.inPlaceConditionType = (SensorConditionType)cboxInPlaceCondType.Items.IndexOf(cboxInPlaceCondType.Value);
                                break;

                            case MotionParameter.InPlaceSensorType:
                                DataGridViewComboBoxCell cboxInPlaceType = (DataGridViewComboBoxCell)dgvMotionParam.Rows[(int)motion].Cells[(int)DgvParamSettingColumn.Value];
                                motionParam.inPlaceType = (InPlaceSensorType)cboxInPlaceType.Items.IndexOf(cboxInPlaceType.Value);
                                break;

                            case MotionParameter.ZAxisBeltType:
                                DataGridViewComboBoxCell cboxZAxisBeltTyp = (DataGridViewComboBoxCell)dgvMotionParam.Rows[(int)motion].Cells[(int)DgvParamSettingColumn.Value];
                                motionParam.ZAxisBeltType = (ZAxisBeltType)cboxZAxisBeltTyp.Items.IndexOf(cboxZAxisBeltTyp.Value);
                                break;

                            case MotionParameter.ZAxisBeltHomeOffset:
                                motionParam.ZAxisBeltHomeOffset = double.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.ZAxisBeltFirstDia:
                                motionParam.ZAxisBeltFirstDia = int.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.ZAxisBeltIncrementDia:
                                motionParam.ZAxisBeltDia = int.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.ToModeAutoSpeedPercent:
                                motionParam.toModeAutoSpeedPercent = int.Parse($"{dgvMotionParam[(int)DgvParamSettingColumn.Value, (int)motion].Value}");
                                break;

                            case MotionParameter.UseRegulator:
                                DataGridViewComboBoxCell cboxRegulator = (DataGridViewComboBoxCell)dgvMotionParam.Rows[(int)motion].Cells[(int)DgvParamSettingColumn.Value];
                                if ((CboxEnalbed)cboxRegulator.Items.IndexOf(cboxRegulator.Value) == CboxEnalbed.Enabled)
                                    motionParam.useRegulator = true;
                                else
                                    motionParam.useRegulator = false;
                                break;

                            case MotionParameter.MaintTarget:
                                DataGridViewComboBoxCell cboxMaintTarget = (DataGridViewComboBoxCell)dgvMotionParam.Rows[(int)motion].Cells[(int)DgvParamSettingColumn.Value];
                                motionParam.maintTarget = (MaintTarget)cboxMaintTarget.Items.IndexOf(cboxMaintTarget.Value);
                                break;

                            case MotionParameter.UseMaint:
                                DataGridViewComboBoxCell cboxUseMaint = (DataGridViewComboBoxCell)dgvMotionParam.Rows[(int)motion].Cells[(int)DgvParamSettingColumn.Value];
                                if ((CboxEnalbed)cboxUseMaint.Items.IndexOf(cboxUseMaint.Value) == CboxEnalbed.Enabled)
                                    motionParam.useMaint = true;
                                else
                                    motionParam.useMaint = false;
                                break;

                            case MotionParameter.PresenseSensorType:
                                DataGridViewComboBoxCell cboxPresenseSensorType = (DataGridViewComboBoxCell)dgvMotionParam.Rows[(int)motion].Cells[(int)DgvParamSettingColumn.Value];
                                motionParam.presensType = (PresenseSensorType)cboxPresenseSensorType.Items.IndexOf(cboxPresenseSensorType.Value);
                                break;
                        }
                    }
                    if (!m_param.SetMotionParam(motionParam)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Motion Parameter Save Failed"));
                        MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
                        return;
                    }

                    //if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                    //    if (!m_dgvCtrl.IsCellDisabled(dgvAxisParam, 0, (int)DgvServoSettingsColumnIndex.STK_T)) {
                    //        DisableTAxis();
                    //    }
                    //}
                    //else {
                    //    if (m_dgvCtrl.IsCellDisabled(dgvAxisParam, 0, (int)DgvServoSettingsColumnIndex.STK_T)) {
                    //        EnableTAxis();
                    //    }
                    //}
                }
                #endregion



                #region Axis Parameter
                foreach (DgvServoSettingsColumnIndex servoCol in Enum.GetValues(typeof(DgvServoSettingsColumnIndex))) {
                    int column = (int)servoCol;
                    AxisList axisIndex = AxisList.X_Axis;

                    if (servoCol == DgvServoSettingsColumnIndex.STK_X)
                        axisIndex = AxisList.X_Axis;
                    else if (servoCol == DgvServoSettingsColumnIndex.STK_Z)
                        axisIndex = AxisList.Z_Axis;
                    else if (servoCol == DgvServoSettingsColumnIndex.STK_A)
                        axisIndex = AxisList.A_Axis;
                    else if (servoCol == DgvServoSettingsColumnIndex.STK_T)
                        axisIndex = AxisList.T_Axis;
                    else if (servoCol == DgvServoSettingsColumnIndex.Parameter)
                        continue;

                    //if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn && axisIndex == AxisList.T_Axis)
                    //    continue;

                        RackMasterMain.RackMasterParam.AxisParam parameters = new RackMasterMain.RackMasterParam.AxisParam();

                    foreach (AxisParameter item in Enum.GetValues(typeof(AxisParameter))) {
                        int row = (int)item;
                        switch (item) {
                            case AxisParameter.AxisNumber:
                                parameters.axisNumber = Convert.ToInt32($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.AutoSpeedPercent:
                                parameters.autoSpeedPercent = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.MaxSpeed:
                                parameters.maxSpeed = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.MaxAccDec:
                                parameters.maxAccDec = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.JerkRatio:
                                parameters.jerkRatio = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.JogHighSpeedLimit:
                                parameters.jogHighSpeedLimit = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.JogLowSpeedLimit:
                                parameters.jogLowSpeedLimit = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.InchingLimit:
                                parameters.inchingLimit = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.ManualHighSpeed:
                                parameters.manualHighSpeed = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.ManualHighAccDec:
                                parameters.manualHighAccDec = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.ManualLowSpeed:
                                parameters.manualLowSpeed = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.ManualLowAccDec:
                                parameters.manualLowAccDec = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.MaxOverload:
                                parameters.maxOverload = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.HomePositionRange:
                                parameters.homePositionRange = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.QuickStop:
                                parameters.quickStop = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.NormalStop:
                                parameters.normalStop = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.SlowStop:
                                parameters.slowStop = float.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.PosSensorByteAddr:
                                parameters.posSensorByteAddr = int.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.PosSensorBitAddr:
                                parameters.posSensorBitAddr = int.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.PosSensorEnabled:
                                DataGridViewComboBoxCell cboxPosSensorEnabled = (DataGridViewComboBoxCell)dgvAxisParam.Rows[row].Cells[column];
                                if ((CboxEnalbed)cboxPosSensorEnabled.Items.IndexOf(cboxPosSensorEnabled.Value) == CboxEnalbed.Enabled)
                                    parameters.posSensorEnabled = true;
                                else
                                    parameters.posSensorEnabled = false;
                                break;

                            case AxisParameter.PosSensor2ByteAddr:
                                parameters.posSensor2ByteAddr = int.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.PosSensor2BitAddr:
                                parameters.posSensor2BitAddr = int.Parse($"{dgvAxisParam[column, row].Value}");
                                break;

                            case AxisParameter.PosSensor2Enabled:
                                DataGridViewComboBoxCell cboxPosSensor2Enabled = (DataGridViewComboBoxCell)dgvAxisParam.Rows[row].Cells[column];
                                if ((CboxEnalbed)cboxPosSensor2Enabled.Items.IndexOf(cboxPosSensor2Enabled.Value) == CboxEnalbed.Enabled)
                                    parameters.posSensor2Enabled = true;
                                else
                                    parameters.posSensor2Enabled = false;
                                break;

                            case AxisParameter.SoftwareLimitPositive:
                                parameters.swLimitPositive = float.Parse($"{dgvAxisParam[column, row].Value}") * 1000;
                                //m_param.SetSoftwareLimitPositiveValue(axisIndex, parameters.swLimitPositive);
                                if (m_param.GetMotionParam().useFullClosed && axisIndex == AxisList.X_Axis) {
                                    m_dgvCtrl.SetCellText(ref dgvFullClosed, (int)FullClosedParameterList.BarcodePositiveLimit, (int)DgvParamSettingColumn.Value, $"{parameters.swLimitPositive:F0}");
                                }
                                break;

                            case AxisParameter.SoftwareLimitNegative:
                                parameters.swLimitNegative = float.Parse($"{dgvAxisParam[column, row].Value}") * 1000;
                                //m_param.SetSoftwareLimitNegativeValue(axisIndex, parameters.swLimitNegative);
                                if (m_param.GetMotionParam().useFullClosed && axisIndex == AxisList.X_Axis) {
                                    m_dgvCtrl.SetCellText(ref dgvFullClosed, (int)FullClosedParameterList.BarcodeNegativeLimit, (int)DgvParamSettingColumn.Value, $"{parameters.swLimitNegative:F0}");
                                }
                                break;
                        }
                    }

                    //m_param.SaveWMXParameter();
                    if (!m_param.SetAxisParameter(axisIndex, parameters)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Axis Parameter Save Failed"));
                        MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
                        return;
                    }
                }
                #endregion

                #region AutoTeaching Parameter
                if(m_currentAccessType == Utility.PasswordType.Admin) {
                    RackMasterMain.RackMasterParam.AutoTeachingParam autoTeachingParam = new RackMasterMain.RackMasterParam.AutoTeachingParam();
                    foreach (AutoTeachingParameter autoTeaching in Enum.GetValues(typeof(AutoTeachingParameter))) {
                        switch (autoTeaching) {
                            case AutoTeachingParameter.SensorType:
                                DataGridViewComboBoxCell cboxSensorType = (DataGridViewComboBoxCell)dgvAutoTeachingParam.Rows[(int)autoTeaching].Cells[(int)DgvParamSettingColumn.Value];
                                autoTeachingParam.sensorType = (AutoTeachingSensorType)cboxSensorType.Items.IndexOf(cboxSensorType.Value);
                                break;

                            //case AutoTeachingParameter.AutoTeachingType:
                            //    DataGridViewComboBoxCell cboxAutoTeachingType = (DataGridViewComboBoxCell)dgvAutoTeachingParam.Rows[(int)autoTeaching].Cells[(int)DgvParamSettingColumn.Value];
                            //    autoTeachingParam.autoTeachingType = (AutoTeachingType)cboxAutoTeachingType.Items.IndexOf(cboxAutoTeachingType.Value);
                            //    break;

                            case AutoTeachingParameter.AutoTeachingSpeedX:
                                autoTeachingParam.autoTeachingSpeedX = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingSpeedZ:
                                autoTeachingParam.autoTeachingSpeedZ = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingAccDecX:
                                autoTeachingParam.autoTeachingAccDecX = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingAccDecZ:
                                autoTeachingParam.autoTeachingAccDecZ = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingDistX:
                                autoTeachingParam.autoTeachingDistX = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingDistZ:
                                autoTeachingParam.autoTeachingDistZ = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingCompensationX:
                                autoTeachingParam.autoTeachingCompensationX = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingCompensationZ:
                                autoTeachingParam.autoTeachingCompensationZ = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingFindSensorRangeX:
                                autoTeachingParam.autoTeachingFindSensorRangeX = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingFindSensorRangeZ:
                                autoTeachingParam.autoTeachingFindSensorRangeZ = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingFindSensorSpeedX:
                                autoTeachingParam.autoTeachingFindSensorSpeedX = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingFindSensorSpeedZ:
                                autoTeachingParam.autoTeachingFindSensorSpeedZ = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingFindSensorAccDecX:
                                autoTeachingParam.autoTeachingFindSensorAccDecX = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingFindSensorAccDecZ:
                                autoTeachingParam.autoTeachingFindSensorAccDecZ = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingReflectorWidth:
                                autoTeachingParam.autoTeachingReflectorWidth = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingReflectorHeight:
                                autoTeachingParam.autoTeachingReflectorHeight = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingReflectorWidthErrorRange:
                                autoTeachingParam.autoTeachingReflectorErrorRangeWidth = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingReflectorHeightErrorRange:
                                autoTeachingParam.autoTeachingReflectorErrorRangeHeight = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingTargetSpeedX:
                                autoTeachingParam.autoTeachingTargetSpeedX = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingTargetSpeedZ:
                                autoTeachingParam.autoTeachingTargetSpeedZ = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingTargetAccDecX:
                                autoTeachingParam.autoTeachingTargetAccDecX = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.AutoTeachingTargetAccDecZ:
                                autoTeachingParam.autoTeachingTargetAccDecZ = float.Parse($"{dgvAutoTeachingParam[(int)DgvParamSettingColumn.Value, (int)autoTeaching].Value}");
                                break;

                            case AutoTeachingParameter.DoubleStorageSensorCheck:
                                DataGridViewComboBoxCell cboxDoubleStorage = (DataGridViewComboBoxCell)dgvAutoTeachingParam.Rows[(int)autoTeaching].Cells[(int)DgvParamSettingColumn.Value];
                                if ((CboxEnalbed)cboxDoubleStorage.Items.IndexOf(cboxDoubleStorage.Value) == CboxEnalbed.Enabled)
                                    autoTeachingParam.doubleStorageSensorCheck = true;
                                else
                                    autoTeachingParam.doubleStorageSensorCheck = false;
                                break;
                        }
                    }
                    if (!m_param.SetAutoTeachingParam(autoTeachingParam)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"AutoTeaching Parameter Save Failed"));
                        MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
                        return;
                    }
                }
                #endregion



                #region Timer Parameter
                RackMasterMain.RackMasterParam.TimerParameter timerParam = new RackMasterMain.RackMasterParam.TimerParameter();
                foreach (TimerParameter timer in Enum.GetValues(typeof(TimerParameter))) {
                    switch (timer) {
                        case TimerParameter.CIMTimerOver:
                            timerParam.CIM_TIMEOVER = Convert.ToInt32($"{dgvTimerParam[(int)DgvParamSettingColumn.Value, (int)timer].Value}");
                            break;

                        case TimerParameter.StepTimerOver:
                            timerParam.STEP_TEIMOVER = Convert.ToInt32($"{dgvTimerParam[(int)DgvParamSettingColumn.Value, (int)timer].Value}");
                            break;

                        case TimerParameter.PIOReadyTimerOver:
                            timerParam.PIO_READY_TIMOVER = Convert.ToInt32($"{dgvTimerParam[(int)DgvParamSettingColumn.Value, (int)timer].Value}");
                            break;

                        case TimerParameter.HomeMoveTimerOver:
                            timerParam.HOME_MOVE_TIMEOVER = Convert.ToInt32($"{dgvTimerParam[(int)DgvParamSettingColumn.Value, (int)timer].Value}");
                            break;

                        case TimerParameter.IoTimer:
                            timerParam.IO_TIMER = Convert.ToInt32($"{dgvTimerParam[(int)DgvParamSettingColumn.Value, (int)timer].Value}");
                            break;

                        case TimerParameter.EventTimer:
                            timerParam.EVENT_TIMEROVER = Convert.ToInt32($"{dgvTimerParam[(int)DgvParamSettingColumn.Value, (int)timer].Value}");
                            break;

                        case TimerParameter.AutoTeachingStepTimeOver:
                            timerParam.AUTO_TEACHING_STEP_TIMEOVER = Convert.ToInt32($"{dgvTimerParam[(int)DgvParamSettingColumn.Value, (int)timer].Value}");
                            break;
                    }
                }
                if (!m_param.SetTimerOverParam(timerParam)) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Timer Parameter Save Failed"));
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
                    return;
                }
                #endregion



                #region SCARA Parameter
                if(m_currentAccessType == Utility.PasswordType.Admin) {
                    RackMasterMain.RackMasterParam.ScaraParameter scaraParam = new RackMasterMain.RackMasterParam.ScaraParameter();
                    foreach (SCARAParameter scara in Enum.GetValues(typeof(SCARAParameter))) {
                        switch (scara) {
                            case SCARAParameter.RZLength:
                                scaraParam.RZ_LENGTH = Convert.ToInt32($"{dgvScaraParam[(int)DgvParamSettingColumn.Value, (int)scara].Value}");
                                break;

                            case SCARAParameter.RXLength:
                                scaraParam.RX_LENGTH = Convert.ToInt32($"{dgvScaraParam[(int)DgvParamSettingColumn.Value, (int)scara].Value}");
                                break;

                            case SCARAParameter.RYLength:
                                scaraParam.RY_LENGTH = Convert.ToInt32($"{dgvScaraParam[(int)DgvParamSettingColumn.Value, (int)scara].Value}");
                                break;
                        }
                    }
                    if (!m_param.SetScaraParameter(scaraParam)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"SCARA Parameter Save Failed"));
                        MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
                        return;
                    }
                }
                #endregion



                #region Full Closed Parameter
                if(m_currentAccessType == Utility.PasswordType.Admin) {
                    BarcodeParameter barcodeParam = new BarcodeParameter();
                    BarcodeSafetyParameter safetyParam = new BarcodeSafetyParameter();
                    foreach (FullClosedParameterList barcode in Enum.GetValues(typeof(FullClosedParameterList))) {
                        switch (barcode) {
                            case FullClosedParameterList.Axis:
                                barcodeParam.m_axis = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.BarcodeStartAddress:
                                barcodeParam.m_startAddr = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.BarcodeSize:
                                barcodeParam.m_size = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.BarcodeScale:
                                barcodeParam.m_barcodeScale = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.HomeOffset:
                                barcodeParam.m_homeOffset = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.SpecInType:
                                DataGridViewComboBoxCell cboxSpecInType = (DataGridViewComboBoxCell)dgvFullClosed.Rows[(int)barcode].Cells[(int)DgvParamSettingColumn.Value];
                                barcodeParam.m_specInType = (BarcodeSpecInType)cboxSpecInType.Items.IndexOf(cboxSpecInType.Value);
                                break;

                            case FullClosedParameterList.SpecInRange:
                                barcodeParam.m_specInRange = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.SpecInTime:
                                barcodeParam.m_specInTimeSec = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.Error1_StartAddress:
                                safetyParam.m_error1_StartAddr = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.Error1_Bit:
                                safetyParam.m_error1_Bit = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.Error2_StartAddress:
                                safetyParam.m_error2_StartAddr = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.Error2_Bit:
                                safetyParam.m_error2_Bit = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.Error3_StartAddress:
                                safetyParam.m_error3_StartAddr = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.Error3_Bit:
                                safetyParam.m_error3_Bit = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.Error4_StartAddress:
                                safetyParam.m_error4_StartAddr = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.Error4_Bit:
                                safetyParam.m_error4_Bit = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.EMO1_StartAddress:
                                safetyParam.m_EMO1_StartAddr = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.EMO1_Bit:
                                safetyParam.m_EMO1_Bit = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.EMO2_StartAddress:
                                safetyParam.m_EMO2_StartAddr = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.EMO2_Bit:
                                safetyParam.m_EMO2_Bit = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.EMO3_StartAddress:
                                safetyParam.m_EMO3_StartAddr = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.EMO3_Bit:
                                safetyParam.m_EMO3_Bit = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.EMO4_StartAddress:
                                safetyParam.m_EMO4_StartAddr = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.EMO4_Bit:
                                safetyParam.m_EMO4_Bit = int.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.AlarmStopDecTimeSecond:
                                safetyParam.m_alarmStopDecTimeSec = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.StopDecTimeSecond:
                                safetyParam.m_stopDecTimeSec = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.FollowingError:
                                safetyParam.m_followingError = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.VelocityLimit:
                                safetyParam.m_velocityLimit = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.AccLimit:
                                safetyParam.m_accLimit = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.DecLimit:
                                safetyParam.m_decLimit = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.UseBarcodePositiveLimit:
                                DataGridViewComboBoxCell cboxUsePosLimit = (DataGridViewComboBoxCell)dgvFullClosed.Rows[(int)barcode].Cells[(int)DgvParamSettingColumn.Value];
                                if ((CboxEnalbed)cboxUsePosLimit.Items.IndexOf(cboxUsePosLimit.Value) == CboxEnalbed.Enabled)
                                    safetyParam.m_useBarcodePositiveLimit = true;
                                else
                                    safetyParam.m_useBarcodePositiveLimit = false;
                                break;

                            case FullClosedParameterList.BarcodePositiveLimit:
                                safetyParam.m_barcodePositiveLimit = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.UseBarcodeNegativeLimit:
                                DataGridViewComboBoxCell cboxUseNegLimit = (DataGridViewComboBoxCell)dgvFullClosed.Rows[(int)barcode].Cells[(int)DgvParamSettingColumn.Value];
                                if ((CboxEnalbed)cboxUseNegLimit.Items.IndexOf(cboxUseNegLimit.Value) == CboxEnalbed.Enabled)
                                    safetyParam.m_useBarcodeNegativeLimit = true;
                                else
                                    safetyParam.m_useBarcodeNegativeLimit = false;
                                break;

                            case FullClosedParameterList.BarcodeNegativeLimit:
                                safetyParam.m_barcodeNegativeLimit = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                break;

                            case FullClosedParameterList.PGain:
                                double pGain = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                m_param.SetFullClosedPGain(pGain);
                                break;

                            case FullClosedParameterList.IGain:
                                double iGain = double.Parse($"{dgvFullClosed[(int)DgvParamSettingColumn.Value, (int)barcode].Value}");
                                m_param.SetFullClosedIGain(iGain);
                                break;
                        }
                    }
                    m_param.SetFullClosedParam(barcodeParam, safetyParam);
                }
                #endregion
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, Log.LogMessage_Main.UI_DataGridViewExceptionErrorOccurred, ex));
                MessageBox.Show(ex.Message);
                //MessageBox.Show("데이터 저장에 실패했습니다.");
                return;
            }

            if (!m_param.SaveAxisParameterFile() || !m_param.SaveSettingParameterFile()) {
                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Parameter Save Failed"));
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
                return;
            }

            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));

            return;
        }

        private void saveTimer_Tick(object sender, EventArgs e) {
            if (!m_saveStopwatch.IsTimerStarted())
                m_saveStopwatch.Restart();

            if (m_saveStopwatch.Delay(UICtrl.m_saveDelayTime)) {
                saveTimer.Stop();
                m_saveStopwatch.Stop();
                if (DialogResult.No == MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureParameterSave}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.YesNo)) {
                    return;
                }

                SaveParameter();
            }
        }

        public void DisableTAxis() {
            //foreach(AxisParameter row in Enum.GetValues(typeof(AxisParameter))) {
            //    m_dgvCtrl.SetCellDisableStyle(ref dgvAxisParam, (int)row, (int)DgvServoSettingsColumnIndex.STK_T, true);
            //}
        }

        public void EnableTAxis() {
            //dgvAxisParam.Rows.Clear();
            //InitDataGridView_AxisParam();
        }

        private void dgvAxisParam_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if(e.ColumnIndex != (int)DgvServoSettingsColumnIndex.Parameter) {
                AxisList axis = AxisList.X_Axis;
                if (e.ColumnIndex == (int)DgvServoSettingsColumnIndex.STK_Z)
                    axis = AxisList.Z_Axis;
                else if (e.ColumnIndex == (int)DgvServoSettingsColumnIndex.STK_A)
                    axis = AxisList.A_Axis;
                else if (e.ColumnIndex == (int)DgvServoSettingsColumnIndex.STK_T)
                    axis = AxisList.T_Axis;

                if (e.RowIndex == (int)AxisParameter.SoftwareLimitPositive) {
                    if (MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureSWPositiveLimit}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.OKCancel) == DialogResult.OK) {
                        if (e.ColumnIndex == (int)DgvServoSettingsColumnIndex.STK_Z && m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum) {
                            dgvAxisParam[e.ColumnIndex, e.RowIndex].Value = $"{m_main.m_motion.GetDrumBeltZAxisActualPosition() / 1000:F0}";
                        }
                        else {
                            dgvAxisParam[e.ColumnIndex, e.RowIndex].Value = $"{m_main.m_motion.GetAxisStatus(AxisStatusType.pos_act, axis) / 1000:F0}";
                        }
                        m_param.SetSoftwareLimitPositiveValue(axis, (float)m_main.m_motion.GetAxisStatus(AxisStatusType.pos_act, axis));
                    }
                }
                else if (e.RowIndex == (int)AxisParameter.SoftwareLimitNegative) {
                    if (MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureSWNegativeLimit}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.OKCancel) == DialogResult.OK) {
                        if (e.ColumnIndex == (int)DgvServoSettingsColumnIndex.STK_Z && m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum) {
                            dgvAxisParam[e.ColumnIndex, e.RowIndex].Value = $"{m_main.m_motion.GetDrumBeltZAxisActualPosition() / 1000:F0}";
                            //m_param.SetSoftwareLimitNegativeValue(AxisList.Z_Axis, (float)m_main.m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.Z_Axis));
                        }
                        else {
                            dgvAxisParam[e.ColumnIndex, e.RowIndex].Value = $"{m_main.m_motion.GetAxisStatus(AxisStatusType.pos_act, axis) / 1000:F0}";
                        }
                        m_param.SetSoftwareLimitNegativeValue(axis, (float)m_main.m_motion.GetAxisStatus(AxisStatusType.pos_act, axis));
                    }
                }
            }
        }

        public void AccessLoginType_Admin() {
            if (!tabParameter.TabPages.Contains(tabPageMotion))
                tabParameter.TabPages.Add(tabPageMotion);
            if (!tabParameter.TabPages.Contains(tabPageAutoTeaching))
                tabParameter.TabPages.Add(tabPageAutoTeaching);
            if (!tabParameter.TabPages.Contains(tabPageScara))
                tabParameter.TabPages.Add(tabPageScara);
            if (!tabParameter.TabPages.Contains(tabPageFullClosed))
                tabParameter.TabPages.Add(tabPageFullClosed);

            m_currentAccessType = Utility.PasswordType.Admin;
        }

        public void AccessLoginType_User() {
            if (tabParameter.TabPages.Contains(tabPageMotion))
                tabParameter.TabPages.Remove(tabPageMotion);
            if (tabParameter.TabPages.Contains(tabPageAutoTeaching))
                tabParameter.TabPages.Remove(tabPageAutoTeaching);
            if (tabParameter.TabPages.Contains(tabPageScara))
                tabParameter.TabPages.Remove(tabPageScara);
            if (tabParameter.TabPages.Contains(tabPageFullClosed))
                tabParameter.TabPages.Remove(tabPageFullClosed);

            m_currentAccessType = Utility.PasswordType.User;
        }
    }
}
