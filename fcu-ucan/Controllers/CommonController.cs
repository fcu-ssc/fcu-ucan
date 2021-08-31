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
        
        /// <summary>
        /// 職場共通職能查詢
        /// </summary>
        [HttpGet("")]
        public IActionResult Index() => View();

        /// <summary>
        /// 溝通表達
        /// </summary>
        [HttpGet("communication")]
        public IActionResult Communication() => View();

        /// <summary>
        /// 問題解決
        /// </summary>
        [HttpGet("problem-solved")]
        public IActionResult ProblemSolved() => View();

        /// <summary>
        /// 持續學習
        /// </summary>
        [HttpGet("keep-learning")]
        public IActionResult KeepLearning() => View();

        /// <summary>
        /// 創新
        /// </summary>
        [HttpGet("innovation")]
        public IActionResult Innovation() => View();

        /// <summary>
        /// 人際互動
        /// </summary>
        [HttpGet("interpersonal-interaction")]
        public IActionResult InterpersonalInteraction() => View();

        /// <summary>
        /// 工作責任及紀律
        /// </summary>
        [HttpGet("responsibility")]
        public IActionResult Responsibility() => View();

        /// <summary>
        /// 團隊合作
        /// </summary>
        [HttpGet("teamwork")]
        public IActionResult Teamwork() => View();

        /// <summary>
        /// 資訊科技應用
        /// </summary>
        [HttpGet("information-technology")]
        public IActionResult InformationTechnology() => View();
    }
}