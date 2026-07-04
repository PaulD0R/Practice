using EmailService.Application.Events;
using EmailService.Domain.Models;

namespace EmailService.Application.Interfaces.Services;

public interface IEmailService
{
    Task<EmailMessage> SendEmailAsync(SendEmailEvent message);
    Task SendRetryMessageAsync(RetrySendEmailEvent message, TimeSpan delay);
    Task SendApproveMessageAsync(EmailMessage message);
    Task SendErrorMessageAsync(Guid messageId, string errorMessage);
}