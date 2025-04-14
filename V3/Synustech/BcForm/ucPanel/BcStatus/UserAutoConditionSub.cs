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
        private Size initialSize_____lblAutoName;
        private Point initialLocation_lblAutoName;

        private bool isResizing = false;
        public UserAutoConditionSub()
        {
            InitializeComponent();
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lblAutoName = lblAutoName.Font.Size;
            initialSize_____lblAutoName = lblAutoName.Size;
            initialLocation_lblAutoName = lblAutoName.Location;

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
                AdjustFontSize(lblAutoName, initialFontSize_lblAutoName, ratio);


                // 라벨 크기 및 위치 조절
                AdjustLabel(lblAutoName, initialSize_____lblAutoName, initialLocation_lblAutoName, widthRatio, heightRatio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
        private void AdjustFontSize(System.Windows.Forms.Label label, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio * 0.85f);
            float maxFontSize = newFontSize * 3;
            //newFontSize = Math.Max(8, Math.Min(maxFontSize, newFontSize));
            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                label.Font = new Font(label.Font.FontFamily, newFontSize, label.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }

        private void AdjustLabel(System.Windows.Forms.Label label, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                label.Width = (int)(initialSize.Width * widthRatio);
                label.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                label.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        private void AutoCondionUpdate()
        {
            int inScrollRowIndex = grid_Auto.FirstDisplayedScrollingRowIndex;
            grid_Auto.Rows.Clear();

            int i = 1;
            string Safetystatus = (IsSafety == true) ? "ON" : "OFF";
            int rowIndexInputSafety = grid_Auto.Rows.Add(i++, "Safety", Safetystatus);
            UpdateCellStyle(grid_Auto.Rows[rowIndexInputSafety].Cells[2], Safetystatus);

            string AllServostatus = (conveyors.All(conveyor => conveyor.servo == ServoOnOff.On)) ? "ON" : "OFF";
            int rowIndexInputServo = grid_Auto.Rows.Add(i++, "All_Servo_On", AllServostatus);
            UpdateCellStyle(grid_Auto.Rows[rowIndexInputServo].Cells[2], AllServostatus);

            if (inScrollRowIndex >= 0 && inScrollRowIndex < grid_Auto.Rows.Count)
            {
                grid_Auto.FirstDisplayedScrollingRowIndex = inScrollRowIndex;
            }
        }
        private void UpdateCellStyle(DataGridViewCell cell, string status)
        {
            if (status == "ON")
            {
                cell.Style.BackColor = Color.FromArgb(0, 126, 249);
                cell.Style.ForeColor = Color.White;
            }
            else // "OFF"
            {
                cell.Style.BackColor = Color.DarkGray;
                cell.Style.ForeColor = Color.White;
            }
        }
        private void UI_Update_Timer_Tick(object sender, EventArgs e)
        {
            AutoCondionUpdate();
        }

        private void grid_Auto_SelectionChanged(object sender, EventArgs e)
        {
            grid_Auto.ClearSelection();
        }


    }
}
