using Synustech.BcForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

namespace Synustech.ucPanel.BcAlarm
{
    
    public partial class UserSolution : UserControl
    {
        UserCurrentAlarm ucCurrentAlarm = new UserCurrentAlarm();

        XmlDocument xdoc = new XmlDocument(); // XmlDocument 객체 생성

        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblSolution;
        private Size initialSize_____lblSolution;
        private Point initialLocation_lblSolution;

        private float initialFontSize_tbSolution;
        private Size initialSize_____tbSolution;
        private Point initialLocation_tbSolution;

        private bool isResizing = false;
        public UserSolution()
        {
            InitializeComponent();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lblSolution = lblSolution.Font.Size;
            initialSize_____lblSolution = lblSolution.Size;
            initialLocation_lblSolution = lblSolution.Location;

            initialFontSize_tbSolution = tbSolution.Font.Size;
            initialSize_____tbSolution = tbSolution.Size;
            initialLocation_tbSolution = tbSolution.Location;

            this.Resize += Panel_Resize;

            LoadXmlData();
        }
        private void LoadXmlData()
        {
            string xmlData = Properties.Resources.AlarmInfo;
            xdoc.LoadXml(xmlData);
        }

        public void UcCurrentAlarm_SendCode(string code)
        {
            // 전달받은 코드로 XML에서 Solution을 찾음
            string solution = FindSolutionByCode(code);

            // Solution을 TextBox에 출력
            if (!string.IsNullOrEmpty(solution))
            {
                tbSolution.Text = solution;
            }
            else
            {
                tbSolution.Text = "해당 코드에 대한 솔루션을 찾을 수 없습니다.";
            }
        }

        private string FindSolutionByCode(string code)
        {
            // XML에서 특정 Code 값을 가진 Alarm 노드를 찾고, Solution 값을 반환
            XmlNode alarmNode = xdoc.SelectSingleNode($"//Alarm[Code='{code}']");

            if (alarmNode != null)
            {
                // Solution 노드가 존재하면 해당 값을 반환
                XmlNode solutionNode = alarmNode.SelectSingleNode("Solution");
                if (solutionNode != null)
                {
                    return solutionNode.InnerText;
                }
            }

            // 해당 코드에 대한 Solution을 찾지 못했을 때
            return null;
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
                AdjustFontSize(lblSolution, initialFontSize_lblSolution, ratio);
                AdjustFontSize2(tbSolution, initialFontSize_tbSolution, ratio);

                // 라벨 크기 및 위치 조절
                AdjustLabel(lblSolution, initialSize_____lblSolution, initialLocation_lblSolution, widthRatio, heightRatio);
                AdjusttextBox(tbSolution, initialSize_____tbSolution, initialLocation_tbSolution, widthRatio, heightRatio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        private void AdjustFontSize(System.Windows.Forms.Label label, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio);
            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                label.Font = new Font(label.Font.FontFamily, newFontSize, label.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        private void AdjustFontSize2(System.Windows.Forms.TextBox textBox, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio) * 3;

            if (newFontSize > initialFontSize)
            {
                newFontSize = initialFontSize;
            }

            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                textBox.Font = new Font(textBox.Font.FontFamily, newFontSize, textBox.Font.Style);

            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        private void AdjustLabel(System.Windows.Forms.Label label, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                label.Width = (int)(initialSize.Width * widthRatio);
                label.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                label.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        private void AdjusttextBox(System.Windows.Forms.TextBox textBox, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                textBox.Width = (int)(initialSize.Width * widthRatio);
                textBox.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                textBox.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }
    }
    
}
