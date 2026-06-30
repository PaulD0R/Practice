using System.Net;
using SmsService.Domain.Models;

namespace SmsService.Application.Interfaces.Services;

public interface ISmsSender
{
    Task<HttpResponseMessage> SendAsync(Sms message);
}