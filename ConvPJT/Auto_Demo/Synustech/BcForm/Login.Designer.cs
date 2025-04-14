using Synustech.ucPanel;

namespace Synustech.BcForm
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblLoginName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_loginCheck = new System.Windows.Forms.Button();
            this.btnX = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bcComoboBox1 = new Synustech.BcForm.ucPanel.BCComoboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.50774F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.49226F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblLoginName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btn_loginCheck, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnX, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.bcComoboBox1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.2069F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.75862F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.44828F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.2069F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(600, 300);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Synustech.Properties.Resources.Logo_Remove;
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(187, 71);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblLoginName
            // 
            this.lblLoginName.AutoSize = true;
            this.lblLoginName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoginName.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginName.ForeColor = System.Drawing.Color.White;
            this.lblLoginName.Location = new System.Drawing.Point(4, 79);
            this.lblLoginName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoginName.Name = "lblLoginName";
            this.lblLoginName.Size = new System.Drawing.Size(187, 69);
            this.lblLoginName.TabIndex = 1;
            this.lblLoginName.Text = "User ID";
            this.lblLoginName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 148);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 71);
            this.label1.TabIndex = 2;
            this.label1.Text = "P/W";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_loginCheck
            // 
            this.btn_loginCheck.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_loginCheck.FlatAppearance.BorderSize = 0;
            this.btn_loginCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_loginCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_loginCheck.ForeColor = System.Drawing.Color.Transparent;
            this.btn_loginCheck.Location = new System.Drawing.Point(412, 222);
            this.btn_loginCheck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_loginCheck.Name = "btn_loginCheck";
            this.btn_loginCheck.Size = new System.Drawing.Size(184, 75);
            this.btn_loginCheck.TabIndex = 8;
            this.btn_loginCheck.Text = "Check Login";
            this.btn_loginCheck.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btn_loginCheck.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_loginCheck.UseVisualStyleBackColor = true;
            this.btn_loginCheck.Click += new System.EventHandler(this.btn_loginCheck_Click);
            // 
            // btnX
            // 
            this.btnX.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnX.FlatAppearance.BorderSize = 0;
            this.btnX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnX.ForeColor = System.Drawing.Color.White;
            this.btnX.Location = new System.Drawing.Point(546, 3);
            this.btnX.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnX.Name = "btnX";
            this.btnX.Size = new System.Drawing.Size(50, 73);
            this.btnX.TabIndex = 7;
            this.btnX.Text = "X";
            this.btnX.UseVisualStyleBackColor = true;
            this.btnX.Click += new System.EventHandler(this.btnX_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(198, 158);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
            this.textBox1.MaxLength = 4;
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(392, 41);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox1.Click += new System.EventHandler(this.KeyBoardShow);
            // 
            // bcComoboBox1
            // 
            this.bcComoboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.bcComoboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.bcComoboBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.bcComoboBox1.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.bcComoboBox1.BorderSize = 1;
            this.bcComoboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcComoboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.bcComoboBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bcComoboBox1.ForeColor = System.Drawing.Color.DimGray;
            this.bcComoboBox1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.bcComoboBox1.Items.AddRange(new object[] {
            "User",
            "Admin",
            "Maker"});
            this.bcComoboBox1.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.bcComoboBox1.ListTextColor = System.Drawing.Color.DimGray;
            this.bcComoboBox1.Location = new System.Drawing.Point(199, 83);
            this.bcComoboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.bcComoboBox1.MinimumSize = new System.Drawing.Size(300, 45);
            this.bcComoboBox1.Name = "bcComoboBox1";
            this.bcComoboBox1.Padding = new System.Windows.Forms.Padding(2);
            this.bcComoboBox1.Size = new System.Drawing.Size(397, 61);
            this.bcComoboBox1.TabIndex = 9;
            this.bcComoboBox1.Text = "bcComoboBox1";
            this.bcComoboBox1.Texts = "";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(500, 500);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Login";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblLoginName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_loginCheck;
        private System.Windows.Forms.Button btnX;
        private System.Windows.Forms.TextBox textBox1;
        private BcForm.ucPanel.BCComoboBox bcComoboBox1;
      
    }
}