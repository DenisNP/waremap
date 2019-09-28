using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventRemoveWaypoint : IEvent
    {
        public int PartId { get; set; }
        public int Id { get; set; }
        
        public void Run(State state)
        {
            var part = state.Equipment.Parts.FirstOrDefault(n => n.Id == PartId);
            if (part != null)
            {
                var waypoint = part.Path.FirstOrDefault(x => x.Id == Id);
                part.Path.Remove(waypoint);
            }
        }
    }
}