using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synustech.BcForm
{
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
        }


        private void Loading_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool bEnd = false;

            if (progressBar1.Value < 100)
            {
                progressBar1.Value += 1;

                label3.Text = progressBar1.Value.ToString() + "%";
            }
            else if (progressBar1.Value >= 100)
            {
                bEnd = true;
                timer1.Stop();
                this.Close();
            }
        }
        public void UpdateProgress(int progress, string message)
        {
            progressBar1.Value = progress;
            label3.Text = message;
        }
    }
}
