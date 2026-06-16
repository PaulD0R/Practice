using SmsService.Application.Events;
using SmsService.Domain.Models;

namespace SmsService.Application.Mappers;

public static class SmsMapper
{
    public static Sms ToSms(this SendSmsEvent message) => new Sms
    {
        Id = message.Id,
        PhoneNumber = message.PhoneNumber,
        Message = message.Message
    };

    public static ApproveSmsEvent ToApproveSmsEvent(this Sms message) => new(message.Id);
}