using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Waremap.Controllers
{
    [ApiController]
    [Route("/background")]
    public class BackgroundController : ControllerBase
    {
        [HttpPost]
        public string Post()
        {
            using var reader = new StreamReader(Request.Body);
            var body = reader.ReadToEnd();
            ReceiveEventController.GetState().Background = body;
            return $"Saved {body.Length} symbols";
        }

        [HttpGet]
        public string Get()
        {
            return ReceiveEventController.GetState().Background;
        }
    }
}