namespace Synustech.ucPanel.BcMotion
{
    partial class UserConvLineView
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
            this.pnlConvLine = new System.Windows.Forms.Panel();
            this.UI_Update_Timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // pnlConvLine
            // 
            this.pnlConvLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.pnlConvLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConvLine.Location = new System.Drawing.Point(0, 0);
            this.pnlConvLine.Margin = new System.Windows.Forms.Padding(1);
            this.pnlConvLine.Name = "pnlConvLine";
            this.pnlConvLine.Size = new System.Drawing.Size(732, 697);
            this.pnlConvLine.TabIndex = 0;
            this.pnlConvLine.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlConvLine_Paint);
            // 
            // UI_Update_Timer
            // 
            this.UI_Update_Timer.Enabled = true;
            this.UI_Update_Timer.Interval = 10;
            this.UI_Update_Timer.Tick += new System.EventHandler(this.UI_Update_Timer_Tick);
            // 
            // UserConvLineView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.pnlConvLine);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "UserConvLineView";
            this.Size = new System.Drawing.Size(732, 697);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlConvLine;
        private System.Windows.Forms.Timer UI_Update_Timer;
    }
}
