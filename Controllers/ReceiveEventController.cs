using System;
using Microsoft.AspNetCore.Mvc;

namespace Waremap.Controllers
{
    [ApiController]
    [Route("/state")]
    public class ReceiveEventController : ControllerBase
    {
        [HttpGet]
        public object Get()
        {
            throw new NotImplementedException();
        }
    }
}