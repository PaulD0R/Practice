using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Models;

public class Notification
{
    public Guid Id { get; set; }
    public string Address { get; set; } = null!;
    public string Text { get; set; } = null!;
    public NotificationStatus Status { get; set; }
    public NotificationChannel Channel { get; set; }
    public DateTime CreatedOn { get; set; }
}