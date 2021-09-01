using System.Diagnostics;
using fcu_ucan.Models.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// 錯誤頁面
        /// </summary>
        [HttpGet("")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// UCAN 錯誤頁面
        /// </summary>
        /// <returns></returns>
        [Route("nid")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult NIDError()
        {
            return View(new NIDErrorViewModel
            {
                RequestId = (string)TempData["RequestId"],
                HttpCode = (int)TempData["HttpCode"],
                Message = (string)TempData["Message"]
            });
        }
        
        /// <summary>
        /// NID 錯誤頁面
        /// </summary>
        /// <returns></returns>
        [Route("ucan")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult UCANError()
        {
            return View(new UCANErrorViewModel()
            {
                RequestId = (string)TempData["RequestId"],
                HttpCode = (int)TempData["HttpCode"],
                Message = (string)TempData["Message"]
            });
        }
    }
}