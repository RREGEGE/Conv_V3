namespace Synustech.ucPanel.BcMotion
{
    partial class UserCycleStatus
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_CycleStatus = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotalCycleCount = new System.Windows.Forms.Label();
            this.lblCycleCount = new System.Windows.Forms.Label();
            this.lblCycleTime = new System.Windows.Forms.Label();
            this.lblLineID = new System.Windows.Forms.Label();
            this.UI_Update_Timer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbl_CycleStatus, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(426, 556);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // lbl_CycleStatus
            // 
            this.lbl_CycleStatus.AutoSize = true;
            this.lbl_CycleStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbl_CycleStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_CycleStatus.ForeColor = System.Drawing.Color.White;
            this.lbl_CycleStatus.Location = new System.Drawing.Point(14, 8);
            this.lbl_CycleStatus.Margin = new System.Windows.Forms.Padding(14, 8, 4, 0);
            this.lbl_CycleStatus.Name = "lbl_CycleStatus";
            this.lbl_CycleStatus.Size = new System.Drawing.Size(147, 33);
            this.lbl_CycleStatus.TabIndex = 3;
            this.lbl_CycleStatus.Text = "Cycle Status";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(420, 509);
            this.panel1.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.lblTotalCycleCount, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblCycleCount, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblCycleTime, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblLineID, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(420, 509);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // lblTotalCycleCount
            // 
            this.lblTotalCycleCount.AutoSize = true;
            this.lblTotalCycleCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalCycleCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTotalCycleCount.ForeColor = System.Drawing.Color.White;
            this.lblTotalCycleCount.Location = new System.Drawing.Point(3, 252);
            this.lblTotalCycleCount.Name = "lblTotalCycleCount";
            this.lblTotalCycleCount.Size = new System.Drawing.Size(414, 84);
            this.lblTotalCycleCount.TabIndex = 3;
            this.lblTotalCycleCount.Text = "Total Cycle Count:";
            this.lblTotalCycleCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCycleCount
            // 
            this.lblCycleCount.AutoSize = true;
            this.lblCycleCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCycleCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblCycleCount.ForeColor = System.Drawing.Color.White;
            this.lblCycleCount.Location = new System.Drawing.Point(3, 84);
            this.lblCycleCount.Name = "lblCycleCount";
            this.lblCycleCount.Size = new System.Drawing.Size(414, 84);
            this.lblCycleCount.TabIndex = 1;
            this.lblCycleCount.Text = "Cycle Count:";
            this.lblCycleCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCycleTime
            // 
            this.lblCycleTime.AutoSize = true;
            this.lblCycleTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCycleTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblCycleTime.ForeColor = System.Drawing.Color.White;
            this.lblCycleTime.Location = new System.Drawing.Point(3, 168);
            this.lblCycleTime.Name = "lblCycleTime";
            this.lblCycleTime.Size = new System.Drawing.Size(414, 84);
            this.lblCycleTime.TabIndex = 2;
            this.lblCycleTime.Text = "Cycle Time:";
            this.lblCycleTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLineID
            // 
            this.lblLineID.AutoSize = true;
            this.lblLineID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLineID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblLineID.ForeColor = System.Drawing.Color.White;
            this.lblLineID.Location = new System.Drawing.Point(3, 0);
            this.lblLineID.Name = "lblLineID";
            this.lblLineID.Size = new System.Drawing.Size(414, 84);
            this.lblLineID.TabIndex = 0;
            this.lblLineID.Text = "Line ID:";
            this.lblLineID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UI_Update_Timer
            // 
            this.UI_Update_Timer.Enabled = true;
            this.UI_Update_Timer.Interval = 500;
            this.UI_Update_Timer.Tick += new System.EventHandler(this.UI_Update_Timer_Tick);
            // 
            // UserCycleStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UserCycleStatus";
            this.Size = new System.Drawing.Size(426, 556);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbl_CycleStatus;
        private System.Windows.Forms.Timer UI_Update_Timer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblLineID;
        private System.Windows.Forms.Label lblCycleCount;
        private System.Windows.Forms.Label lblCycleTime;
        private System.Windows.Forms.Label lblTotalCycleCount;
        private System.Windows.Forms.Panel panel1;
    }
}
