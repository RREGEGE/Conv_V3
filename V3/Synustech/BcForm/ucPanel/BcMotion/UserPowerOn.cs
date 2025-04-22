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
            
        }

        private void btn_PowerOff_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_Homing_Click(object sender, EventArgs e)
        {
            
        }
        private void Conveyor_ID_Update()
        {
            
        }
        private void Current_POS_Update()
        {
            
        }
        private void CNV_POS_Update_Timer_Tick(object sender, EventArgs e)
        {
            Current_POS_Update();
            Conveyor_ID_Update();
        }

        private async void btn_Homing_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void btn_Homing_MouseUp(object sender, MouseEventArgs e)
        {
            
        }
    }
}
