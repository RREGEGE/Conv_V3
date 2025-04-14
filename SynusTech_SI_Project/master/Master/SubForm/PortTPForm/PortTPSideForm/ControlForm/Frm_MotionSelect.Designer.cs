
namespace Master.SubForm.PortTPForm.PortTPSubForm.ControlForm
{
    partial class Frm_MotionSelect
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
            this.groupBox_PortControlCommand = new System.Windows.Forms.GroupBox();
            this.pnl_MotionControl = new System.Windows.Forms.Panel();
            this.groupBox_ControlAxisSelection = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel_ControlListInfo = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_PortControlCommand.SuspendLayout();
            this.groupBox_ControlAxisSelection.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // UIUpdateTimer
            // 
            this.UIUpdateTimer.Interval = 250;
            this.UIUpdateTimer.Tick += new System.EventHandler(this.UIUpdateTimer_Tick);
            // 
            // groupBox_PortControlCommand
            // 
            this.groupBox_PortControlCommand.Controls.Add(this.pnl_MotionControl);
            this.groupBox_PortControlCommand.Controls.Add(this.groupBox_ControlAxisSelection);
            this.groupBox_PortControlCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_PortControlCommand.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_PortControlCommand.Location = new System.Drawing.Point(3, 3);
            this.groupBox_PortControlCommand.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_PortControlCommand.Name = "groupBox_PortControlCommand";
            this.groupBox_PortControlCommand.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_PortControlCommand.Size = new System.Drawing.Size(666, 557);
            this.groupBox_PortControlCommand.TabIndex = 6;
            this.groupBox_PortControlCommand.TabStop = false;
            this.groupBox_PortControlCommand.Text = "Port Control";
            // 
            // pnl_MotionControl
            // 
            this.pnl_MotionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_MotionControl.Location = new System.Drawing.Point(1, 128);
            this.pnl_MotionControl.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_MotionControl.Name = "pnl_MotionControl";
            this.pnl_MotionControl.Size = new System.Drawing.Size(664, 428);
            this.pnl_MotionControl.TabIndex = 8;
            // 
            // groupBox_ControlAxisSelection
            // 
            this.groupBox_ControlAxisSelection.BackColor = System.Drawing.Color.AliceBlue;
            this.groupBox_ControlAxisSelection.Controls.Add(this.panel2);
            this.groupBox_ControlAxisSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_ControlAxisSelection.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_ControlAxisSelection.Location = new System.Drawing.Point(1, 23);
            this.groupBox_ControlAxisSelection.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox_ControlAxisSelection.Name = "groupBox_ControlAxisSelection";
            this.groupBox_ControlAxisSelection.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox_ControlAxisSelection.Size = new System.Drawing.Size(664, 105);
            this.groupBox_ControlAxisSelection.TabIndex = 7;
            this.groupBox_ControlAxisSelection.TabStop = false;
            this.groupBox_ControlAxisSelection.Text = "Axis";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel_ControlListInfo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(1, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(662, 85);
            this.panel2.TabIndex = 37;
            // 
            // tableLayoutPanel_ControlListInfo
            // 
            this.tableLayoutPanel_ControlListInfo.ColumnCount = 4;
            this.tableLayoutPanel_ControlListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_ControlListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_ControlListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_ControlListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_ControlListInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_ControlListInfo.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_ControlListInfo.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel_ControlListInfo.Name = "tableLayoutPanel_ControlListInfo";
            this.tableLayoutPanel_ControlListInfo.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel_ControlListInfo.RowCount = 3;
            this.tableLayoutPanel_ControlListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_ControlListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_ControlListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_ControlListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_ControlListInfo.Size = new System.Drawing.Size(662, 85);
            this.tableLayoutPanel_ControlListInfo.TabIndex = 1;
            // 
            // Frm_MotionSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(672, 563);
            this.Controls.Add(this.groupBox_PortControlCommand);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_MotionSelect";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Frm_ServoMotion";
            this.groupBox_PortControlCommand.ResumeLayout(false);
            this.groupBox_ControlAxisSelection.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer UIUpdateTimer;
        private System.Windows.Forms.GroupBox groupBox_PortControlCommand;
        private System.Windows.Forms.GroupBox groupBox_ControlAxisSelection;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnl_MotionControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_ControlListInfo;
    }
}