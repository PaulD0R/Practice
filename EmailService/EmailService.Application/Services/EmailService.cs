using EmailService.Application.Events;
using EmailService.Application.Interfaces.Providers;
using EmailService.Application.Interfaces.Services;
using EmailService.Application.Mappers;
using EmailService.Domain.Exceptions;
using EmailService.Domain.Models;
using Microsoft.Extensions.Logging;
using NotificationSolution.MessageBroker.Abstraction;

namespace EmailService.Application.Services;

public class EmailService(
    IEmailSender sender, 
    ISmtpProvider smtpProvider,
    IMessageProducer<ApproveEmailEvent> approveProducer,
    IMessageProducer<ErrorEmailEvent> errorProducer,
    IMessageProducer<RetrySendEmailEvent> retryProducer,
    ILogger<EmailService> logger) 
    : IEmailService
{
    public async Task<EmailMessage> SendEmailAsync(SendEmailEvent message)
    {
        var emailMessage = message.ToEmailMessage();
        var options = await smtpProvider.GetSmtpOptionsAsync();
        foreach (var option in options)
        {
            try
            {
                await sender.SendAsync(emailMessage, option);
                return emailMessage;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed send to {host}: {error}", option.Host, e.Message);
            }
        }
        
        logger.LogError("Failed to send message");
        throw new InternalServerException("Failed to send message");
    }

    public async Task SendRetryMessageAsync(RetrySendEmailEvent message, TimeSpan delay)
    {
        await retryProducer.ProduceAsync(message, CancellationToken.None, delay);
    }

    public async Task SendApproveMessageAsync(EmailMessage message)
    {
        var approveEvent = message.ToApproveMessageEvent();
        await approveProducer.ProduceAsync(approveEvent);
    }

    public async Task SendErrorMessageAsync(Guid messageId, string errorMessage)
    {
        await errorProducer.ProduceAsync(new ErrorEmailEvent(messageId, errorMessage));
    }
}