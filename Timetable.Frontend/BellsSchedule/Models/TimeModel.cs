using System.ComponentModel.DataAnnotations;

namespace Timetable.Frontend.BellsSchedule.Models;

public class TimeModel
{
    [Range(0, 23)]
    public int Hour { get; set; }

    [Range(0, 59)]
    public int Minute { get; set; }

    public int TotalMinutes => Hour * 60 + Minute;

    public TimeModel()
    {
    }

    public TimeModel(int hour, int minute)
    {
        Hour = hour;
        Minute = minute;
    }

    public override string ToString()
    {
        return $"{Hour:D2}:{Minute:D2}";
    }
}