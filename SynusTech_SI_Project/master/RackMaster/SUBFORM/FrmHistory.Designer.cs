
namespace RackMaster.SUBFORM {
    partial class FrmHistory {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gboxFilterLogLevel = new System.Windows.Forms.GroupBox();
            this.btnLevelWarning = new System.Windows.Forms.Button();
            this.btnLevelError = new System.Windows.Forms.Button();
            this.btnLevelNormal = new System.Windows.Forms.Button();
            this.gboxFilterLogType = new System.Windows.Forms.GroupBox();
            this.btnTypeUtility = new System.Windows.Forms.Button();
            this.btnTypeUI = new System.Windows.Forms.Button();
            this.btnTypeCIM = new System.Windows.Forms.Button();
            this.btnTypeTCP = new System.Windows.Forms.Button();
            this.btnTypeWMX = new System.Windows.Forms.Button();
            this.btnTypeRM = new System.Windows.Forms.Button();
            this.gboxLogHistory = new System.Windows.Forms.GroupBox();
            this.dgvLogHistory = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gboxLogDate = new System.Windows.Forms.GroupBox();
            this.btnLogLoad = new System.Windows.Forms.Button();
            this.cboxDay = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboxMonth = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboxYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.gboxFilterLogLevel.SuspendLayout();
            this.gboxFilterLogType.SuspendLayout();
            this.gboxLogHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogHistory)).BeginInit();
            this.gboxLogDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboxFilterLogLevel
            // 
            this.gboxFilterLogLevel.Controls.Add(this.btnLevelWarning);
            this.gboxFilterLogLevel.Controls.Add(this.btnLevelError);
            this.gboxFilterLogLevel.Controls.Add(this.btnLevelNormal);
            this.gboxFilterLogLevel.Location = new System.Drawing.Point(12, 12);
            this.gboxFilterLogLevel.Name = "gboxFilterLogLevel";
            this.gboxFilterLogLevel.Size = new System.Drawing.Size(398, 171);
            this.gboxFilterLogLevel.TabIndex = 0;
            this.gboxFilterLogLevel.TabStop = false;
            this.gboxFilterLogLevel.Text = "Filter(Log Level)";
            // 
            // btnLevelWarning
            // 
            this.btnLevelWarning.BackColor = System.Drawing.Color.White;
            this.btnLevelWarning.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLevelWarning.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLevelWarning.Location = new System.Drawing.Point(6, 91);
            this.btnLevelWarning.Name = "btnLevelWarning";
            this.btnLevelWarning.Size = new System.Drawing.Size(190, 60);
            this.btnLevelWarning.TabIndex = 3;
            this.btnLevelWarning.Text = "Warning";
            this.btnLevelWarning.UseVisualStyleBackColor = false;
            // 
            // btnLevelError
            // 
            this.btnLevelError.BackColor = System.Drawing.Color.White;
            this.btnLevelError.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLevelError.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLevelError.Location = new System.Drawing.Point(202, 28);
            this.btnLevelError.Name = "btnLevelError";
            this.btnLevelError.Size = new System.Drawing.Size(190, 60);
            this.btnLevelError.TabIndex = 1;
            this.btnLevelError.Text = "Error";
            this.btnLevelError.UseVisualStyleBackColor = false;
            // 
            // btnLevelNormal
            // 
            this.btnLevelNormal.BackColor = System.Drawing.Color.White;
            this.btnLevelNormal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLevelNormal.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLevelNormal.Location = new System.Drawing.Point(6, 28);
            this.btnLevelNormal.Name = "btnLevelNormal";
            this.btnLevelNormal.Size = new System.Drawing.Size(190, 60);
            this.btnLevelNormal.TabIndex = 0;
            this.btnLevelNormal.Text = "Normal";
            this.btnLevelNormal.UseVisualStyleBackColor = false;
            // 
            // gboxFilterLogType
            // 
            this.gboxFilterLogType.Controls.Add(this.btnTypeUtility);
            this.gboxFilterLogType.Controls.Add(this.btnTypeUI);
            this.gboxFilterLogType.Controls.Add(this.btnTypeCIM);
            this.gboxFilterLogType.Controls.Add(this.btnTypeTCP);
            this.gboxFilterLogType.Controls.Add(this.btnTypeWMX);
            this.gboxFilterLogType.Controls.Add(this.btnTypeRM);
            this.gboxFilterLogType.Location = new System.Drawing.Point(410, 12);
            this.gboxFilterLogType.Name = "gboxFilterLogType";
            this.gboxFilterLogType.Size = new System.Drawing.Size(599, 171);
            this.gboxFilterLogType.TabIndex = 4;
            this.gboxFilterLogType.TabStop = false;
            this.gboxFilterLogType.Text = "Filter(Log Type)";
            // 
            // btnTypeUtility
            // 
            this.btnTypeUtility.BackColor = System.Drawing.Color.White;
            this.btnTypeUtility.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTypeUtility.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTypeUtility.Location = new System.Drawing.Point(398, 94);
            this.btnTypeUtility.Name = "btnTypeUtility";
            this.btnTypeUtility.Size = new System.Drawing.Size(190, 60);
            this.btnTypeUtility.TabIndex = 5;
            this.btnTypeUtility.Text = "Utility";
            this.btnTypeUtility.UseVisualStyleBackColor = false;
            // 
            // btnTypeUI
            // 
            this.btnTypeUI.BackColor = System.Drawing.Color.White;
            this.btnTypeUI.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTypeUI.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTypeUI.Location = new System.Drawing.Point(6, 94);
            this.btnTypeUI.Name = "btnTypeUI";
            this.btnTypeUI.Size = new System.Drawing.Size(190, 60);
            this.btnTypeUI.TabIndex = 4;
            this.btnTypeUI.Text = "UI Control";
            this.btnTypeUI.UseVisualStyleBackColor = false;
            // 
            // btnTypeCIM
            // 
            this.btnTypeCIM.BackColor = System.Drawing.Color.White;
            this.btnTypeCIM.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTypeCIM.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTypeCIM.Location = new System.Drawing.Point(202, 94);
            this.btnTypeCIM.Name = "btnTypeCIM";
            this.btnTypeCIM.Size = new System.Drawing.Size(190, 60);
            this.btnTypeCIM.TabIndex = 3;
            this.btnTypeCIM.Text = "CIM";
            this.btnTypeCIM.UseVisualStyleBackColor = false;
            // 
            // btnTypeTCP
            // 
            this.btnTypeTCP.BackColor = System.Drawing.Color.White;
            this.btnTypeTCP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTypeTCP.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTypeTCP.Location = new System.Drawing.Point(202, 28);
            this.btnTypeTCP.Name = "btnTypeTCP";
            this.btnTypeTCP.Size = new System.Drawing.Size(190, 60);
            this.btnTypeTCP.TabIndex = 2;
            this.btnTypeTCP.Text = "TCP";
            this.btnTypeTCP.UseVisualStyleBackColor = false;
            // 
            // btnTypeWMX
            // 
            this.btnTypeWMX.BackColor = System.Drawing.Color.White;
            this.btnTypeWMX.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTypeWMX.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTypeWMX.Location = new System.Drawing.Point(398, 28);
            this.btnTypeWMX.Name = "btnTypeWMX";
            this.btnTypeWMX.Size = new System.Drawing.Size(190, 60);
            this.btnTypeWMX.TabIndex = 1;
            this.btnTypeWMX.Text = "WMX";
            this.btnTypeWMX.UseVisualStyleBackColor = false;
            // 
            // btnTypeRM
            // 
            this.btnTypeRM.BackColor = System.Drawing.Color.White;
            this.btnTypeRM.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTypeRM.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTypeRM.Location = new System.Drawing.Point(6, 28);
            this.btnTypeRM.Name = "btnTypeRM";
            this.btnTypeRM.Size = new System.Drawing.Size(190, 60);
            this.btnTypeRM.TabIndex = 0;
            this.btnTypeRM.Text = "Rack Master";
            this.btnTypeRM.UseVisualStyleBackColor = false;
            // 
            // gboxLogHistory
            // 
            this.gboxLogHistory.Controls.Add(this.dgvLogHistory);
            this.gboxLogHistory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gboxLogHistory.Location = new System.Drawing.Point(0, 189);
            this.gboxLogHistory.Name = "gboxLogHistory";
            this.gboxLogHistory.Size = new System.Drawing.Size(1689, 652);
            this.gboxLogHistory.TabIndex = 5;
            this.gboxLogHistory.TabStop = false;
            this.gboxLogHistory.Text = "Log History";
            // 
            // dgvLogHistory
            // 
            this.dgvLogHistory.AllowUserToAddRows = false;
            this.dgvLogHistory.AllowUserToDeleteRows = false;
            this.dgvLogHistory.AllowUserToResizeColumns = false;
            this.dgvLogHistory.AllowUserToResizeRows = false;
            this.dgvLogHistory.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLogHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvLogHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTime,
            this.colLevel,
            this.colType,
            this.colMessage});
            this.dgvLogHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLogHistory.Location = new System.Drawing.Point(3, 25);
            this.dgvLogHistory.Name = "dgvLogHistory";
            this.dgvLogHistory.ReadOnly = true;
            this.dgvLogHistory.RowHeadersVisible = false;
            this.dgvLogHistory.RowTemplate.Height = 23;
            this.dgvLogHistory.Size = new System.Drawing.Size(1683, 624);
            this.dgvLogHistory.TabIndex = 0;
            // 
            // colTime
            // 
            this.colTime.HeaderText = "Time";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.Width = 300;
            // 
            // colLevel
            // 
            this.colLevel.HeaderText = "Level";
            this.colLevel.Name = "colLevel";
            this.colLevel.ReadOnly = true;
            this.colLevel.Width = 200;
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Width = 200;
            // 
            // colMessage
            // 
            this.colMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMessage.HeaderText = "Message";
            this.colMessage.Name = "colMessage";
            this.colMessage.ReadOnly = true;
            // 
            // gboxLogDate
            // 
            this.gboxLogDate.Controls.Add(this.btnLogLoad);
            this.gboxLogDate.Controls.Add(this.cboxDay);
            this.gboxLogDate.Controls.Add(this.label3);
            this.gboxLogDate.Controls.Add(this.cboxMonth);
            this.gboxLogDate.Controls.Add(this.label2);
            this.gboxLogDate.Controls.Add(this.cboxYear);
            this.gboxLogDate.Controls.Add(this.label1);
            this.gboxLogDate.Location = new System.Drawing.Point(1015, 12);
            this.gboxLogDate.Name = "gboxLogDate";
            this.gboxLogDate.Size = new System.Drawing.Size(441, 171);
            this.gboxLogDate.TabIndex = 6;
            this.gboxLogDate.TabStop = false;
            this.gboxLogDate.Text = "Date";
            // 
            // btnLogLoad
            // 
            this.btnLogLoad.BackColor = System.Drawing.Color.White;
            this.btnLogLoad.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLogLoad.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogLoad.Location = new System.Drawing.Point(248, 31);
            this.btnLogLoad.Name = "btnLogLoad";
            this.btnLogLoad.Size = new System.Drawing.Size(182, 132);
            this.btnLogLoad.TabIndex = 6;
            this.btnLogLoad.Text = "Load";
            this.btnLogLoad.UseVisualStyleBackColor = false;
            this.btnLogLoad.Click += new System.EventHandler(this.btnLogLoad_Click);
            // 
            // cboxDay
            // 
            this.cboxDay.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxDay.FormattingEnabled = true;
            this.cboxDay.Location = new System.Drawing.Point(117, 123);
            this.cboxDay.Name = "cboxDay";
            this.cboxDay.Size = new System.Drawing.Size(125, 40);
            this.cboxDay.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(38, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 32);
            this.label3.TabIndex = 4;
            this.label3.Text = "Day :";
            // 
            // cboxMonth
            // 
            this.cboxMonth.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxMonth.FormattingEnabled = true;
            this.cboxMonth.Location = new System.Drawing.Point(117, 77);
            this.cboxMonth.Name = "cboxMonth";
            this.cboxMonth.Size = new System.Drawing.Size(125, 40);
            this.cboxMonth.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 32);
            this.label2.TabIndex = 2;
            this.label2.Text = "Month :";
            // 
            // cboxYear
            // 
            this.cboxYear.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxYear.FormattingEnabled = true;
            this.cboxYear.Location = new System.Drawing.Point(117, 31);
            this.cboxYear.Name = "cboxYear";
            this.cboxYear.Size = new System.Drawing.Size(125, 40);
            this.cboxYear.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Year :";
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.BackColor = System.Drawing.Color.White;
            this.btnPrevPage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPrevPage.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevPage.Location = new System.Drawing.Point(1462, 21);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(215, 79);
            this.btnPrevPage.TabIndex = 7;
            this.btnPrevPage.Text = "Prev Page";
            this.btnPrevPage.UseVisualStyleBackColor = false;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.BackColor = System.Drawing.Color.White;
            this.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNextPage.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextPage.Location = new System.Drawing.Point(1462, 106);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(215, 79);
            this.btnNextPage.TabIndex = 8;
            this.btnNextPage.Text = "Next Page";
            this.btnNextPage.UseVisualStyleBackColor = false;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // FrmHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1689, 841);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnPrevPage);
            this.Controls.Add(this.gboxLogDate);
            this.Controls.Add(this.gboxLogHistory);
            this.Controls.Add(this.gboxFilterLogType);
            this.Controls.Add(this.gboxFilterLogLevel);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmHistory";
            this.Text = "FrmErrorList";
            this.gboxFilterLogLevel.ResumeLayout(false);
            this.gboxFilterLogType.ResumeLayout(false);
            this.gboxLogHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogHistory)).EndInit();
            this.gboxLogDate.ResumeLayout(false);
            this.gboxLogDate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboxFilterLogLevel;
        private System.Windows.Forms.Button btnLevelNormal;
        private System.Windows.Forms.Button btnLevelWarning;
        private System.Windows.Forms.Button btnLevelError;
        private System.Windows.Forms.GroupBox gboxFilterLogType;
        private System.Windows.Forms.Button btnTypeCIM;
        private System.Windows.Forms.Button btnTypeTCP;
        private System.Windows.Forms.Button btnTypeWMX;
        private System.Windows.Forms.Button btnTypeRM;
        private System.Windows.Forms.GroupBox gboxLogHistory;
        private System.Windows.Forms.Button btnTypeUtility;
        private System.Windows.Forms.Button btnTypeUI;
        private System.Windows.Forms.GroupBox gboxLogDate;
        private System.Windows.Forms.Button btnLogLoad;
        private System.Windows.Forms.ComboBox cboxDay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboxMonth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboxYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvLogHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnNextPage;
    }
}