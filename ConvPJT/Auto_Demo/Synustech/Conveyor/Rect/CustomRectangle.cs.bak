using System.Collections.Generic;
using System.Drawing;

namespace Synustech
{
    public abstract class CustomRectangle
    {
        public List<string> Sensor_names = new List<string>();
        public int ID { get; }
        public abstract string TYPE { get; }
        public int X { get; set; }
        public int Y { get; set; }
        public virtual int Width { get; }  // 각 클래스에서 고정된 너비를 설정
        public virtual int Height { get; } // 각 클래스에서 고정된 높이를 설정
        public Color FillColor = Color.DarkGray;
        public Color previousColor;
        public Color Autocolor = Color.LimeGreen;
        public Color Manualcolor = Color.Orange;
        public Color Offcolor = Color.DarkGray;
        public Color Alarmcolor = Color.Red;
        public int BorderWidth { get; set; } = 2;
        public int sensornum { get; set; }
        public bool borderline = false;

        public List<CustomRectangle> Sensor { get; set; }
        public bool Contains(Point point)
        {
            return point.X >= X && point.X <= X + Width &&
                   point.Y >= Y && point.Y <= Y + Height;
        }

        // 자식 사각형을 추가하는 추상 메서드 (선언만)
        public abstract void AddSensorRectangle(int count);
        public CustomRectangle(int x, int y)
        {
            X = x;
            Y = y;
            Sensor = new List<CustomRectangle>();
        }
        public CustomRectangle(int x, int y, int iD)
        {
            X = x;
            Y = y;
            Sensor = new List<CustomRectangle>(); // 자식 사각형 리스트 초기화
            ID = iD;
        }
        // 사각형을 그리는 메서드
        public virtual void Draw(Graphics g)
        {
            // 내부 색상 채우기
            using (Brush brush = new SolidBrush(FillColor))
            {
                g.FillRectangle(brush, X, Y, Width, Height);
            }
            // 테두리 그리기
            using (Pen pen = new Pen(Color.Black, BorderWidth))
            {
                g.DrawRectangle(pen, X, Y, Width, Height);
            }
            // 자식 사각형 그리기
            foreach (var child in Sensor)
            {
                child.Draw(g);
            }

            if (G_Var.selectedConvID == ID)
            {
                borderline = !borderline;
            }

            if (borderline)
                using (Pen pen = new Pen(Color.Gray, 2))
                {
                    g.DrawRectangle(pen, X, Y, Width, Height);
                }
            else
                using (Pen pen = new Pen(Color.FromArgb(24, 30, 54), 2))
                {
                    g.DrawRectangle(pen, X, Y, Width, Height);
                }
        }
    }

}
