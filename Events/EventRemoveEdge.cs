using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventRemoveEdge: IEvent
    {
        private int _from;
        private int _to;

        public EventRemoveEdge(int from, int to)
        {
            _from = from;
            _to = to;
        }

        public void Run(State state)
        {
            var edge = state.Geo.Edges.FirstOrDefault(x => x.To == _to && x.From == _from);
            if (edge != null)
            {
                state.Geo.Edges.Remove(edge);
            }
        }
    }
}
