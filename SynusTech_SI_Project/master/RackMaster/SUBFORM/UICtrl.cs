using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SUBFORM {
    public enum LoadingType {
        StartLoading,
        AbsoluteAlarmClearLoading,
    }

    public enum UI_MessageBoxLangPackList {
        ExitProgram,
        DoNotHaveAccess,
        NoServoOn,
        ForkHomePositionCheck,
        ZAxisMaintStopperCheck,
        ValidValueCheck,
        PositiveLimitSensorOn,
        NegativeLimitSensorOn,
        MotorIsRunning,
        DataGridViewInvalidValue,
        ParameterSaveSuccess,
        ParameterSaveFailed,
        SureParameterSave,
        SureAddPort,
        SureDeletePort,
        TeachingZUpDownValueIsInvalid,
        TeachingDataSaveSuccess,
        SureSaveTeachingData,
        SureStartAutoTeaching,
        RefreshedParameterSuccess,
        RefreshedParameterFailed,
        ParameterLoadSuccess,
        ParameterLoadFailed,
        NotExistFunction,
        SureRefreshParameter,
        SureLoadParameter,
        InvalidPassword,
        PasswordIsOnlyNumber,
        CurrentPasswordNotCorrect,
        NewPasswordNotCorrect,
        LanguageCheck,
        PasswordIsChanged,
        UISettingSaved,
        LanguageIsChanged,
        LogFileListLoadFail,
        LogDateSelectedFail,
        LogDataLoadSuccess,
        SureSWPositiveLimit,
        SureSWNegativeLimit,
        SureHomingMode,
        HomingModeEnable,
        IsAutoState,
        EtherCATSettingSave,
        SystemSettingSave,
    }

    public enum UI_MessageBoxTitleLangPackList {
        ExitProgramTitle,
        Warning,
        Notify,
    }

    public class UICtrl {
        private const int m_blinkingTimeMilliseconds = 500;
        public const int m_saveDelayTime = 1000;

        public static readonly Color m_errorColor       = Color.OrangeRed;
        public static readonly Color m_onColor          = Color.YellowGreen;
        public static readonly Color m_enableColor      = Color.LightCyan;
        public static readonly Color m_selectedColor    = Color.SkyBlue;
        public static readonly Color m_disableColor     = Color.Gainsboro;
        public static readonly Color m_blinkOnColor     = Color.GreenYellow;
        public static readonly Color m_blinkOffColor    = Color.Red;
        public static readonly Color m_idleColor        = Color.White;
        public static readonly Color m_warningColor     = Color.Orange;

        public const string m_errorText                 = "Error";
        public const string m_disableText               = "Disabled";
        public const string m_idleText                  = "Idle";
        public const string m_onText                    = "On";
        public const string m_offText                   = "Off";
        public const string m_warningText               = "Warning";

        public static string GetButtonName(Button btn) {
            return btn.Name;
        }

        public static string GetLabelName(Label lbl) {
            return lbl.Name;
        }

        public static string GetDataGridViewName(DataGridView dgv) {
            return dgv.Name;
        }

        public static bool UI_IsIORawData() {
            return (Utility.GetIOViewType() == Utility.UI_IOViewType.RawData);
        }

        public class ButtonCtrl {
            private SEQ.CLS.Timer m_blinkTimer;
            private bool m_blinkState;

            public ButtonCtrl() {
                m_blinkTimer = new SEQ.CLS.Timer();
                m_blinkState = true;
            }

            public void SetSelectButtonStyle(ref Button btn, bool selected) {
                if (selected) {
                    btn.BackColor = m_selectedColor;
                }
                else {
                    btn.BackColor = m_enableColor;
                }
            }

            public void BlinkingButton(ref Button btn, bool isBlinking = true) {
                if (!isBlinking) {
                    btn.BackColor = m_idleColor;
                    return;
                }

                if (!m_blinkTimer.Delay(m_blinkingTimeMilliseconds))
                    return;

                m_blinkTimer.Restart();

                if (m_blinkState) {
                    btn.BackColor = m_blinkOffColor;
                    m_blinkState = false;
                }
                else {
                    btn.BackColor = m_blinkOnColor;
                    m_blinkState = true;
                }
            }

            public void SetButtonIdleStatus(ref Button btn)
            {
                btn.BackColor = m_idleColor;
            }

            public void SetButtonText(ref Button btn, string text) {
                btn.Text = text;
            }

            public void SetOnOffButtonStyle(ref Button btn, bool isOn) {
                if (isOn)
                    btn.BackColor = m_onColor;
                else
                    btn.BackColor = m_idleColor;
            }

            public void EnabledButton(ref Button btn, bool enabled) {
                btn.Enabled = enabled;
                if (enabled) {
                    btn.BackColor = m_idleColor;
                }
                else {
                    btn.BackColor = m_disableColor;
                }
            }
        }

        public class LabelCtrl {
            private SEQ.CLS.Timer m_blinkTimer;
            private bool m_blinkState;

            public LabelCtrl() {
                m_blinkTimer = new SEQ.CLS.Timer();
                m_blinkState = true;
            }

            public void SetErrorLabelStyle(ref Label label, bool isError) {
                if (isError)
                    label.BackColor = m_errorColor;
                else
                    label.BackColor = m_enableColor;
            }

            public void SetLabelText(ref Label label, string text) {
                label.Text = text;
            }

            public void BlinkingLabel(ref Label label, bool isBlinking = true) {
                if (!isBlinking)
                {
                    label.BackColor = m_idleColor;
                    return;
                }

                if (!m_blinkTimer.Delay(m_blinkingTimeMilliseconds))
                    return;

                m_blinkTimer.Restart();

                if (m_blinkState) {
                    label.BackColor = m_blinkOffColor;
                    m_blinkState = false;
                }
                else {
                    label.BackColor = m_blinkOnColor;
                    m_blinkState = true;
                }
            }

            public void SetOnOffLabelStyle(ref Label label, bool isOn) {
                if (isOn) {
                    label.BackColor = m_onColor;
                }
                else {
                    label.BackColor = m_idleColor;
                }
            }

            public void SetEnableLabelStyle(ref Label label, bool isEnabled) {
                if (isEnabled) {
                    label.BackColor = m_enableColor;
                }
                else {
                    label.BackColor = m_disableColor;
                }
            }
        }

        public class DataGridViewCtrl {
            private SEQ.CLS.Timer m_blinkTimer;
            private bool m_blinkState;

            public DataGridViewCtrl() {
                m_blinkTimer = new SEQ.CLS.Timer();
                m_blinkState = true;
            }

            public void DisableUserControl(ref DataGridView dgv) {
                dgv.AllowUserToAddRows          = false;
                dgv.AllowUserToResizeRows       = false;
                dgv.AllowUserToDeleteRows       = false;
                dgv.AllowUserToOrderColumns     = false;
                dgv.AllowUserToResizeColumns    = false;
            }

            public void SetReadonly(ref DataGridView dgv) {
                dgv.ReadOnly = true;
            }

            public void SetReadonlyRows(ref DataGridView dgv, int rowIndex) {
                dgv.Rows[rowIndex].ReadOnly = true;
            }

            public void SetReadonlyColumns(ref DataGridView dgv, int columnIndex) {
                dgv.Columns[columnIndex].ReadOnly = true;
            }

            public void SetReadonlyCell(ref DataGridView dgv, int rowIndex, int columnIndex) {
                dgv[columnIndex, rowIndex].ReadOnly = true;
            }

            public int AddRow(ref DataGridView dgv, string headerText) {
                int idx = dgv.Rows.Add();
                dgv.Rows[idx].HeaderCell.Value = headerText;

                return idx;
            }

            public int AddRow(ref DataGridView dgv, string value, int column) {
                int idx = dgv.Rows.Add();
                dgv[column, idx].Value = value;

                return idx;
            }

            public int AddRow(ref DataGridView dgv, DataGridViewComboBoxCell comboBox, int column) {
                int idx = dgv.Rows.Add();
                dgv.Rows[idx].Cells[column] = comboBox;

                return idx;
            }

            public int AddRow(ref DataGridView dgv, string headerText, DataGridViewComboBoxCell comboBox, int column) {
                int idx = dgv.Rows.Add();
                dgv.Rows[idx].HeaderCell.Value = headerText;
                dgv.Rows[idx].Cells[column] = comboBox;

                return idx;
            }

            public int AddRow(ref DataGridView dgv, string headerText, string value, int column) {
                int idx = dgv.Rows.Add();
                dgv.Rows[idx].HeaderCell.Value = headerText;
                dgv.Rows[idx].Cells[column].Value = value;

                return idx;
            }

            public int AddRow(ref DataGridView dgv, params string[] value) {
                try {
                    int idx = dgv.Rows.Add();
                    for (int i = 0; i < value.Length; i++) {
                        dgv.Rows[idx].Cells[i].Value = value[i];
                    }
                    return idx;
                }
                catch(Exception ex) {
                    return -1;
                }
            }

            public int AddColumn(ref DataGridView dgv, string columnName, string headerText) {
                return dgv.Columns.Add(columnName, headerText);
            }

            public void SetOnOffCell(ref DataGridView dgv, int rowIndex, int columnIndex, bool isOn) {
                if (isOn) {
                    dgv[columnIndex, rowIndex].Style.BackColor = m_onColor;
                    dgv[columnIndex, rowIndex].Value = m_onText;
                }
                else {
                    dgv[columnIndex, rowIndex].Style.BackColor = m_idleColor;
                    dgv[columnIndex, rowIndex].Value = m_offText;
                }
            }

            public void SetErrorCell(ref DataGridView dgv, int rowIndex, int columnIndex, bool isError, bool isTextChanged = true, string errorString = m_errorText) {
                if (isError) {
                    if (isTextChanged) {
                        dgv[columnIndex, rowIndex].Value = errorString;
                    }

                    if (m_blinkState) {
                        dgv[columnIndex, rowIndex].Style.BackColor = m_errorColor;
                    }
                    else {
                        dgv[columnIndex, rowIndex].Style.BackColor = m_idleColor;
                    }

                    if (!m_blinkTimer.Delay(m_blinkingTimeMilliseconds))
                        return;

                    m_blinkTimer.Restart();

                    if (m_blinkState) {
                        //dgv[columnIndex, rowIndex].Style.BackColor = m_errorColor;
                        m_blinkState = false;
                    }
                    else {
                        //dgv[columnIndex, rowIndex].Style.BackColor = m_idleColor;
                        m_blinkState = true;
                    }
                }
                else {
                    dgv[columnIndex, rowIndex].Style.BackColor = m_idleColor;
                    if (isTextChanged) {
                        dgv[columnIndex, rowIndex].Value = m_idleText;
                    }
                }
            }

            public void SetWarningCellStyle(ref DataGridView dgv, int rowIndex, int columnIndex, bool isWarning) {
                if (isWarning) {
                    dgv[columnIndex, rowIndex].Value = m_warningText;

                    if (m_blinkState) {
                        dgv[columnIndex, rowIndex].Style.BackColor = m_warningColor;
                    }
                    else {
                        dgv[columnIndex, rowIndex].Style.BackColor = m_idleColor;
                    }

                    if (!m_blinkTimer.Delay(m_blinkingTimeMilliseconds))
                        return;

                    m_blinkTimer.Restart();

                    if (m_blinkState) {
                        m_blinkState = false;
                    }
                    else {
                        m_blinkState = true;
                    }
                }
                else {
                    dgv[columnIndex, rowIndex].Style.BackColor = m_onColor;
                    dgv[columnIndex, rowIndex].Value = m_onText;
                }
            }

            public void SetCellStyle(ref DataGridView dgv, int rowIndex, int columnIndex, string text, Color color) {
                dgv[columnIndex, rowIndex].Style.BackColor = color;
                dgv[columnIndex, rowIndex].Value = text;
            }
            
            public void SetCellStyle(ref DataGridView dgv, int rowIndex, int columnIndex, Color color) {
                dgv[columnIndex, rowIndex].Style.BackColor = color;
            }

            public void SetCellStyle(ref DataGridView dgv, DataGridViewComboBoxCell cbox, int rowIndex, int columnIndex) {
                dgv.Rows[rowIndex].Cells[columnIndex] = cbox;
            }

            public void SetCellStyle(ref DataGridView dgv, DataGridViewButtonCell btn, int rowIndex, int columnIndex) {
                dgv.Rows[rowIndex].Cells[columnIndex] = btn;
            }

            public void SetCellText(ref DataGridView dgv, int rowIndex, int columnIndex, string text) {
                dgv[columnIndex, rowIndex].Value = text;
            }

            public void SetColumnHeaderText(ref DataGridView dgv, int columnIndex, string text) {
                dgv.Columns[columnIndex].HeaderText = text;
            }

            public void SetCellDisableStyle(ref DataGridView dgv, int rowIndex, int columnIndex) {
                dgv[columnIndex, rowIndex].Style.BackColor = m_disableColor;
                dgv[columnIndex, rowIndex].Value = m_disableText;
            }

            public bool IsCellDisabled(DataGridView dgv, int rowIndex, int columnIndex) {
                if (m_disableText.Equals($"{dgv[columnIndex, rowIndex].Value}"))
                    return true;

                return false;
            }
        }
    }
}
