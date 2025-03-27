using Timetable.Frontend.LessonsSchedule.Models;

namespace Timetable.Frontend.LessonsSchedule.Services;

public sealed class LessonListService
{
    public IEnumerable<Lesson> GetAllLessons()
    {
        return Enumerable.Range(0, 30).Select(x => x + 1).Select(i => new Lesson
        {
            Id = $"{i}",
            Name = $"Урок {i:D2}"
        });
    }
}