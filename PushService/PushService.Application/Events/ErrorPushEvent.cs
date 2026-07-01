namespace PushService.Application.Events;

public record ErrorPushEvent(Guid NotificationId, string Message);