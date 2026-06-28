namespace NotificationService.Application.Events.Sms;   

public record SendSmsEvent(Guid NotificationId, string Address, string Text);