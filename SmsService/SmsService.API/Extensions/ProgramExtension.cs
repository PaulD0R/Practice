using SmsService.Application.Interfaces.Services;
using SmsService.Infrastructure.Options;
using SmsService.Infrastructure.SmsRu;

namespace SmsService.API.Extensions;

public static class ProgramExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServices()
        {
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