using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Synustech.G_Var;

namespace Synustech.BcForm.ucPanel.BcSetting
{
    public partial class LineCreate : UserControl
    {
        public LineCreate()
        {
            InitializeComponent();
        }

        private void btnLineSave_Click(object sender, EventArgs e)
        {
            xmlControl.SaveLineToXml(lineFullPath);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            int startID = 0;
            int endID = 0;
            int convEA = 0;
            string lineID = txtLineName.Text;
            Synustech.Line newLine = null;
            if(!string.IsNullOrEmpty(lineID) && int.TryParse(txtID.Text, out startID) && int.TryParse(txtEndId.Text, out endID) 
                && int.TryParse(txtLinkCvEa.Text, out convEA))
            {
                if(G_Var.lines.Any(line => line.ID == lineID))
                {
                    MessageBox.Show("This ID already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                newLine = new Synustech.Line(lineID);
                newLine.startConvID = startID;
                newLine.endConvID = endID;
                newLine.convEA = convEA;
                G_Var.lines.Add(newLine);
                del_lineAdd?.Invoke(G_Var.lines);
            }
        }
    }
}
