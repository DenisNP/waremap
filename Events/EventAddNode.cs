using Waremap.Models;

namespace Waremap.Events
{
    public class EventAddNode : IEvent
    {
        public int? Id;
        
        public void Run(State state)
        {
            throw new System.NotImplementedException();
        }
    }
}