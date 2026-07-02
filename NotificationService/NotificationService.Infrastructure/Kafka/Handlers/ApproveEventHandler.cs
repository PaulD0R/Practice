using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Commands.SetStatusNotification;
using NotificationService.Application.Events;
using NotificationService.Domain.Enums;
using NotificationSolution.MessageBroker.Abstraction;

namespace NotificationService.Infrastructure.Kafka.Handlers;

public class ApproveEventHandler(
    IServiceScopeFactory scopeFactory, 
    ILogger<ErrorEventHandler> logger)
    : IMessageHandler<ApproveEvent>
{
    public async Task HandleAsync(ApproveEvent message, CancellationToken token = default)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new SetStatusNotificationCommand(message.NotificationId, NotificationStatus.Approve), token);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to update approve status for notification {NotificationId}", message.NotificationId);
            throw; 
        }
    }
}