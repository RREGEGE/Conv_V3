using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Synustech.G_Var;

namespace Synustech.ucPanel
{
    public partial class UserEQstatus : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblMain;
        private float initialFontSize_lblSub;
        private float initialFontSize_lblCheck_1;

        private Point initialLocation_lblMain;
        private Point initialLocation_lblSub;
        private Point initialLocation_lblCheck_1;

        private Size initialSize_lblMain;
        private Size initialSize_lblSub;
        private Size initialSize_lblCheck_1;


        // 중복 처리를 방지하기 위한 플래그
        private bool isResizing = false;

        public UserEQstatus()
        {
            InitializeComponent();
            initialPanelWidth = pnlMain.Width;
            initialPanelHeight = pnlMain.Height;
            initialFontSize_lblMain = lblMain.Font.Size;
            initialFontSize_lblSub = lblCheck_1.Font.Size;
            initialFontSize_lblCheck_1 = lblSub.Font.Size;

            initialSize_lblMain = lblMain.Size;
            initialSize_lblSub = lblCheck_1.Size;
            initialSize_lblCheck_1 = lblSub.Size;

            initialLocation_lblMain = lblMain.Location;
            initialLocation_lblSub = lblCheck_1.Location;
            initialLocation_lblCheck_1 = lblSub.Location;

            // 패널의 크기 변경 이벤트 핸들러 추가
            pnlMain.Resize += UserEqPanel_Resize;
        }

        private void UserEqPanel_Resize(object sender, EventArgs e)
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
                float widthRatio = (float)pnlMain.Width / initialPanelWidth;
                float heightRatio = (float)pnlMain.Height / initialPanelHeight;
                float ratio = Math.Min(widthRatio, heightRatio);

                // 라벨 폰트 사이즈 조절
                UIFunction.AdjustLabelFontSize(lblMain, initialFontSize_lblMain, ratio);
                UIFunction.AdjustLabelFontSize(lblCheck_1, initialFontSize_lblSub, ratio);
                UIFunction.AdjustLabelFontSize(lblSub, initialFontSize_lblCheck_1, ratio);

                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lblMain, initialSize_lblMain, initialLocation_lblMain, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lblCheck_1, initialSize_lblSub, initialLocation_lblSub, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lblSub, initialSize_lblCheck_1, initialLocation_lblCheck_1, widthRatio, heightRatio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
        public void CheckAuto()
        {
            if (isAutoRun)
            {
                lblSub.Text = "Auto Run";
                lblSub.ForeColor = Color.FromArgb(0, 126, 249);
            }
            else if (isCycleRun)
            {
                lblSub.Text = "Cycle Run";
                lblSub.ForeColor = Color.FromArgb(0, 126, 249);
            }
            else if (isAlarm)
            {
                lblSub.Text = "Error";
                lblSub.ForeColor = Color.Red;
            }
            else
            {
                lblSub.Text = "Manual";
                lblSub.ForeColor = Color.FromArgb(0, 126, 249);
            }
            
        }
    }
}
