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

namespace Synustech.ucPanel.BcMotion
{
    public partial class UserCycleTest : UserControl
    {
        Calculator calculator = new Calculator();
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lbl_CycleTest;
        private Size initialSize_____lbl_CycleTest;
        private Point initialLocation_lbl_CycleTest;

        private float initialFontSize_btn_Count;
        private Size initialSize_____btn_Count;
        private Point initialLocation_btn_Count;

        private float initialFontSize_btn_CountNum;
        private Size initialSize_____btn_CountNum;
        private Point initialLocation_btn_CountNum;

        private float initialFontSize_btn_Auto;
        private Size initialSize_____btn_Auto;
        private Point initialLocation_btn_Auto;

        private float initialFontSize_btn_Manual;
        private Size initialSize_____btn_Manual;
        private Point initialLocation_btn_Manual;

        private float initialFontSize_btn_Run;
        private Size initialSize_____btn_Run;
        private Point initialLocation_btn_Run;

        private float initialFontSize_btn_Stop;
        private Size initialSize_____btn_Stop;
        private Point initialLocation_btn_Stop;

        private bool isResizing = false;
        public UserCycleTest()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            //initialFontSize_lbl_CycleTest = lblCycleTest.Font.Size;
            //initialSize_____lbl_CycleTest = lblCycleTest.Size;
            //initialLocation_lbl_CycleTest = lblCycleTest.Location;

            initialFontSize_btn_CountNum = btn_CountNum.Font.Size;
            initialSize_____btn_CountNum = btn_CountNum.Size;
            initialLocation_btn_CountNum = btn_CountNum.Location;

            initialFontSize_btn_Auto = btn_Auto.Font.Size;
            initialSize_____btn_Auto = btn_Auto.Size;
            initialLocation_btn_Auto = btn_Auto.Location;

            initialFontSize_btn_Manual = btn_Manual.Font.Size;
            initialSize_____btn_Manual = btn_Manual.Size;
            initialLocation_btn_Manual = btn_Manual.Location;

            initialFontSize_btn_Run = btn_Run.Font.Size;
            initialSize_____btn_Run = btn_Run.Size;
            initialLocation_btn_Run = btn_Run.Location;

            initialFontSize_btn_Stop = btn_Stop.Font.Size;
            initialSize_____btn_Stop = btn_Stop.Size;
            initialLocation_btn_Stop = btn_Stop.Location;

            this.Resize += Panel_Resize;

            calculator.ValueSend_Cycle += ApplyInching;
        }
        private void ApplyInching(int value)
        {
            btn_CountNum.Text = value.ToString();
        }
        private void btnCount_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;

            // 버튼이 비활성화된 상태에서 글자 색상을 White로 유지
            TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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
                //AdjustFontSize(lblCycleTest, initialFontSize_lbl_CycleTest, ratio);

                // 라벨 크기 및 위치 조절
               // AdjustLabel(lblCycleTest, initialSize_____lbl_CycleTest, initialLocation_lbl_CycleTest, widthRatio, heightRatio);

                AdjustButton(btn_Auto, initialFontSize_btn_Auto, initialLocation_btn_Auto, initialSize_____btn_Auto, ratio);
                AdjustButton(btn_Manual, initialFontSize_btn_Manual, initialLocation_btn_Manual, initialSize_____btn_Manual, ratio);
           
                AdjustButton(btn_CountNum, initialFontSize_btn_CountNum, initialLocation_btn_CountNum, initialSize_____btn_CountNum, ratio);
                AdjustButton(btn_Run, initialFontSize_btn_Run, initialLocation_btn_Run, initialSize_____btn_Run, ratio);
                AdjustButton(btn_Stop, initialFontSize_btn_Stop, initialLocation_btn_Stop, initialSize_____btn_Stop, ratio);
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
            float maxFontSize = newFontSize * 3;
            newFontSize = Math.Max(8, Math.Min(maxFontSize, newFontSize));
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

        private void btn_CountNum_Click(object sender, EventArgs e)
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
