namespace Timetable.Frontend.LessonsSchedule.Models;

public sealed class LessonTable
{
    public CustomDayOfWeek DayOfWeek { get; init; }
    public List<List<string>> Lessons { get; set; } = [];
}