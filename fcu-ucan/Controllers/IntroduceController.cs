using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Controllers
{
    [Route("introduce")]
    public class IntroduceController : Controller
    {
        private readonly ILogger<IntroduceController> _logger;

        public IntroduceController(ILogger<IntroduceController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("about")]
        public IActionResult About()
        {
            return View();
        }
        
        [HttpGet("intent")]
        public IActionResult Intent()
        {
            return View();
        }
        
        [HttpGet("foundation")]
        public IActionResult Foundation()
        {
            return View();
        }
        
        [HttpGet("feature")]
        public IActionResult Feature()
        {
            return View();
        }

        [HttpGet("competency")]
        public IActionResult Competency()
        {
            return View();
        }
        
        [HttpGet("common")]
        public IActionResult Common()
        {
            return Redirect("https://ucan.moe.edu.tw/commsearch/search.aspx");
        }
        
        [HttpGet("professional")]
        public IActionResult Professional()
        {
            return Redirect("https://ucan.moe.edu.tw/search/search.aspx");
        }
        
        [HttpGet("occupation")]
        public IActionResult Occupation()
        {
            return Redirect("https://ucan.moe.edu.tw/Occupation/occupationSearch.aspx");
        }
    }
}