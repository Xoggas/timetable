namespace Timetable.Frontend.BellsSchedule.Services;

public sealed class BellTableService
{
    private readonly HttpClient _httpClient;

    public BellTableService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Models.BellTable> GetBellTableAsync()
    {
        return await _httpClient.GetFromJsonAsync<Models.BellTable>("api/bells-schedule/bell-table") ??
               throw new Exception("An error occured when retrieving the bell table");
    }

    public async Task UpdateBellTableAsync(Models.BellTable bellTable)
    {
        await _httpClient.PutAsJsonAsync("api/bells-schedule/bell-table", bellTable);
    }
}