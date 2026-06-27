namespace NotificationService.Application.Events.Sms;

public record SendSmsEvent(string Address, string Text);