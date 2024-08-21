using fcu_ucan.Models.NID;

namespace fcu_ucan.Services.Interface;

public interface IOAuthService
{
    Task<UserInfoViewModel> GetLoginUser(string userCode);
        
    Task<string> GetToken(string username);
}