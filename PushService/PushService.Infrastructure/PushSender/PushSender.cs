using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using PushService.Application.Interfaces.Services;
using PushService.Domain.Models;
using PushService.Infrastructure.Options;

namespace PushService.Infrastructure.PushSender;

public class PushSender(HttpClient httpClient, IOptions<PushOptions> options) : IPushSender
{
    private readonly PushOptions _options = options.Value;

    public async Task<HttpResponseMessage> SendAsync(PushMessage message)
    {
        var payload = new
        {
            app_id = options.Value.ApiId ?? throw new NullReferenceException("ApiId is null"),
            include_external_user_ids = new[] { message.Address }, 
            contents = new { ru = message.Body, en = message.Body }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, _options.Url)
        {
            Content = JsonContent.Create(payload) 
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _options.ApiKey);

        return await httpClient.SendAsync(request);
    }
}