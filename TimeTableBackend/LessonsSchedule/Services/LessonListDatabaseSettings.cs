namespace TimeTableBackend.LessonsSchedule.Models;

public class LessonListDatabaseSettings
{
    public string ConnectionString { get; init; } = null!;
    public string DatabaseName { get; init; } = null!;
    public string CollectionName { get; init; } = null!;
}