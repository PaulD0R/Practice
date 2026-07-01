using PushService.Domain.Models;

namespace PushService.Application.Interfaces.Services;

public interface IPushSender
{
    Task<HttpResponseMessage> SendAsync(PushMessage message);
}