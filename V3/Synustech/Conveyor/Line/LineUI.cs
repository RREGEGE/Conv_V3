using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synustech
{
    partial class Line
    {
        public void Initialize_Watchdog_btn(Button btn)
        {
            btn.Text = m_LineParameter.ID.ToString();
            btn.BackColor = Color.FromArgb(24, 30, 54);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Serif", 9.75f, FontStyle.Bold);
            btn.AutoSize = true;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Dock = DockStyle.Fill;
            btn.TextAlign = ContentAlignment.MiddleCenter;
        }
        
    }
}
