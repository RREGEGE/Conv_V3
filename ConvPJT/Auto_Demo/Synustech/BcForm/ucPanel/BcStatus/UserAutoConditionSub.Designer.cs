namespace Synustech.ucPanel
{
    partial class UserAutoConditionSub
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
            this.Tb_Main_Auto_Status = new System.Windows.Forms.TableLayoutPanel();
            this.grid_Auto = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Condition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblAutoName = new System.Windows.Forms.Label();
            this.UI_Update_Timer = new System.Windows.Forms.Timer(this.components);
            this.Tb_Main_Auto_Status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Auto)).BeginInit();
            this.SuspendLayout();
            // 
            // Tb_Main_Auto_Status
            // 
            this.Tb_Main_Auto_Status.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.Tb_Main_Auto_Status.ColumnCount = 1;
            this.Tb_Main_Auto_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Tb_Main_Auto_Status.Controls.Add(this.grid_Auto, 0, 1);
            this.Tb_Main_Auto_Status.Controls.Add(this.lblAutoName, 0, 0);
            this.Tb_Main_Auto_Status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_Main_Auto_Status.Location = new System.Drawing.Point(0, 0);
            this.Tb_Main_Auto_Status.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Tb_Main_Auto_Status.Name = "Tb_Main_Auto_Status";
            this.Tb_Main_Auto_Status.RowCount = 2;
            this.Tb_Main_Auto_Status.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.Tb_Main_Auto_Status.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Tb_Main_Auto_Status.Size = new System.Drawing.Size(517, 544);
            this.Tb_Main_Auto_Status.TabIndex = 1;
            // 
            // grid_Auto
            // 
            this.grid_Auto.AllowUserToAddRows = false;
            this.grid_Auto.AllowUserToDeleteRows = false;
            this.grid_Auto.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grid_Auto.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.grid_Auto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid_Auto.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_Auto.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid_Auto.ColumnHeadersHeight = 30;
            this.grid_Auto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grid_Auto.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.Condition,
            this.Status});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid_Auto.DefaultCellStyle = dataGridViewCellStyle2;
            this.grid_Auto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_Auto.EnableHeadersVisualStyles = false;
            this.grid_Auto.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.grid_Auto.Location = new System.Drawing.Point(3, 48);
            this.grid_Auto.MultiSelect = false;
            this.grid_Auto.Name = "grid_Auto";
            this.grid_Auto.ReadOnly = true;
            this.grid_Auto.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_Auto.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grid_Auto.RowHeadersVisible = false;
            this.grid_Auto.RowHeadersWidth = 62;
            this.grid_Auto.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grid_Auto.RowTemplate.Height = 35;
            this.grid_Auto.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_Auto.Size = new System.Drawing.Size(511, 493);
            this.grid_Auto.TabIndex = 5;
            this.grid_Auto.SelectionChanged += new System.EventHandler(this.grid_Auto_SelectionChanged);
            // 
            // No
            // 
            this.No.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.No.FillWeight = 50.30265F;
            this.No.HeaderText = " No";
            this.No.MinimumWidth = 8;
            this.No.Name = "No";
            this.No.ReadOnly = true;
            // 
            // Condition
            // 
            this.Condition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Condition.FillWeight = 215.4709F;
            this.Condition.HeaderText = "    CONDITION";
            this.Condition.MinimumWidth = 8;
            this.Condition.Name = "Condition";
            this.Condition.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Status.FillWeight = 90.90908F;
            this.Status.HeaderText = "    STATUS";
            this.Status.MinimumWidth = 8;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 159;
            // 
            // lblAutoName
            // 
            this.lblAutoName.AutoSize = true;
            this.lblAutoName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoName.ForeColor = System.Drawing.Color.White;
            this.lblAutoName.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblAutoName.Location = new System.Drawing.Point(26, 8);
            this.lblAutoName.Margin = new System.Windows.Forms.Padding(26, 8, 3, 0);
            this.lblAutoName.Name = "lblAutoName";
            this.lblAutoName.Size = new System.Drawing.Size(268, 33);
            this.lblAutoName.TabIndex = 0;
            this.lblAutoName.Text = "AUTO CONDITION";
            // 
            // UI_Update_Timer
            // 
            this.UI_Update_Timer.Interval = 300;
            this.UI_Update_Timer.Tick += new System.EventHandler(this.UI_Update_Timer_Tick);
            // 
            // UserAutoConditionSub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Tb_Main_Auto_Status);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "UserAutoConditionSub";
            this.Size = new System.Drawing.Size(517, 544);
            this.Tb_Main_Auto_Status.ResumeLayout(false);
            this.Tb_Main_Auto_Status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Auto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Tb_Main_Auto_Status;
        private System.Windows.Forms.Label lblAutoName;
        private System.Windows.Forms.Timer UI_Update_Timer;
        private System.Windows.Forms.DataGridView grid_Auto;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Condition;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}
