using DayOfWeek = Timetable.Frontend.LessonsSchedule.Models.DayOfWeek;

namespace Timetable.Frontend.LessonsSchedule.Extensions;

public static class DayOfWeekExtensions
{
    public static string ToShortString(this DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => "ПН",
            DayOfWeek.Tuesday => "ВТ",
            DayOfWeek.Wednesday => "СР",
            DayOfWeek.Thursday => "ЧТ",
            DayOfWeek.Friday => "ПТ",
            DayOfWeek.Saturday => "СБ",
            DayOfWeek.Sunday => "ВС",
            _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
        };
    }
}