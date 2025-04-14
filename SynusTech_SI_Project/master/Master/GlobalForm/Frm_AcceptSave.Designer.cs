
namespace Master.GlobalForm
{
    partial class Frm_AcceptSave
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
            this.btn_SaveAccept = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.Controls.Add(this.btn_SaveAccept, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 111);
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
            // 
            // lbl_IDTitle
            // 
            this.lbl_IDTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_IDTitle.Location = new System.Drawing.Point(3, 0);
            this.lbl_IDTitle.Name = "lbl_IDTitle";
            this.lbl_IDTitle.Size = new System.Drawing.Size(174, 35);
            this.lbl_IDTitle.TabIndex = 1;
            this.lbl_IDTitle.Text = "Admin Name :";
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
            // 
            // btn_SaveAccept
            // 
            this.btn_SaveAccept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_SaveAccept.Location = new System.Drawing.Point(183, 74);
            this.btn_SaveAccept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_SaveAccept.Name = "btn_SaveAccept";
            this.btn_SaveAccept.Size = new System.Drawing.Size(198, 33);
            this.btn_SaveAccept.TabIndex = 3;
            this.btn_SaveAccept.Text = "Save Accept";
            this.btn_SaveAccept.UseVisualStyleBackColor = true;
            this.btn_SaveAccept.Click += new System.EventHandler(this.btn_SaveAccept_Click);
            // 
            // Frm_AcceptSave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(384, 111);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(400, 150);
            this.MinimumSize = new System.Drawing.Size(400, 150);
            this.Name = "Frm_AcceptSave";
            this.Text = "Admin Message";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tbx_Password;
        private System.Windows.Forms.Label lbl_IDTitle;
        private System.Windows.Forms.Label lbl_PasswordTitle;
        private System.Windows.Forms.Button btn_SaveAccept;
        private System.Windows.Forms.TextBox tbx_ID;
    }
}