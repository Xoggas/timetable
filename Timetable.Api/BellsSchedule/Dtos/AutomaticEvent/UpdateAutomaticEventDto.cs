using System.ComponentModel.DataAnnotations;
using Timetable.Api.LessonsSchedule.Validation;
using Timetable.Api.Shared.Validation;

namespace Timetable.Api.BellsSchedule.Dtos;

public sealed class UpdateAutomaticEventDto
{
    [Required]
    [MaxLength(20)]
    public string Name { get; init; } = string.Empty;

    [Required]
    public TimeDto? ActivationTime { get; init; }

    [Required]
    [MongoId]
    public string SoundFileId { get; init; } = string.Empty;

    public bool IsEnabled { get; init; } = true;
}