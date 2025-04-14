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
        bool isAllServoOn = false;
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

        public delCommunicationWMX del_CommunicationWMX;
        public delCloseWMX del_CloseWMX;
        public delServoOn del_ServoOn;
        public delServoOff del_ServoOff;

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
                UIFunction.AdjustButtonFontSize(btnEtherCat, initialFontSize, ratio);
                UIFunction.AdjustButtonFontSize(btnPower, initialFontSize, ratio);
                UIFunction.AdjustButtonFontSize(btnMode, initialFontSize, ratio);


                // 라벨 크기 및 위치 조절
                UIFunction.AdjustButton(btnEtherCat, initialFontSize, initialLocation, initialSize, ratio);
                UIFunction.AdjustButton(btnPower, initialFontSize, initialLocation, initialSize,ratio);
                UIFunction.AdjustButton(btnMode, initialFontSize, initialLocation, initialSize, ratio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        private void btnEtherCat_Click(object sender, EventArgs e)
        {
            if (isEthercatCommunicate)
            {
                Console.WriteLine("Communicate Status: "+isEthercatCommunicate);
                del_CloseWMX?.Invoke();
            }
            else
            {
                Console.WriteLine("Communicate Status: " + isEthercatCommunicate);
                del_CommunicationWMX?.Invoke();
            }
        }

        public void btnEtherCatColor_Change()
        {
            isEthercatCommunicate = WMX3.IsEngineCommunicating();  // 현재 상태를 가져옴

            if (isEthercatCommunicate)
            {
                btnEtherCat.ForeColor = Color.FromArgb(0, 126, 249);
            }
            else
            {
                btnEtherCat.ForeColor = Color.DarkGray;
            }
        }
        private void btnPower_Click(object sender, EventArgs e)
        {
            if (isSafety && !isAllServoOn)
            {
                del_ServoOn?.Invoke();
            }
            else if (isSafety && isAllServoOn)
            {
                del_ServoOff?.Invoke();
            }
            else if (!isSafety)
            {
                MessageBox.Show("Please check Safety");
            }
        }
        public void btnPower_ColorChange()
        {
            if (!isAllServoOn)
            {
                btnPower.ForeColor = Color.DarkGray;
            }
            else
            {
                btnPower.ForeColor = Color.FromArgb(0, 126, 249);
            }
        }
        public void IsAllServoOn()
        {
            if (conveyors == null)
            {
                isAllServoOn = false;
            }
            else
            {
                int count = 0;
                var conveyorsToCheck = conveyors.Where(conveyor => conveyor.axis != -1);
                foreach (var conveyor in conveyorsToCheck)
                {
                    if (conveyor.servo == ServoOnOff.Off)
                    {
                        count++;
                    }
                }
                if (count != 0)
                {
                    isAllServoOn = false;
                }
                else
                {
                    isAllServoOn = true;
                }
            }
            
            btnPower_ColorChange();  // 상태 변경 후 색상 변경
        }
        private async void btnMode_Click(object sender, EventArgs e)
        {
            if (!isAutoEnable)
            {
                MessageBox.Show("Unable to switch to Auto Mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (isAutoEnable && isAutoRun && G_Var.Mode_Change == SensorOnOff.On)
            {
                MessageBox.Show("Please check Mode Changer", "Error");
            }
            else if (isAutoEnable && isAutoRun && G_Var.Mode_Change == SensorOnOff.Off)
            {
                DialogResult result = MessageBox.Show("Switch to Manual Mode?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    isAutoRun = false;
                }
            }
            else if (isAutoEnable && !isAutoRun && G_Var.Mode_Change == SensorOnOff.Off)
            {
                MessageBox.Show("Please check Mode Changer", "Error");
            }
            else if (isAutoEnable && !isAutoRun && isCycleRun && G_Var.Mode_Change == SensorOnOff.On)
            {
                MessageBox.Show("It's in Cycle Mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (isAutoEnable && !isAutoRun && !isCycleRun && G_Var.Mode_Change == SensorOnOff.On)
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
                    isAutoRun = true;
                    isInput = true;
                    await ConvInit();
                }
            }
        }
    }
}