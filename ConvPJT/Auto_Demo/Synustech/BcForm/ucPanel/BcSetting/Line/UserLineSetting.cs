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

namespace Synustech.ucPanel.BcSetting.Line
{
    public partial class UserLineSetting : UserControl
    {
        DataTable dtMain;

        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_dgParam;
        private Size initialSize_____dgParam;
        private Point initialLocation_dgParam;

        private float initialFontSize_btnLineAdd;
        private Size initialSize_____btnLineAdd;
        private Point initialLocation_btnLineAdd;

        private float initialFontSize_btnSave;
        private Size initialSize_____btnSave;
        private Point initialLocation_btnSave;

        private float initialFontSize_btnLoad;
        private Size initialSize_____btnLoad;
        private Point initialLocation_btnLoad;

        private bool isResizing = false;

        public delegate void delLineUpdate(List<Synustech.Line> lines);
        public delLineUpdate LineUpdate;
        public UserLineSetting()
        {
            InitializeComponent();
            dtMain = new DataTable();
            DataSet();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_dgParam = dgParam.Font.Size;
            initialSize_____dgParam = dgParam.Size;
            initialLocation_dgParam = dgParam.Location;

            initialFontSize_btnLineAdd = btnLineAdd.Font.Size;
            initialSize_____btnLineAdd = btnLineAdd.Size;
            initialLocation_btnLineAdd = btnLineAdd.Location;

            initialFontSize_btnSave = btnSave.Font.Size;
            initialSize_____btnSave = btnSave.Size;
            initialLocation_btnSave = btnSave.Location;

            initialFontSize_btnLoad = btnLoad.Font.Size;
            initialSize_____btnLoad = btnLoad.Size;
            initialLocation_btnLoad = btnLoad.Location;

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
                AdjustFontSize1(btnLineAdd, initialFontSize_btnLineAdd, ratio);
                AdjustFontSize1(btnSave, initialFontSize_btnSave, ratio);
                AdjustFontSize1(btnLoad, initialFontSize_btnLoad, ratio);
                AdjustFontSize2(dgParam, initialFontSize_dgParam, ratio);

                // 라벨 크기 및 위치 조절
                AdjustButton(btnLineAdd, initialSize_____btnLineAdd, initialLocation_btnLineAdd, widthRatio, heightRatio);
                AdjustButton(btnSave, initialSize_____btnSave, initialLocation_btnSave, widthRatio, heightRatio);
                AdjustButton(btnLoad, initialSize_____btnLoad, initialLocation_btnLoad, widthRatio, heightRatio);
                AdjustDataGirdView(dgParam, initialSize_____dgParam, initialLocation_dgParam, widthRatio, heightRatio);
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
        private void AdjustFontSize2(System.Windows.Forms.DataGridView dgview, float initialFontSize, float ratio)
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
                dgview.Font = new Font(dgview.Font.FontFamily, newFontSize, dgview.Font.Style);

            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
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
        private void AdjustDataGirdView(System.Windows.Forms.DataGridView dgview, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                dgview.Width = (int)(initialSize.Width * widthRatio);
                dgview.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                dgview.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        private void DataSet()
        {
            DataColumn colLine    = new DataColumn("LINE", typeof(string));
            DataColumn colStartID = new DataColumn("START ID", typeof(int));
            DataColumn colEndID   = new DataColumn("END ID", typeof(int));
            DataColumn colCnvEA   = new DataColumn("Cnv EA", typeof (int));

            //만든 열들을 테이블에 연결한다.
            dtMain.Columns.Add(colLine);
            dtMain.Columns.Add(colStartID);
            dtMain.Columns.Add(colEndID);
            dtMain.Columns.Add(colCnvEA);

            dgParam.DataSource = dtMain;
            dgParam.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            xmlControl.SaveLineToXml(lineFullPath);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            xmlControl.LoadLineFromXml(lineFullPath);
        }

        private void btnLineAdd_Click(object sender, EventArgs e)
        {
            //Synustech.Line line = new Synustech.Line("a");
            //G_Var.lines.Add(line);
            //LineUpdate?.Invoke(G_Var.lines);
        }
        private void AddDataset()
        {
            foreach(var line in lines)
            {

            }
        }
    }
}
