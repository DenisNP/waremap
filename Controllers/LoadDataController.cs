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
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var carsList = JsonConvert.DeserializeObject<List<Car>>(reader.ReadToEnd());
                    LoadCarsToState(carsList, ReceiveEventController.GetState());
                    return "ok";
                }
            }
            catch (Exception e)
            {
                return "bad";
            }
        }

        public static void LoadCarsToState(List<Car> cars, State state)
        {
            var existIds = state.Equipment.Cars.Select(c => c.Id);
            var newCars = cars.Where(c => !existIds.Contains(c.Id));
            state.Equipment.Cars.AddRange(newCars);
        }
    }
}