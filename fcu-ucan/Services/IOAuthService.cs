using System.Threading.Tasks;
using fcu_ucan.Models.NID;

namespace fcu_ucan.Services
{
    public interface IOAuthService
    {
        Task<UserInfoViewModel> GetLoginUser(string userCode);
        
        Task<string> GetToken(string username);
    }
}