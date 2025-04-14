namespace Synustech
{
    partial class UserTopComm_Motion
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
            this.btnCycleTest = new System.Windows.Forms.Button();
            this.btnTeaching = new System.Windows.Forms.Button();
            this.btnManual = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCycleTest
            // 
            this.btnCycleTest.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCycleTest.FlatAppearance.BorderSize = 0;
            this.btnCycleTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycleTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCycleTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnCycleTest.Location = new System.Drawing.Point(398, 0);
            this.btnCycleTest.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btnCycleTest.Name = "btnCycleTest";
            this.btnCycleTest.Size = new System.Drawing.Size(199, 62);
            this.btnCycleTest.TabIndex = 3;
            this.btnCycleTest.Text = "Cycle Test";
            this.btnCycleTest.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnCycleTest.UseVisualStyleBackColor = true;
            this.btnCycleTest.Click += new System.EventHandler(this.btnCycleTest_Click);
            // 
            // btnTeaching
            // 
            this.btnTeaching.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTeaching.FlatAppearance.BorderSize = 0;
            this.btnTeaching.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTeaching.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTeaching.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnTeaching.Location = new System.Drawing.Point(199, 0);
            this.btnTeaching.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btnTeaching.Name = "btnTeaching";
            this.btnTeaching.Size = new System.Drawing.Size(199, 62);
            this.btnTeaching.TabIndex = 2;
            this.btnTeaching.Text = "Teaching";
            this.btnTeaching.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnTeaching.UseVisualStyleBackColor = true;
            this.btnTeaching.Click += new System.EventHandler(this.btnTeaching_Click);
            // 
            // btnManual
            // 
            this.btnManual.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnManual.FlatAppearance.BorderSize = 0;
            this.btnManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManual.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnManual.Location = new System.Drawing.Point(0, 0);
            this.btnManual.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(199, 62);
            this.btnManual.TabIndex = 1;
            this.btnManual.Text = "Manual";
            this.btnManual.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnManual.UseVisualStyleBackColor = true;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // UserTopComm_Motion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.btnCycleTest);
            this.Controls.Add(this.btnTeaching);
            this.Controls.Add(this.btnManual);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserTopComm_Motion";
            this.Size = new System.Drawing.Size(1934, 62);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCycleTest;
        private System.Windows.Forms.Button btnTeaching;
        private System.Windows.Forms.Button btnManual;
    }
}
