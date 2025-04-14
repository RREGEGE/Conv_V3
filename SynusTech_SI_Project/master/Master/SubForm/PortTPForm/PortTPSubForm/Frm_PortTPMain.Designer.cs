
namespace Master.SubForm.PortTPForm.PortTPSubForm
{
    partial class Frm_PortTPMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.UIUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_PowerOff = new System.Windows.Forms.Button();
            this.btn_PowerOn = new System.Windows.Forms.Button();
            this.btn_DirectionChange = new System.Windows.Forms.Button();
            this.btn_MasterMode = new System.Windows.Forms.Button();
            this.btn_CIMMode = new System.Windows.Forms.Button();
            this.btn_AutoRun = new System.Windows.Forms.Button();
            this.btn_AutoStop = new System.Windows.Forms.Button();
            this.btn_ModeChange = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel_Right = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_AutoRunStatus = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.DGV_AutoRunStatus = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox_CycleSettings = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel21 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_CycleRun = new System.Windows.Forms.Button();
            this.btn_CycleStop = new System.Windows.Forms.Button();
            this.tbx_CycleCount = new System.Windows.Forms.TextBox();
            this.pnl_PortTotalStatus = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel_Right.SuspendLayout();
            this.groupBox_AutoRunStatus.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_AutoRunStatus)).BeginInit();
            this.groupBox_CycleSettings.SuspendLayout();
            this.tableLayoutPanel21.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
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
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel9, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1724, 683);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 13;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel9.Controls.Add(this.btn_PowerOff, 10, 0);
            this.tableLayoutPanel9.Controls.Add(this.btn_PowerOn, 9, 0);
            this.tableLayoutPanel9.Controls.Add(this.btn_DirectionChange, 6, 0);
            this.tableLayoutPanel9.Controls.Add(this.btn_MasterMode, 4, 0);
            this.tableLayoutPanel9.Controls.Add(this.btn_CIMMode, 3, 0);
            this.tableLayoutPanel9.Controls.Add(this.btn_AutoRun, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.btn_AutoStop, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.btn_ModeChange, 7, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 603);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(1724, 80);
            this.tableLayoutPanel9.TabIndex = 37;
            // 
            // btn_PowerOff
            // 
            this.btn_PowerOff.BackColor = System.Drawing.Color.AliceBlue;
            this.btn_PowerOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_PowerOff.FlatAppearance.BorderSize = 0;
            this.btn_PowerOff.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btn_PowerOff.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_PowerOff.Location = new System.Drawing.Point(1335, 1);
            this.btn_PowerOff.Margin = new System.Windows.Forms.Padding(1);
            this.btn_PowerOff.Name = "btn_PowerOff";
            this.btn_PowerOff.Size = new System.Drawing.Size(180, 78);
            this.btn_PowerOff.TabIndex = 15;
            this.btn_PowerOff.Text = "Power Off";
            this.btn_PowerOff.UseVisualStyleBackColor = false;
            this.btn_PowerOff.Click += new System.EventHandler(this.btn_PowerOff_Click);
            // 
            // btn_PowerOn
            // 
            this.btn_PowerOn.BackColor = System.Drawing.Color.AliceBlue;
            this.btn_PowerOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_PowerOn.FlatAppearance.BorderSize = 0;
            this.btn_PowerOn.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btn_PowerOn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_PowerOn.Location = new System.Drawing.Point(1153, 1);
            this.btn_PowerOn.Margin = new System.Windows.Forms.Padding(1);
            this.btn_PowerOn.Name = "btn_PowerOn";
            this.btn_PowerOn.Size = new System.Drawing.Size(180, 78);
            this.btn_PowerOn.TabIndex = 14;
            this.btn_PowerOn.Text = "Power On";
            this.btn_PowerOn.UseVisualStyleBackColor = false;
            this.btn_PowerOn.Click += new System.EventHandler(this.btn_PowerOn_Click);
            // 
            // btn_DirectionChange
            // 
            this.btn_DirectionChange.BackColor = System.Drawing.Color.AliceBlue;
            this.btn_DirectionChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_DirectionChange.FlatAppearance.BorderSize = 0;
            this.btn_DirectionChange.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btn_DirectionChange.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_DirectionChange.Location = new System.Drawing.Point(769, 1);
            this.btn_DirectionChange.Margin = new System.Windows.Forms.Padding(1);
            this.btn_DirectionChange.Name = "btn_DirectionChange";
            this.btn_DirectionChange.Size = new System.Drawing.Size(180, 78);
            this.btn_DirectionChange.TabIndex = 13;
            this.btn_DirectionChange.Text = "Direction Change";
            this.btn_DirectionChange.UseVisualStyleBackColor = false;
            this.btn_DirectionChange.Click += new System.EventHandler(this.btn_DirectionChange_Click);
            // 
            // btn_MasterMode
            // 
            this.btn_MasterMode.BackColor = System.Drawing.Color.AliceBlue;
            this.btn_MasterMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_MasterMode.FlatAppearance.BorderSize = 0;
            this.btn_MasterMode.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btn_MasterMode.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_MasterMode.Location = new System.Drawing.Point(567, 1);
            this.btn_MasterMode.Margin = new System.Windows.Forms.Padding(1);
            this.btn_MasterMode.Name = "btn_MasterMode";
            this.btn_MasterMode.Size = new System.Drawing.Size(180, 78);
            this.btn_MasterMode.TabIndex = 12;
            this.btn_MasterMode.Text = "Master Mode On";
            this.btn_MasterMode.UseVisualStyleBackColor = false;
            this.btn_MasterMode.Click += new System.EventHandler(this.btn_MasterMode_Click);
            // 
            // btn_CIMMode
            // 
            this.btn_CIMMode.BackColor = System.Drawing.Color.AliceBlue;
            this.btn_CIMMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_CIMMode.FlatAppearance.BorderSize = 0;
            this.btn_CIMMode.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btn_CIMMode.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_CIMMode.Location = new System.Drawing.Point(385, 1);
            this.btn_CIMMode.Margin = new System.Windows.Forms.Padding(1);
            this.btn_CIMMode.Name = "btn_CIMMode";
            this.btn_CIMMode.Size = new System.Drawing.Size(180, 78);
            this.btn_CIMMode.TabIndex = 11;
            this.btn_CIMMode.Text = "CIM Mode On";
            this.btn_CIMMode.UseVisualStyleBackColor = false;
            this.btn_CIMMode.Click += new System.EventHandler(this.btn_CIMMode_Click);
            // 
            // btn_AutoRun
            // 
            this.btn_AutoRun.BackColor = System.Drawing.Color.AliceBlue;
            this.btn_AutoRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AutoRun.FlatAppearance.BorderSize = 0;
            this.btn_AutoRun.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btn_AutoRun.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_AutoRun.Location = new System.Drawing.Point(1, 1);
            this.btn_AutoRun.Margin = new System.Windows.Forms.Padding(1);
            this.btn_AutoRun.Name = "btn_AutoRun";
            this.btn_AutoRun.Size = new System.Drawing.Size(180, 78);
            this.btn_AutoRun.TabIndex = 9;
            this.btn_AutoRun.Text = "Auto Run";
            this.btn_AutoRun.UseVisualStyleBackColor = false;
            this.btn_AutoRun.Click += new System.EventHandler(this.btn_AutoRun_Click);
            // 
            // btn_AutoStop
            // 
            this.btn_AutoStop.BackColor = System.Drawing.Color.AliceBlue;
            this.btn_AutoStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AutoStop.FlatAppearance.BorderSize = 0;
            this.btn_AutoStop.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btn_AutoStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_AutoStop.Location = new System.Drawing.Point(183, 1);
            this.btn_AutoStop.Margin = new System.Windows.Forms.Padding(1);
            this.btn_AutoStop.Name = "btn_AutoStop";
            this.btn_AutoStop.Size = new System.Drawing.Size(180, 78);
            this.btn_AutoStop.TabIndex = 5;
            this.btn_AutoStop.Text = "Auto Stop";
            this.btn_AutoStop.UseVisualStyleBackColor = false;
            this.btn_AutoStop.Click += new System.EventHandler(this.btn_AutoStop_Click);
            // 
            // btn_ModeChange
            // 
            this.btn_ModeChange.BackColor = System.Drawing.Color.AliceBlue;
            this.btn_ModeChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_ModeChange.FlatAppearance.BorderSize = 0;
            this.btn_ModeChange.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btn_ModeChange.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_ModeChange.Location = new System.Drawing.Point(951, 1);
            this.btn_ModeChange.Margin = new System.Windows.Forms.Padding(1);
            this.btn_ModeChange.Name = "btn_ModeChange";
            this.btn_ModeChange.Size = new System.Drawing.Size(180, 78);
            this.btn_ModeChange.TabIndex = 10;
            this.btn_ModeChange.Text = "Mode Change";
            this.btn_ModeChange.UseVisualStyleBackColor = false;
            this.btn_ModeChange.Click += new System.EventHandler(this.btn_ModeChange_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel_Right, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.pnl_PortTotalStatus, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1724, 603);
            this.tableLayoutPanel2.TabIndex = 38;
            // 
            // tableLayoutPanel_Right
            // 
            this.tableLayoutPanel_Right.ColumnCount = 1;
            this.tableLayoutPanel_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_Right.Controls.Add(this.groupBox_AutoRunStatus, 0, 0);
            this.tableLayoutPanel_Right.Controls.Add(this.groupBox_CycleSettings, 0, 1);
            this.tableLayoutPanel_Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_Right.Location = new System.Drawing.Point(1123, 3);
            this.tableLayoutPanel_Right.Name = "tableLayoutPanel_Right";
            this.tableLayoutPanel_Right.RowCount = 2;
            this.tableLayoutPanel_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel_Right.Size = new System.Drawing.Size(598, 597);
            this.tableLayoutPanel_Right.TabIndex = 40;
            // 
            // groupBox_AutoRunStatus
            // 
            this.groupBox_AutoRunStatus.Controls.Add(this.panel1);
            this.groupBox_AutoRunStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_AutoRunStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.groupBox_AutoRunStatus.Location = new System.Drawing.Point(3, 3);
            this.groupBox_AutoRunStatus.Name = "groupBox_AutoRunStatus";
            this.groupBox_AutoRunStatus.Size = new System.Drawing.Size(592, 491);
            this.groupBox_AutoRunStatus.TabIndex = 36;
            this.groupBox_AutoRunStatus.TabStop = false;
            this.groupBox_AutoRunStatus.Text = "Auto Run Status";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.DGV_AutoRunStatus);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(3, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(586, 463);
            this.panel1.TabIndex = 37;
            // 
            // DGV_AutoRunStatus
            // 
            this.DGV_AutoRunStatus.AllowUserToAddRows = false;
            this.DGV_AutoRunStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_AutoRunStatus.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.DGV_AutoRunStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_AutoRunStatus.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_AutoRunStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV_AutoRunStatus.ColumnHeadersHeight = 25;
            this.DGV_AutoRunStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DGV_AutoRunStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_AutoRunStatus.DefaultCellStyle = dataGridViewCellStyle4;
            this.DGV_AutoRunStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_AutoRunStatus.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DGV_AutoRunStatus.EnableHeadersVisualStyles = false;
            this.DGV_AutoRunStatus.GridColor = System.Drawing.Color.DarkGray;
            this.DGV_AutoRunStatus.Location = new System.Drawing.Point(0, 0);
            this.DGV_AutoRunStatus.Margin = new System.Windows.Forms.Padding(0);
            this.DGV_AutoRunStatus.MultiSelect = false;
            this.DGV_AutoRunStatus.Name = "DGV_AutoRunStatus";
            this.DGV_AutoRunStatus.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_AutoRunStatus.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.DGV_AutoRunStatus.RowHeadersVisible = false;
            this.DGV_AutoRunStatus.RowTemplate.Height = 18;
            this.DGV_AutoRunStatus.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_AutoRunStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.DGV_AutoRunStatus.Size = new System.Drawing.Size(586, 463);
            this.DGV_AutoRunStatus.TabIndex = 35;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn1.FillWeight = 60F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn2.HeaderText = "State";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupBox_CycleSettings
            // 
            this.groupBox_CycleSettings.Controls.Add(this.tableLayoutPanel21);
            this.groupBox_CycleSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_CycleSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_CycleSettings.Location = new System.Drawing.Point(3, 500);
            this.groupBox_CycleSettings.Name = "groupBox_CycleSettings";
            this.groupBox_CycleSettings.Size = new System.Drawing.Size(592, 94);
            this.groupBox_CycleSettings.TabIndex = 35;
            this.groupBox_CycleSettings.TabStop = false;
            this.groupBox_CycleSettings.Text = "Cycle Test";
            // 
            // tableLayoutPanel21
            // 
            this.tableLayoutPanel21.ColumnCount = 1;
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel21.Controls.Add(this.tableLayoutPanel15, 0, 0);
            this.tableLayoutPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel21.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel21.Name = "tableLayoutPanel21";
            this.tableLayoutPanel21.RowCount = 1;
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel21.Size = new System.Drawing.Size(586, 66);
            this.tableLayoutPanel21.TabIndex = 0;
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 4;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.btn_CycleRun, 2, 0);
            this.tableLayoutPanel15.Controls.Add(this.btn_CycleStop, 3, 0);
            this.tableLayoutPanel15.Controls.Add(this.tbx_CycleCount, 1, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(586, 66);
            this.tableLayoutPanel15.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 66);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cycle Count :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_CycleRun
            // 
            this.btn_CycleRun.BackColor = System.Drawing.Color.AliceBlue;
            this.btn_CycleRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_CycleRun.FlatAppearance.BorderSize = 0;
            this.btn_CycleRun.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.btn_CycleRun.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_CycleRun.Location = new System.Drawing.Point(293, 1);
            this.btn_CycleRun.Margin = new System.Windows.Forms.Padding(1);
            this.btn_CycleRun.Name = "btn_CycleRun";
            this.btn_CycleRun.Size = new System.Drawing.Size(144, 64);
            this.btn_CycleRun.TabIndex = 7;
            this.btn_CycleRun.Text = "Cycle Run";
            this.btn_CycleRun.UseVisualStyleBackColor = false;
            this.btn_CycleRun.Click += new System.EventHandler(this.btn_CycleRun_Click);
            // 
            // btn_CycleStop
            // 
            this.btn_CycleStop.BackColor = System.Drawing.Color.AliceBlue;
            this.btn_CycleStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_CycleStop.FlatAppearance.BorderSize = 0;
            this.btn_CycleStop.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.btn_CycleStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_CycleStop.Location = new System.Drawing.Point(439, 1);
            this.btn_CycleStop.Margin = new System.Windows.Forms.Padding(1);
            this.btn_CycleStop.Name = "btn_CycleStop";
            this.btn_CycleStop.Size = new System.Drawing.Size(146, 64);
            this.btn_CycleStop.TabIndex = 6;
            this.btn_CycleStop.Text = "Cycle Stop";
            this.btn_CycleStop.UseVisualStyleBackColor = false;
            this.btn_CycleStop.Click += new System.EventHandler(this.btn_CycleStop_Click);
            // 
            // tbx_CycleCount
            // 
            this.tbx_CycleCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_CycleCount.Font = new System.Drawing.Font("Segoe UI Semibold", 26F, System.Drawing.FontStyle.Bold);
            this.tbx_CycleCount.Location = new System.Drawing.Point(149, 3);
            this.tbx_CycleCount.Name = "tbx_CycleCount";
            this.tbx_CycleCount.Size = new System.Drawing.Size(140, 54);
            this.tbx_CycleCount.TabIndex = 8;
            this.tbx_CycleCount.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tbx_CycleCount_MouseClick);
            // 
            // pnl_PortTotalStatus
            // 
            this.pnl_PortTotalStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_PortTotalStatus.Location = new System.Drawing.Point(3, 3);
            this.pnl_PortTotalStatus.Name = "pnl_PortTotalStatus";
            this.pnl_PortTotalStatus.Size = new System.Drawing.Size(1114, 597);
            this.pnl_PortTotalStatus.TabIndex = 41;
            // 
            // Frm_PortTPMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1724, 683);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_PortTPMain";
            this.Text = "Frm_PortTPMain";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel_Right.ResumeLayout(false);
            this.groupBox_AutoRunStatus.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_AutoRunStatus)).EndInit();
            this.groupBox_CycleSettings.ResumeLayout(false);
            this.tableLayoutPanel21.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel15.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer UIUpdateTimer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox_CycleSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel21;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private System.Windows.Forms.Button btn_CycleStop;
        private System.Windows.Forms.Button btn_CycleRun;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Button btn_AutoRun;
        private System.Windows.Forms.Button btn_AutoStop;
        private System.Windows.Forms.Button btn_ModeChange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Right;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_CIMMode;
        private System.Windows.Forms.Button btn_MasterMode;
        private System.Windows.Forms.Button btn_DirectionChange;
        private System.Windows.Forms.Button btn_PowerOff;
        private System.Windows.Forms.Button btn_PowerOn;
        private System.Windows.Forms.GroupBox groupBox_AutoRunStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView DGV_AutoRunStatus;
        private System.Windows.Forms.Panel pnl_PortTotalStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.TextBox tbx_CycleCount;
    }
}