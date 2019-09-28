using System.Collections.Generic;
using System.Linq;

namespace Waremap.Models
{
    public class Route
    {
        public Graph Graph;
        public readonly Dictionary<int, ProcessPosition> PartPositions = new Dictionary<int, ProcessPosition>();
        public readonly Dictionary<int, List<Process>> OperationsLeft = new Dictionary<int, List<Process>>();
        public readonly Dictionary<int, List<Waypoint>> PartWaypoints = new Dictionary<int, List<Waypoint>>();
        public List<Waypoint> CarWaypoints = new List<Waypoint>();
        public int Time = 0;
        public double Result = 0;

        public Route(Graph graph, State state)
        {
            Graph = graph;
            foreach (var part in state.Equipment.Parts)
            {
                PartPositions.Add(part.Id, new ProcessPosition
                {
                    ToNode = part.Roadmap.CurrentWaypoint().FromNode,
                    EndTime = 0
                });
                
                PartWaypoints.Add(part.Id, new List<Waypoint>
                {
                    part.Roadmap.CurrentWaypoint()
                });
                
                OperationsLeft.Add(part.Id, part.Process.OrderBy(p => p.Order).ToList());
            }
            
            CarWaypoints.Add(state.CarRoadmap.CurrentWaypoint());
        }
    }
        
    public class ProcessPosition
    {
        public int ToNode;
        public int EndTime;
    }
}