using System.ComponentModel.DataAnnotations;

namespace TimeTableBackend.LessonsSchedule.Entities;

public sealed class Lesson
{
    [Key]
    public int Id { get; init; }
    
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}