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
using static Synustech.G_Var;

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

        public delSendCode del_SendCode;
        public UserCurrentAlarm()
        {
            InitializeComponent();

            alarmDictionary = new Dictionary<int, string>
            {
                { 48, "CST_Empty" },
                { 50, "IN_Step_Time_Over_Error" },
                { 51, "OUT_Step_Time_Over_Error" },
                { 77, "CST_Over_Run" },
                { 78, "Turn_Step_Time_Over_Error" }
            };
            DataSet();
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
        public void ApplyID(double value)
        {
            tbID.Text = value.ToString();
        }
        private void SetupDataGridViewColumnWidths()
        {
            dgCurrentAlarm.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 열 크기 설정
            dgCurrentAlarm.Columns[0].FillWeight = 20;
            dgCurrentAlarm.Columns[1].FillWeight = 80;
            //dgCurrentAlarm.Columns[2].FillWeight = 10;
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
                UIFunction.AdjustLabelFontSize(lblCurrentAlarm, initialFontSize_lblCurrentAlarm, ratio);
                UIFunction.AdjustLabelFontSize(lblID, initialFontSize_lblID, ratio);
                UIFunction.AdjustButtonFontSize(btnApply, initialFontSize_btnApply, ratio);
                UIFunction.AdjustButtonFontSize(btnSearch, initialFontSize_btnSearch, ratio);
                UIFunction.AdjustDataGridViewFontSize(dgCurrentAlarm, initialFontSize_dgCurrentAlarm, ratio);
                UIFunction.AdjustTextBoxFontSize(tbID,initialFontSize_tbID, ratio);

                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lblCurrentAlarm, initialSize_____lblCurrentAlarm, initialLocation_lblCurrentAlarm, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lblID, initialSize_____lblID, initialLocation_lblID, widthRatio, heightRatio);
                UIFunction.AdjustButton(btnApply, initialFontSize_btnApply, initialLocation_btnApply, initialSize_____btnApply, ratio);
                UIFunction.AdjustButton(btnSearch, initialFontSize_btnSearch, initialLocation_btnSearch, initialSize_____btnSearch, ratio);
                //UIFunction.AdjustDataGirdView(dgCurrentAlarm, initialSize_____dgCurrentAlarm, initialLocation_dgCurrentAlarm, widthRatio, heightRatio);
                UIFunction.AdjustTextBox(tbID, initialFontSize_tbID, initialLocation_tbID, initialSize_____tbID, ratio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
        private void DataSet()
        {
            dtMain = new DataTable();

            DataColumn colCode = new DataColumn("AlarmCode", typeof(string));
            DataColumn colContent = new DataColumn("Content", typeof(string));
            //DataColumn colON = new DataColumn("On/Off", typeof(string));

            //만든 열들을 테이블에 연결한다.
            dtMain.Columns.Add(colCode);
            dtMain.Columns.Add(colContent);
            //dtMain.Columns.Add(colON);

            foreach (var alarm in alarmDictionary)
            {
                DataRow row = dtMain.NewRow();
                row["AlarmCode"] = alarm.Key.ToString(); // int를 string으로 변환
                row["Content"] = alarm.Value;
                dtMain.Rows.Add(row);
            }

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
                del_SendCode?.Invoke(selectedCode);
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
            //// tbID에 입력된 값을 int로 변환
            //if (int.TryParse(tbID.Text, out int id))
            //{
            //    // 딕셔너리에서 ID에 해당하는 Type을 가져옴
            //    if (alarmDictionary.TryGetValue(id, out string type))
            //    {
            //        // XML에서 해당 Type에 포함된 Code와 Content를 출력
            //        DisplayAlarmInfoFromXML(type);
            //    }
            //    else
            //    {
            //        MessageBox.Show("해당 ID에 대한 Type을 찾을 수 없습니다.");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("유효한 ID를 입력하세요.");
            //}
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
