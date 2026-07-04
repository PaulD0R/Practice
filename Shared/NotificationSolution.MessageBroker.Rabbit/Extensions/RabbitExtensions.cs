using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationSolution.MessageBroker.Abstraction;
using NotificationSolution.MessageBroker.Rabbit.Consumer;
using NotificationSolution.MessageBroker.Rabbit.Producer;

namespace NotificationSolution.MessageBroker.Rabbit.Extensions;

public static class RabbitExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRabbitProducer<TMessage>(IConfigurationSection configuration)
        {
            services.Configure<RabbitProducerOptions>(typeof(TMessage).Name, configuration);
            services.AddSingleton<IMessageProducer<TMessage>, RabbitMessageProducer<TMessage>>();
        
            return services;
        }
        
        public IServiceCollection AddRabbitConsumer<TMessage, THandler>(IConfigurationSection configuration)
            where THandler : class, IMessageHandler<TMessage>
        {
            services.Configure<RabbitConsumerOptions>(typeof(TMessage).Name, configuration);
            services.AddHostedService<RabbitMessageConsumer<TMessage>>();
            services.AddSingleton<IMessageHandler<TMessage>, THandler>();
    
            return services;
        }
    }
}