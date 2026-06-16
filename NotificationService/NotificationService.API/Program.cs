using NotificationService.API.Exceptions;
using NotificationService.API.Extensions;
using NotificationService.Application.Events.Email;
using NotificationService.Application.Events.Push;
using NotificationService.Application.Events.Sms;
using NotificationService.Infrastructure.Kafka.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

builder.Services
    .AddSwaggerWithSecurity()
    .AddInfrastructureStorage(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddApplicationServices(builder.Configuration);

builder.Services.AddKafkaProducer<SendEmailEvent>(builder.Configuration.GetSection("Kafka:SendEmailEvent"))
    .AddKafkaProducer<SendSmsEvent>(builder.Configuration.GetSection("Kafka:SendSmsEvent"))
    .AddKafkaProducer<SendPushEvent>(builder.Configuration.GetSection("Kafka:SendPushEvent"));

builder.Services
    .AddKafkaConsumer<EmailErrorEvent,
        ErrorEventHandler<EmailErrorEvent>>(builder.Configuration.GetSection("Kafka:EmailErrorEvent"))
    .AddKafkaConsumer<SmsErrorEvent,
        ErrorEventHandler<SmsErrorEvent>>(builder.Configuration.GetSection("Kafka:SmsErrorEvent"))
    .AddKafkaConsumer<PushErrorEvent, ErrorEventHandler<PushErrorEvent>>(
        builder.Configuration.GetSection("Kafka:PushErrorEvent"));
 
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();