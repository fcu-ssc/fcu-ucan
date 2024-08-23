using System.Diagnostics;
using fcu_ucan.Data;
using fcu_ucan.Models.NID;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fcu_ucan.Controllers;

[Route("nid")]
public class NIDController(
    ILogger<NIDController> logger,
    IConfiguration configuration,
    ApplicationDbContext dbContext) : Controller
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        var url = configuration.GetSection("NID").GetValue<string>("Url")
            .AppendPathSegment("fcuOauth")
            .AppendPathSegment("Auth.aspx")
            .SetQueryParams(new
            {
                client_id = configuration.GetSection("NID").GetValue<string>("ClientId"),
                client_url = $"{Request.Scheme}://{Request.Host}"
                    .AppendPathSegment("ucan")
                    .AppendPathSegment("nid"),
            });
        
        logger.LogInformation($"開始 NID 登入: {url}");
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
                var user = await GetLoginUserAsync(model.UserCode);
                logger.LogInformation($"NID 登入成功: {user.Status}, {user.Message}, {user.StuId}");
                var member = await dbContext.Members
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.NetworkId == user.StuId);
                if (member is not null)
                {
                    logger.LogInformation($"{user.StuId} 換成 {member.StudentId}");
                }
                var username = member is null ? user.StuId : member.StudentId;
                var token = await GetTokenAsync(username);
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
                        var url = $"{Request.Scheme}://{Request.Host}"
                            .AppendPathSegment("ucann_school")
                            .AppendPathSegment("sso.aspx")
                            .SetQueryParams(new
                            {
                                Plugin = "o_hdu",
                                Action = "ohduschoolssologin",
                                username = username,
                                token = token,
                                school = configuration.GetSection("UCAN").GetValue<string>("School")
                            });
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
    
    /// <summary>
    /// 使用 NID 獲得資訊
    /// </summary>
    [NonAction]
    private async Task<UserInfoViewModel> GetLoginUserAsync(string userCode)
    {
        var url = configuration.GetSection("NID").GetValue<string>("Url")
            .AppendPathSegment("fcuapi")
            .AppendPathSegment("api")
            .AppendPathSegment("GetLoginUser")
            .SetQueryParams(new
            {
                client_id = configuration.GetSection("NID").GetValue<string>("ClientId"),
                user_code = userCode
            });
        
        logger.LogInformation($"NID 登入開始: {url}");

        try
        {
            var response = await url.GetJsonAsync<LoginRespondViewModel>();
            if (response is null)
            {
                logger.LogInformation("NID 登入解析為 null");
                throw new Exception();
            }
            
            var model = response.UserInfo.First();
            logger.LogInformation($"NID 登入解析: {model.Status}, {model.Message}, {model.StuId}");
            return model;
        }
        catch (Exception e)
        {
            logger.LogInformation($"NID 登入失敗: {e}");
            throw;
        }
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