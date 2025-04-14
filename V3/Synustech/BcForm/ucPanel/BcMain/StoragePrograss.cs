using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synustech.ucPanel
{
    public partial class StoragePrograss : UserControl
    {
        /// <summary>
        /// 24.11.03 에니메이션 작동을 위한 불변수 추가.
        /// </summary>
     

        /// <summary>
        /// 24.06.24 신규 정리
        /// </summary>
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float iniFontSize__lblStorageComent;
        private Size iniSize_______lblStorageComent;
        private Point iniLocation__lblStorageComent;

        private float iniFontSize__circularProgressBar1;
        private Size iniSize_______circularProgressBar1;
        private Point iniLocation__circularProgressBar1;

        private float iniFontSize__lblStorage;
        private Size iniSize_______lblStorage;
        private Point iniLocation__lblStorage;

        private bool isResizing = false;


        public StoragePrograss()
        {
            InitializeComponent();

            initialPanelWidth = panel3.Width;
            initialPanelHeight = panel3.Height;

            iniFontSize__lblStorageComent = lblStorageComent.Font.Size;
            iniSize_______lblStorageComent = lblStorageComent.Size;
            iniLocation__lblStorageComent = lblStorageComent.Location;

            iniFontSize__circularProgressBar1 = circularProgressBar1.Font.Size;
            iniSize_______circularProgressBar1 = circularProgressBar1.Size;
            iniLocation__circularProgressBar1 = circularProgressBar1.Location;

            iniFontSize__lblStorage = lblStorage.Font.Size;
            iniSize_______lblStorage = lblStorage.Size;
            iniLocation__lblStorage = lblStorage.Location;

            panel3.Resize += Panel_Resize;
        }
        private void StartAnimation()
        {
            // 애니메이션 시작을 위한 코드
            circularProgressBar1.Style = ProgressBarStyle.Marquee; 
        }

        private void StopAnimation()
        {
            // 애니메이션 정지를 위한 코드
            circularProgressBar1.Style = ProgressBarStyle.Blocks; ;
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
                float widthRatio = (float)panel3.Width / initialPanelWidth;
                float heightRatio = (float)panel3.Height / initialPanelHeight;
                float ratio = Math.Min(widthRatio, heightRatio);

                // 라벨 폰트 사이즈 조절
                AdjustFontSize(lblStorageComent, iniFontSize__lblStorageComent, ratio);
                AdjustFontSize(circularProgressBar1, iniFontSize__circularProgressBar1, ratio);
                AdjustFontSize(lblStorage, iniFontSize__lblStorage, ratio * 0.9f);

                // 라벨 크기 및 위치 조절
                AdjustLabel(lblStorageComent, iniSize_______lblStorageComent, iniLocation__lblStorageComent, widthRatio, heightRatio);
                AdjustLabel(circularProgressBar1, iniSize_______circularProgressBar1, iniLocation__circularProgressBar1, widthRatio, heightRatio);
                AdjustLabel(lblStorage, iniSize_______lblStorage, iniLocation__lblStorage, widthRatio, heightRatio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        private void AdjustFontSize(CircularProgressBar.CircularProgressBar cirBar, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = initialFontSize * ratio*0.95f;
            float maxFontSize = newFontSize * 3;
            newFontSize = Math.Max(10, Math.Min(maxFontSize, newFontSize));

            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                cirBar.Font = new Font(cirBar.Font.FontFamily, newFontSize, cirBar.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        private void AdjustFontSize(System.Windows.Forms.Label label, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = initialFontSize * ratio;
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
        private void AdjustLabel(CircularProgressBar.CircularProgressBar cirBar, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                cirBar.Width = (int)(initialSize.Width * widthRatio);
                cirBar.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                cirBar.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
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
    }
}