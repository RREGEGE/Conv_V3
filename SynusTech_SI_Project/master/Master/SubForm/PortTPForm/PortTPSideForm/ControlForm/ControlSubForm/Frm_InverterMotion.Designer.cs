
namespace Master.SubForm.PortTPForm.PortTPSubForm.ControlForm
{
    partial class Frm_InverterMotion
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
            this.groupBox_InverterCommand = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_InverterFlag = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.btn_InverterStop = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox_Move = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_HighSpeed_BWD = new System.Windows.Forms.Label();
            this.btn_HighSpeed_FWD = new System.Windows.Forms.Button();
            this.btn_LowSpeed_BWD = new System.Windows.Forms.Button();
            this.btn_HighSpeed_BWD = new System.Windows.Forms.Button();
            this.btn_LowSpeed_FWD = new System.Windows.Forms.Button();
            this.lbl_LowSpeed_FWD = new System.Windows.Forms.Label();
            this.lbl_HighSpeed_FWD = new System.Windows.Forms.Label();
            this.lbl_LowSpeed_BWD = new System.Windows.Forms.Label();
            this.groupBox_InverterCommand.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.groupBox_InverterFlag.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox_Move.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.SuspendLayout();
            // 
            // UIUpdateTimer
            // 
            this.UIUpdateTimer.Interval = 250;
            this.UIUpdateTimer.Tick += new System.EventHandler(this.UIUpdateTimer_Tick);
            // 
            // groupBox_InverterCommand
            // 
            this.groupBox_InverterCommand.Controls.Add(this.panel1);
            this.groupBox_InverterCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_InverterCommand.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_InverterCommand.Location = new System.Drawing.Point(3, 3);
            this.groupBox_InverterCommand.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_InverterCommand.Name = "groupBox_InverterCommand";
            this.groupBox_InverterCommand.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_InverterCommand.Size = new System.Drawing.Size(666, 557);
            this.groupBox_InverterCommand.TabIndex = 8;
            this.groupBox_InverterCommand.TabStop = false;
            this.groupBox_InverterCommand.Text = "Inverter Command";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel14);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(664, 537);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 2;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel14.Controls.Add(this.groupBox_InverterFlag, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(664, 537);
            this.tableLayoutPanel14.TabIndex = 13;
            // 
            // groupBox_InverterFlag
            // 
            this.groupBox_InverterFlag.Controls.Add(this.tableLayoutPanel18);
            this.groupBox_InverterFlag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_InverterFlag.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_InverterFlag.Location = new System.Drawing.Point(1, 1);
            this.groupBox_InverterFlag.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_InverterFlag.Name = "groupBox_InverterFlag";
            this.groupBox_InverterFlag.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_InverterFlag.Size = new System.Drawing.Size(130, 535);
            this.groupBox_InverterFlag.TabIndex = 7;
            this.groupBox_InverterFlag.TabStop = false;
            this.groupBox_InverterFlag.Text = "Flag";
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.ColumnCount = 1;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel18.Controls.Add(this.btn_Reset, 0, 1);
            this.tableLayoutPanel18.Controls.Add(this.btn_InverterStop, 0, 0);
            this.tableLayoutPanel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel18.Location = new System.Drawing.Point(1, 19);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.RowCount = 5;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel18.Size = new System.Drawing.Size(128, 515);
            this.tableLayoutPanel18.TabIndex = 0;
            // 
            // btn_Reset
            // 
            this.btn_Reset.BackColor = System.Drawing.Color.White;
            this.btn_Reset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Reset.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_Reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Reset.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Reset.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Reset.Location = new System.Drawing.Point(1, 104);
            this.btn_Reset.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(126, 101);
            this.btn_Reset.TabIndex = 12;
            this.btn_Reset.Text = "Inverter Reset";
            this.btn_Reset.UseVisualStyleBackColor = false;
            this.btn_Reset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Reset_MouseDown);
            this.btn_Reset.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Reset_MouseUp);
            // 
            // btn_InverterStop
            // 
            this.btn_InverterStop.BackColor = System.Drawing.Color.White;
            this.btn_InverterStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_InverterStop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_InverterStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_InverterStop.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_InverterStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_InverterStop.Location = new System.Drawing.Point(1, 1);
            this.btn_InverterStop.Margin = new System.Windows.Forms.Padding(1);
            this.btn_InverterStop.Name = "btn_InverterStop";
            this.btn_InverterStop.Size = new System.Drawing.Size(126, 101);
            this.btn_InverterStop.TabIndex = 11;
            this.btn_InverterStop.Text = "Inverter Stop";
            this.btn_InverterStop.UseVisualStyleBackColor = false;
            this.btn_InverterStop.Click += new System.EventHandler(this.btn_InverterStop_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox_Move);
            this.panel2.Location = new System.Drawing.Point(135, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(522, 527);
            this.panel2.TabIndex = 2;
            // 
            // groupBox_Move
            // 
            this.groupBox_Move.Controls.Add(this.tableLayoutPanel15);
            this.groupBox_Move.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Move.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Move.Location = new System.Drawing.Point(0, 0);
            this.groupBox_Move.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_Move.Name = "groupBox_Move";
            this.groupBox_Move.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_Move.Size = new System.Drawing.Size(522, 100);
            this.groupBox_Move.TabIndex = 12;
            this.groupBox_Move.TabStop = false;
            this.groupBox_Move.Text = "Move";
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 4;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.Controls.Add(this.lbl_HighSpeed_BWD, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.btn_HighSpeed_FWD, 3, 1);
            this.tableLayoutPanel15.Controls.Add(this.btn_LowSpeed_BWD, 1, 1);
            this.tableLayoutPanel15.Controls.Add(this.btn_HighSpeed_BWD, 0, 1);
            this.tableLayoutPanel15.Controls.Add(this.btn_LowSpeed_FWD, 2, 1);
            this.tableLayoutPanel15.Controls.Add(this.lbl_LowSpeed_FWD, 2, 0);
            this.tableLayoutPanel15.Controls.Add(this.lbl_HighSpeed_FWD, 3, 0);
            this.tableLayoutPanel15.Controls.Add(this.lbl_LowSpeed_BWD, 1, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(1, 19);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 2;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(520, 80);
            this.tableLayoutPanel15.TabIndex = 0;
            // 
            // lbl_HighSpeed_BWD
            // 
            this.lbl_HighSpeed_BWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_HighSpeed_BWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_HighSpeed_BWD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_HighSpeed_BWD.Location = new System.Drawing.Point(3, 3);
            this.lbl_HighSpeed_BWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_HighSpeed_BWD.Name = "lbl_HighSpeed_BWD";
            this.lbl_HighSpeed_BWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_HighSpeed_BWD.Size = new System.Drawing.Size(124, 24);
            this.lbl_HighSpeed_BWD.TabIndex = 26;
            this.lbl_HighSpeed_BWD.Text = "High Speed BWD";
            this.lbl_HighSpeed_BWD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btn_HighSpeed_FWD
            // 
            this.btn_HighSpeed_FWD.BackColor = System.Drawing.Color.White;
            this.btn_HighSpeed_FWD.BackgroundImage = global::Master.Properties.Resources.icons8_double_right_96;
            this.btn_HighSpeed_FWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_HighSpeed_FWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_HighSpeed_FWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_HighSpeed_FWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_HighSpeed_FWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_HighSpeed_FWD.Location = new System.Drawing.Point(391, 31);
            this.btn_HighSpeed_FWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_HighSpeed_FWD.Name = "btn_HighSpeed_FWD";
            this.btn_HighSpeed_FWD.Size = new System.Drawing.Size(128, 48);
            this.btn_HighSpeed_FWD.TabIndex = 25;
            this.btn_HighSpeed_FWD.UseVisualStyleBackColor = false;
            this.btn_HighSpeed_FWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_InverterMotion_MouseDown);
            this.btn_HighSpeed_FWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_HighSpeed_FWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_InverterMotion_MouseUp);
            // 
            // btn_LowSpeed_BWD
            // 
            this.btn_LowSpeed_BWD.BackColor = System.Drawing.Color.White;
            this.btn_LowSpeed_BWD.BackgroundImage = global::Master.Properties.Resources.icons8_back_96;
            this.btn_LowSpeed_BWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_LowSpeed_BWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_LowSpeed_BWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_LowSpeed_BWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_LowSpeed_BWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LowSpeed_BWD.Location = new System.Drawing.Point(131, 31);
            this.btn_LowSpeed_BWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_LowSpeed_BWD.Name = "btn_LowSpeed_BWD";
            this.btn_LowSpeed_BWD.Size = new System.Drawing.Size(128, 48);
            this.btn_LowSpeed_BWD.TabIndex = 23;
            this.btn_LowSpeed_BWD.UseVisualStyleBackColor = false;
            this.btn_LowSpeed_BWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_InverterMotion_MouseDown);
            this.btn_LowSpeed_BWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_LowSpeed_BWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_InverterMotion_MouseUp);
            // 
            // btn_HighSpeed_BWD
            // 
            this.btn_HighSpeed_BWD.BackColor = System.Drawing.Color.White;
            this.btn_HighSpeed_BWD.BackgroundImage = global::Master.Properties.Resources.icons8_double_left_96;
            this.btn_HighSpeed_BWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_HighSpeed_BWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_HighSpeed_BWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_HighSpeed_BWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_HighSpeed_BWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_HighSpeed_BWD.Location = new System.Drawing.Point(1, 31);
            this.btn_HighSpeed_BWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_HighSpeed_BWD.Name = "btn_HighSpeed_BWD";
            this.btn_HighSpeed_BWD.Size = new System.Drawing.Size(128, 48);
            this.btn_HighSpeed_BWD.TabIndex = 7;
            this.btn_HighSpeed_BWD.UseVisualStyleBackColor = false;
            this.btn_HighSpeed_BWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_InverterMotion_MouseDown);
            this.btn_HighSpeed_BWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_HighSpeed_BWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_InverterMotion_MouseUp);
            // 
            // btn_LowSpeed_FWD
            // 
            this.btn_LowSpeed_FWD.BackColor = System.Drawing.Color.White;
            this.btn_LowSpeed_FWD.BackgroundImage = global::Master.Properties.Resources.icons8_forward_96;
            this.btn_LowSpeed_FWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_LowSpeed_FWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_LowSpeed_FWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_LowSpeed_FWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_LowSpeed_FWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LowSpeed_FWD.Location = new System.Drawing.Point(261, 31);
            this.btn_LowSpeed_FWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_LowSpeed_FWD.Name = "btn_LowSpeed_FWD";
            this.btn_LowSpeed_FWD.Size = new System.Drawing.Size(128, 48);
            this.btn_LowSpeed_FWD.TabIndex = 24;
            this.btn_LowSpeed_FWD.UseVisualStyleBackColor = false;
            this.btn_LowSpeed_FWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_InverterMotion_MouseDown);
            this.btn_LowSpeed_FWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_LowSpeed_FWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_InverterMotion_MouseUp);
            // 
            // lbl_LowSpeed_FWD
            // 
            this.lbl_LowSpeed_FWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_LowSpeed_FWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_LowSpeed_FWD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_LowSpeed_FWD.Location = new System.Drawing.Point(263, 3);
            this.lbl_LowSpeed_FWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_LowSpeed_FWD.Name = "lbl_LowSpeed_FWD";
            this.lbl_LowSpeed_FWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_LowSpeed_FWD.Size = new System.Drawing.Size(124, 24);
            this.lbl_LowSpeed_FWD.TabIndex = 27;
            this.lbl_LowSpeed_FWD.Text = "Low Speed FWD";
            this.lbl_LowSpeed_FWD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_HighSpeed_FWD
            // 
            this.lbl_HighSpeed_FWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_HighSpeed_FWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_HighSpeed_FWD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_HighSpeed_FWD.Location = new System.Drawing.Point(393, 3);
            this.lbl_HighSpeed_FWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_HighSpeed_FWD.Name = "lbl_HighSpeed_FWD";
            this.lbl_HighSpeed_FWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_HighSpeed_FWD.Size = new System.Drawing.Size(124, 24);
            this.lbl_HighSpeed_FWD.TabIndex = 28;
            this.lbl_HighSpeed_FWD.Text = "High Speed FWD";
            this.lbl_HighSpeed_FWD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_LowSpeed_BWD
            // 
            this.lbl_LowSpeed_BWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_LowSpeed_BWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_LowSpeed_BWD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_LowSpeed_BWD.Location = new System.Drawing.Point(133, 3);
            this.lbl_LowSpeed_BWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_LowSpeed_BWD.Name = "lbl_LowSpeed_BWD";
            this.lbl_LowSpeed_BWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_LowSpeed_BWD.Size = new System.Drawing.Size(124, 24);
            this.lbl_LowSpeed_BWD.TabIndex = 29;
            this.lbl_LowSpeed_BWD.Text = "Low Speed BWD";
            this.lbl_LowSpeed_BWD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // Frm_InverterMotion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(672, 563);
            this.Controls.Add(this.groupBox_InverterCommand);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_InverterMotion";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Frm_InverterMotion";
            this.groupBox_InverterCommand.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.groupBox_InverterFlag.ResumeLayout(false);
            this.tableLayoutPanel18.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox_Move.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer UIUpdateTimer;
        private System.Windows.Forms.GroupBox groupBox_InverterCommand;
        private System.Windows.Forms.GroupBox groupBox_Move;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private System.Windows.Forms.Button btn_HighSpeed_BWD;
        private System.Windows.Forms.Button btn_HighSpeed_FWD;
        private System.Windows.Forms.Button btn_LowSpeed_BWD;
        private System.Windows.Forms.Button btn_LowSpeed_FWD;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_HighSpeed_BWD;
        private System.Windows.Forms.Label lbl_LowSpeed_FWD;
        private System.Windows.Forms.Label lbl_HighSpeed_FWD;
        private System.Windows.Forms.Label lbl_LowSpeed_BWD;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.GroupBox groupBox_InverterFlag;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel18;
        private System.Windows.Forms.Button btn_InverterStop;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_Reset;
    }
}