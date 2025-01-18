using MongoDB.Driver;
using TimeTable.Api.LessonsSchedule.Entities;

namespace TimeTable.Api.Tests.Integration.Shared;

public sealed class MongoDbFixture : IDisposable
{
    private readonly string _databaseName = $"integration-testing_{Guid.NewGuid()}";

    private readonly MongoClient _client;

    public MongoDbFixture()
    {
        _client = new MongoClient("mongodb://localhost:27017");

        Database = _client.GetDatabase(_databaseName);

        SeedDatabase();
    }

    public IMongoDatabase Database { get; }

    private void SeedDatabase()
    {
        var lessonsCollection = Database.GetCollection<Lesson>("lessons");

        lessonsCollection.InsertOne(new Lesson
        {
            Name = "test_lesson"
        });
    }

    public void Dispose()
    {
        _client.DropDatabase(_databaseName);
    }
}