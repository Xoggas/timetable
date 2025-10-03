using Timetable.Frontend.BellsSchedule.Models;

namespace Timetable.Frontend.BellsSchedule.Services;

public sealed class BellTableService
{
    private readonly HttpClient _httpClient;

    public BellTableService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BellTable> GetBellTableAsync()
    {
        return await _httpClient.GetFromJsonAsync<BellTable>("api/bells-schedule/bell-table") ??
               throw new Exception("An error occured when retrieving the bell table");
    }

    public async Task<TimeState> GetTimeStateAsync()
    {
        return await _httpClient.GetFromJsonAsync<TimeState>("api/bells-schedule/bell-table/time-state") ??
               throw new Exception("An error occured when retrieving the time state");
    }

    public async Task UpdateBellTableAsync(Models.BellTable bellTable)
    {
        await _httpClient.PutAsJsonAsync("api/bells-schedule/bell-table", bellTable);
    }
}