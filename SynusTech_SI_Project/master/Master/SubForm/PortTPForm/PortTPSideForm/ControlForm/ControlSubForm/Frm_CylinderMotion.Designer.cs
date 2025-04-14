
namespace Master.SubForm.PortTPForm.PortTPSubForm.ControlForm
{
    partial class Frm_CylinderMotion
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
            this.groupBox_MotionCommand = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_CylinderFlag = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox_Move = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_BWD1 = new System.Windows.Forms.Button();
            this.btn_FWD1 = new System.Windows.Forms.Button();
            this.lbl_FWD1 = new System.Windows.Forms.Label();
            this.lbl_BWD1 = new System.Windows.Forms.Label();
            this.groupBox_MotionCommand.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.groupBox_CylinderFlag.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox_Move.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.SuspendLayout();
            // 
            // UIUpdateTimer
            // 
            this.UIUpdateTimer.Interval = 250;
            this.UIUpdateTimer.Tick += new System.EventHandler(this.UIUpdateTimer_Tick);
            // 
            // groupBox_MotionCommand
            // 
            this.groupBox_MotionCommand.Controls.Add(this.tableLayoutPanel14);
            this.groupBox_MotionCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_MotionCommand.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_MotionCommand.Location = new System.Drawing.Point(3, 3);
            this.groupBox_MotionCommand.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_MotionCommand.Name = "groupBox_MotionCommand";
            this.groupBox_MotionCommand.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_MotionCommand.Size = new System.Drawing.Size(666, 557);
            this.groupBox_MotionCommand.TabIndex = 9;
            this.groupBox_MotionCommand.TabStop = false;
            this.groupBox_MotionCommand.Text = "Cylinder Command";
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 2;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel14.Controls.Add(this.groupBox_CylinderFlag, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(1, 19);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(664, 537);
            this.tableLayoutPanel14.TabIndex = 3;
            // 
            // groupBox_CylinderFlag
            // 
            this.groupBox_CylinderFlag.Controls.Add(this.tableLayoutPanel18);
            this.groupBox_CylinderFlag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_CylinderFlag.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_CylinderFlag.Location = new System.Drawing.Point(1, 1);
            this.groupBox_CylinderFlag.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_CylinderFlag.Name = "groupBox_CylinderFlag";
            this.groupBox_CylinderFlag.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_CylinderFlag.Size = new System.Drawing.Size(130, 535);
            this.groupBox_CylinderFlag.TabIndex = 7;
            this.groupBox_CylinderFlag.TabStop = false;
            this.groupBox_CylinderFlag.Text = "Flag";
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.ColumnCount = 1;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel18.Controls.Add(this.btn_Stop, 0, 0);
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
            // btn_Stop
            // 
            this.btn_Stop.BackColor = System.Drawing.Color.White;
            this.btn_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Stop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Stop.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Stop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Stop.Location = new System.Drawing.Point(1, 1);
            this.btn_Stop.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(126, 101);
            this.btn_Stop.TabIndex = 11;
            this.btn_Stop.Text = "Cylinder Stop";
            this.btn_Stop.UseVisualStyleBackColor = false;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox_Move);
            this.panel1.Location = new System.Drawing.Point(135, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(522, 527);
            this.panel1.TabIndex = 2;
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
            this.tableLayoutPanel15.Controls.Add(this.btn_BWD1, 1, 1);
            this.tableLayoutPanel15.Controls.Add(this.btn_FWD1, 2, 1);
            this.tableLayoutPanel15.Controls.Add(this.lbl_FWD1, 2, 0);
            this.tableLayoutPanel15.Controls.Add(this.lbl_BWD1, 1, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(1, 19);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 2;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(520, 80);
            this.tableLayoutPanel15.TabIndex = 0;
            // 
            // btn_BWD1
            // 
            this.btn_BWD1.BackColor = System.Drawing.Color.White;
            this.btn_BWD1.BackgroundImage = global::Master.Properties.Resources.icons8_back_96;
            this.btn_BWD1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_BWD1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_BWD1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_BWD1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_BWD1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_BWD1.Location = new System.Drawing.Point(131, 31);
            this.btn_BWD1.Margin = new System.Windows.Forms.Padding(1);
            this.btn_BWD1.Name = "btn_BWD1";
            this.btn_BWD1.Size = new System.Drawing.Size(128, 48);
            this.btn_BWD1.TabIndex = 23;
            this.btn_BWD1.UseVisualStyleBackColor = false;
            this.btn_BWD1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_CylinderMove_MouseDown);
            this.btn_BWD1.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_BWD1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_CylinderMove_MouseUp);
            // 
            // btn_FWD1
            // 
            this.btn_FWD1.BackColor = System.Drawing.Color.White;
            this.btn_FWD1.BackgroundImage = global::Master.Properties.Resources.icons8_forward_96;
            this.btn_FWD1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_FWD1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_FWD1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_FWD1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_FWD1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_FWD1.Location = new System.Drawing.Point(261, 31);
            this.btn_FWD1.Margin = new System.Windows.Forms.Padding(1);
            this.btn_FWD1.Name = "btn_FWD1";
            this.btn_FWD1.Size = new System.Drawing.Size(128, 48);
            this.btn_FWD1.TabIndex = 24;
            this.btn_FWD1.UseVisualStyleBackColor = false;
            this.btn_FWD1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_CylinderMove_MouseDown);
            this.btn_FWD1.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_FWD1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_CylinderMove_MouseUp);
            // 
            // lbl_FWD1
            // 
            this.lbl_FWD1.BackColor = System.Drawing.Color.Transparent;
            this.lbl_FWD1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_FWD1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_FWD1.Location = new System.Drawing.Point(263, 3);
            this.lbl_FWD1.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_FWD1.Name = "lbl_FWD1";
            this.lbl_FWD1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_FWD1.Size = new System.Drawing.Size(124, 24);
            this.lbl_FWD1.TabIndex = 27;
            this.lbl_FWD1.Text = "FWD";
            this.lbl_FWD1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_BWD1
            // 
            this.lbl_BWD1.BackColor = System.Drawing.Color.Transparent;
            this.lbl_BWD1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_BWD1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_BWD1.Location = new System.Drawing.Point(133, 3);
            this.lbl_BWD1.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_BWD1.Name = "lbl_BWD1";
            this.lbl_BWD1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_BWD1.Size = new System.Drawing.Size(124, 24);
            this.lbl_BWD1.TabIndex = 29;
            this.lbl_BWD1.Text = "BWD";
            this.lbl_BWD1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // Frm_CylinderMotion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(672, 563);
            this.Controls.Add(this.groupBox_MotionCommand);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_CylinderMotion";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Frm_CylinderMotion";
            this.groupBox_MotionCommand.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.groupBox_CylinderFlag.ResumeLayout(false);
            this.tableLayoutPanel18.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox_Move.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer UIUpdateTimer;
        private System.Windows.Forms.GroupBox groupBox_MotionCommand;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox_Move;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private System.Windows.Forms.Button btn_BWD1;
        private System.Windows.Forms.Button btn_FWD1;
        private System.Windows.Forms.Label lbl_FWD1;
        private System.Windows.Forms.Label lbl_BWD1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.GroupBox groupBox_CylinderFlag;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel18;
        private System.Windows.Forms.Button btn_Stop;
    }
}