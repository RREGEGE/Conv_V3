using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Synustech.BcForm.ucPanel;

namespace Synustech.Common
{
    internal class UIFunction
    {
        public UIFunction() { }

        public void AdjustLabelFontSize(System.Windows.Forms.Label label, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio);
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
        public void AdjustLabelFontSizeTwice(System.Windows.Forms.Label label, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio * 2);
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
        public void AdjustTextBoxFontSize(System.Windows.Forms.TextBox textBox, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio);
            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                textBox.Font = new Font(textBox.Font.FontFamily, newFontSize, textBox.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        public void AdjustButtonFontSize(System.Windows.Forms.Button button, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio);
            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                button.Font = new Font(button.Font.FontFamily, newFontSize, button.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        public void AdjustDataGridViewFontSize(System.Windows.Forms.DataGridView dg, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio);
            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                dg.Font = new Font(dg.Font.FontFamily, newFontSize, dg.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        public void AdjustProgressBarFontSize(CircularProgressBar.CircularProgressBar cirBar, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = initialFontSize * ratio * 0.95f;
            float maxFontSize = newFontSize * 3;
            newFontSize = Math.Max(10, Math.Min(maxFontSize, newFontSize));

            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                cirBar.Font = new Font(cirBar.Font.FontFamily, newFontSize, cirBar.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        public void AdjustListBoxFontSize(System.Windows.Forms.ListBox list, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = initialFontSize * ratio;



            if (newFontSize > initialFontSize)
            {
                newFontSize = initialFontSize;
            }

            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                list.Font = new Font(list.Font.FontFamily, newFontSize, list.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        public void AdjustCellFontSize(DataGridView dgview, float initialFontSize, float ratio)
        {
            float newFontSize = initialFontSize * ratio;
            newFontSize = Math.Max(8, newFontSize); // 최소 폰트 크기 제한

            foreach (DataGridViewRow row in dgview.Rows)
            {
                // 셀의 높이를 새 글꼴 크기에 맞게 조정
                row.Height = (int)(newFontSize * 3); // 폰트 크기에 비례하여 높이 조정
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.Font = new Font(dgview.Font.FontFamily, newFontSize, dgview.Font.Style);
                }
            }
            // 열 머리글의 폰트 크기 및 높이 조정
            dgview.ColumnHeadersDefaultCellStyle.Font = new Font(dgview.Font.FontFamily, newFontSize, dgview.Font.Style);
            dgview.ColumnHeadersHeight = (int)(newFontSize * 3); // 폰트 크기의 두 배로 열 머리글 높이 설정
            dgview.Invalidate(); // 강제 리프레시로 변경 사항 반영
        }
        public void AdjustComboBoxFontSize(BCComoboBox comboBox, float initialFontSize, float ratio)
        {
            try
            {
                // 새로운 폰트 크기를 계산
                float newFontSize = initialFontSize * ratio * 0.9f;
                newFontSize = Math.Max(8, newFontSize); // 최소 폰트 크기 제한

                // 폰트 크기 변경
                comboBox.Font = new Font(comboBox.Font.FontFamily, newFontSize, comboBox.Font.Style);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"콤보박스 폰트 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        public void AdjustLabel(System.Windows.Forms.Label label, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
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
        public void AdjustTextBox(TextBox tBox, float initialFontSize, Point initialLocation, Size initialSize, float ratio)
        {
            tBox.Font = new Font(tBox.Font.FontFamily, initialFontSize * ratio, tBox.Font.Style);
            tBox.Width = (int)(initialSize.Width * ratio);
            tBox.Height = (int)(initialSize.Height * ratio);
            tBox.Location = new Point((int)(initialLocation.X * ratio), (int)(initialLocation.Y * ratio));
        }
        public void AdjustButton(Button btn, float initialFontSize, Point initialLocation, Size initialSize, float ratio)
        {
            btn.Font = new Font(btn.Font.FontFamily, initialFontSize * ratio, btn.Font.Style);
            btn.Width = (int)(initialSize.Width * ratio);
            btn.Height = (int)(initialSize.Height * ratio);
            btn.Location = new Point((int)(initialLocation.X * ratio), (int)(initialLocation.Y * ratio));
        }
        public void AdjustButtonTwice(Button btn, float initialFontSize, Point initialLocation, Size initialSize, float ratio)
        {
            btn.Font = new Font(btn.Font.FontFamily, initialFontSize * ratio * 2, btn.Font.Style);
            btn.Width = (int)(initialSize.Width * ratio);
            btn.Height = (int)(initialSize.Height * ratio);
            btn.Location = new Point((int)(initialLocation.X * ratio), (int)(initialLocation.Y * ratio));
        }
        public void AdjustDataGridView(DataGridView dg, float initialFontSize, Point initialLocation, Size initialSize, float ratio)
        {
            dg.Font = new Font(dg.Font.FontFamily, initialFontSize * ratio, dg.Font.Style);
            dg.Width = (int)(initialSize.Width * ratio);
            dg.Height = (int)(initialSize.Height * ratio);
            dg.Location = new Point((int)(initialLocation.X * ratio), (int)(initialLocation.Y * ratio));
        }

        public void AdjustProgressBar(CircularProgressBar.CircularProgressBar cirBar, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                cirBar.Width = (int)(initialSize.Width * widthRatio);
                cirBar.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                cirBar.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        public void AdjustPictureBox(System.Windows.Forms.PictureBox pictureBox, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                pictureBox.Width = (int)(initialSize.Width * widthRatio);
                pictureBox.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                pictureBox.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        public void UpdateCellStyle(DataGridViewCell cell, string status)
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
    }
}
