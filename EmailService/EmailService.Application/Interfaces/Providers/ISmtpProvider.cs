using EmailService.Domain.Models;

namespace EmailService.Application.Interfaces.Providers;

public interface ISmtpProvider
{
    Task<IEnumerable<SmtpOptions>> GetSmtpOptionsAsync();
}