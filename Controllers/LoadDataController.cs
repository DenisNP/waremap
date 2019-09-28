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
    [Route("/data")]
    public class LoadDataController : ControllerBase
    {
        [HttpPost]
        public string Load()
        {
            if (!Request.Query.ContainsKey("list"))
            {
                return "no data field";
            }
            
            var data = Request.Query["list"];
            try
            {
                using var reader = new StreamReader(Request.Body);
                var state = ReceiveEventController.GetState();
                switch (data)
                {
                    case "nodes":
                        LoadNodesToState(reader.ReadToEnd(), state);
                        return $"Total nodes: {state.Geo.Nodes.Count}";
                    case "parts":
                        LoadPartsToState(reader.ReadToEnd(), state);
                        return $"Total parts: {state.Equipment.Parts.Count}";
                    case "operations":
                        LoadOperationsToState(reader.ReadToEnd(), state);
                        return $"Total operations: {state.Equipment.Operations.Count}";
                    case "assemblies":
                        LoadAssembliesToState(reader.ReadToEnd(), state);
                        return $"Total assemblies: {state.Equipment.Assemblies.Count}";
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

        public static void LoadPartsToState(string json, State state, bool force = false)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(json, Utils.ConverterSettings);
            if (force)
            {
                state.Equipment.Parts.Clear();
            }
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

        public static void LoadOperationsToState(string json, State state, bool force = true)
        {
            var operations = JsonConvert.DeserializeObject<List<Operation>>(json, Utils.ConverterSettings);
            if (force)
            {
                state.Equipment.Operations.Clear();
            }
            var existIds = state.Equipment.Operations.Select(p => p.Id);
            var newOperations = operations.Where(p => !existIds.Contains(p.Id));
            state.Equipment.Operations.AddRange(newOperations);
        }

        public static void LoadAssembliesToState(string json, State state, bool force = true)
        {
            var assemblies = JsonConvert.DeserializeObject<List<Assembly>>(json, Utils.ConverterSettings);
            if (force)
            {
                state.Equipment.Assemblies.Clear();
            }
            var existIds = state.Equipment.Assemblies.Select(p => p.Id);
            var newAssemblies = assemblies.Where(p => !existIds.Contains(p.Id));
            state.Equipment.Assemblies.AddRange(newAssemblies);
        }
    }
}