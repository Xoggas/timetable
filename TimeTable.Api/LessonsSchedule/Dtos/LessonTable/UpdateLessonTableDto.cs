using TimeTable.Api.LessonsSchedule.Validation;

namespace TimeTable.Api.LessonsSchedule.Dtos;

public sealed class UpdateLessonTableDto
{
    [ValidateStringArray(50)]
    public string[][] Lessons { get; init; } = [];
}