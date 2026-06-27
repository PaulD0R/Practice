namespace NotificationService.Application.Events.Email;

public record SendEmailEvent(string Address, string Text);