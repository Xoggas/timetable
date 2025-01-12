using System.ComponentModel.DataAnnotations;

namespace TimeTableBackend.LessonsSchedule.Models;

public sealed class Lesson
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
}