namespace Synustech.ucPanel.BcStatus
{
    partial class UserOutput
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblOutputName = new System.Windows.Forms.Label();
            this.tlpOutput = new System.Windows.Forms.TableLayoutPanel();
            this.dgOutput = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutputName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Output_Update_Timer = new System.Windows.Forms.Timer(this.components);
            this.tlpOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // lblOutputName
            // 
            this.lblOutputName.AutoSize = true;
            this.lblOutputName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblOutputName.ForeColor = System.Drawing.Color.White;
            this.lblOutputName.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblOutputName.Location = new System.Drawing.Point(14, 8);
            this.lblOutputName.Margin = new System.Windows.Forms.Padding(14, 8, 3, 0);
            this.lblOutputName.Name = "lblOutputName";
            this.lblOutputName.Size = new System.Drawing.Size(135, 32);
            this.lblOutputName.TabIndex = 1;
            this.lblOutputName.Text = "OUTPUT";
            // 
            // tlpOutput
            // 
            this.tlpOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tlpOutput.ColumnCount = 1;
            this.tlpOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpOutput.Controls.Add(this.dgOutput, 0, 1);
            this.tlpOutput.Controls.Add(this.lblOutputName, 0, 0);
            this.tlpOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpOutput.Location = new System.Drawing.Point(0, 0);
            this.tlpOutput.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tlpOutput.Name = "tlpOutput";
            this.tlpOutput.RowCount = 2;
            this.tlpOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tlpOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92F));
            this.tlpOutput.Size = new System.Drawing.Size(870, 722);
            this.tlpOutput.TabIndex = 1;
            // 
            // dgOutput
            // 
            this.dgOutput.AllowUserToAddRows = false;
            this.dgOutput.AllowUserToDeleteRows = false;
            this.dgOutput.AllowUserToResizeColumns = false;
            this.dgOutput.AllowUserToResizeRows = false;
            this.dgOutput.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgOutput.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.dgOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgOutput.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgOutput.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgOutput.ColumnHeadersHeight = 30;
            this.dgOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgOutput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.Address,
            this.OutputName,
            this.Status});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgOutput.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgOutput.EnableHeadersVisualStyles = false;
            this.dgOutput.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.dgOutput.Location = new System.Drawing.Point(3, 60);
            this.dgOutput.MultiSelect = false;
            this.dgOutput.Name = "dgOutput";
            this.dgOutput.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgOutput.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgOutput.RowHeadersVisible = false;
            this.dgOutput.RowHeadersWidth = 62;
            this.dgOutput.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgOutput.RowTemplate.Height = 35;
            this.dgOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgOutput.Size = new System.Drawing.Size(864, 659);
            this.dgOutput.TabIndex = 4;
            this.dgOutput.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgOutput_CellClick);
            this.dgOutput.SelectionChanged += new System.EventHandler(this.dgOutput_SelectionChanged);
            // 
            // No
            // 
            this.No.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.No.FillWeight = 52.27272F;
            this.No.HeaderText = " No";
            this.No.MinimumWidth = 8;
            this.No.Name = "No";
            this.No.ReadOnly = true;
            // 
            // Address
            // 
            this.Address.FillWeight = 43.07327F;
            this.Address.HeaderText = " Address";
            this.Address.MinimumWidth = 8;
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            // 
            // OutputName
            // 
            this.OutputName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OutputName.FillWeight = 214.257F;
            this.OutputName.HeaderText = "    OUTPUT NAME";
            this.OutputName.MinimumWidth = 8;
            this.OutputName.Name = "OutputName";
            this.OutputName.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.FillWeight = 90.39693F;
            this.Status.HeaderText = "    STATUS";
            this.Status.MinimumWidth = 8;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // Output_Update_Timer
            // 
            this.Output_Update_Timer.Interval = 400;
            this.Output_Update_Timer.Tick += new System.EventHandler(this.Output_Update_Timer_Tick);
            // 
            // UserOuput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpOutput);
            this.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Name = "UserOuput";
            this.Size = new System.Drawing.Size(870, 722);
            this.tlpOutput.ResumeLayout(false);
            this.tlpOutput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOutput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblOutputName;
        private System.Windows.Forms.TableLayoutPanel tlpOutput;
        private System.Windows.Forms.Timer Output_Update_Timer;
        private System.Windows.Forms.DataGridView dgOutput;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutputName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}
