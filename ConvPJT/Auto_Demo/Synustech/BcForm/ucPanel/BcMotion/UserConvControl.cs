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
using static Synustech.G_Var;

namespace Synustech.ucPanel.BcMotion
{
    public partial class UserConvControl : UserControl
    {
        Calculator calculator = new Calculator();
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lbl_ConvControl;
        private Size initialSize_____lbl_ConvControl;
        private Point initialLocation_lbl_ConvControl;

        private float initialFontSize_btn_Degree;
        private Size initialSize_____btn_Degree;
        private Point initialLocation_btn_Degree;

        private float initialFontSize_btn_Forward;
        private Size initialSize_____btn_Forward;
        private Point initialLocation_btn_Forward;

        private float initialFontSize_btn_Backward;
        private Size initialSize_____btn_Backward;
        private Point initialLocation_btn_Backward;

        private float initialFontSize_btnTurnJogPos;
        private Size initialSize_____btnTurnJogPos;
        private Point initialLocation_btnTurnJogPos;

        private float initialFontSize_btnTurnJogNeg;
        private Size initialSize_____btnTurnJogNeg;
        private Point initialLocation_btnTurnJogNeg;

        private float initialFontSize_btn_Inching;
        private Size initialSize_____btn_Inching;
        private Point initialLocation_btn_Inching;

        private float initialFontSize_btn_POS_1;
        private Size initialSize_____btn_POS_1;
        private Point initialLocation_btn_POS_1;

        private float initialFontSize_btn_POS_2;
        private Size initialSize_____btn_POS_2;
        private Point initialLocation_btn_POS_2;

        private float initialFontSize_btn_POS_3;
        private Size initialSize_____btn_POS_3;
        private Point initialLocation_btn_POS_3;

        private float initialFontSize_btn_POS_4;
        private Size initialSize_____btn_POS_4;
        private Point initialLocation_btn_POS_4;

        private bool isResizing = false;

        public delegate void delJogPOS(int axis, double speed);
        public delegate void delJogNEG(int axis, double speed);
        public delegate void delJogStop(int axis);

        public delJogPOS Jogpos;
        public delJogNEG Jogneg;
        public delJogStop Jogstop;
        public double degreeValue;
        public UserConvControl()
        {
            InitializeComponent();
            
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lbl_ConvControl = lblConvControl.Font.Size;
            initialSize_____lbl_ConvControl = lblConvControl.Size;
            initialLocation_lbl_ConvControl = lblConvControl.Location;

            initialFontSize_btn_Degree = btnDegree.Font.Size;
            initialSize_____btn_Degree = btnDegree.Size;
            initialLocation_btn_Degree = btnDegree.Location;

            initialFontSize_btn_Forward = btn_Forward.Font.Size;
            initialSize_____btn_Forward = btn_Forward.Size;
            initialLocation_btn_Forward = btn_Forward.Location;

            initialFontSize_btn_Backward = btn_Backward.Font.Size;
            initialSize_____btn_Backward = btn_Backward.Size;
            initialLocation_btn_Backward = btn_Backward.Location;

            initialFontSize_btnTurnJogPos = btnTurnJogPos.Font.Size;
            initialSize_____btnTurnJogPos = btnTurnJogPos.Size;
            initialLocation_btnTurnJogPos = btnTurnJogPos.Location;

            initialFontSize_btnTurnJogNeg = btnTurnJogNeg.Font.Size;
            initialSize_____btnTurnJogNeg = btnTurnJogNeg.Size;
            initialLocation_btnTurnJogNeg = btnTurnJogNeg.Location;

            initialFontSize_btn_Inching = btn_Inching.Font.Size;
            initialSize_____btn_Inching = btn_Inching.Size;
            initialLocation_btn_Inching = btn_Inching.Location;

            initialFontSize_btn_POS_1 = btn_POS1.Font.Size;
            initialSize_____btn_POS_1 = btn_POS1.Size;
            initialLocation_btn_POS_1 = btn_POS1.Location;

            initialFontSize_btn_POS_2 = btn_POS2.Font.Size;
            initialSize_____btn_POS_2 = btn_POS2.Size;
            initialLocation_btn_POS_2 = btn_POS2.Location;

            initialFontSize_btn_POS_3 = btn_POS3.Font.Size;
            initialSize_____btn_POS_3 = btn_POS3.Size;
            initialLocation_btn_POS_3 = btn_POS3.Location;

            initialFontSize_btn_POS_4 = btn_POS4.Font.Size;
            initialSize_____btn_POS_4 = btn_POS4.Size;
            initialLocation_btn_POS_4 = btn_POS4.Location;

            this.Resize += Panel_Resize;

            calculator.ValueSend_Control += ApplyInching;
            btn_Forward.MouseDown += new MouseEventHandler(btn_Forward_MouseDown);
            btn_Forward.MouseUp += new MouseEventHandler(btn_Forward_MouseUp);
            btn_Backward.MouseDown += new MouseEventHandler(btn_Backward_MouseDown);
            btn_Backward.MouseUp += new MouseEventHandler(btn_Backward_MouseUp);
        }
        public void ApplyInching(double value)
        {
                btnDegree.Text = value.ToString(); 
        }
        private void Panel_Resize(object sender, EventArgs e)
        {
            // 이미 크기 조정 중이면 중복 처리 방지
            if (isResizing)
                return;
            try
            {
                isResizing = true;

                if (initialPanelWidth == 0 || initialPanelHeight == 0)
                {
                    return; // 초기 패널 크기가 0인 경우 아무 것도 하지 않음
                }

                // Panel 크기 비율 계산
                float widthRatio = (float)this.Width / initialPanelWidth;
                float heightRatio = (float)this.Height / initialPanelHeight;
                float ratio = Math.Min(widthRatio, heightRatio);

                // 라벨 폰트 사이즈 조절
                UIFunction.AdjustLabelFontSize(lblConvControl, initialFontSize_lbl_ConvControl, ratio);
                UIFunction.AdjustButtonFontSize(btn_Forward, initialFontSize_btn_Forward, ratio);
                UIFunction.AdjustButtonFontSize(btn_Backward, initialFontSize_btn_Backward, ratio);
                UIFunction.AdjustButtonFontSize(btnTurnJogPos, initialFontSize_btnTurnJogPos, ratio);
                UIFunction.AdjustButtonFontSize(btnTurnJogNeg, initialFontSize_btnTurnJogNeg, ratio);
                UIFunction.AdjustButtonFontSize(btn_Inching, initialFontSize_btn_Inching, ratio);
                UIFunction.AdjustButtonFontSize(btnDegree, initialFontSize_btn_Degree, ratio);
                UIFunction.AdjustButtonFontSize(btn_POS1, initialFontSize_btn_POS_1, ratio);
                UIFunction.AdjustButtonFontSize(btn_POS2, initialFontSize_btn_POS_2, ratio);
                UIFunction.AdjustButtonFontSize(btn_POS3, initialFontSize_btn_POS_3, ratio);
                UIFunction.AdjustButtonFontSize(btn_POS4, initialFontSize_btn_POS_4, ratio);

                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lblConvControl, initialSize_____lbl_ConvControl, initialLocation_lbl_ConvControl, widthRatio, heightRatio);
                UIFunction.AdjustButton(btn_Forward, initialFontSize_btn_Forward, initialLocation_btn_Forward, initialSize_____btn_Forward, ratio);
                UIFunction.AdjustButton(btn_Backward, initialFontSize_btn_Backward, initialLocation_btn_Backward, initialSize_____btn_Backward, ratio);
                UIFunction.AdjustButton(btnTurnJogPos, initialFontSize_btnTurnJogPos, initialLocation_btnTurnJogPos, initialSize_____btnTurnJogPos, ratio);
                UIFunction.AdjustButton(btnTurnJogNeg, initialFontSize_btnTurnJogNeg, initialLocation_btnTurnJogNeg, initialSize_____btnTurnJogNeg, ratio);
                UIFunction.AdjustButton(btn_Inching, initialFontSize_btn_Inching, initialLocation_btn_Inching, initialSize_____btn_Inching, ratio);
                UIFunction.AdjustButton(btnDegree, initialFontSize_btn_Degree, initialLocation_btn_Degree, initialSize_____btn_Degree, ratio);
                UIFunction.AdjustButton(btn_POS1, initialFontSize_btn_POS_1, initialLocation_btn_POS_1, initialSize_____btn_POS_1, ratio);
                UIFunction.AdjustButton(btn_POS2, initialFontSize_btn_POS_2, initialLocation_btn_POS_2, initialSize_____btn_POS_2, ratio);
                UIFunction.AdjustButton(btn_POS3, initialFontSize_btn_POS_3, initialLocation_btn_POS_3, initialSize_____btn_POS_3, ratio);
                UIFunction.AdjustButton(btn_POS4, initialFontSize_btn_POS_4, initialLocation_btn_POS_4, initialSize_____btn_POS_4, ratio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
        private void AdjustFontSize(System.Windows.Forms.Label label, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio);
            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                label.Font = new Font(label.Font.FontFamily, newFontSize, label.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        private void AdjustFontSize1(System.Windows.Forms.Button button, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio);
            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                button.Font = new Font(button.Font.FontFamily, newFontSize, button.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        private void AdjustLabel(System.Windows.Forms.Label label, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                label.Width = (int)(initialSize.Width * widthRatio);
                label.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                label.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        private void AdjustButton(Button btn, float initialFontSize, Point initialLocation, Size initialSize, float ratio)
        {
            btn.Font = new Font(btn.Font.FontFamily, initialFontSize * ratio, btn.Font.Style);
            btn.Width = (int)(initialSize.Width * ratio);
            btn.Height = (int)(initialSize.Height * ratio);
            btn.Location = new Point((int)(initialLocation.X * ratio), (int)(initialLocation.Y * ratio));
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
        private void btn_Forward_MouseDown(object sender, MouseEventArgs e)
        {
            //Conveyor selectedConveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID.Value);
            if (e.Button == MouseButtons.Left)
            {
                G_Var.bMouse = true;
                double degreeValue;  // 변환된 값 저장
                foreach(var rect in rectangles)
                {
                    if(rect.borderLine == true)
                    {
                        Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                        if (double.TryParse(btnDegree.Text, out degreeValue))
                        {
                            conv.JogPOS(degreeValue);
                        }
                        else
                        {
                            // 변환에 실패했을 경우 처리 (예: 오류 메시지 출력)
                            MessageBox.Show("유효한 숫자를 입력하세요.");
                        }
                    }
                }
            }
        }
        private void btn_Forward_MouseUp(object sender, MouseEventArgs e)
        {
            //Conveyor selectedConveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID.Value);
            if (e.Button == MouseButtons.Left)
            {
                G_Var.bMouse = false;
                foreach (var rect in rectangles)
                {
                    if (rect.borderLine == true)
                    {
                        Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                        conv.JogSTOP();
                    }
                }
            }
        }
        private void btn_Backward_MouseDown(object sender, MouseEventArgs e)
        {
            //Conveyor selectedConveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID.Value);
            if (e.Button == MouseButtons.Left)
            {
                G_Var.bMouse = true;
                double degreeValue;  // 변환된 값 저장
                foreach (var rect in rectangles)
                {
                    if (rect.borderLine == true)
                    {
                        Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                        if (double.TryParse(btnDegree.Text, out degreeValue))
                        {
                            conv.JogNEG(degreeValue);
                        }
                        else
                        {
                            // 변환에 실패했을 경우 처리 (예: 오류 메시지 출력)
                            MessageBox.Show("유효한 숫자를 입력하세요.");
                        }
                    }
                }
            }
        }
        private void btn_Backward_MouseUp(object sender, MouseEventArgs e)
        {
            //Conveyor selectedConveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID.Value);
            if (e.Button == MouseButtons.Left)
            {
                G_Var.bMouse = false;
                foreach (var rect in rectangles)
                {
                    if (rect.borderLine == true)
                    {
                        Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                        conv.JogSTOP();
                    }
                }
            }
        }

        private void btnTurnJogPos_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                G_Var.bMouse = true;
                double degreeValue;  // 변환된 값 저장
                foreach (var rect in rectangles)
                {
                    if (rect.borderLine == true)
                    {
                        Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                        if(conv.type != "Turn")
                        {
                            MessageBox.Show("Turn Conveyor만 선택해주세요.");
                            return;
                        }
                        if (double.TryParse(btnDegree.Text, out degreeValue))
                        {
                            conv.TurnJogPOS(degreeValue);
                        }
                        else
                        {
                            // 변환에 실패했을 경우 처리 (예: 오류 메시지 출력)
                            MessageBox.Show("유효한 숫자를 입력하세요.");
                        }
                    }
                }
            }
        }

        private void btnTurnJogPos_MouseUp(object sender, MouseEventArgs e)
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

        private void btnTurnJogNeg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                G_Var.bMouse = true;
                double degreeValue;  // 변환된 값 저장
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
                        if (double.TryParse(btnDegree.Text, out degreeValue))
                        {
                            conv.TurnJogNEG(degreeValue);
                        }
                        else
                        {
                            // 변환에 실패했을 경우 처리 (예: 오류 메시지 출력)
                            MessageBox.Show("유효한 숫자를 입력하세요.");
                        }
                    }
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
    }
}
