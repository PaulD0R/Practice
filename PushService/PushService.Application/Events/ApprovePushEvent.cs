namespace PushService.Application.Events;

public record ApprovePushEvent(
    Guid MessageId);