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

                foreach (var node in state.Geo.Nodes)
                {
                    if (node.X > depot.X && node.X < depot.X + depot.W 
                                         && node.Y > depot.Y && node.Y < depot.Y + depot.H 
                                         && node.Floor == depot.Floor)
                    {
                        node.Depot = 0;
                    }
                }
            }
        }
    }
}
