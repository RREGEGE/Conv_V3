using System;
using System.Drawing;

namespace Synustech
{
    internal class SensorRect : CustomRectangle
    {
        private const int FixedChildWidth = 5;
        private const int FixedChildHeight = 5;
        public Color OnColor = Color.Lime;
        public Color OffColor = Color.Gray;
        public string Name;
        public bool isOnOff = false;  // 상태 저장을 위한 필드
        public SensorRect(int x, int y, string name) : base(x, y)
        {
            Name = name;
            Console.WriteLine(name);
        }

        // 자식 사각형의 고정된 너비, 높이 및 색상
        public override int Width => FixedChildWidth;
        public override int Height => FixedChildHeight;
        public override string TYPE => throw new NotImplementedException();
        // 자식 사각형은 다른 자식 사각형을 추가하지 않음
        public override void AddSensorRectangle(int count)
        {
            // Sensor에는 자식이 없으므로 구현하지 않음
        }
        public override void Draw(Graphics g)
        {
            // 내부 색상 채우기
            using (Brush brush = new SolidBrush(FillColor))
            {
                g.FillRectangle(brush, X, Y, Width, Height);
            }
            // 테두리를 그리지 않음
        }
        public void ChangeColor()
        {
            FillColor = isOnOff ? OffColor : OnColor;
        }

    }
}
