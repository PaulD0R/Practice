using EmailService.Application.Interfaces.Repositories;
using EmailService.Domain.Models;
using EmailService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EmailService.Infrastructure.Repositories;

public class SmtpRepository(AppDbContext context) : ISmtpRepository     
{
    public async Task<IEnumerable<SmtpOptions>> GetSmtpOptionsAsync()
    {
        return await context.SmtpOptions.ToListAsync();
    }
}