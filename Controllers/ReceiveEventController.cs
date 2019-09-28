using System;
using System.IO;
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
        
        [HttpGet]
        public State Get()
        {
            return State;
        }

        [HttpPost]
        public State Post()
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
                            if (body != "")
                            {
                                var eventAddNode = JsonConvert.DeserializeObject<EventAddNode>(body);
                                eventAddNode.Run(State);
                            }
                            break;
                        case "removeNode":
                            if (body != "")
                            {
                                var eventRemoveNode = JsonConvert.DeserializeObject<EventRemoveNode>(body);
                                eventRemoveNode.Run(State);
                            }
                            break;
                        case "addEdge":
                            if (body != "")
                            {
                                var eventAddEdge = JsonConvert.DeserializeObject<EventAddEdge>(body);
                                eventAddEdge.Run(State);
                            }
                            break;
                        case "removeEdge":
                            if (body != "")
                            {
                                var eventRemoveEdge = JsonConvert.DeserializeObject<EventRemoveEdge>(body);
                                eventRemoveEdge.Run(State);
                            }
                            break;
                        case "addDepot":
                            if (body != "")
                            {
                                var eventAddEdge = JsonConvert.DeserializeObject<EventAddDepot>(body);
                                eventAddEdge.Run(State);
                            }
                            break;
                        case "removeDepot":
                            if (body != "")
                            {
                                var eventRemoveDepot = JsonConvert.DeserializeObject<EventRemoveDepot>(body);
                                eventRemoveDepot.Run(State);
                            }
                            break;
                        case "addWaypoint":
                            if (body != "")
                            {
                                var eventAddWaypoint = JsonConvert.DeserializeObject<EventAddWaypoint>(body);
                                eventAddWaypoint.Run(State);
                            }
                            break;
                        case "removeWaypoint":
                            if (body != "")
                            {
                                var eventRemoveWaypoint = JsonConvert.DeserializeObject<EventRemoveWaypoint>(body);
                                eventRemoveWaypoint.Run(State);
                            }
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

            return State;
        }
    }
}