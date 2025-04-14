
namespace Master.SubForm
{
    partial class Frm_Language
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
            this.cbx_LanguageNation = new System.Windows.Forms.ComboBox();
            this.lbl_LanguageTitle = new System.Windows.Forms.Label();
            this.btn_Apply = new System.Windows.Forms.Button();
            this.btn_LangPackReload = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.cbx_LanguageNation, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_LanguageTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_Apply, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btn_LangPackReload, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 91);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // cbx_LanguageNation
            // 
            this.cbx_LanguageNation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbx_LanguageNation.FormattingEnabled = true;
            this.cbx_LanguageNation.Location = new System.Drawing.Point(153, 4);
            this.cbx_LanguageNation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbx_LanguageNation.Name = "cbx_LanguageNation";
            this.cbx_LanguageNation.Size = new System.Drawing.Size(228, 25);
            this.cbx_LanguageNation.TabIndex = 44;
            // 
            // lbl_LanguageTitle
            // 
            this.lbl_LanguageTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_LanguageTitle.Location = new System.Drawing.Point(3, 0);
            this.lbl_LanguageTitle.Name = "lbl_LanguageTitle";
            this.lbl_LanguageTitle.Size = new System.Drawing.Size(144, 35);
            this.lbl_LanguageTitle.TabIndex = 6;
            this.lbl_LanguageTitle.Text = "Language";
            this.lbl_LanguageTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_Apply
            // 
            this.btn_Apply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Apply.Location = new System.Drawing.Point(153, 39);
            this.btn_Apply.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Apply.Name = "btn_Apply";
            this.btn_Apply.Size = new System.Drawing.Size(228, 48);
            this.btn_Apply.TabIndex = 3;
            this.btn_Apply.Text = "Apply";
            this.btn_Apply.UseVisualStyleBackColor = true;
            this.btn_Apply.Click += new System.EventHandler(this.btn_Apply_Click);
            // 
            // btn_LangPackReload
            // 
            this.btn_LangPackReload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_LangPackReload.Location = new System.Drawing.Point(3, 38);
            this.btn_LangPackReload.Name = "btn_LangPackReload";
            this.btn_LangPackReload.Size = new System.Drawing.Size(144, 50);
            this.btn_LangPackReload.TabIndex = 45;
            this.btn_LangPackReload.Text = "LangPack Reload";
            this.btn_LangPackReload.UseVisualStyleBackColor = true;
            this.btn_LangPackReload.Click += new System.EventHandler(this.btn_LangPackReload_Click);
            // 
            // Frm_Language
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(384, 91);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(400, 130);
            this.MinimumSize = new System.Drawing.Size(400, 130);
            this.Name = "Frm_Language";
            this.Text = "Language";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_Apply;
        private System.Windows.Forms.Label lbl_LanguageTitle;
        private System.Windows.Forms.ComboBox cbx_LanguageNation;
        private System.Windows.Forms.Button btn_LangPackReload;
    }
}