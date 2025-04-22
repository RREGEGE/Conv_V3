using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Watchdog_Test.Line;

namespace Watchdog_Test
{

    public partial class Form1 : Form
    {
        public Line line1 = new Line("synustech");
        public Line line2 = new Line("상일");
        public List<Line> lines;
        public enum DGV_WatchdogSettingsColumn
        {
            Name,
            AppliedValue,
            SetValue,
            Btn,
        }
        public Form1()
        {
            InitializeComponent();
            SynusLangPack.LoadFile(ManagedFileInfo.LangPackDirectory, ManagedFileInfo.LangPackFileName);
            InitLines();
            Initailize_Line_Menu();
            InitializeWatchdogDGV(ref dgWatchdogView);
        }
        private void InitLines()
        {
            lines = new List<Line>();
            lines.Add(line1);
            lines.Add(line2);
        }
        // Line Label 생성
        private void Initailize_Line_Menu()
        {
            foreach(Line line in lines)
            {
                Button btn = new Button();
                line.Initialize_Watchdog_btn(btn);
                btn.Click += (sender, e) => 
                {
                    SetLabelColor(btn);
                    line.UpdateWatchdogSettings(dgWatchdogView);
                };
                tPanelLine.Controls.Add(btn);
            }
        }
        // Line Label 추가 
        public void Add_Line_Menu(Line line)
        {
            Button btn = new Button();
            line.Initialize_Watchdog_btn(btn);
            btn.Click += (sender, e) =>
            {
                SetLabelColor(btn);
                line.UpdateWatchdogSettings(dgWatchdogView);
            };
            tPanelLine.Controls.Add(btn);
        }
        // Watchdog DataGridView HeaderColums 초기화 
        public void InitializeWatchdogDGV(ref DataGridView DGV)
        {
            foreach (DGV_WatchdogSettingsColumn col in Enum.GetValues(typeof(DGV_WatchdogSettingsColumn)))
            {
                if (col == DGV_WatchdogSettingsColumn.Btn)
                {
                    DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
                    btnCol.Name = col.ToString();
                    btnCol.HeaderText = col.ToString();
                    btnCol.Text = "Set";
                    btnCol.UseColumnTextForButtonValue = true;
                    DGV.Columns.Add(btnCol);
                }
                else
                {
                    var dgvCol = new DataGridViewTextBoxColumn
                    {
                        Name = col.ToString(),
                        HeaderText = col.ToString(),
                    };
                    DGV.Columns.Add(dgvCol);
                }
            }
            foreach (DataGridViewColumn col in DGV.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            DGV.Rows.Clear();
            for (int nRowCount = 0; nRowCount < Enum.GetValues(typeof(WatchdogList)).Length; nRowCount++)
            {
                int rowIndex = DGV.Rows.Add();
                DataGridViewRow row = DGV.Rows[rowIndex];
                row.ReadOnly = true;
                WatchdogList eWatchdog = (WatchdogList)nRowCount;

                row.Cells[(int)DGV_WatchdogSettingsColumn.Name].Value = $"{eWatchdog}";
            }
                UpdateWatchdogSettings(ref DGV);
        }
        // SynusLangPack에 맞게 HeaderColumns 수정
        public void UpdateWatchdogSettings(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_WatchdogSettingsColumn.Name:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_WatchdogList"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_WatchdogList");
                        break;
                    case (int)DGV_WatchdogSettingsColumn.AppliedValue:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_AppliedDetectTime"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_AppliedDetectTime");
                        break;
                    case (int)DGV_WatchdogSettingsColumn.SetValue:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SetDetectTime"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SetDetectTime");
                        break;
                }
            }
        }
        // 클릭한 버튼을 제외한 다른 버튼 색상 초기화
        public void SetLabelColor(Button btn)
        {
            foreach(var control in tPanelLine.Controls)
            {
                if (control is Button otherBtn)
                {
                    otherBtn.BackColor = Color.FromArgb(24, 30, 54);
                    otherBtn.ForeColor = Color.White;
                }
            }
            btn.BackColor = Color.FromArgb(37, 41, 64);
            btn.ForeColor = Color.FromArgb(0, 126, 249);
        }
        
        private void btnWatchdogSave_Click(object sender, EventArgs e)
        {
            Line line3 = new Line("123");
            lines.Add(line3);
            Add_Line_Menu(line3);
        }

    }
}