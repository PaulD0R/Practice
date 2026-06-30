using EmailService.Application.Events;
using EmailService.Domain.Models;

namespace EmailService.Application.Interfaces.Services;

public interface IEmailService
{
    Task<EmailMessage> SendEmailAsync(SendEmailEvent message);
    Task SendApproveMessageAsync(EmailMessage message);
    Task SendErrorMessageAsync(Guid messageId, string errorMessage);
}