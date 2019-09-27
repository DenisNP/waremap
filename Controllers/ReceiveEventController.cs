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
                        if (int.TryParse(Request.Query["id"], out var id))
                        {
                            (new EventRemoveNode(id)).Run(State);
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
                        if (int.TryParse(Request.Query["from"], out var from) && int.TryParse(Request.Query["to"], out var to))
                        {
                            (new EventRemoveEdge(from, to)).Run(State);
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
                        if (int.TryParse(Request.Query["id"], out var depotId))
                        {
                            (new EventRemoveDepot(depotId)).Run(State);
                        }
                        break;
                }
            }
            return State;
        }
    }
}