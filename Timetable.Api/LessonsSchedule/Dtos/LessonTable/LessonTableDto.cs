using Timetable.Api.LessonsSchedule.Common;

namespace Timetable.Api.LessonsSchedule.Dtos;

public class LessonTableDto
{
    public CustomDayOfWeek CustomDayOfWeek { get; init; }
    public string[][] Lessons { get; init; } = [];
}