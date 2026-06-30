namespace SmsService.Application.Events;

public record ErrorSmsEvent(Guid NotificationId, string Message);