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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Synustech.ucPanel.BcMotion
{
    public partial class UserConvLineView : UserControl
    {

        public UserConvLineView()
        {
            InitializeComponent();
            pnlConvLine.MouseClick += pnlConvLine_MouseClick;
        }
        private void pnlConvLine_Paint(object sender, PaintEventArgs e)
        {
            // 모든 사각형 그리기
            foreach (var rectangle in G_Var.rectangles)
            {
                rectangle.Draw(e.Graphics);
                //Console.WriteLine(rectangle.ID);
            }
            // 선택된 사각형 강조
            //DrawSelectedRectangle(e.Graphics);
        }

  
        // Panel의 Paint 이벤트에서 선택된 사각형 강조
        //private void DrawSelectedRectangle(Graphics g)
        //{
        //    if (G_Var.selectedRectangle != null)
        //    {
        //        using (Pen pen = new Pen(Color.Gray, 2))
        //        {
        //            g.DrawRectangle(pen, G_Var.selectedRectangle.X, G_Var.selectedRectangle.Y, G_Var.selectedRectangle.Width, G_Var.selectedRectangle.Height);
        //        }
        //    }
        //}
        private void pnlConvLine_MouseClick(object sender, MouseEventArgs e)
        {
            // 클릭한 위치에 있는 사각형 찾기
            selectedRectangle = rectangles.FirstOrDefault(rect => rect.Contains(e.Location));
            if (selectedRectangle == null) { return; }
            selectedConvID = selectedRectangle.ID;
            //패널 다시 그리기
            pnlConvLine.Invalidate();
        }
        public void InvalidatePanel()
        {
            pnlConvLine.Invalidate();
        }
        private void UpdateSensor()
        {
            foreach (var conveyor in conveyors)
            {
                int RectID = conveyor.ID;
                CustomRectangle rect = rectangles.FirstOrDefault(c => c.ID == RectID);
                if (conveyor.type == "Long")
                {
                    if (rect != null)
                    {

                        foreach (var sensor in rect.Sensor)
                        {
                            if (sensor is SensorRect sensorRect)
                            {
                                Color currentColor = sensorRect.FillColor;
                                if (sensorRect.Name == "Foup_Detect1" && conveyor.LCnvFoupDetect[0] == SensorOnOff.On)
                                {
                                    sensorRect.isOnOff = true;
                                }
                                else if (sensorRect.Name == "Foup_Detect2" && conveyor.LCnvFoupDetect[1] == SensorOnOff.On)
                                {
                                    sensorRect.isOnOff = true;
                                }
                                else if (sensorRect.Name == "Foup_Detect3" && conveyor.LCnvFoupDetect[2] == SensorOnOff.On)
                                {
                                    sensorRect.isOnOff = true;
                                }
                                else if (sensorRect.Name == "Foup_Detect1" && conveyor.LCnvFoupDetect[0] == SensorOnOff.Off)
                                {
                                    sensorRect.isOnOff = false;
                                }
                                else if (sensorRect.Name == "Foup_Detect2" && conveyor.LCnvFoupDetect[1] == SensorOnOff.Off)
                                {
                                    sensorRect.isOnOff = false;
                                }
                                else if (sensorRect.Name == "Foup_Detect3" && conveyor.LCnvFoupDetect[2] == SensorOnOff.Off)
                                {
                                    sensorRect.isOnOff = false;
                                }
                                sensorRect.ChangeColor();
                                if (currentColor != sensorRect.FillColor)
                                {
                                    RedrawSpecificRect(sensor, pnlConvLine);
                                    //pnlConvLine.Invalidate(sensor);
                                }

                            }
                        }
                    }
                }
                else
                {
                    if (rect != null)
                    {

                        foreach (var sensor in rect.Sensor)
                        {

                            if (sensor is SensorRect sensorRect)
                            {
                                Color currentColor = sensorRect.FillColor;
                                if (sensorRect.Name == "Foup_Detect1" && conveyor.SCnvFoupDetect[0] == SensorOnOff.On)
                                {
                                    sensorRect.isOnOff = true;
                                }
                                else if (sensorRect.Name == "Foup_Detect2" && conveyor.SCnvFoupDetect[1] == SensorOnOff.On)
                                {
                                    sensorRect.isOnOff = true;
                                }
                                else if (sensorRect.Name == "Foup_Detect1" && conveyor.SCnvFoupDetect[0] == SensorOnOff.Off)
                                {
                                    sensorRect.isOnOff = false;
                                }
                                else if (sensorRect.Name == "Foup_Detect2" && conveyor.SCnvFoupDetect[1] == SensorOnOff.Off)
                                {
                                    sensorRect.isOnOff = false;
                                }
                                sensorRect.ChangeColor();
                                if (currentColor != sensorRect.FillColor)
                                {
                                    RedrawSpecificRect(sensor, pnlConvLine);
                                    //pnlConvLine.Invalidate(sensor);
                                }

                            }
                        }
                    }
                }
            }
        }
        private void UpdateRect()
        {
            foreach (var rect in rectangles)
            {
                if (rect.previousColor != rect.FillColor)
                {
                    RedrawSpecificRect(rect, pnlConvLine);
                }
                rect.previousColor = rect.FillColor;
            }
        }
        public void RedrawSpecificRect(CustomRectangle rect, Panel panel)
        {
            // 사각형의 위치와 크기로 무효화할 영역 지정
            Rectangle invalidRect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);

            // 해당 영역만 무효화하고 다시 그리기
            panel.Invalidate(invalidRect);  // 특정 영역 무효화
            panel.Update();                 // 즉시 다시 그리기
        }

        private void UI_Update_Timer_Tick(object sender, EventArgs e)
        {
            UpdateSensor();
            UpdateRect();
        }
    }
}
