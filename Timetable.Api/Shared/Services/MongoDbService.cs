using MongoDB.Driver;

namespace Timetable.Api.Shared.Services;

public interface IMongoDbService
{
    IMongoCollection<T> GetCollection<T>(string name);
}

public sealed class MongoDbService : IMongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(IMongoDatabase database)
    {
        _database = database;
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}