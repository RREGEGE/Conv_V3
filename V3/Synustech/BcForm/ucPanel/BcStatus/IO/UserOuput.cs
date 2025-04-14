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

namespace Synustech.ucPanel.BcStatus
{
    public partial class UserOuput : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblOutputName;
        private Size initialSize_____lblOutputName;
        private Point initialLocation_lblOutputName;

        private float initialFontSize_dgOutput;
        private Size initialSize_____dgOutput;
        private Point initialLocation_dgOutput;

        private bool isResizing = false;

        public UserOuput()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_dgOutput = dgOutput.Font.Size;
            initialSize_____dgOutput = dgOutput.Size;
            initialLocation_dgOutput = dgOutput.Location;

            initialFontSize_lblOutputName = lblOutputName.Font.Size;
            initialSize_____lblOutputName = lblOutputName.Size;
            initialLocation_lblOutputName = lblOutputName.Location;

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
                AdjustFontSize(lblOutputName, initialFontSize_lblOutputName, ratio);
                AdjustFontSizeDG(dgOutput, initialFontSize_dgOutput, ratio);

                // 라벨 크기 및 위치 조절
                AdjustLabel(lblOutputName, initialSize_____lblOutputName, initialLocation_lblOutputName, widthRatio, heightRatio);
                AdjustFontSizeDG(dgOutput, initialFontSize_dgOutput, ratio);
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
        private void AdjustFontSizeDG(System.Windows.Forms.DataGridView dg, float initialFontSize, float ratio)
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
        private void AdjustDataGirdView(System.Windows.Forms.DataGridView dg, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                dg.Width = (int)(initialSize.Width * widthRatio);
                dg.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                dg.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        private string SafetyOut(SafetyOut output)
        {
            // 예: 입력 타입에 따른 문자열을 반환
            return output.ToString();
        }
        private string LampAndBuzz(LampAndBuzz output)
        {
            return output.ToString();
        }
        private string OHTPIO(OHTPIO_out output)
        {
            return output.ToString();
        }
        private string AGVPIO(AGVPIO_out output)
        {
            return output.ToString();
        }
        public void SetOutput()
        {
            int outScrollRowIndex = dgOutput.FirstDisplayedScrollingRowIndex;
            dgOutput.Rows.Clear();
            int i = 1;

            // SafetyOut 처리
            SafetyOut safetyout = (SafetyOut)bitSafety;
            string status = (SafetyReset == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexOutput = dgOutput.Rows.Add(i++, addrSafety + "." + bitSafety, SafetyOut(safetyout), status);
            UpdateCellStyle(dgOutput.Rows[rowIndexOutput].Cells[3], status);

            // Lampbits 처리
            int j = 0;
            foreach (var bit in Lampbits)
            {
                LampAndBuzz lampout = (LampAndBuzz)bit;
                string statuslamp = (Lamp_Buzz[j++] == SensorOnOff.On) ? "ON" : "OFF";
                int rowIndexOutput1 = dgOutput.Rows.Add(i++, addrLamp_Buzz + "." + bit, LampAndBuzz(lampout), statuslamp);
                UpdateCellStyle(dgOutput.Rows[rowIndexOutput1].Cells[3], statuslamp);
            }

            // OHTbits 처리
            int z = 0;
            foreach (var bit in OHTbits)
            {
                OHTPIO_out ohtout = (OHTPIO_out)bit;
                string statusOHT = (OHTpio[z++] == SensorOnOff.On) ? "ON" : "OFF";
                int rowIndexOutput1 = dgOutput.Rows.Add(i++, addrOHTPIO + "." + bit, OHTPIO(ohtout), statusOHT);
                UpdateCellStyle(dgOutput.Rows[rowIndexOutput1].Cells[3], statusOHT);
            }

            // AGVbits 처리
            int r = 0;
            foreach (var bit in AGVbits)
            {
                AGVPIO_out agvout = (AGVPIO_out)bit;
                string statusAGV = (AGVpio[r++] == SensorOnOff.On) ? "ON" : "OFF";
                int rowIndexOutput1 = dgOutput.Rows.Add(i++, addrAGVPIO + "." + bit, AGVPIO(agvout), statusAGV);
                UpdateCellStyle(dgOutput.Rows[rowIndexOutput1].Cells[3], statusAGV);
            }

            // 열 너비 설정
            dgOutput.Columns[0].Width = 50; // 첫 번째 열 너비 설정
            dgOutput.Columns[1].Width = 100; // 두 번째 열 너비 설정
                                             // 나머지 열도 설정...

            if (outScrollRowIndex >= 0 && outScrollRowIndex < dgOutput.Rows.Count)
            {
                dgOutput.FirstDisplayedScrollingRowIndex = outScrollRowIndex;
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
        private void Output_Update_Timer_Tick(object sender, EventArgs e)
        {
            SetOutput();
        }

        private void dgOutput_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                // 인덱스 가져오기
                string indexValue = dgOutput.Rows[e.RowIndex].Cells[1].Value.ToString();
                // Status 값 가져오기
                string statusValue = dgOutput.Rows[e.RowIndex].Cells[3].Value.ToString();

                string[] index = indexValue.Split('.');
                if (index.Length == 2)
                {
                    int addr = int.Parse(index[0]);
                    int bit = int.Parse(index[1]);
                    if (statusValue == "ON")
                    {
                        WMX3.m_wmx3io.SetOutBit(addr, bit, 0);
                    }
                    else if (statusValue == "OFF")
                    {
                        WMX3.m_wmx3io.SetOutBit(addr, bit, 1);
                    }
                }
            }
        }

        private void dgOutput_SelectionChanged(object sender, EventArgs e)
        {
            dgOutput.ClearSelection();
        }
    }
}
