using Timetable.Api.LessonsSchedule.Common;

namespace Timetable.Api.BellsSchedule.Dtos;

public sealed class PlaylistDto
{
    public CustomDayOfWeek DayOfWeek { get; init; }
    public string[] SoundFilesIds { get; init; } = [];
}