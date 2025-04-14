using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Synustech
{
    internal class MathCalculator
    {
        public static double umTomm(double speed)
        {
            double value = 0;
            value = speed / 1000;
            return value;
        }
        public static double mmToum(double speed)
        {
            double value = 0;
            value = speed * 1000;
            return value;
        }
        public static double umTodegree(double speed)
        {
            double value = 0;
            value = speed * 0.00036;
            return value;
        }
        public static double degreeToum(double speed)
        {
            double value = 0;
            value = speed / 0.00036;
            return value;
        }
    }
}
