using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Master.GlobalForm
{
    public partial class Frm_Lock : Form
    {
        public bool CloseEnable = false;
        public Frm_Lock()
        {
            InitializeComponent();

            this.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                if (!CloseEnable)
                {
                    //종료가 아닌 경우 최소화
                    e.Cancel = true;
                }
            };
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LabelFunc.SetText(lbl_WarningText, SynusLangPack.GetLanguage("GOTWaningText"));

            if (lbl_WarningText.BackColor == Color.Yellow)
            {
                lbl_WarningText.BackColor = Color.Red;
                pictureBox1.BackColor = Color.Red;
                tableLayoutPanel1.BackColor = Color.Red;
            }
            else if (lbl_WarningText.BackColor == Color.Red)
            {
                lbl_WarningText.BackColor = Color.White;
                pictureBox1.BackColor = Color.White;
                tableLayoutPanel1.BackColor = Color.White;
            }
            else
            {
                lbl_WarningText.BackColor = Color.Yellow;
                pictureBox1.BackColor = Color.Yellow;
                tableLayoutPanel1.BackColor = Color.Yellow;
            }
            
        }
    }
}
