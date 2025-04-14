namespace Synustech
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Tb_Main_Dep_01 = new System.Windows.Forms.TableLayoutPanel();
            this.Tb_Main_Dep_02 = new System.Windows.Forms.TableLayoutPanel();
            this.Dep_03_Pn_MainMenu = new Synustech.UserMenu();
            this.SplitMain = new System.Windows.Forms.SplitContainer();
            this.Tb_Main_Dep_03_Monitor = new System.Windows.Forms.TableLayoutPanel();
            this.userAlarmHistory1 = new Synustech.ucPanel.UserAlarmHistory();
            this.SplitLog = new System.Windows.Forms.SplitContainer();
            this.userTop1 = new Synustech.UserTop();
            this.userTopComm_SubMenu = new Synustech.UserTopComm();
            this.UIUpdate_Timer = new System.Windows.Forms.Timer(this.components);
            this.Tb_Main_Dep_01.SuspendLayout();
            this.Tb_Main_Dep_02.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitMain)).BeginInit();
            this.SplitMain.Panel1.SuspendLayout();
            this.SplitMain.Panel2.SuspendLayout();
            this.SplitMain.SuspendLayout();
            this.Tb_Main_Dep_03_Monitor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitLog)).BeginInit();
            this.SplitLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tb_Main_Dep_01
            // 
            this.Tb_Main_Dep_01.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.Tb_Main_Dep_01.ColumnCount = 1;
            this.Tb_Main_Dep_01.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Tb_Main_Dep_01.Controls.Add(this.Tb_Main_Dep_02, 0, 2);
            this.Tb_Main_Dep_01.Controls.Add(this.userTop1, 0, 0);
            this.Tb_Main_Dep_01.Controls.Add(this.userTopComm_SubMenu, 0, 1);
            this.Tb_Main_Dep_01.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_Main_Dep_01.Location = new System.Drawing.Point(0, 0);
            this.Tb_Main_Dep_01.Margin = new System.Windows.Forms.Padding(0);
            this.Tb_Main_Dep_01.Name = "Tb_Main_Dep_01";
            this.Tb_Main_Dep_01.RowCount = 3;
            this.Tb_Main_Dep_01.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.816295F));
            this.Tb_Main_Dep_01.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.481482F));
            this.Tb_Main_Dep_01.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.77315F));
            this.Tb_Main_Dep_01.Size = new System.Drawing.Size(984, 561);
            this.Tb_Main_Dep_01.TabIndex = 0;
            // 
            // Tb_Main_Dep_02
            // 
            this.Tb_Main_Dep_02.ColumnCount = 2;
            this.Tb_Main_Dep_02.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.Tb_Main_Dep_02.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.Tb_Main_Dep_02.Controls.Add(this.Dep_03_Pn_MainMenu, 1, 0);
            this.Tb_Main_Dep_02.Controls.Add(this.SplitMain, 0, 0);
            this.Tb_Main_Dep_02.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_Main_Dep_02.Location = new System.Drawing.Point(0, 63);
            this.Tb_Main_Dep_02.Margin = new System.Windows.Forms.Padding(0);
            this.Tb_Main_Dep_02.Name = "Tb_Main_Dep_02";
            this.Tb_Main_Dep_02.RowCount = 1;
            this.Tb_Main_Dep_02.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Tb_Main_Dep_02.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 676F));
            this.Tb_Main_Dep_02.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 676F));
            this.Tb_Main_Dep_02.Size = new System.Drawing.Size(984, 498);
            this.Tb_Main_Dep_02.TabIndex = 0;
            // 
            // Dep_03_Pn_MainMenu
            // 
            this.Dep_03_Pn_MainMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.Dep_03_Pn_MainMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dep_03_Pn_MainMenu.Location = new System.Drawing.Point(885, 0);
            this.Dep_03_Pn_MainMenu.Margin = new System.Windows.Forms.Padding(0);
            this.Dep_03_Pn_MainMenu.Name = "Dep_03_Pn_MainMenu";
            this.Dep_03_Pn_MainMenu.Size = new System.Drawing.Size(99, 498);
            this.Dep_03_Pn_MainMenu.TabIndex = 1;
            // 
            // SplitMain
            // 
            this.SplitMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.SplitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitMain.Location = new System.Drawing.Point(0, 0);
            this.SplitMain.Margin = new System.Windows.Forms.Padding(0);
            this.SplitMain.Name = "SplitMain";
            this.SplitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitMain.Panel1
            // 
            this.SplitMain.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.SplitMain.Panel1.Controls.Add(this.Tb_Main_Dep_03_Monitor);
            // 
            // SplitMain.Panel2
            // 
            this.SplitMain.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.SplitMain.Panel2.Controls.Add(this.SplitLog);
            this.SplitMain.Size = new System.Drawing.Size(885, 498);
            this.SplitMain.SplitterDistance = 319;
            this.SplitMain.SplitterWidth = 1;
            this.SplitMain.TabIndex = 2;
            // 
            // Tb_Main_Dep_03_Monitor
            // 
            this.Tb_Main_Dep_03_Monitor.ColumnCount = 4;
            this.Tb_Main_Dep_03_Monitor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22221F));
            this.Tb_Main_Dep_03_Monitor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22223F));
            this.Tb_Main_Dep_03_Monitor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22223F));
            this.Tb_Main_Dep_03_Monitor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.Tb_Main_Dep_03_Monitor.Controls.Add(this.userAlarmHistory1, 3, 0);
            this.Tb_Main_Dep_03_Monitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_Main_Dep_03_Monitor.Location = new System.Drawing.Point(0, 0);
            this.Tb_Main_Dep_03_Monitor.Name = "Tb_Main_Dep_03_Monitor";
            this.Tb_Main_Dep_03_Monitor.RowCount = 2;
            this.Tb_Main_Dep_03_Monitor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.98574F));
            this.Tb_Main_Dep_03_Monitor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.01426F));
            this.Tb_Main_Dep_03_Monitor.Size = new System.Drawing.Size(885, 319);
            this.Tb_Main_Dep_03_Monitor.TabIndex = 0;
            // 
            // userAlarmHistory1
            // 
            this.userAlarmHistory1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userAlarmHistory1.Location = new System.Drawing.Point(590, 2);
            this.userAlarmHistory1.Margin = new System.Windows.Forms.Padding(2);
            this.userAlarmHistory1.Name = "userAlarmHistory1";
            this.Tb_Main_Dep_03_Monitor.SetRowSpan(this.userAlarmHistory1, 2);
            this.userAlarmHistory1.Size = new System.Drawing.Size(293, 315);
            this.userAlarmHistory1.TabIndex = 4;
            // 
            // SplitLog
            // 
            this.SplitLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitLog.Location = new System.Drawing.Point(0, 0);
            this.SplitLog.Margin = new System.Windows.Forms.Padding(0);
            this.SplitLog.Name = "SplitLog";
            this.SplitLog.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.SplitLog.Size = new System.Drawing.Size(885, 178);
            this.SplitLog.SplitterDistance = 94;
            this.SplitLog.SplitterWidth = 1;
            this.SplitLog.TabIndex = 0;
            // 
            // userTop1
            // 
            this.userTop1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.userTop1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userTop1.Location = new System.Drawing.Point(0, 0);
            this.userTop1.Margin = new System.Windows.Forms.Padding(0);
            this.userTop1.Name = "userTop1";
            this.userTop1.Size = new System.Drawing.Size(984, 27);
            this.userTop1.TabIndex = 2;
            // 
            // userTopComm_SubMenu
            // 
            this.userTopComm_SubMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.userTopComm_SubMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userTopComm_SubMenu.Location = new System.Drawing.Point(0, 27);
            this.userTopComm_SubMenu.Margin = new System.Windows.Forms.Padding(0);
            this.userTopComm_SubMenu.Name = "userTopComm_SubMenu";
            this.userTopComm_SubMenu.Size = new System.Drawing.Size(984, 36);
            this.userTopComm_SubMenu.TabIndex = 3;
            // 
            // UIUpdate_Timer
            // 
            this.UIUpdate_Timer.Enabled = true;
            this.UIUpdate_Timer.Interval = 300;
            this.UIUpdate_Timer.Tick += new System.EventHandler(this.UIUpdate_Timer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.Tb_Main_Dep_01);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Automation";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Tb_Main_Dep_01.ResumeLayout(false);
            this.Tb_Main_Dep_02.ResumeLayout(false);
            this.SplitMain.Panel1.ResumeLayout(false);
            this.SplitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitMain)).EndInit();
            this.SplitMain.ResumeLayout(false);
            this.Tb_Main_Dep_03_Monitor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitLog)).EndInit();
            this.SplitLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Tb_Main_Dep_01;
        private System.Windows.Forms.TableLayoutPanel Tb_Main_Dep_02;
        private UserTop userTop1;
        private System.Windows.Forms.SplitContainer SplitMain;
        private System.Windows.Forms.SplitContainer SplitLog;
        private ucPanel.UserAutoCondition userAutoCondition1;
        public System.Windows.Forms.TableLayoutPanel Tb_Main_Dep_03_Monitor;
        private UserMenu Dep_03_Pn_MainMenu;
        private UserTopComm userTopComm_SubMenu;
        private ucPanel.UserAlarmHistory userAlarmHistory1;
        private System.Windows.Forms.Timer UIUpdate_Timer;
    }
}

