namespace Synustech.ucPanel.BcAlarm
{
    partial class UserSolution
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
            this.tplSolution = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblSolution = new System.Windows.Forms.Label();
            this.tbSolution = new System.Windows.Forms.TextBox();
            this.tplSolution.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tplSolution
            // 
            this.tplSolution.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tplSolution.ColumnCount = 1;
            this.tplSolution.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tplSolution.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tplSolution.Controls.Add(this.tbSolution, 0, 1);
            this.tplSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tplSolution.Location = new System.Drawing.Point(0, 0);
            this.tplSolution.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.tplSolution.Name = "tplSolution";
            this.tplSolution.RowCount = 2;
            this.tplSolution.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tplSolution.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tplSolution.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tplSolution.Size = new System.Drawing.Size(713, 387);
            this.tplSolution.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.31439F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.68562F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 151F));
            this.tableLayoutPanel1.Controls.Add(this.lblSolution, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(707, 52);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblSolution
            // 
            this.lblSolution.AutoSize = true;
            this.lblSolution.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblSolution.ForeColor = System.Drawing.Color.White;
            this.lblSolution.Location = new System.Drawing.Point(14, 5);
            this.lblSolution.Margin = new System.Windows.Forms.Padding(14, 5, 3, 0);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(172, 47);
            this.lblSolution.TabIndex = 0;
            this.lblSolution.Text = "SOLUTION";
            // 
            // tbSolution
            // 
            this.tbSolution.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tbSolution.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSolution.ForeColor = System.Drawing.Color.White;
            this.tbSolution.Location = new System.Drawing.Point(3, 61);
            this.tbSolution.Multiline = true;
            this.tbSolution.Name = "tbSolution";
            this.tbSolution.ReadOnly = true;
            this.tbSolution.Size = new System.Drawing.Size(707, 323);
            this.tbSolution.TabIndex = 1;
            // 
            // UserSolution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tplSolution);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "UserSolution";
            this.Size = new System.Drawing.Size(713, 387);
            this.tplSolution.ResumeLayout(false);
            this.tplSolution.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tplSolution;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblSolution;
        private System.Windows.Forms.TextBox tbSolution;
    }
}
