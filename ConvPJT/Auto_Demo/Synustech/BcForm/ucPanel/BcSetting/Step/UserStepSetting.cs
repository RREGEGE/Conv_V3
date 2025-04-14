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

namespace Synustech.ucPanel.BcSetting.Step
{
    public partial class UserStepSetting : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblConvID;
        private Size initialSize_____lblConvID;
        private Point initialLocation_lblConvID;

        private float initialFontSize_lblID;
        private Size initialSize_____lblID;
        private Point initialLocation_lblID;

        private float initialFontSize_lblAxis;
        private Size initialSize_____lblAxis;
        private Point initialLocation_lblAxis;

        private float initialFontSize_lblLine;
        private Size initialSize_____lblLine;
        private Point initialLocation_lblLine;

        private float initialFontSize_tbID;
        private Size initialSize_____tbID;
        private Point initialLocation_tbID;

        private float initialFontSize_tbAxis;
        private Size initialSize_____tbAxis;
        private Point initialLocation_tbAxis;

        private float initialFontSize_btnSave;
        private Size initialSize_____btnSave;
        private Point initialLocation_btnSave;

        private float initialFontSize_btnLoad;
        private Size initialSize_____btnLoad;
        private Point initialLocation_btnLoad;

        private float initialFontSize_cboxConv;
        private Size initialSize_____cboxConv;
        private Point initialLocation_cboxConv;

        private float initialFontSize_cboxLine;
        private Size initialSize_____cboxLine;
        private Point initialLocation_cboxLine;

        private bool isResizing = false;
        public UserStepSetting()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lblConvID = lblConvID.Font.Size;
            initialSize_____lblConvID = lblConvID.Size;
            initialLocation_lblConvID = lblConvID.Location;

            initialFontSize_lblID = lblID.Font.Size;
            initialSize_____lblID = lblID.Size;
            initialLocation_lblID = lblID.Location;

            initialFontSize_lblAxis = lblAxis.Font.Size;
            initialSize_____lblAxis = lblAxis.Size;
            initialLocation_lblAxis = lblAxis.Location;

            initialFontSize_lblLine = lblLine.Font.Size;
            initialSize_____lblLine = lblLine.Size;
            initialLocation_lblLine = lblLine.Location;

            initialFontSize_tbID = tbID.Font.Size;
            initialSize_____tbID = tbID.Size;
            initialLocation_tbID = tbID.Location;

            initialFontSize_tbAxis = tbAxis.Font.Size;
            initialSize_____tbAxis = tbAxis.Size;
            initialLocation_tbAxis = tbAxis.Location;


            initialFontSize_btnSave = btnSave.Font.Size;
            initialSize_____btnSave = btnSave.Size;
            initialLocation_btnSave = btnSave.Location;

            initialFontSize_btnLoad = btnLoad.Font.Size;
            initialSize_____btnLoad = btnLoad.Size;
            initialLocation_btnLoad = btnLoad.Location;

            initialFontSize_cboxConv = cboxConv.Font.Size;
            initialSize_____cboxConv = cboxConv.Size;
            initialLocation_cboxConv = cboxConv.Location;

            initialFontSize_cboxLine = cboxLine.Font.Size;
            initialSize_____cboxLine = cboxLine.Size;
            initialLocation_cboxLine = cboxLine.Location;

            this.Resize += Panel_Resize;
            // DataGridView의 열 비율 설정
        }
        public void InitializeConveyors(List<Conveyor> conveyors)
        {
            cboxConv.Items.Clear();
            if (conveyors != null)
            {
                foreach (var conveyor in conveyors)
                {
                    cboxConv.Items.Add(conveyor.name);
                }
            }
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
                AdjustFontSize(lblConvID, initialFontSize_lblConvID, ratio);
                AdjustFontSize(lblID, initialFontSize_lblID, ratio);
                AdjustFontSize(lblAxis, initialFontSize_lblAxis, ratio);
                AdjustFontSize(lblLine, initialFontSize_lblLine, ratio);
                AdjustFontSize1(btnSave, initialFontSize_btnSave, ratio);
                AdjustFontSize1(btnLoad, initialFontSize_btnLoad, ratio);
                AdjustFontSize2(cboxConv, initialFontSize_cboxConv, ratio);
                AdjustFontSize2(cboxLine, initialFontSize_cboxLine, ratio);
                AdjustFontSize3(tbAxis, initialFontSize_tbAxis, ratio);

                // 라벨 크기 및 위치 조절
                AdjustLabel(lblConvID, initialSize_____lblConvID, initialLocation_lblConvID, widthRatio, heightRatio);
                AdjustButton(btnSave, initialSize_____btnSave, initialLocation_btnSave, widthRatio, heightRatio);
                AdjustButton(btnLoad, initialSize_____btnLoad, initialLocation_btnLoad, widthRatio, heightRatio);
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
        private void AdjustFontSize1(System.Windows.Forms.Button button, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio);
            float maxFontSize = newFontSize * 3;
            newFontSize = Math.Max(8, Math.Min(maxFontSize, newFontSize));
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
        private void AdjustFontSize2(System.Windows.Forms.ComboBox comboBox, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio) * 3;
            float maxFontSize = newFontSize * 3;
            newFontSize = Math.Max(8, Math.Min(maxFontSize, newFontSize));
            if (newFontSize > initialFontSize)
            {
                newFontSize = initialFontSize;
            }

            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                comboBox.Font = new Font(comboBox.Font.FontFamily, newFontSize, comboBox.Font.Style);

            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다." + comboBox.Name);
            }
        }
        private void AdjustFontSize3(System.Windows.Forms.TextBox textBox, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio);
            float maxFontSize = newFontSize * 3;
            newFontSize = Math.Max(8, Math.Min(maxFontSize, newFontSize));
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
        private void AdjustButton(System.Windows.Forms.Button button, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                button.Width = (int)(initialSize.Width * widthRatio);
                button.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                button.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        private void AdjustTextBox(System.Windows.Forms.TextBox textBox, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                textBox.Width = (int)(initialSize.Width * widthRatio);
                textBox.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                textBox.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        private void AdjustcomboBox(System.Windows.Forms.ComboBox comboBox, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                comboBox.Width = (int)(initialSize.Width * widthRatio);
                comboBox.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                comboBox.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            xmlControl.SaveConveyorToXML(convParamFilePath);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            xmlControl.LoadConveyorFromXML(convParamFilePath);
        }
    }
}
