using MongoDB.Driver;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.BellsSchedule.Services;

public interface IBellTableService
{
    Task<BellTable> Get();
    Task Update(BellTable bellTable);
}

public sealed class BellTableService : IBellTableService
{
    private readonly IMongoCollection<BellTable> _bellTableCollection;
    private readonly IBellsScheduleEventService _eventService;

    public BellTableService(MongoDbService mongoDbService, IBellsScheduleEventService eventService)
    {
        _bellTableCollection = mongoDbService.GetCollection<BellTable>("bell-table");
        _eventService = eventService;
    }

    public async Task<BellTable> Get()
    {
        var bellTable = await _bellTableCollection.Find(_ => true).FirstOrDefaultAsync();

        if (bellTable is not null)
        {
            return bellTable;
        }

        bellTable = new BellTable();

        await _bellTableCollection.InsertOneAsync(bellTable);

        return bellTable;
    }

    public async Task Update(BellTable bellTable)
    {
        var update = Builders<BellTable>.Update
            .Set(x => x.Rows, bellTable.Rows);

        await _bellTableCollection.UpdateOneAsync(x => true, update, new UpdateOptions
        {
            IsUpsert = true
        });

        await _eventService.NotifyAllClientsAboutUpdate(bellTable);
    }
}