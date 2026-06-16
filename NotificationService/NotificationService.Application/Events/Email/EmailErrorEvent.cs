using NotificationService.Domain.Enums;

namespace NotificationService.Application.Events.Email;

public record EmailErrorEvent(Guid NotificationId, string Message) 
    : ErrorEvent(NotificationId, Message, NotificationChannel.Email);