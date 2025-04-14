namespace Synustech.BcForm.ucPanel.BcSetting
{
    partial class Parameter
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblParameterName = new System.Windows.Forms.Label();
            this.tb_Base = new System.Windows.Forms.TableLayoutPanel();
            this.dgParaView = new System.Windows.Forms.DataGridView();
            this.None = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tb_Button = new System.Windows.Forms.TableLayoutPanel();
            this.btnParaLoad = new System.Windows.Forms.Button();
            this.btnParaSave = new System.Windows.Forms.Button();
            this.plBase = new System.Windows.Forms.Panel();
            this.Header_Update_Timer = new System.Windows.Forms.Timer(this.components);
            this.tb_Base.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgParaView)).BeginInit();
            this.tb_Button.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblParameterName
            // 
            this.lblParameterName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblParameterName.AutoSize = true;
            this.lblParameterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblParameterName.ForeColor = System.Drawing.Color.White;
            this.lblParameterName.Location = new System.Drawing.Point(14, 24);
            this.lblParameterName.Margin = new System.Windows.Forms.Padding(14, 8, 4, 0);
            this.lblParameterName.Name = "lblParameterName";
            this.lblParameterName.Size = new System.Drawing.Size(198, 32);
            this.lblParameterName.TabIndex = 9;
            this.lblParameterName.Text = "PARAMETER";
            // 
            // tb_Base
            // 
            this.tb_Base.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tb_Base.ColumnCount = 1;
            this.tb_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tb_Base.Controls.Add(this.dgParaView, 0, 1);
            this.tb_Base.Controls.Add(this.lblParameterName, 0, 0);
            this.tb_Base.Controls.Add(this.tb_Button, 0, 2);
            this.tb_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_Base.Location = new System.Drawing.Point(0, 0);
            this.tb_Base.Margin = new System.Windows.Forms.Padding(0);
            this.tb_Base.Name = "tb_Base";
            this.tb_Base.RowCount = 3;
            this.tb_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tb_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tb_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tb_Base.Size = new System.Drawing.Size(1143, 560);
            this.tb_Base.TabIndex = 1;
            this.tb_Base.Paint += new System.Windows.Forms.PaintEventHandler(this.tb_Base_Paint);
            // 
            // dgParaView
            // 
            this.dgParaView.AllowUserToAddRows = false;
            this.dgParaView.AllowUserToDeleteRows = false;
            this.dgParaView.AllowUserToResizeColumns = false;
            this.dgParaView.AllowUserToResizeRows = false;
            this.dgParaView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.dgParaView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgParaView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgParaView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgParaView.ColumnHeadersHeight = 45;
            this.dgParaView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgParaView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.None});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgParaView.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgParaView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgParaView.EnableHeadersVisualStyles = false;
            this.dgParaView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgParaView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.dgParaView.Location = new System.Drawing.Point(0, 56);
            this.dgParaView.Margin = new System.Windows.Forms.Padding(0);
            this.dgParaView.Name = "dgParaView";
            this.dgParaView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgParaView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgParaView.RowHeadersWidth = 240;
            this.dgParaView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgParaView.RowTemplate.Height = 35;
            this.dgParaView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgParaView.Size = new System.Drawing.Size(1143, 392);
            this.dgParaView.TabIndex = 10;
            // 
            // None
            // 
            this.None.HeaderText = "None";
            this.None.MinimumWidth = 8;
            this.None.Name = "None";
            this.None.Width = 300;
            // 
            // tb_Button
            // 
            this.tb_Button.ColumnCount = 3;
            this.tb_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tb_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tb_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tb_Button.Controls.Add(this.btnParaLoad, 2, 0);
            this.tb_Button.Controls.Add(this.btnParaSave, 1, 0);
            this.tb_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_Button.Location = new System.Drawing.Point(1, 450);
            this.tb_Button.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.tb_Button.Name = "tb_Button";
            this.tb_Button.RowCount = 1;
            this.tb_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tb_Button.Size = new System.Drawing.Size(1141, 108);
            this.tb_Button.TabIndex = 7;
            // 
            // btnParaLoad
            // 
            this.btnParaLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btnParaLoad.BackgroundImage = global::Synustech.Properties.Resources.Refresh_Re;
            this.btnParaLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnParaLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnParaLoad.FlatAppearance.BorderSize = 0;
            this.btnParaLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParaLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnParaLoad.ForeColor = System.Drawing.Color.White;
            this.btnParaLoad.Location = new System.Drawing.Point(1029, 3);
            this.btnParaLoad.Name = "btnParaLoad";
            this.btnParaLoad.Size = new System.Drawing.Size(109, 102);
            this.btnParaLoad.TabIndex = 4;
            this.btnParaLoad.UseVisualStyleBackColor = false;
            this.btnParaLoad.Click += new System.EventHandler(this.btnParaLoad_Click);
            // 
            // btnParaSave
            // 
            this.btnParaSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btnParaSave.BackgroundImage = global::Synustech.Properties.Resources.Download_Re;
            this.btnParaSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnParaSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnParaSave.FlatAppearance.BorderSize = 0;
            this.btnParaSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParaSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnParaSave.ForeColor = System.Drawing.Color.White;
            this.btnParaSave.Location = new System.Drawing.Point(915, 3);
            this.btnParaSave.Name = "btnParaSave";
            this.btnParaSave.Size = new System.Drawing.Size(108, 102);
            this.btnParaSave.TabIndex = 6;
            this.btnParaSave.UseVisualStyleBackColor = false;
            this.btnParaSave.Click += new System.EventHandler(this.btnParaSave_Click);
            // 
            // plBase
            // 
            this.plBase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.plBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plBase.Location = new System.Drawing.Point(0, 0);
            this.plBase.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.plBase.Name = "plBase";
            this.plBase.Size = new System.Drawing.Size(1143, 560);
            this.plBase.TabIndex = 2;
            // 
            // Header_Update_Timer
            // 
            this.Header_Update_Timer.Interval = 1000;
            this.Header_Update_Timer.Tick += new System.EventHandler(this.Header_Update_Timer_Tick);
            // 
            // Parameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tb_Base);
            this.Controls.Add(this.plBase);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "Parameter";
            this.Size = new System.Drawing.Size(1143, 560);
            this.tb_Base.ResumeLayout(false);
            this.tb_Base.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgParaView)).EndInit();
            this.tb_Button.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblParameterName;
        private System.Windows.Forms.TableLayoutPanel tb_Base;
        private System.Windows.Forms.TableLayoutPanel tb_Button;
        private System.Windows.Forms.Button btnParaLoad;
        private System.Windows.Forms.Button btnParaSave;
        private System.Windows.Forms.Panel plBase;
        private System.Windows.Forms.DataGridView dgParaView;
        public System.Windows.Forms.Timer Header_Update_Timer;
        private System.Windows.Forms.DataGridViewTextBoxColumn None;
    }
}
