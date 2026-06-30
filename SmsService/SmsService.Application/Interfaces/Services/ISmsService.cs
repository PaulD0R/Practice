using SmsService.Application.Events;
using SmsService.Domain.Models;

namespace SmsService.Application.Interfaces.Services;

public interface ISmsService
{
    Task<Sms> SendSmsAsync(SendSmsEvent message);
    Task SendApproveMessageAsync(Sms message);
    Task SendErrorMessageAsync(Guid messageId, string errorMessage);
}