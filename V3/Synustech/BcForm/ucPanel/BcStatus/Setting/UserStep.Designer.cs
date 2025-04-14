namespace Synustech.ucPanel.BcStatus.Setting
{
    partial class UserStep
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgStep = new System.Windows.Forms.DataGridView();
            this.tplStep = new System.Windows.Forms.TableLayoutPanel();
            this.lblStepName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgStep)).BeginInit();
            this.tplStep.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgStep
            // 
            this.dgStep.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgStep.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgStep.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgStep.ColumnHeadersHeight = 30;
            this.dgStep.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgStep.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgStep.EnableHeadersVisualStyles = false;
            this.dgStep.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.dgStep.Location = new System.Drawing.Point(3, 38);
            this.dgStep.Name = "dgStep";
            this.dgStep.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgStep.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgStep.RowHeadersVisible = false;
            this.dgStep.RowHeadersWidth = 62;
            this.dgStep.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial Narrow", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgStep.RowTemplate.Height = 40;
            this.dgStep.Size = new System.Drawing.Size(431, 431);
            this.dgStep.TabIndex = 3;
            // 
            // tplStep
            // 
            this.tplStep.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tplStep.ColumnCount = 1;
            this.tplStep.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tplStep.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tplStep.Controls.Add(this.dgStep, 0, 1);
            this.tplStep.Controls.Add(this.lblStepName, 0, 0);
            this.tplStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tplStep.Location = new System.Drawing.Point(0, 0);
            this.tplStep.Name = "tplStep";
            this.tplStep.RowCount = 2;
            this.tplStep.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.415254F));
            this.tplStep.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.58475F));
            this.tplStep.Size = new System.Drawing.Size(437, 472);
            this.tplStep.TabIndex = 2;
            // 
            // lblStepName
            // 
            this.lblStepName.AutoSize = true;
            this.lblStepName.Font = new System.Drawing.Font("Nirmala UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepName.ForeColor = System.Drawing.Color.White;
            this.lblStepName.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblStepName.Location = new System.Drawing.Point(3, 0);
            this.lblStepName.Name = "lblStepName";
            this.lblStepName.Size = new System.Drawing.Size(66, 35);
            this.lblStepName.TabIndex = 2;
            this.lblStepName.Text = "Step";
            // 
            // UserStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tplStep);
            this.Name = "UserStep";
            this.Size = new System.Drawing.Size(437, 472);
            ((System.ComponentModel.ISupportInitialize)(this.dgStep)).EndInit();
            this.tplStep.ResumeLayout(false);
            this.tplStep.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgStep;
        private System.Windows.Forms.TableLayoutPanel tplStep;
        private System.Windows.Forms.Label lblStepName;
    }
}
