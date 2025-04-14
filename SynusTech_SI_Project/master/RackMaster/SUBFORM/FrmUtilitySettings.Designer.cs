
namespace RackMaster.SUBFORM {
    partial class FrmUtilitySettings {
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
            this.components = new System.ComponentModel.Container();
            this.pnlSide = new System.Windows.Forms.Panel();
            this.listMenu = new System.Windows.Forms.ListBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlSystem = new System.Windows.Forms.Panel();
            this.btnSaveSytem = new System.Windows.Forms.Button();
            this.cboxModifySyncTime = new System.Windows.Forms.CheckBox();
            this.pnlEtherCAT = new System.Windows.Forms.Panel();
            this.btnEtherCATSave = new System.Windows.Forms.Button();
            this.cboxAutoRecovery = new System.Windows.Forms.CheckBox();
            this.pnlPassword = new System.Windows.Forms.Panel();
            this.btnPasswordSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNewPasswordConfirm = new System.Windows.Forms.TextBox();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCurrentPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.gboxIO = new System.Windows.Forms.GroupBox();
            this.radIO_Convert = new System.Windows.Forms.RadioButton();
            this.radIO_Raw = new System.Windows.Forms.RadioButton();
            this.btnUISave = new System.Windows.Forms.Button();
            this.pnlLang = new System.Windows.Forms.Panel();
            this.btnLangSave = new System.Windows.Forms.Button();
            this.radioEnglish = new System.Windows.Forms.RadioButton();
            this.radioKorean = new System.Windows.Forms.RadioButton();
            this.saveTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlSide.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlSystem.SuspendLayout();
            this.pnlEtherCAT.SuspendLayout();
            this.pnlPassword.SuspendLayout();
            this.pnlUI.SuspendLayout();
            this.gboxIO.SuspendLayout();
            this.pnlLang.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSide
            // 
            this.pnlSide.Controls.Add(this.listMenu);
            this.pnlSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSide.Location = new System.Drawing.Point(0, 0);
            this.pnlSide.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlSide.Name = "pnlSide";
            this.pnlSide.Size = new System.Drawing.Size(216, 541);
            this.pnlSide.TabIndex = 0;
            // 
            // listMenu
            // 
            this.listMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listMenu.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listMenu.FormattingEnabled = true;
            this.listMenu.ItemHeight = 25;
            this.listMenu.Items.AddRange(new object[] {
            "Password",
            "Language",
            "UI",
            "EtherCAT",
            "System"});
            this.listMenu.Location = new System.Drawing.Point(0, 0);
            this.listMenu.Name = "listMenu";
            this.listMenu.Size = new System.Drawing.Size(216, 541);
            this.listMenu.TabIndex = 0;
            this.listMenu.SelectedIndexChanged += new System.EventHandler(this.listMenu_SelectedIndexChanged);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlSystem);
            this.pnlMain.Controls.Add(this.pnlEtherCAT);
            this.pnlMain.Controls.Add(this.pnlPassword);
            this.pnlMain.Controls.Add(this.pnlUI);
            this.pnlMain.Controls.Add(this.pnlLang);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(216, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(496, 541);
            this.pnlMain.TabIndex = 1;
            // 
            // pnlSystem
            // 
            this.pnlSystem.Controls.Add(this.btnSaveSytem);
            this.pnlSystem.Controls.Add(this.cboxModifySyncTime);
            this.pnlSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSystem.Location = new System.Drawing.Point(0, 0);
            this.pnlSystem.Name = "pnlSystem";
            this.pnlSystem.Size = new System.Drawing.Size(496, 541);
            this.pnlSystem.TabIndex = 10;
            // 
            // btnSaveSytem
            // 
            this.btnSaveSytem.BackColor = System.Drawing.Color.White;
            this.btnSaveSytem.Location = new System.Drawing.Point(9, 95);
            this.btnSaveSytem.Name = "btnSaveSytem";
            this.btnSaveSytem.Size = new System.Drawing.Size(232, 89);
            this.btnSaveSytem.TabIndex = 9;
            this.btnSaveSytem.Text = "SAVE";
            this.btnSaveSytem.UseVisualStyleBackColor = false;
            // 
            // cboxModifySyncTime
            // 
            this.cboxModifySyncTime.AutoSize = true;
            this.cboxModifySyncTime.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxModifySyncTime.Location = new System.Drawing.Point(28, 23);
            this.cboxModifySyncTime.Name = "cboxModifySyncTime";
            this.cboxModifySyncTime.Size = new System.Drawing.Size(190, 29);
            this.cboxModifySyncTime.TabIndex = 0;
            this.cboxModifySyncTime.Text = "Modify Sync Time";
            this.cboxModifySyncTime.UseVisualStyleBackColor = true;
            // 
            // pnlEtherCAT
            // 
            this.pnlEtherCAT.Controls.Add(this.btnEtherCATSave);
            this.pnlEtherCAT.Controls.Add(this.cboxAutoRecovery);
            this.pnlEtherCAT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEtherCAT.Location = new System.Drawing.Point(0, 0);
            this.pnlEtherCAT.Name = "pnlEtherCAT";
            this.pnlEtherCAT.Size = new System.Drawing.Size(496, 541);
            this.pnlEtherCAT.TabIndex = 9;
            // 
            // btnEtherCATSave
            // 
            this.btnEtherCATSave.BackColor = System.Drawing.Color.White;
            this.btnEtherCATSave.Location = new System.Drawing.Point(9, 95);
            this.btnEtherCATSave.Name = "btnEtherCATSave";
            this.btnEtherCATSave.Size = new System.Drawing.Size(232, 89);
            this.btnEtherCATSave.TabIndex = 9;
            this.btnEtherCATSave.Text = "SAVE";
            this.btnEtherCATSave.UseVisualStyleBackColor = false;
            // 
            // cboxAutoRecovery
            // 
            this.cboxAutoRecovery.AutoSize = true;
            this.cboxAutoRecovery.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxAutoRecovery.Location = new System.Drawing.Point(28, 23);
            this.cboxAutoRecovery.Name = "cboxAutoRecovery";
            this.cboxAutoRecovery.Size = new System.Drawing.Size(162, 29);
            this.cboxAutoRecovery.TabIndex = 0;
            this.cboxAutoRecovery.Text = "Auto Recovery";
            this.cboxAutoRecovery.UseVisualStyleBackColor = true;
            // 
            // pnlPassword
            // 
            this.pnlPassword.Controls.Add(this.btnPasswordSave);
            this.pnlPassword.Controls.Add(this.label3);
            this.pnlPassword.Controls.Add(this.txtNewPasswordConfirm);
            this.pnlPassword.Controls.Add(this.txtNewPassword);
            this.pnlPassword.Controls.Add(this.label2);
            this.pnlPassword.Controls.Add(this.txtCurrentPassword);
            this.pnlPassword.Controls.Add(this.label1);
            this.pnlPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPassword.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlPassword.Location = new System.Drawing.Point(0, 0);
            this.pnlPassword.Name = "pnlPassword";
            this.pnlPassword.Size = new System.Drawing.Size(496, 541);
            this.pnlPassword.TabIndex = 0;
            // 
            // btnPasswordSave
            // 
            this.btnPasswordSave.BackColor = System.Drawing.Color.White;
            this.btnPasswordSave.Location = new System.Drawing.Point(247, 142);
            this.btnPasswordSave.Name = "btnPasswordSave";
            this.btnPasswordSave.Size = new System.Drawing.Size(232, 89);
            this.btnPasswordSave.TabIndex = 7;
            this.btnPasswordSave.Text = "SAVE";
            this.btnPasswordSave.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(235, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "New Password Confirm : ";
            // 
            // txtNewPasswordConfirm
            // 
            this.txtNewPasswordConfirm.Location = new System.Drawing.Point(247, 103);
            this.txtNewPasswordConfirm.Name = "txtNewPasswordConfirm";
            this.txtNewPasswordConfirm.PasswordChar = '*';
            this.txtNewPasswordConfirm.Size = new System.Drawing.Size(233, 33);
            this.txtNewPasswordConfirm.TabIndex = 5;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Location = new System.Drawing.Point(247, 64);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '*';
            this.txtNewPassword.Size = new System.Drawing.Size(233, 33);
            this.txtNewPassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(84, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "New Password : ";
            // 
            // txtCurrentPassword
            // 
            this.txtCurrentPassword.Location = new System.Drawing.Point(247, 25);
            this.txtCurrentPassword.Name = "txtCurrentPassword";
            this.txtCurrentPassword.PasswordChar = '*';
            this.txtCurrentPassword.Size = new System.Drawing.Size(233, 33);
            this.txtCurrentPassword.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Password : ";
            // 
            // pnlUI
            // 
            this.pnlUI.Controls.Add(this.gboxIO);
            this.pnlUI.Controls.Add(this.btnUISave);
            this.pnlUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUI.Location = new System.Drawing.Point(0, 0);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(496, 541);
            this.pnlUI.TabIndex = 9;
            // 
            // gboxIO
            // 
            this.gboxIO.Controls.Add(this.radIO_Convert);
            this.gboxIO.Controls.Add(this.radIO_Raw);
            this.gboxIO.Location = new System.Drawing.Point(6, 12);
            this.gboxIO.Name = "gboxIO";
            this.gboxIO.Size = new System.Drawing.Size(487, 108);
            this.gboxIO.TabIndex = 9;
            this.gboxIO.TabStop = false;
            this.gboxIO.Text = "IO";
            // 
            // radIO_Convert
            // 
            this.radIO_Convert.AutoSize = true;
            this.radIO_Convert.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radIO_Convert.Location = new System.Drawing.Point(22, 56);
            this.radIO_Convert.Name = "radIO_Convert";
            this.radIO_Convert.Size = new System.Drawing.Size(170, 25);
            this.radIO_Convert.TabIndex = 1;
            this.radIO_Convert.Text = "Convert Data View";
            this.radIO_Convert.UseVisualStyleBackColor = true;
            // 
            // radIO_Raw
            // 
            this.radIO_Raw.AutoSize = true;
            this.radIO_Raw.Checked = true;
            this.radIO_Raw.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radIO_Raw.Location = new System.Drawing.Point(22, 25);
            this.radIO_Raw.Name = "radIO_Raw";
            this.radIO_Raw.Size = new System.Drawing.Size(142, 25);
            this.radIO_Raw.TabIndex = 0;
            this.radIO_Raw.TabStop = true;
            this.radIO_Raw.Text = "Raw Data View";
            this.radIO_Raw.UseVisualStyleBackColor = true;
            // 
            // btnUISave
            // 
            this.btnUISave.BackColor = System.Drawing.Color.White;
            this.btnUISave.Location = new System.Drawing.Point(3, 126);
            this.btnUISave.Name = "btnUISave";
            this.btnUISave.Size = new System.Drawing.Size(232, 89);
            this.btnUISave.TabIndex = 8;
            this.btnUISave.Text = "SAVE";
            this.btnUISave.UseVisualStyleBackColor = false;
            // 
            // pnlLang
            // 
            this.pnlLang.Controls.Add(this.btnLangSave);
            this.pnlLang.Controls.Add(this.radioEnglish);
            this.pnlLang.Controls.Add(this.radioKorean);
            this.pnlLang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLang.Location = new System.Drawing.Point(0, 0);
            this.pnlLang.Name = "pnlLang";
            this.pnlLang.Size = new System.Drawing.Size(496, 541);
            this.pnlLang.TabIndex = 8;
            // 
            // btnLangSave
            // 
            this.btnLangSave.BackColor = System.Drawing.Color.White;
            this.btnLangSave.Location = new System.Drawing.Point(28, 103);
            this.btnLangSave.Name = "btnLangSave";
            this.btnLangSave.Size = new System.Drawing.Size(232, 89);
            this.btnLangSave.TabIndex = 8;
            this.btnLangSave.Text = "SAVE";
            this.btnLangSave.UseVisualStyleBackColor = false;
            // 
            // radioEnglish
            // 
            this.radioEnglish.AutoSize = true;
            this.radioEnglish.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioEnglish.Location = new System.Drawing.Point(28, 60);
            this.radioEnglish.Name = "radioEnglish";
            this.radioEnglish.Size = new System.Drawing.Size(93, 29);
            this.radioEnglish.TabIndex = 1;
            this.radioEnglish.Text = "English";
            this.radioEnglish.UseVisualStyleBackColor = true;
            // 
            // radioKorean
            // 
            this.radioKorean.AutoSize = true;
            this.radioKorean.Checked = true;
            this.radioKorean.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioKorean.Location = new System.Drawing.Point(28, 25);
            this.radioKorean.Name = "radioKorean";
            this.radioKorean.Size = new System.Drawing.Size(94, 29);
            this.radioKorean.TabIndex = 0;
            this.radioKorean.TabStop = true;
            this.radioKorean.Text = "Korean";
            this.radioKorean.UseVisualStyleBackColor = true;
            // 
            // saveTimer
            // 
            this.saveTimer.Tick += new System.EventHandler(this.saveTimer_Tick);
            // 
            // FrmUtilitySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 541);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSide);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmUtilitySettings";
            this.Text = "Setting";
            this.pnlSide.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlSystem.ResumeLayout(false);
            this.pnlSystem.PerformLayout();
            this.pnlEtherCAT.ResumeLayout(false);
            this.pnlEtherCAT.PerformLayout();
            this.pnlPassword.ResumeLayout(false);
            this.pnlPassword.PerformLayout();
            this.pnlUI.ResumeLayout(false);
            this.gboxIO.ResumeLayout(false);
            this.gboxIO.PerformLayout();
            this.pnlLang.ResumeLayout(false);
            this.pnlLang.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSide;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ListBox listMenu;
        private System.Windows.Forms.Panel pnlPassword;
        private System.Windows.Forms.Button btnPasswordSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNewPasswordConfirm;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCurrentPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer saveTimer;
        private System.Windows.Forms.Panel pnlLang;
        private System.Windows.Forms.Button btnLangSave;
        private System.Windows.Forms.RadioButton radioEnglish;
        private System.Windows.Forms.RadioButton radioKorean;
        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.Button btnUISave;
        private System.Windows.Forms.GroupBox gboxIO;
        private System.Windows.Forms.RadioButton radIO_Convert;
        private System.Windows.Forms.RadioButton radIO_Raw;
        private System.Windows.Forms.Panel pnlEtherCAT;
        private System.Windows.Forms.CheckBox cboxAutoRecovery;
        private System.Windows.Forms.Button btnEtherCATSave;
        private System.Windows.Forms.Panel pnlSystem;
        private System.Windows.Forms.Button btnSaveSytem;
        private System.Windows.Forms.CheckBox cboxModifySyncTime;
    }
}