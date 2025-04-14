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
    public partial class UserSafetyCondition : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblSafetyName;
        private Size initialSize_____lblSafetyName;
        private Point initialLocation_lblSafetyName;

        private bool isResizing = false;
        public UserSafetyCondition()
        {
            InitializeComponent();
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lblSafetyName = lblSafetyName.Font.Size;
            initialSize_____lblSafetyName = lblSafetyName.Size;
            initialLocation_lblSafetyName = lblSafetyName.Location;

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
                AdjustFontSize(lblSafetyName, initialFontSize_lblSafetyName, ratio);


                // 라벨 크기 및 위치 조절
                AdjustLabel(lblSafetyName, initialSize_____lblSafetyName, initialLocation_lblSafetyName, widthRatio, heightRatio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
        private void AdjustFontSize(System.Windows.Forms.Label label, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio*0.85f);
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
        private string Master(Master input)
        {
            // 예: 입력 타입에 따른 문자열을 반환
            return input.ToString();
        }
        public void SafetyDataGridUpdate()
        {
            int inScrollRowIndex = grid_Safety.FirstDisplayedScrollingRowIndex;
            grid_Safety.Rows.Clear();

            int i = 1;
            Master masterin = (Master)Safetyinbit;
            string status = (G_Var.Safety == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexInput = grid_Safety.Rows.Add(i++, Master(masterin), status);
            UpdateCellStyle(grid_Safety.Rows[rowIndexInput].Cells[2], status);

            Master mainpowerin = (Master)MainPowerbit;
            string MainPowerstatus = (G_Var.MainPower == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexInputMainPower = grid_Safety.Rows.Add(i++, Master(mainpowerin), MainPowerstatus);
            UpdateCellStyle(grid_Safety.Rows[rowIndexInputMainPower].Cells[2], MainPowerstatus);

            Master EMOin = (Master)EMObit;
            string EMOstatus = (G_Var.EMO == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexInput1 = grid_Safety.Rows.Add(i++, Master(EMOin), EMOstatus);
            UpdateCellStyle(grid_Safety.Rows[rowIndexInput1].Cells[2], EMOstatus);

            Master EMS_1 = (Master)22;
            string EMS_1status = (G_Var.EMS_1 == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexInputEMS1 = grid_Safety.Rows.Add(i++, Master(EMS_1), EMS_1status);
            UpdateCellStyle(grid_Safety.Rows[rowIndexInputEMS1].Cells[2], EMS_1status);

            Master EMS_2 = (Master)38;
            string EMS_2status = (G_Var.EMS_2 == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexInputEMS2 = grid_Safety.Rows.Add(i++, Master(EMS_2), EMS_2status);
            UpdateCellStyle(grid_Safety.Rows[rowIndexInputEMS2].Cells[2], EMS_2status);
            if (inScrollRowIndex >= 0 && inScrollRowIndex < grid_Safety.Rows.Count)
            {
                grid_Safety.FirstDisplayedScrollingRowIndex = inScrollRowIndex;
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
            SafetyDataGridUpdate();
        }

        private void grid_Safety_SelectionChanged(object sender, EventArgs e)
        {
            grid_Safety.ClearSelection();
        }
    }
}
