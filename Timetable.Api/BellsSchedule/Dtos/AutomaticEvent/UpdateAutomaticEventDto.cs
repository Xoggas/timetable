using System.ComponentModel.DataAnnotations;
using Timetable.Api.LessonsSchedule.Validation;
using Timetable.Api.Shared.Validation;

namespace Timetable.Api.BellsSchedule.Dtos;

public sealed class UpdateAutomaticEventDto
{
    public bool IsEnabled { get; init; } = true;
    
    [MaxLength(20)]
    public string Name { get; init; } = string.Empty;
    
    public TimeDto ActivationTime { get; init; } = new();
    
    [MongoId]
    public string SoundFileId { get; init; } = string.Empty;
}