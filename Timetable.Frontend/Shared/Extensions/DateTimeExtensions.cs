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
    
    public static string GetDayOfWeekString(this DateTime dateTime)
    {
        return dateTime.DayOfWeek switch
        {
            DayOfWeek.Monday => "Понедельник",
            DayOfWeek.Tuesday => "Вторник",
            DayOfWeek.Wednesday => "Среда",
            DayOfWeek.Thursday => "Четверг",
            DayOfWeek.Friday => "Пятница",
            DayOfWeek.Saturday => "Суббота",
            DayOfWeek.Sunday => "Воскресенье",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static int GetTotalMinutes(this DateTime dateTime)
    {
        return dateTime.Hour * 60 + dateTime.Minute;
    }
}