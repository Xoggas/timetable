using Timetable.Frontend.BellsSchedule.Models;
using Timetable.Frontend.LessonsSchedule.Models;

namespace Timetable.Frontend.BellsSchedule.Services;

public sealed class PlaylistService
{
    private readonly HttpClient _httpClient;

    public PlaylistService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Playlist> GetByDayOfWeekAsync(CustomDayOfWeek dayOfWeek)
    {
        var playlist = await _httpClient.GetFromJsonAsync<Playlist>($"api/bells-schedule/playlist/{dayOfWeek}");

        return playlist!;
    }

    public async Task UpdateByDayOfWeekAsync(CustomDayOfWeek dayOfWeek, Playlist playlist)
    {
        await _httpClient.PutAsJsonAsync($"api/bells-schedule/playlist/{dayOfWeek}", playlist);
    }
}