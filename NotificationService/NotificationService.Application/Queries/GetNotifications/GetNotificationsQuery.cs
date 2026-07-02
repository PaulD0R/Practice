using MediatR;
using NotificationService.Application.DTOs;

namespace NotificationService.Application.Queries.GetNotifications;

public record GetNotificationsQuery(HelpNotificationDto HelpNotificationDto) : IRequest<IEnumerable<NotificationDto>>;