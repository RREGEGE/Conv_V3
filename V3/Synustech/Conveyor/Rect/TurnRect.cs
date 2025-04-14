using System;
using System.Drawing;

namespace Synustech
{
    internal class TurnRect : CustomRectangle
    {
        private const string FixedType = "Turn";
        private const int FixedWidth = 40;  // Interface의 너비 고정값
        private const int FixedHeight = 40; // Interface의 높이 고정값
        private const int SensorNum = 2;

        public TurnRect(int x, int y, int ID) : base(x, y, ID)
        {
            sensornum = SensorNum;
            Sensor_names.Add("Foup_Detect1");
            Sensor_names.Add("Foup_Detect2");
            AddSensorRectangle(sensornum);
        }

        // Interface 클래스의 너비, 높이 및 색상을 고정
        public override string TYPE => FixedType;
        public override int Width => FixedWidth;
        public override int Height => FixedHeight;
        //public override Color FillColor => FixedFillColor;

        public override void AddSensorRectangle(int count)
        {

            int spacing = 15; // 자식 사각형 간격

            for (int i = 0; i < count; i++)
            {
                // 자식 사각형의 위치는 부모 사각형의 좌상단을 기준으로 왼쪽에 배치
                int childX = X + BorderWidth;
                int childY = Y + BorderWidth + (i * spacing) + 5; // 부모 사각형 아래에 자식 배치

                // 자식 사각형 생성

                var sensorRectangle = new SensorRect(childX, childY, Sensor_names[i]);
                Sensor.Add(sensorRectangle);
            }
        }
    }
}
