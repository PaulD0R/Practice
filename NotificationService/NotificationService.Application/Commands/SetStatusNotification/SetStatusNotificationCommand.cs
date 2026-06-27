using MediatR;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Commands.SetStatusNotification;

public record SetStatusNotificationCommand(Guid NotificationId, NotificationStatus Status) : IRequest;