using MongoDB.Driver;

namespace Timetable.Api.Shared;

public sealed class MongoDbService
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