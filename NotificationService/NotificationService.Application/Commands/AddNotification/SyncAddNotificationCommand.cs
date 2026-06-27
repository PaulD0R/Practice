using MediatR;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Commands.AddNotification;

public record SyncAddNotificationCommand(
    Guid NotificationId, 
    string Address,
    string Text,
    NotificationStatus Status,
    NotificationChannel Channel,
    DateTime CreatedOn) 
    : IRequest;