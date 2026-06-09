using PushService.API.Extensions;
using PushService.API.KafkaConsumers.Handlers;
using PushService.Application.Events;

var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddServices()
        .AddMailKitService(builder.Configuration.GetSection("MailKit"))
        .AddConsumer<SendPushEvent, SendPushEventHandler>(builder.Configuration.GetSection("Kafka:SendPushEvent"))
        .AddProducer<ApprovePushEvent>(builder.Configuration.GetSection("Kafka:ApprovePushEvent"));

var host = builder.Build();
host.Run();