namespace Synustech.ucPanel.BcMotion
{
    partial class UserConvControl
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
            this.lblConvControl = new System.Windows.Forms.Label();
            this.btn_Forward = new System.Windows.Forms.Button();
            this.btn_Backward = new System.Windows.Forms.Button();
            this.btn_Inching = new System.Windows.Forms.Button();
            this.btn_POS1 = new System.Windows.Forms.Button();
            this.btn_POS2 = new System.Windows.Forms.Button();
            this.btn_POS3 = new System.Windows.Forms.Button();
            this.btn_POS4 = new System.Windows.Forms.Button();
            this.btnDegree = new System.Windows.Forms.Button();
            this.pnlConvControl = new System.Windows.Forms.Panel();
            this.btnTurnJogNeg = new System.Windows.Forms.Button();
            this.btnTurnJogPos = new System.Windows.Forms.Button();
            this.pnlConvControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblConvControl
            // 
            this.lblConvControl.AutoSize = true;
            this.lblConvControl.Font = new System.Drawing.Font("Nirmala UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConvControl.ForeColor = System.Drawing.Color.White;
            this.lblConvControl.Location = new System.Drawing.Point(2, 0);
            this.lblConvControl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblConvControl.Name = "lblConvControl";
            this.lblConvControl.Size = new System.Drawing.Size(128, 28);
            this.lblConvControl.TabIndex = 0;
            this.lblConvControl.Text = "Conv Control";
            // 
            // btn_Forward
            // 
            this.btn_Forward.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_Forward.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Forward.FlatAppearance.BorderSize = 0;
            this.btn_Forward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Forward.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Forward.ForeColor = System.Drawing.Color.White;
            this.btn_Forward.Location = new System.Drawing.Point(41, 37);
            this.btn_Forward.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Forward.Name = "btn_Forward";
            this.btn_Forward.Size = new System.Drawing.Size(158, 40);
            this.btn_Forward.TabIndex = 16;
            this.btn_Forward.Text = "FWD";
            this.btn_Forward.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Forward.UseVisualStyleBackColor = false;
            // 
            // btn_Backward
            // 
            this.btn_Backward.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_Backward.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Backward.FlatAppearance.BorderSize = 0;
            this.btn_Backward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Backward.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Backward.ForeColor = System.Drawing.Color.White;
            this.btn_Backward.Location = new System.Drawing.Point(224, 37);
            this.btn_Backward.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Backward.Name = "btn_Backward";
            this.btn_Backward.Size = new System.Drawing.Size(158, 40);
            this.btn_Backward.TabIndex = 16;
            this.btn_Backward.Text = "BWD";
            this.btn_Backward.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Backward.UseVisualStyleBackColor = false;
            // 
            // btn_Inching
            // 
            this.btn_Inching.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btn_Inching.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Inching.FlatAppearance.BorderSize = 0;
            this.btn_Inching.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Inching.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Inching.ForeColor = System.Drawing.Color.White;
            this.btn_Inching.Location = new System.Drawing.Point(401, 37);
            this.btn_Inching.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_Inching.Name = "btn_Inching";
            this.btn_Inching.Size = new System.Drawing.Size(127, 40);
            this.btn_Inching.TabIndex = 16;
            this.btn_Inching.Text = "Speed";
            this.btn_Inching.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Inching.UseVisualStyleBackColor = false;
            // 
            // btn_POS1
            // 
            this.btn_POS1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_POS1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_POS1.FlatAppearance.BorderSize = 0;
            this.btn_POS1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_POS1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_POS1.ForeColor = System.Drawing.Color.White;
            this.btn_POS1.Location = new System.Drawing.Point(550, 37);
            this.btn_POS1.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_POS1.Name = "btn_POS1";
            this.btn_POS1.Size = new System.Drawing.Size(112, 40);
            this.btn_POS1.TabIndex = 16;
            this.btn_POS1.Text = "POS_1";
            this.btn_POS1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_POS1.UseVisualStyleBackColor = false;
            // 
            // btn_POS2
            // 
            this.btn_POS2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_POS2.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_POS2.FlatAppearance.BorderSize = 0;
            this.btn_POS2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_POS2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_POS2.ForeColor = System.Drawing.Color.White;
            this.btn_POS2.Location = new System.Drawing.Point(684, 37);
            this.btn_POS2.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_POS2.Name = "btn_POS2";
            this.btn_POS2.Size = new System.Drawing.Size(112, 40);
            this.btn_POS2.TabIndex = 16;
            this.btn_POS2.Text = "POS_2";
            this.btn_POS2.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_POS2.UseVisualStyleBackColor = false;
            // 
            // btn_POS3
            // 
            this.btn_POS3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_POS3.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_POS3.FlatAppearance.BorderSize = 0;
            this.btn_POS3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_POS3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_POS3.ForeColor = System.Drawing.Color.White;
            this.btn_POS3.Location = new System.Drawing.Point(684, 81);
            this.btn_POS3.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_POS3.Name = "btn_POS3";
            this.btn_POS3.Size = new System.Drawing.Size(112, 40);
            this.btn_POS3.TabIndex = 16;
            this.btn_POS3.Text = "POS_3";
            this.btn_POS3.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_POS3.UseVisualStyleBackColor = false;
            // 
            // btn_POS4
            // 
            this.btn_POS4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_POS4.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_POS4.FlatAppearance.BorderSize = 0;
            this.btn_POS4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_POS4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_POS4.ForeColor = System.Drawing.Color.White;
            this.btn_POS4.Location = new System.Drawing.Point(550, 81);
            this.btn_POS4.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btn_POS4.Name = "btn_POS4";
            this.btn_POS4.Size = new System.Drawing.Size(112, 40);
            this.btn_POS4.TabIndex = 16;
            this.btn_POS4.Text = "POS_4";
            this.btn_POS4.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_POS4.UseVisualStyleBackColor = false;
            // 
            // btnDegree
            // 
            this.btnDegree.BackColor = System.Drawing.Color.White;
            this.btnDegree.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnDegree.FlatAppearance.BorderSize = 0;
            this.btnDegree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDegree.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDegree.ForeColor = System.Drawing.Color.Black;
            this.btnDegree.Location = new System.Drawing.Point(401, 81);
            this.btnDegree.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btnDegree.Name = "btnDegree";
            this.btnDegree.Size = new System.Drawing.Size(127, 40);
            this.btnDegree.TabIndex = 17;
            this.btnDegree.Text = "10";
            this.btnDegree.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnDegree.UseVisualStyleBackColor = false;
            this.btnDegree.Click += new System.EventHandler(this.btnDegree_Click);
            // 
            // pnlConvControl
            // 
            this.pnlConvControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.pnlConvControl.Controls.Add(this.btnDegree);
            this.pnlConvControl.Controls.Add(this.btn_POS4);
            this.pnlConvControl.Controls.Add(this.btn_POS3);
            this.pnlConvControl.Controls.Add(this.btn_POS2);
            this.pnlConvControl.Controls.Add(this.btn_POS1);
            this.pnlConvControl.Controls.Add(this.btn_Inching);
            this.pnlConvControl.Controls.Add(this.btnTurnJogNeg);
            this.pnlConvControl.Controls.Add(this.btnTurnJogPos);
            this.pnlConvControl.Controls.Add(this.btn_Backward);
            this.pnlConvControl.Controls.Add(this.btn_Forward);
            this.pnlConvControl.Controls.Add(this.lblConvControl);
            this.pnlConvControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConvControl.Location = new System.Drawing.Point(0, 0);
            this.pnlConvControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlConvControl.Name = "pnlConvControl";
            this.pnlConvControl.Size = new System.Drawing.Size(880, 150);
            this.pnlConvControl.TabIndex = 0;
            // 
            // btnTurnJogNeg
            // 
            this.btnTurnJogNeg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btnTurnJogNeg.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnTurnJogNeg.FlatAppearance.BorderSize = 0;
            this.btnTurnJogNeg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnJogNeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurnJogNeg.ForeColor = System.Drawing.Color.White;
            this.btnTurnJogNeg.Location = new System.Drawing.Point(224, 81);
            this.btnTurnJogNeg.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btnTurnJogNeg.Name = "btnTurnJogNeg";
            this.btnTurnJogNeg.Size = new System.Drawing.Size(158, 40);
            this.btnTurnJogNeg.TabIndex = 16;
            this.btnTurnJogNeg.Text = "Turn -";
            this.btnTurnJogNeg.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnTurnJogNeg.UseVisualStyleBackColor = false;
            this.btnTurnJogNeg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnJogNeg_MouseDown);
            this.btnTurnJogNeg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnJogNeg_MouseUp);
            // 
            // btnTurnJogPos
            // 
            this.btnTurnJogPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTurnJogPos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btnTurnJogPos.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnTurnJogPos.FlatAppearance.BorderSize = 0;
            this.btnTurnJogPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnJogPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurnJogPos.ForeColor = System.Drawing.Color.White;
            this.btnTurnJogPos.Location = new System.Drawing.Point(41, 81);
            this.btnTurnJogPos.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.btnTurnJogPos.Name = "btnTurnJogPos";
            this.btnTurnJogPos.Size = new System.Drawing.Size(158, 40);
            this.btnTurnJogPos.TabIndex = 16;
            this.btnTurnJogPos.Text = "Turn +";
            this.btnTurnJogPos.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnTurnJogPos.UseVisualStyleBackColor = false;
            this.btnTurnJogPos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnJogPos_MouseDown);
            this.btnTurnJogPos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnJogPos_MouseUp);
            // 
            // UserConvControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlConvControl);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "UserConvControl";
            this.Size = new System.Drawing.Size(880, 150);
            this.pnlConvControl.ResumeLayout(false);
            this.pnlConvControl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblConvControl;
        private System.Windows.Forms.Button btn_Forward;
        private System.Windows.Forms.Button btn_Backward;
        private System.Windows.Forms.Button btn_Inching;
        private System.Windows.Forms.Button btn_POS1;
        private System.Windows.Forms.Button btn_POS2;
        private System.Windows.Forms.Button btn_POS3;
        private System.Windows.Forms.Button btn_POS4;
        public System.Windows.Forms.Button btnDegree;
        private System.Windows.Forms.Panel pnlConvControl;
        private System.Windows.Forms.Button btnTurnJogNeg;
        private System.Windows.Forms.Button btnTurnJogPos;
    }
}
