
namespace Master.SubForm.PortTPForm.PortTPSubForm.ControlForm
{
    partial class Frm_ConveyorMotion
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
            this.groupBox_ConveyorCommand = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox_SyncMove = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_SyncBWD = new System.Windows.Forms.Label();
            this.btn_SyncMoveBWD = new System.Windows.Forms.Button();
            this.btn_SyncMoveFWD = new System.Windows.Forms.Button();
            this.lbl_SyncFWD = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Buffer_BP2 = new System.Windows.Forms.Button();
            this.btn_Buffer_BP1 = new System.Windows.Forms.Button();
            this.btn_Buffer_OP = new System.Windows.Forms.Button();
            this.btn_Buffer_LP = new System.Windows.Forms.Button();
            this.btn_Buffer_BP3 = new System.Windows.Forms.Button();
            this.btn_Buffer_BP4 = new System.Windows.Forms.Button();
            this.groupBox_Move = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_HighSpeed_BWD = new System.Windows.Forms.Label();
            this.btn_HighSpeed_FWD = new System.Windows.Forms.Button();
            this.btn_LowSpeed_BWD = new System.Windows.Forms.Button();
            this.btn_HighSpeed_BWD = new System.Windows.Forms.Button();
            this.btn_LowSpeed_FWD = new System.Windows.Forms.Button();
            this.lbl_LowSpeed_FWD = new System.Windows.Forms.Label();
            this.lbl_HighSpeed_FWD = new System.Windows.Forms.Label();
            this.lbl_LowSpeed_BWD = new System.Windows.Forms.Label();
            this.groupBox_CylinderCommand = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox_Centering = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Centering_BWD = new System.Windows.Forms.Button();
            this.btn_Centering_FWD = new System.Windows.Forms.Button();
            this.lbl_CenteringFWD = new System.Windows.Forms.Label();
            this.lbl_CenteringBWD = new System.Windows.Forms.Label();
            this.groupBox_Stopper = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Stopper_BWD = new System.Windows.Forms.Button();
            this.btn_Stopper_FWD = new System.Windows.Forms.Button();
            this.lbl_StopperFWD = new System.Windows.Forms.Label();
            this.lbl_StopperBWD = new System.Windows.Forms.Label();
            this.groupBox_Flag = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_ConveyorReset = new System.Windows.Forms.Button();
            this.btn_ConveyorStop = new System.Windows.Forms.Button();
            this.btn_CenteringStop = new System.Windows.Forms.Button();
            this.btn_StopperStop = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox_ConveyorCommand.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox_SyncMove.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox_Move.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.groupBox_CylinderCommand.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox_Centering.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox_Stopper.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox_Flag.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // UIUpdateTimer
            // 
            this.UIUpdateTimer.Interval = 250;
            this.UIUpdateTimer.Tick += new System.EventHandler(this.UIUpdateTimer_Tick);
            // 
            // groupBox_ConveyorCommand
            // 
            this.groupBox_ConveyorCommand.Controls.Add(this.panel1);
            this.groupBox_ConveyorCommand.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_ConveyorCommand.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_ConveyorCommand.Location = new System.Drawing.Point(0, 230);
            this.groupBox_ConveyorCommand.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_ConveyorCommand.Name = "groupBox_ConveyorCommand";
            this.groupBox_ConveyorCommand.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_ConveyorCommand.Size = new System.Drawing.Size(532, 260);
            this.groupBox_ConveyorCommand.TabIndex = 8;
            this.groupBox_ConveyorCommand.TabStop = false;
            this.groupBox_ConveyorCommand.Text = "Conveyor Command";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox_SyncMove);
            this.panel1.Controls.Add(this.groupBox_Move);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(530, 240);
            this.panel1.TabIndex = 2;
            // 
            // groupBox_SyncMove
            // 
            this.groupBox_SyncMove.Controls.Add(this.tableLayoutPanel4);
            this.groupBox_SyncMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_SyncMove.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_SyncMove.Location = new System.Drawing.Point(0, 100);
            this.groupBox_SyncMove.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_SyncMove.Name = "groupBox_SyncMove";
            this.groupBox_SyncMove.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_SyncMove.Size = new System.Drawing.Size(530, 140);
            this.groupBox_SyncMove.TabIndex = 13;
            this.groupBox_SyncMove.TabStop = false;
            this.groupBox_SyncMove.Text = "Sync Move";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(1, 19);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(528, 120);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Controls.Add(this.lbl_SyncBWD, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btn_SyncMoveBWD, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.btn_SyncMoveFWD, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.lbl_SyncFWD, 3, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 60);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(528, 60);
            this.tableLayoutPanel6.TabIndex = 1;
            // 
            // lbl_SyncBWD
            // 
            this.lbl_SyncBWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_SyncBWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_SyncBWD.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_SyncBWD.Location = new System.Drawing.Point(3, 3);
            this.lbl_SyncBWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_SyncBWD.Name = "lbl_SyncBWD";
            this.lbl_SyncBWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_SyncBWD.Size = new System.Drawing.Size(126, 54);
            this.lbl_SyncBWD.TabIndex = 32;
            this.lbl_SyncBWD.Text = "BWD";
            this.lbl_SyncBWD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_SyncMoveBWD
            // 
            this.btn_SyncMoveBWD.BackColor = System.Drawing.Color.White;
            this.btn_SyncMoveBWD.BackgroundImage = global::Master.Properties.Resources.icons8_back_96;
            this.btn_SyncMoveBWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_SyncMoveBWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_SyncMoveBWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_SyncMoveBWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SyncMoveBWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SyncMoveBWD.Location = new System.Drawing.Point(133, 1);
            this.btn_SyncMoveBWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_SyncMoveBWD.Name = "btn_SyncMoveBWD";
            this.btn_SyncMoveBWD.Size = new System.Drawing.Size(130, 58);
            this.btn_SyncMoveBWD.TabIndex = 23;
            this.btn_SyncMoveBWD.UseVisualStyleBackColor = false;
            this.btn_SyncMoveBWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_SyncMove_MouseDown);
            this.btn_SyncMoveBWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_SyncMoveBWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_SyncMove_MouseUp);
            // 
            // btn_SyncMoveFWD
            // 
            this.btn_SyncMoveFWD.BackColor = System.Drawing.Color.White;
            this.btn_SyncMoveFWD.BackgroundImage = global::Master.Properties.Resources.icons8_forward_96;
            this.btn_SyncMoveFWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_SyncMoveFWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_SyncMoveFWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_SyncMoveFWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SyncMoveFWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SyncMoveFWD.Location = new System.Drawing.Point(265, 1);
            this.btn_SyncMoveFWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_SyncMoveFWD.Name = "btn_SyncMoveFWD";
            this.btn_SyncMoveFWD.Size = new System.Drawing.Size(130, 58);
            this.btn_SyncMoveFWD.TabIndex = 24;
            this.btn_SyncMoveFWD.UseVisualStyleBackColor = false;
            this.btn_SyncMoveFWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_SyncMove_MouseDown);
            this.btn_SyncMoveFWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_SyncMoveFWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_SyncMove_MouseUp);
            // 
            // lbl_SyncFWD
            // 
            this.lbl_SyncFWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_SyncFWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_SyncFWD.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_SyncFWD.Location = new System.Drawing.Point(399, 3);
            this.lbl_SyncFWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_SyncFWD.Name = "lbl_SyncFWD";
            this.lbl_SyncFWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_SyncFWD.Size = new System.Drawing.Size(126, 54);
            this.lbl_SyncFWD.TabIndex = 33;
            this.lbl_SyncFWD.Text = "FWD";
            this.lbl_SyncFWD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 6;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.btn_Buffer_BP2, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.btn_Buffer_BP1, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.btn_Buffer_OP, 5, 0);
            this.tableLayoutPanel5.Controls.Add(this.btn_Buffer_LP, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btn_Buffer_BP3, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.btn_Buffer_BP4, 4, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(528, 60);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // btn_Buffer_BP2
            // 
            this.btn_Buffer_BP2.BackColor = System.Drawing.Color.White;
            this.btn_Buffer_BP2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Buffer_BP2.FlatAppearance.BorderSize = 0;
            this.btn_Buffer_BP2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_Buffer_BP2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Buffer_BP2.Location = new System.Drawing.Point(175, 1);
            this.btn_Buffer_BP2.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Buffer_BP2.Name = "btn_Buffer_BP2";
            this.btn_Buffer_BP2.Size = new System.Drawing.Size(85, 58);
            this.btn_Buffer_BP2.TabIndex = 30;
            this.btn_Buffer_BP2.Text = "Buffer BP2";
            this.btn_Buffer_BP2.UseVisualStyleBackColor = false;
            this.btn_Buffer_BP2.Click += new System.EventHandler(this.btn_SyncBuffer_Click);
            // 
            // btn_Buffer_BP1
            // 
            this.btn_Buffer_BP1.BackColor = System.Drawing.Color.White;
            this.btn_Buffer_BP1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Buffer_BP1.FlatAppearance.BorderSize = 0;
            this.btn_Buffer_BP1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_Buffer_BP1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Buffer_BP1.Location = new System.Drawing.Point(88, 1);
            this.btn_Buffer_BP1.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Buffer_BP1.Name = "btn_Buffer_BP1";
            this.btn_Buffer_BP1.Size = new System.Drawing.Size(85, 58);
            this.btn_Buffer_BP1.TabIndex = 29;
            this.btn_Buffer_BP1.Text = "Buffer BP1";
            this.btn_Buffer_BP1.UseVisualStyleBackColor = false;
            this.btn_Buffer_BP1.Click += new System.EventHandler(this.btn_SyncBuffer_Click);
            // 
            // btn_Buffer_OP
            // 
            this.btn_Buffer_OP.BackColor = System.Drawing.Color.White;
            this.btn_Buffer_OP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Buffer_OP.FlatAppearance.BorderSize = 0;
            this.btn_Buffer_OP.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_Buffer_OP.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Buffer_OP.Location = new System.Drawing.Point(436, 1);
            this.btn_Buffer_OP.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Buffer_OP.Name = "btn_Buffer_OP";
            this.btn_Buffer_OP.Size = new System.Drawing.Size(91, 58);
            this.btn_Buffer_OP.TabIndex = 28;
            this.btn_Buffer_OP.Text = "Buffer OP";
            this.btn_Buffer_OP.UseVisualStyleBackColor = false;
            this.btn_Buffer_OP.Click += new System.EventHandler(this.btn_SyncBuffer_Click);
            // 
            // btn_Buffer_LP
            // 
            this.btn_Buffer_LP.BackColor = System.Drawing.Color.White;
            this.btn_Buffer_LP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Buffer_LP.FlatAppearance.BorderSize = 0;
            this.btn_Buffer_LP.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_Buffer_LP.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Buffer_LP.Location = new System.Drawing.Point(1, 1);
            this.btn_Buffer_LP.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Buffer_LP.Name = "btn_Buffer_LP";
            this.btn_Buffer_LP.Size = new System.Drawing.Size(85, 58);
            this.btn_Buffer_LP.TabIndex = 27;
            this.btn_Buffer_LP.Text = "Buffer LP";
            this.btn_Buffer_LP.UseVisualStyleBackColor = false;
            this.btn_Buffer_LP.Click += new System.EventHandler(this.btn_SyncBuffer_Click);
            // 
            // btn_Buffer_BP3
            // 
            this.btn_Buffer_BP3.BackColor = System.Drawing.Color.White;
            this.btn_Buffer_BP3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Buffer_BP3.FlatAppearance.BorderSize = 0;
            this.btn_Buffer_BP3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_Buffer_BP3.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Buffer_BP3.Location = new System.Drawing.Point(262, 1);
            this.btn_Buffer_BP3.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Buffer_BP3.Name = "btn_Buffer_BP3";
            this.btn_Buffer_BP3.Size = new System.Drawing.Size(85, 58);
            this.btn_Buffer_BP3.TabIndex = 31;
            this.btn_Buffer_BP3.Text = "Buffer BP3";
            this.btn_Buffer_BP3.UseVisualStyleBackColor = false;
            this.btn_Buffer_BP3.Click += new System.EventHandler(this.btn_SyncBuffer_Click);
            // 
            // btn_Buffer_BP4
            // 
            this.btn_Buffer_BP4.BackColor = System.Drawing.Color.White;
            this.btn_Buffer_BP4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Buffer_BP4.FlatAppearance.BorderSize = 0;
            this.btn_Buffer_BP4.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btn_Buffer_BP4.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Buffer_BP4.Location = new System.Drawing.Point(349, 1);
            this.btn_Buffer_BP4.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Buffer_BP4.Name = "btn_Buffer_BP4";
            this.btn_Buffer_BP4.Size = new System.Drawing.Size(85, 58);
            this.btn_Buffer_BP4.TabIndex = 32;
            this.btn_Buffer_BP4.Text = "Buffer BP4";
            this.btn_Buffer_BP4.UseVisualStyleBackColor = false;
            this.btn_Buffer_BP4.Click += new System.EventHandler(this.btn_SyncBuffer_Click);
            // 
            // groupBox_Move
            // 
            this.groupBox_Move.Controls.Add(this.tableLayoutPanel15);
            this.groupBox_Move.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Move.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Move.Location = new System.Drawing.Point(0, 0);
            this.groupBox_Move.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_Move.Name = "groupBox_Move";
            this.groupBox_Move.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_Move.Size = new System.Drawing.Size(530, 100);
            this.groupBox_Move.TabIndex = 12;
            this.groupBox_Move.TabStop = false;
            this.groupBox_Move.Text = "Move";
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 4;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.Controls.Add(this.lbl_HighSpeed_BWD, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.btn_HighSpeed_FWD, 3, 1);
            this.tableLayoutPanel15.Controls.Add(this.btn_LowSpeed_BWD, 1, 1);
            this.tableLayoutPanel15.Controls.Add(this.btn_HighSpeed_BWD, 0, 1);
            this.tableLayoutPanel15.Controls.Add(this.btn_LowSpeed_FWD, 2, 1);
            this.tableLayoutPanel15.Controls.Add(this.lbl_LowSpeed_FWD, 2, 0);
            this.tableLayoutPanel15.Controls.Add(this.lbl_HighSpeed_FWD, 3, 0);
            this.tableLayoutPanel15.Controls.Add(this.lbl_LowSpeed_BWD, 1, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(1, 19);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 2;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(528, 80);
            this.tableLayoutPanel15.TabIndex = 0;
            // 
            // lbl_HighSpeed_BWD
            // 
            this.lbl_HighSpeed_BWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_HighSpeed_BWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_HighSpeed_BWD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_HighSpeed_BWD.Location = new System.Drawing.Point(3, 3);
            this.lbl_HighSpeed_BWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_HighSpeed_BWD.Name = "lbl_HighSpeed_BWD";
            this.lbl_HighSpeed_BWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_HighSpeed_BWD.Size = new System.Drawing.Size(126, 24);
            this.lbl_HighSpeed_BWD.TabIndex = 26;
            this.lbl_HighSpeed_BWD.Text = "High Speed BWD";
            this.lbl_HighSpeed_BWD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btn_HighSpeed_FWD
            // 
            this.btn_HighSpeed_FWD.BackColor = System.Drawing.Color.White;
            this.btn_HighSpeed_FWD.BackgroundImage = global::Master.Properties.Resources.icons8_double_right_96;
            this.btn_HighSpeed_FWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_HighSpeed_FWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_HighSpeed_FWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_HighSpeed_FWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_HighSpeed_FWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_HighSpeed_FWD.Location = new System.Drawing.Point(397, 31);
            this.btn_HighSpeed_FWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_HighSpeed_FWD.Name = "btn_HighSpeed_FWD";
            this.btn_HighSpeed_FWD.Size = new System.Drawing.Size(130, 48);
            this.btn_HighSpeed_FWD.TabIndex = 25;
            this.btn_HighSpeed_FWD.UseVisualStyleBackColor = false;
            this.btn_HighSpeed_FWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_ConveyorMotion_MouseDown);
            this.btn_HighSpeed_FWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_HighSpeed_FWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_ConveyorMotion_MouseUp);
            // 
            // btn_LowSpeed_BWD
            // 
            this.btn_LowSpeed_BWD.BackColor = System.Drawing.Color.White;
            this.btn_LowSpeed_BWD.BackgroundImage = global::Master.Properties.Resources.icons8_back_96;
            this.btn_LowSpeed_BWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_LowSpeed_BWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_LowSpeed_BWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_LowSpeed_BWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_LowSpeed_BWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LowSpeed_BWD.Location = new System.Drawing.Point(133, 31);
            this.btn_LowSpeed_BWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_LowSpeed_BWD.Name = "btn_LowSpeed_BWD";
            this.btn_LowSpeed_BWD.Size = new System.Drawing.Size(130, 48);
            this.btn_LowSpeed_BWD.TabIndex = 23;
            this.btn_LowSpeed_BWD.UseVisualStyleBackColor = false;
            this.btn_LowSpeed_BWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_ConveyorMotion_MouseDown);
            this.btn_LowSpeed_BWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_LowSpeed_BWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_ConveyorMotion_MouseUp);
            // 
            // btn_HighSpeed_BWD
            // 
            this.btn_HighSpeed_BWD.BackColor = System.Drawing.Color.White;
            this.btn_HighSpeed_BWD.BackgroundImage = global::Master.Properties.Resources.icons8_double_left_96;
            this.btn_HighSpeed_BWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_HighSpeed_BWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_HighSpeed_BWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_HighSpeed_BWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_HighSpeed_BWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_HighSpeed_BWD.Location = new System.Drawing.Point(1, 31);
            this.btn_HighSpeed_BWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_HighSpeed_BWD.Name = "btn_HighSpeed_BWD";
            this.btn_HighSpeed_BWD.Size = new System.Drawing.Size(130, 48);
            this.btn_HighSpeed_BWD.TabIndex = 7;
            this.btn_HighSpeed_BWD.UseVisualStyleBackColor = false;
            this.btn_HighSpeed_BWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_ConveyorMotion_MouseDown);
            this.btn_HighSpeed_BWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_HighSpeed_BWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_ConveyorMotion_MouseUp);
            // 
            // btn_LowSpeed_FWD
            // 
            this.btn_LowSpeed_FWD.BackColor = System.Drawing.Color.White;
            this.btn_LowSpeed_FWD.BackgroundImage = global::Master.Properties.Resources.icons8_forward_96;
            this.btn_LowSpeed_FWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_LowSpeed_FWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_LowSpeed_FWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_LowSpeed_FWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_LowSpeed_FWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LowSpeed_FWD.Location = new System.Drawing.Point(265, 31);
            this.btn_LowSpeed_FWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_LowSpeed_FWD.Name = "btn_LowSpeed_FWD";
            this.btn_LowSpeed_FWD.Size = new System.Drawing.Size(130, 48);
            this.btn_LowSpeed_FWD.TabIndex = 24;
            this.btn_LowSpeed_FWD.UseVisualStyleBackColor = false;
            this.btn_LowSpeed_FWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_ConveyorMotion_MouseDown);
            this.btn_LowSpeed_FWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_LowSpeed_FWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_ConveyorMotion_MouseUp);
            // 
            // lbl_LowSpeed_FWD
            // 
            this.lbl_LowSpeed_FWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_LowSpeed_FWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_LowSpeed_FWD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_LowSpeed_FWD.Location = new System.Drawing.Point(267, 3);
            this.lbl_LowSpeed_FWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_LowSpeed_FWD.Name = "lbl_LowSpeed_FWD";
            this.lbl_LowSpeed_FWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_LowSpeed_FWD.Size = new System.Drawing.Size(126, 24);
            this.lbl_LowSpeed_FWD.TabIndex = 27;
            this.lbl_LowSpeed_FWD.Text = "Low Speed FWD";
            this.lbl_LowSpeed_FWD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_HighSpeed_FWD
            // 
            this.lbl_HighSpeed_FWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_HighSpeed_FWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_HighSpeed_FWD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_HighSpeed_FWD.Location = new System.Drawing.Point(399, 3);
            this.lbl_HighSpeed_FWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_HighSpeed_FWD.Name = "lbl_HighSpeed_FWD";
            this.lbl_HighSpeed_FWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_HighSpeed_FWD.Size = new System.Drawing.Size(126, 24);
            this.lbl_HighSpeed_FWD.TabIndex = 28;
            this.lbl_HighSpeed_FWD.Text = "High Speed FWD";
            this.lbl_HighSpeed_FWD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_LowSpeed_BWD
            // 
            this.lbl_LowSpeed_BWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_LowSpeed_BWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_LowSpeed_BWD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_LowSpeed_BWD.Location = new System.Drawing.Point(135, 3);
            this.lbl_LowSpeed_BWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_LowSpeed_BWD.Name = "lbl_LowSpeed_BWD";
            this.lbl_LowSpeed_BWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_LowSpeed_BWD.Size = new System.Drawing.Size(126, 24);
            this.lbl_LowSpeed_BWD.TabIndex = 29;
            this.lbl_LowSpeed_BWD.Text = "Low Speed BWD";
            this.lbl_LowSpeed_BWD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // groupBox_CylinderCommand
            // 
            this.groupBox_CylinderCommand.Controls.Add(this.panel2);
            this.groupBox_CylinderCommand.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_CylinderCommand.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_CylinderCommand.Location = new System.Drawing.Point(0, 0);
            this.groupBox_CylinderCommand.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_CylinderCommand.Name = "groupBox_CylinderCommand";
            this.groupBox_CylinderCommand.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_CylinderCommand.Size = new System.Drawing.Size(532, 230);
            this.groupBox_CylinderCommand.TabIndex = 10;
            this.groupBox_CylinderCommand.TabStop = false;
            this.groupBox_CylinderCommand.Text = "Cylinder Command";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox_Centering);
            this.panel2.Controls.Add(this.groupBox_Stopper);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(1, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(530, 210);
            this.panel2.TabIndex = 2;
            // 
            // groupBox_Centering
            // 
            this.groupBox_Centering.Controls.Add(this.tableLayoutPanel3);
            this.groupBox_Centering.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Centering.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Centering.Location = new System.Drawing.Point(0, 100);
            this.groupBox_Centering.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_Centering.Name = "groupBox_Centering";
            this.groupBox_Centering.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_Centering.Size = new System.Drawing.Size(530, 100);
            this.groupBox_Centering.TabIndex = 13;
            this.groupBox_Centering.TabStop = false;
            this.groupBox_Centering.Text = "Centering";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.btn_Centering_BWD, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.btn_Centering_FWD, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.lbl_CenteringFWD, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.lbl_CenteringBWD, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(1, 19);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(528, 80);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // btn_Centering_BWD
            // 
            this.btn_Centering_BWD.BackColor = System.Drawing.Color.White;
            this.btn_Centering_BWD.BackgroundImage = global::Master.Properties.Resources.icons8_back_96;
            this.btn_Centering_BWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Centering_BWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Centering_BWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_Centering_BWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Centering_BWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Centering_BWD.Location = new System.Drawing.Point(133, 31);
            this.btn_Centering_BWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Centering_BWD.Name = "btn_Centering_BWD";
            this.btn_Centering_BWD.Size = new System.Drawing.Size(130, 48);
            this.btn_Centering_BWD.TabIndex = 23;
            this.btn_Centering_BWD.UseVisualStyleBackColor = false;
            this.btn_Centering_BWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Centering_MouseDown);
            this.btn_Centering_BWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_Centering_BWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Centering_MouseUp);
            // 
            // btn_Centering_FWD
            // 
            this.btn_Centering_FWD.BackColor = System.Drawing.Color.White;
            this.btn_Centering_FWD.BackgroundImage = global::Master.Properties.Resources.icons8_forward_96;
            this.btn_Centering_FWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Centering_FWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Centering_FWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_Centering_FWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Centering_FWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Centering_FWD.Location = new System.Drawing.Point(265, 31);
            this.btn_Centering_FWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Centering_FWD.Name = "btn_Centering_FWD";
            this.btn_Centering_FWD.Size = new System.Drawing.Size(130, 48);
            this.btn_Centering_FWD.TabIndex = 24;
            this.btn_Centering_FWD.UseVisualStyleBackColor = false;
            this.btn_Centering_FWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Centering_MouseDown);
            this.btn_Centering_FWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_Centering_FWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Centering_MouseUp);
            // 
            // lbl_CenteringFWD
            // 
            this.lbl_CenteringFWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_CenteringFWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_CenteringFWD.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_CenteringFWD.Location = new System.Drawing.Point(267, 3);
            this.lbl_CenteringFWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_CenteringFWD.Name = "lbl_CenteringFWD";
            this.lbl_CenteringFWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_CenteringFWD.Size = new System.Drawing.Size(126, 24);
            this.lbl_CenteringFWD.TabIndex = 27;
            this.lbl_CenteringFWD.Text = "FWD";
            this.lbl_CenteringFWD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_CenteringBWD
            // 
            this.lbl_CenteringBWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_CenteringBWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_CenteringBWD.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_CenteringBWD.Location = new System.Drawing.Point(135, 3);
            this.lbl_CenteringBWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_CenteringBWD.Name = "lbl_CenteringBWD";
            this.lbl_CenteringBWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_CenteringBWD.Size = new System.Drawing.Size(126, 24);
            this.lbl_CenteringBWD.TabIndex = 29;
            this.lbl_CenteringBWD.Text = "BWD";
            this.lbl_CenteringBWD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // groupBox_Stopper
            // 
            this.groupBox_Stopper.Controls.Add(this.tableLayoutPanel2);
            this.groupBox_Stopper.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Stopper.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Stopper.Location = new System.Drawing.Point(0, 0);
            this.groupBox_Stopper.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_Stopper.Name = "groupBox_Stopper";
            this.groupBox_Stopper.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_Stopper.Size = new System.Drawing.Size(530, 100);
            this.groupBox_Stopper.TabIndex = 12;
            this.groupBox_Stopper.TabStop = false;
            this.groupBox_Stopper.Text = "Stopper";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.btn_Stopper_BWD, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.btn_Stopper_FWD, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.lbl_StopperFWD, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbl_StopperBWD, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 19);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(528, 80);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btn_Stopper_BWD
            // 
            this.btn_Stopper_BWD.BackColor = System.Drawing.Color.White;
            this.btn_Stopper_BWD.BackgroundImage = global::Master.Properties.Resources.icons8_back_96;
            this.btn_Stopper_BWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Stopper_BWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Stopper_BWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_Stopper_BWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Stopper_BWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Stopper_BWD.Location = new System.Drawing.Point(133, 31);
            this.btn_Stopper_BWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Stopper_BWD.Name = "btn_Stopper_BWD";
            this.btn_Stopper_BWD.Size = new System.Drawing.Size(130, 48);
            this.btn_Stopper_BWD.TabIndex = 23;
            this.btn_Stopper_BWD.UseVisualStyleBackColor = false;
            this.btn_Stopper_BWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Stopper_MouseDown);
            this.btn_Stopper_BWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_Stopper_BWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Stopper_MouseUp);
            // 
            // btn_Stopper_FWD
            // 
            this.btn_Stopper_FWD.BackColor = System.Drawing.Color.White;
            this.btn_Stopper_FWD.BackgroundImage = global::Master.Properties.Resources.icons8_forward_96;
            this.btn_Stopper_FWD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Stopper_FWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Stopper_FWD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_Stopper_FWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Stopper_FWD.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Stopper_FWD.Location = new System.Drawing.Point(265, 31);
            this.btn_Stopper_FWD.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Stopper_FWD.Name = "btn_Stopper_FWD";
            this.btn_Stopper_FWD.Size = new System.Drawing.Size(130, 48);
            this.btn_Stopper_FWD.TabIndex = 24;
            this.btn_Stopper_FWD.UseVisualStyleBackColor = false;
            this.btn_Stopper_FWD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Stopper_MouseDown);
            this.btn_Stopper_FWD.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            this.btn_Stopper_FWD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_Stopper_MouseUp);
            // 
            // lbl_StopperFWD
            // 
            this.lbl_StopperFWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_StopperFWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_StopperFWD.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_StopperFWD.Location = new System.Drawing.Point(267, 3);
            this.lbl_StopperFWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_StopperFWD.Name = "lbl_StopperFWD";
            this.lbl_StopperFWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_StopperFWD.Size = new System.Drawing.Size(126, 24);
            this.lbl_StopperFWD.TabIndex = 27;
            this.lbl_StopperFWD.Text = "FWD";
            this.lbl_StopperFWD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_StopperBWD
            // 
            this.lbl_StopperBWD.BackColor = System.Drawing.Color.Transparent;
            this.lbl_StopperBWD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_StopperBWD.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_StopperBWD.Location = new System.Drawing.Point(135, 3);
            this.lbl_StopperBWD.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_StopperBWD.Name = "lbl_StopperBWD";
            this.lbl_StopperBWD.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_StopperBWD.Size = new System.Drawing.Size(126, 24);
            this.lbl_StopperBWD.TabIndex = 29;
            this.lbl_StopperBWD.Text = "BWD";
            this.lbl_StopperBWD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // groupBox_Flag
            // 
            this.groupBox_Flag.Controls.Add(this.tableLayoutPanel18);
            this.groupBox_Flag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Flag.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Flag.Location = new System.Drawing.Point(1, 1);
            this.groupBox_Flag.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_Flag.Name = "groupBox_Flag";
            this.groupBox_Flag.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_Flag.Size = new System.Drawing.Size(132, 561);
            this.groupBox_Flag.TabIndex = 11;
            this.groupBox_Flag.TabStop = false;
            this.groupBox_Flag.Text = "Flag";
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.ColumnCount = 1;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel18.Controls.Add(this.btn_ConveyorReset, 0, 1);
            this.tableLayoutPanel18.Controls.Add(this.btn_ConveyorStop, 0, 0);
            this.tableLayoutPanel18.Controls.Add(this.btn_CenteringStop, 0, 3);
            this.tableLayoutPanel18.Controls.Add(this.btn_StopperStop, 0, 2);
            this.tableLayoutPanel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel18.Location = new System.Drawing.Point(1, 19);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.RowCount = 5;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel18.Size = new System.Drawing.Size(130, 541);
            this.tableLayoutPanel18.TabIndex = 0;
            // 
            // btn_ConveyorReset
            // 
            this.btn_ConveyorReset.BackColor = System.Drawing.Color.White;
            this.btn_ConveyorReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_ConveyorReset.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_ConveyorReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ConveyorReset.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_ConveyorReset.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_ConveyorReset.Location = new System.Drawing.Point(1, 109);
            this.btn_ConveyorReset.Margin = new System.Windows.Forms.Padding(1);
            this.btn_ConveyorReset.Name = "btn_ConveyorReset";
            this.btn_ConveyorReset.Size = new System.Drawing.Size(128, 106);
            this.btn_ConveyorReset.TabIndex = 15;
            this.btn_ConveyorReset.Text = "Conveyor Reset";
            this.btn_ConveyorReset.UseVisualStyleBackColor = false;
            this.btn_ConveyorReset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_ConveyorReset_MouseDown);
            this.btn_ConveyorReset.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_ConveyorReset_MouseUp);
            // 
            // btn_ConveyorStop
            // 
            this.btn_ConveyorStop.BackColor = System.Drawing.Color.White;
            this.btn_ConveyorStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_ConveyorStop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_ConveyorStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ConveyorStop.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_ConveyorStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_ConveyorStop.Location = new System.Drawing.Point(1, 1);
            this.btn_ConveyorStop.Margin = new System.Windows.Forms.Padding(1);
            this.btn_ConveyorStop.Name = "btn_ConveyorStop";
            this.btn_ConveyorStop.Size = new System.Drawing.Size(128, 106);
            this.btn_ConveyorStop.TabIndex = 13;
            this.btn_ConveyorStop.Text = "Conveyor Stop";
            this.btn_ConveyorStop.UseVisualStyleBackColor = false;
            this.btn_ConveyorStop.Click += new System.EventHandler(this.btn_ConveyorStop_Click);
            // 
            // btn_CenteringStop
            // 
            this.btn_CenteringStop.BackColor = System.Drawing.Color.White;
            this.btn_CenteringStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_CenteringStop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_CenteringStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CenteringStop.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_CenteringStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_CenteringStop.Location = new System.Drawing.Point(1, 325);
            this.btn_CenteringStop.Margin = new System.Windows.Forms.Padding(1);
            this.btn_CenteringStop.Name = "btn_CenteringStop";
            this.btn_CenteringStop.Size = new System.Drawing.Size(128, 106);
            this.btn_CenteringStop.TabIndex = 12;
            this.btn_CenteringStop.Text = "Centering Stop";
            this.btn_CenteringStop.UseVisualStyleBackColor = false;
            this.btn_CenteringStop.Click += new System.EventHandler(this.btn_CenteringStop_Click);
            // 
            // btn_StopperStop
            // 
            this.btn_StopperStop.BackColor = System.Drawing.Color.White;
            this.btn_StopperStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_StopperStop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_StopperStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_StopperStop.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_StopperStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_StopperStop.Location = new System.Drawing.Point(1, 217);
            this.btn_StopperStop.Margin = new System.Windows.Forms.Padding(1);
            this.btn_StopperStop.Name = "btn_StopperStop";
            this.btn_StopperStop.Size = new System.Drawing.Size(128, 106);
            this.btn_StopperStop.TabIndex = 11;
            this.btn_StopperStop.Text = "Stopper Stop";
            this.btn_StopperStop.UseVisualStyleBackColor = false;
            this.btn_StopperStop.Click += new System.EventHandler(this.btn_StopperStop_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox_Flag, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(672, 563);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox_ConveyorCommand);
            this.panel3.Controls.Add(this.groupBox_CylinderCommand);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(137, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(532, 557);
            this.panel3.TabIndex = 12;
            // 
            // Frm_ConveyorMotion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(672, 563);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_ConveyorMotion";
            this.Text = "Frm_InverterMotion";
            this.groupBox_ConveyorCommand.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox_SyncMove.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.groupBox_Move.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.groupBox_CylinderCommand.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox_Centering.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox_Stopper.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox_Flag.ResumeLayout(false);
            this.tableLayoutPanel18.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer UIUpdateTimer;
        private System.Windows.Forms.GroupBox groupBox_ConveyorCommand;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox_SyncMove;
        private System.Windows.Forms.Button btn_SyncMoveBWD;
        private System.Windows.Forms.Button btn_SyncMoveFWD;
        private System.Windows.Forms.GroupBox groupBox_Move;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private System.Windows.Forms.Label lbl_HighSpeed_BWD;
        private System.Windows.Forms.Button btn_HighSpeed_FWD;
        private System.Windows.Forms.Button btn_LowSpeed_BWD;
        private System.Windows.Forms.Button btn_HighSpeed_BWD;
        private System.Windows.Forms.Button btn_LowSpeed_FWD;
        private System.Windows.Forms.Label lbl_LowSpeed_FWD;
        private System.Windows.Forms.Label lbl_HighSpeed_FWD;
        private System.Windows.Forms.Label lbl_LowSpeed_BWD;
        private System.Windows.Forms.Button btn_Buffer_LP;
        private System.Windows.Forms.Button btn_Buffer_OP;
        private System.Windows.Forms.Button btn_Buffer_BP1;
        private System.Windows.Forms.Button btn_Buffer_BP2;
        private System.Windows.Forms.Label lbl_SyncBWD;
        private System.Windows.Forms.Label lbl_SyncFWD;
        private System.Windows.Forms.GroupBox groupBox_CylinderCommand;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox_Centering;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btn_Centering_BWD;
        private System.Windows.Forms.Button btn_Centering_FWD;
        private System.Windows.Forms.Label lbl_CenteringFWD;
        private System.Windows.Forms.Label lbl_CenteringBWD;
        private System.Windows.Forms.GroupBox groupBox_Stopper;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_Stopper_BWD;
        private System.Windows.Forms.Button btn_Stopper_FWD;
        private System.Windows.Forms.Label lbl_StopperFWD;
        private System.Windows.Forms.Label lbl_StopperBWD;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btn_Buffer_BP3;
        private System.Windows.Forms.Button btn_Buffer_BP4;
        private System.Windows.Forms.GroupBox groupBox_Flag;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel18;
        private System.Windows.Forms.Button btn_ConveyorStop;
        private System.Windows.Forms.Button btn_CenteringStop;
        private System.Windows.Forms.Button btn_StopperStop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btn_ConveyorReset;
    }
}