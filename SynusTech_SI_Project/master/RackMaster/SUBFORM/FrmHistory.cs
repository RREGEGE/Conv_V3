using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;
using RackMaster.SEQ.PART;

namespace RackMaster.SUBFORM {
    public partial class FrmHistory : Form {
        private bool[] m_levelFilter;
        private bool[] m_typeFileter;

        private UICtrl.ButtonCtrl m_btnCtrl;
        private UICtrl.DataGridViewCtrl m_dgvCtrl;
        private Thread m_updateThread;

        private int m_historyRowsCount = 25;
        private int m_totalPage = 0;
        private int m_currentPage = 0;
        private List<string[]> m_logs;

        public FrmHistory() {
            InitializeComponent();

            m_btnCtrl = new UICtrl.ButtonCtrl();
            m_dgvCtrl = new UICtrl.DataGridViewCtrl();
            m_updateThread = null;

            int levelFilterCount = Enum.GetValues(typeof(Log.LogLevel)).Length;
            int typeFilterCount = Enum.GetValues(typeof(Log.LogType)).Length;

            m_levelFilter = new bool[levelFilterCount];
            m_typeFileter = new bool[typeFilterCount];

            for(int i = 0; i < m_levelFilter.Length; i++) {
                m_levelFilter[i] = true;
            }
            for(int i = 0; i < m_typeFileter.Length; i++) {
                m_typeFileter[i] = true;
            }

            m_logs = new List<string[]>();

            btnLevelError.Click         += FilterClickEvent;
            btnLevelNormal.Click        += FilterClickEvent;
            btnLevelWarning.Click       += FilterClickEvent;
            btnTypeCIM.Click            += FilterClickEvent;
            btnTypeRM.Click             += FilterClickEvent;
            btnTypeTCP.Click            += FilterClickEvent;
            btnTypeUI.Click             += FilterClickEvent;
            btnTypeUtility.Click        += FilterClickEvent;
            btnTypeWMX.Click            += FilterClickEvent;

            m_btnCtrl.SetOnOffButtonStyle(ref btnLevelError,        true);
            m_btnCtrl.SetOnOffButtonStyle(ref btnLevelNormal,       true);
            m_btnCtrl.SetOnOffButtonStyle(ref btnLevelWarning,      true);
            m_btnCtrl.SetOnOffButtonStyle(ref btnTypeCIM,           true);
            m_btnCtrl.SetOnOffButtonStyle(ref btnTypeRM,            true);
            m_btnCtrl.SetOnOffButtonStyle(ref btnTypeTCP,           true);
            m_btnCtrl.SetOnOffButtonStyle(ref btnTypeUI,            true);
            m_btnCtrl.SetOnOffButtonStyle(ref btnTypeUtility,       true);
            m_btnCtrl.SetOnOffButtonStyle(ref btnTypeWMX,           true);

            LoadLogFileList();

            cboxMonth.Enabled   = false;
            cboxDay.Enabled     = false;
            btnLogLoad.Enabled  = false;

            cboxYear.SelectedIndexChanged       += LogFileListComboBoxValueChanged;
            cboxMonth.SelectedIndexChanged      += LogFileListComboBoxValueChanged;
            cboxDay.SelectedIndexChanged        += LogFileListComboBoxValueChanged;

            foreach(DataGridViewColumn column in dgvLogHistory.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            m_btnCtrl.EnabledButton(ref btnPrevPage, false);
            m_btnCtrl.EnabledButton(ref btnNextPage, false);

            LanguageChanged();
        }

        private void LogFileListComboBoxValueChanged(object sender, EventArgs e) {
            ComboBox cbox = sender as ComboBox;

            if(cbox == cboxYear) {
                int year = (int)cboxYear.SelectedItem;
                int[] monthList = Log.GetLogFileList_Month(year);
                cboxDay.Items.Clear();
                cboxMonth.Items.Clear();
                cboxDay.Text = "";
                cboxMonth.Text = "";
                if (monthList == null || monthList.Length == 0) {
                    cboxDay.Enabled = false;
                    cboxMonth.Enabled = false;
                }
                else {
                    Array.Sort(monthList);
                    cboxDay.Enabled = false;
                    cboxMonth.Enabled = true;
                    cboxMonth.SelectedItem = "";
                    foreach (int month in monthList) {
                        cboxMonth.Items.Add(month);
                    }
                }
                btnLogLoad.Enabled = false;
            }
            else if(cbox == cboxMonth) {
                int year = (int)cboxYear.SelectedItem;
                int month = (int)cboxMonth.SelectedItem;
                int[] dayList = Log.GetLogFileList_Day(year, month);
                cboxDay.Items.Clear();
                cboxDay.Text = "";
                if (dayList == null || dayList.Length == 0) {
                    cboxDay.Enabled = false;
                }
                else {
                    Array.Sort(dayList);
                    cboxDay.Enabled = true;
                    cboxDay.SelectedItem = "";
                    foreach (int day in dayList) {
                        cboxDay.Items.Add(day);
                    }
                }
                btnLogLoad.Enabled = false;
            }
            else if(cbox == cboxDay) {
                int day = 0;
                if(int.TryParse($"{cboxDay.SelectedItem}", out day)) {
                    btnLogLoad.Enabled = true;
                }
            }
        }

        private void LoadLogFileList() {
            int[] yearList = Log.GetLogFileList_Year();
            if(yearList == null) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.LogFileListLoadFail}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }
            else {
                Array.Sort(yearList);
                foreach(int year in yearList) {
                    cboxYear.Items.Add(year);
                }
            }
        }

        private void FilterClickEvent(object sender, EventArgs e) {
            Button btn = sender as Button;

            if(btn == btnLevelError) {
                m_levelFilter[(int)Log.LogLevel.Error] = !m_levelFilter[(int)Log.LogLevel.Error];
                m_btnCtrl.SetOnOffButtonStyle(ref btn, m_levelFilter[(int)Log.LogLevel.Error]);
            }else if(btn == btnLevelNormal) {
                m_levelFilter[(int)Log.LogLevel.Normal] = !m_levelFilter[(int)Log.LogLevel.Normal];
                m_btnCtrl.SetOnOffButtonStyle(ref btn, m_levelFilter[(int)Log.LogLevel.Normal]);
            }else if(btn == btnLevelWarning) {
                m_levelFilter[(int)Log.LogLevel.Warning] = !m_levelFilter[(int)Log.LogLevel.Warning];
                m_btnCtrl.SetOnOffButtonStyle(ref btn, m_levelFilter[(int)Log.LogLevel.Warning]);
            }else if(btn == btnTypeCIM) {
                m_typeFileter[(int)Log.LogType.CIM] = !m_typeFileter[(int)Log.LogType.CIM];
                m_btnCtrl.SetOnOffButtonStyle(ref btn, m_typeFileter[(int)Log.LogType.CIM]);
            }else if(btn == btnTypeRM) {
                m_typeFileter[(int)Log.LogType.RackMaster] = !m_typeFileter[(int)Log.LogType.RackMaster];
                m_btnCtrl.SetOnOffButtonStyle(ref btn, m_typeFileter[(int)Log.LogType.RackMaster]);
            }else if(btn == btnTypeTCP) {
                m_typeFileter[(int)Log.LogType.TCP] = !m_typeFileter[(int)Log.LogType.TCP];
                m_btnCtrl.SetOnOffButtonStyle(ref btn, m_typeFileter[(int)Log.LogType.TCP]);
            }else if(btn == btnTypeUI) {
                m_typeFileter[(int)Log.LogType.UIControl] = !m_typeFileter[(int)Log.LogType.UIControl];
                m_btnCtrl.SetOnOffButtonStyle(ref btn, m_typeFileter[(int)Log.LogType.UIControl]);
            }else if(btn == btnTypeUtility) {
                m_typeFileter[(int)Log.LogType.Utility] = !m_typeFileter[(int)Log.LogType.Utility];
                m_btnCtrl.SetOnOffButtonStyle(ref btn, m_typeFileter[(int)Log.LogType.Utility]);
            }else if(btn == btnTypeWMX) {
                m_typeFileter[(int)Log.LogType.WMX] = !m_typeFileter[(int)Log.LogType.WMX];
                m_btnCtrl.SetOnOffButtonStyle(ref btn, m_typeFileter[(int)Log.LogType.WMX]);
            }
        }

        delegate void DataGridViewUpdate(List<string[]> data);
        private void btnLogLoad_Click(object sender, EventArgs e) {
            int year = 0;
            int month = 0;
            int day = 0;

            if(!int.TryParse($"{cboxYear.SelectedItem}", out year) || !int.TryParse($"{cboxMonth.SelectedItem}", out month) || !int.TryParse($"{cboxDay.SelectedItem}", out day)) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.LogDateSelectedFail}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            if(m_updateThread != null) {
                return;
            }

            dgvLogHistory.Rows.Clear();
            List<string[]> logs = Log.GetLogHistory(year, month, day);
            if(logs == null) {
                return;
            }

            m_updateThread = new Thread(delegate () {
                if (dgvLogHistory.InvokeRequired) {
                    dgvLogHistory.Invoke(new DataGridViewUpdate(UpdateLogHistory), logs);
                }
                else {
                    UpdateLogHistory(logs);
                }
                m_updateThread = null;
            });
            m_updateThread.IsBackground = true;
            m_updateThread.Name = $"Log History Update Thread";
            m_updateThread.Start();
            
            //foreach (string[] log in logs) {
            //    bool isFilter = true;
            //    foreach(Log.LogLevel level in Enum.GetValues(typeof(Log.LogLevel))) {
            //        if (!m_levelFilter[(int)level]) {
            //            if (log[(int)Log.LoggingOrder.Level].Equals($"{level}")) {
            //                isFilter = false;
            //                break;
            //            }
            //        }
            //    }
            //    if (!isFilter)
            //        continue;

            //    foreach(Log.LogType type in Enum.GetValues(typeof(Log.LogType))) {
            //        if (!m_typeFileter[(int)type]) {
            //            if (log[(int)Log.LoggingOrder.Type].Equals($"{type}")) {
            //                isFilter = false;
            //                break;
            //            }
            //        }
            //    }
            //    if (!isFilter)
            //        continue;

            //    int rowIdx = m_dgvCtrl.AddRow(ref dgvLogHistory, log);
            //}
            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.LogDataLoadSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
        }

        private void UpdateLogHistory(List<string[]> logs) {
            m_logs.Clear();

            foreach (string[] log in logs) {
                bool isFilter = true;
                foreach (Log.LogLevel level in Enum.GetValues(typeof(Log.LogLevel))) {
                    if (!m_levelFilter[(int)level]) {
                        if (log[(int)Log.LoggingOrder.Level].Equals($"{level}")) {
                            isFilter = false;
                            break;
                        }
                    }
                }
                if (!isFilter) {
                    logs.Remove(log);

                    continue;
                }

                foreach (Log.LogType type in Enum.GetValues(typeof(Log.LogType))) {
                    if (!m_typeFileter[(int)type]) {
                        if (log[(int)Log.LoggingOrder.Type].Equals($"{type}")) {
                            isFilter = false;
                            break;
                        }
                    }
                }
                if (!isFilter) {
                    logs.Remove(log);

                    continue; 
                }

                //int rowIdx = m_dgvCtrl.AddRow(ref dgvLogHistory, log);
            }

            m_logs = logs;

            if(m_historyRowsCount > m_logs.Count) {
                for (int i = 0; i < m_logs.Count; i++) {
                    m_dgvCtrl.AddRow(ref dgvLogHistory, m_logs[i]);
                }
            }
            else {
                for (int i = 0; i < m_historyRowsCount; i++) {
                    m_dgvCtrl.AddRow(ref dgvLogHistory, m_logs[i]);
                }
            }

            m_totalPage = m_logs.Count / m_historyRowsCount;
            m_currentPage = 0;

            m_btnCtrl.EnabledButton(ref btnPrevPage, false);
            m_btnCtrl.EnabledButton(ref btnNextPage, true);
        }

        public void LanguageChanged() {
            //gboxFilterLogLevel.Text     = SynusLangPack.GetLanguage(gboxFilterLogLevel.Name);
            //gboxFilterLogType.Text      = SynusLangPack.GetLanguage(gboxFilterLogType.Name);
            //gboxLogDate.Text            = SynusLangPack.GetLanguage(gboxLogDate.Name);
            //gboxLogHistory.Text         = SynusLangPack.GetLanguage(gboxLogHistory.Name);

            //m_btnCtrl.SetButtonText(ref btnLogLoad, SynusLangPack.GetLanguage(btnLogLoad.Name));
        }

        private void btnPrevPage_Click(object sender, EventArgs e) {
            if (m_currentPage == 0)
                return;

            dgvLogHistory.Rows.Clear();

            for(int i = 0; i < m_historyRowsCount; i++) {
                int index = m_historyRowsCount * (m_currentPage - 1) + i;

                if (m_logs.Count <= index)
                    break;

                m_dgvCtrl.AddRow(ref dgvLogHistory, m_logs[index]);
            }

            m_currentPage -= 1;

            if(m_currentPage == 0) {
                m_btnCtrl.EnabledButton(ref btnPrevPage, false);
                m_btnCtrl.EnabledButton(ref btnNextPage, true);
            }
            else {
                m_btnCtrl.EnabledButton(ref btnPrevPage, true);
                m_btnCtrl.EnabledButton(ref btnNextPage, true);
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e) {
            if (m_currentPage == m_totalPage)
                return;

            dgvLogHistory.Rows.Clear();

            for(int i = 0; i < m_historyRowsCount; i++) {
                int index = m_historyRowsCount * (m_currentPage + 1) + i;

                if (m_logs.Count <= index)
                    break;

                m_dgvCtrl.AddRow(ref dgvLogHistory, m_logs[index]);
            }

            m_currentPage += 1;

            if (m_currentPage == m_totalPage) {
                m_btnCtrl.EnabledButton(ref btnPrevPage, true);
                m_btnCtrl.EnabledButton(ref btnNextPage, false);
            }
            else {
                m_btnCtrl.EnabledButton(ref btnPrevPage, true);
                m_btnCtrl.EnabledButton(ref btnNextPage, true);
            }
        }
    }
}
