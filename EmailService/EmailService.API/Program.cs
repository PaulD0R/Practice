using EmailService.API.Extensions;
using EmailService.API.KafkaConsumers.Handlers;
using EmailService.Application.Events;
using NotificationSolution.MessageBroker.Kafka.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddServices()
    .AddDbSettings(builder.Configuration)
    .AddMailKitService()
    .AddKafkaConsumer<SendEmailEvent, SendEmailEventHandler>(builder.Configuration.GetSection("Kafka:SendEmailEvent"))
    .AddKafkaProducer<ApproveEmailEvent>(builder.Configuration.GetSection("Kafka:ApproveEvent"))
    .AddKafkaProducer<ErrorEmailEvent>(builder.Configuration.GetSection("Kafka:ErrorEvent"));

var host = builder.Build();
host.Run();