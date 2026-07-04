namespace EmailService.Application.Events;

public record RetrySendEmailEvent(
    Guid NotificationId,
    string Address,
    string Text,
    int RetryNumber);