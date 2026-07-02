namespace NotificationSolution.MessageBroker.Kafka.Producer;

public class KafkaProducerOptions
{
    public string? BootstrapServers { get; set; }
    public string? Topic { get; set; }
}