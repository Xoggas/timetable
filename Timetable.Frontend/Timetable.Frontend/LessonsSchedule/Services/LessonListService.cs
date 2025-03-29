using Timetable.Frontend.LessonsSchedule.Models;

namespace Timetable.Frontend.LessonsSchedule.Services;

public sealed class LessonListService
{
    private static readonly List<Lesson> s_lessons = [];

    public async Task<List<Lesson>> GetAllLessons()
    {
        return await Task.FromResult(s_lessons);
    }

    public async Task<Lesson> CreateLesson()
    {
        var lesson = new Lesson
        {
            Id = Guid.NewGuid().ToString()
        };
        
        return await Task.FromResult(lesson);
    }

    public async Task DeleteLesson(Lesson lesson)
    {
        await Task.CompletedTask;
    }
}