using NotificationService.Domain.Enums;

namespace NotificationService.Application.Events.Push;

public record PushErrorEvent(Guid NotificationId, string Message) 
    : ErrorEvent(NotificationId, Message, NotificationChannel.Push);