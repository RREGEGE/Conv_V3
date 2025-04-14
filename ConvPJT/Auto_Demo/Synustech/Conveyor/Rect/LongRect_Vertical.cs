using System;
using System.Drawing;

namespace Synustech
{
    internal class LongRect_Vertical : CustomRectangle
    {
        private const string FIXED_TYPE = "Long_Vertical";
        private const int FIXED_WIDTH = 40;  // Interface의 너비 고정값
        private const int FIXED_HEIGHT = 80; // Interface의 높이 고정값
        private const int FIXED_SENSOR_NUM = 3;

        public LongRect_Vertical(int x, int y, int ID) : base(x, y, ID)
        {
            sensorNum = FIXED_SENSOR_NUM;
            sensor_names.Add("Foup_Detect1");
            sensor_names.Add("Foup_Detect2");
            sensor_names.Add("Foup_Detect3");
            AddSensorRectangle(sensorNum);
        }

        // Interface 클래스의 너비, 높이 및 색상을 고정
        public override string type => FIXED_TYPE;
        public override int width => FIXED_WIDTH;
        public override int height => FIXED_HEIGHT;
        //public override Color FillColor => FixedFillColor;
        public override void AddSensorRectangle(int count)
        {

            int spacing = 28; // 자식 사각형 간격

            for (int i = 0; i < count; i++)
            {
                // 자식 사각형의 위치는 부모 사각형의 좌상단을 기준으로 왼쪽에 배치
                int childX = x + borderWidth;
                int childY = y + borderWidth + (i * spacing) + 5; // 부모 사각형 아래에 자식 배치

                // 자식 사각형 생성
                var childRectangle = new SensorRect(childX, childY, sensor_names[i]);
                sensors.Add(childRectangle);
            }
        }
    }
}
