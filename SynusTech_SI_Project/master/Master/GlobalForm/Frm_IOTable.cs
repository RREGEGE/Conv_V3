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
    public partial class Frm_IOTable : Form
    {
        MovenCore.WMXIO m_WMXIO = new MovenCore.WMXIO();
        public Frm_IOTable()
        {
            InitializeComponent();
            ControlItemInit();
            this.TopMost = true;
            this.VisibleChanged += (object sender, EventArgs e) =>
            {
                if (this.Visible)
                {
                    UIUpdateTimer.Enabled = true;
                }
                else
                {
                    UIUpdateTimer.Enabled = false;
                }
            };

            this.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;
            };

            this.Disposed += (object sender, EventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;

                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);

                m_WMXIO = null;
            };
        }
        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);

            for(int nCount = 0; nCount < 8000; nCount++)
            {
                DGV_InputInfo.Rows.Add($"{nCount}.0", $"{nCount}.1", $"{nCount}.2", $"{nCount}.3", $"{nCount}.4", $"{nCount}.5", $"{nCount}.6", $"{nCount}.7");
                DGV_OutputInfo.Rows.Add($"{nCount}.0", $"{nCount}.1", $"{nCount}.2", $"{nCount}.3", $"{nCount}.4", $"{nCount}.5", $"{nCount}.6", $"{nCount}.7");
            }
        }

        private void btn_AlwaysTop_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
        }

        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            UIUpdateTimer.Interval = Master.UIUpdateIntervalTime;

            byte[] OutputMap = m_WMXIO.GetOutputBytes(0, 8000);
            byte[] InputMap = m_WMXIO.GetInputBytes(0, 8000);
            for (int nCount = 0; nCount < 8000; nCount++)
            {
                byte Outputvalue = OutputMap[nCount];
                byte Inputvalue = InputMap[nCount];

                for (int nBit = 0; nBit < 8; nBit++)
                {
                    DGV_OutputInfo.Rows[nCount].Cells[nBit].Style.BackColor = Interface.Math.BitOperation.GetBit(Outputvalue, nBit) ? Color.Lime : Color.White;
                    DGV_InputInfo.Rows[nCount].Cells[nBit].Style.BackColor = Interface.Math.BitOperation.GetBit(Inputvalue, nBit) ? Color.Lime : Color.White;
                }
            }

            ButtonFunc.SetBackColor(btn_AlwaysTop, this.TopMost ? Color.Lime : Color.White);
        }

        private void DGV_OutputInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int StartAddr = e.RowIndex;
            int Bit = e.ColumnIndex;

            if (StartAddr < 0 || Bit < 0)
                return;

            m_WMXIO.SetOutputBit(StartAddr, Bit, !m_WMXIO.GetOutputBit(StartAddr, Bit));

            DGV_OutputInfo.CurrentCell = null;
        }
    }
}
