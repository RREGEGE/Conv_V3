using System;
using System.Drawing;
using System.Windows.Forms;
using static Synustech.G_Var;

namespace Synustech.ucPanel
{
    public partial class UserSafetyCondition : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblSafetyName;
        private Size initialSize_lblSafetyName;
        private Point initialLocation_lblSafetyName;

        private float initialFontSize_gridAuto;
        private Size initialSize_gridAuto;
        private Point initialLocation_gridAuto;

        private bool isResizing = false;

        public UserSafetyCondition()
        {
            InitializeComponent();
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_gridAuto = grid_Safety.Font.Size;
            initialLocation_gridAuto = grid_Safety.Location;

            initialFontSize_lblSafetyName = lblSafetyName.Font.Size;
            initialSize_lblSafetyName = lblSafetyName.Size;
            initialLocation_lblSafetyName = lblSafetyName.Location;

            this.Resize += Panel_Resize;

            // 그리드뷰 초기 행 설정
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            grid_Safety.Rows.Clear();
            int i = 1;

            // 초기 데이터 추가
            grid_Safety.Rows.Add(i++, Master((Master)bitSafetyIn), "");
            grid_Safety.Rows.Add(i++, Master((Master)bitMainPower), "");
            grid_Safety.Rows.Add(i++, Master((Master)bitEMO), "");
            grid_Safety.Rows.Add(i++, Master((Master)addrEMS_1), ""); // EMS_1
            grid_Safety.Rows.Add(i++, Master((Master)addrEMS_2), ""); // EMS_2

            // 초기 상태 업데이트
            SafetyDataGridUpdate();
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

                UIFunction.AdjustLabelFontSize(lblSafetyName, initialFontSize_lblSafetyName, ratio);
                UIFunction.AdjustLabel(lblSafetyName, initialSize_lblSafetyName, initialLocation_lblSafetyName, widthRatio, heightRatio);
                UIFunction.AdjustCellFontSize(grid_Safety, initialFontSize_gridAuto, ratio);
            }
            finally
            {
                isResizing = false;
            }
        }
        private string Master(Master input)
        {
            return input.ToString();
        }

        public void SafetyDataGridUpdate()
        {
            // 기존 데이터를 지우지 않고, 셀 상태만 업데이트
            UpdateCellStatus(0, G_Var.Safety, bitSafetyIn);
            UpdateCellStatus(1, G_Var.MainPower, bitMainPower);
            UpdateCellStatus(2, G_Var.EMO, bitEMO);
            UpdateCellStatus(3, G_Var.EMS_1, bitEMS); // EMS_1
            UpdateCellStatus(4, G_Var.EMS_2, bitEMS); // EMS_2
        }

        private void UpdateCellStatus(int rowIndex, SensorOnOff status, int bit)
        {
            string statusText = status == SensorOnOff.On ? "ON" : "OFF";
            grid_Safety.Rows[rowIndex].Cells[2].Value = statusText;
            UIFunction.UpdateCellStyle(grid_Safety.Rows[rowIndex].Cells[2], statusText);
        }

        private void UI_Update_Timer_Tick(object sender, EventArgs e)
        {
            SafetyDataGridUpdate();
        }

        private void grid_Safety_SelectionChanged(object sender, EventArgs e)
        {
            grid_Safety.ClearSelection();
        }
    }
}
