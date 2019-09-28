using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Waremap.Controllers;

namespace Waremap
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var state = ReceiveEventController.GetState();
            
            // preload mock data
            /*using (var reader = new StreamReader("shared/nodes.json"))
            {
                LoadDataController.LoadNodesToState(reader.ReadToEnd(), state);
                Console.WriteLine($"Mock nodes loaded: {state.Geo.Nodes.Count}");
            }*/
            
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