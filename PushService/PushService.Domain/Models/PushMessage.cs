namespace PushService.Domain.Models;

public class PushMessage
{
    public Guid Id { get; set; }
    public string Address { get; set; } = null!;
    public string Body { get; set; } = null!;
} 