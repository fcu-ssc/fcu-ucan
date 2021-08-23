using fcu_ucan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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

        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }
        
        [HttpGet("introduce/competency")]
        public IActionResult Competency()
        {
            return Redirect("https://ucan.moe.edu.tw/introduce/introduce_1.aspx");
        }
        
        [HttpGet("introduce/common")]
        public IActionResult Common()
        {
            return Redirect("https://ucan.moe.edu.tw/commsearch/search.aspx");
        }
        
        [HttpGet("introduce/professional")]
        public IActionResult Professional()
        {
            return Redirect("https://ucan.moe.edu.tw/search/search.aspx");
        }
        
        [HttpGet("introduce/occupation")]
        public IActionResult Occupation()
        {
            return Redirect("https://ucan.moe.edu.tw/Occupation/occupationSearch.aspx");
        }
    }
}
