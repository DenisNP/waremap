using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventComputeEdges : IEvent
    {
        public void Run(State state)
        {
            var floors = state.Geo.Nodes.Select(n => n.Floor).Distinct();
            foreach (var floor in floors)
            {
                var floorNodes = state.Geo.Nodes.Where(n => n.Floor == floor).ToList();
                var depots = floorNodes.Select(n => n.Depot).Distinct();
                foreach (var depot in depots)
                {
                    var nodes = floorNodes.Where(n => n.Depot == depot).ToList();
                    for (var i = 0; i < nodes.Count; i++)
                    {
                        var firstNode = nodes[i];
                        for (var k = i + 1; k < nodes.Count; k++)
                        {
                            var secondNode = nodes[k];
                            if (firstNode.Type == NodeType.Machine && secondNode.Type == NodeType.Machine) continue;
                            CheckAddEdge(firstNode, secondNode, state);
                        }
                    }
                }
            }
        }

        private static void CheckAddEdge(Node firstNode, Node secondNode, State state)
        {
            var edgeExists = state.Geo.Edges.Exists(e =>
                    e.From == firstNode.Id && e.To == secondNode.Id
                    || e.To == firstNode.Id && e.From == secondNode.Id
            );

            if (!edgeExists)
            {
                state.Geo.Edges.Add(new Edge
                {
                    From = firstNode.Id,
                    To = secondNode.Id,
                    Type = firstNode.Depot == 0 ? EdgeType.Road : EdgeType.Footway,
                    Weight = Utils.Dist(firstNode, secondNode)
                });
            }
        }
    }
}