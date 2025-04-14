using System;
using System.Drawing;
using System.Windows.Forms;
using static Synustech.G_Var;
namespace Synustech.ucPanel
{
    public partial class UserAutoCondition : UserControl
    {
     /// <summary>
     /// IntialSize를 위한 변수
     /// </summary>
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


     /// <summary>
     /// 중복처리를 위한 방지 플래그
     /// </summary>
        private bool isResizing = false;
        public UserAutoCondition()
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

     /// <summary>
     /// 패널 크기 변경을 위한 이벤트 연결.
     /// </summary>
            pnlMain.Resize += Panel_Resize;
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

        public void UpdateAutoEnable()
        {
            if (isAutoEnable)
            {
                lblSub.Text = "Enable";
                lblSub.ForeColor = Color.FromArgb(50, 226, 178);
            }
            else
            {
                lblSub.Text = "Disable";
                lblSub.ForeColor = Color.Red;
            }
        }
    }
}
