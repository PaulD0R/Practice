namespace NotificationService.Application.Events;

public record ErrorEvent(Guid NotificationId, string Message);