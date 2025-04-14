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

        private float initialFontSize_dgView;
        private Size initialSize_dgView;
        private Point initialLocation_dgView;

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

            // 폰트 초기값 설정
            initialFontSize_dgView = dgInput.Font.Size;

            // 컨트롤의 초기 크기와 위치 저장
            initialSize_dgView = dgInput.Size;
            initialLocation_dgView = dgInput.Location;

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
                UIFunction.AdjustLabelFontSize(lbl_IOStatus, initialFontSize_lbl_IOStatus, ratio);
                UIFunction.AdjustLabelFontSize(lbl_Input, initialFontSize_lbl_Input, ratio);
                UIFunction.AdjustLabelFontSize(lbl_Output, initialFontSize_lbl_Output, ratio);


                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lbl_IOStatus, initialSize_____lbl_IOStatus, initialLocation_lbl_IOStatus, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lbl_Input, initialSize_____lbl_Input, initialLocation_lbl_Input, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lbl_Output, initialSize_____lbl_Output, initialLocation_lbl_Output, widthRatio, heightRatio);

                // 그리드뷰 변경
                UIFunction.AdjustCellFontSize(dgInput, initialFontSize_dgView, ratio);
                UIFunction.AdjustCellFontSize(dgOutput, initialFontSize_dgView, ratio);
               
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
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
            int i = 1; // 데이터 순번
            Conveyor selectedConveyor = conveyors.FirstOrDefault(c => c.ID == selectedConvID);

            if (selectedConveyor != null)
            {
                if (selectedConveyor.type == "Turn")
                {
                    UpdateOrAddRowsForTurn(selectedConveyor, ref i);
                }
                else if (selectedConveyor.type == "Long")
                {
                    UpdateOrAddRowsForLong(selectedConveyor, ref i);
                }
                else if (selectedConveyor.type == "Normal")
                {
                    UpdateOrAddRowsForNormal(selectedConveyor, ref i);
                }
            }

        }

        /// <summary>
        /// Turn 타입 데이터 업데이트 또는 추가
        /// </summary>
        private void UpdateOrAddRowsForTurn(Conveyor conveyor, ref int i)
        {
            int j = 0;
            foreach (var bit in conveyor.bits)
            {
                MyInput inputType = (MyInput)bit;
                string status = (conveyor.normalCnvFoupDetect[j++] == SensorOnOff.On) ? "OFF" : "ON";
                UpdateOrAddRow(dgInput, i++, conveyor.ID, MyInput(inputType), status);
            }

            int z = 0;
            foreach (var bit in conveyor.bitsTurn)
            {
                TurnInput inputType = (TurnInput)bit;
                string status = (conveyor.POS[z++] == SensorOnOff.On) ? "ON" : "OFF";
                UpdateOrAddRow(dgInput, i++, conveyor.ID, TurnInput(inputType), status);
            }

            TurnInput2 inputType1 = (TurnInput2)conveyor.bitTurn;
            string status1 = (conveyor.POS[z] == SensorOnOff.On) ? "ON" : "OFF";
            UpdateOrAddRow(dgInput, i++, conveyor.ID, TurnInput2(inputType1), status1);
        }

        /// <summary>
        /// Long 타입 데이터 업데이트 또는 추가
        /// </summary>
        private void UpdateOrAddRowsForLong(Conveyor conveyor, ref int i)
        {
            int j = 0;
            foreach (var bit in conveyor.bits)
            {
                LongInput inputType = (LongInput)bit;
                string status = (conveyor.longCnvFoupDetect[j++] == SensorOnOff.On) ? "OFF" : "ON";
                UpdateOrAddRow(dgInput, i++, conveyor.ID, LongInput(inputType), status);
            }
        }

        /// <summary>
        /// Normal 타입 데이터 업데이트 또는 추가
        /// </summary>
        private void UpdateOrAddRowsForNormal(Conveyor conveyor, ref int i)
        {
            int j = 0;
            foreach (var bit in conveyor.bits)
            {
                MyInput inputType = (MyInput)bit;
                string status = (conveyor.normalCnvFoupDetect[j++] == SensorOnOff.On) ? "OFF" : "ON";
                UpdateOrAddRow(dgInput, i++, conveyor.ID, MyInput(inputType), status);
            }
        }

        /// <summary>
        /// 기존 행 업데이트 또는 새 행 추가
        /// </summary>
        private void UpdateOrAddRow(DataGridView grid, int rowIndex, object id, object inputType, string status)
        {
            // 행이 이미 존재하면 업데이트
            if (grid.Rows.Count >= rowIndex)
            {
                DataGridViewRow row = grid.Rows[rowIndex - 1];
                UpdateCellIfChanged(row.Cells[0], rowIndex);
                UpdateCellIfChanged(row.Cells[1], id);
                UpdateCellIfChanged(row.Cells[2], inputType.ToString());
                UpdateCellIfChanged(row.Cells[3], status);
                UIFunction.UpdateCellStyle(row.Cells[3], status);
                UIFunction.AdjustCellFontSize(dgInput, initialFontSize_dgView, (float)this.Width / initialPanelWidth);
                UIFunction.AdjustCellFontSize(dgOutput, initialFontSize_dgView, (float)this.Width / initialPanelWidth);
            }
            else
            {
                // 행이 없으면 새로 추가
                grid.Rows.Add(rowIndex, id, inputType.ToString(), status);
                UIFunction.UpdateCellStyle(grid.Rows[grid.Rows.Count - 1].Cells[3], status);
                UIFunction.AdjustCellFontSize(dgInput, initialFontSize_dgView, (float)this.Width / initialPanelWidth);
                UIFunction.AdjustCellFontSize(dgOutput, initialFontSize_dgView, (float)this.Width / initialPanelWidth);
            }
        }

        /// <summary>
        /// 기존 셀 값이 변경되었는지 확인 후 업데이트
        /// </summary>
        private void UpdateCellIfChanged(DataGridViewCell cell, object newValue)
        {
            if (cell.Value == null || !cell.Value.Equals(newValue))
            {
                cell.Value = newValue; // 셀 값 업데이트
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
