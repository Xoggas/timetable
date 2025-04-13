namespace Timetable.Frontend.BellsSchedule.Models;

public sealed class AutomaticEvent
{
    public string Id { get; init; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    public string Name { get; set; } = string.Empty;
    public TimeModel ActivationTime { get; init; } = new();
    public string SoundFileId { get; set; } = string.Empty;
}