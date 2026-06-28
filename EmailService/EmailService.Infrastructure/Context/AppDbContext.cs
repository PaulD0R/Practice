using EmailService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailService.Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext>  options) : DbContext(options)
{
    public DbSet<SmtpOptions> SmtpOptions { get; set; }
}