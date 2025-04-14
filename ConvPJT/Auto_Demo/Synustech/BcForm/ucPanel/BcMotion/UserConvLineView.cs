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
            }
        }
        private void pnlConvLine_MouseClick(object sender, MouseEventArgs e)
        {
            // 클릭한 위치에 있는 사각형 찾기
            selectedRectangle = rectangles.FirstOrDefault(rect => rect.Contains(e.Location));
            if (selectedRectangle == null)
            {
                selectedConvID = null;
                foreach (var rectangle in G_Var.rectangles)
                {
                    rectangle.borderLine = false;
                }
                return;
            }
            else 
            {
                selectedConvID = selectedRectangle.ID;
            }
            //패널 다시 그리기
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

                        foreach (var sensor in rect.sensors)
                        {
                            if (sensor is SensorRect sensorRect)
                            {
                                Color currentColor = sensorRect.fillColor;
                                if (sensorRect.name == "Foup_Detect1" && conveyor.longCnvFoupDetect[0] == SensorOnOff.On)
                                {
                                    sensorRect.isOnOff = true;
                                }
                                else if (sensorRect.name == "Foup_Detect2" && conveyor.longCnvFoupDetect[1] == SensorOnOff.On)
                                {
                                    sensorRect.isOnOff = true;
                                }
                                else if (sensorRect.name == "Foup_Detect3" && conveyor.longCnvFoupDetect[2] == SensorOnOff.On)
                                {
                                    sensorRect.isOnOff = true;
                                }
                                else if (sensorRect.name == "Foup_Detect1" && conveyor.longCnvFoupDetect[0] == SensorOnOff.Off)
                                {
                                    sensorRect.isOnOff = false;
                                }
                                else if (sensorRect.name == "Foup_Detect2" && conveyor.longCnvFoupDetect[1] == SensorOnOff.Off)
                                {
                                    sensorRect.isOnOff = false;
                                }
                                else if (sensorRect.name == "Foup_Detect3" && conveyor.longCnvFoupDetect[2] == SensorOnOff.Off)
                                {
                                    sensorRect.isOnOff = false;
                                }
                                sensorRect.ChangeColor();
                                if (currentColor != sensorRect.fillColor)
                                {
                                    RedrawSpecificRect(sensor, pnlConvLine);
                                }

                            }
                        }
                    }
                }
                else
                {
                    if (rect != null)
                    {

                        foreach (var sensor in rect.sensors)
                        {

                            if (sensor is SensorRect sensorRect)
                            {
                                Color currentColor = sensorRect.fillColor;
                                if (sensorRect.name == "Foup_Detect1" && conveyor.normalCnvFoupDetect[0] == SensorOnOff.On)
                                {
                                    sensorRect.isOnOff = true;
                                }
                                else if (sensorRect.name == "Foup_Detect2" && conveyor.normalCnvFoupDetect[1] == SensorOnOff.On)
                                {
                                    sensorRect.isOnOff = true;
                                }
                                else if (sensorRect.name == "Foup_Detect1" && conveyor.normalCnvFoupDetect[0] == SensorOnOff.Off)
                                {
                                    sensorRect.isOnOff = false;
                                }
                                else if (sensorRect.name == "Foup_Detect2" && conveyor.normalCnvFoupDetect[1] == SensorOnOff.Off)
                                {
                                    sensorRect.isOnOff = false;
                                }
                                sensorRect.ChangeColor();
                                if (currentColor != sensorRect.fillColor)
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
                if (rect.preColor != rect.fillColor)
                {
                    RedrawSpecificRect(rect, pnlConvLine);
                }
                rect.preColor = rect.fillColor;
            }
        }
        public void RedrawSpecificRect(CustomRectangle rect, Panel panel)
        {
            // 사각형의 위치와 크기로 무효화할 영역 지정
            Rectangle invalidRect = new Rectangle(rect.x, rect.y, rect.width, rect.height);

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
