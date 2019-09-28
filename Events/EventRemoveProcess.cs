using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventRemoveProcess : IEvent
    {
        public int PartId { get; set; }
        public int Id { get; set; }
        
        public void Run(State state)
        {
            var part = state.Equipment.Parts.FirstOrDefault(n => n.Id == PartId);
            var process = part?.Path.FirstOrDefault(x => x.Id == Id);
            if (process != null) part.Path.Remove(process);
        }
    }
}