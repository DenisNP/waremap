using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventRemoveDepot: IEvent
    {
        public int Id { get; set; }

        public void Run(State state)
        {
            var depot = state.Geo.Depots.FirstOrDefault(
                x => (x.Id == Id) 
            );
            if (depot != null)
            {
                state.Geo.Depots.Remove(depot);
            }
        }
    }
}
