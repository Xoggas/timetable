using System.ComponentModel.DataAnnotations;

namespace Timetable.Api.BellsSchedule.Dtos;

public sealed class UpdateBellTableDto
{
    [Required]
    public BellTableRowDto[] Rows { get; init; } = [];
}