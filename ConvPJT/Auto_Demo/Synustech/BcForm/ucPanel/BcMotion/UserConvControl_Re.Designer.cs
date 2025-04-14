namespace Synustech.ucPanel.BcMotion
{
    partial class UserConvControl_Re
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_InPutVal = new System.Windows.Forms.Button();
            this.btn_Txt_Speed = new System.Windows.Forms.Button();
            this.Text_JOG = new System.Windows.Forms.TextBox();
            this.btn_Forward = new System.Windows.Forms.Button();
            this.btn_Backward = new System.Windows.Forms.Button();
            this.btnTurnJogPos = new System.Windows.Forms.Button();
            this.btnTurnJogNeg = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.Text_JOG, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_Forward, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btn_Backward, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnTurnJogPos, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnTurnJogNeg, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1917, 272);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btn_InPutVal, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btn_Txt_Speed, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(802, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel1.SetRowSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(370, 268);
            this.tableLayoutPanel2.TabIndex = 19;
            // 
            // btn_InPutVal
            // 
            this.btn_InPutVal.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_InPutVal.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_InPutVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_InPutVal.FlatAppearance.BorderSize = 0;
            this.btn_InPutVal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_InPutVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_InPutVal.ForeColor = System.Drawing.Color.Black;
            this.btn_InPutVal.Location = new System.Drawing.Point(0, 53);
            this.btn_InPutVal.Margin = new System.Windows.Forms.Padding(0);
            this.btn_InPutVal.Name = "btn_InPutVal";
            this.btn_InPutVal.Size = new System.Drawing.Size(370, 215);
            this.btn_InPutVal.TabIndex = 18;
            this.btn_InPutVal.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_InPutVal.UseVisualStyleBackColor = false;
            this.btn_InPutVal.Click += new System.EventHandler(this.btn_InPutVal_Click);
            // 
            // btn_Txt_Speed
            // 
            this.btn_Txt_Speed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btn_Txt_Speed.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Txt_Speed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Txt_Speed.FlatAppearance.BorderSize = 0;
            this.btn_Txt_Speed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Txt_Speed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Txt_Speed.ForeColor = System.Drawing.Color.White;
            this.btn_Txt_Speed.Location = new System.Drawing.Point(1, 0);
            this.btn_Txt_Speed.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.btn_Txt_Speed.Name = "btn_Txt_Speed";
            this.btn_Txt_Speed.Size = new System.Drawing.Size(368, 53);
            this.btn_Txt_Speed.TabIndex = 17;
            this.btn_Txt_Speed.Text = "SPEED[mm/s]";
            this.btn_Txt_Speed.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Txt_Speed.UseVisualStyleBackColor = false;
            // 
            // Text_JOG
            // 
            this.Text_JOG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.Text_JOG.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Text_JOG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Text_JOG.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_JOG.ForeColor = System.Drawing.Color.White;
            this.Text_JOG.Location = new System.Drawing.Point(1, 2);
            this.Text_JOG.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Text_JOG.Multiline = true;
            this.Text_JOG.Name = "Text_JOG";
            this.tableLayoutPanel1.SetRowSpan(this.Text_JOG, 2);
            this.Text_JOG.Size = new System.Drawing.Size(55, 268);
            this.Text_JOG.TabIndex = 34;
            this.Text_JOG.Text = " ";
            this.Text_JOG.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn_Forward
            // 
            this.btn_Forward.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btn_Forward.BackgroundImage = global::Synustech.Properties.Resources.FWD_Re;
            this.btn_Forward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Forward.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Forward.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Forward.FlatAppearance.BorderSize = 0;
            this.btn_Forward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Forward.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Forward.ForeColor = System.Drawing.Color.White;
            this.btn_Forward.Location = new System.Drawing.Point(432, 57);
            this.btn_Forward.Name = "btn_Forward";
            this.btn_Forward.Size = new System.Drawing.Size(366, 212);
            this.btn_Forward.TabIndex = 17;
            this.btn_Forward.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Forward.UseVisualStyleBackColor = false;
            // 
            // btn_Backward
            // 
            this.btn_Backward.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btn_Backward.BackgroundImage = global::Synustech.Properties.Resources.BWD_Re;
            this.btn_Backward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Backward.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Backward.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Backward.FlatAppearance.BorderSize = 0;
            this.btn_Backward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Backward.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Backward.ForeColor = System.Drawing.Color.White;
            this.btn_Backward.Location = new System.Drawing.Point(60, 57);
            this.btn_Backward.Name = "btn_Backward";
            this.btn_Backward.Size = new System.Drawing.Size(366, 212);
            this.btn_Backward.TabIndex = 18;
            this.btn_Backward.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Backward.UseVisualStyleBackColor = false;
            // 
            // btnTurnJogPos
            // 
            this.btnTurnJogPos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btnTurnJogPos.BackgroundImage = global::Synustech.Properties.Resources.Turn;
            this.btnTurnJogPos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTurnJogPos.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnTurnJogPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTurnJogPos.FlatAppearance.BorderSize = 0;
            this.btnTurnJogPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnJogPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurnJogPos.ForeColor = System.Drawing.Color.White;
            this.btnTurnJogPos.Location = new System.Drawing.Point(1548, 57);
            this.btnTurnJogPos.Name = "btnTurnJogPos";
            this.btnTurnJogPos.Size = new System.Drawing.Size(366, 212);
            this.btnTurnJogPos.TabIndex = 17;
            this.btnTurnJogPos.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnTurnJogPos.UseVisualStyleBackColor = false;
            this.btnTurnJogPos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnJogPos_MouseDown);
            this.btnTurnJogPos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnJogPos_MouseUp);
            // 
            // btnTurnJogNeg
            // 
            this.btnTurnJogNeg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btnTurnJogNeg.BackgroundImage = global::Synustech.Properties.Resources._return;
            this.btnTurnJogNeg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTurnJogNeg.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnTurnJogNeg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTurnJogNeg.FlatAppearance.BorderSize = 0;
            this.btnTurnJogNeg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnJogNeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurnJogNeg.ForeColor = System.Drawing.Color.White;
            this.btnTurnJogNeg.Location = new System.Drawing.Point(1176, 57);
            this.btnTurnJogNeg.Name = "btnTurnJogNeg";
            this.btnTurnJogNeg.Size = new System.Drawing.Size(366, 212);
            this.btnTurnJogNeg.TabIndex = 20;
            this.btnTurnJogNeg.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnTurnJogNeg.UseVisualStyleBackColor = false;
            this.btnTurnJogNeg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnJogNeg_MouseDown);
            this.btnTurnJogNeg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnJogNeg_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(58, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(370, 54);
            this.label1.TabIndex = 35;
            this.label1.Text = "BWD";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(1174, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(370, 54);
            this.label2.TabIndex = 36;
            this.label2.Text = "RETURN";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(1546, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(370, 54);
            this.label3.TabIndex = 37;
            this.label3.Text = "TURN";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(430, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(370, 54);
            this.label4.TabIndex = 38;
            this.label4.Text = "FWD";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserConvControl_Re
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UserConvControl_Re";
            this.Size = new System.Drawing.Size(1917, 272);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_Forward;
        private System.Windows.Forms.Button btn_Backward;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnTurnJogPos;
        private System.Windows.Forms.Button btnTurnJogNeg;
        private System.Windows.Forms.Button btn_Txt_Speed;
        public System.Windows.Forms.Button btn_InPutVal;
        private System.Windows.Forms.TextBox Text_JOG;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
