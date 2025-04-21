using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synustech
{
    class LineParam
    {
        public string ID                            = null;
        public Line.LineDirection elineDirection    = Line.LineDirection.Input;
        public Line.Auto eAuto                      = Line.Auto.Disable;
        public List<Conveyor> conveyors             = new List<Conveyor>();
        public LineParam(string id)
        { 
            this.ID = id;
        }
        public void AddConveyor(Conveyor conveyor)
        {
            conveyors.Add(conveyor);
        }
        public List<int> GetAllConveyorID()
        {
            List<int> list = new List<int>();
            foreach(var conveyor in conveyors)
            {
                list.Add(conveyor.ID);
            }
            return list;
        }
    }
}
