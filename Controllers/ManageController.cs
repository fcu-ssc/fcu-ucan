using fcu_ucan.Models.Manage;
using fcu_ucan.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeTypes;

namespace fcu_ucan.Controllers;

[Authorize]
[Route("manage")]
public class ManageController(IOAuthService oAuthService, IConfiguration configuration) : Controller
{
    /// <summary>
    /// 管理頁面
    /// </summary>
    [HttpGet("")]
    public IActionResult Index() => View();

    /// <summary>
    /// 日誌頁面
    /// </summary>
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
    [HttpGet("logs/{fileName}")]
    public IActionResult Logs([FromRoute] string fileName)
    {
        var di = new DirectoryInfo("Logs");
        var file = di.GetFiles(fileName).SingleOrDefault(x => x.Name == fileName);
        if (file is null)
        {
            return NotFound();
        }
        return File(System.IO.File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), MimeTypeMap.GetMimeType(file.Extension), file.Name);
    }
    
    /// <summary>
    /// UCAN 登入頁面
    /// </summary>
    [HttpGet("ucan-login")]
    public IActionResult UCANLogin() => View();

    /// <summary>
    /// UCAN 登入
    /// </summary>
    [HttpPost("ucan-login")]
    public async Task<IActionResult> UCANLogin([FromForm] UCANLoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var token = await oAuthService.GetToken(model.UserName);
            switch (token[0])
            {
                case '0':
                    ModelState.AddModelError(string.Empty, $"IP 不允許 {token.Substring(2)}");
                    break;
                case '1':
                    ModelState.AddModelError(string.Empty, "學校代碼不存在");
                    break;
                case '2':
                    ModelState.AddModelError(string.Empty, "會員帳號不存在");
                    break;
                default:
                    var url = $"{configuration["Domain"]}/ucann_school/sso.aspx?" +
                              $"Plugin=o_hdu&" +
                              $"Action=ohduschoolssologin&" +
                              $"username={model.UserName}&" +
                              $"token={token}&" +
                              $"school={configuration["UCAN:School"]}";
                    return Redirect(url);
            }
        }
        return View(model);
    }
}