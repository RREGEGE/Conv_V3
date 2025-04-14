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
            this.btnOperation = new System.Windows.Forms.Button();
            this.btnPrameter = new System.Windows.Forms.Button();
            this.btnTime = new System.Windows.Forms.Button();
            this.btnInCvCreate = new System.Windows.Forms.Button();
            this.btnInLineCreate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOperation
            // 
            this.btnOperation.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOperation.FlatAppearance.BorderSize = 0;
            this.btnOperation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOperation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOperation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnOperation.Location = new System.Drawing.Point(139, 0);
            this.btnOperation.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btnOperation.Name = "btnOperation";
            this.btnOperation.Size = new System.Drawing.Size(139, 41);
            this.btnOperation.TabIndex = 5;
            this.btnOperation.Text = "Operation";
            this.btnOperation.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnOperation.UseVisualStyleBackColor = true;
            this.btnOperation.Click += new System.EventHandler(this.btnOperationClick);
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
            this.btnPrameter.Click += new System.EventHandler(this.btnWMXPara_Click);
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
            this.btnTime.Text = "Watchdog";
            this.btnTime.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnTime.UseVisualStyleBackColor = true;
            this.btnTime.Click += new System.EventHandler(this.btnTime_Click);
            // 
            // btnInCvCreate
            // 
            this.btnInCvCreate.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnInCvCreate.FlatAppearance.BorderSize = 0;
            this.btnInCvCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInCvCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInCvCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnInCvCreate.Location = new System.Drawing.Point(417, 0);
            this.btnInCvCreate.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btnInCvCreate.Name = "btnInCvCreate";
            this.btnInCvCreate.Size = new System.Drawing.Size(139, 41);
            this.btnInCvCreate.TabIndex = 8;
            this.btnInCvCreate.Text = "CV Create";
            this.btnInCvCreate.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnInCvCreate.UseVisualStyleBackColor = true;
            this.btnInCvCreate.Click += new System.EventHandler(this.btnInCvCreate_Click);
            // 
            // btnInLineCreate
            // 
            this.btnInLineCreate.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnInLineCreate.FlatAppearance.BorderSize = 0;
            this.btnInLineCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInLineCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInLineCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnInLineCreate.Location = new System.Drawing.Point(556, 0);
            this.btnInLineCreate.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btnInLineCreate.Name = "btnInLineCreate";
            this.btnInLineCreate.Size = new System.Drawing.Size(139, 41);
            this.btnInLineCreate.TabIndex = 9;
            this.btnInLineCreate.Text = "Line Create";
            this.btnInLineCreate.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnInLineCreate.UseVisualStyleBackColor = true;
            this.btnInLineCreate.Click += new System.EventHandler(this.btnInLineCreate_Click);
            // 
            // UserTopComm_Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.btnInLineCreate);
            this.Controls.Add(this.btnInCvCreate);
            this.Controls.Add(this.btnTime);
            this.Controls.Add(this.btnOperation);
            this.Controls.Add(this.btnPrameter);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserTopComm_Setting";
            this.Size = new System.Drawing.Size(1354, 41);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOperation;
        private System.Windows.Forms.Button btnPrameter;
        private System.Windows.Forms.Button btnTime;
        private System.Windows.Forms.Button btnInCvCreate;
        private System.Windows.Forms.Button btnInLineCreate;
    }
}
