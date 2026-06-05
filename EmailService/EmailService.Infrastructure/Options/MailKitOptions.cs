namespace EmailService.Infrastructure.Options;

public record MailKitOptions(
    string DisplayEmail,
    string Name,
    string Host,
    int Port,
    string RealEmail,
    string Password);