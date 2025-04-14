using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Synustech
{
    internal class UnitConverter
    {
        public static double InvertumTomm(double speed)
        {
            double value = 0;
            value = speed / 1000;
            return value;
        }
        public static double InvertmmToum(double speed)
        {
            double value = 0;
            value = speed * 1000;
            return value;
        }
        public static double InvertmmTospeed(double speed)
        {
            double value = 0;
            value = speed / 8;
            return value;
        }
        public static double InvertspeedTomm(double speed)
        {
            double value = 0;
            value = speed * 8;
            return value;
        }
        public static double InvertumTodegree(double speed)
        {
            double value = 0;
            value = speed * 0.036;
            return value;
        }
        public static double InvertdegreeToum(double speed)
        {
            double value = 0;
            value = speed / 0.036;
            return value;
        }
    }
}
