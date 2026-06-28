namespace EmailService.Application.Events;

public record SendEmailEvent(
    Guid NotificationId,
    string Address,
    string Text);