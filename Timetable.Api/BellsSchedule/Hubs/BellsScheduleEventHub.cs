using Microsoft.AspNetCore.SignalR;
using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.Hubs;

public interface IBellsScheduleEventHub
{
    Task Update(BellTable bellTable);
    Task ClassStarted();
    Task ClassEnded(LessonState state);
}

public sealed class BellsScheduleEventHub : Hub<IBellsScheduleEventHub>
{
    public async Task Update(BellTable bellTable)
    {
        await Clients.All.Update(bellTable);
    }

    public async Task ClassStarted()
    {
        await Clients.All.ClassStarted();
    }

    public async Task ClassEnded(LessonState state)
    {
        await Clients.All.ClassEnded(state);
    }
}