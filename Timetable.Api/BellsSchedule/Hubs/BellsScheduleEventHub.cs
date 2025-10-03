using Microsoft.AspNetCore.SignalR;
using Timetable.Api.BellsSchedule.Dtos;

namespace Timetable.Api.BellsSchedule.Hubs;

public interface IBellsScheduleEventHub
{
    Task Update(BellTableDto bellTable);
    Task TimeStateChange(TimeStateDto dto);
}

public sealed class BellsScheduleEventHub : Hub<IBellsScheduleEventHub>
{
    public async Task Update(BellTableDto bellTable)
    {
        await Clients.All.Update(bellTable);
    }

    public async Task TimeStateChange(TimeStateDto dto)
    {
        await Clients.All.TimeStateChange(dto);
    }
}