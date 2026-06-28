using EmailService.API.Extensions;
using EmailService.API.KafkaConsumers.Handlers;
using EmailService.Application.Events;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddServices()
    .AddDbSettings(builder.Configuration)
    .AddMailKitService()
    .AddConsumer<SendEmailEvent, SendEmailEventHandler>(builder.Configuration.GetSection("Kafka:SendEmailEvent"))
    .AddProducer<ApproveEmailEvent>(builder.Configuration.GetSection("Kafka:ApproveEvent"))
    .AddProducer<ErrorEmailEvent>(builder.Configuration.GetSection("Kafka:ErrorEvent"));

var host = builder.Build();
host.Run();