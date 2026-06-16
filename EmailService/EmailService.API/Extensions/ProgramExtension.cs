using EmailService.API.KafkaConsumers;
using EmailService.API.Options;
using EmailService.Application.Factories;
using EmailService.Application.Interfaces.Factories;
using EmailService.Application.Interfaces.Messages;
using EmailService.Application.Interfaces.Services;
using EmailService.Infrastructure.KafkaProducers;
using EmailService.Infrastructure.MailKit;
using EmailService.Infrastructure.Options;

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
            services.AddSingleton<IFactory<IEmailService>, EmailServiceFactory>();
            
            services.AddScoped<IEmailService, Application.Services.EmailService>();
            
            return services;
        }

        public IServiceCollection AddMailKitService(IConfigurationSection configuration)
        {
            services.Configure<MailKitOptions>(configuration);
            services.AddScoped<IEmailSender, MailKitSender>();

            return services;
        }
    }
}