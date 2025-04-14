
namespace RackMaster.SUBFORM.SettingPage {
    partial class SettingPage_WMX {
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvWMXParam = new System.Windows.Forms.DataGridView();
            this.btnReloadWMXParam = new System.Windows.Forms.Button();
            this.btnSaveWMXParam = new System.Windows.Forms.Button();
            this.btnRefreshWMXParam = new System.Windows.Forms.Button();
            this.saveTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWMXParam)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvWMXParam);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1689, 584);
            this.panel1.TabIndex = 1;
            // 
            // dgvWMXParam
            // 
            this.dgvWMXParam.AllowUserToAddRows = false;
            this.dgvWMXParam.AllowUserToDeleteRows = false;
            this.dgvWMXParam.AllowUserToResizeColumns = false;
            this.dgvWMXParam.AllowUserToResizeRows = false;
            this.dgvWMXParam.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvWMXParam.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvWMXParam.BackgroundColor = System.Drawing.Color.White;
            this.dgvWMXParam.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWMXParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWMXParam.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvWMXParam.Location = new System.Drawing.Point(0, 0);
            this.dgvWMXParam.MultiSelect = false;
            this.dgvWMXParam.Name = "dgvWMXParam";
            this.dgvWMXParam.RowHeadersVisible = false;
            this.dgvWMXParam.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvWMXParam.RowTemplate.Height = 23;
            this.dgvWMXParam.Size = new System.Drawing.Size(1689, 584);
            this.dgvWMXParam.TabIndex = 6;
            // 
            // btnReloadWMXParam
            // 
            this.btnReloadWMXParam.BackColor = System.Drawing.Color.White;
            this.btnReloadWMXParam.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReloadWMXParam.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReloadWMXParam.Location = new System.Drawing.Point(245, 590);
            this.btnReloadWMXParam.Name = "btnReloadWMXParam";
            this.btnReloadWMXParam.Size = new System.Drawing.Size(227, 100);
            this.btnReloadWMXParam.TabIndex = 7;
            this.btnReloadWMXParam.Text = "Reload";
            this.btnReloadWMXParam.UseVisualStyleBackColor = false;
            // 
            // btnSaveWMXParam
            // 
            this.btnSaveWMXParam.BackColor = System.Drawing.Color.White;
            this.btnSaveWMXParam.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveWMXParam.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveWMXParam.Location = new System.Drawing.Point(478, 590);
            this.btnSaveWMXParam.Name = "btnSaveWMXParam";
            this.btnSaveWMXParam.Size = new System.Drawing.Size(227, 100);
            this.btnSaveWMXParam.TabIndex = 8;
            this.btnSaveWMXParam.Tag = "Save";
            this.btnSaveWMXParam.Text = "Save";
            this.btnSaveWMXParam.UseVisualStyleBackColor = false;
            // 
            // btnRefreshWMXParam
            // 
            this.btnRefreshWMXParam.BackColor = System.Drawing.Color.White;
            this.btnRefreshWMXParam.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRefreshWMXParam.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshWMXParam.Location = new System.Drawing.Point(12, 590);
            this.btnRefreshWMXParam.Name = "btnRefreshWMXParam";
            this.btnRefreshWMXParam.Size = new System.Drawing.Size(227, 100);
            this.btnRefreshWMXParam.TabIndex = 9;
            this.btnRefreshWMXParam.Text = "Refresh";
            this.btnRefreshWMXParam.UseVisualStyleBackColor = false;
            // 
            // saveTimer
            // 
            this.saveTimer.Tick += new System.EventHandler(this.saveTimer_Tick);
            // 
            // SettingPage_WMX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1689, 702);
            this.Controls.Add(this.btnRefreshWMXParam);
            this.Controls.Add(this.btnSaveWMXParam);
            this.Controls.Add(this.btnReloadWMXParam);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SettingPage_WMX";
            this.Text = "SettingPage_WMX";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWMXParam)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvWMXParam;
        private System.Windows.Forms.Button btnReloadWMXParam;
        private System.Windows.Forms.Button btnSaveWMXParam;
        private System.Windows.Forms.Button btnRefreshWMXParam;
        private System.Windows.Forms.Timer saveTimer;
    }
}