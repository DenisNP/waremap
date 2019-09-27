using System.Linq;
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
            var node = state.Geo.Nodes.FirstOrDefault(n => n.Id == Id);
            if (node != null)
            {
                // update node
                node.Type = Type;
                node.X = X;
                node.Y = Y;
                node.Floor = Floor;
                node.Depot = Depot;
            }
            else
            {
                // add node
                state.Geo.Nodes.Add(new Node
                {
                    Id = Utils.CreateIdFor(state.Geo.Nodes.Select(n => n.Id).ToList()),
                    Depot = Depot,
                    Floor = Floor,
                    Type = Type,
                    X = X,
                    Y = Y
                });
            }
        }
    }
}