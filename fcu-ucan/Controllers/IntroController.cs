using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Controllers
{
    [Route("intro")]
    public class IntroController : Controller
    {
        private readonly ILogger<IntroController> _logger;

        public IntroController(ILogger<IntroController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("")]
        public IActionResult Index() => View();

        [HttpGet("intent")]
        public IActionResult Intent() => View();

        [HttpGet("foundation")]
        public IActionResult Foundation() => View();

        [HttpGet("feature")]
        public IActionResult Feature() => View();

        [HttpGet("competency")]
        public IActionResult Competency() => View();

        [HttpGet("occupation")]
        public IActionResult Occupation() => Redirect("https://ucan.moe.edu.tw/Occupation/occupationSearch.aspx");
    }
}