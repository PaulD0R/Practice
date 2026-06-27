using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Models;

namespace NotificationService.Infrastructure.Context;

public class WriteDbContext(DbContextOptions<WriteDbContext> options) : DbContext(options)
{
    public DbSet<Notification> Notifications { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Notification>()
            .Property(n => n.Status)
            .HasConversion<string>();
        
        modelBuilder.Entity<Notification>()
            .Property(n => n.Channel)
            .HasConversion<string>();
    }
}