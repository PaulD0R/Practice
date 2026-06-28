using EmailService.Domain.Models;

namespace EmailService.Application.Interfaces.Repositories;

public interface ISmtpRepository
{
    Task<IEnumerable<SmtpOptions>> GetSmtpOptionsAsync();
}