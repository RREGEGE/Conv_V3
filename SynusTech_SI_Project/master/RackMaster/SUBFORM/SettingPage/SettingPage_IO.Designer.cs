
namespace RackMaster.SUBFORM.SettingPage {
    partial class SettingPage_IO {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvOutputList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gboxInputSetting = new System.Windows.Forms.GroupBox();
            this.dgvInputList = new System.Windows.Forms.DataGridView();
            this.btnIoSave = new System.Windows.Forms.Button();
            this.saveTimer = new System.Windows.Forms.Timer(this.components);
            this.btnDemoMode = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutputList)).BeginInit();
            this.gboxInputSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvOutputList);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(828, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 687);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output Setting";
            // 
            // dgvOutputList
            // 
            this.dgvOutputList.AllowUserToAddRows = false;
            this.dgvOutputList.AllowUserToDeleteRows = false;
            this.dgvOutputList.AllowUserToResizeColumns = false;
            this.dgvOutputList.AllowUserToResizeRows = false;
            this.dgvOutputList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOutputList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvOutputList.BackgroundColor = System.Drawing.Color.White;
            this.dgvOutputList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOutputList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn17,
            this.dataGridViewTextBoxColumn18,
            this.dataGridViewTextBoxColumn19,
            this.dataGridViewTextBoxColumn20});
            this.dgvOutputList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOutputList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvOutputList.Location = new System.Drawing.Point(3, 25);
            this.dgvOutputList.MultiSelect = false;
            this.dgvOutputList.Name = "dgvOutputList";
            this.dgvOutputList.RowHeadersVisible = false;
            this.dgvOutputList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvOutputList.RowTemplate.Height = 23;
            this.dgvOutputList.Size = new System.Drawing.Size(534, 659);
            this.dgvOutputList.TabIndex = 7;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn17.HeaderText = "List";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.ReadOnly = true;
            this.dataGridViewTextBoxColumn17.Width = 61;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.HeaderText = "Byte";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.HeaderText = "Bit";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            // 
            // dataGridViewTextBoxColumn20
            // 
            this.dataGridViewTextBoxColumn20.HeaderText = "Enabled";
            this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
            // 
            // gboxInputSetting
            // 
            this.gboxInputSetting.Controls.Add(this.dgvInputList);
            this.gboxInputSetting.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxInputSetting.Location = new System.Drawing.Point(12, 5);
            this.gboxInputSetting.Name = "gboxInputSetting";
            this.gboxInputSetting.Size = new System.Drawing.Size(810, 687);
            this.gboxInputSetting.TabIndex = 10;
            this.gboxInputSetting.TabStop = false;
            this.gboxInputSetting.Text = "Input Setting";
            // 
            // dgvInputList
            // 
            this.dgvInputList.AllowUserToAddRows = false;
            this.dgvInputList.AllowUserToDeleteRows = false;
            this.dgvInputList.AllowUserToResizeColumns = false;
            this.dgvInputList.AllowUserToResizeRows = false;
            this.dgvInputList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInputList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvInputList.BackgroundColor = System.Drawing.Color.White;
            this.dgvInputList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInputList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn15,
            this.dataGridViewTextBoxColumn16,
            this.Bit,
            this.Column5,
            this.Column4});
            this.dgvInputList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInputList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvInputList.Location = new System.Drawing.Point(3, 25);
            this.dgvInputList.MultiSelect = false;
            this.dgvInputList.Name = "dgvInputList";
            this.dgvInputList.RowHeadersVisible = false;
            this.dgvInputList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvInputList.RowTemplate.Height = 23;
            this.dgvInputList.Size = new System.Drawing.Size(804, 659);
            this.dgvInputList.TabIndex = 7;
            // 
            // btnIoSave
            // 
            this.btnIoSave.BackColor = System.Drawing.Color.White;
            this.btnIoSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnIoSave.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIoSave.Location = new System.Drawing.Point(1374, 587);
            this.btnIoSave.Name = "btnIoSave";
            this.btnIoSave.Size = new System.Drawing.Size(309, 102);
            this.btnIoSave.TabIndex = 11;
            this.btnIoSave.Tag = "Save";
            this.btnIoSave.Text = "Save";
            this.btnIoSave.UseVisualStyleBackColor = false;
            // 
            // saveTimer
            // 
            this.saveTimer.Tick += new System.EventHandler(this.saveTimer_Tick);
            // 
            // btnDemoMode
            // 
            this.btnDemoMode.BackColor = System.Drawing.Color.White;
            this.btnDemoMode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDemoMode.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDemoMode.Location = new System.Drawing.Point(1374, 479);
            this.btnDemoMode.Name = "btnDemoMode";
            this.btnDemoMode.Size = new System.Drawing.Size(309, 102);
            this.btnDemoMode.TabIndex = 12;
            this.btnDemoMode.Tag = "";
            this.btnDemoMode.Text = "Demo Mode";
            this.btnDemoMode.UseVisualStyleBackColor = false;
            this.btnDemoMode.Click += new System.EventHandler(this.btnDemoMode_Click);
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn15.HeaderText = "List";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            this.dataGridViewTextBoxColumn15.Width = 61;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.HeaderText = "Byte";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            // 
            // Bit
            // 
            this.Bit.HeaderText = "Bit";
            this.Bit.Name = "Bit";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "B Enable";
            this.Column5.Name = "Column5";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Enabled";
            this.Column4.Name = "Column4";
            // 
            // SettingPage_IO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1689, 702);
            this.Controls.Add(this.btnDemoMode);
            this.Controls.Add(this.btnIoSave);
            this.Controls.Add(this.gboxInputSetting);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SettingPage_IO";
            this.Text = "SettingPage_IO";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutputList)).EndInit();
            this.gboxInputSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvOutputList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        private System.Windows.Forms.GroupBox gboxInputSetting;
        private System.Windows.Forms.DataGridView dgvInputList;
        private System.Windows.Forms.Button btnIoSave;
        private System.Windows.Forms.Timer saveTimer;
        private System.Windows.Forms.Button btnDemoMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}