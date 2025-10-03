using System.Net;
using Timetable.Frontend.LessonsSchedule.Models;

namespace Timetable.Frontend.LessonsSchedule.Services;

public sealed class LessonTableService
{
    private readonly HttpClient _httpClient;

    public LessonTableService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LessonTable> GetByDayOfWeekAsync(CustomDayOfWeek dayOfWeek)
    {
        return await _httpClient.GetFromJsonAsync<LessonTable>($"api/lesson-table/{dayOfWeek}") ??
               throw new Exception("An error occured when getting the lesson table");
    }

    public async Task UpdateByDayOfWeekAsync(CustomDayOfWeek dayOfWeek, LessonTable lessonTable)
    {
        await _httpClient.PutAsJsonAsync($"api/lesson-table/{dayOfWeek}", lessonTable);
    }

    public async Task MakeBackupAsync(CustomDayOfWeek dayOfWeek)
    {
        await _httpClient.PostAsync($"api/lesson-table/{dayOfWeek}/backup", null);
    }

    public async Task<LessonTable?> RestoreBackupAsync(CustomDayOfWeek dayOfWeek)
    {
        var response = await _httpClient.PostAsync($"api/lesson-table/{dayOfWeek}/restore", null);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }
        
        return await response.Content.ReadFromJsonAsync<LessonTable>();
    }
}