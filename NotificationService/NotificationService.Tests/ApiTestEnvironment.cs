using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace NotificationService.Tests;

public class ApiTestEnvironment : WebApplicationFactory<Program>
{
    public IMediator MediatorMock { get; } = Substitute.For<IMediator>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMediator));
            if (descriptor != null) services.Remove(descriptor);
            
            services.AddSingleton(MediatorMock);
        });
    }
}