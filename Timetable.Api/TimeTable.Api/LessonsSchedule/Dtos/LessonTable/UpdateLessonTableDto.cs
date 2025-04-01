using Timetable.Api.LessonsSchedule.Validation;

namespace Timetable.Api.LessonsSchedule.Dtos;

public sealed class UpdateLessonTableDto
{
    [MinMaxLength(0, 20)]
    public string[][] Lessons { get; init; } = [];
}