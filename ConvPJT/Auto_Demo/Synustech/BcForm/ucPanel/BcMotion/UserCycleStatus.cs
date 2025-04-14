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

namespace Synustech.ucPanel.BcMotion
{
    public partial class UserCycleStatus : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lbl_CycleStatus;
        private Size  initialSize_____lbl_CycleStatus;
        private Point initialLocation_lbl_CycleStatus;

        private float initialFontSize_lblLineID;
        private Size  initialSize_____lblLineID;
        private Point initialLocation_lblLineID;

        private float initialFontSize_lblID;
        private Size  initialSize_____lblID;
        private Point initialLocation_lblID;

        private float initialFontSize_lblCycleCount;
        private Size  initialSize_____lblCycleCount;
        private Point initialLocation_lblCycleCount;

        private float initialFontSize_lblTotalCycleCount;
        private Size  initialSize_____lblTotalCycleCount;
        private Point initialLocation_lblTotalCycleCount;

        private float initialFontSize_lblCycleTime;
        private Size  initialSize_____lblCycleTime;
        private Point initialLocation_lblCycleTime;

        private float initialFontSize_lblTime;
        private Size  initialSize_____lblTime;
        private Point initialLocation_lblTime;

        private bool isResizing = false;
        public UserCycleStatus()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lblLineID = lblLineID.Font.Size;
            initialSize_____lblLineID = lblLineID.Size;
            initialLocation_lblLineID = lblLineID.Location;

            initialFontSize_lblCycleCount = lblCycleCount.Font.Size;
            initialSize_____lblCycleCount = lblCycleCount.Size;
            initialLocation_lblCycleCount = lblCycleCount.Location;

            initialFontSize_lblCycleTime = lblCycleTime.Font.Size;
            initialSize_____lblCycleTime = lblCycleTime.Size;
            initialLocation_lblCycleTime = lblCycleTime.Location;

            initialFontSize_lblTotalCycleCount = lblTotalCycleCount.Font.Size;
            initialSize_____lblTotalCycleCount = lblTotalCycleCount.Size;
            initialLocation_lblTotalCycleCount = lblTotalCycleCount.Location;

            initialFontSize_lbl_CycleStatus = lbl_CycleStatus.Font.Size;
            initialSize_____lbl_CycleStatus = lbl_CycleStatus.Size;
            initialLocation_lbl_CycleStatus = lbl_CycleStatus.Location;

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
                UIFunction.AdjustLabelFontSize(lblLineID, initialFontSize_lblLineID, ratio);
                UIFunction.AdjustLabelFontSize(lblCycleCount, initialFontSize_lblCycleCount, ratio);
                UIFunction.AdjustLabelFontSize(lblCycleTime, initialFontSize_lblCycleTime, ratio);
                UIFunction.AdjustLabelFontSize(lbl_CycleStatus, initialFontSize_lbl_CycleStatus, ratio);
                UIFunction.AdjustLabelFontSize(lblTotalCycleCount, initialFontSize_lblTotalCycleCount, ratio);

                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lblLineID, initialSize_____lblLineID, initialLocation_lblLineID, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lblCycleCount, initialSize_____lblCycleCount, initialLocation_lblCycleCount, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lblCycleTime, initialSize_____lblCycleTime, initialLocation_lblCycleTime, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lbl_CycleStatus, initialSize_____lbl_CycleStatus, initialLocation_lbl_CycleStatus, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lblTotalCycleCount, initialSize_____lblTotalCycleCount, initialLocation_lblTotalCycleCount, widthRatio, heightRatio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
        private void CycleStatusUpdate()
        {
            if (lines == null)
            {
                return;
            }
            foreach(var line in lines)
            {
                lblLineID.Text = "Line ID: " + line.ID;
                lblCycleCount.Text = "Cycle Count: " + G_Var.currentCycle.ToString() + "/" + G_Var.targetCycle.ToString();
                lblCycleTime.Text = "Cycle Time: " + StopWatchFunc.GetRunningTime(line.cycleStopWatch); 
                lblTotalCycleCount.Text = "Total Cycle Count: " + totalCycle.ToString();
            }
        }

        private void UI_Update_Timer_Tick(object sender, EventArgs e)
        {
            CycleStatusUpdate();
        }
    }
}
