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
            return View();
        }

        [HttpGet("common/communication")]
        public IActionResult Communication()
        {
            return View();
        }
        
        [HttpGet("common/problem-solved")]
        public IActionResult ProblemSolved()
        {
            return View();
        }
        
        [HttpGet("common/keep-learning")]
        public IActionResult KeepLearning()
        {
            return View();
        }
        
        [HttpGet("common/innovation")]
        public IActionResult Innovation()
        {
            return View();
        }
        
        [HttpGet("common/interpersonal-interaction")]
        public IActionResult InterpersonalInteraction()
        {
            return View();
        }
        
        [HttpGet("common/responsibility")]
        public IActionResult Responsibility()
        {
            return View();
        }
        
        [HttpGet("common/teamwork")]
        public IActionResult Teamwork()
        {
            return View();
        }
        
        [HttpGet("common/information-technology")]
        public IActionResult InformationTechnology()
        {
            return View();
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