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
            using var reader = new StreamReader(Request.Body);
            var body = reader.ReadToEnd();
            var state = JsonConvert.DeserializeObject<State>(body);

            var actualState = ReceiveEventController.GetState();
            actualState.Equipment = state.Equipment;
            actualState.Geo = state.Geo;

            return JsonConvert.SerializeObject(actualState);
        }
    }
}