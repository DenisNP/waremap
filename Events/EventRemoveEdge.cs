using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventRemoveEdge: IEvent
    {
        public int From { get; set; }
        public int To { get; set; }
        
        public EventRemoveEdge(int from, int to)
        {
            From = from;
            To = to;
        }

        public void Run(State state)
        {
            var edge = state.Geo.Edges.FirstOrDefault(
                x => (x.To == To && x.From == From) 
                     || (x.From == To && x.To == From)
            );
            if (edge != null)
            {
                state.Geo.Edges.Remove(edge);
            }
        }
    }
}
