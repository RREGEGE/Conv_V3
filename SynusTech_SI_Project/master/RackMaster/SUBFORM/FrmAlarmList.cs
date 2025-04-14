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

namespace RackMaster.SUBFORM {
    public partial class FrmAlarmList : Form {
        private enum Dgv_Alarm_Column {
            No,
            Alarm
        }

        private enum AlarmLanguageType {
            Cause,
            Action,
            Reserved,
        }

        private RackMasterMain m_main;
        private RackMasterMain.RackMasterAlarm m_alarm;

        private UICtrl.DataGridViewCtrl m_dgvCtrl;

        private bool m_isTestMode;

        public FrmAlarmList(RackMasterMain main) {
            m_main = main;
            m_alarm = m_main.m_alarm;

            m_dgvCtrl = new UICtrl.DataGridViewCtrl();

            InitializeComponent();

            InitDataGridViewAlarmList();

            testModeMenu.Click += TestMenu_Click;
            m_isTestMode = false;

            this.VisibleChanged += ThisFormVisibleChangedEvent;
        }

        private void ThisFormVisibleChangedEvent(object sender, EventArgs e) {
            testModeMenu.Text = $"Test Mode On";
            m_isTestMode = false;
        }

        private void InitDataGridViewAlarmList() {
            for (int i = 0; i <= (int)AlarmList.MAX; i++) {
                m_dgvCtrl.AddRow(ref dgvAlarmList, $"{i}", (int)Dgv_Alarm_Column.No);
                m_dgvCtrl.SetCellText(ref dgvAlarmList, i, (int)Dgv_Alarm_Column.Alarm, m_alarm.GetAlarmString((AlarmList)i));
            }
            m_dgvCtrl.SetReadonly(ref dgvAlarmList);
            m_dgvCtrl.DisableUserControl(ref dgvAlarmList);
            foreach (DataGridViewColumn column in dgvAlarmList.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void UpdateFormUI() {
            dgvAlarmList.ClearSelection();

            UpdateDataGridView_AlarmList();
        }

        private void UpdateDataGridView_AlarmList() {
            foreach (AlarmList alarm in Enum.GetValues(typeof(AlarmList))) {
                //m_dgvCtrl.SetErrorCell(ref dgvAlarmList, (int)alarm, (int)Dgv_Alarm_Column.No, m_alarm.IsCurrentAlarmContainAt(alarm), false);
                m_dgvCtrl.SetErrorCell(ref dgvAlarmList, (int)alarm, (int)Dgv_Alarm_Column.Alarm, m_alarm.IsCurrentAlarmContainAt(alarm), false);
            }
        }

        private void TestMenu_Click(object sender, EventArgs e) {
            if (m_isTestMode) {
                testModeMenu.Text = $"Test Mode On";
                m_isTestMode = false;
            }
            else {
                testModeMenu.Text = $"Test Mode Off";
                m_isTestMode = true;
            }
        }

        private void dgvAlarmList_CellClick(object sender, DataGridViewCellEventArgs e) {
            try {
                foreach(AlarmList alarm in Enum.GetValues(typeof(AlarmList))) {
                    if(e.RowIndex == (int)alarm) {
                        if (m_isTestMode) {
                            m_alarm.AddAlarm((AlarmList)e.RowIndex, AlarmState.Alarm);
                        }
                        txtAlarmCause.Text = SynusLangPack.GetLanguage($"{alarm}_{AlarmLanguageType.Cause}");
                        txtAlarmAction.Text = SynusLangPack.GetLanguage($"{alarm}_{AlarmLanguageType.Action}");
                        return;
                    }
                    else {
                        txtAlarmCause.Text = SynusLangPack.GetLanguage($"{AlarmLanguageType.Reserved}");
                        txtAlarmAction.Text = SynusLangPack.GetLanguage($"{AlarmLanguageType.Reserved}");
                    }
                }
            }catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
