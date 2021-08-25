using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using fcu_ucan.Models.NID;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Services
{
    public class OAuthService : IOAuthService
    {
        private readonly ILogger<OAuthService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        public OAuthService(
            ILogger<OAuthService> logger, 
            IConfiguration configuration, 
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// 使用 NID 獲得資訊
        /// </summary>
        public async Task<UserInfoViewModel> GetLoginUser(string userCode)
        {
            var url = $"fcuapi/api/GetLoginUser?" + 
                      $"client_id={_configuration["NID:ClientId"]}&" +
                      $"user_code={userCode}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient("NID");
            _logger.LogInformation($"NID 登入開始: {url}");
            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                
                var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<LoginRespondViewModel>(responseStream);
                if (result == null)
                {
                    _logger.LogInformation("NID 登入解析為 null");
                    throw new Exception();
                }
                var dto = result.UserInfo.First();
                _logger.LogInformation($"NID 登入解析: {dto.Status}, {dto.Message}, {dto.StuId}");
                return dto;
            }
            catch (HttpRequestException e)
            {
                _logger.LogInformation($"NID 登入失敗: {e}");
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
                      $"school={_configuration["UCAN:School"]}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient("UCAN");
            _logger.LogInformation($"獲取 Ucan Token 開始: {url}");
            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseStream = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"獲取 Ucan Token 成功: {responseStream}");
                return responseStream;
            }
            catch (HttpRequestException e)
            {
                _logger.LogInformation($"獲取 Ucan Token 失敗: {e}");
                throw;
            }
        }
    }
}