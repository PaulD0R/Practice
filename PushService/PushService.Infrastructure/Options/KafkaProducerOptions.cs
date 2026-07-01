namespace PushService.Infrastructure.Options;

public record KafkaProducerOptions
{
    public string? BootstrapServers  { get; set; }
    public string? Topic { get; set; }
}