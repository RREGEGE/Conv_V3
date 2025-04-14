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
    public partial class UserConvTeaching : UserControl
    {
        Calculator calculator = new Calculator();
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lbl_ConvTeaching;
        private Size initialSize_____lbl_ConvTeaching;
        private Point initialLocation_lbl_ConvTeaching;

        private float initialFontSize_lbl_ConvID;
        private Size initialSize_____lbl_ConvID;
        private Point initialLocation_lbl_ConvID;

        private float initialFontSize_btn_CurrentPOS;
        private Size initialSize_____btn_CurrentPOS;
        private Point initialLocation_btn_CurrentPOS;

        private float initialFontSize_btn_POS1;
        private Size initialSize_____btn_POS1;
        private Point initialLocation_btn_POS1;

        private float initialFontSize_btn_POS_1;
        private Size initialSize_____btn_POS_1;
        private Point initialLocation_btn_POS_1;

        private float initialFontSize_btn_POS2;
        private Size initialSize_____btn_POS2;
        private Point initialLocation_btn_POS2;

        private float initialFontSize_btn_POS_2;
        private Size initialSize_____btn_POS_2;
        private Point initialLocation_btn_POS_2;

        private float initialFontSize_btn_POS3;
        private Size initialSize_____btn_POS3;
        private Point initialLocation_btn_POS3;

        private float initialFontSize_btn_POS_3;
        private Size initialSize_____btn_POS_3;
        private Point initialLocation_btn_POS_3;

        private float initialFontSize_btn_POS4;
        private Size initialSize_____btn_POS4;
        private Point initialLocation_btn_POS4;

        private float initialFontSize_btn_POS_4;
        private Size initialSize_____btn_POS_4;
        private Point initialLocation_btn_POS_4;

        private float initialFontSize_btn_Inching;
        private Size initialSize_____btn_Inching;
        private Point initialLocation_btn_Inching;

        private float initialFontSize_btn_InchingNum;
        private Size initialSize_____btn_InchingNum;
        private Point initialLocation_btn_InchingNum;

        private float initialFontSize_btn_Setting;
        private Size initialSize_____btn_Setting;
        private Point initialLocation_btn_Setting;

        private bool isResizing = false;
        public UserConvTeaching()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lbl_ConvTeaching = lblConvTeaching.Font.Size;
            initialSize_____lbl_ConvTeaching = lblConvTeaching.Size;
            initialLocation_lbl_ConvTeaching = lblConvTeaching.Location;

            initialFontSize_lbl_ConvID = lblConvID.Font.Size;
            initialSize_____lbl_ConvID = lblConvID.Size;
            initialLocation_lbl_ConvID = lblConvID.Location;

            initialFontSize_btn_CurrentPOS = btn_CurrentPos.Font.Size;
            initialSize_____btn_CurrentPOS = btn_CurrentPos.Size;
            initialLocation_btn_CurrentPOS = btn_CurrentPos.Location;

            initialFontSize_btn_POS1 = btn_POS1.Font.Size;
            initialSize_____btn_POS1 = btn_POS1.Size;
            initialLocation_btn_POS1 = btn_POS1.Location;

            initialFontSize_btn_POS_1 = btn_POS_1.Font.Size;
            initialSize_____btn_POS_1 = btn_POS_1.Size;
            initialLocation_btn_POS_1 = btn_POS_1.Location;

            initialFontSize_btn_POS2 = btn_POS2.Font.Size;
            initialSize_____btn_POS2 = btn_POS2.Size;
            initialLocation_btn_POS2 = btn_POS2.Location;

            initialFontSize_btn_POS_2 = btn_POS_2.Font.Size;
            initialSize_____btn_POS_2 = btn_POS_2.Size;
            initialLocation_btn_POS_2 = btn_POS_2.Location;

            initialFontSize_btn_POS3 = btn_POS3.Font.Size;
            initialSize_____btn_POS3 = btn_POS3.Size;
            initialLocation_btn_POS3 = btn_POS3.Location;

            initialFontSize_btn_POS_3 = btn_POS_3.Font.Size;
            initialSize_____btn_POS_3 = btn_POS_3.Size;
            initialLocation_btn_POS_3 = btn_POS_3.Location;

            initialFontSize_btn_POS4 = btn_POS4.Font.Size;
            initialSize_____btn_POS4 = btn_POS4.Size;
            initialLocation_btn_POS4 = btn_POS4.Location;

            initialFontSize_btn_POS_4 = btn_POS_4.Font.Size;
            initialSize_____btn_POS_4 = btn_POS_4.Size;
            initialLocation_btn_POS_4 = btn_POS_4.Location;

            initialFontSize_btn_Inching = btn_Inching.Font.Size;
            initialSize_____btn_Inching = btn_Inching.Size;
            initialLocation_btn_Inching = btn_Inching.Location;

            initialFontSize_btn_InchingNum = btn_InchingNum.Font.Size;
            initialSize_____btn_InchingNum = btn_InchingNum.Size;
            initialLocation_btn_InchingNum = btn_InchingNum.Location;

            initialFontSize_btn_Setting = btn_Setting.Font.Size;
            initialSize_____btn_Setting = btn_Setting.Size;
            initialLocation_btn_Setting = btn_Setting.Location;
            this.Resize += Panel_Resize;

            calculator.ValueSend_Teaching += ApplyInching;
        }
        private void ApplyInching(int value)
        {
            if (value > 360)
            {
                MessageBox.Show("360이하로 기입하세요.");
            }
            else
            {
                btn_InchingNum.Text = value.ToString() + "°";
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
                AdjustFontSize(lblConvTeaching, initialFontSize_lbl_ConvTeaching, ratio);
                AdjustFontSize(lblConvID, initialFontSize_lbl_ConvID, ratio);

                // 라벨 크기 및 위치 조절
                AdjustLabel(lblConvTeaching, initialSize_____lbl_ConvTeaching, initialLocation_lbl_ConvTeaching, widthRatio, heightRatio);
                AdjustLabel(lblConvID, initialSize_____lbl_ConvID, initialLocation_lbl_ConvID, widthRatio, heightRatio);

                AdjustButton(btn_POS1, initialFontSize_btn_POS1, initialLocation_btn_POS1, initialSize_____btn_POS1, ratio);
                AdjustButton(btn_POS2, initialFontSize_btn_POS2, initialLocation_btn_POS2, initialSize_____btn_POS2, ratio);
                AdjustButton(btn_POS3, initialFontSize_btn_POS3, initialLocation_btn_POS3, initialSize_____btn_POS3, ratio);
                AdjustButton(btn_POS4, initialFontSize_btn_POS4, initialLocation_btn_POS4, initialSize_____btn_POS4, ratio);
                AdjustButton(btn_POS_1, initialFontSize_btn_POS_1, initialLocation_btn_POS_1, initialSize_____btn_POS_1, ratio);
                AdjustButton(btn_POS_2, initialFontSize_btn_POS_2, initialLocation_btn_POS_2, initialSize_____btn_POS_2, ratio);
                AdjustButton(btn_POS_3, initialFontSize_btn_POS_3, initialLocation_btn_POS_3, initialSize_____btn_POS_3, ratio);
                AdjustButton(btn_POS_4, initialFontSize_btn_POS_4, initialLocation_btn_POS_4, initialSize_____btn_POS_4, ratio);
                AdjustButton(btn_InchingNum, initialFontSize_btn_InchingNum, initialLocation_btn_InchingNum, initialSize_____btn_InchingNum, ratio);
                AdjustButton(btn_CurrentPos, initialFontSize_btn_CurrentPOS, initialLocation_btn_CurrentPOS, initialSize_____btn_CurrentPOS, ratio);
                AdjustButton(btn_Inching, initialFontSize_btn_Inching, initialLocation_btn_Inching, initialSize_____btn_Inching, ratio);
                AdjustButton(btn_Setting, initialFontSize_btn_Setting, initialLocation_btn_Setting, initialSize_____btn_Setting, ratio);
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

        private void btn_InchingNum_Click(object sender, EventArgs e)
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
