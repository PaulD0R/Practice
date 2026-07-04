using PushService.Application.Events;
using PushService.Domain.Models;

namespace PushService.Application.Interfaces.Services;

public interface IPushService
{
    Task<PushMessage> SendPushAsync(SendPushEvent message);
    Task SendRetryPushAsync(RetrySendPushEvent message, TimeSpan delay);
    Task SendApproveMessageAsync(PushMessage message);
    Task SendErrorMessageAsync(Guid notificationId, string message);
}