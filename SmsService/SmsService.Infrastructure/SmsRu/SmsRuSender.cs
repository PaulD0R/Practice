using Microsoft.Extensions.Options;
using SmsService.Application.Interfaces.Services;
using SmsService.Infrastructure.Options;

namespace SmsService.Infrastructure.SmsRu;

public class SmsRuSender(HttpClient httpClient, IOptions<SmsRuOptions> options) : ISmsSender
{
    public async Task<HttpResponseMessage> SendAsync(Domain.Models.Sms message)
    {
        var config = new Dictionary<string, string>
        {
            { "api_id", options.Value.ApiId ?? throw new NullReferenceException("ApiId is null") },
            { "to", message.PhoneNumber },
            { "msg", message.Message }
        };
        var content = new FormUrlEncodedContent(config);

        return await httpClient.PostAsync(options.Value.Url, content);
    }
}