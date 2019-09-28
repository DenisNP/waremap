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
                        case "addWaypoint":
                            DeserializeAndRun<EventAddWaypoint>(body);
                            break;
                        case "removeWaypoint":
                            DeserializeAndRun<EventRemoveWaypoint>(body);
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

            return State;
        }

        private static void DeserializeAndRun<T>(string json) where T : IEvent
        {
            if (json == "") return;
            var evt = JsonConvert.DeserializeObject<T>(json, Utils.ConverterSettings);
            evt.Run(State);
        }
    }
}