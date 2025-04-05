using System.ComponentModel.DataAnnotations;

namespace Timetable.Api.BellsSchedule.Dtos;

public sealed class BellTableDto
{
    public string Id { get; init; } = string.Empty;

    [Required]
    public BellTableRowDto[] Rows { get; init; } = [];
}