using NotificationService.Domain.Enums;

namespace NotificationService.Application.Events;

public abstract record ErrorEvent(Guid NotificationId, string Message, NotificationChannel Channel);