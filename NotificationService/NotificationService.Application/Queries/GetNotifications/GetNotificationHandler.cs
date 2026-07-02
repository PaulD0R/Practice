using MediatR;
using Microsoft.Extensions.Logging;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces.Repositories;
using NotificationService.Application.Mappers;
using NotificationService.Domain.Exceptions;

namespace NotificationService.Application.Queries.GetNotifications;

public class GetNotificationHandler(
    INotificationReadRepository notificationReadRepository,
    ILogger<GetNotificationHandler> logger)
    : IRequestHandler<GetNotificationsQuery, IEnumerable<NotificationDto>>
{
    public async Task<IEnumerable<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var notifications = await notificationReadRepository.
                GetNotificationsAsync(request.HelpNotificationDto);
            return notifications.Select(notification => notification.ToNotificationDto());
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting notifications");
            throw new InternalServerException("Error getting notifications");
        }
    }
}