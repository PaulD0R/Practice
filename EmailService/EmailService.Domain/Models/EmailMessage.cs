namespace EmailService.Domain.Models;

public class EmailMessage
{
    public Guid Id { get; set; }
    public string? Name  { get; set; }
    public string Email { get; set; } = null!;
    public string? Subject { get; set; }
    public string Body { get; set; } = null!;
} 