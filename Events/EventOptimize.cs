using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventOptimize : IEvent
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
        
        public void Run(State state)
        {
            var graph = new Graph(state.Geo);
            var coreIds = GraphUtils.FindCore(graph, EdgeType.Road, EdgeType.Elevator);
            if (coreIds.Count == 0)
            {
                // error!
            }
            else
            {
                Console.WriteLine("Core ids: " + JsonConvert.SerializeObject(coreIds));
                GraphUtils.AssignClosestCores(graph, coreIds.Select(n => n.Id).ToList());
                // go
                
            }
        }

        
    }
}