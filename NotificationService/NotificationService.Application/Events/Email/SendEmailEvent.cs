namespace NotificationService.Application.Events.Email;

public record SendEmailEvent(string Email, string Subject, string Text);