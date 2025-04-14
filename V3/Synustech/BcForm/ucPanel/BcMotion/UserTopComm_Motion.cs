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



namespace Synustech
{
    public partial class UserTopComm_Motion : UserControl
    {
        /// <summary>
        /// 현재 UserControl Size를 담을 변수.
        /// </summary>
        private float initialPanelWidth;
        private float initialPanelHeight;

        /// <summary>
        /// Manual 버튼에 대한 Size를 담을 변수.
        /// </summary>
        private float initialFontSize_btnManual;
        private Size initialSize_btnManual;
        private Point initialLocation_btnManual;

        /// <summary>
        /// Manual 버튼에 대한 Size를 담을 변수.
        /// </summary>
        private float initialFontSize_btnTeaching;
        private Size initialSize_btnTeaching;
        private Point initialLocation_btnTeaching;

        /// <summary>
        /// Manual 버튼에 대한 Size를 담을 변수.
        /// </summary>
        private float initialFontSize_btnCycleTest;
        private Size initialSize_btnCycleTest;
        private Point initialLocation_btnCycleTest;

        private bool isResizing = false;
        private bool btn_ManualClick = false;
        private bool btn_TeachingClick = false;
        private bool btn_CycleTestClick = false;

        public delegate void delMonitorManual();
        public delegate void delMonitorTeaching();
        public delegate void delMonitorCycleTest();

        public delMonitorManual PanelManual;
        public delMonitorTeaching PanelTeaching;
        public delMonitorCycleTest PanelCycleTest;

        public UserTopComm_Motion()
        {
            InitializeComponent();
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_btnManual = btnManual.Font.Size;
            initialSize_btnManual = btnManual.Size;
            initialLocation_btnManual = btnManual.Location;

            initialFontSize_btnTeaching = btnTeaching.Font.Size;
            initialSize_btnTeaching = btnTeaching.Size;
            initialLocation_btnTeaching = btnTeaching.Location;

            initialFontSize_btnCycleTest = btnCycleTest.Font.Size;
            initialSize_btnCycleTest = btnCycleTest.Size;
            initialLocation_btnCycleTest = btnCycleTest.Location;

            btnManual.Click += btnManual_Click;
            btnTeaching.Click += btnTeaching_Click;
            btnCycleTest.Click += btnCycleTest_Click;

            this.Resize += Panel_Resize;

        }
        public void init()
        {
            btnManual.BackColor = Color.FromArgb(37, 41, 64);
            btn_ManualClick = true;
        }
        public void ResetButtonStates()
        {
            Color BackColor = Color.FromArgb(24, 30, 54);

            btn_ManualClick = false;
            btn_TeachingClick = false;
            btn_CycleTestClick = false;

            // 원래 버튼 색상으로 초기화
            btnManual.BackColor = BackColor;
            btnTeaching.BackColor = BackColor;
            btnCycleTest.BackColor = BackColor;
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
                AdjustFontSize(btnManual, initialFontSize_btnManual, ratio);
                AdjustFontSize(btnTeaching, initialFontSize_btnTeaching, ratio);
                AdjustFontSize(btnCycleTest, initialFontSize_btnCycleTest, ratio);

                // 라벨 크기 및 위치 조절
                AdjustLabel(btnManual, initialSize_btnManual, initialLocation_btnManual, widthRatio, heightRatio);
                AdjustLabel(btnTeaching, initialSize_btnTeaching, initialLocation_btnTeaching, widthRatio, heightRatio);
                AdjustLabel(btnCycleTest, initialSize_btnCycleTest, initialLocation_btnCycleTest, widthRatio, heightRatio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        private void AdjustFontSize(System.Windows.Forms.Button button, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = initialFontSize * ratio;
            float maxFontSize = newFontSize * 3;
            newFontSize = Math.Max(8, Math.Min(maxFontSize, newFontSize));
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

        private void AdjustFontSize(System.Windows.Forms.Label label, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = initialFontSize * ratio;
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

        private void AdjustLabel(System.Windows.Forms.Button button, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                button.Width = (int)(initialSize.Width * widthRatio);
                button.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                button.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            if (!btn_ManualClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btnManual.BackColor = Color.FromArgb(37, 41, 64);
                PanelManual?.Invoke();
                btn_ManualClick = true;
            }
        }

        private void btnTeaching_Click(object sender, EventArgs e)
        {
            if (!btn_TeachingClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btnTeaching.BackColor = Color.FromArgb(37, 41, 64);
                PanelTeaching?.Invoke();
                btn_TeachingClick = true;
            }
        }

        private void btnCycleTest_Click(object sender, EventArgs e)
        {
            if (!btn_CycleTestClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btnCycleTest.BackColor = Color.FromArgb(37, 41, 64);
                PanelCycleTest?.Invoke();
                btn_CycleTestClick = true;
            }
        }
    }
}
