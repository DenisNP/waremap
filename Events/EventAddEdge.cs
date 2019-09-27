using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventAddEdge: IEvent
    {
        private EdgeType _type;
        private int _weight;
        private int _from;
        private int _to;

        public void Run(State state)
        {
            var edge = state.Geo.Edges.FirstOrDefault(x => x.To == _to && x.From == _from);
            if (edge != null)
            {
                edge.Type = _type;
                edge.Weight = _weight;
                edge.To = _to;
                edge.From = _from;
            }
            else
                state.Geo.Edges.Add(new Edge
                {
                    Type = _type,
                    Weight = _weight,
                    From = _from,
                    To = _to
                });
        }
    }
}
