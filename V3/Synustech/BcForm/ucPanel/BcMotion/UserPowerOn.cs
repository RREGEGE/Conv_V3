using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMX3ApiCLR;
using static Synustech.G_Var;

namespace Synustech.ucPanel.BcMotion
{
    public partial class UserPowerOn : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_Main;
        private Size initialSize_____Main;
        private Point initialLocation_Main;

        private float initialFontSize_Meddle;
        private Size initialSize_____Meddle;
        private Point initialLocation_Meddle;

        bool IsHomingRun = false;
        bool IsHomeDone = false;
        private bool isResizing = false;

        public UserPowerOn()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_Main = lbl_CV_ID.Font.Size;
            initialLocation_Main = lbl_CV_ID.Location;
            initialSize_____Main = lbl_CV_ID.Size;
       

            initialFontSize_Meddle = lbl_Cur_Pos_Out.Font.Size;
            initialLocation_Meddle = lbl_Cur_Pos_Out.Location;
            initialSize_____Meddle = lbl_Cur_Pos_Out.Size;


            this.Resize += Panel_Resize;
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
                float widthRatio = (float)this.Width / initialPanelWidth * 2;
                float heightRatio = (float)this.Height / initialPanelHeight * 2;
                float ratio = Math.Min(widthRatio, heightRatio);

                // 라벨 폰트 사이즈 조절
                AdjustFontSize(lbl_CV_ID, initialFontSize_Main, ratio);
                AdjustFontSize(lbl_CV_ID_Out, initialFontSize_Main, ratio);
                AdjustFontSize(lbl_Cur_Pos, initialFontSize_Main, ratio);
                AdjustFontSize(lbl_MotorPower, initialFontSize_Main, ratio);
                //AdjustFontSize(btn_PowerOn, initialFontSize_Main, ratio);
                //AdjustFontSize(btn_PowerOff, initialFontSize_Main, ratio);
                AdjustFontSize(lbl_Home, initialFontSize_Main, ratio);
                AdjustFontSize(lbl_Cur_Pos_Out, initialFontSize_Meddle, ratio);

                // 라벨 크기 및 위치 조절
                AdjustLabel(lbl_CV_ID, initialSize_____Main, initialLocation_Main, widthRatio, heightRatio);
                AdjustLabel(lbl_CV_ID_Out, initialSize_____Main, initialLocation_Main, widthRatio, heightRatio);
                AdjustLabel(lbl_Home, initialSize_____Main, initialLocation_Main, widthRatio, heightRatio);
                AdjustLabel(lbl_Cur_Pos, initialSize_____Main, initialLocation_Main, widthRatio, heightRatio);
                AdjustLabel(lbl_MotorPower, initialSize_____Main, initialLocation_Main, widthRatio, heightRatio);

                AdjustLabel(lbl_Cur_Pos_Out, initialSize_____Meddle, initialLocation_Meddle, widthRatio, heightRatio);

                AdjustButton(btn_PowerOn, initialFontSize_Main, initialLocation_Main, initialSize_____Main, ratio);
                AdjustButton(btn_PowerOff, initialFontSize_Main, initialLocation_Main, initialSize_____Main, ratio);
                AdjustButton(btn_Homing, initialFontSize_Main, initialLocation_Main, initialSize_____Main, ratio);




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
            //float maxFontSize = newFontSize * 3;
            //newFontSize = Math.Max(8, Math.Min(maxFontSize, newFontSize));
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

        private void btn_PowerOn_Click(object sender, EventArgs e)
        {
            foreach (var rect in rectangles)
            {
                if (rect.borderline == true)
                {
                    Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                    if (conv.type == "Turn")
                    {
                        m_WMXMotion.ServoOn(conv.Axis);
                        m_WMXMotion.ServoOn(conv.TurnAxis);
                    }
                    else
                    {
                        m_WMXMotion.ServoOn(conv.Axis);
                    }
                }
            }
        }

        private void btn_PowerOff_Click(object sender, EventArgs e)
        {
            foreach (var rect in rectangles)
            {
                if (rect.borderline == true)
                {
                    Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                    if (conv.type == "Turn")
                    {
                        m_WMXMotion.ServoOff(conv.Axis);
                        m_WMXMotion.ServoOff(conv.TurnAxis);
                    }
                    else
                    {
                        m_WMXMotion.ServoOff(conv.Axis);
                    }

                }
            }
        }

        private void btn_Homing_Click(object sender, EventArgs e)
        {
            Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
            if (selectedconveyor != null)
            {
                if(selectedconveyor.type == "Turn")
                {
                    m_WMXMotion.HomeStart(selectedconveyor.TurnAxis);
                }
                else
                {
                    MessageBox.Show("Please check only Turn conveyor");
                }
            }
        }
        private void Conveyor_ID_Update()
        {
            if (selectedConvID != null)
            {
                Conveyor conveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
                if (conveyor != null)
                {
                    lbl_CV_ID_Out.Text = conveyor.ID.ToString();
                }
                if (conveyor.IsHomeDone && !IsHomingRun)
                {
                    btn_Homing.Text = "Origin OK";
                    btn_Homing.ForeColor = Color.FromArgb(0, 126, 249);
                }
                else if (!conveyor.IsHomeDone && !IsHomingRun)
                {
                    btn_Homing.Text = "Origin NG";
                    btn_Homing.ForeColor = Color.Red;
                }
                else if (IsHomingRun)
                {
                    btn_Homing.Text = "Homing";
                    btn_Homing.ForeColor = Color.Lime;
                }
            }
        }
        private void Current_POS_Update()
        {
            if (selectedConvID != null)
            {
                Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
                double dCurPos = 0;
                if (selectedconveyor != null)
                {
                    if (selectedconveyor.type == "Turn")
                    {
                        CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[selectedconveyor.TurnAxis];
                        dCurPos = cmAxis.ActualPos;
                        lbl_Cur_Pos_Out.Text = dCurPos.ToString("F2");
                    }
                    else
                    {
                        lbl_Cur_Pos_Out.Text = "-";
                    }
                }
            }
        }
        private void CNV_POS_Update_Timer_Tick(object sender, EventArgs e)
        {
            Current_POS_Update();
            Conveyor_ID_Update();
        }

        private async void btn_Homing_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                IsHomingRun = true;
                Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
                if (selectedconveyor != null)
                {
                    if (selectedconveyor.type == "Turn")
                    {
                        m_WMXMotion.HomeStart(selectedconveyor.TurnAxis);
                        Console.WriteLine(m_WMXMotion.IsServoRun(selectedconveyor.TurnAxis));

                        await Task.Run(() =>
                        {
                            while (IsHomingRun)
                            {
                                CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[selectedconveyor.TurnAxis];

                                if (!m_WMXMotion.IsServoRun(selectedconveyor.TurnAxis) &&
                                    cmAxis.ActualPos == 0 &&
                                    selectedconveyor.POS[0] == SensorOnOff.On)
                                {
                                    // UI 스레드에서 실행될 수 있도록 Invoke 호출
                                    this.Invoke((Action)(() =>
                                    {
                                        MessageBox.Show("The homing operation is complete.");
                                    }));

                                    selectedconveyor.IsHomeDone = true;
                                    IsHomingRun = false; // 루프 종료
                                }
                                Thread.Sleep(10); // 상태 체크 간격
                            }
                        });
                    }
                    else
                    {
                        MessageBox.Show("Please check only Turn conveyor");
                    }
                }
            }
        }

        private void btn_Homing_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                IsHomingRun = false;
                Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
                if (selectedconveyor != null)
                {
                    if (selectedconveyor.type == "Turn")
                    {
                        m_WMXMotion.StopJog(selectedconveyor.TurnAxis);
                        CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[selectedconveyor.TurnAxis];
                        if (cmAxis.ActualPos != 0 || selectedconveyor.POS[0] == SensorOnOff.Off)
                        {
                            MessageBox.Show("The homing operation was not completed.");
                        }
                    }
                }
            }
        }
    }
}
