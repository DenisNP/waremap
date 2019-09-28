using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Waremap.Models;

namespace Waremap.Controllers
{
    [ApiController]
    [Route("/position")]
    public class PositionController : Controller
    {
        public struct TextWaypoint
        {
            public string NodeName { get; set; }
            public int NodeId { get; set; }
        }

        public struct PositionResult
        {
            public TextWaypoint Current { get; set; }
            public TextWaypoint Next { get; set; }
            public bool OffWay { get; set; }
        }

        public struct Presence
        {
            public int PartId { get; set; }
            public int MachineId { get; set; }
        }

        [HttpGet]
        public string GetPosition()
        {  
            return JsonConvert.SerializeObject(Position(), Utils.ConverterSettings);
        }

        [HttpPost]
        public string PostPosition([FromBody] Presence presence)
        {
            var state = ReceiveEventController.GetState();
            var part = state.Equipment.Parts.FirstOrDefault(x => x.Id == presence.PartId);
            var machineNode = state.Geo.Nodes
                .FirstOrDefault(x => x.Id == presence.MachineId);

            if (machineNode != null && machineNode.Type == NodeType.Machine)
            {
                foreach (var wp in state.CarWaypoints)
                {
                    if (wp.FromNode == presence.MachineId && wp.ToNode == presence.MachineId)
                    {
                        state.CarPosition = state.CarWaypoints.IndexOf(wp);
                        if (part != null) part.Waypoint = wp;

                        return GetPosition();
                    }

                }

                state.CarWaypoints.Insert(state.CarPosition + 1, part.Waypoint);

                return JsonConvert.SerializeObject(Position(true),Utils.ConverterSettings);
            }


            return "No machine found";
        }

        public PositionResult Position(bool offWay = false)
        {
            var curNode = ReceiveEventController.GetCurrentNode();
            var nextNode = ReceiveEventController.GetNextNode();

            return new PositionResult
            {
                Current = new TextWaypoint {NodeName = curNode.Name, NodeId = curNode.Id},
                Next = new TextWaypoint {NodeName = nextNode.Name, NodeId = nextNode.Id},
                OffWay = offWay
            };
        }
    }
}