using System.Security.Cryptography;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using NotificationService.Application.Commands.AddNotification;
using NotificationService.Application.Interfaces.Messages;
using NotificationService.Application.Interfaces.Repositories;
using NotificationService.Domain.Enums;
using NotificationService.Infrastructure.BackgroundServices;
using NotificationService.Infrastructure.Context;
using NotificationService.Infrastructure.Kafka;
using NotificationService.Infrastructure.Options;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.API.Extensions;

public static class ProgramExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructureStorage(IConfiguration configuration)
        {
            services.AddDbContext<WriteDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("WritePostgres")));
            services.AddDbContext<ReadDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("ReadPostgres")));

            services.AddSingleton<IMongoClient>(_ => 
                new MongoClient(configuration.GetConnectionString("Mongo")));
            BsonSerializer.RegisterSerializer(new EnumSerializer<NotificationStatus>(BsonType.String));
            
            services.Configure<MongoOptions>(configuration.GetSection("Mongo"));

            services.AddScoped<INotificationWriteRepository, NotificationWriteRepository>();
            services.AddScoped<INotificationReadRepository, NotificationReadRepository>();
            services.AddScoped<IArchiveRepository, ArchiveRepository>();

            return services;
        }

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


        public IServiceCollection AddApplicationServices(IConfiguration configuration)
        {
            services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssembly(typeof(AddNotificationCommand).Assembly);
            });
            
            services.Configure<ArchiveOptions>(configuration.GetSection("Archive"));
            services.AddHostedService<ArchiveBackgroundService>();

            return services;
        }

        public IServiceCollection AddJwtAuthentication(IConfiguration configuration)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var rsa = RSA.Create();
                
                    var publicKeyPem = configuration.GetSection("Jwt:PublicKey").Value 
                                       ?? throw new InvalidOperationException("JWT Public Key is missing in configuration.");
                
                    rsa.ImportFromPem(publicKeyPem);
                
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration.GetRequiredSection("Jwt:Issuer").Value,
                        ValidateAudience = true,
                        ValidAudience = configuration.GetRequiredSection("Jwt:Audience").Value,
                        ValidateLifetime = true,
                        IssuerSigningKey = new RsaSecurityKey(rsa),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddAuthorization();
            return services;
        }

        public IServiceCollection AddSwaggerWithSecurity()
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });

            });

            return services;
        }

        public IServiceCollection AddRequestLimit()
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("NotificationPolicy", fixedOptions =>
                {
                    fixedOptions.PermitLimit = 10;           
                    fixedOptions.Window = TimeSpan.FromSeconds(10); 
                    fixedOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    fixedOptions.QueueLimit = 5;           
                });

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            return services;
        }
    }
}