using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventRemoveWaypoint : IEvent
    {
        private int _partId;
        public int Id { get; set; }
        
        public EventRemoveWaypoint(int partId)
        {
            _partId = partId;
        }

        public void Run(State state)
        {
            var part = state.Equipment.Parts.FirstOrDefault(n => n.Id == _partId);
            if (part != null)
            {
                var waypoint = part.Path.FirstOrDefault(x => x.Id == Id);
                part.Path.Remove(waypoint);
            }
        }
    }
}