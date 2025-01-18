using Common_DayOfWeek = TimeTable.Api.LessonsSchedule.Common.DayOfWeek;
using DayOfWeek = TimeTable.Api.LessonsSchedule.Common.DayOfWeek;

namespace TimeTable.Api.LessonsSchedule.Dtos;

public class LessonTableDto
{
    public Common_DayOfWeek DayOfWeek { get; init; }
    public string[][] Lessons { get; init; } = [];
}