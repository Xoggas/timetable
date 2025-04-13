using System.ComponentModel.DataAnnotations;
using Timetable.Api.Shared.Validation;

namespace Timetable.Api.BellsSchedule.Dtos;

public sealed class UpdateManualEventDto
{
    [MaxLength(20)]
    public string Name { get; set; } = string.Empty;

    [MongoId]
    public string SoundFileId { get; set; } = string.Empty;
}