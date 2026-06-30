namespace SmsService.Application.Events;

public record SendSmsEvent(
    Guid NotificationId,
    string Address,
    string Text);