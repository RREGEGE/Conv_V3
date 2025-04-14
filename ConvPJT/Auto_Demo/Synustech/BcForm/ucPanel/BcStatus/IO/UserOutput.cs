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
    public partial class UserOutput : UserControl
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

        public UserOutput()
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
            InitializeOutputGrid();
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
                UIFunction.AdjustLabelFontSize(lblOutputName, initialFontSize_lblOutputName, ratio);
                //AdjustFontSizeDG(dgOutput, initialFontSize_dgOutput, ratio);
                UIFunction.AdjustCellFontSize(dgOutput, initialFontSize_dgOutput, ratio);

                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lblOutputName, initialSize_____lblOutputName, initialLocation_lblOutputName, widthRatio, heightRatio);
                UIFunction.AdjustDataGridViewFontSize(dgOutput, initialFontSize_dgOutput, ratio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
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
        private string OHTPIO(OHTPIO_Passive output)
        {
            return output.ToString();
        }
        private string AGVPIO(AGVPIO_Passive output)
        {
            return output.ToString();
        }
        public void InitializeOutputGrid()
        {
            int outScrollRowIndex = dgOutput.FirstDisplayedScrollingRowIndex;
            dgOutput.Rows.Clear();

            int i = 1;
            // SafetyOut 처리
            SafetyOut SafetyOut = (SafetyOut)bitSafetyReset;
            string status = (SafetyReset == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexOutput = dgOutput.Rows.Add(i++, addrSafetyReset + "." + bitSafetyReset, this.SafetyOut(SafetyOut), status);
            UIFunction.UpdateCellStyle(dgOutput.Rows[rowIndexOutput].Cells[3], status);

            // Lampbits 처리
            int j = 0;
            foreach (var bit in bitsLamp)
            {
                LampAndBuzz LampOut = (LampAndBuzz)bit;
                string statusLamp = (Lamp_Buzz[j++] == SensorOnOff.On) ? "ON" : "OFF";
                int rowIndexOutput1 = dgOutput.Rows.Add(i++, addrLamp_Buzz + "." + bit, LampAndBuzz(LampOut), statusLamp);
                UIFunction.UpdateCellStyle(dgOutput.Rows[rowIndexOutput1].Cells[3], statusLamp);
            }

            // OHTbits 처리
            int z = 0;
            foreach (var bit in OHTSendBits)
            {
                OHTPIO_Passive oht = (OHTPIO_Passive)bit;
                string statusOHT = (G_Var.OHTPIOSend[z++] == SensorOnOff.On) ? "ON" : "OFF";
                int rowIndexOHOUT = dgOutput.Rows.Add(i++, addrOHTPIO + "." + bit, OHTPIO(oht), statusOHT);
                UIFunction.UpdateCellStyle(dgOutput.Rows[rowIndexOHOUT].Cells[3], statusOHT);
            }
            // AGVbits 처리
            int r = 0;
            foreach (var bit in AGVSendBits)
            {
                AGVPIO_Passive agv = (AGVPIO_Passive)bit;
                string statusAGV = (G_Var.AGVPIOSend[r++] == SensorOnOff.On) ? "ON" : "OFF";
                int rowIndexAGVOUT = dgOutput.Rows.Add(i++, addrAGVPIO + "." + bit, AGVPIO(agv), statusAGV);
                UIFunction.UpdateCellStyle(dgOutput.Rows[rowIndexAGVOUT].Cells[3], statusAGV);
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
        public void UpdateOutputGrid()
        {
            int i = 0;
            // SafetyOut 처리
            string status = (SafetyReset == SensorOnOff.On) ? "ON" : "OFF";
            dgOutput.Rows[i].Cells[3].Value = status;
            UIFunction.UpdateCellStyle(dgOutput.Rows[i++].Cells[3], status);

            // Lampbits 처리
            int j = 0;
            foreach (var bit in bitsLamp)
            {
                string statusLamp = (Lamp_Buzz[j++] == SensorOnOff.On) ? "ON" : "OFF";
                dgOutput.Rows[i].Cells[3].Value = statusLamp;
                UIFunction.UpdateCellStyle(dgOutput.Rows[i++].Cells[3], statusLamp);
            }

            // OHTbits 처리
            int z = 0;
            foreach (var bit in OHTSendBits)
            {
                string statusOHT = (G_Var.OHTPIOSend[z++] == SensorOnOff.On) ? "ON" : "OFF";
                dgOutput.Rows[i].Cells[3].Value = statusOHT;
                UIFunction.UpdateCellStyle(dgOutput.Rows[i++].Cells[3], statusOHT);
            }
            // AGVbits 처리
            int r = 0;
            foreach (var bit in AGVSendBits)
            {
                string statusAGV = (G_Var.AGVPIOSend[r++] == SensorOnOff.On) ? "ON" : "OFF";
                dgOutput.Rows[i].Cells[3].Value = statusAGV;
                UIFunction.UpdateCellStyle(dgOutput.Rows[i++].Cells[3], statusAGV);
            }
        }

        /// <summary>
        /// 셀 값이 변경되었는지 확인 후 업데이트
        /// </summary>
        private void UpdateCellIfChanged(DataGridViewCell cell, string newValue)
        {
            if (cell.Value == null || !cell.Value.ToString().Equals(newValue))
            {
                cell.Value = newValue;
                UIFunction.UpdateCellStyle(cell, newValue); // 상태에 따라 스타일도 업데이트
            }
        }
        private void Output_Update_Timer_Tick(object sender, EventArgs e)
        {
            UpdateOutputGrid();
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
