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
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet("facebook")]
        public IActionResult Facebook()
        {
            return Redirect("https://www.facebook.com/fcussc/");
        }
        
        [HttpGet("instagram")]
        public IActionResult Instagram()
        {
            return Redirect("https://www.instagram.com/fcu.cdc/");
        }
        
        [HttpGet("github")]
        public IActionResult GitHub()
        {
            return Redirect("https://github.com/fcu-ssc/");
        }
        
        [HttpGet("site")]
        public IActionResult Site()
        {
            return Redirect("https://ssc.fcu.edu.tw/");
        }
    }
}
