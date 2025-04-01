using Microsoft.AspNetCore.SignalR;

namespace Timetable.Api.LessonsSchedule.Hubs;

public interface IEventHub
{
    public Task Connected();
    public Task Update();
}

public sealed class EventHub : Hub<IEventHub>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.Connected();
    }

    public async Task Update()
    {
        await Clients.All.Update();
    }
}