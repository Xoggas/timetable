using Microsoft.AspNetCore.SignalR;
using Timetable.Api.BellsSchedule.Entities;

namespace Timetable.Api.BellsSchedule.Hubs;

public interface IBellsScheduleEventHub
{
    public Task Update(BellTable bellTable);
    public Task ClassStarted();
    public Task ClassEnded();
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

    public async Task ClassEnded()
    {
        await Clients.All.ClassEnded();
    }
}