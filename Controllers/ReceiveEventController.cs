using System.IO;
using Microsoft.AspNetCore.Mvc;
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
                
                switch (eventName)
                {
                    case "addNode":
                        if (body != "")
                        {
                            
                        }
                        break;
                }
            }
            return State;
        }
    }
}