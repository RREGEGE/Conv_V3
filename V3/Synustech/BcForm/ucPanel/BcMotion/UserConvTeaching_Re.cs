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
        private bool IsSelectLoad = false;
        private bool IsSelectUnLoad = false;
        public UserConvTeaching_Re()
        {
            InitializeComponent();
            calculator.ValueSend_Teaching += ApplyInching;
        }
        private void ApplyInching(int value)
        {
            btnDegree.Text = value.ToString();
        }
        private void btnTurnJogPos_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
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
                        //selectedconveyor.TurnJogPOS(degreeValue);
                        m_WMXMotion.RelativeMove(m_WMXMotion.m_AxisProfile[selectedconveyor.TurnAxis], degreeValue);
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
                        m_WMXMotion.RelativeMove(m_WMXMotion.m_AxisProfile[selectedconveyor.TurnAxis], degreeValue);
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
                    if (rect.borderline == true)
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
                    if (IsSelectLoad && !IsSelectUnLoad)
                    {
                        if (selectedconveyor.IsHomeDone)
                        {
                            CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[selectedconveyor.TurnAxis];
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
                                    selectedconveyor.LoadPos = cmAxis.ActualPos;
                                    selectedconveyor.LoadInterlock_Plus = selectedconveyor.LoadPos + 2000;
                                    selectedconveyor.LoadInterlock_Minus = selectedconveyor.LoadPos - 2000;
                                    selectedconveyor.LoadLocation = POS; // 설정 완료
                                }
                            }
                            Console.WriteLine("Teaching Loaction:" + selectedconveyor.LoadPos);
                            Console.WriteLine("Setting POS:" + selectedconveyor.LoadLocation);
                        }
                        else
                        {
                            MessageBox.Show("Please check Homing");
                        }
                    }
                    else if (!IsSelectLoad && IsSelectUnLoad)
                    {
                        if (selectedconveyor.IsHomeDone)
                        {
                            CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[selectedconveyor.TurnAxis];
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
                                    selectedconveyor.UnloadPos = cmAxis.ActualPos;
                                    selectedconveyor.UnloadInterlock_Plus = selectedconveyor.UnloadPos + 2000;
                                    selectedconveyor.UnloadInterlock_Minus = selectedconveyor.UnloadPos - 2000;
                                    selectedconveyor.UnloadLocation = POS; // 설정 완료
                                }
                            }
                            Console.WriteLine("Teaching Loaction:" + selectedconveyor.UnloadPos);
                            Console.WriteLine("Setting POS:" + selectedconveyor.UnloadLocation);
                        }
                        else
                        {
                            MessageBox.Show("Please check Homing");
                        }
                    }
                    else if (!IsSelectLoad && !IsSelectUnLoad)
                    {
                        MessageBox.Show("Please select Direction");
                    }
                    IsSelectLoad = false;
                    IsSelectUnLoad = false;
                    lblLoad.BackColor = Color.FromArgb(24, 30, 54);
                    lblUload.BackColor = Color.FromArgb(24, 30, 54);
                }
                else
                {
                    MessageBox.Show("Pleas select Turn conveyor");
                }
            }
            else
            {
                MessageBox.Show("Please select conveyor");
            }
            _xml.SaveConveyorToXML(ConvFullPath);
        }

        private void btnLdMove_Click(object sender, EventArgs e)
        {
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
                            m_WMXMotion.AbsoluteMove(m_WMXMotion.m_AxisProfile[selectedconveyor.TurnAxis], selectedconveyor.LoadPos);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please perform homing");
                    }
                }
            }
        }

        private void btnUldPos_Click(object sender, EventArgs e)
        {
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
                            m_WMXMotion.AbsoluteMove(m_WMXMotion.m_AxisProfile[selectedconveyor.TurnAxis], selectedconveyor.UnloadPos);
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
            IsSelectLoad = !IsSelectLoad;
            if (IsSelectLoad)
            {
                IsSelectUnLoad = false;
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
            IsSelectUnLoad = !IsSelectUnLoad;
            if (IsSelectUnLoad)
            {
                IsSelectLoad = false;
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
                    lblLoadPOS.Text = selectedconveyor.LoadPos.ToString();
                    lblUloadPOS.Text = selectedconveyor.UnloadPos.ToString();
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
                    CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[selectedconveyor.TurnAxis];
                    if ((selectedconveyor.LoadPos == cmAxis.ActualPos) && (selectedconveyor.POS[selectedconveyor.LoadLocation] == SensorOnOff.On))
                    {
                        btnLdMove.BackColor = Color.FromArgb(0, 0, 139);
                    }
                    else
                    {
                        btnLdMove.BackColor = Color.FromArgb(24, 30, 54);
                    }
                    if ((selectedconveyor.UnloadPos == cmAxis.ActualPos) && (selectedconveyor.POS[selectedconveyor.UnloadLocation] == SensorOnOff.On))
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
