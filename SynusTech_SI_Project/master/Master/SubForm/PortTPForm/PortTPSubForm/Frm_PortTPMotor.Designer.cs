
namespace Master.SubForm.PortTPForm.PortTPSubForm
{
    partial class Frm_PortTPMotor
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
            this.UIUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_PortControl = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_PortTotalStatus = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_RackMasterManualPIO = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_RM_Error = new System.Windows.Forms.Button();
            this.btn_RM_Ready = new System.Windows.Forms.Button();
            this.btn_RM_Unload_REQ = new System.Windows.Forms.Button();
            this.btn_RM_Load_REQ = new System.Windows.Forms.Button();
            this.groupBox_RFID = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_Tag = new System.Windows.Forms.Label();
            this.btn_RFIDRead = new System.Windows.Forms.Button();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox_RackMasterManualPIO.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.groupBox_RFID.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // UIUpdateTimer
            // 
            this.UIUpdateTimer.Interval = 250;
            this.UIUpdateTimer.Tick += new System.EventHandler(this.UIUpdateTimer_Tick);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel3.Controls.Add(this.pnl_PortControl, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1724, 673);
            this.tableLayoutPanel3.TabIndex = 7;
            // 
            // pnl_PortControl
            // 
            this.pnl_PortControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_PortControl.Location = new System.Drawing.Point(1120, 0);
            this.pnl_PortControl.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_PortControl.Name = "pnl_PortControl";
            this.pnl_PortControl.Size = new System.Drawing.Size(604, 673);
            this.pnl_PortControl.TabIndex = 43;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.pnl_PortTotalStatus, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1120, 673);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // pnl_PortTotalStatus
            // 
            this.pnl_PortTotalStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_PortTotalStatus.Location = new System.Drawing.Point(0, 0);
            this.pnl_PortTotalStatus.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_PortTotalStatus.Name = "pnl_PortTotalStatus";
            this.pnl_PortTotalStatus.Size = new System.Drawing.Size(1120, 572);
            this.pnl_PortTotalStatus.TabIndex = 42;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox_RackMasterManualPIO, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox_RFID, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 575);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1114, 95);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // groupBox_RackMasterManualPIO
            // 
            this.groupBox_RackMasterManualPIO.Controls.Add(this.tableLayoutPanel10);
            this.groupBox_RackMasterManualPIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_RackMasterManualPIO.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_RackMasterManualPIO.Location = new System.Drawing.Point(560, 3);
            this.groupBox_RackMasterManualPIO.Name = "groupBox_RackMasterManualPIO";
            this.groupBox_RackMasterManualPIO.Size = new System.Drawing.Size(551, 89);
            this.groupBox_RackMasterManualPIO.TabIndex = 8;
            this.groupBox_RackMasterManualPIO.TabStop = false;
            this.groupBox_RackMasterManualPIO.Text = "RackMaster PIO";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 4;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel10.Controls.Add(this.btn_RM_Error, 3, 0);
            this.tableLayoutPanel10.Controls.Add(this.btn_RM_Ready, 2, 0);
            this.tableLayoutPanel10.Controls.Add(this.btn_RM_Unload_REQ, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.btn_RM_Load_REQ, 0, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(545, 61);
            this.tableLayoutPanel10.TabIndex = 0;
            // 
            // btn_RM_Error
            // 
            this.btn_RM_Error.BackColor = System.Drawing.Color.White;
            this.btn_RM_Error.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_RM_Error.FlatAppearance.BorderSize = 0;
            this.btn_RM_Error.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_RM_Error.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_RM_Error.Location = new System.Drawing.Point(409, 1);
            this.btn_RM_Error.Margin = new System.Windows.Forms.Padding(1);
            this.btn_RM_Error.Name = "btn_RM_Error";
            this.btn_RM_Error.Size = new System.Drawing.Size(135, 59);
            this.btn_RM_Error.TabIndex = 8;
            this.btn_RM_Error.Text = "Port-ERROR";
            this.btn_RM_Error.UseVisualStyleBackColor = false;
            this.btn_RM_Error.Click += new System.EventHandler(this.btn_RM_Error_Click);
            // 
            // btn_RM_Ready
            // 
            this.btn_RM_Ready.BackColor = System.Drawing.Color.White;
            this.btn_RM_Ready.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_RM_Ready.FlatAppearance.BorderSize = 0;
            this.btn_RM_Ready.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_RM_Ready.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_RM_Ready.Location = new System.Drawing.Point(273, 1);
            this.btn_RM_Ready.Margin = new System.Windows.Forms.Padding(1);
            this.btn_RM_Ready.Name = "btn_RM_Ready";
            this.btn_RM_Ready.Size = new System.Drawing.Size(134, 59);
            this.btn_RM_Ready.TabIndex = 7;
            this.btn_RM_Ready.Text = "Ready";
            this.btn_RM_Ready.UseVisualStyleBackColor = false;
            this.btn_RM_Ready.Click += new System.EventHandler(this.btn_RM_Complete_Click);
            // 
            // btn_RM_Unload_REQ
            // 
            this.btn_RM_Unload_REQ.BackColor = System.Drawing.Color.White;
            this.btn_RM_Unload_REQ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_RM_Unload_REQ.FlatAppearance.BorderSize = 0;
            this.btn_RM_Unload_REQ.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_RM_Unload_REQ.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_RM_Unload_REQ.Location = new System.Drawing.Point(137, 1);
            this.btn_RM_Unload_REQ.Margin = new System.Windows.Forms.Padding(1);
            this.btn_RM_Unload_REQ.Name = "btn_RM_Unload_REQ";
            this.btn_RM_Unload_REQ.Size = new System.Drawing.Size(134, 59);
            this.btn_RM_Unload_REQ.TabIndex = 6;
            this.btn_RM_Unload_REQ.Text = "Unload-REQ";
            this.btn_RM_Unload_REQ.UseVisualStyleBackColor = false;
            this.btn_RM_Unload_REQ.Click += new System.EventHandler(this.btn_RM_BUSY_Click);
            // 
            // btn_RM_Load_REQ
            // 
            this.btn_RM_Load_REQ.BackColor = System.Drawing.Color.White;
            this.btn_RM_Load_REQ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_RM_Load_REQ.FlatAppearance.BorderSize = 0;
            this.btn_RM_Load_REQ.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_RM_Load_REQ.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_RM_Load_REQ.Location = new System.Drawing.Point(1, 1);
            this.btn_RM_Load_REQ.Margin = new System.Windows.Forms.Padding(1);
            this.btn_RM_Load_REQ.Name = "btn_RM_Load_REQ";
            this.btn_RM_Load_REQ.Size = new System.Drawing.Size(134, 59);
            this.btn_RM_Load_REQ.TabIndex = 5;
            this.btn_RM_Load_REQ.Text = "Load-REQ";
            this.btn_RM_Load_REQ.UseVisualStyleBackColor = false;
            this.btn_RM_Load_REQ.Click += new System.EventHandler(this.btn_RM_TR_REQ_Click);
            // 
            // groupBox_RFID
            // 
            this.groupBox_RFID.Controls.Add(this.tableLayoutPanel5);
            this.groupBox_RFID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_RFID.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_RFID.Location = new System.Drawing.Point(3, 3);
            this.groupBox_RFID.Name = "groupBox_RFID";
            this.groupBox_RFID.Size = new System.Drawing.Size(551, 89);
            this.groupBox_RFID.TabIndex = 11;
            this.groupBox_RFID.TabStop = false;
            this.groupBox_RFID.Text = "RFID";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.lbl_Tag, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btn_RFIDRead, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(545, 61);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // lbl_Tag
            // 
            this.lbl_Tag.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Tag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Tag.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_Tag.Location = new System.Drawing.Point(139, 3);
            this.lbl_Tag.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_Tag.Name = "lbl_Tag";
            this.lbl_Tag.Size = new System.Drawing.Size(403, 55);
            this.lbl_Tag.TabIndex = 7;
            this.lbl_Tag.Text = "Tag :";
            this.lbl_Tag.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_RFIDRead
            // 
            this.btn_RFIDRead.BackColor = System.Drawing.Color.White;
            this.btn_RFIDRead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_RFIDRead.FlatAppearance.BorderSize = 0;
            this.btn_RFIDRead.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_RFIDRead.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_RFIDRead.Location = new System.Drawing.Point(1, 1);
            this.btn_RFIDRead.Margin = new System.Windows.Forms.Padding(1);
            this.btn_RFIDRead.Name = "btn_RFIDRead";
            this.btn_RFIDRead.Size = new System.Drawing.Size(134, 59);
            this.btn_RFIDRead.TabIndex = 5;
            this.btn_RFIDRead.Text = "Read";
            this.btn_RFIDRead.UseVisualStyleBackColor = false;
            this.btn_RFIDRead.Click += new System.EventHandler(this.btn_RFIDRead_Click);
            // 
            // Frm_PortTPMotor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1724, 673);
            this.Controls.Add(this.tableLayoutPanel3);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_PortTPMotor";
            this.Text = "Frm_PortTPMain";
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox_RackMasterManualPIO.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.groupBox_RFID.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer UIUpdateTimer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox_RackMasterManualPIO;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Button btn_RM_Error;
        private System.Windows.Forms.Button btn_RM_Ready;
        private System.Windows.Forms.Button btn_RM_Unload_REQ;
        private System.Windows.Forms.Button btn_RM_Load_REQ;
        private System.Windows.Forms.GroupBox groupBox_RFID;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btn_RFIDRead;
        private System.Windows.Forms.Label lbl_Tag;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnl_PortTotalStatus;
        private System.Windows.Forms.Panel pnl_PortControl;
    }
}