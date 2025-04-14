
namespace Master.SubForm
{
    partial class Frm_LogSet
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
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Apply = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbx_DeleteCycle = new System.Windows.Forms.ComboBox();
            this.cbx_CompressionCycle = new System.Windows.Forms.ComboBox();
            this.lbl_DeleteCycle = new System.Windows.Forms.Label();
            this.lbl_CompressionCycle = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.chx_Error = new System.Windows.Forms.CheckBox();
            this.chx_Warning = new System.Windows.Forms.CheckBox();
            this.chx_Normal = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.chx_CPS = new System.Windows.Forms.CheckBox();
            this.chx_WMX = new System.Windows.Forms.CheckBox();
            this.chx_Master = new System.Windows.Forms.CheckBox();
            this.chx_CIM = new System.Windows.Forms.CheckBox();
            this.chx_RackMaster = new System.Windows.Forms.CheckBox();
            this.chx_Port = new System.Windows.Forms.CheckBox();
            this.chx_Exception = new System.Windows.Forms.CheckBox();
            this.chx_Application = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel8.Controls.Add(this.btn_Apply, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.btn_Close, 2, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 316);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(434, 45);
            this.tableLayoutPanel8.TabIndex = 43;
            // 
            // btn_Apply
            // 
            this.btn_Apply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Apply.FlatAppearance.BorderSize = 0;
            this.btn_Apply.Location = new System.Drawing.Point(235, 1);
            this.btn_Apply.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Apply.Name = "btn_Apply";
            this.btn_Apply.Size = new System.Drawing.Size(98, 43);
            this.btn_Apply.TabIndex = 9;
            this.btn_Apply.TabStop = false;
            this.btn_Apply.Text = "적용";
            this.btn_Apply.UseVisualStyleBackColor = true;
            this.btn_Apply.Click += new System.EventHandler(this.btn_Apply_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Close.FlatAppearance.BorderSize = 0;
            this.btn_Close.Location = new System.Drawing.Point(335, 1);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(98, 43);
            this.btn_Close.TabIndex = 8;
            this.btn_Close.TabStop = false;
            this.btn_Close.Text = "취소";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.cbx_DeleteCycle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbx_CompressionCycle, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_DeleteCycle, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbl_CompressionCycle, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 250);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(428, 62);
            this.tableLayoutPanel1.TabIndex = 44;
            // 
            // cbx_DeleteCycle
            // 
            this.cbx_DeleteCycle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbx_DeleteCycle.FormattingEnabled = true;
            this.cbx_DeleteCycle.Location = new System.Drawing.Point(217, 35);
            this.cbx_DeleteCycle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbx_DeleteCycle.Name = "cbx_DeleteCycle";
            this.cbx_DeleteCycle.Size = new System.Drawing.Size(208, 25);
            this.cbx_DeleteCycle.TabIndex = 47;
            // 
            // cbx_CompressionCycle
            // 
            this.cbx_CompressionCycle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbx_CompressionCycle.FormattingEnabled = true;
            this.cbx_CompressionCycle.Location = new System.Drawing.Point(217, 4);
            this.cbx_CompressionCycle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbx_CompressionCycle.Name = "cbx_CompressionCycle";
            this.cbx_CompressionCycle.Size = new System.Drawing.Size(208, 25);
            this.cbx_CompressionCycle.TabIndex = 46;
            // 
            // lbl_DeleteCycle
            // 
            this.lbl_DeleteCycle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_DeleteCycle.Location = new System.Drawing.Point(3, 31);
            this.lbl_DeleteCycle.Name = "lbl_DeleteCycle";
            this.lbl_DeleteCycle.Size = new System.Drawing.Size(208, 31);
            this.lbl_DeleteCycle.TabIndex = 8;
            this.lbl_DeleteCycle.Text = "Zip Delete Cycle :";
            this.lbl_DeleteCycle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_CompressionCycle
            // 
            this.lbl_CompressionCycle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_CompressionCycle.Location = new System.Drawing.Point(3, 0);
            this.lbl_CompressionCycle.Name = "lbl_CompressionCycle";
            this.lbl_CompressionCycle.Size = new System.Drawing.Size(208, 31);
            this.lbl_CompressionCycle.TabIndex = 7;
            this.lbl_CompressionCycle.Text = "Log Compression Cycle :";
            this.lbl_CompressionCycle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(434, 316);
            this.tableLayoutPanel2.TabIndex = 45;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(428, 240);
            this.tableLayoutPanel3.TabIndex = 45;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel5);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(217, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(208, 234);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Show Level";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.chx_Error, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.chx_Warning, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.chx_Normal, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 21);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 7;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(202, 210);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // chx_Error
            // 
            this.chx_Error.AutoSize = true;
            this.chx_Error.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_Error.Location = new System.Drawing.Point(3, 61);
            this.chx_Error.Name = "chx_Error";
            this.chx_Error.Size = new System.Drawing.Size(196, 23);
            this.chx_Error.TabIndex = 3;
            this.chx_Error.Text = "Error";
            this.chx_Error.UseVisualStyleBackColor = true;
            // 
            // chx_Warning
            // 
            this.chx_Warning.AutoSize = true;
            this.chx_Warning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_Warning.Location = new System.Drawing.Point(3, 32);
            this.chx_Warning.Name = "chx_Warning";
            this.chx_Warning.Size = new System.Drawing.Size(196, 23);
            this.chx_Warning.TabIndex = 2;
            this.chx_Warning.Text = "Warning";
            this.chx_Warning.UseVisualStyleBackColor = true;
            // 
            // chx_Normal
            // 
            this.chx_Normal.AutoSize = true;
            this.chx_Normal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_Normal.Location = new System.Drawing.Point(3, 3);
            this.chx_Normal.Name = "chx_Normal";
            this.chx_Normal.Size = new System.Drawing.Size(196, 23);
            this.chx_Normal.TabIndex = 1;
            this.chx_Normal.Text = "Normal";
            this.chx_Normal.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 234);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Show Type";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.chx_CPS, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.chx_WMX, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.chx_Master, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.chx_CIM, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.chx_RackMaster, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.chx_Port, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.chx_Exception, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.chx_Application, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 21);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 8;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(202, 210);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // chx_CPS
            // 
            this.chx_CPS.AutoSize = true;
            this.chx_CPS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_CPS.Location = new System.Drawing.Point(3, 185);
            this.chx_CPS.Name = "chx_CPS";
            this.chx_CPS.Size = new System.Drawing.Size(196, 22);
            this.chx_CPS.TabIndex = 7;
            this.chx_CPS.Text = "CPS";
            this.chx_CPS.UseVisualStyleBackColor = true;
            // 
            // chx_WMX
            // 
            this.chx_WMX.AutoSize = true;
            this.chx_WMX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_WMX.Location = new System.Drawing.Point(3, 159);
            this.chx_WMX.Name = "chx_WMX";
            this.chx_WMX.Size = new System.Drawing.Size(196, 20);
            this.chx_WMX.TabIndex = 6;
            this.chx_WMX.Text = "WMX";
            this.chx_WMX.UseVisualStyleBackColor = true;
            // 
            // chx_Master
            // 
            this.chx_Master.AutoSize = true;
            this.chx_Master.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_Master.Location = new System.Drawing.Point(3, 133);
            this.chx_Master.Name = "chx_Master";
            this.chx_Master.Size = new System.Drawing.Size(196, 20);
            this.chx_Master.TabIndex = 5;
            this.chx_Master.Text = "Master";
            this.chx_Master.UseVisualStyleBackColor = true;
            // 
            // chx_CIM
            // 
            this.chx_CIM.AutoSize = true;
            this.chx_CIM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_CIM.Location = new System.Drawing.Point(3, 107);
            this.chx_CIM.Name = "chx_CIM";
            this.chx_CIM.Size = new System.Drawing.Size(196, 20);
            this.chx_CIM.TabIndex = 4;
            this.chx_CIM.Text = "CIM";
            this.chx_CIM.UseVisualStyleBackColor = true;
            // 
            // chx_RackMaster
            // 
            this.chx_RackMaster.AutoSize = true;
            this.chx_RackMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_RackMaster.Location = new System.Drawing.Point(3, 81);
            this.chx_RackMaster.Name = "chx_RackMaster";
            this.chx_RackMaster.Size = new System.Drawing.Size(196, 20);
            this.chx_RackMaster.TabIndex = 3;
            this.chx_RackMaster.Text = "RackMaster";
            this.chx_RackMaster.UseVisualStyleBackColor = true;
            // 
            // chx_Port
            // 
            this.chx_Port.AutoSize = true;
            this.chx_Port.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_Port.Location = new System.Drawing.Point(3, 55);
            this.chx_Port.Name = "chx_Port";
            this.chx_Port.Size = new System.Drawing.Size(196, 20);
            this.chx_Port.TabIndex = 2;
            this.chx_Port.Text = "Port";
            this.chx_Port.UseVisualStyleBackColor = true;
            // 
            // chx_Exception
            // 
            this.chx_Exception.AutoSize = true;
            this.chx_Exception.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_Exception.Location = new System.Drawing.Point(3, 29);
            this.chx_Exception.Name = "chx_Exception";
            this.chx_Exception.Size = new System.Drawing.Size(196, 20);
            this.chx_Exception.TabIndex = 1;
            this.chx_Exception.Text = "Exception";
            this.chx_Exception.UseVisualStyleBackColor = true;
            // 
            // chx_Application
            // 
            this.chx_Application.AutoSize = true;
            this.chx_Application.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chx_Application.Location = new System.Drawing.Point(3, 3);
            this.chx_Application.Name = "chx_Application";
            this.chx_Application.Size = new System.Drawing.Size(196, 20);
            this.chx_Application.TabIndex = 0;
            this.chx_Application.Text = "Application";
            this.chx_Application.UseVisualStyleBackColor = true;
            // 
            // Frm_LogSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(434, 361);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel8);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(450, 400);
            this.Name = "Frm_LogSet";
            this.Text = "Log Settings";
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Button btn_Apply;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbl_CompressionCycle;
        private System.Windows.Forms.Label lbl_DeleteCycle;
        private System.Windows.Forms.ComboBox cbx_DeleteCycle;
        private System.Windows.Forms.ComboBox cbx_CompressionCycle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.CheckBox chx_Application;
        private System.Windows.Forms.CheckBox chx_Error;
        private System.Windows.Forms.CheckBox chx_Warning;
        private System.Windows.Forms.CheckBox chx_Normal;
        private System.Windows.Forms.CheckBox chx_WMX;
        private System.Windows.Forms.CheckBox chx_Master;
        private System.Windows.Forms.CheckBox chx_CIM;
        private System.Windows.Forms.CheckBox chx_RackMaster;
        private System.Windows.Forms.CheckBox chx_Port;
        private System.Windows.Forms.CheckBox chx_Exception;
        private System.Windows.Forms.CheckBox chx_CPS;
    }
}