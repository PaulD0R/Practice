namespace NotificationService.Application.Events.Push;

public record SendPushEvent(string Address, string Text);