using System.ComponentModel.DataAnnotations;

namespace TimeTableBackend.LessonsSchedule.Dtos;

public sealed class UpdateLessonDto
{
    [Required]
    [MaxLength(40)]
    public string Name { get; set; } = string.Empty;
}