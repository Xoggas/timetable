using Timetable.Api.LessonsSchedule.Validation;

namespace Timetable.Api.LessonsSchedule.Dtos;

public sealed class UpdateLessonTableDto
{
    [MinMaxLength(1, 40)]
    public string[][] Lessons { get; init; } = [];
}