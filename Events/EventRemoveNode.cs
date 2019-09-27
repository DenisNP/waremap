using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventRemoveNode : IEvent
    {
        public int Id { get; set; }

        public void Run(State state)
        {
            var node = state.Geo.Nodes.FirstOrDefault(n => n.Id == Id);
            if (node != null)
            {
                state.Geo.Nodes.Remove(node);
                var edges = state.Geo.Edges.Where(e => e.From == Id || e.To == Id).ToList();
                edges.ForEach(e => state.Geo.Edges.Remove(e));
            }
        }
    }
}