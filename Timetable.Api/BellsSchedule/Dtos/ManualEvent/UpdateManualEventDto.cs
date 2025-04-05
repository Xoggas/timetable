using System.ComponentModel.DataAnnotations;
using Timetable.Api.Shared.Validation;

namespace Timetable.Api.BellsSchedule.Dtos;

public sealed class UpdateManualEventDto
{
    [Required]
    [MaxLength(20)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MongoId]
    public string SoundFileId { get; set; } = string.Empty;
}