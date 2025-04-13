using Timetable.Frontend.BellsSchedule.Models;

namespace Timetable.Frontend.BellsSchedule.Services;

public sealed class AutomaticEventService
{
    private readonly HttpClient _httpClient;

    public AutomaticEventService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AutomaticEvent>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<AutomaticEvent>>("api/bells-schedule/automatic-event") ??
               throw new Exception("An error occured when retrieving the automatic events.");
    }

    public async Task<AutomaticEvent> CreateAsync()
    {
        var response = await _httpClient.PostAsJsonAsync<AutomaticEvent>("api/bells-schedule/automatic-event", null!);

        return await response.Content.ReadFromJsonAsync<AutomaticEvent>() ??
               throw new Exception("An error occured when creating the automatic event.");
    }

    public async Task UpdateAsync(IEnumerable<AutomaticEvent> events)
    {
        foreach (var automaticEvent in events)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/bells-schedule/automatic-event/{automaticEvent.Id}",
                automaticEvent);
            
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }

    public async Task DeleteAsync(AutomaticEvent automaticEvent)
    {
        await _httpClient.DeleteAsync($"api/bells-schedule/automatic-event/{automaticEvent.Id}");
    }
}