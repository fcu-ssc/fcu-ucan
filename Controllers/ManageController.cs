using fcu_ucan.Models.Manage;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeTypes;

namespace fcu_ucan.Controllers;

[Authorize]
[Route("manage")]
public class ManageController(ILogger<ManageController> logger, IConfiguration configuration) : Controller
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
            var token = await GetTokenAsync(model.UserName);
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
                    var url = $"{Request.Scheme}://{Request.Host}"
                        .AppendPathSegment("ucann_school")
                        .AppendPathSegment("sso.aspx")
                        .SetQueryParams(new
                        {
                            Plugin = "o_hdu",
                            Action = "ohduschoolssologin",
                            username = model.UserName,
                            token = token,
                            school = configuration.GetSection("UCAN").GetValue<string>("School")
                        });
                    return Redirect(url);
            }
        }
        return View(model);
    }
    
    /// <summary>
    /// 使用帳號獲得 UCAN Token
    /// </summary>
    [NonAction]
    private async Task<string> GetTokenAsync(string username)
    {
        var url = $"{Request.Scheme}://{Request.Host}"
            .AppendPathSegment("ucann_school")
            .AppendPathSegment("sso.aspx")
            .SetQueryParams(new
            {
                Plugin = "o_hdu",
                Action = "ohduschoolssogettoken",
                username = username,
                school = configuration.GetSection("UCAN").GetValue<string>("School")
            });
        
        logger.LogInformation($"獲取 Ucan Token 開始: {url}");

        try
        {
            var response = await url.GetStringAsync();
            if (response is null)
            {
                logger.LogInformation("獲取 Ucan Token 為 null");
                throw new Exception();
            }
            
            logger.LogInformation($"獲取 Ucan Token 成功: {response}");
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}