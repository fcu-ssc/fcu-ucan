using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeTypes;

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
        public IActionResult Index() => View();
        
        [Authorize]
        [HttpGet("log")]
        public IActionResult Log()
        {
            var di = new DirectoryInfo("Logs");
            var files = di.GetFiles();
            return View(files);
        }
        
        [Authorize]
        [HttpGet("log/{fileName}")]
        public IActionResult Log([FromRoute] string fileName)
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

        [HttpGet("facebook")]
        public IActionResult Facebook() => Redirect("https://www.facebook.com/fcussc/");

        [HttpGet("instagram")]
        public IActionResult Instagram() => Redirect("https://www.instagram.com/fcu.cdc/");

        [HttpGet("github")]
        public IActionResult GitHub() => Redirect("https://github.com/fcu-ssc/");

        [HttpGet("site")]
        public IActionResult Site() => Redirect("https://ssc.fcu.edu.tw/");
    }
}
