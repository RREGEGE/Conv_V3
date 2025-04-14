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

namespace Synustech
{
    public partial class UserTopComm : UserControl
    {
        bool IsAllServoon = false;
        /// <summary>
        /// 현재 UserControl Size를 담을 변수.
        /// </summary>
        private float initialPanelWidth;
        private float initialPanelHeight;

    /// <summary>
    /// EherCat 버튼에 대한 Size를 담을 변수.
    /// </summary>
        private float initialFontSize;
        private Size initialSize;
        private Point initialLocation;

        private bool isResizing = false;

        public delegate void delCommunicationWMX();
        public delegate void delCloseWMX();
        public delegate void delServoOn();
        public delegate void delServoOff();
        public delegate void delModeChange();

        public delCommunicationWMX CommunicationWMX;
        public delCloseWMX CloseWMX;
        public delServoOn ServoOn;
        public delServoOff ServoOff;

        public UserTopComm()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;
            initialFontSize = btnEtherCat.Font.Size;
            initialSize = btnEtherCat.Size;
            initialLocation = btnEtherCat.Location;


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
                AdjustFontSize(btnEtherCat, initialFontSize, ratio);
                AdjustFontSize(btnPower, initialFontSize, ratio);
                AdjustFontSize(btnMode, initialFontSize, ratio);


                // 라벨 크기 및 위치 조절
                AdjustLabel(btnEtherCat, initialSize, initialLocation, widthRatio, heightRatio);
                AdjustLabel(btnPower, initialSize, initialLocation, widthRatio, heightRatio);
                AdjustLabel(btnMode, initialSize, initialLocation, widthRatio, heightRatio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        private void AdjustFontSize(System.Windows.Forms.Button button, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = initialFontSize * ratio;
            float maxFontSize = newFontSize * 3;
            newFontSize = Math.Max(8, Math.Min(maxFontSize, newFontSize));
            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                button.Font = new Font(button.Font.FontFamily, newFontSize, button.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        private void AdjustLabel(System.Windows.Forms.Button button, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                button.Width = (int)(initialSize.Width * widthRatio);
                button.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                button.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        private void btnEtherCat_Click(object sender, EventArgs e)
        {
            if (IsCommunication)
            {
                Console.WriteLine(IsCommunication);
                CloseWMX?.Invoke();
            }
            else
            {
                Console.WriteLine(IsCommunication);
                CommunicationWMX?.Invoke();
            }
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            if (!IsAuto)
            {
                MessageBox.Show("Unable to switch to Auto Mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (IsAutoRun && IsAutoRun)
            {
                DialogResult result = MessageBox.Show("Switch to Manual Mode?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    IsAutoRun = false;
                }
            }
            else if (IsAuto && !IsAutoRun)
            {
                List<int> falseIds = new List<int>();
                foreach (var conv in conveyors)
                {
                    if (conv.type == "Turn")
                    {
                        if (!conv.IsHomeDone)
                        {
                            falseIds.Add(conv.ID);
                        }
                    }
                }
                if (falseIds.Count > 0)
                {
                    string ids = string.Join(", ", falseIds);
                    MessageBox.Show("The following conveyors have not completed homing: " + ids, "Homing Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult result = MessageBox.Show("Switch to Auto Mode?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    IsAutoRun = true;
                    foreach (var line in lines)
                    {
                        for (int i = 0; i < line.conveyors.Count; i++)
                        {
                            if (line.conveyors[i].mode != Mode.Auto)
                            {
                                line.conveyors[i].Auto_Start_CV_Control();
                            }
                        }
                    }
                }
            }
        }
    }
}