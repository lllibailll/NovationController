using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("login")]
    public class DiscordAuthController : ControllerBase
    {
        private ILogger<DiscordAuthController> _logger;
        public DiscordAuthController(ILogger<DiscordAuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get([FromQuery(Name = "code")] string code)
        {
            _logger.LogInformation($"CODE: {code}");
            return Ok();
        }
    }
}