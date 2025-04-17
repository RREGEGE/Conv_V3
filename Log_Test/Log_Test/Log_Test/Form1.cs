﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_Test
{
    public partial class Form1 : Form
    {
        DataGridView LogDGV;
        public Form1()
        {
            InitializeComponent();
            LogDGVInit();
        }

        private void btnLog1_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"MainForm-Network Settings Click");
        }
        private void LogDGVInit()
        {
            LogDGV = Log.CreateLogGridView();
            FormFunc.SetDoubleBuffer(LogDGV);
            Log.LogDGVReload(LogDGV);
            Log.logInsertEvent += UpdateLogDGV;

            panel1.Controls.Add(LogDGV); 
        }
        private void UpdateLogDGV(LogMsg _LogMsg)
        {
            if (!this.IsDisposed)
                Log.InsertLogDGV(LogDGV, _LogMsg);
        }
    }
}
