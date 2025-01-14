namespace TimeTableBackend.LessonsSchedule.Dtos;

// TODO: Add validation
public sealed class UpdateLessonTableDto
{
    public string[][] Lessons { get; set; } = [];
}