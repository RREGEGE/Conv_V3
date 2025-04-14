using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conv_Step_Simulation
{
    public class Line
    {
        public string ID;
        public List<Conveyor> conveyors;
        public Auto auto;
        public InOutMode mode = InOutMode.InMode;
        public Stopwatch cycleStopWatch = new Stopwatch();

        public int convEA = 0;
        public int CSTEA = 0;
        public int runEA = 0;
        public int idleEA = 0;
        public int manualEA = 0;
        public int errorEA = 0;
        public int warningEA = 0;

        public Line(string id)
        {
            conveyors = new List<Conveyor>();
            ID = id;
        }
    }
}
