using MongoDB.Driver;

namespace Timetable.Api.Tests.Integration.Shared;

public sealed class MongoDbFixture : IDisposable
{
    private readonly string _databaseName = $"integration-testing_{Guid.NewGuid()}";

    private readonly MongoClient _client;

    public MongoDbFixture()
    {
        _client = new MongoClient("mongodb://localhost:27017");

        Database = _client.GetDatabase(_databaseName);
    }

    public IMongoDatabase Database { get; }

    public void Dispose()
    {
        _client.DropDatabase(_databaseName);
    }
}