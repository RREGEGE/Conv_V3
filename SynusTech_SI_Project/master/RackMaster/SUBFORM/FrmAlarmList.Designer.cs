namespace RackMaster.SUBFORM
{
    partial class FrmAlarmList
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvAlarmList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CMSTest = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testModeMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtAlarmCause = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtAlarmAction = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlarmList)).BeginInit();
            this.CMSTest.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvAlarmList);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(633, 798);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Alarm List";
            // 
            // dgvAlarmList
            // 
            this.dgvAlarmList.AllowUserToAddRows = false;
            this.dgvAlarmList.AllowUserToDeleteRows = false;
            this.dgvAlarmList.AllowUserToResizeColumns = false;
            this.dgvAlarmList.AllowUserToResizeRows = false;
            this.dgvAlarmList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAlarmList.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAlarmList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAlarmList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlarmList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14});
            this.dgvAlarmList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAlarmList.Location = new System.Drawing.Point(3, 25);
            this.dgvAlarmList.Name = "dgvAlarmList";
            this.dgvAlarmList.RowHeadersVisible = false;
            this.dgvAlarmList.RowTemplate.Height = 23;
            this.dgvAlarmList.Size = new System.Drawing.Size(627, 770);
            this.dgvAlarmList.TabIndex = 3;
            this.dgvAlarmList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAlarmList_CellClick);
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "No";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.Width = 58;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn14.HeaderText = "Alarm";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            // 
            // panel1
            // 
            this.panel1.ContextMenuStrip = this.CMSTest;
            this.panel1.Location = new System.Drawing.Point(1586, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(91, 58);
            this.panel1.TabIndex = 1;
            // 
            // CMSTest
            // 
            this.CMSTest.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testModeMenu});
            this.CMSTest.Name = "CMSTest";
            this.CMSTest.Size = new System.Drawing.Size(151, 26);
            // 
            // testModeMenu
            // 
            this.testModeMenu.Name = "testModeMenu";
            this.testModeMenu.Size = new System.Drawing.Size(150, 22);
            this.testModeMenu.Text = "Test Mode On";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtAlarmCause);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(651, 99);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1026, 253);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cause";
            // 
            // txtAlarmCause
            // 
            this.txtAlarmCause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAlarmCause.Location = new System.Drawing.Point(3, 25);
            this.txtAlarmCause.Multiline = true;
            this.txtAlarmCause.Name = "txtAlarmCause";
            this.txtAlarmCause.Size = new System.Drawing.Size(1020, 225);
            this.txtAlarmCause.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtAlarmAction);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(651, 358);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1026, 449);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Action";
            // 
            // txtAlarmAction
            // 
            this.txtAlarmAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAlarmAction.Location = new System.Drawing.Point(3, 25);
            this.txtAlarmAction.Multiline = true;
            this.txtAlarmAction.Name = "txtAlarmAction";
            this.txtAlarmAction.Size = new System.Drawing.Size(1020, 421);
            this.txtAlarmAction.TabIndex = 0;
            // 
            // FrmAlarmList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1689, 822);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmAlarmList";
            this.Text = "FrmAlarmList";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlarmList)).EndInit();
            this.CMSTest.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvAlarmList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip CMSTest;
        private System.Windows.Forms.ToolStripMenuItem testModeMenu;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtAlarmCause;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtAlarmAction;
    }
}