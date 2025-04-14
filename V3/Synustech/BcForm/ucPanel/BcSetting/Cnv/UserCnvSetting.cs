using Org.BouncyCastle.Asn1.Ocsp;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Synustech.ucPanel.BcSetting.Cnv
{
    public partial class UserCnvSetting : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblCnv;
        private Size initialSize_____lblCnv;
        private Point initialLocation_lblCnv;

        private float initialFontSize_lblID;
        private Size initialSize_____lblID;
        private Point initialLocation_lblID;

        private float initialFontSize_lblAxis;
        private Size initialSize_____lblAxis;
        private Point initialLocation_lblAxis;

        private float initialFontSize_lblX;
        private Size initialSize_____lblX;
        private Point initialLocation_lblX;

        private float initialFontSize_lblY;
        private Size initialSize_____lblY;
        private Point initialLocation_lblY;

        private float initialFontSize_lblLine;
        private Size initialSize_____lblLine;
        private Point initialLocation_lblLine;

        private float initialFontSize_tbCnv;
        private Size initialSize_____tbCnv;
        private Point initialLocation_tbCnv;

        private float initialFontSize_tbID;
        private Size initialSize_____tbID;
        private Point initialLocation_tbID;

        private float initialFontSize_tbAxis;
        private Size initialSize_____tbAxis;
        private Point initialLocation_tbAxis;

        private float initialFontSize_tbX;
        private Size initialSize_____tbX;
        private Point initialLocation_tbX;

        private float initialFontSize_tbY;
        private Size initialSize_____tbY;
        private Point initialLocation_tbY;

        private float initialFontSize_cboxConv;
        private Size initialSize_____cboxConv;
        private Point initialLocation_cboxConv;

        private float initialFontSize_cboxLine;
        private Size initialSize_____cboxLine;
        private Point initialLocation_cboxLine;

        private float initialFontSize_btnSave;
        private Size initialSize_____btnSave;
        private Point initialLocation_btnSave;

        private float initialFontSize_btnLoad;
        private Size initialSize_____btnLoad;
        private Point initialLocation_btnLoad;

        public delegate void delRectAdd();

        public delRectAdd PanelRect;

        private bool isResizing = false;
        public UserCnvSetting()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lblCnv = lblCnv.Font.Size;
            initialSize_____lblCnv = lblCnv.Size;
            initialLocation_lblCnv = lblCnv.Location;

            initialFontSize_lblID = lblID.Font.Size;
            initialSize_____lblID = lblID.Size;
            initialLocation_lblID = lblID.Location;

            initialFontSize_lblAxis = lblAxis.Font.Size;
            initialSize_____lblAxis = lblAxis.Size;
            initialLocation_lblAxis = lblAxis.Location;

            initialFontSize_lblX = lblX.Font.Size;
            initialSize_____lblX = lblX.Size;
            initialLocation_lblX = lblX.Location;

            initialFontSize_lblY = lblY.Font.Size;
            initialSize_____lblY = lblY.Size;
            initialLocation_lblY = lblY.Location;

            initialFontSize_lblLine = lblLine.Font.Size;
            initialSize_____lblLine = lblLine.Size;
            initialLocation_lblLine = lblLine.Location;

            initialFontSize_tbID = tbID.Font.Size;
            initialSize_____tbID = tbID.Size;
            initialLocation_tbID = tbID.Location;

            initialFontSize_tbAxis = tbAxis.Font.Size;
            initialSize_____tbAxis = tbAxis.Size;
            initialLocation_tbAxis = tbAxis.Location;

            initialFontSize_tbX = tbX.Font.Size;
            initialSize_____tbX = tbX.Size;
            initialLocation_tbX = tbX.Location;

            initialFontSize_tbY = tbY.Font.Size;
            initialSize_____tbY = tbY.Size;
            initialLocation_tbY = tbY.Location;

            initialFontSize_cboxConv = cboxConv.Font.Size;
            initialSize_____cboxConv = cboxConv.Size;
            initialLocation_cboxConv = cboxConv.Location;

            initialFontSize_cboxLine = cboxLine.Font.Size;
            initialSize_____cboxLine = cboxLine.Size;
            initialLocation_cboxLine = cboxLine.Location;


            initialFontSize_btnSave = btnSave.Font.Size;
            initialSize_____btnSave = btnSave.Size;
            initialLocation_btnSave = btnSave.Location;

            initialFontSize_btnLoad = btnLoad.Font.Size;
            initialSize_____btnLoad = btnLoad.Size;
            initialLocation_btnLoad = btnLoad.Location;

            this.Resize += Panel_Resize;
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
        public void InitializeLines(List<Synustech.Line> lines)
        {
            cboxLine.Items.Clear();
            if (lines != null)
            {
                foreach (var line in lines)
                {
                    cboxLine.Items.Add(line.ID);
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
                AdjustFontSize(lblCnv, initialFontSize_lblCnv, ratio);
                AdjustFontSize(lblID, initialFontSize_lblID, ratio);
                AdjustFontSize(lblAxis, initialFontSize_lblAxis, ratio);
                AdjustFontSize(lblX, initialFontSize_lblX, ratio);
                AdjustFontSize(lblY, initialFontSize_lblY, ratio);
                AdjustFontSize(lblLine, initialFontSize_lblLine, ratio);
                AdjustFontSize1(btnSave, initialFontSize_btnSave, ratio);
                AdjustFontSize1(btnLoad, initialFontSize_btnLoad, ratio);
                AdjustFontSize2(cboxConv, initialFontSize_cboxConv, ratio);
                AdjustFontSize2(cboxLine, initialFontSize_cboxLine, ratio);
                AdjustFontSize3(tbID, initialFontSize_tbID, ratio);
                AdjustFontSize3(tbAxis, initialFontSize_tbAxis, ratio);
                AdjustFontSize3(tbX, initialFontSize_tbX, ratio);
                AdjustFontSize3(tbY, initialFontSize_tbY, ratio);

                // 라벨 크기 및 위치 조절
                AdjustLabel(lblCnv, initialSize_____lblCnv, initialLocation_lblCnv, widthRatio, heightRatio);
                AdjustLabel(lblID, initialSize_____lblID, initialLocation_lblID, widthRatio, heightRatio);
                AdjustLabel(lblAxis, initialSize_____lblAxis, initialLocation_lblAxis, widthRatio, heightRatio);
                AdjustLabel(lblX, initialSize_____lblX, initialLocation_lblX, widthRatio, heightRatio);
                AdjustLabel(lblY, initialSize_____lblY, initialLocation_lblY, widthRatio, heightRatio);
                AdjustLabel(lblLine, initialSize_____lblLine, initialLocation_lblLine, widthRatio, heightRatio);
                AdjustButton(btnSave, initialSize_____btnSave, initialLocation_btnSave, widthRatio, heightRatio);
                AdjustButton(btnLoad, initialSize_____btnLoad, initialLocation_btnLoad, widthRatio, heightRatio);
                AdjustTextBox(tbID, initialSize_____tbID, initialLocation_tbID, widthRatio, heightRatio);
                AdjustTextBox(tbAxis, initialSize_____tbAxis, initialLocation_tbAxis, widthRatio, heightRatio);
                AdjustTextBox(tbX, initialSize_____tbX, initialLocation_tbX, widthRatio, heightRatio);
                AdjustTextBox(tbY, initialSize_____tbY, initialLocation_tbY, widthRatio, heightRatio);
                AdjustcomboBox(cboxConv, initialSize_____cboxConv, initialLocation_cboxConv, widthRatio, heightRatio);
                AdjustcomboBox(cboxLine, initialSize_____cboxLine, initialLocation_cboxLine, widthRatio, heightRatio);
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
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다." + label.Name);
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
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다." + button.Name);
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
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다." + textBox.Name);
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
            _xml.SaveRectanglesToXML(RectFullPath);
            _xml.SaveConveyorToXML(ConvFullPath);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            int x, y, id, axis;
            string selectedConveyorName = cboxConv.Text;
            string lineID = cboxLine.Text;
            // 텍스트박스에서 값 읽어오기
            if (int.TryParse(tbX.Text, out x) && int.TryParse(tbY.Text, out y) && int.TryParse(tbID.Text, out id) && int.TryParse(tbAxis.Text, out axis))
            {
                // ID 중복 여부 확인
                if (G_Var.rectangles.Any(rect => rect.ID == id))
                {
                    MessageBox.Show("이미 존재하는 ID입니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // 중복된 ID가 있을 경우 추가 작업을 중단
                }
                if (G_Var.conveyors.Any(conv => conv.Axis == axis) || G_Var.conveyors.Any(conv => conv.TurnAxis == axis + 1))
                {
                    MessageBox.Show("이미 할당된 Axis입니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // cboxConv에서 선택된 Conveyor 객체 가져오기
                Conveyor selectedConveyor = G_Var.conveyors.FirstOrDefault(c => c.name == selectedConveyorName);
                Synustech.Line selectedLine = G_Var.lines.FirstOrDefault(l => l.ID == lineID);

                // selectedConveyor와 selectedLine이 유효할 때 처리
                if (selectedConveyor != null && selectedLine != null)
                {
                    selectedConveyor.lines.Add(selectedLine);

                    // cboxConv에서 선택된 컨베이어의 타입에 따라 Rect 생성
                    CustomRectangle customRectangle = null;
                    if (selectedConveyor.type == "Interface")
                    {
                        customRectangle = new InterfaceRect(x, y, id);
                    }
                    else if (selectedConveyor.type == "Normal")
                    {
                        customRectangle = new NormalRect(x, y, id);
                    }
                    else if (selectedConveyor.type == "Turn")
                    {
                        customRectangle = new TurnRect(x, y, id);
                    }
                    selectedConveyor.ID = id;
                    selectedConveyor.Axis = axis;
                    if (selectedConveyor.type == "Turn")
                    {
                        selectedConveyor.TurnAxis = axis + 1;
                    }
                    // selectedLine의 conveyors 리스트에 selectedConveyor 추가
                    selectedLine.conveyors.Add(selectedConveyor);
                    foreach (var line in lines)
                    {
                        foreach (var conveyor in line.conveyors)
                        {
                            Console.WriteLine(conveyor.ID);
                        }
                        Console.WriteLine(line.ConvEA);
                    }
                    // 선택된 값에 따라 객체 생성
                    if (customRectangle != null)
                    {
                        G_Var.rectangles.Add(customRectangle); // 리스트에 사각형 추가
                        PanelRect?.Invoke(); // 패널 업데이트
                    }
                }
            }
            else
            {
                MessageBox.Show("유효한 값을 입력하세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            var sortedConveyors = conveyors.OrderBy(c => c.ID).ToList();


        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            _xml.LoadRectanglesFromXML(RectFullPath);
            _xml.LoadConveyorFromXML(ConvFullPath);
        }

        private void cboxConv_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = cboxConv.SelectedItem.ToString();
            Conveyor selectedConveyor = conveyors.FirstOrDefault(c => c.name == selectedName);
            CustomRectangle rect = rectangles.FirstOrDefault(r => r.ID == selectedConveyor.ID);
            if (rect != null)
            {
                tbID.Text = rect.ID.ToString();
                tbX.Text = rect.X.ToString();
                tbY.Text = rect.Y.ToString();
                tbAxis.Text = selectedConveyor.Axis.ToString();

                string targetLineID = selectedConveyor.lines[0].ID.ToString();
                // ComboBox에서 해당 아이템 찾기
                int index = -1;
                for (int i = 0; i < cboxLine.Items.Count; i++)
                {
                    if (cboxLine.Items[i].ToString() == targetLineID)
                    {
                        index = i;
                        break;
                    }
                }

                // 해당 아이템이 있으면 선택
                if (index != -1)
                {
                    cboxLine.SelectedIndex = index;
                }
                else
                {
                    // 해당 아이템이 없을 때 처리 (필요하면 빈 상태로 유지)
                    cboxLine.SelectedIndex = -1;
                }
            }
            else
            {
                tbID.Text = string.Empty;
                tbX.Text = string.Empty;
                tbY.Text = string.Empty;
                tbAxis.Text = string.Empty;

            }
        }
    }
}
