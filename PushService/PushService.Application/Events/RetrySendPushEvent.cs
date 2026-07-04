namespace PushService.Application.Events;

public record RetrySendPushEvent(
    Guid NotificationId,
    string Address,
    string Text,
    int RetryNumber);