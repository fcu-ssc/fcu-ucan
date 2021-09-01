using System.IO;
using System.Linq;
using System.Threading.Tasks;
using fcu_ucan.Helpers;
using fcu_ucan.Models.Manage;
using fcu_ucan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeTypes;

namespace fcu_ucan.Controllers
{
    [AuthAuthorize]
    [Route("manage")]
    public class ManageController : Controller
    {
        private readonly ILogger<ManageController> _logger;
        private readonly IOAuthService _oAuthService;
        private readonly IConfiguration _configuration;

        public ManageController(
            ILogger<ManageController> logger, 
            IOAuthService oAuthService, 
            IConfiguration configuration)
        {
            _logger = logger;
            _oAuthService = oAuthService;
            _configuration = configuration;
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
        
        /// <summary>
        /// UCAN 登入頁面
        /// </summary>
        [AuthAuthorize(Roles = "UCAN")]
        [HttpGet("ucan-login")]
        public IActionResult UCANLogin() => View();

        /// <summary>
        /// UCAN 登入
        /// </summary>
        [AuthAuthorize(Roles = "UCAN")]
        [HttpPost("ucan-login")]
        public async Task<IActionResult> UCANLogin([FromForm] UCANLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _oAuthService.GetToken(model.UserName);
                switch (token[0])
                {
                    case '0':
                        ModelState.AddModelError("", $"IP 不允許 {token.Substring(2)}");
                        break;
                    case '1':
                        ModelState.AddModelError("", "學校代碼不存在");
                        break;
                    case '2':
                        ModelState.AddModelError("", "會員帳號不存在");
                        break;
                    default:
                        var url = $"{_configuration["Domain"]}/ucann_school/sso.aspx?" +
                                  $"Plugin=o_hdu&" +
                                  $"Action=ohduschoolssologin&" +
                                  $"username={model.UserName}&" +
                                  $"token={token}&" +
                                  $"school={_configuration["UCAN:School"]}";
                        return Redirect(url);
                }
            }
            return View(model);
        }
    }
}