using PushService.Infrastructure.KafkaProducers;
using PushService.Infrastructure.Options;
using PushService.API.KafkaConsumers;
using PushService.API.Options;
using PushService.Application.Interfaces.Messages;
using PushService.Application.Interfaces.Services;
using PushService.Infrastructure.PushSender;

namespace PushService.API.Extensions;

public static class ProgramExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddConsumer<TMessage, THandler>(IConfigurationSection configuration)
            where THandler : class, IMessageHandler<TMessage>
        {
            services.Configure<KafkaConsumerOptions>(typeof(TMessage).Name, configuration);
            services.AddSingleton<IMessageHandler<TMessage>, THandler>();
            services.AddHostedService<KafkaMessageConsumer<TMessage>>();
        
            return services;
        }

        public IServiceCollection AddProducer<TMessage>(IConfigurationSection configuration)
        {
            services.Configure<KafkaProducerOptions>(typeof(TMessage).Name, configuration);
            services.AddSingleton<IMessageProducer<TMessage>, KafkaMessageProducer<TMessage>>();
        
            return services;
        }

        public IServiceCollection AddServices()
        {
            services.AddScoped<IPushService, PushService.Application.Services.PushService>();
            
            return services;
        }

        public IServiceCollection AddPushSender(IConfigurationSection configuration)
        {
            services.Configure<PushOptions>(configuration);
            services.AddScoped<IPushSender, PushSender>();

            return services;
        }
    }
}