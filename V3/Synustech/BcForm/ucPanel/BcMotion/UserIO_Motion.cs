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
    public partial class UserIO_Motion : UserControl
    {

        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lbl_IOStatus;
        private Size initialSize_____lbl_IOStatus;
        private Point initialLocation_lbl_IOStatus;

        private float initialFontSize_lbl_Input;
        private Size initialSize_____lbl_Input;
        private Point initialLocation_lbl_Input;

        private float initialFontSize_lbl_Output;
        private Size initialSize_____lbl_Output;
        private Point initialLocation_lbl_Output;

        private bool isResizing = false;

        public UserIO_Motion()
        {

            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;


            initialFontSize_lbl_IOStatus = lbl_IOStatus.Font.Size;
            initialSize_____lbl_IOStatus = lbl_IOStatus.Size;
            initialLocation_lbl_IOStatus = lbl_IOStatus.Location;

            initialFontSize_lbl_Input = lbl_Input.Font.Size;
            initialSize_____lbl_Input = lbl_Input.Size;
            initialLocation_lbl_Input = lbl_Input.Location;

            initialFontSize_lbl_Output = lbl_Output.Font.Size;
            initialSize_____lbl_Output = lbl_Output.Size;
            initialLocation_lbl_Output = lbl_Output.Location;

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
                AdjustFontSize(lbl_IOStatus, initialFontSize_lbl_IOStatus, ratio);
                AdjustFontSize(lbl_Input, initialFontSize_lbl_Input, ratio);
                AdjustFontSize(lbl_Output, initialFontSize_lbl_Output, ratio);


                // 라벨 크기 및 위치 조절
                AdjustLabel(lbl_IOStatus, initialSize_____lbl_IOStatus, initialLocation_lbl_IOStatus, widthRatio, heightRatio);
                AdjustLabel(lbl_Input, initialSize_____lbl_Input, initialLocation_lbl_Input, widthRatio, heightRatio);
                AdjustLabel(lbl_Output, initialSize_____lbl_Output, initialLocation_lbl_Output, widthRatio, heightRatio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
        private void AdjustFontSize(System.Windows.Forms.Label label, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio);
            float maxFontSize = newFontSize * 3;
            newFontSize = Math.Max(8, Math.Min(maxFontSize, newFontSize));
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

        private string MyInput(MyInput input)
        {
            // 예: 입력 타입에 따른 문자열을 반환
            return input.ToString();
        }
        private string LongInput(LongInput input)
        {
            // 예: 입력 타입에 따른 문자열을 반환
            return input.ToString();
        }
        private string TurnInput(TurnInput input)
        {
            // 예: 입력 타입에 따른 문자열을 반환
            return input.ToString();
        }
        private string TurnInput2(TurnInput2 input)
        {
            // 예: 입력 타입에 따른 문자열을 반환
            return input.ToString();
        }
        public void SetConvDginput()
        {
            
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

        private void DataGridView_Update_Timer_Tick(object sender, EventArgs e)
        {
            SetConvDginput();
        }

        private void dgInput_SelectionChanged(object sender, EventArgs e)
        {
            dgInput.ClearSelection();
        }

        private void dgOutput_SelectionChanged(object sender, EventArgs e)
        {
            dgOutput.ClearSelection();
        }
    }
}
