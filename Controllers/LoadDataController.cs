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
                    case "cars":
                        LoadCarsToState(reader.ReadToEnd(), ReceiveEventController.GetState());
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
            var newCars = parts.Where(p => !existIds.Contains(p.Id));
            state.Equipment.Parts.AddRange(newCars);
        }

        public static void LoadCarsToState(string json, State state)
        {
            var cars = JsonConvert.DeserializeObject<List<Car>>(json, Utils.ConverterSettings);
            var existIds = state.Equipment.Cars.Select(c => c.Id);
            var newCars = cars.Where(c => !existIds.Contains(c.Id));
            state.Equipment.Cars.AddRange(newCars);
        }
    }
}