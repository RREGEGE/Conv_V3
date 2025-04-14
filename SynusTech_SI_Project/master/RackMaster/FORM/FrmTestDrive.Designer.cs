
namespace RackMaster.FORM {
    partial class FrmTestDrive {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtFromID = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnFromRun = new System.Windows.Forms.Button();
            this.txtToID = new System.Windows.Forms.TextBox();
            this.btnToRun = new System.Windows.Forms.Button();
            this.btnErrorReset = new System.Windows.Forms.Button();
            this.btnForkHoming = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCurrentStep = new System.Windows.Forms.TextBox();
            this.txtCSTOn = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtErrorCode = new System.Windows.Forms.TextBox();
            this.btnFromXZTGoing = new System.Windows.Forms.Button();
            this.btnFromForkFWD = new System.Windows.Forms.Button();
            this.btnFromZUp = new System.Windows.Forms.Button();
            this.btnFromForkBWD = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnToForkBWD = new System.Windows.Forms.Button();
            this.btnToZDown = new System.Windows.Forms.Button();
            this.btnToForkFWD = new System.Windows.Forms.Button();
            this.btnToXZTGoing = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.lblFromIDError = new System.Windows.Forms.Label();
            this.lblToIDError = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnFromForkBWD);
            this.groupBox1.Controls.Add(this.btnFromZUp);
            this.groupBox1.Controls.Add(this.btnFromForkFWD);
            this.groupBox1.Controls.Add(this.btnFromXZTGoing);
            this.groupBox1.Controls.Add(this.pictureBox4);
            this.groupBox1.Controls.Add(this.pictureBox3);
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(489, 817);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "From Mode";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::RackMaster.Properties.Resources.icons8_down_96;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(200, 176);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(77, 67);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = global::RackMaster.Properties.Resources.icons8_down_96;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.Location = new System.Drawing.Point(200, 372);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(77, 67);
            this.pictureBox3.TabIndex = 5;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = global::RackMaster.Properties.Resources.icons8_down_96;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox4.Location = new System.Drawing.Point(200, 568);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(77, 67);
            this.pictureBox4.TabIndex = 7;
            this.pictureBox4.TabStop = false;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(1023, 36);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(183, 62);
            this.label13.TabIndex = 12;
            this.label13.Text = "From ID : ";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFromID
            // 
            this.txtFromID.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFromID.Location = new System.Drawing.Point(1212, 39);
            this.txtFromID.Name = "txtFromID";
            this.txtFromID.Size = new System.Drawing.Size(317, 57);
            this.txtFromID.TabIndex = 13;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(1063, 264);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(143, 62);
            this.label14.TabIndex = 14;
            this.label14.Text = "To ID : ";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFromRun
            // 
            this.btnFromRun.BackColor = System.Drawing.Color.AliceBlue;
            this.btnFromRun.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFromRun.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFromRun.Location = new System.Drawing.Point(1032, 104);
            this.btnFromRun.Name = "btnFromRun";
            this.btnFromRun.Size = new System.Drawing.Size(645, 152);
            this.btnFromRun.TabIndex = 15;
            this.btnFromRun.Text = "From Run";
            this.btnFromRun.UseVisualStyleBackColor = false;
            // 
            // txtToID
            // 
            this.txtToID.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToID.Location = new System.Drawing.Point(1212, 267);
            this.txtToID.Name = "txtToID";
            this.txtToID.Size = new System.Drawing.Size(317, 57);
            this.txtToID.TabIndex = 16;
            // 
            // btnToRun
            // 
            this.btnToRun.BackColor = System.Drawing.Color.AliceBlue;
            this.btnToRun.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnToRun.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToRun.Location = new System.Drawing.Point(1032, 330);
            this.btnToRun.Name = "btnToRun";
            this.btnToRun.Size = new System.Drawing.Size(645, 152);
            this.btnToRun.TabIndex = 17;
            this.btnToRun.Text = "To Run";
            this.btnToRun.UseVisualStyleBackColor = false;
            // 
            // btnErrorReset
            // 
            this.btnErrorReset.BackColor = System.Drawing.Color.AliceBlue;
            this.btnErrorReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnErrorReset.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnErrorReset.Location = new System.Drawing.Point(1032, 677);
            this.btnErrorReset.Name = "btnErrorReset";
            this.btnErrorReset.Size = new System.Drawing.Size(645, 152);
            this.btnErrorReset.TabIndex = 18;
            this.btnErrorReset.Text = "Error Reset";
            this.btnErrorReset.UseVisualStyleBackColor = false;
            this.btnErrorReset.Click += new System.EventHandler(this.btnErrorReset_Click);
            // 
            // btnForkHoming
            // 
            this.btnForkHoming.BackColor = System.Drawing.Color.AliceBlue;
            this.btnForkHoming.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnForkHoming.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnForkHoming.Location = new System.Drawing.Point(1032, 488);
            this.btnForkHoming.Name = "btnForkHoming";
            this.btnForkHoming.Size = new System.Drawing.Size(271, 183);
            this.btnForkHoming.TabIndex = 19;
            this.btnForkHoming.Text = "Fork Homing";
            this.btnForkHoming.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1305, 491);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 30);
            this.label1.TabIndex = 20;
            this.label1.Text = "Current Step :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1313, 532);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 30);
            this.label2.TabIndex = 21;
            this.label2.Text = "Cassette On :";
            // 
            // txtCurrentStep
            // 
            this.txtCurrentStep.BackColor = System.Drawing.Color.White;
            this.txtCurrentStep.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentStep.Location = new System.Drawing.Point(1459, 488);
            this.txtCurrentStep.Name = "txtCurrentStep";
            this.txtCurrentStep.ReadOnly = true;
            this.txtCurrentStep.Size = new System.Drawing.Size(208, 35);
            this.txtCurrentStep.TabIndex = 22;
            // 
            // txtCSTOn
            // 
            this.txtCSTOn.BackColor = System.Drawing.Color.White;
            this.txtCSTOn.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCSTOn.Location = new System.Drawing.Point(1459, 529);
            this.txtCSTOn.Name = "txtCSTOn";
            this.txtCSTOn.ReadOnly = true;
            this.txtCSTOn.Size = new System.Drawing.Size(208, 35);
            this.txtCSTOn.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1324, 573);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 30);
            this.label3.TabIndex = 24;
            this.label3.Text = "Error Code :";
            // 
            // txtErrorCode
            // 
            this.txtErrorCode.BackColor = System.Drawing.Color.White;
            this.txtErrorCode.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtErrorCode.Location = new System.Drawing.Point(1459, 570);
            this.txtErrorCode.Name = "txtErrorCode";
            this.txtErrorCode.ReadOnly = true;
            this.txtErrorCode.Size = new System.Drawing.Size(208, 35);
            this.txtErrorCode.TabIndex = 25;
            // 
            // btnFromXZTGoing
            // 
            this.btnFromXZTGoing.BackColor = System.Drawing.Color.AliceBlue;
            this.btnFromXZTGoing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFromXZTGoing.Location = new System.Drawing.Point(6, 53);
            this.btnFromXZTGoing.Name = "btnFromXZTGoing";
            this.btnFromXZTGoing.Size = new System.Drawing.Size(477, 117);
            this.btnFromXZTGoing.TabIndex = 11;
            this.btnFromXZTGoing.Text = "XZT Going";
            this.btnFromXZTGoing.UseVisualStyleBackColor = false;
            // 
            // btnFromForkFWD
            // 
            this.btnFromForkFWD.BackColor = System.Drawing.Color.AliceBlue;
            this.btnFromForkFWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFromForkFWD.Location = new System.Drawing.Point(6, 249);
            this.btnFromForkFWD.Name = "btnFromForkFWD";
            this.btnFromForkFWD.Size = new System.Drawing.Size(477, 117);
            this.btnFromForkFWD.TabIndex = 15;
            this.btnFromForkFWD.Text = "Fork Forward";
            this.btnFromForkFWD.UseVisualStyleBackColor = false;
            // 
            // btnFromZUp
            // 
            this.btnFromZUp.BackColor = System.Drawing.Color.AliceBlue;
            this.btnFromZUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFromZUp.Location = new System.Drawing.Point(6, 445);
            this.btnFromZUp.Name = "btnFromZUp";
            this.btnFromZUp.Size = new System.Drawing.Size(477, 117);
            this.btnFromZUp.TabIndex = 16;
            this.btnFromZUp.Text = "Z-Up";
            this.btnFromZUp.UseVisualStyleBackColor = false;
            // 
            // btnFromForkBWD
            // 
            this.btnFromForkBWD.BackColor = System.Drawing.Color.AliceBlue;
            this.btnFromForkBWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFromForkBWD.Location = new System.Drawing.Point(6, 641);
            this.btnFromForkBWD.Name = "btnFromForkBWD";
            this.btnFromForkBWD.Size = new System.Drawing.Size(477, 117);
            this.btnFromForkBWD.TabIndex = 17;
            this.btnFromForkBWD.Text = "Fork Backward";
            this.btnFromForkBWD.UseVisualStyleBackColor = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnToForkBWD);
            this.groupBox2.Controls.Add(this.btnToZDown);
            this.groupBox2.Controls.Add(this.btnToForkFWD);
            this.groupBox2.Controls.Add(this.btnToXZTGoing);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.pictureBox5);
            this.groupBox2.Controls.Add(this.pictureBox6);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(507, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(489, 817);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "To Mode";
            // 
            // btnToForkBWD
            // 
            this.btnToForkBWD.BackColor = System.Drawing.Color.AliceBlue;
            this.btnToForkBWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToForkBWD.Location = new System.Drawing.Point(6, 641);
            this.btnToForkBWD.Name = "btnToForkBWD";
            this.btnToForkBWD.Size = new System.Drawing.Size(477, 117);
            this.btnToForkBWD.TabIndex = 17;
            this.btnToForkBWD.Text = "Fork Backward";
            this.btnToForkBWD.UseVisualStyleBackColor = false;
            // 
            // btnToZDown
            // 
            this.btnToZDown.BackColor = System.Drawing.Color.AliceBlue;
            this.btnToZDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToZDown.Location = new System.Drawing.Point(6, 445);
            this.btnToZDown.Name = "btnToZDown";
            this.btnToZDown.Size = new System.Drawing.Size(477, 117);
            this.btnToZDown.TabIndex = 16;
            this.btnToZDown.Text = "Z-Down";
            this.btnToZDown.UseVisualStyleBackColor = false;
            // 
            // btnToForkFWD
            // 
            this.btnToForkFWD.BackColor = System.Drawing.Color.AliceBlue;
            this.btnToForkFWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToForkFWD.Location = new System.Drawing.Point(6, 249);
            this.btnToForkFWD.Name = "btnToForkFWD";
            this.btnToForkFWD.Size = new System.Drawing.Size(477, 117);
            this.btnToForkFWD.TabIndex = 15;
            this.btnToForkFWD.Text = "Fork Forward";
            this.btnToForkFWD.UseVisualStyleBackColor = false;
            // 
            // btnToXZTGoing
            // 
            this.btnToXZTGoing.BackColor = System.Drawing.Color.AliceBlue;
            this.btnToXZTGoing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToXZTGoing.Location = new System.Drawing.Point(6, 53);
            this.btnToXZTGoing.Name = "btnToXZTGoing";
            this.btnToXZTGoing.Size = new System.Drawing.Size(477, 117);
            this.btnToXZTGoing.TabIndex = 11;
            this.btnToXZTGoing.Text = "XZT Going";
            this.btnToXZTGoing.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::RackMaster.Properties.Resources.icons8_down_96;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(200, 568);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(77, 67);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackgroundImage = global::RackMaster.Properties.Resources.icons8_down_96;
            this.pictureBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox5.Location = new System.Drawing.Point(200, 372);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(77, 67);
            this.pictureBox5.TabIndex = 5;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackgroundImage = global::RackMaster.Properties.Resources.icons8_down_96;
            this.pictureBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox6.Location = new System.Drawing.Point(200, 176);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(77, 67);
            this.pictureBox6.TabIndex = 3;
            this.pictureBox6.TabStop = false;
            // 
            // lblFromIDError
            // 
            this.lblFromIDError.AutoSize = true;
            this.lblFromIDError.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromIDError.Location = new System.Drawing.Point(1535, 42);
            this.lblFromIDError.Name = "lblFromIDError";
            this.lblFromIDError.Size = new System.Drawing.Size(142, 50);
            this.lblFromIDError.TabIndex = 26;
            this.lblFromIDError.Text = "ERROR";
            // 
            // lblToIDError
            // 
            this.lblToIDError.AutoSize = true;
            this.lblToIDError.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToIDError.Location = new System.Drawing.Point(1535, 270);
            this.lblToIDError.Name = "lblToIDError";
            this.lblToIDError.Size = new System.Drawing.Size(142, 50);
            this.lblToIDError.TabIndex = 27;
            this.lblToIDError.Text = "ERROR";
            // 
            // FrmTestDrive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1689, 841);
            this.Controls.Add(this.lblToIDError);
            this.Controls.Add(this.lblFromIDError);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtErrorCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCSTOn);
            this.Controls.Add(this.txtCurrentStep);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnForkHoming);
            this.Controls.Add(this.btnErrorReset);
            this.Controls.Add(this.btnToRun);
            this.Controls.Add(this.txtToID);
            this.Controls.Add(this.btnFromRun);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtFromID);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmTestDrive";
            this.Text = "FrmTestDrive";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtFromID;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnFromRun;
        private System.Windows.Forms.TextBox txtToID;
        private System.Windows.Forms.Button btnToRun;
        private System.Windows.Forms.Button btnErrorReset;
        private System.Windows.Forms.Button btnForkHoming;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCurrentStep;
        private System.Windows.Forms.TextBox txtCSTOn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtErrorCode;
        private System.Windows.Forms.Button btnFromForkBWD;
        private System.Windows.Forms.Button btnFromZUp;
        private System.Windows.Forms.Button btnFromForkFWD;
        private System.Windows.Forms.Button btnFromXZTGoing;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnToForkBWD;
        private System.Windows.Forms.Button btnToZDown;
        private System.Windows.Forms.Button btnToForkFWD;
        private System.Windows.Forms.Button btnToXZTGoing;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Label lblFromIDError;
        private System.Windows.Forms.Label lblToIDError;
    }
}