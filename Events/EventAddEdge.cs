using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventAddEdge: IEvent
    {
        public EdgeType EdgeType { get; set; }
        public int Weight { get; set; }
        public int From { get; set; }
        public int To { get; set; }

        public void Run(State state)
        {
            var edge = state.Geo.Edges.FirstOrDefault(x => x.To == To && x.From == From);
            if (edge != null)
            {
                edge.Type = EdgeType;
                edge.Weight = Weight;
                edge.To = To;
                edge.From = From;
            }
            else
                state.Geo.Edges.Add(new Edge
                {
                    Type = EdgeType,
                    Weight = Weight,
                    From = From,
                    To = To
                });
        }
    }
}
