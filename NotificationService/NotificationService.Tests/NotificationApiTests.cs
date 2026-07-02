using System.Net;
using System.Net.Http.Json;
using MediatR;
using NotificationService.Application.Commands.AddNotification;
using NotificationService.Application.DTOs;
using NotificationService.Application.Queries.GetNotifications;
using NotificationService.Domain.Enums;
using NSubstitute;
using Xunit;
using Assert = Xunit.Assert;

namespace NotificationService.Tests;

public class NotificationApiTests(ApiTestEnvironment env) : IClassFixture<ApiTestEnvironment>
{
    private readonly HttpClient _client = env.CreateClient();
    private readonly IMediator _mediatorMock = env.MediatorMock;

    [Fact]
    public async Task CreateNotification_ShouldReturnCreated_WhenPostToUrl()
    {
        var command = new AddNotificationCommand("+79999999999", "Привет!", NotificationChannel.Sms);
        var response = await _client.PostAsJsonAsync("/api/notifications", command);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetAllNotifications_ShouldReturnOkWithData_WhenGetFromUrl()
    {
        await _mediatorMock.Send(Arg.Any<GetNotificationsQuery>(), Arg.Any<CancellationToken>());

        var response = await _client.GetAsync("/api/notifications");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<List<NotificationDto>>();
        Assert.NotNull(result);
    }
}