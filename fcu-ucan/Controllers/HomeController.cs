using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Index() => View();
    }
}
