using Timetable.Frontend.LessonsSchedule.Models;

namespace Timetable.Frontend.LessonsSchedule.Services;

public sealed class LessonListService
{
    private readonly HttpClient _httpClient;

    public LessonListService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Lesson>> GetAllLessons()
    {
        var lessons = await _httpClient.GetFromJsonAsync<List<Lesson>>("api/lesson");

        return lessons ?? [];
    }

    public async Task<Lesson> CreateLesson()
    {
        var response = await _httpClient.PostAsJsonAsync("api/lesson", new Lesson());

        var lesson = await response.Content.ReadFromJsonAsync<Lesson>();
        
        return lesson ??  throw new Exception("Lesson wasn't created");
    }

    public async Task DeleteLesson(Lesson lesson)
    {
        await _httpClient.DeleteAsync($"api/lesson/{lesson.Id}");
    }

    public async Task UpdateLesson(Lesson lesson)
    {
        await _httpClient.PutAsJsonAsync($"api/lesson/{lesson.Id}", lesson);
    }
}