using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Controllers
{
    [Route("intro/common")]
    public class CommonController : Controller
    {
        private readonly ILogger<CommonController> _logger;

        public CommonController(ILogger<CommonController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("")]
        public IActionResult Index() => View();

        [HttpGet("communication")]
        public IActionResult Communication() => View();

        [HttpGet("problem-solved")]
        public IActionResult ProblemSolved() => View();

        [HttpGet("keep-learning")]
        public IActionResult KeepLearning() => View();

        [HttpGet("innovation")]
        public IActionResult Innovation() => View();

        [HttpGet("interpersonal-interaction")]
        public IActionResult InterpersonalInteraction() => View();

        [HttpGet("responsibility")]
        public IActionResult Responsibility() => View();

        [HttpGet("teamwork")]
        public IActionResult Teamwork() => View();

        [HttpGet("information-technology")]
        public IActionResult InformationTechnology() => View();
    }
}