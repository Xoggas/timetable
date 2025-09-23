using Microsoft.AspNetCore.SignalR;
using Timetable.Api.BellsSchedule.Entities;
using Timetable.Api.BellsSchedule.Hubs;

namespace Timetable.Api.Shared.Services;

public interface IBellsScheduleEventService
{
    Task NotifyAllClientsAboutUpdate(BellTable bellTable);
    Task NotifyAllClientsThatClassStarted();
    Task NotifyAllClientsThatClassEnded(LessonState state);
}

public sealed class BellsScheduleEventService : IBellsScheduleEventService
{
    private readonly IHubContext<BellsScheduleEventHub, IBellsScheduleEventHub> _eventHub;

    public BellsScheduleEventService(IHubContext<BellsScheduleEventHub, IBellsScheduleEventHub> eventHub)
    {
        _eventHub = eventHub;
    }

    public async Task NotifyAllClientsAboutUpdate(BellTable bellTable)
    {
        await _eventHub.Clients.All.Update(bellTable);
    }

    public async Task NotifyAllClientsThatClassStarted()
    {
        await _eventHub.Clients.All.ClassStarted();
    }

    public async Task NotifyAllClientsThatClassEnded(LessonState state)
    {
        await _eventHub.Clients.All.ClassEnded(state);
    }
}