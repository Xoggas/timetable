using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace TimeTableBackend.Shared;

public sealed class MongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(IOptions<MongoDbSettings> settings)
    {
        _database = new MongoClient(settings.Value.ConnectionString)
            .GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}