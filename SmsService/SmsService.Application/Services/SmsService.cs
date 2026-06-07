using SmsService.Application.Events;
using SmsService.Application.Interfaces.Messages;
using SmsService.Application.Interfaces.Services;
using SmsService.Application.Mappers;
using SmsService.Domain.Models;

namespace SmsService.Application.Services;

public class SmsService(
    ISmsSender smsSender,
    IMessageProducer<ApproveSmsEvent> approveProducer) : ISmsService
{
    public async Task<Sms> SendAsync(SendSmsEvent message)
    {
        var sms = message.ToSms();
        await smsSender.SendAsync(sms);
        
        return sms;
    }

    public async Task ApproveMessageAsync(Sms message)
    {
        var approveEvent = message.ToApproveSmsEvent();
        await approveProducer.ProduceAsync(approveEvent);
    }
}