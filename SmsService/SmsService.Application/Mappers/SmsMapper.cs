using SmsService.Application.Events;
using SmsService.Domain.Models;

namespace SmsService.Application.Mappers;

public static class SmsMapper
{
    public static Sms ToSms(this SendSmsEvent message) => 
        new() 
    {
        Id = message.NotificationId,
        PhoneNumber = message.Address,
        Message = message.Text
    };

    public static ApproveSmsEvent ToApproveSmsEvent(this Sms message) => new(message.Id);
    
    public static RetrySendSmsEvent ToRetrySendSmsEvent(this SendSmsEvent message) =>
        new(message.NotificationId, message.Address, message.Text, 1);
    
    public static SendSmsEvent ToSendSmsEvent(this RetrySendSmsEvent message) =>
        new(message.NotificationId, message.Address, message.Text);
    
    public static RetrySendSmsEvent ToNewRetrySendSmsEvent(this RetrySendSmsEvent message) =>
        new(message.NotificationId, message.Address, message.Text, message.RetryNumber + 1);
}