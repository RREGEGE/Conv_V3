namespace Synustech.ucPanel.BcSetting
{
    partial class UserTopComm_Setting
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
            this.btnLine = new System.Windows.Forms.Button();
            this.btnCnv = new System.Windows.Forms.Button();
            this.btnPrameter = new System.Windows.Forms.Button();
            this.btnTime = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLine
            // 
            this.btnLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLine.FlatAppearance.BorderSize = 0;
            this.btnLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnLine.Location = new System.Drawing.Point(420, 0);
            this.btnLine.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btnLine.Name = "btnLine";
            this.btnLine.Size = new System.Drawing.Size(139, 41);
            this.btnLine.TabIndex = 7;
            this.btnLine.Text = "Line Setting";
            this.btnLine.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnLine.UseVisualStyleBackColor = true;
            this.btnLine.Click += new System.EventHandler(this.btnLine_Click);
            // 
            // btnCnv
            // 
            this.btnCnv.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCnv.FlatAppearance.BorderSize = 0;
            this.btnCnv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCnv.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCnv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnCnv.Location = new System.Drawing.Point(139, 0);
            this.btnCnv.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btnCnv.Name = "btnCnv";
            this.btnCnv.Size = new System.Drawing.Size(139, 41);
            this.btnCnv.TabIndex = 5;
            this.btnCnv.Text = "Operation";
            this.btnCnv.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnCnv.UseVisualStyleBackColor = true;
            this.btnCnv.Click += new System.EventHandler(this.btnCnv_Click);
            // 
            // btnPrameter
            // 
            this.btnPrameter.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPrameter.FlatAppearance.BorderSize = 0;
            this.btnPrameter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrameter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnPrameter.Location = new System.Drawing.Point(0, 0);
            this.btnPrameter.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btnPrameter.Name = "btnPrameter";
            this.btnPrameter.Size = new System.Drawing.Size(139, 41);
            this.btnPrameter.TabIndex = 4;
            this.btnPrameter.Text = "Parameter";
            this.btnPrameter.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnPrameter.UseVisualStyleBackColor = true;
            this.btnPrameter.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnTime
            // 
            this.btnTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTime.FlatAppearance.BorderSize = 0;
            this.btnTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnTime.Location = new System.Drawing.Point(278, 0);
            this.btnTime.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(139, 41);
            this.btnTime.TabIndex = 6;
            this.btnTime.Text = "Time";
            this.btnTime.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnTime.UseVisualStyleBackColor = true;
            this.btnTime.Click += new System.EventHandler(this.btnTime_Click);
            // 
            // UserTopComm_Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.btnTime);
            this.Controls.Add(this.btnLine);
            this.Controls.Add(this.btnCnv);
            this.Controls.Add(this.btnPrameter);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserTopComm_Setting";
            this.Size = new System.Drawing.Size(1354, 41);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLine;
        private System.Windows.Forms.Button btnCnv;
        private System.Windows.Forms.Button btnPrameter;
        private System.Windows.Forms.Button btnTime;
    }
}
