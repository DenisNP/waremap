using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventAddOperation : IEvent
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int ProcessingTime { get; set; }
        
        public void Run(State state)
        {
            var operation = state.Equipment.Operations.FirstOrDefault(n => n.Id == Id);
            if (operation != null)
            {
                // update node
                operation.Name = Name;
                operation.ProcessingTime = ProcessingTime;
            }
            else
            {
                // add node
                state.Equipment.Operations.Add(new Operation
                {
                    Name = Name,
                    ProcessingTime = ProcessingTime,
                    Id = Utils.CreateIdFor(state.Equipment.Operations.Select(o => o.Id).ToList())
                });
            }
        }
    }
}