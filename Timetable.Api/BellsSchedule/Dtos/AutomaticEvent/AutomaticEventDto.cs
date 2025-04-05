namespace Timetable.Api.BellsSchedule.Dtos;

public sealed class AutomaticEventDto
{
    public string Id { get; init; } = string.Empty;
    public bool IsEnabled { get; init; } = true;
    public string Name { get; init; } = string.Empty;
    public TimeDto ActivationTime { get; init; } = null!;
    public string SoundFileId { get; init; } = string.Empty;
}