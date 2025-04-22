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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlWatchdogBase = new System.Windows.Forms.TableLayoutPanel();
            this.lblWatchdogName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnWatchdogLoad = new System.Windows.Forms.Button();
            this.btnWatchdogSave = new System.Windows.Forms.Button();
            this.tPanelLine = new System.Windows.Forms.TableLayoutPanel();
            this.dgWatchdogView = new System.Windows.Forms.DataGridView();
            this.tlWatchdogBase.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWatchdogView)).BeginInit();
            this.SuspendLayout();
            // 
            // tlWatchdogBase
            // 
            this.tlWatchdogBase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tlWatchdogBase.ColumnCount = 1;
            this.tlWatchdogBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlWatchdogBase.Controls.Add(this.dgWatchdogView, 0, 2);
            this.tlWatchdogBase.Controls.Add(this.lblWatchdogName, 0, 0);
            this.tlWatchdogBase.Controls.Add(this.tableLayoutPanel1, 0, 3);
            this.tlWatchdogBase.Controls.Add(this.tPanelLine, 0, 1);
            this.tlWatchdogBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlWatchdogBase.Location = new System.Drawing.Point(0, 0);
            this.tlWatchdogBase.Margin = new System.Windows.Forms.Padding(4);
            this.tlWatchdogBase.Name = "tlWatchdogBase";
            this.tlWatchdogBase.RowCount = 4;
            this.tlWatchdogBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlWatchdogBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tlWatchdogBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.tlWatchdogBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlWatchdogBase.Size = new System.Drawing.Size(1143, 560);
            this.tlWatchdogBase.TabIndex = 0;
            // 
            // lblWatchdogName
            // 
            this.lblWatchdogName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblWatchdogName.AutoSize = true;
            this.lblWatchdogName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblWatchdogName.ForeColor = System.Drawing.Color.White;
            this.lblWatchdogName.Location = new System.Drawing.Point(14, 24);
            this.lblWatchdogName.Margin = new System.Windows.Forms.Padding(14, 8, 4, 0);
            this.lblWatchdogName.Name = "lblWatchdogName";
            this.lblWatchdogName.Size = new System.Drawing.Size(188, 32);
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 449);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1141, 109);
            this.tableLayoutPanel1.TabIndex = 11;
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
            this.btnWatchdogLoad.Location = new System.Drawing.Point(972, 3);
            this.btnWatchdogLoad.Name = "btnWatchdogLoad";
            this.btnWatchdogLoad.Size = new System.Drawing.Size(166, 103);
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
            this.btnWatchdogSave.Location = new System.Drawing.Point(801, 3);
            this.btnWatchdogSave.Name = "btnWatchdogSave";
            this.btnWatchdogSave.Size = new System.Drawing.Size(165, 103);
            this.btnWatchdogSave.TabIndex = 7;
            this.btnWatchdogSave.UseVisualStyleBackColor = false;
            // 
            // tPanelLine
            // 
            this.tPanelLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(60)))));
            this.tPanelLine.ColumnCount = 10;
            this.tPanelLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tPanelLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tPanelLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tPanelLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tPanelLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tPanelLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tPanelLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tPanelLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tPanelLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tPanelLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tPanelLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tPanelLine.Location = new System.Drawing.Point(3, 59);
            this.tPanelLine.Name = "tPanelLine";
            this.tPanelLine.RowCount = 1;
            this.tPanelLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tPanelLine.Size = new System.Drawing.Size(1137, 38);
            this.tPanelLine.TabIndex = 12;
            // 
            // dgWatchdogView
            // 
            this.dgWatchdogView.AllowUserToAddRows = false;
            this.dgWatchdogView.AllowUserToDeleteRows = false;
            this.dgWatchdogView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgWatchdogView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.dgWatchdogView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgWatchdogView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgWatchdogView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgWatchdogView.ColumnHeadersHeight = 30;
            this.dgWatchdogView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgWatchdogView.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgWatchdogView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgWatchdogView.EnableHeadersVisualStyles = false;
            this.dgWatchdogView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgWatchdogView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.dgWatchdogView.Location = new System.Drawing.Point(0, 100);
            this.dgWatchdogView.Margin = new System.Windows.Forms.Padding(0);
            this.dgWatchdogView.Name = "dgWatchdogView";
            this.dgWatchdogView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgWatchdogView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgWatchdogView.RowHeadersVisible = false;
            this.dgWatchdogView.RowHeadersWidth = 200;
            this.dgWatchdogView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgWatchdogView.RowTemplate.Height = 40;
            this.dgWatchdogView.RowTemplate.ReadOnly = true;
            this.dgWatchdogView.Size = new System.Drawing.Size(1143, 347);
            this.dgWatchdogView.TabIndex = 13;
            // 
            // Watchdog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlWatchdogBase);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "Watchdog";
            this.Size = new System.Drawing.Size(1143, 560);
            this.tlWatchdogBase.ResumeLayout(false);
            this.tlWatchdogBase.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgWatchdogView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlWatchdogBase;
        private System.Windows.Forms.Label lblWatchdogName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnWatchdogSave;
        private System.Windows.Forms.Button btnWatchdogLoad;
        private System.Windows.Forms.TableLayoutPanel tPanelLine;
        private System.Windows.Forms.DataGridView dgWatchdogView;
    }
}
