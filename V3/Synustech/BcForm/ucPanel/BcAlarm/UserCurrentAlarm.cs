using Synustech.BcForm;
using Synustech.ucPanel.BcAlarm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Synustech.ucPanel.BcAlarm
{
    public partial class UserCurrentAlarm : UserControl
    {
        Calculator calculator = new Calculator();

        DataTable dtMain;

        Dictionary<int , string> alarmDictionary;

        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblCurrentAlarm;
        private Size initialSize_____lblCurrentAlarm;
        private Point initialLocation_lblCurrentAlarm;

        private float initialFontSize_lblID;
        private Size initialSize_____lblID;
        private Point initialLocation_lblID;

        private float initialFontSize_dgCurrentAlarm;
        private Size initialSize_____dgCurrentAlarm;
        private Point initialLocation_dgCurrentAlarm;

        private float initialFontSize_tbID;
        private Size initialSize_____tbID;
        private Point initialLocation_tbID;

        private float initialFontSize_btnApply;
        private Size initialSize_____btnApply;
        private Point initialLocation_btnApply;

        private float initialFontSize_btnSearch;
        private Size initialSize_____btnSearch;
        private Point initialLocation_btnSearch;

        private bool isResizing = false;
        private bool OnOff = false;
        public delegate void delSendCode(String code);

        public delSendCode SendCode;
        public UserCurrentAlarm()
        {
            InitializeComponent();
            DataSet();
            

            alarmDictionary = new Dictionary<int, string>
        {
            { 1, "Interface" },   // CnvID에 대한 Type 저장
            { 2, "Normal" },
            { 3, "Interface" },
            { 4, "Normal" }
        };

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lblCurrentAlarm = lblCurrentAlarm.Font.Size;
            initialSize_____lblCurrentAlarm = lblCurrentAlarm.Size;
            initialLocation_lblCurrentAlarm = lblCurrentAlarm.Location;

            initialFontSize_lblID = lblID.Font.Size;
            initialSize_____lblID = lblID.Size;
            initialLocation_lblID = lblID.Location;

            initialFontSize_dgCurrentAlarm = dgCurrentAlarm.Font.Size;
            initialSize_____dgCurrentAlarm = dgCurrentAlarm.Size;
            initialLocation_dgCurrentAlarm = dgCurrentAlarm.Location;

            initialFontSize_tbID = tbID.Font.Size;
            initialSize_____tbID = tbID.Size;
            initialLocation_tbID = tbID.Location;

            initialFontSize_btnApply = btnApply.Font.Size;
            initialSize_____btnApply = btnApply.Size;
            initialLocation_btnApply = btnApply.Location;

            initialFontSize_btnSearch = btnSearch.Font.Size;
            initialSize_____btnSearch = btnSearch.Size;
            initialLocation_btnSearch = btnSearch.Location;

            this.Resize += Panel_Resize;

            calculator.ValueSend_ID += ApplyID;
            // DataGridView의 열 비율 설정
            SetupDataGridViewColumnWidths();
        }
        public void ApplyID(int value)
        {
            tbID.Text = value.ToString();
        }
        private void SetupDataGridViewColumnWidths()
        {
            dgCurrentAlarm.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 열 크기 설정
            dgCurrentAlarm.Columns[0].FillWeight = 30;
            dgCurrentAlarm.Columns[1].FillWeight = 60;
            dgCurrentAlarm.Columns[2].FillWeight = 10;
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
                AdjustFontSize(lblCurrentAlarm, initialFontSize_lblCurrentAlarm, ratio);
                AdjustFontSize(lblID, initialFontSize_lblID, ratio);
                AdjustFontSize1(btnApply, initialFontSize_btnApply, ratio);
                AdjustFontSize1(btnSearch, initialFontSize_btnSearch, ratio);
                AdjustFontSize2(dgCurrentAlarm, initialFontSize_dgCurrentAlarm, ratio);
                AdjustFontSize3(tbID,initialFontSize_tbID, ratio);

                // 라벨 크기 및 위치 조절
                AdjustLabel(lblCurrentAlarm, initialSize_____lblCurrentAlarm, initialLocation_lblCurrentAlarm, widthRatio, heightRatio);
                AdjustLabel(lblID, initialSize_____lblID, initialLocation_lblID, widthRatio, heightRatio);
                AdjustButton(btnApply, initialSize_____btnApply, initialLocation_btnApply, widthRatio, heightRatio);
                AdjustButton(btnSearch, initialSize_____btnSearch, initialLocation_btnSearch, widthRatio, heightRatio);
                AdjustDataGirdView(dgCurrentAlarm, initialSize_____dgCurrentAlarm, initialLocation_dgCurrentAlarm, widthRatio, heightRatio);
                AdjustTextBox(tbID, initialSize_____tbID, initialLocation_tbID, widthRatio, heightRatio);
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
            dtMain = new DataTable();

            DataColumn colcode = new DataColumn("AlarmCode", typeof(string));
            DataColumn colcontent = new DataColumn("Content", typeof(string));
            DataColumn colON = new DataColumn("On/Off", typeof(string));

            //만든 열들을 테이블에 연결한다.
            dtMain.Columns.Add(colcode);
            dtMain.Columns.Add(colcontent);
            dtMain.Columns.Add(colON);

            dgCurrentAlarm.DataSource = dtMain;
            dgCurrentAlarm.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11);

        }
        public void UpdateAlarmStatus()
        {
            // DataSet을 다시 호출하여 DataGridView를 업데이트
            foreach (DataRow row in dtMain.Rows)
            {
                row["On/Off"] = OnOff ? "On" : "Off";
            }
            dgCurrentAlarm.Refresh(); // DataGridView 새로 고침
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // DataGridView에서 현재 선택된 셀이 있는지 확인
            if (dgCurrentAlarm.CurrentRow != null && dgCurrentAlarm.CurrentRow.Index >= 0)
            {
                // 선택된 행의 "AlarmCode" 값을 가져옴
                string selectedCode = dgCurrentAlarm.CurrentRow.Cells["AlarmCode"].Value.ToString();

                // SendCode 델리게이트 호출
                SendCode?.Invoke(selectedCode);
            }
            else
            {
                // 선택된 행이 없을 때 메시지 출력
                MessageBox.Show("Alarm Code를 선택하세요.");
            }
        }
        private void DgCurrentAlarm_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // On/Off 열의 인덱스는 2이므로 이 열에서 배경색을 설정
            if (dgCurrentAlarm.Columns[e.ColumnIndex].HeaderText == "On/Off")
            {
                // 셀의 값에 따라 배경색 설정
                string onOffStatus = e.Value?.ToString();
                if (onOffStatus == "On")
                {
                    e.CellStyle.BackColor = Color.Green;  // On 상태이면 초록색
                }
                else if (onOffStatus == "Off")
                {
                    e.CellStyle.BackColor = Color.Red;  // Off 상태이면 빨간색
                }
            }
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            // tbID에 입력된 값을 int로 변환
            if (int.TryParse(tbID.Text, out int id))
            {
                // 딕셔너리에서 ID에 해당하는 Type을 가져옴
                if (alarmDictionary.TryGetValue(id, out string type))
                {
                    // XML에서 해당 Type에 포함된 Code와 Content를 출력
                    DisplayAlarmInfoFromXML(type);
                }
                else
                {
                    MessageBox.Show("해당 ID에 대한 Type을 찾을 수 없습니다.");
                }
            }
            else
            {
                MessageBox.Show("유효한 ID를 입력하세요.");
            }
        }
        private void DisplayAlarmInfoFromXML(string type)
        {
            // Resources에서 AlarmInfo XML 데이터를 로드
            string xmlData = Properties.Resources.AlarmInfo;

            // XML 데이터를 파싱하기 위한 XmlDocument 객체 생성
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);

            // 지정된 Type에 해당하는 Alarm 정보를 검색
            XmlNodeList alarmNodes = xmlDoc.SelectNodes("//Type[@type='" + type + "']/Alarm");

            if (alarmNodes.Count > 0)
            {
                // DataTable을 초기화하여 새로운 데이터를 추가
                dtMain.Clear();

                foreach (XmlNode node in alarmNodes)
                {
                    DataRow row = dtMain.NewRow();
                    row["AlarmCode"] = node["Code"]?.InnerText ?? "N/A";  // Code 가져오기
                    row["Content"] = node["Content"]?.InnerText ?? "N/A"; // Content 가져오기
                    row["On/Off"] = OnOff ? "On" : "Off";  // On/Off 상태 설정
                    dtMain.Rows.Add(row);
                }

                // DataGridView 업데이트
                dgCurrentAlarm.DataSource = dtMain;
            }
            else
            {
                MessageBox.Show($"Type '{type}'에 해당하는 알람 정보를 찾을 수 없습니다.");
            }
        }
        private void tbID_MouseClick(object sender, MouseEventArgs e)
        {
            // 마우스 커서 위치를 가져옴
            Point mousePosition = Cursor.Position;

            // 폼의 시작 위치를 수동으로 설정
            calculator.StartPosition = FormStartPosition.Manual;

            // 폼 크기를 얻기 위해 미리 보여주지 않고 레이아웃을 계산
            calculator.Load += (s, ev) =>
            {
                // 폼의 위치를 마우스 커서 위치에서 폼의 높이만큼 Y좌표를 빼서 설정
                calculator.Location = new Point(mousePosition.X, mousePosition.Y);
            };

            calculator.ShowDialog();
        }
    }
}
