namespace Synustech
{
    partial class UserMainStatusdg
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
            this.dgMotionView = new System.Windows.Forms.DataGridView();
            this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Colum_Mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CnvEA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CSTEA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Auto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Idle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Manual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Error = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgMotionView)).BeginInit();
            this.SuspendLayout();
            // 
            // dgMotionView
            // 
            this.dgMotionView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgMotionView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.dgMotionView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgMotionView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgMotionView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgMotionView.ColumnHeadersHeight = 30;
            this.dgMotionView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgMotionView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Line,
            this.Colum_Mode,
            this.CnvEA,
            this.CSTEA,
            this.Auto,
            this.Idle,
            this.Manual,
            this.Error});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgMotionView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgMotionView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMotionView.Enabled = false;
            this.dgMotionView.EnableHeadersVisualStyles = false;
            this.dgMotionView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgMotionView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.dgMotionView.Location = new System.Drawing.Point(0, 0);
            this.dgMotionView.Margin = new System.Windows.Forms.Padding(0);
            this.dgMotionView.Name = "dgMotionView";
            this.dgMotionView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgMotionView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgMotionView.RowHeadersVisible = false;
            this.dgMotionView.RowHeadersWidth = 51;
            this.dgMotionView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgMotionView.RowTemplate.Height = 40;
            this.dgMotionView.RowTemplate.ReadOnly = true;
            this.dgMotionView.Size = new System.Drawing.Size(1353, 591);
            this.dgMotionView.TabIndex = 0;
            this.dgMotionView.SelectionChanged += new System.EventHandler(this.dgMotionView_SelectionChanged);
            // 
            // Line
            // 
            this.Line.HeaderText = "   Line";
            this.Line.MinimumWidth = 8;
            this.Line.Name = "Line";
            this.Line.ReadOnly = true;
            // 
            // Colum_Mode
            // 
            this.Colum_Mode.HeaderText = "   Flow Mode";
            this.Colum_Mode.MinimumWidth = 8;
            this.Colum_Mode.Name = "Colum_Mode";
            this.Colum_Mode.ReadOnly = true;
            // 
            // CnvEA
            // 
            this.CnvEA.HeaderText = "   Linked CV";
            this.CnvEA.MinimumWidth = 8;
            this.CnvEA.Name = "CnvEA";
            this.CnvEA.ReadOnly = true;
            // 
            // CSTEA
            // 
            this.CSTEA.HeaderText = "   On CST";
            this.CSTEA.MinimumWidth = 8;
            this.CSTEA.Name = "CSTEA";
            this.CSTEA.ReadOnly = true;
            // 
            // Auto
            // 
            this.Auto.HeaderText = "   Run CV";
            this.Auto.MinimumWidth = 8;
            this.Auto.Name = "Auto";
            this.Auto.ReadOnly = true;
            // 
            // Idle
            // 
            this.Idle.HeaderText = "   Idle CV";
            this.Idle.MinimumWidth = 8;
            this.Idle.Name = "Idle";
            this.Idle.ReadOnly = true;
            // 
            // Manual
            // 
            this.Manual.HeaderText = "   Manual CV";
            this.Manual.MinimumWidth = 8;
            this.Manual.Name = "Manual";
            this.Manual.ReadOnly = true;
            // 
            // Error
            // 
            this.Error.HeaderText = "   Error CV";
            this.Error.MinimumWidth = 8;
            this.Error.Name = "Error";
            this.Error.ReadOnly = true;
            // 
            // UserMainStatusdg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgMotionView);
            this.Margin = new System.Windows.Forms.Padding(1, 0, 1, 2);
            this.Name = "UserMainStatusdg";
            this.Size = new System.Drawing.Size(1353, 591);
            ((System.ComponentModel.ISupportInitialize)(this.dgMotionView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgMotionView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Line;
        private System.Windows.Forms.DataGridViewTextBoxColumn Colum_Mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn CnvEA;
        private System.Windows.Forms.DataGridViewTextBoxColumn CSTEA;
        private System.Windows.Forms.DataGridViewTextBoxColumn Auto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Idle;
        private System.Windows.Forms.DataGridViewTextBoxColumn Manual;
        private System.Windows.Forms.DataGridViewTextBoxColumn Error;
    }
}
