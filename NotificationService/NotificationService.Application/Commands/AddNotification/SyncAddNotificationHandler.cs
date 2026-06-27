using MediatR;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces.Repositories;
using NotificationService.Application.Mappers;
using NotificationService.Domain.Exceptions;

namespace NotificationService.Application.Commands.AddNotification;

public class SyncAddNotificationHandler(
    INotificationReadRepository notificationReadRepository,
    ILogger<AddNotificationHandler> logger) 
    : IRequestHandler<SyncAddNotificationCommand>
{
    public async Task Handle(SyncAddNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await notificationReadRepository.AddNotificationAsync(request.ToNotification());
        }
        catch(Exception e)
        {
            logger.LogError(e, "Error saving notification: {message}", e.Message);
            throw new InternalServerException("Error saving notification");
        }
    }
}