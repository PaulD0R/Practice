using EmailService.Application.Events;
using EmailService.Domain.Models;

namespace EmailService.Application.Interfaces.Services;

public interface IEmailService
{
    Task<EmailMessage> SendAsync(SendEmailEvent message);
    Task ApproveMessageAsync(EmailMessage message);
}