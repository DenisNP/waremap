using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventRemoveEdge: IEvent
    {
        private readonly int _from;
        private readonly int _to;

        public EventRemoveEdge(int from, int to)
        {
            _from = from;
            _to = to;
        }

        public void Run(State state)
        {
            var edge = state.Geo.Edges.FirstOrDefault(
                x => (x.To == _to && x.From == _from) 
                     || (x.From == _to && x.To == _from)
            );
            if (edge != null)
            {
                state.Geo.Edges.Remove(edge);
            }
        }
    }
}
