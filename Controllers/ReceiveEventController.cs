using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Waremap.Events;
using Waremap.Models;

namespace Waremap.Controllers
{
    [ApiController]
    [Route("/state")]
    public class ReceiveEventController : ControllerBase
    {
        private static readonly State State = new State();

        public static State GetState()
        {
            return State;
        }

        public static Node GetNextNode()
        {
            if (State.CarWaypoints.Count == 0) return null;
            var waypoint = State.CarWaypoints.First();
            if (State.CarWaypoints.Count < State.CarPosition) 
                waypoint = State.CarWaypoints[State.CarPosition + 1];
            return State.Geo.Nodes.FirstOrDefault(node => node.Id == waypoint.ToNode);
        }

        public static Node SwitchToNextNode()
        {
            if (State.CarWaypoints.Count == 0) return null;
            if (State.CarPosition < State.CarWaypoints.Count)
                State.CarPosition += 1;
            else
                State.CarPosition = 0;

            var waypoint = State.CarWaypoints.First();
            if (State.CarWaypoints.Count < State.CarPosition) 
                waypoint = State.CarWaypoints[State.CarPosition + 1];

            return State.Geo.Nodes.First(node => node.Id == waypoint.ToNode); 
        }

        public static Node GetCurrentNode()
        {
            if (State.CarWaypoints.Count == 0) return null;
            var carPos = State.CarWaypoints[State.CarPosition];
            return State.Geo.Nodes.FirstOrDefault(node => node.Id == carPos.FromNode);
        }

        [HttpGet]
        public string Get()
        {
            return JsonConvert.SerializeObject(State, Utils.ConverterSettings);
        }

        [HttpPost]
        public string Post()
        {
            try
            {
                if (Request.Query.ContainsKey("event"))
                {
                    var eventName = Request.Query["event"];
                    var body = "";
                    using (var reader = new StreamReader(Request.Body))
                    {
                        body = reader.ReadToEnd();
                    }

                    Console.WriteLine("\nEvent got: " + eventName + "\n" + body + "\n");

                    switch (eventName)
                    {
                        case "addNode":
                            DeserializeAndRun<EventAddNode>(body);
                            break;
                        case "removeNode":
                            DeserializeAndRun<EventRemoveNode>(body);
                            break;
                        case "addEdge":
                            DeserializeAndRun<EventAddEdge>(body);
                            break;
                        case "removeEdge":
                            DeserializeAndRun<EventRemoveEdge>(body);
                            break;
                        case "addDepot":
                            DeserializeAndRun<EventAddDepot>(body);
                            break;
                        case "removeDepot":
                            DeserializeAndRun<EventRemoveDepot>(body);
                            break;
                        case "addProcess":
                            DeserializeAndRun<EventAddProcess>(body);
                            break;
                        case "removeProcess":
                            DeserializeAndRun<EventRemoveProcess>(body);
                            break;
                        case "addOperation":
                            DeserializeAndRun<EventAddOperation>(body);
                            break;
                        case "removeOperation":
                            DeserializeAndRun<EventRemoveOperation>(body);
                            break;
                        case "computeEdges":
                            (new EventComputeEdges()).Run(State);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return JsonConvert.SerializeObject(State, Utils.ConverterSettings);
        }

        private static void DeserializeAndRun<T>(string json) where T : IEvent
        {
            if (json == "") return;
            var evt = JsonConvert.DeserializeObject<T>(json, Utils.ConverterSettings);
            evt.Run(State);
        }
    }
}