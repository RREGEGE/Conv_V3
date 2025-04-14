using Microsoft.Office.Interop.Excel;
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
    public partial class UserCVCreate : UserControl
    {
        public UserCVCreate()
        {
            InitializeComponent();
        }
        public void InitializeLines(List<Synustech.Line> lines)
        {
            bcCbLine.Items.Clear();
            if (lines != null)
            {
                foreach (var line in lines)
                {
                    bcCbLine.Items.Add(line.ID);
                }
            }
        }

        private void btnCvCreate_Click(object sender, EventArgs e)
        {
            int x, y, id, axis;
            string type = bcComboType.SelectedItem?.ToString();
            string line = bcCbLine.SelectedItem?.ToString();
            string Interface = bcCbInterface.SelectedItem?.ToString();
            Conveyor conveyor = null;
            CustomRectangle rectangle = null;
            if (!string.IsNullOrEmpty(Interface) && !string.IsNullOrEmpty(line) && !string.IsNullOrEmpty(type) && int.TryParse(tbMappingX.Text, out x) && int.TryParse(tbMappingY.Text, out y) 
                && int.TryParse(tbID.Text, out id) && int.TryParse(txtAxis.Text, out axis))
            {
                // ID 중복 여부 확인
                if (G_Var.rectangles.Any(rect => rect.ID == id))
                {
                    MessageBox.Show("This ID already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // 중복된 ID가 있을 경우 추가 작업을 중단
                }
                if (G_Var.conveyors.Any(conv => conv.axis == axis) || G_Var.conveyors.Any(conv => conv.turnAxis == axis + 1))
                {
                    MessageBox.Show("This axis is already assigned.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Synustech.Line selectedLine = G_Var.lines.FirstOrDefault(l => l.ID == line);
                if (type == "Normal")
                {
                    if(Interface == "NONE")
                    {
                        conveyor = new NormalConv(id, axis, false);
                    }
                    else
                    {
                        conveyor = new NormalConv(id, axis, true);
                    }
                    rectangle = new NormalRect(x, y, id);
                }
                else if(type == "Long_Vertical")
                {
                    if(Interface == "NONE")
                    {
                        conveyor = new LongConv(id, axis, false);
                    }
                    else
                    {
                        conveyor = new LongConv(id, axis, true);
                    }
                    rectangle = new LongRect_Vertical(x, y,id);
                }
                else if(type == "Long_Horizontal")
                {
                    if(Interface == "NONE")
                    {
                        conveyor = new LongConv(id, axis, false);
                    }
                    else
                    {
                        conveyor = new LongConv(id, axis, true);
                    }
                    rectangle = new LongRect_Horizontal(x, y, id);
                }
                else if(type == "Turn")
                {
                    if(Interface == "NONE")
                    {
                        conveyor = new TurnConv(id, axis, false);
                    }
                    else
                    {
                        conveyor = new TurnConv(id, axis, true);
                    }
                    conveyor.turnAxis = axis + 1;
                    rectangle = new TurnRect(x, y, id);
                }
                conveyor.lines.Add(selectedLine);
                conveyors.Add(conveyor);
                rectangles.Add(rectangle);
                selectedLine.conveyors.Add(conveyor);
                foreach(var conv in conveyors)
                {
                    Console.WriteLine(conv.ID);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            del_param?.Invoke();
        }

        private void btnCvSave_Click(object sender, EventArgs e)
        {
            xmlControl.SaveRectanglesToXML(rectFullPath);
            xmlControl.SaveConveyorToXML(convFullPath);
        }

        private void btnMappingX_Click(object sender, EventArgs e)
        {
            int x;

            // tbMappingX.Text가 숫자로 변환 가능한지 확인
            if (int.TryParse(tbMappingX.Text, out x))
            {
                x += 40; // 숫자에 40을 더함
                tbMappingX.Text = x.ToString(); // 결과를 다시 텍스트로 설정
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMappingY_Click(object sender, EventArgs e)
        {
            int y;

            // tbMappingX.Text가 숫자로 변환 가능한지 확인
            if (int.TryParse(tbMappingX.Text, out y))
            {
                y += 40; // 숫자에 40을 더함
                tbMappingX.Text = y.ToString(); // 결과를 다시 텍스트로 설정
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
