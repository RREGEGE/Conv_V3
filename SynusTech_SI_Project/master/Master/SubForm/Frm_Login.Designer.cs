
namespace Master.SubForm
{
    partial class Frm_Login
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
            this.tbx_Password = new System.Windows.Forms.TextBox();
            this.lbl_IDTitle = new System.Windows.Forms.Label();
            this.lbl_PasswordTitle = new System.Windows.Forms.Label();
            this.tbx_ID = new System.Windows.Forms.TextBox();
            this.lbl_DurationTimeTitle = new System.Windows.Forms.Label();
            this.cbx_LoginDuration = new System.Windows.Forms.ComboBox();
            this.btn_LogIn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tbx_Password, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbl_IDTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_PasswordTitle, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbx_ID, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_DurationTimeTitle, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbx_LoginDuration, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btn_LogIn, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 161);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tbx_Password
            // 
            this.tbx_Password.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_Password.Location = new System.Drawing.Point(183, 39);
            this.tbx_Password.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbx_Password.Name = "tbx_Password";
            this.tbx_Password.PasswordChar = '*';
            this.tbx_Password.Size = new System.Drawing.Size(198, 25);
            this.tbx_Password.TabIndex = 5;
            this.tbx_Password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownEvent);
            // 
            // lbl_IDTitle
            // 
            this.lbl_IDTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_IDTitle.Location = new System.Drawing.Point(3, 0);
            this.lbl_IDTitle.Name = "lbl_IDTitle";
            this.lbl_IDTitle.Size = new System.Drawing.Size(174, 35);
            this.lbl_IDTitle.TabIndex = 1;
            this.lbl_IDTitle.Text = "ID :";
            this.lbl_IDTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_PasswordTitle
            // 
            this.lbl_PasswordTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_PasswordTitle.Location = new System.Drawing.Point(3, 35);
            this.lbl_PasswordTitle.Name = "lbl_PasswordTitle";
            this.lbl_PasswordTitle.Size = new System.Drawing.Size(174, 35);
            this.lbl_PasswordTitle.TabIndex = 2;
            this.lbl_PasswordTitle.Text = "Password :";
            this.lbl_PasswordTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbx_ID
            // 
            this.tbx_ID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_ID.Location = new System.Drawing.Point(183, 4);
            this.tbx_ID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbx_ID.Name = "tbx_ID";
            this.tbx_ID.Size = new System.Drawing.Size(198, 25);
            this.tbx_ID.TabIndex = 4;
            this.tbx_ID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownEvent);
            // 
            // lbl_DurationTimeTitle
            // 
            this.lbl_DurationTimeTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_DurationTimeTitle.Location = new System.Drawing.Point(3, 70);
            this.lbl_DurationTimeTitle.Name = "lbl_DurationTimeTitle";
            this.lbl_DurationTimeTitle.Size = new System.Drawing.Size(174, 35);
            this.lbl_DurationTimeTitle.TabIndex = 6;
            this.lbl_DurationTimeTitle.Text = "Duration Time :";
            this.lbl_DurationTimeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbx_LoginDuration
            // 
            this.cbx_LoginDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbx_LoginDuration.FormattingEnabled = true;
            this.cbx_LoginDuration.Location = new System.Drawing.Point(183, 74);
            this.cbx_LoginDuration.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbx_LoginDuration.Name = "cbx_LoginDuration";
            this.cbx_LoginDuration.Size = new System.Drawing.Size(198, 25);
            this.cbx_LoginDuration.TabIndex = 45;
            this.cbx_LoginDuration.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownEvent);
            // 
            // btn_LogIn
            // 
            this.btn_LogIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_LogIn.Location = new System.Drawing.Point(183, 109);
            this.btn_LogIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_LogIn.Name = "btn_LogIn";
            this.btn_LogIn.Size = new System.Drawing.Size(198, 48);
            this.btn_LogIn.TabIndex = 3;
            this.btn_LogIn.Text = "Login";
            this.btn_LogIn.UseVisualStyleBackColor = true;
            this.btn_LogIn.Click += new System.EventHandler(this.btn_LogIn_Click);
            // 
            // Frm_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(384, 161);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(400, 200);
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "Frm_Login";
            this.Text = "Login";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownEvent);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tbx_Password;
        private System.Windows.Forms.Label lbl_IDTitle;
        private System.Windows.Forms.Label lbl_PasswordTitle;
        private System.Windows.Forms.Button btn_LogIn;
        private System.Windows.Forms.TextBox tbx_ID;
        private System.Windows.Forms.Label lbl_DurationTimeTitle;
        private System.Windows.Forms.ComboBox cbx_LoginDuration;
    }
}