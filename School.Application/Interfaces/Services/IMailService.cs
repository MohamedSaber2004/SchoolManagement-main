using School.Application.Features.Email.Dtos;

namespace School.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailMessageDto mailMessage);
    }
}
