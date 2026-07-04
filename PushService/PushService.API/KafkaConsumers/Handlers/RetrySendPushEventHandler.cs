using Microsoft.Extensions.Options;
using NotificationSolution.MessageBroker.Abstraction;
using PushService.API.Options;
using PushService.Application.Events;
using PushService.Application.Interfaces.Services;
using PushService.Application.Mappers;

namespace PushService.API.KafkaConsumers.Handlers;

public class RetrySendPushEventHandler(
    IServiceScopeFactory scopeFactory, 
    IOptions<RetryOptions> options) 
    : IMessageHandler<RetrySendPushEvent>
{
    private readonly RetryOptions _options = options.Value;
    
    public async Task HandleAsync(RetrySendPushEvent message, CancellationToken token = default)
    {
        using var scope = scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IPushService>();
        try
        {
            var emailMessage = await service.SendPushAsync(message.ToSendPushEvent());
            await service.SendApproveMessageAsync(emailMessage);
        }
        catch(Exception e)
        {
            if (message.RetryNumber >= _options.MaxRetryCount)
            {
                await service.SendErrorMessageAsync(message.NotificationId, e.Message);
                return;
            }

            var delay = (int)Math.Pow(2, message.RetryNumber - 1) * 
                        (_options.StartRetryDelay ?? throw new NullReferenceException("None delay"));
            await service.SendRetryPushAsync(message.ToNewRetrySendPushEvent(), TimeSpan.FromSeconds(delay));
        }
    }
}