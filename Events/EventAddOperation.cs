using System.Collections.Generic;
using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventAddOperation : IEvent
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int ProcessingTime { get; set; }
        
        public List<int>? OperationIds { get; set; }
        public void Run(State state)
        {
            var ops= state.Equipment.Operations.FirstOrDefault(n => n.Id == Id);
            if (ops != null)
            {
                // update node
                ops.Name = Name;
                ops.ProcessingTime = ProcessingTime;
            }
            else
            {
                // add node
                state.Equipment.Operations.Add(new Operation
                {
                    Name = Name,
                    ProcessingTime = ProcessingTime,
                    Id = Id
                });
            }
        }
    }
}