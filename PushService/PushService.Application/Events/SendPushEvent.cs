namespace PushService.Application.Events;

public record SendPushEvent(
    Guid NotificationId,
    string Address,
    string Text);