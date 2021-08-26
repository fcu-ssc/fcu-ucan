using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Controllers
{
    [Route("privacy")]
    public class PrivacyController : Controller
    {
        private readonly ILogger<PrivacyController> _logger;

        public PrivacyController(ILogger<PrivacyController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("")]
        public IActionResult Index() => View();

        [HttpGet("ucan/personal")]
        public IActionResult Personal() => View();

        [HttpGet("ucan/terms")]
        public IActionResult Terms() => View();

        [HttpGet("fcu/announcement")]
        public IActionResult Announcement() => View();

        [HttpGet("fcu/protection")]
        public IActionResult Protection() => View();

        [HttpGet("fcu/rights")]
        public IActionResult Rights() => View();

        [HttpGet("fcu/statement")]
        public IActionResult Statement() => View();
    }
}