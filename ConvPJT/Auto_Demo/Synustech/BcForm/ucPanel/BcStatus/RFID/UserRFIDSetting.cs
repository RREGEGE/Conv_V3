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

namespace Synustech.ucPanel.BcStatus.RFID
{

    public partial class UserRFIDSetting : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblSetting;
        private Size initialSize_____lblSetting;
        private Point initialLocation_lblSetting;

        private bool isResizing = false;
        public UserRFIDSetting()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lblSetting = lblSetting.Font.Size;
            initialSize_____lblSetting = lblSetting.Size;
            initialLocation_lblSetting = lblSetting.Location;

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
                UIFunction.AdjustLabelFontSize(lblSetting, initialFontSize_lblSetting, ratio);


                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lblSetting, initialSize_____lblSetting, initialLocation_lblSetting, widthRatio, heightRatio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
    }
}
