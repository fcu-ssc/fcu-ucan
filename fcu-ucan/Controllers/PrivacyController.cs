using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Controllers
{
    [Route("privacy")]
    public class PrivacyController : Controller
    {
        private readonly ILogger<PrivacyController> _logger;

        public PrivacyController(ILogger<PrivacyController> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// 隱私權頁面
        /// </summary>
        [HttpGet("")]
        public IActionResult Index() => View();

        /// <summary>
        /// UCAN 個人資料蒐集、處理及利用之告知說明頁面
        /// </summary>
        [HttpGet("ucan/personal")]
        public IActionResult Personal() => View();

        /// <summary>
        /// UCAN 使用者條款頁面
        /// </summary>
        [HttpGet("ucan/terms")]
        public IActionResult Terms() => View();

        /// <summary>
        /// 逢甲大學 個資保護宣導事項頁面
        /// </summary>
        [HttpGet("fcu/announcement")]
        public IActionResult Announcement() => View();

        /// <summary>
        /// 逢甲大學 個人資料保護政策頁面
        /// </summary>
        [HttpGet("fcu/protection")]
        public IActionResult Protection() => View();

        /// <summary>
        /// 逢甲大學 個人資料權利行使頁面
        /// </summary>
        [HttpGet("fcu/rights")]
        public IActionResult Rights() => View();

        /// <summary>
        /// 逢甲大學 隱私權聲明頁面
        /// </summary>
        [HttpGet("fcu/statement")]
        public IActionResult Statement() => View();
    }
}