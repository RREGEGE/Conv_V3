namespace Synustech.BcForm.ucPanel.BcMain
{
    partial class UserCVStatusdg
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
            this.dgCvStatusView = new System.Windows.Forms.DataGridView();
            this.CnvId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CnvType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CnvHome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CVStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Auto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Idle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CST_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Manual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgCvStatusView)).BeginInit();
            this.SuspendLayout();
            // 
            // dgCvStatusView
            // 
            this.dgCvStatusView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgCvStatusView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.dgCvStatusView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgCvStatusView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCvStatusView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgCvStatusView.ColumnHeadersHeight = 30;
            this.dgCvStatusView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgCvStatusView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CnvId,
            this.CnvType,
            this.CnvHome,
            this.CVStatus,
            this.Auto,
            this.Idle,
            this.CST_ID,
            this.Manual});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgCvStatusView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgCvStatusView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCvStatusView.EnableHeadersVisualStyles = false;
            this.dgCvStatusView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgCvStatusView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.dgCvStatusView.Location = new System.Drawing.Point(0, 0);
            this.dgCvStatusView.Margin = new System.Windows.Forms.Padding(0);
            this.dgCvStatusView.Name = "dgCvStatusView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCvStatusView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgCvStatusView.RowHeadersVisible = false;
            this.dgCvStatusView.RowHeadersWidth = 51;
            this.dgCvStatusView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgCvStatusView.RowTemplate.Height = 40;
            this.dgCvStatusView.RowTemplate.ReadOnly = true;
            this.dgCvStatusView.Size = new System.Drawing.Size(947, 66);
            this.dgCvStatusView.TabIndex = 1;
            // 
            // CnvId
            // 
            this.CnvId.HeaderText = "   CV ID";
            this.CnvId.MinimumWidth = 8;
            this.CnvId.Name = "CnvId";
            this.CnvId.ReadOnly = true;
            // 
            // CnvType
            // 
            this.CnvType.HeaderText = "   TYPE";
            this.CnvType.Name = "CnvType";
            this.CnvType.ReadOnly = true;
            // 
            // CnvHome
            // 
            this.CnvHome.HeaderText = "   HOME";
            this.CnvHome.MinimumWidth = 8;
            this.CnvHome.Name = "CnvHome";
            this.CnvHome.ReadOnly = true;
            // 
            // CVStatus
            // 
            this.CVStatus.HeaderText = "   STATUS";
            this.CVStatus.MinimumWidth = 8;
            this.CVStatus.Name = "CVStatus";
            this.CVStatus.ReadOnly = true;
            // 
            // Auto
            // 
            this.Auto.HeaderText = "   POSITION";
            this.Auto.MinimumWidth = 8;
            this.Auto.Name = "Auto";
            this.Auto.ReadOnly = true;
            // 
            // Idle
            // 
            this.Idle.HeaderText = "   SPEED";
            this.Idle.MinimumWidth = 8;
            this.Idle.Name = "Idle";
            this.Idle.ReadOnly = true;
            // 
            // CST_ID
            // 
            this.CST_ID.HeaderText = "   STEP";
            this.CST_ID.MinimumWidth = 8;
            this.CST_ID.Name = "CST_ID";
            this.CST_ID.ReadOnly = true;
            // 
            // Manual
            // 
            this.Manual.HeaderText = "   CST ID";
            this.Manual.MinimumWidth = 8;
            this.Manual.Name = "Manual";
            this.Manual.ReadOnly = true;
            // 
            // UserCVStatusdg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.dgCvStatusView);
            this.Margin = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.Name = "UserCVStatusdg";
            this.Size = new System.Drawing.Size(947, 66);
            ((System.ComponentModel.ISupportInitialize)(this.dgCvStatusView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgCvStatusView;
        private System.Windows.Forms.DataGridViewTextBoxColumn CnvId;
        private System.Windows.Forms.DataGridViewTextBoxColumn CnvType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CnvHome;
        private System.Windows.Forms.DataGridViewTextBoxColumn CVStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Auto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Idle;
        private System.Windows.Forms.DataGridViewTextBoxColumn CST_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Manual;
    }
}
