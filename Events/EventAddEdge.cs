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
}
