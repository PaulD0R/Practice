using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Models;

namespace NotificationService.Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Notification>  Notifications { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Notification>()
            .Property(n => n.EmailStatus)
            .HasConversion<string>();

        modelBuilder.Entity<Notification>()
            .Property(n => n.SmsStatus)
            .HasConversion<string>();

        modelBuilder.Entity<Notification>()
            .Property(n => n.PushStatus)
            .HasConversion<string>();
    }
}