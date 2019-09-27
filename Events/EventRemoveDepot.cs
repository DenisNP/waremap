using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventRemoveDepot: IEvent
    {
        private readonly int _id;

        public EventRemoveDepot(int id)
        {
            _id = id;
        }

        public void Run(State state)
        {
            var edge = state.Geo.Edges.FirstOrDefault(
                x => (x.To == _id) 
            );
            if (edge != null)
            {
                state.Geo.Edges.Remove(edge);
            }
        }
    }
}
