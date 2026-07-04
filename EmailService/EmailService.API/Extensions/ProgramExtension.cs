using EmailService.API.Options;
using EmailService.Application.Interfaces.Caching;
using EmailService.Application.Interfaces.Providers;
using EmailService.Application.Interfaces.Repositories;
using EmailService.Application.Interfaces.Services;
using EmailService.Application.Providers;
using EmailService.Infrastructure.Context;
using EmailService.Infrastructure.MailKit;
using EmailService.Infrastructure.Redis;
using EmailService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmailService.API.Extensions;

public static class ProgramExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDbSettings(IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Postgres")));
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "EmailService:"; 
            });

            return services;
        }

        public IServiceCollection AddServiceOptions(IConfiguration configuration)
        {
            services.Configure<RetryOptions>(configuration.GetSection("RetryOptions"));
            
            return services;
        }
        
        public IServiceCollection AddServices()
        {
            services.AddScoped<ISmtpRepository, SmtpRepository>();
            services.AddScoped<ICachingService, RedisCachingService>();
            services.AddScoped<ISmtpProvider, SmtpProvider>();
            services.AddScoped<IEmailService, Application.Services.EmailService>();
            
            return services;
        }

        public IServiceCollection AddMailKitService()
        {
            services.AddScoped<IEmailSender, MailKitSender>();

            return services;
        }
    }
}