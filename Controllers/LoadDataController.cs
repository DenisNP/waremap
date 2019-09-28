using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Waremap.Models;

namespace Waremap.Controllers
{
    [ApiController]
    [Route("/load")]
    public class LoadDataController : ControllerBase
    {
        [HttpPost]
        public string Load()
        {
            if (!Request.Query.ContainsKey("data"))
            {
                return "no data field";
            }
            
            var data = Request.Query["data"];
            try
            {
                using var reader = new StreamReader(Request.Body);
                switch (data)
                {
                    case "nodes":
                        LoadNodesToState(reader.ReadToEnd(), ReceiveEventController.GetState());
                        return "ok";
                    case "parts":
                        LoadPartsToState(reader.ReadToEnd(), ReceiveEventController.GetState());
                        return "ok";
                    default:
                        return "wrong data";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "bad";
            }
        }

        public static void LoadPartsToState(string json, State state)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(json, Utils.ConverterSettings);
            var existIds = state.Equipment.Parts.Select(p => p.Id);
            var newParts = parts.Where(p => !existIds.Contains(p.Id));
            state.Equipment.Parts.AddRange(newParts);
        }
        public static void LoadNodesToState(string json, State state)
        {
            var nodes = JsonConvert.DeserializeObject<List<Node>>(json, Utils.ConverterSettings);
            var existIds = state.Geo.Nodes.Select(p => p.Id);
            var newNodes = nodes.Where(p => !existIds.Contains(p.Id));
            state.Geo.Nodes.AddRange(newNodes);
        }
    }
}