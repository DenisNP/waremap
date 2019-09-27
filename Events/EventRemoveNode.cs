using Waremap.Models;

namespace Waremap.Events
{
    public class EventRemoveNode : IEvent
    {
        private int _id;
        
        public EventRemoveNode(int id)
        {
            _id = id;
        }

        public void Run(State state)
        {
            throw new System.NotImplementedException();
        }
    }
}