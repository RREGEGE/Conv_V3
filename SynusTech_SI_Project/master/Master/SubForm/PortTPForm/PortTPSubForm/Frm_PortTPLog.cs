using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.ManagedFile;
using System.IO;
using System.Threading;

namespace Master.SubForm.PortTPForm.PortTPSubForm
{
    public partial class Frm_PortTPLog : Form
    {
        public class PortLogPrintCase
        {
            bool bNormal    = false;
            bool bWarning   = false;
            bool bError     = false;
            List<string> PortList   = new List<string>();
            DateTime dtStart;
            DateTime dtEnd;

            public PortLogPrintCase(bool _bNormal, bool _bWarning, bool _bError, List<string> _PortList, DateTime _dtStart, DateTime _dtEnd)
            {
                bNormal = _bNormal;
                bWarning = _bWarning;
                bError = _bError;
                PortList = _PortList;
                dtStart = _dtStart;
                dtEnd = _dtEnd;
            }

            static public bool Compare(PortLogPrintCase A, PortLogPrintCase B)
            {
                if (A == null || B == null)
                    return false;

                if (A.bNormal != B.bNormal)
                    return false;

                if (A.bWarning != B.bWarning)
                    return false;

                if (A.bError != B.bError)
                    return false;

                if (A.PortList != B.PortList)
                    return false;

                if (A.dtStart != B.dtStart)
                    return false;

                if (A.dtEnd != B.dtEnd)
                    return false;

                return true;
            }
        }

        public class LogLine
        {
            string Time;
            string Port;
            string Level;
            string Title;
            string Comment;

            public LogLine(string _Time, string _Port, string _Level, string _Title, string _Comment)
            {
                Time = _Time;
                Port = _Port;
                Level = _Level;
                Title = _Title;
                Comment = _Comment;
            }

            public string[] GetArrays()
            {
                return new string[] { Time, Port, Level, Title, Comment };
            }
        }

        List<LogLine> LogLines = new List<LogLine>();
        bool bLogLoading = false;
        Thread LogUpdateThread;

        public Frm_PortTPLog()
        {
            InitializeComponent();
            ControlItemInit();
            LogDGVUpdate();

            this.VisibleChanged += (object sender, EventArgs e) =>
            {
                if (this.Visible)
                {
                    LogDGVUpdate();
                    UIUpdateTimer.Enabled = true;
                }
                else
                {
                    UIUpdateTimer.Enabled = false;

                    if(LogUpdateThread != null &&
                        LogUpdateThread.IsAlive)
                    {
                        LogUpdateThread.Abort();
                        bLogLoading = false;
                        DGV_Log.Visible = true;
                        this.Enabled = true;
                    }
                }
            };

            this.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;
                bLogLoading = false;
            };

            this.Disposed += (object sender, EventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;
                bLogLoading = false;

                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }
        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UIUpdateTimer.Interval = Master.UIUpdateIntervalTime;
                GroupBoxFunc.SetText(groupBox_Filter_PortID, SynusLangPack.GetLanguage("GroupBox_Filter_PortID"));
                GroupBoxFunc.SetText(groupBox_Filter_Level, SynusLangPack.GetLanguage("GroupBox_Filter_Level"));
                GroupBoxFunc.SetText(groupBox_Filter_Days, SynusLangPack.GetLanguage("GroupBox_Filter_Days"));
                GroupBoxFunc.SetText(groupBox_LogHistory, SynusLangPack.GetLanguage("GroupBox_LogHistory"));
                GroupBoxFunc.SetText(groupBox_Function, SynusLangPack.GetLanguage("GroupBox_Function"));

                ButtonFunc.SetText(btn_PortAllCheck, SynusLangPack.GetLanguage("Btn_PortAllCheck"));
                ButtonFunc.SetText(btn_PortAllUncheck, SynusLangPack.GetLanguage("Btn_PortAllUncheck"));

                if (bLogLoading)
                    return;

                for (int nCount = 0; nCount < DGV_Log.Columns.Count; nCount++)
                {
                    switch (nCount)
                    {
                        case 0:
                            if (DGV_Log.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_GenerateTime"))
                                DGV_Log.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_GenerateTime");
                            break;
                        case 1:
                            if (DGV_Log.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LogType"))
                                DGV_Log.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LogType");
                            break;
                        case 2:
                            if (DGV_Log.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LogLevel"))
                                DGV_Log.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LogLevel");
                            break;
                        case 3:
                            if (DGV_Log.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LogTitle"))
                                DGV_Log.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LogTitle");
                            break;
                        case 4:
                            if (DGV_Log.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LogComment"))
                                DGV_Log.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LogComment");
                            break;
                    }
                }
            }
            catch { }
            finally
            {
            }
        }

        private void LogDesignStartInitialize()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    LogDesignStartInitializeFunc();
                }));
            }
            else
            {
                LogDesignStartInitializeFunc();
            }
        }

        private void LogDesignStartInitializeFunc()
        {
            DGV_Log.Visible = false;
            this.Enabled = false;
            label_LoadingProcess.Tag = 0;
        }

        private void LogDesignRowAddProcess()
        {
            for (int nCount = 0; nCount < LogLines.Count; nCount++)
            {
                if (this.Disposing || this.IsDisposed)
                    return;

                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        LogDesignRowAddProcessFunc(nCount);
                    }));
                }
                else
                {
                    LogDesignRowAddProcessFunc(nCount);
                }
            }
        }

        private void LogDesignRowAddProcessFunc(int nCount)
        {
            int ProgPercent = (nCount + 1) * 100 / LogLines.Count;

            DGV_Log.Rows.Add(LogLines[nCount].GetArrays());

            if ((int)label_LoadingProcess.Tag != ProgPercent)
            {
                label_LoadingProcess.Tag = ProgPercent;
                label_LoadingProcess.Text = $"Loading Process => GridView Log Add, Progress : {ProgPercent:0.00}%";
            }
        }
        private void LogDesignColorProcess()
        {
            for (int nCount = 0; nCount < DGV_Log.Rows.Count; nCount++)
            {
                if (this.Disposing || this.IsDisposed)
                    return;

                int ProgPercent = (nCount + 1) * 100 / DGV_Log.Rows.Count;

                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        LogDesignColorProcessFunc(nCount);
                    }));
                }
                else
                {
                    LogDesignColorProcessFunc(nCount);
                }
            }
        }

        private void LogDesignColorProcessFunc(int nCount)
        {
            int ProgPercent = (nCount + 1) * 100 / DGV_Log.Rows.Count;

            string value = (string)DGV_Log.Rows[nCount].Cells[2].Value;

            if (string.IsNullOrEmpty(value))
                return;

            if (value.ToLower().Contains("error"))
            {
                DGV_Log.Rows[nCount].Cells[2].Style.ForeColor = Color.Red;
            }
            else if (value.ToLower().Contains("warning"))
            {
                DGV_Log.Rows[nCount].Cells[2].Style.ForeColor = Color.Orange;
            }
            else if (value.ToLower().Contains("normal"))
            {
                DGV_Log.Rows[nCount].Cells[2].Style.ForeColor = Color.Black;
            }

            if (nCount % 2 == 1)
                DGV_Log.Rows[nCount].DefaultCellStyle.BackColor = Color.LightCyan;

            if ((int)label_LoadingProcess.Tag != ProgPercent)
            {
                label_LoadingProcess.Tag = ProgPercent;
                label_LoadingProcess.Text = $"Loading Process => GridView Design Update, Progress : {ProgPercent:0.00}%";
            }
        }

        private void LogDesignResultProcess()
        {
            int nError = 0;
            int nWarning = 0;
            int nNormal = 0;

            for (int nCount = 0; nCount < DGV_Log.Rows.Count; nCount++)
            {
                if (this.Disposing || this.IsDisposed)
                    return;

                string value = (string)DGV_Log.Rows[nCount].Cells[2].Value;

                if (string.IsNullOrEmpty(value))
                    return;

                if (value.ToLower().Contains("error"))
                {
                    nError++;
                }
                else if (value.ToLower().Contains("warning"))
                {
                    nWarning++;
                }
                else if (value.ToLower().Contains("normal"))
                {
                    nNormal++;
                }
            }

            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    label_LoadingProcess.Text = $"Loaded Result => Log Lines : {DGV_Log.Rows.Count}, Error : {nError}, Warning : {nWarning}, Normal : {nNormal}";
                    //this.Enabled = true;
                    DGV_Log.Visible = true;
                    this.Enabled = true;
                }));
            }
            else
            {
                label_LoadingProcess.Text = $"Loaded Result => Log Lines : {DGV_Log.Rows.Count}, Error : {nError}, Warning : {nWarning}, Normal : {nNormal}";
                //this.Enabled = true;
                DGV_Log.Visible = true;
                this.Enabled = true;
            }
        }

        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);
            Update_PortList();

            string LogDirectory = ManagedFileInfo.LogDirectory;
            DirectoryInfo di = new DirectoryInfo(LogDirectory);
            DirectoryInfo[] YearDirectories = di.GetDirectories("*.*", SearchOption.TopDirectoryOnly);

            foreach (DirectoryInfo YearDirectory in YearDirectories)
            {
                if (YearDirectory.Name.Length != 4)
                    continue;

                string Year;
                string Month;
                string Day;
                try
                {
                    int nYear = Convert.ToInt32(YearDirectory.Name);
                    if (nYear >= 2000 && nYear <= 3000)
                        Year = nYear.ToString("0000");
                    else
                        continue;
                }
                catch
                {
                    continue;
                }

                DirectoryInfo[] MonthDirectories = YearDirectory.GetDirectories("*.*", SearchOption.TopDirectoryOnly);
                foreach (DirectoryInfo MonthDirectory in MonthDirectories)
                {
                    if (MonthDirectory.Name.Length != 2)
                        continue;

                    try
                    {
                        int nMonth = Convert.ToInt32(MonthDirectory.Name);
                        if (nMonth >= 1 && nMonth <= 12)
                            Month = nMonth.ToString("00");
                        else
                            continue;
                    }
                    catch
                    {
                        continue;
                    }

                    DirectoryInfo[] DayDirectories = MonthDirectory.GetDirectories("*.*", SearchOption.TopDirectoryOnly);
                    foreach (DirectoryInfo DayDirectory in DayDirectories)
                    {
                        if (DayDirectory.Name.Length != 2)
                            continue;

                        try
                        {
                            int nDay = Convert.ToInt32(DayDirectory.Name);
                            if (nDay >= 1 && nDay <= 31)
                                Day = nDay.ToString("00");
                            else
                                continue;
                        }
                        catch
                        {
                            continue;
                        }

                        string SelectedDirectoryPath = LogDirectory + @"\" + Year + @"\" + Month + @"\" + Day;

                        if (!Directory.Exists(SelectedDirectoryPath))
                            continue;

                        DirectoryInfo SelectedDirectory = new DirectoryInfo(SelectedDirectoryPath);
                        string[] fileList = Directory.GetFiles(SelectedDirectory.FullName, "*.*", SearchOption.TopDirectoryOnly);

                        if (fileList != null && fileList.Length > 0)
                        {
                            foreach(var file in fileList)
                            {
                                if(file.ToLower().Contains("masterlog") && Path.GetExtension(file).ToLower() == ".txt")
                                {
                                    DateTime BoldDate = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), Convert.ToInt32(Day));
                                    monthCalendar1.AddBoldedDate(BoldDate);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void SetAutoScale(float FactorX, float FactorY)
        {
            this.AutoScaleMode = AutoScaleMode.None;
            //this.Scale(new SizeF(FactorX, FactorY));

            foreach (Control item in this.Controls)
                ControlFunc.ResizeFont(item,FactorY);
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            btn.BackColor = btn.BackColor == Color.Lime ? Color.White : Color.Lime;

            LogDGVUpdate();
        }
        private void LogDGVUpdate()
        {
            if (LogUpdateThread != null && 
                LogUpdateThread.IsAlive)
                return;

            if (!chx_Normal.Checked &&
                !chx_Warning.Checked &&
                !chx_Error.Checked)
                return;

            List<string> EnablePort = new List<string>();
            foreach(var control in tableLayoutPanel_PortListInfo.Controls)
            {
                if(control.GetType() == typeof(Button))
                {
                    Button btn = (Button)control;
                    if(btn.BackColor == Color.Lime && btn.Tag != null)
                        EnablePort.Add((string)btn.Tag);
                }
            }

            string LogDirectory = ManagedFileInfo.LogDirectory;
            DateTime dtStart = monthCalendar1.SelectionStart;
            DateTime dtEnd = monthCalendar1.SelectionEnd;

            int nStart = Convert.ToInt32(dtStart.ToString("yyyyMMdd"));
            int nEnd = Convert.ToInt32(dtEnd.ToString("yyyyMMdd"));

            PortLogPrintCase portLogPrintCase = new PortLogPrintCase(chx_Normal.Checked, chx_Warning.Checked, chx_Error.Checked, EnablePort, dtStart, dtEnd);

            if (PortLogPrintCase.Compare((PortLogPrintCase)DGV_Log.Tag, portLogPrintCase))
                return;
            else
            {
                DGV_Log.Rows.Clear();
                LogLines.Clear();
                DGV_Log.Tag = portLogPrintCase;
            }

            Thread LocalThread = new Thread(delegate ()
            {
                try
                {
                    bLogLoading = true;
                    bool bNormal = chx_Normal.Checked;
                    bool bWarning = chx_Warning.Checked;
                    bool bError = chx_Error.Checked;

                    for (int nCount = 0; nCount <= nEnd - nStart; nCount++)
                    {
                        if (!bLogLoading)
                            break;

                        string Year = dtStart.AddDays(nCount).ToString("yyyy");
                        string Month = dtStart.AddDays(nCount).ToString("MM");
                        string Day = dtStart.AddDays(nCount).ToString("dd");

                        string SelectedDirectory = LogDirectory + @"\" + Year + @"\" + Month + @"\" + Day;

                        if (!Directory.Exists(SelectedDirectory))
                            continue;

                        DirectoryInfo di = new DirectoryInfo(SelectedDirectory);
                        string[] file = Directory.GetFiles(di.FullName, "*.*", SearchOption.TopDirectoryOnly);

                        if (file != null && file.Length > 0)
                        {
                            foreach (string filepath in file)
                            {
                                if (!bLogLoading)
                                    break;

                                if (filepath.ToLower().Contains("masterlog") && Path.GetExtension(filepath).ToLower() == ".txt")
                                {
                                    string[] ReadLines = null;

                                    using (var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                    {
                                        using (StreamReader sr = new StreamReader(fs))
                                        {
                                            ReadLines = sr.ReadToEnd().Split('\n');
                                        }
                                    }

                                    if (ReadLines != null && ReadLines.Length > 0)
                                    {
                                        foreach (string ReadLine in ReadLines)
                                        {
                                            if (!bLogLoading)
                                                break;

                                            string[] ColumnDatas = ReadLine.Split(new string[] { "], [" }, StringSplitOptions.None);

                                            if (ColumnDatas.Length >= 4 && ColumnDatas.Length <= 6)
                                            {
                                                string Time = ColumnDatas[0].Substring(1);
                                                string Port = ColumnDatas[1];
                                                string Level = ColumnDatas[2];
                                                string Title = ColumnDatas.Length == 4 && ColumnDatas[3].Length > 0 ? ColumnDatas[3].Substring(0, ColumnDatas[3].Length - 1) : ColumnDatas[3];
                                                string Comment = ColumnDatas.Length == 5 && ColumnDatas[4].Length > 0 ? ColumnDatas[4].Substring(0, ColumnDatas[4].Length - 1) : string.Empty;
                                                string PortID = Port.ToLower().Contains("port") ? Port.Substring(4).Replace(" ", string.Empty) : string.Empty;

                                                if (!bNormal && Level.ToLower().Contains("normal"))
                                                    continue;

                                                if (!bWarning && Level.ToLower().Contains("warning"))
                                                    continue;

                                                if (!bError && Level.ToLower().Contains("error"))
                                                    continue;

                                                if (PortID == string.Empty || !EnablePort.Contains(PortID))
                                                    continue;

                                                if (Comment.Substring(Comment.Length - 1, 1) == "]")
                                                    Comment = Comment.Substring(0, Comment.Length - 1);

                                                LogLines.Add(new LogLine(Time, Port, Level, Title, Comment));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    LogDesignStartInitialize();
                    LogDesignRowAddProcess();
                    LogDesignColorProcess();
                    LogDesignResultProcess();
                }
                catch(Exception ex)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.FileLoadFail, $"Log Grid Load Exception Fail : {ex.Message}");
                }
                finally
                {
                    bLogLoading = false;
                }
            });
            LocalThread.Name = $"Log Loading Thread";
            LocalThread.IsBackground = true;
            LogUpdateThread = LocalThread;
            LocalThread.Start();
        }
        private void Update_PortList()
        {
            List<Button> ControlButtonList = new List<Button>();

            foreach (var port in Master.m_Ports)
            {
                Button btn = new Button();

                ButtonFunc.SetText(btn, port.Value.GetFocusButtonStr());

                //btn.Text = $"Port [ {port.Value.GetParam().ID} ]";
                btn.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                btn.BackColor = Color.Lime;
                btn.Margin = new Padding(1, 1, 1, 1);
                btn.Tag = port.Value.GetParam().ID;
                btn.Click += Btn_Click;
                ControlButtonList.Add(btn);
            }


            //Column Setting
            tableLayoutPanel_PortListInfo.ColumnCount = 5;
            for (int nColumnCount = 0; nColumnCount < tableLayoutPanel_PortListInfo.ColumnCount; nColumnCount++)
            {
                tableLayoutPanel_PortListInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            }
            //Row Setting
            tableLayoutPanel_PortListInfo.RowCount = (ControlButtonList.Count % 5 == 0 ? ControlButtonList.Count / 5 : ControlButtonList.Count / 5 + 1);

            if (tableLayoutPanel_PortListInfo.RowCount <= 3)
            {
                for (int nRowCount = 0; nRowCount < tableLayoutPanel_PortListInfo.RowCount; nRowCount++)
                {
                    tableLayoutPanel_PortListInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                }
                tableLayoutPanel_PortListInfo.Dock = DockStyle.Fill;
            }
            else
            {
                for (int nRowCount = 0; nRowCount < tableLayoutPanel_PortListInfo.RowCount; nRowCount++)
                {
                    tableLayoutPanel_PortListInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
                }

                tableLayoutPanel_PortListInfo.Width = panel1.Width - 30;
                tableLayoutPanel_PortListInfo.Height = tableLayoutPanel_PortListInfo.RowCount * 45;
            }

            //tableLayoutPanel_PortListInfo.Dock = DockStyle.Fill;

            int nButtonCount = 0;
            for (int nRowCount = 0; nRowCount < tableLayoutPanel_PortListInfo.RowCount; nRowCount++)
            {
                for (int nColumnCount = 0; nColumnCount < tableLayoutPanel_PortListInfo.ColumnCount; nColumnCount++)
                {
                    if (nButtonCount >= ControlButtonList.Count)
                        continue;

                    tableLayoutPanel_PortListInfo.Controls.Add(ControlButtonList[nButtonCount], nColumnCount, nRowCount);
                    ControlButtonList[nButtonCount].Dock = DockStyle.Fill;
                    nButtonCount++;
                }
            }
        }

        private void chx_Normal_CheckedChanged(object sender, EventArgs e)
        {
            LogDGVUpdate();
        }

        private void chx_Warning_CheckedChanged(object sender, EventArgs e)
        {
            LogDGVUpdate();
        }

        private void chx_Error_CheckedChanged(object sender, EventArgs e)
        {
            LogDGVUpdate();
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            LogDGVUpdate();
        }

        private void btn_PortAllCheck_Click(object sender, EventArgs e)
        {
            foreach (var control in tableLayoutPanel_PortListInfo.Controls)
            {
                if (control.GetType() == typeof(Button))
                {
                    Button btn = (Button)control;
                    btn.BackColor = Color.Lime;
                }
            }
            LogDGVUpdate();
        }

        private void btn_PortAllUncheck_Click(object sender, EventArgs e)
        {
            foreach (var control in tableLayoutPanel_PortListInfo.Controls)
            {
                if (control.GetType() == typeof(Button))
                {
                    Button btn = (Button)control;
                    btn.BackColor = Color.White;
                }
            }
            LogDGVUpdate();
        }

        private void btn_FindErrorLog_Click(object sender, EventArgs e)
        {
            if (LogUpdateThread != null &&
                LogUpdateThread.IsAlive)
                return;

            if (bLogLoading)
                return;

            bool bFind = false;
            int nStartCount = DGV_Log.CurrentCell?.RowIndex ?? 0;

            for (int nCount = nStartCount; nCount < DGV_Log.Rows.Count; nCount++)
            {
                string value = (string)DGV_Log.Rows[nCount].Cells[2].Value;

                if (string.IsNullOrEmpty(value))
                    return;

                if (value.ToLower().Contains("error") && nStartCount != nCount)
                {
                    DGV_Log.CurrentCell = DGV_Log.Rows[nCount].Cells[2];
                    bFind = true;
                    break;
                }
            }

            if(!bFind)
            {
                for (int nCount = 0; nCount < DGV_Log.Rows.Count; nCount++)
                {
                    string value = (string)DGV_Log.Rows[nCount].Cells[2].Value;

                    if (string.IsNullOrEmpty(value))
                        return;

                    if (value.ToLower().Contains("error") && nStartCount != nCount)
                    {
                        DGV_Log.CurrentCell = DGV_Log.Rows[nCount].Cells[2];
                        bFind = true;
                        break;
                    }
                }
            }

            if(!bFind)
                MessageBox.Show("Error Log is Not Exist", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
