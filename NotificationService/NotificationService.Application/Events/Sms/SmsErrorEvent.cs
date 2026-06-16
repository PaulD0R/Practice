using NotificationService.Domain.Enums;

namespace NotificationService.Application.Events.Sms;

public record SmsErrorEvent(Guid NotificationId, string Message) 
    : ErrorEvent(NotificationId, Message, NotificationChannel.Sms);