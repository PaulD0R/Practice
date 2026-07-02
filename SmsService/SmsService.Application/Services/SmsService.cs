using Microsoft.Extensions.Logging;
using NotificationSolution.MessageBroker.Abstraction;
using SmsService.Application.Events;
using SmsService.Application.Interfaces.Services;
using SmsService.Application.Mappers;
using SmsService.Domain.Exeptions;
using SmsService.Domain.Models;

namespace SmsService.Application.Services;

public class SmsService(
    ISmsSender smsSender,
    IMessageProducer<ApproveSmsEvent> approveProducer,
    IMessageProducer<ErrorSmsEvent> errorProducer,
    ILogger<SmsService> logger) 
    : ISmsService
{
    public async Task<Sms> SendSmsAsync(SendSmsEvent message)
    {
        try
        {
            var sms = message.ToSms();
            var status = await smsSender.SendAsync(sms);
            return status.IsSuccessStatusCode ? sms : throw new InternalServerException(status.StatusCode.ToString());
        }
        catch(Exception e)
        {
            logger.LogError(e, "Failed to send sms: {Message}", e.Message);
            throw new InternalServerException(e.Message);
        }
    }

    public async Task SendApproveMessageAsync(Sms message)
    {
        var approveEvent = message.ToApproveSmsEvent();
        await approveProducer.ProduceAsync(approveEvent);
    }
    
    public async Task SendErrorMessageAsync(Guid messageId, string errorMessage)
    {
        await errorProducer.ProduceAsync(new ErrorSmsEvent(messageId, errorMessage));
    }
}