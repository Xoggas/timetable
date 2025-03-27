using Timetable.Frontend.LessonsSchedule.Models;

namespace Timetable.Frontend.Shared.Extensions;

public static class DateTimeExtensions
{
    public static CustomDayOfWeek GetNativeDayOfWeek(this DateTime dateTime)
    {
        return dateTime.DayOfWeek switch
        {
            DayOfWeek.Monday => CustomDayOfWeek.Monday,
            DayOfWeek.Tuesday => CustomDayOfWeek.Tuesday,
            DayOfWeek.Wednesday => CustomDayOfWeek.Wednesday,
            DayOfWeek.Thursday => CustomDayOfWeek.Thursday,
            DayOfWeek.Friday => CustomDayOfWeek.Friday,
            DayOfWeek.Saturday => CustomDayOfWeek.Saturday,
            DayOfWeek.Sunday => CustomDayOfWeek.Sunday,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}