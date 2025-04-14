namespace Synustech
{
    partial class UserTopComm
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
            this.btnEtherCat = new System.Windows.Forms.Button();
            this.btnPower = new System.Windows.Forms.Button();
            this.btnMode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEtherCat
            // 
            this.btnEtherCat.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnEtherCat.FlatAppearance.BorderSize = 0;
            this.btnEtherCat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEtherCat.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEtherCat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnEtherCat.Location = new System.Drawing.Point(0, 0);
            this.btnEtherCat.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btnEtherCat.Name = "btnEtherCat";
            this.btnEtherCat.Size = new System.Drawing.Size(199, 62);
            this.btnEtherCat.TabIndex = 7;
            this.btnEtherCat.Text = "EtherCAT";
            this.btnEtherCat.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnEtherCat.UseVisualStyleBackColor = true;
            this.btnEtherCat.Click += new System.EventHandler(this.btnEtherCat_Click);
            // 
            // btnPower
            // 
            this.btnPower.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPower.FlatAppearance.BorderSize = 0;
            this.btnPower.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPower.ForeColor = System.Drawing.Color.DarkGray;
            this.btnPower.Location = new System.Drawing.Point(199, 0);
            this.btnPower.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(199, 62);
            this.btnPower.TabIndex = 7;
            this.btnPower.Text = "All Power On";
            this.btnPower.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnPower.UseVisualStyleBackColor = true;
            this.btnPower.Click += new System.EventHandler(this.btnPower_Click);
            // 
            // btnMode
            // 
            this.btnMode.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMode.FlatAppearance.BorderSize = 0;
            this.btnMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnMode.Location = new System.Drawing.Point(398, 0);
            this.btnMode.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btnMode.Name = "btnMode";
            this.btnMode.Size = new System.Drawing.Size(199, 62);
            this.btnMode.TabIndex = 9;
            this.btnMode.Text = "All Auto Run";
            this.btnMode.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnMode.UseVisualStyleBackColor = true;
            this.btnMode.Click += new System.EventHandler(this.btnMode_Click);
            // 
            // UserTopComm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.btnMode);
            this.Controls.Add(this.btnPower);
            this.Controls.Add(this.btnEtherCat);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserTopComm";
            this.Size = new System.Drawing.Size(1934, 62);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnEtherCat;
        private System.Windows.Forms.Button btnPower;
        private System.Windows.Forms.Button btnMode;
    }
}
