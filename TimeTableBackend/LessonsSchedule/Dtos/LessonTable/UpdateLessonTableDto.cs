using TimeTableBackend.LessonsSchedule.Validation;

namespace TimeTableBackend.LessonsSchedule.Dtos;

public sealed class UpdateLessonTableDto
{
    [ValidateStringArray(50)]
    public string[][] Lessons { get; init; } = [];
}