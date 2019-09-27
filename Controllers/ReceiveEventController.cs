using System;
using Microsoft.AspNetCore.Mvc;

namespace Waremap.Controllers
{
    [ApiController]
    [Route("")]
    public class ReceiveEventController : ControllerBase
    {
        [HttpGet]
        public object Get()
        {
            throw new NotImplementedException();
        }
    }
}