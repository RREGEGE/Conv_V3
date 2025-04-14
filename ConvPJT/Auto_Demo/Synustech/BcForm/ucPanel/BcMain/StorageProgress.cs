using CircularProgressBar;
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

namespace Synustech.ucPanel
{
    public partial class StorageProgress : UserControl
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


        public StorageProgress()
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
                UIFunction.AdjustLabelFontSize(lblStorageComent, iniFontSize__lblStorageComent, ratio);
                UIFunction.AdjustProgressBarFontSize(circularProgressBar1, iniFontSize__circularProgressBar1, ratio);
                UIFunction.AdjustLabelFontSize(lblStorage, iniFontSize__lblStorage, ratio * 0.9f);

                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lblStorageComent, iniSize_______lblStorageComent, iniLocation__lblStorageComent, widthRatio, heightRatio);
                UIFunction.AdjustProgressBar(circularProgressBar1, iniSize_______circularProgressBar1, iniLocation__circularProgressBar1, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lblStorage, iniSize_______lblStorage, iniLocation__lblStorage, widthRatio, heightRatio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
        private void StartCircle()
        {
            circularProgressBar1.Value = 0;
            int percent = 0;
            foreach(var conv in G_Var.conveyors)
            {
                if (conv.type == "Long")
                {
                    conv.LCnV_CST_Empty_Detect();
                    if (conv.longCnvCSTDetected || conv.run == Conveyor.CnvRun.Run)
                    {
                        percent += 100/G_Var.conveyors.Count;
                    }
                }
                else
                {
                    conv.CST_Empty_Detect();
                    if (conv.normalCnvCSTDetected || conv.run == Conveyor.CnvRun.Run)
                    {
                        percent += 100 / G_Var.conveyors.Count;
                    }
                }
            }
            circularProgressBar1.Value = circularProgressBar1.Value + percent;
            circularProgressBar1.Text = circularProgressBar1.Value.ToString() +"%";
        }

        private void Circle_Update_Timer_Tick(object sender, EventArgs e)
        {
            if(G_Var.isAutoRun || G_Var.isCycleRun)
            {
                StartAnimation();
                StartCircle();
            }
            else
            {
                StopAnimation();
                StartCircle();
            }
        }
    }
}