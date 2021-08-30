using System.Threading.Tasks;

namespace fcu_ucan.Services
{
    public interface IMailService
    {
        Task SendRegisterEmailAsync(string email, string code);
    }
}