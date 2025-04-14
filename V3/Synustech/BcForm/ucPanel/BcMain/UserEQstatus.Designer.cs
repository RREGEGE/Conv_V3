namespace Synustech.ucPanel
{
    partial class UserEQstatus
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
            this.lblSub = new System.Windows.Forms.Label();
            this.lblMain = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblCheck_1 = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSub
            // 
            this.lblSub.AutoSize = true;
            this.lblSub.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.lblSub.Location = new System.Drawing.Point(18, 45);
            this.lblSub.Name = "lblSub";
            this.lblSub.Size = new System.Drawing.Size(163, 48);
            this.lblSub.TabIndex = 1;
            this.lblSub.Text = "Manual";
            // 
            // lblMain
            // 
            this.lblMain.AutoSize = true;
            this.lblMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblMain.ForeColor = System.Drawing.Color.White;
            this.lblMain.Location = new System.Drawing.Point(18, 11);
            this.lblMain.Name = "lblMain";
            this.lblMain.Size = new System.Drawing.Size(282, 32);
            this.lblMain.TabIndex = 0;
            this.lblMain.Text = "CURRENT STATUS";
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.pnlMain.Controls.Add(this.lblCheck_1);
            this.pnlMain.Controls.Add(this.lblSub);
            this.pnlMain.Controls.Add(this.lblMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(419, 137);
            this.pnlMain.TabIndex = 5;
            this.pnlMain.Resize += new System.EventHandler(this.UserEqPanel_Resize);
            // 
            // lblCheck_1
            // 
            this.lblCheck_1.AutoSize = true;
            this.lblCheck_1.Font = new System.Drawing.Font("Nirmala UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheck_1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(151)))), ((int)(((byte)(176)))));
            this.lblCheck_1.Location = new System.Drawing.Point(20, 89);
            this.lblCheck_1.Name = "lblCheck_1";
            this.lblCheck_1.Size = new System.Drawing.Size(323, 30);
            this.lblCheck_1.TabIndex = 2;
            this.lblCheck_1.Text = "RUN Check, IN/OUT Mode Check";
            // 
            // UserEQstatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.Name = "UserEQstatus";
            this.Size = new System.Drawing.Size(419, 137);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Label lblMain;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblCheck_1;
    }
}
