using EmailService.Domain.Models;

namespace EmailService.Application.Interfaces.Services;

public interface IEmailSender
{
    Task SendAsync(EmailMessage message);
}