using System.Diagnostics;
using fcu_ucan.Data;
using fcu_ucan.Models.NID;
using fcu_ucan.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fcu_ucan.Controllers;

[Route("nid")]
public class NIDController(
    ILogger<NIDController> logger,
    IConfiguration configuration,
    IOAuthService oAuthService,
    ApplicationDbContext dbContext) : Controller
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        var url = $"{configuration["NID:Url"]}/fcuOauth/Auth.aspx?" +
                  $"client_id={configuration["NID:ClientId"]}&" +
                  $"client_url={configuration["Domain"]}/ucan/nid";
        return Redirect(url);
    }
    
    [HttpPost]
    public async Task<IActionResult> HandleLogin([FromForm] RespondViewModel model)
    {
        logger.LogInformation($"NID 登入: {model.Status}, {model.Message}, {model.UserCode}");
        switch (model.Status)
        {
            case 100:
                logger.LogInformation("NID 登入: 使用者拒絕授權");
                TempData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
                TempData["HttpCode"] = 401;
                TempData["Message"] = "使用者拒絕授權";
                return RedirectToAction("NIDError", "Error");
            case 200:
                var user = await oAuthService.GetLoginUser(model.UserCode);
                logger.LogInformation($"NID 登入成功: {user.Status}, {user.Message}, {user.StuId}");
                var member = await dbContext.Members
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.NetworkId == user.StuId);
                if (member != null)
                {
                    logger.LogInformation($"{user.StuId} 換成 {member.StudentId}");
                }
                var username = member == null ? user.StuId : member.StudentId;
                var token = await oAuthService.GetToken(username);
                logger.LogInformation($"獲取 Ucan Token 成功: {token}");
                switch (token[0])
                {
                    case '0':
                        logger.LogInformation($"Ucan Token 解析: IP 不允許 {token.Substring(2)}");
                        TempData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
                        TempData["HttpCode"] = 403;
                        TempData["Message"] = $"IP 不允許 {token.Substring(2)}";
                        return RedirectToAction("UcanError", "Error");
                    case '1':
                        logger.LogInformation("Ucan Token 解析: 學校代碼不存在");
                        TempData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
                        TempData["HttpCode"] = 404;
                        TempData["Message"] = "學校代碼不存在";
                        return RedirectToAction("UcanError", "Error");
                    case '2':
                        logger.LogInformation("Ucan Token 解析: 會員帳號不存在");
                        TempData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
                        TempData["HttpCode"] = 404;
                        TempData["Message"] = "會員帳號不存在";
                        return RedirectToAction("UcanError", "Error");
                    default:
                        logger.LogInformation("Ucan Token 解析成功");
                        var url = $"{configuration["Domain"]}/ucann_school/sso.aspx?" +
                                  $"Plugin=o_hdu&" +
                                  $"Action=ohduschoolssologin&" +
                                  $"username={username}&" +
                                  $"token={token}&" +
                                  $"school={configuration["UCAN:School"]}";
                        logger.LogInformation($"Ucan 登入: {url}");
                        return Redirect(url);
                }
            case 300:
                logger.LogInformation("NID 登入: 應用程式授權已到期");
                TempData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
                TempData["HttpCode"] = 403;
                TempData["Message"] = "應用程式授權已到期";
                return RedirectToAction("NIDError", "Error");
            case 400:
                logger.LogInformation("NID 登入: 欠缺必要的參數、有不正確的參數、有重複的參數、或其他原因導致無法解讀");
                TempData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
                TempData["HttpCode"] = 400;
                TempData["Message"] = "欠缺必要的參數、有不正確的參數、有重複的參數、或其他原因導致無法解讀";
                return RedirectToAction("NIDError", "Error");
            case 500:
                logger.LogInformation("NID 登入: 認證伺服器因為過載或維修中而暫時無法處理請求");
                TempData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
                TempData["HttpCode"] = 500;
                TempData["Message"] = "認證伺服器因為過載或維修中而暫時無法處理請求";
                return RedirectToAction("NIDError", "Error");
            default:
                logger.LogInformation("NID 登入: 發生例外況狀");
                TempData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
                TempData["HttpCode"] = 500;
                TempData["Message"] = "發生例外況狀";
                return RedirectToAction("NIDError", "Error");
        }
    }
}