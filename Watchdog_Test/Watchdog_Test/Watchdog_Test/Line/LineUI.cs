using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Watchdog_Test.Form1;

namespace Watchdog_Test
{
    partial class Line
    {
        public void Initialize_Watchdog_btn(Button btn)
        {
            btn.Text = m_LineParameter.ID.ToString();
            btn.BackColor = Color.FromArgb(24, 30, 54);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Serif", 12f, FontStyle.Bold);
            btn.AutoSize = true;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Dock = DockStyle.Fill;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Margin = new Padding(left:1, top:3, right:1, bottom:2);
        }
        public void UpdateWatchdogSettings(DataGridView DGV)
        {
                DGV.Rows.Clear();
                for (int nRowCount = 0; nRowCount < Enum.GetValues(typeof(WatchdogList)).Length; nRowCount++)
                {
                //WatchdogList eWatchdog = (WatchdogList)nRowCount;
                //DGV.Rows.Add(new string[4] { $"{eWatchdog}",
                //                                Watchdog_GetParam_DetectTime(eWatchdog).ToString(),
                //                                Watchdog_GetParam_DetectTime(eWatchdog).ToString(),
                //                                "Set" });
                int rowIndex = DGV.Rows.Add();
                DataGridViewRow row = DGV.Rows[rowIndex];

                WatchdogList eWatchdog = (WatchdogList)nRowCount;

                row.Cells[(int)DGV_WatchdogSettingsColumn.Name].Value = $"{eWatchdog}";
                row.Cells[(int)DGV_WatchdogSettingsColumn.AppliedValue].Value = Watchdog_GetParam_DetectTime(eWatchdog).ToString();
                row.Cells[(int)DGV_WatchdogSettingsColumn.SetValue].Value = Watchdog_GetParam_DetectTime(eWatchdog).ToString();
                row.Cells[(int)DGV_WatchdogSettingsColumn.SetValue].ReadOnly = false;
                row.Cells[(int)DGV_WatchdogSettingsColumn.Btn].Value = "Set";
                }
            
        }
    }
}
