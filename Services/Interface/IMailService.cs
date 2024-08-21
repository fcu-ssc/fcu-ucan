namespace fcu_ucan.Services.Interface;

public interface IMailService
{
    Task SendRegisterEmailAsync(string email, string code);
}