namespace Synustech.ucPanel.BcStatus
{
    partial class UserPIOStatus
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
            this.tlp_PIO_Status = new System.Windows.Forms.TableLayoutPanel();
            this.lblPIOName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblValid = new System.Windows.Forms.Label();
            this.lblLReq = new System.Windows.Forms.Label();
            this.lblULReq = new System.Windows.Forms.Label();
            this.lblReady = new System.Windows.Forms.Label();
            this.lblAvbl = new System.Windows.Forms.Label();
            this.lblES = new System.Windows.Forms.Label();
            this.lblCS = new System.Windows.Forms.Label();
            this.lblTRReq = new System.Windows.Forms.Label();
            this.lblBusy = new System.Windows.Forms.Label();
            this.lblCompt = new System.Windows.Forms.Label();
            this.ComboID = new Synustech.BcForm.ucPanel.BCComoboBox();
            this.lblCvId = new System.Windows.Forms.Label();
            this.tlp_PIO_Status.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // tlp_PIO_Status
            // 
            this.tlp_PIO_Status.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tlp_PIO_Status.ColumnCount = 1;
            this.tlp_PIO_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_PIO_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_PIO_Status.Controls.Add(this.lblPIOName, 0, 0);
            this.tlp_PIO_Status.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tlp_PIO_Status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_PIO_Status.Location = new System.Drawing.Point(0, 0);
            this.tlp_PIO_Status.Margin = new System.Windows.Forms.Padding(4);
            this.tlp_PIO_Status.Name = "tlp_PIO_Status";
            this.tlp_PIO_Status.RowCount = 2;
            this.tlp_PIO_Status.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.407407F));
            this.tlp_PIO_Status.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.59259F));
            this.tlp_PIO_Status.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp_PIO_Status.Size = new System.Drawing.Size(530, 675);
            this.tlp_PIO_Status.TabIndex = 5;
            // 
            // lblPIOName
            // 
            this.lblPIOName.AutoSize = true;
            this.lblPIOName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lblPIOName.ForeColor = System.Drawing.Color.White;
            this.lblPIOName.Location = new System.Drawing.Point(14, 16);
            this.lblPIOName.Margin = new System.Windows.Forms.Padding(14, 16, 4, 0);
            this.lblPIOName.Name = "lblPIOName";
            this.lblPIOName.Size = new System.Drawing.Size(128, 26);
            this.lblPIOName.TabIndex = 3;
            this.lblPIOName.Text = "PIO Status";
            this.lblPIOName.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblValid, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblLReq, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblULReq, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblReady, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblAvbl, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblES, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblCS, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblTRReq, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblBusy, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblCompt, 3, 7);
            this.tableLayoutPanel1.Controls.Add(this.ComboID, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCvId, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 53);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.33354F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333541F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333541F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333541F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333541F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333541F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333541F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333541F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333541F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.332708F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.332708F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.332708F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(522, 618);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // pictureBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox2, 3);
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::Synustech.Properties.Resources.CV_PIO_OK;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(4, 412);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBox2, 4);
            this.pictureBox2.Size = new System.Drawing.Size(253, 202);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // lblValid
            // 
            this.lblValid.AutoSize = true;
            this.lblValid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.SetColumnSpan(this.lblValid, 2);
            this.lblValid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValid.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValid.ForeColor = System.Drawing.Color.Gray;
            this.lblValid.Location = new System.Drawing.Point(265, 153);
            this.lblValid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValid.Name = "lblValid";
            this.lblValid.Size = new System.Drawing.Size(166, 51);
            this.lblValid.TabIndex = 2;
            this.lblValid.Text = "VALID";
            this.lblValid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLReq
            // 
            this.lblLReq.AutoSize = true;
            this.lblLReq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.SetColumnSpan(this.lblLReq, 2);
            this.lblLReq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLReq.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLReq.ForeColor = System.Drawing.Color.Gray;
            this.lblLReq.Location = new System.Drawing.Point(91, 153);
            this.lblLReq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLReq.Name = "lblLReq";
            this.lblLReq.Size = new System.Drawing.Size(166, 51);
            this.lblLReq.TabIndex = 3;
            this.lblLReq.Text = "L-REQ";
            this.lblLReq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblULReq
            // 
            this.lblULReq.AutoSize = true;
            this.lblULReq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.SetColumnSpan(this.lblULReq, 2);
            this.lblULReq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblULReq.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblULReq.ForeColor = System.Drawing.Color.Gray;
            this.lblULReq.Location = new System.Drawing.Point(91, 204);
            this.lblULReq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblULReq.Name = "lblULReq";
            this.lblULReq.Size = new System.Drawing.Size(166, 51);
            this.lblULReq.TabIndex = 4;
            this.lblULReq.Text = "UL-REQ";
            this.lblULReq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReady
            // 
            this.lblReady.AutoSize = true;
            this.lblReady.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.SetColumnSpan(this.lblReady, 2);
            this.lblReady.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReady.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReady.ForeColor = System.Drawing.Color.Gray;
            this.lblReady.Location = new System.Drawing.Point(91, 255);
            this.lblReady.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReady.Name = "lblReady";
            this.lblReady.Size = new System.Drawing.Size(166, 51);
            this.lblReady.TabIndex = 5;
            this.lblReady.Text = "READY";
            this.lblReady.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAvbl
            // 
            this.lblAvbl.AutoSize = true;
            this.lblAvbl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.SetColumnSpan(this.lblAvbl, 2);
            this.lblAvbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAvbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvbl.ForeColor = System.Drawing.Color.Gray;
            this.lblAvbl.Location = new System.Drawing.Point(91, 306);
            this.lblAvbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAvbl.Name = "lblAvbl";
            this.lblAvbl.Size = new System.Drawing.Size(166, 51);
            this.lblAvbl.TabIndex = 6;
            this.lblAvbl.Text = "HO-AVBL";
            this.lblAvbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblES
            // 
            this.lblES.AutoSize = true;
            this.lblES.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.SetColumnSpan(this.lblES, 2);
            this.lblES.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblES.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblES.ForeColor = System.Drawing.Color.Gray;
            this.lblES.Location = new System.Drawing.Point(91, 357);
            this.lblES.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblES.Name = "lblES";
            this.lblES.Size = new System.Drawing.Size(166, 51);
            this.lblES.TabIndex = 7;
            this.lblES.Text = "ES";
            this.lblES.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCS
            // 
            this.lblCS.AutoSize = true;
            this.lblCS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.SetColumnSpan(this.lblCS, 2);
            this.lblCS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCS.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCS.ForeColor = System.Drawing.Color.Gray;
            this.lblCS.Location = new System.Drawing.Point(265, 204);
            this.lblCS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCS.Name = "lblCS";
            this.lblCS.Size = new System.Drawing.Size(166, 51);
            this.lblCS.TabIndex = 8;
            this.lblCS.Text = "CS_0";
            this.lblCS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTRReq
            // 
            this.lblTRReq.AutoSize = true;
            this.lblTRReq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.SetColumnSpan(this.lblTRReq, 2);
            this.lblTRReq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTRReq.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTRReq.ForeColor = System.Drawing.Color.Gray;
            this.lblTRReq.Location = new System.Drawing.Point(265, 255);
            this.lblTRReq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTRReq.Name = "lblTRReq";
            this.lblTRReq.Size = new System.Drawing.Size(166, 51);
            this.lblTRReq.TabIndex = 9;
            this.lblTRReq.Text = "TR-REQ";
            this.lblTRReq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBusy
            // 
            this.lblBusy.AutoSize = true;
            this.lblBusy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.SetColumnSpan(this.lblBusy, 2);
            this.lblBusy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBusy.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBusy.ForeColor = System.Drawing.Color.Gray;
            this.lblBusy.Location = new System.Drawing.Point(265, 306);
            this.lblBusy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBusy.Name = "lblBusy";
            this.lblBusy.Size = new System.Drawing.Size(166, 51);
            this.lblBusy.TabIndex = 10;
            this.lblBusy.Text = "BUSY";
            this.lblBusy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCompt
            // 
            this.lblCompt.AutoSize = true;
            this.lblCompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.SetColumnSpan(this.lblCompt, 2);
            this.lblCompt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompt.ForeColor = System.Drawing.Color.Gray;
            this.lblCompt.Location = new System.Drawing.Point(265, 357);
            this.lblCompt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCompt.Name = "lblCompt";
            this.lblCompt.Size = new System.Drawing.Size(166, 51);
            this.lblCompt.TabIndex = 11;
            this.lblCompt.Text = "COMPT";
            this.lblCompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ComboID
            // 
            this.ComboID.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ComboID.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.ComboID.BorderSize = 1;
            this.tableLayoutPanel1.SetColumnSpan(this.ComboID, 3);
            this.ComboID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComboID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.ComboID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboID.ForeColor = System.Drawing.Color.DimGray;
            this.ComboID.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.ComboID.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.ComboID.ListTextColor = System.Drawing.Color.DimGray;
            this.ComboID.Location = new System.Drawing.Point(178, 55);
            this.ComboID.Margin = new System.Windows.Forms.Padding(4);
            this.ComboID.MinimumSize = new System.Drawing.Size(0, 45);
            this.ComboID.Name = "ComboID";
            this.ComboID.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.ComboID.Size = new System.Drawing.Size(253, 45);
            this.ComboID.TabIndex = 12;
            this.ComboID.Text = "bcComoboBox1";
            this.ComboID.Texts = "";
            // 
            // lblCvId
            // 
            this.lblCvId.AutoSize = true;
            this.lblCvId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.lblCvId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCvId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCvId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblCvId.Location = new System.Drawing.Point(91, 51);
            this.lblCvId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCvId.Name = "lblCvId";
            this.lblCvId.Size = new System.Drawing.Size(79, 51);
            this.lblCvId.TabIndex = 13;
            this.lblCvId.Text = "CV ID";
            this.lblCvId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserPIOStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlp_PIO_Status);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "UserPIOStatus";
            this.Size = new System.Drawing.Size(530, 675);
            this.tlp_PIO_Status.ResumeLayout(false);
            this.tlp_PIO_Status.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlp_PIO_Status;
        private System.Windows.Forms.Label lblPIOName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblValid;
        private System.Windows.Forms.Label lblLReq;
        private System.Windows.Forms.Label lblULReq;
        private System.Windows.Forms.Label lblReady;
        private System.Windows.Forms.Label lblAvbl;
        private System.Windows.Forms.Label lblES;
        private System.Windows.Forms.Label lblCS;
        private System.Windows.Forms.Label lblTRReq;
        private System.Windows.Forms.Label lblBusy;
        private System.Windows.Forms.Label lblCompt;
        private BcForm.ucPanel.BCComoboBox ComboID;
        private System.Windows.Forms.Label lblCvId;
    }
}
