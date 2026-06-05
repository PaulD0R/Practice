namespace EmailService.Application.Events;

public record SendEmailEvent(
    Guid MessageId,
    string? Name,
    string Email,
    string? Subject,
    string Body);