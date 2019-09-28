using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Waremap.Models;

namespace Waremap.Controllers
{
    [ApiController]
    [Route("/load")]
    public class LoadStateController : ControllerBase
    {
        [HttpPost]
        public string Post()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = reader.ReadToEnd();
                var actualState = LoadState(body);

                return JsonConvert.SerializeObject(actualState, Utils.ConverterSettings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static State LoadState(string body)
        {
            var state = JsonConvert.DeserializeObject<State>(body, Utils.ConverterSettings);

            var actualState = ReceiveEventController.GetState();
            actualState.Equipment = state.Equipment;
            actualState.Geo = state.Geo;
            actualState.CarRoadmap = state.CarRoadmap;
            return actualState;
        }
    }
}