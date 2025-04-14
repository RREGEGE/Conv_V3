
namespace RackMaster {
    partial class FrmMainForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMainForm));
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblLoginDuration = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnAlarmClear = new System.Windows.Forms.Button();
            this.btnUtilitySettings = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblRamUsage = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCPUUsage = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblMaster = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblEtherCAT = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureLogo = new System.Windows.Forms.PictureBox();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblSWVersion = new System.Windows.Forms.Label();
            this.pnlMenuStrip = new System.Windows.Forms.Panel();
            this.CMSMainForm = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolMemoryMap = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlSideBar = new System.Windows.Forms.Panel();
            this.btnPageAlarmList = new System.Windows.Forms.Button();
            this.btnPageHistory = new System.Windows.Forms.Button();
            this.btnPageTestDrive = new System.Windows.Forms.Button();
            this.btnPageStatus = new System.Windows.Forms.Button();
            this.btnPageSetting = new System.Windows.Forms.Button();
            this.btnPageMotor = new System.Windows.Forms.Button();
            this.btnPageMain = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnBuzzerStop = new System.Windows.Forms.Button();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlTop.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).BeginInit();
            this.CMSMainForm.SuspendLayout();
            this.pnlSideBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.Azure;
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlBottom.Location = new System.Drawing.Point(0, 914);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1689, 147);
            this.pnlBottom.TabIndex = 1;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 92);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1689, 822);
            this.pnlMain.TabIndex = 3;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.Azure;
            this.pnlTop.Controls.Add(this.lblLoginDuration);
            this.pnlTop.Controls.Add(this.btnMinimize);
            this.pnlTop.Controls.Add(this.btnAlarmClear);
            this.pnlTop.Controls.Add(this.btnUtilitySettings);
            this.pnlTop.Controls.Add(this.tableLayoutPanel1);
            this.pnlTop.Controls.Add(this.panel2);
            this.pnlTop.Controls.Add(this.panel1);
            this.pnlTop.Controls.Add(this.pictureLogo);
            this.pnlTop.Controls.Add(this.lblCurrentTime);
            this.pnlTop.Controls.Add(this.lblSWVersion);
            this.pnlTop.Controls.Add(this.pnlMenuStrip);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1689, 92);
            this.pnlTop.TabIndex = 4;
            // 
            // lblLoginDuration
            // 
            this.lblLoginDuration.AutoSize = true;
            this.lblLoginDuration.Location = new System.Drawing.Point(9, 4);
            this.lblLoginDuration.Name = "lblLoginDuration";
            this.lblLoginDuration.Size = new System.Drawing.Size(109, 17);
            this.lblLoginDuration.TabIndex = 35;
            this.lblLoginDuration.Text = "Login Duration : ";
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.White;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMinimize.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.Location = new System.Drawing.Point(868, 4);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(93, 83);
            this.btnMinimize.TabIndex = 34;
            this.btnMinimize.Text = "Minimize";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnAlarmClear
            // 
            this.btnAlarmClear.BackColor = System.Drawing.Color.White;
            this.btnAlarmClear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAlarmClear.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlarmClear.Location = new System.Drawing.Point(1160, 4);
            this.btnAlarmClear.Name = "btnAlarmClear";
            this.btnAlarmClear.Size = new System.Drawing.Size(187, 83);
            this.btnAlarmClear.TabIndex = 33;
            this.btnAlarmClear.Text = "Alarm Clear";
            this.btnAlarmClear.UseVisualStyleBackColor = false;
            this.btnAlarmClear.Click += new System.EventHandler(this.btnAlarmClear_Click_1);
            // 
            // btnUtilitySettings
            // 
            this.btnUtilitySettings.BackColor = System.Drawing.Color.White;
            this.btnUtilitySettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUtilitySettings.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUtilitySettings.Location = new System.Drawing.Point(967, 4);
            this.btnUtilitySettings.Name = "btnUtilitySettings";
            this.btnUtilitySettings.Size = new System.Drawing.Size(187, 83);
            this.btnUtilitySettings.TabIndex = 32;
            this.btnUtilitySettings.Text = "Utility Settings";
            this.btnUtilitySettings.UseVisualStyleBackColor = false;
            this.btnUtilitySettings.Click += new System.EventHandler(this.btnUtilitySettings_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.59292F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.40708F));
            this.tableLayoutPanel1.Controls.Add(this.lblRamUsage, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCPUUsage, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(636, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(226, 82);
            this.tableLayoutPanel1.TabIndex = 31;
            // 
            // lblRamUsage
            // 
            this.lblRamUsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRamUsage.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRamUsage.Location = new System.Drawing.Point(96, 41);
            this.lblRamUsage.Name = "lblRamUsage";
            this.lblRamUsage.Size = new System.Drawing.Size(127, 41);
            this.lblRamUsage.TabIndex = 3;
            this.lblRamUsage.Text = "100";
            this.lblRamUsage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 41);
            this.label5.TabIndex = 2;
            this.label5.Text = "RAM : ";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCPUUsage
            // 
            this.lblCPUUsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCPUUsage.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCPUUsage.Location = new System.Drawing.Point(96, 0);
            this.lblCPUUsage.Name = "lblCPUUsage";
            this.lblCPUUsage.Size = new System.Drawing.Size(127, 41);
            this.lblCPUUsage.TabIndex = 1;
            this.lblCPUUsage.Text = "100";
            this.lblCPUUsage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 41);
            this.label2.TabIndex = 0;
            this.label2.Text = "CPU : ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblMaster);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(451, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(179, 85);
            this.panel2.TabIndex = 30;
            // 
            // lblMaster
            // 
            this.lblMaster.BackColor = System.Drawing.Color.OrangeRed;
            this.lblMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMaster.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaster.Location = new System.Drawing.Point(0, 42);
            this.lblMaster.Name = "lblMaster";
            this.lblMaster.Size = new System.Drawing.Size(179, 43);
            this.lblMaster.TabIndex = 1;
            this.lblMaster.Text = "Offline";
            this.lblMaster.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.SteelBlue;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(179, 42);
            this.label4.TabIndex = 0;
            this.label4.Text = "Master";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblEtherCAT);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(266, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(179, 85);
            this.panel1.TabIndex = 29;
            // 
            // lblEtherCAT
            // 
            this.lblEtherCAT.BackColor = System.Drawing.Color.OrangeRed;
            this.lblEtherCAT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEtherCAT.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEtherCAT.Location = new System.Drawing.Point(0, 42);
            this.lblEtherCAT.Name = "lblEtherCAT";
            this.lblEtherCAT.Size = new System.Drawing.Size(179, 43);
            this.lblEtherCAT.TabIndex = 1;
            this.lblEtherCAT.Text = "Offline";
            this.lblEtherCAT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SteelBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = "EtherCAT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureLogo
            // 
            this.pictureLogo.BackgroundImage = global::RackMaster.Properties.Resources.logo;
            this.pictureLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureLogo.Location = new System.Drawing.Point(12, 26);
            this.pictureLogo.Name = "pictureLogo";
            this.pictureLogo.Size = new System.Drawing.Size(248, 60);
            this.pictureLogo.TabIndex = 28;
            this.pictureLogo.TabStop = false;
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTime.Location = new System.Drawing.Point(1377, 47);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(309, 42);
            this.lblCurrentTime.TabIndex = 0;
            this.lblCurrentTime.Text = "Current Time";
            this.lblCurrentTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSWVersion
            // 
            this.lblSWVersion.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSWVersion.Location = new System.Drawing.Point(1374, 4);
            this.lblSWVersion.Name = "lblSWVersion";
            this.lblSWVersion.Size = new System.Drawing.Size(309, 42);
            this.lblSWVersion.TabIndex = 1;
            this.lblSWVersion.Text = "Ver 0.0";
            this.lblSWVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlMenuStrip
            // 
            this.pnlMenuStrip.ContextMenuStrip = this.CMSMainForm;
            this.pnlMenuStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.pnlMenuStrip.Name = "pnlMenuStrip";
            this.pnlMenuStrip.Size = new System.Drawing.Size(1689, 92);
            this.pnlMenuStrip.TabIndex = 0;
            // 
            // CMSMainForm
            // 
            this.CMSMainForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMemoryMap});
            this.CMSMainForm.Name = "CMSMainForm";
            this.CMSMainForm.Size = new System.Drawing.Size(148, 26);
            // 
            // toolMemoryMap
            // 
            this.toolMemoryMap.Name = "toolMemoryMap";
            this.toolMemoryMap.Size = new System.Drawing.Size(147, 22);
            this.toolMemoryMap.Text = "Memory Map";
            // 
            // pnlSideBar
            // 
            this.pnlSideBar.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlSideBar.Controls.Add(this.btnPageAlarmList);
            this.pnlSideBar.Controls.Add(this.btnPageHistory);
            this.pnlSideBar.Controls.Add(this.btnPageTestDrive);
            this.pnlSideBar.Controls.Add(this.btnPageStatus);
            this.pnlSideBar.Controls.Add(this.btnPageSetting);
            this.pnlSideBar.Controls.Add(this.btnPageMotor);
            this.pnlSideBar.Controls.Add(this.btnPageMain);
            this.pnlSideBar.Controls.Add(this.btnExit);
            this.pnlSideBar.Controls.Add(this.btnBuzzerStop);
            this.pnlSideBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSideBar.Location = new System.Drawing.Point(1689, 0);
            this.pnlSideBar.Name = "pnlSideBar";
            this.pnlSideBar.Size = new System.Drawing.Size(231, 1061);
            this.pnlSideBar.TabIndex = 5;
            // 
            // btnPageAlarmList
            // 
            this.btnPageAlarmList.BackColor = System.Drawing.Color.LightCyan;
            this.btnPageAlarmList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPageAlarmList.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPageAlarmList.Location = new System.Drawing.Point(6, 764);
            this.btnPageAlarmList.Name = "btnPageAlarmList";
            this.btnPageAlarmList.Size = new System.Drawing.Size(222, 105);
            this.btnPageAlarmList.TabIndex = 22;
            this.btnPageAlarmList.Text = "Alarm List";
            this.btnPageAlarmList.UseVisualStyleBackColor = false;
            // 
            // btnPageHistory
            // 
            this.btnPageHistory.BackColor = System.Drawing.Color.LightCyan;
            this.btnPageHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPageHistory.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPageHistory.Location = new System.Drawing.Point(6, 653);
            this.btnPageHistory.Name = "btnPageHistory";
            this.btnPageHistory.Size = new System.Drawing.Size(222, 105);
            this.btnPageHistory.TabIndex = 21;
            this.btnPageHistory.Text = "History";
            this.btnPageHistory.UseVisualStyleBackColor = false;
            // 
            // btnPageTestDrive
            // 
            this.btnPageTestDrive.BackColor = System.Drawing.Color.LightCyan;
            this.btnPageTestDrive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPageTestDrive.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPageTestDrive.Location = new System.Drawing.Point(6, 542);
            this.btnPageTestDrive.Name = "btnPageTestDrive";
            this.btnPageTestDrive.Size = new System.Drawing.Size(222, 105);
            this.btnPageTestDrive.TabIndex = 19;
            this.btnPageTestDrive.Text = "Test Drive";
            this.btnPageTestDrive.UseVisualStyleBackColor = false;
            // 
            // btnPageStatus
            // 
            this.btnPageStatus.BackColor = System.Drawing.Color.LightCyan;
            this.btnPageStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPageStatus.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPageStatus.Location = new System.Drawing.Point(6, 320);
            this.btnPageStatus.Name = "btnPageStatus";
            this.btnPageStatus.Size = new System.Drawing.Size(222, 105);
            this.btnPageStatus.TabIndex = 18;
            this.btnPageStatus.Text = "Status";
            this.btnPageStatus.UseVisualStyleBackColor = false;
            // 
            // btnPageSetting
            // 
            this.btnPageSetting.BackColor = System.Drawing.Color.LightCyan;
            this.btnPageSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPageSetting.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPageSetting.Location = new System.Drawing.Point(6, 431);
            this.btnPageSetting.Name = "btnPageSetting";
            this.btnPageSetting.Size = new System.Drawing.Size(222, 105);
            this.btnPageSetting.TabIndex = 17;
            this.btnPageSetting.Text = "Setting";
            this.btnPageSetting.UseVisualStyleBackColor = false;
            // 
            // btnPageMotor
            // 
            this.btnPageMotor.BackColor = System.Drawing.Color.Gainsboro;
            this.btnPageMotor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPageMotor.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPageMotor.Location = new System.Drawing.Point(6, 209);
            this.btnPageMotor.Name = "btnPageMotor";
            this.btnPageMotor.Size = new System.Drawing.Size(222, 105);
            this.btnPageMotor.TabIndex = 16;
            this.btnPageMotor.Text = "Motor";
            this.btnPageMotor.UseVisualStyleBackColor = false;
            // 
            // btnPageMain
            // 
            this.btnPageMain.BackColor = System.Drawing.Color.SkyBlue;
            this.btnPageMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPageMain.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPageMain.Location = new System.Drawing.Point(6, 98);
            this.btnPageMain.Name = "btnPageMain";
            this.btnPageMain.Size = new System.Drawing.Size(222, 105);
            this.btnPageMain.TabIndex = 15;
            this.btnPageMain.Text = "Main";
            this.btnPageMain.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Azure;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(0, 914);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(231, 147);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "EXIT";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnBuzzerStop
            // 
            this.btnBuzzerStop.BackColor = System.Drawing.Color.Azure;
            this.btnBuzzerStop.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBuzzerStop.FlatAppearance.BorderSize = 0;
            this.btnBuzzerStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuzzerStop.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuzzerStop.Image = global::RackMaster.Properties.Resources.icons8_no_audio_48;
            this.btnBuzzerStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBuzzerStop.Location = new System.Drawing.Point(0, 0);
            this.btnBuzzerStop.Name = "btnBuzzerStop";
            this.btnBuzzerStop.Size = new System.Drawing.Size(231, 92);
            this.btnBuzzerStop.TabIndex = 14;
            this.btnBuzzerStop.Text = "BUZZER STOP";
            this.btnBuzzerStop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBuzzerStop.UseVisualStyleBackColor = false;
            this.btnBuzzerStop.Click += new System.EventHandler(this.btnBuzzerStop_Click);
            // 
            // updateTimer
            // 
            this.updateTimer.Interval = 50;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // FrmMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1920, 1061);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlSideBar);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmMainForm";
            this.Text = "FrmMainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMainForm_FormClosing);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).EndInit();
            this.CMSMainForm.ResumeLayout(false);
            this.pnlSideBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.PictureBox pictureLogo;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label lblSWVersion;
        private System.Windows.Forms.Panel pnlSideBar;
        private System.Windows.Forms.Button btnBuzzerStop;
        private System.Windows.Forms.Button btnPageMain;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPageMotor;
        private System.Windows.Forms.Button btnPageSetting;
        private System.Windows.Forms.Button btnPageStatus;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Button btnPageTestDrive;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblMaster;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblEtherCAT;
        private System.Windows.Forms.Button btnPageHistory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblRamUsage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCPUUsage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUtilitySettings;
        private System.Windows.Forms.Button btnAlarmClear;
        private System.Windows.Forms.Button btnPageAlarmList;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Label lblLoginDuration;
        private System.Windows.Forms.ContextMenuStrip CMSMainForm;
        private System.Windows.Forms.ToolStripMenuItem toolMemoryMap;
        private System.Windows.Forms.Panel pnlMenuStrip;
    }
}