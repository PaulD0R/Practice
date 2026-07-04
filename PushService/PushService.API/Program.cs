using NotificationSolution.MessageBroker.Kafka.Extensions;
using NotificationSolution.MessageBroker.Rabbit.Extensions;
using PushService.API.Extensions;
using PushService.API.KafkaConsumers.Handlers;
using PushService.Application.Events;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddServices().AddHttpClient()
    .AddPushSender(builder.Configuration.GetSection("OneSignal"))
    .AddKafkaConsumer<SendPushEvent, SendPushEventHandler>(builder.Configuration.GetSection("Kafka:SendPushEvent"))
    .AddRabbitConsumer<RetrySendPushEvent, RetrySendPushEventHandler>(builder.Configuration.GetSection("Rabbit:RetrySendEvent"))
    .AddKafkaProducer<ApprovePushEvent>(builder.Configuration.GetSection("Kafka:ApprovePushEvent"))
    .AddKafkaProducer<ErrorPushEvent>(builder.Configuration.GetSection("Kafka:ErrorPushEvent"))
    .AddRabbitProducer<RetrySendPushEvent>(builder.Configuration.GetSection("Rabbit:RetrySendEvent"));

var host = builder.Build();
host.Run();