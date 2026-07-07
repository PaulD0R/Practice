using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using NotificationService.API.Exceptions;
using NotificationService.API.Extensions;
using NotificationService.Application.Events;
using NotificationService.Application.Events.Email;
using NotificationService.Application.Events.Push;
using NotificationService.Application.Events.Sms;
using NotificationService.Infrastructure.Kafka.Handlers;
using NotificationSolution.MessageBroker.Kafka.Extensions;
using Serilog;
using ErrorEventHandler = NotificationService.Infrastructure.Kafka.Handlers.ErrorEventHandler;

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

builder.Services
    .AddSwaggerWithSecurity()
    .AddInfrastructureStorage(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddApplicationServices(builder.Configuration)
    .AddRequestLimit();

builder.Services.AddKafkaProducer<SendEmailEvent>(builder.Configuration.GetSection("Kafka:SendEmailEvent"))
    .AddKafkaProducer<SendSmsEvent>(builder.Configuration.GetSection("Kafka:SendSmsEvent"))
    .AddKafkaProducer<SendPushEvent>(builder.Configuration.GetSection("Kafka:SendPushEvent"))
    .AddKafkaProducer<CreateNotificationEvent>(builder.Configuration.GetSection("Kafka:CreateNotificationEvent"))
    .AddKafkaProducer<SetStatusEvent>(builder.Configuration.GetSection("Kafka:SetStatusEvent"));

builder.Services
    .AddKafkaConsumer<ErrorEvent, ErrorEventHandler>(builder.Configuration.GetSection("Kafka:ErrorEvent"))
    .AddKafkaConsumer<ApproveEvent, ApproveEventHandler>(builder.Configuration.GetSection("Kafka:ApproveEvent"))
    .AddKafkaConsumer<SetStatusEvent, SetStatusEventHandler>(builder.Configuration.GetSection("Kafka:SetStatusEvent"))
    .AddKafkaConsumer<CreateNotificationEvent, CreateNotificationEventHandler>(
        builder.Configuration.GetSection("Kafka:CreateNotificationEvent"));
 
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "{RequestMethod} {RequestPath} responded {StatusCode}";
});
app.UseExceptionHandler();

app.MapControllers();

app.Run();