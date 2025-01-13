using Microsoft.AspNetCore.SignalR;
using TimeTableBackend.LessonsSchedule.Hubs;

namespace TimeTableBackend.LessonsSchedule.Services;

public sealed class EventService
{
    private readonly IHubContext<EventHub, IEventHub> _eventHub;

    public EventService(IHubContext<EventHub, IEventHub> eventHub)
    {
        _eventHub = eventHub;
    }

    public async Task NotifyAboutUpdate()
    {
        await _eventHub.Clients.All.Update();
    }
}