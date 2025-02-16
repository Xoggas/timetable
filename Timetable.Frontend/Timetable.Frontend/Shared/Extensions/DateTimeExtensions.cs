using DayOfWeek = Timetable.Frontend.LessonsSchedule.Models.DayOfWeek;

namespace Timetable.Frontend.Shared.Extensions;

public static class DateTimeExtensions
{
    public static DayOfWeek GetNativeDayOfWeek(this DateTime dateTime)
    {
        return dateTime.DayOfWeek switch
        {
            System.DayOfWeek.Monday => DayOfWeek.Monday,
            System.DayOfWeek.Tuesday => DayOfWeek.Tuesday,
            System.DayOfWeek.Wednesday => DayOfWeek.Wednesday,
            System.DayOfWeek.Thursday => DayOfWeek.Thursday,
            System.DayOfWeek.Friday => DayOfWeek.Friday,
            System.DayOfWeek.Saturday => DayOfWeek.Saturday,
            System.DayOfWeek.Sunday => DayOfWeek.Sunday,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}