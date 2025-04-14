namespace Synustech.BcForm
{
    partial class Calculator
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
            this.TLP_Calculator = new System.Windows.Forms.TableLayoutPanel();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnKeyDot = new System.Windows.Forms.Button();
            this.btnKey0 = new System.Windows.Forms.Button();
            this.btnKeyMinus = new System.Windows.Forms.Button();
            this.btnKey3 = new System.Windows.Forms.Button();
            this.btnKey2 = new System.Windows.Forms.Button();
            this.btnKey1 = new System.Windows.Forms.Button();
            this.btnKey6 = new System.Windows.Forms.Button();
            this.btnKey5 = new System.Windows.Forms.Button();
            this.btnKey4 = new System.Windows.Forms.Button();
            this.btnKey9 = new System.Windows.Forms.Button();
            this.btnKey8 = new System.Windows.Forms.Button();
            this.btnKey7 = new System.Windows.Forms.Button();
            this.btnKeyEnter = new System.Windows.Forms.Button();
            this.lblSendValue = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnX = new System.Windows.Forms.Button();
            this.TLP_Calculator.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TLP_Calculator
            // 
            this.TLP_Calculator.ColumnCount = 4;
            this.TLP_Calculator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TLP_Calculator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TLP_Calculator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TLP_Calculator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TLP_Calculator.Controls.Add(this.btnReset, 3, 1);
            this.TLP_Calculator.Controls.Add(this.btnRemove, 3, 0);
            this.TLP_Calculator.Controls.Add(this.btnKeyDot, 2, 3);
            this.TLP_Calculator.Controls.Add(this.btnKey0, 1, 3);
            this.TLP_Calculator.Controls.Add(this.btnKeyMinus, 0, 3);
            this.TLP_Calculator.Controls.Add(this.btnKey3, 2, 2);
            this.TLP_Calculator.Controls.Add(this.btnKey2, 1, 2);
            this.TLP_Calculator.Controls.Add(this.btnKey1, 0, 2);
            this.TLP_Calculator.Controls.Add(this.btnKey6, 2, 1);
            this.TLP_Calculator.Controls.Add(this.btnKey5, 1, 1);
            this.TLP_Calculator.Controls.Add(this.btnKey4, 0, 1);
            this.TLP_Calculator.Controls.Add(this.btnKey9, 2, 0);
            this.TLP_Calculator.Controls.Add(this.btnKey8, 1, 0);
            this.TLP_Calculator.Controls.Add(this.btnKey7, 0, 0);
            this.TLP_Calculator.Controls.Add(this.btnKeyEnter, 3, 2);
            this.TLP_Calculator.Location = new System.Drawing.Point(17, 142);
            this.TLP_Calculator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TLP_Calculator.Name = "TLP_Calculator";
            this.TLP_Calculator.RowCount = 4;
            this.TLP_Calculator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TLP_Calculator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.17007F));
            this.TLP_Calculator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.4898F));
            this.TLP_Calculator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TLP_Calculator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TLP_Calculator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TLP_Calculator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TLP_Calculator.Size = new System.Drawing.Size(333, 414);
            this.TLP_Calculator.TabIndex = 7;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Crimson;
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnReset.Location = new System.Drawing.Point(253, 107);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(76, 96);
            this.btnReset.TabIndex = 17;
            this.btnReset.Text = "AC";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnRemove.Image = global::Synustech.Properties.Resources.Remove_;
            this.btnRemove.Location = new System.Drawing.Point(253, 4);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(76, 95);
            this.btnRemove.TabIndex = 16;
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnKeyDot
            // 
            this.btnKeyDot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKeyDot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyDot.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKeyDot.ForeColor = System.Drawing.Color.Silver;
            this.btnKeyDot.Location = new System.Drawing.Point(170, 312);
            this.btnKeyDot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKeyDot.Name = "btnKeyDot";
            this.btnKeyDot.Size = new System.Drawing.Size(74, 98);
            this.btnKeyDot.TabIndex = 14;
            this.btnKeyDot.Text = ".";
            this.btnKeyDot.UseVisualStyleBackColor = false;
            this.btnKeyDot.Click += new System.EventHandler(this.btnKeyDot_Click);
            // 
            // btnKey0
            // 
            this.btnKey0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKey0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKey0.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKey0.ForeColor = System.Drawing.Color.Silver;
            this.btnKey0.Location = new System.Drawing.Point(87, 312);
            this.btnKey0.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKey0.Name = "btnKey0";
            this.btnKey0.Size = new System.Drawing.Size(74, 98);
            this.btnKey0.TabIndex = 13;
            this.btnKey0.Text = "0";
            this.btnKey0.UseVisualStyleBackColor = false;
            this.btnKey0.Click += new System.EventHandler(this.btnKey0_Click);
            // 
            // btnKeyMinus
            // 
            this.btnKeyMinus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKeyMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyMinus.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKeyMinus.ForeColor = System.Drawing.Color.Silver;
            this.btnKeyMinus.Location = new System.Drawing.Point(4, 312);
            this.btnKeyMinus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKeyMinus.Name = "btnKeyMinus";
            this.btnKeyMinus.Size = new System.Drawing.Size(74, 98);
            this.btnKeyMinus.TabIndex = 12;
            this.btnKeyMinus.Text = "-";
            this.btnKeyMinus.UseVisualStyleBackColor = false;
            // 
            // btnKey3
            // 
            this.btnKey3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKey3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKey3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKey3.ForeColor = System.Drawing.Color.Silver;
            this.btnKey3.Location = new System.Drawing.Point(170, 211);
            this.btnKey3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKey3.Name = "btnKey3";
            this.btnKey3.Size = new System.Drawing.Size(74, 92);
            this.btnKey3.TabIndex = 10;
            this.btnKey3.Text = "3";
            this.btnKey3.UseVisualStyleBackColor = false;
            this.btnKey3.Click += new System.EventHandler(this.btnKey3_Click);
            // 
            // btnKey2
            // 
            this.btnKey2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKey2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKey2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKey2.ForeColor = System.Drawing.Color.Silver;
            this.btnKey2.Location = new System.Drawing.Point(87, 211);
            this.btnKey2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKey2.Name = "btnKey2";
            this.btnKey2.Size = new System.Drawing.Size(74, 92);
            this.btnKey2.TabIndex = 9;
            this.btnKey2.Text = "2";
            this.btnKey2.UseVisualStyleBackColor = false;
            this.btnKey2.Click += new System.EventHandler(this.btnKey2_Click);
            // 
            // btnKey1
            // 
            this.btnKey1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKey1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKey1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKey1.ForeColor = System.Drawing.Color.Silver;
            this.btnKey1.Location = new System.Drawing.Point(4, 211);
            this.btnKey1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKey1.Name = "btnKey1";
            this.btnKey1.Size = new System.Drawing.Size(74, 92);
            this.btnKey1.TabIndex = 8;
            this.btnKey1.Text = "1";
            this.btnKey1.UseVisualStyleBackColor = false;
            this.btnKey1.Click += new System.EventHandler(this.btnKey1_Click);
            // 
            // btnKey6
            // 
            this.btnKey6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKey6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKey6.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKey6.ForeColor = System.Drawing.Color.Silver;
            this.btnKey6.Location = new System.Drawing.Point(170, 107);
            this.btnKey6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKey6.Name = "btnKey6";
            this.btnKey6.Size = new System.Drawing.Size(74, 94);
            this.btnKey6.TabIndex = 6;
            this.btnKey6.Text = "6";
            this.btnKey6.UseVisualStyleBackColor = false;
            this.btnKey6.Click += new System.EventHandler(this.btnKey6_Click);
            // 
            // btnKey5
            // 
            this.btnKey5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKey5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKey5.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKey5.ForeColor = System.Drawing.Color.Silver;
            this.btnKey5.Location = new System.Drawing.Point(87, 107);
            this.btnKey5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKey5.Name = "btnKey5";
            this.btnKey5.Size = new System.Drawing.Size(74, 94);
            this.btnKey5.TabIndex = 5;
            this.btnKey5.Text = "5";
            this.btnKey5.UseVisualStyleBackColor = false;
            this.btnKey5.Click += new System.EventHandler(this.btnKey5_Click);
            // 
            // btnKey4
            // 
            this.btnKey4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKey4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKey4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKey4.ForeColor = System.Drawing.Color.Silver;
            this.btnKey4.Location = new System.Drawing.Point(4, 107);
            this.btnKey4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKey4.Name = "btnKey4";
            this.btnKey4.Size = new System.Drawing.Size(74, 94);
            this.btnKey4.TabIndex = 4;
            this.btnKey4.Text = "4";
            this.btnKey4.UseVisualStyleBackColor = false;
            this.btnKey4.Click += new System.EventHandler(this.btnKey4_Click);
            // 
            // btnKey9
            // 
            this.btnKey9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKey9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKey9.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKey9.ForeColor = System.Drawing.Color.Silver;
            this.btnKey9.Location = new System.Drawing.Point(170, 4);
            this.btnKey9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKey9.Name = "btnKey9";
            this.btnKey9.Size = new System.Drawing.Size(74, 94);
            this.btnKey9.TabIndex = 2;
            this.btnKey9.Text = "9";
            this.btnKey9.UseVisualStyleBackColor = false;
            this.btnKey9.Click += new System.EventHandler(this.btnKey9_Click);
            // 
            // btnKey8
            // 
            this.btnKey8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKey8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKey8.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKey8.ForeColor = System.Drawing.Color.Silver;
            this.btnKey8.Location = new System.Drawing.Point(87, 4);
            this.btnKey8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKey8.Name = "btnKey8";
            this.btnKey8.Size = new System.Drawing.Size(74, 94);
            this.btnKey8.TabIndex = 1;
            this.btnKey8.Text = "8";
            this.btnKey8.UseVisualStyleBackColor = false;
            this.btnKey8.Click += new System.EventHandler(this.btnKey8_Click);
            // 
            // btnKey7
            // 
            this.btnKey7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKey7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKey7.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKey7.ForeColor = System.Drawing.Color.Silver;
            this.btnKey7.Location = new System.Drawing.Point(4, 4);
            this.btnKey7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKey7.Name = "btnKey7";
            this.btnKey7.Size = new System.Drawing.Size(74, 94);
            this.btnKey7.TabIndex = 0;
            this.btnKey7.Text = "7";
            this.btnKey7.UseVisualStyleBackColor = false;
            this.btnKey7.Click += new System.EventHandler(this.btnKey7_Click);
            // 
            // btnKeyEnter
            // 
            this.btnKeyEnter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnKeyEnter.BackgroundImage = global::Synustech.Properties.Resources.Enter_Re;
            this.btnKeyEnter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnKeyEnter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnKeyEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyEnter.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKeyEnter.ForeColor = System.Drawing.Color.Silver;
            this.btnKeyEnter.Location = new System.Drawing.Point(253, 211);
            this.btnKeyEnter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKeyEnter.Name = "btnKeyEnter";
            this.TLP_Calculator.SetRowSpan(this.btnKeyEnter, 2);
            this.btnKeyEnter.Size = new System.Drawing.Size(76, 199);
            this.btnKeyEnter.TabIndex = 15;
            this.btnKeyEnter.UseVisualStyleBackColor = false;
            this.btnKeyEnter.Click += new System.EventHandler(this.btnKeyEnter_Click);
            // 
            // lblSendValue
            // 
            this.lblSendValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSendValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSendValue.Font = new System.Drawing.Font("Nirmala UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSendValue.ForeColor = System.Drawing.Color.Silver;
            this.lblSendValue.Location = new System.Drawing.Point(23, 78);
            this.lblSendValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSendValue.Name = "lblSendValue";
            this.lblSendValue.Size = new System.Drawing.Size(321, 52);
            this.lblSendValue.TabIndex = 8;
            this.lblSendValue.Text = "0";
            this.lblSendValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(71)))), ((int)(((byte)(93)))));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.4054F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.59459F));
            this.tableLayoutPanel1.Controls.Add(this.btnX, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(370, 57);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // btnX
            // 
            this.btnX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnX.FlatAppearance.BorderSize = 0;
            this.btnX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnX.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnX.ForeColor = System.Drawing.Color.White;
            this.btnX.Location = new System.Drawing.Point(319, 3);
            this.btnX.Name = "btnX";
            this.btnX.Size = new System.Drawing.Size(48, 51);
            this.btnX.TabIndex = 1;
            this.btnX.Text = "X";
            this.btnX.UseVisualStyleBackColor = true;
            this.btnX.Click += new System.EventHandler(this.btnX_Click);
            // 
            // Calculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(370, 574);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblSendValue);
            this.Controls.Add(this.TLP_Calculator);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Calculator";
            this.Text = "Form1";
            this.TLP_Calculator.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TLP_Calculator;
        private System.Windows.Forms.Button btnKey7;
        private System.Windows.Forms.Button btnKeyDot;
        private System.Windows.Forms.Button btnKey0;
        private System.Windows.Forms.Button btnKeyMinus;
        private System.Windows.Forms.Button btnKey3;
        private System.Windows.Forms.Button btnKey2;
        private System.Windows.Forms.Button btnKey1;
        private System.Windows.Forms.Button btnKey6;
        private System.Windows.Forms.Button btnKey5;
        private System.Windows.Forms.Button btnKey4;
        private System.Windows.Forms.Button btnKey9;
        private System.Windows.Forms.Button btnKey8;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnKeyEnter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnX;
        private System.Windows.Forms.Label lblSendValue;
    }
}