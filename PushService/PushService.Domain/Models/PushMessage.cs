namespace PushService.Domain.Models;

public class PushMessage
{
    public Guid Id { get; set; }
    public string? Name  { get; set; }
    public string Email { get; set; } = null!;
    public string? Subject { get; set; }
    public string Body { get; set; } = null!;
} 