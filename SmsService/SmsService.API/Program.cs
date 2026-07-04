using NotificationSolution.MessageBroker.Kafka.Extensions;
using NotificationSolution.MessageBroker.Rabbit.Extensions;
using SmsService.API.Extensions;
using SmsService.API.KafkaConsumers.Handlers;
using SmsService.Application.Events;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient().AddServices()
    .AddServiceOptions(builder.Configuration)
    .AddSmsService(builder.Configuration.GetSection("SmsOptions"))
    .AddKafkaConsumer<SendSmsEvent, SendSmsEventHandler>(builder.Configuration.GetSection("Kafka:SendSmsEvent"))
    .AddRabbitConsumer<RetrySendSmsEvent, RetrySendEmailEventHandler>(builder.Configuration.GetSection("Rabbit:RetrySendEvent"))
    .AddKafkaProducer<ApproveSmsEvent>(builder.Configuration.GetSection("Kafka:ApproveSmsEvent"))
    .AddKafkaProducer<ErrorSmsEvent>(builder.Configuration.GetSection("Kafka:ErrorSmsEvent"))
    .AddRabbitProducer<RetrySendSmsEvent>(builder.Configuration.GetSection("Rabbit:RetrySendEvent"));

var host = builder.Build();
host.Run();