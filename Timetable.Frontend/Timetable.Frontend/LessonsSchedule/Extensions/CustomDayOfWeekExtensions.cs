using Timetable.Frontend.LessonsSchedule.Models;

namespace Timetable.Frontend.LessonsSchedule.Extensions;

public static class CustomDayOfWeekExtensions
{
    public static string ToShortString(this CustomDayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            CustomDayOfWeek.Monday => "ПН",
            CustomDayOfWeek.Tuesday => "ВТ",
            CustomDayOfWeek.Wednesday => "СР",
            CustomDayOfWeek.Thursday => "ЧТ",
            CustomDayOfWeek.Friday => "ПТ",
            CustomDayOfWeek.Saturday => "СБ",
            CustomDayOfWeek.Sunday => "ВС",
            _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
        };
    }
}