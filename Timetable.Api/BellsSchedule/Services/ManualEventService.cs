using MongoDB.Driver;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.BellsSchedule.Services;

public interface IManualEventService
{
    Task<IEnumerable<ManualEvent>> GetAllEvents();
    Task<ManualEvent?> GetEventById(string id);
    Task<ManualEvent> CreateEvent();
    Task UpdateEvent(string id, ManualEvent automaticEvent);
    Task DeleteEvent(string id);
}

public sealed class ManualEventService : IManualEventService
{
    private readonly IMongoCollection<ManualEvent> _manualEventsCollection;

    public ManualEventService(MongoDbService mongoDbService)
    {
        _manualEventsCollection = mongoDbService.GetCollection<ManualEvent>("manual-events");
    }

    public async Task<IEnumerable<ManualEvent>> GetAllEvents()
    {
        return await _manualEventsCollection
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task<ManualEvent?> GetEventById(string id)
    {
        return await _manualEventsCollection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<ManualEvent> CreateEvent()
    {
        var manualEvent = new ManualEvent();

        await _manualEventsCollection.InsertOneAsync(manualEvent);

        return manualEvent;
    }

    public async Task UpdateEvent(string id, ManualEvent automaticEvent)
    {
        var update = Builders<ManualEvent>.Update
            .Set(x => x.Name, automaticEvent.Name)
            .Set(x => x.SoundFileId, automaticEvent.SoundFileId);

        await _manualEventsCollection.UpdateOneAsync(x => x.Id == id, update);
    }

    public async Task DeleteEvent(string id)
    {
        await _manualEventsCollection.DeleteOneAsync(x => x.Id == id);
    }
}