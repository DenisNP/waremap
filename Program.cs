using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using MoreLinq;
using Newtonsoft.Json;
using Waremap.Controllers;
using Waremap.Models;

namespace Waremap
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var state = ReceiveEventController.GetState();
            
            // preload mock data
            /* using (var reader = new StreamReader("shared/nodes.json"))
            {
                LoadDataController.LoadNodesToState(reader.ReadToEnd(), state);
                foreach (var node in state.Geo.Nodes)
                {
                    node.Name = $"Участок {node.Id}, Цех {node.Depot}, Этаж {node.Floor}";
                    node.Icon = node.Type == NodeType.Machine ? "Machine" : "Node";
                }
                Console.WriteLine($"Mock nodes loaded: {state.Geo.Nodes.Count}");
            } */
            
            using (var reader = new StreamReader("shared/waremap-state.json"))
            {
                var savedState = JsonConvert.DeserializeObject<SavedState>(reader.ReadToEnd(),Utils.ConverterSettings);
                
                state.Geo = savedState.State.Geo;
                state.Equipment = savedState.State.Equipment;
                state.CarRoadmap = savedState.State.CarRoadmap;

                savedState.Backgrounds.Where(x => x != null).ForEach(x => { state.Background.Add(x.Floor, x.Base64); });

                Console.WriteLine($"Saved state loaded: {state.Geo.Nodes.Count}");
            }

            using (var reader = new StreamReader("shared/parts.json"))
            {
                LoadDataController.LoadPartsToState(reader.ReadToEnd(), state);
                Console.WriteLine($"Mock parts loaded: {state.Equipment.Parts.Count}");
            }

            using (var reader = new StreamReader("shared/operations.json"))
            {
                LoadDataController.LoadOperationsToState(reader.ReadToEnd(), state);
                Console.WriteLine($"Mock operations loaded: {state.Equipment.Operations.Count}");
            }

            using (var reader = new StreamReader("shared/assemblies.json"))
            {
                LoadDataController.LoadAssembliesToState(reader.ReadToEnd(), state);
                Console.WriteLine($"Mock assemblies loaded: {state.Equipment.Assemblies.Count}");
            }
            // start server
            StartServer();
        }

        private static void StartServer()
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}