using Synustech.BcForm.ucPanel;
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

namespace Synustech.ucPanel.BcStatus
{
    public partial class UserPIOStatus : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblPIOName;
        private Size initialSize_____lblPIOName;
        private Point initialLocation_lblPIOName;

        private float initialFontSize_comboBox; // ComboBox 초기 폰트 크기
        private Size initialSize_comboBox;      // ComboBox 초기 크기
        private Point initialLocation_comboBox; // ComboBox 초기 위치

        private bool isResizing = false;
        public UserPIOStatus()
        {
            InitializeComponent();
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lblPIOName = lblPIOName.Font.Size;
            initialSize_____lblPIOName = lblPIOName.Size;
            initialLocation_lblPIOName = lblPIOName.Location;


            // ComboBox 초기값 저장
            initialFontSize_comboBox = ComboID.Font.Size;
            initialSize_comboBox = ComboID.Size;
            initialLocation_comboBox = ComboID.Location;



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
                UIFunction.AdjustLabelFontSize(lblPIOName, initialFontSize_lblPIOName, ratio);
                UIFunction.AdjustLabelFontSize(lblLReq, initialFontSize_lblPIOName, ratio);
                UIFunction.AdjustLabelFontSize(lblES, initialFontSize_lblPIOName, ratio);
                UIFunction.AdjustLabelFontSize(lblReady, initialFontSize_lblPIOName, ratio);
                UIFunction.AdjustLabelFontSize(lblULReq, initialFontSize_lblPIOName, ratio);
                UIFunction.AdjustLabelFontSize(lblValid, initialFontSize_lblPIOName, ratio);
                UIFunction.AdjustLabelFontSize(lblCompt, initialFontSize_lblPIOName, ratio);
                UIFunction.AdjustLabelFontSize(lblBusy, initialFontSize_lblPIOName, ratio);
                UIFunction.AdjustLabelFontSize(lblTRReq, initialFontSize_lblPIOName, ratio);
                UIFunction.AdjustLabelFontSize(lblCvId, initialFontSize_lblPIOName, ratio);
                UIFunction.AdjustLabelFontSize(lblAvbl, initialFontSize_lblPIOName, ratio);
                UIFunction.AdjustLabelFontSize(lblCS, initialFontSize_lblPIOName, ratio);
                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lblPIOName, initialSize_____lblPIOName, initialLocation_lblPIOName, widthRatio, heightRatio);


                // 콤보박스 폰트 크기 조정
                UIFunction.AdjustComboBoxFontSize(ComboID, initialFontSize_comboBox, ratio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
    }
}
