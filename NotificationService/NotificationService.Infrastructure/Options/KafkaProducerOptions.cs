namespace NotificationService.Infrastructure.Options;

public class KafkaProducerOptions
{
    public string? BootstrapServers { get; set; }
    public string? Topic { get; set; }
}