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

        public static void LoadOperationsToState(string json, State state)
        {
            var operations = JsonConvert.DeserializeObject<List<Operation>>(json, Utils.ConverterSettings);
            var existIds = state.Equipment.Operations.Select(p => p.Id);
            var newOperations = operations.Where(p => !existIds.Contains(p.Id));
            state.Equipment.Operations.AddRange(newOperations);
        }

        public static void LoadAssembliesToState(string json, State state)
        {
            var assemblies = JsonConvert.DeserializeObject<List<Assembly>>(json, Utils.ConverterSettings);
            var existIds = state.Equipment.Assemblies.Select(p => p.Id);
            var newAssemblies = assemblies.Where(p => !existIds.Contains(p.Id));
            state.Equipment.Assemblies.AddRange(newAssemblies);
        }
    }
}