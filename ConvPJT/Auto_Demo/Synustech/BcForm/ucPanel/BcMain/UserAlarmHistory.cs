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

namespace Synustech.ucPanel
{
    public partial class UserAlarmHistory : UserControl
    {

        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblAlarmName;
        private Size  initialSize_____lblAlarmName;
        private Point initialLocation_lblAlarmName;

        private bool isResizing = false;

        public UserAlarmHistory()
        {
            InitializeComponent();
            initialPanelWidth = UcAlarmPnl.Width;
            initialPanelHeight = UcAlarmPnl.Height;

            initialFontSize_lblAlarmName = lblAlarmName.Font.Size;
            initialSize_____lblAlarmName = lblAlarmName.Size;
            initialLocation_lblAlarmName = lblAlarmName.Location;

            del_alarm += GetMonitorStatus;

            UcAlarmPnl.Resize += Panel_Resize;
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
                float widthRatio = (float)UcAlarmPnl.Width / initialPanelWidth;
                float heightRatio = (float)UcAlarmPnl.Height / initialPanelHeight;
                float ratio = Math.Min(widthRatio, heightRatio);

                // 라벨 폰트 사이즈 조절
                UIFunction.AdjustLabelFontSize(lblAlarmName, initialFontSize_lblAlarmName, ratio*0.8f);


                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lblAlarmName, initialSize_____lblAlarmName, initialLocation_lblAlarmName, widthRatio, heightRatio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        void GetMonitorStatus()
        {
            if (dgView.InvokeRequired)
            {
                dgView.Invoke(new Action(GetMonitorStatus));
                return;
            }

            Console.WriteLine("GetMonitor");
            // TotalLog.Ready_to_Convert();
            DataTable dataTable = G_Var.ConvertListToDataTable(G_Var.totalLogs.GetAlarmList());
            dgView.DataSource = dataTable;
            dgView.Columns["Solution"].Visible = false;
            foreach (DataGridViewColumn column in dgView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void dgView_SelectionChanged(object sender, EventArgs e)
        {
            dgView.ClearSelection();
        }
    }
}
