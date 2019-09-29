using System;
using System.Linq;
using Newtonsoft.Json;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventOptimize : IEvent
    {
        public void Run(State state)
        {
            var graph = new Graph(state.Geo);
            var coreIds = GraphUtils.FindCore(graph, EdgeType.Road, EdgeType.Elevator);
            if (coreIds.Count == 0)
            {
                // error!
                Console.WriteLine("No core");
            }
            else
            {
                Console.WriteLine("Core ids: " + JsonConvert.SerializeObject(coreIds.Select(n => n.Id)));
                coreIds.ForEach(cNode => cNode.AssignIsCore(true));
                GraphUtils.AssignClosestCores(graph, coreIds.Select(n => n.Id).ToList());

                state.CarRoadmap.LeaveFirst();
                state.Equipment.Parts.ForEach(p => p.Roadmap.LeaveFirst());
                
                // go
                var ann = new SimulatedAnnealing(graph, state);
                var result = ann.Run();
                Console.WriteLine(result.Item1);
                Console.WriteLine(JsonConvert.SerializeObject(result.Item2));
            }
        }

        
    }
}