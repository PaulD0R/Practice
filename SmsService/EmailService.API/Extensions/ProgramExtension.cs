using EmailService.API.KafkaConsumers;
using EmailService.API.Options;
using SmsService.Application.Interfaces.Messages;
using SmsService.Application.Interfaces.Services;
using SmsService.Infrastructure.KafkaProducers;
using SmsService.Infrastructure.Options;
using SmsService.Infrastructure.SmsRu;

namespace EmailService.API.Extensions;

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
            services.AddHttpClient();
            
            services.AddScoped<ISmsService, SmsService.Application.Services.SmsService>();
            
            return services;
        }

        public IServiceCollection AddSmsService(IConfigurationSection configuration)
        {
            services.Configure<SmsRuOptions>(configuration);
            services.AddScoped<ISmsSender, SmsRuSender>();

            return services;
        }
    }
}