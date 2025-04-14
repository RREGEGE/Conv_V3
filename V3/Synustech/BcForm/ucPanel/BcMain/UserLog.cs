using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synustech
{
    public partial class UserLog : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;
        private float initialFontSize_listLog;

        //private Point initialLocation_listLog;
        //private Size initialSize_listLog;

        private bool isResizing = false;

        public UserLog()
        {
            InitializeComponent();
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;
            initialFontSize_listLog = listLog.Font.Size;
            //initialSize_listLog = listLog.Size;
            //initialLocation_listLog = listLog.Location;

            this.Resize += Panel_Resize;
        }

        //Log Overloading
        public void Log(enLogLevel eLevel1, string LogDesc)
        {
            DateTime dTime = DateTime.Now;
            string LogInfo = $"{dTime:yyyy-MM-dd hh:mm:ss.fff} [{eLevel1.ToString()}]{LogDesc}";
            listLog.Items.Insert(0, LogInfo);
        }
        private void Log(enLogType eType1, enLogLevel eLevel1, enLogTitle eTitle, string LogDesc)
        {
            DateTime dTime = DateTime.Now;
            string LogInfo = $"{dTime:yyyy-MM-dd hh:mm:ss.fff} [{eType1.ToString()}] [{eLevel1.ToString()}] [{eTitle.ToString()}] {LogDesc}";
            listLog.Items.Insert(0, LogInfo);
        }

        private void UserLog_Load(object sender, EventArgs e)
        {

            UserMenu.ELogSender += User_ELogSender;

        }

        private void User_ELogSender(object oSender, enLogType eType1, enLogLevel eLevel, enLogTitle eTitle1, string strLog)
        {
            Log( eType1, eLevel, eTitle1, $"[{oSender.ToString()}]{strLog}");
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
                float widthRatio = (float)this.Width / initialPanelWidth * 3;
                float heightRatio = (float)this.Height / initialPanelHeight * 3;
                float ratio = Math.Min(widthRatio, heightRatio);

                // 라벨 폰트 사이즈 조절
                AdjustFontSize(listLog, initialFontSize_listLog, ratio);
     

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        private void AdjustFontSize(System.Windows.Forms.ListBox list, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = initialFontSize * ratio;



            if (newFontSize > initialFontSize)
            {
                newFontSize = initialFontSize;
            }

            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                list.Font = new Font(list.Font.FontFamily, newFontSize, list.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
    }
}
