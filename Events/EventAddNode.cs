using Waremap.Models;

namespace Waremap.Events
{
    public class EventAddNode : IEvent
    {
        public int? Id { get; set; }
        public NodeType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Floor { get; set; }
        public int Depot { get; set; }
        
        public void Run(State state)
        {
            throw new System.NotImplementedException();
        }
    }
}