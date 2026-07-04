namespace SmsService.API.Options;

public class RetryOptions
{
    public int? MaxRetryCount { get; set; }
    public int? StartRetryDelay { get; set; }
}