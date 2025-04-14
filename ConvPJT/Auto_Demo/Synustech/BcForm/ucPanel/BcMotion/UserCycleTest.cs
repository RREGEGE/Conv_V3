using Synustech.BcForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Synustech.G_Var;

namespace Synustech.ucPanel.BcMotion
{
    public partial class UserCycleTest : UserControl
    {
        Calculator calculator = new Calculator();
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lbl_CycleTest;
        private Size initialSize_____lbl_CycleTest;
        private Point initialLocation_lbl_CycleTest;

        private float initialFontSize_btn_Count;
        private Size initialSize_____btn_Count;
        private Point initialLocation_btn_Count;

        private float initialFontSize_btn_CountNum;
        private Size initialSize_____btn_CountNum;
        private Point initialLocation_btn_CountNum;

        private float initialFontSize_btn_Auto;
        private Size initialSize_____btn_Auto;
        private Point initialLocation_btn_Auto;

        private float initialFontSize_btn_Manual;
        private Size initialSize_____btn_Manual;
        private Point initialLocation_btn_Manual;

        private float initialFontSize_btn_Run;
        private Size initialSize_____btn_Run;
        private Point initialLocation_btn_Run;

        private float initialFontSize_btn_Stop;
        private Size initialSize_____btn_Stop;
        private Point initialLocation_btn_Stop;

        private bool isResizing = false;

        public static event delMonitorLogView del_ELogSender_UserCycleTest;

        public UserCycleTest()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            //initialFontSize_lbl_CycleTest = lblCycleTest.Font.Size;
            //initialSize_____lbl_CycleTest = lblCycleTest.Size;
            //initialLocation_lbl_CycleTest = lblCycleTest.Location;

            initialFontSize_btn_CountNum = btn_CountNum.Font.Size;
            initialSize_____btn_CountNum = btn_CountNum.Size;
            initialLocation_btn_CountNum = btn_CountNum.Location;

            initialFontSize_btn_Auto = btn_Auto.Font.Size;
            initialSize_____btn_Auto = btn_Auto.Size;
            initialLocation_btn_Auto = btn_Auto.Location;

            initialFontSize_btn_Manual = btn_Manual.Font.Size;
            initialSize_____btn_Manual = btn_Manual.Size;
            initialLocation_btn_Manual = btn_Manual.Location;

            initialFontSize_btn_Run = btn_Run.Font.Size;
            initialSize_____btn_Run = btn_Run.Size;
            initialLocation_btn_Run = btn_Run.Location;

            initialFontSize_btn_Stop = btn_Stop.Font.Size;
            initialSize_____btn_Stop = btn_Stop.Size;
            initialLocation_btn_Stop = btn_Stop.Location;

            this.Resize += Panel_Resize;

            calculator.ValueSend_Cycle += ApplyInching;
        }
        private void ApplyInching(double value)
        {
            btn_CountNum.Text = value.ToString();
        }
        private void btnCount_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;

            // 버튼이 비활성화된 상태에서 글자 색상을 White로 유지
            TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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
                //AdjustFontSize(lblCycleTest, initialFontSize_lbl_CycleTest, ratio);

                // 라벨 크기 및 위치 조절
               // AdjustLabel(lblCycleTest, initialSize_____lbl_CycleTest, initialLocation_lbl_CycleTest, widthRatio, heightRatio);

                UIFunction.AdjustButton(btn_Auto, initialFontSize_btn_Auto, initialLocation_btn_Auto, initialSize_____btn_Auto, ratio);
                UIFunction.AdjustButton(btn_Manual, initialFontSize_btn_Manual, initialLocation_btn_Manual, initialSize_____btn_Manual, ratio);

                UIFunction.AdjustButton(btn_CountNum, initialFontSize_btn_CountNum, initialLocation_btn_CountNum, initialSize_____btn_CountNum, ratio);
                UIFunction.AdjustButton(btn_Run, initialFontSize_btn_Run, initialLocation_btn_Run, initialSize_____btn_Run, ratio);
                UIFunction.AdjustButton(btn_Stop, initialFontSize_btn_Stop, initialLocation_btn_Stop, initialSize_____btn_Stop, ratio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        private void btn_CountNum_Click(object sender, EventArgs e)
        {
            // 마우스 커서 위치를 가져옴
            Point mousePosition = Cursor.Position;

            // 폼의 시작 위치를 수동으로 설정
            calculator.StartPosition = FormStartPosition.Manual;

            // 폼 크기를 얻기 위해 미리 보여주지 않고 레이아웃을 계산
            calculator.Load += (s, ev) =>
            {
                // 폼의 위치를 마우스 커서 위치에서 폼의 높이만큼 Y좌표를 빼서 설정
                calculator.Location = new Point(mousePosition.X, mousePosition.Y - calculator.Height);
            };

            calculator.ShowDialog();
        }

        private async void btn_Run_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            del_ELogSender_UserCycleTest(enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"[{btn.Text}] Button Click");
            if (!isAutoEnable)
            {
                MessageBox.Show("Unable to switch to Cycle Mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (isAutoRun && isAutoRun)
            {
                MessageBox.Show("It's in Auto Mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if(isAutoEnable && !isAutoRun && isCycleRun)
            {
                MessageBox.Show("It's already in  Cycle Mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (isAutoEnable && !isAutoRun && !isCycleRun && G_Var.Mode_Change == SensorOnOff.Off)
            {
                MessageBox.Show("Please check Mode Changer", "Error");
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
                if (int.TryParse(btn_CountNum.Text, out targetCycle))
                {
                    DialogResult result = MessageBox.Show("Switch to Cycle Mode?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        isCycleRun = true;
                        isInput = true;
                        stopCycle = false;
                        await ConvInit();
                        del_ELogSender_UserCycleTest(enLogType.Application, enLogLevel.Normal, enLogTitle.CycleStart, "CycleStart");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter the number of cycles.", "Error", MessageBoxButtons.OK);
                }
                
            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            del_ELogSender_UserCycleTest(enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"[{btn.Text}] Button Click");
            if (isCycleRun)
            {
                DialogResult result = MessageBox.Show("Would you like to exit Cycle Mode?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    stopCycle = true;
                }
            }
            else
            {
                MessageBox.Show("Cycle Mode is not active.", "Confirmation", MessageBoxButtons.OK);
            }
        }

        private void tbl_Cycle_Control_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
