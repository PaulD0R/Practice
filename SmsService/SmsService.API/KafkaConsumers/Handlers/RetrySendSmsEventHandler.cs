using Microsoft.Extensions.Options;
using NotificationSolution.MessageBroker.Abstraction;
using SmsService.API.Options;
using SmsService.Application.Events;
using SmsService.Application.Interfaces.Services;
using SmsService.Application.Mappers;

namespace SmsService.API.KafkaConsumers.Handlers;

public class RetrySendEmailEventHandler(
    IServiceScopeFactory scopeFactory, 
    IOptions<RetryOptions> options) 
    : IMessageHandler<RetrySendSmsEvent>
{
    private readonly RetryOptions _options = options.Value;
    
    public async Task HandleAsync(RetrySendSmsEvent message, CancellationToken token = default)
    {
        using var scope = scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ISmsService>();
        try
        {
            var emailMessage = await service.SendSmsAsync(message.ToSendSmsEvent());
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
            await service.SendRetryMessageAsync(message.ToNewRetrySendSmsEvent(), TimeSpan.FromSeconds(delay));
        }
    }
}