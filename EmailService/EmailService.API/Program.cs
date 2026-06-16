using EmailService.API.Extensions;
using EmailService.API.KafkaConsumers.Handlers;
using EmailService.Application.Events;

var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddServices()
        .AddMailKitService(builder.Configuration.GetSection("MailKit"))
        .AddConsumer<SendEmailEvent, SendEmailEventHandler>(builder.Configuration.GetSection("Kafka:SendEmailEvent"))
        .AddProducer<ApproveEmailEvent>(builder.Configuration.GetSection("Kafka:ApproveEmailEvent"));

var host = builder.Build();
host.Run();