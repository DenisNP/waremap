using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventAddDepot: IEvent
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public int Floor { get; set; }

        public void Run(State state)
        {
            var depot = state.Geo.Depots.FirstOrDefault(x => x.X == X && x.Y == Y);
            if (depot != null)
            {
                depot.Id = Id;
                depot.X = X;
                depot.Y = Y;
                depot.W = W;
                depot.H = H;
                depot.Floor = Floor;
            }
            else
                state.Geo.Depots.Add(new Depot
                {
                    Id = Id,
                    X = X,
                    Y = Y,
                    W = W,
                    H = H,
                    Floor = Floor
        });
        }
    }
}
