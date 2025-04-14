using Org.BouncyCastle.Asn1.Cms.Ecc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static Synustech.G_Var;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Synustech.ucPanel.BcStatus
{
    public partial class UserInput : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblInputName;
        private Size initialSize_____lblInputName;
        private Point initialLocation_lblInputName;

        private float initialFontSize_dgInput;
        private Size initialSize_____dgInput;
        private Point initialLocation_dgInput;

        private bool isResizing = false;

        bool isMasterDG;

        public UserInput()
        {
            InitializeComponent();
            isMasterDG = true;


            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_dgInput = dgInput.Font.Size;
            initialSize_____dgInput = dgInput.Size;
            initialLocation_dgInput = dgInput.Location;

            initialFontSize_lblInputName = lblInputName.Font.Size;
            initialSize_____lblInputName = lblInputName.Size;
            initialLocation_lblInputName = lblInputName.Location;

            this.Resize += Panel_Resize;
            InitializeMasterGrid();
        }
        public void IsMaster()
        {
            if (!isMasterDG) 
            {
                InitializeMasterGrid();
                isMasterDG = true;
            }
        }
        public void IsConv()
        {
            if (isMasterDG)
            {
                InitializeConvInputGrid();
                isMasterDG = false;
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
                UIFunction.AdjustLabelFontSize(lblInputName, initialFontSize_lblInputName, ratio);
                UIFunction.AdjustCellFontSize(dgInput, initialFontSize_dgInput, ratio);

                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lblInputName, initialSize_____lblInputName, initialLocation_lblInputName, widthRatio, heightRatio);
                UIFunction.AdjustDataGridViewFontSize(dgInput, initialFontSize_dgInput, ratio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
        private void Init()
        {
            foreach (DataGridViewColumn column in dgInput.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
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
        private string Master(Master input)
        {
            // 예: 입력 타입에 따른 문자열을 반환
            return input.ToString();
        }
        private string OHTPIO_in(OHTPIO_Passive input)
        {
            // 예: 입력 타입에 따른 문자열을 반환
            return input.ToString();
        }
        private string AGVPIO_in(AGVPIO_Passive input)
        {
            // 예: 입력 타입에 따른 문자열을 반환
            return input.ToString();
        }
        public void InitializeConvInputGrid()
        {
            //int inScrollRowIndex = -1;
            //try
            //{
            //    inScrollRowIndex = dgInput.FirstDisplayedScrollingRowIndex;
            //}
            //catch { }
            int inScrollRowIndex = dgInput.FirstDisplayedScrollingRowIndex;
            dgInput.Rows.Clear();
            int i = 1;

            foreach (var conveyor in conveyors)
            {
                int rowIndexInput;

                if (conveyor.type == "Turn")
                {
                    // conveyor.bits를 처리
                    int j = 0;
                    foreach (var bit in conveyor.bits)
                    {
                        MyInput inputType = (MyInput)bit;
                        string status = (conveyor.normalCnvFoupDetect[j++] == SensorOnOff.On) ? "OFF" : "ON";
                        rowIndexInput = dgInput.Rows.Add(i++, conveyor.ID, MyInput(inputType), status);
                        UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexInput].Cells[3], status);
                    }

                    // conveyor.bitsTurn를 처리
                    int z = 0;
                    foreach (var bit in conveyor.bitsTurn)
                    {
                        TurnInput inputType = (TurnInput)bit;
                        string status = (conveyor.POS[z++] == SensorOnOff.On) ? "ON" : "OFF";
                        rowIndexInput = dgInput.Rows.Add(i++, conveyor.ID, TurnInput(inputType), status);
                        UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexInput].Cells[3], status);
                    }

                    // conveyor.bitTurn을 처리
                    TurnInput2 inputType1 = (TurnInput2)conveyor.bitTurn;
                    string status1 = (conveyor.POS[z] == SensorOnOff.On) ? "ON" : "OFF";
                    rowIndexInput = dgInput.Rows.Add(i++, conveyor.ID, TurnInput2(inputType1), status1);
                    UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexInput].Cells[3], status1);
                }
                else if (conveyor.type == "Long")
                {
                    int j = 0;
                    foreach (var bit in conveyor.bits)
                    {
                        LongInput inputType = (LongInput)bit;
                        string status = (conveyor.longCnvFoupDetect[j++] == SensorOnOff.On) ? "OFF" : "ON";
                        rowIndexInput = dgInput.Rows.Add(i++, conveyor.ID, LongInput(inputType), status);
                        UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexInput].Cells[3], status);
                    }
                }
                else if (conveyor.type == "Normal")
                {
                    // conveyor.bits를 처리
                    int j = 0;
                    foreach (var bit in conveyor.bits)
                    {
                        MyInput inputType = (MyInput)bit;
                        string status = (conveyor.normalCnvFoupDetect[j++] == SensorOnOff.On) ? "OFF" : "ON";
                        rowIndexInput = dgInput.Rows.Add(i++, conveyor.ID, MyInput(inputType), status);
                        UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexInput].Cells[3], status);
                    }
                }
            }

            // 열 너비 설정 (필요에 따라 조정)
            dgInput.Columns[0].Width = 50; // 첫 번째 열 너비 설정
            dgInput.Columns[1].Width = 100; // 두 번째 열 너비 설정
            //dgInput.Columns[2].Width = 150; // 세 번째 열 너비 설정
            //dgInput.Columns[3].Width = 100; // 네 번째 열 너비 설정

            // 스크롤 위치 유지
            if (inScrollRowIndex >= 0 && inScrollRowIndex < dgInput.Rows.Count)
            {
                dgInput.FirstDisplayedScrollingRowIndex = inScrollRowIndex;
            }
            // Panel 크기 비율 계산
            float widthRatio = (float)this.Width / initialPanelWidth;
            float heightRatio = (float)this.Height / initialPanelHeight;
            float ratio = Math.Min(widthRatio, heightRatio);

            UIFunction.AdjustCellFontSize(dgInput, initialFontSize_dgInput, ratio);
        }
        public void UpdateConvInputGrid()
        {
            int i = 0;

            foreach (var conveyor in conveyors)
            {
                if (conveyor.type == "Turn")
                {
                    // conveyor.bits를 처리
                    int j = 0;
                    foreach (var bit in conveyor.bits)
                    {
                        string foupDetectStatus = (conveyor.normalCnvFoupDetect[j++] == SensorOnOff.On) ? "OFF" : "ON";
                        dgInput.Rows[i].Cells[3].Value = foupDetectStatus;
                        UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], foupDetectStatus);
                    }

                    // conveyor.bitsTurn를 처리
                    int z = 0;
                    foreach (var bit in conveyor.bitsTurn)
                    {
                        string statusPOS = (conveyor.POS[z++] == SensorOnOff.On) ? "ON" : "OFF";
                        dgInput.Rows[i].Cells[3].Value = statusPOS;
                        UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], statusPOS);
                    }

                    // conveyor.bitTurn을 처리
                    string statusPOS3 = (conveyor.POS[z] == SensorOnOff.On) ? "ON" : "OFF";
                    dgInput.Rows[i].Cells[3].Value = statusPOS3;
                    UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], statusPOS3);
                }
                else if (conveyor.type == "Long")
                {
                    int j = 0;
                    foreach (var bit in conveyor.bits)
                    {
                        string statusLongFoupDetect = (conveyor.longCnvFoupDetect[j++] == SensorOnOff.On) ? "OFF" : "ON";
                        dgInput.Rows[i].Cells[3].Value = statusLongFoupDetect;
                        UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], statusLongFoupDetect);
                    }
                }
                else if (conveyor.type == "Normal")
                {
                    // conveyor.bits를 처리
                    int j = 0;
                    foreach (var bit in conveyor.bits)
                    {
                        string statusFoupDetect = (conveyor.normalCnvFoupDetect[j++] == SensorOnOff.On) ? "OFF" : "ON";
                        dgInput.Rows[i].Cells[3].Value = statusFoupDetect;
                        UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], statusFoupDetect);
                    }
                }
            }
        }
        public void InitializeMasterGrid()
        {
            int inScrollRowIndex = dgInput.FirstDisplayedScrollingRowIndex;
            dgInput.Rows.Clear();

            int i = 1;
            Master MasterIn = (Master)bitSafetyIn;
            string status = (G_Var.Safety == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexInput = dgInput.Rows.Add(i++, "Master", Master(MasterIn), status);
            UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexInput].Cells[3], status);

            Master MainPowerIn = (Master)bitMainPower;
            string statusMainPower = (G_Var.MainPower == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexInputMainPower = dgInput.Rows.Add(i++, "Master", Master(MainPowerIn), statusMainPower);
            UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexInputMainPower].Cells[3], statusMainPower);

            Master EMOIn = (Master)bitEMO;
            string statusEMO = (G_Var.EMO == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexInput1 = dgInput.Rows.Add(i++, "Master", Master(EMOIn), statusEMO);
            UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexInput1].Cells[3], statusEMO);

            Master EMS_1 = (Master)addrEMS_1;
            string statusEMS_1 = (G_Var.EMS_1 == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexInputEMS1 = dgInput.Rows.Add(i++, "Master", Master(EMS_1), statusEMS_1);
            UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexInputEMS1].Cells[3], statusEMS_1);

            Master EMS_2 = (Master)addrEMS_2;
            string statusEMS_2 = (G_Var.EMS_2 == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexInputEMS2 = dgInput.Rows.Add(i++, "Master", Master(EMS_2), statusEMS_2);
            UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexInputEMS2].Cells[3], statusEMS_2);

            Master ModeChange = (Master)bitModeChange;
            string statusModeChange = (G_Var.Mode_Change == SensorOnOff.On) ? "ON" : "OFF";
            int rowIndexInputModeChange = dgInput.Rows.Add(i++,"Master", Master(ModeChange),statusModeChange);
            UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexInputModeChange].Cells[3], statusModeChange);

            int j = 0;
            foreach (var bit in OHTReceiveBits)
            {
                OHTPIO_Active OHT = (OHTPIO_Active)bit;
                string statusOHT = (OHTPIOReceive[j++] == SensorOnOff.On) ? "ON" : "OFF";
                int rowIndexOutput1 = dgInput.Rows.Add(i++, "OHT_PIO", OHT, statusOHT);
                UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexOutput1].Cells[3], statusOHT);
            }
            int z = 0;
            foreach (var bit in AGVReceiveBits)
            {
                AGVPIO_Active AGV = (AGVPIO_Active)bit;
                string statusAGV = (AGVPIOReceive[z++] == SensorOnOff.On) ? "ON" : "OFF";
                int rowIndexOutput1 = dgInput.Rows.Add(i++, "AGV_PIO", AGV, statusAGV);
                UIFunction.UpdateCellStyle(dgInput.Rows[rowIndexOutput1].Cells[3], statusAGV);
            }
            if (inScrollRowIndex >= 0 && inScrollRowIndex < dgInput.Rows.Count)
            {
                dgInput.FirstDisplayedScrollingRowIndex = inScrollRowIndex;
            }
            // Panel 크기 비율 계산
            float widthRatio = (float)this.Width / initialPanelWidth;
            float heightRatio = (float)this.Height / initialPanelHeight;
            float ratio = Math.Min(widthRatio, heightRatio);

            UIFunction.AdjustCellFontSize(dgInput, initialFontSize_dgInput, ratio);
        }
        private void UpdateMasterGrid()
        {
            int i = 0;
            string status = (G_Var.Safety == SensorOnOff.On) ? "ON" : "OFF";
            dgInput.Rows[i].Cells[3].Value = status;
            UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], status);

            string statusMainPower = (G_Var.MainPower == SensorOnOff.On) ? "ON" : "OFF";
            dgInput.Rows[i].Cells[3].Value = statusMainPower;
            UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], statusMainPower);

            string statusEMO = (G_Var.EMO == SensorOnOff.On) ? "ON" : "OFF";
            dgInput.Rows[i].Cells[3].Value = statusEMO;
            UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], statusEMO);
            
            string statusEMS_1 = (G_Var.EMS_1 == SensorOnOff.On) ? "ON" : "OFF";
            dgInput.Rows[i].Cells[3].Value = statusEMS_1;
            UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], statusEMS_1);

            string statusEMS_2 = (G_Var.EMS_2 == SensorOnOff.On) ? "ON" : "OFF";
            dgInput.Rows[i].Cells[3].Value = statusEMS_2;
            UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], statusEMS_2);

            string statusModeChange = (G_Var.Mode_Change == SensorOnOff.On) ? "ON" : "OFF";
            dgInput.Rows[i].Cells[3].Value = statusModeChange;
            UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], statusModeChange);

            int j = 0;
            foreach (var bit in OHTReceiveBits)
            {
                string statusOHT = (OHTPIOReceive[j++] == SensorOnOff.On) ? "ON" : "OFF";
                dgInput.Rows[i].Cells[3].Value = statusOHT;
                UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], statusOHT);
            }
            int z = 0;
            foreach (var bit in AGVReceiveBits)
            {
                string statusAGV = (AGVPIOReceive[z++] == SensorOnOff.On) ? "ON" : "OFF";
                dgInput.Rows[i].Cells[3].Value = statusAGV;
                UIFunction.UpdateCellStyle(dgInput.Rows[i++].Cells[3], statusAGV);
            }
        }
        private void IO_Update_Timer_Tick(object sender, EventArgs e)
        {
            if (isMasterDG == true)
            {
                UpdateMasterGrid();
            }
            else
            {
                UpdateConvInputGrid();
            }
        }

        private void dgInput_SelectionChanged(object sender, EventArgs e)
        {
            dgInput.ClearSelection();
        }

        private void dgInput_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
