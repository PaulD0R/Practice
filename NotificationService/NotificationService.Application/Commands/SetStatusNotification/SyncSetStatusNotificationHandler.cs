using MediatR;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces.Repositories;

namespace NotificationService.Application.Commands.SetStatusNotification;

public class SyncSetStatusNotificationHandler(
    INotificationReadRepository notificationReadRepository,
    ILogger<SyncSetStatusNotificationHandler> logger)
    : IRequestHandler<SyncSetStatusNotificationCommand>
{
    public async Task Handle(SyncSetStatusNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await notificationReadRepository.UpdateStatusAsync(request.NotificationId, request.Status);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update status for notification {NotificationId}", request.NotificationId);
            throw; 
        }
    }
}