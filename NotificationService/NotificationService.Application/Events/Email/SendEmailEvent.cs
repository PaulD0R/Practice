namespace NotificationService.Application.Events.Email;

public record SendEmailEvent(Guid NotificationId, string Address, string Text);