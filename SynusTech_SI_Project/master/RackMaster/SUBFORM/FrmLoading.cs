using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RackMaster.SUBFORM {
    public partial class FrmLoading : Form {
        private int count = 0;
        LoadingType m_loadType;

        public FrmLoading(LoadingType type) {
            InitializeComponent();

            m_loadType = type;

            LoadingTimer.Start();
        }

        private void LoadingTimer_Tick(object sender, EventArgs e) {
            switch (m_loadType) {
                case LoadingType.StartLoading:
                    if (count == 0) {
                        lblLoading.Text = "Loading.";
                    }
                    else if (count == 1) {
                        lblLoading.Text = "Loading..";
                    }
                    else if (count == 2) {
                        lblLoading.Text = "Loading...";
                    }
                    else if (count == 3) {
                        lblLoading.Text = "Loading....";
                    }
                    else if (count == 4) {
                        lblLoading.Text = "Loading.....";
                        count = -1;
                    }
                    count++;
                    break;

                case LoadingType.AbsoluteAlarmClearLoading:
                    if (count == 0) {
                        lblLoading.Text = "Absolute Alarm Clear.";
                    }
                    else if (count == 1) {
                        lblLoading.Text = "Absolute Alarm Clear..";
                    }
                    else if (count == 2) {
                        lblLoading.Text = "Absolute Alarm Loading...";
                    }
                    else if (count == 3) {
                        lblLoading.Text = "Absolute Alarm Loading....";
                    }
                    else if (count == 4) {
                        lblLoading.Text = "Absolute Alarm Loading.....";
                        count = -1;
                    }
                    count++;
                    break;
            }
        }

        private void FrmLoading_FormClosing(object sender, FormClosingEventArgs e) {
            LoadingTimer.Stop();
        }
    }
}
