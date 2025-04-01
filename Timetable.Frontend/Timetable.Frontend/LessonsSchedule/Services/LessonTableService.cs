using Timetable.Frontend.LessonsSchedule.Models;

namespace Timetable.Frontend.LessonsSchedule.Services;

public sealed class LessonTableService
{
    private readonly HttpClient _httpClient;

    public LessonTableService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LessonTable> GetByDayOfWeek(CustomDayOfWeek dayOfWeek)
    {
        return await _httpClient.GetFromJsonAsync<LessonTable>($"api/lesson-table/{dayOfWeek}") ??
               throw new Exception("An error occured when getting the lesson table");
    }

    public async Task UpdateByDayOfWeek(CustomDayOfWeek dayOfWeek, LessonTable lessonTable)
    {
        await _httpClient.PutAsJsonAsync($"api/lesson-table/{dayOfWeek}", lessonTable);
    }
}