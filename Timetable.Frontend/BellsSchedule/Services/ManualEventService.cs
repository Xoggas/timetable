using Timetable.Frontend.BellsSchedule.Models;

namespace Timetable.Frontend.BellsSchedule.Services;

public sealed class ManualEventService
{
    private readonly HttpClient _httpClient;

    public ManualEventService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ManualEvent>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ManualEvent>>("api/bells-schedule/manual-event") ??
               throw new Exception("An error occured when retrieving the manual events.");
    }

    public async Task<ManualEvent> CreateAsync()
    {
        var response = await _httpClient.PostAsJsonAsync<ManualEvent>("api/bells-schedule/manual-event", null!);

        return await response.Content.ReadFromJsonAsync<ManualEvent>() ??
               throw new Exception("An error occured when creating the manual event.");
    }

    public async Task UpdateAsync(IEnumerable<ManualEvent> events)
    {
        foreach (var automaticEvent in events)
        {
            await _httpClient.PutAsJsonAsync($"api/bells-schedule/manual-event/{automaticEvent.Id}",
                automaticEvent);
        }
    }

    public async Task DeleteAsync(ManualEvent manualEvent)
    {
        await _httpClient.DeleteAsync($"api/bells-schedule/manual-event/{manualEvent.Id}");
    }
}