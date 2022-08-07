using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
    [ApiController]
    [Route("/")]
    public class CheckaliveController : ControllerBase
    {
        [HttpGet]
        public string Get() {
            return "Aaaaaa! I miss the ocean!";
        }
    }
}