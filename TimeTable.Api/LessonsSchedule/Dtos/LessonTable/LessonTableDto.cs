using DayOfWeek = TimeTable.Api.LessonsSchedule.Common.DayOfWeek;

namespace TimeTable.Api.LessonsSchedule.Dtos;

public class LessonTableDto
{
    public DayOfWeek DayOfWeek { get; init; }
    public string[][] Lessons { get; init; } = [];
}