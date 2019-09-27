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
                    case "addEdge":
                        if (body != "")
                        {
                            var eventAddEdge = JsonConvert.DeserializeObject<EventAddEdge>(body);
                            eventAddEdge.Run(State);
                        }
                        break;
                    case "removeNode":
                        if (body != "")
                        {
                            var eventAddNode = JsonConvert.DeserializeObject<EventRemoveNode>(body);
                            eventAddNode.Run(State);
                        }
                        break;
                    case "removeEdge":
                        if (body != "")
                        {
                            var eventAddEdge = JsonConvert.DeserializeObject<EventRemoveEdge>(body);
                            eventAddEdge.Run(State);
                        }
                        break;
                }
            }
            return State;
        }
    }
}