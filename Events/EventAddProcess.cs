using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    #nullable enable
    public class EventAddProcess: IEvent
    {
        public int? Id { get; set; }
        public int PartId { get; set; }
        public int OperationId { get; set; }
        public int Order { get; set; }

        public void Run(State state)
        {
            var part = state.Equipment.Parts.FirstOrDefault(x => x.Id == PartId);
            if (part == null) return;

            var process = part.Process.FirstOrDefault(x => x.Id == Id);
            if (process != null)
            {
                process.OperationId = OperationId;
                process.Order = Order;
            }
            else
            {
                part.Process.Add(new Process
                {
                    Id = Utils.CreateIdFor(part.Process.Select(p => p.Id).ToList()),
                    OperationId = OperationId,
                    Order = Order,
                });
            }
        }
    }
    #nullable disable
}
