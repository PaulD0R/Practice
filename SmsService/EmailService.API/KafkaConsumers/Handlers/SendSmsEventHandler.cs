using SmsService.Application.Events;
using SmsService.Application.Interfaces.Messages;
using SmsService.Application.Interfaces.Services;

namespace EmailService.API.KafkaConsumers.Handlers;

public class SendSmsEventHandler(IServiceScopeFactory scopeFactory) : IMessageHandler<SendSmsEvent>
{
    public async Task HandleAsync(SendSmsEvent message, CancellationToken token)
    {
        using var scope = scopeFactory.CreateScope();
        var smsService = scope.ServiceProvider.GetRequiredService<ISmsService>();
        var sms = await smsService.SendAsync(message);
        await smsService.ApproveMessageAsync(sms); 
    }
}