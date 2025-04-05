using System.ComponentModel.DataAnnotations;

namespace Timetable.Api.BellsSchedule.Dtos;

public class TimeDto
{
    [Required]
    [Range(0, 23)]
    public int Hour { get; init; }

    [Required]
    [Range(0, 59)]
    public int Minute { get; init; }
}