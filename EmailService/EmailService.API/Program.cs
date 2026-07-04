using EmailService.API.Extensions;
using EmailService.API.KafkaConsumers.Handlers;
using EmailService.Application.Events;
using NotificationSolution.MessageBroker.Kafka.Extensions;
using NotificationSolution.MessageBroker.Rabbit.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddServices()
    .AddMailKitService()
    .AddDbSettings(builder.Configuration)
    .AddServiceOptions(builder.Configuration)
    .AddKafkaConsumer<SendEmailEvent, SendEmailEventHandler>(builder.Configuration.GetSection("Kafka:SendEmailEvent"))
    .AddRabbitConsumer<RetrySendEmailEvent, RetrySendEmailEventHandler>(builder.Configuration.GetSection("Rabbit:RetrySendEvent"))
    .AddKafkaProducer<ApproveEmailEvent>(builder.Configuration.GetSection("Kafka:ApproveEvent"))
    .AddKafkaProducer<ErrorEmailEvent>(builder.Configuration.GetSection("Kafka:ErrorEvent"))
    .AddRabbitProducer<RetrySendEmailEvent>(builder.Configuration.GetSection("Rabbit:RetrySendEvent"));

var host = builder.Build();
host.Run();