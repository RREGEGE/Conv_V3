namespace Synustech.ucPanel.BcStatus.Setting
{
    partial class UserMotion
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
            this.dgMotion = new System.Windows.Forms.DataGridView();
            this.lblMotionName = new System.Windows.Forms.Label();
            this.tplMotion = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgMotion)).BeginInit();
            this.tplMotion.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgMotion
            // 
            this.dgMotion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgMotion.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgMotion.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgMotion.ColumnHeadersHeight = 30;
            this.dgMotion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgMotion.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgMotion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMotion.EnableHeadersVisualStyles = false;
            this.dgMotion.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.dgMotion.Location = new System.Drawing.Point(3, 38);
            this.dgMotion.Name = "dgMotion";
            this.dgMotion.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgMotion.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgMotion.RowHeadersVisible = false;
            this.dgMotion.RowHeadersWidth = 62;
            this.dgMotion.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial Narrow", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgMotion.RowTemplate.Height = 30;
            this.dgMotion.Size = new System.Drawing.Size(431, 431);
            this.dgMotion.TabIndex = 3;
            // 
            // lblMotionName
            // 
            this.lblMotionName.AutoSize = true;
            this.lblMotionName.Font = new System.Drawing.Font("Nirmala UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMotionName.ForeColor = System.Drawing.Color.White;
            this.lblMotionName.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblMotionName.Location = new System.Drawing.Point(3, 0);
            this.lblMotionName.Name = "lblMotionName";
            this.lblMotionName.Size = new System.Drawing.Size(98, 35);
            this.lblMotionName.TabIndex = 2;
            this.lblMotionName.Text = "Motion";
            // 
            // tplMotion
            // 
            this.tplMotion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tplMotion.ColumnCount = 1;
            this.tplMotion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tplMotion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tplMotion.Controls.Add(this.dgMotion, 0, 1);
            this.tplMotion.Controls.Add(this.lblMotionName, 0, 0);
            this.tplMotion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tplMotion.Location = new System.Drawing.Point(0, 0);
            this.tplMotion.Name = "tplMotion";
            this.tplMotion.RowCount = 2;
            this.tplMotion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.415254F));
            this.tplMotion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.58475F));
            this.tplMotion.Size = new System.Drawing.Size(437, 472);
            this.tplMotion.TabIndex = 3;
            // 
            // UserMotion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tplMotion);
            this.Name = "UserMotion";
            this.Size = new System.Drawing.Size(437, 472);
            ((System.ComponentModel.ISupportInitialize)(this.dgMotion)).EndInit();
            this.tplMotion.ResumeLayout(false);
            this.tplMotion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgMotion;
        private System.Windows.Forms.Label lblMotionName;
        private System.Windows.Forms.TableLayoutPanel tplMotion;
    }
}
