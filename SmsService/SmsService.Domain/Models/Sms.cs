namespace SmsService.Domain.Models;

public class Sms
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Message { get; set; } = null!;
}