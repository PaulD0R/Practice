using PushService.Application.Events;
using PushService.Domain.Models;

namespace PushService.Application.Interfaces.Services;

public interface IPushService
{
    Task<PushMessage> SendAsync(SendPushEvent message);
    Task ApproveMessageAsync(PushMessage message);
}