using Common_DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;
using DayOfWeek = Timetable.Api.LessonsSchedule.Common.DayOfWeek;

namespace Timetable.Api.LessonsSchedule.Dtos;

public class LessonTableDto
{
    public Common_DayOfWeek DayOfWeek { get; init; }
    public string[][] Lessons { get; init; } = [];
}