using DayOfWeek = TimeTableBackend.LessonsSchedule.Common.DayOfWeek;

namespace TimeTableBackend.LessonsSchedule.Dtos;

public class LessonTableDto
{
    public DayOfWeek DayOfWeek { get; init; }
    public string[][] Lessons { get; init; } = [];
}