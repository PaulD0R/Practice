using PushService.Domain.Models;

namespace PushService.Application.Interfaces.Services;

public interface IPushSender
{
    Task SendAsync(PushMessage message);
}