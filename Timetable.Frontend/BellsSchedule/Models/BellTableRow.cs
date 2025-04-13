namespace Timetable.Frontend.BellsSchedule.Models;

public sealed class BellTableRow
{
    public string StartSoundId { get; set; } = string.Empty;
    public TimeModel StartTime { get; init; } = new();
    public string EndSoundId { get; set; } = string.Empty;
    public TimeModel EndTime { get; init; } = new();

    public override string ToString()
    {
        return $"{StartTime} - {EndTime}";
    }
}