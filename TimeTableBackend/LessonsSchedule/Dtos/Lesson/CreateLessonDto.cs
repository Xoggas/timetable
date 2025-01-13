using System.ComponentModel.DataAnnotations;

namespace TimeTableBackend.LessonsSchedule.Dtos;

public sealed class CreateLessonDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}