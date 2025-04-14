namespace Synustech.ucPanel.BcStatus
{
    partial class UserTopComm_Status
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
            this.btn_Main = new System.Windows.Forms.Button();
            this.btn_IO = new System.Windows.Forms.Button();
            this.btn_RFID = new System.Windows.Forms.Button();
            this.btn_Conv = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_Main
            // 
            this.btn_Main.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Main.FlatAppearance.BorderSize = 0;
            this.btn_Main.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Main.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Main.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btn_Main.Location = new System.Drawing.Point(0, 0);
            this.btn_Main.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btn_Main.Name = "btn_Main";
            this.btn_Main.Size = new System.Drawing.Size(199, 62);
            this.btn_Main.TabIndex = 0;
            this.btn_Main.Text = "Status";
            this.btn_Main.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Main.UseVisualStyleBackColor = true;
            this.btn_Main.Click += new System.EventHandler(this.btn_Main_Click);
            // 
            // btn_IO
            // 
            this.btn_IO.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_IO.FlatAppearance.BorderSize = 0;
            this.btn_IO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_IO.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_IO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btn_IO.Location = new System.Drawing.Point(199, 0);
            this.btn_IO.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btn_IO.Name = "btn_IO";
            this.btn_IO.Size = new System.Drawing.Size(199, 62);
            this.btn_IO.TabIndex = 13;
            this.btn_IO.Text = "I/O Monitor";
            this.btn_IO.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_IO.UseVisualStyleBackColor = true;
            this.btn_IO.Click += new System.EventHandler(this.btn_IO_Click);
            // 
            // btn_RFID
            // 
            this.btn_RFID.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn_RFID.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_RFID.FlatAppearance.BorderSize = 0;
            this.btn_RFID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_RFID.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_RFID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btn_RFID.Location = new System.Drawing.Point(398, 0);
            this.btn_RFID.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btn_RFID.Name = "btn_RFID";
            this.btn_RFID.Size = new System.Drawing.Size(199, 62);
            this.btn_RFID.TabIndex = 16;
            this.btn_RFID.Text = "RFID";
            this.btn_RFID.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_RFID.UseVisualStyleBackColor = true;
            this.btn_RFID.Click += new System.EventHandler(this.btn_RFID_Click);
            // 
            // btn_Conv
            // 
            this.btn_Conv.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Conv.FlatAppearance.BorderSize = 0;
            this.btn_Conv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Conv.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Conv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btn_Conv.Location = new System.Drawing.Point(597, 0);
            this.btn_Conv.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btn_Conv.Name = "btn_Conv";
            this.btn_Conv.Size = new System.Drawing.Size(199, 62);
            this.btn_Conv.TabIndex = 17;
            this.btn_Conv.Text = "CV Line View";
            this.btn_Conv.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Conv.UseVisualStyleBackColor = true;
            this.btn_Conv.Click += new System.EventHandler(this.btn_Conv_Click);
            // 
            // UserTopComm_Status
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.btn_Conv);
            this.Controls.Add(this.btn_RFID);
            this.Controls.Add(this.btn_IO);
            this.Controls.Add(this.btn_Main);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserTopComm_Status";
            this.Size = new System.Drawing.Size(1934, 62);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Main;
        private System.Windows.Forms.Button btn_IO;
        private System.Windows.Forms.Button btn_RFID;
        private System.Windows.Forms.Button btn_Conv;
    }
}
