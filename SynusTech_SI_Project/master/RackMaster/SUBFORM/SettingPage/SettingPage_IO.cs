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
using RackMaster.SUBFORM;

namespace RackMaster.SUBFORM.SettingPage {
    public partial class SettingPage_IO : Form {
        private enum DgvInputSettingsColumn {
            Parameter,
            ByteAddr,
            BitAddr,
            BInvert,
            Enabled,
        }

        private enum DgvOutputSettingsColumn {
            Parameter,
            ByteAddr,
            BitAddr,
            Enabled,
        }

        private enum CboxEnalbed {
            Enabled,
            Disabled,
        }

        private UICtrl.DataGridViewCtrl m_dgvCtrl;
        private UICtrl.ButtonCtrl m_btnCtrl;
        private RackMasterMain m_rackMaster;
        private RackMasterMain.RackMasterParam m_param;
        private SEQ.CLS.Timer m_saveStopwatch;

        public SettingPage_IO(RackMasterMain rackMaster) {
            InitializeComponent();

            m_rackMaster = rackMaster;
            m_param = m_rackMaster.m_param;
            m_dgvCtrl = new UICtrl.DataGridViewCtrl();
            m_btnCtrl = new UICtrl.ButtonCtrl();
            m_saveStopwatch = new SEQ.CLS.Timer();
            m_saveStopwatch.Stop();

            InitDataGridView_IoParam();

            btnIoSave.MouseDown     += SaveMouseDown;
            btnIoSave.MouseUp       += SaveMouseUp;
        }

        public void UpdateFormUI() {
            if(m_rackMaster.GetCurrentSettingMode() == SettingMode.Demo) {
                m_btnCtrl.SetOnOffButtonStyle(ref btnDemoMode, true);
            }
            else {
                m_btnCtrl.SetOnOffButtonStyle(ref btnDemoMode, false);
            }
        }

        private void InitDataGridView_IoParam() {
            try {
                int rowIdx = 0;
                foreach (InputList input in Enum.GetValues(typeof(InputList))) {
                    switch (input) {
                        case InputList.RM_MC_On:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "RM MC On", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.EMO_HP:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "EMO HP", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.EMO_OP:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "EMO OP", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.HP_DTP_EMS_SW:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "HP DTP EMS SW", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.HP_DTP_DeadMan_SW:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "HP DTP DeadMan SW", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.HP_DTP_Mode_Select_SW_1:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "HP DTP Mode Select SW 1", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.HP_DTP_Mode_Select_SW_2:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "HP DTP Mode Select SW 2", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.OP_DTP_EMS_SW:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "OP DTP EMS SW", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.OP_DTP_DeadMan_SW:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "OP DTP DeadMan SW", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.OP_DTP_Mode_Select_SW_1:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "OP DTP Mode Select SW 1", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.OP_DTP_Mode_Select_SW_2:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "OP DTP Mode Select SW 2", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Z_Axis_Maint_Stopper_Sensor_1:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Z Axis Maint Stopper Sensor 1", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Z_Axis_Maint_Stopper_Sensor_2:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Z Axis Maint Stopper Sensor 2", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Z_Axis_Wire_Cut_Sensor:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Z Axis Wire Cut Sensor", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Presense_Detection_1:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Presense Detection 1", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Presense_Detection_2:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Presense Detection 2", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Double_Storage_Sensor_1:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Double Storage Sensor 1", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Double_Storage_Sensor_2:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Double Storage Sensor 2", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Fork_Pick_Sensor_Left:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Fork Pick Sensor Left", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Fork_Place_Sensor_Left:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Fork Place sensor Left", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Fork_Pick_Sensor_Right:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Fork Pick Sensor Right", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Fork_Place_Sensor_Right:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Fork Place Sensor Right", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Fork_In_Place_1:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Placement Sensor 1", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.Fork_In_Place_2:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Placement Sensor 2", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.StickDetectSensor_1:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Stick Detect Sensor 1", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.StickDetectSensor_2:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Stick Detect Sensor 2", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.StickDetectSensor_3:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Stick Detect Sensor 3", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.StickDetectSensor_4:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "Stick Detect Sensor 4", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.X_Axis_Position_Sensor_1:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "X Axis Position Sensor 1", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.X_Axis_Position_Sensor_2:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "X Axis Position Sensor 2", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.X_Axis_HP_Moving_Speed_Cut_Neg:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "X Axis HP Moving Speed Cut Neg", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.X_Axis_HP_Moving_Speed_Cut_Pos:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "X Axis HP Moving Speed Cut Pos", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.CPS_Regulator_Run:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "CPS Regulator Run", (int)DgvInputSettingsColumn.Parameter);
                            break;

                        case InputList.CPS_Regulator_Fault:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvInputList, "CPS Regulator Fault", (int)DgvInputSettingsColumn.Parameter);
                            break;
                    }
                    SetCellStyleInputParameter(input, rowIdx);
                }

                foreach (OutputList output in Enum.GetValues(typeof(OutputList))) {
                    switch (output) {
                        case OutputList.Handy_Touch_EMO_Relay_On:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvOutputList, "Handy Touch EMO Relay On", (int)DgvOutputSettingsColumn.Parameter);
                            break;

                        case OutputList.Voice_Buzzer_CH1:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvOutputList, "Voice Buzzer CH1", (int)DgvOutputSettingsColumn.Parameter);
                            break;

                        case OutputList.Voice_Buzzer_CH2:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvOutputList, "Voice Buzzer CH2", (int)DgvOutputSettingsColumn.Parameter);
                            break;

                        case OutputList.Voice_Buzzer_CH3:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvOutputList, "Voice Buzzer CH3", (int)DgvOutputSettingsColumn.Parameter);
                            break;

                        case OutputList.Voice_Buzzer_CH4:
                            rowIdx = m_dgvCtrl.AddRow(ref dgvOutputList, "Voice Buzzer CH4", (int)DgvOutputSettingsColumn.Parameter);
                            break;
                    }
                    SetCellStyleOutputParameter(output, rowIdx);
                }

                m_dgvCtrl.DisableUserControl(ref dgvInputList);
                m_dgvCtrl.DisableUserControl(ref dgvOutputList);

                foreach (DataGridViewColumn column in dgvInputList.Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                foreach (DataGridViewColumn column in dgvOutputList.Columns) {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, "Load IO Parameter Fail", ex));
            }
        }

        private void SetCellStyleInputParameter(InputList input, int rowIdx) {
            DataGridViewComboBoxCell cboxBInvert = new DataGridViewComboBoxCell();
            cboxBInvert.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            foreach (CboxEnalbed type in Enum.GetValues(typeof(CboxEnalbed))) {
                cboxBInvert.Items.Add($"{type}");
            }
            DataGridViewComboBoxCell cboxEnabled = new DataGridViewComboBoxCell();
            cboxEnabled.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            foreach (CboxEnalbed type in Enum.GetValues(typeof(CboxEnalbed))) {
                cboxEnabled.Items.Add($"{type}");
            }
            m_dgvCtrl.SetCellText(ref dgvInputList, rowIdx, (int)DgvInputSettingsColumn.ByteAddr, $"{m_param.GetInputParameter(input).byteAddr}");
            m_dgvCtrl.SetCellText(ref dgvInputList, rowIdx, (int)DgvInputSettingsColumn.BitAddr, $"{m_param.GetInputParameter(input).bitAddr}");
            if (m_param.GetInputParameter(input).isBInvert) {
                cboxBInvert.Value = $"{CboxEnalbed.Enabled}";
            }
            else {
                cboxBInvert.Value = $"{CboxEnalbed.Disabled}";
            }
            m_dgvCtrl.SetCellStyle(ref dgvInputList, cboxBInvert, rowIdx, (int)DgvInputSettingsColumn.BInvert);
            if (m_param.GetInputParameter(input).isEnabled) {
                cboxEnabled.Value = $"{CboxEnalbed.Enabled}";
            }
            else {
                cboxEnabled.Value = $"{CboxEnalbed.Disabled}";
            }
            m_dgvCtrl.SetCellStyle(ref dgvInputList, cboxEnabled, rowIdx, (int)DgvInputSettingsColumn.Enabled);
        }

        private void SetCellStyleOutputParameter(OutputList output, int rowIdx) {
            DataGridViewComboBoxCell cboxEnabled = new DataGridViewComboBoxCell();
            cboxEnabled.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            foreach (CboxEnalbed type in Enum.GetValues(typeof(CboxEnalbed))) {
                cboxEnabled.Items.Add($"{type}");
            }
            m_dgvCtrl.SetCellText(ref dgvOutputList, rowIdx, (int)DgvOutputSettingsColumn.ByteAddr, $"{m_param.GetOutputParameter(output).byteAddr}");
            m_dgvCtrl.SetCellText(ref dgvOutputList, rowIdx, (int)DgvOutputSettingsColumn.BitAddr, $"{m_param.GetOutputParameter(output).bitAddr}");
            if (m_param.GetOutputParameter(output).isEnabled) {
                cboxEnabled.Value = $"{CboxEnalbed.Enabled}";
            }
            else {
                cboxEnabled.Value = $"{CboxEnalbed.Disabled}";
            }
            m_dgvCtrl.SetCellStyle(ref dgvOutputList, cboxEnabled, rowIdx, (int)DgvOutputSettingsColumn.Enabled);
        }

        private void SaveMouseDown(object sender, MouseEventArgs e) {
            if (!CheckDataGridView_IoParameter()) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.DataGridViewInvalidValue}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            saveTimer.Start();
        }

        private void SaveMouseUp(object sender, MouseEventArgs e) {
            saveTimer.Stop();
            m_saveStopwatch.Stop();
        }

        private bool CheckDataGridView_IoParameter() {
            for (int row = 0; row < dgvInputList.Rows.Count; row++) {
                int byteAddr;
                int bitAddr;

                if (!int.TryParse($"{dgvInputList[(int)DgvInputSettingsColumn.ByteAddr, row].Value}", out byteAddr) || byteAddr < 0) {
                    return false;
                }
                if (!int.TryParse($"{dgvInputList[(int)DgvInputSettingsColumn.BitAddr, row].Value}", out bitAddr) || bitAddr < 0 || bitAddr > 7) {
                    return false;
                }
            }

            for (int row = 0; row < dgvOutputList.Rows.Count; row++) {
                int byteAddr;
                int bitAddr;

                if (!int.TryParse($"{dgvOutputList[(int)DgvOutputSettingsColumn.ByteAddr, row].Value}", out byteAddr) || byteAddr < 0) {
                    return false;
                }
                if (!int.TryParse($"{dgvOutputList[(int)DgvOutputSettingsColumn.BitAddr, row].Value}", out bitAddr) || bitAddr < 0 || bitAddr > 7) {
                    return false;
                }
            }

            return true;
        }

        private void SaveIoParameter() {
            try {
                foreach (InputList input in Enum.GetValues(typeof(InputList))) {
                    m_param.GetInputParameter(input).byteAddr = short.Parse($"{dgvInputList[(int)DgvInputSettingsColumn.ByteAddr, (int)input].Value}");
                    m_param.GetInputParameter(input).bitAddr = byte.Parse($"{dgvInputList[(int)DgvInputSettingsColumn.BitAddr, (int)input].Value}");
                    DataGridViewComboBoxCell cboxBInvert = (DataGridViewComboBoxCell)dgvInputList.Rows[(int)input].Cells[(int)DgvInputSettingsColumn.BInvert];
                    if ((CboxEnalbed)cboxBInvert.Items.IndexOf(cboxBInvert.Value) == CboxEnalbed.Enabled) {
                        m_param.GetInputParameter(input).isBInvert = true;
                    }
                    else {
                        m_param.GetInputParameter(input).isBInvert = false;
                    }
                    DataGridViewComboBoxCell cboxEnabled = (DataGridViewComboBoxCell)dgvInputList.Rows[(int)input].Cells[(int)DgvInputSettingsColumn.Enabled];
                    if ((CboxEnalbed)cboxEnabled.Items.IndexOf(cboxEnabled.Value) == CboxEnalbed.Enabled) {
                        m_param.GetInputParameter(input).isEnabled = true;
                    }
                    else {
                        m_param.GetInputParameter(input).isEnabled = false;
                    }
                }

                foreach (OutputList output in Enum.GetValues(typeof(OutputList))) {
                    m_param.GetOutputParameter(output).byteAddr = short.Parse($"{dgvOutputList[(int)DgvOutputSettingsColumn.ByteAddr, (int)output].Value}");
                    m_param.GetOutputParameter(output).bitAddr = byte.Parse($"{dgvOutputList[(int)DgvOutputSettingsColumn.BitAddr, (int)output].Value}");
                    DataGridViewComboBoxCell cboxEnabled = (DataGridViewComboBoxCell)dgvOutputList.Rows[(int)output].Cells[(int)DgvOutputSettingsColumn.Enabled];
                    if ((CboxEnalbed)cboxEnabled.Items.IndexOf(cboxEnabled.Value) == CboxEnalbed.Enabled) {
                        m_param.GetOutputParameter(output).isEnabled = true;
                    }
                    else {
                        m_param.GetOutputParameter(output).isEnabled = false;
                    }
                }

                if(m_rackMaster.GetCurrentSettingMode() == SettingMode.Setup) {
                    bool ret = m_param.SaveIoParameter();
                    if (ret) {
                        MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
                        //Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.UIControl, $"IO Parameter Save Succes"));
                    }
                    else {
                        MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
                        //Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.UIControl, $"IO Parameter Save Fail"));
                    }
                }
                else {
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.UIControl, $"IO Parameter Set Success_Demo Version"));
                }

            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, $"IO Parameter Save Fail", ex));
            }
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

                FrmLogin frmLogin = new FrmLogin(false);
                frmLogin.loginEvent += AccessLogin_SaveButton;
                frmLogin.ShowDialog();
            }
        }

        private void AccessLogin_SaveButton(Utility.PasswordType type) {
            if(type == Utility.PasswordType.Admin) {
                SaveIoParameter();
            }
            else {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.DoNotHaveAccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
            }
        }

        private void btnDemoMode_Click(object sender, EventArgs e) {
            if(m_rackMaster.GetCurrentSettingMode() == SettingMode.Demo) {
                dgvInputList.Rows.Clear();
                dgvOutputList.Rows.Clear();
                m_rackMaster.m_param.RefreshIOParameter();
                InitDataGridView_IoParam();

                m_rackMaster.SetSettingMode(SettingMode.Setup);
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.UIControl, $"IO Setting Mode is Changed, Demo -> Setup"));

            }else if(m_rackMaster.GetCurrentSettingMode() == SettingMode.Setup) {
                FrmLogin frmLogin = new FrmLogin(false);
                frmLogin.loginEvent += AccessLogin_DemoModeButton;
                frmLogin.ShowDialog();
            }
        }


        private void AccessLogin_DemoModeButton(Utility.PasswordType type) {
            if(type == Utility.PasswordType.Admin) {
                m_rackMaster.SetSettingMode(SettingMode.Demo);
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.UIControl, $"IO Setting Mode is Changed, Setup -> Demo"));
            }
            else {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.DoNotHaveAccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
            }
        }
    }
}
