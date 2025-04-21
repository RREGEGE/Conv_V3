using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Watchdog_Test.Port;

namespace Watchdog_Test
{

    public partial class Form1 : Form
    {
        public Port_WatchdogParam WatchdogDetectParam = new Port_WatchdogParam();
        public class Port_WatchdogParam
        {
            public int T_Axis_HomingTimer = 1000;
            public int T_Axis_Move_To_LoadTimer = 1000;
            public int T_Axis_Move_To_UnLoadTimer = 1000;
            public int OHT_HoistDetectTimer = 1000;
            public int AGVorOHT_PIO_Timer = 1000;
            public int Init_Step_Timer = 1000;
            public int IN_Step_Timer = 1000;
            public int OUT_Step_Timer = 1000;
        }
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
            Init_DGVWatchdog();
        }

        private void Init_DGVWatchdog()
        {
            SynusLangPack.LoadFile(ManagedFileInfo.LangPackDirectory, ManagedFileInfo.LangPackFileName);
            foreach (DGV_WatchdogSettingsColumn col in Enum.GetValues(typeof(DGV_WatchdogSettingsColumn)))
            {
                if (col == DGV_WatchdogSettingsColumn.Btn)
                {
                    DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
                    btnCol.Name = col.ToString();
                    btnCol.HeaderText = col.ToString();
                    btnCol.Text = "Set";
                    btnCol.UseColumnTextForButtonValue = true;
                    dgWatchdogView.Columns.Add(btnCol);
                }
                else
                {
                    dgWatchdogView.Columns.Add(col.ToString(), col.ToString());
                }
            }
            foreach (DataGridViewColumn col in dgWatchdogView.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            Update_DGVWatchdog(dgWatchdogView);
        }
        private void Update_DGVWatchdog(DataGridView dgv)
        {
            var fields = typeof(Port_WatchdogParam).GetFields();
            for (int nCount = 0; nCount < dgv.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_WatchdogSettingsColumn.Name:
                        if (dgv.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_WatchdogList"))
                            dgv.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_WatchdogList");
                        break;
                    case (int)DGV_WatchdogSettingsColumn.AppliedValue:
                        if (dgv.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_AppliedDetectTime"))
                            dgv.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_AppliedDetectTime");
                        break;
                    case (int)DGV_WatchdogSettingsColumn.SetValue:
                        if (dgv.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SetDetectTime"))
                            dgv.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SetDetectTime");
                        break;
                }
            }
            if (dgv.Rows.Count != Enum.GetValues(typeof(WatchdogList)).Length)
            {
                dgv.Rows.Clear();
                for (int nRowCount = 0; nRowCount < Enum.GetValues(typeof(WatchdogList)).Length; nRowCount++)
                {
                    WatchdogList eWatchdog = (WatchdogList)nRowCount;
                    string fieldName = eWatchdog.ToString();
                    var fieldInfo = fields.FirstOrDefault(f => f.Name == fieldName);
                    int value = 0;
                    if (fieldInfo != null)
                    {
                        value = (int)fieldInfo.GetValue(WatchdogDetectParam);
                    }
                    dgv.Rows.Add(new string[4]{fieldName,
                                               value.ToString(), // AppliedValue
                                               value.ToString(), // SetValue
                                               "Set"});
                }
            }
            else
            {
                for (int nRowCount = 0; nRowCount < dgv.Rows.Count; nRowCount++)
                {
                    for (int nColumnCount = 0; nColumnCount < dgv.Columns.Count; nColumnCount++)
                    {
                        WatchdogList eWatchdog = (WatchdogList)nRowCount;
                        DGV_WatchdogSettingsColumn edgv_WatchdogSettingsColumn = (DGV_WatchdogSettingsColumn)nColumnCount;
                        DataGridViewCell dgv_Cell = dgv.Rows[nRowCount].Cells[nColumnCount];
                        string Data = string.Empty;

                        if (edgv_WatchdogSettingsColumn == DGV_WatchdogSettingsColumn.Name ||
                            edgv_WatchdogSettingsColumn == DGV_WatchdogSettingsColumn.SetValue ||
                            edgv_WatchdogSettingsColumn == DGV_WatchdogSettingsColumn.Btn)
                            continue;

                        switch (edgv_WatchdogSettingsColumn)
                        {
                            //case DGV_WatchdogSettingsColumn.AppliedValue:
                            //    Data = Watchdog_GetParam_DetectTime(eWatchdog).ToString();
                            //    break;

                        }

                        if ((string)dgv_Cell.Value != Data)
                            dgv_Cell.Value = Data;
                    }
                }
            }
        }
    }
}