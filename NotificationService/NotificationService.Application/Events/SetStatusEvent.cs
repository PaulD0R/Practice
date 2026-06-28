using NotificationService.Domain.Enums;

namespace NotificationService.Application.Events;

public record SetStatusEvent(Guid NotificationId, NotificationStatus Status);