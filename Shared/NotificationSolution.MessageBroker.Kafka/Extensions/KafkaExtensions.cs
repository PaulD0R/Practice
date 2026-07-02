using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationSolution.MessageBroker.Abstraction;
using NotificationSolution.MessageBroker.Kafka.Consumer;
using NotificationSolution.MessageBroker.Kafka.Producer;

namespace NotificationSolution.MessageBroker.Kafka.Extensions;

public static class KafkaExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddKafkaProducer<TMessage>(IConfigurationSection configuration)
        {
            services.Configure<KafkaProducerOptions>(typeof(TMessage).Name, configuration);
            services.AddSingleton<IMessageProducer<TMessage>, KafkaMessageProducer<TMessage>>();
        
            return services;
        }
        
        public IServiceCollection AddKafkaConsumer<TMessage, THandler>(IConfigurationSection configuration)
            where THandler : class, IMessageHandler<TMessage>
        {
            services.Configure<KafkaConsumerOptions>(typeof(TMessage).Name, configuration);
            services.AddHostedService<KafkaMessageConsumer<TMessage>>();
            services.AddSingleton<IMessageHandler<TMessage>, THandler>();

            return services;
        }
    }
}