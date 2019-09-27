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
            var depot = state.Geo.Depots.FirstOrDefault(
                x => (x.Id == _id) 
            );
            if (depot != null)
            {
                state.Geo.Depots.Remove(depot);
            }
        }
    }
}
