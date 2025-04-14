namespace Synustech.BcForm.ucPanel.BcSetting
{
    partial class Watchdog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlWatchdogBase = new System.Windows.Forms.TableLayoutPanel();
            this.dgWatchdogView = new System.Windows.Forms.DataGridView();
            this.Wt_Applied_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Wt_Set_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblWatchdogName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnWatchdogLoad = new System.Windows.Forms.Button();
            this.btnWatchdogSave = new System.Windows.Forms.Button();
            this.tlWatchdogBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWatchdogView)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlWatchdogBase
            // 
            this.tlWatchdogBase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tlWatchdogBase.ColumnCount = 1;
            this.tlWatchdogBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlWatchdogBase.Controls.Add(this.dgWatchdogView, 0, 1);
            this.tlWatchdogBase.Controls.Add(this.lblWatchdogName, 0, 0);
            this.tlWatchdogBase.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.tlWatchdogBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlWatchdogBase.Location = new System.Drawing.Point(0, 0);
            this.tlWatchdogBase.Name = "tlWatchdogBase";
            this.tlWatchdogBase.RowCount = 3;
            this.tlWatchdogBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlWatchdogBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tlWatchdogBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlWatchdogBase.Size = new System.Drawing.Size(800, 373);
            this.tlWatchdogBase.TabIndex = 0;
            // 
            // dgWatchdogView
            // 
            this.dgWatchdogView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.dgWatchdogView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgWatchdogView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgWatchdogView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgWatchdogView.ColumnHeadersHeight = 30;
            this.dgWatchdogView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgWatchdogView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Wt_Applied_Value,
            this.Wt_Set_Value});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgWatchdogView.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgWatchdogView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgWatchdogView.EnableHeadersVisualStyles = false;
            this.dgWatchdogView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgWatchdogView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.dgWatchdogView.Location = new System.Drawing.Point(0, 37);
            this.dgWatchdogView.Margin = new System.Windows.Forms.Padding(0);
            this.dgWatchdogView.Name = "dgWatchdogView";
            this.dgWatchdogView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgWatchdogView.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgWatchdogView.RowHeadersWidth = 200;
            this.dgWatchdogView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgWatchdogView.RowTemplate.Height = 40;
            this.dgWatchdogView.RowTemplate.ReadOnly = true;
            this.dgWatchdogView.Size = new System.Drawing.Size(800, 261);
            this.dgWatchdogView.TabIndex = 11;
            // 
            // Wt_Applied_Value
            // 
            this.Wt_Applied_Value.HeaderText = "  Applied Value";
            this.Wt_Applied_Value.MinimumWidth = 8;
            this.Wt_Applied_Value.Name = "Wt_Applied_Value";
            this.Wt_Applied_Value.ReadOnly = true;
            this.Wt_Applied_Value.Width = 116;
            // 
            // Wt_Set_Value
            // 
            this.Wt_Set_Value.HeaderText = "   Set Value";
            this.Wt_Set_Value.MinimumWidth = 8;
            this.Wt_Set_Value.Name = "Wt_Set_Value";
            this.Wt_Set_Value.ReadOnly = true;
            this.Wt_Set_Value.Width = 118;
            // 
            // lblWatchdogName
            // 
            this.lblWatchdogName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblWatchdogName.AutoSize = true;
            this.lblWatchdogName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblWatchdogName.ForeColor = System.Drawing.Color.White;
            this.lblWatchdogName.Location = new System.Drawing.Point(10, 13);
            this.lblWatchdogName.Margin = new System.Windows.Forms.Padding(10, 5, 3, 0);
            this.lblWatchdogName.Name = "lblWatchdogName";
            this.lblWatchdogName.Size = new System.Drawing.Size(130, 24);
            this.lblWatchdogName.TabIndex = 10;
            this.lblWatchdogName.Text = "WATCHDOG";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Controls.Add(this.btnWatchdogLoad, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnWatchdogSave, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 299);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(798, 73);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // btnWatchdogLoad
            // 
            this.btnWatchdogLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btnWatchdogLoad.BackgroundImage = global::Synustech.Properties.Resources.Refresh_Re;
            this.btnWatchdogLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnWatchdogLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWatchdogLoad.FlatAppearance.BorderSize = 0;
            this.btnWatchdogLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWatchdogLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnWatchdogLoad.ForeColor = System.Drawing.Color.White;
            this.btnWatchdogLoad.Location = new System.Drawing.Point(679, 2);
            this.btnWatchdogLoad.Margin = new System.Windows.Forms.Padding(2);
            this.btnWatchdogLoad.Name = "btnWatchdogLoad";
            this.btnWatchdogLoad.Size = new System.Drawing.Size(117, 69);
            this.btnWatchdogLoad.TabIndex = 8;
            this.btnWatchdogLoad.UseVisualStyleBackColor = false;
            // 
            // btnWatchdogSave
            // 
            this.btnWatchdogSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btnWatchdogSave.BackgroundImage = global::Synustech.Properties.Resources.Download_Re;
            this.btnWatchdogSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnWatchdogSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWatchdogSave.FlatAppearance.BorderSize = 0;
            this.btnWatchdogSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWatchdogSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnWatchdogSave.ForeColor = System.Drawing.Color.White;
            this.btnWatchdogSave.Location = new System.Drawing.Point(560, 2);
            this.btnWatchdogSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnWatchdogSave.Name = "btnWatchdogSave";
            this.btnWatchdogSave.Size = new System.Drawing.Size(115, 69);
            this.btnWatchdogSave.TabIndex = 7;
            this.btnWatchdogSave.UseVisualStyleBackColor = false;
            // 
            // Watchdog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlWatchdogBase);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "Watchdog";
            this.Size = new System.Drawing.Size(800, 373);
            this.tlWatchdogBase.ResumeLayout(false);
            this.tlWatchdogBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWatchdogView)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlWatchdogBase;
        private System.Windows.Forms.Label lblWatchdogName;
        private System.Windows.Forms.DataGridView dgWatchdogView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Wt_Applied_Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Wt_Set_Value;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnWatchdogSave;
        private System.Windows.Forms.Button btnWatchdogLoad;
    }
}
