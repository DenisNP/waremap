using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    #nullable enable
    public class EventAddDepot: IEvent
    {
        public int Id { get; set; }
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
                depot.Name = Name;

                foreach (var node in state.Geo.Nodes)
                {
                    if (node.X > X && node.X < X + W && node.Y > Y && node.Y < Y + H && node.Floor == Floor)
                    {
                        node.Depot = Id;
                    }
                    else
                    {
                        node.Depot = 0;
                    }
                }
            }
            else
            {
                state.Geo.Depots.Add(new Depot
                {
                    Id = Utils.CreateIdFor(state.Geo.Depots.Select(d => d.Id).ToList()),
                    X = X,
                    Y = Y,
                    W = W,
                    H = H,
                    Floor = Floor,
                    Name = $"{Name} №{Id}, Этаж {Floor}"
                });

                foreach (var node in state.Geo.Nodes)
                {
                    if (node.X > X && node.X < X + W 
                                   && node.Y > Y && node.Y < Y + H 
                                   && node.Floor == Floor)
                    {
                        node.Depot = Id;
                    }
                    else
                    {
                        node.Depot = 0;
                    }
                }
            }
        }
    }
    #nullable disable
}
