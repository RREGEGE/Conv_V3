namespace Synustech.ucPanel
{
    partial class UserAlarmHistory
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.UcAlarmPnl = new System.Windows.Forms.Panel();
            this.tlp_Alarm_History = new System.Windows.Forms.TableLayoutPanel();
            this.lblAlarmName = new System.Windows.Forms.Label();
            this.dgView = new System.Windows.Forms.DataGridView();
            this.UcAlarmPnl.SuspendLayout();
            this.tlp_Alarm_History.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).BeginInit();
            this.SuspendLayout();
            // 
            // UcAlarmPnl
            // 
            this.UcAlarmPnl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.UcAlarmPnl.Controls.Add(this.tlp_Alarm_History);
            this.UcAlarmPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UcAlarmPnl.Location = new System.Drawing.Point(0, 0);
            this.UcAlarmPnl.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.UcAlarmPnl.Name = "UcAlarmPnl";
            this.UcAlarmPnl.Size = new System.Drawing.Size(530, 675);
            this.UcAlarmPnl.TabIndex = 4;
            // 
            // tlp_Alarm_History
            // 
            this.tlp_Alarm_History.ColumnCount = 1;
            this.tlp_Alarm_History.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Alarm_History.Controls.Add(this.lblAlarmName, 0, 0);
            this.tlp_Alarm_History.Controls.Add(this.dgView, 0, 1);
            this.tlp_Alarm_History.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Alarm_History.Location = new System.Drawing.Point(0, 0);
            this.tlp_Alarm_History.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.tlp_Alarm_History.Name = "tlp_Alarm_History";
            this.tlp_Alarm_History.RowCount = 2;
            this.tlp_Alarm_History.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.555555F));
            this.tlp_Alarm_History.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.44444F));
            this.tlp_Alarm_History.Size = new System.Drawing.Size(530, 675);
            this.tlp_Alarm_History.TabIndex = 5;
            // 
            // lblAlarmName
            // 
            this.lblAlarmName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAlarmName.AutoSize = true;
            this.lblAlarmName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlarmName.ForeColor = System.Drawing.Color.White;
            this.lblAlarmName.Location = new System.Drawing.Point(14, 16);
            this.lblAlarmName.Margin = new System.Windows.Forms.Padding(14, 8, 4, 0);
            this.lblAlarmName.Name = "lblAlarmName";
            this.lblAlarmName.Size = new System.Drawing.Size(205, 26);
            this.lblAlarmName.TabIndex = 3;
            this.lblAlarmName.Text = "ALARM HISTORY";
            // 
            // dgView
            // 
            this.dgView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.dgView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.dgView.Location = new System.Drawing.Point(1, 52);
            this.dgView.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.dgView.MultiSelect = false;
            this.dgView.Name = "dgView";
            this.dgView.ReadOnly = true;
            this.dgView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgView.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgView.RowHeadersWidth = 62;
            this.dgView.RowTemplate.Height = 30;
            this.dgView.Size = new System.Drawing.Size(528, 621);
            this.dgView.TabIndex = 0;
            // 
            // UserAlarmHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UcAlarmPnl);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "UserAlarmHistory";
            this.Size = new System.Drawing.Size(530, 675);
            this.UcAlarmPnl.ResumeLayout(false);
            this.tlp_Alarm_History.ResumeLayout(false);
            this.tlp_Alarm_History.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel UcAlarmPnl;
        private System.Windows.Forms.Label lblAlarmName;
        private System.Windows.Forms.TableLayoutPanel tlp_Alarm_History;
        private System.Windows.Forms.DataGridView dgView;
    }
}
