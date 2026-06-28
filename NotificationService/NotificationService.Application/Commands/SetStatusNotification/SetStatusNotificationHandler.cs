using MediatR;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Events;
using NotificationService.Application.Interfaces.Messages;
using NotificationService.Application.Interfaces.Repositories;

namespace NotificationService.Application.Commands.SetStatusNotification;

public class SetStatusNotificationHandler(
    INotificationWriteRepository notificationWriteRepository, 
    IMessageProducer<SetStatusEvent> setStatusProducer,
    ILogger<SetStatusNotificationHandler> logger)
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
        
        await setStatusProducer.ProduceAsync(new SetStatusEvent(request.NotificationId, request.Status), cancellationToken);
    }
}