using NotificationSolution.MessageBroker.Abstraction;
using SmsService.Application.Events;
using SmsService.Application.Interfaces.Services;
using SmsService.Application.Mappers;

namespace SmsService.API.KafkaConsumers.Handlers;

public class SendSmsEventHandler(IServiceScopeFactory scopeFactory) : IMessageHandler<SendSmsEvent>
{
    public async Task HandleAsync(SendSmsEvent message, CancellationToken token = default)
    {
        using var scope = scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ISmsService>();
        try
        {
            var smsMessage = await service.SendSmsAsync(message);
            await service.SendApproveMessageAsync(smsMessage);
        }
        catch(Exception e)
        {
            await service.SendRetryMessageAsync(message.ToRetrySendSmsEvent(), TimeSpan.FromSeconds(1));
        }
    }
}