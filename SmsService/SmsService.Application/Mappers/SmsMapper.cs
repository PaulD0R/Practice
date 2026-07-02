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
}