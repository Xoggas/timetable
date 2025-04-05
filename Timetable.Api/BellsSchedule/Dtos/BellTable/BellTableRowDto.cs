using System.ComponentModel.DataAnnotations;
using Timetable.Api.Shared.Validation;

namespace Timetable.Api.BellsSchedule.Dtos;

public sealed class BellTableRowDto
{
    [MongoId]
    public string StartSoundId { get; init; } = string.Empty;

    [Required]
    public TimeDto? StartTime { get; init; }

    [MongoId]
    public string EndSoundId { get; init; } = string.Empty;

    [Required]
    public TimeDto? EndTime { get; init; }
}