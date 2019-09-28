using System.Collections.Generic;

namespace Waremap.Models
{
    public class Route
    {
        public Graph Graph;
        public Dictionary<int, ProcessPosition> PartPositions = new Dictionary<int, ProcessPosition>();
        public Dictionary<int, List<Operation>> OperationsLeft = new Dictionary<int, List<Operation>>();
            
        public int Time = 0;
    }
        
    public class ProcessPosition
    {
        public int ToNode;
        public int EndTime;
    }
}