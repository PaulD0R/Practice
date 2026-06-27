using MediatR;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces.Repositories;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Commands.SetStatusNotification;

public class ApproveNotificationHandler(
    INotificationWriteRepository notificationWriteRepository, 
    ILogger<ApproveNotificationHandler> logger)
    : IRequestHandler<SetStatusNotificationCommand>
{
    public async Task Handle(SetStatusNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await notificationWriteRepository.UpdateStatusAsync(request.NotificationId, request.Status);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update status for notification {NotificationId}", request.NotificationId);
            throw; 
        }
    }
}