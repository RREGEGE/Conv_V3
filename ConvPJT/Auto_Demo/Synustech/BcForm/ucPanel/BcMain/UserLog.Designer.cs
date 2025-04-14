namespace Synustech
{
    partial class UserLog
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
            this.listLog = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listLog
            // 
            this.listLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.listLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listLog.ForeColor = System.Drawing.Color.White;
            this.listLog.FormattingEnabled = true;
            this.listLog.ItemHeight = 18;
            this.listLog.Location = new System.Drawing.Point(0, 0);
            this.listLog.Margin = new System.Windows.Forms.Padding(0);
            this.listLog.Name = "listLog";
            this.listLog.Size = new System.Drawing.Size(1310, 493);
            this.listLog.TabIndex = 0;
            // 
            // UserLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listLog);
            this.Name = "UserLog";
            this.Size = new System.Drawing.Size(1310, 493);
            this.Load += new System.EventHandler(this.UserLog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listLog;
    }
}
