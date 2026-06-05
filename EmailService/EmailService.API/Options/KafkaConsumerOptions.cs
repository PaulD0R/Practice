namespace EmailService.API.Options;

public record KafkaConsumerOptions
{
    public string? BootstrapServers { get; set; }
    public string? Topic { get; set; }
    public string? GroupId { get; set; }
}