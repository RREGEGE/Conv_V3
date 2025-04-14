using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synustech
{
    public class Line
    {
        public string ID;
        public List<Conveyor> conveyors;
        public Auto auto;
        public InOutMode inoutmode = InOutMode.InMode;
        public Stopwatch cycleStopWatch = new Stopwatch();

        public int startConvID;
        public int endConvID;

        public int convEA = 0;
        public int CSTEA = 0;
        public int runEA = 0;
        public int idleEA = 0;
        public int manualEA = 0;
        public int errorEA = 0;
        public int warningEA = 0;

        public DateTime? StartTime;

        public Line(string id)
        {
            conveyors = new List<Conveyor>();
            ID = id;
        }
        public bool IdleCheck()
        {
            foreach (var conv in conveyors)
            {
                if (conv.run == Conveyor.CnvRun.Run)
                {
                    return false;
                }
            }
            return true;
        }
        public void ChangeMode(Conveyor.LineDirection direction)
        {
            if(direction == Conveyor.LineDirection.Output)
            {
                foreach(var conv in conveyors)
                {
                    conv.portDirection = Conveyor.LineDirection.Input;
                }
                inoutmode = InOutMode.InMode;
            }
            else
            {
                foreach(var conv in conveyors)
                {
                    conv.portDirection = Conveyor.LineDirection.Output;
                }
                inoutmode = InOutMode.OutMode;
            }
            foreach(var conveyor in conveyors)
            {
                Console.WriteLine(conveyor.ID + ": " + conveyor.portDirection);
            }
        }
        public void ChangeStep()
        {
            foreach(var conv in conveyors)
            {
                conv.CV_AutoStep = Conveyor.AutoStep.Step000_Check_Direction;
                Console.WriteLine(conv.ID+": " + conv.CV_AutoStep);
            }
        }
        public void StatusCheck()
        {

            int convEA = 0;
            int runEA = 0;
            int idleEA = 0;
            int manualEA = 0;
            int errorEA = 0;
            foreach (Conveyor conveyor in conveyors)
            {
                if (conveyor != null)
                {
                    convEA++;
                    if ((conveyor.run == Conveyor.CnvRun.Run) && (conveyor.servo == ServoOnOff.On))
                    {
                        runEA++;
                    }
                    else if ((conveyor.run == Conveyor.CnvRun.Stop) && (conveyor.servo == ServoOnOff.On))
                    {
                        idleEA++;
                    }
                    else if ((conveyor.servo == ServoOnOff.On) && (conveyor.mode == Mode.Manual))
                    {
                        manualEA++;
                    }
                    else if ((conveyor.servo == ServoOnOff.On) && (conveyor.mode == Mode.Alarm))
                    {
                        errorEA++;
                    }
                }
            }
            this.convEA = convEA;
            this.runEA = runEA;
            this.idleEA = idleEA;
            this.manualEA = manualEA;
            this.errorEA = errorEA;
        }
    }
}
