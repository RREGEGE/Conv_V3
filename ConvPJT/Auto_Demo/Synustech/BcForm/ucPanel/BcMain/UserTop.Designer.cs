namespace Synustech
{
    partial class UserTop
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbl_Time = new System.Windows.Forms.Label();
            this.Timer_Now = new System.Windows.Forms.Timer(this.components);
            this.lblCpuValue = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Alarm = new System.Windows.Forms.Button();
            this.btn_Buzzer = new System.Windows.Forms.Button();
            this.lblRamValue = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureLogo = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Time
            // 
            this.lbl_Time.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Time.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Time.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.lbl_Time.Location = new System.Drawing.Point(2145, 0);
            this.lbl_Time.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Time.Name = "lbl_Time";
            this.tableLayoutPanel1.SetRowSpan(this.lbl_Time, 2);
            this.lbl_Time.Size = new System.Drawing.Size(268, 80);
            this.lbl_Time.TabIndex = 1;
            this.lbl_Time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Timer_Now
            // 
            this.Timer_Now.Interval = 1000;
            this.Timer_Now.Tick += new System.EventHandler(this.Timer_Now_Tick);
            // 
            // lblCpuValue
            // 
            this.lblCpuValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCpuValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCpuValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.lblCpuValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCpuValue.Location = new System.Drawing.Point(1873, 0);
            this.lblCpuValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCpuValue.Name = "lblCpuValue";
            this.lblCpuValue.Size = new System.Drawing.Size(264, 40);
            this.lblCpuValue.TabIndex = 4;
            this.lblCpuValue.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0.5072576F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.91328F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.9451F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.27239F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.27239F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.27239F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.27239F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.27239F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.27239F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_Alarm, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_Buzzer, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Time, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCpuValue, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblRamValue, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureLogo, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(2417, 80);
            this.tableLayoutPanel1.TabIndex = 6;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(784, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 18);
            this.label1.TabIndex = 19;
            // 
            // btn_Alarm
            // 
            this.btn_Alarm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btn_Alarm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Alarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Alarm.FlatAppearance.BorderSize = 0;
            this.btn_Alarm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Alarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Alarm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btn_Alarm.Location = new System.Drawing.Point(1053, 3);
            this.btn_Alarm.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btn_Alarm.Name = "btn_Alarm";
            this.tableLayoutPanel1.SetRowSpan(this.btn_Alarm, 2);
            this.btn_Alarm.Size = new System.Drawing.Size(268, 74);
            this.btn_Alarm.TabIndex = 14;
            this.btn_Alarm.Text = "Alarm Reset";
            this.btn_Alarm.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Alarm.UseVisualStyleBackColor = false;
            this.btn_Alarm.Click += new System.EventHandler(this.btn_Alarm_Click);
            // 
            // btn_Buzzer
            // 
            this.btn_Buzzer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.btn_Buzzer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Buzzer.FlatAppearance.BorderSize = 0;
            this.btn_Buzzer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Buzzer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Buzzer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(226)))), ((int)(((byte)(178)))));
            this.btn_Buzzer.Location = new System.Drawing.Point(1325, 3);
            this.btn_Buzzer.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btn_Buzzer.Name = "btn_Buzzer";
            this.tableLayoutPanel1.SetRowSpan(this.btn_Buzzer, 2);
            this.btn_Buzzer.Size = new System.Drawing.Size(268, 74);
            this.btn_Buzzer.TabIndex = 20;
            this.btn_Buzzer.Text = "Buzzer On";
            this.btn_Buzzer.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Buzzer.UseVisualStyleBackColor = false;
            this.btn_Buzzer.Click += new System.EventHandler(this.btn_Buzzer_Click_1);
            // 
            // lblRamValue
            // 
            this.lblRamValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRamValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRamValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.lblRamValue.Location = new System.Drawing.Point(1873, 40);
            this.lblRamValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRamValue.Name = "lblRamValue";
            this.lblRamValue.Size = new System.Drawing.Size(264, 40);
            this.lblRamValue.TabIndex = 22;
            this.lblRamValue.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(278, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 18);
            this.label2.TabIndex = 21;
            // 
            // pictureLogo
            // 
            this.pictureLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureLogo.Image = global::Synustech.Properties.Resources.Logo_Remove;
            this.pictureLogo.Location = new System.Drawing.Point(26, 4);
            this.pictureLogo.Margin = new System.Windows.Forms.Padding(14, 4, 4, 3);
            this.pictureLogo.Name = "pictureLogo";
            this.pictureLogo.Padding = new System.Windows.Forms.Padding(14, 0, 5, 0);
            this.tableLayoutPanel1.SetRowSpan(this.pictureLogo, 2);
            this.pictureLogo.Size = new System.Drawing.Size(245, 73);
            this.pictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureLogo.TabIndex = 23;
            this.pictureLogo.TabStop = false;
            // 
            // UserTop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "UserTop";
            this.Size = new System.Drawing.Size(2417, 80);
            this.Load += new System.EventHandler(this.UserTop_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbl_Time;
        private System.Windows.Forms.Timer Timer_Now;
        private System.Windows.Forms.Label lblCpuValue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_Alarm;
        private System.Windows.Forms.Button btn_Buzzer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRamValue;
        private System.Windows.Forms.PictureBox pictureLogo;
    }
}
