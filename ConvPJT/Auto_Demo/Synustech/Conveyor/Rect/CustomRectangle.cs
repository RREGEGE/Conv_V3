using System.Collections.Generic;
using System.Drawing;

namespace Synustech
{
    public abstract class CustomRectangle
    {
        public List<string> sensor_names = new List<string>();
        public int ID { get; }
        public abstract string type { get; }
        public int x { get; set; }
        public int y { get; set; }
        public virtual int width { get; }  // 각 클래스에서 고정된 너비를 설정
        public virtual int height { get; } // 각 클래스에서 고정된 높이를 설정
        public Color fillColor = Color.DarkGray;
        public Color preColor;
        public Color autoColor = Color.LimeGreen;
        public Color manualColor = Color.Orange;
        public Color offColor = Color.DarkGray;
        public Color alarmColor = Color.Red;
        public int borderWidth { get; set; } = 2;
        public int sensorNum { get; set; }
        public bool borderLine = false;
        public List<CustomRectangle> sensors { get; set; }

        public bool Contains(Point point)
        {
            return point.X >= x && point.X <= x + width &&
                   point.Y >= y && point.Y <= y + height;
        }

        // 자식 사각형을 추가하는 추상 메서드 (선언만)
        public abstract void AddSensorRectangle(int count);
        public CustomRectangle(int x, int y)
        {
            this.x = x;
            this.y = y;
            sensors = new List<CustomRectangle>();
        }
        public CustomRectangle(int x, int y, int iD)
        {
            this.x = x;
            this.y = y;
            sensors = new List<CustomRectangle>(); // 자식 사각형 리스트 초기화
            ID = iD;
        }
        // 사각형을 그리는 메서드
        public virtual void Draw(Graphics g)
        {
            // 내부 색상 채우기
            using (Brush brush = new SolidBrush(fillColor))
            {
                g.FillRectangle(brush, x, y, width, height);
            }
            // 테두리 그리기
            using (Pen pen = new Pen(Color.Black, borderWidth))
            {
                g.DrawRectangle(pen, x, y, width, height);
            }
            // 자식 사각형 그리기
            foreach (var child in sensors)
            {
                child.Draw(g);
            }

            if (G_Var.selectedConvID == ID)
            {
                borderLine = !borderLine;
            }

            if (borderLine)
                using (Pen pen = new Pen(Color.Blue, 2))
                {
                    g.DrawRectangle(pen, x, y, width, height);
                }
            else
                using (Pen pen = new Pen(Color.FromArgb(24, 30, 54), 2))
                {
                    g.DrawRectangle(pen, x, y, width, height);
                }
        }
    }

}
