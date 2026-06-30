using EmailService.Application.Interfaces.Caching;
using EmailService.Application.Interfaces.Providers;
using EmailService.Application.Interfaces.Repositories;
using EmailService.Domain.Models;

namespace EmailService.Application.Providers;

public class SmtpProvider(ISmtpRepository smtpRepository, ICachingService cachingService) : ISmtpProvider
{
    private const string SmtpKey = "SmtpOptionsKey";
    
    public async Task<IEnumerable<SmtpOptions>> GetSmtpOptionsAsync()
    {
        var options = await cachingService.GetAsync<IList<SmtpOptions>>(SmtpKey);
        if (options != null) return options;
        
        options = (await smtpRepository.GetSmtpOptionsAsync()).ToList();
        await cachingService.SetAsync(SmtpKey, options, TimeSpan.FromHours(2));

        return options;
    }
}