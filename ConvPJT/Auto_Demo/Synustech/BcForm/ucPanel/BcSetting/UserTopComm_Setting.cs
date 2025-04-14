using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Synustech.G_Var;

namespace Synustech.ucPanel.BcSetting
{
    public partial class UserTopComm_Setting : UserControl
    {
        /// <summary>
        /// 현재 UserControl Size를 담을 변수.
        /// </summary>
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize;
        private Size initialSize;
        private Point initialLocation;

        private bool isResizing = false;
        private bool btn_WMXParaClick = false;
        private bool btn_OperationClick = false;
        private bool btn_TimeClick = false;
        private bool btn_CvCreateClick = false;
        private bool btn_InLineCreate = false;

        public delegate void delMonitorWMXPara();
        public delegate void delMonitorOperation();
        public delegate void delMonitorCnvCreate();
        public delegate void delMonitorTime();
        public delegate void delMonitorLine();

        public delMonitorWMXPara del_PanelWMXPara_Setting;
        public delMonitorOperation del_PanelOperation;
        public delMonitorTime del_PanelTime;
        public delMonitorCnvCreate del_PanelCnvCreate;
        public delMonitorLine del_PanelLine;

        public UserTopComm_Setting()
        {
            InitializeComponent();
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize = btnPrameter.Font.Size;
            initialSize = btnPrameter.Size;
            initialLocation = btnPrameter.Location;


            btnPrameter.Click += btnWMXPara_Click;
            btnOperation.Click += btnOperationClick;
            btnTime.Click += btnTime_Click;
            btnInCvCreate.Click += btnInCvCreate_Click;
            btnInLineCreate.Click += btnInLineCreate_Click;

            this.Resize += Panel_Resize;
        }
        public void init()
        {
            btnPrameter.BackColor = Color.FromArgb(37, 41, 64);
            btn_WMXParaClick = true;
        }
        public void ResetButtonStates()
        {
            Color BackColor = Color.FromArgb(24, 30, 54);

            btn_WMXParaClick = false;
            btn_OperationClick = false;
            btn_TimeClick = false;
            btn_CvCreateClick = false;
            btn_InLineCreate = false;


            // 원래 버튼 색상으로 초기화
            btnPrameter.BackColor = BackColor;
            btnOperation.BackColor = BackColor;
            btnTime.BackColor = BackColor;
            btnInCvCreate.BackColor = BackColor; 
            btnInLineCreate.BackColor = BackColor; 
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
                UIFunction.AdjustButtonFontSize(btnPrameter, initialFontSize, ratio);
                UIFunction.AdjustButtonFontSize(btnOperation, initialFontSize, ratio);
                UIFunction.AdjustButtonFontSize(btnTime, initialFontSize, ratio);
                UIFunction.AdjustButtonFontSize(btnInCvCreate, initialFontSize, ratio);
                UIFunction.AdjustButtonFontSize(btnInLineCreate, initialFontSize, ratio);

                // 라벨 크기 및 위치 조절
                UIFunction.AdjustButton(btnPrameter, initialFontSize, initialLocation, initialSize, ratio);
                UIFunction.AdjustButton(btnOperation, initialFontSize, initialLocation, initialSize, ratio);
                UIFunction.AdjustButton(btnTime, initialFontSize, initialLocation, initialSize, ratio);
                UIFunction.AdjustButton(btnInCvCreate, initialFontSize, initialLocation, initialSize, ratio);
                UIFunction.AdjustButton(btnInLineCreate, initialFontSize, initialLocation, initialSize, ratio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        private void btnWMXPara_Click(object sender, EventArgs e)
        {
            if (!btn_WMXParaClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btnPrameter.BackColor = Color.FromArgb(37, 41, 64);
                del_PanelWMXPara_Setting?.Invoke();
                btn_WMXParaClick = true;
            }
        }

        private void btnOperationClick(object sender, EventArgs e)
        {
            if (!btn_OperationClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btnOperation.BackColor = Color.FromArgb(37, 41, 64);
                del_PanelOperation?.Invoke();
                btn_OperationClick = true;
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
                del_PanelTime?.Invoke();
                btn_TimeClick = true;
            }
        }

        private void btnInCvCreate_Click(object sender, EventArgs e)
        {
            if (!btn_CvCreateClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btnInCvCreate.BackColor = Color.FromArgb(37, 41, 64);
                del_PanelCnvCreate?.Invoke();
                btn_CvCreateClick = true;
            }
        }

        private void btnInLineCreate_Click(object sender, EventArgs e)
        {
            if (!btn_InLineCreate)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btnInLineCreate.BackColor = Color.FromArgb(37, 41, 64);
                del_PanelLine?.Invoke();
                btn_InLineCreate = true;
            }
        }
    }
}
