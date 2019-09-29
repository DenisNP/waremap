using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventAddEdge: IEvent
    {
        public EdgeType Type { get; set; }
        public int Weight { get; set; }
        public int From { get; set; }
        public int To { get; set; }

        public void Run(State state)
        {
            var edge = state.Geo.Edges.FirstOrDefault(x => x.To == To && x.From == From);
            if (edge != null)
            {
                edge.Type = Type;
                edge.Weight = Weight;
                edge.To = To;
                edge.From = From;
            }
            else
            {
                if (state.Geo.Nodes.Count(n => n.Id == From || n.Id == To) == 2)
                {
                    state.Geo.Edges.Add(new Edge
                    {
                        Type = Type,
                        Weight = Weight,
                        From = From,
                        To = To
                    });
                }
            }
        }

        public static void RecalculateWeights(Node node, State state)
        {
            state.Geo.Edges.ForEach(e =>
            {
                if (node == null || node.Id == e.From || node.Id == e.To)
                {
                    var fromNode = state.Geo.Nodes.FirstOrDefault(n => n.Id == e.From);
                    var toNode = state.Geo.Nodes.FirstOrDefault(n => n.Id == e.To);
                    if (fromNode != null && toNode != null)
                    {
                        e.Weight = Utils.Dist(fromNode, toNode);
                    }
                }
            });
        }
    }
}
