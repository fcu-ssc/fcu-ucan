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

        [HttpGet("facebook")]
        public IActionResult Facebook() => Redirect("https://www.facebook.com/fcussc/");

        [HttpGet("instagram")]
        public IActionResult Instagram() => Redirect("https://www.instagram.com/fcu.cdc/");

        [HttpGet("github")]
        public IActionResult GitHub() => Redirect("https://github.com/fcu-ssc/");

        [HttpGet("site")]
        public IActionResult Site() => Redirect("https://ssc.fcu.edu.tw/");
    }
}
