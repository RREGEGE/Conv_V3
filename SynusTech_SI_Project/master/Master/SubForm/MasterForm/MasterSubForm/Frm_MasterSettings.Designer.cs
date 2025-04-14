
namespace Master.SubForm.MasterForm.MasterSubForm
{
    partial class Frm_MasterSettings
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.UIUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.tabcontrol_Settings = new System.Windows.Forms.TabControl();
            this.tabPage_IOMap = new System.Windows.Forms.TabPage();
            this.groupBox_WMXIOParameterSettings = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel22 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel23 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel26 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.DGV_InputMapSettings = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel25 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.DGV_OutputMapSettings = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel24 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_IOMapRefresh = new System.Windows.Forms.Button();
            this.btn_IOMapApply = new System.Windows.Forms.Button();
            this.btn_IOMapApplyAndSave = new System.Windows.Forms.Button();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tabcontrol_Settings.SuspendLayout();
            this.tabPage_IOMap.SuspendLayout();
            this.groupBox_WMXIOParameterSettings.SuspendLayout();
            this.tableLayoutPanel22.SuspendLayout();
            this.tableLayoutPanel23.SuspendLayout();
            this.tableLayoutPanel26.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_InputMapSettings)).BeginInit();
            this.tableLayoutPanel25.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_OutputMapSettings)).BeginInit();
            this.tableLayoutPanel24.SuspendLayout();
            this.SuspendLayout();
            // 
            // UIUpdateTimer
            // 
            this.UIUpdateTimer.Interval = 250;
            this.UIUpdateTimer.Tick += new System.EventHandler(this.UIUpdateTimer_Tick);
            // 
            // tabcontrol_Settings
            // 
            this.tabcontrol_Settings.Controls.Add(this.tabPage_IOMap);
            this.tabcontrol_Settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabcontrol_Settings.Location = new System.Drawing.Point(0, 0);
            this.tabcontrol_Settings.Name = "tabcontrol_Settings";
            this.tabcontrol_Settings.SelectedIndex = 0;
            this.tabcontrol_Settings.Size = new System.Drawing.Size(1730, 754);
            this.tabcontrol_Settings.TabIndex = 1;
            // 
            // tabPage_IOMap
            // 
            this.tabPage_IOMap.BackColor = System.Drawing.Color.AliceBlue;
            this.tabPage_IOMap.Controls.Add(this.groupBox_WMXIOParameterSettings);
            this.tabPage_IOMap.Location = new System.Drawing.Point(4, 26);
            this.tabPage_IOMap.Name = "tabPage_IOMap";
            this.tabPage_IOMap.Size = new System.Drawing.Size(1722, 724);
            this.tabPage_IOMap.TabIndex = 7;
            this.tabPage_IOMap.Text = "I/O (WMX)";
            // 
            // groupBox_WMXIOParameterSettings
            // 
            this.groupBox_WMXIOParameterSettings.Controls.Add(this.tableLayoutPanel22);
            this.groupBox_WMXIOParameterSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_WMXIOParameterSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_WMXIOParameterSettings.Location = new System.Drawing.Point(0, 0);
            this.groupBox_WMXIOParameterSettings.Name = "groupBox_WMXIOParameterSettings";
            this.groupBox_WMXIOParameterSettings.Size = new System.Drawing.Size(1722, 724);
            this.groupBox_WMXIOParameterSettings.TabIndex = 49;
            this.groupBox_WMXIOParameterSettings.TabStop = false;
            this.groupBox_WMXIOParameterSettings.Text = "WMX I/O Parameter Settings";
            // 
            // tableLayoutPanel22
            // 
            this.tableLayoutPanel22.ColumnCount = 1;
            this.tableLayoutPanel22.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel22.Controls.Add(this.tableLayoutPanel23, 0, 1);
            this.tableLayoutPanel22.Controls.Add(this.tableLayoutPanel24, 0, 0);
            this.tableLayoutPanel22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel22.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel22.Name = "tableLayoutPanel22";
            this.tableLayoutPanel22.RowCount = 2;
            this.tableLayoutPanel22.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel22.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel22.Size = new System.Drawing.Size(1716, 696);
            this.tableLayoutPanel22.TabIndex = 1;
            // 
            // tableLayoutPanel23
            // 
            this.tableLayoutPanel23.ColumnCount = 2;
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel23.Controls.Add(this.tableLayoutPanel26, 1, 0);
            this.tableLayoutPanel23.Controls.Add(this.tableLayoutPanel25, 0, 0);
            this.tableLayoutPanel23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel23.Location = new System.Drawing.Point(0, 100);
            this.tableLayoutPanel23.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel23.Name = "tableLayoutPanel23";
            this.tableLayoutPanel23.RowCount = 1;
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel23.Size = new System.Drawing.Size(1716, 596);
            this.tableLayoutPanel23.TabIndex = 46;
            // 
            // tableLayoutPanel26
            // 
            this.tableLayoutPanel26.ColumnCount = 1;
            this.tableLayoutPanel26.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel26.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel26.Location = new System.Drawing.Point(861, 3);
            this.tableLayoutPanel26.Name = "tableLayoutPanel26";
            this.tableLayoutPanel26.RowCount = 1;
            this.tableLayoutPanel26.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel26.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 590F));
            this.tableLayoutPanel26.Size = new System.Drawing.Size(852, 590);
            this.tableLayoutPanel26.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.DGV_InputMapSettings);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(852, 590);
            this.panel3.TabIndex = 38;
            // 
            // DGV_InputMapSettings
            // 
            this.DGV_InputMapSettings.AllowUserToAddRows = false;
            this.DGV_InputMapSettings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_InputMapSettings.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.DGV_InputMapSettings.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_InputMapSettings.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_InputMapSettings.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV_InputMapSettings.ColumnHeadersHeight = 30;
            this.DGV_InputMapSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DGV_InputMapSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Column9,
            this.dataGridViewTextBoxColumn2,
            this.Column8,
            this.dataGridViewTextBoxColumn6,
            this.Column6});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_InputMapSettings.DefaultCellStyle = dataGridViewCellStyle8;
            this.DGV_InputMapSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_InputMapSettings.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DGV_InputMapSettings.EnableHeadersVisualStyles = false;
            this.DGV_InputMapSettings.GridColor = System.Drawing.Color.DarkGray;
            this.DGV_InputMapSettings.Location = new System.Drawing.Point(0, 0);
            this.DGV_InputMapSettings.Margin = new System.Windows.Forms.Padding(0);
            this.DGV_InputMapSettings.MultiSelect = false;
            this.DGV_InputMapSettings.Name = "DGV_InputMapSettings";
            this.DGV_InputMapSettings.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_InputMapSettings.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.DGV_InputMapSettings.RowHeadersVisible = false;
            this.DGV_InputMapSettings.RowTemplate.Height = 30;
            this.DGV_InputMapSettings.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_InputMapSettings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.DGV_InputMapSettings.Size = new System.Drawing.Size(852, 590);
            this.DGV_InputMapSettings.TabIndex = 35;
            // 
            // tableLayoutPanel25
            // 
            this.tableLayoutPanel25.ColumnCount = 1;
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel25.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel25.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel25.Name = "tableLayoutPanel25";
            this.tableLayoutPanel25.RowCount = 1;
            this.tableLayoutPanel25.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel25.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 590F));
            this.tableLayoutPanel25.Size = new System.Drawing.Size(852, 590);
            this.tableLayoutPanel25.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.DGV_OutputMapSettings);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(852, 590);
            this.panel2.TabIndex = 37;
            // 
            // DGV_OutputMapSettings
            // 
            this.DGV_OutputMapSettings.AllowUserToAddRows = false;
            this.DGV_OutputMapSettings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_OutputMapSettings.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.DGV_OutputMapSettings.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_OutputMapSettings.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_OutputMapSettings.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.DGV_OutputMapSettings.ColumnHeadersHeight = 30;
            this.DGV_OutputMapSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DGV_OutputMapSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column2,
            this.Column4,
            this.Column7,
            this.Column1,
            this.Column3});
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_OutputMapSettings.DefaultCellStyle = dataGridViewCellStyle17;
            this.DGV_OutputMapSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_OutputMapSettings.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DGV_OutputMapSettings.EnableHeadersVisualStyles = false;
            this.DGV_OutputMapSettings.GridColor = System.Drawing.Color.DarkGray;
            this.DGV_OutputMapSettings.Location = new System.Drawing.Point(0, 0);
            this.DGV_OutputMapSettings.Margin = new System.Windows.Forms.Padding(0);
            this.DGV_OutputMapSettings.MultiSelect = false;
            this.DGV_OutputMapSettings.Name = "DGV_OutputMapSettings";
            this.DGV_OutputMapSettings.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_OutputMapSettings.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.DGV_OutputMapSettings.RowHeadersVisible = false;
            this.DGV_OutputMapSettings.RowTemplate.Height = 30;
            this.DGV_OutputMapSettings.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_OutputMapSettings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.DGV_OutputMapSettings.Size = new System.Drawing.Size(852, 590);
            this.DGV_OutputMapSettings.TabIndex = 35;
            // 
            // tableLayoutPanel24
            // 
            this.tableLayoutPanel24.ColumnCount = 4;
            this.tableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel24.Controls.Add(this.btn_IOMapRefresh, 1, 0);
            this.tableLayoutPanel24.Controls.Add(this.btn_IOMapApply, 2, 0);
            this.tableLayoutPanel24.Controls.Add(this.btn_IOMapApplyAndSave, 3, 0);
            this.tableLayoutPanel24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel24.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel24.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel24.Name = "tableLayoutPanel24";
            this.tableLayoutPanel24.RowCount = 1;
            this.tableLayoutPanel24.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel24.Size = new System.Drawing.Size(1716, 100);
            this.tableLayoutPanel24.TabIndex = 37;
            // 
            // btn_IOMapRefresh
            // 
            this.btn_IOMapRefresh.BackColor = System.Drawing.Color.White;
            this.btn_IOMapRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_IOMapRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_IOMapRefresh.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_IOMapRefresh.FlatAppearance.BorderSize = 0;
            this.btn_IOMapRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_IOMapRefresh.Image = global::Master.Properties.Resources.icons8_reset_48;
            this.btn_IOMapRefresh.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_IOMapRefresh.Location = new System.Drawing.Point(1267, 1);
            this.btn_IOMapRefresh.Margin = new System.Windows.Forms.Padding(1);
            this.btn_IOMapRefresh.Name = "btn_IOMapRefresh";
            this.btn_IOMapRefresh.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.btn_IOMapRefresh.Size = new System.Drawing.Size(148, 98);
            this.btn_IOMapRefresh.TabIndex = 33;
            this.btn_IOMapRefresh.Text = "Refresh";
            this.btn_IOMapRefresh.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_IOMapRefresh.UseVisualStyleBackColor = false;
            this.btn_IOMapRefresh.Click += new System.EventHandler(this.btn_IOMapRefresh_Click);
            // 
            // btn_IOMapApply
            // 
            this.btn_IOMapApply.BackColor = System.Drawing.Color.White;
            this.btn_IOMapApply.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_IOMapApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_IOMapApply.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_IOMapApply.FlatAppearance.BorderSize = 0;
            this.btn_IOMapApply.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_IOMapApply.Image = global::Master.Properties.Resources.icons8_check_48;
            this.btn_IOMapApply.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_IOMapApply.Location = new System.Drawing.Point(1417, 1);
            this.btn_IOMapApply.Margin = new System.Windows.Forms.Padding(1);
            this.btn_IOMapApply.Name = "btn_IOMapApply";
            this.btn_IOMapApply.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.btn_IOMapApply.Size = new System.Drawing.Size(148, 98);
            this.btn_IOMapApply.TabIndex = 32;
            this.btn_IOMapApply.Text = "Apply";
            this.btn_IOMapApply.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_IOMapApply.UseVisualStyleBackColor = false;
            this.btn_IOMapApply.Click += new System.EventHandler(this.btn_IOMapApply_Click);
            // 
            // btn_IOMapApplyAndSave
            // 
            this.btn_IOMapApplyAndSave.BackColor = System.Drawing.Color.White;
            this.btn_IOMapApplyAndSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_IOMapApplyAndSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_IOMapApplyAndSave.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_IOMapApplyAndSave.FlatAppearance.BorderSize = 0;
            this.btn_IOMapApplyAndSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_IOMapApplyAndSave.Image = global::Master.Properties.Resources.icons8_save_48;
            this.btn_IOMapApplyAndSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_IOMapApplyAndSave.Location = new System.Drawing.Point(1567, 1);
            this.btn_IOMapApplyAndSave.Margin = new System.Windows.Forms.Padding(1);
            this.btn_IOMapApplyAndSave.Name = "btn_IOMapApplyAndSave";
            this.btn_IOMapApplyAndSave.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.btn_IOMapApplyAndSave.Size = new System.Drawing.Size(148, 98);
            this.btn_IOMapApplyAndSave.TabIndex = 27;
            this.btn_IOMapApplyAndSave.Text = "Apply And Save";
            this.btn_IOMapApplyAndSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_IOMapApplyAndSave.UseVisualStyleBackColor = false;
            this.btn_IOMapApplyAndSave.Click += new System.EventHandler(this.btn_IOMapApplyAndSave_Click);
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle11;
            this.Column5.FillWeight = 200F;
            this.Column5.HeaderText = "Output Name";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle12;
            this.Column2.HeaderText = "StartAddr [0~7999]";
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle13;
            this.Column4.HeaderText = "Bit Num [0~7]";
            this.Column4.Name = "Column4";
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column7.DefaultCellStyle = dataGridViewCellStyle14;
            this.Column7.HeaderText = "Ctrl Status";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column7.Width = 80;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle15;
            this.Column1.HeaderText = "Bit Row Data";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 80;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle16;
            this.Column3.HeaderText = "Bit Row Invert";
            this.Column3.Items.AddRange(new object[] {
            "False",
            "True"});
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.Width = 80;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn1.FillWeight = 200F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Input Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column9
            // 
            this.Column9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column9.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column9.HeaderText = "StartAddr [0~7999]";
            this.Column9.Name = "Column9";
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTextBoxColumn2.HeaderText = "Bit Num [0~7]";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column8.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column8.HeaderText = "Ctrl Status";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column8.Width = 80;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn6.HeaderText = "Bit Row Data";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn6.Width = 80;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column6.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column6.HeaderText = "Bit Row Invert";
            this.Column6.Items.AddRange(new object[] {
            "False",
            "True"});
            this.Column6.Name = "Column6";
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column6.Width = 80;
            // 
            // Frm_MasterSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1730, 754);
            this.Controls.Add(this.tabcontrol_Settings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_MasterSettings";
            this.Text = "Frm_PortTPMain";
            this.tabcontrol_Settings.ResumeLayout(false);
            this.tabPage_IOMap.ResumeLayout(false);
            this.groupBox_WMXIOParameterSettings.ResumeLayout(false);
            this.tableLayoutPanel22.ResumeLayout(false);
            this.tableLayoutPanel23.ResumeLayout(false);
            this.tableLayoutPanel26.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_InputMapSettings)).EndInit();
            this.tableLayoutPanel25.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_OutputMapSettings)).EndInit();
            this.tableLayoutPanel24.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer UIUpdateTimer;
        private System.Windows.Forms.TabControl tabcontrol_Settings;
        private System.Windows.Forms.TabPage tabPage_IOMap;
        private System.Windows.Forms.GroupBox groupBox_WMXIOParameterSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel22;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel23;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel24;
        private System.Windows.Forms.Button btn_IOMapApplyAndSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel26;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel25;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView DGV_InputMapSettings;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView DGV_OutputMapSettings;
        private System.Windows.Forms.Button btn_IOMapRefresh;
        private System.Windows.Forms.Button btn_IOMapApply;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column3;
    }
}