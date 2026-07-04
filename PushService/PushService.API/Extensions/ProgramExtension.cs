using PushService.API.Options;
using PushService.Infrastructure.Options;
using PushService.Application.Interfaces.Services;
using PushService.Infrastructure.PushSender;

namespace PushService.API.Extensions;

public static class ProgramExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServiceOptions(IConfiguration configuration)
        {
            services.Configure<RetryOptions>(configuration.GetSection("RetryOptions"));

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