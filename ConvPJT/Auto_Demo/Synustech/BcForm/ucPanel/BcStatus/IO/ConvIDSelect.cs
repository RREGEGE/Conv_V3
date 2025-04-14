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

namespace Synustech.ucPanel.BcStatus.IO
{
    public partial class ConvIDSelect : UserControl
    {
        private List<int> pages;
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

        public delMasterUpdate del_MasterUpdate;
        public delConvUpdate del_ConvUpdate;

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
                UIFunction.AdjustButton(btnMaster, initialFontSize_btnMaster, initialLocation_btnMaster, initialSize_____btnMaster, ratio);
                UIFunction.AdjustButton(btnConv, initialFontSize_btnConv, initialLocation_btnConv, initialSize_____btnMaster, ratio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
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
            del_MasterUpdate?.Invoke();
            btnMaster.BackColor = Color.FromArgb(37, 41, 64);
            btnMaster.ForeColor = Color.FromArgb(0, 126, 249);
            btnConv.BackColor = Color.FromArgb(24, 30, 54);
            btnConv.ForeColor = Color.White;
        }

        private void btnConv_Click(object sender, EventArgs e)
        {
            del_ConvUpdate?.Invoke();
            btnMaster.BackColor = Color.FromArgb(24, 30, 54);
            btnMaster.ForeColor = Color.White;
            btnConv.BackColor = Color.FromArgb(37, 41, 64);
            btnConv.ForeColor = Color.FromArgb(0, 126, 249);
        }
    }
}
