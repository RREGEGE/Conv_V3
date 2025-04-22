using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchdog_Test
{
    public partial class Line
    {
        public enum LineDirection
        {
            Input,
            Output
        }
        public enum Auto
        {
            Enable,
            Disable
        }
        public MotionParam.LineParam m_LineParameter;
        public Line(string id)
        {
            m_LineParameter = new MotionParam.LineParam(id);
        }
        public MotionParam.LineParam GetMotionParam()
        {
            return m_LineParameter;
        }

    }
}
