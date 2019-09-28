using System;
using System.Linq;
using Newtonsoft.Json;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventOptimize : IEvent
    {
        private Route _route;
        private AntColony _colony;
        
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
                _colony = new AntColony(graph);
                var bestRoute = new Route(graph, state);
                var initialResult = bestRoute.Result;
                var k = 1000;
                while (bestRoute.Result > initialResult / 2.0 && k-- > 0)
                {
                    _route = new Route(graph, state);
                    _colony.RunAnt(_route);
                    if (bestRoute.Result > _route.Result)
                    {
                        bestRoute = _route;
                    }
                }
            }
        }

        
    }
}