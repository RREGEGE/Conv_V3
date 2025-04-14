using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synustech.ucPanel.BcSetting
{
    public partial class UserTopComm_Setting : UserControl
    {
        /// <summary>
        /// 현재 UserControl Size를 담을 변수.
        /// </summary>
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_btnStep;
        private Size initialSize_btnStep;
        private Point initialLocation_btnStep;

        private float initialFontSize_btnCnv;
        private Size initialSize_btnCnv;
        private Point initialLocation_btnCnv;

        private float initialFontSize_btnLine;
        private Size initialSize_btnLine;
        private Point initialLocation_btnLine;
        
        private float initialFontSize_btnTime;
        private Size initialSize_btnTime;
        private Point initialLocation_btnTime;

        private bool isResizing = false;
        private bool btn_StepClick = false;
        private bool btn_CnvClick = false;
        private bool btn_LineClick = false;
        private bool btn_TimeClick = false;

        public delegate void delMonitorStep();
        public delegate void delMonitorCnv();
        public delegate void delMonitorLine();
        public delegate void delMonitorTime();

        public delMonitorStep PanelStep_Setting;
        public delMonitorCnv PanelCnv;
        public delMonitorLine PanelLine;
        public delMonitorTime PanelTime;
        public UserTopComm_Setting()
        {
            InitializeComponent();
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_btnStep = btnPrameter.Font.Size;
            initialSize_btnStep = btnPrameter.Size;
            initialLocation_btnStep = btnPrameter.Location;

            initialFontSize_btnCnv = btnCnv.Font.Size;
            initialSize_btnCnv = btnCnv.Size;
            initialLocation_btnCnv = btnCnv.Location;

            initialFontSize_btnLine = btnLine.Font.Size;
            initialSize_btnLine = btnLine.Size;
            initialLocation_btnLine = btnLine.Location;

            initialFontSize_btnTime = btnTime.Font.Size;
            initialSize_btnTime = btnTime.Size;
            initialLocation_btnTime = btnTime.Location;

            btnPrameter.Click += btnStep_Click;
            btnCnv.Click += btnCnv_Click;
            btnTime.Click += btnTime_Click;
            btnLine.Click += btnLine_Click;

            this.Resize += Panel_Resize;
        }
        public void init()
        {
            btnPrameter.BackColor = Color.FromArgb(37, 41, 64);
            btn_StepClick = true;
        }
        public void ResetButtonStates()
        {
            Color BackColor = Color.FromArgb(24, 30, 54);

            btn_StepClick = false;
            btn_CnvClick = false;
            btn_LineClick = false;
            btn_TimeClick = false;
  
            // 원래 버튼 색상으로 초기화
            btnPrameter.BackColor = BackColor;
            btnCnv.BackColor = BackColor;
            btnLine.BackColor = BackColor;
            btnTime.BackColor = BackColor;
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
                AdjustFontSize(btnPrameter, initialFontSize_btnStep, ratio);
                AdjustFontSize(btnCnv, initialFontSize_btnCnv, ratio);
                AdjustFontSize(btnLine, initialFontSize_btnLine, ratio);
                AdjustFontSize(btnTime, initialFontSize_btnTime, ratio);

                // 라벨 크기 및 위치 조절
                AdjustLabel(btnPrameter, initialSize_btnStep, initialLocation_btnStep, widthRatio, heightRatio);
                AdjustLabel(btnCnv, initialSize_btnCnv, initialLocation_btnCnv, widthRatio, heightRatio);
                AdjustLabel(btnLine, initialSize_btnLine, initialLocation_btnLine, widthRatio, heightRatio);
                AdjustLabel(btnTime, initialSize_btnTime, initialLocation_btnTime, widthRatio, heightRatio);

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

        private void btnStep_Click(object sender, EventArgs e)
        {
            if (!btn_StepClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btnPrameter.BackColor = Color.FromArgb(37, 41, 64);
                PanelStep_Setting?.Invoke();
                btn_StepClick = true;
            }
        }

        private void btnCnv_Click(object sender, EventArgs e)
        {
            if (!btn_CnvClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btnCnv.BackColor = Color.FromArgb(37, 41, 64);
                PanelCnv?.Invoke();
                btn_CnvClick = true;
            }
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            if (!btn_LineClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btnLine.BackColor = Color.FromArgb(37, 41, 64);
                PanelLine?.Invoke();
                btn_LineClick = true;
            }
        }

        private void btnTime_Click(object sender, EventArgs e)
        {
            if (!btn_TimeClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btnTime.BackColor = Color.FromArgb(37, 41, 64);
                PanelTime?.Invoke();
                btn_TimeClick = true;
            }
        }
    }
}
