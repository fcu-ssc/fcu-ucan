using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace fcu_ucan.Services
{
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly IConfiguration _configuration;

        public MailService(ILogger<MailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendRegisterEmailAsync(string email, string code)
        {
            var message = new MimeMessage {Importance = MessageImportance.High};
            message.From.Add(new MailboxAddress(_configuration["Mail:SenderName"], _configuration["Mail:SenderEmail"]));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = "FCU x UCAN 註冊邀請信";
            
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<p>請點擊下方連結註冊</p><a href=\"{new Uri(_configuration["Domain"])}ucan/account/register/{code}\">{new Uri(_configuration["Domain"])}ucan/account/register/{code}</a>"
            };
            message.Body = bodyBuilder.ToMessageBody();

            #region 寄信

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_configuration["Mail:Server"], Convert.ToInt32(_configuration["Mail:Port"]), SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuration["Mail:UserName"], _configuration["Mail:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            #endregion
        }
    }
}