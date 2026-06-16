using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Models;

public class Notification
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;
    
    public string? Subject { get; set; }
    public string? Email { get; set; }
    public NotificationStatus EmailStatus { get; set; }
    
    public string? Phone { get; set; }
    public NotificationStatus SmsStatus { get; set; }
    
    public string? Push {get; set; }
    public NotificationStatus PushStatus { get; set; }
    
    public DateTime CreatedOn { get; set; }
}