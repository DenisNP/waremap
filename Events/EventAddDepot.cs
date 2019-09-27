using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventAddDepot: IEvent
    {
        private int _id;
        private int _x;
        private int _y;
        private int _w;
        private int _h;
        private int _floor;

        public void Run(State state)
        {
            var depot = state.Geo.Depots.FirstOrDefault(x => x.X == _x && x.Y == _y);
            if (depot != null)
            {
                depot.Id = _id;
                depot.X = _x;
                depot.Y = _y;
                depot.W = _w;
                depot.H = _h;
                depot.Floor = _floor;
            }
            else
                state.Geo.Depots.Add(new Depot
                {
                    Id = _id,
                    X = _x,
                    Y = _y,
                    W = _w,
                    H = _h,
                    Floor = _floor
        });
        }
    }
}
