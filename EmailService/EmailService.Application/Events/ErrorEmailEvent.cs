namespace EmailService.Application.Events;

public record ErrorEmailEvent(Guid NotificationId, string Message);