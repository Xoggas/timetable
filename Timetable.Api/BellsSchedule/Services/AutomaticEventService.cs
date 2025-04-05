using MongoDB.Driver;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.Shared.Services;

namespace Timetable.Api.BellsSchedule.Services;

public interface IAutomaticEventService
{
    Task<IEnumerable<AutomaticEvent>> GetAllEvents();
    Task<AutomaticEvent?> GetEventById(string id);
    Task<AutomaticEvent> CreateEvent();
    Task UpdateEvent(string id, AutomaticEvent automaticEvent);
    Task DeleteEvent(string id);
}

public sealed class AutomaticEventService : IAutomaticEventService
{
    private readonly IMongoCollection<AutomaticEvent> _automaticEventsCollection;

    public AutomaticEventService(MongoDbService mongoDbService)
    {
        _automaticEventsCollection = mongoDbService.GetCollection<AutomaticEvent>("automatic-events");
    }

    public async Task<IEnumerable<AutomaticEvent>> GetAllEvents()
    {
        return await _automaticEventsCollection
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task<AutomaticEvent?> GetEventById(string id)
    {
        return await _automaticEventsCollection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<AutomaticEvent> CreateEvent()
    {
        var automaticEvent = new AutomaticEvent();

        await _automaticEventsCollection.InsertOneAsync(automaticEvent);

        return automaticEvent;
    }

    public async Task UpdateEvent(string id, AutomaticEvent automaticEvent)
    {
        var update = Builders<AutomaticEvent>.Update
            .Set(x => x.IsEnabled, automaticEvent.IsEnabled)
            .Set(x => x.Name, automaticEvent.Name)
            .Set(x => x.ActivationTime, automaticEvent.ActivationTime)
            .Set(x => x.SoundFileId, automaticEvent.SoundFileId);

        await _automaticEventsCollection.UpdateOneAsync(x => x.Id == id, update);
    }

    public async Task DeleteEvent(string id)
    {
        await _automaticEventsCollection.DeleteOneAsync(x => x.Id == id);
    }
}