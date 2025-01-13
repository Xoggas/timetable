using System.ComponentModel.DataAnnotations;

namespace TimeTableBackend.LessonsSchedule.Entities;

public sealed class Lesson
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(50)]
    public required string Name { get; set; } = string.Empty;
}