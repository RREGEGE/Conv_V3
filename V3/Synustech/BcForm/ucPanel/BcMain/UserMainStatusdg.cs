using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Synustech.G_Var;

namespace Synustech
{
    public partial class UserMainStatusdg : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_dgMotionView;
        private Size initialSize_____dgMotionView;
        private Point initialLocation_dgMotionView;

        private bool isResizing = false;
        public UserMainStatusdg()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_dgMotionView = dgMotionView.Font.Size;
            initialSize_____dgMotionView = dgMotionView.Size;
            initialLocation_dgMotionView = dgMotionView.Location;


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
                float widthRatio = (float)this.Width / initialPanelWidth;
                float heightRatio = (float)this.Height / initialPanelHeight;
                float ratio = Math.Min(widthRatio, heightRatio);

                // 폰트 사이즈 조절
                AdjustFontSize(dgMotionView, initialFontSize_dgMotionView, ratio);

                // 라벨 크기 및 위치 조절
                AdjustLabel(dgMotionView, initialSize_____dgMotionView, initialLocation_dgMotionView, widthRatio, heightRatio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }

        }

        private void AdjustFontSize(System.Windows.Forms.DataGridView dgview, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio) * 4;
            float maxFontSize = newFontSize * 4;
            newFontSize = Math.Max(10, Math.Min(maxFontSize, newFontSize));
            if (newFontSize > initialFontSize)
            {
                newFontSize = initialFontSize;
            }

            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                dgview.Font = new Font(dgview.Font.FontFamily, newFontSize, dgview.Font.Style);

            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }

        private void AdjustLabel(System.Windows.Forms.DataGridView dgview, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                dgview.Width = (int)(initialSize.Width * widthRatio);
                dgview.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                dgview.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        private void SetLineStatusDg()
        {
            int inScrollRowIndex = dgMotionView.FirstDisplayedScrollingRowIndex;
            dgMotionView.Rows.Clear();
            foreach (var line in lines)
            {
                line.StatusCheck();
                dgMotionView.Rows.Add(line.inoutmode,line.ID, line.ConvEA, line.CSTEA, line.AutoEA, line.IdleEA, line.ManualEA, line.ErrorEA, line.WarningEA);
            }
        }

        private void UI_Update_timer_Tick(object sender, EventArgs e)
        {
            SetLineStatusDg();
        }
    }
}
