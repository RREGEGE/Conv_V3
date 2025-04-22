using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synustech.BcForm.ucPanel.BcSetting
{
    public partial class Watchdog : UserControl
    {
        private int currentlblCount = 0;
        public Watchdog()
        {
            InitializeComponent();
            Initialize_Line_Menu();
            InitializeWatchdogDGV(ref dgWatchdogView);
        }
        private void Initialize_Line_Menu()
        {
            foreach(var line in G_Var.lines)
            {
                Button btn = new Button();
                line.Initialize_Watchdog_btn(btn);
                tPanelLine.Controls.Add(btn,currentlblCount,0);
                currentlblCount++;
            }
        }
        public void Add_Line_Menu(Line line)
        {
            Button btn = new Button();
            line.Initialize_Watchdog_btn(btn);
            tPanelLine.Controls.Add(btn, currentlblCount, 0);
            currentlblCount++;
        }
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
                    DGV.Columns.Add(col.ToString(), col.ToString());
                }
            }
            foreach (DataGridViewColumn col in DGV.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            UpdateWatchdogSettings(ref DGV);
        }
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
    }
}
