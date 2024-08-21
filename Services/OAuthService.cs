using System.Text.Json;
using fcu_ucan.Models.NID;
using fcu_ucan.Services.Interface;

namespace fcu_ucan.Services;

public class OAuthService(ILogger<OAuthService> logger, IConfiguration configuration, IHttpClientFactory clientFactory) 
    : IOAuthService
{
    /// <summary>
    /// 使用 NID 獲得資訊
    /// </summary>
    public async Task<UserInfoViewModel> GetLoginUser(string userCode)
    {
        var url = $"fcuapi/api/GetLoginUser?" + 
                  $"client_id={configuration["NID:ClientId"]}&" +
                  $"user_code={userCode}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var client = clientFactory.CreateClient("NID");
        logger.LogInformation($"NID 登入開始: {url}");
        try
        {
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
                
            var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<LoginRespondViewModel>(responseStream);
            if (result == null)
            {
                logger.LogInformation("NID 登入解析為 null");
                throw new Exception();
            }
            var dto = result.UserInfo.First();
            logger.LogInformation($"NID 登入解析: {dto.Status}, {dto.Message}, {dto.StuId}");
            return dto;
        }
        catch (HttpRequestException e)
        {
            logger.LogInformation($"NID 登入失敗: {e}");
            throw;
        }
    }
    
    /// <summary>
    /// 使用帳號獲得 UCAN Token
    /// </summary>
    public async Task<string> GetToken(string username)
    {
        var url = $"ucann_school/sso.aspx?" +
                  $"Plugin=o_hdu&" +
                  $"Action=ohduschoolssogettoken&" +
                  $"username={username}&" +
                  $"school={configuration["UCAN:School"]}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var client = clientFactory.CreateClient("UCAN");
        logger.LogInformation($"獲取 Ucan Token 開始: {url}");
        try
        {
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStringAsync();
            logger.LogInformation($"獲取 Ucan Token 成功: {responseStream}");
            return responseStream;
        }
        catch (HttpRequestException e)
        {
            logger.LogInformation($"獲取 Ucan Token 失敗: {e}");
            throw;
        }
    }
}