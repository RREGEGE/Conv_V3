using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synustech.Conveyor
{
    partial class Conveyor
    {
        public enum TurnInitStep
        {
            Step000_POS_Check = 0,
            Step100_Foup_Check = 100,
            Step110_Foup_Both_Check = 110,

            Step200_Turn_Move = 200,
            Step210_Turn_Stop_Check = 210,

            Step500_Turn_Init_Done = 500,
        }
        public enum InitStep
        {
            Step000_Foup_Check = 0,
            Step100_Move = 100,

            Step500_Init_Done = 500,
        }
        /// <summary>
        /// Common
        /// </summary>
        public bool initComp = false;
        public InitStep preCV_InitStep;
        public InitStep CV_InitStep = InitStep.Step000_Foup_Check;

        /// <summary>
        /// Turn
        /// </summary>
        public bool turn_CV_InitComp = false;
        public TurnInitStep preTCV_InitStep;
        public TurnInitStep TCV_InitStep = TurnInitStep.Step000_POS_Check;
        public bool isInitDone = false;
    }
}
