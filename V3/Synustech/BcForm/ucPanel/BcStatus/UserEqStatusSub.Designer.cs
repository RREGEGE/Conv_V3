namespace Synustech.ucPanel
{
    partial class UserEqStatusSub
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
            this.Tb_Main_Cv_Auto = new System.Windows.Forms.Panel();
            this.tlp_Conv_Auto_Status = new System.Windows.Forms.TableLayoutPanel();
            this.grid_Conv = new System.Windows.Forms.DataGridView();
            this.lblConvName = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.Tb_Main_Cv_Auto.SuspendLayout();
            this.tlp_Conv_Auto_Status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Conv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // Tb_Main_Cv_Auto
            // 
            this.Tb_Main_Cv_Auto.Controls.Add(this.tlp_Conv_Auto_Status);
            this.Tb_Main_Cv_Auto.Controls.Add(this.dataGridView2);
            this.Tb_Main_Cv_Auto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_Main_Cv_Auto.Location = new System.Drawing.Point(0, 0);
            this.Tb_Main_Cv_Auto.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Tb_Main_Cv_Auto.Name = "Tb_Main_Cv_Auto";
            this.Tb_Main_Cv_Auto.Size = new System.Drawing.Size(616, 608);
            this.Tb_Main_Cv_Auto.TabIndex = 0;
            // 
            // tlp_Conv_Auto_Status
            // 
            this.tlp_Conv_Auto_Status.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tlp_Conv_Auto_Status.ColumnCount = 1;
            this.tlp_Conv_Auto_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Conv_Auto_Status.Controls.Add(this.grid_Conv, 0, 1);
            this.tlp_Conv_Auto_Status.Controls.Add(this.lblConvName, 0, 0);
            this.tlp_Conv_Auto_Status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Conv_Auto_Status.Location = new System.Drawing.Point(0, 0);
            this.tlp_Conv_Auto_Status.Name = "tlp_Conv_Auto_Status";
            this.tlp_Conv_Auto_Status.RowCount = 2;
            this.tlp_Conv_Auto_Status.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlp_Conv_Auto_Status.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Conv_Auto_Status.Size = new System.Drawing.Size(616, 608);
            this.tlp_Conv_Auto_Status.TabIndex = 3;
            // 
            // grid_Conv
            // 
            this.grid_Conv.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.grid_Conv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid_Conv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.grid_Conv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Conv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_Conv.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.grid_Conv.Location = new System.Drawing.Point(4, 49);
            this.grid_Conv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grid_Conv.Name = "grid_Conv";
            this.grid_Conv.RowHeadersWidth = 62;
            this.grid_Conv.RowTemplate.Height = 30;
            this.grid_Conv.Size = new System.Drawing.Size(608, 555);
            this.grid_Conv.TabIndex = 0;
            // 
            // lblConvName
            // 
            this.lblConvName.AutoSize = true;
            this.lblConvName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConvName.ForeColor = System.Drawing.Color.White;
            this.lblConvName.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblConvName.Location = new System.Drawing.Point(26, 8);
            this.lblConvName.Margin = new System.Windows.Forms.Padding(26, 8, 3, 0);
            this.lblConvName.Name = "lblConvName";
            this.lblConvName.Size = new System.Drawing.Size(287, 33);
            this.lblConvName.TabIndex = 0;
            this.lblConvName.Text = "PROGRAM STATUS";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(614, 45);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 62;
            this.dataGridView2.RowTemplate.Height = 30;
            this.dataGridView2.Size = new System.Drawing.Size(9, 8);
            this.dataGridView2.TabIndex = 2;
            // 
            // UserEqStatusSub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Tb_Main_Cv_Auto);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "UserEqStatusSub";
            this.Size = new System.Drawing.Size(616, 608);
            this.Tb_Main_Cv_Auto.ResumeLayout(false);
            this.tlp_Conv_Auto_Status.ResumeLayout(false);
            this.tlp_Conv_Auto_Status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Conv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Tb_Main_Cv_Auto;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.TableLayoutPanel tlp_Conv_Auto_Status;
        private System.Windows.Forms.DataGridView grid_Conv;
        private System.Windows.Forms.Label lblConvName;
    }
}
