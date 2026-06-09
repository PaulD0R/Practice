namespace PushService.Application.Events;

public record SendPushEvent(
    Guid MessageId,
    string? Name,
    string Email,
    string? Subject,
    string Body);