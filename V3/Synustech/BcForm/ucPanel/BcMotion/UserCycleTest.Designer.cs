namespace Synustech.ucPanel.BcMotion
{
    partial class UserCycleTest
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
            this.btn_CountNum = new System.Windows.Forms.Button();
            this.btn_Manual = new System.Windows.Forms.Button();
            this.btn_Auto = new System.Windows.Forms.Button();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.btn_Run = new System.Windows.Forms.Button();
            this.tbl_Cycle_Control = new System.Windows.Forms.TableLayoutPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lbl_Cycle_Auto = new System.Windows.Forms.Label();
            this.lbl_Cycle = new System.Windows.Forms.Label();
            this.lbl_Cycle_Count = new System.Windows.Forms.Label();
            this.tbl_Cycle_Control.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_CountNum
            // 
            this.btn_CountNum.BackColor = System.Drawing.Color.White;
            this.btn_CountNum.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn_CountNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_CountNum.FlatAppearance.BorderSize = 0;
            this.btn_CountNum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CountNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CountNum.ForeColor = System.Drawing.Color.Black;
            this.btn_CountNum.Location = new System.Drawing.Point(420, 61);
            this.btn_CountNum.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_CountNum.Name = "btn_CountNum";
            this.btn_CountNum.Size = new System.Drawing.Size(187, 86);
            this.btn_CountNum.TabIndex = 18;
            this.btn_CountNum.Text = "count";
            this.btn_CountNum.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_CountNum.UseVisualStyleBackColor = false;
            this.btn_CountNum.Click += new System.EventHandler(this.btn_CountNum_Click);
            // 
            // btn_Manual
            // 
            this.btn_Manual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_Manual.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Manual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Manual.FlatAppearance.BorderSize = 0;
            this.btn_Manual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Manual.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Manual.ForeColor = System.Drawing.Color.DarkGray;
            this.btn_Manual.Location = new System.Drawing.Point(230, 61);
            this.btn_Manual.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Manual.Name = "btn_Manual";
            this.btn_Manual.Size = new System.Drawing.Size(187, 86);
            this.btn_Manual.TabIndex = 16;
            this.btn_Manual.Text = "Auto Stop";
            this.btn_Manual.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Manual.UseVisualStyleBackColor = false;
            // 
            // btn_Auto
            // 
            this.btn_Auto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_Auto.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Auto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Auto.FlatAppearance.BorderSize = 0;
            this.btn_Auto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Auto.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Auto.ForeColor = System.Drawing.Color.White;
            this.btn_Auto.Location = new System.Drawing.Point(40, 61);
            this.btn_Auto.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Auto.Name = "btn_Auto";
            this.btn_Auto.Size = new System.Drawing.Size(187, 86);
            this.btn_Auto.TabIndex = 16;
            this.btn_Auto.Text = "Auto Mode";
            this.btn_Auto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Auto.UseVisualStyleBackColor = false;
            // 
            // btn_Stop
            // 
            this.btn_Stop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_Stop.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Stop.FlatAppearance.BorderSize = 0;
            this.btn_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Stop.ForeColor = System.Drawing.Color.DarkGray;
            this.btn_Stop.Location = new System.Drawing.Point(800, 61);
            this.btn_Stop.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(190, 86);
            this.btn_Stop.TabIndex = 16;
            this.btn_Stop.Text = "Cycle Stop";
            this.btn_Stop.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Stop.UseVisualStyleBackColor = false;
            // 
            // btn_Run
            // 
            this.btn_Run.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_Run.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Run.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Run.FlatAppearance.BorderSize = 0;
            this.btn_Run.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Run.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Run.ForeColor = System.Drawing.Color.White;
            this.btn_Run.Location = new System.Drawing.Point(610, 61);
            this.btn_Run.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Run.Name = "btn_Run";
            this.btn_Run.Size = new System.Drawing.Size(187, 86);
            this.btn_Run.TabIndex = 16;
            this.btn_Run.Text = "Cycle Run";
            this.btn_Run.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Run.UseVisualStyleBackColor = false;
            // 
            // tbl_Cycle_Control
            // 
            this.tbl_Cycle_Control.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tbl_Cycle_Control.ColumnCount = 6;
            this.tbl_Cycle_Control.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tbl_Cycle_Control.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tbl_Cycle_Control.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tbl_Cycle_Control.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tbl_Cycle_Control.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tbl_Cycle_Control.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tbl_Cycle_Control.Controls.Add(this.lbl_Cycle_Count, 3, 0);
            this.tbl_Cycle_Control.Controls.Add(this.textBox1, 0, 0);
            this.tbl_Cycle_Control.Controls.Add(this.btn_Stop, 5, 1);
            this.tbl_Cycle_Control.Controls.Add(this.btn_Run, 4, 1);
            this.tbl_Cycle_Control.Controls.Add(this.btn_CountNum, 3, 1);
            this.tbl_Cycle_Control.Controls.Add(this.btn_Manual, 2, 1);
            this.tbl_Cycle_Control.Controls.Add(this.btn_Auto, 1, 1);
            this.tbl_Cycle_Control.Controls.Add(this.lbl_Cycle_Auto, 1, 0);
            this.tbl_Cycle_Control.Controls.Add(this.lbl_Cycle, 4, 0);
            this.tbl_Cycle_Control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbl_Cycle_Control.Location = new System.Drawing.Point(0, 0);
            this.tbl_Cycle_Control.Margin = new System.Windows.Forms.Padding(1);
            this.tbl_Cycle_Control.Name = "tbl_Cycle_Control";
            this.tbl_Cycle_Control.RowCount = 2;
            this.tbl_Cycle_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tbl_Cycle_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tbl_Cycle_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tbl_Cycle_Control.Size = new System.Drawing.Size(993, 149);
            this.tbl_Cycle_Control.TabIndex = 17;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(1, 1);
            this.textBox1.Margin = new System.Windows.Forms.Padding(1);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.tbl_Cycle_Control.SetRowSpan(this.textBox1, 2);
            this.textBox1.Size = new System.Drawing.Size(38, 147);
            this.textBox1.TabIndex = 34;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbl_Cycle_Auto
            // 
            this.lbl_Cycle_Auto.AutoSize = true;
            this.lbl_Cycle_Auto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.tbl_Cycle_Control.SetColumnSpan(this.lbl_Cycle_Auto, 2);
            this.lbl_Cycle_Auto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Cycle_Auto.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_Cycle_Auto.ForeColor = System.Drawing.Color.White;
            this.lbl_Cycle_Auto.Location = new System.Drawing.Point(43, 0);
            this.lbl_Cycle_Auto.Name = "lbl_Cycle_Auto";
            this.lbl_Cycle_Auto.Size = new System.Drawing.Size(374, 59);
            this.lbl_Cycle_Auto.TabIndex = 36;
            this.lbl_Cycle_Auto.Text = "AUTO";
            this.lbl_Cycle_Auto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Cycle
            // 
            this.lbl_Cycle.AutoSize = true;
            this.lbl_Cycle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.tbl_Cycle_Control.SetColumnSpan(this.lbl_Cycle, 2);
            this.lbl_Cycle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Cycle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_Cycle.ForeColor = System.Drawing.Color.White;
            this.lbl_Cycle.Location = new System.Drawing.Point(613, 0);
            this.lbl_Cycle.Name = "lbl_Cycle";
            this.lbl_Cycle.Size = new System.Drawing.Size(377, 59);
            this.lbl_Cycle.TabIndex = 38;
            this.lbl_Cycle.Text = "CYCLE";
            this.lbl_Cycle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Cycle_Count
            // 
            this.lbl_Cycle_Count.AutoSize = true;
            this.lbl_Cycle_Count.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.lbl_Cycle_Count.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Cycle_Count.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_Cycle_Count.ForeColor = System.Drawing.Color.White;
            this.lbl_Cycle_Count.Location = new System.Drawing.Point(423, 0);
            this.lbl_Cycle_Count.Name = "lbl_Cycle_Count";
            this.lbl_Cycle_Count.Size = new System.Drawing.Size(184, 59);
            this.lbl_Cycle_Count.TabIndex = 39;
            this.lbl_Cycle_Count.Text = "CYCLE";
            this.lbl_Cycle_Count.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserCycleTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbl_Cycle_Control);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "UserCycleTest";
            this.Size = new System.Drawing.Size(993, 149);
            this.tbl_Cycle_Control.ResumeLayout(false);
            this.tbl_Cycle_Control.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_Manual;
        private System.Windows.Forms.Button btn_Auto;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.Button btn_Run;
        private System.Windows.Forms.Button btn_CountNum;
        private System.Windows.Forms.TableLayoutPanel tbl_Cycle_Control;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lbl_Cycle_Auto;
        private System.Windows.Forms.Label lbl_Cycle_Count;
        private System.Windows.Forms.Label lbl_Cycle;
    }
}
