using EmailService.API.Options;
using EmailService.Application.Events;
using EmailService.Application.Interfaces.Services;
using EmailService.Application.Mappers;
using Microsoft.Extensions.Options;
using NotificationSolution.MessageBroker.Abstraction;

namespace EmailService.API.KafkaConsumers.Handlers;

public class RetrySendEmailEventHandler(
    IServiceScopeFactory scopeFactory, 
    IOptions<RetryOptions> options) 
    : IMessageHandler<RetrySendEmailEvent>
{
    private readonly RetryOptions _options = options.Value;
    
    public async Task HandleAsync(RetrySendEmailEvent message, CancellationToken token = default)
    {
        using var scope = scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IEmailService>();
        try
        {
            var emailMessage = await service.SendEmailAsync(message.ToSendEmailEvent());
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
            await service.SendRetryMessageAsync(message.ToNewRetrySendEmailEvent(), TimeSpan.FromSeconds(delay));
        }
    }
}