using Timetable.Api.Shared.Validation;

namespace Timetable.Api.BellsSchedule.Dtos;

public sealed class UpdatePlaylistDto
{
    [MongoId]
    public string[] SoundFilesIds { get; init; } = [];
}