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
            // preload mock data
            using (var reader = new StreamReader("shared/cars.json"))
            {
                LoadDataController.LoadCarsToState(reader.ReadToEnd(), ReceiveEventController.GetState());
                Console.WriteLine("Mock cars loaded: " + ReceiveEventController.GetState().Equipment.Cars.Count);
            }
            using (var reader = new StreamReader("shared/cars.json"))
            {
                LoadDataController.LoadPartsToState(reader.ReadToEnd(), ReceiveEventController.GetState());
                Console.WriteLine("Mock parts loaded: " + ReceiveEventController.GetState().Equipment.Parts.Count);
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