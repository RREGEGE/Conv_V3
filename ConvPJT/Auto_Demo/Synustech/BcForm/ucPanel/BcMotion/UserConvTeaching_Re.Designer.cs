namespace Synustech.ucPanel.BcMotion
{
    partial class UserConvTeaching_Re
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLdMove = new System.Windows.Forms.Button();
            this.btnUldMove = new System.Windows.Forms.Button();
            this.btn_Setting = new System.Windows.Forms.Button();
            this.btnTurnJogNeg = new System.Windows.Forms.Button();
            this.lblInching = new System.Windows.Forms.Label();
            this.btnTurnJogPos = new System.Windows.Forms.Button();
            this.lblUload = new System.Windows.Forms.Label();
            this.lblLoad = new System.Windows.Forms.Label();
            this.lblUloadPOS = new System.Windows.Forms.Label();
            this.btnDegree = new System.Windows.Forms.Button();
            this.lblLoadPOS = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.POS_Update_Timer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Controls.Add(this.btnLdMove, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnUldMove, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_Setting, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnTurnJogNeg, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblInching, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnTurnJogPos, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblUload, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblLoad, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblUloadPOS, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnDegree, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblLoadPOS, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1929, 296);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnLdMove
            // 
            this.btnLdMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btnLdMove.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnLdMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLdMove.FlatAppearance.BorderSize = 0;
            this.btnLdMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLdMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLdMove.ForeColor = System.Drawing.Color.White;
            this.btnLdMove.Location = new System.Drawing.Point(994, 0);
            this.btnLdMove.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.btnLdMove.Name = "btnLdMove";
            this.btnLdMove.Size = new System.Drawing.Size(309, 148);
            this.btnLdMove.TabIndex = 26;
            this.btnLdMove.Text = "LOAD MOVE";
            this.btnLdMove.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnLdMove.UseVisualStyleBackColor = false;
            this.btnLdMove.Click += new System.EventHandler(this.btnLdMove_Click);
            // 
            // btnUldMove
            // 
            this.btnUldMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btnUldMove.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnUldMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUldMove.FlatAppearance.BorderSize = 0;
            this.btnUldMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUldMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUldMove.ForeColor = System.Drawing.Color.White;
            this.btnUldMove.Location = new System.Drawing.Point(1305, 0);
            this.btnUldMove.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.btnUldMove.Name = "btnUldMove";
            this.btnUldMove.Size = new System.Drawing.Size(310, 148);
            this.btnUldMove.TabIndex = 27;
            this.btnUldMove.Text = "UNLOAD MOVE";
            this.btnUldMove.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnUldMove.UseVisualStyleBackColor = false;
            this.btnUldMove.Click += new System.EventHandler(this.btnUldMove_Click);
            // 
            // btn_Setting
            // 
            this.btn_Setting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.btn_Setting.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btn_Setting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Setting.FlatAppearance.BorderSize = 0;
            this.btn_Setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Setting.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Setting.ForeColor = System.Drawing.Color.White;
            this.btn_Setting.Location = new System.Drawing.Point(1617, 0);
            this.btn_Setting.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.btn_Setting.Name = "btn_Setting";
            this.tableLayoutPanel1.SetRowSpan(this.btn_Setting, 2);
            this.btn_Setting.Size = new System.Drawing.Size(311, 296);
            this.btn_Setting.TabIndex = 24;
            this.btn_Setting.Text = "Setting";
            this.btn_Setting.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Setting.UseVisualStyleBackColor = false;
            this.btn_Setting.Click += new System.EventHandler(this.btn_Setting_Click);
            // 
            // btnTurnJogNeg
            // 
            this.btnTurnJogNeg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btnTurnJogNeg.BackgroundImage = global::Synustech.Properties.Resources.minus_ReRe;
            this.btnTurnJogNeg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTurnJogNeg.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnTurnJogNeg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTurnJogNeg.FlatAppearance.BorderSize = 0;
            this.btnTurnJogNeg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnJogNeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurnJogNeg.ForeColor = System.Drawing.Color.White;
            this.btnTurnJogNeg.Location = new System.Drawing.Point(1304, 151);
            this.btnTurnJogNeg.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.btnTurnJogNeg.Name = "btnTurnJogNeg";
            this.btnTurnJogNeg.Size = new System.Drawing.Size(308, 142);
            this.btnTurnJogNeg.TabIndex = 26;
            this.btnTurnJogNeg.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnTurnJogNeg.UseVisualStyleBackColor = false;
            this.btnTurnJogNeg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnJogNeg_MouseDown);
            this.btnTurnJogNeg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnJogNeg_MouseUp);
            // 
            // lblInching
            // 
            this.lblInching.AutoSize = true;
            this.lblInching.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.lblInching.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInching.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lblInching.ForeColor = System.Drawing.Color.White;
            this.lblInching.Location = new System.Drawing.Point(682, 0);
            this.lblInching.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblInching.Name = "lblInching";
            this.lblInching.Size = new System.Drawing.Size(310, 148);
            this.lblInching.TabIndex = 28;
            this.lblInching.Text = "INCHING";
            this.lblInching.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTurnJogPos
            // 
            this.btnTurnJogPos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.btnTurnJogPos.BackgroundImage = global::Synustech.Properties.Resources.plus_re__;
            this.btnTurnJogPos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTurnJogPos.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnTurnJogPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTurnJogPos.FlatAppearance.BorderSize = 0;
            this.btnTurnJogPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnJogPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurnJogPos.ForeColor = System.Drawing.Color.White;
            this.btnTurnJogPos.Location = new System.Drawing.Point(996, 156);
            this.btnTurnJogPos.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.btnTurnJogPos.Name = "btnTurnJogPos";
            this.btnTurnJogPos.Size = new System.Drawing.Size(305, 132);
            this.btnTurnJogPos.TabIndex = 25;
            this.btnTurnJogPos.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnTurnJogPos.UseVisualStyleBackColor = false;
            this.btnTurnJogPos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnJogPos_MouseDown);
            this.btnTurnJogPos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnJogPos_MouseUp);
            // 
            // lblUload
            // 
            this.lblUload.AutoSize = true;
            this.lblUload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.lblUload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUload.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lblUload.ForeColor = System.Drawing.Color.White;
            this.lblUload.Location = new System.Drawing.Point(370, 0);
            this.lblUload.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblUload.Name = "lblUload";
            this.lblUload.Size = new System.Drawing.Size(310, 148);
            this.lblUload.TabIndex = 29;
            this.lblUload.Text = "UNLOAD POS";
            this.lblUload.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblUload.Click += new System.EventHandler(this.lblUload_Click);
            // 
            // lblLoad
            // 
            this.lblLoad.AutoSize = true;
            this.lblLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.lblLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lblLoad.ForeColor = System.Drawing.Color.White;
            this.lblLoad.Location = new System.Drawing.Point(58, 0);
            this.lblLoad.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblLoad.Name = "lblLoad";
            this.lblLoad.Size = new System.Drawing.Size(310, 148);
            this.lblLoad.TabIndex = 30;
            this.lblLoad.Text = "LOAD POS";
            this.lblLoad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoad.Click += new System.EventHandler(this.lblLoad_Click);
            // 
            // lblUloadPOS
            // 
            this.lblUloadPOS.AutoSize = true;
            this.lblUloadPOS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.lblUloadPOS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUloadPOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUloadPOS.ForeColor = System.Drawing.Color.White;
            this.lblUloadPOS.Location = new System.Drawing.Point(373, 148);
            this.lblUloadPOS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUloadPOS.Name = "lblUloadPOS";
            this.lblUloadPOS.Size = new System.Drawing.Size(304, 148);
            this.lblUloadPOS.TabIndex = 31;
            this.lblUloadPOS.Text = "0";
            this.lblUloadPOS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDegree
            // 
            this.btnDegree.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDegree.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnDegree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDegree.FlatAppearance.BorderSize = 0;
            this.btnDegree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDegree.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDegree.ForeColor = System.Drawing.Color.Black;
            this.btnDegree.Location = new System.Drawing.Point(685, 148);
            this.btnDegree.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnDegree.Name = "btnDegree";
            this.btnDegree.Size = new System.Drawing.Size(304, 148);
            this.btnDegree.TabIndex = 19;
            this.btnDegree.Text = "0";
            this.btnDegree.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnDegree.UseVisualStyleBackColor = false;
            this.btnDegree.Click += new System.EventHandler(this.btnDegree_Click);
            // 
            // lblLoadPOS
            // 
            this.lblLoadPOS.AutoSize = true;
            this.lblLoadPOS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.lblLoadPOS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoadPOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoadPOS.ForeColor = System.Drawing.Color.White;
            this.lblLoadPOS.Location = new System.Drawing.Point(61, 148);
            this.lblLoadPOS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoadPOS.Name = "lblLoadPOS";
            this.lblLoadPOS.Size = new System.Drawing.Size(304, 148);
            this.lblLoadPOS.TabIndex = 32;
            this.lblLoadPOS.Text = "0";
            this.lblLoadPOS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(1, 2);
            this.textBox1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.tableLayoutPanel1.SetRowSpan(this.textBox1, 2);
            this.textBox1.Size = new System.Drawing.Size(55, 292);
            this.textBox1.TabIndex = 33;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // POS_Update_Timer
            // 
            this.POS_Update_Timer.Enabled = true;
            this.POS_Update_Timer.Tick += new System.EventHandler(this.POS_Update_Timer_Tick);
            // 
            // UserConvTeaching_Re
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UserConvTeaching_Re";
            this.Size = new System.Drawing.Size(1929, 296);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_Setting;
        private System.Windows.Forms.Button btnTurnJogPos;
        private System.Windows.Forms.Button btnTurnJogNeg;
        private System.Windows.Forms.Label lblInching;
        private System.Windows.Forms.Label lblUload;
        private System.Windows.Forms.Label lblLoad;
        public System.Windows.Forms.Button btnDegree;
        private System.Windows.Forms.Label lblUloadPOS;
        private System.Windows.Forms.Label lblLoadPOS;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnLdMove;
        private System.Windows.Forms.Button btnUldMove;
        private System.Windows.Forms.Timer POS_Update_Timer;
    }
}
