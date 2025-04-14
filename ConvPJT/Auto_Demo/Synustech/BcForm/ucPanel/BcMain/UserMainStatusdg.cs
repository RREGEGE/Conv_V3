using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Synustech.G_Var;

namespace Synustech
{
    public partial class UserMainStatusdg : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_dgMotionView;
        private Size initialSize_____dgMotionView;
        private Point initialLocation_dgMotionView;

        private bool isResizing = false;

        public UserMainStatusdg()
        {
            InitializeComponent();

            // 초기 패널 크기 설정
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            // 폰트 초기값 설정
            initialFontSize_dgMotionView = dgMotionView.Font.Size;

            // 컨트롤의 초기 크기와 위치 저장
            initialSize_____dgMotionView = dgMotionView.Size;
            initialLocation_dgMotionView = dgMotionView.Location;

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
                UIFunction.AdjustCellFontSize(dgMotionView, initialFontSize_dgMotionView, ratio);

            }
            finally
            {
                isResizing = false;
            }
        }

        public void SetLineStatusDg()
        {
            foreach (var line in lines)
            {
                line.StatusCheck(); // 상태 갱신

                // 기존 행 검색
                DataGridViewRow existingRow = dgMotionView.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(row => row.Cells[0].Value != null && row.Cells[0].Value.ToString() == line.ID.ToString());

                if (existingRow != null)
                {
                    // 기존 행 값 업데이트
                    UpdateCellIfChanged(existingRow.Cells[1], line.inoutmode);
                    UpdateCellIfChanged(existingRow.Cells[2], line.convEA.ToString());
                    UpdateCellIfChanged(existingRow.Cells[3], line.CSTEA.ToString());
                    UpdateCellIfChanged(existingRow.Cells[4], line.runEA.ToString());
                    UpdateCellIfChanged(existingRow.Cells[5], line.idleEA.ToString());
                    UpdateCellIfChanged(existingRow.Cells[6], line.manualEA.ToString());
                    UpdateCellIfChanged(existingRow.Cells[7], line.errorEA.ToString());
                    //UpdateCellIfChanged(existingRow.Cells[8], line.WarningEA.ToString());
                }
                else
                {
                    // 기존 행이 없으면 새 행 추가
                    dgMotionView.Rows.Add(line.ID, line.inoutmode, line.convEA, line.CSTEA, line.runEA, line.idleEA, line.manualEA, line.errorEA, line.warningEA);
                }
               
                UIFunction.AdjustCellFontSize(dgMotionView, initialFontSize_dgMotionView, (float)this.Width / initialPanelWidth);
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

        private void dgMotionView_SelectionChanged(object sender, EventArgs e)
        {
            dgMotionView.ClearSelection();
        }
    }
}
