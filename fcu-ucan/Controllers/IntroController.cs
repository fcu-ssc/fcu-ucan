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
        
        /// <summary>
        /// 認識 UCAN 頁面
        /// </summary>
        [HttpGet("")]
        public IActionResult Index() => View();

        /// <summary>
        /// 平台緣起及建置目的頁面
        /// </summary>
        [HttpGet("intent")]
        public IActionResult Intent() => View();

        /// <summary>
        /// 平台發展及依據頁面
        /// </summary>
        [HttpGet("foundation")]
        public IActionResult Foundation() => View();

        /// <summary>
        /// 主要功能簡介頁面
        /// </summary>
        [HttpGet("feature")]
        public IActionResult Feature() => View();

        /// <summary>
        /// 職能介紹頁面
        /// </summary>
        [HttpGet("competency")]
        public IActionResult Competency() => View();

        /// <summary>
        /// 職業查詢頁面
        /// </summary>
        [HttpGet("occupation")]
        public IActionResult Occupation() => Redirect("https://ucan.moe.edu.tw/Occupation/occupationSearch.aspx");
    }
}