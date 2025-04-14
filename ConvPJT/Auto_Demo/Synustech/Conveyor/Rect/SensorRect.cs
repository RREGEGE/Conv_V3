using System;
using System.Drawing;

namespace Synustech
{
    internal class SensorRect : CustomRectangle
    {
        private const int FIXED_WIDTH = 5;
        private const int FIXED_HEIGHT = 5;
        public Color onColor = Color.Lime;
        public Color offSensorColor = Color.Gray;
        public string name;
        public bool isOnOff = false;  // 상태 저장을 위한 필드
        public SensorRect(int x, int y, string name) : base(x, y)
        {
            this.name = name;
            Console.WriteLine(name);
        }

        // 자식 사각형의 고정된 너비, 높이 및 색상
        public override int width => FIXED_WIDTH;
        public override int height => FIXED_HEIGHT;
        public override string type => throw new NotImplementedException();
        // 자식 사각형은 다른 자식 사각형을 추가하지 않음
        public override void AddSensorRectangle(int count)
        {
            // Sensor에는 자식이 없으므로 구현하지 않음
        }
        public override void Draw(Graphics g)
        {
            // 내부 색상 채우기
            using (Brush brush = new SolidBrush(fillColor))
            {
                g.FillRectangle(brush, x, y, width, height);
            }
            // 테두리를 그리지 않음
        }
        public void ChangeColor()
        {
            fillColor = isOnOff ? offSensorColor : onColor;
        }

    }
}
