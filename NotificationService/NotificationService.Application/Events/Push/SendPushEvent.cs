namespace NotificationService.Application.Events.Push;

public record SendPushEvent(Guid NotificationId, string Address, string Text);