
namespace Master.SubForm
{
    partial class Frm_MapSender
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cbx_EquipType = new System.Windows.Forms.ComboBox();
            this.cbx_Direction = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_SendWordMap = new System.Windows.Forms.Button();
            this.tbx_WordMapLength = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbx_WordMapStartAddr = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbx_BitMapLength = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_SendBitMap = new System.Windows.Forms.Button();
            this.tbx_BitMapStartAddr = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_RemainingTimeTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.DGV_BitMap = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_WordMap = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UIUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_BitMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_WordMap)).BeginInit();
            this.SuspendLayout();
            // 
            // cbx_EquipType
            // 
            this.cbx_EquipType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbx_EquipType.FormattingEnabled = true;
            this.cbx_EquipType.Location = new System.Drawing.Point(231, 4);
            this.cbx_EquipType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbx_EquipType.Name = "cbx_EquipType";
            this.cbx_EquipType.Size = new System.Drawing.Size(222, 25);
            this.cbx_EquipType.TabIndex = 0;
            // 
            // cbx_Direction
            // 
            this.cbx_Direction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbx_Direction.FormattingEnabled = true;
            this.cbx_Direction.Location = new System.Drawing.Point(687, 4);
            this.cbx_Direction.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbx_Direction.Name = "cbx_Direction";
            this.cbx_Direction.Size = new System.Drawing.Size(224, 25);
            this.cbx_Direction.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(914, 637);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.Controls.Add(this.btn_SendWordMap, 4, 1);
            this.tableLayoutPanel4.Controls.Add(this.tbx_WordMapLength, 3, 1);
            this.tableLayoutPanel4.Controls.Add(this.label5, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.tbx_WordMapStartAddr, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.tbx_BitMapLength, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btn_SendBitMap, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.tbx_BitMapStartAddr, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 566);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(914, 71);
            this.tableLayoutPanel4.TabIndex = 37;
            // 
            // btn_SendWordMap
            // 
            this.btn_SendWordMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_SendWordMap.Location = new System.Drawing.Point(731, 39);
            this.btn_SendWordMap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_SendWordMap.Name = "btn_SendWordMap";
            this.btn_SendWordMap.Size = new System.Drawing.Size(180, 28);
            this.btn_SendWordMap.TabIndex = 14;
            this.btn_SendWordMap.Text = "Send";
            this.btn_SendWordMap.UseVisualStyleBackColor = true;
            this.btn_SendWordMap.Click += new System.EventHandler(this.btn_SendWordMap_Click);
            // 
            // tbx_WordMapLength
            // 
            this.tbx_WordMapLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_WordMapLength.Location = new System.Drawing.Point(549, 39);
            this.tbx_WordMapLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbx_WordMapLength.Name = "tbx_WordMapLength";
            this.tbx_WordMapLength.Size = new System.Drawing.Size(176, 25);
            this.tbx_WordMapLength.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(367, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(176, 36);
            this.label5.TabIndex = 12;
            this.label5.Text = "Data Length :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbx_WordMapStartAddr
            // 
            this.tbx_WordMapStartAddr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_WordMapStartAddr.Location = new System.Drawing.Point(185, 39);
            this.tbx_WordMapStartAddr.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbx_WordMapStartAddr.Name = "tbx_WordMapStartAddr";
            this.tbx_WordMapStartAddr.Size = new System.Drawing.Size(176, 25);
            this.tbx_WordMapStartAddr.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 36);
            this.label4.TabIndex = 10;
            this.label4.Text = "WordMap Start Addr :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbx_BitMapLength
            // 
            this.tbx_BitMapLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_BitMapLength.Location = new System.Drawing.Point(549, 4);
            this.tbx_BitMapLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbx_BitMapLength.Name = "tbx_BitMapLength";
            this.tbx_BitMapLength.Size = new System.Drawing.Size(176, 25);
            this.tbx_BitMapLength.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(367, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 35);
            this.label2.TabIndex = 6;
            this.label2.Text = "Data Length :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 35);
            this.label3.TabIndex = 5;
            this.label3.Text = "BitMap Start Addr :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_SendBitMap
            // 
            this.btn_SendBitMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_SendBitMap.Location = new System.Drawing.Point(731, 4);
            this.btn_SendBitMap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_SendBitMap.Name = "btn_SendBitMap";
            this.btn_SendBitMap.Size = new System.Drawing.Size(180, 27);
            this.btn_SendBitMap.TabIndex = 7;
            this.btn_SendBitMap.Text = "Send";
            this.btn_SendBitMap.UseVisualStyleBackColor = true;
            this.btn_SendBitMap.Click += new System.EventHandler(this.btn_SendBitMap_Click);
            // 
            // tbx_BitMapStartAddr
            // 
            this.tbx_BitMapStartAddr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_BitMapStartAddr.Location = new System.Drawing.Point(185, 4);
            this.tbx_BitMapStartAddr.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbx_BitMapStartAddr.Name = "tbx_BitMapStartAddr";
            this.tbx_BitMapStartAddr.Size = new System.Drawing.Size(176, 25);
            this.tbx_BitMapStartAddr.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbl_RemainingTimeTitle, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbx_EquipType, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbx_Direction, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(914, 35);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(459, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 35);
            this.label1.TabIndex = 6;
            this.label1.Text = "Message Direction :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_RemainingTimeTitle
            // 
            this.lbl_RemainingTimeTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_RemainingTimeTitle.Location = new System.Drawing.Point(3, 0);
            this.lbl_RemainingTimeTitle.Name = "lbl_RemainingTimeTitle";
            this.lbl_RemainingTimeTitle.Size = new System.Drawing.Size(222, 35);
            this.lbl_RemainingTimeTitle.TabIndex = 5;
            this.lbl_RemainingTimeTitle.Text = "EquipType :";
            this.lbl_RemainingTimeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel3.Controls.Add(this.DGV_BitMap, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.DGV_WordMap, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 39);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(908, 523);
            this.tableLayoutPanel3.TabIndex = 36;
            // 
            // DGV_BitMap
            // 
            this.DGV_BitMap.AllowUserToAddRows = false;
            this.DGV_BitMap.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_BitMap.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.DGV_BitMap.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_BitMap.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_BitMap.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV_BitMap.ColumnHeadersHeight = 25;
            this.DGV_BitMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DGV_BitMap.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_BitMap.DefaultCellStyle = dataGridViewCellStyle4;
            this.DGV_BitMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_BitMap.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DGV_BitMap.EnableHeadersVisualStyles = false;
            this.DGV_BitMap.GridColor = System.Drawing.Color.LightGray;
            this.DGV_BitMap.Location = new System.Drawing.Point(0, 0);
            this.DGV_BitMap.Margin = new System.Windows.Forms.Padding(0);
            this.DGV_BitMap.MultiSelect = false;
            this.DGV_BitMap.Name = "DGV_BitMap";
            this.DGV_BitMap.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_BitMap.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.DGV_BitMap.RowHeadersVisible = false;
            this.DGV_BitMap.RowTemplate.Height = 23;
            this.DGV_BitMap.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_BitMap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.DGV_BitMap.Size = new System.Drawing.Size(227, 523);
            this.DGV_BitMap.TabIndex = 35;
            this.DGV_BitMap.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_BitMap_CellClick);
            this.DGV_BitMap.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_BitMap_CellContentClick);
            this.DGV_BitMap.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_BitMap_CellEndEdit);
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn10.HeaderText = "BitMap Index";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn11.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn11.HeaderText = "BitMap Value";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DGV_WordMap
            // 
            this.DGV_WordMap.AllowUserToAddRows = false;
            this.DGV_WordMap.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_WordMap.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.DGV_WordMap.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_WordMap.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_WordMap.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.DGV_WordMap.ColumnHeadersHeight = 25;
            this.DGV_WordMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DGV_WordMap.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.Column1,
            this.Column2});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_WordMap.DefaultCellStyle = dataGridViewCellStyle11;
            this.DGV_WordMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_WordMap.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DGV_WordMap.EnableHeadersVisualStyles = false;
            this.DGV_WordMap.GridColor = System.Drawing.Color.LightGray;
            this.DGV_WordMap.Location = new System.Drawing.Point(227, 0);
            this.DGV_WordMap.Margin = new System.Windows.Forms.Padding(0);
            this.DGV_WordMap.MultiSelect = false;
            this.DGV_WordMap.Name = "DGV_WordMap";
            this.DGV_WordMap.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_WordMap.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.DGV_WordMap.RowHeadersVisible = false;
            this.DGV_WordMap.RowTemplate.Height = 23;
            this.DGV_WordMap.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_WordMap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.DGV_WordMap.Size = new System.Drawing.Size(681, 523);
            this.DGV_WordMap.TabIndex = 36;
            this.DGV_WordMap.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_WordMap_CellClick);
            this.DGV_WordMap.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_WordMap_CellContentClick);
            this.DGV_WordMap.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_WordMap_CellEndEdit);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn3.HeaderText = "WordMap Index";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn4.HeaderText = "WordMap Value(Hex)";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle9;
            this.Column1.HeaderText = "WordMap Value(Dec)";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle10;
            this.Column2.HeaderText = "WordMap Value(Str)";
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UIUpdateTimer
            // 
            this.UIUpdateTimer.Interval = 100;
            this.UIUpdateTimer.Tick += new System.EventHandler(this.UIUpdateTimer_Tick);
            // 
            // Frm_MapSender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 637);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_MapSender";
            this.Text = "Map Data Sender";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_BitMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_WordMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbx_EquipType;
        private System.Windows.Forms.ComboBox cbx_Direction;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_RemainingTimeTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView DGV_BitMap;
        private System.Windows.Forms.DataGridView DGV_WordMap;
        private System.Windows.Forms.Timer UIUpdateTimer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btn_SendWordMap;
        private System.Windows.Forms.TextBox tbx_WordMapLength;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbx_WordMapStartAddr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbx_BitMapLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_SendBitMap;
        private System.Windows.Forms.TextBox tbx_BitMapStartAddr;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}