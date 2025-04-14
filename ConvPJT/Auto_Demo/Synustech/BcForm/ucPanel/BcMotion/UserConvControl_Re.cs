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
using static Synustech.UnitConverter;
using static Synustech.G_Var;

namespace Synustech.ucPanel.BcMotion
{
    public partial class UserConvControl_Re : UserControl
    {
        Calculator calculator = new Calculator();
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lbl;
        private Size initialSize_____lbl;
        private Point initialLocation_lbl;

        private float initialFontSize_btn;
        private Size initialSize_____btn;
        private Point initialLocation_btn;

        private bool isResizing = false;

        public static event delMonitorLogView del_ELogSender_UserConvControl;

        public delegate void delJogPOS(int axis, double speed);
        public delegate void delJogNEG(int axis, double speed);
        public delegate void delJogStop(int axis);

        public delJogPOS del_Jogpos;
        public delJogNEG del_Jogneg;
        public delJogStop del_Jogstop;
        public double degreeValue;

        public UserConvControl_Re()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lbl = Text_JOG.Font.Size;
            initialSize_____lbl = Text_JOG.Size;
            initialLocation_lbl = Text_JOG.Location;

            initialFontSize_btn = btn_Txt_Speed.Font.Size;
            initialSize_____btn = btn_Txt_Speed.Size;
            initialLocation_btn = btn_Txt_Speed.Location;

            

            this.Resize += Panel_Resize;

            calculator.ValueSend_Control += ApplyInching;

            btn_Forward.MouseDown += new MouseEventHandler(btn_Forward_MouseDown);
            btn_Forward.MouseUp += new MouseEventHandler(btn_Forward_MouseUp);
            btn_Backward.MouseDown += new MouseEventHandler(btn_Backward_MouseDown);
            btn_Backward.MouseUp += new MouseEventHandler(btn_Backward_MouseUp);
        }
        private void ApplyInching(double value)
        {
            if (value > 80)
            {
                MessageBox.Show("Please set it to below 80");
                return;
            }
            else
            {
                btn_InPutVal.Text = value.ToString();
            }
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
                //AdjustFontSize(Text_JOG, initialFontSize_lbl, ratio);
                UIFunction.AdjustButtonFontSize(btn_Forward, initialFontSize_btn, ratio);
                UIFunction.AdjustButtonFontSize(btn_Backward, initialFontSize_btn, ratio);
                UIFunction.AdjustButtonFontSize(btnTurnJogPos, initialFontSize_btn, ratio);
                UIFunction.AdjustButtonFontSize(btnTurnJogNeg, initialFontSize_btn, ratio);

                // 라벨 크기 및 위치 조절
                //AdjustLabel(Text_JOG, initialFontSize_lbl, initialLocation_lbl, widthRatio, heightRatio);
                UIFunction.AdjustButton(btn_Forward, initialFontSize_btn, initialLocation_btn, initialSize_____btn, ratio);
                UIFunction.AdjustButton(btn_Backward, initialFontSize_btn, initialLocation_btn, initialSize_____btn, ratio);
                UIFunction.AdjustButton(btnTurnJogPos, initialFontSize_btn, initialLocation_btn, initialSize_____btn, ratio);
                UIFunction.AdjustButton(btnTurnJogNeg, initialFontSize_btn, initialLocation_btn, initialSize_____btn, ratio);
            

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        private void btn_Forward_MouseDown(object sender, MouseEventArgs e)
        {
            //Conveyor selectedConveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID.Value);

            if (e.Button == MouseButtons.Left)
            {
                Button btn = sender as Button;
                del_ELogSender_UserConvControl(enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"FWD Button Mouse Down");
                if (isAlarm)
                {
                    MessageBox.Show("Unable to operate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                G_Var.bMouse = true;
                double degreeValue;  // 변환된 값 저장
                foreach (var rect in rectangles)
                {
                    if (rect.borderLine == true)
                    {
                        Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                        if (double.TryParse(btn_InPutVal.Text, out degreeValue))
                        {
                            conv.JogPOS(InvertmmTospeed(degreeValue));
                            del_ELogSender_UserConvControl(enLogType.Application, enLogLevel.Normal, enLogTitle.JogPositiveStart, $"Conveyor ID: {conv.ID}");
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
            Button btn = sender as Button;
            if (e.Button == MouseButtons.Left)
            {
                G_Var.bMouse = false;
                foreach (var rect in rectangles)
                {
                    if (rect.borderLine == true)
                    {
                        Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                        conv.JogSTOP();
                        del_ELogSender_UserConvControl(enLogType.Application, enLogLevel.Normal, enLogTitle.JogStop, $"Conveyor ID: {conv.ID}");
                    }
                }
            }
        }
        private void btn_Backward_MouseDown(object sender, MouseEventArgs e)
        {
            //Conveyor selectedConveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID.Value);
            if (e.Button == MouseButtons.Left)
            {
                Button btn = sender as Button;
                del_ELogSender_UserConvControl(enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"BWD Button Mouse Down");
                if (isAlarm)
                {
                    MessageBox.Show("Unable to operate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                G_Var.bMouse = true;
                double degreeValue;  // 변환된 값 저장
                foreach (var rect in rectangles)
                {
                    if (rect.borderLine == true)
                    {
                        Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                        if (double.TryParse(btn_InPutVal.Text, out degreeValue))
                        {
                            conv.JogNEG(InvertmmTospeed(degreeValue));
                            del_ELogSender_UserConvControl(enLogType.Application, enLogLevel.Normal, enLogTitle.JogNegativeStart, $"Conveyor ID: {conv.ID}");
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
            Button btn = sender as Button;
            if (e.Button == MouseButtons.Left)
            {
                G_Var.bMouse = false;
                foreach (var rect in rectangles)
                {
                    if (rect.borderLine == true)
                    {
                        Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                        conv.JogSTOP();
                        del_ELogSender_UserConvControl(enLogType.Application, enLogLevel.Normal, enLogTitle.JogStop, $"Conveyor ID: {conv.ID}");
                    }
                }
            }
        }

        private void btnTurnJogPos_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                Button btn = sender as Button;
                del_ELogSender_UserConvControl(enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"TurnCW Button Mouse Down");
                if (isAlarm)
                {
                    MessageBox.Show("Unable to operate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
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
                        else if (conv.type == "Turn" && !conv.IsHomeDone)
                        {

                        }
                        if (double.TryParse(btn_InPutVal.Text, out degreeValue))
                        {
                            conv.TurnJogPos(InvertmmTospeed(degreeValue));
                            del_ELogSender_UserConvControl(enLogType.Application, enLogLevel.Normal, enLogTitle.TurnCWStart, $"Conveyor ID: {conv.ID}");
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
                        del_ELogSender_UserConvControl(enLogType.Application, enLogLevel.Normal, enLogTitle.TurnStop, $"Conveyor ID: {conv.ID}");
                    }
                }
            }
        }

        private void btnTurnJogNeg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                del_ELogSender_UserConvControl(enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"TurnNEG Button Mouse Down");
                if (isAlarm)
                {
                    MessageBox.Show("Unable to operate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
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
                        if (double.TryParse(btn_InPutVal.Text, out degreeValue))
                        {
                            conv.TurnJogNeg(InvertmmTospeed(degreeValue));
                            del_ELogSender_UserConvControl(enLogType.Application, enLogLevel.Normal, enLogTitle.TurnCCWStart, $"Conveyor ID: {conv.ID}");
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
                        del_ELogSender_UserConvControl(enLogType.Application, enLogLevel.Normal, enLogTitle.TurnStop, $"Conveyor ID: {conv.ID}");
                    }
                }
            }
        }

        private void btn_InPutVal_Click(object sender, EventArgs e)
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
