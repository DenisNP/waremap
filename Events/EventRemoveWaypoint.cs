using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventRemoveWaypoint : IEvent
    {
        private readonly int _partId;
        private readonly int _id;
        
        public EventRemoveWaypoint(int partId, int id)
        {
            _partId = partId;
            _id = id;
        }

        public void Run(State state)
        {
            var part = state.Equipment.Parts.FirstOrDefault(n => n.Id == _partId);
            if (part != null)
            {
                var waypoint = part.Path.FirstOrDefault(x => x.Id == _id);
                part.Path.Remove(waypoint);
            }
        }
    }
}