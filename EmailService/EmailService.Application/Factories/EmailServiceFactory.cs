using EmailService.Application.Interfaces.Factories;
using EmailService.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmailService.Application.Factories;

public class EmailServiceFactory(IServiceScopeFactory scopeFactory) : IFactory<IEmailService>, IDisposable
{
    private readonly IServiceScope _serviceScope = scopeFactory.CreateScope();
    
    public IEmailService Create() => _serviceScope.ServiceProvider.GetRequiredService<IEmailService>();

    public void Dispose() =>  _serviceScope.Dispose();
}