using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace fcu_ucan.Services;

public class MailService(IConfiguration configuration) : Interface.IMailService
{
    public async Task SendRegisterEmailAsync(string email, string code)
    {
        var message = new MimeMessage {Importance = MessageImportance.High};
        message.From.Add(new MailboxAddress(configuration["Mail:SenderName"], configuration["Mail:SenderEmail"]));
        message.To.Add(new MailboxAddress(email, email));
        message.Subject = "FCU x UCAN 註冊邀請信";
            
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"<p>請點擊下方連結註冊</p><a href=\"{new Uri(configuration["Domain"]!)}ucan/account/register/{code}\">{new Uri(configuration["Domain"]!)}ucan/account/register/{code}</a>"
        };
        message.Body = bodyBuilder.ToMessageBody();

        #region 寄信

        using var client = new SmtpClient();
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
        await client.ConnectAsync(configuration["Mail:Server"], Convert.ToInt32(configuration["Mail:Port"]), SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(configuration["Mail:UserName"], configuration["Mail:Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        #endregion
    }
}