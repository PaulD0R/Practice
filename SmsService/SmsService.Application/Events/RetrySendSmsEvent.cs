namespace SmsService.Application.Events;

public record RetrySendSmsEvent(
    Guid NotificationId,
    string Address,
    string Text,
    int RetryNumber);