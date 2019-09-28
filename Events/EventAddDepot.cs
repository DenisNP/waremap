using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    #nullable enable
    public class EventAddDepot: IEvent
    {
        public int? Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public int Floor { get; set; }
        public string? Name { get; set; }

        public void Run(State state)
        {
            var depot = state.Geo.Depots.FirstOrDefault(x => x.X == X && x.Y == Y);
            if (depot != null)
            {
                depot.X = X;
                depot.Y = Y;
                depot.W = W;
                depot.H = H;
                depot.Floor = Floor;
                depot.Name = Name ?? depot.Name;
            }
            else
            {
                var id = Utils.CreateIdFor(state.Geo.Depots.Select(d => d.Id).ToList());

                state.Geo.Depots.Add(new Depot
                {
                    Id = id,
                    X = X,
                    Y = Y,
                    W = W,
                    H = H,
                    Floor = Floor,
                    Name = Name ?? $"Цех {id}, Этаж {Floor}"
                });
            }
            
            RedefineDepots(state);
        }

        public static void RedefineDepots(State state)
        {
            foreach (var node in state.Geo.Nodes)
            {
                node.Depot = 0;
                foreach (var depot in state.Geo.Depots)
                {
                    if (
                        node.X >= depot.X
                        && node.X <= depot.X + depot.W
                        && node.Y >= depot.Y
                        && node.Y <= depot.Y + depot.H
                        && depot.Floor == node.Floor
                    )
                    {
                        node.Depot = depot.Id;
                        break;
                    }
                }
            }
        }
    }
    #nullable disable
}
