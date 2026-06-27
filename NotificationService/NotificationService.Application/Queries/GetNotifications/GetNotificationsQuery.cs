using MediatR;
using NotificationService.Application.DTOs;

namespace NotificationService.Application.Queries.GetNotifications;

public record GetNotificationsQuery() : IRequest<IEnumerable<NotificationDto>>;