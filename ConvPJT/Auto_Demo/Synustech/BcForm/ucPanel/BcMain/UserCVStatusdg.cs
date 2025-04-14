using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wmx3Api;
using WMX3ApiCLR;
using static Synustech.G_Var;
using static Synustech.UnitConverter;


namespace Synustech.BcForm.ucPanel.BcMain
{
    public partial class UserCVStatusdg : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_dgView;
        private Size initialSize_dgView;
        private Point initialLocation_dgView;

        private bool isResizing = false;

        public UserCVStatusdg()
        {
            InitializeComponent();

            // 초기 패널 크기 설정
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            // 폰트 초기값 설정
            initialFontSize_dgView = dgCvStatusView.Font.Size;

            // 컨트롤의 초기 크기와 위치 저장
            initialSize_dgView = dgCvStatusView.Size;
            initialLocation_dgView = dgCvStatusView.Location;

            // 폼 로드와 크기 조정 이벤트 등록
            this.Resize += Panel_Resize;
        }

        private void Panel_Resize(object sender, EventArgs e)
        {
            if (isResizing)
                return;

            try
            {
                isResizing = true;

                if (initialPanelWidth == 0 || initialPanelHeight == 0)
                {
                    return;
                }

                // 현재 크기에 대한 비율 계산
                float widthRatio = (float)this.Width / initialPanelWidth;
                float heightRatio = (float)this.Height / initialPanelHeight;
                float ratio = Math.Min(widthRatio, heightRatio);

                // 비율에 따라 셀 폰트 크기 및 셀 높이 조정
                UIFunction.AdjustCellFontSize(dgCvStatusView, initialFontSize_dgView, ratio);

            }
            finally
            {
                isResizing = false;
            }
        }

        private void SetGridView()
        {
            foreach (var rect in rectangles)
            {
                if (rect.borderLine == true)
                {
                    Conveyor conv = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                    if (conv != null)
                    {
                        // 기존 행이 있는지 확인
                        DataGridViewRow existingRow = dgCvStatusView.Rows
                            .Cast<DataGridViewRow>()
                            .FirstOrDefault(row => row.Cells[0].Value != null && (int)row.Cells[0].Value == conv.ID);

                        if (existingRow != null)
                        {
                            // 기존 행의 값 업데이트
                            UpdateCellIfChanged(existingRow.Cells[1], conv.type);
                            UpdateCellIfChanged(existingRow.Cells[2], conv.type == "Turn" ? (conv.IsHomeDone ? "Done" : "-") : "-");
                            UpdateCellIfChanged(existingRow.Cells[3], conv.run == Conveyor.CnvRun.Run ? "Busy" : "Idle");
                            UpdateCellIfChanged(existingRow.Cells[4], conv.type == "Turn" ? UnitConverter.InvertumTodegree(WMX3.m_coreMotionStatus.AxesStatus[conv.turnAxis].ActualPos).ToString("F2") + "°": "-");
                            UpdateCellIfChanged(existingRow.Cells[5], conv.autoVelocity != 0 ? InvertspeedTomm(conv.autoVelocity).ToString()+"(mm/s)" : "-");
                            UpdateCellIfChanged(existingRow.Cells[6], conv.CV_AutoStep != 0 ? conv.CV_AutoStep.ToString() : "-");
                        }
                        else
                        {
                            // 기존 행이 없으면 새로 추가
                            if (conv.type == "Turn")
                            {
                                dgCvStatusView.Rows.Add(conv.ID, conv.type, conv.IsHomeDone, "-", "-", conv.autoVelocity, conv.CV_AutoStep, "-");
                            }
                            else
                            {
                                dgCvStatusView.Rows.Add(conv.ID, conv.type, "-", "-", "-", conv.autoVelocity, conv.CV_AutoStep, "-");
                            }
                        }
                    }
                }
            }
            UIFunction.AdjustCellFontSize(dgCvStatusView, initialFontSize_dgView, (float)this.Width / initialPanelWidth);
        }
        /// <summary>
        /// 기존 셀의 값이 변경되었는지 확인 후 업데이트
        /// </summary>
        private void UpdateCellIfChanged(DataGridViewCell cell, string newValue)
        {
            if (cell.Value == null || !cell.Value.ToString().Equals(newValue))
            {
                cell.Value = newValue; // 셀 값 업데이트
            }
        }
        private void UI_Update_Timer_Tick(object sender, EventArgs e)
        {
            SetGridView();
        }

        private void dgCvStatusView_SelectionChanged(object sender, EventArgs e)
        {
            dgCvStatusView.ClearSelection();
        }
    }
}
