using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventRemoveOperation : IEvent
    {
        private readonly int _id;
        
        public EventRemoveOperation(int id)
        {
            _id = id;
        }

        public void Run(State state)
        {
            var operation = state.Equipment.Operations.FirstOrDefault(n => n.Id == _id);
            if (operation != null)
            {
                state.Equipment.Operations.Remove(operation);
            }
        }
    }
}