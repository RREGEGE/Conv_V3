﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synustech
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
        public Line()
        {
        }

    }
}
