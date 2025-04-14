using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Interface.Math
{
    /// <summary>
    /// 실수 비교를 위한 클래스
    /// </summary>
    static class Compare
    {
        public static bool Equal(double A, double B, uint point = 3)
        {
            if (point < 1)
                point = 1;
            // Define the tolerance for variation in their values
            double difference = System.Math.Abs(1.0 / (double)(System.Math.Pow(10, point)));

            // Compare the values
            // The output to the console indicates that the two values are equal
            if (System.Math.Abs(A - B) <= difference)
                return true;
            else
                return false;
        }
        public static bool Equal(float A, float B, uint point = 3)
        {
            if (point < 1)
                point = 1;

            // Define the tolerance for variation in their values
            double difference = System.Math.Abs(1.0 / (double)(System.Math.Pow(10, point)));

            // Compare the values
            // The output to the console indicates that the two values are equal
            if (System.Math.Abs(A - B) <= difference)
                return true;
            else
                return false;
        }
    }
}
