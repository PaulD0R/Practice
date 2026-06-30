using EmailService.API.Extensions;
using EmailService.API.KafkaConsumers.Handlers;
using SmsService.Application.Events;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddServices()
    .AddSmsService(builder.Configuration.GetSection("SmsOptions"))
    .AddConsumer<SendSmsEvent, SendSmsEventHandler>(builder.Configuration.GetSection("Kafka:SendSmsEvent"))
    .AddProducer<ApproveSmsEvent>(builder.Configuration.GetSection("Kafka:ApproveSmsEvent"))
    .AddProducer<ErrorSmsEvent>(builder.Configuration.GetSection("Kafka:ErrorSmsEvent"));

var host = builder.Build();
host.Run();