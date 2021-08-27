using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Controllers
{
    [Authorize]
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
    }
}