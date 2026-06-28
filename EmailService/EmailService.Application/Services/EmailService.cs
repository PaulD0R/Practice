using EmailService.Application.Events;
using EmailService.Application.Interfaces.Caching;
using EmailService.Application.Interfaces.Messages;
using EmailService.Application.Interfaces.Repositories;
using EmailService.Application.Interfaces.Services;
using EmailService.Application.Mappers;
using EmailService.Domain.Exceptions;
using EmailService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EmailService.Application.Services;

public class EmailService(
    ISmtpRepository smtpRepository,
    IEmailSender sender, 
    IMessageProducer<ApproveEmailEvent> approveProducer,
    IMessageProducer<ErrorEmailEvent> errorProducer,
    ICachingService cachingService,
    ILogger<EmailService> logger) 
    : IEmailService
{
    private const string SmtpKey = "smtp-options";
    
    public async Task<EmailMessage> SendAsync(SendEmailEvent message)
    {
        var emailMessage = message.ToEmailMessage();
        try
        {
            var options = await cachingService.GetAsync<SmtpOptions>(SmtpKey);
            if (options == null) throw new NullReferenceException("Smtp options not found");
            return await TrySendAsync(emailMessage, options);
        }
        catch
        {
            var optionsList = await smtpRepository.GetSmtpOptionsAsync();
            foreach (var option in optionsList)
            {
                try
                {
                    var result = await TrySendAsync(emailMessage, option);
                    await cachingService.SetAsync(SmtpKey, result);
                    return result;
                }
                catch(Exception e)
                {
                    logger.LogError(e, "Failed send to {host}: {error}", option.Host, e.Message);
                }
            }

            await errorProducer.ProduceAsync(new ErrorEmailEvent(message.NotificationId, "Failed to send message"));
            logger.LogError("Failed to send message");
            throw new InternalServerException("Failed to send message");
        }
    }

    public async Task ApproveMessageAsync(EmailMessage message)
    {
        var approveEvent = message.ToApproveMessageEvent();
        await approveProducer.ProduceAsync(approveEvent);
    }

    private async Task<EmailMessage> TrySendAsync(EmailMessage emailMessage, SmtpOptions options)
    {
        try
        {    
            await sender.SendAsync(emailMessage, options);
            return emailMessage;
        }
        catch (Exception e)
        {
            await errorProducer.ProduceAsync(new ErrorEmailEvent(emailMessage.Id,  e.Message));
            logger.LogError(e, e.Message);
            throw new InternalServerException("Failed to send message");
        }
    }
}