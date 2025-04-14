namespace Synustech.ucPanel.BcMotion
{
    partial class UserPowerOn
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
            this.tbl_UserPower_Base = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_Cur_Pos_Out = new System.Windows.Forms.Label();
            this.lbl_CV_ID_Out = new System.Windows.Forms.Label();
            this.lbl_CV_ID = new System.Windows.Forms.Label();
            this.btn_Homing = new System.Windows.Forms.Button();
            this.lbl_Home = new System.Windows.Forms.Label();
            this.btn_PowerOff = new System.Windows.Forms.Button();
            this.btn_PowerOn = new System.Windows.Forms.Button();
            this.lbl_MotorPower = new System.Windows.Forms.Label();
            this.lbl_Cur_Pos = new System.Windows.Forms.Label();
            this.CNV_POS_Update_Timer = new System.Windows.Forms.Timer(this.components);
            this.tbl_UserPower_Base.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbl_UserPower_Base
            // 
            this.tbl_UserPower_Base.ColumnCount = 1;
            this.tbl_UserPower_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbl_UserPower_Base.Controls.Add(this.lbl_Cur_Pos_Out, 0, 3);
            this.tbl_UserPower_Base.Controls.Add(this.lbl_CV_ID_Out, 0, 1);
            this.tbl_UserPower_Base.Controls.Add(this.lbl_CV_ID, 0, 0);
            this.tbl_UserPower_Base.Controls.Add(this.btn_Homing, 0, 8);
            this.tbl_UserPower_Base.Controls.Add(this.lbl_Home, 0, 7);
            this.tbl_UserPower_Base.Controls.Add(this.btn_PowerOff, 0, 6);
            this.tbl_UserPower_Base.Controls.Add(this.btn_PowerOn, 0, 5);
            this.tbl_UserPower_Base.Controls.Add(this.lbl_MotorPower, 0, 4);
            this.tbl_UserPower_Base.Controls.Add(this.lbl_Cur_Pos, 0, 2);
            this.tbl_UserPower_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbl_UserPower_Base.Location = new System.Drawing.Point(0, 0);
            this.tbl_UserPower_Base.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.tbl_UserPower_Base.Name = "tbl_UserPower_Base";
            this.tbl_UserPower_Base.RowCount = 9;
            this.tbl_UserPower_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.166667F));
            this.tbl_UserPower_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tbl_UserPower_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.166667F));
            this.tbl_UserPower_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tbl_UserPower_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.166667F));
            this.tbl_UserPower_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tbl_UserPower_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tbl_UserPower_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.166667F));
            this.tbl_UserPower_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tbl_UserPower_Base.Size = new System.Drawing.Size(449, 1138);
            this.tbl_UserPower_Base.TabIndex = 0;
            // 
            // lbl_Cur_Pos_Out
            // 
            this.lbl_Cur_Pos_Out.AutoSize = true;
            this.lbl_Cur_Pos_Out.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.lbl_Cur_Pos_Out.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbl_Cur_Pos_Out.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Cur_Pos_Out.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_Cur_Pos_Out.ForeColor = System.Drawing.Color.White;
            this.lbl_Cur_Pos_Out.Location = new System.Drawing.Point(4, 283);
            this.lbl_Cur_Pos_Out.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Cur_Pos_Out.Name = "lbl_Cur_Pos_Out";
            this.lbl_Cur_Pos_Out.Size = new System.Drawing.Size(441, 189);
            this.lbl_Cur_Pos_Out.TabIndex = 21;
            this.lbl_Cur_Pos_Out.Text = "0";
            this.lbl_Cur_Pos_Out.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_CV_ID_Out
            // 
            this.lbl_CV_ID_Out.AutoSize = true;
            this.lbl_CV_ID_Out.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.lbl_CV_ID_Out.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbl_CV_ID_Out.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_CV_ID_Out.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_CV_ID_Out.ForeColor = System.Drawing.Color.White;
            this.lbl_CV_ID_Out.Location = new System.Drawing.Point(4, 47);
            this.lbl_CV_ID_Out.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_CV_ID_Out.Name = "lbl_CV_ID_Out";
            this.lbl_CV_ID_Out.Size = new System.Drawing.Size(441, 189);
            this.lbl_CV_ID_Out.TabIndex = 20;
            this.lbl_CV_ID_Out.Text = "0";
            this.lbl_CV_ID_Out.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_CV_ID
            // 
            this.lbl_CV_ID.AutoSize = true;
            this.lbl_CV_ID.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbl_CV_ID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_CV_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_CV_ID.ForeColor = System.Drawing.Color.White;
            this.lbl_CV_ID.Location = new System.Drawing.Point(4, 0);
            this.lbl_CV_ID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_CV_ID.Name = "lbl_CV_ID";
            this.lbl_CV_ID.Size = new System.Drawing.Size(441, 47);
            this.lbl_CV_ID.TabIndex = 18;
            this.lbl_CV_ID.Text = "CV ID";
            this.lbl_CV_ID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Homing
            // 
            this.btn_Homing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_Homing.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Homing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Homing.FlatAppearance.BorderSize = 0;
            this.btn_Homing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Homing.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Homing.ForeColor = System.Drawing.Color.Lime;
            this.btn_Homing.Location = new System.Drawing.Point(0, 947);
            this.btn_Homing.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btn_Homing.Name = "btn_Homing";
            this.btn_Homing.Size = new System.Drawing.Size(445, 188);
            this.btn_Homing.TabIndex = 16;
            this.btn_Homing.Text = "Homing";
            this.btn_Homing.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Homing.UseVisualStyleBackColor = false;
            this.btn_Homing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Homing_MouseDown);
            this.btn_Homing.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Homing_MouseUp);
            // 
            // lbl_Home
            // 
            this.lbl_Home.AutoSize = true;
            this.lbl_Home.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Home.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_Home.ForeColor = System.Drawing.Color.White;
            this.lbl_Home.Location = new System.Drawing.Point(4, 897);
            this.lbl_Home.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Home.Name = "lbl_Home";
            this.lbl_Home.Size = new System.Drawing.Size(441, 47);
            this.lbl_Home.TabIndex = 6;
            this.lbl_Home.Text = "Home";
            this.lbl_Home.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_PowerOff
            // 
            this.btn_PowerOff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_PowerOff.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_PowerOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_PowerOff.FlatAppearance.BorderSize = 0;
            this.btn_PowerOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_PowerOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_PowerOff.ForeColor = System.Drawing.Color.Red;
            this.btn_PowerOff.Location = new System.Drawing.Point(0, 711);
            this.btn_PowerOff.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btn_PowerOff.Name = "btn_PowerOff";
            this.btn_PowerOff.Size = new System.Drawing.Size(445, 183);
            this.btn_PowerOff.TabIndex = 17;
            this.btn_PowerOff.Text = "Power Off";
            this.btn_PowerOff.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_PowerOff.UseVisualStyleBackColor = false;
            this.btn_PowerOff.Click += new System.EventHandler(this.btn_PowerOff_Click);
            // 
            // btn_PowerOn
            // 
            this.btn_PowerOn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_PowerOn.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_PowerOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_PowerOn.FlatAppearance.BorderSize = 0;
            this.btn_PowerOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_PowerOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_PowerOn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btn_PowerOn.Location = new System.Drawing.Point(0, 522);
            this.btn_PowerOn.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btn_PowerOn.Name = "btn_PowerOn";
            this.btn_PowerOn.Size = new System.Drawing.Size(445, 183);
            this.btn_PowerOn.TabIndex = 16;
            this.btn_PowerOn.Text = "Power On";
            this.btn_PowerOn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_PowerOn.UseVisualStyleBackColor = false;
            this.btn_PowerOn.Click += new System.EventHandler(this.btn_PowerOn_Click);
            // 
            // lbl_MotorPower
            // 
            this.lbl_MotorPower.AutoSize = true;
            this.lbl_MotorPower.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbl_MotorPower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_MotorPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_MotorPower.ForeColor = System.Drawing.Color.White;
            this.lbl_MotorPower.Location = new System.Drawing.Point(4, 472);
            this.lbl_MotorPower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_MotorPower.Name = "lbl_MotorPower";
            this.lbl_MotorPower.Size = new System.Drawing.Size(441, 47);
            this.lbl_MotorPower.TabIndex = 4;
            this.lbl_MotorPower.Text = "Motor Power";
            this.lbl_MotorPower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Cur_Pos
            // 
            this.lbl_Cur_Pos.AutoSize = true;
            this.lbl_Cur_Pos.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbl_Cur_Pos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Cur_Pos.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_Cur_Pos.ForeColor = System.Drawing.Color.White;
            this.lbl_Cur_Pos.Location = new System.Drawing.Point(4, 236);
            this.lbl_Cur_Pos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Cur_Pos.Name = "lbl_Cur_Pos";
            this.lbl_Cur_Pos.Size = new System.Drawing.Size(441, 47);
            this.lbl_Cur_Pos.TabIndex = 19;
            this.lbl_Cur_Pos.Text = "Cur. POS";
            this.lbl_Cur_Pos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CNV_POS_Update_Timer
            // 
            this.CNV_POS_Update_Timer.Enabled = true;
            this.CNV_POS_Update_Timer.Tick += new System.EventHandler(this.CNV_POS_Update_Timer_Tick);
            // 
            // UserPowerOn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.tbl_UserPower_Base);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "UserPowerOn";
            this.Size = new System.Drawing.Size(449, 1138);
            this.tbl_UserPower_Base.ResumeLayout(false);
            this.tbl_UserPower_Base.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbl_UserPower_Base;
        private System.Windows.Forms.Label lbl_Home;
        private System.Windows.Forms.Label lbl_MotorPower;
        private System.Windows.Forms.Button btn_Homing;
        private System.Windows.Forms.Button btn_PowerOff;
        private System.Windows.Forms.Button btn_PowerOn;
        private System.Windows.Forms.Label lbl_Cur_Pos_Out;
        private System.Windows.Forms.Label lbl_CV_ID_Out;
        private System.Windows.Forms.Label lbl_CV_ID;
        private System.Windows.Forms.Label lbl_Cur_Pos;
        private System.Windows.Forms.Timer CNV_POS_Update_Timer;
    }
}
