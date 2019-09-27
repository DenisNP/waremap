using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waremap.Models;

namespace Waremap.Events
{
    public class EventAddWaypoint: IEvent
    {
        private int _partId;

        public int Id { get; set; }
        public int Nodeid { get; set; }
        public int ProcessingTime { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }

        public EventAddWaypoint(int partId)
        {
            _partId = partId;
        }

        public void Run(State state)
        {
            var part = state.Equipment.Parts.FirstOrDefault(x => x.Id == _partId);
            if (part != null)
            {
                var waypoint = part.Path.FirstOrDefault(x => x.Id == Id);

                waypoint.Id = Id;
                waypoint.NodeId = Nodeid;
                waypoint.ProcessingTime = ProcessingTime;
                waypoint.StarTime = StartTime;
                waypoint.EndTime = EndTime;

            }
            else
                part.Path.Add(new Waypoint
                {
                    Id = Id,
                    NodeId = Nodeid,
                    EndTime = EndTime,
                    ProcessingTime = ProcessingTime,
                    StarTime = StartTime
                });
        }
    }
}
