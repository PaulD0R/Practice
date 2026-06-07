namespace SmsService.Application.Events;

public record SendSmsEvent(
    Guid Id,
    string PhoneNumber,
    string Message);