
namespace RackMaster.SUBFORM {
    partial class FrmStatus {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnOutputMode = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnAxisPage = new System.Windows.Forms.Button();
            this.btnFullClosedPage = new System.Windows.Forms.Button();
            this.btnIOPage = new System.Windows.Forms.Button();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.btnMotionPage = new System.Windows.Forms.Button();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOutputMode
            // 
            this.btnOutputMode.BackColor = System.Drawing.Color.White;
            this.btnOutputMode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOutputMode.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOutputMode.Location = new System.Drawing.Point(1488, 3);
            this.btnOutputMode.Name = "btnOutputMode";
            this.btnOutputMode.Size = new System.Drawing.Size(196, 77);
            this.btnOutputMode.TabIndex = 2;
            this.btnOutputMode.Text = "Output Mode";
            this.btnOutputMode.UseVisualStyleBackColor = false;
            this.btnOutputMode.Click += new System.EventHandler(this.btnModifyMode_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTop.Controls.Add(this.btnMotionPage);
            this.pnlTop.Controls.Add(this.btnAxisPage);
            this.pnlTop.Controls.Add(this.btnFullClosedPage);
            this.pnlTop.Controls.Add(this.btnIOPage);
            this.pnlTop.Controls.Add(this.btnOutputMode);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1689, 87);
            this.pnlTop.TabIndex = 3;
            // 
            // btnAxisPage
            // 
            this.btnAxisPage.BackColor = System.Drawing.Color.White;
            this.btnAxisPage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAxisPage.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAxisPage.Location = new System.Drawing.Point(205, 3);
            this.btnAxisPage.Name = "btnAxisPage";
            this.btnAxisPage.Size = new System.Drawing.Size(196, 77);
            this.btnAxisPage.TabIndex = 5;
            this.btnAxisPage.Text = "Axis";
            this.btnAxisPage.UseVisualStyleBackColor = false;
            // 
            // btnFullClosedPage
            // 
            this.btnFullClosedPage.BackColor = System.Drawing.Color.White;
            this.btnFullClosedPage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFullClosedPage.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFullClosedPage.Location = new System.Drawing.Point(609, 3);
            this.btnFullClosedPage.Name = "btnFullClosedPage";
            this.btnFullClosedPage.Size = new System.Drawing.Size(196, 77);
            this.btnFullClosedPage.TabIndex = 4;
            this.btnFullClosedPage.Text = "Full Closed";
            this.btnFullClosedPage.UseVisualStyleBackColor = false;
            // 
            // btnIOPage
            // 
            this.btnIOPage.BackColor = System.Drawing.Color.White;
            this.btnIOPage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnIOPage.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIOPage.Location = new System.Drawing.Point(3, 3);
            this.btnIOPage.Name = "btnIOPage";
            this.btnIOPage.Size = new System.Drawing.Size(196, 77);
            this.btnIOPage.TabIndex = 3;
            this.btnIOPage.Text = "I/O";
            this.btnIOPage.UseVisualStyleBackColor = false;
            // 
            // pnlCenter
            // 
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(0, 87);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Size = new System.Drawing.Size(1689, 735);
            this.pnlCenter.TabIndex = 3;
            // 
            // btnMotionPage
            // 
            this.btnMotionPage.BackColor = System.Drawing.Color.White;
            this.btnMotionPage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMotionPage.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMotionPage.Location = new System.Drawing.Point(407, 3);
            this.btnMotionPage.Name = "btnMotionPage";
            this.btnMotionPage.Size = new System.Drawing.Size(196, 77);
            this.btnMotionPage.TabIndex = 6;
            this.btnMotionPage.Text = "Motion";
            this.btnMotionPage.UseVisualStyleBackColor = false;
            // 
            // FrmStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1689, 822);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmStatus";
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOutputMode;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnIOPage;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Button btnFullClosedPage;
        private System.Windows.Forms.Button btnAxisPage;
        private System.Windows.Forms.Button btnMotionPage;
    }
}