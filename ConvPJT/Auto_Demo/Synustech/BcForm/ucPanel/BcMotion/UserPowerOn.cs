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

        bool isHomingRun = false;
        bool isHomeDone = false;
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
                UIFunction.AdjustLabelFontSize(lbl_CV_ID, initialFontSize_Main, ratio);
                UIFunction.AdjustLabelFontSize(lbl_CV_ID_Out, initialFontSize_Main, ratio);
                UIFunction.AdjustLabelFontSize(lbl_Cur_Pos, initialFontSize_Main, ratio);
                UIFunction.AdjustLabelFontSize(lbl_MotorPower, initialFontSize_Main, ratio);
                //AdjustFontSize(btn_PowerOn, initialFontSize_Main, ratio);
                //AdjustFontSize(btn_PowerOff, initialFontSize_Main, ratio);
                UIFunction.AdjustLabelFontSize(lbl_Home, initialFontSize_Main, ratio);
                UIFunction.AdjustLabelFontSize(lbl_Cur_Pos_Out, initialFontSize_Meddle, ratio);

                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lbl_CV_ID, initialSize_____Main, initialLocation_Main, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lbl_CV_ID_Out, initialSize_____Main, initialLocation_Main, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lbl_Home, initialSize_____Main, initialLocation_Main, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lbl_Cur_Pos, initialSize_____Main, initialLocation_Main, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lbl_MotorPower, initialSize_____Main, initialLocation_Main, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lbl_Cur_Pos_Out, initialSize_____Meddle, initialLocation_Meddle, widthRatio, heightRatio);

                UIFunction.AdjustButton(btn_PowerOn, initialFontSize_Main, initialLocation_Main, initialSize_____Main, ratio);
                UIFunction.AdjustButton(btn_PowerOff, initialFontSize_Main, initialLocation_Main, initialSize_____Main, ratio);
                UIFunction.AdjustButton(btn_Homing, initialFontSize_Main, initialLocation_Main, initialSize_____Main, ratio);




            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
        private void btn_PowerOn_Click(object sender, EventArgs e)
        {
            if (isSafety)
            {
                foreach (var rect in rectangles)
                {
                    if (rect.borderLine == true)
                    {
                        Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                        if (conv.servo == ServoOnOff.Off)
                        {
                            if (conv.type == "Turn")
                            {
                                w_motion.ServoOn(conv.axis);
                                w_motion.ServoOn(conv.turnAxis);
                            }
                            else
                            {
                                w_motion.ServoOn(conv.axis);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please check Safety.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_PowerOff_Click(object sender, EventArgs e)
        {
            foreach (var rect in rectangles)
            {
                if (rect.borderLine == true)
                {
                    Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                    if (conv.type == "Turn")
                    {
                        w_motion.ServoOff(conv.axis);
                        w_motion.ServoOff(conv.turnAxis);
                    }
                    else
                    {
                        w_motion.ServoOff(conv.axis);
                    }

                }
            }
        }

        private void btn_Homing_Click(object sender, EventArgs e)
        {
            Conveyor selectedConveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
            if (selectedConveyor != null)
            {
                if(selectedConveyor.type == "Turn")
                {
                    w_motion.HomeStart(selectedConveyor.turnAxis);
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
                if (conveyor.IsHomeDone && !isHomingRun)
                {
                    btn_Homing.Text = "Origin OK";
                    btn_Homing.ForeColor = Color.FromArgb(0, 126, 249);
                }
                else if (!conveyor.IsHomeDone && !isHomingRun)
                {
                    btn_Homing.Text = "Origin NG";
                    btn_Homing.ForeColor = Color.Red;
                }
                else if (isHomingRun)
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
                        CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[selectedconveyor.turnAxis];
                        dCurPos = cmAxis.ActualPos;

                        lbl_Cur_Pos_Out.Text = UnitConverter.InvertumTodegree(dCurPos).ToString("F2") + "°";
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
                isHomingRun = true;
                Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
                if (selectedconveyor != null)
                {
                    if (selectedconveyor.type == "Turn")
                    {
                        w_motion.HomeStart(selectedconveyor.turnAxis);
                        Console.WriteLine(w_motion.IsServoRun(selectedconveyor.turnAxis));

                        await Task.Run(() =>
                        {
                            while (isHomingRun)
                            {
                                CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[selectedconveyor.turnAxis];

                                if (!w_motion.IsServoRun(selectedconveyor.turnAxis) &&
                                    cmAxis.ActualPos == 0 &&
                                    selectedconveyor.POS[0] == SensorOnOff.On)
                                {
                                    // UI 스레드에서 실행될 수 있도록 Invoke 호출
                                    this.Invoke((Action)(() =>
                                    {
                                        MessageBox.Show("The homing operation is complete.");
                                    }));

                                    selectedconveyor.IsHomeDone = true;
                                    isHomingRun = false; // 루프 종료
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
                isHomingRun = false;
                Conveyor selectedconveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);
                if (selectedconveyor != null)
                {
                    if (selectedconveyor.type == "Turn")
                    {
                        w_motion.StopJog(selectedconveyor.turnAxis);
                        CoreMotionAxisStatus cmAxis = WMX3.m_coreMotionStatus.AxesStatus[selectedconveyor.turnAxis];
                        if (cmAxis.ActualPos != 0 || selectedconveyor.POS[0] == SensorOnOff.Off)
                        {
                            MessageBox.Show("The homing operation was not completed.");
                        }
                    }
                }
            }
        }

        private void tbl_UserPower_Base_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
