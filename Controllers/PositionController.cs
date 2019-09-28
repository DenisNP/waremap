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
    public class PositionController : ControllerBase
    {
        public struct WaypointInfo
        {
            public string NodeName { get; set; }
            public int NodeId { get; set; }
        }

        public struct PositionResult
        {
            public WaypointInfo Current { get; set; }
            public WaypointInfo Next { get; set; }
            public bool OffWay { get; set; }
        }

        public struct Presence
        {
            public int PartId { get; set; }
            public int MachineId { get; set; }
            public int OperationId { get; set; }
        }

        [HttpGet]
        public string GetPosition()
        {
            try
            {
                return JsonConvert.SerializeObject(Position(), Utils.ConverterSettings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public string PostPosition([FromBody] Presence presence)
        {
            try
            {
                var state = ReceiveEventController.GetState();
                var part = state.Equipment.Parts.FirstOrDefault(x => x.Id == presence.PartId);
                var machineNode = state.Geo.Nodes.FirstOrDefault(x => x.Id == presence.MachineId);

                if (machineNode == null || machineNode.Type != NodeType.Machine || part == null)
                    return "No machine or part found";

                var potentialWaypoint = state.CarWaypoints
                    .FirstOrDefault(
                        wp => wp.FromNode == presence.MachineId
                              && wp.ToNode == presence.MachineId
                              && part.Path.Select(process => process.Id).Contains(presence.OperationId)
                    );

                if (potentialWaypoint != null)
                {
                    state.CarPosition = state.CarWaypoints.IndexOf(potentialWaypoint);
                    part.Waypoint = potentialWaypoint;
                    return GetPosition();
                }

                // new waypoint
                part.Waypoint = new Waypoint
                {
                    FromNode = presence.MachineId,
                    ToNode = presence.MachineId,
                    OperationId = presence.OperationId,
                    OffWay = true
                };
                state.CarWaypoints.Insert(state.CarPosition + 1, part.Waypoint);
                return JsonConvert.SerializeObject(Position(true), Utils.ConverterSettings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public PositionResult Position(bool offWay = false)
        {
            var curNode = ReceiveEventController.GetCurrentNode();
            var nextNode = ReceiveEventController.GetNextNode();

            if (curNode == null || nextNode == null)
            {
                return new PositionResult
                {
                    Current = new WaypointInfo {NodeName = "Текущий участок", NodeId = 1},
                    Next = new WaypointInfo {NodeName = "Следующий участок", NodeId = 2}
                };
            }

            return new PositionResult
            {
                Current = new WaypointInfo {NodeName = curNode.Name, NodeId = curNode.Id},
                Next = new WaypointInfo {NodeName = nextNode.Name, NodeId = nextNode.Id},
                OffWay = offWay
            };
        }
    }
}