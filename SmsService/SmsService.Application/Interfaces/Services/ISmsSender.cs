using SmsService.Domain.Models;

namespace SmsService.Application.Interfaces.Services;

public interface ISmsSender
{
    Task<bool> SendAsync(Sms message);
}