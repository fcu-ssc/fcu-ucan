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
        public IActionResult Privacy()
        {
            return View();
        }
        
        [HttpGet("ucan/personal")]
        public IActionResult Personal()
        {
            return View();
        }
        
        [HttpGet("ucan/terms")]
        public IActionResult Terms()
        {
            return View();
        }
        
        [HttpGet("fcu/announcement")]
        public IActionResult Announcement()
        {
            return View();
        }
        
        [HttpGet("fcu/protection")]
        public IActionResult Protection()
        {
            return View();
        }
        
        [HttpGet("fcu/rights")]
        public IActionResult Rights()
        {
            return View();
        }
        
        [HttpGet("fcu/statement")]
        public IActionResult Statement()
        {
            return View();
        }
    }
}