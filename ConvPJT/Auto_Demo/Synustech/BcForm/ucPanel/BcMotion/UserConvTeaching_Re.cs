using Synustech.BcForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WMX3ApiCLR;
using static Synustech.G_Var;

namespace Synustech.ucPanel.BcMotion
{
    public partial class UserConvTeaching_Re : UserControl
    {
        Calculator calculator = new Calculator();
        private bool isSelectLoad = false;
        private bool isSelectUnLoad = false;
        private double teachingVelocity = 10000;
        public UserConvTeaching_Re()
        {
            InitializeComponent();
            calculator.ValueSend_Teaching += ApplyInching;
        }
        private void ApplyInching(double value)
        {
            if (value > 3.6)
            {
                MessageBox.Show("Please set it to below 3.6");
                return;
            }
            else
            {
                btnDegree.Text = value.ToString();
            }
        }
        private void btnTurnJogPos_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (isAlarm)
                {
                    MessageBox.Show("Unable to operate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                double degreeValue;
                Conveyor selectedConveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
                if (selectedConveyor != null)
                {
                    if (selectedConveyor.type != "Turn")
                    {
                        MessageBox.Show("Turn Conveyor만 선택해주세요.");
                        return;
                    }
                    if (double.TryParse(btnDegree.Text, out degreeValue))
                    {
                        w_motion.RelativeMove(w_motion.m_AxisProfile[selectedConveyor.turnAxis], UnitConverter.InvertdegreeToum(degreeValue), teachingVelocity);
                    }
                    else
                    {
                        // 변환에 실패했을 경우 처리 (예: 오류 메시지 출력)
                        MessageBox.Show("유효한 숫자를 입력하세요.");
                    }
                }
            }

        }

        private void btnTurnJogNeg_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (isAlarm)
                {
                    MessageBox.Show("Unable to operate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                double degreeValue;
                Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
                if (selectedconveyor != null)
                {
                    if (selectedconveyor.type != "Turn")
                    {
                        MessageBox.Show("Turn Conveyor만 선택해주세요.");
                        return;
                    }
                    if (double.TryParse(btnDegree.Text, out degreeValue))
                    {
                        degreeValue = -1 * degreeValue;
                        //selectedconveyor.TurnJogNEG(degreeValue);
                        w_motion.RelativeMove(w_motion.m_AxisProfile[selectedconveyor.turnAxis], UnitConverter.InvertdegreeToum(degreeValue), teachingVelocity);
                    }
                    else
                    {
                        // 변환에 실패했을 경우 처리 (예: 오류 메시지 출력)
                        MessageBox.Show("유효한 숫자를 입력하세요.");
                    }
                }
            }

        }

        private void btnTurnJogPos_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
                if (selectedconveyor != null)
                {
                    if (selectedconveyor.type != "Turn")
                    {
                        MessageBox.Show("Turn Conveyor만 선택해주세요.");
                        return;
                    }
                    selectedconveyor.TurnJogSTOP();
                }
            }
        }

        private void btnTurnJogNeg_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                G_Var.bMouse = false;
                foreach (var rect in rectangles)
                {
                    if (rect.borderLine == true)
                    {
                        Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                        if (conv.type != "Turn")
                        {
                            MessageBox.Show("Turn Conveyor만 선택해주세요.");
                            return;
                        }
                        conv.TurnJogSTOP();
                    }
                }
            }
        }

        private void btn_Setting_Click(object sender, EventArgs e)
        {
            Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
            if (selectedconveyor != null)
            {
                if (selectedconveyor.type == "Turn")
                {
                    if (isSelectLoad && !isSelectUnLoad)
                    {
                        if (selectedconveyor.IsHomeDone)
                        {
                            CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[selectedconveyor.turnAxis];
                            int POS = -1;
                            for (int i = 0; i < selectedconveyor.POS.Length; i++)
                            {
                                if (selectedconveyor.POS[i] == SensorOnOff.On)
                                {
                                    POS = i;
                                }
                            }
                            if (POS == -1)
                            {
                                MessageBox.Show("Please check POS_Sensor");
                                return;
                            }
                            else
                            {
                                string message = $"Would you like to set this?\n\nLoad POS: {cmAxis.ActualPos}\nUnload Index: POS{POS}";
                                DialogResult result = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.Yes)
                                {
                                    selectedconveyor.loadPOS = cmAxis.ActualPos;
                                    selectedconveyor.loadLocation = POS; // 설정 완료
                                }
                            }
                            Console.WriteLine("Teaching Loaction:" + selectedconveyor.loadPOS);
                            Console.WriteLine("Setting POS:" + selectedconveyor.loadLocation);
                        }
                        else
                        {
                            MessageBox.Show("Please check Homing");
                        }
                    }
                    else if (!isSelectLoad && isSelectUnLoad)
                    {
                        if (selectedconveyor.IsHomeDone)
                        {
                            CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[selectedconveyor.turnAxis];
                            int POS = -1;
                            for (int i = 0; i < selectedconveyor.POS.Length; i++)
                            {
                                if (selectedconveyor.POS[i] == SensorOnOff.On)
                                {
                                    POS = i;
                                }
                            }
                            if (POS == -1)
                            {
                                MessageBox.Show("Please check POS_Sensor");
                                return;
                            }
                            else
                            {
                                string message = $"Would you like to set this?\n\nUnload POS: {cmAxis.ActualPos}\nUnload Index: POS :{POS}";
                                DialogResult result = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.Yes)
                                {
                                    selectedconveyor.unloadPOS = cmAxis.ActualPos;
                                    selectedconveyor.unloadLocation = POS; // 설정 완료
                                }
                            }
                            Console.WriteLine("Teaching Loaction:" + selectedconveyor.unloadPOS);
                            Console.WriteLine("Setting POS:" + selectedconveyor.unloadLocation);
                        }
                        else
                        {
                            MessageBox.Show("Please check Homing");
                        }
                    }
                    else if (!isSelectLoad && !isSelectUnLoad)
                    {
                        MessageBox.Show("Please select Direction");
                    }
                    isSelectLoad = false;
                    isSelectUnLoad = false;
                    lblLoad.BackColor = Color.FromArgb(24, 30, 54);
                    lblUload.BackColor = Color.FromArgb(24, 30, 54);
                }
                else
                {
                    MessageBox.Show("Pleas select Turn conveyor");
                }
                xmlControl.SaveConveyorToXML(convFullPath);
            }
            else
            {
                MessageBox.Show("Please select conveyor");
            }
        }

        private void btnLdMove_Click(object sender, EventArgs e)
        {
            if (isAlarm)
            {
                MessageBox.Show("Unable to operate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
            if (selectedconveyor != null)
            {
                if (selectedconveyor.type == "Turn")
                {
                    if (selectedconveyor.IsHomeDone)
                    {
                        DialogResult result = MessageBox.Show("Would you like to move with Load POS?", "Move Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            w_motion.AbsoluteMove(w_motion.m_AxisProfile[selectedconveyor.turnAxis], selectedconveyor.loadPOS);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please perform homing");
                    }
                }
            }

        }

        private void btnUldMove_Click(object sender, EventArgs e)
        {
            if (isAlarm)
            {
                MessageBox.Show("Unable to operate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
            if (selectedconveyor != null)
            {
                if (selectedconveyor.type == "Turn")
                {
                    if (selectedconveyor.IsHomeDone)
                    {
                        DialogResult result = MessageBox.Show("Would you like to move with Unload POS?", "Move Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            w_motion.AbsoluteMove(w_motion.m_AxisProfile[selectedconveyor.turnAxis], selectedconveyor.unloadPOS);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please perform homing");
                    }
                }
            }

        }
        private void lblLoad_Click(object sender, EventArgs e)
        {
            isSelectLoad = !isSelectLoad;
            if (isSelectLoad)
            {
                isSelectUnLoad = false;
                lblLoad.BackColor = Color.FromArgb(0, 0, 139);
                lblUload.BackColor = Color.FromArgb(24, 30, 54);
            }
            else
            {
                lblLoad.BackColor = Color.FromArgb(24, 30, 54);
            }
        }
        private void lblUload_Click(object sender, EventArgs e)
        {
            isSelectUnLoad = !isSelectUnLoad;
            if (isSelectUnLoad)
            {
                isSelectLoad = false;
                lblUload.BackColor = Color.FromArgb(0, 0, 139);
                lblLoad.BackColor = Color.FromArgb(24, 30, 54);
            }
            else
            {
                lblUload.BackColor = Color.FromArgb(24, 30, 54);
            }
        }
        private void POSUpdate()
        {
            Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
            if (selectedconveyor != null)
            {
                if (selectedconveyor.type == "Turn")
                {
                    lblLoadPOS.Text = UnitConverter.InvertumTodegree(selectedconveyor.loadPOS).ToString("F2");
                    lblUloadPOS.Text = UnitConverter.InvertumTodegree(selectedconveyor.unloadPOS).ToString("F2");
                }
            }
        }
        private void POS_Done()
        {
            Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
            if (selectedconveyor != null)
            {
                if (selectedconveyor.type == "Turn")
                {
                    CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[selectedconveyor.turnAxis];
                    if ((selectedconveyor.loadPOS == cmAxis.ActualPos) && (selectedconveyor.POS[selectedconveyor.loadLocation] == SensorOnOff.On))
                    {
                        btnLdMove.BackColor = Color.FromArgb(0, 0, 139);
                    }
                    else
                    {
                        btnLdMove.BackColor = Color.FromArgb(24, 30, 54);
                    }
                    if ((selectedconveyor.unloadPOS == cmAxis.ActualPos) && (selectedconveyor.POS[selectedconveyor.unloadLocation] == SensorOnOff.On))
                    {
                        btnUldMove.BackColor = Color.FromArgb(0, 0, 139);
                    }
                    else
                    {
                        btnUldMove.BackColor = Color.FromArgb(24, 30, 54);
                    }
                }
            }
        }
        private void POS_Update_Timer_Tick(object sender, EventArgs e)
        {
            POSUpdate();
            POS_Done();
        }

        private void btnDegree_Click(object sender, EventArgs e)
        {
            // 마우스 커서 위치를 가져옴
            Point mousePosition = Cursor.Position;

            // 폼의 시작 위치를 수동으로 설정
            calculator.StartPosition = FormStartPosition.Manual;

            // 폼 크기를 얻기 위해 미리 보여주지 않고 레이아웃을 계산
            calculator.Load += (s, ev) =>
            {
                // 폼의 위치를 마우스 커서 위치에서 폼의 높이만큼 Y좌표를 빼서 설정
                calculator.Location = new Point(mousePosition.X, mousePosition.Y - calculator.Height);
            };

            calculator.ShowDialog();
        }
    }
}
