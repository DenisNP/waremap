using System;
using System.Collections.Generic;
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
            try
            {
                if (Request.Query.ContainsKey("floor") && int.TryParse(Request.Query["floor"], out var floor))
                {
                    using var reader = new StreamReader(Request.Body);
                    var body = reader.ReadToEnd();
                    ReceiveEventController.GetState().Background.Add(floor, body);
                    return $"Saved {body.Length} symbols on floor {floor}";
                }

                return "No floor specified";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public string Get()
        {
            try
            {
                if (Request.Query.ContainsKey("floor") && int.TryParse(Request.Query["floor"], out var floor))
                {
                    return ReceiveEventController.GetState().Background.GetValueOrDefault(floor);
                }

                return "No floor specified";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}