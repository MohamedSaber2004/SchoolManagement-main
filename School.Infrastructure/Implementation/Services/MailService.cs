using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using School.Application.Common.Configuration;
using School.Application.Features.Email.Dtos;
using School.Application.Interfaces.Services;

namespace School.Infrastructure.Implementation.Services
{
    public class MailService(IOptions<MailSettings> _mailSettings) : IMailService
    {
        public async Task SendEmailAsync(MailMessageDto mailMessage)
        {
            var email = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_mailSettings.Value.Email),
                Subject = mailMessage.Subject,
            };

            email.To.Add(MailboxAddress.Parse(mailMessage.To));
            email.From.Add(MailboxAddress.Parse(_mailSettings.Value.DisplayName));

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = mailMessage.Body;

            email.Body = bodyBuilder.ToMessageBody();

            var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Value.Host,_mailSettings.Value.Port,SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Value.Email, _mailSettings.Value.Password);
            await smtp.SendAsync(email);   
            await smtp.DisconnectAsync(true);
        }
    }
}
