using Timetable.Frontend.LessonsSchedule.Models;

namespace Timetable.Frontend.BellsSchedule.Models;

public sealed class Playlist
{
    public CustomDayOfWeek DayOfWeek { get; init; }
    public List<string> SoundFilesIds { get; init; } = [];
}