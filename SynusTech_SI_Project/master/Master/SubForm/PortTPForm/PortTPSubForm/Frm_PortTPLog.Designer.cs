
namespace Master.SubForm.PortTPForm.PortTPSubForm
{
    partial class Frm_PortTPLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.UIUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_LogHistory = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.DGV_Log = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label_LoadingProcess = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_Filter_Days = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.groupBox_Filter_PortID = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel_PortListInfo = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_Function = new System.Windows.Forms.GroupBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_FindErrorLog = new System.Windows.Forms.Button();
            this.btn_PortAllCheck = new System.Windows.Forms.Button();
            this.btn_PortAllUncheck = new System.Windows.Forms.Button();
            this.groupBox_Filter_Level = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.chx_Error = new System.Windows.Forms.CheckBox();
            this.chx_Warning = new System.Windows.Forms.CheckBox();
            this.chx_Normal = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox_LogHistory.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Log)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox_Filter_Days.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox_Filter_PortID.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox_Function.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox_Filter_Level.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // UIUpdateTimer
            // 
            this.UIUpdateTimer.Interval = 250;
            this.UIUpdateTimer.Tick += new System.EventHandler(this.UIUpdateTimer_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox_LogHistory, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 196F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1734, 904);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox_LogHistory
            // 
            this.groupBox_LogHistory.Controls.Add(this.panel4);
            this.groupBox_LogHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_LogHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_LogHistory.Location = new System.Drawing.Point(3, 199);
            this.groupBox_LogHistory.Name = "groupBox_LogHistory";
            this.groupBox_LogHistory.Size = new System.Drawing.Size(1728, 702);
            this.groupBox_LogHistory.TabIndex = 38;
            this.groupBox_LogHistory.TabStop = false;
            this.groupBox_LogHistory.Text = "Log History";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.DGV_Log);
            this.panel4.Controls.Add(this.label_LoadingProcess);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel4.Location = new System.Drawing.Point(3, 25);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1722, 674);
            this.panel4.TabIndex = 37;
            // 
            // DGV_Log
            // 
            this.DGV_Log.AllowUserToAddRows = false;
            this.DGV_Log.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Log.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.DGV_Log.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Log.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_Log.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV_Log.ColumnHeadersHeight = 30;
            this.DGV_Log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DGV_Log.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Column2,
            this.Column3,
            this.Column4});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Log.DefaultCellStyle = dataGridViewCellStyle7;
            this.DGV_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_Log.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DGV_Log.EnableHeadersVisualStyles = false;
            this.DGV_Log.GridColor = System.Drawing.Color.DarkGray;
            this.DGV_Log.Location = new System.Drawing.Point(0, 25);
            this.DGV_Log.Margin = new System.Windows.Forms.Padding(0);
            this.DGV_Log.MultiSelect = false;
            this.DGV_Log.Name = "DGV_Log";
            this.DGV_Log.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_Log.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.DGV_Log.RowHeadersVisible = false;
            this.DGV_Log.RowTemplate.Height = 20;
            this.DGV_Log.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Log.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.DGV_Log.Size = new System.Drawing.Size(1722, 649);
            this.DGV_Log.TabIndex = 36;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn1.FillWeight = 300F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Time";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 180;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn2.HeaderText = "Type";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column2.HeaderText = "Level";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column3.HeaderText = "Title";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column4.FillWeight = 300F;
            this.Column4.HeaderText = "Comment";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label_LoadingProcess
            // 
            this.label_LoadingProcess.BackColor = System.Drawing.Color.Transparent;
            this.label_LoadingProcess.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_LoadingProcess.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label_LoadingProcess.Location = new System.Drawing.Point(0, 0);
            this.label_LoadingProcess.Name = "label_LoadingProcess";
            this.label_LoadingProcess.Size = new System.Drawing.Size(1722, 25);
            this.label_LoadingProcess.TabIndex = 37;
            this.label_LoadingProcess.Text = "Loading Process : ";
            this.label_LoadingProcess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox_Filter_Days, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox_Filter_PortID, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1734, 196);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox_Filter_Days
            // 
            this.groupBox_Filter_Days.Controls.Add(this.panel3);
            this.groupBox_Filter_Days.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Filter_Days.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Filter_Days.Location = new System.Drawing.Point(1507, 3);
            this.groupBox_Filter_Days.Name = "groupBox_Filter_Days";
            this.groupBox_Filter_Days.Size = new System.Drawing.Size(224, 190);
            this.groupBox_Filter_Days.TabIndex = 39;
            this.groupBox_Filter_Days.TabStop = false;
            this.groupBox_Filter_Days.Text = "Filter(Days)";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panel3.Controls.Add(this.monthCalendar1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel3.Location = new System.Drawing.Point(3, 25);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(1);
            this.panel3.Size = new System.Drawing.Size(218, 162);
            this.panel3.TabIndex = 37;
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.monthCalendar1.BackColor = System.Drawing.Color.DodgerBlue;
            this.monthCalendar1.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            this.monthCalendar1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.monthCalendar1.Location = new System.Drawing.Point(2, 1);
            this.monthCalendar1.Margin = new System.Windows.Forms.Padding(0);
            this.monthCalendar1.MaximumSize = new System.Drawing.Size(214, 160);
            this.monthCalendar1.MaxSelectionCount = 28;
            this.monthCalendar1.MinimumSize = new System.Drawing.Size(214, 160);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 1;
            this.monthCalendar1.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
            // 
            // groupBox_Filter_PortID
            // 
            this.groupBox_Filter_PortID.Controls.Add(this.panel1);
            this.groupBox_Filter_PortID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Filter_PortID.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Filter_PortID.Location = new System.Drawing.Point(3, 3);
            this.groupBox_Filter_PortID.Name = "groupBox_Filter_PortID";
            this.groupBox_Filter_PortID.Size = new System.Drawing.Size(1298, 190);
            this.groupBox_Filter_PortID.TabIndex = 37;
            this.groupBox_Filter_PortID.TabStop = false;
            this.groupBox_Filter_PortID.Text = "Filter(Port ID)";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tableLayoutPanel_PortListInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(3, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1292, 162);
            this.panel1.TabIndex = 37;
            // 
            // tableLayoutPanel_PortListInfo
            // 
            this.tableLayoutPanel_PortListInfo.ColumnCount = 5;
            this.tableLayoutPanel_PortListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_PortListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_PortListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_PortListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_PortListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_PortListInfo.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_PortListInfo.Name = "tableLayoutPanel_PortListInfo";
            this.tableLayoutPanel_PortListInfo.RowCount = 3;
            this.tableLayoutPanel_PortListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_PortListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_PortListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_PortListInfo.Size = new System.Drawing.Size(1234, 125);
            this.tableLayoutPanel_PortListInfo.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.groupBox_Function, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.groupBox_Filter_Level, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(1304, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(200, 196);
            this.tableLayoutPanel4.TabIndex = 40;
            // 
            // groupBox_Function
            // 
            this.groupBox_Function.Controls.Add(this.panel5);
            this.groupBox_Function.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Function.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Function.Location = new System.Drawing.Point(3, 92);
            this.groupBox_Function.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.groupBox_Function.Name = "groupBox_Function";
            this.groupBox_Function.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.groupBox_Function.Size = new System.Drawing.Size(194, 101);
            this.groupBox_Function.TabIndex = 39;
            this.groupBox_Function.TabStop = false;
            this.groupBox_Function.Text = "Function";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.tableLayoutPanel5);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel5.Location = new System.Drawing.Point(3, 22);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(188, 76);
            this.panel5.TabIndex = 37;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.btn_FindErrorLog, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.btn_PortAllCheck, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btn_PortAllUncheck, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(188, 76);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // btn_FindErrorLog
            // 
            this.btn_FindErrorLog.BackColor = System.Drawing.Color.White;
            this.btn_FindErrorLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_FindErrorLog.FlatAppearance.BorderSize = 0;
            this.btn_FindErrorLog.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_FindErrorLog.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_FindErrorLog.Location = new System.Drawing.Point(0, 50);
            this.btn_FindErrorLog.Margin = new System.Windows.Forms.Padding(0);
            this.btn_FindErrorLog.Name = "btn_FindErrorLog";
            this.btn_FindErrorLog.Size = new System.Drawing.Size(188, 26);
            this.btn_FindErrorLog.TabIndex = 8;
            this.btn_FindErrorLog.Text = "Find Error Log";
            this.btn_FindErrorLog.UseVisualStyleBackColor = false;
            this.btn_FindErrorLog.Click += new System.EventHandler(this.btn_FindErrorLog_Click);
            // 
            // btn_PortAllCheck
            // 
            this.btn_PortAllCheck.BackColor = System.Drawing.Color.White;
            this.btn_PortAllCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_PortAllCheck.FlatAppearance.BorderSize = 0;
            this.btn_PortAllCheck.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_PortAllCheck.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_PortAllCheck.Location = new System.Drawing.Point(0, 0);
            this.btn_PortAllCheck.Margin = new System.Windows.Forms.Padding(0);
            this.btn_PortAllCheck.Name = "btn_PortAllCheck";
            this.btn_PortAllCheck.Size = new System.Drawing.Size(188, 25);
            this.btn_PortAllCheck.TabIndex = 6;
            this.btn_PortAllCheck.Text = "Port All Check";
            this.btn_PortAllCheck.UseVisualStyleBackColor = false;
            this.btn_PortAllCheck.Click += new System.EventHandler(this.btn_PortAllCheck_Click);
            // 
            // btn_PortAllUncheck
            // 
            this.btn_PortAllUncheck.BackColor = System.Drawing.Color.White;
            this.btn_PortAllUncheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_PortAllUncheck.FlatAppearance.BorderSize = 0;
            this.btn_PortAllUncheck.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_PortAllUncheck.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_PortAllUncheck.Location = new System.Drawing.Point(0, 25);
            this.btn_PortAllUncheck.Margin = new System.Windows.Forms.Padding(0);
            this.btn_PortAllUncheck.Name = "btn_PortAllUncheck";
            this.btn_PortAllUncheck.Size = new System.Drawing.Size(188, 25);
            this.btn_PortAllUncheck.TabIndex = 7;
            this.btn_PortAllUncheck.Text = "Port All Uncheck";
            this.btn_PortAllUncheck.UseVisualStyleBackColor = false;
            this.btn_PortAllUncheck.Click += new System.EventHandler(this.btn_PortAllUncheck_Click);
            // 
            // groupBox_Filter_Level
            // 
            this.groupBox_Filter_Level.Controls.Add(this.panel2);
            this.groupBox_Filter_Level.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Filter_Level.Location = new System.Drawing.Point(3, 3);
            this.groupBox_Filter_Level.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBox_Filter_Level.Name = "groupBox_Filter_Level";
            this.groupBox_Filter_Level.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.groupBox_Filter_Level.Size = new System.Drawing.Size(194, 89);
            this.groupBox_Filter_Level.TabIndex = 38;
            this.groupBox_Filter_Level.TabStop = false;
            this.groupBox_Filter_Level.Text = "Filter(Level)";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(3, 22);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(188, 64);
            this.panel2.TabIndex = 37;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.chx_Error, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.chx_Warning, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.chx_Normal, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(188, 64);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // chx_Error
            // 
            this.chx_Error.AutoSize = true;
            this.chx_Error.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_Error.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chx_Error.Location = new System.Drawing.Point(0, 42);
            this.chx_Error.Margin = new System.Windows.Forms.Padding(0);
            this.chx_Error.Name = "chx_Error";
            this.chx_Error.Size = new System.Drawing.Size(188, 22);
            this.chx_Error.TabIndex = 2;
            this.chx_Error.Text = "Error";
            this.chx_Error.UseVisualStyleBackColor = true;
            this.chx_Error.CheckedChanged += new System.EventHandler(this.chx_Error_CheckedChanged);
            // 
            // chx_Warning
            // 
            this.chx_Warning.AutoSize = true;
            this.chx_Warning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_Warning.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chx_Warning.Location = new System.Drawing.Point(0, 21);
            this.chx_Warning.Margin = new System.Windows.Forms.Padding(0);
            this.chx_Warning.Name = "chx_Warning";
            this.chx_Warning.Size = new System.Drawing.Size(188, 21);
            this.chx_Warning.TabIndex = 1;
            this.chx_Warning.Text = "Warning";
            this.chx_Warning.UseVisualStyleBackColor = true;
            this.chx_Warning.CheckedChanged += new System.EventHandler(this.chx_Warning_CheckedChanged);
            // 
            // chx_Normal
            // 
            this.chx_Normal.AutoSize = true;
            this.chx_Normal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_Normal.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chx_Normal.Location = new System.Drawing.Point(0, 0);
            this.chx_Normal.Margin = new System.Windows.Forms.Padding(0);
            this.chx_Normal.Name = "chx_Normal";
            this.chx_Normal.Size = new System.Drawing.Size(188, 21);
            this.chx_Normal.TabIndex = 0;
            this.chx_Normal.Text = "Normal";
            this.chx_Normal.UseVisualStyleBackColor = true;
            this.chx_Normal.CheckedChanged += new System.EventHandler(this.chx_Normal_CheckedChanged);
            // 
            // Frm_PortTPLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1734, 904);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_PortTPLog";
            this.Text = "Frm_PortTPLog";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox_LogHistory.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Log)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox_Filter_Days.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox_Filter_PortID.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.groupBox_Function.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.groupBox_Filter_Level.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer UIUpdateTimer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox_LogHistory;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox_Filter_Days;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.GroupBox groupBox_Filter_Level;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox_Filter_PortID;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chx_Error;
        private System.Windows.Forms.CheckBox chx_Warning;
        private System.Windows.Forms.CheckBox chx_Normal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_PortListInfo;
        private System.Windows.Forms.DataGridView DGV_Log;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox groupBox_Function;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btn_PortAllCheck;
        private System.Windows.Forms.Button btn_PortAllUncheck;
        private System.Windows.Forms.Label label_LoadingProcess;
        private System.Windows.Forms.Button btn_FindErrorLog;
    }
}