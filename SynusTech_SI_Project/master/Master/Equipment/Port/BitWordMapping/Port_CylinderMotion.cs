using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Equipment.Port
{
    partial class Port
    {
        bool bCylinderForward = false;
        bool bCylinderBackward = false;

        public void CylinderMotion_FWD_Move()
        {
            bCylinderForward = true;
        }
        public void CylinderMotion_FWD_Stop()
        {
            bCylinderForward = false;
        }
        public void CylinderMotion_BWD_Move()
        {
            bCylinderBackward = true;
        }
        public void CylinderMotion_BWD_Stop()
        {
            bCylinderBackward = false;
        }
    }
}
