using Microsoft.AspNetCore.SignalR.Client;
using Timetable.Frontend.BellsSchedule.Models;
using Timetable.Frontend.LessonsSchedule.Policies;
using Timetable.Frontend.Shared.Services;

namespace Timetable.Frontend.BellsSchedule.Services;

public sealed class BellTableEventService : IAsyncDisposable
{
    public event Func<Task>? Disconnected;
    public event Func<Task>? Reconnected;
    public event Action<BellTable>? BellTableUpdated;
    public event Action<TimeState>? TimeStateUpdated;

    private readonly HubConnection _connection;

    public BellTableEventService(ApiUrlService apiUrlService)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(apiUrlService.Retrieve("bells-schedule"))
            .WithAutomaticReconnect(new InfiniteRetryPolicy())
            .Build();

        _connection.Reconnecting += OnDisconnected;
        _connection.Reconnected += OnReconnected;
        _connection.On<BellTable>("Update", OnUpdate);
        _connection.On<TimeState>("LessonStateChange", OnLessonStateChange);
    }

    public async Task OpenConnectionAsync()
    {
        await _connection.StartAsync();
    }

    private async Task OnDisconnected(Exception? exception)
    {
        if (Disconnected is not null)
        {
            await Disconnected.Invoke();
        }
    }

    private async Task OnReconnected(string? newConnectionId)
    {
        if (Reconnected is not null)
        {
            await Reconnected.Invoke();
        }
    }

    private void OnUpdate(BellTable bellTable)
    {
        BellTableUpdated?.Invoke(bellTable);
    }

    private void OnLessonStateChange(TimeState notification)
    {
        TimeStateUpdated?.Invoke(notification);
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}