namespace Timetable.Api.BellsSchedule.Dtos;

public sealed class ManualEventDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SoundFileId { get; set; } = string.Empty;
}