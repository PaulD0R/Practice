using SmsService.Application.Events;
using SmsService.Domain.Models;

namespace SmsService.Application.Interfaces.Services;

public interface ISmsService
{
    Task<Sms> SendAsync(SendSmsEvent message);
    Task ApproveMessageAsync(Sms message);
}