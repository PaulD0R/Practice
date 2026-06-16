using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NotificationService.Application.Interfaces.Repositories;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Options;

namespace NotificationService.Infrastructure.Repositories;

public class ArchiveRepository(
    IMongoClient mongoClient,
    IOptions<MongoOptions> options) : IArchiveRepository
{
    private readonly IMongoCollection<Notification> _collection = mongoClient.GetDatabase(options.Value.DatabaseName)
        .GetCollection<Notification>(options.Value.CollectionName);
    
    public async Task SaveRangeAsync(IEnumerable<Notification> notifications)
    {
        await _collection.InsertManyAsync(notifications);
    }
}