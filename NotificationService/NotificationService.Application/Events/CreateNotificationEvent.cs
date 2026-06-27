using NotificationService.Domain.Enums;

namespace NotificationService.Application.Events;

public record CreateNotificationEvent(
    Guid NotificationId, 
    string Address,
    string Text,
    NotificationStatus Status,
    NotificationChannel Channel,
    DateTime CreatedOn);