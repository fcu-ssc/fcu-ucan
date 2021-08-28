using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeTypes;

namespace fcu_ucan.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("manage")]
    public class ManageController : Controller
    {
        private readonly ILogger<ManageController> _logger;

        public ManageController(ILogger<ManageController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet("logs")]
        public IActionResult Logs()
        {
            var di = new DirectoryInfo("Logs");
            var files = di.GetFiles();
            return View(files);
        }
        
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
        
        [HttpGet("users")]
        public IActionResult Users()
        {
            return View();
        }
        
        [HttpGet("members")]
        public IActionResult Members()
        {
            return View();
        }
    }
}