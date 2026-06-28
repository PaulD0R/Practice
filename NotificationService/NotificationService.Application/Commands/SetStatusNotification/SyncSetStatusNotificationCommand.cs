using MediatR;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Commands.SetStatusNotification;

public record SyncSetStatusNotificationCommand(Guid NotificationId, NotificationStatus Status) : IRequest;