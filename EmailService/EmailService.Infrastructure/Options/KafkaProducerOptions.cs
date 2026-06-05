namespace EmailService.Infrastructure.Options;

public record KafkaProducerOptions
{
    public string? BootstrapServers  { get; set; }
    public string? Topic { get; set; }
}