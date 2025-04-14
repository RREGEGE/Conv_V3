
namespace RackMaster.SUBFORM.SettingPage {
    partial class SettingPage_Teaching {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gboxAutoTeaching = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvShelfData = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvAutoTeachingStatus = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAutoTeachingStop = new System.Windows.Forms.Button();
            this.btnAutoTeachingStart = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtAutoTeachingTargetZ = new System.Windows.Forms.TextBox();
            this.txtAutoTeachingTargetX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblAutoTeachingIDError = new System.Windows.Forms.Label();
            this.txtAutoTeachingID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gboxManualTeaching = new System.Windows.Forms.GroupBox();
            this.btnMaintTeaching = new System.Windows.Forms.Button();
            this.lblTeachingID = new System.Windows.Forms.Label();
            this.btnXZTeaching = new System.Windows.Forms.Button();
            this.btnOneShelfTeaching = new System.Windows.Forms.Button();
            this.btnZUpDownTeaching = new System.Windows.Forms.Button();
            this.btnAAllTeaching = new System.Windows.Forms.Button();
            this.btnTAllTeacing = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTeachZDown = new System.Windows.Forms.TextBox();
            this.txtCurZDown = new System.Windows.Forms.TextBox();
            this.txtTeachZUp = new System.Windows.Forms.TextBox();
            this.txtCurZUp = new System.Windows.Forms.TextBox();
            this.txtTeachTPos = new System.Windows.Forms.TextBox();
            this.txtCurTPos = new System.Windows.Forms.TextBox();
            this.txtTeachAPos = new System.Windows.Forms.TextBox();
            this.txtCurAPos = new System.Windows.Forms.TextBox();
            this.txtTeachZPos = new System.Windows.Forms.TextBox();
            this.txtCurZPos = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTeachXPos = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCurXPos = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.saveTimer = new System.Windows.Forms.Timer(this.components);
            this.autoTeachingTimer = new System.Windows.Forms.Timer(this.components);
            this.gboxAutoTeaching.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShelfData)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAutoTeachingStatus)).BeginInit();
            this.gboxManualTeaching.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboxAutoTeaching
            // 
            this.gboxAutoTeaching.Controls.Add(this.groupBox2);
            this.gboxAutoTeaching.Controls.Add(this.groupBox1);
            this.gboxAutoTeaching.Controls.Add(this.btnAutoTeachingStop);
            this.gboxAutoTeaching.Controls.Add(this.btnAutoTeachingStart);
            this.gboxAutoTeaching.Controls.Add(this.label21);
            this.gboxAutoTeaching.Controls.Add(this.label20);
            this.gboxAutoTeaching.Controls.Add(this.label19);
            this.gboxAutoTeaching.Controls.Add(this.txtAutoTeachingTargetZ);
            this.gboxAutoTeaching.Controls.Add(this.txtAutoTeachingTargetX);
            this.gboxAutoTeaching.Controls.Add(this.label3);
            this.gboxAutoTeaching.Controls.Add(this.lblAutoTeachingIDError);
            this.gboxAutoTeaching.Controls.Add(this.txtAutoTeachingID);
            this.gboxAutoTeaching.Controls.Add(this.label2);
            this.gboxAutoTeaching.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxAutoTeaching.Location = new System.Drawing.Point(687, 6);
            this.gboxAutoTeaching.Name = "gboxAutoTeaching";
            this.gboxAutoTeaching.Size = new System.Drawing.Size(1002, 699);
            this.gboxAutoTeaching.TabIndex = 4;
            this.gboxAutoTeaching.TabStop = false;
            this.gboxAutoTeaching.Text = "Auto Teaching";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvShelfData);
            this.groupBox2.Location = new System.Drawing.Point(6, 224);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(397, 469);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Auto Teaching Shelf Data";
            // 
            // dgvShelfData
            // 
            this.dgvShelfData.AllowUserToAddRows = false;
            this.dgvShelfData.AllowUserToDeleteRows = false;
            this.dgvShelfData.AllowUserToResizeColumns = false;
            this.dgvShelfData.AllowUserToResizeRows = false;
            this.dgvShelfData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvShelfData.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvShelfData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvShelfData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShelfData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14});
            this.dgvShelfData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvShelfData.Location = new System.Drawing.Point(3, 29);
            this.dgvShelfData.Name = "dgvShelfData";
            this.dgvShelfData.RowHeadersVisible = false;
            this.dgvShelfData.RowTemplate.Height = 23;
            this.dgvShelfData.Size = new System.Drawing.Size(391, 437);
            this.dgvShelfData.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "Name";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.Width = 89;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn14.HeaderText = "State";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvAutoTeachingStatus);
            this.groupBox1.Location = new System.Drawing.Point(409, 224);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(587, 469);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto Teaching Status";
            // 
            // dgvAutoTeachingStatus
            // 
            this.dgvAutoTeachingStatus.AllowUserToAddRows = false;
            this.dgvAutoTeachingStatus.AllowUserToDeleteRows = false;
            this.dgvAutoTeachingStatus.AllowUserToResizeColumns = false;
            this.dgvAutoTeachingStatus.AllowUserToResizeRows = false;
            this.dgvAutoTeachingStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAutoTeachingStatus.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAutoTeachingStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvAutoTeachingStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAutoTeachingStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgvAutoTeachingStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAutoTeachingStatus.Location = new System.Drawing.Point(3, 29);
            this.dgvAutoTeachingStatus.Name = "dgvAutoTeachingStatus";
            this.dgvAutoTeachingStatus.RowHeadersVisible = false;
            this.dgvAutoTeachingStatus.RowTemplate.Height = 23;
            this.dgvAutoTeachingStatus.Size = new System.Drawing.Size(581, 437);
            this.dgvAutoTeachingStatus.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 89;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "State";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // btnAutoTeachingStop
            // 
            this.btnAutoTeachingStop.BackColor = System.Drawing.Color.White;
            this.btnAutoTeachingStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAutoTeachingStop.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoTeachingStop.Location = new System.Drawing.Point(694, 125);
            this.btnAutoTeachingStop.Name = "btnAutoTeachingStop";
            this.btnAutoTeachingStop.Size = new System.Drawing.Size(302, 93);
            this.btnAutoTeachingStop.TabIndex = 48;
            this.btnAutoTeachingStop.Text = "AUTO TEACHING STOP";
            this.btnAutoTeachingStop.UseVisualStyleBackColor = false;
            this.btnAutoTeachingStop.Click += new System.EventHandler(this.btnAutoTeachingStop_Click);
            // 
            // btnAutoTeachingStart
            // 
            this.btnAutoTeachingStart.BackColor = System.Drawing.Color.White;
            this.btnAutoTeachingStart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAutoTeachingStart.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoTeachingStart.Location = new System.Drawing.Point(694, 26);
            this.btnAutoTeachingStart.Name = "btnAutoTeachingStart";
            this.btnAutoTeachingStart.Size = new System.Drawing.Size(302, 93);
            this.btnAutoTeachingStart.TabIndex = 40;
            this.btnAutoTeachingStart.Text = "AUTO TEACHING START";
            this.btnAutoTeachingStart.UseVisualStyleBackColor = false;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(497, 128);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(69, 40);
            this.label21.TabIndex = 47;
            this.label21.Text = "mm";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(497, 79);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(69, 40);
            this.label20.TabIndex = 40;
            this.label20.Text = "mm";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(149, 128);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(144, 40);
            this.label19.TabIndex = 46;
            this.label19.Text = "TARGET Z";
            // 
            // txtAutoTeachingTargetZ
            // 
            this.txtAutoTeachingTargetZ.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAutoTeachingTargetZ.Location = new System.Drawing.Point(300, 127);
            this.txtAutoTeachingTargetZ.Name = "txtAutoTeachingTargetZ";
            this.txtAutoTeachingTargetZ.Size = new System.Drawing.Size(191, 43);
            this.txtAutoTeachingTargetZ.TabIndex = 45;
            // 
            // txtAutoTeachingTargetX
            // 
            this.txtAutoTeachingTargetX.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAutoTeachingTargetX.Location = new System.Drawing.Point(300, 78);
            this.txtAutoTeachingTargetX.Name = "txtAutoTeachingTargetX";
            this.txtAutoTeachingTargetX.Size = new System.Drawing.Size(191, 43);
            this.txtAutoTeachingTargetX.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(149, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 40);
            this.label3.TabIndex = 43;
            this.label3.Text = "TARGET X";
            // 
            // lblAutoTeachingIDError
            // 
            this.lblAutoTeachingIDError.BackColor = System.Drawing.Color.GreenYellow;
            this.lblAutoTeachingIDError.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoTeachingIDError.Location = new System.Drawing.Point(497, 29);
            this.lblAutoTeachingIDError.Name = "lblAutoTeachingIDError";
            this.lblAutoTeachingIDError.Size = new System.Drawing.Size(191, 43);
            this.lblAutoTeachingIDError.TabIndex = 42;
            this.lblAutoTeachingIDError.Text = "ID ERROR";
            this.lblAutoTeachingIDError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtAutoTeachingID
            // 
            this.txtAutoTeachingID.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAutoTeachingID.Location = new System.Drawing.Point(300, 29);
            this.txtAutoTeachingID.Name = "txtAutoTeachingID";
            this.txtAutoTeachingID.Size = new System.Drawing.Size(191, 43);
            this.txtAutoTeachingID.TabIndex = 41;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(277, 40);
            this.label2.TabIndex = 40;
            this.label2.Text = "AUTO TEACHING ID";
            // 
            // gboxManualTeaching
            // 
            this.gboxManualTeaching.Controls.Add(this.btnMaintTeaching);
            this.gboxManualTeaching.Controls.Add(this.lblTeachingID);
            this.gboxManualTeaching.Controls.Add(this.btnXZTeaching);
            this.gboxManualTeaching.Controls.Add(this.btnOneShelfTeaching);
            this.gboxManualTeaching.Controls.Add(this.btnZUpDownTeaching);
            this.gboxManualTeaching.Controls.Add(this.btnAAllTeaching);
            this.gboxManualTeaching.Controls.Add(this.btnTAllTeacing);
            this.gboxManualTeaching.Controls.Add(this.label18);
            this.gboxManualTeaching.Controls.Add(this.label17);
            this.gboxManualTeaching.Controls.Add(this.label16);
            this.gboxManualTeaching.Controls.Add(this.label15);
            this.gboxManualTeaching.Controls.Add(this.label14);
            this.gboxManualTeaching.Controls.Add(this.label13);
            this.gboxManualTeaching.Controls.Add(this.label12);
            this.gboxManualTeaching.Controls.Add(this.label11);
            this.gboxManualTeaching.Controls.Add(this.label10);
            this.gboxManualTeaching.Controls.Add(this.label9);
            this.gboxManualTeaching.Controls.Add(this.txtTeachZDown);
            this.gboxManualTeaching.Controls.Add(this.txtCurZDown);
            this.gboxManualTeaching.Controls.Add(this.txtTeachZUp);
            this.gboxManualTeaching.Controls.Add(this.txtCurZUp);
            this.gboxManualTeaching.Controls.Add(this.txtTeachTPos);
            this.gboxManualTeaching.Controls.Add(this.txtCurTPos);
            this.gboxManualTeaching.Controls.Add(this.txtTeachAPos);
            this.gboxManualTeaching.Controls.Add(this.txtCurAPos);
            this.gboxManualTeaching.Controls.Add(this.txtTeachZPos);
            this.gboxManualTeaching.Controls.Add(this.txtCurZPos);
            this.gboxManualTeaching.Controls.Add(this.label8);
            this.gboxManualTeaching.Controls.Add(this.label7);
            this.gboxManualTeaching.Controls.Add(this.txtTeachXPos);
            this.gboxManualTeaching.Controls.Add(this.label6);
            this.gboxManualTeaching.Controls.Add(this.txtCurXPos);
            this.gboxManualTeaching.Controls.Add(this.label5);
            this.gboxManualTeaching.Controls.Add(this.txtID);
            this.gboxManualTeaching.Controls.Add(this.label4);
            this.gboxManualTeaching.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxManualTeaching.Location = new System.Drawing.Point(6, 6);
            this.gboxManualTeaching.Name = "gboxManualTeaching";
            this.gboxManualTeaching.Size = new System.Drawing.Size(675, 693);
            this.gboxManualTeaching.TabIndex = 5;
            this.gboxManualTeaching.TabStop = false;
            this.gboxManualTeaching.Text = "Manual Teaching";
            // 
            // btnMaintTeaching
            // 
            this.btnMaintTeaching.BackColor = System.Drawing.Color.White;
            this.btnMaintTeaching.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMaintTeaching.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaintTeaching.Location = new System.Drawing.Point(454, 553);
            this.btnMaintTeaching.Name = "btnMaintTeaching";
            this.btnMaintTeaching.Size = new System.Drawing.Size(215, 110);
            this.btnMaintTeaching.TabIndex = 40;
            this.btnMaintTeaching.Text = "MAINT TEACHING";
            this.btnMaintTeaching.UseVisualStyleBackColor = false;
            // 
            // lblTeachingID
            // 
            this.lblTeachingID.BackColor = System.Drawing.Color.GreenYellow;
            this.lblTeachingID.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTeachingID.Location = new System.Drawing.Point(404, 26);
            this.lblTeachingID.Name = "lblTeachingID";
            this.lblTeachingID.Size = new System.Drawing.Size(191, 43);
            this.lblTeachingID.TabIndex = 39;
            this.lblTeachingID.Text = "ID ERROR";
            this.lblTeachingID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnXZTeaching
            // 
            this.btnXZTeaching.BackColor = System.Drawing.Color.White;
            this.btnXZTeaching.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnXZTeaching.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXZTeaching.Location = new System.Drawing.Point(233, 553);
            this.btnXZTeaching.Name = "btnXZTeaching";
            this.btnXZTeaching.Size = new System.Drawing.Size(215, 110);
            this.btnXZTeaching.TabIndex = 38;
            this.btnXZTeaching.Text = "X/Z AXIS TEACHING";
            this.btnXZTeaching.UseVisualStyleBackColor = false;
            // 
            // btnOneShelfTeaching
            // 
            this.btnOneShelfTeaching.BackColor = System.Drawing.Color.White;
            this.btnOneShelfTeaching.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOneShelfTeaching.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOneShelfTeaching.Location = new System.Drawing.Point(12, 437);
            this.btnOneShelfTeaching.Name = "btnOneShelfTeaching";
            this.btnOneShelfTeaching.Size = new System.Drawing.Size(215, 110);
            this.btnOneShelfTeaching.TabIndex = 34;
            this.btnOneShelfTeaching.Text = "ONE SHELF TEACHING";
            this.btnOneShelfTeaching.UseVisualStyleBackColor = false;
            // 
            // btnZUpDownTeaching
            // 
            this.btnZUpDownTeaching.BackColor = System.Drawing.Color.White;
            this.btnZUpDownTeaching.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnZUpDownTeaching.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZUpDownTeaching.Location = new System.Drawing.Point(12, 553);
            this.btnZUpDownTeaching.Name = "btnZUpDownTeaching";
            this.btnZUpDownTeaching.Size = new System.Drawing.Size(215, 110);
            this.btnZUpDownTeaching.TabIndex = 37;
            this.btnZUpDownTeaching.Text = "Z-UP/DOWN TEACHING";
            this.btnZUpDownTeaching.UseVisualStyleBackColor = false;
            // 
            // btnAAllTeaching
            // 
            this.btnAAllTeaching.BackColor = System.Drawing.Color.White;
            this.btnAAllTeaching.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAAllTeaching.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAAllTeaching.Location = new System.Drawing.Point(454, 437);
            this.btnAAllTeaching.Name = "btnAAllTeaching";
            this.btnAAllTeaching.Size = new System.Drawing.Size(215, 110);
            this.btnAAllTeaching.TabIndex = 36;
            this.btnAAllTeaching.Text = "A-AXIS ALL TEACHING";
            this.btnAAllTeaching.UseVisualStyleBackColor = false;
            // 
            // btnTAllTeacing
            // 
            this.btnTAllTeacing.BackColor = System.Drawing.Color.White;
            this.btnTAllTeacing.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTAllTeacing.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTAllTeacing.Location = new System.Drawing.Point(233, 437);
            this.btnTAllTeacing.Name = "btnTAllTeacing";
            this.btnTAllTeacing.Size = new System.Drawing.Size(215, 110);
            this.btnTAllTeacing.TabIndex = 35;
            this.btnTAllTeacing.Text = "T-AXIS ALL TEACHING";
            this.btnTAllTeacing.UseVisualStyleBackColor = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(601, 389);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(69, 40);
            this.label18.TabIndex = 33;
            this.label18.Text = "mm";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(601, 340);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(69, 40);
            this.label17.TabIndex = 32;
            this.label17.Text = "mm";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(601, 291);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(69, 40);
            this.label16.TabIndex = 31;
            this.label16.Text = "mm";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(601, 242);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(69, 40);
            this.label15.TabIndex = 30;
            this.label15.Text = "mm";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(601, 193);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 40);
            this.label14.TabIndex = 29;
            this.label14.Text = "mm";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(16, 389);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(139, 40);
            this.label13.TabIndex = 28;
            this.label13.Text = "Z-DOWN";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(72, 340);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 40);
            this.label12.TabIndex = 27;
            this.label12.Text = "Z-UP";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(49, 291);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(106, 40);
            this.label11.TabIndex = 26;
            this.label11.Text = "T-AXIS";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(46, 242);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 40);
            this.label10.TabIndex = 25;
            this.label10.Text = "A-AXIS";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(48, 193);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 40);
            this.label9.TabIndex = 24;
            this.label9.Text = "Z-AXIS";
            // 
            // txtTeachZDown
            // 
            this.txtTeachZDown.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTeachZDown.Location = new System.Drawing.Point(381, 388);
            this.txtTeachZDown.Name = "txtTeachZDown";
            this.txtTeachZDown.Size = new System.Drawing.Size(214, 43);
            this.txtTeachZDown.TabIndex = 23;
            // 
            // txtCurZDown
            // 
            this.txtCurZDown.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurZDown.Location = new System.Drawing.Point(161, 388);
            this.txtCurZDown.Name = "txtCurZDown";
            this.txtCurZDown.Size = new System.Drawing.Size(214, 43);
            this.txtCurZDown.TabIndex = 22;
            // 
            // txtTeachZUp
            // 
            this.txtTeachZUp.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTeachZUp.Location = new System.Drawing.Point(381, 339);
            this.txtTeachZUp.Name = "txtTeachZUp";
            this.txtTeachZUp.Size = new System.Drawing.Size(214, 43);
            this.txtTeachZUp.TabIndex = 21;
            // 
            // txtCurZUp
            // 
            this.txtCurZUp.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurZUp.Location = new System.Drawing.Point(161, 339);
            this.txtCurZUp.Name = "txtCurZUp";
            this.txtCurZUp.Size = new System.Drawing.Size(214, 43);
            this.txtCurZUp.TabIndex = 20;
            // 
            // txtTeachTPos
            // 
            this.txtTeachTPos.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTeachTPos.Location = new System.Drawing.Point(381, 290);
            this.txtTeachTPos.Name = "txtTeachTPos";
            this.txtTeachTPos.Size = new System.Drawing.Size(214, 43);
            this.txtTeachTPos.TabIndex = 19;
            // 
            // txtCurTPos
            // 
            this.txtCurTPos.BackColor = System.Drawing.Color.White;
            this.txtCurTPos.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurTPos.Location = new System.Drawing.Point(161, 290);
            this.txtCurTPos.Name = "txtCurTPos";
            this.txtCurTPos.ReadOnly = true;
            this.txtCurTPos.Size = new System.Drawing.Size(214, 43);
            this.txtCurTPos.TabIndex = 18;
            // 
            // txtTeachAPos
            // 
            this.txtTeachAPos.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTeachAPos.Location = new System.Drawing.Point(381, 241);
            this.txtTeachAPos.Name = "txtTeachAPos";
            this.txtTeachAPos.Size = new System.Drawing.Size(214, 43);
            this.txtTeachAPos.TabIndex = 17;
            // 
            // txtCurAPos
            // 
            this.txtCurAPos.BackColor = System.Drawing.Color.White;
            this.txtCurAPos.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurAPos.Location = new System.Drawing.Point(161, 241);
            this.txtCurAPos.Name = "txtCurAPos";
            this.txtCurAPos.ReadOnly = true;
            this.txtCurAPos.Size = new System.Drawing.Size(214, 43);
            this.txtCurAPos.TabIndex = 16;
            // 
            // txtTeachZPos
            // 
            this.txtTeachZPos.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTeachZPos.Location = new System.Drawing.Point(381, 192);
            this.txtTeachZPos.Name = "txtTeachZPos";
            this.txtTeachZPos.Size = new System.Drawing.Size(214, 43);
            this.txtTeachZPos.TabIndex = 15;
            // 
            // txtCurZPos
            // 
            this.txtCurZPos.BackColor = System.Drawing.Color.White;
            this.txtCurZPos.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurZPos.Location = new System.Drawing.Point(161, 192);
            this.txtCurZPos.Name = "txtCurZPos";
            this.txtCurZPos.ReadOnly = true;
            this.txtCurZPos.Size = new System.Drawing.Size(214, 43);
            this.txtCurZPos.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(601, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 40);
            this.label8.TabIndex = 13;
            this.label8.Text = "mm";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(410, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(156, 40);
            this.label7.TabIndex = 12;
            this.label7.Text = "TEACHING";
            // 
            // txtTeachXPos
            // 
            this.txtTeachXPos.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTeachXPos.Location = new System.Drawing.Point(381, 143);
            this.txtTeachXPos.Name = "txtTeachXPos";
            this.txtTeachXPos.Size = new System.Drawing.Size(214, 43);
            this.txtTeachXPos.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(199, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(144, 40);
            this.label6.TabIndex = 10;
            this.label6.Text = "CURRENT";
            // 
            // txtCurXPos
            // 
            this.txtCurXPos.BackColor = System.Drawing.Color.White;
            this.txtCurXPos.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurXPos.Location = new System.Drawing.Point(161, 143);
            this.txtCurXPos.Name = "txtCurXPos";
            this.txtCurXPos.ReadOnly = true;
            this.txtCurXPos.Size = new System.Drawing.Size(214, 43);
            this.txtCurXPos.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(47, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 40);
            this.label5.TabIndex = 8;
            this.label5.Text = "X-AXIS";
            // 
            // txtID
            // 
            this.txtID.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtID.Location = new System.Drawing.Point(207, 26);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(191, 43);
            this.txtID.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(193, 40);
            this.label4.TabIndex = 5;
            this.label4.Text = "TEACHING ID";
            // 
            // saveTimer
            // 
            this.saveTimer.Tick += new System.EventHandler(this.saveTimer_Tick);
            // 
            // autoTeachingTimer
            // 
            this.autoTeachingTimer.Tick += new System.EventHandler(this.autoTeachingTimer_Tick);
            // 
            // SettingPage_Teaching
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1689, 702);
            this.Controls.Add(this.gboxManualTeaching);
            this.Controls.Add(this.gboxAutoTeaching);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SettingPage_Teaching";
            this.Text = "SettingPage_Teaching";
            this.gboxAutoTeaching.ResumeLayout(false);
            this.gboxAutoTeaching.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvShelfData)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAutoTeachingStatus)).EndInit();
            this.gboxManualTeaching.ResumeLayout(false);
            this.gboxManualTeaching.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboxAutoTeaching;
        private System.Windows.Forms.GroupBox gboxManualTeaching;
        private System.Windows.Forms.Label lblTeachingID;
        private System.Windows.Forms.Button btnXZTeaching;
        private System.Windows.Forms.Button btnOneShelfTeaching;
        private System.Windows.Forms.Button btnZUpDownTeaching;
        private System.Windows.Forms.Button btnAAllTeaching;
        private System.Windows.Forms.Button btnTAllTeacing;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTeachZDown;
        private System.Windows.Forms.TextBox txtCurZDown;
        private System.Windows.Forms.TextBox txtTeachZUp;
        private System.Windows.Forms.TextBox txtCurZUp;
        private System.Windows.Forms.TextBox txtTeachTPos;
        private System.Windows.Forms.TextBox txtCurTPos;
        private System.Windows.Forms.TextBox txtTeachAPos;
        private System.Windows.Forms.TextBox txtCurAPos;
        private System.Windows.Forms.TextBox txtTeachZPos;
        private System.Windows.Forms.TextBox txtCurZPos;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTeachXPos;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCurXPos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer saveTimer;
        private System.Windows.Forms.Button btnAutoTeachingStop;
        private System.Windows.Forms.Button btnAutoTeachingStart;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtAutoTeachingTargetZ;
        private System.Windows.Forms.TextBox txtAutoTeachingTargetX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAutoTeachingIDError;
        private System.Windows.Forms.TextBox txtAutoTeachingID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvShelfData;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridView dgvAutoTeachingStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Timer autoTeachingTimer;
        private System.Windows.Forms.Button btnMaintTeaching;
    }
}