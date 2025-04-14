namespace Synustech
{
    partial class UserMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserMenu));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.btn_Setting = new System.Windows.Forms.Button();
            this.btn_Motion = new System.Windows.Forms.Button();
            this.PictureLogin = new System.Windows.Forms.PictureBox();
            this.PanelNav = new System.Windows.Forms.Panel();
            this.btn_Main = new System.Windows.Forms.Button();
            this.btn_Status = new System.Windows.Forms.Button();
            this.lbl_Login = new System.Windows.Forms.Label();
            this.lblMaker = new System.Windows.Forms.Label();
            this.btn_Alarm = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureLogin)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btn_Exit, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.btn_Setting, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.btn_Motion, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.PictureLogin, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.PanelNav, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btn_Main, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btn_Status, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Login, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblMaker, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btn_Alarm, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.857143F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.571428F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.857143F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(318, 864);
            this.tableLayoutPanel1.TabIndex = 19;
            // 
            // btn_Exit
            // 
            this.btn_Exit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Exit.FlatAppearance.BorderSize = 0;
            this.btn_Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Exit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btn_Exit.Image = global::Synustech.Properties.Resources.Exit_48;
            this.btn_Exit.Location = new System.Drawing.Point(10, 739);
            this.btn_Exit.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(305, 123);
            this.btn_Exit.TabIndex = 18;
            this.btn_Exit.Text = "EXIT";
            this.btn_Exit.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click_1);
            this.btn_Exit.Leave += new System.EventHandler(this.btn_Exit_Leave_1);
            // 
            // btn_Setting
            // 
            this.btn_Setting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Setting.FlatAppearance.BorderSize = 0;
            this.btn_Setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Setting.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Setting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btn_Setting.Image = global::Synustech.Properties.Resources.setting;
            this.btn_Setting.Location = new System.Drawing.Point(10, 616);
            this.btn_Setting.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Setting.Name = "btn_Setting";
            this.btn_Setting.Size = new System.Drawing.Size(305, 119);
            this.btn_Setting.TabIndex = 16;
            this.btn_Setting.Text = "Setting";
            this.btn_Setting.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Setting.UseVisualStyleBackColor = true;
            this.btn_Setting.Click += new System.EventHandler(this.btn_Setting_Click_1);
            this.btn_Setting.Leave += new System.EventHandler(this.btn_Setting_Leave_1);
            // 
            // btn_Motion
            // 
            this.btn_Motion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Motion.FlatAppearance.BorderSize = 0;
            this.btn_Motion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Motion.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Motion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btn_Motion.Image = global::Synustech.Properties.Resources.Motion_48;
            this.btn_Motion.Location = new System.Drawing.Point(10, 370);
            this.btn_Motion.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Motion.Name = "btn_Motion";
            this.btn_Motion.Size = new System.Drawing.Size(305, 119);
            this.btn_Motion.TabIndex = 14;
            this.btn_Motion.Text = "Motion";
            this.btn_Motion.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Motion.UseVisualStyleBackColor = true;
            this.btn_Motion.Click += new System.EventHandler(this.btn_Motion_Click_1);
            this.btn_Motion.Leave += new System.EventHandler(this.btn_Motion_Leave_1);
            // 
            // PictureLogin
            // 
            this.PictureLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PictureLogin.Image = ((System.Drawing.Image)(resources.GetObject("PictureLogin.Image")));
            this.PictureLogin.Location = new System.Drawing.Point(131, 39);
            this.PictureLogin.Margin = new System.Windows.Forms.Padding(0);
            this.PictureLogin.Name = "PictureLogin";
            this.PictureLogin.Size = new System.Drawing.Size(66, 44);
            this.PictureLogin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureLogin.TabIndex = 0;
            this.PictureLogin.TabStop = false;
            this.PictureLogin.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureLogin_MouseDown);
            // 
            // PanelNav
            // 
            this.PanelNav.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.PanelNav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelNav.Location = new System.Drawing.Point(3, 124);
            this.PanelNav.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PanelNav.Name = "PanelNav";
            this.PanelNav.Size = new System.Drawing.Size(8, 118);
            this.PanelNav.TabIndex = 3;
            // 
            // btn_Main
            // 
            this.btn_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Main.FlatAppearance.BorderSize = 0;
            this.btn_Main.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Main.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Main.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btn_Main.Image = global::Synustech.Properties.Resources.main_48;
            this.btn_Main.Location = new System.Drawing.Point(10, 124);
            this.btn_Main.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Main.Name = "btn_Main";
            this.btn_Main.Size = new System.Drawing.Size(305, 119);
            this.btn_Main.TabIndex = 12;
            this.btn_Main.Text = "Main";
            this.btn_Main.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Main.UseVisualStyleBackColor = true;
            this.btn_Main.Click += new System.EventHandler(this.btn_Main_Click_1);
            this.btn_Main.Leave += new System.EventHandler(this.btn_Main_Leave_1);
            // 
            // btn_Status
            // 
            this.btn_Status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Status.FlatAppearance.BorderSize = 0;
            this.btn_Status.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btn_Status.Image = global::Synustech.Properties.Resources.IO_48;
            this.btn_Status.Location = new System.Drawing.Point(10, 247);
            this.btn_Status.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Status.Name = "btn_Status";
            this.btn_Status.Size = new System.Drawing.Size(305, 119);
            this.btn_Status.TabIndex = 13;
            this.btn_Status.Text = "Status";
            this.btn_Status.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Status.UseVisualStyleBackColor = true;
            this.btn_Status.Click += new System.EventHandler(this.btn_Status_Click_1);
            this.btn_Status.Leave += new System.EventHandler(this.btn_Status_Leave_1);
            // 
            // lbl_Login
            // 
            this.lbl_Login.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Login.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.lbl_Login.Location = new System.Drawing.Point(10, 0);
            this.lbl_Login.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lbl_Login.Name = "lbl_Login";
            this.lbl_Login.Size = new System.Drawing.Size(305, 24);
            this.lbl_Login.TabIndex = 2;
            this.lbl_Login.Text = "Login";
            this.lbl_Login.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMaker
            // 
            this.lblMaker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaker.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblMaker.Location = new System.Drawing.Point(13, 98);
            this.lblMaker.Name = "lblMaker";
            this.lblMaker.Size = new System.Drawing.Size(302, 24);
            this.lblMaker.TabIndex = 3;
            this.lblMaker.Text = "Maker";
            this.lblMaker.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btn_Alarm
            // 
            this.btn_Alarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Alarm.FlatAppearance.BorderSize = 0;
            this.btn_Alarm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Alarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Alarm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btn_Alarm.Image = global::Synustech.Properties.Resources.Error_48;
            this.btn_Alarm.Location = new System.Drawing.Point(10, 493);
            this.btn_Alarm.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Alarm.Name = "btn_Alarm";
            this.btn_Alarm.Size = new System.Drawing.Size(305, 119);
            this.btn_Alarm.TabIndex = 15;
            this.btn_Alarm.Text = "Alarm";
            this.btn_Alarm.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Alarm.UseVisualStyleBackColor = true;
            this.btn_Alarm.Click += new System.EventHandler(this.btn_Alarm_Click_1);
            this.btn_Alarm.Leave += new System.EventHandler(this.btn_Alarm_Leave_1);
            // 
            // UserMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserMenu";
            this.Size = new System.Drawing.Size(318, 864);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureLogin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblMaker;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.Label lbl_Login;
        private System.Windows.Forms.PictureBox PictureLogin;
        private System.Windows.Forms.Button btn_Setting;
        private System.Windows.Forms.Button btn_Alarm;
        private System.Windows.Forms.Button btn_Status;
        private System.Windows.Forms.Button btn_Motion;
        private System.Windows.Forms.Button btn_Main;
        private System.Windows.Forms.Panel PanelNav;
    }
}
