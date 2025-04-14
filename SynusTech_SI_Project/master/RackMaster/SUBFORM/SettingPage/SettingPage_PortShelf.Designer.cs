
namespace RackMaster.SUBFORM.SettingPage {
    partial class SettingPage_PortShelf {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gboxShelfSetting = new System.Windows.Forms.GroupBox();
            this.dgvShelf = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gboxPortSetting = new System.Windows.Forms.GroupBox();
            this.dgvPort = new System.Windows.Forms.DataGridView();
            this.btnPortSave = new System.Windows.Forms.Button();
            this.btnDeletePort = new System.Windows.Forms.Button();
            this.btnAddPort = new System.Windows.Forms.Button();
            this.saveTimer = new System.Windows.Forms.Timer(this.components);
            this.colPortType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPortID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPortRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPortCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDirection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUseSensor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFromUp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFromDown = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToUp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToDown = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colForkPosSensor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsePosSensor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gboxShelfSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShelf)).BeginInit();
            this.gboxPortSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPort)).BeginInit();
            this.SuspendLayout();
            // 
            // gboxShelfSetting
            // 
            this.gboxShelfSetting.Controls.Add(this.dgvShelf);
            this.gboxShelfSetting.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxShelfSetting.Location = new System.Drawing.Point(12, 12);
            this.gboxShelfSetting.Name = "gboxShelfSetting";
            this.gboxShelfSetting.Size = new System.Drawing.Size(1671, 89);
            this.gboxShelfSetting.TabIndex = 10;
            this.gboxShelfSetting.TabStop = false;
            this.gboxShelfSetting.Text = "Shelf Settings";
            // 
            // dgvShelf
            // 
            this.dgvShelf.AllowUserToAddRows = false;
            this.dgvShelf.AllowUserToDeleteRows = false;
            this.dgvShelf.AllowUserToResizeColumns = false;
            this.dgvShelf.AllowUserToResizeRows = false;
            this.dgvShelf.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvShelf.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvShelf.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvShelf.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvShelf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShelf.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvShelf.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvShelf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvShelf.Location = new System.Drawing.Point(3, 25);
            this.dgvShelf.Name = "dgvShelf";
            this.dgvShelf.RowHeadersVisible = false;
            this.dgvShelf.RowTemplate.Height = 23;
            this.dgvShelf.Size = new System.Drawing.Size(1665, 61);
            this.dgvShelf.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Shelf Row Count";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Shelf Column Count";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // gboxPortSetting
            // 
            this.gboxPortSetting.Controls.Add(this.dgvPort);
            this.gboxPortSetting.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxPortSetting.Location = new System.Drawing.Point(12, 107);
            this.gboxPortSetting.Name = "gboxPortSetting";
            this.gboxPortSetting.Size = new System.Drawing.Size(1665, 479);
            this.gboxPortSetting.TabIndex = 11;
            this.gboxPortSetting.TabStop = false;
            this.gboxPortSetting.Text = "Port Settings";
            // 
            // dgvPort
            // 
            this.dgvPort.AllowUserToAddRows = false;
            this.dgvPort.AllowUserToDeleteRows = false;
            this.dgvPort.AllowUserToResizeColumns = false;
            this.dgvPort.AllowUserToResizeRows = false;
            this.dgvPort.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPort.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPort.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPort.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvPort.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPort.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPortType,
            this.colPortID,
            this.colPortRow,
            this.colPortCol,
            this.colDirection,
            this.colUseSensor,
            this.colFromUp,
            this.colFromDown,
            this.colToUp,
            this.colToDown,
            this.colForkPosSensor,
            this.colUsePosSensor});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPort.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPort.Location = new System.Drawing.Point(3, 25);
            this.dgvPort.Name = "dgvPort";
            this.dgvPort.RowHeadersVisible = false;
            this.dgvPort.RowTemplate.Height = 23;
            this.dgvPort.Size = new System.Drawing.Size(1659, 451);
            this.dgvPort.TabIndex = 0;
            // 
            // btnPortSave
            // 
            this.btnPortSave.BackColor = System.Drawing.Color.White;
            this.btnPortSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPortSave.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPortSave.Location = new System.Drawing.Point(1426, 589);
            this.btnPortSave.Name = "btnPortSave";
            this.btnPortSave.Size = new System.Drawing.Size(248, 103);
            this.btnPortSave.TabIndex = 9;
            this.btnPortSave.Tag = "Save";
            this.btnPortSave.Text = "Save";
            this.btnPortSave.UseVisualStyleBackColor = false;
            // 
            // btnDeletePort
            // 
            this.btnDeletePort.BackColor = System.Drawing.Color.White;
            this.btnDeletePort.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDeletePort.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeletePort.Location = new System.Drawing.Point(266, 589);
            this.btnDeletePort.Name = "btnDeletePort";
            this.btnDeletePort.Size = new System.Drawing.Size(248, 103);
            this.btnDeletePort.TabIndex = 12;
            this.btnDeletePort.Text = "Delete Port";
            this.btnDeletePort.UseVisualStyleBackColor = false;
            this.btnDeletePort.Click += new System.EventHandler(this.btnDeletePort_Click);
            // 
            // btnAddPort
            // 
            this.btnAddPort.BackColor = System.Drawing.Color.White;
            this.btnAddPort.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddPort.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPort.Location = new System.Drawing.Point(12, 589);
            this.btnAddPort.Name = "btnAddPort";
            this.btnAddPort.Size = new System.Drawing.Size(248, 103);
            this.btnAddPort.TabIndex = 13;
            this.btnAddPort.Text = "Add Port";
            this.btnAddPort.UseVisualStyleBackColor = false;
            this.btnAddPort.Click += new System.EventHandler(this.btnAddPort_Click);
            // 
            // saveTimer
            // 
            this.saveTimer.Tick += new System.EventHandler(this.saveTimer_Tick);
            // 
            // colPortType
            // 
            this.colPortType.HeaderText = "Port Type";
            this.colPortType.Name = "colPortType";
            // 
            // colPortID
            // 
            this.colPortID.HeaderText = "Port ID";
            this.colPortID.Name = "colPortID";
            // 
            // colPortRow
            // 
            this.colPortRow.HeaderText = "Row Index";
            this.colPortRow.Name = "colPortRow";
            // 
            // colPortCol
            // 
            this.colPortCol.HeaderText = "Colmun Index";
            this.colPortCol.Name = "colPortCol";
            // 
            // colDirection
            // 
            this.colDirection.HeaderText = "Direction(HP)";
            this.colDirection.Name = "colDirection";
            // 
            // colUseSensor
            // 
            this.colUseSensor.HeaderText = "Use Sensor";
            this.colUseSensor.Name = "colUseSensor";
            // 
            // colFromUp
            // 
            this.colFromUp.HeaderText = "From Up Position(mm)";
            this.colFromUp.Name = "colFromUp";
            // 
            // colFromDown
            // 
            this.colFromDown.HeaderText = "From Down Position(mm)";
            this.colFromDown.Name = "colFromDown";
            // 
            // colToUp
            // 
            this.colToUp.HeaderText = "To Up Position(mm)";
            this.colToUp.Name = "colToUp";
            // 
            // colToDown
            // 
            this.colToDown.HeaderText = "To Down Position(mm)";
            this.colToDown.Name = "colToDown";
            // 
            // colForkPosSensor
            // 
            this.colForkPosSensor.HeaderText = "Fork Position Sensor Type";
            this.colForkPosSensor.Name = "colForkPosSensor";
            // 
            // colUsePosSensor
            // 
            this.colUsePosSensor.HeaderText = "Use Position Sensor";
            this.colUsePosSensor.Name = "colUsePosSensor";
            // 
            // SettingPage_PortShelf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1689, 702);
            this.Controls.Add(this.btnAddPort);
            this.Controls.Add(this.btnDeletePort);
            this.Controls.Add(this.btnPortSave);
            this.Controls.Add(this.gboxPortSetting);
            this.Controls.Add(this.gboxShelfSetting);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SettingPage_PortShelf";
            this.Text = "SettingPage_PortShelf";
            this.gboxShelfSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvShelf)).EndInit();
            this.gboxPortSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboxShelfSetting;
        private System.Windows.Forms.DataGridView dgvShelf;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.GroupBox gboxPortSetting;
        private System.Windows.Forms.DataGridView dgvPort;
        private System.Windows.Forms.Button btnPortSave;
        private System.Windows.Forms.Button btnDeletePort;
        private System.Windows.Forms.Button btnAddPort;
        private System.Windows.Forms.Timer saveTimer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPortType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPortID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPortRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPortCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDirection;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUseSensor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFromUp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFromDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToUp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn colForkPosSensor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsePosSensor;
    }
}