using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synustech.ucPanel.BcStatus.IO
{
    public partial class ConvIDSelect : UserControl
    {
        private List<int> Pages;
        private int currentPage;

        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_btnMaster;
        private Size initialSize_____btnMaster;
        private Point initialLocation_btnMaster;

        private float initialFontSize_btnConv;
        private Size initialSize_____btnConv;
        private Point initialLocation_btnConv;


        public delegate void delMasterUpdate();
        public delegate void delConvUpdate();

        public delMasterUpdate MasterUpdate;
        public delConvUpdate ConvUpdate;

        private bool isResizing = false;
        public ConvIDSelect()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_btnMaster = btnMaster.Font.Size;
            initialSize_____btnMaster = btnMaster.Size;
            initialLocation_btnMaster = btnMaster.Location;

            initialFontSize_btnConv = btnConv.Font.Size;
            initialSize_____btnConv = btnConv.Size;
            initialLocation_btnConv = btnConv.Location;

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

                // 라벨 폰트 사이즈 조절
                //AdjustFontSize1(btnConv, initialFontSize_btnConv, ratio);

                // 라벨 크기 및 위치 조절
                AdjustButton(btnMaster, initialFontSize_btnMaster, initialLocation_btnMaster, initialSize_____btnMaster, ratio);
                AdjustButton(btnConv, initialFontSize_btnConv, initialLocation_btnConv, initialSize_____btnMaster, ratio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
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
        private void btnMaster_Click(object sender, EventArgs e)
        {
            MasterUpdate?.Invoke();
        }

        private void btnConv_Click(object sender, EventArgs e)
        {
            ConvUpdate?.Invoke();
        }
    }
}
