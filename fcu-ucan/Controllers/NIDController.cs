using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Controllers
{
    [Route("nid")]
    public class NIDController : Controller
    {
        private readonly ILogger<NIDController> _logger;

        public NIDController(ILogger<NIDController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("login")]
        public IActionResult Login()
        {
            return Redirect("https://www.fcu.edu.tw/");
        }
    }
}