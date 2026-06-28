namespace EmailService.Domain.Models;

public record SmtpOptions
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string RealEmail { get; set; } = null!;
    public string Password { get; set; } = null!;
}