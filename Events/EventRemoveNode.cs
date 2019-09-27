using System.Linq;
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
            var node = state.Geo.Nodes.FirstOrDefault(n => n.Id == _id);
            if (node != null)
            {
                state.Geo.Nodes.Remove(node);
                var edges = state.Geo.Edges.Where(e => e.From == _id || e.To == _id).ToList();
                edges.ForEach(e => state.Geo.Edges.Remove(e));
            }
        }
    }
}