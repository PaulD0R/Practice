using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Events;
using NotificationService.Application.Interfaces.Messages;
using NotificationService.Application.Mappers;

namespace NotificationService.Infrastructure.Kafka.Handlers;

public class CreateNotificationEventHandler(
    IServiceScopeFactory scopeFactory,
    ILogger<CreateNotificationEventHandler> logger) 
    : IMessageHandler<CreateNotificationEvent>
{
    public async Task HandleAsync(CreateNotificationEvent message, CancellationToken token = default)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(message.ToSyncAddNotificationCommand(), token);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to sync notification {NotificationId}", message.NotificationId);
            throw; 
        }
    }
}