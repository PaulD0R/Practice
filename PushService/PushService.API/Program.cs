using PushService.API.Extensions;
using PushService.API.KafkaConsumers.Handlers;
using PushService.Application.Events;

var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddServices().AddHttpClient()
        .AddPushSender(builder.Configuration.GetSection("OneSignal"))
        .AddConsumer<SendPushEvent, SendPushEventHandler>(builder.Configuration.GetSection("Kafka:SendPushEvent"))
        .AddProducer<ApprovePushEvent>(builder.Configuration.GetSection("Kafka:ApprovePushEvent"))
        .AddProducer<ErrorPushEvent>(builder.Configuration.GetSection("Kafka:ErrorPushEvent"));

var host = builder.Build();
host.Run();