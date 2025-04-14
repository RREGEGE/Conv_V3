
namespace Master.SubForm.RMTPForm.RMTPSubForm
{
    partial class Frm_RackMasterTPSettings
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
            this.components = new System.ComponentModel.Container();
            this.UIUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_RackMasterSubCommand = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_TimeSync = new System.Windows.Forms.Button();
            this.groupBox_RackMasterSpeedSetting = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_SetValue = new System.Windows.Forms.Label();
            this.lbl_AppliedValue = new System.Windows.Forms.Label();
            this.lbl_XAxisSpeedTitle = new System.Windows.Forms.Label();
            this.lbl_ZAxisSpeedTitle = new System.Windows.Forms.Label();
            this.lbl_AAxisSpeedTitle = new System.Windows.Forms.Label();
            this.lbl_TAxisSpeedTitle = new System.Windows.Forms.Label();
            this.lbl_X_Axis_SpeedSetting = new System.Windows.Forms.Label();
            this.lbl_Z_Axis_SpeedSetting = new System.Windows.Forms.Label();
            this.lbl_A_Axis_SpeedSetting = new System.Windows.Forms.Label();
            this.lbl_T_Axis_SpeedSetting = new System.Windows.Forms.Label();
            this.tbx_X_Axis_SpeedSettings = new System.Windows.Forms.TextBox();
            this.tbx_Z_Axis_SpeedSettings = new System.Windows.Forms.TextBox();
            this.tbx_A_Axis_SpeedSettings = new System.Windows.Forms.TextBox();
            this.tbx_T_Axis_SpeedSettings = new System.Windows.Forms.TextBox();
            this.btn_X_Axis_SpeedSettings_Send = new System.Windows.Forms.Button();
            this.btn_Z_Axis_SpeedSettings_Send = new System.Windows.Forms.Button();
            this.btn_A_Axis_SpeedSettings_Send = new System.Windows.Forms.Button();
            this.btn_T_Axis_SpeedSettings_Send = new System.Windows.Forms.Button();
            this.btn_All_Axis_SpeedSettings_Send = new System.Windows.Forms.Button();
            this.groupBox_RackMasterOverLoadSetting = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_SetValue2 = new System.Windows.Forms.Label();
            this.lbl_AppliedValue2 = new System.Windows.Forms.Label();
            this.lbl_XAxisMaxLoadTitle = new System.Windows.Forms.Label();
            this.lbl_ZAxisMaxLoadTitle = new System.Windows.Forms.Label();
            this.lbl_AAxisMaxLoadTitle = new System.Windows.Forms.Label();
            this.lbl_TAxisMaxLoadTitle = new System.Windows.Forms.Label();
            this.lbl_X_Axis_Read_SetMaxLoad = new System.Windows.Forms.Label();
            this.lbl_Z_Axis_Read_SetMaxLoad = new System.Windows.Forms.Label();
            this.lbl_A_Axis_Read_SetMaxLoad = new System.Windows.Forms.Label();
            this.lbl_T_Axis_Read_SetMaxLoad = new System.Windows.Forms.Label();
            this.tbx_X_Axis_OverLoadSettings = new System.Windows.Forms.TextBox();
            this.tbx_Z_Axis_OverLoadSettings = new System.Windows.Forms.TextBox();
            this.tbx_A_Axis_OverLoadSettings = new System.Windows.Forms.TextBox();
            this.tbx_T_Axis_OverLoadSettings = new System.Windows.Forms.TextBox();
            this.btn_X_Axis_OverLoadSettings_Send = new System.Windows.Forms.Button();
            this.btn_Z_Axis_OverLoadSettings_Send = new System.Windows.Forms.Button();
            this.btn_A_Axis_OverLoadSettings_Send = new System.Windows.Forms.Button();
            this.btn_T_Axis_OverLoadSettings_Send = new System.Windows.Forms.Button();
            this.btn_All_Axis_OverLoadSettings_Send = new System.Windows.Forms.Button();
            this.groupBox_RackMasterOverLoadClear = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_XAxisMaxLoadTitle2 = new System.Windows.Forms.Label();
            this.lbl_ZAxisMaxLoadTitle2 = new System.Windows.Forms.Label();
            this.lbl_AAxisMaxLoadTitle2 = new System.Windows.Forms.Label();
            this.lbl_TAxisMaxLoadTitle2 = new System.Windows.Forms.Label();
            this.btn_X_Axis_Detected_OverLoad_Clear = new System.Windows.Forms.Button();
            this.btn_Z_Axis_Detected_OverLoad_Clear = new System.Windows.Forms.Button();
            this.btn_A_Axis_Detected_OverLoad_Clear = new System.Windows.Forms.Button();
            this.btn_T_Axis_Detected_OverLoad_Clear = new System.Windows.Forms.Button();
            this.btn_All_Axis_Detected_OverLoad_Clear = new System.Windows.Forms.Button();
            this.lbl_DetectedValue = new System.Windows.Forms.Label();
            this.lbl_X_Axis_Detected_OverLoad = new System.Windows.Forms.Label();
            this.lbl_Z_Axis_Detected_OverLoad = new System.Windows.Forms.Label();
            this.lbl_A_Axis_Detected_OverLoad = new System.Windows.Forms.Label();
            this.lbl_T_Axis_Detected_OverLoad = new System.Windows.Forms.Label();
            this.btn_MaintMove = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox_RackMasterSubCommand.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox_RackMasterSpeedSetting.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox_RackMasterOverLoadSetting.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox_RackMasterOverLoadClear.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // UIUpdateTimer
            // 
            this.UIUpdateTimer.Interval = 250;
            this.UIUpdateTimer.Tick += new System.EventHandler(this.UIUpdateTimer_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox_RackMasterSubCommand, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox_RackMasterSpeedSetting, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox_RackMasterOverLoadSetting, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox_RackMasterOverLoadClear, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1730, 754);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox_RackMasterSubCommand
            // 
            this.groupBox_RackMasterSubCommand.Controls.Add(this.tableLayoutPanel5);
            this.groupBox_RackMasterSubCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_RackMasterSubCommand.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_RackMasterSubCommand.Location = new System.Drawing.Point(3, 380);
            this.groupBox_RackMasterSubCommand.Name = "groupBox_RackMasterSubCommand";
            this.groupBox_RackMasterSubCommand.Size = new System.Drawing.Size(859, 371);
            this.groupBox_RackMasterSubCommand.TabIndex = 9;
            this.groupBox_RackMasterSubCommand.TabStop = false;
            this.groupBox_RackMasterSubCommand.Text = "RackMaster Advanced Command";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Controls.Add(this.btn_TimeSync, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btn_MaintMove, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 4;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(853, 343);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // btn_TimeSync
            // 
            this.btn_TimeSync.BackColor = System.Drawing.Color.White;
            this.btn_TimeSync.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_TimeSync.FlatAppearance.BorderSize = 0;
            this.btn_TimeSync.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_TimeSync.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_TimeSync.Location = new System.Drawing.Point(1, 1);
            this.btn_TimeSync.Margin = new System.Windows.Forms.Padding(1);
            this.btn_TimeSync.Name = "btn_TimeSync";
            this.btn_TimeSync.Size = new System.Drawing.Size(211, 83);
            this.btn_TimeSync.TabIndex = 27;
            this.btn_TimeSync.Text = "Time Sync";
            this.btn_TimeSync.UseVisualStyleBackColor = false;
            this.btn_TimeSync.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_TimeSync_MouseDown);
            this.btn_TimeSync.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // groupBox_RackMasterSpeedSetting
            // 
            this.groupBox_RackMasterSpeedSetting.Controls.Add(this.tableLayoutPanel2);
            this.groupBox_RackMasterSpeedSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_RackMasterSpeedSetting.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_RackMasterSpeedSetting.Location = new System.Drawing.Point(3, 3);
            this.groupBox_RackMasterSpeedSetting.Name = "groupBox_RackMasterSpeedSetting";
            this.groupBox_RackMasterSpeedSetting.Size = new System.Drawing.Size(859, 371);
            this.groupBox_RackMasterSpeedSetting.TabIndex = 6;
            this.groupBox_RackMasterSpeedSetting.TabStop = false;
            this.groupBox_RackMasterSpeedSetting.Text = "RackMaster Speed Settings";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.lbl_SetValue, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbl_AppliedValue, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbl_XAxisSpeedTitle, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lbl_ZAxisSpeedTitle, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lbl_AAxisSpeedTitle, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lbl_TAxisSpeedTitle, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.lbl_X_Axis_SpeedSetting, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lbl_Z_Axis_SpeedSetting, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.lbl_A_Axis_SpeedSetting, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.lbl_T_Axis_SpeedSetting, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.tbx_X_Axis_SpeedSettings, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.tbx_Z_Axis_SpeedSettings, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.tbx_A_Axis_SpeedSettings, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.tbx_T_Axis_SpeedSettings, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.btn_X_Axis_SpeedSettings_Send, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.btn_Z_Axis_SpeedSettings_Send, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.btn_A_Axis_SpeedSettings_Send, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.btn_T_Axis_SpeedSettings_Send, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.btn_All_Axis_SpeedSettings_Send, 3, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(853, 343);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // lbl_SetValue
            // 
            this.lbl_SetValue.BackColor = System.Drawing.Color.Transparent;
            this.lbl_SetValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_SetValue.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_SetValue.Location = new System.Drawing.Point(429, 0);
            this.lbl_SetValue.Name = "lbl_SetValue";
            this.lbl_SetValue.Size = new System.Drawing.Size(207, 57);
            this.lbl_SetValue.TabIndex = 25;
            this.lbl_SetValue.Text = "Set Value";
            this.lbl_SetValue.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_AppliedValue
            // 
            this.lbl_AppliedValue.BackColor = System.Drawing.Color.Transparent;
            this.lbl_AppliedValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_AppliedValue.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_AppliedValue.Location = new System.Drawing.Point(216, 0);
            this.lbl_AppliedValue.Name = "lbl_AppliedValue";
            this.lbl_AppliedValue.Size = new System.Drawing.Size(207, 57);
            this.lbl_AppliedValue.TabIndex = 24;
            this.lbl_AppliedValue.Text = "Read Value";
            this.lbl_AppliedValue.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_XAxisSpeedTitle
            // 
            this.lbl_XAxisSpeedTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbl_XAxisSpeedTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_XAxisSpeedTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_XAxisSpeedTitle.Location = new System.Drawing.Point(3, 57);
            this.lbl_XAxisSpeedTitle.Name = "lbl_XAxisSpeedTitle";
            this.lbl_XAxisSpeedTitle.Size = new System.Drawing.Size(207, 57);
            this.lbl_XAxisSpeedTitle.TabIndex = 8;
            this.lbl_XAxisSpeedTitle.Text = "X-Axis Speed [%] :";
            this.lbl_XAxisSpeedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_ZAxisSpeedTitle
            // 
            this.lbl_ZAxisSpeedTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbl_ZAxisSpeedTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_ZAxisSpeedTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_ZAxisSpeedTitle.Location = new System.Drawing.Point(3, 114);
            this.lbl_ZAxisSpeedTitle.Name = "lbl_ZAxisSpeedTitle";
            this.lbl_ZAxisSpeedTitle.Size = new System.Drawing.Size(207, 57);
            this.lbl_ZAxisSpeedTitle.TabIndex = 9;
            this.lbl_ZAxisSpeedTitle.Text = "Z-Axis Speed [%] :";
            this.lbl_ZAxisSpeedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_AAxisSpeedTitle
            // 
            this.lbl_AAxisSpeedTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbl_AAxisSpeedTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_AAxisSpeedTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_AAxisSpeedTitle.Location = new System.Drawing.Point(3, 171);
            this.lbl_AAxisSpeedTitle.Name = "lbl_AAxisSpeedTitle";
            this.lbl_AAxisSpeedTitle.Size = new System.Drawing.Size(207, 57);
            this.lbl_AAxisSpeedTitle.TabIndex = 10;
            this.lbl_AAxisSpeedTitle.Text = "A-Axis Speed [%] :";
            this.lbl_AAxisSpeedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_TAxisSpeedTitle
            // 
            this.lbl_TAxisSpeedTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TAxisSpeedTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_TAxisSpeedTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_TAxisSpeedTitle.Location = new System.Drawing.Point(3, 228);
            this.lbl_TAxisSpeedTitle.Name = "lbl_TAxisSpeedTitle";
            this.lbl_TAxisSpeedTitle.Size = new System.Drawing.Size(207, 57);
            this.lbl_TAxisSpeedTitle.TabIndex = 11;
            this.lbl_TAxisSpeedTitle.Text = "T-Axis Speed [%] :";
            this.lbl_TAxisSpeedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_X_Axis_SpeedSetting
            // 
            this.lbl_X_Axis_SpeedSetting.BackColor = System.Drawing.Color.Transparent;
            this.lbl_X_Axis_SpeedSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_X_Axis_SpeedSetting.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_X_Axis_SpeedSetting.Location = new System.Drawing.Point(216, 57);
            this.lbl_X_Axis_SpeedSetting.Name = "lbl_X_Axis_SpeedSetting";
            this.lbl_X_Axis_SpeedSetting.Size = new System.Drawing.Size(207, 57);
            this.lbl_X_Axis_SpeedSetting.TabIndex = 12;
            this.lbl_X_Axis_SpeedSetting.Text = "0 %";
            this.lbl_X_Axis_SpeedSetting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_Z_Axis_SpeedSetting
            // 
            this.lbl_Z_Axis_SpeedSetting.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Z_Axis_SpeedSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Z_Axis_SpeedSetting.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_Z_Axis_SpeedSetting.Location = new System.Drawing.Point(216, 114);
            this.lbl_Z_Axis_SpeedSetting.Name = "lbl_Z_Axis_SpeedSetting";
            this.lbl_Z_Axis_SpeedSetting.Size = new System.Drawing.Size(207, 57);
            this.lbl_Z_Axis_SpeedSetting.TabIndex = 13;
            this.lbl_Z_Axis_SpeedSetting.Text = "0 %";
            this.lbl_Z_Axis_SpeedSetting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_A_Axis_SpeedSetting
            // 
            this.lbl_A_Axis_SpeedSetting.BackColor = System.Drawing.Color.Transparent;
            this.lbl_A_Axis_SpeedSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_A_Axis_SpeedSetting.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_A_Axis_SpeedSetting.Location = new System.Drawing.Point(216, 171);
            this.lbl_A_Axis_SpeedSetting.Name = "lbl_A_Axis_SpeedSetting";
            this.lbl_A_Axis_SpeedSetting.Size = new System.Drawing.Size(207, 57);
            this.lbl_A_Axis_SpeedSetting.TabIndex = 14;
            this.lbl_A_Axis_SpeedSetting.Text = "0 %";
            this.lbl_A_Axis_SpeedSetting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_T_Axis_SpeedSetting
            // 
            this.lbl_T_Axis_SpeedSetting.BackColor = System.Drawing.Color.Transparent;
            this.lbl_T_Axis_SpeedSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_T_Axis_SpeedSetting.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_T_Axis_SpeedSetting.Location = new System.Drawing.Point(216, 228);
            this.lbl_T_Axis_SpeedSetting.Name = "lbl_T_Axis_SpeedSetting";
            this.lbl_T_Axis_SpeedSetting.Size = new System.Drawing.Size(207, 57);
            this.lbl_T_Axis_SpeedSetting.TabIndex = 15;
            this.lbl_T_Axis_SpeedSetting.Text = "0 %";
            this.lbl_T_Axis_SpeedSetting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbx_X_Axis_SpeedSettings
            // 
            this.tbx_X_Axis_SpeedSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_X_Axis_SpeedSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_X_Axis_SpeedSettings.Location = new System.Drawing.Point(429, 60);
            this.tbx_X_Axis_SpeedSettings.MaxLength = 3;
            this.tbx_X_Axis_SpeedSettings.Multiline = true;
            this.tbx_X_Axis_SpeedSettings.Name = "tbx_X_Axis_SpeedSettings";
            this.tbx_X_Axis_SpeedSettings.Size = new System.Drawing.Size(207, 51);
            this.tbx_X_Axis_SpeedSettings.TabIndex = 16;
            this.tbx_X_Axis_SpeedSettings.Text = "50";
            this.tbx_X_Axis_SpeedSettings.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbx_X_Axis_SpeedSettings.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // tbx_Z_Axis_SpeedSettings
            // 
            this.tbx_Z_Axis_SpeedSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_Z_Axis_SpeedSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_Z_Axis_SpeedSettings.Location = new System.Drawing.Point(429, 117);
            this.tbx_Z_Axis_SpeedSettings.MaxLength = 3;
            this.tbx_Z_Axis_SpeedSettings.Multiline = true;
            this.tbx_Z_Axis_SpeedSettings.Name = "tbx_Z_Axis_SpeedSettings";
            this.tbx_Z_Axis_SpeedSettings.Size = new System.Drawing.Size(207, 51);
            this.tbx_Z_Axis_SpeedSettings.TabIndex = 17;
            this.tbx_Z_Axis_SpeedSettings.Text = "50";
            this.tbx_Z_Axis_SpeedSettings.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbx_Z_Axis_SpeedSettings.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // tbx_A_Axis_SpeedSettings
            // 
            this.tbx_A_Axis_SpeedSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_A_Axis_SpeedSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_A_Axis_SpeedSettings.Location = new System.Drawing.Point(429, 174);
            this.tbx_A_Axis_SpeedSettings.MaxLength = 3;
            this.tbx_A_Axis_SpeedSettings.Multiline = true;
            this.tbx_A_Axis_SpeedSettings.Name = "tbx_A_Axis_SpeedSettings";
            this.tbx_A_Axis_SpeedSettings.Size = new System.Drawing.Size(207, 51);
            this.tbx_A_Axis_SpeedSettings.TabIndex = 18;
            this.tbx_A_Axis_SpeedSettings.Text = "50";
            this.tbx_A_Axis_SpeedSettings.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbx_A_Axis_SpeedSettings.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // tbx_T_Axis_SpeedSettings
            // 
            this.tbx_T_Axis_SpeedSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_T_Axis_SpeedSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_T_Axis_SpeedSettings.Location = new System.Drawing.Point(429, 231);
            this.tbx_T_Axis_SpeedSettings.MaxLength = 3;
            this.tbx_T_Axis_SpeedSettings.Multiline = true;
            this.tbx_T_Axis_SpeedSettings.Name = "tbx_T_Axis_SpeedSettings";
            this.tbx_T_Axis_SpeedSettings.Size = new System.Drawing.Size(207, 51);
            this.tbx_T_Axis_SpeedSettings.TabIndex = 19;
            this.tbx_T_Axis_SpeedSettings.Text = "50";
            this.tbx_T_Axis_SpeedSettings.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbx_T_Axis_SpeedSettings.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // btn_X_Axis_SpeedSettings_Send
            // 
            this.btn_X_Axis_SpeedSettings_Send.BackColor = System.Drawing.Color.White;
            this.btn_X_Axis_SpeedSettings_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_X_Axis_SpeedSettings_Send.FlatAppearance.BorderSize = 0;
            this.btn_X_Axis_SpeedSettings_Send.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_X_Axis_SpeedSettings_Send.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_X_Axis_SpeedSettings_Send.Location = new System.Drawing.Point(640, 58);
            this.btn_X_Axis_SpeedSettings_Send.Margin = new System.Windows.Forms.Padding(1);
            this.btn_X_Axis_SpeedSettings_Send.Name = "btn_X_Axis_SpeedSettings_Send";
            this.btn_X_Axis_SpeedSettings_Send.Size = new System.Drawing.Size(212, 55);
            this.btn_X_Axis_SpeedSettings_Send.TabIndex = 20;
            this.btn_X_Axis_SpeedSettings_Send.Text = "Send";
            this.btn_X_Axis_SpeedSettings_Send.UseVisualStyleBackColor = false;
            this.btn_X_Axis_SpeedSettings_Send.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_SpeedSettings_Send_MouseDown);
            this.btn_X_Axis_SpeedSettings_Send.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // btn_Z_Axis_SpeedSettings_Send
            // 
            this.btn_Z_Axis_SpeedSettings_Send.BackColor = System.Drawing.Color.White;
            this.btn_Z_Axis_SpeedSettings_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Z_Axis_SpeedSettings_Send.FlatAppearance.BorderSize = 0;
            this.btn_Z_Axis_SpeedSettings_Send.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Z_Axis_SpeedSettings_Send.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Z_Axis_SpeedSettings_Send.Location = new System.Drawing.Point(640, 115);
            this.btn_Z_Axis_SpeedSettings_Send.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Z_Axis_SpeedSettings_Send.Name = "btn_Z_Axis_SpeedSettings_Send";
            this.btn_Z_Axis_SpeedSettings_Send.Size = new System.Drawing.Size(212, 55);
            this.btn_Z_Axis_SpeedSettings_Send.TabIndex = 21;
            this.btn_Z_Axis_SpeedSettings_Send.Text = "Send";
            this.btn_Z_Axis_SpeedSettings_Send.UseVisualStyleBackColor = false;
            this.btn_Z_Axis_SpeedSettings_Send.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_SpeedSettings_Send_MouseDown);
            this.btn_Z_Axis_SpeedSettings_Send.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // btn_A_Axis_SpeedSettings_Send
            // 
            this.btn_A_Axis_SpeedSettings_Send.BackColor = System.Drawing.Color.White;
            this.btn_A_Axis_SpeedSettings_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_A_Axis_SpeedSettings_Send.FlatAppearance.BorderSize = 0;
            this.btn_A_Axis_SpeedSettings_Send.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_A_Axis_SpeedSettings_Send.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_A_Axis_SpeedSettings_Send.Location = new System.Drawing.Point(640, 172);
            this.btn_A_Axis_SpeedSettings_Send.Margin = new System.Windows.Forms.Padding(1);
            this.btn_A_Axis_SpeedSettings_Send.Name = "btn_A_Axis_SpeedSettings_Send";
            this.btn_A_Axis_SpeedSettings_Send.Size = new System.Drawing.Size(212, 55);
            this.btn_A_Axis_SpeedSettings_Send.TabIndex = 22;
            this.btn_A_Axis_SpeedSettings_Send.Text = "Send";
            this.btn_A_Axis_SpeedSettings_Send.UseVisualStyleBackColor = false;
            this.btn_A_Axis_SpeedSettings_Send.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_SpeedSettings_Send_MouseDown);
            this.btn_A_Axis_SpeedSettings_Send.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // btn_T_Axis_SpeedSettings_Send
            // 
            this.btn_T_Axis_SpeedSettings_Send.BackColor = System.Drawing.Color.White;
            this.btn_T_Axis_SpeedSettings_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_T_Axis_SpeedSettings_Send.FlatAppearance.BorderSize = 0;
            this.btn_T_Axis_SpeedSettings_Send.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_T_Axis_SpeedSettings_Send.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_T_Axis_SpeedSettings_Send.Location = new System.Drawing.Point(640, 229);
            this.btn_T_Axis_SpeedSettings_Send.Margin = new System.Windows.Forms.Padding(1);
            this.btn_T_Axis_SpeedSettings_Send.Name = "btn_T_Axis_SpeedSettings_Send";
            this.btn_T_Axis_SpeedSettings_Send.Size = new System.Drawing.Size(212, 55);
            this.btn_T_Axis_SpeedSettings_Send.TabIndex = 23;
            this.btn_T_Axis_SpeedSettings_Send.Text = "Send";
            this.btn_T_Axis_SpeedSettings_Send.UseVisualStyleBackColor = false;
            this.btn_T_Axis_SpeedSettings_Send.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_SpeedSettings_Send_MouseDown);
            this.btn_T_Axis_SpeedSettings_Send.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // btn_All_Axis_SpeedSettings_Send
            // 
            this.btn_All_Axis_SpeedSettings_Send.BackColor = System.Drawing.Color.White;
            this.btn_All_Axis_SpeedSettings_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_All_Axis_SpeedSettings_Send.FlatAppearance.BorderSize = 0;
            this.btn_All_Axis_SpeedSettings_Send.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_All_Axis_SpeedSettings_Send.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_All_Axis_SpeedSettings_Send.Location = new System.Drawing.Point(640, 286);
            this.btn_All_Axis_SpeedSettings_Send.Margin = new System.Windows.Forms.Padding(1);
            this.btn_All_Axis_SpeedSettings_Send.Name = "btn_All_Axis_SpeedSettings_Send";
            this.btn_All_Axis_SpeedSettings_Send.Size = new System.Drawing.Size(212, 56);
            this.btn_All_Axis_SpeedSettings_Send.TabIndex = 26;
            this.btn_All_Axis_SpeedSettings_Send.Text = "All Send";
            this.btn_All_Axis_SpeedSettings_Send.UseVisualStyleBackColor = false;
            this.btn_All_Axis_SpeedSettings_Send.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_SpeedSettings_Send_MouseDown);
            this.btn_All_Axis_SpeedSettings_Send.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // groupBox_RackMasterOverLoadSetting
            // 
            this.groupBox_RackMasterOverLoadSetting.Controls.Add(this.tableLayoutPanel3);
            this.groupBox_RackMasterOverLoadSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_RackMasterOverLoadSetting.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_RackMasterOverLoadSetting.Location = new System.Drawing.Point(868, 3);
            this.groupBox_RackMasterOverLoadSetting.Name = "groupBox_RackMasterOverLoadSetting";
            this.groupBox_RackMasterOverLoadSetting.Size = new System.Drawing.Size(859, 371);
            this.groupBox_RackMasterOverLoadSetting.TabIndex = 7;
            this.groupBox_RackMasterOverLoadSetting.TabStop = false;
            this.groupBox_RackMasterOverLoadSetting.Text = "RackMaster Over Load Settings";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.lbl_SetValue2, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.lbl_AppliedValue2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lbl_XAxisMaxLoadTitle, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lbl_ZAxisMaxLoadTitle, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lbl_AAxisMaxLoadTitle, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.lbl_TAxisMaxLoadTitle, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.lbl_X_Axis_Read_SetMaxLoad, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.lbl_Z_Axis_Read_SetMaxLoad, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.lbl_A_Axis_Read_SetMaxLoad, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.lbl_T_Axis_Read_SetMaxLoad, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.tbx_X_Axis_OverLoadSettings, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.tbx_Z_Axis_OverLoadSettings, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.tbx_A_Axis_OverLoadSettings, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.tbx_T_Axis_OverLoadSettings, 2, 4);
            this.tableLayoutPanel3.Controls.Add(this.btn_X_Axis_OverLoadSettings_Send, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.btn_Z_Axis_OverLoadSettings_Send, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.btn_A_Axis_OverLoadSettings_Send, 3, 3);
            this.tableLayoutPanel3.Controls.Add(this.btn_T_Axis_OverLoadSettings_Send, 3, 4);
            this.tableLayoutPanel3.Controls.Add(this.btn_All_Axis_OverLoadSettings_Send, 3, 5);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(853, 343);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // lbl_SetValue2
            // 
            this.lbl_SetValue2.BackColor = System.Drawing.Color.Transparent;
            this.lbl_SetValue2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_SetValue2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_SetValue2.Location = new System.Drawing.Point(429, 0);
            this.lbl_SetValue2.Name = "lbl_SetValue2";
            this.lbl_SetValue2.Size = new System.Drawing.Size(207, 57);
            this.lbl_SetValue2.TabIndex = 25;
            this.lbl_SetValue2.Text = "Set Value";
            this.lbl_SetValue2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_AppliedValue2
            // 
            this.lbl_AppliedValue2.BackColor = System.Drawing.Color.Transparent;
            this.lbl_AppliedValue2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_AppliedValue2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_AppliedValue2.Location = new System.Drawing.Point(216, 0);
            this.lbl_AppliedValue2.Name = "lbl_AppliedValue2";
            this.lbl_AppliedValue2.Size = new System.Drawing.Size(207, 57);
            this.lbl_AppliedValue2.TabIndex = 24;
            this.lbl_AppliedValue2.Text = "Read Value";
            this.lbl_AppliedValue2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_XAxisMaxLoadTitle
            // 
            this.lbl_XAxisMaxLoadTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbl_XAxisMaxLoadTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_XAxisMaxLoadTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_XAxisMaxLoadTitle.Location = new System.Drawing.Point(3, 57);
            this.lbl_XAxisMaxLoadTitle.Name = "lbl_XAxisMaxLoadTitle";
            this.lbl_XAxisMaxLoadTitle.Size = new System.Drawing.Size(207, 57);
            this.lbl_XAxisMaxLoadTitle.TabIndex = 8;
            this.lbl_XAxisMaxLoadTitle.Text = "X-Axis Max Load [%] :";
            this.lbl_XAxisMaxLoadTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_ZAxisMaxLoadTitle
            // 
            this.lbl_ZAxisMaxLoadTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbl_ZAxisMaxLoadTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_ZAxisMaxLoadTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_ZAxisMaxLoadTitle.Location = new System.Drawing.Point(3, 114);
            this.lbl_ZAxisMaxLoadTitle.Name = "lbl_ZAxisMaxLoadTitle";
            this.lbl_ZAxisMaxLoadTitle.Size = new System.Drawing.Size(207, 57);
            this.lbl_ZAxisMaxLoadTitle.TabIndex = 9;
            this.lbl_ZAxisMaxLoadTitle.Text = "Z-Axis Max Load [%] :";
            this.lbl_ZAxisMaxLoadTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_AAxisMaxLoadTitle
            // 
            this.lbl_AAxisMaxLoadTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbl_AAxisMaxLoadTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_AAxisMaxLoadTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_AAxisMaxLoadTitle.Location = new System.Drawing.Point(3, 171);
            this.lbl_AAxisMaxLoadTitle.Name = "lbl_AAxisMaxLoadTitle";
            this.lbl_AAxisMaxLoadTitle.Size = new System.Drawing.Size(207, 57);
            this.lbl_AAxisMaxLoadTitle.TabIndex = 10;
            this.lbl_AAxisMaxLoadTitle.Text = "A-Axis Max Load [%] :";
            this.lbl_AAxisMaxLoadTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_TAxisMaxLoadTitle
            // 
            this.lbl_TAxisMaxLoadTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TAxisMaxLoadTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_TAxisMaxLoadTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_TAxisMaxLoadTitle.Location = new System.Drawing.Point(3, 228);
            this.lbl_TAxisMaxLoadTitle.Name = "lbl_TAxisMaxLoadTitle";
            this.lbl_TAxisMaxLoadTitle.Size = new System.Drawing.Size(207, 57);
            this.lbl_TAxisMaxLoadTitle.TabIndex = 11;
            this.lbl_TAxisMaxLoadTitle.Text = "T-Axis Max Load [%] :";
            this.lbl_TAxisMaxLoadTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_X_Axis_Read_SetMaxLoad
            // 
            this.lbl_X_Axis_Read_SetMaxLoad.BackColor = System.Drawing.Color.Transparent;
            this.lbl_X_Axis_Read_SetMaxLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_X_Axis_Read_SetMaxLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_X_Axis_Read_SetMaxLoad.Location = new System.Drawing.Point(216, 57);
            this.lbl_X_Axis_Read_SetMaxLoad.Name = "lbl_X_Axis_Read_SetMaxLoad";
            this.lbl_X_Axis_Read_SetMaxLoad.Size = new System.Drawing.Size(207, 57);
            this.lbl_X_Axis_Read_SetMaxLoad.TabIndex = 12;
            this.lbl_X_Axis_Read_SetMaxLoad.Text = "0 %";
            this.lbl_X_Axis_Read_SetMaxLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_Z_Axis_Read_SetMaxLoad
            // 
            this.lbl_Z_Axis_Read_SetMaxLoad.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Z_Axis_Read_SetMaxLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Z_Axis_Read_SetMaxLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_Z_Axis_Read_SetMaxLoad.Location = new System.Drawing.Point(216, 114);
            this.lbl_Z_Axis_Read_SetMaxLoad.Name = "lbl_Z_Axis_Read_SetMaxLoad";
            this.lbl_Z_Axis_Read_SetMaxLoad.Size = new System.Drawing.Size(207, 57);
            this.lbl_Z_Axis_Read_SetMaxLoad.TabIndex = 13;
            this.lbl_Z_Axis_Read_SetMaxLoad.Text = "0 %";
            this.lbl_Z_Axis_Read_SetMaxLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_A_Axis_Read_SetMaxLoad
            // 
            this.lbl_A_Axis_Read_SetMaxLoad.BackColor = System.Drawing.Color.Transparent;
            this.lbl_A_Axis_Read_SetMaxLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_A_Axis_Read_SetMaxLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_A_Axis_Read_SetMaxLoad.Location = new System.Drawing.Point(216, 171);
            this.lbl_A_Axis_Read_SetMaxLoad.Name = "lbl_A_Axis_Read_SetMaxLoad";
            this.lbl_A_Axis_Read_SetMaxLoad.Size = new System.Drawing.Size(207, 57);
            this.lbl_A_Axis_Read_SetMaxLoad.TabIndex = 14;
            this.lbl_A_Axis_Read_SetMaxLoad.Text = "0 %";
            this.lbl_A_Axis_Read_SetMaxLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_T_Axis_Read_SetMaxLoad
            // 
            this.lbl_T_Axis_Read_SetMaxLoad.BackColor = System.Drawing.Color.Transparent;
            this.lbl_T_Axis_Read_SetMaxLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_T_Axis_Read_SetMaxLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_T_Axis_Read_SetMaxLoad.Location = new System.Drawing.Point(216, 228);
            this.lbl_T_Axis_Read_SetMaxLoad.Name = "lbl_T_Axis_Read_SetMaxLoad";
            this.lbl_T_Axis_Read_SetMaxLoad.Size = new System.Drawing.Size(207, 57);
            this.lbl_T_Axis_Read_SetMaxLoad.TabIndex = 15;
            this.lbl_T_Axis_Read_SetMaxLoad.Text = "0 %";
            this.lbl_T_Axis_Read_SetMaxLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbx_X_Axis_OverLoadSettings
            // 
            this.tbx_X_Axis_OverLoadSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_X_Axis_OverLoadSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_X_Axis_OverLoadSettings.Location = new System.Drawing.Point(429, 60);
            this.tbx_X_Axis_OverLoadSettings.MaxLength = 3;
            this.tbx_X_Axis_OverLoadSettings.Multiline = true;
            this.tbx_X_Axis_OverLoadSettings.Name = "tbx_X_Axis_OverLoadSettings";
            this.tbx_X_Axis_OverLoadSettings.Size = new System.Drawing.Size(207, 51);
            this.tbx_X_Axis_OverLoadSettings.TabIndex = 16;
            this.tbx_X_Axis_OverLoadSettings.Text = "100";
            this.tbx_X_Axis_OverLoadSettings.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbx_X_Axis_OverLoadSettings.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // tbx_Z_Axis_OverLoadSettings
            // 
            this.tbx_Z_Axis_OverLoadSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_Z_Axis_OverLoadSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_Z_Axis_OverLoadSettings.Location = new System.Drawing.Point(429, 117);
            this.tbx_Z_Axis_OverLoadSettings.MaxLength = 3;
            this.tbx_Z_Axis_OverLoadSettings.Multiline = true;
            this.tbx_Z_Axis_OverLoadSettings.Name = "tbx_Z_Axis_OverLoadSettings";
            this.tbx_Z_Axis_OverLoadSettings.Size = new System.Drawing.Size(207, 51);
            this.tbx_Z_Axis_OverLoadSettings.TabIndex = 17;
            this.tbx_Z_Axis_OverLoadSettings.Text = "100";
            this.tbx_Z_Axis_OverLoadSettings.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbx_Z_Axis_OverLoadSettings.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // tbx_A_Axis_OverLoadSettings
            // 
            this.tbx_A_Axis_OverLoadSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_A_Axis_OverLoadSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_A_Axis_OverLoadSettings.Location = new System.Drawing.Point(429, 174);
            this.tbx_A_Axis_OverLoadSettings.MaxLength = 3;
            this.tbx_A_Axis_OverLoadSettings.Multiline = true;
            this.tbx_A_Axis_OverLoadSettings.Name = "tbx_A_Axis_OverLoadSettings";
            this.tbx_A_Axis_OverLoadSettings.Size = new System.Drawing.Size(207, 51);
            this.tbx_A_Axis_OverLoadSettings.TabIndex = 18;
            this.tbx_A_Axis_OverLoadSettings.Text = "100";
            this.tbx_A_Axis_OverLoadSettings.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbx_A_Axis_OverLoadSettings.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // tbx_T_Axis_OverLoadSettings
            // 
            this.tbx_T_Axis_OverLoadSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx_T_Axis_OverLoadSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_T_Axis_OverLoadSettings.Location = new System.Drawing.Point(429, 231);
            this.tbx_T_Axis_OverLoadSettings.MaxLength = 3;
            this.tbx_T_Axis_OverLoadSettings.Multiline = true;
            this.tbx_T_Axis_OverLoadSettings.Name = "tbx_T_Axis_OverLoadSettings";
            this.tbx_T_Axis_OverLoadSettings.Size = new System.Drawing.Size(207, 51);
            this.tbx_T_Axis_OverLoadSettings.TabIndex = 19;
            this.tbx_T_Axis_OverLoadSettings.Text = "100";
            this.tbx_T_Axis_OverLoadSettings.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbx_T_Axis_OverLoadSettings.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // btn_X_Axis_OverLoadSettings_Send
            // 
            this.btn_X_Axis_OverLoadSettings_Send.BackColor = System.Drawing.Color.White;
            this.btn_X_Axis_OverLoadSettings_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_X_Axis_OverLoadSettings_Send.FlatAppearance.BorderSize = 0;
            this.btn_X_Axis_OverLoadSettings_Send.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_X_Axis_OverLoadSettings_Send.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_X_Axis_OverLoadSettings_Send.Location = new System.Drawing.Point(640, 58);
            this.btn_X_Axis_OverLoadSettings_Send.Margin = new System.Windows.Forms.Padding(1);
            this.btn_X_Axis_OverLoadSettings_Send.Name = "btn_X_Axis_OverLoadSettings_Send";
            this.btn_X_Axis_OverLoadSettings_Send.Size = new System.Drawing.Size(212, 55);
            this.btn_X_Axis_OverLoadSettings_Send.TabIndex = 20;
            this.btn_X_Axis_OverLoadSettings_Send.Text = "Send";
            this.btn_X_Axis_OverLoadSettings_Send.UseVisualStyleBackColor = false;
            this.btn_X_Axis_OverLoadSettings_Send.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_OverLoadSettings_Send_MouseDown);
            this.btn_X_Axis_OverLoadSettings_Send.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // btn_Z_Axis_OverLoadSettings_Send
            // 
            this.btn_Z_Axis_OverLoadSettings_Send.BackColor = System.Drawing.Color.White;
            this.btn_Z_Axis_OverLoadSettings_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Z_Axis_OverLoadSettings_Send.FlatAppearance.BorderSize = 0;
            this.btn_Z_Axis_OverLoadSettings_Send.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Z_Axis_OverLoadSettings_Send.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Z_Axis_OverLoadSettings_Send.Location = new System.Drawing.Point(640, 115);
            this.btn_Z_Axis_OverLoadSettings_Send.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Z_Axis_OverLoadSettings_Send.Name = "btn_Z_Axis_OverLoadSettings_Send";
            this.btn_Z_Axis_OverLoadSettings_Send.Size = new System.Drawing.Size(212, 55);
            this.btn_Z_Axis_OverLoadSettings_Send.TabIndex = 21;
            this.btn_Z_Axis_OverLoadSettings_Send.Text = "Send";
            this.btn_Z_Axis_OverLoadSettings_Send.UseVisualStyleBackColor = false;
            this.btn_Z_Axis_OverLoadSettings_Send.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_OverLoadSettings_Send_MouseDown);
            this.btn_Z_Axis_OverLoadSettings_Send.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // btn_A_Axis_OverLoadSettings_Send
            // 
            this.btn_A_Axis_OverLoadSettings_Send.BackColor = System.Drawing.Color.White;
            this.btn_A_Axis_OverLoadSettings_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_A_Axis_OverLoadSettings_Send.FlatAppearance.BorderSize = 0;
            this.btn_A_Axis_OverLoadSettings_Send.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_A_Axis_OverLoadSettings_Send.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_A_Axis_OverLoadSettings_Send.Location = new System.Drawing.Point(640, 172);
            this.btn_A_Axis_OverLoadSettings_Send.Margin = new System.Windows.Forms.Padding(1);
            this.btn_A_Axis_OverLoadSettings_Send.Name = "btn_A_Axis_OverLoadSettings_Send";
            this.btn_A_Axis_OverLoadSettings_Send.Size = new System.Drawing.Size(212, 55);
            this.btn_A_Axis_OverLoadSettings_Send.TabIndex = 22;
            this.btn_A_Axis_OverLoadSettings_Send.Text = "Send";
            this.btn_A_Axis_OverLoadSettings_Send.UseVisualStyleBackColor = false;
            this.btn_A_Axis_OverLoadSettings_Send.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_OverLoadSettings_Send_MouseDown);
            this.btn_A_Axis_OverLoadSettings_Send.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // btn_T_Axis_OverLoadSettings_Send
            // 
            this.btn_T_Axis_OverLoadSettings_Send.BackColor = System.Drawing.Color.White;
            this.btn_T_Axis_OverLoadSettings_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_T_Axis_OverLoadSettings_Send.FlatAppearance.BorderSize = 0;
            this.btn_T_Axis_OverLoadSettings_Send.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_T_Axis_OverLoadSettings_Send.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_T_Axis_OverLoadSettings_Send.Location = new System.Drawing.Point(640, 229);
            this.btn_T_Axis_OverLoadSettings_Send.Margin = new System.Windows.Forms.Padding(1);
            this.btn_T_Axis_OverLoadSettings_Send.Name = "btn_T_Axis_OverLoadSettings_Send";
            this.btn_T_Axis_OverLoadSettings_Send.Size = new System.Drawing.Size(212, 55);
            this.btn_T_Axis_OverLoadSettings_Send.TabIndex = 23;
            this.btn_T_Axis_OverLoadSettings_Send.Text = "Send";
            this.btn_T_Axis_OverLoadSettings_Send.UseVisualStyleBackColor = false;
            this.btn_T_Axis_OverLoadSettings_Send.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_OverLoadSettings_Send_MouseDown);
            this.btn_T_Axis_OverLoadSettings_Send.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // btn_All_Axis_OverLoadSettings_Send
            // 
            this.btn_All_Axis_OverLoadSettings_Send.BackColor = System.Drawing.Color.White;
            this.btn_All_Axis_OverLoadSettings_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_All_Axis_OverLoadSettings_Send.FlatAppearance.BorderSize = 0;
            this.btn_All_Axis_OverLoadSettings_Send.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_All_Axis_OverLoadSettings_Send.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_All_Axis_OverLoadSettings_Send.Location = new System.Drawing.Point(640, 286);
            this.btn_All_Axis_OverLoadSettings_Send.Margin = new System.Windows.Forms.Padding(1);
            this.btn_All_Axis_OverLoadSettings_Send.Name = "btn_All_Axis_OverLoadSettings_Send";
            this.btn_All_Axis_OverLoadSettings_Send.Size = new System.Drawing.Size(212, 56);
            this.btn_All_Axis_OverLoadSettings_Send.TabIndex = 26;
            this.btn_All_Axis_OverLoadSettings_Send.Text = "All Send";
            this.btn_All_Axis_OverLoadSettings_Send.UseVisualStyleBackColor = false;
            this.btn_All_Axis_OverLoadSettings_Send.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_OverLoadSettings_Send_MouseDown);
            this.btn_All_Axis_OverLoadSettings_Send.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // groupBox_RackMasterOverLoadClear
            // 
            this.groupBox_RackMasterOverLoadClear.Controls.Add(this.tableLayoutPanel4);
            this.groupBox_RackMasterOverLoadClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_RackMasterOverLoadClear.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_RackMasterOverLoadClear.Location = new System.Drawing.Point(868, 380);
            this.groupBox_RackMasterOverLoadClear.Name = "groupBox_RackMasterOverLoadClear";
            this.groupBox_RackMasterOverLoadClear.Size = new System.Drawing.Size(859, 371);
            this.groupBox_RackMasterOverLoadClear.TabIndex = 8;
            this.groupBox_RackMasterOverLoadClear.TabStop = false;
            this.groupBox_RackMasterOverLoadClear.Text = "RackMaster Over Load Clear";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.Controls.Add(this.lbl_XAxisMaxLoadTitle2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lbl_ZAxisMaxLoadTitle2, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.lbl_AAxisMaxLoadTitle2, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.lbl_TAxisMaxLoadTitle2, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.btn_X_Axis_Detected_OverLoad_Clear, 3, 1);
            this.tableLayoutPanel4.Controls.Add(this.btn_Z_Axis_Detected_OverLoad_Clear, 3, 2);
            this.tableLayoutPanel4.Controls.Add(this.btn_A_Axis_Detected_OverLoad_Clear, 3, 3);
            this.tableLayoutPanel4.Controls.Add(this.btn_T_Axis_Detected_OverLoad_Clear, 3, 4);
            this.tableLayoutPanel4.Controls.Add(this.btn_All_Axis_Detected_OverLoad_Clear, 3, 5);
            this.tableLayoutPanel4.Controls.Add(this.lbl_DetectedValue, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.lbl_X_Axis_Detected_OverLoad, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.lbl_Z_Axis_Detected_OverLoad, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.lbl_A_Axis_Detected_OverLoad, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.lbl_T_Axis_Detected_OverLoad, 2, 4);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 6;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(853, 343);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // lbl_XAxisMaxLoadTitle2
            // 
            this.lbl_XAxisMaxLoadTitle2.BackColor = System.Drawing.Color.Transparent;
            this.lbl_XAxisMaxLoadTitle2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_XAxisMaxLoadTitle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_XAxisMaxLoadTitle2.Location = new System.Drawing.Point(3, 57);
            this.lbl_XAxisMaxLoadTitle2.Name = "lbl_XAxisMaxLoadTitle2";
            this.lbl_XAxisMaxLoadTitle2.Size = new System.Drawing.Size(207, 57);
            this.lbl_XAxisMaxLoadTitle2.TabIndex = 8;
            this.lbl_XAxisMaxLoadTitle2.Text = "X-Axis Max Load [%] :";
            this.lbl_XAxisMaxLoadTitle2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_ZAxisMaxLoadTitle2
            // 
            this.lbl_ZAxisMaxLoadTitle2.BackColor = System.Drawing.Color.Transparent;
            this.lbl_ZAxisMaxLoadTitle2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_ZAxisMaxLoadTitle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_ZAxisMaxLoadTitle2.Location = new System.Drawing.Point(3, 114);
            this.lbl_ZAxisMaxLoadTitle2.Name = "lbl_ZAxisMaxLoadTitle2";
            this.lbl_ZAxisMaxLoadTitle2.Size = new System.Drawing.Size(207, 57);
            this.lbl_ZAxisMaxLoadTitle2.TabIndex = 9;
            this.lbl_ZAxisMaxLoadTitle2.Text = "Z-Axis Max Load [%] :";
            this.lbl_ZAxisMaxLoadTitle2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_AAxisMaxLoadTitle2
            // 
            this.lbl_AAxisMaxLoadTitle2.BackColor = System.Drawing.Color.Transparent;
            this.lbl_AAxisMaxLoadTitle2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_AAxisMaxLoadTitle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_AAxisMaxLoadTitle2.Location = new System.Drawing.Point(3, 171);
            this.lbl_AAxisMaxLoadTitle2.Name = "lbl_AAxisMaxLoadTitle2";
            this.lbl_AAxisMaxLoadTitle2.Size = new System.Drawing.Size(207, 57);
            this.lbl_AAxisMaxLoadTitle2.TabIndex = 10;
            this.lbl_AAxisMaxLoadTitle2.Text = "A-Axis Max Load [%] :";
            this.lbl_AAxisMaxLoadTitle2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_TAxisMaxLoadTitle2
            // 
            this.lbl_TAxisMaxLoadTitle2.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TAxisMaxLoadTitle2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_TAxisMaxLoadTitle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_TAxisMaxLoadTitle2.Location = new System.Drawing.Point(3, 228);
            this.lbl_TAxisMaxLoadTitle2.Name = "lbl_TAxisMaxLoadTitle2";
            this.lbl_TAxisMaxLoadTitle2.Size = new System.Drawing.Size(207, 57);
            this.lbl_TAxisMaxLoadTitle2.TabIndex = 11;
            this.lbl_TAxisMaxLoadTitle2.Text = "T-Axis Max Load [%] :";
            this.lbl_TAxisMaxLoadTitle2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_X_Axis_Detected_OverLoad_Clear
            // 
            this.btn_X_Axis_Detected_OverLoad_Clear.BackColor = System.Drawing.Color.White;
            this.btn_X_Axis_Detected_OverLoad_Clear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_X_Axis_Detected_OverLoad_Clear.FlatAppearance.BorderSize = 0;
            this.btn_X_Axis_Detected_OverLoad_Clear.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_X_Axis_Detected_OverLoad_Clear.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_X_Axis_Detected_OverLoad_Clear.Location = new System.Drawing.Point(640, 58);
            this.btn_X_Axis_Detected_OverLoad_Clear.Margin = new System.Windows.Forms.Padding(1);
            this.btn_X_Axis_Detected_OverLoad_Clear.Name = "btn_X_Axis_Detected_OverLoad_Clear";
            this.btn_X_Axis_Detected_OverLoad_Clear.Size = new System.Drawing.Size(212, 55);
            this.btn_X_Axis_Detected_OverLoad_Clear.TabIndex = 20;
            this.btn_X_Axis_Detected_OverLoad_Clear.Text = "Clear";
            this.btn_X_Axis_Detected_OverLoad_Clear.UseVisualStyleBackColor = false;
            this.btn_X_Axis_Detected_OverLoad_Clear.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Detected_OverLoad_Clear_MouseDown);
            this.btn_X_Axis_Detected_OverLoad_Clear.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // btn_Z_Axis_Detected_OverLoad_Clear
            // 
            this.btn_Z_Axis_Detected_OverLoad_Clear.BackColor = System.Drawing.Color.White;
            this.btn_Z_Axis_Detected_OverLoad_Clear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Z_Axis_Detected_OverLoad_Clear.FlatAppearance.BorderSize = 0;
            this.btn_Z_Axis_Detected_OverLoad_Clear.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Z_Axis_Detected_OverLoad_Clear.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Z_Axis_Detected_OverLoad_Clear.Location = new System.Drawing.Point(640, 115);
            this.btn_Z_Axis_Detected_OverLoad_Clear.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Z_Axis_Detected_OverLoad_Clear.Name = "btn_Z_Axis_Detected_OverLoad_Clear";
            this.btn_Z_Axis_Detected_OverLoad_Clear.Size = new System.Drawing.Size(212, 55);
            this.btn_Z_Axis_Detected_OverLoad_Clear.TabIndex = 21;
            this.btn_Z_Axis_Detected_OverLoad_Clear.Text = "Clear";
            this.btn_Z_Axis_Detected_OverLoad_Clear.UseVisualStyleBackColor = false;
            this.btn_Z_Axis_Detected_OverLoad_Clear.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Detected_OverLoad_Clear_MouseDown);
            this.btn_Z_Axis_Detected_OverLoad_Clear.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // btn_A_Axis_Detected_OverLoad_Clear
            // 
            this.btn_A_Axis_Detected_OverLoad_Clear.BackColor = System.Drawing.Color.White;
            this.btn_A_Axis_Detected_OverLoad_Clear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_A_Axis_Detected_OverLoad_Clear.FlatAppearance.BorderSize = 0;
            this.btn_A_Axis_Detected_OverLoad_Clear.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_A_Axis_Detected_OverLoad_Clear.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_A_Axis_Detected_OverLoad_Clear.Location = new System.Drawing.Point(640, 172);
            this.btn_A_Axis_Detected_OverLoad_Clear.Margin = new System.Windows.Forms.Padding(1);
            this.btn_A_Axis_Detected_OverLoad_Clear.Name = "btn_A_Axis_Detected_OverLoad_Clear";
            this.btn_A_Axis_Detected_OverLoad_Clear.Size = new System.Drawing.Size(212, 55);
            this.btn_A_Axis_Detected_OverLoad_Clear.TabIndex = 22;
            this.btn_A_Axis_Detected_OverLoad_Clear.Text = "Clear";
            this.btn_A_Axis_Detected_OverLoad_Clear.UseVisualStyleBackColor = false;
            this.btn_A_Axis_Detected_OverLoad_Clear.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Detected_OverLoad_Clear_MouseDown);
            this.btn_A_Axis_Detected_OverLoad_Clear.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // btn_T_Axis_Detected_OverLoad_Clear
            // 
            this.btn_T_Axis_Detected_OverLoad_Clear.BackColor = System.Drawing.Color.White;
            this.btn_T_Axis_Detected_OverLoad_Clear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_T_Axis_Detected_OverLoad_Clear.FlatAppearance.BorderSize = 0;
            this.btn_T_Axis_Detected_OverLoad_Clear.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_T_Axis_Detected_OverLoad_Clear.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_T_Axis_Detected_OverLoad_Clear.Location = new System.Drawing.Point(640, 229);
            this.btn_T_Axis_Detected_OverLoad_Clear.Margin = new System.Windows.Forms.Padding(1);
            this.btn_T_Axis_Detected_OverLoad_Clear.Name = "btn_T_Axis_Detected_OverLoad_Clear";
            this.btn_T_Axis_Detected_OverLoad_Clear.Size = new System.Drawing.Size(212, 55);
            this.btn_T_Axis_Detected_OverLoad_Clear.TabIndex = 23;
            this.btn_T_Axis_Detected_OverLoad_Clear.Text = "Clear";
            this.btn_T_Axis_Detected_OverLoad_Clear.UseVisualStyleBackColor = false;
            this.btn_T_Axis_Detected_OverLoad_Clear.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Detected_OverLoad_Clear_MouseDown);
            this.btn_T_Axis_Detected_OverLoad_Clear.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // btn_All_Axis_Detected_OverLoad_Clear
            // 
            this.btn_All_Axis_Detected_OverLoad_Clear.BackColor = System.Drawing.Color.White;
            this.btn_All_Axis_Detected_OverLoad_Clear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_All_Axis_Detected_OverLoad_Clear.FlatAppearance.BorderSize = 0;
            this.btn_All_Axis_Detected_OverLoad_Clear.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_All_Axis_Detected_OverLoad_Clear.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_All_Axis_Detected_OverLoad_Clear.Location = new System.Drawing.Point(640, 286);
            this.btn_All_Axis_Detected_OverLoad_Clear.Margin = new System.Windows.Forms.Padding(1);
            this.btn_All_Axis_Detected_OverLoad_Clear.Name = "btn_All_Axis_Detected_OverLoad_Clear";
            this.btn_All_Axis_Detected_OverLoad_Clear.Size = new System.Drawing.Size(212, 56);
            this.btn_All_Axis_Detected_OverLoad_Clear.TabIndex = 26;
            this.btn_All_Axis_Detected_OverLoad_Clear.Text = "All Clear";
            this.btn_All_Axis_Detected_OverLoad_Clear.UseVisualStyleBackColor = false;
            this.btn_All_Axis_Detected_OverLoad_Clear.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Detected_OverLoad_Clear_MouseDown);
            this.btn_All_Axis_Detected_OverLoad_Clear.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // lbl_DetectedValue
            // 
            this.lbl_DetectedValue.BackColor = System.Drawing.Color.Transparent;
            this.lbl_DetectedValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_DetectedValue.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_DetectedValue.Location = new System.Drawing.Point(429, 0);
            this.lbl_DetectedValue.Name = "lbl_DetectedValue";
            this.lbl_DetectedValue.Size = new System.Drawing.Size(207, 57);
            this.lbl_DetectedValue.TabIndex = 24;
            this.lbl_DetectedValue.Text = "Detect Value";
            this.lbl_DetectedValue.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_X_Axis_Detected_OverLoad
            // 
            this.lbl_X_Axis_Detected_OverLoad.BackColor = System.Drawing.Color.Transparent;
            this.lbl_X_Axis_Detected_OverLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_X_Axis_Detected_OverLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_X_Axis_Detected_OverLoad.Location = new System.Drawing.Point(429, 57);
            this.lbl_X_Axis_Detected_OverLoad.Name = "lbl_X_Axis_Detected_OverLoad";
            this.lbl_X_Axis_Detected_OverLoad.Size = new System.Drawing.Size(207, 57);
            this.lbl_X_Axis_Detected_OverLoad.TabIndex = 12;
            this.lbl_X_Axis_Detected_OverLoad.Text = "0 %";
            this.lbl_X_Axis_Detected_OverLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_Z_Axis_Detected_OverLoad
            // 
            this.lbl_Z_Axis_Detected_OverLoad.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Z_Axis_Detected_OverLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Z_Axis_Detected_OverLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_Z_Axis_Detected_OverLoad.Location = new System.Drawing.Point(429, 114);
            this.lbl_Z_Axis_Detected_OverLoad.Name = "lbl_Z_Axis_Detected_OverLoad";
            this.lbl_Z_Axis_Detected_OverLoad.Size = new System.Drawing.Size(207, 57);
            this.lbl_Z_Axis_Detected_OverLoad.TabIndex = 13;
            this.lbl_Z_Axis_Detected_OverLoad.Text = "0 %";
            this.lbl_Z_Axis_Detected_OverLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_A_Axis_Detected_OverLoad
            // 
            this.lbl_A_Axis_Detected_OverLoad.BackColor = System.Drawing.Color.Transparent;
            this.lbl_A_Axis_Detected_OverLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_A_Axis_Detected_OverLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_A_Axis_Detected_OverLoad.Location = new System.Drawing.Point(429, 171);
            this.lbl_A_Axis_Detected_OverLoad.Name = "lbl_A_Axis_Detected_OverLoad";
            this.lbl_A_Axis_Detected_OverLoad.Size = new System.Drawing.Size(207, 57);
            this.lbl_A_Axis_Detected_OverLoad.TabIndex = 14;
            this.lbl_A_Axis_Detected_OverLoad.Text = "0 %";
            this.lbl_A_Axis_Detected_OverLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_T_Axis_Detected_OverLoad
            // 
            this.lbl_T_Axis_Detected_OverLoad.BackColor = System.Drawing.Color.Transparent;
            this.lbl_T_Axis_Detected_OverLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_T_Axis_Detected_OverLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_T_Axis_Detected_OverLoad.Location = new System.Drawing.Point(429, 228);
            this.lbl_T_Axis_Detected_OverLoad.Name = "lbl_T_Axis_Detected_OverLoad";
            this.lbl_T_Axis_Detected_OverLoad.Size = new System.Drawing.Size(207, 57);
            this.lbl_T_Axis_Detected_OverLoad.TabIndex = 15;
            this.lbl_T_Axis_Detected_OverLoad.Text = "0 %";
            this.lbl_T_Axis_Detected_OverLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_MaintMove
            // 
            this.btn_MaintMove.BackColor = System.Drawing.Color.White;
            this.btn_MaintMove.FlatAppearance.BorderSize = 0;
            this.btn_MaintMove.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_MaintMove.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_MaintMove.Location = new System.Drawing.Point(214, 1);
            this.btn_MaintMove.Margin = new System.Windows.Forms.Padding(1);
            this.btn_MaintMove.Name = "btn_MaintMove";
            this.btn_MaintMove.Size = new System.Drawing.Size(211, 83);
            this.btn_MaintMove.TabIndex = 28;
            this.btn_MaintMove.Text = "Maint Move";
            this.btn_MaintMove.UseVisualStyleBackColor = false;
            this.btn_MaintMove.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MaintMove_MouseDown);
            this.btn_MaintMove.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Send_MouseUp);
            // 
            // Frm_RackMasterTPSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1730, 754);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_RackMasterTPSettings";
            this.Text = "Frm_PortTPMain";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox_RackMasterSubCommand.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.groupBox_RackMasterSpeedSetting.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox_RackMasterOverLoadSetting.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox_RackMasterOverLoadClear.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer UIUpdateTimer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox_RackMasterSpeedSetting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lbl_XAxisSpeedTitle;
        private System.Windows.Forms.Label lbl_ZAxisSpeedTitle;
        private System.Windows.Forms.Label lbl_AAxisSpeedTitle;
        private System.Windows.Forms.Label lbl_TAxisSpeedTitle;
        private System.Windows.Forms.Label lbl_X_Axis_SpeedSetting;
        private System.Windows.Forms.Label lbl_Z_Axis_SpeedSetting;
        private System.Windows.Forms.Label lbl_A_Axis_SpeedSetting;
        private System.Windows.Forms.Label lbl_T_Axis_SpeedSetting;
        private System.Windows.Forms.TextBox tbx_X_Axis_SpeedSettings;
        private System.Windows.Forms.TextBox tbx_Z_Axis_SpeedSettings;
        private System.Windows.Forms.TextBox tbx_A_Axis_SpeedSettings;
        private System.Windows.Forms.TextBox tbx_T_Axis_SpeedSettings;
        private System.Windows.Forms.Button btn_X_Axis_SpeedSettings_Send;
        private System.Windows.Forms.Button btn_Z_Axis_SpeedSettings_Send;
        private System.Windows.Forms.Button btn_A_Axis_SpeedSettings_Send;
        private System.Windows.Forms.Button btn_T_Axis_SpeedSettings_Send;
        private System.Windows.Forms.Label lbl_AppliedValue;
        private System.Windows.Forms.GroupBox groupBox_RackMasterSubCommand;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btn_TimeSync;
        private System.Windows.Forms.Label lbl_SetValue;
        private System.Windows.Forms.Button btn_All_Axis_SpeedSettings_Send;
        private System.Windows.Forms.GroupBox groupBox_RackMasterOverLoadSetting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lbl_SetValue2;
        private System.Windows.Forms.Label lbl_AppliedValue2;
        private System.Windows.Forms.Label lbl_XAxisMaxLoadTitle;
        private System.Windows.Forms.Label lbl_ZAxisMaxLoadTitle;
        private System.Windows.Forms.Label lbl_AAxisMaxLoadTitle;
        private System.Windows.Forms.Label lbl_TAxisMaxLoadTitle;
        private System.Windows.Forms.Label lbl_X_Axis_Read_SetMaxLoad;
        private System.Windows.Forms.Label lbl_Z_Axis_Read_SetMaxLoad;
        private System.Windows.Forms.Label lbl_A_Axis_Read_SetMaxLoad;
        private System.Windows.Forms.Label lbl_T_Axis_Read_SetMaxLoad;
        private System.Windows.Forms.TextBox tbx_X_Axis_OverLoadSettings;
        private System.Windows.Forms.TextBox tbx_Z_Axis_OverLoadSettings;
        private System.Windows.Forms.TextBox tbx_A_Axis_OverLoadSettings;
        private System.Windows.Forms.TextBox tbx_T_Axis_OverLoadSettings;
        private System.Windows.Forms.Button btn_X_Axis_OverLoadSettings_Send;
        private System.Windows.Forms.Button btn_Z_Axis_OverLoadSettings_Send;
        private System.Windows.Forms.Button btn_A_Axis_OverLoadSettings_Send;
        private System.Windows.Forms.Button btn_T_Axis_OverLoadSettings_Send;
        private System.Windows.Forms.Button btn_All_Axis_OverLoadSettings_Send;
        private System.Windows.Forms.GroupBox groupBox_RackMasterOverLoadClear;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label lbl_XAxisMaxLoadTitle2;
        private System.Windows.Forms.Label lbl_ZAxisMaxLoadTitle2;
        private System.Windows.Forms.Label lbl_AAxisMaxLoadTitle2;
        private System.Windows.Forms.Label lbl_TAxisMaxLoadTitle2;
        private System.Windows.Forms.Button btn_X_Axis_Detected_OverLoad_Clear;
        private System.Windows.Forms.Button btn_Z_Axis_Detected_OverLoad_Clear;
        private System.Windows.Forms.Button btn_A_Axis_Detected_OverLoad_Clear;
        private System.Windows.Forms.Button btn_T_Axis_Detected_OverLoad_Clear;
        private System.Windows.Forms.Button btn_All_Axis_Detected_OverLoad_Clear;
        private System.Windows.Forms.Label lbl_DetectedValue;
        private System.Windows.Forms.Label lbl_X_Axis_Detected_OverLoad;
        private System.Windows.Forms.Label lbl_Z_Axis_Detected_OverLoad;
        private System.Windows.Forms.Label lbl_A_Axis_Detected_OverLoad;
        private System.Windows.Forms.Label lbl_T_Axis_Detected_OverLoad;
        private System.Windows.Forms.Button btn_MaintMove;
    }
}