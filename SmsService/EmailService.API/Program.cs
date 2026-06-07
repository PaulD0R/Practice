using EmailService.API.Extensions;
using EmailService.API.KafkaConsumers.Handlers;
using SmsService.Application.Events;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddServices()
    .AddSmsService(builder.Configuration.GetSection("SmsRu"))
    .AddConsumer<SendSmsEvent, SendSmsEventHandler>(builder.Configuration.GetSection("Kafka:SendSmsEvent"))
    .AddProducer<ApproveSmsEvent>(builder.Configuration.GetSection("Kafka:ApproveSmsEvent"));

var host = builder.Build();
host.Run();