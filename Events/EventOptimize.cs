using System;
using System.Linq;
using MoreLinq;
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
                Console.WriteLine("No core");
            }
            else
            {
                Console.WriteLine("Core ids: " + JsonConvert.SerializeObject(coreIds.Select(n => n.Id)));
                GraphUtils.AssignClosestCores(graph, coreIds.Select(n => n.Id).ToList());
                
                state.CarRoadmap.Path.RemoveRange(1, state.CarRoadmap.Path.Count - 1);
                state.CarRoadmap.Position = 0;
                state.Equipment.Parts.ForEach(p =>
                {
                    p.Roadmap.Position = 0;
                    p.Roadmap.Path.RemoveRange(1, p.Roadmap.Path.Count - 1);
                });
                
                // go
                _colony = new AntColony(graph);
                Route bestRoute = null;
                var k = 1000;
                while (bestRoute == null || k-- > 0)
                {
                    _route = new Route(graph, state);
                    _colony.RunAnt(_route);
                    if (bestRoute == null || bestRoute.Result > _route.Result)
                    {
                        bestRoute = _route;
                    }

                    Console.WriteLine(_route.Result);
                }
                
                Console.WriteLine(JsonConvert.SerializeObject(bestRoute.CarWaypoints));
            }
        }

        
    }
}