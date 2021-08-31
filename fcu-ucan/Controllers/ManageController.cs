using System.IO;
using System.Linq;
using fcu_ucan.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeTypes;

namespace fcu_ucan.Controllers
{
    [AuthAuthorize]
    [Route("manage")]
    public class ManageController : Controller
    {
        private readonly ILogger<ManageController> _logger;

        public ManageController(ILogger<ManageController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 管理頁面
        /// </summary>
        [HttpGet("")]
        public IActionResult Index() => View();

        /// <summary>
        /// 日誌頁面
        /// </summary>
        [AuthAuthorize(Roles = "Recorder")]
        [HttpGet("logs")]
        public ActionResult<FileInfo[]> Logs()
        {
            var di = new DirectoryInfo("Logs");
            var files = di.GetFiles();
            return View(files);
        }
        
        /// <summary>
        /// 日誌下載
        /// </summary>
        [AuthAuthorize(Roles = "Recorder")]
        [HttpGet("logs/{fileName}")]
        public IActionResult Logs([FromRoute] string fileName)
        {
            var di = new DirectoryInfo("Logs");
            var file = di.GetFiles(fileName)
                .SingleOrDefault(x => x.Name == fileName);
            if (file == null)
            {
                return NotFound();
            }
            return File(System.IO.File.OpenRead(file.FullName), MimeTypeMap.GetMimeType(file.Extension), file.Name);
        }
    }
}