using Synustech.ucPanel.BcMotion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Synustech.BcForm
{
    public partial class ConveyorLine : Form
    {
        UserConvLineView conLine = new UserConvLineView();
        private float zoomFactor = 1.0f; // 확대/축소 비율


        [DllImport("user32.dll")]
        private static extern bool RegisterTouchWindow(IntPtr hWnd, uint ulFlags);
        private const int WM_GESTURE = 0x0119;
        private const int GID_ZOOM = 3;

        public ConveyorLine()
        {
            InitializeComponent();
            InitializeTableLayoutPanel();
            this.MouseWheel += ConveyorLine_MouseWheel; // 마우스 휠 이벤트 추가
            RegisterTouchWindow(this.Handle, 0); // 터치 활성화
        }
 
        private void InitializeTableLayoutPanel()
        {
            conLine.Dock = DockStyle.Fill;
            tplConView.Controls.Add(conLine,0,1);
        }
        private void btnX_Click(object sender, EventArgs e)
        {
            Close();
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GESTURE)
            {
                ProcessGesture(m);
            }
            base.WndProc(ref m);
        }
        private void ProcessGesture(Message m)
        {
            // GESTUREINFO 구조체 설정 및 초기화
            GESTUREINFO gestureInfo = new GESTUREINFO();
            gestureInfo.cbSize = Marshal.SizeOf(gestureInfo);

            if (GetGestureInfo(m.LParam, ref gestureInfo))
            {
                if (gestureInfo.dwID == GID_ZOOM)
                {
                    // 확대/축소 감지
                    float newZoomFactor = gestureInfo.ullArguments > 0 ? zoomFactor + 0.1f : zoomFactor - 0.1f;
                    zoomFactor = Math.Max(0.1f, Math.Min(newZoomFactor, 5.0f)); // 확대/축소 제한 설정
                    AdjustRowSize(1); // 확대/축소 반영
                }
            }
        }
        private void ConveyorLine_MouseWheel(object sender, MouseEventArgs e)
        {
            // 마우스 휠을 위로 움직이면 확대, 아래로 움직이면 축소
            if (e.Delta > 0)
            {
                zoomFactor += 0.1f; // 확대
            }
            else if (e.Delta < 0 && zoomFactor > 0.1f)
            {
                zoomFactor -= 0.1f; // 축소
            }

            // TableLayoutPanel의 RowStyles에서 (0,1)의 크기를 조정
            AdjustRowSize(1); // 1번째 행의 높이를 조정
        }


        private void AdjustRowSize(int rowIndex)
        {
            // rowIndex에 해당하는 RowStyle 크기 조정
            if (rowIndex >= 0 && rowIndex < tplConView.RowCount)
            {
                // Height 값을 zoomFactor에 맞게 조정 (Percent로 사용)
                tplConView.RowStyles[rowIndex].Height = 50 * zoomFactor; // 원하는 기본값에 zoomFactor 적용
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct GESTUREINFO
        {
            public int cbSize;
            public int dwFlags;
            public int dwID;
            public IntPtr hwndTarget;
            public POINTS ptsLocation;
            public int dwInstanceID;
            public int dwSequenceID;
            public ulong ullArguments;
            public int cbExtraArgs;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINTS
        {
            public short x;
            public short y;
        }

        [DllImport("user32.dll")]
        private static extern bool GetGestureInfo(IntPtr hGestureInfo, ref GESTUREINFO pGestureInfo);
    }
}
