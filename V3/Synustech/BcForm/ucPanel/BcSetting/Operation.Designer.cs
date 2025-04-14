namespace Synustech.BcForm.ucPanel.BcSetting
{
    partial class Operation
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
            this.tb_Op_Base = new System.Windows.Forms.TableLayoutPanel();
            this.lblOperationName = new System.Windows.Forms.Label();
            this.dgOperationView = new System.Windows.Forms.DataGridView();
            this.Op_Applide_Val = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Op_Set_Val = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tb_Op_Button = new System.Windows.Forms.TableLayoutPanel();
            this.btnOperationSave = new System.Windows.Forms.Button();
            this.btnOperationLoad = new System.Windows.Forms.Button();
            this.pl_Main_Base = new System.Windows.Forms.Panel();
            this.tb_Op_Base.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOperationView)).BeginInit();
            this.tb_Op_Button.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_Op_Base
            // 
            this.tb_Op_Base.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tb_Op_Base.ColumnCount = 1;
            this.tb_Op_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tb_Op_Base.Controls.Add(this.lblOperationName, 0, 0);
            this.tb_Op_Base.Controls.Add(this.dgOperationView, 0, 1);
            this.tb_Op_Base.Controls.Add(this.tb_Op_Button, 0, 2);
            this.tb_Op_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_Op_Base.Location = new System.Drawing.Point(0, 0);
            this.tb_Op_Base.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.tb_Op_Base.Name = "tb_Op_Base";
            this.tb_Op_Base.RowCount = 3;
            this.tb_Op_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tb_Op_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tb_Op_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tb_Op_Base.Size = new System.Drawing.Size(1143, 560);
            this.tb_Op_Base.TabIndex = 1;
            // 
            // lblOperationName
            // 
            this.lblOperationName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOperationName.AutoSize = true;
            this.lblOperationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblOperationName.ForeColor = System.Drawing.Color.White;
            this.lblOperationName.Location = new System.Drawing.Point(14, 24);
            this.lblOperationName.Margin = new System.Windows.Forms.Padding(14, 8, 4, 0);
            this.lblOperationName.Name = "lblOperationName";
            this.lblOperationName.Size = new System.Drawing.Size(188, 32);
            this.lblOperationName.TabIndex = 9;
            this.lblOperationName.Text = "OPERATION";
            // 
            // dgOperationView
            // 
            this.dgOperationView.AllowUserToAddRows = false;
            this.dgOperationView.AllowUserToDeleteRows = false;
            this.dgOperationView.AllowUserToResizeColumns = false;
            this.dgOperationView.AllowUserToResizeRows = false;
            this.dgOperationView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.dgOperationView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgOperationView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgOperationView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgOperationView.ColumnHeadersHeight = 45;
            this.dgOperationView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgOperationView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Op_Applide_Val,
            this.Op_Set_Val});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgOperationView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgOperationView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgOperationView.EnableHeadersVisualStyles = false;
            this.dgOperationView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgOperationView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.dgOperationView.Location = new System.Drawing.Point(0, 56);
            this.dgOperationView.Margin = new System.Windows.Forms.Padding(0);
            this.dgOperationView.Name = "dgOperationView";
            this.dgOperationView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgOperationView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgOperationView.RowHeadersWidth = 180;
            this.dgOperationView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgOperationView.RowTemplate.Height = 40;
            this.dgOperationView.Size = new System.Drawing.Size(1143, 392);
            this.dgOperationView.TabIndex = 8;
            // 
            // Op_Applide_Val
            // 
            this.Op_Applide_Val.HeaderText = "  Applied Value";
            this.Op_Applide_Val.MinimumWidth = 8;
            this.Op_Applide_Val.Name = "Op_Applide_Val";
            this.Op_Applide_Val.Width = 116;
            // 
            // Op_Set_Val
            // 
            this.Op_Set_Val.HeaderText = "   Set Value";
            this.Op_Set_Val.MinimumWidth = 8;
            this.Op_Set_Val.Name = "Op_Set_Val";
            this.Op_Set_Val.Width = 118;
            // 
            // tb_Op_Button
            // 
            this.tb_Op_Button.ColumnCount = 3;
            this.tb_Op_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tb_Op_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tb_Op_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tb_Op_Button.Controls.Add(this.btnOperationSave, 1, 0);
            this.tb_Op_Button.Controls.Add(this.btnOperationLoad, 2, 0);
            this.tb_Op_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_Op_Button.Location = new System.Drawing.Point(1, 450);
            this.tb_Op_Button.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.tb_Op_Button.Name = "tb_Op_Button";
            this.tb_Op_Button.RowCount = 1;
            this.tb_Op_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tb_Op_Button.Size = new System.Drawing.Size(1141, 108);
            this.tb_Op_Button.TabIndex = 7;
            // 
            // btnOperationSave
            // 
            this.btnOperationSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btnOperationSave.BackgroundImage = global::Synustech.Properties.Resources.Download_Re;
            this.btnOperationSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOperationSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOperationSave.FlatAppearance.BorderSize = 0;
            this.btnOperationSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOperationSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnOperationSave.ForeColor = System.Drawing.Color.White;
            this.btnOperationSave.Location = new System.Drawing.Point(801, 3);
            this.btnOperationSave.Name = "btnOperationSave";
            this.btnOperationSave.Size = new System.Drawing.Size(165, 102);
            this.btnOperationSave.TabIndex = 6;
            this.btnOperationSave.UseVisualStyleBackColor = false;
            this.btnOperationSave.Click += new System.EventHandler(this.btnOperationSave_Click);
            // 
            // btnOperationLoad
            // 
            this.btnOperationLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btnOperationLoad.BackgroundImage = global::Synustech.Properties.Resources.Refresh_Re;
            this.btnOperationLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOperationLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOperationLoad.FlatAppearance.BorderSize = 0;
            this.btnOperationLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOperationLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnOperationLoad.ForeColor = System.Drawing.Color.White;
            this.btnOperationLoad.Location = new System.Drawing.Point(972, 3);
            this.btnOperationLoad.Name = "btnOperationLoad";
            this.btnOperationLoad.Size = new System.Drawing.Size(166, 102);
            this.btnOperationLoad.TabIndex = 4;
            this.btnOperationLoad.UseVisualStyleBackColor = false;
            this.btnOperationLoad.Click += new System.EventHandler(this.btnOperationLoad_Click);
            // 
            // pl_Main_Base
            // 
            this.pl_Main_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pl_Main_Base.Location = new System.Drawing.Point(0, 0);
            this.pl_Main_Base.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.pl_Main_Base.Name = "pl_Main_Base";
            this.pl_Main_Base.Size = new System.Drawing.Size(1143, 560);
            this.pl_Main_Base.TabIndex = 2;
            // 
            // Operation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.tb_Op_Base);
            this.Controls.Add(this.pl_Main_Base);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "Operation";
            this.Size = new System.Drawing.Size(1143, 560);
            this.tb_Op_Base.ResumeLayout(false);
            this.tb_Op_Base.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOperationView)).EndInit();
            this.tb_Op_Button.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tb_Op_Base;
        private System.Windows.Forms.Label lblOperationName;
        private System.Windows.Forms.TableLayoutPanel tb_Op_Button;
        private System.Windows.Forms.Button btnOperationLoad;
        private System.Windows.Forms.Button btnOperationSave;
        private System.Windows.Forms.DataGridView dgOperationView;
        private System.Windows.Forms.Panel pl_Main_Base;
        private System.Windows.Forms.DataGridViewTextBoxColumn Op_Applide_Val;
        private System.Windows.Forms.DataGridViewTextBoxColumn Op_Set_Val;
    }
}
