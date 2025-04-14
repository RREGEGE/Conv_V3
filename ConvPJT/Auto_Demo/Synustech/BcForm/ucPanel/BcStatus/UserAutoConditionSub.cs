using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Synustech.G_Var;

namespace Synustech.ucPanel
{
    public partial class UserAutoConditionSub : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblAutoName;
        private Size initialSize_lblAutoName;
        private Point initialLocation_lblAutoName;

        private float initialFontSize_gridAuto;
        private Size initialSize_gridAuto;
        private Point initialLocation_gridAuto;

        private bool isResizing = false;

        public UserAutoConditionSub()
        {
            InitializeComponent();
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_gridAuto = grid_Auto.Font.Size;
            initialLocation_gridAuto = grid_Auto.Location;
            initialFontSize_lblAutoName = lblAutoName.Font.Size;
            initialSize_lblAutoName = lblAutoName.Size;
            initialLocation_lblAutoName = lblAutoName.Location;

            this.Resize += Panel_Resize;

            // 그리드뷰 초기 행 설정
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            grid_Auto.Rows.Clear();
            grid_Auto.Rows.Add(1, "Safety", "");  // 초기값 설정
            grid_Auto.Rows.Add(2, "All_Servo_On", "");
            grid_Auto.Rows.Add(3, "Auto Key On", "");
            AutoConditionUpdate();  // 초기 상태 업데이트
        }

        private void Panel_Resize(object sender, EventArgs e)
        {
            if (isResizing)
                return;
            try
            {
                isResizing = true;

                if (initialPanelWidth == 0 || initialPanelHeight == 0)
                {
                    return;
                }

                float widthRatio = (float)this.Width / initialPanelWidth;
                float heightRatio = (float)this.Height / initialPanelHeight;
                float ratio = Math.Min(widthRatio, heightRatio);

                UIFunction.AdjustLabelFontSize(lblAutoName, initialFontSize_lblAutoName, ratio * 0.85f);
                UIFunction.AdjustLabel(lblAutoName, initialSize_lblAutoName, initialLocation_lblAutoName, widthRatio, heightRatio);
                UIFunction.AdjustCellFontSize(grid_Auto, initialFontSize_gridAuto, ratio);
            }
            finally
            {
                isResizing = false;
            }
        }
        public void AutoConditionUpdate()
        {
            // 기존 행을 새로 추가하지 않고, 셀의 상태만 업데이트
            string safetyStatus = isSafety ? "ON" : "OFF";
            grid_Auto.Rows[0].Cells[2].Value = safetyStatus;
            UIFunction.UpdateCellStyle(grid_Auto.Rows[0].Cells[2], safetyStatus);

            // conveyors가 null이 아닌지 확인 후 접근
            string allServoStatus = (conveyors != null && conveyors.All(conveyor => conveyor.servo == ServoOnOff.On)) ? "ON" : "OFF";
            grid_Auto.Rows[1].Cells[2].Value = allServoStatus;
            UIFunction.UpdateCellStyle(grid_Auto.Rows[1].Cells[2], allServoStatus);

            string autochange = (Mode_Change == SensorOnOff.On) ? "ON" : "OFF";
            grid_Auto.Rows[2].Cells[2].Value = autochange;
            UIFunction.UpdateCellStyle(grid_Auto.Rows[2].Cells[2], autochange);

        }

        private void UI_Update_Timer_Tick(object sender, EventArgs e)
        {
            AutoConditionUpdate();
        }

        private void grid_Auto_SelectionChanged(object sender, EventArgs e)
        {
            grid_Auto.ClearSelection();
        }
    }
}
